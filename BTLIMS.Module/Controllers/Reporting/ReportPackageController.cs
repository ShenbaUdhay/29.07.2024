using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.Reporting
{
    public partial class ReportPackageController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        ReportPackageInfo RPInfo = new ReportPackageInfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        private System.ComponentModel.IContainer components;
        private SimpleAction AddNewReport;
        private SimpleAction Removereports;
        private PopupWindowShowAction EditReport;
        string strPackageName = string.Empty;
        public ReportPackageController()
        {
            InitializeComponent();
            TargetViewId = "ReportPackage_ListView_Copy;" + "ReportPackage_DetailView;" + "ReportPackage_ListView_New;" + "ReportPackage_ListView_ReportName;" + "ReportPackage_DetailView_Edit;" + "ReportPackage_ListView_New_Copy;" + "CustomReportBuilder_ListView;" + "ReportPackage_DetailView_Edit_Copy;";
            newReportPackageAction.TargetViewId = "ReportPackage_ListView_Copy;";
            AddNewReport.TargetViewId = "ReportPackage_ListView_New;";
            saveReportPackageAction.TargetViewId = "ReportPackage_ListView_ReportName;";
            deleteReportPackageAction.TargetViewId = "ReportPackage_ListView_Copy;";
            //newReportAction.TargetViewId = "CustomReportBuilder_ListView";
            Removereports.TargetViewId = "ReportPackage_ListView_New;";
            Removereports.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.ReportPackageIsCreate = false;
                        objPermissionInfo.ReportPackageIsWrite = false;
                        objPermissionInfo.ReportPackageIsDelete = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.ReportPackageIsCreate = true;
                            objPermissionInfo.ReportPackageIsWrite = true;
                            objPermissionInfo.ReportPackageIsDelete = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportPackage" && i.Create == true) != null)
                                {
                                    objPermissionInfo.ReportPackageIsCreate = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportPackage" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ReportPackageIsWrite = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportPackage" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.ReportPackageIsDelete = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                if (View.Id == "ReportPackage_ListView_Copy")
                {
                    //DashboardViewItem vilstReports = ((DashboardView)Application.MainWindow.View).FindItem("Report") as DashboardViewItem;
                    //if (vilstReports != null)
                    //{
                    //    if (vilstReports.Control == null)
                    //    {
                    //        vilstReports.CreateControl();
                    //    }
                    //    ((Control)vilstReports.Control).Visible = false; 
                    //}
                    //vilstReports.Dispose();
                }

                else if (View.Id == "ReportPackage_ListView_New")
                {
                    if (Frame.GetType() != typeof(DevExpress.ExpressApp.Web.PopupWindow))
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(" PackageName=?", RPInfo.CurrentPackageName);
                    }
                }
                if (View.Id == "ReportPackage_DetailView" || View.Id == "ReportPackage_ListView_New")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
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
            e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
        }

        private void PopupControl_CustomizePopupWindowSize(object sender, CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "ReportPackage_DetailView" || e.PopupFrame.View.Id == "ReportPackage_DetailView_Edit_Copy")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(900);
                    e.Height = new System.Web.UI.WebControls.Unit(500);
                    e.Handled = true;
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
                if (View.Id == "ReportPackage_DetailView")
                {
                    if (e.PropertyName == "PackageName")
                    {
                        ReportPackage objRP = (ReportPackage)e.Object;
                        if (objRP != null)
                        {
                            strPackageName = RPInfo.CurrentPackageName = objRP.PackageName;
                        }
                    }
                }
                else if (View.Id == "ReportPackage_ListView_New" && e.PropertyName == "sort")
                {
                    ((ListView)View).CollectionSource.Sorting.OrderBy(i => i.PropertyName == "sort");
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
                //if (View.Id == "ReportPackage_DetailView_Edit" && View.CurrentObject != null)
                //{
                //    if (strPackageName != RPInfo.PackageOldName)
                //    {
                //        IObjectSpace os = Application.CreateObjectSpace();
                //        IList<ReportPackage> lstPackages = os.GetObjects<ReportPackage>(CriteriaOperator.Parse("[PackageName] = ?", RPInfo.PackageOldName));
                //        foreach (ReportPackage package in lstPackages)
                //        {
                //            package.PackageName = strPackageName;
                //        }
                //        os.CommitChanges();
                //    }
                //    RPInfo.PackageOldName = ((ReportPackage)View.CurrentObject).PackageName;
                //}
                //if(View.Id== "ReportPackage_DetailView")
                //{
                //    ReportPackage objRP = (ReportPackage)View.CurrentObject;
                //    if(objRP!=null)
                //    {
                //        objRP.PackageName = strPackageName;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //if (View.Id == "ReportPackage_DetailView_Edit" && View.CurrentObject != null)
                //{
                //    DashboardViewItem vilstReports = (DashboardViewItem)((DetailView)View).FindItem("lstReports");
                //    if (vilstReports != null && vilstReports.InnerView != null)
                //    {
                //        ASPxGridListEditor editor = (ASPxGridListEditor)((ListView)vilstReports.InnerView).Editor;
                //        if (editor != null && editor.Grid != null)
                //        {
                //            editor.Grid.UpdateEdit();
                //        }
                //    }
                //    e.Cancel = true;
                //    if (strPackageName != RPInfo.PackageOldName)
                //    {
                //        IObjectSpace os = Application.CreateObjectSpace();
                //        IList<ReportPackage> lstPackages = os.GetObjects<ReportPackage>(CriteriaOperator.Parse("[PackageName] = ?", RPInfo.PackageOldName));
                //        foreach (ReportPackage package in lstPackages)
                //        {
                //            package.PackageName = strPackageName;
                //        }
                //        os.CommitChanges();
                //    }
                //    RPInfo.PackageOldName = ((ReportPackage)View.CurrentObject).PackageName;
                //    View.Refresh();
                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AddReportToPackageAction_ExecuteCompleted(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "ReportPackage_ListView_ReportName")
                {
                    ObjectSpace.Refresh();
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
                if (base.View != null && base.View.Id == "ReportPackage_ListView_Copy")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct"] = CriteriaOperator.Parse("IsNullOrEmpty([ReportName])");
                    //using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(ReportPackage)))
                    //{
                    //    lstview.Properties.Add(new ViewProperty("PackageName", SortDirection.Ascending, "PackageName", true, true));
                    //    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //    List<object> groups = new List<object>();
                    //    foreach (ViewRecord rec in lstview)
                    //        groups.Add(rec["Toid"]);
                    //    if (groups.Count > 0)
                    //    {
                    //        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                    //    }
                    //}
                    //ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    //if (editor != null && editor.Grid != null)
                    //{
                    //    ASPxGridView gridView = (ASPxGridView)editor.Grid;
                    //    if (gridView != null)
                    //    {
                    //        //gridView.SettingsBehavior.AllowSelectByRowClick = false;
                    //        gridView.SettingsBehavior.AllowSelectByRowClick = true;
                    //        gridView.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    //        //gridView.SettingsBehavior.AllowSelectSingleRowOnly = false;
                    //        gridView.Load += GridView_Load;
                    //        XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    //        callbackManager.RegisterHandler("ReportHandler", this);
                    //        //gridView.ClientSideEvents.RowClick = @"function(s, e) 
                    //        //                                    {
                    //        //                                        RaiseXafCallback(globalCallbackControl, 'ReportHandler', e.visibleIndex, '', false)
                    //        //                                    }";
                    //        gridView.ClientSideEvents.SelectionChanged = @"function(s,e)
                    //                                                            {
                    //                                                              if(e.visibleIndex != '-1')
                    //                                                              {
                    //                                                                s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                    //                                                                if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                    //                                                                    var value = 'ReportHandler|Selected|' + Oidvalue;
                    //                                                                    RaiseXafCallback(globalCallbackControl, 'ReportHandler', value, '', false);    
                    //                                                                }else{
                    //                                                                    var value = 'ProductTemplateselection|UNSelected|' + Oidvalue;
                    //                                                                    RaiseXafCallback(globalCallbackControl, 'ReportHandler', value, '', false);    
                    //                                                                }
                    //                                                              });
                    //                                                             }
                    //                                                            }";

                    //        if (!string.IsNullOrEmpty(RPInfo.NewPackageName))
                    //        {
                    //            IList<ReportPackage> newOid = ((ListView)View).CollectionSource.List.Cast<ReportPackage>().ToList().Where(x => x.PackageName == RPInfo.NewPackageName).ToList();
                    //            if (newOid != null && newOid.Count > 0)
                    //            {
                    //                RPInfo.ReportPackageOid = newOid[0];
                    //            }
                    //            gridView.Selection.SelectRowByKey(newOid[0]);

                    //        }
                    //        if (!string.IsNullOrEmpty(RPInfo.CurrentPackageName))
                    //        {
                    //            IList<ReportPackage> newOid = ((ListView)View).CollectionSource.List.Cast<ReportPackage>().ToList().Where(x => x.PackageName == RPInfo.CurrentPackageName).ToList();
                    //            if (newOid != null && newOid.Count > 0)
                    //            {
                    //                RPInfo.ReportPackageOid = newOid[0];
                    //            }
                    //            gridView.Selection.SelectRowByKey(newOid[0]);
                    //        }
                    //    }
                    //}
                }
                else if (View.Id == "ReportPackage_ListView_ReportName" || View.Id == "ReportPackage_ListView_New")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                    if (View.Id == "ReportPackage_ListView_ReportName" && !RPInfo.IsNewPackage)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[PackageName] = ?", RPInfo.PackageOldName);
                    }
                    else if (View.Id == "ReportPackage_ListView_New")
                    {
                        //RPInfo.IsNewPackage = true;
                        //RemoveReportAction.Enabled.SetItemValue("ShowRemoveReportAction", true);
                        if (Frame.GetType() != typeof(DevExpress.ExpressApp.Web.PopupWindow))
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(" [PackageName] = ? And [ReportName] Is Not Null", RPInfo.CurrentPackageName);
                        }
                    }

                    //VK
                    //if(View.Id == "ReportPackage_ListView_New")
                    //{
                    //    if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                    //    {
                    //        CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                    //        if (cv != null)
                    //        {
                    //            DashboardViewItem viewPackage = ((DashboardView)cv).FindItem("Package") as DashboardViewItem;
                    //            if (viewPackage != null && viewPackage.InnerView != null)
                    //            {
                    //                if(viewPackage.InnerView.SelectedObjects.Count==0)
                    //                {
                    //                   ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(" PackageName=?",RPInfo.CurrentPackageName);
                    //                   View.Refresh();
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
                //else if (View.Id == "ReportPackage_DetailView_Edit" && View.CurrentObject != null)
                //{
                //    ReportPackage package = (ReportPackage)View.CurrentObject;
                //    if (package != null)
                //    {
                //        strPackageName = package.PackageName;
                //    }
                //}
                if (View.Id == "ReportPackage_DetailView")
                {
                    ReportPackage objRP = (ReportPackage)View.CurrentObject;
                    if (objRP != null)
                    {
                        objRP.PackageName = strPackageName;
                    }
                }

                //if(Frame is DevExpress.ExpressApp.Web.PopupWindow)
                //{
                //    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                //    ReportPackage objRP = popupWindow.Application.MainWindow.View.CurrentObject as ReportPackage;
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_Load(object sender, EventArgs e)
        {
            try
            {
                //VK
                //if(View.Id== "ReportPackage_ListView_Copy")
                //{
                //    if (RPInfo.IsNewPackage == true && !string.IsNullOrEmpty(RPInfo.NewPackageName))
                //    {
                //        ASPxGridView view = sender as ASPxGridView;
                //        if (view != null)
                //        {
                //            ReportPackage objNew = ((ListView)View).CollectionSource.List.Cast<ReportPackage>().ToList().Where(x => x.PackageName == RPInfo.NewPackageName).FirstOrDefault();
                //            if (objNew != null)
                //            {
                //                if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                //                {
                //                   // view.Selection.SelectRowByKey(RPInfo.ReportPackageOid.Oid);
                //                    CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                //                    if (cv != null)
                //                    {
                //                        DashboardViewItem lvAvailReports = ((DashboardView)cv).FindItem("Report") as DashboardViewItem;
                //                        if (lvAvailReports != null && lvAvailReports.InnerView != null)
                //                        {
                //                            ReportPackage objReportPackage = ObjectSpace.FindObject<ReportPackage>(CriteriaOperator.Parse("Oid=?", objNew));
                //                            if (lvAvailReports.InnerView.Id == "ReportPackage_ListView_New")
                //                            {
                //                                ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria.Clear();
                //                                ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(" PackageName=?", objReportPackage.PackageName);
                //                                view.Selection.SelectRowByKey(RPInfo.ReportPackageOid);
                //                                ObjectSpace.Refresh();
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (!string.IsNullOrEmpty(RPInfo.CurrentPackageName))
                //        {
                //            ASPxGridView view = sender as ASPxGridView;
                //            if (view != null)
                //            {
                //                ReportPackage objNew = ((ListView)View).CollectionSource.List.Cast<ReportPackage>().ToList().Where(x => x.PackageName == RPInfo.CurrentPackageName).FirstOrDefault();
                //                if (objNew != null)
                //                {
                //                    if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                //                    {
                //                       // view.Selection.SelectRowByKey(RPInfo.ReportPackageOid.Oid);
                //                        CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                //                        if (cv != null)
                //                        {
                //                            DashboardViewItem lvAvailReports = ((DashboardView)cv).FindItem("Report") as DashboardViewItem;
                //                            if (lvAvailReports != null && lvAvailReports.InnerView != null)
                //                            {
                //                                ReportPackage objReportPackage = ObjectSpace.FindObject<ReportPackage>(CriteriaOperator.Parse("Oid=?", objNew));
                //                                if (lvAvailReports.InnerView.Id == "ReportPackage_ListView_New")
                //                                {
                //                                    ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria.Clear();
                //                                    ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(" PackageName=?", objReportPackage.PackageName);
                //                                    view.Selection.SelectRowByKey(RPInfo.ReportPackageOid);
                //                                    ObjectSpace.Refresh();

                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }



                ASPxGridView view = sender as ASPxGridView;
                ReportPackage objNew = null;
                if (view != null)
                {
                    if (RPInfo.IsNewPackage == true && !string.IsNullOrEmpty(RPInfo.NewPackageName))
                    {
                        objNew = ((ListView)View).CollectionSource.List.Cast<ReportPackage>().ToList().Where(x => x.PackageName == RPInfo.NewPackageName).FirstOrDefault();

                    }
                    else if (!string.IsNullOrEmpty(RPInfo.CurrentPackageName))
                    {
                        objNew = ((ListView)View).CollectionSource.List.Cast<ReportPackage>().ToList().Where(x => x.PackageName == RPInfo.CurrentPackageName).FirstOrDefault();
                    }

                    if (objNew != null)
                    {
                        if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                        {
                            view.Selection.SelectRowByKey(RPInfo.ReportPackageOid.Oid);
                            CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                            if (cv != null)
                            {
                                DashboardViewItem lvAvailReports = ((DashboardView)cv).FindItem("Report") as DashboardViewItem;
                                if (lvAvailReports != null && lvAvailReports.InnerView != null)
                                {
                                    ReportPackage objReportPackage = ObjectSpace.FindObject<ReportPackage>(CriteriaOperator.Parse("Oid=?", objNew));
                                    //if (lvAvailReports.InnerView.Id == "ReportPackage_ListView_New")
                                    //{
                                    //    ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria.Clear();
                                    //    ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(" PackageName=?", objReportPackage.PackageName);
                                    //    ObjectSpace.Refresh();
                                    //}
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

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            try
            {
                if (View.Id == "ReportPackage_DetailView_Edit" && View.CurrentObject != null)
                {
                    View.ObjectSpace.Committing -= ObjectSpace_Committing;
                    View.ObjectSpace.Committed -= ObjectSpace_Committed;
                    if (Frame.GetController<ModificationsController>().Active.Contains("ShowSaveAndCloseAction"))
                    {
                        Frame.GetController<ModificationsController>().Active.RemoveItem("ShowSaveAndCloseAction");
                    }
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

        private void newReportPackageAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                if (View.Id == "ReportPackage_ListView_Copy" || View.Id == "ReportPackage_DetailView_Edit")
                {
                    IObjectSpace reportObjectSpace = Application.CreateObjectSpace();
                    ReportPackage newReportPackage = reportObjectSpace.CreateObject<ReportPackage>();
                    RPInfo.IsNewPackage = true;
                    RPInfo.CurrentPackageName = string.Empty;
                    DetailView createdView = Application.CreateDetailView(reportObjectSpace, newReportPackage);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    e.View = createdView;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewReportAction_Cancel(object sender, EventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                IList<ReportPackage> lstRP = os.GetObjects<ReportPackage>(CriteriaOperator.Parse("[PackageName] = ?", RPInfo.CurrentPackageName));
                IList<ReportPackage> report = new List<ReportPackage>();
                foreach (ReportPackage reobj in lstRP)
                {
                    report.Add(reobj);
                }
                foreach (ReportPackage objRP in report)
                {
                    //os.RemoveFromModifiedObjects(objRP);
                    os.Delete(objRP);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void newReportPackageAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                //if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                //{
                //    CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                //    if (cv != null)
                //    {
                //        DashboardViewItem lvAvailReports = ((DetailView)cv).FindItem("Reportaddpack") as DashboardViewItem;
                //        if (lvAvailReports != null && lvAvailReports.InnerView != null)
                //        {
                if (e.PopupWindowView.Id == "ReportPackage_DetailView")
                {
                    DashboardViewItem lvAvailReports = ((DetailView)e.PopupWindowView).FindItem("Reportaddpack") as DashboardViewItem;
                    if (lvAvailReports != null)
                    {
                        ASPxGridListEditor editor = ((ListView)lvAvailReports.InnerView).Editor as ASPxGridListEditor;
                        editor.Grid.UpdateEdit();
                    }
                }
                //ReportPackage objReportPackage = ObjectSpace.FindObject<ReportPackage>(CriteriaOperator.Parse("Oid=?", objNew));
                //if (lvAvailReports.InnerView.Id == "ReportPackage_ListView_New")
                //{
                //    ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria.Clear();
                //    ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(" PackageName=?", objReportPackage.PackageName);
                //    ObjectSpace.Refresh();
                //}
                //        }
                //    }
                //}
                //if (e.PopupWindowViewCurrentObject != null || e.PopupWindowViewSelectedObjects.Count > 0)
                //{
                //    if (e.PopupWindowView.Id == "ReportPackage_ListView_New")
                //    {
                //        if (RPInfo.NewPackageName != null && !string.IsNullOrEmpty(RPInfo.NewPackageName))
                //        {
                //            IObjectSpace os = e.PopupWindowView.ObjectSpace;
                //            ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)e.PopupWindowView).Editor;
                //            var selection = gridlst.GetSelectedObjects();
                //            IList<ReportPackage> report = new List<ReportPackage>();
                //            foreach (ReportPackage reobj in ((ListView)e.PopupWindowView).CollectionSource.List)
                //            {
                //                if (!selection.Contains(reobj))
                //                {
                //                    report.Add(reobj);
                //                }
                //            }
                //            foreach (ReportPackage package in report)
                //            {
                //                ((ListView)e.PopupWindowView).CollectionSource.Remove(package);
                //                os.RemoveFromModifiedObjects(package);
                //            }
                //            gridlst.Grid.UpdateEdit();
                //            os.CommitChanges();
                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //        }
                //    }
                //    else
                //    {
                //        ReportPackage newReportPackage = (ReportPackage)e.PopupWindowViewCurrentObject;
                //        if (newReportPackage != null && !string.IsNullOrEmpty(newReportPackage.PackageName))
                //        {
                //            RPInfo.NewPackageName = newReportPackage.PackageName;
                //            RPInfo.CurrentPackageName = "";
                //            e.CanCloseWindow = AddReportToPackage(e.ShowViewParameters);
                //        }
                //    }
                //}
                //else
                //{
                //    e.CanCloseWindow = false;
                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public virtual bool AddReportToPackage(ShowViewParameters showViewParameters)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(ReportPackage));
                cs.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([PackageName]) ");
                Session currentSession = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)(this.ObjectSpace)).Session;
                SelectedData sproc = currentSession.ExecuteSproc("SelectReportName_SP", "");
                if (sproc.ResultSet != null)
                {
                    foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                    {
                        ReportPackage report = objspace.CreateObject<ReportPackage>();
                        if (RPInfo.IsNewPackage)
                        {
                            report.PackageName = RPInfo.NewPackageName;
                        }
                        else
                        {
                            report.PackageName = RPInfo.PackageOldName;
                        }
                        report.ReportName = row.Values[0].ToString();
                        if (row.Values[1] != null && !string.IsNullOrEmpty(row.Values[1].ToString()))
                        {
                            report.UserDefinedReportName = row.Values[1].ToString();
                        }
                        report.sort = 0;
                        report.PageDisplay = false;
                        report.PageCount = false;
                        cs.Add(report);
                    }
                }
                ListView CreateListView = Application.CreateListView("ReportPackage_ListView_New", cs, false);
                showViewParameters.CreatedView = CreateListView;
                showViewParameters.TargetWindow = TargetWindow.Current;
                showViewParameters.NewWindowTarget = NewWindowTarget.MdiChild;
                showViewParameters.CreatedView.Caption = strPackageName;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                dc.AcceptAction.Active.SetItemValue("enb", false);
                dc.CancelAction.Active.SetItemValue("enb", false);
                dc.Accepting += Dc_Accepting;
                showViewParameters.Controllers.Add(dc);


            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

            return false;
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)View).Editor;
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    IList<ReportPackage> report = new List<ReportPackage>();
                    foreach (ReportPackage reobj in e.AcceptActionArgs.SelectedObjects)
                    {
                        report.Add(reobj);
                    }
                    foreach (ReportPackage package in report)
                    {
                        ReportPackage obj = ObjectSpace.CreateObject<ReportPackage>();
                        obj.PackageName = RPInfo.CurrentPackageName;
                        obj.ReportName = package.ReportName;
                        obj.ReportId = package.ReportId;
                        //obj.sort = package.sort;
                        ((ListView)View).CollectionSource.Add(obj);
                        ObjectSpace.CommitChanges();
                    }
                    //view.ObjectSpace.CommitChanges();
                    //gridlst.Grid.UpdateEdit();
                    View.Refresh();
                    ObjectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void addReportToPackageAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                RPInfo.NewPackageName = string.Empty;
                RPInfo.IsNewPackage = false;
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(ReportPackage));
                cs.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([PackageName])");
                Session currentSession = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)(this.ObjectSpace)).Session;
                SelectedData sproc = currentSession.ExecuteSproc("SelectReportName_SP", "");
                if (sproc.ResultSet != null)
                {
                    foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                    {
                        // ReportPackage report = objspace.FindObject<ReportPackage>(CriteriaOperator.Parse("PackageName=? and ReportName=?", RPInfo.PackageOldName, row.Values[0].ToString()));
                        ReportPackage report = objspace.FindObject<ReportPackage>(CriteriaOperator.Parse("PackageName=? and ReportName=?", RPInfo.CurrentPackageName, row.Values[0].ToString()));
                        if (report == null)
                        {
                            report = objspace.CreateObject<ReportPackage>();
                            report.PackageName = RPInfo.PackageOldName;
                            report.ReportName = row.Values[0].ToString();
                            if (row.Values[1] != null && !string.IsNullOrEmpty(row.Values[1].ToString()))
                            {
                                report.UserDefinedReportName = row.Values[1].ToString();
                            }
                            report.ReportId = Convert.ToInt32(row.Values[2]);
                            report.sort = 0;
                            report.PageDisplay = false;
                            report.PageCount = false;
                            cs.Add(report);
                        }
                    }
                }
                e.View = Application.CreateListView("ReportPackage_ListView_New_Reports", cs, true);
                e.View.Caption = strPackageName;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void addReportToPackageAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                //if (e.PopupWindowViewSelectedObjects.Count > 0)
                //{
                //    IObjectSpace os = e.PopupWindowView.ObjectSpace;
                //    ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)e.PopupWindowView).Editor;
                //    var selection = gridlst.GetSelectedObjects();
                //    IList<ReportPackage> report = new List<ReportPackage>();
                //    foreach (ReportPackage reobj in ((ListView)e.PopupWindowView).CollectionSource.List)
                //    {
                //        if (!selection.Contains(reobj))
                //        {
                //            report.Add(reobj);
                //        }
                //    }
                //    foreach (ReportPackage package in report)
                //    {
                //        ((ListView)e.PopupWindowView).CollectionSource.Remove(package);
                //        os.RemoveFromModifiedObjects(package);
                //    }
                //    gridlst.Grid.UpdateEdit();
                //    os.CommitChanges();
                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //}
                //else
                //{
                //    e.CanCloseWindow = false;
                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                //}

                if (e.PopupWindowViewSelectedObjects.Count > 0)
                {
                    IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                    DashboardViewItem vilstReports = ((DashboardView)Application.MainWindow.View).FindItem("Report") as DashboardViewItem;
                    ListView view = (ListView)vilstReports.InnerView;
                    ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)e.PopupWindowView).Editor;
                    // var selection = gridlst.GetSelectedObjects();
                    IList<ReportPackage> report = new List<ReportPackage>();
                    //foreach (ReportPackage reobj in ((ListView)e.PopupWindowView).CollectionSource.List)
                    //{
                    //    if (!selection.Contains(reobj))
                    //    {
                    //        report.Add(reobj);
                    //    }
                    //}
                    //foreach (ReportPackage package in report)
                    //{
                    //    ((ListView)e.PopupWindowView).CollectionSource.Remove(package);
                    //    os.RemoveFromModifiedObjects(package);
                    //}
                    //gridlst.Grid.UpdateEdit();
                    //os.CommitChanges();
                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                    foreach (ReportPackage reobj in ((ListView)e.PopupWindowView).SelectedObjects)
                    {
                        report.Add(reobj);
                    }
                    foreach (ReportPackage package in report)
                    {
                        ReportPackage obj = view.ObjectSpace.CreateObject<ReportPackage>();
                        obj.PackageName = RPInfo.CurrentPackageName;
                        obj.ReportName = package.ReportName;
                        obj.sort = package.sort;
                        obj.PageDisplay = package.PageDisplay;
                        obj.PageCount = package.PageCount;
                        view.CollectionSource.Add(obj);
                        view.ObjectSpace.CommitChanges();
                    }
                    //view.ObjectSpace.CommitChanges();
                    gridlst.Grid.UpdateEdit();
                    view.Refresh();
                    os.CommitChanges();
                    view.ObjectSpace.Refresh();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    e.CanCloseWindow = false;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void editReportPackageAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //if (View != null && View.Id == "ReportPackage_ListView_Copy" && View.CurrentObject != null)
                //{
                //    ReportPackage report = (ReportPackage)View.CurrentObject;
                //    if (report != null && !string.IsNullOrEmpty(report.PackageName))
                //    {
                //        IObjectSpace reportObjectSpace = Application.CreateObjectSpace();
                //        CollectionSource cs = new CollectionSource(reportObjectSpace, typeof(ReportPackage));
                //        cs.Criteria["reportCriteria"] = CriteriaOperator.Parse("[PackageName]='" + report.PackageName + "'");
                //        ListView CreateListView = Application.CreateListView("ReportPackage_ListView_ReportName", cs, false);
                //        CreateListView.Caption = report.PackageName;
                //        e.ShowViewParameters.CreatedView = CreateListView;
                //    }
                //}

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void saveReportPackageAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ASPxGridListEditor editor = (ASPxGridListEditor)((ListView)View).Editor;
                editor.Grid.UpdateEdit();
                View.ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void deleteReportPackageAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string deletePackageName = string.Empty;
                if (View.Id == "ReportPackage_ListView_Copy")
                {
                    foreach (ReportPackage package in View.SelectedObjects)
                    {
                        IList<ReportPackage> reports = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", package.PackageName));
                        deletePackageName = package.PackageName.ToString();
                        foreach (ReportPackage report in reports.ToList())
                        {
                            ObjectSpace.Delete(report);
                        }
                    }
                }
                else if (View.Id == "ReportPackage_ListView_ReportName")
                {
                    foreach (ReportPackage package in View.SelectedObjects)
                    {
                        ObjectSpace.Delete(package);
                    }
                }

                if (View.Id == "ReportPackage_ListView_New")
                {
                    foreach (ReportPackage package in View.SelectedObjects)
                    {
                        ObjectSpace.Delete(package);
                    }
                }

                DashboardViewItem lvAvailReports = ((DashboardView)Application.MainWindow.View).FindItem("Report") as DashboardViewItem;
                if (lvAvailReports != null && lvAvailReports.InnerView != null)
                {
                    if (lvAvailReports.InnerView.Id == "ReportPackage_ListView_New")
                    {

                        ((ListView)lvAvailReports.InnerView).CollectionSource.List.Clear();
                        ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(" PackageName=?", "");
                        ((ListView)lvAvailReports.InnerView).Refresh();
                        RPInfo.CurrentPackageName = "";
                    }
                }

                ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        //private void newReportAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private void newReportAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "CustomReportBuilder_ListView")
        //        {
        //            IObjectSpace reportObjectSpace = Application.CreateObjectSpace();
        //            Modules.BusinessObjects.SampleManagement.CustomReportBuilder newReportPackage = reportObjectSpace.CreateObject<Modules.BusinessObjects.SampleManagement.CustomReportBuilder>();
        //            // RPInfo.IsNewPackage = true;
        //            DetailView createdView = Application.CreateDetailView(reportObjectSpace, newReportPackage);
        //            createdView.ViewEditMode = ViewEditMode.Edit;
        //            e.View = createdView;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }

        //}

        private void deleteReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardView dv = (DashboardView)Application.MainWindow.View;
                DashboardViewItem viReport = dv.FindItem("Report") as DashboardViewItem;
                var selObjects = ((ListView)View).SelectedObjects;
                var selObjects2 = ((ASPxGridListEditor)((ListView)View).Editor).GetSelectedObjects();
                if (View.Id == "ReportPackage_ListView_New" && View.SelectedObjects.Count > 0)
                {
                    foreach (ReportPackage package in View.SelectedObjects)
                    {
                        ObjectSpace.Delete(package);
                    }
                    ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
                if (View.Id == "ReportPackage_ListView_Copy")
                {
                    if (!string.IsNullOrEmpty(parameter))
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            string[] values = parameter.Split('|');
                            if (values[0] == "ReportHandler")
                            {
                                if (values[1] == "Selected")
                                {
                                    string strSelProductsCriteria = string.Empty;
                                    Guid curguid = new Guid(values[2]);
                                    List<Guid> lstSelProducts = new List<Guid>();
                                    if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                                    {
                                        CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                                        if (cv != null)
                                        {
                                            ReportPackage objReportPackage = ObjectSpace.FindObject<ReportPackage>(CriteriaOperator.Parse("Oid=?", curguid));
                                            DashboardViewItem lvAvailReports = ((DashboardView)cv).FindItem("Report") as DashboardViewItem;
                                            if (objReportPackage != null)
                                            {
                                                //ReportPackage objReportPackage = ObjectSpace.FindObject<ReportPackage>(CriteriaOperator.Parse("Oid=?", curguid));
                                                if (lvAvailReports != null && lvAvailReports.InnerView != null)
                                                {
                                                    if (lvAvailReports.InnerView.Id == "ReportPackage_ListView_New")
                                                    {
                                                        ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria.Clear();
                                                        ((ListView)lvAvailReports.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[PackageName] = ? And [ReportName] Is Not Null", objReportPackage.PackageName);
                                                        RPInfo.CurrentPackageName = objReportPackage.PackageName;
                                                        RPInfo.NewPackageName = "";
                                                        RPInfo.ReportPackageOid = objReportPackage;
                                                        ObjectSpace.Refresh();
                                                    }

                                                }
                                                editor.Grid.Selection.SelectRowByKey(objReportPackage.Oid);
                                            }
                                        }


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

        private void AddNewReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (RPInfo.CurrentPackageName != null && RPInfo.CurrentPackageName != string.Empty)
                {
                    RPInfo.NewPackageName = string.Empty;
                    RPInfo.IsNewPackage = false;
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objspace, typeof(ReportPackage));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([PackageName])");
                    Session currentSession = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)(this.ObjectSpace)).Session;
                    SelectedData sproc = currentSession.ExecuteSproc("SelectReportName_SP", "");
                    if (sproc.ResultSet != null)
                    {
                        foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                        {
                            // ReportPackage report = objspace.FindObject<ReportPackage>(CriteriaOperator.Parse("PackageName=? and ReportName=?", RPInfo.PackageOldName, row.Values[0].ToString()));
                            ReportPackage report = objspace.FindObject<ReportPackage>(CriteriaOperator.Parse("PackageName=? and ReportName=?", RPInfo.CurrentPackageName, row.Values[0].ToString()));
                            if (report == null)
                            {
                                report = objspace.CreateObject<ReportPackage>();
                                report.PackageName = RPInfo.PackageOldName;
                                report.ReportName = row.Values[0].ToString();
                                if (row.Values.Length > 1 && row.Values[1] != null && !string.IsNullOrEmpty(row.Values[1].ToString()))
                                {
                                    report.UserDefinedReportName = row.Values[1].ToString();
                                }
                                if (row.Values.Length > 2 && row.Values[2] != null && !string.IsNullOrEmpty(row.Values[2].ToString()))
                                {
                                    report.ReportId = Convert.ToInt32(row.Values[2]);
                                }
                                report.sort = 0;
                                report.PageDisplay = false;
                                report.PageCount = false;
                                cs.Add(report);
                            }
                        }
                    }
                    ListView ceratelistview = Application.CreateListView("ReportPackage_ListView_New_Reports", cs, true);
                    ceratelistview.Caption = RPInfo.CurrentPackageName;
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = ceratelistview;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.Accepting += Dc_Accepting;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Enter Package Name", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Removereports_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                foreach (ReportPackage objRP in e.SelectedObjects)
                {
                    ObjectSpace.Delete(objRP);
                }
                ObjectSpace.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditReport_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                if (View.Id == "ReportPackage_ListView_Copy")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    ReportPackage objRB = objectSpace.GetObject((ReportPackage)View.CurrentObject);
                    DetailView createdView = Application.CreateDetailView(objectSpace, "ReportPackage_DetailView_Edit_Copy", true, objRB);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    e.View = createdView;
                    strPackageName = RPInfo.CurrentPackageName = objRB.PackageName;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditReport_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (e.PopupWindowView.Id == "ReportPackage_DetailView_Edit_Copy")
                {
                    DashboardViewItem lvAvailReports = ((DetailView)e.PopupWindowView).FindItem("Reportaddpack") as DashboardViewItem;
                    if (lvAvailReports != null)
                    {
                        ASPxGridListEditor editor = ((ListView)lvAvailReports.InnerView).Editor as ASPxGridListEditor;
                        editor.Grid.UpdateEdit();
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
