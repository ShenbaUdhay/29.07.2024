using DevExpress.Data.Filtering;
using DevExpress.DataProcessing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
//using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.Settings.TestParameter
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TestParameterEditViewController : ViewController, IXafCallbackHandler
    {
        TestInfo testInfo = new TestInfo();
        MessageTimer timer = new MessageTimer();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        private ListViewProcessCurrentObjectController processCurrentObjectController;
        curlanguage objLanguage = new curlanguage();

        //private FilterController standardFilterController;
        public TestParameterEditViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "TestParamterEdit;" + "TestMethod_QCTypes_ListView_RunType;" + "TestMethod_QCTypes_ListView_CopyToQcType;" + "Parameter_LookupListView_AvailableTest;"
                + "TestMethod_ListView_Copy;" + "Testparameter_LookupListView_SelectedTest_Copy;" + "TestParameter_ListView_DefaultSettings;" + "TestParameter_ListView_DefaultSettings_QC;"
                + "QcParameter_ListView_DefaultSettings;" + "TestParameterSettings;";
            //"TestMethod_QCTypes_ListView_RunType;" + "TestMethod_QCTypes_ListView_CopyToQcType;" 

            //+ "Parameter_LookupListView_AvailableTest;" 


            addSampleParameterAction.TargetViewId = "TestParamterEdit;";
            removeSampleParameterAction.TargetViewId = "TestParamterEdit;";
            saveQCParameterAction.TargetViewId = "TestParamterEdit;";
            prevQCParameterAction.TargetViewId = "TestParamterEdit;";
            nextQCParameterAction.TargetViewId = "TestParamterEdit;";
            //copyToAllTestParamAction.TargetViewId = "TestParameter_ListView_DefaultSettings;";
            //copyToAllQCParamAction.TargetViewId = "QcParameter_ListView_DefaultSettings;";
            saveParameterDefaultsAction.TargetViewId = "TestParameterSettings;";
            gotoTestparameterEditAction.TargetViewId = "TestParameterSettings;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            try
            {
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;

                objPermissionInfo.ParameterDefaultsIsDelete = false;
                objPermissionInfo.ParameterDefaultsIsWrite = false;
                objPermissionInfo.TestsIsWrite = false;
                objPermissionInfo.TestsIsDelete = false;
                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                if (currentUser != null && currentUser.Roles != null && currentUser.Roles.Count > 0)
                {
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        objPermissionInfo.ParameterDefaultsIsDelete = true;
                        objPermissionInfo.ParameterDefaultsIsWrite = true;
                        objPermissionInfo.TestsIsWrite = true;
                        objPermissionInfo.TestsIsDelete = true;
                    }
                    else
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "TestParameterDefaultSetup" && i.Write == true) != null)
                            {
                                objPermissionInfo.ParameterDefaultsIsWrite = true;
                                //return;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "TestParameterDefaultSetup" && i.Delete == true) != null)
                            {
                                objPermissionInfo.ParameterDefaultsIsDelete = true;
                                //return;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Test Parameter" && i.Write == true) != null)
                            {
                                objPermissionInfo.TestsIsWrite = true;
                                //return;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Test Parameter" && i.Delete == true) != null)
                            {
                                objPermissionInfo.TestsIsDelete = true;
                                //return;
                            }
                        }
                    }
                }

                if (View != null && View.Id == "TestMethod_QCTypes_ListView_RunType")//QCType Runtype
                {
                    if (testInfo.CurrentTest != null)
                    {
                        TestMethod currentTest = ((ListView)View).ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", testInfo.CurrentTest.Oid));
                        if (currentTest != null)
                        {
                            List<object> groups = new List<object>();
                            bool boolHasSample = false;

                            if (currentTest.QCTypes != null && currentTest.QCTypes.Count > 0)
                            {
                                foreach (QCType qcType in currentTest.QCTypes)
                                {
                                    if (qcType != null)
                                    {
                                        groups.Add(qcType.Oid);
                                        if (qcType != null && qcType.QCTypeName == "Sample")
                                        {
                                            boolHasSample = true;
                                        }
                                    }
                                }
                            }

                            if (boolHasSample == false)
                            {
                                IObjectSpace sampleObjectSpace = Application.CreateObjectSpace();
                                QCType sampleQCType = ((ListView)View).ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = 'Sample'"));
                                if (sampleQCType != null)
                                {
                                    currentTest.QCTypes.Add(sampleQCType);
                                    groups.Add(sampleQCType.Oid);
                                }
                                sampleObjectSpace.CommitChanges();
                            }

                            ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", groups);
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                        }
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                }
                else if (View != null && (View.Id == "Parameter_LookupListView_AvailableTest" || View.Id == "Testparameter_LookupListView_SelectedTest_Copy" || View.Id == "TestMethod_QCTypes_ListView_CopyToQcType"))
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                }
                else if (View.Id == "TestParameter_ListView_DefaultSettings" && testInfo.CurrentTest != null)
                {
                    TestMethod test = ObjectSpace.GetObjectByKey<TestMethod>(testInfo.CurrentTest.Oid);

                    if (test != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", test.Oid);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                }
                else if (View.Id == "TestParameter_ListView_DefaultSettings_QC" && testInfo.CurrentTest != null)
                {
                    //copyToAllQCParamAction.Active["hidecopytoallqc"] = false;
                    TestMethod test = ObjectSpace.GetObjectByKey<TestMethod>(testInfo.CurrentTest.Oid);

                    if (test != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", test.Oid);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }

                }
                else if (View != null && View.Id == "TestMethod_ListView_Copy")
                {
                    processCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (processCurrentObjectController != null)
                    {
                        processCurrentObjectController.CustomProcessSelectedItem += processCurrentObjectController_CustomProcessSelectedItem;
                    }
                }
                else if (View.Id == "TestParameterSettings")
                {
                    saveParameterDefaultsAction.Active["ShowSaveParameterDefaults"] = objPermissionInfo.ParameterDefaultsIsWrite;
                    gotoTestparameterEditAction.Active["ShowGototestparamedit"] = objPermissionInfo.ParameterDefaultsIsWrite;
                }
                //else if (View.Id == "TestParamterEdit")
                //{
                //    ActionContainerViewItem addSampleparam = ((DashboardView)View).FindItem("catAddQCParamContainer") as ActionContainerViewItem;
                //    addSampleparam.Actions[0].Enabled.SetItemValue("key", objPermissionInfo.TestsIsWrite);
                //    ActionContainerViewItem removeSampleparam = ((DashboardView)View).FindItem("catRemoveQCParamContainer") as ActionContainerViewItem;
                //    removeSampleparam.Actions[0].Enabled.SetItemValue("key", objPermissionInfo.TestsIsWrite);
                //    ActionContainerViewItem saveSampleparam = ((DashboardView)View).FindItem("catSaveQCParamContainer") as ActionContainerViewItem;
                //    saveSampleparam.Actions[0].Enabled.SetItemValue("key", objPermissionInfo.TestsIsWrite);
                //    ActionContainerViewItem copyFromQC = ((DashboardView)View).FindItem("catCopyFromQCContainer") as ActionContainerViewItem;
                //    copyFromQC.Actions[0].Enabled.SetItemValue("key", objPermissionInfo.TestsIsWrite);

                //    //addSampleParameterAction.Active["ShowAddSampleParam"] = objPermissionInfo.TestsIsWrite;
                //    //removeSampleParameterAction.Active["ShowRemoveSampleParam"] = objPermissionInfo.TestsIsWrite;
                //    //saveQCParameterAction.Active["ShowSaveQCParam"] = objPermissionInfo.TestsIsWrite;
                //    //copyFromQCTypeAction.Active["ShowCopyformQC"] = objPermissionInfo.TestsIsWrite;
                //}

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void processCurrentObjectController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (e != null && e.InnerArgs != null && e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject.GetType() == typeof(TestMethod))
                {
                    TestMethod currentTest = (TestMethod)e.InnerArgs.CurrentObject;
                    if (currentTest != null)
                    {
                        testInfo.CurrentTest = currentTest;
                        DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "TestParamterEdit", false);
                        Frame.SetView(dashboard);
                    }
                }
                e.Handled = true;
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
                if (e.PopupFrame.View.Id == "TestParamterEdit" || e.PopupFrame.View.Id == "TestMethod_ListView_Copy")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1100);
                    e.Height = new System.Web.UI.WebControls.Unit(648);
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
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            try
            {
                if (View is ListView && (View.Id == "TestMethod_QCTypes_ListView_RunType" /*|| View.Id == "TestMethod_QCTypes_ListView_CopyToQcType"*/
                    || View.Id == "Parameter_LookupListView_AvailableTest"))
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {

                        editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        editor.Grid.Settings.VerticalScrollableHeight = 150;
                        editor.Grid.ClientSideEvents.Init = @"function(s,e)
                                                            {
                                                                s.SetWidth(500);
                                                            }";
                        editor.Grid.Load += Grid_Load;
                        editor.Grid.SelectionChanged += Grid_SelectionChanged;
                        editor.Grid.SettingsPager.PageSize = 200;
                        if (View.Id == "TestMethod_QCTypes_ListView_RunType" || View.Id == "TestMethod_QCTypes_ListView_CopyToQcType")
                        {
                            editor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                            if (View.Id == "TestMethod_QCTypes_ListView_RunType")
                            {
                                ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                                selparameter.CallbackManager.RegisterHandler("TestParameter", this);

                                editor.Grid.HtmlRowPrepared += Grid_HtmlRowPrepared;
                                editor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                                editor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e)
                                                                                {
                                                                                  if(e.visibleIndex != '-1')
                                                                                  {
                                                                                    s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                                                                                    if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                                                                                        var value = 'TestParameterselection|Selected|' + Oidvalue;
                                                                                        RaiseXafCallback(globalCallbackControl, 'TestParameter', value, '', false);    
                                                                                    }else{
                                                                                        var value = 'TestParameterselection|UNSelected|' + Oidvalue;
                                                                                        RaiseXafCallback(globalCallbackControl, 'TestParameter', value, '', false);    
                                                                                    }
                                                                                  });
                                                                                 }
                                                                                 //else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                                                                                 //{
                                                                                 //   var Oidvalues = '';
                                                                                 //   for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                                                                 //   {
                                                                                 //       Oidvalues = Oidvalues + '|' + s.GetRowKey(i);
                                                                                 //   }   
                                                                                 //   var value = 'TestParameterselection|Selectall' + Oidvalues;
                                                                                 //   RaiseXafCallback(globalCallbackControl, 'TestParameter', value, '', false);                        
                                                                                 //}   
                                                                                 //else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                                                                                 //{
                                                                                 //   var Oidvalues = '';
                                                                                 //   for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                                                                 //   {
                                                                                 //       Oidvalues = Oidvalues + '|' + s.GetRowKey(i);
                                                                                 //   }   
                                                                                 //   var value = 'TestParameterselection|UNSelectall' + Oidvalues;
                                                                                 //   RaiseXafCallback(globalCallbackControl, 'TestParameter', value, '', false);                        
                                                                                 //}  
                                                                                }";
                            }
                        }
                    }
                }
                else if (View is ListView && (View.Id == "TestParameter_ListView_DefaultSettings" || View.Id == "TestParameter_ListView_DefaultSettings_QC"))
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        editor.Grid.Settings.VerticalScrollableHeight = 300;
                        editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                        editor.Grid.Load += Grid_Load;
                        editor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                        editor.Grid.SettingsContextMenu.Enabled = true;
                        editor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        //editor.Grid.Init += Grid_Init;

                        //editor.Grid.ClientSideEvents.Init = @"function(s,e)
                        //                                    {
                        //                                        s.SetWidth(1200);
                        //                                    }";

                        if (View.Id == "TestParameter_ListView_DefaultSettings")
                        {
                            ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                            selparameter.CallbackManager.RegisterHandler("TestParameterSettings", this);

                            if (editor.Grid.Columns["FinalDefaultResult"] != null)
                            {
                                editor.Grid.Columns["FinalDefaultResult"].Width = 120;
                            }
                            if (editor.Grid.Columns["FinalDefaultUnits"] != null)
                            {
                                editor.Grid.Columns["FinalDefaultUnits"].Width = 120;
                            }

                            editor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {   
                                sessionStorage.setItem('TPSettingsFocusedColumn', null);  
                                if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                                {                              
                                    e.cancel = true;
                                }
                                else if (e.cellInfo.column.fieldName == 'Parameter.ParameterName')
                                {                         
                                    e.cancel = true;
                                }                        
                                else
                                {
                                    var fieldName = e.cellInfo.column.fieldName;                       
                                    sessionStorage.setItem('TPSettingsFocusedColumn', fieldName);  
                                }                                         
                            }";

                            editor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('TPSettingsFocusedColumn');      //alert(FocusedColumn);                          
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
                                    else
                                    {                                                         
                                        var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                        if (e.item.name =='CopyToAllCell')
                                        {     
                                            var visibleIndices = Grid.batchEditApi.GetRowVisibleIndices();
                                            var totalRowsCountOnPage = visibleIndices.length;

                                            //for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                            for(var i = 0; i < totalRowsCountOnPage; i++)
                                            {                                              
                                                //if (s.IsRowSelectedOnPage(i)) 
                                                if (s.IsRowSelectedOnPage(visibleIndices[i])) 
                                                {
                                                    s.batchEditApi.SetCellValue(visibleIndices[i],FocusedColumn,CopyValue);
                                                    //s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                                    //alert(i);
                                                }
                                            }
                                        }                            
                                    }
                                }
                                e.processOnServer = false;
                            }";
                        }

                        if (View.Id == "TestParameter_ListView_DefaultSettings_QC")
                        {
                            ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                            selparameter.CallbackManager.RegisterHandler("QCTestParameterSettings", this);

                            //GridViewColumn colFinalDefaultResult = editor.Grid.Columns.Cast<GridViewColumn>().ToList().FirstOrDefault<GridViewColumn>(i=>i.Name== "FinalDefaultResult");

                            if (editor.Grid.Columns["FinalDefaultResult"] != null)
                            {
                                editor.Grid.Columns["FinalDefaultResult"].Width = 120;
                            }
                            if (editor.Grid.Columns["FinalDefaultUnits"] != null)
                            {
                                editor.Grid.Columns["FinalDefaultUnits"].Width = 120;
                            }
                            if (editor.Grid.Columns["SpikeAmountUnit"] != null)
                            {
                                editor.Grid.Columns["SpikeAmountUnit"].Width = 120;
                            }

                            editor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {   
                                sessionStorage.setItem('QCTPSettingsFocusedColumn', null);  
                                if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                                {                              
                                    e.cancel = true;
                                }
                                else if (e.cellInfo.column.fieldName == 'Parameter.ParameterName')
                                {                         
                                    e.cancel = true;
                                }                        
                                else
                                {
                                    var fieldName = e.cellInfo.column.fieldName;                       
                                    sessionStorage.setItem('QCTPSettingsFocusedColumn', fieldName);  
                                }                                         
                            }";

                            editor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('QCTPSettingsFocusedColumn');                                
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
                                    else
                                    {                                                         
                                        var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                        if (e.item.name =='CopyToAllCell')
                                        {     
                                            var visibleIndices = Grid.batchEditApi.GetRowVisibleIndices();
                                            var totalRowsCountOnPage = visibleIndices.length;

                                            //for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                            for(var i = 0; i < totalRowsCountOnPage; i++)
                                            {                                              
                                                //if (s.IsRowSelectedOnPage(i)) 
                                                if (s.IsRowSelectedOnPage(visibleIndices[i])) 
                                                {
                                                    s.batchEditApi.SetCellValue(visibleIndices[i],FocusedColumn,CopyValue);
                                                    //s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                                    //alert(i);
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
                else if (View.Id == "TestParamterEdit")
                {
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("CanCloseTestParameter", this);

                    ActionContainerViewItem addSampleparam = ((DashboardView)View).FindItem("catAddQCParamContainer") as ActionContainerViewItem;
                    if (addSampleparam != null && addSampleparam.Actions.Count > 0 && addSampleparam.Actions[0] != null)
                    {
                        addSampleparam.Actions[0].Enabled.SetItemValue("key", objPermissionInfo.TestsIsWrite);
                    }
                    ActionContainerViewItem removeSampleparam = ((DashboardView)View).FindItem("catRemoveQCParamContainer") as ActionContainerViewItem;
                    if (removeSampleparam != null && removeSampleparam.Actions.Count > 0 && removeSampleparam.Actions[0] != null)
                    {
                        removeSampleparam.Actions[0].Enabled.SetItemValue("key", objPermissionInfo.TestsIsWrite);
                    }
                    ActionContainerViewItem saveSampleparam = ((DashboardView)View).FindItem("catSaveQCParamContainer") as ActionContainerViewItem;
                    if (saveSampleparam != null && saveSampleparam.Actions.Count > 0 && saveSampleparam.Actions[0] != null)
                    {
                        saveSampleparam.Actions[0].Enabled.SetItemValue("key", objPermissionInfo.TestsIsWrite);
                    }
                    ActionContainerViewItem copyFromQC = ((DashboardView)View).FindItem("catCopyFromQCContainer") as ActionContainerViewItem;
                    if (copyFromQC != null && copyFromQC.Actions.Count > 0 && copyFromQC.Actions[0] != null)
                    {
                        copyFromQC.Actions[0].Enabled.SetItemValue("key", objPermissionInfo.TestsIsWrite);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    //var os = Application.CreateObjectSpace();
                    //Session currentSession = ((XPObjectSpace)(os)).Session;
                    //DevExpress.Xpo.DB.SelectedData sproc = currentSession.ExecuteSproc("getCurrentLanguage", "");
                    // var CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                    if (objLanguage.strcurlanguage == "En")
                    {
                        e.Items.Add("Copy To All Cell", "CopyToAllCell");
                        e.Items.Remove(e.Items.FindByText("Edit"));
                    }
                    else
                    {
                        e.Items.Add("拷贝到全列", "CopyToAllCell");
                        e.Items.Remove(e.Items.FindByText("编辑"));
                    }
                    e.Items.Remove(e.Items.FindByText("Refresh"));
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

        private void Grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (e.RowType == GridViewRowType.Data)
                {
                    if (testInfo.ModifiedQCTypes != null && testInfo.ModifiedQCTypes.Count > 0)
                    {
                        foreach (QCType qcType in testInfo.ModifiedQCTypes)
                        {
                            if (testInfo.CurrentQcType != null && e.GetValue("Oid").ToString() == qcType.Oid.ToString() && e.GetValue("Oid").ToString() != testInfo.CurrentQcType.Oid.ToString())
                            {
                                e.Row.BackColor = System.Drawing.Color.LightGreen;
                            }
                        }
                    }

                    Guid qctypeoid = new Guid(e.GetValue("Oid").ToString());
                    IList<Testparameter> testlist = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.Oid] = ?", testInfo.CurrentTest.Oid, qctypeoid));
                    //if (testlist.Count > 0)
                    //{
                    //    e.Row.BackColor = System.Drawing.Color.LightGreen;
                    //}
                    if (testInfo.CurrentQcType != null)
                    {
                        if (e.GetValue("Oid") != null && e.GetValue("Oid").ToString() == testInfo.CurrentQcType.Oid.ToString())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                        }
                        else
                        if (testlist.Count > 0 && e.GetValue("Oid") != null && e.GetValue("Oid").ToString() != testInfo.CurrentQcType.Oid.ToString())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
                    else
                    {
                        if (testlist.Count > 0)
                        {
                            e.Row.BackColor = System.Drawing.Color.LightGreen;
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
                ASPxGridView gv = sender as ASPxGridView;
                if (gv != null && (View.Id == "TestMethod_QCTypes_ListView_RunType" || View.Id == "TestMethod_QCTypes_ListView_CopyToQcType"))
                {
                    gv.SettingsPager.Visible = false;
                    if (((ListView)View).CollectionSource.GetCount() == 1 && ((ListView)View).SelectedObjects != null && ((ListView)View).SelectedObjects.Count == 0 && ((ListView)View).Editor != null)
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            editor.Grid.Selection.SelectRow(0);
                            QCType selQCType = ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[TestMethods][[Oid] = ?] And [QCTypeName] = 'Sample'", testInfo.CurrentTest.Oid));
                            if (selQCType != null)
                            {
                                string strParameter = "TestParameterselection|Selected|" + selQCType.Oid;
                                ProcessAction(strParameter);
                            }
                        }
                    }
                }
                else
                if (gv != null && (View.Id == "TestParameter_ListView_DefaultSettings" || View.Id == "TestParameter_ListView_DefaultSettings_QC"))
                {
                    gv.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                    gv.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                    gv.VisibleColumns[2].FixedStyle = GridViewColumnFixedStyle.Left;
                    if (View.Id == "TestParameter_ListView_DefaultSettings_QC")
                    {
                        gv.VisibleColumns[3].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
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
                if (View.Id == "TestMethod_QCTypes_ListView_RunType")
                {
                    testInfo.CurrentQcType = ((ListView)View).CurrentObject as QCType;
                }
                else
                if (View.Id == "Parameter_LookupListView_AvailableTest")
                {
                    testInfo.AllSelAvailableTestParam = new List<object>();
                    foreach (Modules.BusinessObjects.Setting.Parameter test in ((ListView)View).SelectedObjects)
                    {
                        testInfo.AllSelAvailableTestParam.Add(test.Oid);
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();

            try
            {
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View != null && View.Id == "TestMethod_ListView_Copy")
                {
                    processCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (processCurrentObjectController != null)
                    {
                        processCurrentObjectController.CustomProcessSelectedItem -= processCurrentObjectController_CustomProcessSelectedItem;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void saveQCParameterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TestParamterEdit")
                {
                    DashboardViewItem qcTypeView = ((DashboardView)View).FindItem("QCTypeView") as DashboardViewItem;
                    DashboardViewItem viewSelectedTest = ((DashboardView)View).FindItem("SelectedTestParam") as DashboardViewItem;

                    if (qcTypeView != null && qcTypeView.InnerView != null)
                    {
                        if (viewSelectedTest != null && viewSelectedTest.InnerView != null)
                        {
                            System.Collections.IList newTestList = ((ListView)viewSelectedTest.InnerView).CollectionSource.List;
                            IObjectSpace testParamEditObjectSpace = Application.CreateObjectSpace();
                            bool HasChanges = false;

                            if (testInfo.RemovedTestParameters != null && testInfo.RemovedTestParameters.Count > 0)
                            {

                                foreach (Testparameter param in testInfo.RemovedTestParameters)
                                {
                                    Testparameter removeParam = testParamEditObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Parameter.Oid] = ? And [QCType.Oid] = ? And [TestMethod.Oid] = ?", param.Parameter.Oid, param.QCType.Oid, param.TestMethod.Oid));
                                    if (removeParam != null)
                                    {
                                        testParamEditObjectSpace.Delete(removeParam);
                                        HasChanges = true;
                                    }
                                }
                            }

                            if (testInfo.NewTestParameters != null && testInfo.NewTestParameters.Count > 0)
                            {

                                foreach (Testparameter param in testInfo.NewTestParameters)
                                {
                                    Testparameter newParam = testParamEditObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Parameter.Oid] = ? And [QCType.Oid] = ? And [TestMethod.Oid] = ?", param.Parameter.Oid, param.QCType.Oid, param.TestMethod.Oid));
                                    if (newParam == null)
                                    {
                                        newParam = testParamEditObjectSpace.CreateObject<Testparameter>();
                                        newParam.Parameter = testParamEditObjectSpace.GetObjectByKey<Parameter>(param.Parameter.Oid);
                                        newParam.QCType = testParamEditObjectSpace.GetObjectByKey<QCType>(param.QCType.Oid);
                                        newParam.TestMethod = testParamEditObjectSpace.GetObjectByKey<TestMethod>(param.TestMethod.Oid);
                                        HasChanges = true;
                                    }
                                }

                            }

                            if (HasChanges == true)
                            {
                                testParamEditObjectSpace.CommitChanges();
                                testInfo.IsSaved = true;
                            }

                            WebWindow.CurrentRequestWindow.RegisterClientScript("CanClosePopup", ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager.GetScript("CanCloseTestParameter", "confirm('Saved successfully. Do you want to close the popup window?','Alert')"));

                            //if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
                            //{
                            //    (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
                            //}
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

        private void SaveParameterDefaultsAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TestParameterSettings" && View is DashboardView)
                {
                    DashboardViewItem TestParamDefaultSettings = ((DashboardView)View).FindItem("TestParamDefaultSettings") as DashboardViewItem;
                    DashboardViewItem QCParamDefaultSettings = ((DashboardView)View).FindItem("QCTestParamDefaultSettings") as DashboardViewItem;
                    if (TestParamDefaultSettings != null && TestParamDefaultSettings.InnerView == null)
                    {
                        TestParamDefaultSettings.CreateControl();
                    }
                    else if (QCParamDefaultSettings != null && QCParamDefaultSettings.InnerView == null)
                    {
                        QCParamDefaultSettings.CreateControl();
                    }
                    if (TestParamDefaultSettings != null && TestParamDefaultSettings.InnerView != null)
                    {
                        ((ASPxGridListEditor)((ListView)TestParamDefaultSettings.InnerView).Editor).Grid.UpdateEdit();
                    }

                    if (QCParamDefaultSettings != null && QCParamDefaultSettings.InnerView != null)
                    {
                        ((ASPxGridListEditor)((ListView)QCParamDefaultSettings.InnerView).Editor).Grid.UpdateEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void removeSampleParameterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TestParamterEdit")
                {
                    DashboardViewItem availTestParamView = ((DashboardView)View).FindItem("AvailableTestParam") as DashboardViewItem;
                    DashboardViewItem viewSelectedTest = ((DashboardView)View).FindItem("SelectedTestParam") as DashboardViewItem;
                    if (viewSelectedTest != null && viewSelectedTest.InnerView != null && viewSelectedTest.InnerView is ListView && viewSelectedTest.InnerView.SelectedObjects != null && viewSelectedTest.InnerView.SelectedObjects.Count > 0)
                    {
                        if (testInfo.RemovedTestParameters == null)
                        {
                            testInfo.RemovedTestParameters = new List<Testparameter>();
                        }

                        foreach (Testparameter item in viewSelectedTest.InnerView.SelectedObjects)
                        {
                            ((ListView)viewSelectedTest.InnerView).CollectionSource.Remove(item);

                            Testparameter removeParam = ((ListView)viewSelectedTest.InnerView).ObjectSpace.GetObjectByKey<Testparameter>(item.Oid);

                            //if (removeParam != null)
                            //{
                            //    ((ListView)viewSelectedTest.InnerView).ObjectSpace.Delete(removeParam);
                            //}

                            Testparameter removeTest = item as Testparameter;

                            if (testInfo.NewTestParameters != null && testInfo.NewTestParameters.Where<Testparameter>(i => i.Oid == removeTest.Oid).Count() >= 0)
                            {
                                testInfo.NewTestParameters.Remove(removeTest);
                            }
                            if (testInfo.RemovedTestParameters != null && testInfo.RemovedTestParameters.Where<Testparameter>(i => i.Oid == removeTest.Oid).Count() <= 0)
                            {
                                testInfo.RemovedTestParameters.Add(item);
                            }
                        }

                        if (availTestParamView != null && availTestParamView.InnerView != null)
                        {
                            List<Testparameter> selectedParams = ((ListView)viewSelectedTest.InnerView).CollectionSource.List.Cast<Testparameter>().ToList();
                            if (selectedParams != null)
                            {
                                ((ListView)availTestParamView.InnerView).CollectionSource.Criteria.Clear();
                                if (selectedParams.Count > 0)
                                {
                                    ((ListView)availTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [ParameterName] In(" + string.Format("'{0}'", string.Join("','", selectedParams.Select(i => i.Parameter.ParameterName.Replace("'", "''")))) + ")");
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

        private void addSampleParameterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View is DashboardView && View.Id == "TestParamterEdit")
                {
                    DashboardViewItem availTestParamView = ((DashboardView)View).FindItem("AvailableTestParam") as DashboardViewItem;
                    DashboardViewItem selectedSampleTest = ((DashboardView)View).FindItem("SelectedTestParam") as DashboardViewItem;

                    if (testInfo.NewTestParameters == null)
                    {
                        testInfo.NewTestParameters = new List<Testparameter>();
                    }

                    if (testInfo.AllSelAvailableTestParam != null && testInfo.AllSelAvailableTestParam.Count > 0)
                    {
                        if (selectedSampleTest != null && selectedSampleTest.InnerView != null && selectedSampleTest.InnerView is ListView)
                        {
                            foreach (var val in testInfo.AllSelAvailableTestParam)
                            {
                                if (val != null && testInfo.CurrentTest != null && testInfo.CurrentQcType != null)
                                {
                                    Modules.BusinessObjects.Setting.Testparameter test = ((ListView)selectedSampleTest.InnerView).ObjectSpace.FindObject<Modules.BusinessObjects.Setting.Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.Oid] = ? And [Parameter.Oid] = ?", testInfo.CurrentTest.Oid, testInfo.CurrentQcType.Oid, val));
                                    if (test != null)
                                    {
                                        ((ListView)selectedSampleTest.InnerView).CollectionSource.Add(test);

                                        IObjectSpace testObjectSpace = Application.CreateObjectSpace();

                                        if (testObjectSpace.GetObjectByKey<Testparameter>(test.Oid) == null && !testInfo.NewTestParameters.Contains(test))// testInfo.NewTestParameters.Where<Testparameter>(i=>i.Oid==test.Oid)==null)
                                        {
                                            testInfo.NewTestParameters.Add(test);
                                        }
                                    }
                                    else
                                    {
                                        Modules.BusinessObjects.Setting.Testparameter newTestParam = ((ListView)selectedSampleTest.InnerView).ObjectSpace.CreateObject<Modules.BusinessObjects.Setting.Testparameter>();
                                        newTestParam.QCType = ((ListView)selectedSampleTest.InnerView).ObjectSpace.GetObjectByKey<QCType>(testInfo.CurrentQcType.Oid);
                                        newTestParam.TestMethod = ((ListView)selectedSampleTest.InnerView).ObjectSpace.GetObjectByKey<TestMethod>(testInfo.CurrentTest.Oid);
                                        newTestParam.Parameter = ((ListView)selectedSampleTest.InnerView).ObjectSpace.GetObjectByKey<Parameter>(val);
                                        ((ListView)selectedSampleTest.InnerView).CollectionSource.Add(newTestParam);

                                        //((ListView)selectedSampleTest.InnerView).ObjectSpace.CommitChanges();
                                        testInfo.NewTestParameters.Add(newTestParam);
                                    }

                                    if (testInfo.RemovedTestParameters != null && testInfo.RemovedTestParameters.Where<Testparameter>(i => i.Oid == test.Oid) != null)
                                    {
                                        testInfo.RemovedTestParameters.Remove(test);
                                    }
                                }
                            }
                            ((ListView)selectedSampleTest.InnerView).Refresh();
                        }


                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }

                    if (availTestParamView != null && availTestParamView.InnerView != null && selectedSampleTest != null && selectedSampleTest.InnerView != null)
                    {
                        List<Testparameter> selectedParams = ((ListView)selectedSampleTest.InnerView).CollectionSource.List.Cast<Testparameter>().ToList();
                        if (selectedParams != null && selectedParams.Count > 0)
                        {
                            ((ListView)availTestParamView.InnerView).CollectionSource.Criteria.Clear();
                            ((ListView)availTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [ParameterName] In(" + string.Format("'{0}'", string.Join("','", selectedParams.Select(i => i.Parameter.ParameterName.Replace("'", "''")))) + ")");
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

        private void nextQCParameterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View is DashboardView && View.Id == "TestParamterEdit")
                {
                    DashboardViewItem qcTypeView = ((DashboardView)View).FindItem("QCTypeView") as DashboardViewItem;

                    if (qcTypeView != null && qcTypeView.InnerView != null && testInfo.CurrentTest != null && testInfo.CurrentQcType != null)
                    {
                        TestMethod currentTest = ((ListView)qcTypeView.InnerView).ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", testInfo.CurrentTest.Oid));
                        if (currentTest != null && currentTest.QCTypes != null && currentTest.QCTypes.Count > 0)
                        {
                            List<QCType> allQCTypes = new List<QCType>();

                            foreach (QCType qcType in currentTest.QCTypes)
                            {
                                allQCTypes.Add(qcType);
                            }

                            if (allQCTypes.Count > 0)
                            {
                                IOrderedEnumerable<QCType> orderedQCTypeList = allQCTypes.Cast<QCType>().OrderBy(x => x.index).ThenBy(x => x.QCTypeName);
                                int currentIndex = orderedQCTypeList.ToList().FindIndex(x => x.Oid == testInfo.CurrentQcType.Oid);
                                int nextFocusIndex = 0;
                                if (currentIndex + 1 <= allQCTypes.Count - 1)
                                {
                                    nextFocusIndex = currentIndex + 1;
                                }

                                QCType currentQCType = ObjectSpace.GetObjectByKey<QCType>(testInfo.CurrentQcType.Oid);
                                QCType nextQCType = orderedQCTypeList.ElementAt(nextFocusIndex);

                                if (nextQCType != null)
                                {
                                    if (testInfo.ModifiedQCTypes == null)
                                    {
                                        testInfo.ModifiedQCTypes = new List<QCType>();
                                    }

                                    List<object> groups = new List<object>();
                                    string strCriteria = string.Empty;

                                    DashboardViewItem availableTestParamView = ((DashboardView)View).FindItem("AvailableTestParam") as DashboardViewItem;
                                    DashboardViewItem selectedTestParam = ((DashboardView)View).FindItem("SelectedTestParam") as DashboardViewItem;
                                    DashboardViewItem qcToSampleView = ((DashboardView)View).FindItem("QCToSampleView") as DashboardViewItem;

                                    if (currentQCType != null)
                                    {
                                        int modifiedObjectIndex = testInfo.ModifiedQCTypes.ToList().FindIndex(x => x != null && x.Oid == currentQCType.Oid);
                                        if (modifiedObjectIndex == -1)
                                        {
                                            testInfo.ModifiedQCTypes.Add(currentQCType);
                                        }
                                        else
                                        {
                                            testInfo.ModifiedQCTypes[modifiedObjectIndex] = currentQCType;
                                        }
                                    }

                                    if (availableTestParamView != null && availableTestParamView.InnerView != null)
                                    {
                                        testInfo.CurrentQcType = nextQCType;
                                        ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria.Clear();

                                        ASPxGridListEditor availTestListEditor = ((ListView)availableTestParamView.InnerView).Editor as ASPxGridListEditor;
                                        if (availTestListEditor != null && availTestListEditor.Grid != null)
                                        {
                                            availTestListEditor.Grid.Selection.UnselectAll();
                                        }
                                        ((ListView)availableTestParamView.InnerView).Refresh();

                                        List<object> CopyFromGroup = new List<object>();

                                        if (testInfo.ModifiedQCTypes != null && testInfo.ModifiedQCTypes.Count > 0)
                                        {
                                            foreach (QCType obj in testInfo.ModifiedQCTypes)
                                            {
                                                if (obj.Oid != testInfo.CurrentQcType.Oid)
                                                {
                                                    CopyFromGroup.Add(obj.Oid);
                                                }
                                            }
                                        }

                                        if (currentTest != null)
                                        {
                                            foreach (QCType qcType in currentTest.QCTypes)
                                            {
                                                if (testInfo.CurrentQcType != null && testInfo.CurrentQcType.Oid != qcType.Oid)
                                                {
                                                    IList<Testparameter> testlist = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod]=? and [QCType]=?", testInfo.CurrentTest.Oid, qcType.Oid));
                                                    if (testlist != null && testlist.Count > 0)
                                                    {
                                                        CopyFromGroup.Add(qcType.Oid);
                                                    }
                                                }
                                            }
                                        }

                                        ASPxGridListEditor editor = ((ListView)qcTypeView.InnerView).Editor as ASPxGridListEditor;
                                        if (editor != null && editor.Grid != null)
                                        {
                                            editor.Grid.Selection.SelectRow(nextFocusIndex);
                                        }

                                        ((ListView)selectedTestParam.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");

                                        if (testInfo.RemovedTestParameters != null && testInfo.RemovedTestParameters.Count > 0)
                                        {
                                            strCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testInfo.RemovedTestParameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                        }

                                        IList<Testparameter> testParamList = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And[QCType.Oid] = ?", currentTest.Oid, testInfo.CurrentQcType.Oid));
                                        bool HasParams = false;
                                        if (testParamList != null && testParamList.Count > 0)
                                        {
                                            HasParams = true;
                                            if (!string.IsNullOrEmpty(strCriteria))
                                            {
                                                strCriteria = strCriteria + "and [Oid] In(" + string.Format("'{0}'", string.Join("','", testParamList.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                            }
                                            else
                                            {
                                                strCriteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", testParamList.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(strCriteria) && HasParams == true)
                                        {
                                            ((ListView)selectedTestParam.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(strCriteria);
                                        }

                                        if (testInfo.NewTestParameters != null && testInfo.NewTestParameters.Count > 0)
                                        {
                                            foreach (Testparameter param in testInfo.NewTestParameters)
                                            {
                                                if (param.QCType.Oid == testInfo.CurrentQcType.Oid && param.TestMethod.Oid == testInfo.CurrentTest.Oid)
                                                {
                                                    ((ListView)selectedTestParam.InnerView).CollectionSource.Add(param);
                                                }
                                            }
                                        }
                                        ((ListView)selectedTestParam.InnerView).Refresh();

                                        List<Testparameter> selectedParams = ((ListView)selectedTestParam.InnerView).CollectionSource.List.Cast<Testparameter>().ToList();
                                        if (selectedParams != null)
                                        {
                                            if (selectedParams.Count > 0)
                                            {
                                                ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [ParameterName] In(" + string.Format("'{0}'", string.Join("','", selectedParams.Select(i => i.Parameter.ParameterName.Replace("'", "''")))) + ")");
                                            }
                                        }

                                        if (CopyFromGroup.Count > 0)
                                        {
                                            ((ListView)qcToSampleView.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", CopyFromGroup);
                                        }
                                        else
                                        {
                                            ((ListView)qcToSampleView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                                        }
                                        ((ListView)qcToSampleView.InnerView).Refresh();

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

        private void prevQCParameterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View is DashboardView && View.Id == "TestParamterEdit")
                {
                    DashboardViewItem qcTypeView = ((DashboardView)View).FindItem("QCTypeView") as DashboardViewItem;

                    if (qcTypeView != null && qcTypeView.InnerView != null && testInfo.CurrentTest != null && testInfo.CurrentQcType != null)
                    {
                        TestMethod currentTest = ((ListView)qcTypeView.InnerView).ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", testInfo.CurrentTest.Oid));
                        if (currentTest != null && currentTest.QCTypes != null && currentTest.QCTypes.Count > 0)
                        {
                            List<QCType> allQCTypes = new List<QCType>();

                            foreach (QCType qcType in currentTest.QCTypes)
                            {
                                allQCTypes.Add(qcType);
                            }

                            if (allQCTypes.Count > 0)
                            {
                                IOrderedEnumerable<QCType> orderedQCTypeList = allQCTypes.Cast<QCType>().OrderBy(x => x.index).ThenBy(x => x.QCTypeName);
                                int currentIndex = orderedQCTypeList.ToList().FindIndex(x => x.Oid == testInfo.CurrentQcType.Oid);
                                int nextFocusIndex = 0;
                                if (currentIndex - 1 <= -1)
                                {
                                    nextFocusIndex = allQCTypes.Count - 1;
                                }
                                else
                                {
                                    nextFocusIndex = currentIndex - 1;
                                }

                                QCType currentQCType = ObjectSpace.GetObjectByKey<QCType>(testInfo.CurrentQcType.Oid);
                                QCType prevQCType = orderedQCTypeList.ElementAt(nextFocusIndex);

                                if (prevQCType != null)
                                {
                                    if (testInfo.ModifiedQCTypes == null)
                                    {
                                        testInfo.ModifiedQCTypes = new List<QCType>();
                                    }

                                    List<object> groups = new List<object>();
                                    string strCriteria = string.Empty;

                                    DashboardViewItem availableTestParamView = ((DashboardView)View).FindItem("AvailableTestParam") as DashboardViewItem;
                                    DashboardViewItem selectedTestParam = ((DashboardView)View).FindItem("SelectedTestParam") as DashboardViewItem;
                                    DashboardViewItem qcToSampleView = ((DashboardView)View).FindItem("QCToSampleView") as DashboardViewItem;

                                    if (currentQCType != null)
                                    {

                                        int modifiedObjectIndex = testInfo.ModifiedQCTypes.ToList().FindIndex(x => x != null && x.Oid == currentQCType.Oid);
                                        if (modifiedObjectIndex == -1)
                                        {
                                            testInfo.ModifiedQCTypes.Add(currentQCType);
                                        }
                                        else
                                        {
                                            testInfo.ModifiedQCTypes[modifiedObjectIndex] = currentQCType;
                                        }
                                    }
                                    if (availableTestParamView != null && availableTestParamView.InnerView != null)
                                    {

                                        testInfo.CurrentQcType = prevQCType;
                                        ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria.Clear();

                                        ASPxGridListEditor availTestListEditor = ((ListView)availableTestParamView.InnerView).Editor as ASPxGridListEditor;
                                        if (availTestListEditor != null && availTestListEditor.Grid != null)
                                        {
                                            availTestListEditor.Grid.Selection.UnselectAll();
                                        }
                                        ((ListView)availableTestParamView.InnerView).Refresh();

                                        List<object> CopyFromGroup = new List<object>();

                                        if (testInfo.ModifiedQCTypes != null && testInfo.ModifiedQCTypes.Count > 0)
                                        {
                                            foreach (QCType obj in testInfo.ModifiedQCTypes)
                                            {
                                                if (obj.Oid != testInfo.CurrentQcType.Oid)
                                                {
                                                    CopyFromGroup.Add(obj.Oid);
                                                }
                                            }
                                        }

                                        if (currentTest != null)
                                        {
                                            foreach (QCType qcType in currentTest.QCTypes)
                                            {
                                                if (testInfo.CurrentQcType != null && testInfo.CurrentQcType.Oid != qcType.Oid)
                                                {
                                                    IList<Testparameter> testlist = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod]=? and [QCType]=?", testInfo.CurrentTest.Oid, qcType.Oid));
                                                    if (testlist != null && testlist.Count > 0)
                                                    {
                                                        CopyFromGroup.Add(qcType.Oid);
                                                    }
                                                }
                                            }
                                        }

                                        ASPxGridListEditor editor = ((ListView)qcTypeView.InnerView).Editor as ASPxGridListEditor;
                                        if (editor != null && editor.Grid != null)
                                        {
                                            editor.Grid.Selection.SelectRow(nextFocusIndex);
                                        }

                                        ((ListView)selectedTestParam.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");

                                        if (testInfo.RemovedTestParameters != null && testInfo.RemovedTestParameters.Count > 0)
                                        {
                                            strCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testInfo.RemovedTestParameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                        }

                                        IList<Testparameter> testParamList = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And[QCType.Oid] = ?", currentTest.Oid, testInfo.CurrentQcType.Oid));
                                        bool HasParams = false;
                                        if (testParamList != null && testParamList.Count > 0)
                                        {
                                            HasParams = true;
                                            if (!string.IsNullOrEmpty(strCriteria))
                                            {
                                                strCriteria = strCriteria + "and [Oid] In(" + string.Format("'{0}'", string.Join("','", testParamList.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                            }
                                            else
                                            {
                                                strCriteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", testParamList.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(strCriteria) && HasParams == true)
                                        {
                                            ((ListView)selectedTestParam.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(strCriteria);
                                        }

                                        if (testInfo.NewTestParameters != null && testInfo.NewTestParameters.Count > 0)
                                        {
                                            foreach (Testparameter param in testInfo.NewTestParameters)
                                            {
                                                if (param.QCType.Oid == testInfo.CurrentQcType.Oid && param.TestMethod.Oid == testInfo.CurrentTest.Oid)
                                                {
                                                    ((ListView)selectedTestParam.InnerView).CollectionSource.Add(param);
                                                }
                                            }
                                        }

                                        ((ListView)selectedTestParam.InnerView).Refresh();

                                        List<Testparameter> selectedParams = ((ListView)selectedTestParam.InnerView).CollectionSource.List.Cast<Testparameter>().ToList();
                                        if (selectedParams != null)
                                        {
                                            if (selectedParams.Count > 0)
                                            {
                                                ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [ParameterName] In(" + string.Format("'{0}'", string.Join("','", selectedParams.Select(i => i.Parameter.ParameterName.Replace("'", "''")))) + ")");
                                            }
                                        }

                                        if (CopyFromGroup.Count > 0)
                                        {
                                            ((ListView)qcToSampleView.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", CopyFromGroup);
                                        }
                                        else
                                        {
                                            ((ListView)qcToSampleView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                                        }
                                        ((ListView)qcToSampleView.InnerView).Refresh();

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

        private void copyFromQCTypeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View is DashboardView && View.Id == "TestParamterEdit")
                {
                    DashboardViewItem qcTypeView = ((DashboardView)View).FindItem("QCTypeView") as DashboardViewItem;
                    DashboardViewItem qcToSampleView = ((DashboardView)View).FindItem("QCToSampleView") as DashboardViewItem;
                    DashboardViewItem availableTestParamView = ((DashboardView)View).FindItem("AvailableTestParam") as DashboardViewItem;
                    DashboardViewItem selectedTestParam = ((DashboardView)View).FindItem("SelectedTestParam") as DashboardViewItem;

                    if (qcTypeView != null && qcTypeView.InnerView != null && qcTypeView.InnerView.CurrentObject != null)
                    {
                        QCType selCopyToQCType = (QCType)((ListView)qcTypeView.InnerView).CurrentObject;

                        IObjectSpace copyToObjectSpace = Application.CreateObjectSpace();
                        QCType copyToQCType = copyToObjectSpace.GetObjectByKey<QCType>(selCopyToQCType.Oid);

                        if (testInfo != null && testInfo.NewTestParameters == null)
                        {
                            testInfo.NewTestParameters = new List<Testparameter>();
                        }

                        IList<Testparameter> copySourceParameters = null;
                        IList<Testparameter> copiedParameters = new List<Testparameter>();
                        IList<Testparameter> copyUnsavedNewParameters = new List<Testparameter>();

                        if (qcToSampleView != null && qcToSampleView.InnerView != null && qcToSampleView.InnerView.CurrentObject != null)
                        {
                            QCType copyFromQCType = (QCType)((ListView)qcToSampleView.InnerView).CurrentObject;

                            if (testInfo.CurrentTest != null && testInfo.CurrentQcType != null)
                            {
                                copySourceParameters = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.Oid] = ?", testInfo.CurrentTest.Oid, copyFromQCType.Oid));
                            }

                            if (testInfo != null && testInfo.NewTestParameters != null)
                            {
                                foreach (Testparameter param in testInfo.NewTestParameters)
                                {
                                    if (param.TestMethod.Oid == testInfo.CurrentTest.Oid && param.QCType.Oid == copyFromQCType.Oid && copySourceParameters != null && !copySourceParameters.Contains(param))
                                    {
                                        copyUnsavedNewParameters.Add(param);
                                    }
                                }
                            }

                            if (copySourceParameters != null)
                            {
                                foreach (Testparameter param in copySourceParameters.ToList())
                                {
                                    //copyToQCType.Testparameter.Add(copyToObjectSpace.GetObject<Testparameter>(param));

                                    if (((ListView)selectedTestParam.InnerView).ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.Oid] = ? And [Parameter.Oid] = ?", testInfo.CurrentTest.Oid, copyToQCType.Oid, param.Parameter.Oid)) == null)
                                    {
                                        Testparameter copyToParam = ((ListView)selectedTestParam.InnerView).ObjectSpace.CreateObject<Testparameter>();
                                        copyToParam.TestMethod = ((ListView)selectedTestParam.InnerView).ObjectSpace.GetObjectByKey<TestMethod>(testInfo.CurrentTest.Oid);
                                        copyToParam.QCType = ((ListView)selectedTestParam.InnerView).ObjectSpace.GetObjectByKey<QCType>(copyToQCType.Oid);
                                        copyToParam.Parameter = ((ListView)selectedTestParam.InnerView).ObjectSpace.GetObjectByKey<Parameter>(param.Parameter.Oid);

                                        if (copiedParameters != null && !copiedParameters.Contains(copyToParam))
                                        {
                                            copiedParameters.Add(copyToParam);
                                        }
                                        if (testInfo != null && testInfo.NewTestParameters != null && !testInfo.NewTestParameters.Contains(copyToParam))
                                        {
                                            testInfo.NewTestParameters.Add(copyToParam);
                                        }
                                    }
                                }
                            }

                            if (copyUnsavedNewParameters != null)
                            {
                                foreach (Testparameter param in copyUnsavedNewParameters.ToList())
                                {
                                    //copyToQCType.Testparameter.Add(copyToObjectSpace.GetObject<Testparameter>(param));

                                    if (((ListView)selectedTestParam.InnerView).ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.Oid] = ? And [Parameter.Oid] = ?", testInfo.CurrentTest.Oid, copyToQCType.Oid, param.Parameter.Oid)) == null)
                                    {
                                        Testparameter copyToParam = ((ListView)selectedTestParam.InnerView).ObjectSpace.CreateObject<Testparameter>();
                                        copyToParam.TestMethod = ((ListView)selectedTestParam.InnerView).ObjectSpace.GetObjectByKey<TestMethod>(testInfo.CurrentTest.Oid);
                                        copyToParam.QCType = ((ListView)selectedTestParam.InnerView).ObjectSpace.GetObjectByKey<QCType>(copyToQCType.Oid);
                                        copyToParam.Parameter = ((ListView)selectedTestParam.InnerView).ObjectSpace.GetObjectByKey<Parameter>(param.Parameter.Oid);

                                        if (copiedParameters != null && !copiedParameters.Contains(copyToParam))
                                        {
                                            copiedParameters.Add(copyToParam);
                                        }
                                        if (testInfo != null && testInfo.NewTestParameters != null && !testInfo.NewTestParameters.Contains(copyToParam))
                                        {
                                            testInfo.NewTestParameters.Add(copyToParam);
                                        }
                                    }
                                }
                            }

                            if (testInfo.ModifiedQCTypes == null)
                            {
                                testInfo.ModifiedQCTypes = new List<QCType>();
                            }

                            int modifiedObjectIndex = testInfo.ModifiedQCTypes.ToList().FindIndex(x => x != null && x.Oid == copyToQCType.Oid);
                            if (modifiedObjectIndex == -1)
                            {
                                testInfo.ModifiedQCTypes.Add(copyToQCType);
                            }
                            else
                            {
                                testInfo.ModifiedQCTypes[modifiedObjectIndex] = copyToQCType;
                            }

                            ((ListView)selectedTestParam.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                            if (copiedParameters != null)
                            {
                                foreach (Testparameter param in copiedParameters)
                                {
                                    ((ListView)selectedTestParam.InnerView).CollectionSource.Add(param);
                                }
                            }

                            //((ListView)selectedTestParam.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", groups);
                            selectedTestParam.InnerView.Refresh();

                            List<Testparameter> selectedParams = ((ListView)selectedTestParam.InnerView).CollectionSource.List.Cast<Testparameter>().ToList();
                            if (selectedParams != null)
                            {
                                if (selectedParams.Count > 0)
                                {
                                    ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [ParameterName] In(" + string.Format("'{0}'", string.Join("','", selectedParams.Select(i => i.Parameter.ParameterName.Replace("'", "''")))) + ")");
                                }
                            }
                            availableTestParamView.InnerView.Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Select at least one QCType", InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }

                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select at least one Runtype", InformationType.Info, timer.Seconds, InformationPosition.Top);
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
                if (!string.IsNullOrEmpty(parameter))
                {
                    string[] values = parameter.Split('|');
                    if (values[0] == "TestParameterselection")
                    {
                        if (values[1] == "Selected")
                        {
                            Guid curguid = new Guid(values[2]);
                            if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                            {
                                List<object> groups = new List<object>();
                                string strCriteria = string.Empty;
                                DashboardViewItem availableTestParamView = ((NestedFrame)Frame).ViewItem.View.FindItem("AvailableTestParam") as DashboardViewItem;
                                DashboardViewItem selectedTestParamView = ((NestedFrame)Frame).ViewItem.View.FindItem("SelectedTestParam") as DashboardViewItem;
                                DashboardViewItem qcToSampleView = ((NestedFrame)Frame).ViewItem.View.FindItem("QCToSampleView") as DashboardViewItem;

                                if (availableTestParamView != null && availableTestParamView.InnerView != null)
                                {
                                    QCType currentQCType = ObjectSpace.GetObjectByKey<QCType>(curguid);
                                    if (currentQCType != null)
                                    {
                                        ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria.Clear();
                                    }
                                    else
                                    {
                                        ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                                    }

                                    ((ListView)availableTestParamView.InnerView).Refresh();
                                }

                                if (selectedTestParamView != null && selectedTestParamView.InnerView != null)
                                {
                                    ((ListView)selectedTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");

                                    if (testInfo.RemovedTestParameters != null && testInfo.RemovedTestParameters.Count > 0)
                                    {
                                        strCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testInfo.RemovedTestParameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                    }

                                    if (curguid != null && testInfo.CurrentTest != null)
                                    {
                                        TestMethod currentTest = ObjectSpace.GetObjectByKey<TestMethod>(testInfo.CurrentTest.Oid);
                                        if (curguid != null && currentTest != null)
                                        {
                                            IList<Testparameter> testParamList = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And[QCType.Oid] = ?", currentTest.Oid, curguid));
                                            bool HasParams = false;
                                            if (testParamList != null && testParamList.Count > 0)
                                            {
                                                HasParams = true;
                                                if (!string.IsNullOrEmpty(strCriteria))
                                                {
                                                    strCriteria = strCriteria + "and [Oid] In(" + string.Format("'{0}'", string.Join("','", testParamList.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                                }
                                                else
                                                {
                                                    strCriteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", testParamList.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(strCriteria) && HasParams == true)
                                            {
                                                ((ListView)selectedTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(strCriteria);
                                            }

                                            if (testInfo.NewTestParameters != null && testInfo.NewTestParameters.Count > 0)
                                            {
                                                foreach (Testparameter param in testInfo.NewTestParameters)
                                                {
                                                    if (param.QCType.Oid == testInfo.CurrentQcType.Oid && param.TestMethod.Oid == testInfo.CurrentTest.Oid)
                                                    {
                                                        ((ListView)selectedTestParamView.InnerView).CollectionSource.Add(param);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    ((ListView)selectedTestParamView.InnerView).Refresh();

                                    if (availableTestParamView != null && availableTestParamView.InnerView != null)
                                    {
                                        ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria.Clear();
                                        List<Testparameter> selectedParams = ((ListView)selectedTestParamView.InnerView).CollectionSource.List.Cast<Testparameter>().ToList();
                                        if (selectedParams != null && selectedParams.Count > 0)
                                        {
                                            ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [ParameterName] In(" + string.Format("'{0}'", string.Join("','", selectedParams.Select(i => i.Parameter.ParameterName.Replace("'", "''")))) + ")");
                                        }

                                        ((ListView)availableTestParamView.InnerView).Refresh();
                                    }

                                }

                                if (qcToSampleView != null && qcToSampleView.InnerView != null)
                                {
                                    List<object> CopyFromGroup = new List<object>();

                                    if (testInfo.ModifiedQCTypes != null && testInfo.ModifiedQCTypes.Count > 0)
                                    {
                                        foreach (QCType obj in testInfo.ModifiedQCTypes)
                                        {
                                            if (obj.Oid != testInfo.CurrentQcType.Oid)
                                            {
                                                CopyFromGroup.Add(obj.Oid);
                                            }
                                        }
                                    }

                                    TestMethod currentTestMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", testInfo.CurrentTest.Oid));
                                    if (currentTestMethod != null && currentTestMethod.QCTypes != null && currentTestMethod.QCTypes.Count > 0)
                                    {
                                        foreach (QCType qcType in currentTestMethod.QCTypes)
                                        {
                                            if (testInfo.CurrentQcType != null && testInfo.CurrentQcType.Oid != qcType.Oid)
                                            {
                                                IList<Testparameter> testlist = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod]=? and [QCType]=?", testInfo.CurrentTest.Oid, qcType.Oid));
                                                if (testlist != null && testlist.Count > 0)
                                                {
                                                    CopyFromGroup.Add(qcType.Oid);
                                                }
                                            }
                                        }
                                    }

                                    if (CopyFromGroup.Count > 0)
                                    {
                                        ((ListView)qcToSampleView.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", CopyFromGroup);
                                    }
                                    else
                                    {
                                        ((ListView)qcToSampleView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                                    }
                                    ((ListView)qcToSampleView.InnerView).Refresh();
                                }


                            }
                        }
                        else if (values[1] == "UNSelected")
                        {
                            if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                            {
                                DashboardViewItem availableTestParamView = ((NestedFrame)Frame).ViewItem.View.FindItem("AvailableTestParam") as DashboardViewItem;
                                if (availableTestParamView != null && availableTestParamView.InnerView != null && availableTestParamView.InnerView is ListView && ((ListView)availableTestParamView.InnerView).SelectedObjects.Count == 0)
                                {
                                    ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                                    ((ListView)availableTestParamView.InnerView).Refresh();
                                }
                            }
                        }
                        else if (values[1] == "UNSelectall")
                        {
                            if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                            {
                                if (((ListView)View).SelectedObjects.Count == 0)
                                {
                                    DashboardViewItem availableTestParamView = ((NestedFrame)Frame).ViewItem.View.FindItem("AvailableTestParam") as DashboardViewItem;
                                    if (availableTestParamView != null && availableTestParamView.InnerView != null && availableTestParamView.InnerView is ListView && ((ListView)availableTestParamView.InnerView).SelectedObjects.Count == 0)
                                    {
                                        ((ListView)availableTestParamView.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                                        ((ListView)availableTestParamView.InnerView).Refresh();
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                    else if (values[0] == "true" && Frame is DevExpress.ExpressApp.Web.PopupWindow)
                    {
                        testInfo.OpenSettings = true;
                        (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void gotoTestparameterEditAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TestParameterSettings")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(TestMethod));
                    ListView lstviewTestMethod = Application.CreateListView("TestMethod_ListView_Copy", cs, false);
                    //ListView lstviewTestMethod = Application.CreateListView(typeof(TestMethod),false);
                    //DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "TestParamterEdit", false);
                    e.ShowViewParameters.CreatedView = lstviewTestMethod;
                    e.ShowViewParameters.Context = TemplateContext.NestedFrame;
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.AcceptAction.Active.SetItemValue("disable", false);
                    dc.CancelAction.Active.SetItemValue("disable", false);
                    dc.CloseOnCurrentObjectProcessing = false;
                    e.ShowViewParameters.Controllers.Add(dc);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void copyToAllQCParamAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "QcParameter_ListView_DefaultSettings")
        //        {
        //            if (View.SelectedObjects.Count==1)
        //            {
        //                bool isCopied = false;

        //                Testparameter sourceParam = (Testparameter)((ListView)View).CurrentObject;
        //                if (sourceParam != null)
        //                {
        //                    foreach (Testparameter param in ((ListView)View).CollectionSource.List)
        //                    {
        //                        if (param.Oid!=sourceParam.Oid)
        //                        {
        //                            //param.Parameter = ((ListView)View).ObjectSpace.GetObjectByKey<Parameter>(sourceParam.Parameter.Oid);
        //                            param.Surroagate = sourceParam.Surroagate;
        //                            //param.Sort = sourceParam.Sort;
        //                            param.InternalStandard = sourceParam.InternalStandard;
        //                            param.STDConc = sourceParam.STDConc;
        //                            param.STDVolAdd = sourceParam.STDVolAdd;
        //                            param.SpikeAmount = sourceParam.SpikeAmount;
        //                            param.RecLCLimit = sourceParam.RecLCLimit;
        //                            param.RecHCLimit = sourceParam.RecHCLimit;
        //                            param.RPDLCLimit = sourceParam.RPDLCLimit;
        //                            param.RPDHCLimit = sourceParam.RPDHCLimit;
        //                            param.LowCLimit = sourceParam.LowCLimit;
        //                            param.HighCLimit = sourceParam.HighCLimit;
        //                            param.RecLCLimit = sourceParam.RecLCLimit;
        //                            param.REHCLimit = sourceParam.REHCLimit;
        //                            param.SigFig = sourceParam.SigFig;
        //                            param.CutOff = sourceParam.CutOff;
        //                            param.Decimal = sourceParam.Decimal;
        //                            param.RetireDate = sourceParam.RetireDate;
        //                            param.Comment = sourceParam.Comment;

        //                            if (sourceParam.STDConcUnit != null)
        //                            {
        //                                param.STDConcUnit = ((ListView)View).ObjectSpace.GetObjectByKey<Unit>(sourceParam.STDConcUnit.Oid);
        //                            }
        //                            else
        //                            {
        //                                param.STDConcUnit = null;
        //                            }

        //                            if (sourceParam.STDVolUnit != null)
        //                            {
        //                                param.STDVolUnit = ((ListView)View).ObjectSpace.GetObjectByKey<Unit>(sourceParam.STDVolUnit.Oid);
        //                            }
        //                            else
        //                            {
        //                                param.STDVolUnit = null;
        //                            }

        //                            if (sourceParam.SpikeAmountUnit != null)
        //                            {
        //                                param.SpikeAmountUnit = ((ListView)View).ObjectSpace.GetObjectByKey<Unit>(sourceParam.SpikeAmountUnit.Oid);
        //                            }
        //                            else
        //                            {
        //                                param.SpikeAmountUnit = null;
        //                            }

        //                            isCopied = true;
        //                        }
        //                    }                            
        //                }

        //                if (isCopied == true)
        //                {
        //                    ((ListView)View).Refresh();
        //                }
        //            }
        //            else
        //            {
        //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void copyToAllTestParamAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "TestParameter_ListView_DefaultSettings")
        //        {
        //            if (View.SelectedObjects.Count == 1)
        //            {
        //                bool isCopied = false;
        //                Testparameter sourceParam = (Testparameter)((ListView)View).CurrentObject;
        //                if (sourceParam != null)
        //                {
        //                    foreach (Testparameter param in ((ListView)View).CollectionSource.List)
        //                    {
        //                        if (param.Oid != sourceParam.Oid)
        //                        {
        //                            //param.Parameter = ((ListView)View).ObjectSpace.GetObjectByKey<Parameter>(sourceParam.Parameter.Oid);
        //                            param.Surroagate = sourceParam.Surroagate;
        //                            //param.Sort = sourceParam.Sort;
        //                            param.InternalStandard = sourceParam.InternalStandard;
        //                            param.DefaultResult = sourceParam.DefaultResult;
        //                            param.FinalDefaultResult = sourceParam.FinalDefaultResult;
        //                            param.SurrogateAmount = sourceParam.SurrogateAmount;
        //                            param.SurrogateHighLimit = sourceParam.SurrogateHighLimit;
        //                            param.SurrogateLowLimit = sourceParam.SurrogateLowLimit;
        //                            param.LOQ = sourceParam.LOQ;
        //                            param.UQL = sourceParam.UQL;
        //                            param.RptLimit = sourceParam.RptLimit;
        //                            param.MDL = sourceParam.MDL;
        //                            param.MCL = sourceParam.MCL;
        //                            param.SigFig = sourceParam.SigFig;
        //                            param.CutOff = sourceParam.CutOff;
        //                            param.Decimal = sourceParam.Decimal;
        //                            param.Comment = sourceParam.Comment;
        //                            param.RetireDate = sourceParam.RetireDate;

        //                            if (sourceParam.DefaultUnits != null)
        //                            {
        //                                param.DefaultUnits = ((ListView)View).ObjectSpace.GetObjectByKey<Unit>(sourceParam.DefaultUnits.Oid);
        //                            }
        //                            else
        //                            {
        //                                param.DefaultUnits = null;
        //                            }

        //                            if (sourceParam.FinalDefaultUnits != null)
        //                            {
        //                                param.FinalDefaultUnits = ((ListView)View).ObjectSpace.GetObjectByKey<Unit>(sourceParam.FinalDefaultUnits.Oid);
        //                            }
        //                            else
        //                            {
        //                                param.FinalDefaultUnits = null;
        //                            }

        //                            if (sourceParam.SurrogateUnits != null)
        //                            {
        //                                param.SurrogateUnits = ((ListView)View).ObjectSpace.GetObjectByKey<Unit>(sourceParam.SurrogateUnits.Oid);
        //                            }
        //                            else
        //                            {
        //                                param.SurrogateUnits = null;
        //                            }

        //                            isCopied = true;
        //                        }
        //                    }

        //                }

        //                if (isCopied==true)
        //                {
        //                    ((ListView)View).Refresh();
        //                }
        //            }
        //            else
        //            {
        //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
    }
}
