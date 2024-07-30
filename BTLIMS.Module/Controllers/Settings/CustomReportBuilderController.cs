using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.CustomReportBuilder
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomReportBuilderController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        CustomReportBuilderInfo objCRBinfo = new CustomReportBuilderInfo();
        public CustomReportBuilderController()
        {
            InitializeComponent();
            //TargetViewId = "ReportPackage_ListView_CustomReportBuilder_ReportName;" + "ReportPackage_ListView_Copy_ReportBuilder;" + "CustomReportBuilder_ListView;" + "ReportDesigner_DetailView;";
            TargetViewId = "ReportPackage_ListView_CustomReportBuilder_ReportName;" + "ReportPackage_ListView_Copy_ReportBuilder;" + "CustomReportBuilder_ListView;" + "ReportDesigner_DetailView;" + "CustomReportBuilder_DetailView;";
            btnAddReport.TargetViewId = "ReportPackage_ListView_CustomReportBuilder_ReportName;";
            btnNewPackage.TargetViewId = "ReportPackage_ListView_Copy_ReportBuilder;";
            btnReportDesigner.TargetViewId = "CustomReportBuilder_ListView";

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                //Frame.GetController<DevExpress.ExpressApp.ReportsV2.Web.WebEditReportController>().ShowReportDesignerAction.Category = "RecordEdit";
                Frame.GetController<DevExpress.ExpressApp.ReportsV2.Web.WebEditReportController>().ShowReportDesignerAction.Active.SetItemValue("HideDesignerAction", false);
                Frame.GetController<DevExpress.ExpressApp.ReportsV2.Web.WebEditReportController>().ShowReportDesignerAction.Executing += ShowReportDesignerAction_Executing;
                if (View.Id == "CustomReportBuilder_DetailView")
                {

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

        }

        private void ShowReportDesignerAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
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
                if (View != null && View.Id == "ReportDesigner_DetailView")
                {
                    e.Width = 800;
                    e.Height = 900;
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
            try
            {
                if (View.Id == "ReportPackage_ListView_Copy_ReportBuilder")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {

                        ASPxGridView gridView = (ASPxGridView)editor.Grid;
                        if (gridView != null)
                        {
                            //  gridView.SettingsBehavior.AllowSelectByRowClick = true;
                            // gridView.SettingsBehavior.AllowSelectSingleRowOnly = true;

                            XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                            callbackManager.RegisterHandler("ReportHandler", this);
                            gridView.ClientSideEvents.RowClick = @"function(s, e) 
                                                                {
                                                                    RaiseXafCallback(globalCallbackControl, 'ReportHandler', e.visibleIndex, '', false)
                                                                }";
                            //gridView.ClientSideEvents.SelectionChanged = @"function(s,e)
                            //                                                    {
                            //                                                      if(e.visibleIndex != '-1')
                            //                                                      {
                            //                                                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                            //                                                        if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                            //                                                            var value = 'ProductTemplateselection|Selected|' + Oidvalue;
                            //                                                            RaiseXafCallback(globalCallbackControl, 'ReportHandler', value, '', false);    
                            //                                                        }else{
                            //                                                            var value = 'ProductTemplateselection|UNSelected|' + Oidvalue;
                            //                                                            RaiseXafCallback(globalCallbackControl, 'ReportHandler', value, '', false);    
                            //                                                        }
                            //                                                      });
                            //                                                     }
                            //                                                    }";
                        }
                    }
                }

                if (View.Id == "CustomReportBuilder_ListView")
                {
                    List<Modules.BusinessObjects.SampleManagement.CustomReportBuilder> lstCustom = ((ListView)View).CollectionSource.List.Cast<Modules.BusinessObjects.SampleManagement.CustomReportBuilder>().ToList();
                    if (lstCustom != null && lstCustom.Count() > 0)
                    {
                        foreach (Modules.BusinessObjects.SampleManagement.CustomReportBuilder obj in lstCustom)
                        {
                            if (obj.ReportLayout != null)
                            {
                                byte[] report = System.Text.Encoding.Default.GetBytes(obj.ReportXml);
                                if (report != null && report.Length > 0 && obj.ReportDesignerName == "XXX")
                                {
                                    ReportDataV2 objReportData = (ReportDataV2)View.ObjectSpace.GetObject<ReportDataV2>(obj);
                                    objReportData.DisplayName = "XXX";
                                    obj.ReportLayout = report;
                                    ObjectSpace.CommitChanges();
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            //if (View.Id == "CustomReportBuilder_ListView")
            //{
            //    Frame.GetController<DevExpress.ExpressApp.ReportsV2.Web.WebEditReportController>().ShowReportDesignerAction.Category = "Reports";
            //}
        }

        private void btnNewPackage_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                ReportPackage objReportPackage = os.CreateObject<ReportPackage>();
                DetailView CreatedDetailView = Application.CreateDetailView(os, objReportPackage);
                CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                //showViewParameters.CreatedView.Caption = "SDMS";
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                dc.Accepting += Dc_Accepting;
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

            }
            catch (Exception ex)
            {

            }
        }

        private void btnReportDesigner_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.SampleManagement.CustomReportBuilder objReportBuilder = (Modules.BusinessObjects.SampleManagement.CustomReportBuilder)e.CurrentObject;
                if (objReportBuilder != null && !string.IsNullOrEmpty(objReportBuilder.DisplayName))
                {
                    SimpleAction actionShowReportDesignerAction = Frame.GetController<DevExpress.ExpressApp.ReportsV2.Web.WebEditReportController>().ShowReportDesignerAction;
                    actionShowReportDesignerAction.Active.SetItemValue("HideDesignerAction", true);
                    actionShowReportDesignerAction.DoExecute();
                    actionShowReportDesignerAction.Active.SetItemValue("HideDesignerAction", false);

                    //IObjectSpace os = Application.CreateObjectSpace();
                    //ReportDataV2 obj = os.CreateObject<ReportDataV2>();
                    // ReportDataV2 objReport;
                    // if (objReportBuilder.ReportLayout != null)
                    // {
                    //     objReport = View.ObjectSpace.GetObjectByKey<ReportDataV2>(objReportBuilder.ReportLayout.Oid);
                    // }
                    // else
                    // {
                    //     objReport = View.ObjectSpace.CreateObject<ReportDataV2>();
                    // }
                    // objReport.DisplayName = objReportBuilder.ReportDesignerName;
                    // objCRBinfo.ReportDataOid = objReport;
                    // DetailView createdDetailView = Application.CreateDetailView(View.ObjectSpace, "ReportDesigner_DetailView", false, objReport);
                    // ShowViewParameters showViewParams = new ShowViewParameters(createdDetailView);
                    // showViewParams.Context = TemplateContext.PopupWindow;
                    // showViewParams.TargetWindow = TargetWindow.NewModalWindow;
                    // showViewParams.CreatedView.Caption = "Report Designer";
                    // DialogController dc = Application.CreateController<DialogController>();
                    // dc.SaveOnAccept = false;
                    // dc.CloseOnCurrentObjectProcessing = false;
                    //dc.ViewClosing += Dc_ViewClosing;
                    // dc.ViewClosed += Dc_ViewClosed;
                    // dc.Accepting += Dc_Accepting;
                    // dc.AcceptAction.Active.SetItemValue("Acceptdisable", false);
                    // dc.CancelAction.Active.SetItemValue("Canceldisable", false);
                    // showViewParams.Controllers.Add(dc);
                    // Application.ShowViewStrategy.ShowView(showViewParams, new ShowViewSource(null, null)); 
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Please enter report name", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void Dc_ViewClosed(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "CustomReportBuilder_ListView")
        //        {
        //            //Modules.BusinessObjects.SampleManagement.CustomReportBuilder objCustomReportBuilder = (Modules.BusinessObjects.SampleManagement.CustomReportBuilder)View.CurrentObject;

        //            //objCustomReportBuilder.ReportLayout = ObjectSpace.GetObject<ReportDataV2>(objCRBinfo.ReportDataOid);
        //            //objCustomReportBuilder.CustomReportName = "";
        //            //ObjectSpace.CommitChanges();
        //            //;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void Dc_ViewClosing(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        //if (View.Id == "CustomReportBuilder_ListView")
        //        //{
        //        //    Modules.BusinessObjects.SampleManagement.CustomReportBuilder objCustomReportBuilder = (Modules.BusinessObjects.SampleManagement.CustomReportBuilder)View.CurrentObject;
        //        //    objCustomReportBuilder.ReportLayout = ObjectSpace.GetObjectByKey<ReportDataV2>(objCRBinfo.ReportDataOid.Oid);
        //        //    objCustomReportBuilder.CustomReportName = "";
        //        //    ObjectSpace.CommitChanges();
        //        //    ;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        public void ProcessAction(string parameter)
        {
            try
            {
                try
                {
                    if (View.Id == "ReportPackage_ListView_Copy_ReportBuilder")
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
                                                DashboardViewItem lvAvailProducts = ((DashboardView)cv).FindItem("AvailableProducts") as DashboardViewItem;
                                                DashboardViewItem lvSelProducts = ((DashboardView)cv).FindItem("SelectedProducts") as DashboardViewItem;

                                                if (View.Id == "SpreadSheetBuilder_TemplateInfo_LookupListView_ProductLink")
                                                {
                                                    //SpreadSheetBuilder_TemplateInfo objTemplate = linkInfo.lstTemplates.FirstOrDefault(i => i.Oid == curguid);
                                                    //if (objTemplate == null)
                                                    //{
                                                    //    objTemplate = linkInfo.objectSpace.GetObjectByKey<SpreadSheetBuilder_TemplateInfo>(curguid);
                                                    //}
                                                    //if (objTemplate != null)
                                                    //{
                                                    //    if (linkInfo.lstTemplates.FirstOrDefault(i => i.Oid == objTemplate.Oid) == null)
                                                    //    {
                                                    //        linkInfo.SelTemplate = objTemplate;
                                                    //        linkInfo.lstTemplates.Add(objTemplate);
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        linkInfo.SelTemplate = linkInfo.lstTemplates.FirstOrDefault(i => i.Oid == objTemplate.Oid);
                                                    //    }
                                                    //    lstSelProducts = objTemplate.Products.Select(i => i.Oid).ToList();
                                                    //    if (lstSelProducts != null && lstSelProducts.Count > 0)
                                                    //    {
                                                    //        strSelProductsCriteria = string.Format("('{0}')", string.Join("','", lstSelProducts));
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    linkInfo.SelTemplate = null;
                                                    //}
                                                }

                                                if (!string.IsNullOrEmpty(strSelProductsCriteria))
                                                {
                                                    if (lvAvailProducts != null && lvAvailProducts.InnerView != null)
                                                    {
                                                        ((ListView)lvAvailProducts.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not Oid in" + strSelProductsCriteria);
                                                        ASPxGridListEditor availeditor = ((ListView)lvAvailProducts.InnerView).Editor as ASPxGridListEditor;
                                                        if (availeditor != null && availeditor.Grid != null)
                                                        {
                                                            availeditor.Grid.Selection.UnselectAll();
                                                        }
                                                    }
                                                    if (lvSelProducts != null && lvSelProducts.InnerView != null)
                                                    {
                                                        ((ListView)lvSelProducts.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstSelProducts);
                                                        ASPxGridListEditor availeditor = ((ListView)lvSelProducts.InnerView).Editor as ASPxGridListEditor;
                                                        if (availeditor != null && availeditor.Grid != null)
                                                        {
                                                            availeditor.Grid.Selection.UnselectAll();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (lvAvailProducts != null && lvAvailProducts.InnerView != null)
                                                    {
                                                        ((ListView)lvAvailProducts.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is not null");
                                                        ASPxGridListEditor availeditor = ((ListView)lvAvailProducts.InnerView).Editor as ASPxGridListEditor;
                                                        if (availeditor != null && availeditor.Grid != null)
                                                        {
                                                            availeditor.Grid.Selection.UnselectAll();
                                                        }
                                                    }
                                                    if (lvSelProducts != null && lvSelProducts.InnerView != null)
                                                    {
                                                        ((ListView)lvSelProducts.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstSelProducts);
                                                        ASPxGridListEditor availeditor = ((ListView)lvSelProducts.InnerView).Editor as ASPxGridListEditor;
                                                        if (availeditor != null && availeditor.Grid != null)
                                                        {
                                                            availeditor.Grid.Selection.UnselectAll();
                                                        }
                                                    }
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
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
