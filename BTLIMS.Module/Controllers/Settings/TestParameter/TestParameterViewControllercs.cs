﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors;
//using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using Component = Modules.BusinessObjects.Setting.Component;

namespace LDM.Module.Web.Controllers.TestParameter
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TestParameterViewControllercs : ViewController, IXafCallbackHandler
    {
        #region Declaration
        bool isprocessact = false;
        bool chkparametercommitted = false;
        bool chktestcommitted = false;
        bool chkunlink = false;
        bool chkpricetype = false;
        bool Isseleted = false;
        string strtmOid = string.Empty;
        string strtestmethod = string.Empty;
        List<string> lststrtestmethod = new List<string>();
        ASPxGridListEditor GridListEditor;
        MessageTimer timer = new MessageTimer();
        TestInfo testInfo = new TestInfo();
        TestmethodQctypeinfo qctypeinfo = new TestmethodQctypeinfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        testparameter objtestparametrinfo = new testparameter();
        Testmethodmethodname lstmethodname = new Testmethodmethodname();
        Testpriceinfo objtestpriceinfo = new Testpriceinfo();
        GroupTestParameterInfo grptstinfo = new GroupTestParameterInfo();
        ICallbackManagerHolder saveAlert;
        ModificationsController mdcSave;
        ModificationsController mdcSaveClose;
        ModificationsController mdcSaveNew;
        ModificationsController msdcunlink;
        ModificationsController mdcCancel;
        DashboardViewItem lstpertest = null;
        DashboardViewItem lstperparameter = null;
        DashboardViewItem vipertest = null;
        DashboardViewItem viperparameter = null;
        Testpriceinfo testpriceinfo = new Testpriceinfo();
        TestMethodInfo objTMInfo = new TestMethodInfo();
        AuditInfo objAuditInfo = new AuditInfo();

        //TATinfo tatinfo = new TATinfo();
        string strpricetype = string.Empty;
        //const string BatchEditLostFocus =
        //    @"function (s, e) {
        //    var clientGridView = s.grid;
        //    if (!window.batchPreventEndEditOnLostFocus)
        //        clientGridView.batchEditApi.EndEdit();
        //    window.batchPreventEndEditOnLostFocus = false;
        //}";


        #endregion

        #region Constructor
        public TestParameterViewControllercs()
        {
            InitializeComponent();
            this.TargetViewId = "TestPrice_ListView;" + "TestPrice_ListView_Copy_pertest;" + "TestPrice_ListView_Copy_perparameter;" + "TestPrice_DetailView;" + "TestMethod_DetailView;" + "Testparameter_ListView;" + "Testparameter_ListView_Copy;" + "TestParamterEdit;" + "TestMethod_DetailView_CopyParameter;" + "TestGuide_DetailView;"
                + "TestMethod_QCTypes_ListView_CopyTo;" + "TestMethod_ListView;" + "TestMethod_ListView_Copyto;" + "Testparameter_DetailView_CopyTest;" + "TestMethod_DetailView_CopyTest;"
                + "TestCopy_ListView;" + "Testparameter_DetailView_Copyparameters;" + "Testparameter_ListView_Test_Surrogates;" + "Testparameter_ListView_Test_QCSampleParameter;"
                + "TestMethod_QCTypes_ListView;" + "Testparameter_ListView_Test_SampleParameter_Copy;" + "Parameter_LookupListView_Surrogates;" + "TestMethod_PrepMethods_ListView;"
                + "Method_ListView_Copy_TestMethod;" + "TestMethod_SamplingMethods_ListView;" + "TestMethod_Preservatives_ListView;" + "TestMethod_surrogates_ListView;"
                + "Testparameter_ListView_Test_SampleParameter;" + "Testparameter_ListView_Test_InternalStandards;" + "Component_ListView_Test;" + "Testparameter_DetailView_QCSampleParameter;" + "TestMethod;" + "TestMethod_DetailView_Copy;"
                + "TestMethod_ListView_Copy_CopyTest;" + "Parameter_ListView_Test;" + "TestMethod_DetailView_Copy;" + "PrepMethod_DetailView;" + "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault;"
                + "Testparameter_ListView_Test_Component;" + "Parameter_LookupListView_Component;" + "Component_DetailView_Test;" + "Component_LookupListView;" + "TestPriceDetail_ListView_Copy_pertest;" + "TestPriceDetail_ListView_Copy_perparameter;" + "TestPrice_DetailView;"
                + "Method_ListView_Combo;" + "GroupTestMethod_ListView_IsGroup;" + "Testparameter_ListView_Testmethod_IsGroup;" + "Testparameter_ListView_Testmethod_IsGroup_parameter_popup;"
                + "ResultDefaultValue_LookupListView;"+ "Accrediation_ListView_Copy;" + "TestMethod_TestGuides_ListView;";
            this.parameterAction.TargetViewId = "TestMethod_DetailView;";
            this.Copyparameter.TargetViewId = "TestMethod_DetailView1;";
            this.CopyTest.TargetViewId = "TestMethod_DetailView;";
            this.CopyParameters.TargetViewId = "TestMethod_DetailView;";
            this.ADDAction.TargetViewId = "TestPrice_ListView;" + "TestPriceDetail_ListView_Copy_pertest;" + "TestPriceDetail_ListView_Copy_perparameter;" + "Testparameter_ListView_Test_SampleParameter;" + "Testparameter_ListView_Test_InternalStandards;" + "Component_ListView_Test;" + "Testparameter_ListView_Test_QCSampleParameter;" + "Testparameter_ListView_Test_Surrogates;"
                + "Testparameter_ListView_Test_Component;" + "GroupTestMethod_ListView_IsGroup;";
            this.RemoveAction.TargetViewId = "TestPrice_ListView;" + "TestPriceDetail_ListView_Copy_pertest;" + "TestPriceDetail_ListView_Copy_perparameter;" + "Testparameter_ListView_Test_SampleParameter;" + "Testparameter_ListView_Test_InternalStandards;" + "Component_ListView_Test;" + "Testparameter_ListView_Test_QCSampleParameter;" + "Testparameter_ListView_Test_Surrogates;"
                + "Testparameter_ListView_Test_Component;" + "GroupTestMethod_ListView_IsGroup;";
            this.SaveAction.TargetViewId = "TestPriceDetail_ListView_Copy_pertest;" + "TestPriceDetail_ListView_Copy_perparameter;";
            this.TestDefaultResult.TargetViewId = "Testparameter_ListView_Test_SampleParameter;";
            this.AddNewTestDefaultResult.TargetViewId = "ResultDefaultValue_LookupListView;";
            this.DeleteTestDefaultResult.TargetViewId = "ResultDefaultValue_LookupListView;";

        }
        #endregion

        #region DefaultEvent
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "TestMethod_DetailView_CopyTest" || View.Id == "Testparameter_DetailView_CopyTest" || View.Id == "Testparameter_DetailView_Copyparameters")
                {
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated += ChangeLayoutGroupCaptionViewController_ItemCreated;
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                if (Frame is DevExpress.ExpressApp.Web.PopupWindow && Frame.View != null && (Frame.View.Id == "Testparameter_DetailView_CopyTest" || Frame.View.Id == "TestMethod"))
                {
                    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                    if (popupWindow != null)
                    {
                        popupWindow.RefreshParentWindowOnCloseButtonClick = true;// This is for the cross (X) button of ASPxPopupControl.
                        DialogController dc = popupWindow.GetController<DialogController>();
                        if (dc != null)
                        {
                            dc.Accepting += SaveTest_Execute;
                            dc.SaveOnAccept = false;
                        }
                    }
                }
                if (View is ListView)
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
                else if (View is DetailView)
                {
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
                if (View != null && View.Id == "TestPrice_DetailView")
                {
                    //((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "TestGuide_DetailView" || View.Id == "PrepMethod_DetailView")
                {

                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    if (View.Id == "PrepMethod_DetailView")
                    {
                        View.Closing += View_Closing;
                    }

                }
                else if (View.Id == "TestMethod_DetailView")
                {
                    View.QueryCanClose += View_QueryCanClose;
                    CopyTest.Executing += CopyTest_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed += DeleteAction_Executed;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    mdcSave = Frame.GetController<ModificationsController>();
                    mdcSave.SaveAction.Executing += SaveAction_Executing;
                    mdcSaveClose = Frame.GetController<ModificationsController>();
                    mdcSaveClose.SaveAndCloseAction.Executing += SaveAction_Executing;
                    mdcSaveNew = Frame.GetController<ModificationsController>();
                    mdcSaveNew.SaveAndNewAction.Executing += SaveAction_Executing;
                    Frame.GetController<RefreshController>().RefreshAction.Executing += RefreshAction_Executing;
                    if (CopyParameters.Items.Count > 0 && CopyParameters.Items != null)
                    {
                        CopyParameters.SelectedItemChanged += CopyParameters_SelectedItemChanged;
                    }
                    testInfo.lstSampleParameters = new List<Testparameter>();
                    testInfo.lstRemovedSampleParameters = new List<Testparameter>();
                    testInfo.lstSampleParameters = new List<Testparameter>();
                    testInfo.lstRemovedQcParameters = new List<Testparameter>();
                    testInfo.IsNewTest = ObjectSpace.IsNewObject((TestMethod)View.CurrentObject);
                    mdcCancel = Frame.GetController<ModificationsController>();
                    mdcCancel.CancelAction.Executing += CancelAction_Executing;
                    var editor = (WebPropertyEditor)((DetailView)View).FindItem("QCtypesCombo");
                    if (editor != null) editor.NullText = " ";
                    objPermissionInfo.TestsViewEditMode = ((DetailView)View).ViewEditMode;
                    objPermissionInfo.TestsIsWrite = false;
                    objPermissionInfo.TestsIsDelete = false;

                    Employee currentUser = SecuritySystem.CurrentUser as Employee;
                    if (currentUser != null && currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.TestsIsWrite = true;
                            objPermissionInfo.TestsIsDelete = true;
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
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
                    if (objPermissionInfo.TestsIsWrite == true && objPermissionInfo.TestsViewEditMode == ViewEditMode.Edit)
                    {
                        //parameterAction.Active["ShowParameter"] = true;
                    }
                    else
                    {
                        //parameterAction.Active["ShowParameter"] = false;
                    }
                }
                else
                if (View.Id == "TestMethod_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                }
                else
                if (View.Id == "TestMethod")
                {
                    Application.DetailViewCreating += Application_DetailViewCreating;
                    DashboardViewItem viAvailableFields = ((DashboardView)View).FindItem("CopyTestSettings") as DashboardViewItem;
                    if (viAvailableFields != null)
                    {
                        viAvailableFields.ControlCreated += ViAvailableFields_ControlCreated;
                    }
                }
                else
                if (View.Id == "TestMethod_DetailView_Copy")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("CopyTest") as DashboardViewItem;
                    if (viAvailableFields != null)
                    {
                        viAvailableFields.ControlCreated += ViAvailableFields_ControlCreated;
                    }
                }
                else
                if (View.Id == "TestMethod_QCTypes_ListView")
                {
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Executed += UnlinkAction_Executed;
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing += UnlinkAction_Executing;
                }
                else
                if (View.Id == "Testparameter_DetailView_CopyTest")
                {
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("CopyTest") as DashboardViewItem;
                    if (viAvailableFields != null)
                    {
                        viAvailableFields.ControlCreated += ViAvailableFields_ControlCreated;
                    }
                }
                else
                if (View.Id == "Testparameter_ListView_Test_QCSampleParameter")
                {
                    ((ListView)View).CollectionSource.Criteria["QCFilter"] = CriteriaOperator.Parse("oid is null");
                }
                else if (View.Id == "TestMethod_QCTypes_ListView_CopyTo")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    TestMethod maincurrentobj = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (maincurrentobj != null)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        if (view != null && view is DetailView)
                        {
                            TestMethod currentobj = (TestMethod)view.CurrentObject;
                            currentobj.TestName = maincurrentobj.TestName;
                            ListPropertyEditor lstqctypes = ((DetailView)Application.MainWindow.View).FindItem("QCTypes") as ListPropertyEditor;
                            ListView lstprpedit = lstqctypes.ListView;
                            List<QCType> lstobjqctype = new List<QCType>();
                            DashboardViewItem lstcrtqctypesct = ((DetailView)view).FindItem("QCTypeto") as DashboardViewItem;
                            ListView lstqctypesct = lstcrtqctypesct.InnerView as ListView;
                            if (lstqctypes.ListView != null && lstprpedit.CollectionSource.List.Count > 0)
                            {
                                //foreach (QCType objqctype in lstprpedit.CollectionSource.List.Cast<QCType>().ToList())
                                //{
                                //    lstobjqctype.Add(objqctype);
                                //}
                                lstobjqctype = lstprpedit.CollectionSource.List.Cast<QCType>().Where(i => i.QCTypeName != null).ToList();
                                if (lstcrtqctypesct != null && lstcrtqctypesct.InnerView != null && lstobjqctype != null)
                                {
                                    ((ListView)lstqctypesct).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstobjqctype.Select(i => i.Oid.ToString().Replace("'", "''")).Distinct())) + ")");
                                }
                            }
                            else if (lstqctypes.ListView != null && lstprpedit.CollectionSource.List.Count == 0)
                            {
                                if (lstcrtqctypesct != null && lstcrtqctypesct.InnerView != null)
                                {
                                    ((ListView)lstqctypesct).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[QCTypeName] = 'Sample'");
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "Parameter_LookupListView_Component")
                {
                    TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (objTest != null)
                    {
                        //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestParameter][[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample']", objTest.Oid);

                    }
                }
                else if (View.Id == "Testparameter_ListView_Test_Component")
                {

                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    Component objComponent = (Component)view.CurrentObject;
                    if (objComponent != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Component=?", objComponent.Oid);
                    }
                }
                else if (View.Id == "Component_DetailView_Test")
                {
                    View.Closed += View_ClosedInComponent;
                }
                else if (View.Id == "ResultDefaultValue_LookupListView")
                {
                    //View.ControlsCreated += View_ControlsCreated;
                    DashboardViewItem dvSampleparam = ((DetailView)Application.MainWindow.View).FindItem("Sampleparameter") as DashboardViewItem;
                    if (dvSampleparam != null && dvSampleparam.InnerView != null && dvSampleparam.InnerView.SelectedObjects.Count == 1)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        Testparameter objTest = (Testparameter)dvSampleparam.InnerView.CurrentObject;
                        if (objTest != null && !string.IsNullOrEmpty(objTest.ParameterDefaultResults))
                        {
                            List<String> lstDefaultResult = objTest.ParameterDefaultResults.Split(';').Select(i => i.Trim()).ToList();
                            ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstDefaultResult.Select(i => new Guid(i)));
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is Null");
                        }
                    }

                }
                else if (View.Id == "TestMethod_PrepMethods_ListView" ||View.Id== "TestMethod_TestGuides_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;

                }
                if (View != null && View.Id == "PrepMethod_DetailView")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(TestMethod))
                    {
                        TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                        if (objtm != null)
                        {
                            testInfo.methodguid = objtm.MethodName.Oid;
                        }
                    }
                }
                //else if(View.Id== "Testparameter_LookupListView_Component")
                // {
                //     TestMethod objTest =(TestMethod) Application.MainWindow.View.CurrentObject;
                //     if(objTest!=null)
                //     {
                //         IList<Component> objComponent = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("TestMethod=?", objTest.Oid));
                //         if(objComponent!=null && objComponent.Count>0)
                //         {
                //             List<Guid> lstParamOid = new List<Guid>();
                //             foreach (Component obj in objComponent)
                //             {
                //                foreach(Testparameter testparameter in obj.TestParameters)
                //                 {
                //                     lstParamOid.Add(testparameter.Oid);
                //                 }
                //             }
                //             if(lstParamOid.Count>0)
                //             {
                //                 ((ListView)View).CollectionSource.Criteria["Filter1"] = new NotOperator(new InOperator("Oid", lstParamOid));
                //             }
                //         }
                //     }
                // }
                if (View.Id == "GroupTestMethod_ListView_IsGroup")
                {
                    if (testInfo.delGtestmethod == null)
                    {
                        testInfo.delGtestmethod = new List<GroupTestMethod>();
                    }
                    if (Application.MainWindow.View.Id == "TestMethod_DetailView")
                    {
                        TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                        if (objtm != null)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid);
                        }
                    }

                }
                else if (View.Id == "Testparameter_ListView_Testmethod_IsGroup")
                {
                    DashboardViewItem DVgrouptest = ((DetailView)Application.MainWindow.View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                    if (DVgrouptest != null && DVgrouptest.InnerView != null)
                    {
                        foreach (GroupTestMethod objgtm in ((ListView)DVgrouptest.InnerView).CollectionSource.List)
                        {
                            if (objgtm.TestParameter != null)
                            {
                                Testparameter objtp = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", objgtm.TestParameter.Oid));
                                if (objtp != null && objtp.TestMethod != null && objtp.Component != null)
                                {
                                    strtestmethod = objtp.TestMethod.TestName + "| " + objtp.TestMethod.MethodName.MethodNumber + "| " + objtp.Component.Components;
                                    if (!lststrtestmethod.Contains(strtestmethod) && !string.IsNullOrEmpty(strtestmethod))
                                    {
                                        lststrtestmethod.Add(strtestmethod);
                                    }
                                }
                            }
                        }
                    }
                    TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (objtm != null && objtm.MatrixName != null)
                    {
                        List<Guid> GroupOid = new List<Guid>();
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Testparameter)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [QCType.QCTypeName] = 'Sample' And ([IsGroup] <> True Or [IsGroup] Is Null)", objtm.MatrixName.MatrixName);
                            lstview.Properties.Add(new ViewProperty("TestMethodOid", SortDirection.Ascending, "Oid", true, true));
                            foreach (ViewRecord Vrec in lstview)
                            {
                                Testparameter objtpara = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", new Guid(Vrec["TestMethodOid"].ToString())));
                                if (objtpara != null && objtpara.TestMethod != null && objtpara.TestMethod.MethodName != null && objtpara.Component != null)
                                {
                                    strtestmethod = objtpara.TestMethod.TestName + "| " + objtpara.TestMethod.MethodName.MethodNumber + "| " + objtpara.Component.Components;
                                    if (!lststrtestmethod.Contains(strtestmethod) && !string.IsNullOrEmpty(strtestmethod))
                                    {
                                        lststrtestmethod.Add(strtestmethod);
                                        GroupOid.Add(new Guid(Vrec["TestMethodOid"].ToString()));
                                    }
                                }
                            }
                        }
                        if (GroupOid != null && GroupOid.Count > 0)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", GroupOid);
                        }
                    }
                }
                ObjectSpace.Committed += ObjectSpace_Committed;
                chkparametercommitted = false;
                chktestcommitted = false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void UnlinkAction_Executing(object sender, CancelEventArgs e)
        {
            if (View.ObjectTypeInfo.Type == typeof(QCType) && View.Id == "TestMethod_QCTypes_ListView")
            {
                if (View.SelectedObjects.Count > 0)
                {
                    TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (objTest != null)
                    {
                        foreach (QCType qCType in View.SelectedObjects)
                        {
                            UnitOfWork uow = new UnitOfWork(((XPObjectSpace)View.ObjectSpace).Session.DataLayer);
                            if (uow.Query<Testparameter>().Where(i => i.TestMethod != null && i.QCType != null && i.TestMethod.Oid == objTest.Oid && i.QCType.Oid == qCType.Oid).FirstOrDefault() != null)
                            {
                                Application.ShowViewStrategy.ShowMessage("Please remove the qc parameters first", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void View_Closing(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "PrepMethod_DetailView")
                {
                    PrepMethod objPrepmethod = (PrepMethod)View.CurrentObject;
                    if (objPrepmethod != null)
                    {
                        ListPropertyEditor lv = ((DetailView)Application.MainWindow.View).FindItem("PrepMethods") as ListPropertyEditor;
                        uint sort = 1;
                        foreach (PrepMethod objPrep in ((ListView)lv.ListView).CollectionSource.List.Cast<PrepMethod>().Where(i => i.Tier != null).OrderBy(i => i.Tier))
                        {
                            objPrep.Sort = sort;
                            sort++;
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

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                View.ControlsCreated -= View_ControlsCreated;
                if (View.Id == "ResultDefaultValue_LookupListView")
                {
                    DashboardViewItem dvSampleparam = ((DetailView)Application.MainWindow.View).FindItem("Sampleparameter") as DashboardViewItem;
                    if (dvSampleparam != null && dvSampleparam.InnerView != null && dvSampleparam.InnerView.SelectedObjects.Count == 1)
                    {

                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        Testparameter objTest = (Testparameter)dvSampleparam.InnerView.CurrentObject;
                        if (objTest != null && !string.IsNullOrEmpty(objTest.ParameterDefaultResults))
                        {
                            List<String> lstDefaultResult = objTest.ParameterDefaultResults.Split(';').Select(i => i.Trim()).ToList();
                            ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstDefaultResult.Select(i => new Guid(i)));
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is Null");
                        }
                        //if (objTest != null && !string.IsNullOrEmpty(objTest.DefaultResultValue))
                        //{
                        //    List<String> lstDefaultResult = objTest.DefaultResultValue.Split(';').Select(i => i).ToList();
                        //    foreach (string objValue in lstDefaultResult.ToList())
                        //    {
                        //        ResultDefaultValue objVal = View.ObjectSpace.FindObject<ResultDefaultValue>(CriteriaOperator.Parse("[Oid]=?",new Guid(objValue)));
                        //        if (objVal != null)
                        //        {
                        //            gridListEditor.Grid.Selection.SelectRowByKey(objVal.Oid);
                        //        }
                        //    }
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

        private void ChangeLayoutGroupCaptionViewController_ItemCreated(object sender, ItemCreatedEventArgs e)
        {
            try
            {
                if (e.ViewItem is PropertyEditor && (((PropertyEditor)e.ViewItem).PropertyName == "Matrix" || ((PropertyEditor)e.ViewItem).PropertyName == "MatrixName"))
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
                if (e.ViewItem is PropertyEditor && (((PropertyEditor)e.ViewItem).PropertyName == "Test" || ((PropertyEditor)e.ViewItem).PropertyName == "TestName"))
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
                if (e.ViewItem is PropertyEditor && (((PropertyEditor)e.ViewItem).PropertyName == "Method" || ((PropertyEditor)e.ViewItem).PropertyName == "MethodName"))
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_ClosedInComponent(object sender, EventArgs e)
        {
            try
            {
                if (View is DetailView)
                {
                    DashboardViewItem dvSampleparam = ((DetailView)View).FindItem("Parameter") as DashboardViewItem;
                    if (dvSampleparam != null && dvSampleparam.InnerView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)dvSampleparam.InnerView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)dvSampleparam.InnerView).Editor).Grid.UpdateEdit();
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

        private void CopyTest_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (Application.MainWindow.View.Id == "TestMethod_DetailView")
                {
                    testInfo.IsCopyTestAction = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeleteAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_ListView")
                {
                    testInfo.IsTestMethodDelete = false;
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
                if (View.Id == "TestMethod_PrepMethods_ListView")
                {
                    foreach (PrepMethod objPrep in View.SelectedObjects)
                    {
                        if (objPrep.TestMethod.PrepMethods.Count == 2)
                        {
                            List<SampleParameter> lstParam = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [IsPrepMethodComplete] = False And [Testparameter.TestMethod.PrepMethods][].Count() = 2 And [PrepMethodCount] = 1", objPrep.TestMethod.Oid)).ToList();
                            if (lstParam.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "prepmethodcannotdelete"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
                                return;
                            }
                        }
                        if (View.SelectedObjects.Count == 1)
                        {
                            foreach (PrepMethod objPreps in ((ListView)View).CollectionSource.List.Cast<PrepMethod>().Where(i => i.Oid != objPrep.Oid))
                            {
                                objPreps.Sort = 1;
                            }
                        }
                        Frame.GetController<AuditlogViewController>().createdeleteaudit(ObjectSpace, objPrep.TestMethod.Oid, objPrep.TestMethod.TestCode, "PrepMethod", objPrep.PrepType.SamplePrepType.ToString());
                    }
                }

                else if (View.Id == "TestMethod_TestGuides_ListView")
                {
                    foreach (TestGuide objGuide in View.SelectedObjects)
                    {
                        Frame.GetController<AuditlogViewController>().createdeleteaudit(ObjectSpace, objGuide.TestMethod.Oid, objGuide.TestMethod.TestCode, "TestGuide", objGuide.Container.ContainerName.ToString());

                    }
                }
                else
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(Testparameter));
                    if (View.Id == "TestMethod_DetailView")
                    {
                        testInfo.IsTestMethodDelete = true;
                        TestMethod objtm = (TestMethod)View.CurrentObject;
                        if (objtm != null && objtm.IsGroup == true)
                        {
                            Testparameter objtp = os.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid]= ?", objtm.Oid));
                            if (objtp != null)
                            {
                                os.Delete(objtp);
                                os.CommitChanges();
                            }
                        }
                    }
                    else if (View.Id == "TestMethod_ListView" && View.SelectedObjects.Count > 0)
                    {
                        foreach (TestMethod objtm in View.SelectedObjects)
                        {
                            if (objtm.IsGroup == true)
                            {
                                Testparameter objtp = os.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid]= ?", objtm.Oid));
                                if (objtp != null)
                                {
                                    os.Delete(objtp);
                                    os.CommitChanges();
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

        private void CancelAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_DetailView")
                {
                    testInfo.IsCancel = true;
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
                if (View.Id == "TestMethod_DetailView")
                {
                    DashboardViewItem dvSampleparam = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                    DashboardViewItem dvQCSampleparam = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                    DashboardViewItem dvInternalStandards = ((DetailView)View).FindItem("InternalStandards") as DashboardViewItem;
                    DashboardViewItem dvSurrogates = ((DetailView)View).FindItem("dvSurrogates") as DashboardViewItem;
                    DashboardViewItem dvComponents = ((DetailView)View).FindItem("Components") as DashboardViewItem;
                    DashboardViewItem dvQCParameterDefault = ((DetailView)View).FindItem("QCParameterDefault") as DashboardViewItem;
                    if (dvSampleparam != null && dvSampleparam.InnerView != null)
                    {
                        dvSampleparam.InnerView.ObjectSpace.Refresh();
                    }
                    if (dvQCSampleparam != null && dvQCSampleparam.InnerView != null)
                    {
                        dvQCSampleparam.InnerView.ObjectSpace.Refresh();
                    }
                    if (dvInternalStandards != null && dvInternalStandards.InnerView != null)
                    {
                        dvInternalStandards.InnerView.ObjectSpace.Refresh();
                    }
                    if (dvSurrogates != null && dvSurrogates.InnerView != null)
                    {
                        dvSurrogates.InnerView.ObjectSpace.Refresh();
                    }
                    if (dvQCParameterDefault != null && dvSurrogates.InnerView != null)
                    {
                        dvQCParameterDefault.InnerView.ObjectSpace.Refresh();
                    }
                    if (dvComponents != null && dvComponents.InnerView != null)
                    {
                        dvComponents.InnerView.ObjectSpace.Refresh();
                    }
                }
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
                DashboardViewItem dvSampleparam = null;
                DashboardViewItem dvQCSampleparam = null;
                DashboardViewItem dvInternalStandards = null;
                DashboardViewItem dvSurrogates = null;
                DashboardViewItem dvParameterDefault = null;
                DashboardViewItem dvtestprice = null;
                DashboardViewItem dvComponents = null;

                if (View is DetailView)
                {
                    dvSampleparam = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                    dvQCSampleparam = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                    dvInternalStandards = ((DetailView)View).FindItem("InternalStandards") as DashboardViewItem;
                    dvSurrogates = ((DetailView)View).FindItem("dvSurrogates") as DashboardViewItem;
                    dvParameterDefault = ((DetailView)View).FindItem("QCParameterDefault") as DashboardViewItem;
                    dvtestprice = ((DetailView)View).FindItem("DVTestPrice") as DashboardViewItem;
                    dvComponents = ((DetailView)View).FindItem("Components") as DashboardViewItem;
                }
                if (dvSampleparam != null && dvSampleparam.InnerView != null && ((ListView)dvSampleparam.InnerView).CollectionSource.List.Count == 0 && testInfo.isgroup == false)
                {
                    Application.ShowViewStrategy.ShowMessage("Select at least one Sample Parameter.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }

                if (dvParameterDefault != null && dvParameterDefault.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvParameterDefault.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvParameterDefault.InnerView).Editor).Grid.UpdateEdit();
                    }
                   ((ListView)dvParameterDefault.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvParameterDefault.InnerView).ObjectSpace.Refresh();
                }
                if (dvComponents != null && dvComponents.InnerView != null)
                {
                    dvComponents.InnerView.ObjectSpace.CommitChanges();
                    dvComponents.InnerView.ObjectSpace.Refresh();
                }
                if (dvSampleparam != null && dvSampleparam.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvSampleparam.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvSampleparam.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (testInfo.lstRemovedSurrogates != null && testInfo.lstRemovedSurrogates.Count > 0)
                    {
                        foreach (Testparameter objTestParam in testInfo.lstRemovedSurrogates.ToList())
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objTestParam.Oid + "'");
                            SampleParameter objTP = ((ListView)dvSampleparam.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                ((ListView)dvSampleparam.InnerView).ObjectSpace.Delete(objTestParam);
                            }
                        }
                    }
                    ((ListView)dvSampleparam.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvSampleparam.InnerView).ObjectSpace.Refresh();
                    if (testInfo.lstSampleParameters != null)
                    {
                        testInfo.lstSampleParameters.Clear();
                    }
                    if (testInfo.lstRemovedSampleParameters != null)
                    {
                        testInfo.lstRemovedSampleParameters.Clear();
                    }
                }
                if (dvQCSampleparam != null && dvQCSampleparam.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvQCSampleparam.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvQCSampleparam.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (testInfo.lstRemovedQcParameters != null && testInfo.lstRemovedQcParameters.Count > 0)
                    {
                        foreach (Testparameter objTestParam in testInfo.lstRemovedQcParameters.ToList())
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objTestParam.Oid + "'");
                            SampleParameter objTP = ((ListView)dvQCSampleparam.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                ((ListView)dvQCSampleparam.InnerView).ObjectSpace.Delete(objTestParam);
                            }
                        }
                    }
                    ((ListView)dvQCSampleparam.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvQCSampleparam.InnerView).ObjectSpace.Refresh();
                    if (testInfo.lstQcParameters != null)
                    {
                        testInfo.lstQcParameters.Clear();
                    }
                    if (testInfo.lstRemovedQcParameters != null)
                    {
                        testInfo.lstRemovedQcParameters.Clear();
                    }
                }
                if (dvInternalStandards != null && dvInternalStandards.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvInternalStandards.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvInternalStandards.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (testInfo.lstRemovedInternalStandard != null && testInfo.lstRemovedInternalStandard.Count > 0)
                    {
                        foreach (Testparameter objTestParam in testInfo.lstRemovedInternalStandard.ToList())
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objTestParam.Oid + "'");
                            SampleParameter objTP = ((ListView)dvInternalStandards.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                ((ListView)dvInternalStandards.InnerView).ObjectSpace.Delete(objTestParam);
                            }
                        }
                    }
                    ((ListView)dvInternalStandards.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvInternalStandards.InnerView).ObjectSpace.Refresh();
                    if (testInfo.lstInternalStandard != null)
                    {
                        testInfo.lstInternalStandard.Clear();
                    }
                    if (testInfo.lstRemovedInternalStandard != null)
                    {
                        testInfo.lstRemovedInternalStandard.Clear();
                    }
                }
                if (dvSurrogates != null && dvSurrogates.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvSurrogates.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvSurrogates.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (testInfo.lstRemovedSurrogates != null && testInfo.lstRemovedSurrogates.Count > 0)
                    {
                        foreach (Testparameter objTestParam in testInfo.lstRemovedSurrogates.ToList())
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objTestParam.Oid + "'");
                            SampleParameter objTP = ((ListView)dvSurrogates.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                ((ListView)dvSurrogates.InnerView).ObjectSpace.Delete(objTestParam);
                            }
                        }
                    }
                    ((ListView)dvSurrogates.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvSurrogates.InnerView).ObjectSpace.Refresh();
                    if (testInfo.lstSurrogates != null)
                    {
                        testInfo.lstSurrogates.Clear();
                    }
                    if (testInfo.lstRemovedSurrogates != null)
                    {
                        testInfo.lstRemovedSurrogates.Clear();
                    }
                }
                if (dvtestprice != null && dvtestprice.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvtestprice.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvtestprice.InnerView).Editor).Grid.UpdateEdit();
                    }
                    //if (testpriceinfo.lstRemovedTestPrice != null && testpriceinfo.lstRemovedTestPrice.Count > 0)
                    //{
                    //    foreach (TestPrice objTestPrice in testpriceinfo.lstRemovedTestPrice.ToList())
                    //    {
                    //        //CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]='" + objTestPrice.Oid + "'");
                    //        //SampleParameter objTP = ((ListView)dvSampleparam.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                    //        //if (objTP == null)
                    //        {
                    //            ((ListView)dvtestprice.InnerView).ObjectSpace.Delete(objTestPrice);
                    //        }
                    //    }
                    //}
                    ((ListView)dvtestprice.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvtestprice.InnerView).ObjectSpace.Refresh();
                    //if (testpriceinfo.lsttestprice != null)
                    //{
                    //    testpriceinfo.lsttestprice.Clear();
                    //}
                    //if (testpriceinfo.lstRemovedTestPrice != null)
                    //{
                    //    testpriceinfo.lstRemovedTestPrice.Clear();
                    //}
                }
                testInfo.IsNewTest = false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Application_DetailViewCreating(object sender, DetailViewCreatingEventArgs e)
        {
            try
            {
                if (e.ViewID == "TestMethod_DetailView_Copy" && Application != null)
                {
                    Application.DetailViewCreating -= Application_DetailViewCreating;
                    IObjectSpace os = Application.CreateObjectSpace();
                    //TestMethod testMethod = os.CreateObject<TestMethod>();
                    //e.View = Application.CreateDetailView(os, testMethod);
                    //e.View.ViewEditMode = ViewEditMode.Edit;
                    TestMethod testMethod = os.CreateObject<TestMethod>();
                    DetailView dv = Application.CreateDetailView(os, "TestMethod_DetailView_Copy", true, testMethod);
                    // DetailView dv = Application.CreateDetailView(os, "TestMethod_DetailView_Copy", true, os.CreateObject(View.ObjectTypeInfo.Type));
                    //dv.ViewEditMode = ViewEditMode.Edit;
                    e.View = dv;
                    e.View.ViewEditMode = ViewEditMode.Edit;
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
                bool IsCreateNew = true;
                if (View != null && (View.Id == "TestMethod_ListView" || View.Id == "TestMethod_DetailView"))
                {
                    if (View.Id == "TestMethod_DetailView" && View.CurrentObject != null)
                    {
                        string strqctype = "Sample";
                        TestMethod objtm = (TestMethod)View.CurrentObject;
                        if (objtm != null && objtm.IsGroup == false)
                        {
                            DashboardViewItem dvSampleparam = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                            if (dvSampleparam != null && dvSampleparam.InnerView == null)
                            {
                                dvSampleparam.CreateControl();
                                dvSampleparam.InnerView.CreateControls();
                            }
                            if (dvSampleparam != null && dvSampleparam.InnerView != null && ((ListView)dvSampleparam.InnerView).CollectionSource.List.Count == 0)
                            {
                                e.Cancel = true;
                                IsCreateNew = false;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "AddSampleParameter"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else if (dvSampleparam != null && dvSampleparam.InnerView != null && ((ListView)dvSampleparam.InnerView).CollectionSource.List.Count > 0)
                            {
                                ((ListView)dvSampleparam.InnerView).ObjectSpace.CommitChanges();
                            }
                        }
                        else if (objtm != null && objtm.IsGroup == true)
                        {
                            DashboardViewItem dvtestgroup = ((DetailView)View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                            if (dvtestgroup != null && dvtestgroup.InnerView == null)
                            {
                                dvtestgroup.CreateControl();
                                dvtestgroup.InnerView.CreateControls();
                            }
                            if (dvtestgroup != null && dvtestgroup.InnerView != null && ((ListView)dvtestgroup.InnerView).CollectionSource.List.Count == 0)
                            {
                                e.Cancel = true;
                                IsCreateNew = false;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "AddTestMethod"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else if (dvtestgroup != null && dvtestgroup.InnerView != null && ((ListView)dvtestgroup.InnerView).CollectionSource.List.Count > 0)
                            {
                                ((ListView)dvtestgroup.InnerView).ObjectSpace.CommitChanges();
                            }
                        }
                    }
                    if (IsCreateNew == true)
                    {
                        e.Cancel = true;
                        if (Frame.GetType() != typeof(DevExpress.ExpressApp.Web.PopupWindow))
                        {
                            var lstContext = Application.MainWindow.Context;
                            //Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "TestMethod", true));
                            ShowViewParameters showViewParameters = new ShowViewParameters(Application.CreateDashboardView(Application.CreateObjectSpace(), "TestMethod", true));
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            //dc.Accepting += SaveTest_Execute;
                            ////dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                }
                else if (View.Id == "TestMethod_PrepMethods_ListView")
                {
                    if (((ListView)View).CollectionSource.GetCount() == 2)
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Already added  two preptypes", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveTest_Execute(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.Id != null && View.Id == "TestMethod")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Session currentSession = ((XPObjectSpace)os).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);

                    DashboardViewItem dviTestMethod = (DashboardViewItem)((DashboardView)View).FindItem("TestMethodDetail");
                    if (dviTestMethod != null && dviTestMethod.InnerView != null && dviTestMethod.InnerView is DetailView)
                    {
                        TestMethod obj = (TestMethod)dviTestMethod.InnerView.CurrentObject;
                        DetailView dv = (DetailView)dviTestMethod.InnerView;
                        if (obj != null && dv != null)
                        {
                            bool CanSave = false;
                            bool groupfieldtest = false;
                            DashboardViewItem dviSettings = (DashboardViewItem)dv.FindItem("CopyTest");
                            if (obj.IsGroup)
                            {
                                if (!string.IsNullOrEmpty(obj.TestName) && obj.MatrixName != null)
                                {
                                    CanSave = true;
                                    groupfieldtest = true;
                                }
                                else
                                {
                                    string msg = string.Empty;
                                    if (string.IsNullOrEmpty(obj.TestName))
                                    {
                                        msg = "TestName ";
                                    }
                                    if (obj.MatrixName == null)
                                    {
                                        if (msg.Length == 0)
                                        {
                                            msg = "MatrixName ";
                                        }
                                        else
                                        {
                                            msg = msg + ",MatrixName ";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(msg))
                                    {
                                        Application.ShowViewStrategy.ShowMessage(msg + "Should not be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        e.Cancel = true;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (!obj.CanCopyTest && !string.IsNullOrEmpty(obj.TestName) && obj.MatrixName != null && obj.MethodName != null)
                                {
                                    CanSave = true;
                                }
                                else if (obj.CanCopyTest && !string.IsNullOrEmpty(obj.TestName) && obj.MatrixName != null && obj.MethodName != null && dviSettings != null && dviSettings.InnerView != null && dviSettings.InnerView.SelectedObjects.Count > 0)
                                {
                                    CanSave = true;
                                }
                            }
                            if (CanSave && obj != null)
                            {
                                if (groupfieldtest && uow.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] = ? and [MatrixName.Oid] = ? And [IsGroup] = True", obj.TestName, obj.MatrixName.Oid)) == null)
                                {
                                    TestMethod objNewTest = new TestMethod(uow);
                                    objNewTest.TestName = obj.TestName;
                                    objNewTest.MatrixName = uow.GetObjectByKey<Matrix>(obj.MatrixName.Oid);
                                    objNewTest.Save();
                                    objNewTest.IsGroup = obj.IsGroup;
                                    testInfo.isTestsave = true;
                                    uow.CommitChanges();
                                    if (obj.CanCopyTest)
                                    {
                                        string strCopyMatrixName = obj.Matrix.MatrixName;
                                        string strCopyTestName = obj.Test.TestName;
                                        TestMethod objCopySourceTest = uow.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [IsGroup] = True", strCopyMatrixName, strCopyTestName));
                                        if (objCopySourceTest != null)
                                        {
                                            CopyFromOneTestgroupToAnother(objNewTest, objCopySourceTest, uow);
                                        }
                                    }
                                    objNewTest = os.GetObjectByKey<TestMethod>(objNewTest.Oid);
                                    DetailView detailView = Application.CreateDetailView(os, "TestMethod_DetailView", true, objNewTest);
                                    detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                                    Application.MainWindow.SetView(detailView);
                                }
                                else
                                {
                                    if (!groupfieldtest && uow.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] = ? and [MethodName.Oid] = ? and [MatrixName.Oid] = ?", obj.TestName, obj.MethodName.Oid, obj.MatrixName.Oid)) == null)
                                    {
                                        TestMethod objNewTest = new TestMethod(uow);
                                        objNewTest.TestName = obj.TestName;
                                        objNewTest.MethodName = uow.GetObjectByKey<Method>(obj.MethodName.Oid);
                                        objNewTest.MatrixName = uow.GetObjectByKey<Matrix>(obj.MatrixName.Oid);
                                        objNewTest.Category = obj.Category;
                                        objNewTest.Comment = obj.Comment;
                                        testInfo.isTestsave = true;

                                        objNewTest.Save();
                                        uow.CommitChanges();
                                        if (obj.CanCopyTest)
                                        {
                                            string strCopyMatrixName = obj.Matrix.MatrixName;
                                            string strCopyTestName = obj.Test.TestName;
                                            string strCopyMethodName = obj.Method.MethodName.MethodName;
                                            TestMethod objCopySourceTest = uow.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [MethodName.MethodName] = ?", strCopyMatrixName, strCopyTestName, strCopyMethodName));
                                            if (objCopySourceTest != null)
                                            {
                                                CopyFromOneTestToAnother(objNewTest, objCopySourceTest, uow, dviSettings.InnerView.SelectedObjects);
                                            }
                                        }
                                        objNewTest = os.GetObjectByKey<TestMethod>(objNewTest.Oid);
                                        DetailView detailView = Application.CreateDetailView(os, "TestMethod_DetailView", true, objNewTest);
                                        detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                                        Application.MainWindow.SetView(detailView);
                                    }
                                    else
                                    {
                                        e.Cancel = true;
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "testexists"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                            }
                            else
                            {
                                string msg = string.Empty;
                                if (string.IsNullOrEmpty(obj.TestName))
                                {
                                    msg = "TestName ";
                                }
                                if (obj.MatrixName == null)
                                {
                                    if (msg.Length == 0)
                                    {
                                        msg = "MatrixName ";
                                    }
                                    else
                                    {
                                        msg = msg + ",MatrixName ";
                                    }
                                }

                                if (obj.MethodName == null)
                                {
                                    if (msg.Length == 0)
                                    {
                                        msg = "Method ";
                                    }
                                    else
                                    {
                                        msg = msg + ",Method ";
                                    }
                                }
                                if (obj.CanCopyTest)
                                {
                                    if (obj.Test == null)
                                    {
                                        if (msg.Length == 0)
                                        {
                                            msg = "Copy From Test ";
                                        }
                                        else
                                        {
                                            msg = msg + ",MatrixName ";
                                        }
                                    }
                                    if (obj.MatrixName == null)
                                    {
                                        if (msg.Length == 0)
                                        {
                                            msg = "Copy From Matrix";
                                        }
                                        else
                                        {
                                            msg = msg + ",MatrixName ";
                                        }
                                    }
                                    if (obj.MethodName == null)
                                    {
                                        if (msg.Length == 0)
                                        {
                                            msg = "Copy From Method ";
                                        }
                                        else
                                        {
                                            msg = msg + ",Method ";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(msg))
                                    {
                                        Application.ShowViewStrategy.ShowMessage(msg + "Should not be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        e.Cancel = true;
                                        return;
                                    }
                                    else if (dviSettings != null && dviSettings.InnerView != null && dviSettings.InnerView.SelectedObjects.Count == 0)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        e.Cancel = true;
                                        return;
                                    }
                                    else if (dviSettings == null || dviSettings.InnerView == null)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Copy test process terminated. please try again after some time.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        e.Cancel = true;
                                        return;
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(msg + "Should not be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    e.Cancel = true;
                                    return;
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

        private void CopyFromOneTestgroupToAnother(TestMethod objNewTest, TestMethod objCopySourceTest, UnitOfWork uow)
        {
            try
            {
                IList<GroupTestMethod> lstgtm = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objCopySourceTest.Oid));
                if (lstgtm != null && lstgtm.Count > 0)
                {
                    foreach (GroupTestMethod objgtm in lstgtm.ToList())
                    {
                        GroupTestMethod objcrtgtm = ObjectSpace.CreateObject<GroupTestMethod>();
                        objcrtgtm.TestParameter = objgtm.TestParameter;
                        objcrtgtm.TestMethod = ObjectSpace.GetObject(objNewTest);
                        objcrtgtm.Tests = objgtm.Tests;
                        ObjectSpace.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_DetailView_Copy" && View.CurrentObject == e.Object && e.PropertyName == "IsGroup")
                {
                    TestMethod currentTest = e.Object as TestMethod;
                    if (currentTest != null)
                    {
                        if (currentTest.IsGroup == true)
                        {
                            if (View is DetailView)
                            {
                                foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "CanCopyTest"))
                                {
                                    ASPxBooleanPropertyEditor editor = (ASPxBooleanPropertyEditor)item;
                                    if (editor != null && editor.Editor != null)
                                    {
                                        editor.Caption = "Copy Test";
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "CanCopyTest"))
                            {
                                ASPxBooleanPropertyEditor editor = (ASPxBooleanPropertyEditor)item;
                                if (editor != null && editor.Editor != null)
                                {
                                    editor.Caption = "Copy Group Test";
                                }
                            }
                        }
                    }
                }
                if (View.Id == "Method_ListView_Combo")
                {
                    TestMethod testMethod = e.Object as TestMethod;
                    if (testMethod != null)
                    {
                        if (testMethod.Matrix == null || testMethod.Test == null)
                        {
                            testMethod.Test = null;
                            testMethod.Method = null;
                        }
                    }
                }

                //if (View.Id == "Testparameter_ListView_Test_SampleParameter" || View.Id == "Testparameter_ListView_Test_QCSampleParameter" || View.Id == "Testparameter_ListView_Test_InternalStandards" || View.Id == "Testparameter_ListView_Test_Surrogates")
                //{
                //    if (View.SelectedObjects.Count > 0)
                //    {
                //        IObjectSpace objSpace = Application.CreateObjectSpace();
                //        TestMethod objcruttm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //        foreach (Testparameter obj in View.SelectedObjects.Cast<Testparameter>().ToList())
                //        {
                //            Frame.GetController<AuditlogViewController>().createdeleteaudit(sender, objcruttm.Oid, objcruttm.TestCode, "QCSampleParameter", obj.ToString());

                //        }
                //    }
                //}


                  

                   
                //    if (View.Id == "Testparameter_ListView_Test_QCSampleParameter")
                //    {
                        

                //    }
                //    if (View.Id == "Testparameter_ListView_Test_InternalStandards")
                //    {
                        
                //            Frame.GetController<AuditlogViewController>().createdeleteaudit(sender, objcruttm.Oid, objcruttm.TestCode, "InternalStandards", obj.ToString());

                //    }
                //    if (View.Id == "Testparameter_ListView_Test_Surrogates")
                //    {
                //            Frame.GetController<AuditlogViewController>().createdeleteaudit(sender, objcruttm.Oid, objcruttm.TestCode, "Surrogates", obj.ToString());

                //    }
                //}
                //if (View != null && View.Id == "TestPrice_DetailView")
                //{
                //    if (View.CurrentObject == e.Object && e.PropertyName == "FastestTAT")
                //    {
                //        TestPrice objprice = (TestPrice)e.Object;
                //        if (objprice.RegularTAT != null && objprice.FastestTAT != null)
                //        {
                //            int temptime = 0;
                //            int fastestTAT = 0;
                //            int regularTAT = 0;
                //            int numericValue;
                //            bool isNumber;
                //            string strregularTAT = string.Empty;
                //            string strfastestTAT = string.Empty;
                //            if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("DAYS"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("DAYS", "");
                //                temptime = 24;
                //            }
                //            else if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("DAY"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("DAY", "");
                //                temptime = 24;
                //            }
                //            if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("WEEKS"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("WEEKS", "");
                //                temptime = 24 * 7;
                //            }
                //            else if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("WEEK"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("WEEK", "");
                //                temptime = 24 * 7;
                //            }
                //            if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("MONTHS"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("MONTHS", "");
                //                temptime = 24 * 30;
                //            }
                //            else if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("MONTH"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("MONTH", "");
                //                temptime = 24 * 30;
                //            }
                //            if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("YEARS"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("YEARS", "");
                //                temptime = 24 * 365;
                //            }
                //            else if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("YEAR"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("YEAR", "");
                //                temptime = 24 * 365;
                //            }
                //            strregularTAT = strregularTAT.Trim();
                //            isNumber = int.TryParse(strregularTAT, out numericValue);
                //            if (isNumber == true)
                //            {
                //                regularTAT = Convert.ToInt32(strregularTAT);
                //                regularTAT = regularTAT * temptime;
                //            }

                //            //FastestTAT
                //            if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("DAYS"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("DAYS", "");
                //                temptime = 24;
                //            }
                //            else if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("DAY"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("DAY", "");
                //                temptime = 24;
                //            }
                //            if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("WEEKS"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("WEEKS", "");
                //                temptime = 24 * 7;
                //            }
                //            else if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("WEEK"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("WEEK", "");
                //                temptime = 24 * 7;
                //            }
                //            if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("MONTHS"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("MONTHS", "");
                //                temptime = 24 * 30;
                //            }
                //            else if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("MONTH"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("MONTH", "");
                //                temptime = 24 * 30;
                //            }
                //            if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("YEARS"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("YEARS", "");
                //                temptime = 24 * 365;
                //            }
                //            else if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("YEAR"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("YEAR", "");
                //                temptime = 24 * 365;
                //            }
                //            strfastestTAT = strfastestTAT.Trim();
                //            isNumber = int.TryParse(strfastestTAT, out numericValue);
                //            if (isNumber == true)
                //            {
                //                fastestTAT = Convert.ToInt32(strfastestTAT);
                //                fastestTAT = fastestTAT * temptime;
                //            }


                //            if (fastestTAT > regularTAT)
                //            {
                //                objprice.FastestTAT = null;
                //                View.Refresh();
                //                Application.ShowViewStrategy.ShowMessage("Regular TAT should be greather than Fastest TAT", InformationType.Error, 3000, InformationPosition.Top);
                //            }
                //            //else
                //            //if (regularTAT == fastestTAT)
                //            //{
                //            //    objprice.FastestTAT.TAT = null;
                //            //    Application.ShowViewStrategy.ShowMessage("Select a different TAT, its already selected", InformationType.Error, 3000, InformationPosition.Top);
                //            //    return;
                //            //}
                //            tatcombofill();
                //        }
                //    }
                //    if (View.CurrentObject == e.Object && e.PropertyName == "RegularTAT")
                //    {
                //        TestPrice objprice = (TestPrice)e.Object;

                //        if (objprice.RegularTAT != null && objprice.FastestTAT != null)
                //        {
                //            int temptime = 0;
                //            int fastestTAT = 0;
                //            int regularTAT = 0;
                //            int numericValue;
                //            bool isNumber;
                //            string strregularTAT = string.Empty;
                //            string strfastestTAT = string.Empty;
                //            if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("DAYS"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("DAYS", "");
                //                temptime = 24;
                //            }
                //            else if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("DAY"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("DAY", "");
                //                temptime = 24;
                //            }
                //            if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("WEEKS"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("WEEKS", "");
                //                temptime = 24 * 7;
                //            }
                //            else if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("WEEK"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("WEEK", "");
                //                temptime = 24 * 7;
                //            }
                //            if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("MONTHS"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("MONTHS", "");
                //                temptime = 24 * 30;
                //            }
                //            else if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("MONTH"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("MONTH", "");
                //                temptime = 24 * 30;
                //            }
                //            if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("YEARS"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("YEARS", "");
                //                temptime = 24 * 365;
                //            }
                //            else if (objprice.RegularTAT.TAT.ToString().ToUpper().Contains("YEAR"))
                //            {
                //                strregularTAT = objprice.RegularTAT.TAT.ToString().ToUpper().Replace("YEAR", "");
                //                temptime = 24 * 365;
                //            }
                //            strregularTAT = strregularTAT.Trim();
                //            isNumber = int.TryParse(strregularTAT, out numericValue);
                //            if (isNumber == true)
                //            {
                //                regularTAT = Convert.ToInt32(strregularTAT);
                //                regularTAT = regularTAT * temptime;
                //            }

                //            //FastestTAT
                //            if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("DAYS"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("DAYS", "");
                //                temptime = 24;
                //            }
                //            else if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("DAY"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("DAY", "");
                //                temptime = 24;
                //            }
                //            if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("WEEKS"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("WEEKS", "");
                //                temptime = 24 * 7;
                //            }
                //            else if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("WEEK"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("WEEK", "");
                //                temptime = 24 * 7;
                //            }
                //            if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("MONTHS"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("MONTHS", "");
                //                temptime = 24 * 30;
                //            }
                //            else if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("MONTH"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("MONTH", "");
                //                temptime = 24 * 30;
                //            }
                //            if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("YEARS"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("YEARS", "");
                //                temptime = 24 * 365;
                //            }
                //            else if (objprice.FastestTAT.TAT.ToString().ToUpper().Contains("YEAR"))
                //            {
                //                strfastestTAT = objprice.FastestTAT.TAT.ToString().ToUpper().Replace("YEAR", "");
                //                temptime = 24 * 365;
                //            }
                //            strfastestTAT = strfastestTAT.Trim();
                //            isNumber = int.TryParse(strfastestTAT, out numericValue);
                //            if (isNumber == true)
                //            {
                //                fastestTAT = Convert.ToInt32(strfastestTAT);
                //                fastestTAT = fastestTAT * temptime;
                //            }


                //            if (fastestTAT > regularTAT)
                //            {
                //                objprice.RegularTAT.TAT = null;
                //                Application.ShowViewStrategy.ShowMessage("Regular TAT should be greather than Fastest TAT", InformationType.Error, 3000, InformationPosition.Top);
                //            }
                //            tatcombofill();
                //        }
                //    }
                //    if (View.CurrentObject == e.Object && e.PropertyName == "Component")
                //    {
                //        TestPrice objprice = (TestPrice)e.Object;
                //        DashboardViewItem dvpertest = ((DetailView)View).FindItem("PerTest") as DashboardViewItem;
                //        DashboardViewItem dvperparameter = ((DetailView)View).FindItem("PerParameter") as DashboardViewItem;
                //        if (objprice.Component != null && objprice.Component.Components != null)
                //        {
                //            objtestpriceinfo.TestPriceCurrentObject = objprice;
                //            if (dvpertest != null && dvpertest.InnerView != null)
                //            {
                //                //dvpertest.ControlCreated += dvpertest_ControlCreated;
                //                ((ListView)dvpertest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestPrice.Component.Components]= ?", objtestpriceinfo.TestPriceCurrentObject.Component.Components);

                //            }
                //            if (dvperparameter != null && dvperparameter.InnerView != null)
                //            {
                //                //dvperparameter.ControlCreated += dvperparameter_ControlCreated;
                //                ((ListView)dvperparameter.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestPrice.Component.Components] = ?", objtestpriceinfo.TestPriceCurrentObject.Component.Components);

                //            }
                //        }
                //        else
                //        {
                //            if (dvpertest != null && dvpertest.InnerView != null)
                //            {
                //                //dvpertest.ControlCreated += dvpertest_ControlCreated;
                //                ((ListView)dvpertest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("TestPrice.Component.Components Is Null");

                //            }
                //            if (dvperparameter != null && dvperparameter.InnerView != null)
                //            {
                //                //dvperparameter.ControlCreated += dvperparameter_ControlCreated;
                //                ((ListView)dvperparameter.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("TestPrice.Component.Components Is Null");

                //            }
                //        }
                //    }
                //    if (View.CurrentObject == e.Object && e.PropertyName == "PriceType")
                //    {
                //        TestPrice objtm = (TestPrice)View.CurrentObject;
                //        testpriceinfo.testpricetypeinfo = objtm.PriceType.ToString();
                //        chkpricetype = true;
                //        foreach (ViewItem item in ((DetailView)View).Items)
                //        {
                //            if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                //            {
                //                ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                //                if (propertyEditor != null && propertyEditor.Editor != null)
                //                {
                //                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                //                    selparameter.CallbackManager.RegisterHandler("pricetype", this);
                //                    //                                    propertyEditor.Editor.ClientSideEvents.SelectedIndexChanged = @"function(s,e) {var tab = myPageControl.GetTab(1);
                //                    //alert(tab);
                //                    //}";   
                //                }
                //            }
                //        }
                //    }
                //}
                //if (View != null && (View.Id == "TestPriceDetail_ListView_Copy_perparameter" || View.Id == "TestPriceDetail_ListView_Copy_pertest"))
                //{
                //    TestPriceDetail objprice = (TestPriceDetail)e.Object;
                //    if (e.PropertyName == "TAT")
                //    {

                //        if (objprice.TAT != null)
                //        {
                //            var lstchkTAT = ((ListView)View).CollectionSource.List.Cast<TestPriceDetail>().Where(i => i.TAT == objprice.TAT).ToList();
                //            if (lstchkTAT != null && lstchkTAT.Count > 1)
                //            {
                //                objprice.TAT = null;
                //                Application.ShowViewStrategy.ShowMessage("TAT already selected", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //                return;
                //            }
                //        }
                //    }
                //    //if(e.PropertyName =="Price" || e.PropertyName== "Surcharge")
                //    //{
                //    //    if (objprice.Price > 0 && objprice.Surcharge > 0)
                //    //    {
                //    //        double intPrice;
                //    //        intPrice = (objprice.Price * (Convert.ToDouble(objprice.Surcharge) / 100)) + objprice.Price;
                //    //        objprice.Price = Convert.ToInt32(intPrice);
                //    //    }
                //    //}
                //}
                if (View != null && View.Id == "TestGuide_DetailView")
                {
                    if (View.CurrentObject == e.Object && e.PropertyName == "SetPrepTimeAsAnalysisTime" && ObjectSpace.IsModified && e.OldValue != e.NewValue)
                    {
                        TestGuide objGuide = (TestGuide)e.Object;
                        if (objGuide.SetPrepTimeAsAnalysisTime == true)
                        {
                            objGuide.HoldingTimeBeforeAnalysis = objGuide.HoldingTimeBeforePrep;
                        }
                        else if (objGuide.SetPrepTimeAsAnalysisTime == false)
                        {
                            objGuide.HoldingTimeBeforeAnalysis = null;
                            View.Refresh();
                        }
                    }
                }
                if (View != null && View.Id == "PrepMethod_DetailView")
                {
                    if (View.CurrentObject == e.Object && e.PropertyName == "PrepType" && ObjectSpace.IsModified && e.OldValue != e.NewValue)
                    {
                        PrepMethod objPrep = (PrepMethod)e.Object;
                        if (objPrep != null && objPrep.PrepType != null && objPrep.Tier != 0)
                        {
                            ListPropertyEditor lvperpmethod = ((DetailView)Application.MainWindow.View).FindItem("PrepMethods") as ListPropertyEditor;
                            if (lvperpmethod != null && lvperpmethod.ListView != null)
                            {
                                if (((ListView)lvperpmethod.ListView).CollectionSource.List.Cast<PrepMethod>().FirstOrDefault(i => i.Tier == objPrep.Tier && i.Oid != objPrep.Oid) != null)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Tier " + objPrep.Tier + " already added", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    objPrep.PrepType = null;
                                }

                            }
                        }
                        PrepTypes objtestmethod = ObjectSpace.FindObject<PrepTypes>(CriteriaOperator.Parse("[Oid] = ?", objPrep.PrepType));
                        if (objtestmethod != null)
                        {
                            objPrep.Tier = objtestmethod.Tier;
                        }
                        else
                        {
                            objPrep.Tier = 0;
                            View.Refresh();
                        }
                    }
                    if (View.CurrentObject == e.Object && e.PropertyName == "Method")
                    {
                        PrepMethod objprepmed = (PrepMethod)e.Object;
                        if (objprepmed.Method != null && objprepmed != null && objprepmed.Method.MethodName != null)
                        {
                            ListPropertyEditor lvperpmethod = ((DetailView)Application.MainWindow.View).FindItem("PrepMethods") as ListPropertyEditor;
                            if (lvperpmethod != null && lvperpmethod.ListView != null)
                            {
                                foreach (PrepMethod objpremedthod in ((ListView)lvperpmethod.ListView).CollectionSource.List.Cast<PrepMethod>().Where(i => i.Method != null).ToList())
                                {
                                    if (objpremedthod.Method != null && objpremedthod.Method.MethodName != null)
                                    {
                                        if (objprepmed.Method.MethodName == objpremedthod.Method.MethodName)
                                        {
                                            objprepmed.Method = null;
                                            Application.ShowViewStrategy.ShowMessage("Selected method already inserted", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (View != null && View.Id == "TestMethod_DetailView")
                {
                    if (View != null && View.CurrentObject == e.Object && e.PropertyName == "IsGroup")
                    {
                        TestMethod objtm = (TestMethod)e.Object;
                        testInfo.isgroup = objtm.IsGroup;
                        if (objtm.IsGroup)
                        {
                            CopyParameters.Active.SetItemValue("Disable", false);
                        }
                        else
                        {
                            CopyParameters.Active.SetItemValue("Disable", true);
                        }
                    }
                    if (View != null && View.CurrentObject == e.Object && e.PropertyName == "QCtypesCombo")
                    {
                        TestMethod objtm = (TestMethod)e.Object;
                        testInfo.CurrentQcType = (QCType)e.NewValue;
                        if (objtm.QCtypesCombo != null && objtm.QCtypesCombo.QCTypeName != null)
                        {
                            DashboardViewItem lstqcparameter = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                            if (lstqcparameter != null && lstqcparameter.InnerView != null)
                            {
                                ((ListView)lstqcparameter.InnerView).CollectionSource.Criteria["QCFilter"] = CriteriaOperator.Parse("[QCType.QCTypeName] = ?", objtm.QCtypesCombo.QCTypeName);
                                ((ListView)lstqcparameter.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] In(" + string.Format("'{0}'", string.Join("','", objtm.lstQCTypes.Select(i => i.QCTypeName.ToString().Replace("'", "''")))) + ")");
                                if (testInfo.lstQcParameters != null && testInfo.lstQcParameters.Where(i => i.QCType != null && testInfo.CurrentQcType != null && i.QCType.Oid == testInfo.CurrentQcType.Oid).Count() > 0)
                                {
                                    List<Testparameter> lstCurQCTempTestParameters = testInfo.lstQcParameters.Where(i => i.QCType.Oid == testInfo.CurrentQcType.Oid).ToList();
                                    for (int index = 0; index < lstCurQCTempTestParameters.Count(); index++)
                                    {
                                        Testparameter objTestParam = lstCurQCTempTestParameters[index];
                                        if (objTestParam != null)
                                        {
                                            if (((ListView)lstqcparameter.InnerView).CollectionSource.List.Cast<Testparameter>().FirstOrDefault(i => i.QCType != null && i.QCType.Oid == objTestParam.QCType.Oid && i.Parameter.Oid == objTestParam.Parameter.Oid) == null)
                                            {
                                                ((ListView)lstqcparameter.InnerView).CollectionSource.Add(objTestParam);
                                            }
                                        }
                                    }
                                }
                                gridcount();
                            }
                        }
                        else
                        {
                            DashboardViewItem lstqcparameter = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                            if (lstqcparameter != null && lstqcparameter.InnerView != null)
                            {
                                ((ListView)lstqcparameter.InnerView).CollectionSource.Criteria["QCFilter"] = CriteriaOperator.Parse("oid is null");
                            }
                        }

                    }
                    //if (View != null && View.CurrentObject == e.Object && e.PropertyName == "IsGroup")
                    //{
                    //    TestMethod objtm = (TestMethod)e.Object;
                    //    if(objtm != null)
                    //    {
                    //        objtm
                    //    }
                    //}
                }
                if (View != null && View.Id == "TestMethod_DetailView_Copy")
                {
                    if (View.CurrentObject == e.Object && e.PropertyName == "Test" && ObjectSpace.IsModified && e.OldValue != e.NewValue)
                    {
                        TestMethod objTestMethod = (TestMethod)e.Object;
                        if (objTestMethod != null && objTestMethod.Matrix != null && objTestMethod.Test != null)
                        {
                            ASPxGridLookupPropertyEditor methodEditor = ((DetailView)View).FindItem("Method") as ASPxGridLookupPropertyEditor;
                            if (methodEditor != null)
                            {
                                methodEditor.CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName]=?", objTestMethod.Test.TestName, objTestMethod.Matrix.MatrixName);
                                methodEditor.CollectionSource.Reload();
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

        public void tatcombofill()
        {
            try
            {
                //if (View.Id == "TestPrice_DetailView")
                //{
                //    TestPrice objprice = (TestPrice)View.CurrentObject;
                //    int numericValue;
                //    bool isNumber;
                //    if (tatinfo.lsttat == null)
                //    {
                //        tatinfo.lsttat = new List<TurnAroundTime>();
                //    }
                //    else
                //    {
                //        tatinfo.lsttat.Clear();
                //    }
                //    TurnAroundTime objregulartp = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", objprice.RegularTAT.TAT));
                //    TurnAroundTime objfastesttp = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", objprice.FastestTAT.TAT));
                //    IList<TurnAroundTime> lstTAt = ObjectSpace.GetObjects<TurnAroundTime>(CriteriaOperator.Parse("[Sort] Between('" + objfastesttp.Sort + "','" + objregulartp.Sort + "')"));
                //    if (lstTAt != null && lstTAt.Count > 0)
                //    {
                //        foreach (TurnAroundTime objtat in lstTAt.ToList())
                //        {
                //            if (!tatinfo.lsttat.Contains(objtat))
                //            {
                //                tatinfo.lsttat.Add(objtat);
                //            }
                //        }
                //    }
                //    DashboardViewItem dvpertest = ((DetailView)View).FindItem("PerTest") as DashboardViewItem;
                //    DashboardViewItem dvperparameter = ((DetailView)View).FindItem("PerParameter") as DashboardViewItem;
                //    if (dvpertest != null && dvpertest.InnerView != null && dvperparameter != null && dvperparameter.InnerView != null)
                //    {
                //        ((ListView)dvpertest.InnerView).Refresh();
                //        ((ListView)dvperparameter.InnerView).Refresh();
                //        //((ListView)dvpertest.InnerView).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[Component.Oid] = ? And [TestMethod] = ?", objtp.Component.Oid, objtm.Oid);
                //        //((ListView)dvperparameter.InnerView).CollectionSource.Criteria["Filter2"] = CriteriaOperator.Parse("[Component.Oid] = ? And [TestMethod] = ?", objtp.Component.Oid, objtm.Oid);
                //    }
                //    View.Refresh();

                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void dvperparameter_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                //DashboardViewItem dvperparameter = ((DetailView)View).FindItem("PerParameter") as DashboardViewItem;
                //if (dvperparameter.InnerView != null)
                //{
                //    ((ListView)dvperparameter.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Component] = ?", objtestpriceinfo.TestPriceCurrentObject.Component);
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void dvpertest_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                //DashboardViewItem dvpertest = ((DetailView)View).FindItem("PerTest") as DashboardViewItem;
                //if (dvpertest.InnerView != null)
                //{
                //    ((ListView)dvpertest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Component] = ?", objtestpriceinfo.TestPriceCurrentObject.Component);
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void UnlinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (chkunlink is true)
                {
                    //ObjectSpace.CommitChanges();
                    DashboardViewItem dvqcpara = ((DetailView)Application.MainWindow.View).FindItem("QCSampleParameter") as DashboardViewItem;
                    ListPropertyEditor lstqctype = ((DetailView)Application.MainWindow.View).FindItem("QCTypes") as ListPropertyEditor;
                    if (lstqctype != null && lstqctype.ListView != null)
                    {
                        ListView lstdvqcpara = dvqcpara.InnerView as ListView;
                        ListView lvqctype = lstqctype.ListView;
                        if (lvqctype.CollectionSource.List.Count > 0)
                        {
                            List<Guid> lstqcguid = new List<Guid>();
                            foreach (Guid objqcclear in ((ListView)lvqctype).CollectionSource.List.Cast<QCType>().Select(i => new Guid(i.Oid.ToString())).ToList())
                            {
                                QCType objqc = ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[Oid] = ?", objqcclear));
                                lstqcguid.Add(objqc.Oid);
                            }

                            ((ListView)dvqcpara.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[QCType] In(" + string.Format("'{0}'", string.Join("','", lstqcguid.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        }
                        else
                        {
                            foreach (Guid objqcclear in ((ListView)dvqcpara.InnerView).CollectionSource.List.Cast<Testparameter>().Select(i => new Guid(i.Oid.ToString())).ToList())
                            {
                                Testparameter objqc = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", objqcclear));
                                ((ListView)dvqcpara.InnerView).CollectionSource.List.Remove(objqc);
                            }
                            dvqcpara.InnerView.Refresh();
                        }
                    }
                    chkunlink = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DashboardViewItem_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                ListPropertyEditor lstqctype = ((DetailView)View).FindItem("QCTypes") as ListPropertyEditor;
                DashboardViewItem dashvi = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                if (dashvi != null)
                {
                    ObjectSpace.Refresh();
                    ListView lstqc = lstqctype.ListView;
                    ListView lstdashvi = dashvi.InnerView as ListView;
                    List<Guid> lstqcguid = new List<Guid>();
                    if (lstqc.CollectionSource.List.Count > 0)
                    {
                        foreach (Guid objqcclear in ((ListView)lstqc).CollectionSource.List.Cast<QCType>().Select(i => new Guid(i.Oid.ToString())).ToList())
                        {
                            QCType objqc = ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[Oid] = ?", objqcclear));
                            lstqcguid.Add(objqc.Oid);
                        }
                        ((ListView)lstdashvi).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[QCType] In(" + string.Format("'{0}'", string.Join("','", lstqcguid.Select(i => i.ToString().Replace("'", "''")))) + ")");
                    }
                    else
                    {
                        lstdashvi.CollectionSource.List.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_DetailView")
                {
                    objAuditInfo.SaveData = true;
                    TestMethod objtm = (TestMethod)View.CurrentObject;
                    if (objtm != null && objtm.IsGroup == true)
                    {
                        DashboardViewItem dvtestmethod = ((DetailView)View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                        if (dvtestmethod != null && dvtestmethod.InnerView != null && ((ListView)dvtestmethod.InnerView).CollectionSource.List.Count == 0)
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Add at least one Testmethod.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        else if (dvtestmethod != null && dvtestmethod.InnerView != null)
                        {
                            ((ListView)dvtestmethod.InnerView).ObjectSpace.CommitChanges();
                        }
                    }
                    else if (objtm != null && objtm.IsGroup == false)
                    {
                        if (objtm.MatrixName != null && objtm.TestName != null && objtm.MethodName != null)
                        {
                            DashboardViewItem dvSampleParameter = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                            if (dvSampleParameter != null && dvSampleParameter.InnerView == null)
                            {
                                dvSampleParameter.CreateControl();
                                dvSampleParameter.InnerView.CreateControls();
                            }
                            if (dvSampleParameter != null && dvSampleParameter.InnerView != null)
                            {
                                if (((ListView)dvSampleParameter.InnerView).CollectionSource.GetCount() == 0)
                                {
                                    e.Cancel = true;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "AddSampleParameter"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                else if (((ListView)dvSampleParameter.InnerView).CollectionSource.GetCount() > 0)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace(typeof(GroupTestMethod));
                                    IList<GroupTestMethod> lstgtm = os.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid));
                                    if (lstgtm != null && lstgtm.Count > 0)
                                    {
                                        os.Delete(lstgtm);
                                        os.CommitChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            string msg = string.Empty;
                            if (string.IsNullOrEmpty(objtm.TestName))
                            {
                                msg = "TestName ";
                            }
                            if (objtm.MatrixName == null)
                            {
                                if (msg.Length == 0)
                                {
                                    msg = "Matrix ";
                                }
                                else
                                {
                                    msg = msg + ",Matrix ";
                                }
                            }
                            if (objtm.MethodName == null)
                            {
                                if (msg.Length == 0)
                                {
                                    msg = "Method ";
                                }
                                else
                                {
                                    msg = msg + ",Method ";
                                }
                            }
                            if (!string.IsNullOrEmpty(msg))
                            {
                                Application.ShowViewStrategy.ShowMessage(msg + "Should not be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                    if (!ObjectSpace.IsNewObject(objtm))
                    {
                        objAuditInfo.SaveData = null;
                    }
                }
                DashboardViewItem dvSampleparam = null;
                DashboardViewItem dvQCSampleparam = null;
                DashboardViewItem dvInternalStandards = null;
                DashboardViewItem dvSurrogates = null;
                DashboardViewItem dvParameterDefault = null;
                DashboardViewItem dvtestprice = null;
                DashboardViewItem dvComponents = null;

                if (View is DetailView)
                {
                    dvSampleparam = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                    dvQCSampleparam = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                    dvInternalStandards = ((DetailView)View).FindItem("InternalStandards") as DashboardViewItem;
                    dvSurrogates = ((DetailView)View).FindItem("dvSurrogates") as DashboardViewItem;
                    dvParameterDefault = ((DetailView)View).FindItem("QCParameterDefault") as DashboardViewItem;
                    dvtestprice = ((DetailView)View).FindItem("DVTestPrice") as DashboardViewItem;
                    dvComponents = ((DetailView)View).FindItem("Components") as DashboardViewItem;
                }
                if (dvSampleparam != null && dvSampleparam.InnerView != null && ((ListView)dvSampleparam.InnerView).CollectionSource.List.Count == 0 && testInfo.isgroup == false)
                {

                    Application.ShowViewStrategy.ShowMessage("Select at least one Sample Parameter.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }

                if (dvParameterDefault != null && dvParameterDefault.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvParameterDefault.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvParameterDefault.InnerView).Editor).Grid.UpdateEdit();
                    }
                   ((ListView)dvParameterDefault.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvParameterDefault.InnerView).ObjectSpace.Refresh();
                }
                if (dvComponents != null && dvComponents.InnerView != null)
                {
                    dvComponents.InnerView.ObjectSpace.CommitChanges();
                    dvComponents.InnerView.ObjectSpace.Refresh();
                }
                if (dvSampleparam != null && dvSampleparam.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvSampleparam.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvSampleparam.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (testInfo.lstRemovedSurrogates != null && testInfo.lstRemovedSurrogates.Count > 0)
                    {
                        foreach (Testparameter objTestParam in testInfo.lstRemovedSurrogates.ToList())
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objTestParam.Oid + "'");
                            SampleParameter objTP = ((ListView)dvSampleparam.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                ((ListView)dvSampleparam.InnerView).ObjectSpace.Delete(objTestParam);
                            }
                        }
                    }
                    ((ListView)dvSampleparam.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvSampleparam.InnerView).ObjectSpace.Refresh();
                    if (testInfo.lstSampleParameters != null)
                    {
                        testInfo.lstSampleParameters.Clear();
                    }
                    if (testInfo.lstRemovedSampleParameters != null)
                    {
                        testInfo.lstRemovedSampleParameters.Clear();
                    }
                }
                if (dvQCSampleparam != null && dvQCSampleparam.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvQCSampleparam.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvQCSampleparam.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (testInfo.lstRemovedQcParameters != null && testInfo.lstRemovedQcParameters.Count > 0)
                    {
                        foreach (Testparameter objTestParam in testInfo.lstRemovedQcParameters.ToList())
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objTestParam.Oid + "'");
                            SampleParameter objTP = ((ListView)dvQCSampleparam.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                ((ListView)dvQCSampleparam.InnerView).ObjectSpace.Delete(objTestParam);
                            }
                        }
                    }
                    ((ListView)dvQCSampleparam.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvQCSampleparam.InnerView).ObjectSpace.Refresh();
                    if (testInfo.lstQcParameters != null)
                    {
                        testInfo.lstQcParameters.Clear();
                    }
                    if (testInfo.lstRemovedQcParameters != null)
                    {
                        testInfo.lstRemovedQcParameters.Clear();
                    }
                }
                if (dvInternalStandards != null && dvInternalStandards.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvInternalStandards.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvInternalStandards.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (testInfo.lstRemovedInternalStandard != null && testInfo.lstRemovedInternalStandard.Count > 0)
                    {
                        foreach (Testparameter objTestParam in testInfo.lstRemovedInternalStandard.ToList())
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objTestParam.Oid + "'");
                            SampleParameter objTP = ((ListView)dvInternalStandards.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                ((ListView)dvInternalStandards.InnerView).ObjectSpace.Delete(objTestParam);
                            }
                        }
                    }
                    ((ListView)dvInternalStandards.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvInternalStandards.InnerView).ObjectSpace.Refresh();
                    if (testInfo.lstInternalStandard != null)
                    {
                        testInfo.lstInternalStandard.Clear();
                    }
                    if (testInfo.lstRemovedInternalStandard != null)
                    {
                        testInfo.lstRemovedInternalStandard.Clear();
                    }
                }
                if (dvSurrogates != null && dvSurrogates.InnerView != null)
                {
                    if (((ASPxGridListEditor)((ListView)dvSurrogates.InnerView).Editor).Grid != null)
                    {
                        ((ASPxGridListEditor)((ListView)dvSurrogates.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (testInfo.lstRemovedSurrogates != null && testInfo.lstRemovedSurrogates.Count > 0)
                    {
                        foreach (Testparameter objTestParam in testInfo.lstRemovedSurrogates.ToList())
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objTestParam.Oid + "'");
                            SampleParameter objTP = ((ListView)dvSurrogates.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                ((ListView)dvSurrogates.InnerView).ObjectSpace.Delete(objTestParam);
                            }
                        }
                    }
                    ((ListView)dvSurrogates.InnerView).ObjectSpace.CommitChanges();
                    ((ListView)dvSurrogates.InnerView).ObjectSpace.Refresh();
                    if (testInfo.lstSurrogates != null)
                    {
                        testInfo.lstSurrogates.Clear();
                    }
                    if (testInfo.lstRemovedSurrogates != null)
                    {
                        testInfo.lstRemovedSurrogates.Clear();
                    }
                }
                //if (dvtestprice != null && dvtestprice.InnerView != null)
                //{
                //    if (((ASPxGridListEditor)((ListView)dvtestprice.InnerView).Editor).Grid != null)
                //    {
                //        ((ASPxGridListEditor)((ListView)dvtestprice.InnerView).Editor).Grid.UpdateEdit();
                //    }
                //    if (testpriceinfo.lstRemovedTestPrice != null && testpriceinfo.lstRemovedTestPrice.Count > 0)
                //    {
                //        foreach (TestPrice objTestPrice in testpriceinfo.lstRemovedTestPrice.ToList())
                //        {
                //            //CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]='" + objTestPrice.Oid + "'");
                //            //SampleParameter objTP = ((ListView)dvSampleparam.InnerView).ObjectSpace.FindObject<SampleParameter>(criteria1);
                //            //if (objTP == null)
                //            {
                //                ((ListView)dvtestprice.InnerView).ObjectSpace.Delete(objTestPrice);
                //            }
                //        }
                //    }
                //    ((ListView)dvtestprice.InnerView).ObjectSpace.CommitChanges();
                //    ((ListView)dvtestprice.InnerView).ObjectSpace.Refresh();
                //    //if (testpriceinfo.lsttestprice != null)
                //    //{
                //    //    testpriceinfo.lsttestprice.Clear();
                //    //}
                //    //if (testpriceinfo.lstRemovedTestPrice != null)
                //    //{
                //    //    testpriceinfo.lstRemovedTestPrice.Clear();
                //    //}
                //}

                testInfo.IsNewTest = false;


            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ViAvailableFields_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem viAvailableFields = null;
                if (View is DetailView)
                {
                    viAvailableFields = ((DetailView)View).FindItem("CopyTest") as DashboardViewItem;
                }
                else
                if (View is DashboardView)
                {
                    viAvailableFields = ((DashboardView)View).FindItem("CopyTestSettings") as DashboardViewItem;
                }

                if (viAvailableFields != null && viAvailableFields.InnerView != null && viAvailableFields.InnerView is ListView) // && IsFieldsPopulated == false)
                {
                    ListView liAvailableFields = viAvailableFields.InnerView as ListView;
                    if (liAvailableFields != null)
                    {
                        List<string> lststring = new List<string>();
                        lststring.Add("Prep Method");
                        lststring.Add("Sampling Method");
                        lststring.Add("QC Type");
                        lststring.Add("Test Guide");
                        //lststring.Add("Test Price");
                        lststring.Add("Sample Parameters");
                        lststring.Add("Surrogates");
                        lststring.Add("Internal Standard");
                        lststring.Add("QC Sample Parameters");
                        //lststring.Add("Components");
                        if (liAvailableFields.CollectionSource.List.Count == 0)
                        {
                            foreach (string objstr in lststring.ToList())
                            {
                                TestCopy slField = liAvailableFields.ObjectSpace.CreateObject<TestCopy>();
                                slField.Title = objstr;
                                liAvailableFields.CollectionSource.Add(slField);
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
        private void CopyParameters_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (CopyParameters.SelectedItem != null && CopyParameters.SelectedItem.Id == "Copyqcparameterssametest")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    TestMethod objtm = os.CreateObject<TestMethod>();
                    DetailView createdv = Application.CreateDetailView(os, "TestMethod_DetailView_CopyParameter", false, objtm);
                    createdv.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showviewparameter = new ShowViewParameters(createdv);
                    showviewparameter.Context = TemplateContext.PopupWindow;
                    showviewparameter.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += dc_Accepting;
                    dc.AcceptAction.Executed += AcceptAction_Executed;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showviewparameter.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showviewparameter, new ShowViewSource(null, null));
                }
                else if (CopyParameters.SelectedItem != null)
                {
                    if (CopyParameters.SelectedItem.Id == "Copyparameters" || CopyParameters.SelectedItem.Id == "Copysurrogates"
                        || CopyParameters.SelectedItem.Id == "Copyinternalstandards" || CopyParameters.SelectedItem.Id == "Copyqcparameters")
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        Testparameter objtm = os.CreateObject<Testparameter>();
                        DetailView createdv = Application.CreateDetailView(os, "Testparameter_DetailView_Copyparameters", true, objtm);
                        createdv.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showviewparameter = new ShowViewParameters(createdv);
                        showviewparameter.Context = TemplateContext.PopupWindow;
                        showviewparameter.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += dc_Accepting;
                        dc.AcceptAction.Executed += AcceptAction_Executed;
                        dc.CloseOnCurrentObjectProcessing = false;
                        showviewparameter.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showviewparameter, new ShowViewSource(null, null));
                    }
                }
                if (CopyParameters.SelectedItem!=null)
                {
                    ChoiceActionItem itemToSelect = CopyParameters.Items.FindItemByID("None");
                    CopyParameters.SelectedItem = (itemToSelect != null) ? itemToSelect : null;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void gridcount()
        {

            try
            {
                if (View.Id == "Method_ListView_Copy_TestMethod" || View.Id == "Testparameter_ListView_Test_InternalStandards" || View.Id == "Testparameter_ListView_Test_SampleParameter"
                        || View.Id == "Component_ListView_Test" || View.Id == "Testparameter_ListView_Test_QCSampleParameter" || View.Id == "TestMethod_PrepMethods_ListView"
                        || View.Id == "TestMethod_SamplingMethods_ListView" || View.Id == "TestMethod_Preservatives_ListView" || View.Id == "TestMethod_surrogates_ListView"
                        || View.Id == "TestMethod_QCTypes_ListView")
                {
                    if (View.Id == "TestMethod_PrepMethods_ListView" || View.Id == "TestMethod_SamplingMethods_ListView" || View.Id == "TestMethod_Preservatives_ListView" || View.Id == "TestMethod_surrogates_ListView"
                        || View.Id == "TestMethod_QCTypes_ListView")
                    {
                        Frame.GetController<LinkUnlinkController>().LinkAction.Caption = "ADD";
                        Frame.GetController<LinkUnlinkController>().LinkAction.ImageName = "Add_16x16";
                        Frame.GetController<LinkUnlinkController>().UnlinkAction.Caption = "REMOVE";
                        Frame.GetController<LinkUnlinkController>().UnlinkAction.ImageName = "Remove";
                    }

                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null && ((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.GetCount() > 0)
                    {
                        //((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView(View.Id)).PageSize = ((ListView)View).CollectionSource.GetCount();
                        editor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        //var obj = (IModelListView)Application.FindModelView(View.Id);
                        //obj.IsFooterVisible = true;
                        ((ListView)View).Refresh();
                        View.Refresh();
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

                if (View.Id == "Testparameter_ListView_Test_SampleParameter")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("TestAccrediation", this);
                    Frame.GetController<ExportController>().ExportAction.Active["ShowExport"] = true;
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;

                    if (gridListEditor.Grid.Columns["DefaultResult"] != null)
                    {
                        int VisiblIndex = gridListEditor.Grid.Columns["DefaultResult"].VisibleIndex;
                        if (gridListEditor.Grid.Columns["TestDefaultResult"] != null)
                        {
                            gridListEditor.Grid.Columns["TestDefaultResult"].VisibleIndex = VisiblIndex;
                            gridListEditor.Grid.Columns["TestDefaultResult"].Width = 30;
                        }
                        if (gridListEditor.Grid.Columns["IsResultDefaultValue"] != null)
                        {
                            gridListEditor.Grid.Columns["IsResultDefaultValue"].Width = 0;
                        }
                        gridListEditor.Grid.HtmlCommandCellPrepared += Grid_HtmlCommandCellPrepared;


                    }
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared1;
                    gridListEditor.Grid.Load += Grid_Load1;

                }
                if (View.Id == "Accrediation_ListView_Copy")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Load += Grid_Load1;

                }
                if (Application.MainWindow.View is DetailView)
                {
                    if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                    {
                        ADDAction.Active["ShowAdd"] = true;
                        RemoveAction.Active["ShowRemove"] = true;
                    }
                    else
                    {
                        ADDAction.Active["ShowAdd"] = false;
                        RemoveAction.Active["ShowRemove"] = false;
                    }
                }

                //if(View is ListView)
                //{
                //    if(View != null && View.Id == "TestPrice_ListView")
                //    {
                //        IObjectSpace os = Application.CreateObjectSpace();
                //        TestMethod tm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //        IList<TestPrice> lstTestPrice = ((ListView)View).CollectionSource.List.Cast<TestPrice>().ToList().Where(x => x.TestMethod.Oid == tm.Oid).ToList();
                //        if (lstTestPrice != null && lstTestPrice.Count >0)
                //        {
                //            ADDAction.Active["ShowAdd"] = false;
                //        }
                //        else
                //        {
                //            ADDAction.Active["ShowAdd"] = true;
                //        }
                //    }
                //    else if(View.Id =="TestPriceDetail_ListView_Copy_pertest" || View.Id =="TestPriceDetail_ListView_Copy_perparameter" || View.Id== "Testparameter_ListView_Test_SampleParameter" || View.Id=="Testparameter_ListView_Test_InternalStandards" || View.Id =="Component_ListView_Test" || View.Id =="Testparameter_ListView_Test_QCSampleParameter;" + "Testparameter_ListView_Test_Surrogates" || View.Id == "Testparameter_ListView_Test_Component")
                //    {
                //        if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                //        {
                //            ADDAction.Active["ShowAdd"] = true;
                //            RemoveAction.Active["ShowRemove"] = true;
                //        }
                //        else
                //        {
                //            ADDAction.Active["ShowAdd"] = false;
                //            RemoveAction.Active["ShowRemove"] = false;
                //        }
                //    }
                //}

                //                if (View != null && View.Id == "TestPrice_DetailView")
                //                {
                //                    IObjectSpace os = Application.CreateObjectSpace();
                //                    TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //                    Component testscomponent = os.FindObject<Component>(CriteriaOperator.Parse("[Components]= 'Default'"));
                //                    if (testscomponent == null)
                //                    {
                //                        Component objcomp = os.CreateObject<Component>();
                //                        objcomp.Components = "Default";
                //                        os.CommitChanges();
                //                        os.Refresh();
                //                    }
                //                    if (View.CurrentObject != null)
                //                    {
                //                        TestPrice objtp = (TestPrice)View.CurrentObject;
                //                        //objtestpriceinfo.TestPriceCurrentObject.Component = objtp.Component;
                //                        //objtestpriceinfo.TestPriceCurrentObject.PriceType = objtp.PriceType;
                //                        objtestpriceinfo.testpricetypeinfo = objtp.PriceType.ToString();
                //                        if (objtp.Component == null)
                //                        {
                //                            Component objcmp = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components]= 'Default'"));
                //                            if (objcmp != null)
                //                            {
                //                                objtp.Component = objcmp;
                //                            }
                //                        }
                //                        if (objtp.RegularTAT != null && objtp.FastestTAT != null)
                //                        {
                //                            tatcombofill();
                //                        }
                //                        testpriceinfo.CurrentTestPrice = objtp;
                //                        //IList<TestPrice> lsttestprice = ((ListView)dvpertest.InnerView).CollectionSource.List.Cast<TestPrice>().ToList().Where(i => i.TAT == null && i.Surcharge == 0 && i.Price == 0).ToList(); //(CriteriaOperator.Parse("[TAT] Is Null And[Surcharge] Is Null And[Price] Is Null And[AdditionalParamPriceItemPerUnit] Is Null And[BasicParamPrice] Is Null And[BasicParamPriceItemPerUnit] Is Null"));
                //                        //IList<TestPrice> lsttestprice = os.GetObjects<TestPrice>(CriteriaOperator.Parse("[Component.Oid] = ? And [TestMethod.Oid] = ? And [TAT] Is Null And [Surcharge] = '0' And [Price] = '0' And [AdditionalParamPriceItemPerUnit] = '0' And[BasicParamPrice] = '0' And[BasicParamPriceItemPerUnit] = '0'", objtp.Component.Oid, objtm.Oid));
                //                        //if (lsttestprice.Count == 0)
                //                        //{
                //                        //    TestPrice ctobjtp = os.CreateObject<TestPrice>();
                //                        //    ctobjtp.Component = os.GetObject(objtp.Component);
                //                        //    ctobjtp.TestMethod = os.GetObject(objtm);
                //                        //    os.CommitChanges();
                //                        //    os.Refresh();
                //                        //}


                //                        DashboardViewItem dvpertest = ((DetailView)View).FindItem("PerTest") as DashboardViewItem;
                //                        DashboardViewItem dvperparameter = ((DetailView)View).FindItem("PerParameter") as DashboardViewItem;
                //                        if (dvpertest != null && dvpertest.InnerView != null && dvperparameter != null && dvperparameter.InnerView != null)
                //                        {
                //                            //((ListView)dvpertest.InnerView).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[Component.Oid] = ? And [TestMethod] = ?", objtp.Component.Oid, objtm.Oid);
                //                            //((ListView)dvperparameter.InnerView).CollectionSource.Criteria["Filter2"] = CriteriaOperator.Parse("[Component.Oid] = ? And [TestMethod] = ?", objtp.Component.Oid, objtm.Oid);
                //                            ((ListView)dvpertest.InnerView).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[TestMethod] = ? And [TestPrice.Oid] = ?", objtm.Oid, testpriceinfo.CurrentTestPrice.Oid);
                //                            ((ListView)dvperparameter.InnerView).CollectionSource.Criteria["Filter2"] = CriteriaOperator.Parse("[TestMethod] = ? And [TestPrice.Oid] = ?", objtm.Oid, testpriceinfo.CurrentTestPrice.Oid);
                //                        }
                //                    }
                //                }
                //                else if (View != null && View.Id == "TestPrice_ListView")
                //                {
                //                    TestMethod crtobjtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", crtobjtm.Oid);
                //                    ((ListView)View).ObjectSpace.Refresh();

                //                    //List<string> objOid = ((ListView)View).CollectionSource.List.Cast<TestPrice>().Where(i => i.Component.Components != null && i.TestMethod.Oid == crtobjtm.Oid).Select(i => i.Component.Components.ToString()).Distinct().ToList();
                //                    //if (objOid.Count > 0)
                //                    //{
                //                    //    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Component.Components", objOid);
                //                    //}

                //                    //using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(TestPrice)))
                //                    //{
                //                    //    lstview.Criteria = CriteriaOperator.Parse("[TestMethod.Oid] = ? AND [Component] Is Not Null", crtobjtm.Oid);
                //                    //    lstview.Properties.Add(new ViewProperty("Component", SortDirection.Ascending, "Component", true, true));
                //                    //    List<object> lstComponent = new List<object>();
                //                    //    foreach (ViewRecord rec in lstview)
                //                    //        lstComponent.Add(rec["Component"]);
                //                    //    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Component", lstComponent);
                //                    //}


                //                    //List<object> groups = new List<object>();
                //                    //TestMethod crtobjtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //                    //IList<TestPrice> lsttp = ObjectSpace.GetObjects<TestPrice>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [GCRecord] is NUll", crtobjtm.Oid));
                //                    //if (lsttp != null && lsttp.Count > 0)
                //                    //{
                //                    //    foreach (TestPrice testprice in lsttp)
                //                    //    {
                //                    //        if(!groups.Contains(testprice.Component.Components))
                //                    //        {
                //                    //            groups.Add(testprice.Component.Components);
                //                    //        }
                //                    //    }
                //                    //    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new GroupOperator(GroupOperatorType.And, new InOperator("Component.Components", groups));
                //                    //}
                //                }
                //                else if (View != null && View.Id == "TestPriceDetail_ListView_Copy_perparameter")
                //                {
                //                    TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid);
                //                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //                    if (gridListEditor != null && gridListEditor.Grid != null)
                //                    {
                //                        ASPxGridView gv = (ASPxGridView)gridListEditor.Grid;
                //                        gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                //                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                //                        selparameter.CallbackManager.RegisterHandler("perparameter", this);
                //                        if (gridListEditor != null && gridListEditor.Grid != null)
                //                        {
                //                            gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                //                            gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                //                   var chkselect = s.cpEndCallbackHandlers;
                //                        if (e.visibleIndex != '-1')
                //                        {
                //                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                //                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                //                            RaiseXafCallback(globalCallbackControl, 'perparameter', 'Selected|' + Oidvalue , '', false);    
                //                         }else{
                //                            RaiseXafCallback(globalCallbackControl, 'perparameter', 'UNSelected|' + Oidvalue, '', false);    
                //                         }
                //                        }); 
                //                        }
                //                        else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                //                        {    
                //if(chkselect != 'selectedall')
                //{
                //RaiseXafCallback(globalCallbackControl, 'perparameter', 'SelectAll', '', false);       
                //}
                //                        }
                //                         else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                //                         {
                //                        RaiseXafCallback(globalCallbackControl, 'perparameter', 'UNSelectAll', '', false);                        
                //                        }                      
                //                    }";
                //                            gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                //                            {  
                //                                if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Priority') == 'Regular')
                //                                {
                //                                    if(e.focusedColumn.fieldName == 'Surcharge')
                //                                    {
                //                                        e.cancel = true;
                //                                        alert('In priority is regular, surcharge was non editable');
                //                                    }
                //                                }
                //                                if(e.focusedColumn.fieldName == 'Price')
                //                                {
                //                                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'TAT') == null)
                //                                    {
                //                                        e.cancel = true;
                //                                        alert('TAT cannot be empty');   
                //                                    }
                //                                }
                //                                if(e.focusedColumn.fieldName == 'Surcharge')
                //                                {
                //                                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'TAT') == null)
                //                                    {
                //                                        e.cancel = true;
                //                                        alert('TAT cannot be empty');   
                //                                    }
                //                                    //else if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Price') == null || s.batchEditApi.GetCellValue(e.visibleIndex, 'Price') == '0')
                //                                    //{
                //                                    //    e.cancel = true;
                //                                    //    alert('Price cannot be empty');   
                //                                    //}
                //                                }
                //                            }";

                //                            gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                //                            window.setTimeout(function() {   
                //                            if (s.batchEditApi.HasChanges()) {  
                //                                   s.UpdateEdit();  
                //                                 } 
                //                            var basicparamprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'BasicParamPrice');
                //                            var basicparampriceitemperunit = s.batchEditApi.GetCellValue(e.visibleIndex, 'BasicParamPriceItemPerUnit'); 
                //                            var surcharge = s.batchEditApi.GetCellValue(e.visibleIndex, 'Surcharge'); 
                //                            var additionalparampriceItemperUnit = s.batchEditApi.GetCellValue(e.visibleIndex, 'AdditionalParamPriceItemPerUnit'); 
                //                            var price = s.batchEditApi.GetCellValue(e.visibleIndex, 'Price'); 
                //                            var priority = s.batchEditApi.GetCellValue(e.visibleIndex, 'Priority'); 
                //                            var tat = s.batchEditApi.GetCellValue(e.visibleIndex, 'TAT.Oid'); 
                //                            if(basicparamprice < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'BasicParamPrice', 0);
                //                             }
                //                             if(basicparampriceitemperunit < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'BasicParamPriceItemPerUnit', 0);
                //                             }
                //                             if(surcharge < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'Surcharge', 0);
                //                             }
                //                             if(additionalparampriceItemperUnit < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'AdditionalParamPriceItemPerUnit', 0);
                //                             }
                //                            if(price < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'Price', 0);
                //                             }
                //                            var count = 0;
                //                            var allGirds = ASPx.GetControlCollection().GetControlsByType(ASPxClientGridView);
                //                            for (var a in allGirds) {{        
                //                            for (var i = 0; i <= allGirds[a].GetVisibleRowsOnPage() - 1; i++) {{
                //                            var extat = allGirds[a].batchEditApi.GetCellValue(i, 'TAT.Oid');
                //                                if (extat != null && tat != null && extat == tat)
                //                                {
                //                                    count = count + 1;
                //                                    if(count > 1)
                //                                    {
                //                                    alert('TAT already selected');
                //                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'TAT', null);
                //                                    }
                //                                }
                //                            }}
                //                            }}
                //                            }, 20); }";
                //                        }
                //                    }
                //                }
                //                else if (View != null && View.Id == "TestPriceDetail_ListView_Copy_pertest")
                //                {
                //                    TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid);
                //                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //                    if (gridListEditor != null && gridListEditor.Grid != null)
                //                    {
                //                        ASPxGridView gv = (ASPxGridView)gridListEditor.Grid;
                //                        gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                //                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                //                        selparameter.CallbackManager.RegisterHandler("pertest", this);
                //                        if (gridListEditor != null && gridListEditor.Grid != null)
                //                        {
                //                            gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;

                //                            gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) {  
                //                        var chkselect = s.cpEndCallbackHandlers;
                //                        if (e.visibleIndex != '-1')
                //                        {
                //                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                //                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                //                            RaiseXafCallback(globalCallbackControl, 'pertest', 'Selected|' + Oidvalue , '', false);    
                //                         }else{
                //                            RaiseXafCallback(globalCallbackControl, 'pertest', 'UNSelected|' + Oidvalue, '', false);    
                //                         }
                //                        }); 
                //                        }
                //                        else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                //                        {    
                //                        if(chkselect != 'selectedall')
                //                        {
                //                        RaiseXafCallback(globalCallbackControl, 'pertest', 'SelectAll', '', false);       
                //                        }
                //                        }
                //                         else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                //                         {
                //                        RaiseXafCallback(globalCallbackControl, 'pertest', 'UNSelectAll', '', false);                        
                //                        }                      
                //                    }";
                //                            gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                //                            {  
                //                                if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Priority') == 'Regular')
                //                                {
                //                                    if(e.focusedColumn.fieldName == 'Surcharge')
                //                                    {
                //                                        e.cancel = true;
                //                                        alert('In Priority is Regular, Surcharge is non editable');
                //                                    }
                //                                }
                //                                if(e.focusedColumn.fieldName == 'Price')
                //                                {
                //                                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'TAT') == null)
                //                                    {
                //                                        e.cancel = true;
                //                                        alert('TAT cannot be empty');   
                //                                    }
                //                                }
                //                                if(e.focusedColumn.fieldName == 'Surcharge')
                //                                {
                //                                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'TAT') == null)
                //                                    {
                //                                        e.cancel = true;
                //                                        alert('TAT cannot be empty');   
                //                                    }
                //                                    else if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Price') == null || s.batchEditApi.GetCellValue(e.visibleIndex, 'Price') == '0')
                //                                    {
                //                                        e.cancel = true;
                //                                        alert('Price cannot be empty');   
                //                                    }
                //                                }
                //                            }";
                //                            gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                //                            window.setTimeout(function() {   
                //                              if (s.batchEditApi.HasChanges()) {  
                //                                   s.UpdateEdit();  
                //                                 } 
                //                            var basicparamprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'BasicParamPrice');
                //                            var basicparampriceitemperunit = s.batchEditApi.GetCellValue(e.visibleIndex, 'BasicParamPriceItemPerUnit'); 
                //                            var surcharge = s.batchEditApi.GetCellValue(e.visibleIndex, 'Surcharge'); 
                //                            var additionalparampriceItemperUnit = s.batchEditApi.GetCellValue(e.visibleIndex, 'AdditionalParamPriceItemPerUnit'); 
                //                            var price = s.batchEditApi.GetCellValue(e.visibleIndex, 'Price'); 
                //                            var priority = s.batchEditApi.GetCellValue(e.visibleIndex, 'Priority'); 
                //                            var tat = s.batchEditApi.GetCellValue(e.visibleIndex, 'TAT.Oid'); 
                //                            if(basicparamprice < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'BasicParamPrice', 0);
                //                             }
                //                             if(basicparampriceitemperunit < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'BasicParamPriceItemPerUnit', 0);
                //                             }
                //                             if(surcharge < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'Surcharge', 0);
                //                             }
                //                             if(additionalparampriceItemperUnit < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'AdditionalParamPriceItemPerUnit', 0);
                //                             }
                //                            if(price < 0 )
                //                             {
                //                              alert('Not allow negative value');
                //                              s.batchEditApi.SetCellValue(e.visibleIndex, 'Price', 0);
                //                             }
                //                             if(surcharge >0 && price >0)
                //                              {
                //                                   var tempprice = (surcharge/100) * price;
                //                                   tempprice = Math.round(tempprice + price); 
                //                                   //alert(tempprice);
                //                                   s.batchEditApi.SetCellValue(e.visibleIndex, 'Price', tempprice); 
                //                                   s.UpdateEdit();  
                //                              } 
                //                            var count = 0;
                //                            var allGirds = ASPx.GetControlCollection().GetControlsByType(ASPxClientGridView);
                //                            for (var a in allGirds) {{        
                //                            for (var i = 0; i <= allGirds[a].GetVisibleRowsOnPage() - 1; i++) {{
                //                            var extat = allGirds[a].batchEditApi.GetCellValue(i, 'TAT.Oid');
                //                                if (extat != null && tat != null && extat == tat)
                //                                {
                //                                    count = count + 1;
                //                                    if(count > 1)
                //                                    {
                //                                    alert('TAT already selected');
                //                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'TAT', null);
                //                                    }
                //                                }
                //                            }}
                //                            }}
                //                            }, 20); }";
                //                        }
                //                        //ICallbackManagerHolder holder = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                //                        //holder.CallbackManager.RegisterHandler("sam", this);
                //                        //SaveAction.SetClientScript(string.Format(CultureInfo.InvariantCulture, js, holder.CallbackManager.GetScript("sam", "errstat")), false);
                //                    }
                //                }

                else if (View.Id == "TestCopy_ListView")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                }

                if (View.Id == "Testparameter_ListView_Test_InternalStandards" || View.Id == "TestMethod_QCTypes_ListView" || View.Id == "Testparameter_ListView_Test_QCSampleParameter" || View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault"
                    || View.Id == "TestMethod_surrogates_ListView" || View.Id == "Testparameter_ListView_Test_SampleParameter" || View.Id == "Component_ListView_Test" || View.Id == "Testparameter_ListView_Test_Surrogates"
                    || View.Id == "TestMethod_PrepMethods_ListView" || View.Id == "TestMethod_Preservatives_ListView" || View.Id == "TestMethod_SamplingMethods_ListView" || View.Id == "Method_ListView_Copy_TestMethod")
                {
                    ASPxGridListEditor gridlist = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlist != null && gridlist.Grid != null)
                    {
                        gridlist.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridlist.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridlist.IsFooterVisible = true;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        gridlist.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        selparameter.CallbackManager.RegisterHandler("Test", this);
                        //gridlist.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                        //    {                  
                        //        if(!s.batchEditApi.HasChanges(e.visibleIndex)) 
                        //        {
                        //        sessionStorage.setItem('TPFocusedColumn', null);  
                        //        var fieldName = e.cellInfo.column.fieldName;                       
                        //        sessionStorage.setItem('TPFocusedColumn', fieldName);
                        //        }
                        //    }";

                        gridlist.Grid.CustomJSProperties += Grid_CustomJSProperties;
                        gridlist.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e) { 
                            sessionStorage.setItem('TPFocusedColumn', null);  
                            var fieldName = e.cellInfo.column.fieldName;                       
                            sessionStorage.setItem('TPFocusedColumn', fieldName);
                        }";
                        if (View.Id == "Testparameter_ListView_Test_SampleParameter")
                        {
                            gridlist.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'DefaultResult')
                                             {   
                                                var IsDefault=s.batchEditApi.GetCellValue(e.visibleIndex, 'IsResultDefaultValue');
                                                //alert(IsDefault);
                                                if(s.cpAllowbatchEdit == false)
                                                    {
                                                       e.cancel = true;
                                                    }
                                                else if (IsDefault == true)
	                                              {
                                                     //alert('A');
	                                             	 e.cancel = true; 
                                                  }
                                             }
                                          else
                                              {
                                                 e.cancel = false;
                                              }
                                           }";
                        }
                        else
                        {
                            gridlist.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {
                                    if(s.cpAllowbatchEdit == false)
                                    {
                                        e.cancel = true;
                                    }
                            }";
                        }
                    }
                }

                else if (View.Id == "TestMethod_ListView_Copy_CopyTest" || View.Id == "Parameter_ListView_Test")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = (ASPxGridView)editor.Grid;
                    if (gridView != null)
                    {
                        gridView.SettingsBehavior.AllowSelectByRowClick = true;
                        if (View.Id == "TestMethod_ListView_Copy_CopyTest")
                        {
                            gridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
                        }
                        gridView.Settings.VerticalScrollableHeight = 300;
                        ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView(View.Id)).PageSize = 10;

                    }
                }
                else if (View.Id == "Parameter_LookupListView_Surrogates")
                {
                    TestMethod crtobjtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                    IList<Testparameter> lsttp = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", crtobjtm.Oid));
                    List<Guid> lstparaoid = new List<Guid>();
                    if (lsttp != null && lsttp.Count > 0)
                    {
                        foreach (Testparameter objp in lsttp.ToList())
                        {
                            Testparameter objpara = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", objp.Oid));
                            if (objpara.Parameter != null)
                            {
                                lstparaoid.Add(objpara.Parameter.Oid);
                            }
                        }
                        CriteriaOperator cs = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", lstparaoid.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")";
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", lstparaoid.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")");
                    }
                }
                if (View.Id == "TestMethod_QCTypes_ListView")
                {
                    //Frame.GetController<LinkUnlinkController>().LinkAction.Executed += LinkAction_Executed;
                    //Frame.GetController<LinkUnlinkController>().UnlinkAction.Executed += UnlinkAction_Executed;
                    chkunlink = true;
                }
                //if (View.Id == "Testparameter_ListView_Test_QCSampleParameter")
                //{
                //    //TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //    //if (testInfo.lstQcParameters != null)
                //    //{
                //    //    if (testInfo.lstQcParameters.Count > 0)
                //    //    {
                //    //        foreach (Testparameter param in testInfo.lstQcParameters)
                //    //        {
                //    //            if (!((ListView)View).CollectionSource.List.Contains(param) && param.QCType.QCTypeName.ToString() == objtm.QCtypesCombo.QCTypeName.ToString())
                //    //            {
                //    //                ((ListView)View).CollectionSource.Add(param);
                //    //            }
                //    //        }
                //    //    }
                //    //}
                //    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                //    if (editor != null && editor.Grid != null && ((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.GetCount() > 0)
                //    {
                //        ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("Testparameter_ListView_Test_QCSampleParameter")).PageSize = ((ListView)View).CollectionSource.GetCount();
                //    }

                //    //ListPropertyEditor lstqctype = ((DetailView)Application.MainWindow.View).FindItem("QCTypes") as ListPropertyEditor;
                //    //if (lstqctype != null && lstqctype.ListView != null)
                //    //{
                //    //    // ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.Oid] = ?",lstqctype);
                //    //    ListView lvqctype = lstqctype.ListView;
                //    //    if (lvqctype.CollectionSource.List.Count == 0)
                //    //    {
                //    //        ((ListView)View).CollectionSource.List.Clear();
                //    //        foreach (Guid objqcclear in ((ListView)View).CollectionSource.List.Cast<Testparameter>().Select(i => new Guid(i.Oid.ToString())).ToList())
                //    //        {
                //    //            Testparameter objqc = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", objqcclear));
                //    //            ((ListView)View).CollectionSource.List.Remove(objqc);
                //    //        }
                //    //        //ObjectSpace.CommitChanges();
                //    //    }
                //    //}
                //    ////TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //    //if(objtm != null && objtm.QCtypesCombo == null && qctypeinfo.objtmQCType != null && qctypeinfo.objtmQCType.Count > 0)
                //    //{
                //    //    if (testInfo.CurrentQcType != null)
                //    //    {
                //    //        objtm.QCtypesCombo = testInfo.CurrentQcType;
                //    //    }
                //    //    else
                //    //    {
                //    //        objtm.QCtypesCombo = qctypeinfo.objtmQCType[0];
                //    //    }
                //    //    if (((ListView)View).CollectionSource.List.Count >0)
                //    //    {
                //    //        objtm.QCtypesCombo = qctypeinfo.objtmQCType[0];
                //    //        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] = ? And [TestMethod.Oid] = ? and [QCType.QCTypeName] <> 'Sample'", objtm.QCtypesCombo.QCTypeName, objtm.Oid);
                //    //    }
                //    //    else
                //    //    {
                //    //        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] = ? And [TestMethod.Oid] = ? and [QCType.QCTypeName] <> 'Sample'",null, null);
                //    //    }

                //    //}
                //    //else if (objtm != null && objtm.QCtypesCombo != null && qctypeinfo.objtmQCType != null)
                //    //{
                //    //    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] = ? And [TestMethod.Oid] = ? and [QCType.QCTypeName] <> 'Sample'", objtm.QCtypesCombo.QCTypeName, objtm.Oid);
                //    //}
                //}
                if (View.Id == "Testparameter_ListView_Test_Surrogates")
                {
                    //TestMethod crtobjtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                    //IList<Testparameter> lsttp = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", crtobjtm.Oid));
                    //if (lsttp != null && lsttp.Count > 0)
                    //{
                    //    CriteriaOperator cs1 = new InOperator("TestMethod", lsttp);
                    //    CriteriaOperator cs = "Not [TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lsttp.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = new NotOperator(new InOperator("TestMethod", lsttp));
                    //}
                }
                else if (View != null && View.Id == "Testparameter_ListView_Copy")
                {
                    GridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (GridListEditor != null)
                    {
                        ASPxGridView gridView = GridListEditor.Grid;
                        if (gridView != null)
                        {
                            gridView.Load += gridView_Load;

                            gridView.FillContextMenuItems += GridView_FillContextMenuItems;
                            gridView.SettingsContextMenu.Enabled = true;
                            gridView.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                            gridView.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {                  
                                sessionStorage.setItem('TPFocusedColumn', null);  
                                if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                                {                              
                                    e.cancel = true;
                                }
                                else if (e.cellInfo.column.fieldName == 'Parameter.ParameterName' ||e.cellInfo.column.fieldName == 'TestMethod.MethodName.MethodName')
                                {                         
                                    e.cancel = true;
                                }                        
                                else
                                {
                                    var fieldName = e.cellInfo.column.fieldName;                       
                                    sessionStorage.setItem('TPFocusedColumn', fieldName);  
                                }                                         
                            }";

                            gridView.ClientSideEvents.BatchEditEndEditing = @"function(s, e){
                            window.setTimeout(function () {
                                var FocusedColumn = sessionStorage.getItem('TPFocusedColumn');   
                                if(FocusedColumn == 'RptLimit'){
                                    if ((s.batchEditApi.GetCellValue(e.visibleIndex, 'DefaultResult') == null || s.batchEditApi.GetCellValue(e.visibleIndex, 'DefaultResult').toString().length == 0) && (s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') != null || s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit').toString().length > 0)) {
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'DefaultResult', '<' + s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit'));
                                        s.UpdateEdit();
                                    }
                                }
                                else
                                {
                                    s.UpdateEdit();
                                }
                                }, 10);
                            }";

                            gridView.ClientSideEvents.BatchEditRowValidating = @"function(s, e) {
                                   var SigFig = s.GetColumnByField('SigFig').index;
                                   var Decimal = s.GetColumnByField('Decimal').index;
                                   var CutOff = s.GetColumnByField('CutOff').index;
                                   if (e.validationInfo[SigFig].value != null && e.validationInfo[Decimal].value != null && e.validationInfo[CutOff].value == null)
                                   {
                                    e.validationInfo[CutOff].isValid = false;                                   
                                    e.validationInfo[CutOff].errorText = 'SigFig and Decimal cannot coexist without a CutOff values entered!';
                                   }
                             }";

                            gridView.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('TPFocusedColumn');                                
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
                                            s.batchEditApi.ValidateRows();
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
                                           s.batchEditApi.ValidateRows();
                                        }                            
                                    }
                                }
                                e.processOnServer = false;
                            }";

                        }
                    }
                }
                else if (View.Id == "TestMethod_DetailView")
                {
                    foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible))
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(FileDataPropertyEditor))
                        {
                            FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                        {
                            ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                {
                                    propertyEditor.FindEdit.Editor.BackColor = Color.LightYellow;
                                }
                                else if (propertyEditor.DropDownEdit != null)
                                {
                                    propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightYellow;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                    }
                    TestMethod objtm = (TestMethod)View.CurrentObject;
                    if (objtm != null)
                    {
                        testInfo.isgroup = objtm.IsGroup;
                        if (objtm.IsGroup)
                        {
                            CopyParameters.Active.SetItemValue("Disable", false);
                        }
                        else
                        {
                            CopyParameters.Active.SetItemValue("Disable", true);
                        }
                        if (qctypeinfo.objtmQCType == null)
                        {
                            qctypeinfo.objtmQCType = new List<QCType>();
                        }
                        //foreach (QCType objqc in objtm.QCTypes.OrderBy(i => i.QCTypeName).ToList())
                        //{
                        //    if (!qctypeinfo.objtmQCType.Contains(objqc))
                        //    {
                        //        qctypeinfo.objtmQCType.Add(objqc);
                        //    }
                        //}
                        qctypeinfo.objtmQCType = objtm.QCTypes.Where(i => i.QCTypeName != null).OrderBy(i => i.QCTypeName).Distinct().ToList();
                        ASPxLookupPropertyEditor dropdownEditor = (ASPxLookupPropertyEditor)((DetailView)View).FindItem("QCtypesCombo");
                        if (dropdownEditor != null && qctypeinfo.objtmQCType.Count > 0 && objtm.QCtypesCombo == null)
                        {

                            objtm.QCtypesCombo = this.View.ObjectSpace.GetObject<QCType>(qctypeinfo.objtmQCType[0]); DashboardViewItem lstqcparameter = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                            if (lstqcparameter != null && lstqcparameter.InnerView != null)
                            {
                                ((ListView)lstqcparameter.InnerView).CollectionSource.Criteria["QCFilter"] = CriteriaOperator.Parse("[QCType.QCTypeName] = ?", objtm.QCtypesCombo.QCTypeName);

                                gridcount();
                            }
                            else
                            {
                                lstqcparameter.CreateControl();
                                ((ListView)lstqcparameter.InnerView).CollectionSource.Criteria["QCFilter"] = CriteriaOperator.Parse("[QCType.QCTypeName] = ?", objtm.QCtypesCombo.QCTypeName);

                                gridcount();
                            }
                        }
                    }
                    DashboardViewItem dvtestmethod = ((DetailView)View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                    if (dvtestmethod != null && dvtestmethod.InnerView != null)
                    {
                        ((ListView)dvtestmethod.InnerView).Refresh();
                    }

                }
                else if (View.Id == "TestParamterEdit")
                {
                    saveAlert = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    saveAlert.CallbackManager.RegisterHandler("savechanges", this);
                }
                else if (View.Id == "TestMethod_DetailView_CopyParameter")
                {
                    //IObjectSpace objspace = Application.CreateObjectSpace();
                    //TestMethod maincurrentobj = (TestMethod)Application.MainWindow.View.CurrentObject;
                    //if (maincurrentobj != null)
                    //{
                    //    TestMethod currentobj = (TestMethod)View.CurrentObject;
                    //    currentobj.TestName = maincurrentobj.TestName;
                    //    ListPropertyEditor lstqctypes = ((DetailView)Application.MainWindow.View).FindItem("QCTypes") as ListPropertyEditor;
                    //    ListView lstprpedit = lstqctypes.ListView;
                    //    List<QCType> lstobjqctype = new List<QCType>();
                    //    DashboardViewItem lstcrtqctypesct = ((DetailView)View).FindItem("QCTypeto") as DashboardViewItem;
                    //    ListView lstqctypesct = lstcrtqctypesct.InnerView as ListView;
                    //    if (lstqctypes.ListView != null && lstprpedit.CollectionSource.List.Count > 0)
                    //    {
                    //        foreach (QCType objqctype in lstprpedit.CollectionSource.List.Cast<QCType>().ToList())
                    //        {
                    //            lstobjqctype.Add(objqctype);
                    //        }

                    //        //IList<Testparameter> lsttestparameter = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ?", maincurrentobj.Oid));

                    //        if (lstcrtqctypesct != null && lstcrtqctypesct.InnerView != null && lstobjqctype != null)
                    //        {
                    //            CriteriaOperator criteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", lstobjqctype.Select(i => i.Oid.ToString().Replace("'", "''")).Distinct())) + ")";
                    //            ((ListView)lstqctypesct).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstobjqctype.Select(i => i.Oid.ToString().Replace("'", "''")).Distinct())) + ")");
                    //        }
                    //        qctypeinfo.QcTypeCopyTo = true;
                    //    }
                    //    else if (lstqctypes.ListView != null && lstprpedit.CollectionSource.List.Count == 0)
                    //    {
                    //        if (lstcrtqctypesct != null && lstcrtqctypesct.InnerView != null)
                    //        {
                    //            ((ListView)lstqctypesct).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[QCTypeName] = 'Sample'");
                    //        }
                    //    }
                    //}
                }
                else if (View.Id == "TestMethod_DetailView_CopyTest")
                {
                    //IObjectSpace os = Application.CreateObjectSpace();
                    TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (objtm != null)
                    {
                        TestMethod objcrttm = (TestMethod)View.CurrentObject;
                        Matrix objmat = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[Oid] = ?", objtm.MatrixName.Oid));
                        Method objmeth = ObjectSpace.FindObject<Method>(CriteriaOperator.Parse("[Oid] = ?", objtm.MethodName.Oid));
                        objcrttm.TestName = objtm.TestName;
                        objcrttm.MethodName = objmeth;
                        objcrttm.MatrixName = objmat;
                    }
                }
                else if (View.Id == "TestMethod_ListView_Copyto")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor.AllowEdit != null && editor.Grid != null)
                    {
                        foreach (GridViewColumn column in editor.Grid.Columns)
                        {
                            if (column.Name == "InlineEditCommandColumn" || column.Name == "Edit")
                            {
                                column.Visible = false;
                            }
                        }
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("CopyTest", this);
                        if (editor != null && editor.Grid != null)
                        {
                            editor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                            editor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
var chkselect = s.cpEndCallbackHandlers;
                        if (e.visibleIndex != '-1')
                        {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'CopyTest', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'CopyTest', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                        }
                        else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                        {    
if(chkselect != 'selectedall')
{
RaiseXafCallback(globalCallbackControl, 'CopyTest', 'SelectAll', '', false);       
}
                        }
                         else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                         {
                        RaiseXafCallback(globalCallbackControl, 'CopyTest', 'UNSelectAll', '', false);                        
                        }                      
                    }";
                            // editor.Grid.ClientSideEvents.Init = js;
                        }
                    }
                    TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] <> ?", objtm.Oid);
                }
                else if (View.Id == "ResultDefaultValue_LookupListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Load += Grid_Load;
                }
                gridcount();
                if (View.Id == "TestMethod_QCTypes_ListView_CopyTo") //"TestCopy_ListView")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("CopyTestParameterFromSameTest", this);
                        if (editor != null && editor.Grid != null)
                        {
                            editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            editor.Grid.CustomJSProperties += Grid_CustomJSProperties;

                            //            editor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                            //var chkselect = s.cpEndCallbackHandlers;
                            //                        if (e.visibleIndex != '-1')
                            //                        {
                            //                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                            //                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            //                            RaiseXafCallback(globalCallbackControl, 'TestParamter', 'Selected|' + Oidvalue , '', false);    
                            //                         }else{
                            //                            RaiseXafCallback(globalCallbackControl, 'TestParamter', 'UNSelected|' + Title, '', false);    
                            //                         }
                            //                        }); 
                            //                        }
                            //                        else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                            //                        {    
                            //if(chkselect != 'selectedall')
                            //{
                            //RaiseXafCallback(globalCallbackControl, 'TestParamter', 'SelectAll', '', false);       
                            //}
                            //                        }
                            //                         else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                            //                         {
                            //                        RaiseXafCallback(globalCallbackControl, 'TestParamter', 'UNSelectAll', '', false);                        
                            //                        }                      
                            //                    }";

                            editor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'CopyTestParameterFromSameTest', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'CopyTestParameterFromSameTest', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'CopyTestParameterFromSameTest', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'CopyTestParameterFromSameTest', 'UNSelectall', '', false);                        
                      }                      
                    }";

                            // editor.Grid.ClientSideEvents.Init = js;
                        }
                    }
                }

                else if (View != null && View.Id == "TestMethod_ListView_Copy")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    gv.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                    gv.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                }
                if (View.Id == "Testparameter_ListView_Test_SampleParameter" || View.Id == "Testparameter_ListView_Test_InternalStandards"
                    || View.Id == "TestMethod_surrogates_ListView" || View.Id == "Testparameter_ListView_Test_QCSampleParameter" || View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault"
                    || View.Id == "Testparameter_ListView_Test_Component")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {                  
                                sessionStorage.setItem('CopyToAllCellFocusedColumn', null); 
                                //alert(e.cellInfo.column.fieldName);
                                if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                                {                              
                                    e.cancel = true;
                                }
                                //else if (e.cellInfo.column.fieldName == 'Parameter.Oid' ||e.cellInfo.column.fieldName == 'Sort'
                                //        ||e.cellInfo.column.fieldName == 'ParameterName' ||e.cellInfo.column.fieldName == 'Testparameter.Sort')
                                //{                         
                                //   // e.cancel = true;
                                //}                        
                                else
                                {
                                    if (e.cellInfo.column.fieldName != 'Parameter.Oid' &&e.cellInfo.column.fieldName != 'Sort'
                                   &&e.cellInfo.column.fieldName != 'ParameterName' &&e.cellInfo.column.fieldName != 'Testparameter.Sort')
                                    { 
                                    var fieldName = e.cellInfo.column.fieldName;                       
                                    sessionStorage.setItem('CopyToAllCellFocusedColumn', fieldName);  
                                    }
                                }                                         
                            }";
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('CopyToAllCellFocusedColumn');                                
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
                                            s.batchEditApi.ValidateRows();
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
                            }";
                    if (View.Id == "Testparameter_ListView_Test_Component")
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                    window.setTimeout(function() { 
                     var FocusedColumn = sessionStorage.getItem('CopyToAllCellFocusedColumn'); 
                    if(FocusedColumn=='RptLimit'||FocusedColumn=='CustomLimit')
                    {
                      var rptlimit = s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit');
                      var customlimit = s.batchEditApi.GetCellValue(e.visibleIndex, 'CustomLimit'); 
                      
                      if(rptlimit!=null && customlimit!=null && (parseFloat(customlimit) > parseFloat(rptlimit)))
                         {
                           alert('Enter the RptLimit GreaterThanOrEqual CustomLimit')
                           s.batchEditApi.SetCellValue(e.visibleIndex, 'CustomLimit', null);
                         }
                    }                                   
                    }, 20);}";
                    }
                }
                else if (View.Id == "Testparameter_ListView_Testmethod_IsGroup")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("GroupTestparameter", this);
                    ASPxGridListEditor gridlistedit = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlistedit != null)
                    {
                        gridlistedit.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridlistedit.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridlistedit.Grid.Load += Grid_Load;
                    }

                }
                else if (View.Id == "Testparameter_DetailView_CopyTest")
                {
                    TestMethod currentTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (currentTest != null)
                    {
                        testInfo.CurrentTest = currentTest;
                    }
                }
                else if (View.Id == "Testparameter_ListView_Testmethod_IsGroup_parameter_popup")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("GroupTestparameter", this);
                    ASPxGridListEditor gridlistedit = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlistedit != null)
                    {
                        gridlistedit.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridlistedit.Grid.Load += Grid_Load;
                        gridlistedit.Grid.JSProperties["cpVisibleRowCount"] = gridlistedit.Grid.VisibleRowCount;
                        gridlistedit.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.batchEditApi.ResetChanges(e.visibleIndex);
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {     

                            RaiseXafCallback(globalCallbackControl, 'GroupTestparameter', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'GroupTestparameter', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'GroupTestparameter', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'GroupTestparameter', 'UNSelectall', '', false);                        
                      }                      
                    }";
                    }

                }
                else if (View.Id == "GroupTestMethod_ListView_IsGroup")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("GroupTestparameter", this);
                    ASPxGridListEditor gridlistedit = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlistedit != null)
                    {
                        gridlistedit.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridlistedit.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridlistedit.Grid.Load += Grid_Load;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_Load1(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridview = (ASPxGridView)sender;
                if (View.Id== "Accrediation_ListView_Copy")
                {
                    if (!Isseleted)
                    {
                        gridview.JSProperties["cpVisibleRowCount"] = gridview.VisibleRowCount;
                        List<string> lststrcontainer = new List<string>();
                        objtestparametrinfo.lstaccridation = new List<string>();
                        if (Application.MainWindow.View.Id == "TestMethod_DetailView")
                        {
                            Testparameter objtestparameter = Application.MainWindow.View.ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objtestparameter != null && !string.IsNullOrEmpty(objtestparameter.lAccrediation))
                            {
                                string[] strarr = objtestparameter.lAccrediation.Split(';');
                                foreach (string objstr in strarr)
                                {
                                    Accrediation objacc = ObjectSpace.FindObject<Accrediation>(CriteriaOperator.Parse("[lAccrediation] = ?", objstr.Trim()));
                                    if (objacc != null)
                                    {
                                        lststrcontainer.Add(objacc.lAccrediation.Trim());
                                    }
                                }
                            }
                            else
                            {
                                Accrediation objac = View.ObjectSpace.FindObject<Accrediation>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                if (objac != null && !string.IsNullOrEmpty(objac.lAccrediation))
                                {
                                    string[] strarr = objac.lAccrediation.Split(';');
                                    foreach (string objstr in strarr)
                                    {
                                        Accrediation objacc = ObjectSpace.FindObject<Accrediation>(CriteriaOperator.Parse("[lAccrediation] = ?", objstr.Trim()));
                                        if (objacc != null)
                                        {
                                            lststrcontainer.Add(objacc.lAccrediation.Trim());
                                        }
                                    }
                                }

                            }
                        }
                        for (int i = 0; i <= gridview.VisibleRowCount - 1; i++)
                        {
                            string strcontainer = gridview.GetRowValues(i, "lAccrediation").ToString();
                            if (!string.IsNullOrEmpty(strcontainer) && lststrcontainer.Contains(strcontainer.Trim()))
                            {
                                gridview.Selection.SelectRow(i);
                                if (!objtestparametrinfo.lstaccridation.Contains(strcontainer.Trim()))
                                {
                                    objtestparametrinfo.lstaccridation.Add(strcontainer.Trim());
                                }
                            }
                            Isseleted = true;
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

        private void Grid_HtmlDataCellPrepared1(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if(View.Id== "Testparameter_ListView_Test_SampleParameter")
                {
                     if (e.DataColumn.FieldName == "lAccrediation")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'TestAccrediation', '{0}|{1}', '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            try
            {
                if (View.Id == "Testparameter_ListView_Test_SampleParameter")
                {
                    ASPxGridView grid = sender as ASPxGridView;
                    if (e.CommandCellType == GridViewTableCommandCellType.Data && e.CommandColumn.Name == "TestDefaultResult")
                    {
                        string value = (string)grid.GetRowValues(e.VisibleIndex, "ParameterDefaultResults");
                        if (!string.IsNullOrEmpty(value))
                            e.Cell.BackColor = Color.LightBlue;
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
                ASPxGridView gridview = (ASPxGridView)sender;
                if (View.Id == "Testparameter_ListView_Testmethod_IsGroup_parameter_popup" && isprocessact != true)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    if (!string.IsNullOrEmpty(HttpContext.Current.Session["rowid"].ToString()))
                    {
                        if (grptstinfo.lstgrouptestparameter == null)
                        {
                            grptstinfo.lstgrouptestparameter = new List<Guid>();
                        }
                        List<string> lsttempparaoid = new List<string>();
                        Testparameter objtp = os.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objtp != null && !string.IsNullOrEmpty(objtp.GroupTestParameters))
                        {
                            string[] strparaarr = objtp.GroupTestParameters.Split(';');
                            if (strparaarr != null && strparaarr.Length > 0)
                            {
                                foreach (string strparaoid in strparaarr)
                                {
                                    lsttempparaoid.Add(strparaoid.Trim());
                                }
                                for (int i = 0; i <= gridview.VisibleRowCount - 1; i++)
                                {
                                    string strparaoid = gridview.GetRowValues(i, "Oid").ToString();
                                    if (!string.IsNullOrEmpty(strparaoid) && lsttempparaoid.Contains(strparaoid.Trim()))
                                    {
                                        if (!grptstinfo.lstgrouptestparameter.Contains(new Guid(strparaoid)))
                                        {
                                            grptstinfo.lstgrouptestparameter.Add(new Guid(strparaoid.Trim()));
                                        }
                                        gridview.Selection.SelectRow(i);
                                    }
                                }
                            }
                        }
                        else if (objtp != null && string.IsNullOrEmpty(objtp.GroupTestParameters))
                        {
                            for (int i = 0; i <= gridview.VisibleRowCount - 1; i++)
                            {
                                gridview.Selection.SelectRow(i);
                                string strparaoid = gridview.GetRowValues(i, "Oid").ToString();
                                if (!grptstinfo.lstgrouptestparameter.Contains(new Guid(strparaoid)))
                                {
                                    grptstinfo.lstgrouptestparameter.Add(new Guid(strparaoid.Trim()));
                                }
                            }
                        }
                        else
                        {
                            GroupTestMethod objgrptp = os.FindObject<GroupTestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objgrptp != null && !string.IsNullOrEmpty(objgrptp.GroupTestParameters))
                            {
                                string[] strparaarr = objgrptp.GroupTestParameters.Split(';');
                                if (strparaarr != null && strparaarr.Length > 0)
                                {
                                    foreach (string strparaoid in strparaarr)
                                    {
                                        lsttempparaoid.Add(strparaoid.Trim());
                                    }
                                    for (int i = 0; i <= gridview.VisibleRowCount - 1; i++)
                                    {
                                        string strparaoid = gridview.GetRowValues(i, "Oid").ToString();
                                        if (!string.IsNullOrEmpty(strparaoid) && lsttempparaoid.Contains(strparaoid.Trim()))
                                        {
                                            gridview.Selection.SelectRow(i);
                                            if (!grptstinfo.lstgrouptestparameter.Contains(new Guid(strparaoid)))
                                            {
                                                grptstinfo.lstgrouptestparameter.Add(new Guid(strparaoid.Trim()));
                                            }
                                        }
                                    }
                                }
                            }
                            else if (objgrptp != null && string.IsNullOrEmpty(objgrptp.GroupTestParameters))
                            {
                                for (int i = 0; i <= gridview.VisibleRowCount - 1; i++)
                                {
                                    gridview.Selection.SelectRow(i);
                                    string strparaoid = gridview.GetRowValues(i, "Oid").ToString();
                                    if (!grptstinfo.lstgrouptestparameter.Contains(new Guid(strparaoid)))
                                    {
                                        grptstinfo.lstgrouptestparameter.Add(new Guid(strparaoid.Trim()));
                                    }
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "ResultDefaultValue_LookupListView")
                {
                    var selectionBoxColumn = gridview.Columns.OfType<GridViewCommandColumn>().Where(i => i.ShowSelectCheckbox).FirstOrDefault();
                    if (selectionBoxColumn != null)
                    {
                        selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
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
            //e.Cell.Attributes.Add("onblur", "alert('hi')");     
            try
            {
                if (View.Id == "Testparameter_ListView_Testmethod_IsGroup")
                {
                    if (e.DataColumn.FieldName == "GroupTestParameter" && e.CellValue != null)
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'GroupTestparameter', 'GroupTest|'+{0}, '', false)", e.VisibleIndex));
                    }
                }
                if (View.Id == "GroupTestMethod_ListView_IsGroup")
                {
                    if (e.DataColumn.FieldName == "GroupTestParameter" && e.CellValue != null)
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'GroupTestparameter', 'GroupTest|'+{0}, '', false)", e.VisibleIndex));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Surrogates_UnlinkAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Surrogates_LinkAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                Testparameter objtm = os.CreateObject<Testparameter>();
                DetailView createdv = Application.CreateDetailView(os, "Testparameter_ListView_Test_Surrogates_Copy", true, objtm);
                createdv.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showviewparameter = new ShowViewParameters(createdv);
                showviewparameter.Context = TemplateContext.PopupWindow;
                showviewparameter.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += Surrogates_dc_Accepting;
                dc.AcceptAction.Executed += AcceptAction_Executed;
                dc.CloseOnCurrentObjectProcessing = false;
                showviewparameter.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showviewparameter, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Surrogates_dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    DashboardViewItem dvsurrogates = ((DetailView)Application.MainWindow.View).FindItem("Surrogates") as DashboardViewItem;
                    if (dvsurrogates != null && dvsurrogates.InnerView != null)
                    {
                        ListView lstdvsurrogates = dvsurrogates.InnerView as ListView;
                        foreach (Testparameter objtp in View.SelectedObjects)
                        {
                            lstdvsurrogates.CollectionSource.Add(objtp);
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

        private void LinkAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {

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
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (View.Id == "TestCopy_ListView") //"TestMethod_QCTypes_ListView_CopyTo")
                    {
                        string[] arrParams = parameter.Split('|');
                        IObjectSpace space = Application.CreateObjectSpace();
                        if (qctypeinfo.lstcopytest == null)
                        {
                            qctypeinfo.lstcopytest = new List<string>();
                        }

                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        DashboardViewItem objCopyFieldsFrom = ((DetailView)view).FindItem("CopyTest") as DashboardViewItem;
                        ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        //DashboardViewItem objProjectList = (DashboardViewItem)((DetailView)view).FindItem("ProjectList");
                        if (objCopyFieldsFrom != null && objCopyFieldsFrom.InnerView != null && objCopyFieldsFrom.InnerView is ListView)
                        {
                            if (parameter == "SelectAll" && string.IsNullOrEmpty(qctypeinfo.chkselectall))
                            {
                                foreach (Guid objqcselall in ((ListView)objCopyFieldsFrom.InnerView).CollectionSource.List.Cast<TestCopy>().Select(i => new Guid(i.Title.ToString())).ToList())
                                {
                                    qctypeinfo.lstcopytest.Add(objqcselall.ToString());
                                    gridlisteditor.Grid.Selection.SelectRowByKey(objqcselall);
                                }
                                    ((ListView)View).Refresh();
                                qctypeinfo.chkselectall = "selectedall";
                            }
                            else if (parameter == "UNSelectAll")
                            {
                                foreach (Guid objqcunselall in ((ListView)objCopyFieldsFrom.InnerView).CollectionSource.List.Cast<TestCopy>().Select(i => new Guid(i.Oid.ToString())).ToList())
                                {
                                    qctypeinfo.lstcopytest.Remove(objqcunselall.ToString());
                                    gridlisteditor.Grid.Selection.UnselectRowByKey(objqcunselall);
                                }
                                qctypeinfo.chkselectall = null;
                            }
                            else
                            {
                                //string[] arrParams = parameter.Split('|');
                                if (arrParams[0] == "Selected")
                                {
                                    //qctypeinfo.lstQCType.Clear();
                                    TestCopy objqcsel = objCopyFieldsFrom.InnerView.ObjectSpace.FindObject<TestCopy>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                                    if (objqcsel != null)
                                    {
                                        qctypeinfo.lstcopytest.Add(objqcsel.Title.ToString());
                                    }
                                }
                                else if (arrParams[0] == "UNSelected")
                                {
                                    TestCopy objqcunsel = objCopyFieldsFrom.InnerView.ObjectSpace.FindObject<TestCopy>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                                    if (objqcunsel != null)
                                    {
                                        qctypeinfo.lstcopytest.Remove(objqcunsel.Title.ToString());
                                        gridlisteditor.Grid.Selection.UnselectRowByKey(objqcunsel.Oid);
                                    }
                                }
                            }

                            if (qctypeinfo.lstcopytest != null && qctypeinfo.lstcopytest.Count > 0)
                            {
                                CriteriaOperator criteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", qctypeinfo.lstcopytest.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")";
                                IList<TestCopy> lstoctypeoid = ObjectSpace.GetObjects<TestCopy>(CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", qctypeinfo.lstcopytest.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")"));
                                if (lstoctypeoid != null && lstoctypeoid.Count > 0)
                                {
                                    foreach (TestCopy oid in lstoctypeoid.ToList())
                                    {
                                        gridlisteditor.Grid.Selection.SelectRowByKey(oid.Oid);
                                    }
                                }
                                ((ListView)View).Refresh();

                            }
                        }

                        //if (View.Id == "TestMethod_QCTypes_ListView_CopyTo")
                        //{
                        //    if (qctypeinfo.lstQCType != null)
                        //    {
                        //        ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        //        foreach (Guid obj in qctypeinfo.lstQCType)
                        //        {
                        //            gridlisteditor.Grid.Selection.SelectRow(obj);
                        //        //}
                        //    }
                        //}
                    }
                    else if (View.Id == "Testparameter_ListView_Test_SampleParameter")
                    {
                        string[] arrParams = parameter.Split('|');

                        if(!string.IsNullOrEmpty(arrParams[0]) && arrParams[0]== "lAccrediation")
                        {
                            SampleParameter sampleParameter = View.CurrentObject as SampleParameter;
                            //Guid sampleparameteroid = new Guid();
                            //sampleparameteroid=sampleParameter.Oid;
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(arrParams[1]), "Oid");
                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(objectSpace, typeof(Accrediation));
                            ListView lvcontainer = Application.CreateListView("Accrediation_ListView_Copy", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lvcontainer);
                            showViewParameters.CreatedView = lvcontainer;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Dc_Accepting1; 
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                    else if (View.Id == "TestMethod_ListView_Copyto")
                    {
                        string[] arrParams = parameter.Split('|');
                        IObjectSpace space = Application.CreateObjectSpace();
                        if (qctypeinfo.lstQCType == null)
                        {
                            qctypeinfo.lstQCType = new List<Guid>();
                        }

                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        DashboardViewItem objCopyFieldsFrom = (DashboardViewItem)((DetailView)view).FindItem("copytestto");
                        ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        //DashboardViewItem objProjectList = (DashboardViewItem)((DetailView)view).FindItem("ProjectList");
                        if (objCopyFieldsFrom != null && objCopyFieldsFrom.InnerView != null && objCopyFieldsFrom.InnerView is ListView)
                        {
                            if (parameter == "SelectAll" && string.IsNullOrEmpty(qctypeinfo.chkselectall))
                            {
                                foreach (Guid objqcselall in ((ListView)objCopyFieldsFrom.InnerView).CollectionSource.List.Cast<TestMethod>().Select(i => new Guid(i.Oid.ToString())).ToList())
                                {
                                    qctypeinfo.lstQCType.Add(objqcselall);
                                    gridlisteditor.Grid.Selection.SelectRowByKey(objqcselall);
                                }
                                    ((ListView)View).Refresh();
                                qctypeinfo.chkselectall = "selectedall";
                            }
                            else if (parameter == "UNSelectAll")
                            {
                                foreach (Guid objqcunselall in ((ListView)objCopyFieldsFrom.InnerView).CollectionSource.List.Cast<TestMethod>().Select(i => new Guid(i.Oid.ToString())).ToList())
                                {
                                    qctypeinfo.lstQCType.Remove(objqcunselall);
                                    gridlisteditor.Grid.Selection.UnselectRowByKey(objqcunselall);
                                }
                                qctypeinfo.chkselectall = null;
                            }
                            else
                            {
                                //string[] arrParams = parameter.Split('|');
                                if (arrParams[0] == "Selected")
                                {
                                    //qctypeinfo.lstQCType.Clear();
                                    TestMethod objqcsel = objCopyFieldsFrom.InnerView.ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                                    if (objqcsel != null)
                                    {
                                        qctypeinfo.lstQCType.Add(objqcsel.Oid);
                                    }
                                }
                                else if (arrParams[0] == "UNSelected")
                                {
                                    TestMethod objqcunsel = objCopyFieldsFrom.InnerView.ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                                    if (objqcunsel != null)
                                    {
                                        qctypeinfo.lstQCType.Remove(objqcunsel.Oid);
                                        gridlisteditor.Grid.Selection.UnselectRowByKey(objqcunsel.Oid);
                                    }
                                }
                            }

                            if (qctypeinfo.lstQCType != null && qctypeinfo.lstQCType.Count > 0)
                            {
                                CriteriaOperator criteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", qctypeinfo.lstQCType.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")";
                                IList<TestMethod> lstoctypeoid = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", qctypeinfo.lstQCType.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")"));
                                if (lstoctypeoid != null && lstoctypeoid.Count > 0)
                                {
                                    foreach (TestMethod oid in lstoctypeoid.ToList())
                                    {
                                        gridlisteditor.Grid.Selection.SelectRowByKey(oid.Oid);
                                    }
                                }
                                ((ListView)View).Refresh();
                            }
                        }

                        //if (View.Id == "TestMethod_QCTypes_ListView_CopyTo")
                        //{
                        //    if (qctypeinfo.lstQCType != null)
                        //    {
                        //        ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        //        foreach (Guid obj in qctypeinfo.lstQCType)
                        //        {
                        //            gridlisteditor.Grid.Selection.SelectRow(obj);
                        //        //}
                        //    }
                        //}
                    }
                    else if (View.Id == "TestMethod_QCTypes_ListView_CopyTo")
                    {
                        string[] arrParams = parameter.Split('|');
                        IObjectSpace space = Application.CreateObjectSpace();
                        if (qctypeinfo.lstQCType == null)
                        {
                            qctypeinfo.lstQCType = new List<Guid>();
                        }
                        if (parameter == "SelectAll" && string.IsNullOrEmpty(qctypeinfo.chkselectall))
                        {
                            foreach (Guid objqcselall in ((ListView)View).CollectionSource.List.Cast<QCType>().Select(i => new Guid(i.Oid.ToString())).ToList())
                            {
                                qctypeinfo.lstQCType.Add(objqcselall);
                            }
                            qctypeinfo.chkselectall = "selectedall";
                        }
                        else if (parameter == "UNSelectAll")
                        {
                            foreach (Guid objqcunselall in ((ListView)View).CollectionSource.List.Cast<QCType>().Select(i => new Guid(i.Oid.ToString())).ToList())
                            {
                                qctypeinfo.lstQCType.Remove(objqcunselall);
                            }
                            qctypeinfo.chkselectall = null;
                        }
                        else
                        {
                            //string[] arrParams = parameter.Split('|');
                            if (arrParams[0] == "Selected")
                            {
                                //qctypeinfo.lstQCType.Clear();
                                QCType objqcsel = View.ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                                if (objqcsel != null)
                                {
                                    qctypeinfo.lstQCType.Add(objqcsel.Oid);
                                }
                            }
                            else if (arrParams[0] == "UNSelected")
                            {
                                QCType objqcunsel = View.ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                                if (objqcunsel != null)
                                {
                                    qctypeinfo.lstQCType.Remove(objqcunsel.Oid);
                                }
                            }
                        }
                    }
                    //else if (View.Id == "TestPriceDetail_ListView_Copy_perparameter")
                    //{
                    //    string[] arrParams = parameter.Split('|');
                    //    IObjectSpace space = Application.CreateObjectSpace();
                    //    if (testpriceinfo.lstRemovedperparameter == null)
                    //    {
                    //        testpriceinfo.lstRemovedperparameter = new List<Guid>();
                    //    }

                    //    NestedFrame nestedFrame = (NestedFrame)Frame;
                    //    CompositeView view = nestedFrame.ViewItem.View;
                    //    //DashboardViewItem objCopyFieldsFrom = (DashboardViewItem)((DetailView)view).FindItem("copytestto");
                    //    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //    //DashboardViewItem objProjectList = (DashboardViewItem)((DetailView)view).FindItem("ProjectList");
                    //    //if (objCopyFieldsFrom != null && objCopyFieldsFrom.InnerView != null && objCopyFieldsFrom.InnerView is ListView)
                    //    {
                    //        if (parameter == "SelectAll" && string.IsNullOrEmpty(qctypeinfo.chkselectall))
                    //        {
                    //            foreach (Guid objptselall in ((ListView)View).CollectionSource.List.Cast<TestPriceDetail>().Select(i => new Guid(i.Oid.ToString())).ToList())
                    //            {
                    //                testpriceinfo.lstRemovedperparameter.Add(objptselall);
                    //                gridlisteditor.Grid.Selection.SelectRowByKey(objptselall);
                    //            }
                    //                ((ListView)View).Refresh();
                    //            qctypeinfo.chkselectall = "selectedall";
                    //        }
                    //        else if (parameter == "UNSelectAll")
                    //        {
                    //            foreach (Guid objptunselall in ((ListView)View).CollectionSource.List.Cast<TestPriceDetail>().Select(i => new Guid(i.Oid.ToString())).ToList())
                    //            {
                    //                testpriceinfo.lstRemovedperparameter.Remove(objptunselall);
                    //                gridlisteditor.Grid.Selection.UnselectRowByKey(objptunselall);
                    //            }
                    //            qctypeinfo.chkselectall = null;
                    //        }
                    //        else
                    //        {
                    //            //string[] arrParams = parameter.Split('|');
                    //            if (arrParams[0] == "Selected")
                    //            {
                    //                TestPriceDetail objqcsel = View.ObjectSpace.FindObject<TestPriceDetail>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                    //                if (objqcsel != null)
                    //                {
                    //                    testpriceinfo.lstRemovedperparameter.Add(objqcsel.Oid);
                    //                }
                    //            }
                    //            else if (arrParams[0] == "UNSelected")
                    //            {
                    //                TestPriceDetail objptunsel = View.ObjectSpace.FindObject<TestPriceDetail>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                    //                if (objptunsel != null)
                    //                {
                    //                    testpriceinfo.lstRemovedperparameter.Remove(objptunsel.Oid);
                    //                    gridlisteditor.Grid.Selection.UnselectRowByKey(objptunsel.Oid);
                    //                }
                    //            }
                    //        }

                    //        //if (testpriceinfo.lstRemovedperparameter != null && testpriceinfo.lstRemovedperparameter.Count > 0)
                    //        //{
                    //        //    CriteriaOperator criteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", testpriceinfo.lstRemovedperparameter.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")";
                    //        //    IList<TestPriceDetail> lstoctypeoid = ObjectSpace.GetObjects<TestPriceDetail>(CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", testpriceinfo.lstRemovedperparameter.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")"));
                    //        //    if (lstoctypeoid != null && lstoctypeoid.Count > 0)
                    //        //    {
                    //        //        foreach (TestPriceDetail oid in lstoctypeoid.ToList())
                    //        //        {
                    //        //            gridlisteditor.Grid.Selection.SelectRowByKey(oid.Oid);
                    //        //        }
                    //        //    }
                    //        //    ((ListView)View).Refresh();
                    //        //}
                    //    }
                    //}
                    //else if (View.Id == "TestPriceDetail_ListView_Copy_pertest")
                    //{
                    //    string[] arrParams = parameter.Split('|');
                    //    IObjectSpace space = Application.CreateObjectSpace();
                    //    if (testpriceinfo.lstremovepertest == null)
                    //    {
                    //        testpriceinfo.lstremovepertest = new List<Guid>();
                    //    }

                    //    NestedFrame nestedFrame = (NestedFrame)Frame;
                    //    CompositeView view = nestedFrame.ViewItem.View;
                    //    //DashboardViewItem objCopyFieldsFrom = (DashboardViewItem)((DetailView)view).FindItem("copytestto");
                    //    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //    //DashboardViewItem objProjectList = (DashboardViewItem)((DetailView)view).FindItem("ProjectList");
                    //    //if (objCopyFieldsFrom != null && objCopyFieldsFrom.InnerView != null && objCopyFieldsFrom.InnerView is ListView)
                    //    {
                    //        if (parameter == "SelectAll" && string.IsNullOrEmpty(qctypeinfo.chkselectall))
                    //        {
                    //            foreach (Guid objptselall in ((ListView)View).CollectionSource.List.Cast<TestPriceDetail>().Select(i => new Guid(i.Oid.ToString())).ToList())
                    //            {
                    //                testpriceinfo.lstremovepertest.Add(objptselall);
                    //                gridlisteditor.Grid.Selection.SelectRowByKey(objptselall);
                    //            }
                    //                ((ListView)View).Refresh();
                    //            qctypeinfo.chkselectall = "selectedall";
                    //        }
                    //        else if (parameter == "UNSelectAll")
                    //        {
                    //            foreach (Guid objptunselall in ((ListView)View).CollectionSource.List.Cast<TestPriceDetail>().Select(i => new Guid(i.Oid.ToString())).ToList())
                    //            {
                    //                testpriceinfo.lstremovepertest.Remove(objptunselall);
                    //                gridlisteditor.Grid.Selection.UnselectRowByKey(objptunselall);
                    //            }
                    //            qctypeinfo.chkselectall = null;
                    //        }
                    //        else
                    //        {
                    //            //string[] arrParams = parameter.Split('|');
                    //            if (arrParams[0] == "Selected")
                    //            {
                    //                TestPriceDetail objqcsel = View.ObjectSpace.FindObject<TestPriceDetail>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                    //                if (objqcsel != null)
                    //                {
                    //                    testpriceinfo.lstremovepertest.Add(objqcsel.Oid);
                    //                }
                    //            }
                    //            else if (arrParams[0] == "UNSelected")
                    //            {
                    //                TestPriceDetail objptunsel = View.ObjectSpace.FindObject<TestPriceDetail>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                    //                if (objptunsel != null)
                    //                {
                    //                    testpriceinfo.lstremovepertest.Remove(objptunsel.Oid);
                    //                    gridlisteditor.Grid.Selection.UnselectRowByKey(objptunsel.Oid);
                    //                }
                    //            }
                    //        }

                    //        if (testpriceinfo.lstremovepertest != null && testpriceinfo.lstremovepertest.Count > 0)
                    //        {
                    //            CriteriaOperator criteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", testpriceinfo.lstremovepertest.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")";
                    //            IList<TestPriceDetail> lstoctypeoid = ObjectSpace.GetObjects<TestPriceDetail>(CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", testpriceinfo.lstremovepertest.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")"));
                    //            if (lstoctypeoid != null && lstoctypeoid.Count > 0)
                    //            {
                    //                foreach (TestPriceDetail oid in lstoctypeoid.ToList())
                    //                {
                    //                    gridlisteditor.Grid.Selection.SelectRowByKey(oid.Oid);
                    //                }
                    //            }
                    //            ((ListView)View).Refresh();
                    //        }
                    //    }
                    //}
                    else if (View.Id == "Testparameter_ListView_Testmethod_IsGroup_parameter_popup")
                    {
                        isprocessact = true;
                        if (grptstinfo.lstgrouptestparameter == null)
                        {
                            grptstinfo.lstgrouptestparameter = new List<Guid>();
                        }
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        string[] arrParams = parameter.Split('|');
                        if (parameter == "Selectall")
                        {
                            grptstinfo.lstgrouptestparameter.Clear();
                            for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                            {
                                editor.Grid.Selection.SelectRow(i);
                            }
                            //foreach (Testparameter tstpara in ((ListView)View).CollectionSource.List.Cast<Testparameter>().ToList())
                            //{                                
                            //    editor.Grid.Selection.SelectRowByKey(tstpara.Oid);
                            //}
                        }
                        else if (parameter == "UNSelectall")
                        {
                            grptstinfo.lstgrouptestparameter.Clear();
                            //foreach (Testparameter tstpara in ((ListView)View).CollectionSource.List.Cast<Testparameter>().ToList())
                            //{
                            //    editor.Grid.Selection.UnselectRowByKey(tstpara.Oid);
                            //}
                            for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                            {
                                editor.Grid.Selection.UnselectRow(i);
                            }
                        }
                        else
                        {
                            string[] splparm = parameter.Split('|');
                            if (splparm[0] == "Selected")
                            {
                                Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                if (objtp != null)
                                {
                                    editor.Grid.Selection.SelectRowByKey(objtp.Oid);
                                    if (!grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                    {
                                        grptstinfo.lstgrouptestparameter.Add(objtp.Oid);
                                    }
                                }
                                ////Testparameter exsobjtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                ////if(exsobjtp != null && !string.IsNullOrEmpty(exsobjtp.GroupTestParameters))
                                ////{
                                ////    string[] strparaarr = exsobjtp.GroupTestParameters.Split(';');
                                ////    if (strparaarr != null && strparaarr.Length > 0)
                                ////    {
                                ////        foreach (string strparaoid in strparaarr)
                                ////        {
                                ////            if (!grptstinfo.lstgrouptestparameter.Contains(new Guid(strparaoid)))
                                ////            {
                                ////                grptstinfo.lstgrouptestparameter.Add(new Guid(strparaoid));
                                ////            }
                                ////        }
                                ////    }
                                ////    Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                ////    if (objtp != null)
                                ////    {
                                ////        editor.Grid.Selection.SelectRowByKey(objtp.Oid);
                                ////        if (!grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                ////        {
                                ////            grptstinfo.lstgrouptestparameter.Add(objtp.Oid);
                                ////        }
                                ////    }
                                ////}
                                ////else if (exsobjtp != null && string.IsNullOrEmpty(exsobjtp.GroupTestParameters))
                                ////{
                                ////    Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                ////    if (objtp != null)
                                ////    {
                                ////        editor.Grid.Selection.SelectRowByKey(objtp.Oid);
                                ////        if (!grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                ////        {
                                ////            grptstinfo.lstgrouptestparameter.Add(objtp.Oid);
                                ////        }
                                ////    }
                                ////}
                                ////else
                                ////{
                                ////    GroupTestMethod exsgrptp = os.FindObject<GroupTestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                ////    if (exsgrptp != null && !string.IsNullOrEmpty(exsgrptp.GroupTestParameters))
                                ////    {
                                ////        string[] strparaarr = exsgrptp.GroupTestParameters.Split(';');
                                ////        if (strparaarr != null && strparaarr.Length > 0)
                                ////        {
                                ////            foreach (string strparaoid in strparaarr)
                                ////            {
                                ////                if (!grptstinfo.lstgrouptestparameter.Contains(new Guid(strparaoid)))
                                ////                {
                                ////                    grptstinfo.lstgrouptestparameter.Add(new Guid(strparaoid));
                                ////                }
                                ////            }
                                ////        }
                                ////        Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                ////        if (objtp != null)
                                ////        {
                                ////            editor.Grid.Selection.SelectRowByKey(objtp.Oid);
                                ////            if (!grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                ////            {
                                ////                grptstinfo.lstgrouptestparameter.Add(objtp.Oid);
                                ////            }
                                ////        }
                                ////    }
                                ////    else if (exsgrptp != null && string.IsNullOrEmpty(exsgrptp.GroupTestParameters))
                                ////    {
                                ////        Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                ////        if (objtp != null)
                                ////        {
                                ////            editor.Grid.Selection.SelectRowByKey(objtp.Oid);
                                ////            if (!grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                ////            {
                                ////                grptstinfo.lstgrouptestparameter.Add(objtp.Oid);
                                ////            }
                                ////        }
                                ////    }
                                ////}
                            }
                            else if (splparm[0] == "UNSelected")
                            {
                                Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                if (objtp != null)
                                {
                                    editor.Grid.Selection.UnselectRowByKey(objtp.Oid);
                                    if (grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                    {
                                        grptstinfo.lstgrouptestparameter.Remove(objtp.Oid);
                                    }
                                }
                                ////Testparameter exsobjtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                ////if (exsobjtp != null && !string.IsNullOrEmpty(exsobjtp.GroupTestParameters))
                                ////{
                                ////    string[] strparaarr = exsobjtp.GroupTestParameters.Split(';');
                                ////    if (strparaarr != null && strparaarr.Length > 0)
                                ////    {
                                ////        foreach (string strparaoid in strparaarr)
                                ////        {
                                ////            if (!grptstinfo.lstgrouptestparameter.Contains(new Guid(strparaoid)))
                                ////            {
                                ////                grptstinfo.lstgrouptestparameter.Add(new Guid(strparaoid));
                                ////            }
                                ////        }
                                ////    }
                                ////    Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                ////    if (objtp != null)
                                ////    {
                                ////        editor.Grid.Selection.UnselectRowByKey(objtp.Oid);
                                ////        if (grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                ////        {
                                ////            grptstinfo.lstgrouptestparameter.Remove(objtp.Oid);
                                ////        }
                                ////    }
                                ////}
                                ////else if (exsobjtp != null && string.IsNullOrEmpty(exsobjtp.GroupTestParameters))
                                ////{
                                ////    Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                ////    if (objtp != null)
                                ////    {
                                ////        editor.Grid.Selection.UnselectRowByKey(objtp.Oid);
                                ////        if (grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                ////        {
                                ////            grptstinfo.lstgrouptestparameter.Remove(objtp.Oid);
                                ////        }
                                ////    }
                                ////}
                                ////else
                                ////{
                                ////    IObjectSpace os = Application.CreateObjectSpace();
                                ////    GroupTestMethod exsgrptp = os.FindObject<GroupTestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                ////    if (exsgrptp != null && !string.IsNullOrEmpty(exsgrptp.GroupTestParameters))
                                ////    {
                                ////        string[] strparaarr = exsgrptp.GroupTestParameters.Split(';');
                                ////        if (strparaarr != null && strparaarr.Length > 0)
                                ////        {
                                ////            foreach (string strparaoid in strparaarr)
                                ////            {
                                ////                if (!grptstinfo.lstgrouptestparameter.Contains(new Guid(strparaoid)))
                                ////                {
                                ////                    grptstinfo.lstgrouptestparameter.Add(new Guid(strparaoid));
                                ////                }
                                ////            }
                                ////        }
                                ////        Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                ////        if (objtp != null)
                                ////        {
                                ////            editor.Grid.Selection.UnselectRowByKey(objtp.Oid);
                                ////            if (grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                ////            {
                                ////                grptstinfo.lstgrouptestparameter.Remove(objtp.Oid);
                                ////            }
                                ////        }
                                ////    }
                                ////    else if (exsgrptp != null && string.IsNullOrEmpty(exsgrptp.GroupTestParameters))
                                ////    {
                                ////        Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                ////        if (objtp != null)
                                ////        {
                                ////            editor.Grid.Selection.UnselectRowByKey(objtp.Oid);
                                ////            if (grptstinfo.lstgrouptestparameter.Contains(objtp.Oid))
                                ////            {
                                ////                grptstinfo.lstgrouptestparameter.Remove(objtp.Oid);
                                ////            }
                                ////        }
                                ////    }
                                ////}
                            }
                        }
                    }
                    else if (View.Id == "Testparameter_ListView_Testmethod_IsGroup")
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        string[] arrParams = parameter.Split('|');
                        if (arrParams[0] == "GroupTest")
                        {
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(arrParams[1]), "Oid");

                            CollectionSource cs = new CollectionSource(objectSpace, typeof(Testparameter));
                            Testparameter objtp = objectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objtp != null && objtp.TestMethod != null)
                            {
                                List<Guid> paraoid = new List<Guid>();
                                IList<Testparameter> lsttestpara = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid]=? And [QCType.QCTypeName]='Sample' And [Component.Oid] = ?", objtp.TestMethod.Oid, objtp.Component.Oid));
                                foreach (Testparameter objtstpara in lsttestpara.ToList())
                                {
                                    paraoid.Add(objtstpara.Oid);
                                }
                                if (paraoid.Count > 0)
                                {
                                    cs.Criteria["filter"] = new InOperator("Oid", paraoid);
                                }
                            }
                            ListView lvtestparar = Application.CreateListView("Testparameter_ListView_Testmethod_IsGroup_parameter_popup", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lvtestparar);
                            showViewParameters.CreatedView = lvtestparar;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Dcparameterpopup_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }

                    }
                    else if (View.Id == "GroupTestMethod_ListView_IsGroup")
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        string[] arrParams = parameter.Split('|');
                        if (arrParams[0] == "GroupTest")
                        {
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(arrParams[1]), "Oid");
                            CollectionSource cs = new CollectionSource(objectSpace, typeof(Testparameter));
                            GroupTestMethod objgrptp = View.ObjectSpace.FindObject<GroupTestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objgrptp != null && objgrptp.Tests != null)
                            {
                                List<Guid> paraoid = new List<Guid>();
                                IList<Testparameter> lsttestpara = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid]=? And [QCType.QCTypeName]='Sample' And [Component.Oid] = ?", objgrptp.Tests.Oid, objgrptp.TestParameter.Component.Oid));
                                foreach (Testparameter objtstpara in lsttestpara.ToList())
                                {
                                    paraoid.Add(objtstpara.Oid);
                                }
                                if (paraoid.Count > 0)
                                {
                                    cs.Criteria["filter"] = new InOperator("Oid", paraoid);
                                }
                            }
                            ListView lvtestparar = Application.CreateListView("Testparameter_ListView_Testmethod_IsGroup_parameter_popup", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lvtestparar);
                            showViewParameters.CreatedView = lvtestparar;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Dcparameterpopup_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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

        private void Dc_Accepting1(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                string lacc = string.Empty;
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    if (objtestparametrinfo.lstaccridation != null && objtestparametrinfo.lstaccridation.Count > 0)
                    {
                        objtestparametrinfo.lstaccridation.Clear();
                    }
                    objtestparametrinfo.lstaccridation = e.AcceptActionArgs.SelectedObjects.Cast<Accrediation>().Where(i => i.lAccrediation != null).Select(i => i.lAccrediation).ToList();

                    foreach (string stracc in objtestparametrinfo.lstaccridation.ToList())
                    {
                        Accrediation objaccrediation = ObjectSpace.FindObject<Accrediation>(CriteriaOperator.Parse("[lAccrediation] = ?", stracc));
                        if (objaccrediation != null)
                        {
                            if (string.IsNullOrEmpty(lacc))
                            {
                                lacc = objaccrediation.lAccrediation.ToString();
                            }
                            else if (!string.IsNullOrEmpty(lacc))
                            {
                                lacc = lacc + "; " + objaccrediation.lAccrediation.ToString();
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(lacc))
                    {
                        Testparameter objtestpara = View.ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objtestpara != null)
                        {
                            objtestpara.lAccrediation = lacc;
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select atleast one checkbox.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dcparameterpopup_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();

                if (View.Id == "Testparameter_ListView_Testmethod_IsGroup")
                {
                    if (sender != null)
                    {
                        string strpara = string.Empty;
                        DialogController dc = sender as DialogController;
                        if (dc != null && dc.Window != null && dc.Window.View != null && dc.Window.View.Id == "Testparameter_ListView_Testmethod_IsGroup_parameter_popup" && dc.Window.View.SelectedObjects.Count > 0)
                        {
                            int rowcnt = 0;
                            ListView lv = dc.Window.View as ListView;
                            ASPxGridListEditor gridlist = ((ListView)lv).Editor as ASPxGridListEditor;
                            if (gridlist != null && gridlist.Grid != null)
                            {
                                rowcnt = gridlist.Grid.VisibleRowCount;
                            }
                            if (dc.Window.View.SelectedObjects.Count != rowcnt)
                            {
                                foreach (Testparameter objtpsel in dc.Window.View.SelectedObjects)
                                {
                                    if (string.IsNullOrEmpty(strpara))
                                    {
                                        strpara = objtpsel.Oid.ToString();
                                    }
                                    else
                                    {
                                        strpara = strpara + "; " + objtpsel.Oid.ToString();
                                    }
                                }
                            }
                        }
                        Testparameter objtp = os.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objtp != null)
                        {
                            objtp.GroupTestParameters = strpara;
                            os.CommitChanges();
                            View.ObjectSpace.Refresh();
                        }
                    }
                }
                else if (View.Id == "GroupTestMethod_ListView_IsGroup")
                {
                    if (sender != null)
                    {
                        string strpara = string.Empty;
                        DialogController dc = sender as DialogController;
                        if (dc != null && dc.Window != null && dc.Window.View != null && dc.Window.View.Id == "Testparameter_ListView_Testmethod_IsGroup_parameter_popup" && dc.Window.View.SelectedObjects.Count > 0)
                        {
                            int rowcnt = 0;
                            ListView lv = dc.Window.View as ListView;
                            ASPxGridListEditor gridlist = ((ListView)lv).Editor as ASPxGridListEditor;
                            if (gridlist != null && gridlist.Grid != null)
                            {
                                rowcnt = gridlist.Grid.VisibleRowCount;
                            }
                            if (dc.Window.View.SelectedObjects.Count != rowcnt)
                            {
                                foreach (Testparameter objtpsel in dc.Window.View.SelectedObjects)
                                {
                                    if (string.IsNullOrEmpty(strpara))
                                    {
                                        strpara = objtpsel.Oid.ToString();
                                    }
                                    else
                                    {
                                        strpara = strpara + "; " + objtpsel.Oid.ToString();
                                    }
                                }
                            }
                        }
                        GroupTestMethod objgrptp = os.FindObject<GroupTestMethod>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        Testparameter objtp = os.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", objgrptp.TestParameter.Oid));
                        if (objgrptp != null)
                        {
                            objgrptp.GroupTestParameters = strpara;
                            if (string.IsNullOrEmpty(objgrptp.GroupTestParameters))
                            {
                                objgrptp.GroupTestParameter = "Default";
                            }
                            else if (!string.IsNullOrEmpty(objgrptp.GroupTestParameters))
                            {
                                objgrptp.GroupTestParameter = "Customized";
                            }
                            if (objtp != null)
                            {
                                objtp.GroupTestParameters = strpara;
                                if (string.IsNullOrEmpty(objtp.GroupTestParameters))
                                {
                                    objtp.GroupTestParameter = "Default";
                                }
                                else if (!string.IsNullOrEmpty(objtp.GroupTestParameters))
                                {
                                    objtp.GroupTestParameter = "Customized";
                                }
                            }
                            os.CommitChanges();
                            View.ObjectSpace.Refresh();
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
                e.Properties["cpEndCallbackHandlers"] = qctypeinfo.chkselectall;
                if (Application.MainWindow.View is DetailView)
                {
                    if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                    {
                        e.Properties["cpAllowbatchEdit"] = true;
                    }
                    else
                    {
                        e.Properties["cpAllowbatchEdit"] = false;
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
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");
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
            //throw new NotImplementedException();
        }
        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View != null && View.Id == "PrepMethod_DetailView")
                {
                    testInfo.methodguid = new Guid();
                    View.Closing -= View_Closing;
                }
                if (View.Id == "TestPrice_DetailView")
                {
                    //tatinfo.lsttat = null;
                }
                if (View.Id == "TestPriceDetail_ListView_Copy_pertest")
                {
                    if (testpriceinfo.lstremovepertest != null && testpriceinfo.lstremovepertest.Count > 0)
                    {
                        testpriceinfo.lstremovepertest.Clear();
                    }
                }
                if (View.Id == "TestPrice_ListView_Copy_perparameter")
                {
                    if (testpriceinfo.lstRemovedperparameter != null && testpriceinfo.lstRemovedperparameter.Count > 0)
                    {
                        testpriceinfo.lstRemovedperparameter.Clear();
                    }
                }
                else if (View.Id == "ResultDefaultValue_LookupListView")
                {
                    View.ControlsCreated -= View_ControlsCreated;
                }
                else if (View.Id == "TestMethod_PrepMethods_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                }
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                ObjectSpace.Committed -= ObjectSpace_Committed;
                if (View is ListView)
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                else if (View is DetailView)
                {
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }

                if (View.Id == "TestMethod_DetailView")
                {
                    testInfo.isgroup = false;
                    testInfo.isTestsave = false;
                    isprocessact = false;
                    if (grptstinfo.lsttempgrouptest != null)
                    {
                        grptstinfo.lsttempgrouptest.Clear();
                    }
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    qctypeinfo.objtmQCType = null;
                    if (CopyParameters != null && CopyParameters.Items != null && CopyParameters.Items.Count > 0)
                    {
                        CopyParameters.SelectedItemChanged -= CopyParameters_SelectedItemChanged;
                    }
                    Frame.GetController<RefreshController>().RefreshAction.Executing -= RefreshAction_Executing;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    mdcSave = Frame.GetController<ModificationsController>();
                    mdcSave.SaveAction.Executing -= SaveAction_Executing;
                    mdcSaveClose = Frame.GetController<ModificationsController>();
                    mdcSaveClose.SaveAndCloseAction.Executing -= SaveAction_Executing;
                    mdcSaveNew = Frame.GetController<ModificationsController>();
                    mdcSaveNew.SaveAndNewAction.Executing -= SaveAction_Executing;
                    View.QueryCanClose -= View_QueryCanClose;
                    CopyTest.Executing -= CopyTest_Executing;
                    TestMethod Crtobjtm = (TestMethod)View.CurrentObject;
                    IObjectSpace os = Application.CreateObjectSpace();
                    DashboardViewItem dvsamplepara = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                    DashboardViewItem dvQCSamplePara = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                    DashboardViewItem dvInternalStand = ((DetailView)View).FindItem("InternalStandards") as DashboardViewItem;
                    DashboardViewItem dvComponents = ((DetailView)View).FindItem("Components") as DashboardViewItem;
                    if (dvsamplepara != null)
                    {
                        IList<Testparameter> lstsamplepara = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And IsNullOrEmpty([Parameter.ParameterName]) And [QCType.QCTypeName] = 'Sample'", Crtobjtm.Oid));
                        if (lstsamplepara != null && lstsamplepara.Count > 0)
                        {
                            foreach (Testparameter objtp in lstsamplepara.ToList())
                            {
                                if (objtp.IsGroup != true)
                                {
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objtp.Oid + "'");
                                    SampleParameter objTP = os.FindObject<SampleParameter>(criteria1);
                                    if (objTP == null)
                                    {
                                        os.Delete(objtp);
                                    }
                                }
                            }
                            os.CommitChanges();
                        }
                    }
                    if (dvQCSamplePara != null)
                    {
                        IList<Testparameter> lstQCSamplePara = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And IsNullOrEmpty([Parameter.ParameterName])", Crtobjtm.Oid));
                        if (lstQCSamplePara != null && lstQCSamplePara.Count > 0)
                        {
                            foreach (Testparameter objtp in lstQCSamplePara.ToList())
                            {
                                if (objtp.IsGroup != true)
                                {
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objtp.Oid + "'");
                                    SampleParameter objTP = os.FindObject<SampleParameter>(criteria1);
                                    if (objTP == null)
                                    {
                                        os.Delete(objtp);
                                    }
                                }
                            }
                            os.CommitChanges();
                        }
                    }
                    if (dvInternalStand != null)
                    {
                        IList<Testparameter> lstInternalStand = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And IsNullOrEmpty([Parameter.ParameterName]) And IsNullOrEmpty([QCType.QCTypeName])", Crtobjtm.Oid));
                        if (lstInternalStand != null && lstInternalStand.Count > 0)
                        {
                            foreach (Testparameter objtp in lstInternalStand.ToList())
                            {
                                if (objtp.IsGroup != true)
                                {
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objtp.Oid + "'");
                                    SampleParameter objTP = os.FindObject<SampleParameter>(criteria1);
                                    if (objTP == null)
                                    {
                                        os.Delete(objtp);
                                    }
                                }
                            }
                            os.CommitChanges();
                        }
                    }
                    if (dvComponents != null)
                    {
                        IList<Component> lstComponents = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And IsNullOrEmpty([Components]) ", Crtobjtm.Oid));
                        if (lstComponents != null && lstComponents.Count > 0)
                        {
                            foreach (Component objtp in lstComponents.ToList())
                            {
                                os.Delete(os.GetObject(objtp));
                            }
                            os.CommitChanges();
                        }
                    }
                }
                else
                if (View.Id == "Testparameter_DetailView_CopyTest")
                {
                    testInfo.IsCopyTestAction = false;
                    qctypeinfo.lstcopytest = null;
                    qctypeinfo.chkselectall = null;
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("CopyTest") as DashboardViewItem;
                    if (viAvailableFields != null)
                    {
                        viAvailableFields.ControlCreated -= ViAvailableFields_ControlCreated;
                        ListView lstvi = viAvailableFields.InnerView as ListView;
                        lstvi.CollectionSource.List.Clear();
                    }
                }
                else
                if (View.Id == "TestMethod_DetailView_CopyParameter" || View.Id == "Testparameter_DetailView_Copyparameters")
                {
                    qctypeinfo.lstQCType = null;
                    qctypeinfo.chkselectall = null;
                    //CopyParameters.SelectedItem.Id = "Empty";
                }
                else
                if (View.Id == "TestMethod_DetailView_CopyTest")
                {
                    qctypeinfo.lstQCType = null;
                    qctypeinfo.chkselectall = null;
                }
                else
                if (View.Id == "TestMethod")
                {
                    testInfo.IsCopyTestAction = false;
                    Application.DetailViewCreating += Application_DetailViewCreating;
                    DashboardViewItem viAvailableFields = ((DashboardView)View).FindItem("CopyTestSettings") as DashboardViewItem;
                    if (viAvailableFields != null)
                    {
                        viAvailableFields.ControlCreated += ViAvailableFields_ControlCreated;
                    }
                }
                else
                if (View.Id == "TestMethod_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                }
                else if (View.Id == "ResultDefaultValue_LookupListView")
                {
                    View.ControlsCreated -= View_ControlsCreated;
                }
                else if (View.Id == "Component_DetailView_Test")
                {
                    View.Closed -= View_ClosedInComponent;
                }
                else
                if (View.Id == "TestMethod_QCTypes_ListView")
                {
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Executed -= UnlinkAction_Executed;
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing -= UnlinkAction_Executing;
                }
                if (View.Id == "Testparameter_ListView_Testmethod_IsGroup_parameter_popup")
                {
                    if (grptstinfo.lstgrouptestparameter != null)
                    {
                        grptstinfo.lstgrouptestparameter.Clear();
                    }
                }
                if (View.Id == "TestMethod_DetailView_CopyTest" || View.Id == "Testparameter_DetailView_CopyTest" || View.Id == "Testparameter_DetailView_Copyparameters")
                {
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated -= ChangeLayoutGroupCaptionViewController_ItemCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void SaveTestAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_QueryCanClose(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_DetailView" && View.CurrentObject != null)
                {
                    TestMethod objtm = (TestMethod)View.CurrentObject;
                    if (objtm != null && objtm.IsGroup == true)
                    {
                        if (testInfo.IsCancel == true)
                        {
                            DashboardViewItem dvtestmethod = ((DetailView)View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                            if (dvtestmethod != null && dvtestmethod.InnerView != null && ((ListView)dvtestmethod.InnerView).CollectionSource.List.Count == 0)
                            {
                                ObjectSpace.Delete(objtm);
                            }
                            else
                            {
                                ((ListView)dvtestmethod.InnerView).ObjectSpace.CommitChanges();
                            }
                        }
                        if (testInfo.IsCancel == false)
                        {
                            DashboardViewItem dvtestmethod = ((DetailView)View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                            if (dvtestmethod != null && dvtestmethod.InnerView != null && ((ListView)dvtestmethod.InnerView).CollectionSource.List.Count == 0)
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage("Select at least one Testmethod.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                    else if (objtm != null && objtm.IsGroup == false)
                    {
                        if (testInfo.IsCancel == true)
                        {
                            DashboardViewItem dvSampleparam = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                            if (dvSampleparam != null && dvSampleparam.InnerView == null)
                            {
                                dvSampleparam.CreateControl();
                                dvSampleparam.InnerView.CreateControls();
                            }
                            if (dvSampleparam != null && dvSampleparam.InnerView != null && ((ListView)dvSampleparam.InnerView).CollectionSource.List.Count == 0)
                            {
                                IList<Testparameter> lstTestParams = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", ((TestMethod)View.CurrentObject).Oid));
                                foreach (Testparameter param in lstTestParams.ToList())
                                {
                                    ObjectSpace.Delete(param);
                                    ObjectSpace.CommitChanges();
                                }
                                TestMethod objtp = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", (TestMethod)View.CurrentObject));
                                if (objtp != null)
                                {
                                    ObjectSpace.Delete(objtp);
                                    ObjectSpace.CommitChanges();
                                }
                            }
                        }
                        else if (testInfo.IsCancel == false)
                        {
                            string strqctype = "Sample";
                            DashboardViewItem dvSampleParameter = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                            if (dvSampleParameter != null && dvSampleParameter.InnerView == null)
                            {
                                dvSampleParameter.CreateControl();
                                dvSampleParameter.InnerView.CreateControls();
                            }
                            if (dvSampleParameter != null && dvSampleParameter.InnerView != null)
                            {
                                //if (((ListView)dvSampleParameter.InnerView).CollectionSource.GetCount() == 0)
                                //{
                                //    e.Cancel = true;
                                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "AddSampleParameter"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                //    return;
                                //}
                            }
                        }
                    }
                }
                testInfo.IsCancel = false;
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
                if (Application.MainWindow.View.Id == "TestMethod_DetailView")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    TestMethod Crtobjtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (Crtobjtm != null && Crtobjtm.IsGroup == true)
                    {
                        IList<Testparameter> lsttestpara = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample'", Crtobjtm.Oid));
                        if (lsttestpara != null && lsttestpara.Count > 0)
                        {
                            os.Delete(lsttestpara);
                            os.CommitChanges();
                        }
                        DashboardViewItem dvSampleparam = ((DetailView)Application.MainWindow.View).FindItem("Sampleparameter") as DashboardViewItem;
                        if (dvSampleparam != null && dvSampleparam.InnerView == null)
                        {
                            dvSampleparam.CreateControl();
                            dvSampleparam.InnerView.CreateControls();
                        }
                        if (dvSampleparam != null && dvSampleparam.InnerView != null)
                        {
                            ((ListView)dvSampleparam.InnerView).ObjectSpace.Refresh();
                        }
                    }
                    else if (Crtobjtm != null && Crtobjtm.IsGroup == false)
                    {
                        IList<Testparameter> lsttestpara = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [IsGroup] = 'True'", Crtobjtm.Oid));
                        if (lsttestpara != null && lsttestpara.Count > 0)
                        {
                            os.Delete(lsttestpara);
                            os.CommitChanges();
                        }
                        IList<GroupTestMethod> lstgrptest = os.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", Crtobjtm.Oid));
                        if (lstgrptest != null && lstgrptest.Count > 0)
                        {
                            os.Delete(lstgrptest);
                            os.CommitChanges();
                        }
                        DashboardViewItem dvtestgroup = ((DetailView)Application.MainWindow.View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                        if (dvtestgroup != null && dvtestgroup.InnerView == null)
                        {
                            dvtestgroup.CreateControl();
                            dvtestgroup.InnerView.CreateControls();
                        }
                        if (dvtestgroup != null && dvtestgroup.InnerView != null)
                        {
                            ((ListView)dvtestgroup.InnerView).ObjectSpace.Refresh();
                        }
                    }
                }
                if (View != null && View.Id == "TestMethod_DetailView")
                {
                    DashboardViewItem dvtestmethod = ((DetailView)View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                    if (dvtestmethod != null && dvtestmethod.InnerView != null)
                    {
                        foreach (GroupTestMethod objgm in testInfo.delGtestmethod.ToList())
                        {
                            ((ListView)dvtestmethod.InnerView).ObjectSpace.Delete(objgm);
                        }
                        ((ListView)dvtestmethod.InnerView).ObjectSpace.CommitChanges();
                    }
                    if (View.CurrentObject != null)
                    {
                        Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                        IObjectSpace objectspace = Application.CreateObjectSpace();
                        TestMethod objtm = (TestMethod)View.CurrentObject;
                        if (objtm != null)
                        {
                            CriteriaOperator criteria = CriteriaOperator.Parse("[TestMethod]='" + objtm.Oid + "'");
                            IList<QcParameter> objqp = objectspace.GetObjects<QcParameter>(criteria);
                            if (objqp != null)
                            {
                                foreach (QcParameter qp in objqp)
                                {
                                    foreach (Parameter objp in objtm.Parameters)
                                    {
                                        criteria = CriteriaOperator.Parse("QcParameter='" + qp.Oid + "' AND Parameter='" + objp.Oid + "'");
                                        bool exists = Convert.ToBoolean(ObjectSpace.Evaluate(typeof(QCTestParameter), (new AggregateOperand("", Aggregate.Exists)), (criteria)));
                                        if (!exists)
                                        {
                                            QCTestParameter obj = (QCTestParameter)objectspace.CreateObject(typeof(QCTestParameter));
                                            obj.QcParameter = objectspace.GetObjectByKey<QcParameter>(qp.Oid);
                                            obj.Parameter = objectspace.GetObjectByKey<Parameter>(objp.Oid);
                                            objectspace.CommitChanges();
                                        }
                                    }
                                }
                            }
                            Testparameter objtp = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid));
                            if (objtp == null)
                            {
                                Testparameter objtestpara = objectspace.CreateObject<Testparameter>();
                                objtestpara.TestMethod = objectspace.GetObject(objtm);
                                objtestpara.IsGroup = objtm.IsGroup;
                                ////Component objcomp = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                                ////if (objcomp != null)
                                ////{
                                ////    objtestpara.Component = objectspace.GetObject(objcomp);
                                ////}
                                objectspace.CommitChanges();
                            }
                        }
                    }
                }
                chkparametercommitted = true;
                chktestcommitted = true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        void gridView_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                gridView.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                foreach (WebColumnBase column in gridView.Columns)
                {
                    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)GridListEditor).GetColumnInfo(column);
                    if (columnInfo != null)
                    {
                        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                        column.Width = modelColumn.Width;
                    }
                }
                var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                selectionBoxColumn.SelectCheckBoxPosition = GridSelectCheckBoxPosition.Left;
                selectionBoxColumn.FixedStyle = GridViewColumnFixedStyle.Left;
                selectionBoxColumn.VisibleIndex = 0;
                //selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;

                gridView.Columns[12].FixedStyle = GridViewColumnFixedStyle.Left;
                gridView.Columns[12].VisibleIndex = 1;
                gridView.Columns[22].FixedStyle = GridViewColumnFixedStyle.Left;
                gridView.Columns[22].VisibleIndex = 2;

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        //private void LimitSetup_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        //{
        //    try
        //    {
        //        if (View != null && View.Id == "TestMethod_DetailView")
        //        {
        //            if (View.CurrentObject != null)
        //            {
        //                strtmOid = ObjectSpace.GetKeyValueAsString((TestMethod)View.CurrentObject);
        //            }
        //        }               
        //        IObjectSpace objspace = Application.CreateObjectSpace();
        //        object objToShow = objspace.CreateObject(typeof(Testparameter));
        //        if (objToShow != null)
        //        {
        //            CollectionSource cs = new CollectionSource(objspace, typeof(Testparameter));
        //            cs.Criteria.Clear();
        //            cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod]='" + strtmOid + "'");
        //            //ListView CreateListView = Application.CreateListView("Testparameter_LookupListView", cs, true);
        //            ListView CreateListView = Application.CreateListView("Testparameter_ListView_Copy", cs, true);// Testparameter_LookupListView
        //            e.Size = new Size(1200,600);

        //            e.View = CreateListView;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); 
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        #endregion

        private void parameterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_DetailView")
                {
                    if (View.CurrentObject != null)
                    {
                        View.ObjectSpace.CommitChanges();
                        TestMethod currentTest = (TestMethod)View.CurrentObject;
                        if (currentTest != null)
                        {
                            testInfo.CurrentTest = currentTest;
                        }
                    }
                    testInfo.AllSelAvailableTestParam = null;
                    testInfo.CurrentQcType = null;
                    testInfo.ModifiedQCTypes = null;
                    testInfo.IsSaved = false;
                    testInfo.OpenSettings = false;
                    testInfo.NewTestParameters = null;
                    testInfo.RemovedTestParameters = null;
                    testInfo.ExistingTestParameters = null;

                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    TestMethod test = objectSpace.GetObject<TestMethod>((TestMethod)View.CurrentObject);
                    //DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "TestView", false);
                    //DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "TestParamterEdit_Copy", false);
                    DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "TestParamterEdit", false);
                    e.ShowViewParameters.CreatedView = dashboard;
                    e.ShowViewParameters.Context = TemplateContext.NestedFrame;
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.AcceptAction.Active.SetItemValue("disable", false);
                    dc.CancelAction.Active.SetItemValue("disable", false);
                    //dc.CloseAction.Executing += CloseAction_Executing;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.ViewClosed += Dc_ViewClosed;
                    e.ShowViewParameters.Controllers.Add(dc);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CloseAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                WebWindow.CurrentRequestWindow.RegisterClientScript("savechanges", ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager.GetScript("CanCloseTestParameter", "confirm('Saved unsaved changes?','Alert')"));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Dc_ViewClosed(object sender, EventArgs e)
        {
            try
            {
                if (View != null && testInfo.OpenSettings == true)
                {
                    DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "TestParameterSettings", false);
                    Frame.SetView(dashboard);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void LimitSetup_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_DetailView" && View.CurrentObject != null)
                {
                    TestMethod currentTest = (TestMethod)View.CurrentObject;
                    if (currentTest != null)
                    {
                        testInfo.CurrentTest = currentTest;

                        DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "TestParameterSettings", false);
                        Frame.SetView(dashboard);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Copyparameter_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                TestMethod objtm = os.CreateObject<TestMethod>();
                DetailView createdv = Application.CreateDetailView(os, "TestMethod_DetailView_CopyParameter", true, objtm);
                createdv.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showviewparameter = new ShowViewParameters(createdv);
                showviewparameter.Context = TemplateContext.PopupWindow;
                showviewparameter.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += dc_Accepting;
                dc.AcceptAction.Executed += AcceptAction_Executed;
                dc.CloseOnCurrentObjectProcessing = false;
                showviewparameter.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showviewparameter, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dc = (DialogController)sender;
                    if (dc != null)
                    {
                        if (dc.Window.View.Id == "TestMethod_DetailView_CopyParameter")
                        {
                            DashboardViewItem dvi = (DashboardViewItem)((DetailView)dc.Window.View).FindItem("QCTypeto");
                            var lstSelObjects = ((ListView)dvi.InnerView).SelectedObjects;
                            //if (qctypeinfo.lstQCType != null && qctypeinfo.lstQCType.Count > 0)
                            if (((ListView)dvi.InnerView).SelectedObjects.Count > 0)
                            {
                                qctypeinfo.lstQCType = ((ListView)dvi.InnerView).SelectedObjects.Cast<QCType>().Select(i => i.Oid).Distinct().ToList();
                                IObjectSpace os = Application.CreateObjectSpace();
                                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                TestMethod testmethod = (TestMethod)Application.MainWindow.View.CurrentObject;

                                if (testmethod != null)
                                {
                                    //string testmethod = "FMI";
                                    string foid = "Sample";
                                    //string toid = "Blank";
                                    TestMethod objtm = uow.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] = ? And [MatrixName.MatrixName] = ? And [MethodName.Oid] = ?", testmethod.TestName, testmethod.MatrixName.MatrixName, testmethod.MethodName.Oid));
                                    if (objtm != null)
                                    {
                                        List<Testparameter> lstcrttp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objtm.Oid && i.QCType.QCTypeName == foid).ToList();
                                        //IList<Testparameter> lstcrttp = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.QCTypeName] = ? and [TestMethod.Oid] = ?", foid, objtm.Oid));
                                        if (lstcrttp != null && lstcrttp.Count > 0)
                                        {
                                            foreach (Guid objsel in qctypeinfo.lstQCType.ToList())
                                            {
                                                List<Testparameter> lstchktpdel = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objtm.Oid && i.QCType.Oid == objsel).ToList();
                                                //IList<Testparameter> lstchktpdel = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.Oid] = ? and [TestMethod.Oid] = ?", objsel, testmethod.Oid));
                                                if (lstchktpdel.Count > 0)
                                                {
                                                    foreach (Testparameter objchktpdel in lstchktpdel.ToList())
                                                    {
                                                        CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objchktpdel.Oid + "'");
                                                        SampleParameter objTP = uow.FindObject<SampleParameter>(criteria1);
                                                        if (objchktpdel != null && objTP == null)
                                                        {
                                                            uow.Delete(objchktpdel);
                                                            objchktpdel.Save();
                                                        }
                                                    }
                                                    uow.CommitChanges();
                                                }
                                            }
                                            foreach (Testparameter tp in lstcrttp)
                                            {
                                                foreach (Guid obj in qctypeinfo.lstQCType.ToList())
                                                {
                                                    QCType objqctyp = uow.FindObject<QCType>(CriteriaOperator.Parse("[Oid] = ?", obj));
                                                    Testparameter objchktp = uow.FindObject<Testparameter>(CriteriaOperator.Parse("[Parameter.Oid] = ? and [TestMethod.Oid] = ? and [QCType.Oid] = ?", tp.Parameter.Oid, tp.TestMethod.Oid, obj));
                                                    if (objchktp == null)
                                                    {
                                                        Testparameter objtp = new Testparameter(uow);
                                                        objtp.Parameter = tp.Parameter;
                                                        objtp.TestMethod = tp.TestMethod;
                                                        objtp.Sort = tp.Sort;
                                                        objtp.QCType = objqctyp;
                                                        objtp.DefaultResult = tp.DefaultResult;
                                                        objtp.Sort = tp.Sort;
                                                        if (tp.DefaultUnits != null)
                                                        {
                                                            objtp.DefaultUnits = uow.GetObjectByKey<Unit>(tp.DefaultUnits.Oid);
                                                        }
                                                        objtp.FinalDefaultResult = tp.FinalDefaultResult;
                                                        if (tp.FinalDefaultUnits != null)
                                                        {
                                                            objtp.FinalDefaultUnits = uow.GetObjectByKey<Unit>(tp.FinalDefaultUnits.Oid);
                                                        }
                                                        if (tp.Component != null)
                                                        {
                                                            objtp.Component = uow.GetObjectByKey<Component>(tp.Component.Oid);
                                                        }
                                                        objtp.RptLimit = tp.RptLimit;
                                                        objtp.MDL = tp.MDL;
                                                        objtp.MCL = tp.MCL;
                                                        objtp.UQL = tp.UQL;
                                                        objtp.LOQ = tp.LOQ;
                                                        objtp.SigFig = tp.SigFig;
                                                        objtp.CutOff = tp.CutOff;
                                                        objtp.Decimal = tp.Decimal;
                                                        objtp.Comment = tp.Comment;
                                                        objtp.RPDHCLimit = tp.RPDHCLimit;
                                                        objtp.RPDLCLimit = tp.RPDLCLimit;
                                                        objtp.RecHCLimit = tp.RecHCLimit;
                                                        objtp.RecLCLimit = tp.RecLCLimit;
                                                        objtp.REHCLimit = tp.REHCLimit;
                                                        objtp.RELCLimit = tp.RELCLimit;
                                                        objtp.SpikeAmount = tp.SpikeAmount;
                                                        if (tp.SpikeAmountUnit != null)
                                                        {
                                                            objtp.SpikeAmountUnit = uow.GetObjectByKey<Unit>(tp.SpikeAmountUnit.Oid);
                                                        }
                                                        if (tp.STDConcUnit != null)
                                                        {
                                                            objtp.STDConcUnit = uow.GetObjectByKey<Unit>(tp.STDConcUnit.Oid);
                                                        }
                                                        if (tp.STDVolUnit != null)
                                                        {
                                                            objtp.STDVolUnit = uow.GetObjectByKey<Unit>(tp.STDVolUnit.Oid);
                                                        }
                                                        objtp.STDConc = tp.STDConc;
                                                        objtp.STDVolAdd = tp.STDVolAdd;
                                                        objtp.SurrogateAmount = tp.SurrogateAmount;
                                                        objtp.SurrogateHighLimit = tp.SurrogateHighLimit;
                                                        objtp.SurrogateLowLimit = tp.SurrogateLowLimit;
                                                        if (tp.SurrogateUnits != null)
                                                        {
                                                            objtp.SurrogateUnits = uow.GetObjectByKey<Unit>(tp.SurrogateUnits.Oid);
                                                        }
                                                        objtp.HighCLimit = tp.HighCLimit;
                                                        objtp.LowCLimit = tp.LowCLimit;
                                                        objtp.Save();
                                                        uow.CommitChanges();
                                                        Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, testmethod.Oid, objtm.TestCode, "QC Parameter", objtp.Parameter.ParameterName);

                                                    }
                                                }
                                            }
                                            uow.CommitChanges();
                                            DashboardViewItem dvQCSP = (DashboardViewItem)((DetailView)View).FindItem("QCSampleParameter");
                                            if (dvQCSP != null && dvQCSP.InnerView != null)
                                            {
                                                dvQCSP.InnerView.ObjectSpace.Refresh();
                                            }
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Please add and save source parameters", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage("Select Checkbox", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            Testparameter objtp = (Testparameter)dc.Window.View.CurrentObject;
                            if (objtp.Matrix != null && objtp.Method != null && objtp.Test != null)
                            {
                                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                TestMethod crttm = (TestMethod)Application.MainWindow.View.CurrentObject;
                                TestMethod objtm = uow.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] = ? And [MatrixName.MatrixName] = ? And [MethodName.MethodNumber] = ?", objtp.Test.TestName, objtp.Matrix.MatrixName, objtp.Method.MethodName.MethodNumber));
                                if (objtm != null)
                                {
                                    if (CopyParameters.SelectedItem.Id == "Copyparameters")
                                    {
                                        List<Testparameter> lsttp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objtm.Oid && i.QCType != null && i.QCType.QCTypeName == "Sample").ToList();
                                        //IList<Testparameter> lsttp = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ? and [QCType.QCTypeName] = 'Sample'", objtm.Oid));
                                        if (lsttp != null && lsttp.Count > 0)
                                        {
                                            CopySampleParmeters(crttm, objtm, uow, lsttp);
                                            chkparametercommitted = true;
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Sample parameters not available in selected test", InformationType.Error, 3000, InformationPosition.Top);
                                        }
                                    }
                                    else if (CopyParameters.SelectedItem.Id == "Copysurrogates")
                                    {
                                        List<Testparameter> lsttp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objtm.Oid && i.Surroagate == true).ToList();
                                        //IList<Testparameter> lsttp = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ? and [Surroagate] = true", objtm.Oid));
                                        if (lsttp != null && lsttp.Count > 0)
                                        {
                                            CopySurrogateParameters(crttm, objtm, uow, lsttp);
                                            chkparametercommitted = true;
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Surrogate parameters not available in selected test", InformationType.Error, 3000, InformationPosition.Top);
                                        }
                                    }
                                    else if (CopyParameters.SelectedItem.Id == "Copyinternalstandards")
                                    {
                                        List<Testparameter> lsttp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objtm.Oid && i.InternalStandard == true).ToList();
                                        //IList<Testparameter> lsttp = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ? and [InternalStandard] = true", objtm.Oid));
                                        if (lsttp != null && lsttp.Count > 0)
                                        {
                                            CopyInternalStandardsParameters(crttm, objtm, uow, lsttp);
                                            chkparametercommitted = true;
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Surrogate parameters not available in selected test", InformationType.Error, 3000, InformationPosition.Top);
                                        }
                                    }
                                    else if (CopyParameters.SelectedItem.Id == "Copyqcparameters")
                                    {
                                        TestMethod crtobjtest = (TestMethod)Application.MainWindow.View.CurrentObject;
                                        List<Testparameter> lsttp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objtm.Oid && i.QCType != null && i.QCType.QCTypeName != "Sample").ToList();
                                        //IList<Testparameter> lsttp = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.QCTypeName] <> 'Sample' And [TestMethod] = ?", objtm.Oid));
                                        if (lsttp != null && lsttp.Count > 0)
                                        {
                                            CopyQCParameters(crttm, objtm, uow, lsttp);
                                            chkparametercommitted = true;
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Qc parameters not available in selected test", InformationType.Error, 3000, InformationPosition.Top);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage("Enter all fields", InformationType.Error, timer.Seconds, InformationPosition.Top);
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

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (chkparametercommitted == true)
                {
                    chkparametercommitted = false;
                    Application.ShowViewStrategy.ShowMessage("Parameter copied success", InformationType.Success, 3000, InformationPosition.Top);
                    if (View is DetailView)
                    {
                        DashboardViewItem dvSampleparam = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                        DashboardViewItem dvQCSampleparam = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                        DashboardViewItem dvInternalStandards = ((DetailView)View).FindItem("InternalStandards") as DashboardViewItem;
                        DashboardViewItem dvQCParameterDefault = ((DetailView)View).FindItem("QCParameterDefault") as DashboardViewItem;
                        DashboardViewItem dvSurrogates = ((DetailView)View).FindItem("dvSurrogates") as DashboardViewItem;
                        ListPropertyEditor lvQCTypes = ((DetailView)View).FindItem("QCTypes") as ListPropertyEditor;
                        if (dvSampleparam != null && dvSampleparam.InnerView != null)
                        {
                            dvSampleparam.InnerView.ObjectSpace.Refresh();
                        }
                        if (dvQCSampleparam != null && dvQCSampleparam.InnerView != null)
                        {
                            dvQCSampleparam.InnerView.ObjectSpace.Refresh();
                        }
                        if (dvInternalStandards != null && dvInternalStandards.InnerView != null)
                        {
                            dvInternalStandards.InnerView.ObjectSpace.Refresh();
                        }
                        if (dvSurrogates != null && dvSurrogates.InnerView != null)
                        {
                            dvSurrogates.InnerView.ObjectSpace.Refresh();
                        }
                        if (dvQCParameterDefault != null && dvQCParameterDefault.InnerView != null)
                        {
                            dvQCParameterDefault.InnerView.ObjectSpace.Refresh();
                        }
                        if (lvQCTypes != null && lvQCTypes.ListView != null)
                        {
                            lvQCTypes.ListView.ObjectSpace.Refresh();
                        }
                        ((DetailView)View).Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CopyTest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                Testparameter objtm = objspace.CreateObject<Testparameter>();
                DetailView dv = Application.CreateDetailView(objspace, "Testparameter_DetailView_CopyTest", true, objtm);
                dv.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showviewparameter = new ShowViewParameters(dv);
                showviewparameter.Context = TemplateContext.PopupWindow;
                showviewparameter.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dialogcontroller = Application.CreateController<DialogController>();
                dialogcontroller.SaveOnAccept = false;

                dialogcontroller.Accepting += dialogcontroller_Accepting;
                dialogcontroller.AcceptAction.Execute += dialogcontrollerAcceptAction_Execute;
                dialogcontroller.AcceptAction.Executed += AcceptAction_Executed1;
                dialogcontroller.CloseOnCurrentObjectProcessing = false;
                showviewparameter.Controllers.Add(dialogcontroller);
                Application.ShowViewStrategy.ShowView(showviewparameter, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AcceptAction_Executed1(object sender, ActionBaseEventArgs e)
        {
            try
            {
                ////if(testInfo.isgroup == true)
                ////{
                ////    DashboardViewItem dvtestmethod = ((DetailView)Application.MainWindow.View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                ////    if(dvtestmethod != null && dvtestmethod.InnerView != null)
                ////    {
                ////        ((ListView)dvtestmethod.InnerView).Refresh();
                ////    }
                ////}
                DashboardViewItem dvSampleparam = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                DashboardViewItem dvQCSampleparam = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                DashboardViewItem dvInternalStandards = ((DetailView)View).FindItem("InternalStandards") as DashboardViewItem;
                DashboardViewItem dvSurrogates = ((DetailView)View).FindItem("dvSurrogates") as DashboardViewItem;
                ListPropertyEditor lvQCTypes = ((DetailView)View).FindItem("QCTypes") as ListPropertyEditor;
                DashboardViewItem dvQCParameterDefault = ((DetailView)View).FindItem("QCParameterDefault") as DashboardViewItem;
                ListPropertyEditor lvTestGuides = ((DetailView)View).FindItem("TestGuides") as ListPropertyEditor;
                ListPropertyEditor lvSamplingMethods = ((DetailView)View).FindItem("SamplingMethods") as ListPropertyEditor;
                ListPropertyEditor lvPrepMethods = ((DetailView)View).FindItem("PrepMethods") as ListPropertyEditor;
                if (dvSampleparam != null && dvSampleparam.InnerView != null)
                {
                    dvSampleparam.InnerView.ObjectSpace.Refresh();
                }
                if (dvQCSampleparam != null && dvQCSampleparam.InnerView != null)
                {
                    dvQCSampleparam.InnerView.ObjectSpace.Refresh();
                }
                if (dvInternalStandards != null && dvInternalStandards.InnerView != null)
                {
                    dvInternalStandards.InnerView.ObjectSpace.Refresh();
                }
                if (dvSurrogates != null && dvSurrogates.InnerView != null)
                {
                    dvSurrogates.InnerView.ObjectSpace.Refresh();
                }
                if (dvQCParameterDefault != null && dvQCParameterDefault.InnerView != null)
                {
                    dvQCParameterDefault.InnerView.ObjectSpace.Refresh();
                }
                if (lvQCTypes != null && lvQCTypes.ListView != null)
                {
                    lvQCTypes.ListView.ObjectSpace.Refresh();
                }
                if (lvTestGuides != null && lvTestGuides.ListView != null)
                {
                    lvTestGuides.ListView.ObjectSpace.Refresh();
                }
                if (lvPrepMethods != null && lvPrepMethods.ListView != null)
                {
                    lvPrepMethods.ListView.ObjectSpace.Refresh();
                }
                if (lvSamplingMethods != null && lvSamplingMethods.ListView != null)
                {
                    lvSamplingMethods.ListView.ObjectSpace.Refresh();
                }
                ((DetailView)View).Refresh();
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void dialogcontrollerAcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (chktestcommitted == true)
                {
                    Application.ShowViewStrategy.ShowMessage("Test copied success", InformationType.Success, 3000, InformationPosition.Top);
                }
                if (testInfo.isgroup == true)
                {
                    Application.ShowViewStrategy.ShowMessage("Testgroup copied success", InformationType.Success, 3000, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void dialogcontroller_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (testInfo.isgroup == true)
                {
                    if (sender != null)
                    {
                        DialogController dc = (DialogController)sender;
                        if (dc != null && dc.Window.View.Id == "Testparameter_DetailView_CopyTest")
                        {
                            Testparameter curtobjtp = (Testparameter)dc.Window.View.CurrentObject;
                            if (curtobjtp.Matrix != null && curtobjtp.Test.TestName != null && curtobjtp.Test != null)
                            {
                                TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ?", curtobjtp.Matrix.MatrixName, curtobjtp.Test.TestName));
                                if (objtm != null)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    IList<GroupTestMethod> lstgtm = os.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid));
                                    if (lstgtm != null && lstgtm.Count > 0)
                                    {
                                        TestMethod curtobjtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                                        IList<GroupTestMethod> dellstgtm = os.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", curtobjtm.Oid));
                                        if (dellstgtm != null && dellstgtm.Count > 0)
                                        {
                                            os.Delete(dellstgtm);
                                            os.CommitChanges();
                                        }
                                        DashboardViewItem dvtestmethod = ((DetailView)Application.MainWindow.View).FindItem("TestMethodIsGroup") as DashboardViewItem;
                                        if (dvtestmethod != null && dvtestmethod.InnerView != null)
                                        {
                                            foreach (GroupTestMethod objgtm in lstgtm.ToList())
                                            {
                                                GroupTestMethod crtgtm = dvtestmethod.InnerView.ObjectSpace.CreateObject<GroupTestMethod>();
                                                crtgtm.TestMethod = dvtestmethod.InnerView.ObjectSpace.GetObject<TestMethod>(curtobjtm);
                                                crtgtm.TestParameter = dvtestmethod.InnerView.ObjectSpace.GetObject<Testparameter>(objgtm.TestParameter);
                                                ((ListView)dvtestmethod.InnerView).ObjectSpace.CommitChanges();
                                                ((ListView)dvtestmethod.InnerView).CollectionSource.Add(dvtestmethod.InnerView.ObjectSpace.GetObject(crtgtm));
                                                ((ListView)dvtestmethod.InnerView).ObjectSpace.Refresh();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage("Enter all combo box", InformationType.Error, 3000, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (sender != null)
                    {
                        DialogController dc = (DialogController)sender;
                        if (dc != null && dc.Window.View.Id == "Testparameter_DetailView_CopyTest")
                        {
                            DashboardViewItem dvi = ((DetailView)dc.Window.View).FindItem("CopyTest") as DashboardViewItem;
                            if (dvi != null && dvi.InnerView != null)
                            {
                                ListView lstview = dvi.InnerView as ListView;
                                if (lstview.SelectedObjects.Count > 0)
                                {
                                    Testparameter curtobjtp = (Testparameter)dc.Window.View.CurrentObject;
                                    if (curtobjtp.Matrix != null && curtobjtp.Test != null && curtobjtp.Method != null)
                                    {
                                        TestMethod objCurTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                                        if (objCurTest != null)
                                        {
                                            Session currentSession = ((XPObjectSpace)ObjectSpace).Session;
                                            UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                            TestMethod objCopySourceTest = uow.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And  [TestName] = ? And [MethodName.MethodNumber] = ?", curtobjtp.Matrix.MatrixName, curtobjtp.Test.TestName, curtobjtp.Method.MethodName.MethodNumber));
                                            if (objCopySourceTest != null)
                                            {
                                                CopyFromOneTestToAnother(objCurTest, objCopySourceTest, uow, lstview.SelectedObjects);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        e.Cancel = true;
                                        Application.ShowViewStrategy.ShowMessage("Enter all combo box", InformationType.Error, 3000, InformationPosition.Top);
                                    }
                                }
                                else
                                {
                                    e.Cancel = true;
                                    Application.ShowViewStrategy.ShowMessage("Select Check Box", InformationType.Error, 3000, InformationPosition.Top);
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

        private void CopyFromOneTestToAnother(TestMethod objCurTest, TestMethod objCopySourceTest, UnitOfWork uow, IList selectedObjects)
        {
            try
            {
                string errstring = string.Empty;
                foreach (TestCopy objguid in selectedObjects)
                {
                    if (objguid.Title == "Sampling Method")
                    {
                        if (objCopySourceTest.SamplingMethods.Count > 0)
                        {
                            if (View is DetailView)
                            {
                                ListPropertyEditor lstsamplemet = ((DetailView)View).FindItem("SamplingMethods") as ListPropertyEditor;
                                if (lstsamplemet != null && lstsamplemet.ListView != null)
                                {
                                    lstsamplemet.ListView.CollectionSource.List.Clear();
                                }
                            }
                            foreach (Method objmeth in objCopySourceTest.SamplingMethods.ToList())
                            {
                                if (testInfo.IsCopyTestAction == true)
                                {
                                    objCurTest.SamplingMethods.Add(ObjectSpace.GetObjectByKey<Method>(objmeth.Oid));
                                    ObjectSpace.CommitChanges();
                                }
                                else
                                {
                                    objCurTest.SamplingMethods.Add(uow.GetObjectByKey<Method>(objmeth.Oid));
                                }
                            }
                            testInfo.IsCopyTestAction = false;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(errstring))
                            {
                                errstring = "Sampling methods";
                            }
                            else
                            {
                                errstring = errstring + "," + "Sampling methods";
                            }
                        }

                    }
                    else if (objguid.Title == "Prep Method")
                    {
                        if (objCopySourceTest.PrepMethods.Count > 0)
                        {
                            List<PrepMethod> lsttp = uow.Query<PrepMethod>().Where(i => i.TestMethod.Oid == objCopySourceTest.Oid).ToList();
                            //IList<PrepMethod> lsttp = objectSpace.GetObjects<PrepMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objCopySourceTest.Oid));
                            if (lsttp != null && lsttp.Count > 0)
                            {
                                CopyPrepMethods(objCurTest, objCopySourceTest, uow, lsttp);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(errstring))
                            {
                                errstring = "PrepMethods";
                            }
                            else
                            {
                                errstring = errstring + "," + "PrepMethods";
                            }
                        }
                    }
                    else if (objguid.Title == "QC Type" || objguid.Title == "QC Sample Parameters")
                    {
                        if (objCopySourceTest.QCTypes.Count > 0)
                        {
                            List<Testparameter> lsttp = uow.Query<Testparameter>().Where(i => i.QCType.QCTypeName != "Sample" && i.TestMethod.Oid == objCopySourceTest.Oid).ToList();
                            //IList<Testparameter> lsttp = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.QCTypeName] <> 'Sample' And [TestMethod.Oid] = ?", objCopySourceTest.Oid));
                            if (lsttp != null && lsttp.Count > 0)
                            {
                                CopyQCParameters(objCurTest, objCopySourceTest, uow, lsttp);
                            }
                            else
                            {
                                if (objguid.Title == "QC Type")
                                {
                                    objCurTest = uow.GetObjectByKey<TestMethod>(objCurTest.Oid);
                                    if (objCurTest.QCTypes.Count > 0)
                                    {
                                        foreach (QCType objQctype in objCurTest.QCTypes.ToList())//tests.Where(a => a.TestName !=null).Distinct())
                                        {
                                            objCurTest.QCTypes.Remove(objQctype);
                                            objCurTest.Save();
                                        }
                                        uow.CommitChanges();
                                    }
                                    foreach (QCType objQctype in objCopySourceTest.QCTypes.ToList())//tests.Where(a => a.TestName !=null).Distinct())
                                    {
                                        QCType objqc = uow.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = ?", objQctype.QCTypeName));
                                        if (objqc != null)
                                        {
                                            objCurTest.QCTypes.Add(objqc);
                                            objCurTest.Save();
                                            if (!uow.IsNewObject(objCurTest))
                                            { 
                                                Frame.GetController<AuditlogViewController>().insertauditdata(uow, objCurTest.Oid, OperationType.Created, "Tests", objCurTest.TestCode, "QCTypes", "", objqc.QCTypeName, "");
                                        }
                                        }
                                    }
                                    uow.CommitChanges();
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(errstring))
                            {
                                errstring = "QCTypes";
                            }
                            else
                            {
                                errstring = errstring + "," + "QCTypes";
                            }
                        }
                    }
                    else if (objguid.Title == "Test Price")
                    {

                    }
                    else if (objguid.Title == "Test Guide")
                    {
                        if (objCopySourceTest.TestGuides.Count > 0)
                        {
                            List<TestGuide> lsttp = uow.Query<TestGuide>().Where(i => i.TestMethod.Oid == objCopySourceTest.Oid).ToList();
                            //IList<TestGuide> lsttp = objectSpace.GetObjects<TestGuide>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objCopySourceTest.Oid));
                            if (lsttp != null && lsttp.Count > 0)
                            {
                                CopyTestGuide(objCurTest, objCopySourceTest, uow, lsttp);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(errstring))
                            {
                                errstring = "Test Guide";
                            }
                            else
                            {
                                errstring = errstring + "," + "Test Guide";
                            }
                        }
                    }
                    else if (objguid.Title == "Sample Parameters")
                    {
                        List<Testparameter> lsttp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objCopySourceTest.Oid && i.QCType.QCTypeName == "Sample").ToList();
                        //IList<Testparameter> lsttp = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ? and [QCType.QCTypeName] = 'Sample'", objCopySourceTest.Oid));
                        if (lsttp != null && lsttp.Count > 0)
                        {
                            CopySampleParmeters(objCurTest, objCopySourceTest, uow, lsttp);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(errstring))
                            {
                                errstring = "Sample Parameters";
                            }
                            else
                            {
                                errstring = errstring + "," + "Sample Parameters";
                            }
                        }
                    }
                    else if (objguid.Title == "Surrogates")
                    {
                        List<Testparameter> lsttp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objCopySourceTest.Oid && i.Surroagate == true).ToList();
                        //IList<Testparameter> lsttp = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ? and [Surroagate] = true", objCopySourceTest.Oid));
                        if (lsttp != null && lsttp.Count > 0)
                        {
                            CopySurrogateParameters(objCurTest, objCopySourceTest, uow, lsttp);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(errstring))
                            {
                                errstring = "Surrogates";
                            }
                            else
                            {
                                errstring = errstring + "," + "Surrogates";
                            }
                        }
                    }
                    else if (objguid.Title == "Internal Standard")
                    {
                        List<Testparameter> lsttp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objCopySourceTest.Oid && i.InternalStandard == true).ToList();
                        //IList<Testparameter> lsttp = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ? and [InternalStandard] = true", objCopySourceTest.Oid));
                        if (lsttp != null && lsttp.Count > 0)
                        {
                            CopyInternalStandardsParameters(objCurTest, objCopySourceTest, uow, lsttp);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(errstring))
                            {
                                errstring = "Internal Standard";
                            }
                            else
                            {
                                errstring = errstring + "," + "Internal Standard";
                            }
                        }
                    }
                    else if (objguid.Title == "")
                    {

                    }
                    else if (objguid.Title == "Components")
                    {
                        IList<Component> lstComponent = uow.Query<Component>().Where(i => i.TestMethod.Oid == objCopySourceTest.Oid).ToList();
                        //IList<Component> lstComponent = objectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objCopySourceTest.Oid));
                        if (lstComponent != null && lstComponent.Count > 0)
                        {
                            foreach (Component objcom in lstComponent.ToList())
                            {
                                Component crtcomp = new Component(uow);
                                crtcomp.TestMethod = uow.GetObjectByKey<TestMethod>(objCurTest.Oid);
                                crtcomp.Components = objcom.Components;
                                crtcomp.Comment = objcom.Comment;
                                crtcomp.RetireDate = objcom.RetireDate;
                                uow.CommitChanges();
                                IList<Testparameter> objTestParameter = uow.Query<Testparameter>().Where(i => i.Component.Oid == objcom.Oid).ToList();
                                if (objTestParameter.Count > 0)
                                {
                                    foreach (Testparameter objParam in objTestParameter)
                                    {
                                        Testparameter objtp = new Testparameter(uow);
                                        objtp.TestMethod = uow.GetObjectByKey<TestMethod>(objCurTest.Oid);
                                        objtp.QCType = uow.GetObjectByKey<QCType>(objParam.QCType.Oid);
                                        objtp.Parameter = uow.GetObjectByKey<Parameter>(objParam.Parameter.Oid);
                                        objtp.Component = uow.GetObjectByKey<Component>(crtcomp.Oid);
                                        testInfo.lstSampleParameters.Add(objtp);
                                    }
                                }
                                uow.CommitChanges();
                                //if (objcom.TestParameters.Count > 0)
                                //{
                                //    foreach (Testparameter objtp in objcom.TestParameters.ToList())
                                //    {
                                //        crtcomp.TestParameters.Add(objtp);
                                //    }
                                //}
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(errstring))
                            {
                                errstring = "Component";
                            }
                            else
                            {
                                errstring = errstring + "," + "Component";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(errstring))
                {
                    Application.ShowViewStrategy.ShowMessage(errstring + " not available in selected test", InformationType.Error, 3000, InformationPosition.Top);
                }
                uow.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyTestGuide(TestMethod objCurTest, TestMethod objCopySourceTest, UnitOfWork uow, IList<TestGuide> lsttp)
        {
            try
            {
                List<TestGuide> lstoldpm = uow.Query<TestGuide>().Where(i => i.TestMethod.Oid == objCurTest.Oid).ToList();
                //IList<TestGuide> lstoldpm = objectSpace.GetObjects<TestGuide>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objCurTest.Oid));
                if (lstoldpm.Count > 0 && lstoldpm != null)
                {
                    foreach (TestGuide objchktmdel in lstoldpm.ToList())
                    {
                        uow.Delete(objchktmdel);
                        objchktmdel.Save();
                    }
                    uow.CommitChanges();
                }
                foreach (TestGuide objTestGuide in lsttp)
                {
                    if (uow.FindObject<TestGuide>(CriteriaOperator.Parse("[TestMethod.Oid] = ? and [Container.Oid] = ?", objCurTest.Oid, objTestGuide.Oid)) == null)
                    {
                        TestGuide crtobjtm = new TestGuide(uow);
                        crtobjtm.TestMethod = uow.GetObjectByKey<TestMethod>(objCurTest.Oid);
                        //crtobjtm.Method = objectSpace.GetObject<Method>(objPrepMethod.Method);
                        if (objTestGuide.Container != null)
                        {
                            crtobjtm.Container = uow.GetObjectByKey<Modules.BusinessObjects.Setting.Container>(objTestGuide.Container.Oid);
                        }
                        if (objTestGuide.Preservative != null)
                        {
                            crtobjtm.Preservative = uow.GetObjectByKey<Preservative>(objTestGuide.Preservative.Oid);
                        }
                        crtobjtm.Temperature = objTestGuide.Temperature;
                        if (objTestGuide.HoldingTimeBeforePrep != null)
                        {
                            crtobjtm.HoldingTimeBeforePrep = uow.GetObjectByKey<HoldingTimes>(objTestGuide.HoldingTimeBeforePrep.Oid);
                        }
                        if (objTestGuide.HoldingTimeBeforeAnalysis != null)
                        {
                            crtobjtm.HoldingTimeBeforeAnalysis = uow.GetObjectByKey<HoldingTimes>(objTestGuide.HoldingTimeBeforeAnalysis.Oid);
                        }
                        crtobjtm.SetPrepTimeAsAnalysisTime = objTestGuide.SetPrepTimeAsAnalysisTime;
                        crtobjtm.Save();
                        if (View.ObjectSpace.IsNewObject(objCurTest))
                        {
                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, objCurTest.Oid, OperationType.Created, "Tests", objCurTest.TestCode, "TestGuide", "", crtobjtm.Container.ContainerName, "");

                        }

                    }
                }
                uow.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyPrepMethods(TestMethod objCurTest, TestMethod objCopySourceTest, UnitOfWork uow, IList<PrepMethod> lstpm)
        {
            try
            {
                List<PrepMethod> lstoldpm = uow.Query<PrepMethod>().Where(i => i.TestMethod.Oid == objCurTest.Oid).ToList();
                //IList<PrepMethod> lstoldpm = objectSpace.GetObjects<PrepMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objCurTest.Oid));
                if (lstoldpm.Count > 0 && lstoldpm != null)
                {
                    foreach (PrepMethod objchktmdel in lstoldpm.ToList())
                    {
                        uow.Delete(objchktmdel);
                        objchktmdel.Save();
                    }
                    uow.CommitChanges();
                }
                foreach (PrepMethod objPrepMethod in lstpm)
                {
                    if (uow.FindObject<PrepMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ? and [Method.Oid] = ?", objCurTest.Oid, objPrepMethod.Oid)) == null)
                    {
                        PrepMethod crtobjtm = new PrepMethod(uow);
                        crtobjtm.TestMethod = uow.GetObjectByKey<TestMethod>(objCurTest.Oid);
                        if (objPrepMethod.Method != null)
                        {
                            crtobjtm.Method = uow.GetObjectByKey<Method>(objPrepMethod.Method.Oid);
                        }
                        crtobjtm.ActiveDate = objPrepMethod.ActiveDate;
                        crtobjtm.Department = objPrepMethod.Department;
                        crtobjtm.Description = objPrepMethod.Description;
                        crtobjtm.Instrument = objPrepMethod.Instrument;
                        if (objPrepMethod.PrepType != null)
                        {
                            crtobjtm.PrepType = uow.GetObjectByKey<PrepTypes>(objPrepMethod.PrepType.Oid);
                        }
                        crtobjtm.RetireDate = objPrepMethod.RetireDate;
                        crtobjtm.Tier = objPrepMethod.Tier;
                        objPrepMethod.Save();
                        if (!View.ObjectSpace.IsNewObject(objCurTest))
                        {
                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, objCurTest.Oid, OperationType.Created, "Tests", objCurTest.TestCode, "PrepMethod", "", crtobjtm.PrepType.SamplePrepType, "");

                        }

                    }
                }
                uow.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyQCParameters(TestMethod objCurTest, TestMethod objCopySourceTest, UnitOfWork uow, IList<Testparameter> lsttp)
        {
            try
            {
                objCurTest = uow.GetObjectByKey<TestMethod>(objCurTest.Oid);
                List<Testparameter> lstoldtp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objCurTest.Oid && i.QCType.QCTypeName != "Sample").ToList();
                //IList<Testparameter> lstoldtp = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.QCTypeName] <> 'Sample' and [TestMethod.Oid] = ?", objCurTest.Oid));
                if (lstoldtp.Count > 0 && lstoldtp != null)
                {
                    foreach (Testparameter objchktpdel in lstoldtp.ToList())
                    {
                        if (objchktpdel != null)
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objchktpdel.Oid + "'");
                            SampleParameter objTP = uow.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                uow.Delete(objchktpdel);
                                objchktpdel.Save();
                            }
                        }
                    }
                    uow.CommitChanges();
                }
                List<string> ids = lsttp.Where(i => i.QCType != null && i.QCType.QCTypeName != null).Select(i => i.QCType.QCTypeName.ToString()).Distinct().ToList();
                foreach (string objids in ids.ToList())//tests.Where(a => a.TestName !=null).Distinct())
                {
                    QCType objqc = uow.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = ?", objids));
                    if (objqc != null && !objCurTest.QCTypes.Contains(objqc))
                    {
                        objCurTest.QCTypes.Add(objqc);
                        if (!uow.IsNewObject(objCurTest))
                        {
                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, objCurTest.Oid, OperationType.Created, "Tests", objCurTest.TestCode, "QCTypes", "", objqc.QCTypeName, "");
                        }
                    }
                }
                //objCurTest.Save();
                uow.CommitChanges();
                foreach (Testparameter tp in lsttp)
                {
                    if (tp.Parameter != null && tp.QCType != null)
                    {
                        if (uow.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Parameter.Oid] = ? And [QCType.Oid] = ?", objCurTest.Oid, tp.Parameter.Oid, tp.QCType.Oid)) == null)
                        {
                            Parameter objpara = uow.GetObjectByKey<Parameter>(tp.Parameter.Oid);
                            TestMethod objtestmeth = uow.GetObjectByKey<TestMethod>(objCurTest.Oid);
                            QCType objqctype = uow.GetObjectByKey<QCType>(tp.QCType.Oid);
                            Testparameter crtobjtp = new Testparameter(uow);
                            crtobjtp.TestMethod = objtestmeth;
                            crtobjtp.Parameter = objpara;
                            crtobjtp.QCType = objqctype;
                            crtobjtp.DefaultResult = tp.DefaultResult;
                            crtobjtp.Sort = tp.Sort;
                            if (tp.DefaultUnits != null)
                            {
                                crtobjtp.DefaultUnits = uow.GetObjectByKey<Unit>(tp.DefaultUnits.Oid);
                            }
                            crtobjtp.FinalDefaultResult = tp.FinalDefaultResult;
                            if (tp.FinalDefaultUnits != null)
                            {
                                crtobjtp.FinalDefaultUnits = uow.GetObjectByKey<Unit>(tp.FinalDefaultUnits.Oid);
                            }
                            if (tp.Component != null)
                            {
                                crtobjtp.Component = uow.GetObjectByKey<Component>(tp.Component.Oid);
                            }
                            crtobjtp.RptLimit = tp.RptLimit;
                            crtobjtp.MDL = tp.MDL;
                            crtobjtp.MCL = tp.MCL;
                            crtobjtp.UQL = tp.UQL;
                            crtobjtp.LOQ = tp.LOQ;
                            crtobjtp.SigFig = tp.SigFig;
                            crtobjtp.CutOff = tp.CutOff;
                            crtobjtp.Decimal = tp.Decimal;
                            crtobjtp.Comment = tp.Comment;
                            crtobjtp.RPDHCLimit = tp.RPDHCLimit;
                            crtobjtp.RPDLCLimit = tp.RPDLCLimit;
                            crtobjtp.RecHCLimit = tp.RecHCLimit;
                            crtobjtp.RecLCLimit = tp.RecLCLimit;
                            crtobjtp.REHCLimit = tp.REHCLimit;
                            crtobjtp.RELCLimit = tp.RELCLimit;
                            crtobjtp.SpikeAmount = tp.SpikeAmount;
                            if (tp.SpikeAmountUnit != null)
                            {
                                crtobjtp.SpikeAmountUnit = uow.GetObjectByKey<Unit>(tp.SpikeAmountUnit.Oid);
                            }
                            if (tp.STDConcUnit != null)
                            {
                                crtobjtp.STDConcUnit = uow.GetObjectByKey<Unit>(tp.STDConcUnit.Oid);
                            }
                            if (tp.STDVolUnit != null)
                            {
                                crtobjtp.STDVolUnit = uow.GetObjectByKey<Unit>(tp.STDVolUnit.Oid);
                            }
                            crtobjtp.STDConc = tp.STDConc;
                            crtobjtp.STDVolAdd = tp.STDVolAdd;
                            crtobjtp.SurrogateAmount = tp.SurrogateAmount;
                            crtobjtp.SurrogateHighLimit = tp.SurrogateHighLimit;
                            crtobjtp.SurrogateLowLimit = tp.SurrogateLowLimit;
                            if (tp.SurrogateUnits != null)
                            {
                                crtobjtp.SurrogateUnits = uow.GetObjectByKey<Unit>(tp.SurrogateUnits.Oid);
                            }
                            crtobjtp.HighCLimit = tp.HighCLimit;
                            crtobjtp.LowCLimit = tp.LowCLimit;
                            crtobjtp.Save();
                            //   Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, objtestmeth.Oid, objtestmeth.TestCode, "QCParameters", crtobjtp.Parameter.ParameterName);
                            if (!uow.IsNewObject(objtestmeth))
                            {
                                Frame.GetController<AuditlogViewController>().insertauditdata(uow, objtestmeth.Oid, OperationType.Created, "Tests", objtestmeth.TestCode, "QCParameters", "", crtobjtp.Parameter.ParameterName, "");
                            }
                        }
                    }
                }
                uow.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyInternalStandardsParameters(TestMethod objCurTest, TestMethod objCopySourceTest, UnitOfWork uow, IList<Testparameter> lsttp)
        {
            try
            {
                List<Testparameter> lstoldtp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objCurTest.Oid && i.InternalStandard == true).ToList();
                //IList<Testparameter> lstoldtp = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ?  and [InternalStandard] = true", objCurTest.Oid));
                if (lstoldtp != null && lstoldtp.Count > 0)
                {
                    foreach (Testparameter objchktpdel in lstoldtp.ToList())
                    {
                        if (objchktpdel != null)
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objchktpdel.Oid + "'");
                            SampleParameter objTP = uow.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                uow.Delete(objchktpdel);
                                objchktpdel.Save();
                            }
                        }
                    }
                    uow.CommitChanges();
                }
                foreach (Testparameter tp in lsttp.Where(i => i.Parameter != null))
                {
                    if (uow.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Parameter.Oid] = ? And [InternalStandard] = true", objCurTest.Oid, tp.Parameter.Oid)) == null)
                    {
                        Parameter objpara = uow.GetObjectByKey<Parameter>(tp.Parameter.Oid);
                        TestMethod objtestmeth = uow.GetObjectByKey<TestMethod>(objCurTest.Oid);
                        Testparameter crtobjtp = new Testparameter(uow);
                        crtobjtp.TestMethod = objtestmeth;
                        crtobjtp.Parameter = objpara;
                        crtobjtp.InternalStandard = true;
                        crtobjtp.DefaultResult = tp.DefaultResult;
                        crtobjtp.Sort = tp.Sort;
                        if (tp.DefaultUnits != null)
                        {
                            crtobjtp.DefaultUnits = uow.GetObjectByKey<Unit>(tp.DefaultUnits.Oid);
                        }
                        crtobjtp.FinalDefaultResult = tp.FinalDefaultResult;
                        if (tp.FinalDefaultUnits != null)
                        {
                            crtobjtp.FinalDefaultUnits = uow.GetObjectByKey<Unit>(tp.FinalDefaultUnits.Oid);
                        }
                        if (tp.Component != null)
                        {
                            crtobjtp.Component = uow.GetObjectByKey<Component>(tp.Component.Oid);
                        }
                        crtobjtp.RptLimit = tp.RptLimit;
                        crtobjtp.MDL = tp.MDL;
                        crtobjtp.MCL = tp.MCL;
                        crtobjtp.UQL = tp.UQL;
                        crtobjtp.LOQ = tp.LOQ;
                        crtobjtp.SigFig = tp.SigFig;
                        crtobjtp.CutOff = tp.CutOff;
                        crtobjtp.Decimal = tp.Decimal;
                        crtobjtp.Comment = tp.Comment;
                        crtobjtp.RPDHCLimit = tp.RPDHCLimit;
                        crtobjtp.RPDLCLimit = tp.RPDLCLimit;
                        crtobjtp.RecHCLimit = tp.RecHCLimit;
                        crtobjtp.RecLCLimit = tp.RecLCLimit;
                        crtobjtp.REHCLimit = tp.REHCLimit;
                        crtobjtp.RELCLimit = tp.RELCLimit;
                        crtobjtp.SpikeAmount = tp.SpikeAmount;
                        if (tp.SpikeAmountUnit != null)
                        {
                            crtobjtp.SpikeAmountUnit = uow.GetObjectByKey<Unit>(tp.SpikeAmountUnit.Oid);
                        }
                        if (tp.STDConcUnit != null)
                        {
                            crtobjtp.STDConcUnit = uow.GetObjectByKey<Unit>(tp.STDConcUnit.Oid);
                        }
                        if (tp.STDVolUnit != null)
                        {
                            crtobjtp.STDVolUnit = uow.GetObjectByKey<Unit>(tp.STDVolUnit.Oid);
                        }
                        crtobjtp.STDConc = tp.STDConc;
                        crtobjtp.STDVolAdd = tp.STDVolAdd;
                        crtobjtp.SurrogateAmount = tp.SurrogateAmount;
                        crtobjtp.SurrogateHighLimit = tp.SurrogateHighLimit;
                        crtobjtp.SurrogateLowLimit = tp.SurrogateLowLimit;
                        if (tp.SurrogateUnits != null)
                        {
                            crtobjtp.SurrogateUnits = uow.GetObjectByKey<Unit>(tp.SurrogateUnits.Oid);
                        }
                        crtobjtp.HighCLimit = tp.HighCLimit;
                        crtobjtp.LowCLimit = tp.LowCLimit;
                        crtobjtp.Save();
                        //Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, objtestmeth.Oid, objtestmeth.TestCode, "InternalStandard", crtobjtp.Parameter.ParameterName);
                        if (!uow.IsNewObject(objtestmeth))
                        {
                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, objtestmeth.Oid, OperationType.Created, "Tests", objtestmeth.TestCode, "InternalStandard", "", crtobjtp.Parameter.ParameterName, "");
                        }
                    }
                }
                uow.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopySurrogateParameters(TestMethod objCurTest, TestMethod objCopySourceTest, UnitOfWork uow, IList<Testparameter> lsttp)
        {
            try
            {
                List<Testparameter> lstoldtp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objCurTest.Oid && i.Surroagate == true).ToList();
                //IList<Testparameter> lstoldtp = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ?  and [Surroagate] = true", objCurTest.Oid));
                if (lstoldtp != null && lstoldtp.Count > 0)
                {
                    foreach (Testparameter objchktpdel in lstoldtp.ToList())
                    {
                        if (objchktpdel != null)
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objchktpdel.Oid + "'");
                            SampleParameter objTP = uow.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                uow.Delete(objchktpdel);
                                objchktpdel.Save();
                            }
                        }
                    }
                    uow.CommitChanges();
                }
                foreach (Testparameter tp in lsttp)
                {
                    if (uow.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Parameter.Oid] = ? And [Surroagate] = true", objCurTest.Oid, tp.Parameter.Oid)) == null)
                    {
                        Parameter objpara = uow.FindObject<Parameter>(CriteriaOperator.Parse("[Oid] = ?", tp.Parameter.Oid));
                        TestMethod objtestmeth = uow.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", objCurTest.Oid));
                        Testparameter crtobjtp = new Testparameter(uow);
                        crtobjtp.TestMethod = objtestmeth;
                        crtobjtp.Parameter = objpara;
                        crtobjtp.Surroagate = true;
                        crtobjtp.DefaultResult = tp.DefaultResult;
                        crtobjtp.Sort = tp.Sort;
                        if (tp.DefaultUnits != null)
                        {
                            crtobjtp.DefaultUnits = uow.GetObjectByKey<Unit>(tp.DefaultUnits.Oid);
                        }
                        crtobjtp.FinalDefaultResult = tp.FinalDefaultResult;
                        if (tp.FinalDefaultUnits != null)
                        {
                            crtobjtp.FinalDefaultUnits = uow.GetObjectByKey<Unit>(tp.FinalDefaultUnits.Oid);
                        }
                        if (tp.Component != null)
                        {
                            crtobjtp.Component = uow.GetObjectByKey<Component>(tp.Component.Oid);
                        }
                        crtobjtp.RptLimit = tp.RptLimit;
                        crtobjtp.MDL = tp.MDL;
                        crtobjtp.MCL = tp.MCL;
                        crtobjtp.UQL = tp.UQL;
                        crtobjtp.LOQ = tp.LOQ;
                        crtobjtp.SigFig = tp.SigFig;
                        crtobjtp.CutOff = tp.CutOff;
                        crtobjtp.Decimal = tp.Decimal;
                        crtobjtp.Comment = tp.Comment;
                        crtobjtp.RPDHCLimit = tp.RPDHCLimit;
                        crtobjtp.RPDLCLimit = tp.RPDLCLimit;
                        crtobjtp.RecHCLimit = tp.RecHCLimit;
                        crtobjtp.RecLCLimit = tp.RecLCLimit;
                        crtobjtp.REHCLimit = tp.REHCLimit;
                        crtobjtp.RELCLimit = tp.RELCLimit;
                        crtobjtp.SpikeAmount = tp.SpikeAmount;
                        if (tp.SpikeAmountUnit != null)
                        {
                            crtobjtp.SpikeAmountUnit = uow.GetObjectByKey<Unit>(tp.SpikeAmountUnit.Oid);
                        }
                        if (tp.STDConcUnit != null)
                        {
                            crtobjtp.STDConcUnit = uow.GetObjectByKey<Unit>(tp.STDConcUnit.Oid);
                        }
                        if (tp.STDVolUnit != null)
                        {
                            crtobjtp.STDVolUnit = uow.GetObjectByKey<Unit>(tp.STDVolUnit.Oid);
                        }
                        crtobjtp.STDConc = tp.STDConc;
                        crtobjtp.STDVolAdd = tp.STDVolAdd;
                        crtobjtp.SurrogateAmount = tp.SurrogateAmount;
                        crtobjtp.SurrogateHighLimit = tp.SurrogateHighLimit;
                        crtobjtp.SurrogateLowLimit = tp.SurrogateLowLimit;
                        if (tp.SurrogateUnits != null)
                        {
                            crtobjtp.SurrogateUnits = uow.GetObjectByKey<Unit>(tp.SurrogateUnits.Oid);
                        }
                        crtobjtp.HighCLimit = tp.HighCLimit;
                        crtobjtp.LowCLimit = tp.LowCLimit;
                        crtobjtp.Save();
                        //Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, objtestmeth.Oid, objtestmeth.TestCode, "Surrogate", crtobjtp.Parameter.ParameterName);
                        if (!uow.IsNewObject(objtestmeth))
                        {
                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, objtestmeth.Oid, OperationType.Created, "Tests", objtestmeth.TestCode, "Surrogate", "", crtobjtp.Parameter.ParameterName, "");

                        }
                    }
                }
                uow.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopySampleParmeters(TestMethod objCurTest, TestMethod objCopySourceTest, UnitOfWork uow, IList<Testparameter> lsttp)
        {
            try
            {
                List<Testparameter> lstoldtp = uow.Query<Testparameter>().Where(i => i.TestMethod.Oid == objCurTest.Oid && i.QCType.QCTypeName == "Sample").ToList();
                //IList<Testparameter> lstoldtp = objectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ? and [QCType.QCTypeName] = 'Sample'", objCurTest.Oid));
                if (lstoldtp != null && lstoldtp.Count > 0)
                {
                    List<Guid> lstParameterstobeRemoved = lstoldtp.Select(i => i.Parameter.Oid).Except(lsttp.Select(i => i.Parameter.Oid)).ToList();
                    foreach (Testparameter objchktpdel in lstoldtp.ToList())
                    {
                        if (objchktpdel != null && lstParameterstobeRemoved.Contains(objchktpdel.Parameter.Oid))
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objchktpdel.Oid + "'");
                            SampleParameter objTP = uow.FindObject<SampleParameter>(criteria1);
                            if (objTP == null)
                            {
                                uow.Delete(objchktpdel);
                                objchktpdel.Save();
                            }
                        }
                    }
                    uow.CommitChanges();
                }
                foreach (Testparameter tp in lsttp)
                {
                    if (uow.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Parameter.Oid] = ? And [QCType.Oid] = ?", objCurTest.Oid, tp.Parameter.Oid, tp.QCType.Oid)) == null)
                    {
                        Parameter objpara = uow.FindObject<Parameter>(CriteriaOperator.Parse("[Oid] = ?", tp.Parameter.Oid));
                        TestMethod objtestmeth = uow.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", objCurTest.Oid));
                        QCType objqctype = uow.FindObject<QCType>(CriteriaOperator.Parse("[Oid] = ?", tp.QCType));
                        Testparameter crtobjtp = new Testparameter(uow);
                        crtobjtp.TestMethod = objtestmeth;
                        crtobjtp.Parameter = objpara;
                        crtobjtp.QCType = objqctype;
                        crtobjtp.DefaultResult = tp.DefaultResult;
                        crtobjtp.Sort = tp.Sort;
                        if (tp.DefaultUnits != null)
                        {
                            crtobjtp.DefaultUnits = uow.GetObjectByKey<Unit>(tp.DefaultUnits.Oid);
                        }
                        crtobjtp.FinalDefaultResult = tp.FinalDefaultResult;
                        if (tp.FinalDefaultUnits != null)
                        {
                            crtobjtp.FinalDefaultUnits = uow.GetObjectByKey<Unit>(tp.FinalDefaultUnits.Oid);
                        }
                        if (tp.Component != null)
                        {
                            crtobjtp.Component = uow.GetObjectByKey<Component>(tp.Component.Oid);
                        }
                        crtobjtp.RptLimit = tp.RptLimit;
                        crtobjtp.MDL = tp.MDL;
                        crtobjtp.MCL = tp.MCL;
                        crtobjtp.UQL = tp.UQL;
                        crtobjtp.LOQ = tp.LOQ;
                        crtobjtp.SigFig = tp.SigFig;
                        crtobjtp.CutOff = tp.CutOff;
                        crtobjtp.Decimal = tp.Decimal;
                        crtobjtp.Comment = tp.Comment;
                        crtobjtp.RPDHCLimit = tp.RPDHCLimit;
                        crtobjtp.RPDLCLimit = tp.RPDLCLimit;
                        crtobjtp.RecHCLimit = tp.RecHCLimit;
                        crtobjtp.RecLCLimit = tp.RecLCLimit;
                        crtobjtp.REHCLimit = tp.REHCLimit;
                        crtobjtp.RELCLimit = tp.RELCLimit;
                        crtobjtp.SpikeAmount = tp.SpikeAmount;
                        crtobjtp.RSD = tp.RSD;
                        crtobjtp.RegulatoryLimit = tp.RegulatoryLimit;
                        if (tp.SpikeAmountUnit != null)
                        {
                            crtobjtp.SpikeAmountUnit = uow.GetObjectByKey<Unit>(tp.SpikeAmountUnit.Oid);
                        }
                        if (tp.STDConcUnit != null)
                        {
                            crtobjtp.STDConcUnit = uow.GetObjectByKey<Unit>(tp.STDConcUnit.Oid);
                        }
                        if (tp.STDVolUnit != null)
                        {
                            crtobjtp.STDVolUnit = uow.GetObjectByKey<Unit>(tp.STDVolUnit.Oid);
                        }
                        crtobjtp.STDConc = tp.STDConc;
                        crtobjtp.STDVolAdd = tp.STDVolAdd;
                        crtobjtp.SurrogateAmount = tp.SurrogateAmount;
                        crtobjtp.SurrogateHighLimit = tp.SurrogateHighLimit;
                        crtobjtp.SurrogateLowLimit = tp.SurrogateLowLimit;
                        if (tp.SurrogateUnits != null)
                        {
                            crtobjtp.SurrogateUnits = uow.GetObjectByKey<Unit>(tp.SurrogateUnits.Oid);
                        }
                        crtobjtp.HighCLimit = tp.HighCLimit;
                        crtobjtp.LowCLimit = tp.LowCLimit;
                        //Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, objtestmeth.Oid, objtestmeth.TestCode, "Parameter", crtobjtp.Parameter.ParameterName);
                        // Frame.GetController<AuditlogViewController>().insertauditdata(uow, objtestmeth.Oid, OperationType.Created, "Tests", objtestmeth.TestCode, "SampleParameter", "", crtobjtp.Parameter.ParameterName, "");
                        if (!uow.IsNewObject(objtestmeth))
                        {
                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, objCurTest.Oid, OperationType.Created, "Tests", objCurTest.TestCode, "SampleParameter", "", crtobjtp.Parameter.ParameterName, "");
                        }

                    }
                    //DashboardViewItem dvsamplepara = ((DetailView)Application.MainWindow.View).FindItem("SampleParameter") as DashboardViewItem;     
                    //if(dvsamplepara != null && dvsamplepara.InnerView != null)
                    //{
                    //    ListView lstsamplepara = dvsamplepara.InnerView as ListView;
                    //    lstsamplepara.CollectionSource.List.Add(tp);
                    //}
                }
                uow.CommitChanges();
                View.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyParameters_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {

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
                if (e.PopupFrame.View.Id == "TestMethod_DetailView_CopyTest")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(700);
                    e.Height = new System.Web.UI.WebControls.Unit(570);
                    e.Handled = true;
                }
                //if (e.PopupFrame.View.Id == "TestPrice_DetailView" && Application !=null && Application.MainWindow !=null && Application.MainWindow.View != null)
                //{
                //    TestMethod crtobjtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                //    TestPrice crtobjtestprice = (TestPrice)e.PopupFrame.View.CurrentObject;
                //    //objtestpriceinfo.TestPriceCurrentObject = crtobjtestprice;
                //    objtestpriceinfo.testmethod = crtobjtm;
                //    e.Width = new System.Web.UI.WebControls.Unit(1200);
                //    e.Height = new System.Web.UI.WebControls.Unit(700);
                //    e.Handled = true;
                //}
                else if (e.PopupFrame.View.Id == "TestMethod")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(780);
                    e.Height = new System.Web.UI.WebControls.Unit(720);
                    e.Handled = true;
                    //e.Width = new System.Web.UI.WebControls.Unit(500);
                    //e.Height = new System.Web.UI.WebControls.Unit(500);
                    //e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ADDAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)
                {
                    if (View.Id == "GroupTestMethod_ListView_IsGroup")
                    {
                        if (testInfo.testmethodguid == null)
                        {
                            testInfo.testmethodguid = new List<Guid>();
                        }
                        else
                        {
                            testInfo.testmethodguid = new List<Guid>();
                        }
                        testInfo.testmethodguid = ((ListView)View).CollectionSource.List.Cast<GroupTestMethod>().Select(i => i.TestParameter.Oid).ToList();
                        IObjectSpace os = Application.CreateObjectSpace();
                        List<Guid> lsttestpara = new List<Guid>();
                        //foreach (GroupTestMethod objtm in ((ListView)View).CollectionSource.List)
                        //{
                        //    lsttestpara.Add(objtm.Tests.Oid);
                        //}
                        lsttestpara = ((ListView)View).CollectionSource.List.Cast<GroupTestMethod>().Where(i => i.Tests != null).Select(i => i.Tests.Oid).ToList();
                        CollectionSource cs = new CollectionSource(os, typeof(Testparameter));
                        if (lsttestpara.Count > 0)
                        {
                            cs.Criteria["filter"] = new NotOperator(new InOperator("TestMethod.Oid", lsttestpara));
                        }
                        ListView lv = Application.CreateListView("Testparameter_ListView_Testmethod_IsGroup", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.AcceptAction.Execute += TestGroup_AcceptAction_Execute;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    //if (View.Id == "TestPrice_ListView")
                    //{
                    //    //IObjectSpace os = ((ListView)View).ObjectSpace;//Application.CreateObjectSpace();
                    //    //IObjectSpace os = Application.CreateObjectSpace();


                    //    if (((ListView)View).CollectionSource.List.Count == 0)
                    //    {
                    //        IObjectSpace os = Application.CreateObjectSpace();
                    //        //NonPersistentObjectSpace npos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(TestPrice));
                    //        TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                    //        TestPrice objTestPrice = os.CreateObject<TestPrice>();
                    //        testpriceinfo.crttestprice = objTestPrice.Oid;
                    //        testpriceinfo.CurrentTestPrice = objTestPrice;
                    //        objTestPrice.TestMethod = os.GetObject(objtm);
                    //        DetailView dv = Application.CreateDetailView(os, "TestPrice_DetailView", true, objTestPrice);
                    //        dv.ViewEditMode = ViewEditMode.Edit;
                    //        ShowViewParameters showViewParameters = new ShowViewParameters();
                    //        showViewParameters.CreatedView = dv;
                    //        showViewParameters.Context = TemplateContext.PopupWindow;
                    //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //        DialogController dc = Application.CreateController<DialogController>();
                    //        dc.AcceptAction.Execute += AcceptAction_Execute;
                    //        dc.Accepting += dcTestprice_Accepting;
                    //        showViewParameters.Controllers.Add(dc);
                    //        //testpriceinfo.CurrentTestPrice = objTestPrice;
                    //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null)); 
                    //    }
                    //    else
                    //    {
                    //        Application.ShowViewStrategy.ShowMessage("Unable to create a new TestPrice beacause TestPrice was already created to this TestMethod", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //    }
                    //}
                    //if (View.Id == "TestPriceDetail_ListView_Copy_perparameter" || View.Id == "TestPriceDetail_ListView_Copy_pertest")
                    //{
                    //    TestMethod objtm = (TestMethod)Application.MainWindow.View.CurrentObject;
                    //    if (objtestpriceinfo.TestPriceCurrentObject.Component != null)
                    //    {
                    //        IObjectSpace os = View.ObjectSpace;
                    //        NestedFrame nestedFrame = (NestedFrame)Frame;
                    //        CompositeView view = nestedFrame.ViewItem.View;
                    //        IList<TestPriceDetail> lsttestprice =view.ObjectSpace.GetObjects<TestPriceDetail>(CriteriaOperator.Parse("[TestMethod]=?",view.ObjectSpace.GetObject(objtm))).ToList().Cast<TestPriceDetail>().ToList().Where(i => i.TAT == null && i.Surcharge == 0 && i.Price == 0 && i.AdditionalParamPriceItemPerUnit == 0 && i.BasicParamPrice == 0 && i.BasicParamPriceItemPerUnit == 0).ToList();
                    //        //IList<TestPriceDetail> lsttestprice = ((ListView)View).CollectionSource.List.Cast<TestPriceDetail>().ToList().Where(i => i.TAT == null && i.Surcharge == 0 && i.Price == 0 && i.AdditionalParamPriceItemPerUnit == 0 && i.BasicParamPrice == 0 && i.BasicParamPriceItemPerUnit == 0).ToList(); //(CriteriaOperator.Parse("[TAT] Is Null And[Surcharge] Is Null And[Price] Is Null And[AdditionalParamPriceItemPerUnit] Is Null And[BasicParamPrice] Is Null And[BasicParamPriceItemPerUnit] Is Null"));
                    //        if (lsttestprice.Count == 0)
                    //        {
                    //            //TestPriceDetail objtp = os.CreateObject<TestPriceDetail>();
                    //            //objtp.TestMethod = os.GetObject(objtm);
                    //            //view.ObjectSpace.CommitChanges();
                    //            //TestPrice objView = (TestPrice)os.GetObject<TestPrice>((TestPrice)view.CurrentObject);
                    //            //objtp.TestPrice = objView;
                    //            //os.CommitChanges();
                    //            //((ListView)View).ObjectSpace.Refresh();

                    //            TestPriceDetail testPriceDetail = view.ObjectSpace.CreateObject<TestPriceDetail>();
                    //            testPriceDetail.TestMethod = view.ObjectSpace.GetObject(objtm);
                    //            view.ObjectSpace.CommitChanges();
                    //            TestPrice objView = (TestPrice)view.ObjectSpace.GetObject<TestPrice>((TestPrice)view.CurrentObject);
                    //            testPriceDetail.TestPrice = objView;
                    //            view.ObjectSpace.CommitChanges();
                    //            ((ListView)View).ObjectSpace.CommitChanges();
                    //            ((ListView)View).ObjectSpace.Refresh();
                    //        }
                    //        else
                    //        {
                    //            Application.ShowViewStrategy.ShowMessage("Please enter Test Price details", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Application.ShowViewStrategy.ShowMessage("Select test code", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //        return;
                    //    }
                    //}
                    if (View.Id == "Testparameter_ListView_Test_SampleParameter" || View.Id == "Testparameter_ListView_Test_QCSampleParameter" || View.Id == "Testparameter_ListView_Test_InternalStandards" || View.Id == "Testparameter_ListView_Test_Surrogates")
                    {
                        TestMethod objcrutm = (TestMethod)Application.MainWindow.View.CurrentObject;

                        CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Parameter));
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        DashboardViewItem dvSampleparam = ((DetailView)view).FindItem("Sampleparameter") as DashboardViewItem;
                        DashboardViewItem dvQCSampleparam = ((DetailView)view).FindItem("QCSampleParameter") as DashboardViewItem;
                        DashboardViewItem dvInternalStandards = ((DetailView)view).FindItem("InternalStandards") as DashboardViewItem;
                        DashboardViewItem dvSurrogates = ((DetailView)view).FindItem("dvSurrogates") as DashboardViewItem;

                        if (View.Id == "Testparameter_ListView_Test_SampleParameter" || View.Id == "Testparameter_ListView_Test_InternalStandards" || View.Id == "Testparameter_ListView_Test_Surrogates")
                        {
                            List<string> lstExistingParams = ((ListView)View).CollectionSource.List.Cast<Testparameter>().Where(i => i.Parameter != null).Select(i => i.Parameter.ParameterName).Distinct().ToList();
                            if (lstExistingParams != null && lstExistingParams.Count > 0)
                            {
                                cs.Criteria["Filter"] = new NotOperator(new InOperator("ParameterName", lstExistingParams));
                            }

                            if (View.Id != "Testparameter_ListView_Test_Surrogates")
                            {
                                if (dvSurrogates != null && dvSurrogates.InnerView != null)
                                {
                                    if (((ListView)dvSurrogates.InnerView).CollectionSource != null && ((ListView)dvSurrogates.InnerView).CollectionSource.GetCount() > 0)
                                    {
                                        List<string> lstSurrogatesParamter = ((ListView)dvSurrogates.InnerView).CollectionSource.List.Cast<Testparameter>().Where(i => i.Parameter != null).Select(i => i.Parameter.ParameterName).Distinct().ToList();
                                        if (lstSurrogatesParamter != null && lstSurrogatesParamter.Count > 0)
                                        {
                                            cs.Criteria["SurrogatesParamter"] = new NotOperator(new InOperator("ParameterName", lstSurrogatesParamter));
                                        }
                                    }
                                }
                            }
                            if (View.Id == "Testparameter_ListView_Test_Surrogates")
                            {
                                if (dvSampleparam != null && dvSampleparam.InnerView != null)
                                {
                                    if (((ListView)dvSampleparam.InnerView).CollectionSource != null && ((ListView)dvSampleparam.InnerView).CollectionSource.GetCount() > 0)
                                    {
                                        List<string> lstAvailableSampleParamter = ((ListView)dvSampleparam.InnerView).CollectionSource.List.Cast<Testparameter>().Where(i => i.Parameter != null).Select(i => i.Parameter.ParameterName).Distinct().ToList();
                                        if (lstAvailableSampleParamter != null && lstAvailableSampleParamter.Count > 0)
                                        {
                                            cs.Criteria["SampleParamFilter"] = new NotOperator(new InOperator("ParameterName", lstAvailableSampleParamter));
                                        }
                                    }
                                }

                                if (dvQCSampleparam != null && dvQCSampleparam.InnerView != null)
                                {
                                    if (((ListView)dvQCSampleparam.InnerView).CollectionSource != null && ((ListView)dvQCSampleparam.InnerView).CollectionSource.GetCount() > 0)
                                    {
                                        List<string> lstAvailableQCSampleparam = ((ListView)dvQCSampleparam.InnerView).CollectionSource.List.Cast<Testparameter>().Where(i => i.Parameter != null).Select(i => i.Parameter.ParameterName).Distinct().ToList();
                                        if (lstAvailableQCSampleparam != null && lstAvailableQCSampleparam.Count > 0)
                                        {
                                            cs.Criteria["QcParameters"] = new NotOperator(new InOperator("ParameterName", lstAvailableQCSampleparam));
                                        }
                                    }
                                }

                                if (dvInternalStandards != null && dvInternalStandards.InnerView != null)
                                {
                                    if (((ListView)dvInternalStandards.InnerView).CollectionSource != null && ((ListView)dvInternalStandards.InnerView).CollectionSource.GetCount() > 0)
                                    {
                                        List<string> lstAvailableInternalStandards = ((ListView)dvInternalStandards.InnerView).CollectionSource.List.Cast<Testparameter>().Where(i => i.Parameter != null).Select(i => i.Parameter.ParameterName).Distinct().ToList();
                                        if (lstAvailableInternalStandards != null && lstAvailableInternalStandards.Count > 0)
                                        {
                                            cs.Criteria["InternalStandards"] = new NotOperator(new InOperator("ParameterName", lstAvailableInternalStandards));
                                        }
                                    }
                                }

                                //if (testInfo.lstSampleParameters != null && testInfo.lstSampleParameters.Count > 0)
                                //{
                                //    cs.Criteria["SampleParamFilter"] = new NotOperator(new InOperator("ParameterName", testInfo.lstSampleParameters));
                                //}
                                //if (testInfo.lstQcParameters != null && testInfo.lstQcParameters.Count > 0)
                                //{
                                //    cs.Criteria["QcParameters"] = new NotOperator(new InOperator("ParameterName", testInfo.lstQcParameters));
                                //}
                                //if (testInfo .lstInternalStandard != null && testInfo.lstInternalStandard.Count > 0)
                                //{
                                //    cs.Criteria["InternalStandard"] = new NotOperator(new InOperator("ParameterName", testInfo.lstInternalStandard));
                                //}
                            }

                        }
                        else if (View.Id == "Testparameter_ListView_Test_QCSampleParameter")
                        {
                            TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                            if (objTest != null && objTest.QCtypesCombo != null)
                            {
                                List<string> lstExistingParams = ((ListView)View).CollectionSource.List.Cast<Testparameter>().Where(i => i.Parameter != null && i.QCType != null && i.QCType.Oid == objTest.QCtypesCombo.Oid).Select(i => i.Parameter.ParameterName).Distinct().ToList();
                                if (testInfo.lstQcParameters != null && testInfo.lstQcParameters.Count > 0 && testInfo.lstQcParameters.FirstOrDefault(i => i.QCType != null && i.QCType.Oid == objTest.QCtypesCombo.Oid) != null)
                                {
                                    foreach (Testparameter testparam in testInfo.lstQcParameters.Where(i => i.QCType.Oid == objTest.QCtypesCombo.Oid).ToList())
                                    {
                                        lstExistingParams.Add(testparam.Parameter.ParameterName);
                                    }
                                }
                                if (lstExistingParams != null && lstExistingParams.Count > 0)
                                {
                                    cs.Criteria["Filter"] = new NotOperator(new InOperator("ParameterName", lstExistingParams));
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectQCTypeinCombo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }

                        ListView lv = Application.CreateListView("Parameter_ListView_Test", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.AcceptAction.Execute += AcceptAction_Execute;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (View.Id == "Component_ListView_Test")
                    {
                        //NestedFrame nestedFrame = (NestedFrame)Frame;
                        //CompositeView view = nestedFrame.ViewItem.View;
                        //DashboardViewItem dvSampleparam = ((DetailView)view).FindItem("Sampleparameter") as DashboardViewItem;
                        //if(dvSampleparam !=null && dvSampleparam.InnerView !=null)
                        //{
                        //    if(testInfo.lstAvailableComponentParam != null && testInfo.lstAvailableComponentParam.Count >0)
                        //    {
                        //        testInfo.lstAvailableComponentParam.Clear();
                        //    }
                        //    if (((ListView)dvSampleparam.InnerView).CollectionSource != null && ((ListView)dvSampleparam.InnerView).CollectionSource.GetCount() > 0)
                        //    {
                        //       testInfo.lstAvailableComponentParam = ((ListView)dvSampleparam.InnerView).CollectionSource.List.Cast<Testparameter>().Where(i => i.Parameter != null).Select(i => i.Parameter.ParameterName).Distinct().ToList();

                        //    }
                        //}

                        IObjectSpace os = Application.CreateObjectSpace();
                        Component objComponent = os.CreateObject<Component>();
                        DetailView dv = Application.CreateDetailView(os, "Component_DetailView_Test", true, objComponent);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters();
                        showViewParameters.CreatedView = dv;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        //dc.SaveOnAccept = false;
                        //dc.CloseOnCurrentObjectProcessing = false;
                        //dc.Accepting += Dc_Accepting;
                        dc.AcceptAction.Execute += AcceptAction_Execute;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (View.Id == "Testparameter_ListView_Test_Component")
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        Component objComponent = (Component)view.CurrentObject;
                        DashboardViewItem dvSampleparam = ((DetailView)Application.MainWindow.View).FindItem("Sampleparameter") as DashboardViewItem;
                        TestMethod obj = (TestMethod)Application.MainWindow.View.CurrentObject;
                        //IList<string> lstAvalCompParam = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid]=?", obj.Oid)).Cast<Testparameter>().Where(i => i.Parameter != null && i.QCType !=null &&  i.QCType.QCTypeName == "Sample").Select(i => i.Parameter.ParameterName).Distinct().ToList();
                        if (objComponent != null && !string.IsNullOrEmpty(objComponent.Components))
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(os, typeof(Parameter));
                            ListView lv = Application.CreateListView("Parameter_LookupListView_Component", cs, false);
                            //if (lstAvalCompParam != null && lstAvalCompParam.Count()>0)
                            //{
                            //    cs.Criteria["CompListParameters"] = new InOperator("ParameterName", lstAvalCompParam);
                            //}
                            if (dvSampleparam != null && dvSampleparam.InnerView == null)
                            {
                                dvSampleparam.CreateControl();
                            }

                            if (dvSampleparam != null && dvSampleparam.InnerView != null)
                            {
                                if (testInfo.lstAvailableComponentParam != null && testInfo.lstAvailableComponentParam.Count > 0)
                                {
                                    testInfo.lstAvailableComponentParam.Clear();
                                }
                                if (((ListView)dvSampleparam.InnerView).CollectionSource != null && ((ListView)dvSampleparam.InnerView).CollectionSource.GetCount() > 0)
                                {
                                    //testInfo.lstAvailableComponentParam = ((ListView)dvSampleparam.InnerView).CollectionSource.List.Cast<Testparameter>().Where(i =>i.TestMethod.Oid == obj.Oid).Select(i => i.Parameter.ParameterName).Distinct().ToList();
                                    testInfo.lstAvailableComponentParam = ((ListView)dvSampleparam.InnerView).CollectionSource.List.Cast<Testparameter>().Where(i => i.QCType != null && i.QCType.QCTypeName == "Sample" && i.TestMethod != null && i.TestMethod.Oid == obj.Oid && i.Component != null && i.Component.Components == "Default").Select(i => i.Parameter.ParameterName).Distinct().ToList();
                                }
                            }

                            if (testInfo.lstAvailableComponentParam != null && testInfo.lstAvailableComponentParam.Count > 0)
                            {
                                cs.Criteria["CompListParametersAvailable"] = new InOperator("ParameterName", testInfo.lstAvailableComponentParam);
                            }

                            List<string> lstExistingParams = ((ListView)View).CollectionSource.List.Cast<Testparameter>().Where(i => i.Parameter.ParameterName != null).Select(i => i.Parameter.ParameterName).Distinct().ToList();
                            if (lstExistingParams != null && lstExistingParams.Count > 0)
                            {
                                cs.Criteria["CompListParametersExisting"] = new NotOperator(new InOperator("ParameterName", lstExistingParams));
                            }

                            ShowViewParameters showViewParameters = new ShowViewParameters();
                            showViewParameters.CreatedView = lv;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            //dc.SaveOnAccept = false;
                            //dc.CloseOnCurrentObjectProcessing = false;
                            //dc.Accepting += Dc_Accepting;
                            dc.AcceptAction.Execute += AcceptAction_Execute;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Enter the Component Name", InformationType.Error, 3000, InformationPosition.Top);
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

        private void TestGroup_AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (grptstinfo.lsttempgrouptest == null)
                {
                    grptstinfo.lsttempgrouptest = new List<GroupTestMethod>();
                }
                //IObjectSpace os = Application.CreateObjectSpace();
                TestMethod objcruttm = (TestMethod)Application.MainWindow.View.CurrentObject;
                foreach (Testparameter objtp in e.SelectedObjects)
                {
                    ////Testparameter objtp = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtp.Oid));
                    if (View.Id == "GroupTestMethod_ListView_IsGroup" && objtp != null)
                    {
                        GroupTestMethod objgtm = View.ObjectSpace.CreateObject<GroupTestMethod>();
                        objgtm.TestMethod = View.ObjectSpace.GetObject<TestMethod>(objcruttm);
                        objgtm.Tests = View.ObjectSpace.GetObject<TestMethod>(objtp.TestMethod);
                        objgtm.TestParameter = View.ObjectSpace.GetObject<Testparameter>(objtp);
                        objgtm.GroupTestParameters = objtp.GroupTestParameters;
                        if (!string.IsNullOrEmpty(objtp.GroupTestParameters))
                        {
                            objgtm.GroupTestParameter = "Customized";
                        }
                        else
                        {
                            objgtm.GroupTestParameter = "Default";
                        }
                        ((ListView)View).CollectionSource.Add(objgtm);
                        //grptstinfo.lsttempgrouptest.Add(objgtm);
                    }
                }
                ((ListView)View).Refresh();
                View.ObjectSpace.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void viSampleParam_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem viSampleParam = null;
                if (View is DetailView)
                {
                    viSampleParam = ((DetailView)View).FindItem("Sampleparameter") as DashboardViewItem;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }



        private void dcTestprice_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                //if (sender != null)
                //{
                //    DialogController dc = (DialogController)sender;
                //    if (dc != null)
                //    {
                //        if (dc.Window.View.Id == "TestPrice_DetailView")
                //        {
                //            IObjectSpace os = Application.CreateObjectSpace(typeof(TestPrice));
                //            TestPrice crtobjtestprice = (TestPrice)dc.Window.View.CurrentObject;

                //            TestMethod crtobjtp = (TestMethod)Application.MainWindow.View.CurrentObject;
                //            TestMethod objtstmet = os.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", crtobjtp.Oid));
                //            DashboardViewItem dvpertest = ((DetailView)dc.Window.View).FindItem("PerTest") as DashboardViewItem;
                //            DashboardViewItem dvperparameter = ((DetailView)dc.Window.View).FindItem("PerParameter") as DashboardViewItem;
                //            if (dvpertest != null && dvpertest.InnerView != null)
                //            {
                //                ASPxGridListEditor gridlist = ((ListView)dvpertest.InnerView).Editor as ASPxGridListEditor;
                //                if (gridlist != null && gridlist.Grid != null)
                //                {
                //                    gridlist.Grid.UpdateEdit();
                //                }

                //                //ASPxGridListEditor gridlist = ((ListView)dvpertest.InnerView).Editor as ASPxGridListEditor;
                //                //if (gridlist != null && gridlist.Grid != null)
                //                //{
                //                //    gridlist.Grid.UpdateEdit();
                //                //    ObjectSpace.CommitChanges();
                //                //    IList<TestPriceDetail> lsttstprice = ((ListView)dvpertest.InnerView).CollectionSource.List.Cast<TestPriceDetail>().ToList();
                //                //    foreach (TestPriceDetail objtstpr in lsttstprice.ToList())
                //                //    {
                //                //        objtstpr.TestPrice =   testpriceinfo.CurrentTestPrice;
                //                //       ((ListView)dvpertest.InnerView).ObjectSpace.CommitChanges();
                //                //        ObjectSpace.CommitChanges();
                //                //    }
                //                //}

                //                //IList<TestPriceDetail> lsttstprice = ((ListView)dvpertest.InnerView).CollectionSource.List.Cast<TestPriceDetail>().ToList();
                //                //if (lsttstprice != null && lsttstprice.Count > 0)
                //                //{
                //                //    //foreach (TestPrice objtstpr in lsttstprice.ToList())
                //                //    //{
                //                //    //    objtstpr.TestMethod = ((ListView)dvpertest.InnerView).ObjectSpace.GetObject(crtobjtp);
                //                //    //    objtstpr.PriceType = ((ListView)dvpertest.InnerView).ObjectSpace.GetObject(crtobjtestprice.PriceType);
                //                //    //    objtstpr.RegularTAT = ((ListView)dvpertest.InnerView).ObjectSpace.GetObject(crtobjtestprice.RegularTAT);
                //                //    //    objtstpr.FastestTAT = ((ListView)dvpertest.InnerView).ObjectSpace.GetObject(crtobjtestprice.FastestTAT);
                //                //    //    ((ListView)dvpertest.InnerView).ObjectSpace.CommitChanges();
                //                //    //    ObjectSpace.CommitChanges();
                //                //    //}
                //                //    //((ListView)dvpertest.InnerView).ObjectSpace.Refresh();
                //                //    ((ListView)dvpertest.InnerView).ObjectSpace.CommitChanges();
                //                //}

                //                //IList<Guid> lsttestpriceoid = ((ListView)dvpertest.InnerView).CollectionSource.List.Cast<TestPrice>().Where(i => i.TAT == null && i.Price == 0 && i.Surcharge == 0).Select(i => i.Oid).ToList();
                //                //if (lsttestpriceoid != null && lsttestpriceoid.Count > 0)
                //                //{
                //                //    foreach (Guid objguid in lsttestpriceoid.ToList())
                //                //    {
                //                //        TestPrice objtstpri = ObjectSpace.FindObject<TestPrice>(CriteriaOperator.Parse("[Oid] = ?", objguid));
                //                //        if (objtstpri != null)
                //                //        {
                //                //            ObjectSpace.Delete(objtstpri);
                //                //            ObjectSpace.CommitChanges();
                //                //        }
                //                //    }
                //                //    ObjectSpace.Refresh();
                //                //}
                //            }
                //            if (dvperparameter != null && dvperparameter.InnerView != null)
                //            {
                //                ASPxGridListEditor gridlist = ((ListView)dvperparameter.InnerView).Editor as ASPxGridListEditor;
                //                if (gridlist != null && gridlist.Grid != null)
                //                {
                //                    gridlist.Grid.UpdateEdit();
                //                }

                //                //IList<TestPriceDetail> lsttstprice = ((ListView)dvperparameter.InnerView).CollectionSource.List.Cast<TestPriceDetail>().ToList();
                //                //if (lsttstprice != null && lsttstprice.Count > 0)
                //                //{
                //                //    //foreach (TestPrice objtstpr in lsttstprice.ToList())
                //                //    //{
                //                //    //    objtstpr.TestMethod = ((ListView)dvperparameter.InnerView).ObjectSpace.GetObject(crtobjtp);
                //                //    //    objtstpr.PriceType = ((ListView)dvperparameter.InnerView).ObjectSpace.GetObject(crtobjtestprice.PriceType);
                //                //    //    objtstpr.RegularTAT = ((ListView)dvperparameter.InnerView).ObjectSpace.GetObject(crtobjtestprice.RegularTAT);
                //                //    //    objtstpr.FastestTAT = ((ListView)dvperparameter.InnerView).ObjectSpace.GetObject(crtobjtestprice.FastestTAT);

                //                //    //    ((ListView)dvperparameter.InnerView).ObjectSpace.CommitChanges();
                //                //    //}
                //                //    ((ListView)dvperparameter.InnerView).ObjectSpace.CommitChanges();
                //                //    ((ListView)dvperparameter.InnerView).ObjectSpace.Refresh();
                //                //}
                //            }
                //            //IList<Guid> lsttestpriceoid = ((ListView)dvpertest.InnerView).CollectionSource.List.Cast<TestPrice>().Where(i => i.TAT == null && i.BasicParamPrice == 0 && i.BasicParamPriceItemPerUnit == 0 && i.AdditionalParamPriceItemPerUnit == 0 && i.Price == 0 && i.Surcharge == 0).Select(i => i.Oid).ToList();
                //            //IList<TestPrice> lsttestpriceoid = ObjectSpace.GetObjects<TestPrice>(CriteriaOperator.Parse("[TAT] Is Null And [Price] = 0.0 And [Surcharge] = 0 And [AdditionalParamPriceItemPerUnit] = 0.0 And [BasicParamPrice] = 0.0 And [BasicParamPriceItemPerUnit] = 0.0"));
                //            //if (lsttestpriceoid != null && lsttestpriceoid.Count > 0)
                //            //{
                //            //    foreach (TestPrice objguid in lsttestpriceoid.ToList())
                //            //    {
                //            //        ObjectSpace.Delete(objguid);
                //            //        ObjectSpace.CommitChanges();
                //            //    }
                //            //    ObjectSpace.Refresh();
                //            //}
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
        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                TestMethod crtobjtp = (TestMethod)Application.MainWindow.View.CurrentObject;
                if (View != null && View.Id == "Testparameter_ListView_Test_SampleParameter")
                {
                    IObjectSpace spaceSampleParameter = ((ListView)View).ObjectSpace;
                    TestMethod objtstmet = spaceSampleParameter.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", crtobjtp.Oid));
                    QCType objqctype = spaceSampleParameter.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = 'Sample'"));
                    if (testInfo.lstSampleParameters == null)
                    {
                        testInfo.lstSampleParameters = new List<Testparameter>();
                    }

                    foreach (Parameter objParam in e.SelectedObjects)
                    {
                        if (testInfo.lstRemovedSampleParameters != null && testInfo.lstRemovedSampleParameters.FirstOrDefault(i => i.TestMethod != null && i.TestMethod.Oid == objtstmet.Oid && i.QCType.Oid == objqctype.Oid && i.Parameter.Oid == objParam.Oid) != null)
                        {
                            Testparameter objtp = testInfo.lstRemovedSampleParameters.FirstOrDefault(i => i.TestMethod.Oid == objtstmet.Oid && i.QCType.Oid == objqctype.Oid && i.Parameter.Oid == objParam.Oid);
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstSampleParameters.Add(objtp);
                            testInfo.lstRemovedSampleParameters.Remove(objtp);
                        }
                        else
                        {
                            Testparameter objtp = spaceSampleParameter.CreateObject<Testparameter>();
                            objtp.TestMethod = objtstmet;
                            objtp.QCType = objqctype;
                            objtp.Parameter = objParam;
                            IObjectSpace objectspce = Application.CreateObjectSpace();
                            Component objComponent = objectspce.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                            if (objComponent == null)
                            {
                                objComponent = objectspce.CreateObject<Component>();
                                objComponent.Components = "Default";
                                objectspce.CommitChanges();
                            }
                            objtp.Component = spaceSampleParameter.GetObject<Component>(objComponent);
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstSampleParameters.Add(objtp);
                            objectspce.Dispose();
                        }
                        Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, crtobjtp.Oid, crtobjtp.TestCode, "SampleParameter", objParam.ParameterName.ToString());

                    }
                }
                if (View != null && View.Id == "Testparameter_ListView_Test_QCSampleParameter")
                {
                    IObjectSpace SpaceQcparameter = ((ListView)View).ObjectSpace;
                    TestMethod objtstmet = SpaceQcparameter.GetObject<TestMethod>(crtobjtp);
                    QCType objqctype = SpaceQcparameter.GetObjectByKey<QCType>(crtobjtp.QCtypesCombo.Oid);
                    if (testInfo.lstQcParameters == null)
                    {
                        testInfo.lstQcParameters = new List<Testparameter>();
                    }

                    foreach (Parameter objParam in e.SelectedObjects)
                    {
                        if (testInfo.lstRemovedQcParameters != null && testInfo.lstRemovedQcParameters.FirstOrDefault(i => i.TestMethod != null && i.TestMethod.Oid == objtstmet.Oid && i.QCType.Oid == objqctype.Oid && i.Parameter.Oid == objParam.Oid) != null)
                        {
                            Testparameter objtp = testInfo.lstRemovedQcParameters.FirstOrDefault(i => i.TestMethod.Oid == objtstmet.Oid && i.QCType.Oid == objqctype.Oid && i.Parameter.Oid == objParam.Oid);
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstQcParameters.Add(objtp);
                            testInfo.lstRemovedQcParameters.Remove(objtp);
                        }
                        else
                        {
                            Testparameter objtp = SpaceQcparameter.CreateObject<Testparameter>();
                            objtp.TestMethod = objtstmet;
                            objtp.QCType = objqctype;
                            objtp.Parameter = objParam;
                            IObjectSpace objectspce = Application.CreateObjectSpace();
                            Component objComponent = objectspce.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                            if (objComponent == null)
                            {
                                objComponent = objectspce.CreateObject<Component>();
                                objComponent.Components = "Default";
                                objectspce.CommitChanges();
                            }
                            objtp.Component = SpaceQcparameter.GetObject<Component>(objComponent);
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstQcParameters.Add(objtp);
                            objectspce.Dispose();
                        }
                        Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, crtobjtp.Oid, crtobjtp.TestCode, "QCSampleParameter", objParam.ParameterName);

                    }
                    ((ListView)View).Refresh();
                }
                if (View != null && View.Id == "Testparameter_ListView_Test_InternalStandards")
                {
                    IObjectSpace spaceSampleParameter = ((ListView)View).ObjectSpace;
                    TestMethod objtstmet = spaceSampleParameter.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", crtobjtp.Oid));
                    if (testInfo.lstInternalStandard == null)
                    {
                        testInfo.lstInternalStandard = new List<Testparameter>();
                    }
                    if (testInfo.lstRemovedInternalStandard == null)
                    {
                        testInfo.lstRemovedInternalStandard = new List<Testparameter>();
                    }

                    foreach (Parameter objParam in e.SelectedObjects)
                    {
                        if (testInfo.lstRemovedSampleParameters != null && testInfo.lstRemovedSampleParameters.FirstOrDefault(i => i.TestMethod != null && i.TestMethod.Oid == objtstmet.Oid && i.InternalStandard == true && i.Parameter.Oid == objParam.Oid) != null)
                        {
                            Testparameter objtp = testInfo.lstRemovedSampleParameters.FirstOrDefault(i => i.TestMethod.Oid == objtstmet.Oid && i.InternalStandard == true && i.Parameter.Oid == objParam.Oid);
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstInternalStandard.Add(objtp);
                            testInfo.lstRemovedInternalStandard.Remove(objtp);
                        }
                        else
                        {
                            Testparameter objtp = spaceSampleParameter.CreateObject<Testparameter>();
                            objtp.TestMethod = objtstmet;
                            objtp.Parameter = objParam;
                            objtp.InternalStandard = true;
                            IObjectSpace objectspce = Application.CreateObjectSpace();
                            Component objComponent = objectspce.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                            if (objComponent == null)
                            {
                                objComponent = objectspce.CreateObject<Component>();
                                objComponent.Components = "Default";
                                objectspce.CommitChanges();
                            }
                            objtp.Component = spaceSampleParameter.GetObject<Component>(objComponent);
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstInternalStandard.Add(objtp);
                            objectspce.Dispose();
                        }
                        Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, crtobjtp.Oid, crtobjtp.TestCode, "InternalStandards", objParam.ParameterName);

                    }
                }
                if (View != null && View.Id == "Testparameter_ListView_Test_Surrogates")
                {
                    IObjectSpace spaceSampleParameter = ((ListView)View).ObjectSpace;
                    TestMethod objtstmet = spaceSampleParameter.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", crtobjtp.Oid));
                    if (testInfo.lstSurrogates == null)
                    {
                        testInfo.lstSurrogates = new List<Testparameter>();
                    }
                    if (testInfo.lstRemovedSurrogates == null)
                    {
                        testInfo.lstRemovedSurrogates = new List<Testparameter>();
                    }

                    foreach (Parameter objParam in e.SelectedObjects)
                    {
                        if (testInfo.lstRemovedSurrogates != null && testInfo.lstRemovedSurrogates.FirstOrDefault(i => i.TestMethod != null && i.TestMethod.Oid == objtstmet.Oid && i.Surroagate == true && i.Parameter.Oid == objParam.Oid) != null)
                        {
                            Testparameter objtp = testInfo.lstRemovedSampleParameters.FirstOrDefault(i => i.TestMethod.Oid == objtstmet.Oid && i.Surroagate == true && i.Parameter.Oid == objParam.Oid);
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstSurrogates.Add(objtp);
                            testInfo.lstRemovedSurrogates.Remove(objtp);
                        }
                        else
                        {
                            Testparameter objtp = spaceSampleParameter.CreateObject<Testparameter>();
                            objtp.TestMethod = objtstmet;
                            objtp.Parameter = objParam;
                            objtp.Surroagate = true;
                            IObjectSpace objectspce = Application.CreateObjectSpace();
                            Component objComponent = objectspce.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                            if (objComponent == null)
                            {
                                objComponent = objectspce.CreateObject<Component>();
                                objComponent.Components = "Default";
                                objectspce.CommitChanges();
                            }
                            objtp.Component = spaceSampleParameter.GetObject<Component>(objComponent);
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstSurrogates.Add(objtp);
                            objectspce.Dispose();
                        }
                        Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, crtobjtp.Oid, crtobjtp.TestCode, "Surrogates", objParam.ParameterName);

                    }
                }
                if (View != null && View.Id == "Component_ListView_Test")
                {
                    TestMethod objtstmet = os.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", crtobjtp.Oid));
                    //Component objtc = os.CreateObject<Component>();

                    Component objtc = os.GetObject<Component>((Component)e.CurrentObject);
                    if (objtc != null)
                    {
                        objtc.TestMethod = objtstmet;
                        os.CommitChanges();
                        ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(objtc));
                        os.Refresh();
                        View.Refresh();
                    }
                    Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, crtobjtp.Oid, crtobjtp.TestCode, "Component", objtc.Components);

                }
                if(View != null && View.Id== "TestMethod_PrepMethods_ListView")
                {
                    TestMethod objtstmet = os.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", crtobjtp.Oid));
                    //Component objtc = os.CreateObject<Component>();

                    PrepMethod objtc = os.GetObject<PrepMethod>((PrepMethod)e.CurrentObject);
                    if(objtc != null)
                    {
                        Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, crtobjtp.Oid, crtobjtp.TestCode, "PrepMethod", objtc.PrepType.SamplePrepType);

                    }
                }
                else if (View.Id == "Testparameter_ListView_Test_Component")
                {
                    IObjectSpace spaceSampleParameter = ((ListView)View).ObjectSpace;
                    TestMethod objtstmet = spaceSampleParameter.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", crtobjtp.Oid));
                    QCType objqctype = spaceSampleParameter.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = 'Sample'"));
                    if (testInfo.lstSampleParameters == null)
                    {
                        testInfo.lstSampleParameters = new List<Testparameter>();
                    }
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    foreach (Parameter objParam in e.SelectedObjects)
                    {
                        if (testInfo.lstRemovedSampleParameters != null && testInfo.lstRemovedSampleParameters.FirstOrDefault(i => i.TestMethod != null && i.TestMethod.Oid == objtstmet.Oid && i.QCType.Oid == objqctype.Oid && i.Parameter.Oid == objParam.Oid) != null)
                        {
                            Testparameter objtp = testInfo.lstRemovedSampleParameters.FirstOrDefault(i => i.TestMethod.Oid == objtstmet.Oid && i.QCType.Oid == objqctype.Oid && i.Parameter.Oid == objParam.Oid);
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstSampleParameters.Add(objtp);
                            testInfo.lstRemovedSampleParameters.Remove(objtp);
                        }
                        else
                        {
                            Testparameter objtp = spaceSampleParameter.CreateObject<Testparameter>();
                            objtp.TestMethod = objtstmet;
                            objtp.QCType = objqctype;
                            objtp.Parameter = spaceSampleParameter.GetObject<Parameter>(objParam);
                            Component objComponent = spaceSampleParameter.GetObject<Component>(((Component)view.CurrentObject));
                            if (objComponent != null)
                            {
                                objtp.Component = spaceSampleParameter.GetObject<Component>(objComponent);
                            }
                            else
                            {
                                view.ObjectSpace.CommitChanges();
                                Component objComponents = spaceSampleParameter.GetObject<Component>(((Component)view.CurrentObject));
                                objtp.Component = spaceSampleParameter.GetObject<Component>(objComponents);
                            }
                            ((ListView)View).CollectionSource.Add(objtp);
                            testInfo.lstSampleParameters.Add(objtp);
                        }
                        Frame.GetController<AuditlogViewController>().createaudit(ObjectSpace, crtobjtp.Oid, crtobjtp.TestCode, "Parameter", objParam.ParameterName);

                    }
                    spaceSampleParameter.CommitChanges();
                }
                //if (View != null && View.Id == "TestPrice_ListView")
                //{
                //    TestPrice objtm = ObjectSpace.FindObject<TestPrice>(CriteriaOperator.Parse("[Oid] = ?", testpriceinfo.crttestprice));
                //    if (objtm != null)
                //    {
                //        //ObjectSpace.Delete(objtm);
                //        ObjectSpace.CommitChanges();
                //    }
                //    //TestMethod objtstmet = os.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", crtobjtp.Oid));
                //    //TestPrice objtstprice = os.GetObject<TestPrice>((TestPrice)e.CurrentObject);
                //    //if (objtstprice != null)
                //    //{
                //    //    //objtstmet.Testprice = objtstprice;
                //    //    objtstprice.TestMethod = objtstmet;
                //    //    os.CommitChanges();
                //    //    ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(objtstprice));
                //    //    os.Refresh();
                //    //    View.Refresh();
                //    //}
                //}
                gridcount();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void RemoveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = ((ListView)View).ObjectSpace;
                TestMethod objcruttm = (TestMethod)Application.MainWindow.View.CurrentObject;

                if (View.Id == "GroupTestMethod_ListView_IsGroup")
                {
                    foreach (GroupTestMethod objgtm in View.SelectedObjects)
                    {
                        testInfo.delGtestmethod.Add(objgtm);
                        ((ListView)View).CollectionSource.Remove(objgtm);
                    }
                    ((ListView)View).Refresh();
                }
                //else if (View.Id == "TestPriceDetail_ListView_Copy_perparameter")
                //{
                //    if (testpriceinfo.lstRemovedperparameter != null && testpriceinfo.lstRemovedperparameter.Count > 0)
                //    {
                //        foreach (Guid guid in testpriceinfo.lstRemovedperparameter)
                //        {
                //            TestPriceDetail objtp = os.FindObject<TestPriceDetail>(CriteriaOperator.Parse("[Oid] = ?", guid));
                //            if (objtp != null)
                //            {
                //                os.Delete(objtp);
                //                os.CommitChanges();
                //            }
                //            os.Refresh();
                //        }
                //        testpriceinfo.lstRemovedperparameter.Clear();
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage("Select checkbox", InformationType.Info, 3000, InformationPosition.Top);
                //    }
                //}
                //else if (View.Id == "TestPriceDetail_ListView_Copy_pertest")
                //{
                //    if (testpriceinfo.lstremovepertest != null && testpriceinfo.lstremovepertest.Count > 0)
                //    {
                //        foreach (Guid guid in testpriceinfo.lstremovepertest)
                //        {
                //            TestPriceDetail objtp = os.FindObject<TestPriceDetail>(CriteriaOperator.Parse("[Oid] = ?", guid));
                //            if (objtp != null)
                //            {
                //                os.Delete(objtp);
                //                os.CommitChanges();
                //            }
                //            os.Refresh();
                //        }
                //        testpriceinfo.lstremovepertest.Clear();
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage("Select checkbox", InformationType.Info, 3000, InformationPosition.Top);
                //    }

                //}
                else if (View.SelectedObjects.Count > 0)
                {
                    if (View != null && View.ObjectTypeInfo.Type == typeof(Testparameter))
                    {
                        if (testInfo.lstRemovedSampleParameters == null)
                        {
                            testInfo.lstRemovedSampleParameters = new List<Testparameter>();
                        }
                        if (testInfo.lstRemovedQcParameters == null)
                        {
                            testInfo.lstRemovedQcParameters = new List<Testparameter>();
                        }
                        if (testInfo.lstRemovedInternalStandard == null)
                        {
                            testInfo.lstRemovedInternalStandard = new List<Testparameter>();
                        }
                        if (testInfo.lstRemovedSurrogates == null)
                        {
                            testInfo.lstRemovedSurrogates = new List<Testparameter>();
                        }

                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        TestMethod objcruttm1 = (TestMethod)Application.MainWindow.View.CurrentObject;
                        foreach (Testparameter obj in View.SelectedObjects.Cast<Testparameter>().ToList())
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter.Oid] = ?", obj.Oid);
                            SampleParameter objTP = objSpace.FindObject<SampleParameter>(criteria1);
                            var objparm = obj.Parameter;
                            if (objTP != null)
                            {
                                Exception ex = new Exception(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletetestparameter"));
                                throw ex;
                            }
                            else
                            {
                                //Testparameter objdeltp = os.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", obj.Oid));
                                //os.Delete(obj);
                                TestMethod objTestMethod = (TestMethod)Application.MainWindow.View.CurrentObject;
                                IObjectSpace osTestParam = ((ListView)View).ObjectSpace;
                                ((ListView)View).CollectionSource.Remove(obj);

                                IList<Testparameter> lstDelParam = osTestParam.GetObjects<Testparameter>(CriteriaOperator.Parse("[Parameter.Oid]=? AND [TestMethod.Oid]=? And [Component.Components]!=?", obj.Parameter.Oid, objTestMethod.Oid, "Default")).Cast<Testparameter>().ToList();

                                if (lstDelParam != null && lstDelParam.Count > 0)
                                {
                                    foreach (Testparameter objtp in lstDelParam)
                                    {
                                        IList<Component> lstComp = osTestParam.GetObjects<Component>(CriteriaOperator.Parse("[Oid]=?", objtp.Component.Oid));
                                        if (lstComp != null && lstComp.Count > 1)
                                        {
                                            osTestParam.Delete(objtp);
                                        }
                                        else
                                        {
                                            osTestParam.Delete(objtp.Component);
                                            osTestParam.Delete(objtp);
                                        }
                                    }
                                }
                                osTestParam.Delete(obj);
                                osTestParam.CommitChanges();

                                if (View.Id == "Testparameter_ListView_Test_SampleParameter")
                                {
                                    if (testInfo.lstSampleParameters != null && testInfo.lstSampleParameters.FirstOrDefault(i => i.Oid == obj.Oid) != null)
                                    {
                                        testInfo.lstSampleParameters.Remove(testInfo.lstSampleParameters.FirstOrDefault(i => i.Oid == obj.Oid));
                                    }
                                    if (testInfo.lstRemovedSampleParameters != null)
                                    {
                                        testInfo.lstRemovedSampleParameters.Add(obj);
                                    }
                                    Frame.GetController<AuditlogViewController>().createdeleteaudit(ObjectSpace, objTestMethod.Oid, objTestMethod.TestCode, "SampleParameter", objparm.ParameterName);

                                }
                                else if (View.Id == "Testparameter_ListView_Test_QCSampleParameter")
                                {
                                    if (testInfo.lstQcParameters != null && testInfo.lstQcParameters.FirstOrDefault(i => i.Oid == obj.Oid) != null)
                                    {
                                        testInfo.lstQcParameters.Remove(testInfo.lstQcParameters.FirstOrDefault(i => i.Oid == obj.Oid));
                                    }
                                    if (testInfo.lstRemovedQcParameters != null)
                                    {
                                        testInfo.lstRemovedQcParameters.Add(obj);
                                    }
                                    Frame.GetController<AuditlogViewController>().createdeleteaudit(ObjectSpace, objTestMethod.Oid, objTestMethod.TestCode, "QCSampleParameter", objparm.ParameterName);

                                }
                                else if (View.Id == "Testparameter_ListView_Test_InternalStandards")
                                {
                                    if (testInfo.lstInternalStandard != null && testInfo.lstInternalStandard.FirstOrDefault(i => i.Oid == obj.Oid) != null)
                                    {
                                        testInfo.lstInternalStandard.Remove(testInfo.lstInternalStandard.FirstOrDefault(i => i.Oid == obj.Oid));
                                    }
                                    if (testInfo.lstRemovedInternalStandard != null)
                                    {
                                        testInfo.lstRemovedInternalStandard.Add(obj);
                                    }
                                    Frame.GetController<AuditlogViewController>().createdeleteaudit(ObjectSpace, objTestMethod.Oid, objTestMethod.TestCode, "InternalStandards", objparm.ParameterName);

                                }
                                else if (View.Id == "Testparameter_ListView_Test_Surrogates")
                                {
                                    if (testInfo.lstSurrogates != null && testInfo.lstSurrogates.FirstOrDefault(i => i.Oid == obj.Oid) != null)
                                    {
                                        testInfo.lstSurrogates.Remove(testInfo.lstSurrogates.FirstOrDefault(i => i.Oid == obj.Oid));
                                    }
                                    if (testInfo.lstRemovedSurrogates != null)
                                    {
                                        testInfo.lstRemovedSurrogates.Add(obj);
                                    }
                                    Frame.GetController<AuditlogViewController>().createdeleteaudit(ObjectSpace, objTestMethod.Oid, objTestMethod.TestCode, "Surrogates", objparm.ParameterName);

                                }
                                else if (View.Id == "Testparameter_ListView_Test_Surrogates")
                                {
                                    if (testInfo.lstSurrogates != null && testInfo.lstSurrogates.FirstOrDefault(i => i.Oid == obj.Oid) != null)
                                    {
                                        testInfo.lstSurrogates.Remove(testInfo.lstSurrogates.FirstOrDefault(i => i.Oid == obj.Oid));
                                    }
                                    if (testInfo.lstRemovedSurrogates != null)
                                    {
                                        testInfo.lstRemovedSurrogates.Add(obj);
                                    }
                                    Frame.GetController<AuditlogViewController>().createdeleteaudit(ObjectSpace, objTestMethod.Oid, objTestMethod.TestCode, "Surrogates", objparm.ParameterName);

                                }
                            }
                        }
                        //os.CommitChanges();
                        ((ListView)View).Refresh();
                        Application.ShowViewStrategy.ShowMessage("Removed Successfully", InformationType.Success, 3000, InformationPosition.Top);
                        //if (View.Id == "Testparameter_ListView_Test_QCSampleParameter")
                        //{
                        //    TestMethod crtobjtp = (TestMethod)Application.MainWindow.View.CurrentObject;
                        //    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] = ? And [TestMethod.Oid] = ? and [QCType.QCTypeName] <> 'Sample'", crtobjtp.QCtypesCombo.QCTypeName, crtobjtp.Oid);
                        //}
                    }
                    else
                    if (View != null && View.ObjectTypeInfo.Type == typeof(Method))
                    {
                        foreach (Method objtp in View.SelectedObjects)
                        {
                            //Method objdeltp = os.FindObject<Method>(CriteriaOperator.Parse("[Oid] = ?", objtp.Oid));
                            //os.Delete(objdeltp);
                            ((ListView)View).CollectionSource.Remove(os.GetObject(objtp));
                        }
                        os.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage("Deleted Successfully", InformationType.Success, 3000, InformationPosition.Top);
                    }
                    else
                    if (View != null && View.ObjectTypeInfo.Type == typeof(Component))
                    {
                        foreach (Component objtp in View.SelectedObjects.Cast<Component>().ToList())
                        {
                            //Component objdeltp = os.FindObject<Component>(CriteriaOperator.Parse("[Oid] = ?", objtp.Oid));
                            //os.Delete(objdeltp);
                            TestMethod objcruttm1 = (TestMethod)Application.MainWindow.View.CurrentObject;

                            IList<Testparameter> objTestParam = os.GetObjects<Testparameter>(CriteriaOperator.Parse("Component=?", objtp.Oid));
                            if (objTestParam != null && objTestParam.Count > 0)
                            {
                                foreach (Testparameter objTest in objTestParam.ToList())
                                {
                                    var objenew = objTest.Parameter;
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter.Oid] = ?", objTest.Oid);
                                    SampleParameter objTP = os.FindObject<SampleParameter>(criteria1);
                                    if (objTP != null)
                                    {
                                        Exception ex = new Exception(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletetestparameter"));
                                        throw ex;
                                    }
                                    else
                                    {
                                        os.Delete(objTest);
                                        Frame.GetController<AuditlogViewController>().createdeleteaudit(ObjectSpace, objcruttm1.Oid, objcruttm1.TestCode, "Component", objenew.ParameterName);

                                    }
                                }
                            }
                            ((ListView)View).CollectionSource.Remove(os.GetObject(objtp));
                            os.Delete(os.GetObject(objtp));
                        }
                        // os.CommitChanges();

                        Application.ShowViewStrategy.ShowMessage("Deleted Successfully", InformationType.Success, 3000, InformationPosition.Top);
                    }
                    //else
                    //if (View != null && View.ObjectTypeInfo.Type == typeof(TestPrice))
                    //{
                    //    //if (testpriceinfo.lstRemovedTestPrice == null)
                    //    //{
                    //    //    testpriceinfo.lstRemovedTestPrice = new List<TestPrice>();
                    //    //}
                    //    //foreach (TestPrice obj in View.SelectedObjects.Cast<TestPrice>().ToList())
                    //    //{
                    //    //    //CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid] = ?", obj.Oid);
                    //    //    //SampleParameter objTP = objSpace.FindObject<SampleParameter>(criteria1);
                    //    //    //if (objTP != null)
                    //    //    //{
                    //    //    //    Exception ex = new Exception(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletetestparameter"));
                    //    //    //    throw ex;
                    //    //    //}
                    //    //    //else
                    //    //    {
                    //    //        //Testparameter objdeltp = os.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", obj.Oid));
                    //    //        //os.Delete(obj);
                    //    //        ((ListView)View).CollectionSource.Remove(obj);
                    //    //        //ObjectSpace.Delete(obj);
                    //    //        if (View.Id == "TestPrice_ListView")
                    //    //        {
                    //    //            if (testpriceinfo.lsttestprice != null && testpriceinfo.lsttestprice.FirstOrDefault(i => i.Oid == obj.Oid) != null)
                    //    //            {
                    //    //                testpriceinfo.lsttestprice.Remove(testpriceinfo.lsttestprice.FirstOrDefault(i => i.Oid == obj.Oid));
                    //    //            }
                    //    //            if (testpriceinfo.lstRemovedTestPrice != null)
                    //    //            {
                    //    //                testpriceinfo.lstRemovedTestPrice.Add(obj);
                    //    //            } 
                    //    //        }
                    //    //        if (View.Id == "TestPrice_ListView_Copy_perparameter")
                    //    //        {
                    //    //            if (testpriceinfo.lstRemovedperparameter != null && testpriceinfo.lstRemovedperparameter.Count>0)
                    //    //            {
                    //    //                foreach(Guid guid in testpriceinfo.lstRemovedperparameter)
                    //    //                {
                    //    //                    TestPrice objtp = os.FindObject<TestPrice>(CriteriaOperator.Parse("[Oid] = ?", guid));
                    //    //                    if(objtp != null)
                    //    //                    {
                    //    //                        os.Delete(objtp);
                    //    //                        os.CommitChanges();
                    //    //                    }
                    //    //                    os.Refresh();
                    //    //                }
                    //    //            }
                    //    //        }
                    //    //        if (View.Id == "TestPriceDetail_ListView_Copy_pertest")
                    //    //        {
                    //    //            if (testpriceinfo.lstremovepertest != null && testpriceinfo.lstremovepertest.Count > 0)
                    //    //            {
                    //    //                foreach (Guid guid in testpriceinfo.lstremovepertest)
                    //    //                {
                    //    //                    TestPrice objtp = os.FindObject<TestPrice>(CriteriaOperator.Parse("[Oid] = ?", guid));
                    //    //                    if (objtp != null)
                    //    //                    {
                    //    //                        os.Delete(objtp);
                    //    //                        os.CommitChanges();
                    //    //                    }
                    //    //                    os.Refresh();
                    //    //                }
                    //    //            }
                    //    //        }
                    //    //    }
                    //    //}
                    //    //os.CommitChanges();
                    //    TestPrice objtp = (TestPrice)e.CurrentObject;
                    //    IList<TestPriceDetail> lsttpDetails = os.GetObjects<TestPriceDetail>(CriteriaOperator.Parse("[TestPrice] = ?", objtp)).ToList();
                    //    if (lsttpDetails != null && lsttpDetails.Count > 0)
                    //    {
                    //        foreach (TestPriceDetail obj in lsttpDetails)
                    //        {
                    //            os.Delete(objtp);
                    //            os.CommitChanges();
                    //        }
                    //        os.Refresh();
                    //    }
                    //    ((ListView)View).CollectionSource.Remove(os.GetObject(objtp));
                    //    os.Delete(os.GetObject(objtp));
                    //    os.CommitChanges();
                    //    ((ListView)View).Refresh();
                    //    Application.ShowViewStrategy.ShowMessage("Removed Successfully", InformationType.Success, 3000, InformationPosition.Top);
                    //}
                    //ObjectSpace.Refresh();
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select checkbox", InformationType.Info, 3000, InformationPosition.Top);
                }
                gridcount();
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
                if (View.Id == "TestPriceDetail_ListView_Copy_perparameter" || View.Id == "TestPriceDetail_ListView_Copy_pertest")
                {
                    ASPxGridListEditor gridlist = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlist != null && gridlist.Grid != null)
                    {
                        gridlist.Grid.UpdateEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DefaultResultSelect(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                CollectionSource cs = new CollectionSource(ObjectSpace, typeof(ResultDefaultValue));
                ListView lv = Application.CreateListView("ResultDefaultValue_LookupListView", cs, false);
                ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += Dc_Accepting;
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                Testparameter objCurrParam = (Testparameter)View.CurrentObject;
                if (objCurrParam != null && !string.IsNullOrEmpty(objCurrParam.DefaultResult))
                {
                    objCurrParam.DefaultResult = string.Empty;
                    View.Refresh();
                    Session currentSession = ((XPObjectSpace)View.ObjectSpace).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }



        private void DefaultResultAddNew(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(ResultDefaultValue));
                ResultDefaultValue obj = os.CreateObject<ResultDefaultValue>();
                DetailView createdView = Application.CreateDetailView(os, "ResultDefaultValue_DetailView", true, obj);
                createdView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += Dc_AcceptingNewDefaultResult;
                dc.CloseOnCurrentObjectProcessing = true;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_AcceptingNewDefaultResult(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                DialogController dc = sender as DialogController;
                ResultDefaultValue objValue = (ResultDefaultValue)e.AcceptActionArgs.CurrentObject;
                if (dc != null && dc.Frame != null && dc.Frame.View != null && objValue != null)
                {
                    dc.Frame.View.ObjectSpace.CommitChanges();
                    DashboardViewItem dvSampleparam = ((DetailView)Application.MainWindow.View).FindItem("Sampleparameter") as DashboardViewItem;
                    if (dvSampleparam != null && dvSampleparam.InnerView != null && dvSampleparam.InnerView.SelectedObjects.Count == 1)
                    {
                        Testparameter objTestParam = (Testparameter)dvSampleparam.InnerView.CurrentObject;
                        DevExpress.ExpressApp.Web.PopupWindow nestedFrame = (DevExpress.ExpressApp.Web.PopupWindow)Frame;
                        ResultDefaultValue objCurr = nestedFrame.View.ObjectSpace.GetObject(objValue);
                        if (objTestParam != null)
                        {
                            if (string.IsNullOrEmpty(objTestParam.ParameterDefaultResults))
                            {
                                objTestParam.ParameterDefaultResults = objCurr.Oid.ToString();
                            }
                            else
                            {
                                objTestParam.ParameterDefaultResults = objTestParam.ParameterDefaultResults + "; " + objCurr.Oid.ToString();
                            }
                            dvSampleparam.InnerView.ObjectSpace.CommitChanges();
                        }
                        if (objCurr != null && nestedFrame != null)
                        {
                            ((ListView)nestedFrame.View).CollectionSource.Add(objCurr);
                            nestedFrame.View.Refresh();
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
        private void DeleteDefaultResult(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

                if (View.SelectedObjects.Count > 0)
                {
                    List<string> lstGuid = new List<string>();
                    DashboardViewItem dvSampleparam = ((DetailView)Application.MainWindow.View).FindItem("Sampleparameter") as DashboardViewItem;
                    if (dvSampleparam != null && dvSampleparam.InnerView != null && dvSampleparam.InnerView.SelectedObjects.Count == 1)
                    {
                        Testparameter objTest = (Testparameter)dvSampleparam.InnerView.CurrentObject;
                        if (objTest != null && !string.IsNullOrEmpty(objTest.ParameterDefaultResults))
                        {
                            lstGuid = objTest.ParameterDefaultResults.Split(';').Select(i => i.Trim()).ToList();
                        }
                        foreach (ResultDefaultValue objResult in View.SelectedObjects)
                        {
                            View.ObjectSpace.Delete(objResult);
                            if (lstGuid.Contains(objResult.Oid.ToString()))
                            {
                                lstGuid.Remove(objResult.Oid.ToString());
                            }
                        }
                        objTest.ParameterDefaultResults = string.Format("{0}", string.Join("; ", lstGuid.Select(i => i).ToList()));
                    }
                    View.ObjectSpace.CommitChanges();
                    dvSampleparam.InnerView.ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
