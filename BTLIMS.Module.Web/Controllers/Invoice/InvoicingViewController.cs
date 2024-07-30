using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Pdf;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraReports.UI;
using DynamicDesigner;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
//using Microsoft.Azure.Management.Batch.Fluent;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Invoicing;
using Modules.BusinessObjects.Setting.Quotes;
using Modules.BusinessObjects.TestPricing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web;
using static Modules.BusinessObjects.Setting.ProjectCategory;
using Parameter = Modules.BusinessObjects.Setting.Parameter;
using Priority = Modules.BusinessObjects.Setting.Priority;

namespace LDM.Module.Controllers.Invoices
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InvoicingViewController : ViewController, IXafCallbackHandler
    {
        string strfocusedcolumn = string.Empty;
        MessageTimer timer = new MessageTimer();
        ResourceManager rm;
        private IContainer components;
        string CurrentLanguage = string.Empty;
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        bool IsPoupView = false;
        string strEDDId = string.Empty;
        string objInvoiceID = string.Empty;
        string strJobID = string.Empty;
        ICallbackManagerHolder NotSetPrice;
        InvoiceInfo objInvoiceInfo = new InvoiceInfo();
        bool IsReviewRefresh = false;
        private SimpleAction Export;
        private SimpleAction ExportToQuickbook;
        string msg;
        public InvoicingViewController()
        {
            InitializeComponent();
            TargetViewId = "Invoicing_DetailView;" + "Samplecheckin_ListView_InvoiceJobID;" + "InvoicingAnalysisCharge_ListView_Invoice;"
                + "Invoicing_ListView;" + "Invoicing_ListView_View_History;" + "Invoicing_ListView_Review;" + "Invoicing_DetailView_Review;"
                + "Invoicing_DetailView_View_History;" + "InvoicingAnalysisCharge_ListView_Invoice_View;" + "InvoicingAnalysisCharge_ListView_Invoice_Review;"
                + "InvoicingAnalysisCharge_ListView_Invoice_Delivery;" + "Invoicing_ListView_Delivery;" + "Invoicing_DetailView_Delivery;"
                + "Samplecheckin_ListView_Invoice;" + "InvoicingAnalysisCharge_ListView_Queue;" + "Invoicing_DetailView_Queue;" + "Invoicing_ListView_BatchInvoice;"
                + "Samplecheckin_DetailView_Copy_SampleRegistration;" + "Invoicing_ListView_DataCenter;" + "Invoicing_ListView_Review_History;"
                + "Invoicing_DetailView_Review_History;" + "Invoicing_ItemCharges_ListView;" + "Invoicing_ListView_Delivery_History;" + "InvoicingEDDExport_ListView;"
                + "Invoicing_ListView_PendingReview_History;" + "Invoicing_DetailView_PendingReview;" + "ItemChargePricing_ListView_Invoice_Popup;"
                + "Priority_ListView_Invoice;"; /*+ "TurnAroundTime_ListView_Invoice;"*//*+ "TestPriceSurcharge_ListView_Invoice;"*/
            InvoiceSubmit.TargetViewId = "Invoicing_ListView;" + "Invoicing_DetailView;";
            InvoiceSubmit.TargetObjectsCriteria = "Not IsNullOrEmpty([InvoiceID])";
            Invoicehistory.TargetViewId = "Invoicing_ListView;" + "Invoicing_ListView_Delivery;" + "Samplecheckin_ListView_Invoice;";
            InvoiceViewDateFilter.TargetViewId = "Invoicing_ListView_View_History;" + "Invoicing_ListView_Review_History;";
            InvoiceReview.TargetViewId = "Invoicing_ListView_Review;" + "Invoicing_DetailView_Review;";
            InvoicePreview.TargetViewId = "Invoicing_DetailView_PendingReview;" + "Invoicing_DetailView_Review;" + "Invoicing_ListView_Review;" + "Invoicing_ListView;"
                + "Invoicing_DetailView_Queue;" + "Invoicing_ListView_Delivery;" + "Invoicing_ListView_Delivery_History;" + "Invoicing_DetailView_Delivery;";
            InvoicePreviewDC.TargetViewId = "Invoicing_ListView_DataCenter;";
            BatchInvoicing.TargetViewId = "Invoicing_ListView_BatchInvoice;";
            PreInvoicingReport.TargetViewId = "Samplecheckin_DetailView_Copy_SampleRegistration;";
            InvoiceRollback.TargetViewId = "Invoicing_ListView_Review_History;" +  "Invoicing_DetailView_Review_History;" + "Invoicing_DetailView_Delivery;" + "Invoicing_ListView_Delivery;";
            InvoiceReviewHistory.TargetViewId = "Invoicing_ListView_Review;";
            Export.TargetViewId = "InvoicingEDDExport_ListView;";
            Export.Caption = "Export to Sage";
            Add.TargetViewId = "Invoicing_ItemCharges_ListView;";
            Remove.TargetViewId = "Invoicing_ItemCharges_ListView;";
            ExportToQuickbook.TargetViewId = "Invoicing_ListView_Review_History;InvoicingEDDExport_ListView;";
            /*InvoiceEDDDetails.TargetViewId = "InvoicingEDDExport_ListView;"*/
            //InvoicePreview.TargetObjectsCriteria = "Not IsNullOrEmpty([InvoiceID])";

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (Application.MainWindow.View.Id == "Invoicing_DetailView_PendingReview" || Application.MainWindow.View.Id == "Invoicing_DetailView_Delivery" || Application.MainWindow.View.Id == "Invoicing_DetailView_Review_History")
                {
                    Add.Active.SetItemValue("addbtn", false);
                    Remove.Active.SetItemValue("addbtn", false);
                }
                PreInvoicingReport.Active.SetItemValue("valPreInvoice", false);
                Employee currentUser = SecuritySystem.CurrentUser as Employee;

                if (View.Id == "Samplecheckin_ListView_InvoiceJobID")
                {
                    View.ControlsCreated += View_ControlsCreated;
                }
                else if (View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Review")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "Invoicing_DetailView" || View.Id == "Invoicing_DetailView_Queue" || View.Id == "Invoicing_DetailView_Review")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAction.Executed += SaveAction_Executed;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed += DeleteAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed += SaveAndCloseAction_Executed;
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    InvoiceJobIDSelect.Enabled.SetItemValue("valJobID", true);
                    if (View.Id == "Invoicing_DetailView_Queue")
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        IList<Invoicing> LstInvoice = os.GetObjects<Invoicing>(CriteriaOperator.Parse("[Status] = 'PendingInvoicing'"));
                        foreach (Invoicing obj in LstInvoice.ToList())
                        {
                            IList<InvoicingAnalysisCharge> lstInvAnalCharge = os.GetObjects<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Invoice]=?", obj.Oid));
                            foreach (InvoicingAnalysisCharge ObjCharge in lstInvAnalCharge.ToList())
                            {
                                os.Delete(ObjCharge);
                            }
                            os.Delete(obj);
                        }
                        os.CommitChanges();
                        os.Dispose();
                    }

                }

                else if (View.Id == "InvoicingAnalysisCharge_ListView_Invoice" || View.Id == "InvoicingAnalysisCharge_ListView_Queue")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
   					ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    Invoicing invoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                    if (!Application.MainWindow.View.ObjectSpace.IsNewObject(invoice))
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Invoice]=?", invoice.Oid);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is null");
                    }
                }
                if (View.Id == "Invoicing_DetailView_View_History" || View.Id == "Invoicing_DetailView_Review" || View.Id == "Invoicing_DetailView_Delivery"
                     || View.Id == "Invoicing_DetailView_Review_History")
                {
                    InvoiceJobIDSelect.Enabled.SetItemValue("valJobID", false);
                    DashboardViewItem lvAnalysisCharge = ((DetailView)View).FindItem("AnalysisCharge") as DashboardViewItem;
                    if (lvAnalysisCharge != null)
                    {
                        lvAnalysisCharge.ControlCreated += lvAnalysisCharge_ControlCreated;
                    }
                     ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
                }
                else if (View.Id == "Invoicing_ListView" || View.Id == "Invoicing_ListView_Review" || View.Id == "Invoicing_ListView_Review_History")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed += DeleteAction_Executed;
                    if (View.Id == "Invoicing_ListView_Review" || View.Id == "Invoicing_ListView_Review_History")
                    {
                        if (View.Id == "Invoicing_ListView_Review")
                        {
                            IsReviewRefresh = false;
                        }
                        InvoiceReview.Active.SetItemValue("valReview", false);
                        InvoiceRollback.Active.SetItemValue("valRollback", false);
                        if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                        {
                            objPermissionInfo.InvoiceReviewIsWrite = false;


                            if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                            {
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    InvoiceReview.Active.SetItemValue("valReview", true);
                                    InvoiceRollback.Active.SetItemValue("valRollback", true);

                                }
                                else
                                {
                                    foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                                    {
                                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "InvoiceReview" && i.NavigationItem.IsDeleted == false && i.Write == true) != null)
                                        {
                                            InvoiceReview.Active.SetItemValue("valReview", true);
                                            InvoiceRollback.Active.SetItemValue("valRollback", true);
                                        }

                                    }
                                }

                            }



                        }

                    }
                    View.ControlsCreated += View_ControlsCreated;
                }
                else if (View.Id == "Invoicing_ListView_View_History" || View.Id == "Invoicing_ListView_Review_History" || View.Id == "Invoicing_ListView_Delivery_History")
                {
                    if (InvoiceViewDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        InvoiceViewDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        InvoiceViewDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        InvoiceViewDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        InvoiceViewDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        InvoiceViewDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        InvoiceViewDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        InvoiceViewDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    InvoiceViewDateFilter.SelectedIndex = 1;
                    ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced , Now()) <= 3 And [DateInvoiced] Is Not Null");
                }
                else if (View.Id == "Samplecheckin_ListView_Invoice")
                {
                    List<Guid> lstinvoiceOid = new List<Guid>();
                    //List<Guid> lstIndexId = ObjectSpace.GetObjects<PaymentStatus>().Where(i => i.IsInvoiceQueue == true).Select(i => i.Status.Oid).ToList();
                    // IList<SampleParameter> samples = ObjectSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Samplelogin.JobID.Status]  > 1 And [Samplelogin.ExcludeInvoice] = False And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"), new InOperator("Samplelogin.JobID.Index", lstIndexId)));
                    //IList<SampleParameter> samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Status]  > 1 And [Samplelogin.ExcludeInvoice] = False And [Samplelogin.JobID.PaymentStatus.IsInvoiceQueue] = True And([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"));
                    IList<SampleParameter> samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Status]  > 1 And [Samplelogin.ExcludeInvoice] = False And ([Samplelogin.JobID.PaymentStatus.IsInvoiceQueue] = True Or [Status] = 'Reported' ) And([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"));
                    List<Guid> lstScGuid = samples.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID.Oid).Distinct().ToList();
                    //foreach (Guid obj in lstScGuid)
                    //{
                    //    IList<SampleParameter> lstsamplesInvoice = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] <> 'Reported' And [Samplelogin.ExcludeInvoice] = False And [Samplelogin.JobID.Oid]=? ", obj));
                    //    if (lstsamplesInvoice.Count == 0)
                    //    {
                    //        if (!lstinvoiceOid.Contains(obj))
                    //        {
                    //            lstinvoiceOid.Add(obj);
                    //        }
                    //    }
                    //}
                    ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstScGuid);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.CustomGetTotalTooltip += NewObjectAction_CustomGetTotalTooltip;

                }
                if (View.Id == "InvoicingAnalysisCharge_ListView_Queue" || View.Id == "Invoicing_ItemCharges_ListView")
                {
                    View.ControlsCreated += View_ControlsCreated;
                    if (View.Id == "InvoicingAnalysisCharge_ListView_Queue")
                    {
                        objInvoiceInfo.IsDataLoaded = false;
                    }
                    //if(View.Id== "InvoicingAnalysisCharge_ListView_Queue")
                    //{
                    //    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    //}
                }
                if (View.Id == "Invoicing_ItemCharges_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
                if (View.Id == "Invoicing_ItemCharges_ListView")
                {
                    Invoicing crtquotes = null;
                    if (Application.MainWindow.View.Id == "Invoicing_DetailView_Queue" || Application.MainWindow.View.Id == "Invoicing_DetailView_Review" || Application.MainWindow.View.Id == "Invoicing_DetailView_View_History")
                    {
                        crtquotes = (Invoicing)Application.MainWindow.View.CurrentObject;
                    }
                    else if (View.Id == "Invoicing_DetailView_Queue" || View.Id == "Invoicing_DetailView_Review" || View.Id == "Invoicing_DetailView_View_History")
                    {
                        crtquotes = (Invoicing)View.CurrentObject;
                    }
                    else if (objInvoiceInfo.popupcurtinvoice != null)
                    {
                        crtquotes = objInvoiceInfo.popupcurtinvoice;
                    }
                    //CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                    if (crtquotes != null)
                    {
                        ((ListView)View).CollectionSource.Criteria.Clear();
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Invoicing.Oid] = ?", crtquotes.Oid);
                        ((ListView)View).Refresh();
                    }
                }
                if (View.Id == "ItemChargePricing_ListView_Invoice_Popup")
                {
                    objInvoiceInfo.IsItemchargePricingpopupselectall = false;
                    if (objInvoiceInfo.lsttempItemPricingpopup == null)
                    {
                        objInvoiceInfo.lsttempItemPricingpopup = new List<ItemChargePricing>();
                    }
                    //DashboardViewItem dvitemprice = ((DetailView)Application.MainWindow.View).FindItem("DVItemChargePrice") as DashboardViewItem;
                    //if (dvitemprice != null && dvitemprice.InnerView != null)
                    //{
                    //    //foreach (QuotesItemChargePrice objitempricing in ((ListView)dvitemprice.InnerView).CollectionSource.List.Cast<QuotesItemChargePrice>().Where(i=>i.ItemPrice!=null))
                    //    //{
                    //    //    quotesinfo.lsttempItemPricingpopup.Add(objitempricing.ItemPrice);
                    //    //}
                    //    quotesinfo.lsttempItemPricingpopup = ((ListView)dvitemprice.InnerView).CollectionSource.List.Cast<QuotesItemChargePrice>().Where(i => i.ItemPrice != null).Select(i=>i.ItemPrice).ToList();
                    //}
                    ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                    if (lvitemprice != null && lvitemprice.ListView != null)
                    {
                        //foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                        //{
                        //    if (objitempricing.ItemPrice != null)
                        //    {
                        //        quotesinfo.lsttempItemPricingpopup.Add(objitempricing.ItemPrice);
                        //    }
                        //}
                        objInvoiceInfo.lsttempItemPricingpopup = ((ListView)lvitemprice.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().Where(i => i.ItemPrice != null).Select(i => i.ItemPrice).ToList();
                    }
                }
                else if (View.Id == "Invoicing_DetailView_Queue" || View.Id == "InvoicingAnalysisCharge_ListView_Queue")
                {
                    Frame.GetController<RefreshController>().RefreshAction.Executing += RefreshAction_Executing;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewObjectAction_CustomGetTotalTooltip(object sender, CustomGetTotalTooltipEventArgs e)
        {
            try
            {
                e.Tooltip = ((ActionBase)sender).Enabled ? "New Invoice" : null;
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
                Invoicing objInvoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                if (objInvoice != null && !Application.MainWindow.View.ObjectSpace.IsNewObject(objInvoice))
                {
                    if (View is ListView && View.ObjectTypeInfo.Type == typeof(InvoicingAnalysisCharge))
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Invoice]=?", objInvoice.Oid);
                    }
                    else if (View is DetailView)
                    {
                        DashboardViewItem LvAnalysischarge = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                        if (LvAnalysischarge != null && LvAnalysischarge.InnerView != null)
                        {
                            ((ListView)LvAnalysischarge.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Invoice]=?", objInvoice.Oid);
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

        private void Remove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Invoicing_ItemCharges_ListView")
                {
                    if (objInvoiceInfo.lsttempItemPricing == null)
                    {
                        objInvoiceInfo.lsttempItemPricing = new List<InvoicingItemCharge>();
                    }
                    if (objInvoiceInfo.lsttempItemPricing.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        //CRMQuotes crtquotesobj = (CRMQuotes)Application.MainWindow.View.CurrentObject;

                        if (Application.MainWindow.View.Id == "Invoicing_DetailView_Queue" || Application.MainWindow.View.Id == "Invoicing_DetailView_Review" || Application.MainWindow.View.Id == "Invoicing_DetailView_View_History")
                        {
                            Invoicing crtquotesobj = (Invoicing)Application.MainWindow.View.CurrentObject;
                            objInvoiceInfo.lsttempItemPricing.ToList().ForEach(i => { i.Invoicing = null; });
                            foreach (InvoicingItemCharge obj in objInvoiceInfo.lsttempItemPricing.ToList())
                            {
                                ((ListView)View).CollectionSource.Remove(obj);
                            }
                        ((ListView)View).Refresh();
                            decimal finalamt = 0;
                            decimal totalprice = 0;
                            decimal disamt = 0;
                            decimal dispr = 0;
                            ListPropertyEditor lstitemprice = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                            DashboardViewItem lstAnalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                            if (lstitemprice != null)
                            {
                                if (lstitemprice.ListView == null)
                                {
                                    lstitemprice.CreateControl();
                                }
                                if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.FinalAmount);
                                    totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.UnitPrice * i.Qty);
                                }
                            }
                            if (lstAnalysisprice != null)
                            {
                                if (lstAnalysisprice.InnerView == null)
                                {
                                    lstAnalysisprice.CreateControl();
                                }
                                if (((ListView)lstAnalysisprice.InnerView).CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.Amount);
                                    totalprice = totalprice + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.TierPrice * i.Qty);
                                }
                            }
                            Invoicing crtquotes = null;
                            if (Application.MainWindow.View.Id == "Invoicing_DetailView_Queue" || Application.MainWindow.View.Id == "Invoicing_DetailView_Review" || Application.MainWindow.View.Id == "Invoicing_DetailView_View_History")
                            {
                                crtquotes = (Invoicing)Application.MainWindow.View.CurrentObject;
                            }
                            else if (View.Id == "Invoicing_DetailView_Queue" || View.Id == "Invoicing_DetailView_Review" || View.Id == "Invoicing_DetailView_View_History")
                            {
                                crtquotes = (Invoicing)View.CurrentObject;
                            }
                            else if (objInvoiceInfo.popupcurtinvoice != null)
                            {
                                crtquotes = objInvoiceInfo.popupcurtinvoice;
                            }
                            //CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            if (crtquotes != null)
                            {
                                if (finalamt != 0 && totalprice != 0)
                                {
                                    disamt = totalprice - finalamt;
                                    //disamt = finalamt - totalprice;
                                    if (disamt != 0)
                                    {
                                        dispr = ((disamt) / totalprice) * 100;
                                    }
                                    //dispr = ((totalprice - finalamt) / totalprice) * 100;

                                    objInvoiceInfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                }
                                else if (finalamt == 0 && totalprice == 0)
                                {
                                    crtquotes.DetailedAmount = 0;
                                    crtquotes.Amount = 0;
                                    crtquotes.Discount = 0;
                                    crtquotes.DiscountAmount = 0;
                                    //crtquotes.QuotedAmount = 0;
                                }
                            }
                            Invoicing objquotes = Application.MainWindow.View.ObjectSpace.FindObject<Invoicing>(CriteriaOperator.Parse("[Oid] = ?", crtquotes.Oid));
                            if (objquotes != null)
                            {
                                objquotes.DetailedAmount = Math.Round(totalprice, 2);
                                objquotes.Amount = Math.Round(finalamt, 2);
                                //objquotes.QuotedAmount = Math.Round(finalamt, 2);
                                objquotes.Discount = (int)Math.Round(dispr, 2);
                                objquotes.DiscountAmount = Math.Round(disamt, 2);
                            }
                            //objectSpace.CommitChanges();
                            Application.MainWindow.View.Refresh();
                            //NestedFrame nestedFrame = (NestedFrame)Frame;
                            //CompositeView view = nestedFrame.ViewItem.View;
                            //foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                            //{
                            //    if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                            //    {
                            //        frameContainer.Frame.View.ObjectSpace.Refresh();
                            //    }
                            //}
                        }
                        else if (nestedFrame != null && nestedFrame.ViewItem.View != null && nestedFrame.ViewItem.View.Id == "Invoicing_DetailView_Queue")
                        {
                            Invoicing crtquotesobj = (Invoicing)nestedFrame.ViewItem.View.CurrentObject;
                            objInvoiceInfo.lsttempItemPricing.ToList().ForEach(i => { i.Invoicing = null; });
                            foreach (InvoicingItemCharge obj in objInvoiceInfo.lsttempItemPricing.ToList())
                            {
                                ((ListView)View).CollectionSource.Remove(obj);
                            }
                            ((ListView)View).Refresh();
                            decimal finalamt = 0;
                            decimal totalprice = 0;
                            decimal disamt = 0;
                            decimal dispr = 0;
                            ListPropertyEditor lstitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("ItemCharges") as ListPropertyEditor;
                            DashboardViewItem lstAnalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisCharge") as DashboardViewItem;
                            if (lstitemprice != null)
                            {
                                if (lstitemprice.ListView == null)
                                {
                                    lstitemprice.CreateControl();
                                }
                                if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.FinalAmount);
                                    totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.UnitPrice * i.Qty);
                                }
                            }
                            if (lstAnalysisprice != null)
                            {
                                if (lstAnalysisprice.InnerView == null)
                                {
                                    lstAnalysisprice.CreateControl();
                                }
                                if (((ListView)lstAnalysisprice.InnerView).CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.Amount);
                                    totalprice = totalprice + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.TierPrice * i.Qty);
                                }
                            }
                            Invoicing crtquotes = (Invoicing)nestedFrame.ViewItem.View.CurrentObject;
                            if (crtquotes != null)
                            {
                                if (finalamt != 0 && totalprice != 0)
                                {
                                    disamt = totalprice - finalamt;
                                    //disamt = finalamt - totalprice;
                                    if (disamt != 0)
                                    {
                                        dispr = ((disamt) / totalprice) * 100;
                                    }
                                    objInvoiceInfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                }
                                else if (finalamt == 0 && totalprice == 0)
                                {
                                    crtquotes.DetailedAmount = 0;
                                    crtquotes.Amount = 0;
                                    crtquotes.Discount = 0;
                                    crtquotes.DiscountAmount = 0;
                                    //crtquotes.QuotedAmount = 0;
                                }
                            }
                            Invoicing objquotes = nestedFrame.ViewItem.View.ObjectSpace.FindObject<Invoicing>(CriteriaOperator.Parse("[Oid] = ?", crtquotes.Oid));
                            if (objquotes != null)
                            {
                                objquotes.DetailedAmount = Math.Round(totalprice, 2);
                                objquotes.Amount = Math.Round(finalamt, 2);
                                objquotes.Amount = Math.Round(finalamt, 2);
                                objquotes.Discount = (int)Math.Round(dispr, 2);
                                objquotes.DiscountAmount = Math.Round(disamt, 2);
                            }
                            nestedFrame.ViewItem.View.Refresh();
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
        private void Add_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Invoicing_ItemCharges_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(ItemChargePricing));
                    ItemChargePricing objcrtdummy = os.CreateObject<ItemChargePricing>();
                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(ItemChargePricing));
                    ListView lvparameter = Application.CreateListView("ItemChargePricing_ListView_Invoice_Popup", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                    showViewParameters.CreatedView = lvparameter;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += AddItemChargeDC_Accepting;
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

        private void AddItemChargeDC_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                //if (sender != null)
                {
                    decimal finalamt = 0;
                    decimal totalprice = 0;
                    decimal disamt = 0;
                    decimal dispr = 0;
                    //CRMQuotes crtquotesobj = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                    if (Application.MainWindow.View.Id == "Invoicing_DetailView_Queue" || Application.MainWindow.View.Id == "Invoicing_DetailView_View_History" || Application.MainWindow.View.Id == "Invoicing_DetailView_Review")
                    {
                        Invoicing crtInvoiceobj = (Invoicing)Application.MainWindow.View.CurrentObject;
                        if (objInvoiceInfo.lsttempItemPricingpopup != null && objInvoiceInfo.lsttempItemPricingpopup.Count > 0)
                        {
                            InvoicingItemCharge objitempriceNew = null;
                            foreach (ItemChargePricing objitemprice in objInvoiceInfo.lsttempItemPricingpopup.ToList())
                            {
                                //QuotesItemChargePrice objquoteitemcharge = Application.MainWindow.View.ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[ItemPrice.Oid] = ? And [CRMQuotes.Oid] = ?", objitemprice.Oid, crtquotesobj.Oid));
                                InvoicingItemCharge objquoteitemcharge = null;
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    objquoteitemcharge = nestedFrame.View.ObjectSpace.FindObject<InvoicingItemCharge>(CriteriaOperator.Parse("[ItemPrice.Oid] = ? And [Invoicing.Oid] = ?", objitemprice.Oid, crtInvoiceobj.Oid));
                                    if (objquoteitemcharge == null)
                                    {
                                        objitempriceNew = nestedFrame.View.ObjectSpace.CreateObject<InvoicingItemCharge>();
                                        objitempriceNew.UnitPrice = objitemprice.UnitPrice;
                                        objitempriceNew.NpUnitPrice = objitemprice.UnitPrice;
                                        objitempriceNew.Qty = 1;
                                        objitempriceNew.Amount = objitemprice.UnitPrice;
                                        objitempriceNew.FinalAmount = objitemprice.UnitPrice;
                                        objitempriceNew.Discount = 0;
                                        objitempriceNew.ItemPrice = nestedFrame.View.ObjectSpace.GetObjectByKey<ItemChargePricing>(objitemprice.Oid);
                                        objitempriceNew.Invoicing = View.ObjectSpace.GetObject(crtInvoiceobj);
                                        objitempriceNew.Description = objitemprice.Description;
                                        //uow.CommitChanges();
                                        ((ListView)View).CollectionSource.Add(objitempriceNew);
                                        crtInvoiceobj.ItemCharges.Add(objitempriceNew);
                                    }
                                }
                                else
                                {
                                    objquoteitemcharge = Application.MainWindow.View.ObjectSpace.FindObject<InvoicingItemCharge>(CriteriaOperator.Parse("[ItemPrice.Oid] = ? And [Invoicing.Oid] = ?", objitemprice.Oid, crtInvoiceobj.Oid));
                                    if (objquoteitemcharge == null)
                                    {
                                        objitempriceNew = Application.MainWindow.View.ObjectSpace.CreateObject<InvoicingItemCharge>();
                                        objitempriceNew.UnitPrice = objitemprice.UnitPrice;
                                        objitempriceNew.NpUnitPrice = objitemprice.UnitPrice;
                                        objitempriceNew.Qty = 1;
                                        objitempriceNew.Amount = objitemprice.UnitPrice;
                                        objitempriceNew.FinalAmount = objitemprice.UnitPrice;
                                        objitempriceNew.Discount = 0;
                                        objitempriceNew.ItemPrice = Application.MainWindow.View.ObjectSpace.GetObjectByKey<ItemChargePricing>(objitemprice.Oid);
                                        objitempriceNew.Invoicing = View.ObjectSpace.GetObject(crtInvoiceobj);
                                        objitempriceNew.Description = objitemprice.Description;
                                        //uow.CommitChanges();
                                        ((ListView)View).CollectionSource.Add(objitempriceNew);
                                        crtInvoiceobj.ItemCharges.Add(objitempriceNew);
                                    }
                                }
                            }
                            ((ListView)View).Refresh();
                        }
                        DashboardViewItem dvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                        //ListPropertyEditor lstAnalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as ListPropertyEditor;
                        //if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                        //{
                        //    foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                        //    {
                        //        finalamt = finalamt + objanapricing.FinalAmount;
                        //        totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                        //    }
                        //}
                        ListPropertyEditor lstitemprice = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                        //if (lvitemprice != null && lvitemprice.ListView != null)
                        //{
                        //    foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                        //    {
                        //        finalamt = finalamt + objitempricing.FinalAmount;
                        //        totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                        //    }
                        //}
                        if (lstitemprice != null)
                        {
                            if (lstitemprice.ListView == null)
                            {
                                lstitemprice.CreateControl();
                            }
                            if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                            {
                                finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.FinalAmount);
                                totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.UnitPrice * i.Qty);
                            }
                        }
                        //if (lstAnalysisprice != null)
                        //{
                        //    if (lstAnalysisprice.ListView == null)
                        //    {
                        //        lstAnalysisprice.CreateControl();
                        //    }
                        //    if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                        //    {
                        //        finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.Amount);
                        //        totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.TierPrice * i.Qty);
                        //    }
                        //}
                        if (dvanalysisprice != null)
                        {
                            if (dvanalysisprice.InnerView == null)
                            {
                                dvanalysisprice.CreateControl();
                            }
                            if (((ListView)dvanalysisprice.InnerView).CollectionSource.GetCount() > 0)
                            {
                                finalamt = finalamt + ((ListView)dvanalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.Amount);
                                totalprice = totalprice + ((ListView)dvanalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.TierPrice * i.Qty);
                            }
                        }
                        Invoicing crtquotes = null;
                        if (Application.MainWindow.View.Id == "Invoicing_DetailView_Queue" || Application.MainWindow.View.Id == "Invoicing_DetailView_Review" || Application.MainWindow.View.Id == "Invoicing_DetailView_View_History")
                        {
                            crtquotes = (Invoicing)Application.MainWindow.View.CurrentObject;
                        }
                        else if (View.Id == "Invoicing_DetailView_Queue" || View.Id == "Invoicing_DetailView_Review" || View.Id == "Invoicing_DetailView_View_History")
                        {
                            crtquotes = (Invoicing)View.CurrentObject;
                        }
                        else if (objInvoiceInfo.popupcurtinvoice != null)
                        {
                            crtquotes = objInvoiceInfo.popupcurtinvoice;
                        }
                        //CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                        if (crtquotes != null)
                        {
                            if (finalamt != 0 && totalprice != 0)
                            {
                                //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                //disamt = finalamt - totalprice;
                                disamt = totalprice - finalamt;
                                if (disamt != 0)
                                {
                                    dispr = ((disamt) / totalprice) * 100;
                                }
                                //disamt = finalamt - totalprice;
                                objInvoiceInfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                crtquotes.Amount = Math.Round(finalamt, 2);
                                //crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                crtquotes.Discount = (int)Math.Round(dispr, 2);
                                crtquotes.DiscountAmount = Math.Round(disamt, 2);
                            }
                            else if (finalamt == 0 && totalprice == 0)
                            {
                                crtquotes.DetailedAmount = 0;
                                crtquotes.Amount = 0;
                                crtquotes.Discount = 0;
                                crtquotes.DiscountAmount = 0;
                                //crtquotes.QuotedAmount = 0;
                            }
                        }
                        Application.MainWindow.View.Refresh();
                    }
                    else
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem.View != null && nestedFrame.ViewItem.View.Id == "Invoicing_DetailView_Queue")
                        {
                            Invoicing crtquotesobj = (Invoicing)nestedFrame.ViewItem.View.CurrentObject;
                            if (crtquotesobj != null)
                            {
                                if (objInvoiceInfo.lsttempItemPricingpopup != null && objInvoiceInfo.lsttempItemPricingpopup.Count > 0)
                                {
                                    InvoicingItemCharge objitempriceNew = null;
                                    foreach (ItemChargePricing objitemprice in objInvoiceInfo.lsttempItemPricingpopup.ToList())
                                    {
                                        InvoicingItemCharge objquoteitemcharge = nestedFrame.ViewItem.View.ObjectSpace.FindObject<InvoicingItemCharge>(CriteriaOperator.Parse("[ItemPrice.Oid] = ? And [Invoicing.Oid] = ?", objitemprice.Oid, crtquotesobj.Oid));
                                        if (objquoteitemcharge == null)
                                        {
                                            objitempriceNew = nestedFrame.ViewItem.View.ObjectSpace.CreateObject<InvoicingItemCharge>();
                                            objitempriceNew.UnitPrice = objitemprice.UnitPrice;
                                            objitempriceNew.NpUnitPrice = objitemprice.UnitPrice;
                                            objitempriceNew.Qty = 1;
                                            objitempriceNew.Amount = objitemprice.UnitPrice;
                                            objitempriceNew.FinalAmount = objitemprice.UnitPrice;
                                            objitempriceNew.Discount = 0;
                                            objitempriceNew.ItemPrice = nestedFrame.ViewItem.View.ObjectSpace.GetObjectByKey<ItemChargePricing>(objitemprice.Oid);
                                            objitempriceNew.Invoicing = View.ObjectSpace.GetObject(crtquotesobj);
                                            ((ListView)View).CollectionSource.Add(objitempriceNew);
                                            crtquotesobj.ItemCharges.Add(objitempriceNew);
                                        }
                                    }
                                ((ListView)View).Refresh();
                                }
                                DashboardViewItem lstAnalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisCharge") as DashboardViewItem;
                                ListPropertyEditor lstitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("ItemCharges") as ListPropertyEditor;
                                if (lstitemprice != null)
                                {
                                    if (lstitemprice.ListView == null)
                                    {
                                        lstitemprice.CreateControl();
                                    }
                                    if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.FinalAmount);
                                        totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.UnitPrice * i.Qty);
                                    }
                                }
                                if (lstAnalysisprice != null)
                                {
                                    if (lstAnalysisprice.InnerView == null)
                                    {
                                        lstAnalysisprice.CreateControl();
                                    }
                                    if (((ListView)lstAnalysisprice.InnerView).CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.Amount);
                                        totalprice = totalprice + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.TierPrice * i.Qty);
                                    }
                                }
                                Invoicing crtquotes = (Invoicing)nestedFrame.ViewItem.View.CurrentObject;
                                if (crtquotes != null)
                                {
                                    if (finalamt != 0 && totalprice != 0)
                                    {
                                        disamt = totalprice - finalamt;
                                        if (disamt != 0)
                                        {
                                            dispr = ((disamt) / totalprice) * 100;
                                        }
                                        objInvoiceInfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.Amount = Math.Round(finalamt, 2);
                                        //crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = (int)Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                    }
                                    else if (finalamt == 0 && totalprice == 0)
                                    {
                                        crtquotes.DetailedAmount = 0;
                                        crtquotes.Amount = 0;
                                        crtquotes.Discount = 0;
                                        crtquotes.DiscountAmount = 0;
                                        //crtquotes.QuotedAmount = 0;
                                    }
                                }
                                nestedFrame.ViewItem.View.Refresh();
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
        private void SaveAndCloseAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                //ResetNavigationCount();
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
                if (View.Id == "Invoicing_DetailView_Queue")
                {
                    DashboardViewItem liInvoicingAnalysisCharges = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;

                    Invoicing objInvoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                    if (liInvoicingAnalysisCharges != null && liInvoicingAnalysisCharges.InnerView != null)
                    {
                        ASPxGridListEditor gridlisteditor = ((ListView)liInvoicingAnalysisCharges.InnerView).Editor as ASPxGridListEditor;
                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                        {
                            Session currentSession = ((XPObjectSpace)liInvoicingAnalysisCharges.InnerView.ObjectSpace).Session;
                            UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                            InvoicingEDDExport depositEDDExport = uow.FindObject<InvoicingEDDExport>(CriteriaOperator.Parse("[InvoiceID]=?", objInvoice.Oid));
                            if (depositEDDExport == null)
                            {
                                MemoryStream ms = new MemoryStream();
                                ASPxGridView gridView = gridlisteditor.Grid;
                                //gridView.TotalSummary.Clear();
                                gridView.ExportToCsv(ms);
                                InvoicingEDDExport newDepEDDExp = new InvoicingEDDExport(uow);
                                newDepEDDExp.InvoiceID = uow.GetObjectByKey<Invoicing>(objInvoice.Oid);
                                newDepEDDExp.EDDDetail = ms.ToArray();
                                newDepEDDExp.Save();

                            }
                            else
                            {
                                MemoryStream ms = new MemoryStream();
                                ASPxGridView gridView = gridlisteditor.Grid;
                                //gridView.TotalSummary.Clear();
                                gridView.ExportToCsv(ms);
                                depositEDDExport.EDDDetail = ms.ToArray();
                                depositEDDExport.Save();
                            }
                            uow.CommitChanges();
                        }
                    }
                    //ResetNavigationCount();
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
                if (View.ObjectTypeInfo.Type == typeof(Invoicing))
                {
                    //ResetNavigationCount();
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
                if (View.Id == "Samplecheckin_ListView_Invoice")
                {
                    e.Cancel = true;
                    if (((ListView)View).CollectionSource.GetCount() > 0)
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            int clientCount = View.SelectedObjects.Cast<Samplecheckin>().Where(i => i.ClientName != null).Select(i => i.ClientName).Distinct().Count();
                            if (clientCount > 1)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "jobidsameclient"), InformationType.Error, timer.Seconds, InformationPosition.Top);

                            }
                            else
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                Invoicing objNewInvoice = os.CreateObject<Invoicing>();
                                objNewInvoice.Status = InviceStatus.PendingInvoicing;
                                List<Samplecheckin> lstSampleCheckins = View.SelectedObjects.Cast<Samplecheckin>().ToList();
                                List<Guid> lstOid = lstSampleCheckins.Select(i => i.Oid).Distinct().ToList();
                                Guid objClient = lstSampleCheckins.Select(i => i.ClientName.Oid).FirstOrDefault();
                                List<Guid> lstQuoteID = lstSampleCheckins.Where(i => i.QuoteID != null).Select(i => i.QuoteID.Oid).Distinct().ToList();
                                string strJobID = string.Format("{0}", string.Join("; ", lstSampleCheckins.Select(i => i.JobID).Distinct().ToList()));
                                objNewInvoice.JobID = strJobID;
                                if (lstQuoteID.Count > 0)
                                {
                                    CRMQuotes objQuotes = os.FindObject<CRMQuotes>(CriteriaOperator.Parse("[Oid]=?", new Guid(lstQuoteID.FirstOrDefault().ToString())));
                                    if (objQuotes != null)
                                    {
                                        objNewInvoice.QuoteID = objQuotes;
                                        objNewInvoice.QuotedBy = objQuotes.QuotedBy;
                                        objNewInvoice.QuotedDate = objQuotes.QuotedDate;
                                    }
                                }
                                Guid objSample = lstSampleCheckins.OrderByDescending(i => i.JobID).Select(i => i.Oid).FirstOrDefault();
                                if (objSample != null)
                                {
                                    Samplecheckin objcheck = os.GetObjectByKey<Samplecheckin>(objSample);
                                    if (objcheck != null)
                                    {
                                        if (objcheck.InvoiceContact  != null)
                                        {
                                            objNewInvoice.PrimaryContact = os.GetObjectByKey<Contact>(objcheck.InvoiceContact.Oid);
                                            if (objcheck.InvoiceContact.City != null)
                                            {
                                                objNewInvoice.BillCity = objcheck.InvoiceContact.City.CityName;
                                            }
                                            objNewInvoice.BillStreet1 = objcheck.InvoiceContact.Street1;
                                            objNewInvoice.BillStreet2 = objcheck.InvoiceContact.Street2;
                                            objNewInvoice.BillZipCode = objcheck.InvoiceContact.Zip;
                                            if (objcheck.InvoiceContact.Country != null)
                                            {
                                                objNewInvoice.BillCountry = objcheck.InvoiceContact.Country.EnglishLongName;
                                            }
                                            if (objcheck.InvoiceContact.State != null)
                                            {
                                                objNewInvoice.BillCountry = objcheck.InvoiceContact.State.LongName;
                                            }

                                        }
                                        else if (objcheck.ContactName != null)
                                        {
                                            objNewInvoice.PrimaryContact = os.GetObjectByKey<Contact>(objcheck.ContactName.Oid);
                                            if (objcheck.ContactName.City != null)
                                            {
                                                objNewInvoice.BillCity = objcheck.ContactName.City.CityName;
                                            }
                                            objNewInvoice.BillStreet1 = objcheck.ContactName.Street1;
                                            objNewInvoice.BillStreet2 = objcheck.ContactName.Street2;
                                            objNewInvoice.BillZipCode = objcheck.ContactName.Zip;
                                            if (objcheck.ContactName.Country != null)
                                            {
                                                objNewInvoice.BillCountry = objcheck.ContactName.Country.EnglishLongName;
                                            }
                                            if (objcheck.ContactName.State != null)
                                            {
                                                objNewInvoice.BillCountry = objcheck.ContactName.State.LongName;
                                            }

                                        }
                                        objNewInvoice.PO = objcheck.PO;
                                        //objNewInvoice.DueDate = (DateTime)objcheck.DueDate;
                                        if (objcheck.DueDate != null)
                                        {
                                        objNewInvoice.DueDate = (DateTime)objcheck.DueDate;
                                        }
                                        if (objcheck.TAT != null)
                                        {
                                            objNewInvoice.TAT = os.GetObjectByKey<TurnAroundTime>(objcheck.TAT.Oid);
                                        }
                                        if (objcheck.ProjectID != null)
                                        {
                                            objNewInvoice.ProjectID = os.GetObjectByKey<Project>(objcheck.ProjectID.Oid);
                                        }
                                        if (objcheck.ClientName != null)
                                        {
                                            objNewInvoice.AccountNumber = objcheck.ClientName.Account;
                                            objNewInvoice.Client = os.GetObjectByKey<Customer>(objcheck.ClientName.Oid);
                                            objNewInvoice.Email = objNewInvoice.Client.Contacts.Where(i => i.Email != null && i.IsInvoice == true).Select(i => i.Email).FirstOrDefault();
                                        }
                                    }
                                }
                                DetailView dv = Application.CreateDetailView(os, "Invoicing_DetailView_Queue", true, objNewInvoice);
                                dv.ViewEditMode = ViewEditMode.Edit;
                                Frame.SetView(dv);

                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "notpendingjobid"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void lvAnalysisCharge_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem lvAnalysisCharge = ((DetailView)View).FindItem("AnalysisCharge") as DashboardViewItem;
                Invoicing objInvoice = (Invoicing)View.CurrentObject;
                if (lvAnalysisCharge != null && objInvoice != null)
                {
                    ((ListView)lvAnalysisCharge.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Invoice]=?", objInvoice.Oid);
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
                IObjectSpace os = Application.CreateObjectSpace();
                foreach (Invoicing objinvoice in View.SelectedObjects)
                {
                    IList<InvoicingAnalysisCharge> lstInvAnalCharge = os.GetObjects<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Invoice]=?", objinvoice.Oid));
                    if (lstInvAnalCharge.Count > 0)
                    {
                        foreach (InvoicingAnalysisCharge ObjCharge in lstInvAnalCharge.ToList())
                        {
                            IList<SampleParameter> lstSampleparam = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[InvoicingAnalysisCharge]=?", ObjCharge.Oid));
                            if (lstSampleparam.Count > 0)
                            {
                                foreach (SampleParameter Objparam in lstSampleparam.ToList())
                                {
                                    Objparam.InvoiceIsDone = false;
                                    Objparam.InvoicingAnalysisCharge = null;
                                    Objparam.OSSync = true;
                                }
                            }
                            os.Delete(ObjCharge);
                        }
                    }
                    InvoicingEDDExport onjInvoiceEDDExport = os.FindObject<InvoicingEDDExport>(CriteriaOperator.Parse("[InvoiceID]=?", objinvoice.Oid));
                    if (onjInvoiceEDDExport != null)
                    {
                        os.Delete(onjInvoiceEDDExport);
                    }
                    List<Samplecheckin> lstSampleCheckins = os.GetObjects<Samplecheckin>(new InOperator("JobID", objinvoice.JobID.Split(';').ToList().Select(i => i = i.Trim()).ToList())).ToList();
                    foreach (Samplecheckin objSamplecheckin in lstSampleCheckins.ToList())
                    {
                        // IList<Modules.BusinessObjects.SampleManagement.Reporting> reportings = os.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(i => i.JobID != null && i.JobID.JobID == objSamplecheckin.JobID).ToList();
                        IList<SampleParameter> lstSamples1 = os.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                        if (lstSamples1.Count() == lstSamples1.Where(i => i.Status == Samplestatus.Reported).Count() && lstSamples1.Count() == lstSamples1.Where(i => i.InvoiceIsDone == true).Count())
                        {
                            StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=25"));
                            if (status != null)
                            {
                                objSamplecheckin.Index = status;
                            }
                        }
                    }
                    //os.Delete(os.GetObject(objinvoice));
                }
                os.CommitChanges();
                //ResetNavigationCount();
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
                if (View.Id == "InvoicingAnalysisCharge_ListView_Queue" && e.PropertyName == "TAT")
                {
                    if (e.OldValue != e.NewValue)
                    {
                        InvoicingAnalysisCharge objAnaCha = (InvoicingAnalysisCharge)e.Object;
                        if (objAnaCha != null)
                        {
                            TestPriceSurcharge testPriceSurcharges = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And[Component.Components] = ? And Contains([TAT], ?)", objAnaCha.Matrix.MatrixName, objAnaCha.Method.MethodNumber, objAnaCha.Test.TestName, objAnaCha.Component.Components, objAnaCha.TAT.TAT.Trim()));
                            if (testPriceSurcharges != null)
                            {
                                objAnaCha.PriceCode = testPriceSurcharges.PriceCode;
                                objAnaCha.Priority = View.ObjectSpace.GetObjectByKey<Priority>(testPriceSurcharges.Priority.Oid);
                            }
                        }
                    }
                }
                else if ((View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Review" || View.Id == "InvoicingAnalysisCharge_ListView_Queue") && e.PropertyName == "Discount")
                {
                    if (e.OldValue != e.NewValue)
                    {
                        InvoicingAnalysisCharge objAnaCha = (InvoicingAnalysisCharge)e.Object;
                        if (objAnaCha != null)
                        {
                            decimal dblDiscount =   objAnaCha.Discount;
                            decimal dblUnitPrice = objAnaCha.TierPrice;
                            uint intQty = objAnaCha.Qty;
                            if (dblDiscount != null)
                            {
                                decimal dbldiscountamount = dblUnitPrice * (dblDiscount / 100);
                                     
                                decimal totalamount = ((dblUnitPrice) - (dbldiscountamount)) * intQty;
                                objAnaCha.Amount = totalamount;
                            }
                        }
                    }
                }
                if (View.Id == "Invoicing_DetailView_Queue" && View.CurrentObject == e.Object)
                {
                    Invoicing objinvoice = (Invoicing)e.Object;

                    if (objinvoice != null)
                    {
                        if (e.PropertyName == "SameAddressForBothShippingAndBilling")
                        {
                            if (objinvoice.SameAddressForBothShippingAndBilling == true)
                            {
                                objinvoice.ReportStreet1 = objinvoice.BillStreet1;
                                objinvoice.ReportStreet2 = objinvoice.BillStreet2;
                                objinvoice.ReportCity = objinvoice.BillCity;
                                objinvoice.ReportZipCode = objinvoice.BillZipCode;
                                objinvoice.ReportState = objinvoice.BillState;
                                objinvoice.ReportCountry = objinvoice.BillCountry;
                            }
                            else
                            {
                                objinvoice.ReportStreet1 = null;
                                objinvoice.ReportStreet2 = null;
                                objinvoice.ReportCity = null;
                                objinvoice.ReportZipCode = null;
                                objinvoice.ReportState = null;
                                objinvoice.ReportCountry = null;
                            }
                        }
                        else if (e.PropertyName == "TAT")
                        {
                            //Invoicing crtquotes = (Invoicing)e.Object;
                            //DashboardViewItem dvanalysisprice = ((DetailView)View).FindItem("AnalysisCharge") as DashboardViewItem;
                            //if(dvanalysisprice != null && dvanalysisprice.InnerView == null)
                            //{
                            //    ((ListView)dvanalysisprice.InnerView).CreateControls();
                            //}
                            //else
                            //{
                            //    bool IsNotTATavl = false;
                            //    string strIsNotTATavlTest = string.Empty;
                            //    foreach (InvoicingAnalysisCharge objap in ((ListView)dvanalysisprice.InnerView).CollectionSource.List)
                            //    {
                            //        TestPriceSurcharge objtps = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And[Component.Components] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Method.MethodNumber, objap.Test.TestName, objap.Component.Components, objinvoice.TAT.TAT.Trim()));
                            //        if(objtps != null)
                            //        {
                            //            objap.TAT = dvanalysisprice.InnerView.ObjectSpace.GetObject(objinvoice.TAT);
                            //            uint intqty = 0;
                            //            decimal unitamt = 0;
                            //            decimal discntamt = 0;
                            //            decimal surchargevalue = 0;
                            //            decimal schargeamt = 0;
                            //            decimal surchargeprice = 0;
                            //            decimal pcdisamt = 0;
                            //            string tat = string.Empty;
                            //            if (objap.TAT != null && objap.Test != null && objap.Component != null)
                            //            {
                            //                intqty = objap.Qty;
                            //                unitamt = objap.UnitPrice;
                            //                discntamt = objap.Discount;
                            //                //string str = tatoid.ToString().Replace(" ", "");
                            //               // TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components, tatoid.Trim()));
                            //                if (objtps != null)
                            //                {
                            //                    objap.PriceCode = objtps.PriceCode;
                            //                    if (objtps.Surcharge != null)
                            //                    {
                            //                        surchargevalue = (int)objtps.Surcharge;
                            //                    }
                            //                    if (Frame is NestedFrame)
                            //                    {
                            //                        NestedFrame nestedFrame = (NestedFrame)Frame;
                            //                        objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                            //                    }
                            //                    else
                            //                    {
                            //                        objap.Priority = dvanalysisprice.InnerView.ObjectSpace.GetObject(objtps.Priority);
                            //                    }
                            //                    if (objtps.SurchargePrice != null)
                            //                    {
                            //                        surchargeprice = (decimal)objtps.SurchargePrice;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    //objap.NPSurcharge = 0;
                            //                }
                            //            }
                            //            else if (objap.TAT != null && objap.Test != null && objap.IsGroup == true)
                            //            {
                            //                intqty = objap.Qty;
                            //                unitamt = objap.UnitPrice;
                            //                discntamt = objap.Discount;
                            //                //TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, tatoid.Trim()));
                            //                if (objtps != null)
                            //                {
                            //                    objap.PriceCode = objtps.PriceCode;
                            //                    if (objtps.Surcharge != null)
                            //                    {
                            //                        //objap.NPSurcharge = (int)objtps.Surcharge;
                            //                    }
                            //                    if (Frame is NestedFrame)
                            //                    {
                            //                        NestedFrame nestedFrame = (NestedFrame)Frame;
                            //                        objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                            //                    }
                            //                    else
                            //                    {
                            //                        objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                            //                    }
                            //                    //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                            //                    if (objtps.Surcharge != null)
                            //                    {
                            //                        surchargevalue = (int)objtps.Surcharge;
                            //                    }
                            //                    if (objtps.SurchargePrice != null)
                            //                    {
                            //                        surchargeprice = (decimal)objtps.SurchargePrice;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    //objap.NPSurcharge = 0;
                            //                }
                            //            }
                            //            if (surchargevalue > 0)
                            //            {
                            //                if (objap.IsGroup == false)
                            //                {
                            //                    ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                            //                    if (objcps != null && objcps.ChargeType == ChargeType.Test)
                            //                    {
                            //                        ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                            //                        if (objconpricetier != null)
                            //                        {
                            //                            objap.TierNo = objconpricetier.TierNo;
                            //                            objap.From = objconpricetier.From;
                            //                            objap.To = objconpricetier.To;
                            //                            objap.TierPrice = objconpricetier.TierPrice;
                            //                            if (strfocusedcolumn == "Qty")
                            //                            {
                            //                                unitamt = objconpricetier.TierPrice;
                            //                            }
                            //                            schargeamt = unitamt * (surchargevalue / 100);
                            //                            //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                            //                            //unitamt = objconpricetier.TierPrice;
                            //                            //schargeamt = unitamt * (surchargevalue / 100);
                            //                            //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                            //                            if (discntamt > 0)
                            //                            {
                            //                                pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                                objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                                objap.UnitPrice = unitamt;
                            //                            }
                            //                            else if (discntamt < 0)
                            //                            {
                            //                                pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                                objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                                objap.UnitPrice = unitamt;
                            //                            }
                            //                            else
                            //                            {
                            //                                objap.Amount = (objap.TotalUnitPrice) * intqty;
                            //                                objap.UnitPrice = unitamt;
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                            //                            foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                            //                            {
                            //                                objap.TierNo = objconpricetier.TierNo;
                            //                                objap.From = objconpricetier.From;
                            //                                objap.To = objconpricetier.To;
                            //                                objap.TierPrice = objconpricetier.TierPrice;
                            //                                if (strfocusedcolumn == "Qty")
                            //                                {
                            //                                    unitamt = objconpricetier.TierPrice;
                            //                                }
                            //                                schargeamt = unitamt * (surchargevalue / 100);
                            //                                objap.TotalUnitPrice = (unitamt + schargeamt)/* * intqty*/;
                            //                                //unitamt = objconpricetier.TierPrice;
                            //                                //schargeamt = unitamt * (surchargevalue / 100);
                            //                                //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                            //                                if (discntamt > 0)
                            //                                {
                            //                                    pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                                    objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                                    objap.UnitPrice = unitamt;
                            //                                }
                            //                                else if (discntamt < 0)
                            //                                {
                            //                                    pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                                    objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                                    objap.UnitPrice = unitamt;
                            //                                }
                            //                                else
                            //                                {
                            //                                    objap.Amount = (objap.TotalUnitPrice) * intqty;
                            //                                    objap.UnitPrice = unitamt;
                            //                                }
                            //                                break;
                            //                            }
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        //schargeamt = unitamt * (surchargevalue / 100);
                            //                        schargeamt = surchargeprice;
                            //                        objap.TotalUnitPrice = schargeamt;
                            //                        if (discntamt > 0)
                            //                        {
                            //                            pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                            objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                            objap.UnitPrice = unitamt;
                            //                        }
                            //                        else if (discntamt < 0)
                            //                        {
                            //                            pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                            objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                            objap.UnitPrice = unitamt;
                            //                        }
                            //                        else
                            //                        {
                            //                            objap.Amount = (objap.TotalUnitPrice) * intqty;
                            //                            objap.UnitPrice = unitamt;
                            //                        }
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    //schargeamt = unitamt * (surchargevalue / 100);
                            //                    schargeamt = surchargeprice;
                            //                    objap.TotalUnitPrice = schargeamt;
                            //                    if (discntamt > 0)
                            //                    {
                            //                        pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                        objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                        objap.UnitPrice = unitamt;
                            //                    }
                            //                    else if (discntamt < 0)
                            //                    {
                            //                        pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                        objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                        objap.UnitPrice = unitamt;
                            //                    }
                            //                    else
                            //                    {
                            //                        objap.Amount = (objap.TotalUnitPrice) * intqty;
                            //                        objap.UnitPrice = unitamt;
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                if (objap.IsGroup == false)
                            //                {
                            //                    ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                            //                    if (objcps != null && objcps.ChargeType == ChargeType.Test)
                            //                    {
                            //                        ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                            //                        if (objconpricetier != null)
                            //                        {
                            //                            objap.TierNo = objconpricetier.TierNo;
                            //                            objap.From = objconpricetier.From;
                            //                            objap.To = objconpricetier.To;
                            //                            objap.TierPrice = objconpricetier.TierPrice;
                            //                            //objap.TotalTierPrice = objconpricetier.TierPrice /** intqty*/;
                            //                            //unitamt = objconpricetier.TierPrice;
                            //                            if (strfocusedcolumn == "Qty")
                            //                            {
                            //                                unitamt = objconpricetier.TierPrice;
                            //                            }
                            //                            //schargeamt = unitamt * (surchargevalue / 100);
                            //                            objap.TotalUnitPrice = (unitamt + schargeamt)/* * intqty*/;
                            //                            if (discntamt > 0)
                            //                            {
                            //                                pcdisamt = (unitamt) * (discntamt / 100);
                            //                                objap.Amount = ((unitamt) - (pcdisamt)) * intqty;
                            //                                objap.UnitPrice = unitamt;
                            //                            }
                            //                            else if (discntamt < 0)
                            //                            {
                            //                                pcdisamt = (unitamt) * (discntamt / 100);
                            //                                objap.Amount = ((unitamt) - (pcdisamt)) * intqty;
                            //                                objap.UnitPrice = unitamt;
                            //                            }
                            //                            else
                            //                            {
                            //                                objap.Amount = (unitamt) * intqty;
                            //                                objap.UnitPrice = unitamt;
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                            //                            foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                            //                            {
                            //                                objap.TierNo = objconprice.TierNo;
                            //                                objap.From = objconprice.From;
                            //                                objap.To = objconprice.To;
                            //                                objap.TierPrice = objconprice.TierPrice;
                            //                                //unitamt = objconpricetier.TierPrice;
                            //                                //schargeamt = unitamt * (surchargevalue / 100);
                            //                                //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                            //                                if (strfocusedcolumn == "Qty")
                            //                                {
                            //                                    unitamt = objconprice.TierPrice;
                            //                                }
                            //                                schargeamt = unitamt * (surchargevalue / 100);
                            //                                objap.TotalUnitPrice = (unitamt + schargeamt)/* * intqty*/;
                            //                                if (discntamt > 0)
                            //                                {
                            //                                    pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                                    objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                                    objap.UnitPrice = unitamt;
                            //                                }
                            //                                else if (discntamt < 0)
                            //                                {
                            //                                    pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                                    objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                                    objap.UnitPrice = unitamt;
                            //                                }
                            //                                else
                            //                                {
                            //                                    objap.Amount = (objap.TotalUnitPrice) * intqty;
                            //                                    objap.UnitPrice = unitamt;
                            //                                }
                            //                                break;
                            //                            }
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        schargeamt = surchargeprice;
                            //                        objap.TotalUnitPrice = schargeamt;
                            //                        if (discntamt > 0)
                            //                        {
                            //                            pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                            objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                            objap.UnitPrice = unitamt;
                            //                        }
                            //                        else if (discntamt < 0)
                            //                        {
                            //                            pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                            objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                            objap.UnitPrice = unitamt;
                            //                        }
                            //                        else
                            //                        {
                            //                            objap.Amount = (objap.TotalUnitPrice) * intqty;
                            //                            objap.UnitPrice = unitamt;
                            //                        }
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    schargeamt = surchargeprice;
                            //                    objap.TotalUnitPrice = schargeamt;
                            //                    if (discntamt > 0)
                            //                    {
                            //                        pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                        objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                        objap.UnitPrice = unitamt;
                            //                    }
                            //                    else if (discntamt < 0)
                            //                    {
                            //                        pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                            //                        objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                            //                        objap.UnitPrice = unitamt;
                            //                    }
                            //                    else
                            //                    {
                            //                        objap.Amount = (objap.TotalUnitPrice) * intqty;
                            //                        objap.UnitPrice = unitamt;
                            //                    }
                            //                }
                            //            }
                            //            //TurnAroundTime objtat = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", tatoid));
                            //            //if (objtat != null)
                            //            {
                            //                if (Frame is NestedFrame)
                            //                {
                            //                    NestedFrame nestedFrame = (NestedFrame)Frame;
                            //                    objap.TAT = nestedFrame.View.ObjectSpace.GetObject(objinvoice.TAT);
                            //                }
                            //                else
                            //                {
                            //                    objap.TAT = dvanalysisprice.InnerView.ObjectSpace.GetObject(objinvoice.TAT);
                            //                }
                            //            }
                            //                //((ListView)View).Refresh();
                            //            decimal finalamt = 0;
                            //            decimal totalprice = 0;
                            //            decimal disamt = 0;
                            //            decimal dispr = 0;
                            //            DashboardViewItem lstAnalysisprice = ((DetailView)View).FindItem("AnalysisCharge") as DashboardViewItem;
                            //            ListPropertyEditor lstitemprice = ((DetailView)View).FindItem("ItemCharges") as ListPropertyEditor;
                            //            if (lstitemprice != null)
                            //            {
                            //                if (lstitemprice.ListView == null)
                            //                {
                            //                    lstitemprice.CreateControl();
                            //                }
                            //                if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                            //                {
                            //                    finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.FinalAmount);
                            //                    totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.UnitPrice * i.Qty);
                            //                }
                            //                ((ListView)lstitemprice.ListView).Refresh();
                            //            }
                            //            if (lstAnalysisprice != null)
                            //            {
                            //                if (lstAnalysisprice.InnerView == null)
                            //                {
                            //                    lstAnalysisprice.CreateControl();
                            //                }
                            //                if (((ListView)lstAnalysisprice.InnerView).CollectionSource.GetCount() > 0)
                            //                {
                            //                    finalamt = finalamt + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.Amount);
                            //                    totalprice = totalprice + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.TotalUnitPrice * i.Qty);
                            //                }
                            //                ((ListView)lstAnalysisprice.InnerView).Refresh();
                            //            }
                            //            if (finalamt < 0)
                            //            {
                            //                objap.Discount = 0;
                            //                objap.Amount = 0;
                            //                finalamt = 0;
                            //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            //            }
                            //            if (Frame is NestedFrame)
                            //            {
                            //                NestedFrame nestedFrame = (NestedFrame)Frame;
                            //                //Invoicing crtquotes = (Invoicing)nestedFrame.ViewItem.View.CurrentObject;
                            //                if (crtquotes != null)
                            //                {
                            //                    if (finalamt >= 0 && totalprice >= 0)
                            //                    {
                            //                        //disamt = finalamt - totalprice;
                            //                        disamt = totalprice - finalamt;
                            //                        if (disamt != 0)
                            //                        {
                            //                            dispr = ((disamt) / totalprice) * 100;
                            //                        }
                            //                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                            //                        crtquotes.Amount = Math.Round(finalamt, 2);
                            //                        crtquotes.Discount = Convert.ToInt32(Math.Round(dispr, 2));
                            //                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                            //                    }
                            //                    else
                            //                    {
                            //                        dispr = ((totalprice) / totalprice) * 100;
                            //                        //disamt = finalamt - totalprice;
                            //                        disamt = totalprice - finalamt;
                            //                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                            //                        crtquotes.Amount = Math.Round(finalamt, 2);
                            //                        crtquotes.Discount = Convert.ToInt32(Math.Round(dispr, 2));
                            //                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                //Invoicing crtquotes = (Invoicing)Application.MainWindow.View.CurrentObject;
                            //                if (crtquotes != null)
                            //                {
                            //                    if (finalamt >= 0 && totalprice >= 0)
                            //                    {
                            //                        disamt = totalprice - finalamt;
                            //                        if (disamt != 0)
                            //                        {
                            //                            dispr = ((disamt) / totalprice) * 100;
                            //                        }
                            //                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                            //                        crtquotes.Amount = Math.Round(finalamt, 2);
                            //                        crtquotes.Discount = Convert.ToInt16(Math.Round(dispr, 2));
                            //                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                            //                    }
                            //                    else
                            //                    {
                            //                        dispr = ((totalprice) / totalprice) * 100;
                            //                        disamt = totalprice - finalamt;
                            //                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                            //                        crtquotes.Amount = Math.Round(finalamt, 2);
                            //                        crtquotes.Discount = Convert.ToInt32(Math.Round(dispr, 2));
                            //                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                            //                    }
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            IsNotTATavl = true;
                            //            if (string.IsNullOrEmpty(strIsNotTATavlTest))
                            //            {
                            //                strIsNotTATavlTest = objap.Test.TestName + ";" + objap.Test.MethodName.MethodNumber;
                            //            }
                            //            else
                            //            {
                            //                strIsNotTATavlTest = strIsNotTATavlTest + ", " + objap.Test.TestName + ";" + objap.Test.MethodName.MethodNumber;
                            //            }
                            //        }
                            //    }
                            //    if (IsNotTATavl)
                            //    {
                            //        Application.ShowViewStrategy.ShowMessage("Selected TAT value not available on selected test " + strIsNotTATavlTest + ".", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            //    }
                            //}
                        }
                    }
                }
                if (View.Id == "Invoicing_DetailView_Queue" && e.PropertyName == "Priority")
                {
                    Invoicing objINV = (Invoicing)View.CurrentObject;

                    //ActualPrioryApplied column intial value show
                    if (objINV.ActualPriorityApplied == null && objINV.Priority.Prioritys != null)
                    {
                        objINV.ActualPriorityApplied = objINV.Priority;
                    }
                    DashboardViewItem dvanalysisprice = ((DetailView)View).FindItem("AnalysisCharge") as DashboardViewItem;
                    IObjectSpace os = dvanalysisprice.InnerView.ObjectSpace;
                    bool isTATChanged = false;
                    if (dvanalysisprice != null && dvanalysisprice.InnerView != null)
                    {
                        foreach (InvoicingAnalysisCharge objInvanlych in ((ListView)dvanalysisprice.InnerView).CollectionSource.List)
                        {
                            if (objInvanlych.Matrix != null && objInvanlych.Test != null && objInvanlych.Method != null && objInvanlych.Component != null && objINV.Priority != null)
                            {
                                TestPriceSurcharge lsttstpricing = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ? And [Priority.Prioritys] = ?", objInvanlych.Matrix.MatrixName, objInvanlych.Test.TestName, objInvanlych.Method.MethodNumber, objInvanlych.Component.Components, objINV.Priority.Prioritys));
                                if (lsttstpricing != null)
                                {
                                    objInvanlych.Priority = dvanalysisprice.InnerView.ObjectSpace.GetObject<Priority>(objINV.Priority);
                                    objInvanlych.PriceCode = lsttstpricing.PriceCode;
                                    objInvanlych.TierPrice = (decimal)lsttstpricing.SurchargePrice;
                                    objInvanlych.Parameter = "Allparam";
                                    if (objInvanlych.Priority != null)
                                    {
                                        decimal disamount = ((objInvanlych.TierPrice) * (objInvanlych.Discount) / 100);//* objInvanlych.Qty
                                        decimal finalamount = ((objInvanlych.TierPrice) - disamount) * objInvanlych.Qty;
                                        objInvanlych.Amount = finalamount;
                                    }
                                    if (lsttstpricing != null && lsttstpricing.SurchargePrice != null)
                                    {
                                        objInvanlych.TierPrice = (decimal)lsttstpricing.SurchargePrice;
                                    }
                                    //if (objInvanlych.Qty != null && objInvanlych.UnitPrice != null)
                                    //{
                                    //    objInvanlych.Amount = (objInvanlych.Qty) * objInvanlych.TierPrice;
                                    //}
                                    List<string> lstTAT = lsttstpricing.TAT.Replace("; ", ";").Split(';').ToList();
                                    if (lstTAT.Count == 1)
                                    {
                                        string strTAT = lstTAT.FirstOrDefault();
                                        TurnAroundTime objTAT = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", strTAT));
                                        objInvanlych.TAT = dvanalysisprice.InnerView.ObjectSpace.GetObject(objTAT);
                                        objINV.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objTAT);
                                        isTATChanged = true;
                                        //objINV.DueDateCompleted = DateTime.Today.AddDays(objTAT.);
                                        if (objTAT != null)
                                        {
                                            if (objInvanlych != null && objInvanlych.JobID != null)
                                            {
                                                Samplecheckin objsam = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", objInvanlych.JobID));
                                                if (objsam != null && objsam.RecievedDate != null)
                                                {
                                                    int tatHour = objTAT.Count;
                                                    int Day = 0;
                                                    if (tatHour >= 24)
                                                    {
                                                        Day = tatHour / 24;
                                                        objINV.DueDate = AddWorkingDays(objsam.RecievedDate, Day);
                                                    }
                                                    else
                                                    {
                                                        objINV.DueDate = AddWorkingHours(objsam.RecievedDate, tatHour);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string strTAT = string.Join("','", lsttstpricing.TAT.Replace("; ", ";").Split(';'));
                                        //string strTAT = lstTAT.FirstOrDefault();
                                        TurnAroundTime objTAT = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse(string.Format("[TAT] IN ('{0}')", strTAT)));
                                        objInvanlych.TAT = dvanalysisprice.InnerView.ObjectSpace.GetObject(objTAT);
                                        objINV.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objTAT);
                                        isTATChanged = true;
                                        //objINV.DueDateCompleted = DateTime.Today.AddDays(objTAT.);
                                        if (objTAT != null)
                                        {
                                            if (objInvanlych != null && objInvanlych.JobID != null)
                                            {
                                                Samplecheckin objsam = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", objInvanlych.JobID));
                                                if (objsam != null && objsam.RecievedDate != null)
                                                {
                                                    int tatHour = objTAT.Count;
                                                    int Day = 0;
                                                    if (tatHour >= 24)
                                                    {
                                                        Day = tatHour / 24;
                                                        objINV.DueDate = AddWorkingDays(objsam.RecievedDate, Day);
                                                    }
                                                    else
                                                    {
                                                        objINV.DueDate = AddWorkingHours(objsam.RecievedDate, tatHour);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    objInvanlych.UnitPrice = 0;
                                    objInvanlych.TierPrice = 0;
                                    objInvanlych.Discount = 0;
                                    objInvanlych.Amount = 0;
                                    objInvanlych.TierNo = 0;
                                    objInvanlych.From = 0;
                                    objInvanlych.To = 0;
                                    objInvanlych.PriceCode = null;
                                    objInvanlych.Priority = null;
                                    objInvanlych.Description = null;
                                    objInvanlych.Parameter = null;
                                    objInvanlych.TAT = null;
                                    if (!isTATChanged)
                                    {
                                        objINV.TAT = null;
                                        List<string> lstJobID = objINV.JobID.Split(',').ToList();
                                        IList<Samplecheckin> lstSamplecheckin = ObjectSpace.GetObjects<Samplecheckin>(new InOperator("JobID", lstJobID.Select(i => i.Replace(" ", ""))));
                                        if (lstSamplecheckin != null && lstSamplecheckin.Count > 0 && lstSamplecheckin[0].TAT != null)
                                        {
                                            objINV.TAT = ObjectSpace.GetObjectByKey<TurnAroundTime>(lstSamplecheckin[0].TAT.Oid);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dvanalysisprice.InnerView.Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
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
        private void SaveAndCloseAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "Invoicing_DetailView" || View.Id == "Invoicing_DetailView_Queue")
                {
                    if (View.CurrentObject != null)
                    {
                        Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                        View.ObjectSpace.CommitChanges();
                        View.ObjectSpace.Refresh();
                        DashboardViewItem liInvoicingAnalysisCharges = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                        ListPropertyEditor liInvoicingItemCharges = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                        if (liInvoicingAnalysisCharges != null && liInvoicingAnalysisCharges.InnerView != null)
                        {
                            ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();
                            List<string> lstJobID = objInVoice.JobID.Split(',').ToList();
                            decimal AnalysisChargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.Amount));
                            decimal AnalysisChargedetailAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.TierPrice * i.Qty));
                            decimal itemCahargeFinalAmount = 0;
                            decimal itemCahargeDetailAmount = 0;
                            if (liInvoicingItemCharges != null && liInvoicingItemCharges.ListView != null)
                            {
                                itemCahargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.FinalAmount));
                                itemCahargeDetailAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.Qty * i.NpUnitPrice));
                            }
                            decimal FinalAmount = AnalysisChargeFinalAmount + itemCahargeFinalAmount;
                            decimal detailAmount = AnalysisChargedetailAmount + itemCahargeDetailAmount;
                            objInVoice.DetailedAmount = detailAmount;
                            objInVoice.Amount = FinalAmount;
                            if (detailAmount > FinalAmount)
                            {
                                objInVoice.DiscountAmount = detailAmount - FinalAmount;
                                objInVoice.Discount = Convert.ToDecimal(100 * ((detailAmount - FinalAmount) / detailAmount));
                            }
                            objInVoice.Status = InviceStatus.PendingReview;
                            List<Samplecheckin> lstSampleCheckins = View.ObjectSpace.GetObjects<Samplecheckin>(new InOperator("JobID", objInVoice.JobID.Split(';').ToList().Select(i => i = i.Trim()).ToList())).ToList();
                            foreach (Samplecheckin objSamplecheckin in lstSampleCheckins.ToList())
                            {
                                IList<Modules.BusinessObjects.SampleManagement.Reporting> reportings = View.ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(i => i.JobID != null && i.JobID.JobID == objSamplecheckin.JobID).ToList();
                                IList<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                if (lstSamples1.Count() == lstSamples1.Where(i => i.Status == Samplestatus.Reported).Count() && (reportings.FirstOrDefault(i => i.DeliveredBy == null) == null))
                                {
                                    StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 26"));
                                    if (objStatus != null)
                                    {
                                        objSamplecheckin.Index = objStatus;
                                        objSamplecheckin.Isinvoicesummary = true;
                                    }
                                }
                            }
                            foreach (InvoicingAnalysisCharge objAnalysisCharge in ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Where(i => i.IsGroup == false).ToList())
                            {

                                if (objAnalysisCharge.Matrix != null && objAnalysisCharge.Test != null && objAnalysisCharge.Method != null)
                                {
                                    IList<SampleParameter> lstSampleParams = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Samplelogin.JobID.JobID", lstJobID.Select(i => i.Replace(" ", ""))));
                                    if (lstSampleParams.Count > 0)
                                    {
                                        List<SampleParameter> lstSamples = lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == objAnalysisCharge.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == objAnalysisCharge.Testparameter.TestMethod.TestName && i.Testparameter.TestMethod.MethodName.MethodNumber == objAnalysisCharge.Method.MethodNumber && i.InvoicingAnalysisCharge == null && (i.InvoiceIsDone == false || i.InvoiceIsDone == null)).ToList();
                                        foreach (SampleParameter objParam in lstSamples.ToList())
                                        {
                                            objParam.InvoiceIsDone = true;
                                            objParam.InvoicingAnalysisCharge = objAnalysisCharge;
                                        }
                                    }
                                }
                                objAnalysisCharge.Invoice = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjectByKey<Invoicing>(objInVoice.Oid);
                            }
                            foreach (InvoicingAnalysisCharge objAnalysisCharge in ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Where(i => i.IsGroup == true).ToList())
                            {

                                if (objAnalysisCharge.Matrix != null && objAnalysisCharge.Test != null)
                                {
                                    IList<SampleParameter> lstSampleParams = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Samplelogin.JobID.JobID", lstJobID.Select(i => i.Replace(" ", ""))));
                                    if (lstSampleParams.Count > 0)
                                    {
                                        List<SampleParameter> lstSamples = lstSampleParams.Where(i => i.GroupTest != null && i.GroupTest.TestMethod != null && i.Testparameter.TestMethod.MatrixName.MatrixName == objAnalysisCharge.Matrix.MatrixName && i.GroupTest.TestMethod.TestName == objAnalysisCharge.Test.TestName && i.InvoicingAnalysisCharge == null && i.IsGroup == true && (i.InvoiceIsDone == false || i.InvoiceIsDone == null)).ToList();
                                        foreach (SampleParameter objParam in lstSamples.ToList())
                                        {
                                            objParam.InvoiceIsDone = true;
                                            objParam.InvoicingAnalysisCharge = objAnalysisCharge;
                                        }
                                    }
                                }
                                objAnalysisCharge.Invoice = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjectByKey<Invoicing>(objInVoice.Oid);
                            }
                            liInvoicingAnalysisCharges.InnerView.ObjectSpace.CommitChanges();
                            ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();
                        }

                        if (objInVoice != null)
                        {
                            string strInvoiceID = objInVoice.InvoiceID;
                            XtraReport xtraReport = new XtraReport();

                            objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            SetConnectionString();

                            DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);

                            ObjReportingInfo.strInvoiceID = "'" + strInvoiceID + "'";
                            xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("Invoicing_Reports", ObjReportingInfo, false);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                xtraReport.ExportToPdf(ms);
                                objInVoice.Report = ms.ToArray();
                                NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                                PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                                objToShow.ReportID = objInVoice.InvoiceID;
                                objToShow.PDFData = ms.ToArray();
                                objToShow.ViewID = View.Id;
                                DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow, true);
                                CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                                ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.SaveOnAccept = false;
                                dc.AcceptAction.Active.SetItemValue("disable", false);
                                dc.CancelAction.Active.SetItemValue("disable", false);
                                dc.CloseOnCurrentObjectProcessing = false;
                                dc.ViewClosing += Dc_ViewClosing; ;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                View.ObjectSpace.CommitChanges();
                                View.ObjectSpace.Refresh();
                                IsPoupView = true;
                            }

                        }
                        //ResetNavigationCount();
                        Application.MainWindow.View.Refresh();

                    }
                }
                else if (View.Id == "Invoicing_DetailView_Review")
                {
                    Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                    View.ObjectSpace.CommitChanges();
                    DashboardViewItem liInvoicingAnalysisCharges = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                    ListPropertyEditor liInvoicingItemCharges = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;

                    if (liInvoicingAnalysisCharges != null && liInvoicingAnalysisCharges.InnerView != null)
                    {
                        ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();
                        List<string> lstJobID = objInVoice.JobID.Split(',').ToList();
                        decimal AnalysisChargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.Amount));
                        decimal AnalysisChargedetailAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.TierPrice * i.Qty));
                        decimal itemCahargeFinalAmount = 0;
                        decimal itemCahargeDetailAmount = 0;
                        if (liInvoicingItemCharges != null && liInvoicingItemCharges.ListView != null)
                        {
                            itemCahargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.FinalAmount));
                            itemCahargeDetailAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.Qty * i.NpUnitPrice));
                        }
                        decimal FinalAmount = AnalysisChargeFinalAmount + itemCahargeFinalAmount;
                        decimal detailAmount = AnalysisChargedetailAmount + itemCahargeDetailAmount;
                        objInVoice.DetailedAmount = detailAmount;
                        objInVoice.Amount = FinalAmount;
                        if (detailAmount > FinalAmount)
                        {
                            objInVoice.DiscountAmount = (detailAmount) - (FinalAmount);
                            objInVoice.Discount = Convert.ToDecimal(100 * ((detailAmount - FinalAmount) / detailAmount));
                        }
                        else if (detailAmount < FinalAmount)
                        {
                            decimal disamt = FinalAmount - detailAmount;
                            if (disamt != 0)
                            {
                                objInVoice.DiscountAmount = disamt;
                                objInVoice.Discount = Convert.ToDecimal(((disamt) / detailAmount) * 100);
                            }
                        }
                        else if (detailAmount == FinalAmount)
                        {
                            objInVoice.DiscountAmount = 0;
                            objInVoice.Discount = 0;
                        }
                        liInvoicingAnalysisCharges.InnerView.ObjectSpace.CommitChanges();
                        ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();

                    }
                    View.ObjectSpace.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_ViewClosing(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_ListView_Invoice")
                {
                    ASPxGridListEditor aSPxGridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (aSPxGridListEditor != null && aSPxGridListEditor.Grid != null)
                    {
                        //aSPxGridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        aSPxGridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){RaiseXafCallback(globalCallbackControl, 'ParameterPopup',  'Close', '', false); }";
                    }
                }
                WebWindow.CurrentRequestWindow.RegisterClientScript("PriceNotSet", string.Format(CultureInfo.InvariantCulture, @"var cancelconfirm = confirm('" + msg + "'); {0}", NotSetPrice.CallbackManager.GetScript("PriceNotSet", "cancelconfirm")));

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
                if (View.Id == "Invoicing_DetailView" || View.Id == "Invoicing_DetailView_Queue" || View.Id == "Invoicing_DetailView_View_History")
                {
                    if (View.CurrentObject != null)
                    {
                        Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                        if (objInVoice.DueDateCompleted > objInVoice.DueDate)
                        {
                            ICallbackManagerHolder handlerid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                            handlerid.CallbackManager.RegisterHandler("InvoiceHandler", this);
                            string msg = "Reported date exceeds request due date!. Do you want to continue?";
                            WebWindow.CurrentRequestWindow.RegisterClientScript("ReportingDateGreaterthanDueDate", string.Format(CultureInfo.InvariantCulture, @"var SaveInvoiceID = confirm('" + msg + "'); {0}", handlerid.CallbackManager.GetScript("InvoiceHandler", "SaveInvoiceID")));
                            e.Cancel = true;
                        }
                        else
                        {
                            View.ObjectSpace.CommitChanges();
                            DashboardViewItem liInvoicingAnalysisCharges = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                            ListPropertyEditor liInvoicingItemCharges = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;

                            if (liInvoicingAnalysisCharges != null && liInvoicingAnalysisCharges.InnerView != null)
                            {
                                ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();
                                List<string> lstJobID = objInVoice.JobID.Split(';').ToList();
                                decimal AnalysisChargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.Amount));
                                decimal AnalysisChargedetailAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.TierPrice * i.Qty));
                                decimal itemCahargeFinalAmount = 0;
                                decimal itemCahargeDetailAmount = 0;
                                if (liInvoicingItemCharges != null && liInvoicingItemCharges.ListView != null)
                                {
                                    itemCahargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.FinalAmount));
                                    itemCahargeDetailAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.Qty * i.NpUnitPrice));
                                }
                                decimal FinalAmount = AnalysisChargeFinalAmount + itemCahargeFinalAmount;
                                decimal detailAmount = AnalysisChargedetailAmount + itemCahargeDetailAmount;
                                objInVoice.DetailedAmount = detailAmount;//1350
                                objInVoice.Amount = FinalAmount;//1050
                                if (detailAmount > FinalAmount)//true
                                {
                                    objInVoice.DiscountAmount = (detailAmount) - (FinalAmount);
                                    objInVoice.Discount = Convert.ToDecimal(100 * ((detailAmount - FinalAmount) / detailAmount));//DetailDiscount
                                }
                                else if (detailAmount < FinalAmount)
                                {
                                    decimal disamt = FinalAmount - detailAmount;
                                    if (disamt != 0)
                                    {
                                        objInVoice.DiscountAmount = disamt;
                                        objInVoice.Discount = Convert.ToDecimal(((disamt) / detailAmount) * 100);
                                    }
                                }
                                else if (detailAmount == FinalAmount)
                                {
                                    objInVoice.DiscountAmount = 0;
                                    objInVoice.Discount = 0;
                                }
                                objInVoice.Status = InviceStatus.PendingReview;

                                Samplecheckin objSamplecheckin = View.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", objInVoice.JobID));
                                IList<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                if (lstSamples1.Where(i => i.Status == Samplestatus.PendingEntry && i.Status == Samplestatus.PendingApproval && i.Status == Samplestatus.PendingReportValidation && i.Status == Samplestatus.PendingValidation && i.Status == Samplestatus.PendingReporting).Count() == 0)
                                {
                                    StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = '26'"));
                                    if (objStatus != null)
                                    {
                                        objSamplecheckin.Index = objStatus;
                                    }

                                }
                                foreach (InvoicingAnalysisCharge objAnalysisCharge in ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Where(i => i.IsGroup == false).ToList())
                                {

                                    if (objAnalysisCharge.Matrix != null && objAnalysisCharge.Test != null && objAnalysisCharge.Method != null)
                                    {
                                        IList<SampleParameter> lstSampleParams = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Samplelogin.JobID.JobID", lstJobID.Select(i => i.Replace(" ", ""))));
                                        if (lstSampleParams.Count > 0)
                                        {
                                            List<SampleParameter> lstSamples = lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == objAnalysisCharge.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == objAnalysisCharge.Testparameter.TestMethod.TestName && i.Testparameter.TestMethod.MethodName.MethodNumber == objAnalysisCharge.Method.MethodNumber && i.InvoicingAnalysisCharge == null && (i.InvoiceIsDone == false || i.InvoiceIsDone == null)).ToList();
                                            lstSamples.ForEach(i => { i.InvoiceIsDone = true; i.InvoicingAnalysisCharge = objAnalysisCharge; });
                                            //foreach (SampleParameter objParam in lstSamples.ToList())
                                            //{
                                            //    objParam.InvoiceIsDone = true;
                                            //    objParam.InvoicingAnalysisCharge = objAnalysisCharge;
                                            //}
                                        }
                                    }
                                    objAnalysisCharge.Invoice = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjectByKey<Invoicing>(objInVoice.Oid);
                                }
                                foreach (InvoicingAnalysisCharge objAnalysisCharge in ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Where(i => i.IsGroup == true).ToList())
                                {

                                    if (objAnalysisCharge.Matrix != null && objAnalysisCharge.Test != null)
                                    {
                                        IList<SampleParameter> lstSampleParams = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Samplelogin.JobID.JobID", lstJobID.Select(i => i.Replace(" ", ""))));
                                        if (lstSampleParams.Count > 0)
                                        {
                                            List<SampleParameter> lstSamples = lstSampleParams.Where(i => i.GroupTest != null && i.GroupTest.TestMethod != null && i.Testparameter.TestMethod.MatrixName.MatrixName == objAnalysisCharge.Matrix.MatrixName && i.GroupTest.TestMethod.TestName == objAnalysisCharge.Test.TestName && i.InvoicingAnalysisCharge == null && i.IsGroup == true && (i.InvoiceIsDone == false || i.InvoiceIsDone == null)).ToList();
                                            lstSamples.ForEach(i => { i.InvoiceIsDone = true; i.InvoicingAnalysisCharge = objAnalysisCharge; });
                                            //foreach (SampleParameter objParam in lstSamples.ToList())
                                            //{
                                            //    objParam.InvoiceIsDone = true;
                                            //    objParam.InvoicingAnalysisCharge = objAnalysisCharge;
                                            //}
                                        }
                                    }
                                    objAnalysisCharge.Invoice = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjectByKey<Invoicing>(objInVoice.Oid);
                                }
                                liInvoicingAnalysisCharges.InnerView.ObjectSpace.CommitChanges();
                                ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();

                            }
                            #region SuboutStatus
                            //IObjectSpace os = Application.CreateObjectSpace();
                            //List<Samplecheckin> lstSampleCheckins = os.GetObjects<Samplecheckin>(new InOperator("JobID", objInVoice.JobID.Split(';').ToList().Select(i => i = i.Trim()).ToList())).ToList();
                            //IList<SampleParameter> lstSample = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.Oid", lstSampleCheckins.Select(i => i.Oid).Distinct().ToList()), (CriteriaOperator.Parse("[Status] = 'Reported' And [InvoiceIsDone] = True  And [Samplelogin.ExcludeInvoice] = False"))));
                            //if (lstSample.ToList().FirstOrDefault(i => i.SubOut == true) != null)
                            //{
                            //    foreach (SubOutSampleRegistrations objSubout in lstSample.Where(i => i.SubOut == true).Select(i => i.SuboutSample).Distinct().ToList())
                            //    {
                            //        if (objSubout.SampleParameter.ToList().FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation || i.Status == Samplestatus.SuboutPendingApproval || i.Status == Samplestatus.PendingReporting) == null)
                            //        {
                            //            if (objSubout.SubOutQcSample.ToList().FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation || i.Status == Samplestatus.SuboutPendingApproval) == null)
                            //            {
                            //                SubOutSampleRegistrations obj = os.FindObject<SubOutSampleRegistrations>(CriteriaOperator.Parse("[Oid]=?", objSubout.Oid));
                            //                if (obj != null)
                            //                {
                            //                    obj.SuboutStatus = SuboutTrackingStatus.Invoiced;
                            //                }
                            //            }
                            //        }
                            //    }
                            //    os.CommitChanges();
                            //}
                            //os.Dispose();
                            # endregion

                            View.ObjectSpace.CommitChanges();
                            if (objInVoice != null)
                            {
                                string strInvoiceID = objInVoice.InvoiceID;
                                XtraReport xtraReport = new XtraReport();

                                objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                SetConnectionString();

                                DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);

                                ObjReportingInfo.strInvoiceID = "'" + strInvoiceID + "'";
                                xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("Invoicing_Reports", ObjReportingInfo, false);

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    xtraReport.ExportToPdf(ms);
                                    objInVoice.Report = ms.ToArray();
                                    //NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                                    //PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                                    //objToShow.ReportID = objInVoice.InvoiceID;
                                    //objToShow.PDFData = ms.ToArray();
                                    //objToShow.ViewID = View.Id;
                                    //DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                                    //CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                                    //ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                                    //showViewParameters.Context = TemplateContext.PopupWindow;
                                    //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                    //DialogController dc = Application.CreateController<DialogController>();
                                    //dc.SaveOnAccept = false;
                                    //dc.AcceptAction.Active.SetItemValue("disable", false);
                                    //dc.CancelAction.Active.SetItemValue("disable", false);
                                    //dc.CloseOnCurrentObjectProcessing = false;
                                    //showViewParameters.Controllers.Add(dc);
                                    //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                    View.ObjectSpace.CommitChanges();
                                    //IsPoupView = true;
                                }

                            }
                        }
                        //ResetNavigationCount();

                        Application.MainWindow.View.Refresh();
                    }
                }
                else if (View.Id == "Invoicing_DetailView_Review")
                {
                    Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                    View.ObjectSpace.CommitChanges();
                    DashboardViewItem liInvoicingAnalysisCharges = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                    ListPropertyEditor liInvoicingItemCharges = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;

                    if (liInvoicingAnalysisCharges != null && liInvoicingAnalysisCharges.InnerView != null)
                    {
                        ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();
                        List<string> lstJobID = objInVoice.JobID.Split(',').ToList();
                        decimal AnalysisChargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.Amount));
                        decimal AnalysisChargedetailAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.TierPrice * i.Qty));
                        decimal itemCahargeFinalAmount = 0;
                        decimal itemCahargeDetailAmount = 0;
                        if (liInvoicingItemCharges != null && liInvoicingItemCharges.ListView != null)
                        {
                            itemCahargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.FinalAmount));
                            itemCahargeDetailAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.Qty * i.NpUnitPrice));
                        }
                        decimal FinalAmount = AnalysisChargeFinalAmount + itemCahargeFinalAmount;
                        decimal detailAmount = AnalysisChargedetailAmount + itemCahargeDetailAmount;
                        objInVoice.DetailedAmount = detailAmount;
                        objInVoice.Amount = FinalAmount;
                        if (detailAmount > FinalAmount)
                        {
                            objInVoice.DiscountAmount = (detailAmount) - (FinalAmount);
                            objInVoice.Discount = Convert.ToDecimal(100 * ((detailAmount - FinalAmount) / detailAmount));
                        }
                        else if (detailAmount < FinalAmount)
                        {
                            decimal disamt = FinalAmount - detailAmount;
                            if (disamt != 0)
                            {
                                objInVoice.DiscountAmount = disamt;
                                objInVoice.Discount = Convert.ToDecimal(((disamt) / detailAmount) * 100);
                            }
                        }
                        else if (detailAmount == FinalAmount)
                        {
                            objInVoice.DiscountAmount = 0;
                            objInVoice.Discount = 0;
                        }
                        liInvoicingAnalysisCharges.InnerView.ObjectSpace.CommitChanges();
                        ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();

                    }
                    View.ObjectSpace.CommitChanges();

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
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
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {

                if (View.Id == "Invoicing_ListView_View_History")

                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.SelectionChanged += Gridlisteditor_SelectionChanged;
                    }
                        
                    
                }

                //if (View.Id == "TestPriceSurcharge_ListView_Invoice")
                //{
                //    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    if (gridlisteditor != null && gridlisteditor.Grid != null)
                //    {
                //        gridlisteditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                //        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                //        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                //        selparameter.CallbackManager.RegisterHandler("ParameterPopup", this);
                //        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                //        gridlisteditor.Grid.Load += Grid_Load;
                //        gridlisteditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                //    }
                //}
                if (View.Id == "Priority_ListView_Invoice")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        ASPxGridView gridView = gridlisteditor.Grid;
                        if (gridView != null)
                        {
                            gridView.PreRender += GridView_PreRender;
                        }
                    }
                }
                if (View.Id == "Invoicing_DetailView_Queue")
                {
                    ICallbackManagerHolder handlerid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    handlerid.CallbackManager.RegisterHandler("InvoiceHandler", this);
                }
                if (View.Id == "Samplecheckin_ListView_InvoiceJobID")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                }
                else if (View.Id == "InvoicingAnalysisCharge_ListView_Invoice" || View.Id == "InvoicingAnalysisCharge_ListView_Queue") //InvoicingAnalysisCharge_ListView_Queue
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.JSProperties["cpPagesize"] = gridListEditor.Grid.SettingsPager.PageSize;
                    Invoicing objCurr = (Invoicing)Application.MainWindow.View.CurrentObject;
                    if (objCurr != null && string.IsNullOrEmpty(objCurr.InvoiceID) && objInvoiceInfo.IsDataLoaded == false)
                    {
                        gridListEditor.Grid.JSProperties["cpIsNew"] = true;
                        objInvoiceInfo.IsDataLoaded = true;
                    }
                    else
                    {
                        gridListEditor.Grid.JSProperties["cpIsNew"] = false;
                    }
                    XafCallbackManager parameter = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    parameter.RegisterHandler("ParameterPopup", this);
                    gridListEditor.Grid.ClientInstanceName = "ParameterShow";
                    gridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e)
                    {
                      if(s.cpIsNew == true)
                         {
                             var i = s.cpPagesize * s.GetPageIndex();
                             var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                            for (i; i < totrow; i++)
                              {
                               var unitprice = s.batchEditApi.GetCellValue(i, 'UnitPrice');
                               var discount = s.batchEditApi.GetCellValue(i, 'Discount');  
                               var qty = s.batchEditApi.GetCellValue(i, 'Qty');
                               var prep1 = s.batchEditApi.GetCellValue(i, 'Prep1Price');
                               var prep2 = s.batchEditApi.GetCellValue(i, 'Prep2Price');
                               var surcharge=s.batchEditApi.GetCellValue(i, 'Surcharge');
                               var surchargePrice=s.batchEditApi.GetCellValue(i, 'TierPrice');
                               var persucharge =0;
                               tatamt= unitprice + prep1 + prep2 ;
                               //if(surcharge>0)
                               //{
                               //    persucharge=tatamt * (surcharge / 100);
                               //    tatamt= tatamt + persucharge;
                               //}
                             
                              var totalamount=0;
                              //s.batchEditApi.SetCellValue(i, 'TierPrice', tatamt);
                              var fieldName = sessionStorage.getItem('FocusedColumn');
                              s.batchEditApi.SetCellValue(i, 'TotalUnitPrice', surchargePrice);
                              var totalprice=surchargePrice;
                             
                              //var totalprice = s.batchEditApi.GetCellValue(i, 'TotalUnitPrice');
                              if(discount>0)
                               {
                                 var discutamt = totalprice * (discount / 100);
                                 totalamount=qty * (totalprice - discutamt);
                               }
                              else if(discount<0)
                              {
                                  var discoamt = totalprice * (discount / 100);
                                 //totalamount=qty * (totalprice - discutamt) ;
                                  totalamount= ((totalprice) - (discoamt)) * qty;
                              }
                              else
                              {
                                totalamount=qty * totalprice ;
                              }
                              s.batchEditApi.SetCellValue(i, 'Amount',Math.round(totalamount * 100) / 100);  
                              //s.UpdateEdit();
                              
                            } 
                       s.cpIsNew=false;
                       }
                       
                    }";
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
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
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() { 
                           var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');                                
                                var oid;
                                var text;
                                                                                           
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
//console.log(FocusedColumn) ;                                  
                                        
                                            if(FocusedColumn=='Discount')
                                                {
console.log('1');
                                                  var unprice = s.batchEditApi.GetCellValue(e.visibleIndex,'TierPrice');
                                                  var qty = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                                                  var discount = s.batchEditApi.GetCellValue(e.visibleIndex, 'Discount'); 
console.log(s.batchEditApi.GetCellValue(e.visibleIndex, 'Amount'));
                                                    if(unprice != null){                                         
                                                     var discoamt = unprice * (discount / 100);                        
                                                     totalamount= ((unprice) - (discoamt)) * qty;
                                                    s.batchEditApi.SetCellValue(e.visibleIndex,'Amount',totalamount); 
//s.UpdateEdit();
//console.log(totalamount)
                                                   }
                                                }                                          
                                           // s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                            },20); }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                                          if( e.focusedColumn.fieldName == 'Parameter')
                                                               {
                                                                e.cancel = true;
                                                                }
                                                            else
                                                                 {
                                                                      e.cancel = false;
                                                                 }
                                                             }";

                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                                                  {   

                                                //  var FocusedColumn = sessionStorage.getItem('FocusedColumn');                                
                    var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');                                
                                                  var oid;
                                                  var text;
                                                  if(FocusedColumn.includes('.'))
                                                  {             
                                                      console.log(a);                         
                                                      oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                                      text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                                      if (e.item.name =='CopyToAllCell')
                                                      {
                                                          for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                                          {                                                                                 
                                                              s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);                                                                                 
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
                                                              if(FocusedColumn=='Discount')
                                                                  {
                                                                    var unprice = s.batchEditApi.GetCellValue(i,'TierPrice');
                                                                    var qty = s.batchEditApi.GetCellValue(i, 'Qty');

                                                                      if(unprice != null){                                         
                                                                       var discoamt = unprice * (CopyValue / 100);                        
                                                                       totalamount= ((unprice) - (discoamt)) * qty;
                                                                      s.batchEditApi.SetCellValue(i,'Amount',totalamount); 
                                                                     }
                                                                  }                                          
                                                              s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                                          }
                                                      }                            
                                                   }
                                               e.processOnServer = false;

                              }";

                    //gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s, e)
                    //{
                    //   if (s.IsRowSelectedOnPage(e.elementIndex))  
                    //   {  
                    //        var FocusedColumn = sessionStorage.getItem('FocusedColumn');
                    //        var oid;
                    //        var text;

                    //        if(FocusedColumn.includes('.'))
                    //        {          
                    //          console.log('A');                            
                    //          oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                    //          text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText; 
                    //          if (e.item.name =='CopyToAllCell' )
                    //          {
                    //            if (FocusedColumn=='Discount')
                    //             {
                    //          for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                    //                { 
                    //                   if (s.IsRowSelectedOnPage(i)) 
                    //                        {
                    //                           s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);   

                    //                        }                                      
                    //                 }   
                    //              }              
                    //            }
                    //         }
                    //       }
                    //       else
                    //       {    
                    //              console.log('B');   
                    //              var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                    //                if (e.item.name =='CopyToAllCell')
                    //                {
                    //                    for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                    //                    { 
                    //                        if (s.IsRowSelectedOnPage(i)) 
                    //                        {
                    //                            s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                    //                        }
                    //                    }
                    //                }      
                    //        }                               
                    //         e.processOnServer = false;
                    // }";

                    if (gridListEditor.Grid.Columns["Qty"] != null)
                    {
                        gridListEditor.Grid.Columns["Qty"].Width = 40;
                    }
                    if (gridListEditor.Grid.Columns["Surcharge"] != null)
                    {
                        gridListEditor.Grid.Columns["Surcharge"].Width = 0;
                    }
                    gridListEditor.Grid.ClientSideEvents.BatchEditConfirmShowing = @"function(s,e) 
                    { 
                        e.cancel = true;
                    }";
                    gridListEditor.Grid.Load += Grid_Load;
                    NotSetPrice = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    NotSetPrice.CallbackManager.RegisterHandler("PriceNotSet", this);
                }
                else if (View.Id == "Invoicing_ListView" || View.Id == "Invoicing_ListView_View_History" || View.Id == "Invoicing_ListView_Review" || View.Id == "Invoicing_ListView_Delivery"
                    || View.Id == "Invoicing_ListView_Review_History" || View.Id == "Invoicing_ListView_Delivery_History")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        if (Convert.ToInt32(strscreenwidth) < 1600)
                        {
                            gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                        }
                        if (gridListEditor.Grid.Columns["Client"] != null)
                        {
                            gridListEditor.Grid.Columns["Client"].Width = 200;
                        }
                    }
                    gridListEditor.Grid.Load += Grid_Load;
                    if (View.Id == "Invoicing_ListView_Delivery")
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                         e.cancel = true;
                                           }";
                        if (gridListEditor.Grid.Columns["Email"] != null)
                        {
                            gridListEditor.Grid.Columns["Email"].Width = 135;
                        }
                    }
                }
                else if (View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Review" || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_View"
                    || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Delivery")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid.Columns["Qty"] != null)
                    {
                        gridListEditor.Grid.Columns["Qty"].Width = 40;
                    }
                    XafCallbackManager parameter = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    parameter.RegisterHandler("ParameterPopupView", this);
                    gridListEditor.Grid.ClientInstanceName = "ParameterShowView";
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    if (View.Id != "InvoicingAnalysisCharge_ListView_Invoice_Review")
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                         e.cancel = true;
                                           }";
                    }
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                }
                else if (View.Id == "Invoicing_DetailView_Delivery")
                {
                    foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible == true))
                    {
                        if (item is ASPxDoublePropertyEditor)
                        {
                            ASPxDoublePropertyEditor propertyEditor = (ASPxDoublePropertyEditor)item;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxLookupPropertyEditor)
                        {
                            ASPxLookupPropertyEditor propertyEditor = (ASPxLookupPropertyEditor)item;
                            if (propertyEditor != null && propertyEditor.DropDownEdit != null && propertyEditor.DropDownEdit.DropDown != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.DropDownEdit.DropDown.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxDecimalPropertyEditor)
                        {
                            ASPxDecimalPropertyEditor propertyEditor = (ASPxDecimalPropertyEditor)item;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }

                        }
                        else if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(AspxGridLookupCustomEditor))
                        {
                            AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                        {
                            ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                        {
                            ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                else if (View.Id == "Samplecheckin_ListView_Invoice")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        if (gridListEditor.Grid.Columns["Client"] != null)
                        {
                            gridListEditor.Grid.Columns["Client"].Width = 200;
                        }
                    }
                }
                if (View.Id == "Invoicing_ListView_Review")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        if (gridListEditor.Grid.Columns["InlineEditCommandColumn"] != null)
                        {
                            gridListEditor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                        }
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                        gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    }
                }
                else if (View.Id == "Invoicing_ItemCharges_ListView")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor.AllowEdit != null && gridlisteditor.Grid != null)
                    {
                        if (gridlisteditor.Grid.Columns["InlineEditCommandColumn"] != null)
                        {
                            gridlisteditor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                        }
                        if (objInvoiceInfo.IsItemchargePricingpopupselectall == true)
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = "selectall";
                        }
                        else
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = null;
                        }
                        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("QuotesItemCharge", this);
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        gridlisteditor.Grid.JSProperties["cpViewID"] = View.Id;
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
var chkselect = s.cpEndCallbackHandlers;
                        var viewid = s.cpViewID;
                        s.batchEditApi.ResetChanges(e.visibleIndex);
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {     

                            RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                         var viewid = s.cpViewID;
        if(chkselect != 'selectall')
        {
            RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|Selectall', '', false); 
        }
                        //RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                            var viewid = s.cpViewID;
                        RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|UNSelectall', '', false);                        
                      }   
                      ////else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.pageRowCount)
                      ////{   
                      ////   var viewid = s.cpViewID;
                      ////  RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|Selectall', '', false);     
                      ////}
                    }";
                        gridlisteditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
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
                        gridlisteditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function (s, e){
                                                window.setTimeout(function() {
                                                //s.UpdateEdit();  
                                                            if (e.visibleIndex != '-1')
                                                            {
                                                                var viewid = s.cpViewID;
                                                                var previousColumn = sessionStorage.getItem('PrevFocusedColumn');
                                                                //s.batchEditApi.ResetChanges(e.visibleIndex);
                                                                s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                                                                RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|ValuesChange|' + Oidvalue +'|'+  s.cpViewID+'|'+  previousColumn, '', false); 
                                                                }); 
                                                            }
                                                }, 20); 
                                            }";
                    }
                }
                else if (View.Id == "ItemChargePricing_ListView_Invoice_Popup")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        if (objInvoiceInfo.IsItemchargePricingpopupselectall == true)
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = "selectall";
                        }
                        else
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = null;
                        }
                        gridlisteditor.Grid.Settings.VerticalScrollableHeight = 300;
                        gridlisteditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("QuotesItemPrice", this);
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.HtmlCommandCellPrepared += Grid_HtmlCommandCellPrepared;
                        gridlisteditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
var chkselect = s.cpEndCallbackHandlers;
                        s.batchEditApi.ResetChanges(e.visibleIndex);
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {     

                            RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {  
if(chkselect != 'selectall')
{
RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|Selectall', '', false);        
}
                        RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|UNSelectall', '', false);                        
                      }   
                      //else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.pageRowCount)
                      //{        
                      //  RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|Selectall', '', false);     
                      //}
                    }";
                    }
                }
                else if (View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Review")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.JSProperties["cpPagesize"] = gridListEditor.Grid.SettingsPager.PageSize;
                    gridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        {
                            var fieldName = e.cellInfo.column.fieldName;
                            sessionStorage.setItem('FocusedColumn', fieldName);
                        }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() { 
                            var fieldName = sessionStorage.getItem('FocusedColumn');
                            if( s.batchEditApi.HasChanges(e.visibleIndex) && (fieldName == 'UnitPrice' || fieldName== 'Prep1Price' || fieldName == 'Prep2Price' || fieldName == 'Discount' ))
                            {
                               var unitprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'UnitPrice');
                               var discount = s.batchEditApi.GetCellValue(e.visibleIndex, 'Discount');  
                               var qty = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty'); 
                               var prep1 = s.batchEditApi.GetCellValue(e.visibleIndex, 'Prep1Price'); 
                               var prep2 = s.batchEditApi.GetCellValue(e.visibleIndex, 'Prep2Price'); 
                               tatamt= unitprice + prep1 + prep2 ;
                               var totalamount=0
                               var surcharge=s.batchEditApi.GetCellValue(e.visibleIndex, 'Surcharge');
                               var surchargePrice=s.batchEditApi.GetCellValue(e.visibleIndex, 'TierPrice');
                               var persucharge =0;
                               //if(surcharge>0)
                               //  {
                               //     persucharge=tatamt * (surcharge / 100);
                               //     tatamt= tatamt + persucharge;
                               //  }
                           
                               if(fieldName == 'UnitPrice' || fieldName== 'Prep1Price' || fieldName == 'Prep2Price')
                               {
                                   s.batchEditApi.SetCellValue(e.visibleIndex, 'TotalUnitPrice', tatamt); 
                               }
                                var totalprice=surchargePrice;
                               //s.batchEditApi.SetCellValue(e.visibleIndex, 'TierPrice', tatamt);
                               //var totalprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'TotalUnitPrice');
                               if(discount>0)
                               {
                                 var discutamt = totalprice * (discount / 100);
                                 totalamount=qty * (totalprice - discutamt);
                               }
                               else if(discount<0)
                               {
                                 var discoamt = totalprice * (discount / 100);
                                 //totalamount=qty * (totalprice - discutamt);
                                totalamount= ((totalprice) - (discoamt)) * qty;
                               }
                               else
                               {
                                 totalamount=qty * totalprice ;
                               }
                               s.batchEditApi.SetCellValue(e.visibleIndex, 'Amount',Math.round(totalamount * 100) / 100);   
                              
                          }
                          else if(s.batchEditApi.HasChanges(e.visibleIndex) && (fieldName == 'TAT.Oid'))
                               {
                                    var currentTAT = s.batchEditApi.GetCellValue(e.visibleIndex, 'TAT.Oid');
                                    s.batchEditApi.ResetChanges(e.visibleIndex);
                                    s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {    
                                    RaiseXafCallback(globalCallbackControl, 'ParameterPopup',  'TATValuesChanged|'+ Oidvalue + '|' +  currentTAT, '', false); 
                                       }); 
                               }
                           
                            }, 20); }";
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                                {   
                           
                                var FocusedColumn = sessionStorage.getItem('FocusedColumn');                                
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
                                            s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);                                                                                 
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
                                            if(FocusedColumn=='Discount')
                                                {
                                                  var unprice = s.batchEditApi.GetCellValue(i,'TierPrice');
                                                  var qty = s.batchEditApi.GetCellValue(i, 'Qty');

                                                    if(unprice != null){                                         
                                                     var discoamt = unprice * (CopyValue / 100);                        
                                                     totalamount= ((unprice) - (discoamt)) * qty;
                                                    s.batchEditApi.SetCellValue(i,'Amount',totalamount); 
                                                   }
                                                }                                          
                                            s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                        }
                                    }                            
                                 }
                             e.processOnServer = false;
                        }";
                    if (gridListEditor.Grid.Columns["Qty"] != null)
                    {
                        gridListEditor.Grid.Columns["Qty"].Width = 40;
                    }
                    gridListEditor.Grid.ClientSideEvents.BatchEditConfirmShowing = @"function(s,e) 
                    { 
                        e.cancel = true;
                    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'Parameter')
                                             {
                                              e.cancel = true;
                                              }
                                          else
                                               {
                                                    e.cancel = false;
                                               }
                                           }";
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Access and customize the target View control.
        }

        private void Gridlisteditor_SelectionChanged(object sender, EventArgs e)
        {
            if (Application.MainWindow.View.SelectedObjects.Count > 0)
            {
                Invoicing invoicing = View.CurrentObject as Invoicing;
                //Invoicing LstInvoice = ObjectSpace.FindObject<Invoicing>(CriteriaOperator.Parse("[Status] = 'PendingReview'", invoicing.Oid));
                if (invoicing.Status == InviceStatus.PendingReview)
                {
                    Frame.GetController<ListViewController>().EditAction.Enabled.SetItemValue("pendingReview", true);

                }
                else
                {
                    Frame.GetController<ListViewController>().EditAction.Enabled.SetItemValue("othersstatus", false);

                }
            }
        }

        private void GridView_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Priority_ListView_Invoice")
                {
                    ASPxGridView grid = (ASPxGridView)sender;
                    if (grid != null)
                    {
                        foreach (Priority objPriority in ((ListView)View).CollectionSource.List)
                        {
                            if (objInvoiceInfo.InvoicePopupPriorityOid == objPriority.Oid)
                            {
                                grid.Selection.SelectRowByKey(objPriority.Oid);
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

        private void Grid_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (gridView != null)
                {
                    if (e.CommandCellType == GridViewTableCommandCellType.Data)
                    {
                        //if (e.CommandColumn.Name == "SelectionCommandColumn")
                        {
                            List<Guid> lstitemchargeoid = new List<Guid>();
                            ListPropertyEditor liInvoicingItemCharges = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                            lstitemchargeoid = liInvoicingItemCharges.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Select(i => i.ItemPrice.Oid).ToList();
                            string strpricecodeoid = gridView.GetRowValuesByKeyValue(e.KeyValue, "Oid").ToString();
                            if (lstitemchargeoid.Contains(new Guid(strpricecodeoid.ToString())))
                            {
                                ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Enabled = false;
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

        private void Grid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");

                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";
                    e.Items.Remove(e.Items.FindByText("Edit"));
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
                if (View.Id == "Invoicing_ListView_Review")
                {

                    foreach (Invoicing objAB in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objAB))
                        {
                            objAB.DateReviewed = DateTime.Now;
                            objAB.ReviewedBy = View.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            objAB.DateReviewed = null;
                            objAB.ReviewedBy = null;
                        }
                    }
                    View.Refresh();
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
                //if (View.Id == "TurnAroundTime_ListView_Invoice")
                //{
                //    InvoicingAnalysisCharge objanalysisprice = null;
                //    if (Frame is NestedFrame)
                //    {
                //        NestedFrame nestedFrame = (NestedFrame)Frame;
                //        objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                //    }
                //    else
                //    {
                //        objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid()));
                //    }
                //    //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                //    if (objanalysisprice != null && objanalysisprice.TAT != null)
                //    {
                //        gridView.Selection.SelectRowByKey(objanalysisprice.TAT.Oid);
                //    }
                //}
                if (View.Id == "InvoicingAnalysisCharge_ListView_Invoice" || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Review"
                    || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Delivery" || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_View"
                    || View.Id == "InvoicingAnalysisCharge_ListView_Queue")
                {

                    if (gridView.VisibleColumns["SelectionCommandColumn"] != null)
                    {
                        gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["Qty"] != null)
                    {
                        gridView.VisibleColumns["Qty"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["PriceCode"] != null)
                    {
                        gridView.VisibleColumns["PriceCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["Matrix"] != null)
                    {
                        gridView.VisibleColumns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["Test"] != null)
                    {
                        gridView.VisibleColumns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }

                }
                else if (View.Id == "Invoicing_ListView_Delivery" || View.Id == "Invoicing_ListView_Review")
                {
                    if (gridView.VisibleColumns["SelectionCommandColumn"] != null)
                    {
                        gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["Edit"] != null)
                    {
                        gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["MailContent"] != null)
                    {
                        gridView.VisibleColumns["MailContent"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["InvoicePreview"] != null)
                    {
                        gridView.VisibleColumns["InvoicePreview"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["InvoiceID"] != null)
                    {
                        gridView.VisibleColumns["InvoiceID"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["JobID"] != null)
                    {
                        gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.VisibleColumns["Client"] != null)
                    {
                        gridView.VisibleColumns["Client"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (!IsReviewRefresh)
                    {
                        gridView.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        IsReviewRefresh = true;
                    }
                }
                //else if(View.Id== "Invoicing_ListView")
                //{
                //    if (gridView.VisibleColumns["SelectionCommandColumn"] != null)
                //    {
                //        gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    }
                //    if (gridView.VisibleColumns["Edit"] != null)
                //    {
                //        gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    }
                //    if (gridView.VisibleColumns["InvoicePreview"] != null)
                //    {
                //        gridView.VisibleColumns["InvoicePreview"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    }
                //    if (gridView.VisibleColumns["InvoiceID"] != null)
                //    {
                //        gridView.VisibleColumns["InvoiceID"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    }
                //    if (gridView.VisibleColumns["JobID"] != null)
                //    {
                //        gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    }
                //    if (gridView.VisibleColumns["Client"] != null)
                //    {
                //        gridView.VisibleColumns["Client"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    }
                //    gridView.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                //}
                else if (View.Id == "ItemChargePricing_ListView_Invoice_Popup")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.JSProperties["cpVisibleRowCount"] = editor.Grid.VisibleRowCount;
                        if (editor.Grid.Columns["InlineEditCommandColumn"] != null)
                        {
                            editor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                        }
                        List<ItemChargePricing> lstitemchargeoid = new List<ItemChargePricing>();
                        ListPropertyEditor liInvoicingItemCharges = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                        lstitemchargeoid = liInvoicingItemCharges.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Select(i => i.ItemPrice).ToList();
                        foreach (ItemChargePricing objitemcharge in lstitemchargeoid.ToList())
                        {
                            editor.Grid.Selection.SelectRowByKey(objitemcharge.Oid);
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

        private void GridLookup_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ((Invoicing)View.CurrentObject).JobID = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("JobID"));
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
                if (View.Id == "Invoicing_DetailView" || View.Id == "Invoicing_DetailView_Queue" || View.Id == "Invoicing_DetailView_Review" || View.Id == "Invoicing_DetailView_View_History")
                {
                    objInvoiceInfo.popupcurtinvoice = null;
                    objInvoiceInfo.lvDetailedPrice = 0;
                    //objInvoiceInfo.IsobjChangedproperty = false;
                    //objInvoiceInfo.IsobjChangedpropertyinQuotes = false;
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing; ;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAction.Executed -= SaveAction_Executed;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed -= DeleteAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed -= SaveAndCloseAction_Executed;
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    if (View.Id == "Invoicing_DetailView_Queue")
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        IList<Invoicing> LstInvoice = os.GetObjects<Invoicing>(CriteriaOperator.Parse("[Status] = 'PendingInvoicing'"));
                        foreach (Invoicing obj in LstInvoice.ToList())
                        {
                            IList<InvoicingAnalysisCharge> lstInvAnalCharge = os.GetObjects<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Invoice]=?", obj.Oid));
                            foreach (InvoicingAnalysisCharge ObjCharge in lstInvAnalCharge.ToList())
                            {
                                os.Delete(ObjCharge);
                            }
                            os.Delete(obj);
                        }
                        os.CommitChanges();
                    }
                }
                else if (View.Id == "Invoicing_ItemCharges_ListView")
                {
                    if (objInvoiceInfo.lsttempItemPricing != null)
                    {
                        objInvoiceInfo.lsttempItemPricing.Clear();
                    }
                }
                else if (View.Id == "Invoicing_ListView" || View.Id == "Invoicing_ListView_Review")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed -= DeleteAction_Executed;
                    View.ControlsCreated -= View_ControlsCreated;
                    IsReviewRefresh = false;
                }
                else if (View.Id == "Samplecheckin_ListView_Invoice")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.CustomGetTotalTooltip -= NewObjectAction_CustomGetTotalTooltip;
                }
                else if (View.Id == "InvoicingAnalysisCharge_ListView_Invoice" || View.Id == "InvoicingAnalysisCharge_ListView_Queue")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                    View.ControlsCreated -= View_ControlsCreated;
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "Invoicing_ItemCharges_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                    View.ControlsCreated -= View_ControlsCreated;
                }
                else if (View.Id == "Samplecheckin_ListView_InvoiceJobID")
                {
                    View.ControlsCreated -= View_ControlsCreated;
                }
                if (View.Id == "Invoicing_DetailView_View_History" || View.Id == "Invoicing_DetailView_Review" || View.Id == "Invoicing_DetailView_Delivery"
                  || View.Id == "Invoicing_DetailView_Review_History")
                {
                    DashboardViewItem lvAnalysisCharge = ((DetailView)View).FindItem("AnalysisCharge") as DashboardViewItem;
                    if (lvAnalysisCharge != null)
                    {
                        lvAnalysisCharge.ControlCreated -= lvAnalysisCharge_ControlCreated;
                    }
                }
                else if (View.Id == "ItemChargePricing_ListView_Invoice_Popup")
                {
                    objInvoiceInfo.IsItemchargePricingpopupselectall = false;
                    if (objInvoiceInfo.lsttempItemPricingpopup != null && objInvoiceInfo.lsttempItemPricingpopup.Count > 0)
                    {
                        objInvoiceInfo.lsttempItemPricingpopup.Clear();
                    }
                }
                else if (View.Id == "Invoicing_DetailView_Queue" || View.Id == "InvoicingAnalysisCharge_ListView_Queue")
                {
                    Frame.GetController<RefreshController>().RefreshAction.Executing -= RefreshAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SelectJobID_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

                CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Samplecheckin));
                Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                cs.Criteria.Clear();
                if (objInVoice != null && objInVoice.Client != null)
                {
                    cs.Criteria["Filter1"] = CriteriaOperator.Parse("ClientName.Oid=?", objInVoice.Client.Oid);
                }
                if (objInVoice != null && !string.IsNullOrEmpty(objInVoice.JobID))
                {
                    HttpContext.Current.Session["JobID"] = objInVoice.JobID;
                }
                IList<SampleParameter> samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] = 'Reported' And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"));

                if (samples.Count > 0)
                {
                    cs.Criteria["Filter"] = new InOperator("Oid", samples.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID.Oid).Distinct().ToList());

                }
                else
                {
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("Oid is null");
                }
                ListView lv = Application.CreateListView("Samplecheckin_ListView_InvoiceJobID", cs, false);
                ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                showViewParameters.CreatedView.Caption = "JobID";
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                dc.Accepting += Dc_Accepting;
                dc.AcceptAction.Execute += AcceptAction_Execute;
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
                if (sender != null)
                {
                    DialogController dc = (DialogController)sender;
                    if (dc != null)
                    {
                        if (dc.Window.View.SelectedObjects.Count == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                        else
                        {
                            int clientCount = dc.Window.View.SelectedObjects.Cast<Samplecheckin>().Where(i => i.ClientName != null).Select(i => i.ClientName).Distinct().Count();
                            if (clientCount > 1)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "jobidsameclient"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
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
        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string assignedJobId = string.Empty;
                Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                DashboardViewItem liInvoicingAnalysisCharges = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                if (liInvoicingAnalysisCharges != null)
                {
                    IObjectSpace os = liInvoicingAnalysisCharges.InnerView.ObjectSpace;
                    List<Samplecheckin> lstSampleCheckins = e.SelectedObjects.Cast<Samplecheckin>().ToList();
                    List<Guid> lstOid = lstSampleCheckins.Select(i => i.Oid).Distinct().ToList();
                    List<Guid> lstQuoteID = lstSampleCheckins.Where(i => i.QuoteID != null).Select(i => i.QuoteID.Oid).Distinct().ToList();
                    string strJobID = string.Format("{0}", string.Join(", ", lstSampleCheckins.Select(i => i.JobID).Distinct().ToList()));
                    IList<SampleParameter> lstSampleParams = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.Oid", lstOid), (CriteriaOperator.Parse("[Status] = 'Reported' And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"))));
                    //IList<SampleParameter> lstSampleParams = os.GetObjects<SampleParameter>(new InOperator("Samplelogin.JobID.Oid", lstOid));
                    if (lstSampleParams.Count > 0)
                    {
                        //List<Guid> lstTestParamOids = lstSampleParams.Select(i => i.Testparameter.Oid).Distinct().ToList();
                        if (lstSampleParams.FirstOrDefault(i => i.IsGroup == false) != null)
                        {
                            List<Guid> lstTestParamOids = lstSampleParams.Where(i => i.IsGroup == false).GroupBy(p => new { p.Testparameter.TestMethod.MatrixName.MatrixName, p.Testparameter.TestMethod.MethodName.MethodNumber, p.Testparameter.TestMethod.TestName, p.Testparameter.Component.Components }).Select(g => g.FirstOrDefault().Testparameter.Oid).ToList();
                            if (lstTestParamOids != null && lstTestParamOids.Count > 0)
                            {
                                foreach (Guid oid in lstTestParamOids)
                                {
                                    Testparameter param = os.GetObjectByKey<Testparameter>(oid);
                                    InvoicingAnalysisCharge newObj = os.CreateObject<InvoicingAnalysisCharge>();
                                    newObj.Test = param.TestMethod;
                                    newObj.Method = param.TestMethod.MethodName;
                                    newObj.Matrix = param.TestMethod.MatrixName;
                                    newObj.Component = param.Component;
                                    newObj.Testparameter = os.GetObjectByKey<Testparameter>(oid);
                                    newObj.JobID = strJobID;
                                    newObj.IsGroup = false;
                                    // IList<SampleParameter> lstSamplePriority = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.Oid", lstOid), (CriteriaOperator.Parse("([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null) And [Testparameter.TestMethod.MatrixName]=? And [Testparameter.TestMethod.TestName]=?" +
                                    //     "And [.Testparameter.TestMethod.MethodName]=?", param.TestMethod.MatrixName,param.TestMethod.TestName,param.TestMethod.MethodName))));
                                    if (lstSampleCheckins.Count > 0)
                                    {
                                        Guid TAT = lstSampleCheckins.Where(i => i.TAT != null).Select(i => i.TAT.Oid).FirstOrDefault();
                                        if (TAT != null)
                                        {
                                            TestPriceSurcharge testPriceSurcharges = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And[Component.Components] = ? And Contains([TAT], ?)", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components, TAT.ToString().Replace(" ", "")));
                                            if (testPriceSurcharges != null)
                                            {
                                                if (testPriceSurcharges.Surcharge != null)
                                                {
                                                    newObj.Surcharge = (int)testPriceSurcharges.Surcharge;
                                                }
                                                newObj.Priority = testPriceSurcharges.Priority;
                                                newObj.TAT = os.GetObjectByKey<TurnAroundTime>(TAT);
                                            }
                                            else
                                            {
                                                Modules.BusinessObjects.Setting.Priority priority = os.FindObject<Modules.BusinessObjects.Setting.Priority>(CriteriaOperator.Parse("[Priority] = 'Regular'"));
                                                if (priority != null)
                                                {
                                                    newObj.Surcharge = 0;
                                                    newObj.Priority = priority;
                                                    newObj.TAT = os.GetObjectByKey<TurnAroundTime>(TAT);
                                                }
                                            }
                                        }
                                    }
                                    bool isQuotes = false;
                                    bool isConstutientPrice = false;
                                    foreach (Guid objquoteid in lstQuoteID)
                                    {
                                        AnalysisPricing objAnaprice = os.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ? And [CRMQuotes]=?", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components, objquoteid));
                                        if (objAnaprice != null)
                                        {
                                            newObj.UnitPrice = objAnaprice.UnitPrice;
                                            //newObj.TierPrice = objAnaprice.UnitPrice;
                                            newObj.ChargeType = objAnaprice.ChargeType;
                                            newObj.Prep1Price = objAnaprice.Prep1Charge;
                                            newObj.Prep2Price = objAnaprice.Prep2Charge;
                                            newObj.PriceCode = objAnaprice.PriceCode;
                                            newObj.Parameter = objAnaprice.Parameter;
                                            newObj.Discount = Convert.ToDecimal(objAnaprice.Discount);
                                            isQuotes = true;
                                            break;
                                        }
                                    }
                                    if (isQuotes == false)
                                    {
                                        ConstituentPricing constitudentPricing = os.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ?", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components));
                                        if (constitudentPricing != null)
                                        {
                                            IList<ConstituentPricingTier> priceTier = os.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid]=?", constitudentPricing.Oid));
                                            int paramCount = lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == constitudentPricing.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == constitudentPricing.Test.TestName && i.Testparameter.TestMethod.MethodName.MethodNumber == constitudentPricing.Test.MethodName.MethodNumber && i.Testparameter.Component.Components == constitudentPricing.Component.Components).Select(i => i.Testparameter.Oid).Distinct().Count();
                                            foreach (ConstituentPricingTier objTirePrice in priceTier.OrderBy(i => i.From).ToList())
                                            {
                                                var obj = priceTier.OrderBy(i => i.From).LastOrDefault();
                                                if (paramCount == objTirePrice.From || paramCount <= objTirePrice.To || objTirePrice.Oid == obj.Oid)
                                                {
                                                    newObj.UnitPrice = objTirePrice.TierPrice * paramCount;
                                                    //newObj.TierPrice = objTirePrice.TierPrice * paramCount;
                                                    newObj.ChargeType = constitudentPricing.ChargeType;
                                                    newObj.Prep1Price = objTirePrice.Prep1Charge;
                                                    newObj.Prep2Price = objTirePrice.Prep2Charge;
                                                    newObj.PriceCode = constitudentPricing.PriceCode;
                                                    newObj.From = objTirePrice.From;
                                                    newObj.To = objTirePrice.To;
                                                    if (lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == constitudentPricing.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == constitudentPricing.Test.TestName && i.Testparameter.Component.Components == constitudentPricing.Component.Components).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count() > 0)
                                                    {
                                                        List<Guid> lstMethodOid = lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == constitudentPricing.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == constitudentPricing.Test.TestName && i.Testparameter.Component.Components == constitudentPricing.Component.Components).Select(i => i.Testparameter.TestMethod.Oid).Distinct().ToList();
                                                        IList<Testparameter> tests = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid]=? And [Component.Components]=?", constitudentPricing.Test.Oid, constitudentPricing.Component.Components));
                                                        int count = tests.Where(i => i.QCType != null && i.QCType.QCTypeName == "Sample").Count();
                                                        if (count != paramCount)
                                                        {
                                                            newObj.Parameter = "Customized";
                                                        }
                                                        else
                                                        {
                                                            newObj.Parameter = "AllParam";
                                                        }
                                                    }
                                                    isConstutientPrice = true;
                                                    break;
                                                }
                                            }


                                        }
                                    }
                                    if (isQuotes == false && isConstutientPrice == false)
                                    {
                                        DefaultPricing objDefaultPricing = os.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ?", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components));
                                        if (objDefaultPricing != null)
                                        {
                                            newObj.UnitPrice = objDefaultPricing.UnitPrice;
                                            //newObj.TierPrice = objDefaultPricing.UnitPrice;
                                            newObj.ChargeType = objDefaultPricing.ChargeType;
                                            newObj.Prep1Price = objDefaultPricing.Prep1Charge;
                                            newObj.Prep2Price = objDefaultPricing.Prep2Charge;
                                            newObj.PriceCode = objDefaultPricing.PriceCode;
                                            newObj.Parameter = "Allparam";
                                        }
                                    }
                                    ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.Add(newObj);


                                }
                            }
                        }
                        if (lstSampleParams.FirstOrDefault(i => i.IsGroup == true) != null)
                        {
                            //lstSampleParams.Select(i => i.Samplelogin.Oid).Distinct();
                            IList<SampleParameter> lstExistSampleParam = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.Oid", lstOid), (CriteriaOperator.Parse("[IsGroup] = True And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"))));
                            int Samplecount = lstSampleParams.Where(i => i.IsGroup == true && i.Status == Samplestatus.Reported).Count();
                            //IList<SampleParameter> lstExistSampleParam = os.GetObjects<SampleParameter>( CriteriaOperator.Parse(""));
                            if (lstExistSampleParam.Count == Samplecount)
                            {
                                IList<GroupTestMethod> lstGroupTest = os.GetObjects<GroupTestMethod>(new InOperator("Oid", lstExistSampleParam.Where(i => i.GroupTest != null && i.IsGroup == true).Select(i => i.GroupTest.Oid).Distinct().ToList()));

                                List<Guid> lstTestParamOids = lstGroupTest.Where(i => i.TestMethod != null).GroupBy(p => new { p.TestMethod.MatrixName.MatrixName, p.TestMethod.TestName }).Select(g => g.FirstOrDefault().TestMethod.Oid).ToList();
                                if (lstTestParamOids != null && lstTestParamOids.Count > 0)
                                {
                                    foreach (Guid oid in lstTestParamOids)
                                    {
                                        TestMethod objTest = os.GetObjectByKey<TestMethod>(oid);
                                        InvoicingAnalysisCharge newObj = os.CreateObject<InvoicingAnalysisCharge>();
                                        newObj.Test = objTest;
                                        //newObj.Method = param.TestMethod.MethodName;
                                        newObj.Matrix = objTest.MatrixName;
                                        //newObj.Component = param.Component;
                                        //newObj.Testparameter = os.GetObjectByKey<Testparameter>(oid);
                                        newObj.JobID = strJobID;
                                        newObj.IsGroup = true;
                                        bool isQuotes = false;
                                        if (lstSampleCheckins.Count > 0)
                                        {
                                            Guid TAT = lstSampleCheckins.Where(i => i.TAT != null).Select(i => i.TAT.Oid).FirstOrDefault();
                                            if (TAT != null)
                                            {
                                                TestPriceSurcharge testPriceSurcharges = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[IsGroup] = True And  [Matrix.MatrixName] = ? And [Test.TestName] = ? And Contains([TAT], ?)", objTest.MatrixName, objTest.TestName, TAT.ToString().Replace(" ", "")));
                                                if (testPriceSurcharges != null)
                                                {
                                                    if (testPriceSurcharges.Surcharge != null)
                                                    {
                                                        newObj.Surcharge = (int)testPriceSurcharges.Surcharge;
                                                    }
                                                    newObj.Priority = testPriceSurcharges.Priority;
                                                    newObj.TAT = os.GetObjectByKey<TurnAroundTime>(TAT);
                                                }
                                                else
                                                {
                                                    Modules.BusinessObjects.Setting.Priority priority = os.FindObject<Modules.BusinessObjects.Setting.Priority>(CriteriaOperator.Parse("[Priority] = 'Regular'"));
                                                    if (priority != null)
                                                    {
                                                        newObj.Surcharge = 0;
                                                        newObj.Priority = priority;
                                                        newObj.TAT = os.GetObjectByKey<TurnAroundTime>(TAT);
                                                    }
                                                }
                                            }
                                        }
                                        foreach (Guid objquoteid in lstQuoteID)
                                        {
                                            AnalysisPricing objAnaprice = os.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ?  And [CRMQuotes]=?", objTest.MatrixName.MatrixName, objTest.TestName, objquoteid));
                                            if (objAnaprice != null)
                                            {
                                                newObj.UnitPrice = objAnaprice.UnitPrice;
                                                //newObj.TierPrice = objAnaprice.UnitPrice;
                                                newObj.ChargeType = objAnaprice.ChargeType;
                                                newObj.Prep1Price = objAnaprice.Prep1Charge;
                                                newObj.Prep2Price = objAnaprice.Prep2Charge;
                                                newObj.PriceCode = objAnaprice.PriceCode;
                                                newObj.Parameter = objAnaprice.Parameter;
                                                newObj.Discount = Convert.ToDecimal(objAnaprice.Discount);
                                                isQuotes = true;
                                                break;
                                            }
                                        }
                                        if (isQuotes == false)
                                        {
                                            DefaultPricing objDefaultPricing = os.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ?", objTest.MatrixName.MatrixName, objTest.MethodName.MethodNumber, objTest.TestName));
                                            if (objDefaultPricing != null)
                                            {
                                                newObj.UnitPrice = objDefaultPricing.UnitPrice;
                                                //newObj.TierPrice = objDefaultPricing.UnitPrice;
                                                newObj.ChargeType = objDefaultPricing.ChargeType;
                                                newObj.Prep1Price = objDefaultPricing.Prep1Charge;
                                                newObj.Prep2Price = objDefaultPricing.Prep2Charge;
                                                newObj.PriceCode = objDefaultPricing.PriceCode;
                                                newObj.Parameter = "Allparam";
                                            }
                                        }
                                        ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.Add(newObj);


                                    }
                                }
                            }
                            //List<Guid> lstTestmethodOid = lstSampleParams.Where(i => i.IsGroup == true && i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().ToList();

                        }
                    }
                    ((ListView)liInvoicingAnalysisCharges.InnerView).Refresh();
                    if (objInVoice != null)
                    {
                        objInVoice.JobID = strJobID;
                        if (lstQuoteID.Count > 0)
                        {
                            CRMQuotes objQuotes = Application.MainWindow.View.ObjectSpace.FindObject<CRMQuotes>(CriteriaOperator.Parse("[Oid]=?", new Guid(lstQuoteID.FirstOrDefault().ToString())));
                            if (objQuotes != null)
                            {
                                objInVoice.QuoteID = objQuotes;
                                objInVoice.QuotedBy = objQuotes.QuotedBy;
                                objInVoice.QuotedDate = objQuotes.QuotedDate;
                            }
                        }
                        Guid objSample = lstSampleCheckins.OrderByDescending(i => i.JobID).Select(i => i.Oid).FirstOrDefault();
                        if (objSample != null)
                        {
                            Samplecheckin objcheck = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Samplecheckin>(objSample);
                            if (objcheck != null)
                            {
                                if (objcheck.ClientContact != null)
                                {
                                    objInVoice.PrimaryContact = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Contact>(objcheck.ClientContact.Oid);
                                }
                                objInVoice.DueDate = (DateTime)objcheck.DueDate;
                                if (objcheck.TAT != null)
                                {
                                    objInVoice.TAT = Application.MainWindow.View.ObjectSpace.GetObjectByKey<TurnAroundTime>(objcheck.TAT.Oid);
                                }
                                if (objcheck.ProjectID != null)
                                {
                                    objInVoice.ProjectID = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Project>(objcheck.ProjectID.Oid);
                                }
                                if (objcheck.ClientName != null)
                                {
                                    objInVoice.AccountNumber = objcheck.ClientName.Account;
                                }
                            }
                        }


                    }
                }
                Application.MainWindow.View.Refresh();
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
                if (View.Id == "InvoicingAnalysisCharge_ListView_Queue")
                {
                    Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                    DashboardViewItem liInvoicingAnalysisCharges = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                    ListPropertyEditor liInvoicingItemCharges = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                    if (((ListView)View).CollectionSource.GetCount() == 0 && !string.IsNullOrEmpty(objInVoice.JobID))
                    {
                        IObjectSpace os = liInvoicingAnalysisCharges.InnerView.ObjectSpace;
                        //List<string> lstJobID = e.SelectedObjects.Cast<Samplecheckin>().ToList();
                        List<Samplecheckin> lstSampleCheckins = os.GetObjects<Samplecheckin>(new InOperator("JobID", objInVoice.JobID.Split(';').ToList().Select(i => i = i.Trim()).ToList())).ToList();
                        List<Guid> lstOid = lstSampleCheckins.Select(i => i.Oid).Distinct().ToList();
                        List<Guid> lstQuoteID = lstSampleCheckins.Where(i => i.QuoteID != null).Select(i => i.QuoteID.Oid).Distinct().ToList();
                        string strJobID = string.Format("{0}", string.Join(", ", lstSampleCheckins.Select(i => i.JobID).Distinct().ToList()));
                        string TAT = lstSampleCheckins.Where(i => i.TAT != null).Select(i => i.TAT.TAT).FirstOrDefault();
                        Guid TATOid = lstSampleCheckins.Where(i => i.TAT != null).Select(i => i.TAT.Oid).FirstOrDefault();
                        IList<SampleParameter> lstSampleParams = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.Oid", lstOid), (CriteriaOperator.Parse("[Samplelogin.JobID.Status]  > 1 And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null And [Samplelogin.ExcludeInvoice] = False) And ([TestHold] = False Or [TestHold] Is null)"))));
                        //IList<SampleParameter> lstSampleParams = os.GetObjects<SampleParameter>(new InOperator("Samplelogin.JobID.Oid", lstOid));
                        if (lstSampleParams.Count > 0)
                        {
                            //List<Guid> lstTestParamOids = lstSampleParams.Select(i => i.Testparameter.Oid).Distinct().ToList();
                            if (lstSampleParams.FirstOrDefault(i => i.IsGroup == false) != null)
                            {
                                List<Guid> lstTestParamOids = lstSampleParams.Where(i => i.IsGroup == false).GroupBy(p => new { p.Testparameter.TestMethod.MatrixName.MatrixName, p.Testparameter.TestMethod.MethodName.MethodNumber, p.Testparameter.TestMethod.TestName, p.Testparameter.Component.Components }).Select(g => g.FirstOrDefault().Testparameter.Oid).ToList();
                                if (lstTestParamOids != null && lstTestParamOids.Count > 0)
                                {
                                    foreach (Guid oid in lstTestParamOids)
                                    {
                                        TAT = lstSampleParams.Where(i => i.Testparameter.Oid == oid && i.TAT != null).Select(j=> j.TAT.TAT).FirstOrDefault();
                                        TATOid = lstSampleParams.Where(i => i.Testparameter.Oid == oid && i.TAT != null).Select(j => j.TAT.Oid).FirstOrDefault();
                                        Testparameter param = os.GetObjectByKey<Testparameter>(oid);
                                        InvoicingAnalysisCharge newObj = os.CreateObject<InvoicingAnalysisCharge>();
                                        newObj.Test = param.TestMethod;
                                        newObj.Method = param.TestMethod.MethodName;
                                        newObj.Matrix = param.TestMethod.MatrixName;
                                        newObj.Component = param.Component;
                                        newObj.Testparameter = os.GetObjectByKey<Testparameter>(oid);
                                        newObj.JobID = strJobID;
                                        newObj.IsGroup = false;
                                        // IList<SampleParameter> lstSamplePriority = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.Oid", lstOid), (CriteriaOperator.Parse("([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null) And [Testparameter.TestMethod.MatrixName]=? And [Testparameter.TestMethod.TestName]=?" +
                                        //     "And [.Testparameter.TestMethod.MethodName]=?", param.TestMethod.MatrixName,param.TestMethod.TestName,param.TestMethod.MethodName))));
                                        if (lstSampleCheckins.Count > 0)
                                        {
                                            //Guid TAT = lstSampleCheckins.Where(i => i.TAT != null).Select(i => i.TAT.Oid).FirstOrDefault();
                                            if (TAT != null)
                                            {
                                                TestPriceSurcharge testPriceSurcharges = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And[Component.Components] = ? And Contains([TAT], ?)", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components, TAT.Trim()));
                                                if (testPriceSurcharges != null)
                                                {
                                                    //if (testPriceSurcharges.Surcharge != null)
                                                    //{
                                                    //    newObj.Surcharge = (int)testPriceSurcharges.Surcharge;
                                                    //}
                                                    newObj.Priority = testPriceSurcharges.Priority;
                                                    newObj.TAT = os.GetObjectByKey<TurnAroundTime>(TATOid);
                                                    newObj.PriceCode = testPriceSurcharges.PriceCode;
                                                    if (testPriceSurcharges.Priority != null)
                                                    {
                                                        objInVoice.Priority = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Priority>(testPriceSurcharges.Priority.Oid);
                                                    }
                                                }
                                                else
                                                {
                                                    //Modules.BusinessObjects.Setting.Priority priority = os.FindObject<Modules.BusinessObjects.Setting.Priority>(CriteriaOperator.Parse("[Prioritys] = 'Regular'"));
                                                    //if (priority != null)
                                                    //{
                                                    //    newObj.Surcharge = 0;
                                                    //    newObj.Priority = priority;
                                                    newObj.TAT = os.GetObjectByKey<TurnAroundTime>(TATOid);
                                                    //}
                                                }
                                            }
                                        }
                                        bool isQuotes = false;
                                        bool isConstutientPrice = false;
                                        foreach (Guid objquoteid in lstQuoteID)
                                        {
                                            AnalysisPricing objAnaprice = os.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ? And [CRMQuotes]=? And [TAT] = ?", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components, objquoteid, TATOid));
                                            if (objAnaprice != null)
                                            {
                                                newObj.UnitPrice = objAnaprice.TotalTierPrice;
                                                newObj.TierPrice = objAnaprice.TotalTierPrice;
                                                newObj.ChargeType = objAnaprice.ChargeType;
                                                newObj.Prep1Price = objAnaprice.Prep1Charge;
                                                newObj.Prep2Price = objAnaprice.Prep2Charge;
                                                //newObj.PriceCode = objAnaprice.PriceCode;
                                                newObj.Parameter = objAnaprice.Parameter;
                                                newObj.Discount = Convert.ToDecimal(objAnaprice.Discount);
                                                newObj.TierNo = objAnaprice.TierNo;
                                                newObj.From = objAnaprice.From;
                                                newObj.To = objAnaprice.To;
                                                newObj.Description = objAnaprice.TestDescription;
                                                isQuotes = true;
                                                break;
                                            }
                                        }
                                        //if (isQuotes == false)
                                        //{
                                        //    ConstituentPricing constitudentPricing = os.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ?", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components));
                                        //    if (constitudentPricing != null && constitudentPricing.ChargeType == ChargeType.Parameter)
                                        //    {
                                        //        IList<ConstituentPricingTier> priceTier = os.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid]=?", constitudentPricing.Oid));
                                        //        int paramCount = lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == constitudentPricing.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == constitudentPricing.Test.TestName && i.Testparameter.TestMethod.MethodName.MethodNumber == constitudentPricing.Test.MethodName.MethodNumber && i.Testparameter.Component.Components == constitudentPricing.Component.Components).Select(i => i.Testparameter.Oid).Distinct().Count();
                                        //        foreach (ConstituentPricingTier objTirePrice in priceTier.OrderBy(i => i.From).ToList())
                                        //        {
                                        //            var obj = priceTier.OrderBy(i => i.From).LastOrDefault();
                                        //            if (paramCount == objTirePrice.From || paramCount <= objTirePrice.To || objTirePrice.Oid == obj.Oid)
                                        //            {
                                        //                newObj.UnitPrice = objTirePrice.TierPrice * paramCount;
                                        //                //newObj.TierPrice = objTirePrice.TierPrice * paramCount;
                                        //                newObj.ChargeType = constitudentPricing.ChargeType;
                                        //                newObj.Prep1Price = objTirePrice.Prep1Charge;
                                        //                newObj.Prep2Price = objTirePrice.Prep2Charge;
                                        //                //newObj.PriceCode = constitudentPricing.PriceCode;
                                        //                newObj.From = objTirePrice.From;
                                        //                newObj.To = objTirePrice.To;
                                        //                newObj.TierNo = objTirePrice.TierNo;
                                        //                if (lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == constitudentPricing.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == constitudentPricing.Test.TestName && i.Testparameter.Component.Components == constitudentPricing.Component.Components).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count() > 0)
                                        //                {
                                        //                    List<Guid> lstMethodOid = lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == constitudentPricing.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == constitudentPricing.Test.TestName && i.Testparameter.Component.Components == constitudentPricing.Component.Components).Select(i => i.Testparameter.TestMethod.Oid).Distinct().ToList();
                                        //                    IList<Testparameter> tests = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid]=? And [Component.Components]=?", constitudentPricing.Test.Oid, constitudentPricing.Component.Components));
                                        //                    int count = tests.Where(i => i.QCType != null && i.QCType.QCTypeName == "Sample").Count();
                                        //                    if (count != paramCount)
                                        //                    {
                                        //                        newObj.Parameter = "Customized";
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        newObj.Parameter = "AllParam";
                                        //                    }
                                        //                }
                                        //                isConstutientPrice = true;
                                        //                break;
                                        //            }
                                        //        }

                                        //    }
                                        //    else if (constitudentPricing != null && constitudentPricing.ChargeType == ChargeType.Test)
                                        //    {
                                        //        IList<ConstituentPricingTier> priceTier = os.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid]=?", constitudentPricing.Oid));
                                        //        int testCount = lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == constitudentPricing.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == constitudentPricing.Test.TestName && i.Testparameter.TestMethod.MethodName.MethodNumber == constitudentPricing.Test.MethodName.MethodNumber && i.Testparameter.Component.Components == constitudentPricing.Component.Components).Select(i => i.Samplelogin.Oid).Distinct().Count();
                                        //        foreach (ConstituentPricingTier objTirePrice in priceTier.OrderBy(i => i.From).ToList())
                                        //        {
                                        //            var obj = priceTier.OrderBy(i => i.From).LastOrDefault();
                                        //            if (testCount == objTirePrice.From || testCount <= objTirePrice.To || objTirePrice.Oid == obj.Oid)
                                        //            {
                                        //                newObj.UnitPrice = objTirePrice.TierPrice;
                                        //                newObj.ChargeType = constitudentPricing.ChargeType;
                                        //                newObj.Prep1Price = objTirePrice.Prep1Charge;
                                        //                newObj.Prep2Price = objTirePrice.Prep2Charge;
                                        //                newObj.From = objTirePrice.From;
                                        //                newObj.To = objTirePrice.To;
                                        //                newObj.TierNo = objTirePrice.TierNo;
                                        //                isConstutientPrice = true;
                                        //                break;
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        if (isQuotes == false && isConstutientPrice == false)
                                        {
                                            //DefaultPricing objDefaultPricing = os.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ?", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components));
                                            //if (objDefaultPricing != null)
                                            //{
                                            //    newObj.UnitPrice = objDefaultPricing.UnitPrice;
                                            //    //newObj.TierPrice = objDefaultPricing.UnitPrice;
                                            //    newObj.ChargeType = objDefaultPricing.ChargeType;
                                            //    newObj.Prep1Price = objDefaultPricing.Prep1Charge;
                                            //    newObj.Prep2Price = objDefaultPricing.Prep2Charge;
                                            //    //newObj.PriceCode = objDefaultPricing.PriceCode;
                                            //    newObj.Parameter = "Allparam";
                                            //}
                                            //TestPriceSurcharge testPriceSurcharges = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ?", param.TestMethod.MatrixName.MatrixName,param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName,param.Component.Components));
                                            TestPriceSurcharge testPriceSurcharges = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And[Component.Components] = ? And Contains([TAT], ?)", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components, TAT));
                                            if (testPriceSurcharges != null)
                                            {
                                                if (testPriceSurcharges.SurchargePrice != null)
                                                {
                                                    TestPriceSurcharge testPriceIsRegular = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And[Component.Components] = ? And [Priority.IsRegular] = True", param.TestMethod.MatrixName.MatrixName, param.TestMethod.MethodName.MethodNumber, param.TestMethod.TestName, param.Component.Components));
                                                    if (testPriceIsRegular != null && testPriceIsRegular.SurchargePrice != null)
                                                    {
                                                        newObj.UnitPrice = (decimal)testPriceIsRegular.SurchargePrice;
                                                    }
                                                    newObj.TierPrice = (decimal)testPriceSurcharges.SurchargePrice;

                                                    decimal discount = newObj.Discount;
                                                    uint qty = newObj.Qty;
                                                    decimal totalprice = newObj.TierPrice;
                                                    decimal totalamount;
                                                    if (discount > 0)
                                                    {
                                                        var discutamt = totalprice * (discount / 100);
                                                        totalamount = qty * (totalprice - discutamt);
                                                    }
                                                    else if (discount < 0)
                                                    {
                                                        var discoamt = totalprice * (discount / 100);
                                                        //totalamount=qty * (totalprice - discutamt) ;
                                                        totalamount = ((totalprice) - (discoamt)) * qty;
                                                    }
                                                    else
                                                    {
                                                        totalamount = qty * totalprice;
                                                    }
                                                    newObj.Amount = totalamount;
                                                }
                                                //newObj.ChargeType = testPriceSurcharges.ChargeType;
                                                //newObj.Prep1Price = objDefaultPricing.Prep1Charge;
                                                //newObj.Prep2Price = objDefaultPricing.Prep2Charge;
                                                //newObj.PriceCode = objDefaultPricing.PriceCode;
                                                newObj.Parameter = "Allparam";
                                            }
                                            else
                                            {
                                                msg = newObj.Test.TestName + " Test Not Set DefaultPrice! Do You Want to Continue ?";
                                                WebWindow.CurrentRequestWindow.RegisterClientScript("PriceNotSet", string.Format(CultureInfo.InvariantCulture, @"var cancelconfirm = confirm('" + msg + "'); {0}", NotSetPrice.CallbackManager.GetScript("PriceNotSet", "cancelconfirm")));
                                            }
                                        }

                                        ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.Add(newObj);


                                    }
                                }
                            }
                            if (lstSampleParams.FirstOrDefault(i => i.IsGroup == true) != null)
                            {
                                //lstSampleParams.Select(i => i.Samplelogin.Oid).Distinct();
                                IList<SampleParameter> lstExistSampleParam = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.Oid", lstOid), (CriteriaOperator.Parse("[IsGroup] = True And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"))));
                                int Samplecount = lstSampleParams.Where(i => i.IsGroup == true && i.Status == Samplestatus.Reported).Count();
                                //IList<SampleParameter> lstExistSampleParam = os.GetObjects<SampleParameter>( CriteriaOperator.Parse(""));
                                if (lstExistSampleParam.Count == Samplecount)
                                {
                                    IList<GroupTestMethod> lstGroupTest = os.GetObjects<GroupTestMethod>(new InOperator("Oid", lstExistSampleParam.Where(i => i.GroupTest != null && i.IsGroup == true).Select(i => i.GroupTest.Oid).Distinct().ToList()));

                                    List<Guid> lstTestParamOids = lstGroupTest.Where(i => i.TestMethod != null).GroupBy(p => new { p.TestMethod.MatrixName.MatrixName, p.TestMethod.TestName }).Select(g => g.FirstOrDefault().TestMethod.Oid).ToList();
                                    if (lstTestParamOids != null && lstTestParamOids.Count > 0)
                                    {
                                        foreach (Guid oid in lstTestParamOids)
                                        {
                                            TestMethod objTest = os.GetObjectByKey<TestMethod>(oid);
                                            InvoicingAnalysisCharge newObj = os.CreateObject<InvoicingAnalysisCharge>();
                                            newObj.Test = objTest;
                                            //newObj.Method = param.TestMethod.MethodName;
                                            newObj.Matrix = objTest.MatrixName;
                                            //newObj.Component = param.Component;
                                            //newObj.Testparameter = os.GetObjectByKey<Testparameter>(oid);
                                            newObj.JobID = strJobID;
                                            newObj.IsGroup = true;
                                            bool isQuotes = false;
                                            if (lstSampleCheckins.Count > 0)
                                            {
                                                //Guid TAT = lstSampleCheckins.Where(i => i.TAT != null).Select(i => i.TAT.Oid).FirstOrDefault();
                                                if (TAT != null)
                                                {
                                                    TestPriceSurcharge testPriceSurcharges = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[IsGroup] = True And  [Matrix.MatrixName] = ?  And [Test.TestName] = ? And Contains([TAT], ?)", objTest.MatrixName.MatrixName, objTest.TestName, TAT));
                                                    if (testPriceSurcharges != null)
                                                    {
                                                        //if (testPriceSurcharges.Surcharge != null)
                                                        //{
                                                        //    newObj.Surcharge = (int)testPriceSurcharges.Surcharge;
                                                        //}
                                                        newObj.Priority = testPriceSurcharges.Priority;
                                                        newObj.TAT = os.GetObjectByKey<TurnAroundTime>(TATOid);
                                                        newObj.PriceCode = testPriceSurcharges.PriceCode;
                                                        newObj.ChargeType = testPriceSurcharges.ChargeType;
                                                        if (testPriceSurcharges.Priority != null)
                                                        {
                                                            objInVoice.Priority = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Priority>(testPriceSurcharges.Priority.Oid);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Modules.BusinessObjects.Setting.Priority priority = os.FindObject<Modules.BusinessObjects.Setting.Priority>(CriteriaOperator.Parse("[Prioritys] = 'Regular'"));
                                                        if (priority != null)
                                                        {
                                                            newObj.Surcharge = 0;
                                                            newObj.Priority = priority;
                                                            newObj.TAT = os.GetObjectByKey<TurnAroundTime>(TATOid);
                                                        }
                                                    }
                                                }
                                            }
                                            foreach (Guid objquoteid in lstQuoteID)
                                            {
                                                AnalysisPricing objAnaprice = os.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ?  And [CRMQuotes]=?", objTest.MatrixName.MatrixName, objTest.TestName, objquoteid));
                                                if (objAnaprice != null)
                                                {
                                                    newObj.UnitPrice = objAnaprice.TotalTierPrice;
                                                    newObj.TierPrice = objAnaprice.TotalTierPrice;
                                                    newObj.ChargeType = objAnaprice.ChargeType;
                                                    newObj.Prep1Price = objAnaprice.Prep1Charge;
                                                    newObj.Prep2Price = objAnaprice.Prep2Charge;
                                                    //newObj.PriceCode = objAnaprice.PriceCode;
                                                    newObj.Parameter = objAnaprice.Parameter;
                                                    newObj.Discount = Convert.ToDecimal(objAnaprice.Discount);
                                                    newObj.TierNo = objAnaprice.TierNo;
                                                    newObj.From = objAnaprice.From;
                                                    newObj.To = objAnaprice.To;
                                                    newObj.Description = objAnaprice.TestDescription;
                                                    isQuotes = true;
                                                    break;
                                                }
                                            }
                                            if (isQuotes == false)
                                            {
                                                //DefaultPricing objDefaultPricing = os.FindObject<DefaultPricing>(CriteriaOperator.Parse("[IsGroup] = 'Yes' And [Matrix.MatrixName] = ? And [Test.TestName] = ? ", objTest.MatrixName.MatrixName, objTest.TestName));
                                                //if (objDefaultPricing != null)
                                                //{
                                                //    newObj.UnitPrice = objDefaultPricing.UnitPrice;
                                                //    //newObj.TierPrice = objDefaultPricing.UnitPrice;
                                                //    newObj.ChargeType = objDefaultPricing.ChargeType;
                                                //    newObj.Prep1Price = objDefaultPricing.Prep1Charge;
                                                //    newObj.Prep2Price = objDefaultPricing.Prep2Charge;
                                                //    //newObj.PriceCode = objDefaultPricing.PriceCode;
                                                //    newObj.Parameter = "Allparam";
                                                //}
                                                //TestPriceSurcharge testPriceSurcharges = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[IsGroup] = True And  [Matrix.MatrixName] = ?  And [Test.TestName] = ? And [Priority.IsRegular] = 'True'", objTest.MatrixName.MatrixName, objTest.TestName));
                                                TestPriceSurcharge testPriceSurcharges = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[IsGroup] = True And  [Matrix.MatrixName] = ?  And [Test.TestName] = ? And Contains([TAT], ?)", objTest.MatrixName.MatrixName, objTest.TestName, TAT));
                                                if (testPriceSurcharges != null)
                                                {
                                                    if (testPriceSurcharges.SurchargePrice != null)
                                                    {
                                                        TestPriceSurcharge testPriceIsRegular = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And  [Test.TestName] = ? And [Priority.IsRegular] = True", objTest.MatrixName.MatrixName, objTest.TestName));
                                                        if (testPriceIsRegular != null && testPriceIsRegular.SurchargePrice != null)
                                                        {
                                                            newObj.UnitPrice = (decimal)testPriceIsRegular.SurchargePrice;
                                                        }
                                                        newObj.TierPrice = (decimal)testPriceSurcharges.SurchargePrice;
                                                        decimal discount = newObj.Discount;
                                                        uint qty = newObj.Qty;
                                                        decimal totalprice = newObj.TierPrice;
                                                        decimal totalamount;
                                                        if (discount > 0)
                                                        {
                                                            var discutamt = totalprice * (discount / 100);
                                                            totalamount = qty * (totalprice - discutamt);
                                                        }
                                                        else if (discount < 0)
                                                        {
                                                            var discoamt = totalprice * (discount / 100);
                                                            //totalamount=qty * (totalprice - discutamt) ;
                                                            totalamount = ((totalprice) - (discoamt)) * qty;
                                                        }
                                                        else
                                                        {
                                                            totalamount = qty * totalprice;
                                                        }
                                                        newObj.Amount = totalamount;
                                                    }
                                                    //newObj.ChargeType = testPriceSurcharges.ChargeType;
                                                    //newObj.Prep1Price = objDefaultPricing.Prep1Charge;
                                                    //newObj.Prep2Price = objDefaultPricing.Prep2Charge;
                                                    //newObj.PriceCode = objDefaultPricing.PriceCode;
                                                    newObj.Parameter = "Allparam";
                                                }
                                                else
                                                {
                                                    msg = newObj.Test.TestName + " Test Not Set DefaultPrice! Do You Want to Continue ?";
                                                    WebWindow.CurrentRequestWindow.RegisterClientScript("PriceNotSet", string.Format(CultureInfo.InvariantCulture, @"var cancelconfirm = confirm('" + msg + "'); {0}", NotSetPrice.CallbackManager.GetScript("PriceNotSet", "cancelconfirm")));
                                                }
                                            }
                                            ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.Add(newObj);


                                        }
                                    }
                                }
                                //List<Guid> lstTestmethodOid = lstSampleParams.Where(i => i.IsGroup == true && i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().ToList();

                            }
                        }

                        if (liInvoicingItemCharges != null)
                        {
                            if (liInvoicingItemCharges.ListView == null)
                            {
                                liInvoicingItemCharges.CreateControl();
                                foreach (Samplecheckin obj in lstSampleCheckins.ToList())
                                {
                                    foreach (SampleCheckinItemChargePricing objAnalysisCahrge in obj.SCItemCharges.ToList())
                                    {
                                        //ItemChargePricing objnew = new ItemChargePricing(uow);
                                        InvoicingItemCharge objnew = Application.MainWindow.View.ObjectSpace.CreateObject<InvoicingItemCharge>();
                                        objnew.Invoicing = objInVoice;
                                        //objnew.ItemName = objAnalysisCahrge.ItemName;
                                        objnew.Qty = objAnalysisCahrge.Qty;
                                        //if (objAnalysisCahrge.Category != null)
                                        //{
                                        //    objnew.Category = Application.MainWindow.View.ObjectSpace.GetObjectByKey<ItemChargePricingCategory>(objAnalysisCahrge.Category.Oid);
                                        //}
                                        objnew.FinalAmount = objAnalysisCahrge.FinalAmount;
                                        objnew.UnitPrice = objAnalysisCahrge.NpUnitPrice;
                                        objnew.Discount = objAnalysisCahrge.Discount;
                                        objnew.ItemPrice = Application.MainWindow.View.ObjectSpace.GetObjectByKey<ItemChargePricing>(objAnalysisCahrge.ItemPrice.Oid);
                                        //objnew.Description = objAnalysisCahrge.Description;
                                        objnew.Amount = objAnalysisCahrge.FinalAmount;
                                        objInVoice.ItemCharges.Add(objnew);
                                        objnew.Remark = objAnalysisCahrge.Remark;
                                        objnew.Description = objAnalysisCahrge.Description;
                                        objnew.NpUnitPrice = objAnalysisCahrge.NpUnitPrice;
                                        //if (objAnalysisCahrge.Units != null)
                                        //{
                                        //    objnew.Units = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Unit>(objAnalysisCahrge.Units.Oid);
                                        //}
                                        ((ListView)liInvoicingItemCharges.ListView).CollectionSource.Add(objnew);

                                    }
                                        ((ListView)liInvoicingItemCharges.ListView).Refresh();
                                }
                            }

                        }
                    ((ListView)liInvoicingAnalysisCharges.InnerView).Refresh();
                    }
                }
                ////else if (View.Id == "Invoicing_ListView")
                ////{
                ////    ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ////    if (listEditor != null)
                ////    {
                ////        ASPxGridView gridView = sender as ASPxGridView;
                ////        if (gridView != null)
                ////        {
                ////            if (gridView.VisibleColumns["SelectionCommandColumn"] != null)
                ////            {
                ////                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                ////            }
                ////            if (gridView.VisibleColumns["Edit"] != null)
                ////            {
                ////                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                ////            }
                ////            if (gridView.VisibleColumns["InvoicePreview"] != null)
                ////            {
                ////                gridView.VisibleColumns["InvoicePreview"].FixedStyle = GridViewColumnFixedStyle.Left;
                ////            }
                ////            if (gridView.VisibleColumns["InvoiceID"] != null)
                ////            {
                ////                gridView.VisibleColumns["InvoiceID"].FixedStyle = GridViewColumnFixedStyle.Left;
                ////            }
                ////            if (gridView.VisibleColumns["JobID"] != null)
                ////            {
                ////                gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                ////            }
                ////            if (gridView.VisibleColumns["Client"] != null)
                ////            {
                ////                gridView.VisibleColumns["Client"].FixedStyle = GridViewColumnFixedStyle.Left;
                ////            }
                ////        }
                ////    }
                ////}
                else if (View.Id == "Invoicing_ItemCharges_ListView")
                {
                    Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                    ListPropertyEditor liInvoicingItemCharges = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                    List<Samplecheckin> lstSampleCheckins = Application.MainWindow.View.ObjectSpace.GetObjects<Samplecheckin>(new InOperator("JobID", objInVoice.JobID.Split(';').ToList().Select(i => i = i.Trim()).ToList())).ToList();

                    if (liInvoicingItemCharges != null && ((ListView)liInvoicingItemCharges.ListView).CollectionSource.GetCount() == 0 && lstSampleCheckins.Count > 0)
                    {
                        foreach (Samplecheckin obj in lstSampleCheckins.ToList())
                        {
                            foreach (SampleCheckinItemChargePricing objAnalysisCahrge in obj.SCItemCharges.ToList())
                            {
                                //ItemChargePricing objnew = new ItemChargePricing(uow);
                                InvoicingItemCharge objnew = Application.MainWindow.View.ObjectSpace.CreateObject<InvoicingItemCharge>();
                                objnew.Invoicing = objInVoice;
                                //objnew.ItemPrice = objAnalysisCahrge.ItemPrice.ItemName;
                                objnew.Qty = objAnalysisCahrge.Qty;
                                //if (objAnalysisCahrge.Category != null)
                                //{
                                //    objnew.Category = Application.MainWindow.View.ObjectSpace.GetObjectByKey<ItemChargePricingCategory>(objAnalysisCahrge.Category.Oid);
                                //}
                                objnew.FinalAmount = objAnalysisCahrge.FinalAmount;
                                objnew.UnitPrice = objAnalysisCahrge.NpUnitPrice;
                                objnew.Discount = objAnalysisCahrge.Discount;
                                objnew.ItemPrice = Application.MainWindow.View.ObjectSpace.GetObjectByKey<ItemChargePricing>(objAnalysisCahrge.ItemPrice.Oid);
                                //objnew.ItemCode = objAnalysisCahrge.ItemPrice.ItemCode;
                                //objnew.Description = objAnalysisCahrge.Description;
                                objnew.Amount = objAnalysisCahrge.Amount;
                                objInVoice.ItemCharges.Add(objnew);
                                objnew.Remark = objAnalysisCahrge.Remark;
                                objnew.Description = objAnalysisCahrge.Description;
                                objnew.NpUnitPrice = objAnalysisCahrge.NpUnitPrice;
                                //if (objAnalysisCahrge.uni != null)
                                //{
                                //    objnew.Units = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Unit>(objAnalysisCahrge.Units.Oid);
                                //}
                                ((ListView)liInvoicingItemCharges.ListView).CollectionSource.Add(objnew);

                            }
                         ((ListView)liInvoicingItemCharges.ListView).Refresh();
                        }
                    }
                }
                //ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //if (gridListEditor != null && gridListEditor.Grid != null)
                //{
                //    if (HttpContext.Current.Session["JobID"] != null)
                //    {
                //        string[] assignedto = HttpContext.Current.Session["JobID"].ToString().Split(new string[] { ", " }, StringSplitOptions.None);
                //        foreach (string val in assignedto)
                //        {
                //            Samplecheckin employee = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", val));
                //            if (employee != null)
                //            {
                //                gridListEditor.Grid.Selection.SelectRowByKey(employee.Oid);
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
        private void Submit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    if (View is ListView)
                    {
                        foreach (Invoicing objInvoice in View.SelectedObjects)
                        {
                            objInvoice.Status = InviceStatus.PendingReview;
                            objInvoice.Submittedby = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objInvoice.SubmittedDate = DateTime.Now;
                        }
                        View.ObjectSpace.CommitChanges();
                        ((ListView)View).CollectionSource.ObjectSpace.Refresh();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Invoicing objInvoice = (Invoicing)View.CurrentObject;
                        if (objInvoice != null)
                        {
                            objInvoice.Status = InviceStatus.PendingReview;
                            objInvoice.Submittedby = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objInvoice.SubmittedDate = DateTime.Now;
                            View.ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(objspace, typeof(Invoicing));
                            ListView createListview = Application.CreateListView("Invoicing_ListView", cs, true);
                            Frame.SetView(createListview);

                        }
                    }
                    //ResetNavigationCount();
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
        private void History_InvoiceView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Invoicing));
                if (View.Id == "Invoicing_ListView_Delivery")
                {
                    ListView createListview = Application.CreateListView("Invoicing_ListView_Delivery_History", cs, true);
                    Frame.SetView(createListview);
                }
                else
                {
                    ListView createListview = Application.CreateListView("Invoicing_ListView_View_History", cs, true);
                    Frame.SetView(createListview);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void InvoiceViewDateFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && InvoiceViewDateFilter != null && InvoiceViewDateFilter.SelectedItem != null)
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (View.Id == "Invoicing_ListView_View_History" || View.Id == "Invoicing_ListView_Review_History" || View.Id == "Invoicing_ListView_Delivery_History")
                    {
                        if (strSelectedItem == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced, Now()) <= 1 And [DateInvoiced] Is Not Null");
                        }
                        else if (strSelectedItem == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced, Now()) <= 3 And [DateInvoiced] Is Not Null");
                        }
                        else if (strSelectedItem == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced, Now()) <= 6 And [DateInvoiced] Is Not Null");
                        }
                        else if (strSelectedItem == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(DateInvoiced, Now()) <= 1 And [DateInvoiced] Is Not Null");
                        }
                        else if (strSelectedItem == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(DateInvoiced, Now()) <= 2 And [DateInvoiced] Is Not Null");
                        }
                        else if (strSelectedItem == "5Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(DateInvoiced, Now()) <= 5 And [DateInvoiced] Is Not Null");
                        }
                        else if (strSelectedItem == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria.Remove("DateFilter");
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
        private void Review_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    if (View is ListView)
                    {
                        foreach (Invoicing objInvoice in View.SelectedObjects)
                        {
                            objInvoice.Status = InviceStatus.PendingDelivery;
                            objInvoice.ReviewedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objInvoice.DateReviewed = DateTime.Now;
                            List<Samplecheckin> lstSampleCheckins = View.ObjectSpace.GetObjects<Samplecheckin>(new InOperator("JobID", objInvoice.JobID.Split(';').ToList().Select(i => i = i.Trim()).ToList())).ToList();
                            foreach (Samplecheckin objSamplecheckin in lstSampleCheckins.ToList())
                            {
                                IList<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                if (lstSamples1.Count() == lstSamples1.Where(i => i.Status == Samplestatus.Reported).Count() && lstSamples1.Count() == lstSamples1.Where(i => i.InvoiceIsDone == true).Count())
                                {
                                    StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 27"));
                                    if (objStatus != null)
                                    {
                                        objSamplecheckin.Index = objStatus;
                                        objSamplecheckin.Isinvoicesummary = true;
                                    }
                                }
                            }
                        }
                        View.ObjectSpace.CommitChanges();
                        ((ListView)View).CollectionSource.ObjectSpace.Refresh();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reviewsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Invoicing objInvoice = (Invoicing)View.CurrentObject;
                        if (objInvoice != null)
                        {
                            objInvoice.Status = InviceStatus.PendingDelivery;
                            objInvoice.ReviewedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objInvoice.DateReviewed = DateTime.Now;
                            View.ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reviewsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(objspace, typeof(Invoicing));
                            ListView createListview = Application.CreateListView("Invoicing_ListView_Review", cs, true);
                            Frame.SetView(createListview);

                        }
                    }
                    //ResetNavigationCount();
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
        private void PreviewReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null)
                {
                    Invoicing objInvoice = (Invoicing)View.CurrentObject;
                    if (objInvoice != null && !string.IsNullOrEmpty(objInvoice.InvoiceID))
                    {
                        NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                        PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                        objToShow.ReportID = objInvoice.InvoiceID;
                        Company cmp = ObjectSpace.FindObject<Company>(CriteriaOperator.Parse(""));
                        if (cmp != null && cmp.Logo != null)
                        {
                            GlobalReportSourceCode.strLogo = Convert.ToBase64String(cmp.Logo);
                        }
                        if (View.Id == "Invoicing_DetailView_Queue" || View.Id == "Invoicing_ListView_Review")
                        {
                            objToShow.PDFData = AddWaterMarkToReport(objInvoice.Report);
                        }
                        else
                        {
                            string strInvoiceID = objInvoice.InvoiceID;
                            XtraReport xtraReport = new XtraReport();
                            objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            SetConnectionString();
                            DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);

                            ObjReportingInfo.strInvoiceID = "'" + strInvoiceID + "'";
                            xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("Invoicing_Reports", ObjReportingInfo, false);
                            using (MemoryStream ms = new MemoryStream())
                            {
                                xtraReport.ExportToPdf(ms);
                                objToShow.PDFData = ms.ToArray();
                                objInvoice.Report = ms.ToArray();
                                View.ObjectSpace.CommitChanges();
                            }
                        }
                        objToShow.ViewID = View.Id;
                        DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                        CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        //showViewParameters.CreatedView.Caption = "PDFViewer";
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.AcceptAction.Active.SetItemValue("disable", false);
                        dc.CancelAction.Active.SetItemValue("disable", false);
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (objInvoice != null && !string.IsNullOrEmpty(objInvoice.JobID))
                    {
                        View.ObjectSpace.CommitChanges();
                        DashboardViewItem lvAnalysisCharge = ((DetailView)View).FindItem("AnalysisCharge") as DashboardViewItem;
                        if (lvAnalysisCharge != null && lvAnalysisCharge.InnerView != null)
                        {
                            ((ASPxGridListEditor)((ListView)lvAnalysisCharge.InnerView).Editor).Grid.UpdateEdit();

                        }

                        objInvoice = (Invoicing)View.CurrentObject;
                        foreach (InvoicingAnalysisCharge objAnalysisCharge in ((ListView)lvAnalysisCharge.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Where(i => i.IsGroup == false).ToList())
                        {
                            objAnalysisCharge.Invoice = lvAnalysisCharge.InnerView.ObjectSpace.GetObjectByKey<Invoicing>(objInvoice.Oid);
                        }
                        foreach (InvoicingAnalysisCharge objAnalysisCharge in ((ListView)lvAnalysisCharge.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Where(i => i.IsGroup == true).ToList())
                        {
                            objAnalysisCharge.Invoice = lvAnalysisCharge.InnerView.ObjectSpace.GetObjectByKey<Invoicing>(objInvoice.Oid);
                        }

                        lvAnalysisCharge.InnerView.ObjectSpace.CommitChanges();
                        string strInvoiceID = objInvoice.InvoiceID;
                        XtraReport xtraReport = new XtraReport();
                        objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();
                        DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);

                        ObjReportingInfo.strInvoiceID = "'" + strInvoiceID + "'";
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("Invoicing_Reports", ObjReportingInfo, false);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            xtraReport.ExportToPdf(ms);
                            objInvoice.Report = ms.ToArray();
                            NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                            PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                            objToShow.ReportID = objInvoice.InvoiceID;
                            objToShow.PDFData = ms.ToArray();
                            if (View.Id == "Invoicing_DetailView_Queue" || View.Id == "Invoicing_ListView_Review" || View.Id == "Invoicing_ListView_Delivery" || View.Id == "Invoicing_DetailView_View_History")
                            {
                                objToShow.PDFData = AddWaterMarkToReport(objInvoice.Report);
                            }
                            else
                            {
                                objToShow.PDFData = objInvoice.Report;
                            }
                            objToShow.ViewID = View.Id;
                            DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                            CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.AcceptAction.Active.SetItemValue("disable", false);
                            dc.CancelAction.Active.SetItemValue("disable", false);
                            dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            View.ObjectSpace.CommitChanges();
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
        private byte[] AddWaterMarkToReport(byte[] report)
        {
            try
            {
                string WatermarkText;
                Session currentSession = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)(this.ObjectSpace)).Session;
                SelectedData sproclang = currentSession.ExecuteSproc("getCurrentLanguage", "");
                var CurrentLanguage = sproclang.ResultSet[1].Rows[0].Values[0].ToString();
                if (CurrentLanguage == "En")
                {
                    WatermarkText = "UnApproved";
                }
                else
                {
                    WatermarkText = ConfigurationManager.AppSettings["ReportWaterMarkText"];
                }

                if (string.IsNullOrEmpty(WatermarkText))
                {
                    return report;
                }
                else
                {
                    using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                    {
                        string fontName = "Microsoft Yahei";
                        int fontSize = 25;
                        PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                        stringFormat.Alignment = PdfStringAlignment.Center;
                        stringFormat.LineAlignment = PdfStringAlignment.Center;
                        MemoryStream ms = new MemoryStream(report);
                        MemoryStream tempms = new MemoryStream();
                        documentProcessor.LoadDocument(ms);
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
                        return tempms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        public void ProcessAction(string parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    if (objInvoiceInfo.lsttempItemPricing == null)
                    {
                        objInvoiceInfo.lsttempItemPricing = new List<InvoicingItemCharge>();
                    }
                    if (objInvoiceInfo.lsttempItemPricingpopup == null)
                    {
                        objInvoiceInfo.lsttempItemPricingpopup = new List<ItemChargePricing>();
                    }
                    string[] param = parameter.Split('|');
                    if (param[0] == "Parameter")
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null)
                        {
                            HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            string strGuid = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid").ToString();
                            IObjectSpace os = Application.CreateObjectSpace();
                            if (param[0] == "Parameter" && !string.IsNullOrEmpty(strGuid))
                            {
                                InvoicingAnalysisCharge obj = View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("Oid=?", new Guid(strGuid)));

                                //HttpContext.Current.Session["AssignTo"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "AssignTo");
                                if (obj != null)
                                {
                                    List<string> lstJobId = obj.JobID.Split(',').ToList();
                                    IList<SampleParameter> lstSampleParams = new List<SampleParameter>();
                                    if (View.Id == "InvoicingAnalysisCharge_ListView_Queue")
                                    {
                                        lstSampleParams = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.JobID", lstJobId.Select(i => i.Replace(" ", ""))), (CriteriaOperator.Parse("[Status] = 'Reported' And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"))));
                                    }
                                    else
                                    {
                                        lstSampleParams = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.JobID", lstJobId.Select(i => i.Replace(" ", ""))), (CriteriaOperator.Parse("[Status] = 'Reported' And ([InvoiceIsDone] = True)"))));
                                    }
                                    if (lstSampleParams.Count > 0)
                                    {
                                        IList<Guid> lstParamOid = new List<Guid>();
                                        if (!obj.IsGroup)
                                        {
                                            lstParamOid = lstSampleParams.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.Parameter != null && i.Testparameter.TestMethod.MatrixName.MatrixName == obj.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == obj.Test.TestName && i.Testparameter.TestMethod.MethodName.MethodNumber == obj.Method.MethodNumber).Select(i => i.Testparameter.Parameter.Oid).Distinct().ToList();
                                        }
                                        else
                                        {
                                            lstParamOid = lstSampleParams.Where(i => i.IsGroup == true && i.GroupTest != null && i.GroupTest.TestMethod != null && i.GroupTest.TestMethod.MatrixName != null && i.GroupTest.TestMethod.MatrixName.MatrixName == obj.Matrix.MatrixName && i.GroupTest.TestMethod.TestName == obj.Test.TestName).Select(i => i.Testparameter.Parameter.Oid).Distinct().ToList();
                                        }
                                        if (lstParamOid.Count > 0)
                                        {
                                            CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Parameter));
                                            cs.Criteria["Filter"] = new InOperator("Oid", lstParamOid);
                                            ListView lv = Application.CreateListView("Parameter_ListView_Invoice", cs, false);
                                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                                            showViewParameters.Context = TemplateContext.PopupWindow;
                                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                            DialogController dc = Application.CreateController<DialogController>();
                                            dc.SaveOnAccept = false;
                                            dc.CancelAction.Active.SetItemValue("Cancel", false);
                                            dc.AcceptAction.Active.SetItemValue("ok", false);
                                            dc.CloseOnCurrentObjectProcessing = false;
                                            showViewParameters.Controllers.Add(dc);
                                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                        }
                                    }

                                }
                            }
                        }
                    }

                    else if (param[0] == "TAT.Oid" || param[0] == "Priority.Prioritys")
                    {
                        bool IsTATNotaval = false;
                        List<string> lsttatinfo = new List<string>();
                        List<string> lsttatoid = new List<string>();
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null)
                        {
                            HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            string strGuid = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid").ToString();
                            if (param[0] == "TAT.Oid" || param[0] == "Priority.Prioritys" && !string.IsNullOrEmpty(strGuid))
                            {
                                objInvoiceInfo.InvoicePopupTATOid = new Guid(strGuid);
                                InvoicingAnalysisCharge objanalysisprice = View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("Oid=?", new Guid(strGuid)));
                                if (objanalysisprice.PriceCode != null)
                                {
                                    if (objanalysisprice != null && objanalysisprice.Test != null && objanalysisprice.IsGroup == false)
                                    {
                                        objInvoiceInfo.InvoicePopupCrtAnalysispriceOid = objanalysisprice.Oid;
                                        List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ?", objanalysisprice.Matrix.MatrixName, objanalysisprice.Test.TestName, objanalysisprice.Method.MethodNumber, objanalysisprice.Component.Components)).ToList();
                                        if (lsttstpricing != null && lsttstpricing.Count > 0)
                                        {
                                            foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                            {
                                                string[] strTAToidarr = objtstpricesurcharge.TAT.Split(';');
                                                if (strTAToidarr.Length > 1)
                                                {
                                                    foreach (string objoid in strTAToidarr.ToList())
                                                    {
                                                        if (!lsttatinfo.Contains((objoid.Trim())))
                                                        {
                                                            lsttatinfo.Add((objoid.Trim()));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!lsttatinfo.Contains((objtstpricesurcharge.TAT.Trim())))
                                                    {
                                                        lsttatinfo.Add((objtstpricesurcharge.TAT.Trim()));
                                                    }
                                                }
                                            }
                                        }
                                        if (lsttatinfo.Count > 0)
                                        {
                                            IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatinfo));
                                            foreach (TurnAroundTime objturntime in lstturntime.ToList())
                                            {
                                                if (!lsttatoid.Contains(objturntime.TAT))
                                                {
                                                    lsttatoid.Add(objturntime.TAT);
                                                }
                                            }
                                        }
                                    }
                                    else if (objanalysisprice != null && objanalysisprice.Test != null && objanalysisprice.IsGroup == true)
                                    {
                                        List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True'", objanalysisprice.Matrix.MatrixName, objanalysisprice.Test.TestName)).ToList();
                                        if (lsttstpricing != null && lsttstpricing.Count > 0)
                                        {
                                            foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                            {
                                                string[] strTAToidarr = objtstpricesurcharge.TAT.Split(';');
                                                if (strTAToidarr.Length > 1)
                                                {
                                                    foreach (string objoid in strTAToidarr.ToList())
                                                    {
                                                        if (!lsttatinfo.Contains((objoid.Trim())))
                                                        {
                                                            lsttatinfo.Add((objoid.Trim()));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!lsttatinfo.Contains((objtstpricesurcharge.TAT.Trim())))
                                                    {
                                                        lsttatinfo.Add((objtstpricesurcharge.TAT.Trim()));
                                                    }
                                                }
                                            }
                                        }
                                        if (lsttatinfo.Count > 0)
                                        {
                                            IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatinfo));
                                            foreach (TurnAroundTime objturntime in lstturntime.ToList())
                                            {
                                                if (!lsttatoid.Contains(objturntime.TAT))
                                                {
                                                    lsttatoid.Add(objturntime.TAT);
                                                }
                                            }
                                        }
                                    }
                                    IObjectSpace os = Application.CreateObjectSpace(typeof(TestPriceSurcharge));
                                    CollectionSource cs = new CollectionSource(os, typeof(TestPriceSurcharge));
                                    if (lsttatoid.Count > 0)
                                    {
                                        cs.Criteria["filter"] = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ? And[Method.MethodName.MethodNumber] = ? And[Component.Components] = ? ", objanalysisprice.Matrix.MatrixName, objanalysisprice.Test.TestName, objanalysisprice.Method.MethodNumber, objanalysisprice.Component.Components);
                                        //cs.Criteria["filter"] = new InOperator("TAT",lsttatoid);
                                    }
                                    else
                                    {
                                        IsTATNotaval = true;
                                    }
                                    if (!IsTATNotaval)
                                    {
                                        ListView lvtpstat = Application.CreateListView("TestPriceSurcharge_ListView_Invoice", cs, false);
                                        ShowViewParameters showViewParameters = new ShowViewParameters(lvtpstat);
                                        showViewParameters.CreatedView = lvtpstat;
                                        showViewParameters.Context = TemplateContext.PopupWindow;
                                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                        DialogController dc = Application.CreateController<DialogController>();
                                        dc.SaveOnAccept = false;
                                        dc.Accepting += TAT_dc_Accepting;
                                        showViewParameters.Controllers.Add(dc);
                                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("TAT was not available for selected test", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                }
                            }
                        }
                    }
                    //else
                    //if (param[0] == "TAT" && View.Id == "TurnAroundTime_ListView_Invoice")
                    //{
                    //    //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    //    InvoicingAnalysisCharge objanalysisprice = null;
                    //    if (Frame is NestedFrame)
                    //    {
                    //        NestedFrame nestedFrame = (NestedFrame)Frame;
                    //        objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    //    }
                    //    else
                    //    {
                    //        objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    //    }
                    //    if (objanalysisprice != null)
                    //    {
                    //        objInvoiceInfo.InvoicePopupCrtAnalysispriceOid = objanalysisprice.Oid;
                    //    }

                    //    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    //    if (editor != null && editor.Grid != null)
                    //    {
                    //        HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                    //        TurnAroundTime objitemprice = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    //        if (objitemprice != null)
                    //        {
                    //            objInvoiceInfo.InvoicePopupTATOid = objitemprice.Oid;
                    //            editor.Grid.Selection.SelectRowByKey(objitemprice.Oid);
                    //        }
                    //    }
                    //}
                    else if (param[0] == "false" && View.Id != "Invoicing_DetailView_Queue")
                    {
                        Application.MainWindow.View.Close();
                        //IObjectSpace objspace = Application.CreateObjectSpace();
                        //CollectionSource cs = new CollectionSource(objspace, typeof(Samplecheckin));
                        //ListView createListview = Application.CreateListView("Samplecheckin_ListView_Invoice", cs, true);
                        //Frame.SetView(createListview);
                    }
                    else if (param[0] == "false" && View.Id == "Invoicing_DetailView_Queue")
                    {
                        Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                        objInvoiceInfo.InvoicePopupPriorityOid = objInVoice.Priority.Oid;
                        IObjectSpace os = Application.CreateObjectSpace(typeof(Priority));
                        CollectionSource cs = new CollectionSource(os, typeof(Priority));
                        ListView lvtpstat = Application.CreateListView("Priority_ListView_Invoice", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(lvtpstat);
                        showViewParameters.CreatedView = lvtpstat;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += TAT_dc_Accepting;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (param[0] == "true" && View.Id == "Invoicing_DetailView_Queue")
                    {
                        Invoicing objInVoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                        View.ObjectSpace.CommitChanges();
                        DashboardViewItem liInvoicingAnalysisCharges = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                        ListPropertyEditor liInvoicingItemCharges = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;

                        if (liInvoicingAnalysisCharges != null && liInvoicingAnalysisCharges.InnerView != null)
                        {
                            ((ListView)liInvoicingAnalysisCharges.InnerView).ObjectSpace.CommitChanges();
                            ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();
                            List<string> lstJobID = objInVoice.JobID.Split(';').ToList();
                            decimal AnalysisChargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.Amount));
                            decimal AnalysisChargedetailAmount = Convert.ToDecimal(((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().ToList().Sum(i => i.TierPrice * i.Qty));
                            decimal itemCahargeFinalAmount = 0;
                            decimal itemCahargeDetailAmount = 0;
                            if (liInvoicingItemCharges != null && liInvoicingItemCharges.ListView != null)
                            {
                                itemCahargeFinalAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.FinalAmount));
                                itemCahargeDetailAmount = Convert.ToDecimal(((ListView)liInvoicingItemCharges.ListView).CollectionSource.List.Cast<InvoicingItemCharge>().ToList().Sum(i => i.Qty * i.NpUnitPrice));
                            }
                            decimal FinalAmount = AnalysisChargeFinalAmount + itemCahargeFinalAmount;
                            decimal detailAmount = AnalysisChargedetailAmount + itemCahargeDetailAmount;
                            objInVoice.DetailedAmount = detailAmount;
                            objInVoice.Amount = FinalAmount;
                            if (detailAmount > FinalAmount)
                            {
                                objInVoice.DiscountAmount = (detailAmount) - (FinalAmount);
                                objInVoice.Discount = Convert.ToDecimal(100 * ((detailAmount - FinalAmount) / detailAmount));
                            }
                            else if (detailAmount < FinalAmount)
                            {
                                decimal disamt = FinalAmount - detailAmount;
                                if (disamt != 0)
                                {
                                    objInVoice.DiscountAmount = disamt;
                                    objInVoice.Discount = Convert.ToDecimal(((disamt) / detailAmount) * 100);
                                }
                            }
                            else if (detailAmount == FinalAmount)
                            {
                                objInVoice.DiscountAmount = 0;
                                objInVoice.Discount = 0;
                            }
                            objInVoice.Status = InviceStatus.PendingReview;
                            foreach (InvoicingAnalysisCharge objAnalysisCharge in ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Where(i => i.IsGroup == false).ToList())
                            {

                                if (objAnalysisCharge.Matrix != null && objAnalysisCharge.Test != null && objAnalysisCharge.Method != null)
                                {
                                    IList<SampleParameter> lstSampleParams = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Samplelogin.JobID.JobID", lstJobID.Select(i => i.Replace(" ", ""))));
                                    if (lstSampleParams.Count > 0)
                                    {
                                        List<SampleParameter> lstSamples = lstSampleParams.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName == objAnalysisCharge.Matrix.MatrixName && i.Testparameter.TestMethod.TestName == objAnalysisCharge.Testparameter.TestMethod.TestName && i.Testparameter.TestMethod.MethodName.MethodNumber == objAnalysisCharge.Method.MethodNumber && i.InvoicingAnalysisCharge == null && (i.InvoiceIsDone == false || i.InvoiceIsDone == null)).ToList();
                                        lstSamples.ForEach(i => { i.InvoiceIsDone = true; i.InvoicingAnalysisCharge = objAnalysisCharge; });
                                    }
                                }
                                objAnalysisCharge.Invoice = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjectByKey<Invoicing>(objInVoice.Oid);
                            }
                            foreach (InvoicingAnalysisCharge objAnalysisCharge in ((ListView)liInvoicingAnalysisCharges.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Where(i => i.IsGroup == true).ToList())
                            {

                                if (objAnalysisCharge.Matrix != null && objAnalysisCharge.Test != null)
                                {
                                    IList<SampleParameter> lstSampleParams = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Samplelogin.JobID.JobID", lstJobID.Select(i => i.Replace(" ", ""))));
                                    if (lstSampleParams.Count > 0)
                                    {
                                        List<SampleParameter> lstSamples = lstSampleParams.Where(i => i.GroupTest != null && i.GroupTest.TestMethod != null && i.Testparameter.TestMethod.MatrixName.MatrixName == objAnalysisCharge.Matrix.MatrixName && i.GroupTest.TestMethod.TestName == objAnalysisCharge.Test.TestName && i.InvoicingAnalysisCharge == null && i.IsGroup == true && (i.InvoiceIsDone == false || i.InvoiceIsDone == null)).ToList();
                                        lstSamples.ForEach(i => { i.InvoiceIsDone = true; i.InvoicingAnalysisCharge = objAnalysisCharge; });
                                    }
                                }
                                objAnalysisCharge.Invoice = liInvoicingAnalysisCharges.InnerView.ObjectSpace.GetObjectByKey<Invoicing>(objInVoice.Oid);
                            }
                            liInvoicingAnalysisCharges.InnerView.ObjectSpace.CommitChanges();
                            ((ASPxGridListEditor)((ListView)liInvoicingAnalysisCharges.InnerView).Editor).Grid.UpdateEdit();

                        }
                        View.ObjectSpace.CommitChanges();
                        if (objInVoice != null)
                        {
                            string strInvoiceID = objInVoice.InvoiceID;
                            XtraReport xtraReport = new XtraReport();

                            objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            SetConnectionString();

                            DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);

                            ObjReportingInfo.strInvoiceID = "'" + strInvoiceID + "'";
                            xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("Invoicing_Reports", ObjReportingInfo, false);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                xtraReport.ExportToPdf(ms);
                                objInVoice.Report = ms.ToArray();
                                View.ObjectSpace.CommitChanges();
                            }

                            //DashboardViewItem liInvoicingAnalysisCharges = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;

                            Invoicing objInvoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                            if (liInvoicingAnalysisCharges != null && liInvoicingAnalysisCharges.InnerView != null)
                            {
                                ASPxGridListEditor gridlisteditor = ((ListView)liInvoicingAnalysisCharges.InnerView).Editor as ASPxGridListEditor;
                                if (gridlisteditor != null && gridlisteditor.Grid != null)
                                {
                                    Session currentSession = ((XPObjectSpace)liInvoicingAnalysisCharges.InnerView.ObjectSpace).Session;
                                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                    InvoicingEDDExport depositEDDExport = uow.FindObject<InvoicingEDDExport>(CriteriaOperator.Parse("[InvoiceID]=?", objInvoice.Oid));
                                    if (depositEDDExport == null)
                                    {
                                        MemoryStream ms = new MemoryStream();
                                        ASPxGridView gridView = gridlisteditor.Grid;
                                        //gridView.TotalSummary.Clear();
                                        gridView.ExportToCsv(ms);
                                        InvoicingEDDExport newDepEDDExp = new InvoicingEDDExport(uow);
                                        newDepEDDExp.InvoiceID = uow.GetObjectByKey<Invoicing>(objInvoice.Oid);
                                        newDepEDDExp.EDDDetail = ms.ToArray();
                                        newDepEDDExp.Save();

                                    }
                                    else
                                    {
                                        MemoryStream ms = new MemoryStream();
                                        ASPxGridView gridView = gridlisteditor.Grid;
                                        //gridView.TotalSummary.Clear();
                                        gridView.ExportToCsv(ms);
                                        depositEDDExport.EDDDetail = ms.ToArray();
                                        depositEDDExport.Save();
                                    }
                                    uow.CommitChanges();
                                }
                            }
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        Application.MainWindow.View.Close();
                    }
                    else if (param[0] == "TATValuesChanged" && param.Count() == 3)
                    {
                        InvoicingAnalysisCharge objAnaCha = View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("Oid=?", new Guid(param[1])));
                        TurnAroundTime objTAT = View.ObjectSpace.GetObjectByKey<TurnAroundTime>(new Guid(param[2]));
                        Invoicing objInvoice = (Invoicing)Application.MainWindow.View.CurrentObject;
                        if (objAnaCha != null && objTAT != null && objAnaCha.IsGroup == false)
                        {
                            bool isQuotes = false;
                            bool isConstutientPrice = false;
                            if (objInvoice != null && objInvoice.QuoteID != null)
                            {
                                AnalysisPricing objAnaprice = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ? And [CRMQuotes]=? And [TAT]=?", objAnaCha.Matrix.MatrixName, objAnaCha.Method.MethodNumber, objAnaCha.Test.TestName, objAnaCha.Component.Components, objInvoice.QuoteID.Oid, objTAT.Oid));
                                if (objAnaprice != null)
                                {
                                    objAnaCha.UnitPrice = objAnaprice.TotalTierPrice;
                                    //newObj.TierPrice = objAnaprice.UnitPrice;
                                    objAnaCha.ChargeType = objAnaprice.ChargeType;
                                    objAnaCha.Prep1Price = objAnaprice.Prep1Charge;
                                    objAnaCha.Prep2Price = objAnaprice.Prep2Charge;
                                    //newObj.PriceCode = objAnaprice.PriceCode;
                                    objAnaCha.Parameter = objAnaprice.Parameter;
                                    objAnaCha.Discount = Convert.ToInt32(objAnaprice.Discount);
                                    objAnaCha.TierNo = objAnaprice.TierNo;
                                    objAnaCha.From = objAnaprice.From;
                                    objAnaCha.To = objAnaprice.To;
                                    objAnaCha.TierPrice = objAnaprice.UnitPrice;
                                    objAnaCha.Amount = objAnaCha.Qty * objAnaprice.FinalAmount;
                                    objAnaCha.TAT = objTAT;
                                    isQuotes = true;

                                }
                            }
                            if (isQuotes == false && isConstutientPrice == false)
                            {
                                TestPriceSurcharge testPriceSurcharges = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And[Component.Components] = ? And Contains([TAT], ?)", objAnaCha.Matrix.MatrixName, objAnaCha.Method.MethodNumber, objAnaCha.Test.TestName, objAnaCha.Component.Components, objTAT.TAT.Trim()));
                                if (testPriceSurcharges != null)
                                {
                                    objAnaCha.PriceCode = testPriceSurcharges.PriceCode;
                                    objAnaCha.Priority = View.ObjectSpace.GetObjectByKey<Priority>(testPriceSurcharges.Priority.Oid);
                                    if (testPriceSurcharges.SurchargePrice != null)
                                    {
                                        TestPriceSurcharge testPriceIsRegular = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And[Component.Components] = ? And [Priority.IsRegular] = True", objAnaCha.Matrix.MatrixName, objAnaCha.Method.MethodNumber, objAnaCha.Test.TestName, objAnaCha.Component.Components));
                                        if (testPriceIsRegular != null)
                                        {
                                            objAnaCha.UnitPrice = (decimal)testPriceIsRegular.SurchargePrice;
                                        }
                                        objAnaCha.TierPrice = (decimal)testPriceSurcharges.SurchargePrice;
                                        objAnaCha.Discount = 0;
                                        objAnaCha.TAT = objTAT;
                                        objAnaCha.Amount = objAnaCha.TierPrice * objAnaCha.Qty;
                                    }

                                }
                            }
                            ((ListView)View).Refresh();
                        }
                        else if (objAnaCha != null && objTAT != null && objAnaCha.IsGroup == true)
                        {
                            bool isQuotes = false;
                            bool isConstutientPrice = false;
                            if (objInvoice != null && objInvoice.QuoteID != null)
                            {
                                AnalysisPricing objAnaprice = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[IsGroup] = True And  [Matrix.MatrixName] = ?  And [Test.TestName] = ? And [CRMQuotes]=? And [TAT]=?", objAnaCha.Matrix.MatrixName, objAnaCha.Test.TestName, objInvoice.QuoteID.Oid, objTAT.Oid));
                                //AnalysisPricing objAnaprice = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodNumber] = ? And [Test.TestName] = ? And [Component.Components] = ? And [CRMQuotes]=? And [TAT]=?", objAnaCha.Matrix.MatrixName, objAnaCha.Method.MethodNumber, objAnaCha.Test.TestName, objAnaCha.Component.Components, objInvoice.QuoteID.Oid, objTAT.Oid));
                                if (objAnaprice != null)
                                {
                                    objAnaCha.UnitPrice = objAnaprice.TotalTierPrice;
                                    //newObj.TierPrice = objAnaprice.UnitPrice;
                                    objAnaCha.ChargeType = objAnaprice.ChargeType;
                                    objAnaCha.Prep1Price = objAnaprice.Prep1Charge;
                                    objAnaCha.Prep2Price = objAnaprice.Prep2Charge;
                                    //newObj.PriceCode = objAnaprice.PriceCode;
                                    objAnaCha.Parameter = objAnaprice.Parameter;
                                    objAnaCha.Discount = Convert.ToInt32(objAnaprice.Discount);
                                    objAnaCha.TierNo = objAnaprice.TierNo;
                                    objAnaCha.From = objAnaprice.From;
                                    objAnaCha.To = objAnaprice.To;
                                    objAnaCha.TierPrice = objAnaprice.UnitPrice;
                                    objAnaCha.Amount = objAnaCha.Qty * objAnaprice.FinalAmount;
                                    objAnaCha.TAT = objTAT;
                                    isQuotes = true;

                                }
                            }
                            if (isQuotes == false && isConstutientPrice == false)
                            {
                                TestPriceSurcharge testPriceSurcharges = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[IsGroup] = True And [Matrix.MatrixName] = ? And [Test.TestName] = ? And Contains([TAT], ?)", objAnaCha.Matrix.MatrixName, objAnaCha.Test.TestName, objTAT.TAT.Trim()));
                                //TestPriceSurcharge testPriceSurcharges = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Method.MethodName.MethodNumber] = ? And [Test.TestName] = ? And[Component.Components] = ? And Contains([TAT], ?)", objAnaCha.Matrix.MatrixName, objAnaCha.Method.MethodNumber, objAnaCha.Test.TestName, objAnaCha.Component.Components, objTAT.TAT.Trim()));
                                if (testPriceSurcharges != null)
                                {
                                    objAnaCha.PriceCode = testPriceSurcharges.PriceCode;
                                    objAnaCha.Priority = View.ObjectSpace.GetObjectByKey<Priority>(testPriceSurcharges.Priority.Oid);
                                    if (testPriceSurcharges.SurchargePrice != null)
                                    {
                                        TestPriceSurcharge testPriceIsRegular = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[IsGroup] = True And [Matrix.MatrixName] = ? And  [Test.TestName] = ? And [Priority.IsRegular] = True", objAnaCha.Matrix.MatrixName, objAnaCha.Test.TestName));
                                        if (testPriceIsRegular != null)
                                        {
                                            objAnaCha.UnitPrice = (decimal)testPriceIsRegular.SurchargePrice;
                                        }
                                        objAnaCha.TierPrice = (decimal)testPriceSurcharges.SurchargePrice;
                                        objAnaCha.Discount = 0;
                                        objAnaCha.TAT = objTAT;
                                        objAnaCha.Amount = objAnaCha.TierPrice * objAnaCha.Qty;
                                    }

                                }
                            }
                            ((ListView)View).Refresh();
                        }
                    }
                    else if (param[0] == "Invoicing_ItemCharges_ListView")
                    {
                        ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                        {
                            if (param[1] == "ValuesChange")
                            {
                                Guid rowoid = new Guid(param[2]);
                                if (rowoid != Guid.Empty)
                                {
                                    decimal surchargevalue = 0;
                                    decimal schargeamt = 0;
                                    decimal pcdisamt = 0;
                                    InvoicingItemCharge objitemcharge = null;
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objitemcharge = nestedFrame.View.ObjectSpace.FindObject<InvoicingItemCharge>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                    }
                                    else
                                    {
                                        objitemcharge = Application.MainWindow.View.ObjectSpace.FindObject<InvoicingItemCharge>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                    }
                                    //QuotesItemChargePrice objitemcharge = Application.MainWindow.View.ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                    if (objitemcharge != null)
                                    {
                                        if (param[4] == "FinalAmount")
                                        {
                                            objitemcharge.Amount = objitemcharge.NpUnitPrice * objitemcharge.Qty;
                                            objitemcharge.Discount = ((objitemcharge.Amount - objitemcharge.FinalAmount) / objitemcharge.Amount) * 100;
                                            objitemcharge.Discount = Math.Round(objitemcharge.Discount);
                                        }
                                        else
                                        {
                                            objitemcharge.Amount = objitemcharge.NpUnitPrice * objitemcharge.Qty;
                                            objitemcharge.FinalAmount = objitemcharge.UnitPrice * objitemcharge.Qty;
                                            objitemcharge.Discount = ((objitemcharge.NpUnitPrice - objitemcharge.UnitPrice) / objitemcharge.NpUnitPrice) * 100;
                                        }
                                    }
                                    ((ListView)View).Refresh();
                                    decimal finalamt = 0;
                                    decimal totalprice = 0;
                                    decimal disamt = 0;
                                    decimal dispr = 0;

                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        DashboardViewItem lstAnalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisCharge") as DashboardViewItem;
                                        if (lstAnalysisprice != null)
                                        {
                                            if (lstAnalysisprice.InnerView == null)
                                            {
                                                lstAnalysisprice.CreateControl();
                                            }
                                            if (((ListView)lstAnalysisprice.InnerView).CollectionSource.GetCount() > 0)
                                            {
                                                finalamt = finalamt + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.Amount);
                                                totalprice = totalprice + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.TierPrice * i.Qty);
                                            }
                                        }
                                        ListPropertyEditor lvitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("ItemCharges") as ListPropertyEditor;
                                        if (lvitemprice != null && lvitemprice.ListView != null)
                                        {
                                            foreach (InvoicingItemCharge objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                            {
                                                finalamt = finalamt + objitempricing.FinalAmount;
                                                totalprice = totalprice + (objitempricing.Amount/* * objitempricing.Qty*/);
                                            }
                                        }
                                        Invoicing crtquotes = (Invoicing)nestedFrame.ViewItem.View.CurrentObject;
                                        if (crtquotes != null)
                                        {
                                            if (finalamt != 0 && totalprice != 0)
                                            {
                                                //disamt = finalamt - totalprice;
                                                disamt = totalprice - finalamt;
                                                if (disamt != 0)
                                                {
                                                    dispr = ((disamt) / totalprice) * 100;
                                                }
                                                //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                                //disamt = finalamt - totalprice;
                                                objInvoiceInfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                                crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                                crtquotes.Amount = Math.Round(finalamt, 2);
                                                //crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                                crtquotes.Discount = (int)Math.Round(dispr, 2);
                                                crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                            }
                                            else if (finalamt == 0 && totalprice == 0)
                                            {
                                                crtquotes.DetailedAmount = 0;
                                                crtquotes.Amount = 0;
                                                crtquotes.Discount = 0;
                                                crtquotes.DiscountAmount = 0;
                                                //crtquotes.QuotedAmount = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DashboardViewItem lstAnalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;
                                        if (lstAnalysisprice != null)
                                        {
                                            if (lstAnalysisprice.InnerView == null)
                                            {
                                                lstAnalysisprice.CreateControl();
                                            }
                                            if (((ListView)lstAnalysisprice.InnerView).CollectionSource.GetCount() > 0)
                                            {
                                                finalamt = finalamt + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.Amount);
                                                totalprice = totalprice + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.TierPrice * i.Qty);
                                            }
                                        }
                                        ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                                        if (lvitemprice != null && lvitemprice.ListView != null)
                                        {
                                            foreach (InvoicingItemCharge objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                            {
                                                finalamt = finalamt + objitempricing.FinalAmount;
                                                totalprice = totalprice + (objitempricing.Amount/* * objitempricing.Qty*/);
                                            }
                                        }
                                        Invoicing crtquotes = (Invoicing)Application.MainWindow.View.CurrentObject;
                                        if (crtquotes != null)
                                        {
                                            if (finalamt != 0 && totalprice != 0)
                                            {
                                                //disamt = finalamt - totalprice;
                                                disamt = totalprice - finalamt;
                                                if (disamt != 0)
                                                {
                                                    dispr = ((disamt) / totalprice) * 100;
                                                }
                                                //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                                //disamt = finalamt - totalprice;
                                                objInvoiceInfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                                crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                                crtquotes.Amount = Math.Round(finalamt, 2);
                                                //crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                                crtquotes.Discount = (int)Math.Round(dispr, 2);
                                                crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                            }
                                            else if (finalamt == 0 && totalprice == 0)
                                            {
                                                crtquotes.DetailedAmount = 0;
                                                crtquotes.Amount = 0;
                                                crtquotes.Discount = 0;
                                                crtquotes.DiscountAmount = 0;
                                                //crtquotes.QuotedAmount = 0;
                                            }
                                        }

                                    }
                                }
                            }
                            if (param[1] == "Selectall" && objInvoiceInfo.IsItemchargePricingpopupselectall == false)
                            {
                                objInvoiceInfo.IsItemchargePricingpopupselectall = true;
                                foreach (InvoicingItemCharge objitemprice in ((ListView)View).CollectionSource.List)
                                {
                                    if (!objInvoiceInfo.lsttempItemPricing.Contains(objitemprice))
                                    {
                                        objInvoiceInfo.lsttempItemPricing.Add(objitemprice);
                                    }
                                }
                                gridlisteditor.Grid.Selection.SelectAll();
                            }
                            else if (param[1] == "UNSelectall")
                            {
                                objInvoiceInfo.lsttempItemPricing.Clear();
                                gridlisteditor.Grid.Selection.UnselectAll();
                                objInvoiceInfo.IsItemchargePricingpopupselectall = false;
                            }
                            else if (param[1] == "Selected" || param[1] == "UNSelected")
                            {
                                if (!string.IsNullOrEmpty(param[1]))
                                {
                                    if (param[1] == "Selected")
                                    {
                                        InvoicingItemCharge objitemprice = ObjectSpace.FindObject<InvoicingItemCharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[2])));
                                        if (objitemprice != null && !objInvoiceInfo.lsttempItemPricing.Contains(objitemprice))
                                        {
                                            objInvoiceInfo.lsttempItemPricing.Add(objitemprice);
                                            gridlisteditor.Grid.Selection.SelectRowByKey(objitemprice);
                                        }
                                        objInvoiceInfo.IsItemchargePricingpopupselectall = false;
                                    }
                                    else if (param[1] == "UNSelected")
                                    {
                                        InvoicingItemCharge objitemprice = ObjectSpace.FindObject<InvoicingItemCharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[2])));
                                        if (objitemprice != null && objInvoiceInfo.lsttempItemPricing.Contains(objitemprice))
                                        {
                                            objInvoiceInfo.lsttempItemPricing.Remove(objitemprice);
                                            gridlisteditor.Grid.Selection.SelectRowByKey(objitemprice);
                                        }
                                        objInvoiceInfo.IsItemchargePricingpopupselectall = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (param[0] == "QuotesItemPrice")
                    {
                        ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                        {
                            if (View.Id == "ItemChargePricing_ListView_Invoice_Popup" || View.Id == "Invoicing_ItemCharges_ListView")
                            {
                                if (param[1] == "Selectall" && objInvoiceInfo.IsItemchargePricingpopupselectall == false)
                                {
                                    objInvoiceInfo.IsItemchargePricingpopupselectall = true;
                                    foreach (ItemChargePricing objitemprice in ((ListView)View).CollectionSource.List)
                                    {
                                        if (!objInvoiceInfo.lsttempItemPricingpopup.Contains(objitemprice))
                                        {
                                            objInvoiceInfo.lsttempItemPricingpopup.Add(objitemprice);
                                        }
                                    }
                                    gridlisteditor.Grid.Selection.SelectAll();
                                }
                                else if (param[1] == "UNSelectall")
                                {
                                    objInvoiceInfo.IsItemchargePricingpopupselectall = false;
                                    objInvoiceInfo.lsttempItemPricingpopup.Clear();
                                    gridlisteditor.Grid.Selection.UnselectAll();
                                }
                                else if (param[1] == "Selected" || param[1] == "UNSelected")
                                {
                                    if (!string.IsNullOrEmpty(param[1]))
                                    {
                                        if (param[1] == "Selected")
                                        {
                                            objInvoiceInfo.IsItemchargePricingpopupselectall = false;
                                            ItemChargePricing objitemprice = ObjectSpace.FindObject<ItemChargePricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[2])));
                                            if (objitemprice != null && !objInvoiceInfo.lsttempItemPricingpopup.Contains(objitemprice))
                                            {
                                                objInvoiceInfo.lsttempItemPricingpopup.Add(objitemprice);
                                                gridlisteditor.Grid.Selection.SelectRowByKey(objitemprice);
                                            }
                                        }
                                        else if (param[1] == "UNSelected")
                                        {
                                            objInvoiceInfo.IsItemchargePricingpopupselectall = false;
                                            ItemChargePricing objitemprice = ObjectSpace.FindObject<ItemChargePricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[2])));
                                            if (objitemprice != null && objInvoiceInfo.lsttempItemPricingpopup.Contains(objitemprice))
                                            {
                                                objInvoiceInfo.lsttempItemPricingpopup.Remove(objitemprice);
                                                gridlisteditor.Grid.Selection.UnselectRowByKey(objitemprice);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (param[0] == "AmountValuesChanged")
                    {
                        InvoicingAnalysisCharge objAnaCha = View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("Oid=?", new Guid(param[1])));
                        if (objAnaCha != null && param.Count() == 3)
                        {
                            objAnaCha.Amount = Convert.ToDecimal(param[2]);
                            ((ListView)View).Refresh();
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


        private void TAT_dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dc = (DialogController)sender;
                    Guid quotetatoid = objInvoiceInfo.InvoicePopupTATOid;
                    Guid quotecrtanalysisoid = objInvoiceInfo.InvoicePopupCrtAnalysispriceOid;
                    if (dc != null && dc.Window != null && dc.Window.View != null && dc.Window.View.Id == "TestPriceSurcharge_ListView_Invoice" && quotetatoid != Guid.Empty && quotecrtanalysisoid != Guid.Empty)
                    {
                        if (dc.Window.View.SelectedObjects.Count == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage("Select atleast one checkbox.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        if (dc.Window.View.SelectedObjects.Count > 1)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", quotecrtanalysisoid));
                        InvoicingAnalysisCharge objanalysisprice = null;
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Oid] = ?", quotecrtanalysisoid));
                        }
                        else
                        {
                            objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Oid] = ?", quotecrtanalysisoid));
                        }
                        if (objanalysisprice != null)
                        {
                            TestPriceSurcharge objTestPriceSurcharge = dc.Window.View.CurrentObject as TestPriceSurcharge;
                            if (objTestPriceSurcharge != null && objTestPriceSurcharge.Priority != null)
                            {
                                TurnAroundTime objtat = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] =?", objTestPriceSurcharge.TAT));
                                Priority objPriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] =?", objTestPriceSurcharge.Priority.Oid));
                                //TestPriceSurcharge objquotetat = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] =?", quotetatoid));
                                if (objtat != null && objPriority != null)
                                {
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objanalysisprice.TAT = nestedFrame.View.ObjectSpace.GetObject(objtat);
                                        objanalysisprice.Priority = nestedFrame.View.ObjectSpace.GetObject(objPriority);
                                    }
                                    else
                                    {
                                        objanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                                        objanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objPriority);
                                    }
                                    //objanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objquotetat);
                                }
                            }
                            uint intqty = 0;
                            decimal unitamt = 0;
                            decimal discntamt = 0;
                            Guid rowoid = objanalysisprice.Oid;
                            string tatoid = objanalysisprice.TAT.TAT;
                            if (rowoid != Guid.Empty && tatoid != null)
                            {
                                decimal surchargevalue = 0;
                                decimal schargeamt = 0;
                                decimal surchargeprice = 0;
                                decimal pcdisamt = 0;
                                string tat = string.Empty;
                                //AnalysisPricing objap = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                InvoicingAnalysisCharge objap = null;
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    objap = nestedFrame.View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                }
                                else
                                {
                                    objap = Application.MainWindow.View.ObjectSpace.FindObject<InvoicingAnalysisCharge>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                }
                                if (objap.TAT != null && objap.Test != null && objap.Component != null)
                                {
                                    intqty = objap.Qty;
                                    unitamt = objap.UnitPrice;
                                    discntamt = objap.Discount;
                                    string str = tatoid.ToString().Replace(" ", "");
                                    TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components, tatoid.Trim()));
                                    if (objtps != null)
                                    {
                                        objap.PriceCode = objtps.PriceCode;
                                        if (objtps.Surcharge != null)
                                        {
                                            //objap.NPSurcharge = (int)objtps.Surcharge;
                                            surchargevalue = (int)objtps.Surcharge;
                                        }
                                        if (Frame is NestedFrame)
                                        {
                                            NestedFrame nestedFrame = (NestedFrame)Frame;
                                            objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        else
                                        {
                                            objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        if (objtps.SurchargePrice != null)
                                        {
                                            surchargeprice = (decimal)objtps.SurchargePrice;
                                        }
                                    }
                                    else
                                    {
                                        //objap.NPSurcharge = 0;
                                    }
                                }
                                else if (objap.TAT != null && objap.Test != null && objap.IsGroup == true)
                                {
                                    intqty = objap.Qty;
                                    unitamt = objap.UnitPrice;
                                    discntamt = objap.Discount;
                                    TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, tatoid.Trim()));
                                    if (objtps != null)
                                    {
                                        objap.PriceCode = objtps.PriceCode;
                                        if (objtps.Surcharge != null)
                                        {
                                            //objap.NPSurcharge = (int)objtps.Surcharge;
                                        }
                                        if (Frame is NestedFrame)
                                        {
                                            NestedFrame nestedFrame = (NestedFrame)Frame;
                                            objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        else
                                        {
                                            objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        if (objtps.Surcharge != null)
                                        {
                                            surchargevalue = (int)objtps.Surcharge;
                                        }
                                        if (objtps.SurchargePrice != null)
                                        {
                                            surchargeprice = (decimal)objtps.SurchargePrice;
                                        }
                                    }
                                    else
                                    {
                                        //objap.NPSurcharge = 0;
                                    }
                                }
                                if (surchargevalue > 0)
                                {
                                    if (objap.IsGroup == false)
                                    {
                                        ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                        if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                        {
                                            ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                            if (objconpricetier != null)
                                            {
                                                objap.TierNo = objconpricetier.TierNo;
                                                objap.From = objconpricetier.From;
                                                objap.To = objconpricetier.To;
                                                objap.TierPrice = objconpricetier.TierPrice;
                                                if (strfocusedcolumn == "Qty")
                                                {
                                                    unitamt = objconpricetier.TierPrice;
                                                }
                                                schargeamt = unitamt * (surchargevalue / 100);
                                                //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                //unitamt = objconpricetier.TierPrice;
                                                //schargeamt = unitamt * (surchargevalue / 100);
                                                //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                if (discntamt > 0)
                                                {
                                                    pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                    objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else if (discntamt < 0)
                                                {
                                                    pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                    objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else
                                                {
                                                    objap.Amount = (objap.TotalUnitPrice) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                            }
                                            else
                                            {
                                                List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                                foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                                {
                                                    objap.TierNo = objconpricetier.TierNo;
                                                    objap.From = objconpricetier.From;
                                                    objap.To = objconpricetier.To;
                                                    objap.TierPrice = objconpricetier.TierPrice;
                                                    if (strfocusedcolumn == "Qty")
                                                    {
                                                        unitamt = objconpricetier.TierPrice;
                                                    }
                                                    schargeamt = unitamt * (surchargevalue / 100);
                                                    objap.TotalUnitPrice = (unitamt + schargeamt)/* * intqty*/;
                                                    //unitamt = objconpricetier.TierPrice;
                                                    //schargeamt = unitamt * (surchargevalue / 100);
                                                    //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                    if (discntamt > 0)
                                                    {
                                                        pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                        objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else if (discntamt < 0)
                                                    {
                                                        pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                        objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else
                                                    {
                                                        objap.Amount = (objap.TotalUnitPrice) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            schargeamt = surchargeprice;
                                            objap.TotalUnitPrice = schargeamt;
                                            objap.TierPrice = surchargeprice;
                                            unitamt = surchargeprice;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                //objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                //objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.Amount = (objap.TotalUnitPrice) * intqty;
                                                //objap.UnitPrice = unitamt;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        schargeamt = surchargeprice;
                                        objap.TotalUnitPrice = schargeamt;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                            objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                            //objap.UnitPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                            objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                            //objap.UnitPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.Amount = (objap.TotalUnitPrice) * intqty;
                                            //objap.UnitPrice = unitamt;
                                        }
                                    }
                                }
                                else
                                {
                                    if (objap.IsGroup == false)
                                    {
                                        ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                        if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                        {
                                            ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                            if (objconpricetier != null)
                                            {
                                                objap.TierNo = objconpricetier.TierNo;
                                                objap.From = objconpricetier.From;
                                                objap.To = objconpricetier.To;
                                                objap.TierPrice = objconpricetier.TierPrice;
                                                //objap.TotalTierPrice = objconpricetier.TierPrice /** intqty*/;
                                                //unitamt = objconpricetier.TierPrice;
                                                if (strfocusedcolumn == "Qty")
                                                {
                                                    unitamt = objconpricetier.TierPrice;
                                                }
                                                //schargeamt = unitamt * (surchargevalue / 100);
                                                objap.TotalUnitPrice = (unitamt + schargeamt)/* * intqty*/;
                                                if (discntamt > 0)
                                                {
                                                    pcdisamt = (unitamt) * (discntamt / 100);
                                                    objap.Amount = ((unitamt) - (pcdisamt)) * intqty;
                                                    //objap.UnitPrice = unitamt;
                                                }
                                                else if (discntamt < 0)
                                                {
                                                    pcdisamt = (unitamt) * (discntamt / 100);
                                                    objap.Amount = ((unitamt) - (pcdisamt)) * intqty;
                                                    //objap.UnitPrice = unitamt;
                                                }
                                                else
                                                {
                                                    objap.Amount = (unitamt) * intqty;
                                                    //objap.UnitPrice = unitamt;
                                                }
                                            }
                                            else
                                            {
                                                List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                                foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                                {
                                                    objap.TierNo = objconprice.TierNo;
                                                    objap.From = objconprice.From;
                                                    objap.To = objconprice.To;
                                                    objap.TierPrice = objconprice.TierPrice;
                                                    //unitamt = objconpricetier.TierPrice;
                                                    //schargeamt = unitamt * (surchargevalue / 100);
                                                    //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                    if (strfocusedcolumn == "Qty")
                                                    {
                                                        unitamt = objconprice.TierPrice;
                                                    }
                                                    schargeamt = unitamt * (surchargevalue / 100);
                                                    objap.TotalUnitPrice = (unitamt + schargeamt)/* * intqty*/;
                                                    if (discntamt > 0)
                                                    {
                                                        pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                        objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                        //objap.UnitPrice = unitamt;
                                                    }
                                                    else if (discntamt < 0)
                                                    {
                                                        pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                        objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                        //objap.UnitPrice = unitamt;
                                                    }
                                                    else
                                                    {
                                                        objap.Amount = (objap.TotalUnitPrice) * intqty;
                                                        //objap.UnitPrice = unitamt;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            schargeamt = surchargeprice;
                                            unitamt = surchargeprice;
                                            //objap.TotalTierPrice = schargeamt + unitamt;
                                            objap.TotalUnitPrice = schargeamt;
                                            objap.TierPrice = schargeamt;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                //objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                                objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                                //objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.Amount = (objap.TotalUnitPrice) * intqty;
                                                //objap.UnitPrice = unitamt;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        schargeamt = surchargeprice;
                                        objap.TotalUnitPrice = schargeamt;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                            objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                            //objap.UnitPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (objap.TotalUnitPrice) * (discntamt / 100);
                                            objap.Amount = ((objap.TotalUnitPrice) - (pcdisamt)) * intqty;
                                            //objap.UnitPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.Amount = (objap.TotalUnitPrice) * intqty;
                                            //objap.UnitPrice = unitamt;
                                        }
                                    }
                                }
                                TurnAroundTime objtat = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", tatoid));
                                if (objtat != null)
                                {
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objap.TAT = nestedFrame.View.ObjectSpace.GetObject(objtat);
                                    }
                                    else
                                    {
                                        objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                                    }
                                    //objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                                }
                                ((ListView)View).Refresh();
                                decimal finalamt = 0;
                                decimal totalprice = 0;
                                decimal disamt = 0;
                                decimal dispr = 0;
                                DashboardViewItem lstAnalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisCharge") as DashboardViewItem;

                                {//if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                 //{
                                 //    foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                 //    {
                                 //        finalamt = finalamt + objanapricing.FinalAmount;
                                 //        totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                 //    }
                                 //}
                                 //DashboardViewItem dvitemprice = ((DetailView)Application.MainWindow.View).FindItem("DVItemChargePrice") as DashboardViewItem;
                                 //if (dvitemprice != null && dvitemprice.InnerView != null)
                                 //{
                                 //    foreach (QuotesItemChargePrice objitempricing in ((ListView)dvitemprice.InnerView).CollectionSource.List)
                                 //    {
                                 //        finalamt = finalamt + objitempricing.FinalAmount;
                                 //        totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                                 //    }
                                 //}
                                }
                                ListPropertyEditor lstitemprice = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                                //if (lvitemprice != null && lvitemprice.ListView != null)
                                //{
                                //    foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                //    {
                                //        finalamt = finalamt + objitempricing.FinalAmount;
                                //        totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                                //    }
                                //}
                                if (lstitemprice != null)
                                {
                                    if (lstitemprice.ListView == null)
                                    {
                                        lstitemprice.CreateControl();
                                    }
                                    if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.FinalAmount);
                                        totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<InvoicingItemCharge>().Sum(i => i.UnitPrice * i.Qty);
                                    }
                                }
                                if (lstAnalysisprice != null)
                                {
                                    if (lstAnalysisprice.InnerView == null)
                                    {
                                        lstAnalysisprice.CreateControl();
                                    }
                                    if (((ListView)lstAnalysisprice.InnerView).CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.Amount);
                                        totalprice = totalprice + ((ListView)lstAnalysisprice.InnerView).CollectionSource.List.Cast<InvoicingAnalysisCharge>().Sum(i => i.TotalUnitPrice * i.Qty);
                                    }
                                    lstAnalysisprice.InnerView.Refresh();
                                }
                                if (finalamt < 0)
                                {
                                    objap.Discount = 0;
                                    objap.Amount = 0;
                                    finalamt = 0;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                }
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    Invoicing crtquotes = (Invoicing)nestedFrame.ViewItem.View.CurrentObject;
                                    if (crtquotes != null)
                                    {
                                        if (finalamt >= 0 && totalprice >= 0)
                                        {
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            //quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.Amount = Math.Round(finalamt, 2);
                                            //crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Convert.ToInt32(Math.Round(dispr, 2));
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        }
                                        else
                                        {
                                            dispr = ((totalprice) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            //quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.Amount = Math.Round(finalamt, 2);
                                            //crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Convert.ToInt32(Math.Round(dispr, 2));
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        }
                                    }
                                }
                                else
                                {
                                    Invoicing crtquotes = (Invoicing)Application.MainWindow.View.CurrentObject;
                                    if (crtquotes != null)
                                    {
                                        if (finalamt >= 0 && totalprice >= 0)
                                        {
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            //quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.Amount = Math.Round(finalamt, 2);
                                            //crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Convert.ToInt16(Math.Round(dispr, 2));
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        }
                                        else
                                        {
                                            dispr = ((totalprice) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            //quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.Amount = Math.Round(finalamt, 2);
                                            //crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Convert.ToInt32(Math.Round(dispr, 2));
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (dc != null && dc.Window != null && dc.Window.View != null && dc.Window.View.Id == "Priority_ListView_Invoice" && e.AcceptActionArgs.SelectedObjects.Count == 1)
                    {
                        Invoicing objInv = Application.MainWindow.View.CurrentObject as Invoicing;
                        Priority objPriority = ((Priority)e.AcceptActionArgs.CurrentObject);
                        if (objInv != null)
                        {
                            Priority objPrty = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Priority>(objPriority.Oid);
                            objInv.Priority = objPrty;

                        }
                        //objInv.DueDateCompleted = DateTime.Today.AddDays(objInv.TAT.TAT);
                    }
                }
                if (View.Id == "CRMQuotes_AnalysisPricing_ListView" && e.AcceptActionArgs.SelectedObjects.Count == 1)
                {
                    AnalysisPricing objanalysisprice = null;
                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    }
                    else
                    {
                        objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    }
                    //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    if (objanalysisprice != null)
                    {
                        foreach (TurnAroundTime objtat in e.AcceptActionArgs.SelectedObjects)
                        {
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                objanalysisprice.TAT = nestedFrame.View.ObjectSpace.GetObject(objtat);
                            }
                            else
                            {
                                objanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                            }
                            //objanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                        }
                        uint intqty = 0;
                        decimal unitamt = 0;
                        decimal discntamt = 0;
                        Guid rowoid = objanalysisprice.Oid;
                        string tatoid = objanalysisprice.TAT.TAT;
                        if (rowoid != Guid.Empty && tatoid != null)
                        {
                            decimal surchargevalue = 0;
                            decimal schargeamt = 0;
                            decimal pcdisamt = 0;
                            string tat = string.Empty;
                            AnalysisPricing objap = null;
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                objap = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                            }
                            else
                            {
                                objap = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                            }
                            //AnalysisPricing objap = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                            if (objap.TAT != null && objap.Test != null && objap.Component != null)
                            {
                                intqty = objap.Qty;
                                unitamt = objap.UnitPrice;
                                discntamt = objap.Discount;
                                TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components, tatoid.Trim()));
                                if (objtps != null)
                                {
                                    objap.PriceCode = objtps.PriceCode;
                                    if (objtps.Surcharge != null)
                                    {
                                        objap.NPSurcharge = (int)objtps.Surcharge;
                                        surchargevalue = (int)objtps.Surcharge;
                                    }
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                                    }
                                    else
                                    {
                                        objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                    }
                                    //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                }
                                else
                                {
                                    objap.NPSurcharge = 0;
                                }
                            }
                            else if (objap.TAT != null && objap.Test != null && objap.IsGroup == true)
                            {
                                TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, tatoid.Trim()));
                                if (objtps != null)
                                {
                                    objap.PriceCode = objtps.PriceCode;
                                    if (objtps.Surcharge != null)
                                    {
                                        objap.NPSurcharge = (int)objtps.Surcharge;
                                        surchargevalue = (int)objtps.Surcharge;
                                    }
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                                    }
                                    else
                                    {
                                        objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                    }
                                    //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                }
                                else
                                {
                                    objap.NPSurcharge = 0;
                                }
                            }
                            if (surchargevalue > 0)
                            {
                                ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                {
                                    ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                    if (objconpricetier != null)
                                    {
                                        objap.TierNo = objconpricetier.TierNo;
                                        objap.From = objconpricetier.From;
                                        objap.To = objconpricetier.To;
                                        objap.TierPrice = objconpricetier.TierPrice;
                                        //unitamt = objconpricetier.TierPrice;
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                        if (strfocusedcolumn == "Qty")
                                        {
                                            unitamt = objconpricetier.TierPrice;
                                        }
                                        schargeamt = unitamt * (surchargevalue / 100);
                                        //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                        objap.TotalTierPrice = schargeamt;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                    }
                                    else
                                    {
                                        List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                        foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                        {
                                            objap.TierNo = objconprice.TierNo;
                                            objap.From = objconprice.From;
                                            objap.To = objconprice.To;
                                            objap.TierPrice = objconprice.TierPrice;
                                            if (strfocusedcolumn == "Qty")
                                            {
                                                unitamt = objconprice.TierPrice;
                                            }
                                            schargeamt = unitamt * (surchargevalue / 100);
                                            objap.TotalTierPrice = schargeamt;
                                            //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                            //unitamt = objconpricetier.TierPrice;
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    schargeamt = unitamt * (surchargevalue / 100);
                                    objap.TotalTierPrice = schargeamt;
                                    //objap.TotalTierPrice = schargeamt + unitamt;
                                    if (discntamt > 0)
                                    {
                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                    else if (discntamt < 0)
                                    {
                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                    else
                                    {
                                        objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                }
                            }
                            else
                            {
                                ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                {
                                    ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                    if (objconpricetier != null)
                                    {
                                        objap.TierNo = objconpricetier.TierNo;
                                        objap.From = objconpricetier.From;
                                        objap.To = objconpricetier.To;
                                        objap.TierPrice = objconpricetier.TierPrice;
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        //objap.TotalTierPrice = objconpricetier.TierPrice /** intqty*/;
                                        //unitamt = objconpricetier.TierPrice;
                                        if (strfocusedcolumn == "Qty")
                                        {
                                            unitamt = objconpricetier.TierPrice;
                                        }
                                        schargeamt = unitamt * (surchargevalue / 100);
                                        //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                        objap.TotalTierPrice = surchargevalue;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (unitamt) * (discntamt / 100);
                                            objap.FinalAmount = ((unitamt) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (unitamt) * (discntamt / 100);
                                            objap.FinalAmount = ((unitamt) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.FinalAmount = (unitamt) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                    }
                                    else
                                    {
                                        List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                        foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                        {
                                            objap.TierNo = objconprice.TierNo;
                                            objap.From = objconprice.From;
                                            objap.To = objconprice.To;
                                            objap.TierPrice = objconprice.TierPrice;
                                            //unitamt = objconpricetier.TierPrice;
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                            if (strfocusedcolumn == "Qty")
                                            {
                                                unitamt = objconprice.TierPrice;
                                            }
                                            schargeamt = unitamt * (surchargevalue / 100);
                                            objap.TotalTierPrice = schargeamt;
                                            //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    schargeamt = unitamt * (surchargevalue / 100);
                                    objap.TotalTierPrice = schargeamt;
                                    //objap.TotalTierPrice = schargeamt + unitamt;
                                    if (discntamt > 0)
                                    {
                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                    else if (discntamt < 0)
                                    {
                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                    else
                                    {
                                        objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                }
                            }
                            TurnAroundTime objtat = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", tatoid));
                            if (objtat != null)
                            {
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    objap.TAT = nestedFrame.View.ObjectSpace.GetObject(objtat);
                                }
                                else
                                {
                                    objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                                }
                                //objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                            }
                            ((ListView)View).Refresh();
                            decimal finalamt = 0;
                            decimal totalprice = 0;
                            decimal disamt = 0;
                            decimal dispr = 0;
                            ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                            if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                            {
                                foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                {
                                    finalamt = finalamt + objanapricing.FinalAmount;
                                    totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                }
                            }
                            //DashboardViewItem dvitemprice = ((DetailView)Application.MainWindow.View).FindItem("DVItemChargePrice") as DashboardViewItem;
                            //if (dvitemprice != null && dvitemprice.InnerView != null)
                            //{
                            //    foreach (QuotesItemChargePrice objitempricing in ((ListView)dvitemprice.InnerView).CollectionSource.List)
                            //    {
                            //        finalamt = finalamt + objitempricing.FinalAmount;
                            //        totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                            //    }
                            //}
                            ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("ItemCharges") as ListPropertyEditor;
                            if (lvitemprice != null && lvitemprice.ListView != null)
                            {
                                foreach (InvoicingItemCharge objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                {
                                    finalamt = finalamt + objitempricing.FinalAmount;
                                    totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                                }
                            }
                            if (finalamt < 0)
                            {
                                objap.Discount = 0;
                                objap.FinalAmount = 0;
                                finalamt = 0;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            }
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                if (crtquotes != null)
                                {
                                    if (finalamt >= 0 && totalprice >= 0)
                                    {
                                        disamt = totalprice - finalamt;
                                        //disamt = finalamt - totalprice;
                                        if (disamt != 0)
                                        {
                                            dispr = ((disamt) / totalprice) * 100;
                                        }
                                        //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        //quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                    }
                                    else
                                    {
                                        dispr = ((totalprice) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        disamt = totalprice - finalamt;
                                        //quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                    }
                                }
                            }
                            else
                            {
                                CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                                if (crtquotes != null)
                                {
                                    if (finalamt >= 0 && totalprice >= 0)
                                    {
                                        disamt = totalprice - finalamt;
                                        //disamt = finalamt - totalprice;
                                        if (disamt != 0)
                                        {
                                            dispr = ((disamt) / totalprice) * 100;
                                        }
                                        //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        //quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                    }
                                    else
                                    {
                                        dispr = ((totalprice) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        disamt = totalprice - finalamt;
                                        //quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
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

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                //if (/*View.Id == "TurnAroundTime_ListView_Invoice" ||*/ View.Id == "TestPriceSurcharge_ListView_Invoice")
                //{
                //    if (e.DataColumn.FieldName == "TAT")
                //    {
                //        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ParameterPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                //    }
                //    if (e.DataColumn.FieldName == "Prioritys")
                //    {
                //        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ParameterPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                //    }
                //}
                if (View.Id == "InvoicingAnalysisCharge_ListView_Invoice" || View.Id == "InvoicingAnalysisCharge_ListView_Queue")
                {
                    if (e.DataColumn.FieldName == "Parameter")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ParameterPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                    else if (e.DataColumn.FieldName == "TAT.Oid")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ParameterPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                    else if (e.DataColumn.FieldName == "Priority.Prioritys")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ParameterPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                }
                else
                {
                    if (e.DataColumn.FieldName != "Parameter") return;
                    e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ParameterPopupView', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void ResetNavigationCount()
        {
            try
            {
                ShowNavigationItemController ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                ChoiceActionItem parentInvoicing = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "Accounting");
                if (parentInvoicing.Id == "Accounting")
                {
                    ChoiceActionItem Invoicing = parentInvoicing.Items.FirstOrDefault(i => i.Id == "Invoicing");
                    if (Invoicing != null)
                    {
                        ChoiceActionItem childInvoiceQueue = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceQueue" || i.Id == "InvoiceQueue ");
                        if (childInvoiceQueue != null)
                        {
                            int count = 0;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            List<Guid> lstinvoiceOid = new List<Guid>();
                            IList<SampleParameter> samples = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] = 'Reported' And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"));
                            List<Guid> lstScGuid = samples.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && (i.Samplelogin.JobID.ProjectCategory is null || (i.Samplelogin.JobID.ProjectCategory != null && i.Samplelogin.JobID.ProjectCategory.Non_Commercial != CommercialType.Yes))).Select(i => i.Samplelogin.JobID.Oid).Distinct().ToList();
                            foreach (Guid obj in lstScGuid)
                            {
                                IList<SampleParameter> lstsamplesInvoice = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] <> 'Reported' And [Samplelogin.JobID.Oid]=? ", obj));
                                if (lstsamplesInvoice.Count == 0)
                                {
                                    if (!lstinvoiceOid.Contains(obj))
                                    {
                                        lstinvoiceOid.Add(obj);
                                        count = count + 1;
                                    }
                                }
                            }

                            var cap = childInvoiceQueue.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (count > 0)
                            {
                                childInvoiceQueue.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childInvoiceQueue.Caption = cap[0];
                            }
                        }
                        ChoiceActionItem childInvoiceSubmi = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceSubmission");
                        if (childInvoiceSubmi != null)
                        {
                            int count = 0;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            IList<Invoicing> lstCount = objSpace.GetObjects<Invoicing>(CriteriaOperator.Parse("[Status] = 'PendingSubmit'"));
                            var cap = childInvoiceSubmi.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (lstCount.Count > 0)
                            {
                                count = lstCount.Count();
                                childInvoiceSubmi.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childInvoiceSubmi.Caption = cap[0];
                            }
                        }
                        ChoiceActionItem childInvoiceReview = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceReview");
                        if (childInvoiceReview != null)
                        {
                            int count = 0;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            IList<Invoicing> lstCount = objSpace.GetObjects<Invoicing>(CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                            var cap = childInvoiceReview.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (lstCount.Count > 0)
                            {
                                count = lstCount.Count();
                                childInvoiceReview.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childInvoiceReview.Caption = cap[0];
                            }
                        }
                        ChoiceActionItem childInvoiceDelivery = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceDelivery");
                        if (childInvoiceDelivery != null)
                        {
                            int count = 0;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            IList<Invoicing> lstCount = objSpace.GetObjects<Invoicing>(CriteriaOperator.Parse("[Status] = 'PendingDelivery'"));
                            var cap = childInvoiceDelivery.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (lstCount.Count > 0)
                            {
                                count = lstCount.Count();
                                childInvoiceDelivery.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childInvoiceDelivery.Caption = cap[0];
                            }
                        }
                        ChoiceActionItem childInvoiceEDDExport = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceEDDExport");
                        if (childInvoiceEDDExport != null)
                        {
                            int count = 0;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            IList<InvoicingEDDExport> lstCount = objSpace.GetObjects<InvoicingEDDExport>(CriteriaOperator.Parse("Not IsNullOrEmpty([EDDID])"));
                            var cap = childInvoiceEDDExport.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (lstCount.Count > 0)
                            {
                                count = lstCount.Count();
                                childInvoiceEDDExport.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childInvoiceEDDExport.Caption = cap[0];
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
        private void BatchInvoicing_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(Samplecheckin));
                Samplecheckin obj = os.CreateObject<Samplecheckin>();
                DetailView createdView = Application.CreateDetailView(os, "Samplecheckin_DetailView_BatchInvoice", true, obj);
                createdView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
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
        private void PreInvoiceReport_Execute(object sender, SimpleActionExecuteEventArgs e)
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
        private void Rollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)
                {
                    
                    if (View != null && (View.SelectedObjects.Count > 0 || View.CurrentObject != null))
                    {
                        IObjectSpace os = Application.CreateObjectSpace(typeof(Invoicing));
                        Invoicing obj = os.CreateObject<Invoicing>();
                        DetailView createdView = Application.CreateDetailView(os, "Invoicing_DetailView_Rollback", true, obj);
                        createdView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += RollBackReason_Accepting;
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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

        private void RollBackReason_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                Invoicing objCurrentObject = (Invoicing)e.AcceptActionArgs.CurrentObject;

                if (objCurrentObject != null && !string.IsNullOrEmpty(objCurrentObject.RollbackReason) && !string.IsNullOrWhiteSpace(objCurrentObject.RollbackReason))
                {
                    if (View is ListView)
                    {
                        foreach (Invoicing objInvoice in View.SelectedObjects.Cast<Invoicing>().ToList())
                        {
                            //if (View.Id == "Invoicing_ListView_Review_History")
                            //{
                            //    objInvoice.Status = InviceStatus.PendingReview;
                            //}
                            //else
                            //{
                            //    objInvoice.Status = InviceStatus.PendingSubmit;
                            //}
                            objInvoice.Status = InviceStatus.PendingReview;
                            objInvoice.ReviewedBy = null;
                            objInvoice.DateReviewed = null;
                            objInvoice.RollbackReason = objCurrentObject.RollbackReason;
                            objInvoice.RollbackedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objInvoice.RollbackedDate = DateTime.Now;
                            List<Samplecheckin> lstSampleCheckins = View.ObjectSpace.GetObjects<Samplecheckin>(new InOperator("JobID", objInvoice.JobID.Split(';').ToList().Select(i => i = i.Trim()).ToList())).ToList();
                            foreach (Samplecheckin objSamplecheckin in lstSampleCheckins.ToList())
                            {
                                IList<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                if (lstSamples1.Count() == lstSamples1.Where(i => i.Status == Samplestatus.Reported).Count() && lstSamples1.Count() == lstSamples1.Where(i => i.InvoiceIsDone == true).Count()) ;
                                {
                                    StatusDefinition statusDefinition = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=9"));
                                    if (statusDefinition != null)
                                    {
                                        objSamplecheckin.Index = statusDefinition;
                                    }
                                }
                            }
                        }
                    }
                    else if (View is DetailView)
                    {
                        Invoicing objInvoice = (Invoicing)View.CurrentObject;
                        if (objInvoice != null)
                        {
                            objInvoice.Status = InviceStatus.PendingReview;
                            objInvoice.ReviewedBy = null;
                            objInvoice.DateReviewed = null;
                            objInvoice.RollbackReason = objCurrentObject.RollbackReason;
                            objInvoice.RollbackedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objInvoice.RollbackedDate = DateTime.Now;
                            List<Samplecheckin> lstSampleCheckins = View.ObjectSpace.GetObjects<Samplecheckin>(new InOperator("JobID", objInvoice.JobID.Split(';').ToList().Select(i => i = i.Trim()).ToList())).ToList();
                            foreach (Samplecheckin objSamplecheckin in lstSampleCheckins.ToList())
                            {
                                IList<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                if (lstSamples1.Count() == lstSamples1.Where(i => i.Status == Samplestatus.Reported).Count() && lstSamples1.Count() == lstSamples1.Where(i => i.InvoiceIsDone == true).Count()) ;
                                {
                                    StatusDefinition statusDefinition = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=9"));
                                    if (statusDefinition != null)
                                    {
                                        objSamplecheckin.Index = statusDefinition;
                                    }
                                }
                            }
                        }
                    }
                    ObjectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    //ResetNavigationCount();
                    if (View is DetailView)
                    {
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace, typeof(Invoicing));
                        ListView createListView = Application.CreateListView("Invoicing_ListView_Review", cs, true);
                        Frame.SetView(createListView);
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void HistoryReview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Invoicing));
                ListView createListview = Application.CreateListView("Invoicing_ListView_Review_History", cs, true);
                Frame.SetView(createListview);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        //private void EDDDetails_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (View.CurrentObject != null)
        //        {
        //            InvoicingEDDExport objExport = (InvoicingEDDExport)View.CurrentObject;
        //            if (objExport != null)
        //            {
        //                HttpContext.Current.Response.Clear();
        //                HttpContext.Current.Response.ContentType = "application/excel";
        //                HttpContext.Current.Response.AddHeader("Content-disposition", "filename=" + objExport.EDDID + "EDDDetails.csv");
        //                HttpContext.Current.Response.OutputStream.Write(objExport.EDDDetail, 0, objExport.EDDDetail.Length);
        //                HttpContext.Current.Response.OutputStream.Flush();
        //                HttpContext.Current.Response.OutputStream.Close();
        //                HttpContext.Current.Response.Flush();
        //                HttpContext.Current.Response.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}


        private void Export_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            try
            {
                strEDDId = string.Empty;
                strJobID = string.Empty;
                if (View.SelectedObjects.Count > 0)
                {
                    if (View.Id == "InvoicingEDDExport_ListView")
                    {
                        foreach (Modules.BusinessObjects.Setting.Invoicing.InvoicingEDDExport obj in View.SelectedObjects)
                        {

                            if (strEDDId == string.Empty)
                            {
                                strEDDId = obj.EDDID.ToString();
                                strJobID = obj.InvoiceID.JobID.ToString();
                            }
                            else
                            {
                                if (!strEDDId.Contains(obj.EDDID.ToString()))
                                    strEDDId = strEDDId + "," + obj.EDDID.ToString();
                                strJobID = strJobID + "," + obj.InvoiceID.JobID.ToString();
                            }

                            //if (strEDDId == string.Empty)
                            //{
                            //    strEDDId = "'" + obj.EDDID.ToString() + "'";

                            //}
                            //else
                            //{
                            //    if (!strEDDId.Contains(obj.EDDID.ToString()))
                            //        strEDDId = strEDDId + ",'" + obj.EDDID.ToString() + "'";
                            //}
                        }
                    }
                    else if (View.Id == "Samplecheckin_ListView_Copy")
                    {
                        foreach (Modules.BusinessObjects.Setting.Invoicing.InvoicingEDDExport obj in View.SelectedObjects)
                        {
                            if (strEDDId == string.Empty)
                            {
                                strEDDId = "'" + obj.EDDID.ToString() + "'";
                            }
                            else
                            {
                                strEDDId = strEDDId + ",'" + obj.EDDID.ToString() + "'";
                            }
                        }
                    }
                    string strTempPath = Path.GetTempPath();
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                    }

                    string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".csv");
                    //var workbook = new ExcelFile();
                    //var worksheet = workbook.Worksheets.Add("DataTable to Sheet");
                    //DataView dv = new DataView();
                    //DataTable dataTable = new DataTable();
                    //dataTable.Columns.Add("CustomerID");
                    //dataTable.Columns.Add("ClientName");
                    //dataTable.Columns.Add("Invoice/CM #");
                    //dataTable.Columns.Add("CreditMemo");
                    //dataTable.Columns.Add("Date");
                    //dataTable.Columns.Add("Quote");
                    //dataTable.Columns.Add("Quote#");
                    //dataTable.Columns.Add("QuoteGoodThruDate");
                    //dataTable.Columns.Add("DropShip");
                    //dataTable.Columns.Add("ShipToName");
                    //dataTable.Columns.Add("ShipToAddressLineOne");
                    //dataTable.Columns.Add("ShipToAddressLineTwo");
                    //dataTable.Columns.Add("ShipToCity");
                    //dataTable.Columns.Add("ShipToState");
                    //dataTable.Columns.Add("ShipToZipCode"); 
                    //dataTable.Columns.Add("ShipToCountry");
                    //dataTable.Columns.Add("CustomerPO");
                    //dataTable.Columns.Add("ShipVia");
                    //dataTable.Columns.Add("ShipDate");
                    //dataTable.Columns.Add("DateDue");
                    //dataTable.Columns.Add("DiscountAmount");
                    //dataTable.Columns.Add("DiscountDate");
                    //dataTable.Columns.Add("DisplayedTerms");
                    //dataTable.Columns.Add("SalesRepresentativeID");
                    //dataTable.Columns.Add("AccountsReceivableAccount");
                    //dataTable.Columns.Add("AccountsReceivableAmount");
                    //dataTable.Columns.Add("InvoiceNote");
                    //dataTable.Columns.Add("NumberOfDistributions");
                    //dataTable.Columns.Add("Invoice/CMDistribution");
                    //dataTable.Columns.Add("ApplyToInvoiceDistribution");
                    //dataTable.Columns.Add("Quantity");
                    //dataTable.Columns.Add("ItemID");
                    //dataTable.Columns.Add("SerialNumber");
                    //dataTable.Columns.Add("Description");
                    //dataTable.Columns.Add("G/LAccount");
                    //dataTable.Columns.Add("UnitPrice");
                    //dataTable.Columns.Add("TaxType");
                    //dataTable.Columns.Add("Amount");
                    //dataTable.Columns.Add("InventoryAccount");
                    //dataTable.Columns.Add("CostOfSalesAccount");
                    //dataTable.Columns.Add("JobID");
                    //dataTable.Columns.Add("SalesTaxAgencyID");
                    //dataTable.Columns.Add("RecurNumber");
                    //dataTable.Columns.Add("RecurFrequency");
                    ////dataTable.Columns.Add("EDDID");
                    ////dataTable.Columns.Remove("EDDID");                    
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                    string serverName = connectionStringBuilder.DataSource.Trim();
                    string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                    string userID = connectionStringBuilder.UserID.Trim();
                    string password = connectionStringBuilder.Password.Trim();
                    //string sqlSelect = "Select * from EDDExport_View where [EDDID] in(" + strEDDId + ")ORDER BY [Invoice/CM #] ASC,[Invoice/CMDistribut ion] ASC";
                    SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                    DataTable dt = new DataTable();
                    using (SqlCommand sqlCommand = new SqlCommand("Invoice_EDDExport_SP", sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] param = new SqlParameter[2];
                        param[0] = new SqlParameter("@EDDID", strEDDId);
                        param[1] = new SqlParameter("@JobID", strJobID);
                        sqlCommand.Parameters.AddRange(param);
                        SqlDataAdapter sda = new SqlDataAdapter(sqlCommand);
                        sda.Fill(dt);
                        sqlConnection.Close();
                    }
                    //SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);                      
                    //sqlDa.Fill(dataTable);
                    dt.Columns["NumberOfDistributions"].SetOrdinal(28);
                    dt.Columns["Invoice/CMDistribution"].SetOrdinal(29);
                    dt.Columns.Remove("EDDID");
                    dt.AcceptChanges();
                    Workbook wb = new Workbook();
                    Worksheet worksheet0 = wb.Worksheets[0];
                    worksheet0.Name = "data";
                    wb.Worksheets[0].Import(dt, true, 0, 0);
                    wb.SaveDocument(strExportedPath);
                    string[] path = strExportedPath.Split('\\');
                    int arrcount = path.Count();
                    int sc = arrcount - 2;
                    string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                    WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

                    //var fileBytes = Encoding.UTF8.GetBytes(dataTable.ToString());
                    //HttpContext.Current.Response.Clear();
                    //HttpContext.Current.Response.Buffer = true;
                    //HttpContext.Current.Response.ContentType = "text/csv";
                    //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=data.csv");
                    //HttpContext.Current.Response.BinaryWrite(fileBytes);
                    //HttpContext.Current.Response.Flush();
                    //HttpContext.Current.Response.Close();
                    //dataTable.Exp
                    //Workbook wb =  Workbook.();
                    //Worksheet ws = wb.Worksheets[0];
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


        private void ExportToQuickbook_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                objInvoiceID = string.Empty;
                if (View.SelectedObjects.Count > 0)
                {
                    if (View.Id == "Invoicing_ListView_Review_History")
                    {
                        foreach (Invoicing objinvoice in View.SelectedObjects)
                        {
                            if (objInvoiceID == string.Empty)
                            {
                                objInvoiceID = objinvoice.InvoiceID.ToString();
                            }
                            else
                            {
                                if (!objInvoiceID.Contains(objinvoice.JobID.ToString()))
                                {
                                    objInvoiceID = objInvoiceID + "," + objinvoice.InvoiceID.ToString();
                                }
                            }
                        }
                    }

                    else if (View.Id == "InvoicingEDDExport_ListView")
                    {
                        foreach (InvoicingEDDExport  objinvoice in View.SelectedObjects)
                        {
                            if (objInvoiceID == string.Empty)
                            {
                                objInvoiceID = objinvoice.InvoiceID.InvoiceID.ToString();
                            }
                            else
                            {
                                if (!objInvoiceID.Contains(objinvoice.InvoiceID.JobID.ToString()))
                                {
                                    objInvoiceID = objInvoiceID + "," + objinvoice.InvoiceID.InvoiceID.ToString();
                                }
                            }
                        }
                    }

                    string strTempPath = Path.GetTempPath();
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                    }

                    string strExportedPath2 = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".csv");
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                    string serverName = connectionStringBuilder.DataSource.Trim();
                    string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                    string userId = connectionStringBuilder.UserID.Trim();
                    string password = connectionStringBuilder.Password.Trim();
                    SqlConnection sqlConnection = new SqlConnection(connectionString.ToString());
                    StringBuilder stringBuilder = new StringBuilder();
                    DataTable dataTable = new DataTable();
                    using (SqlCommand sqlCommand = new SqlCommand("Invoice_ExportToQucikBook_SP", sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] param = new SqlParameter[1];
                        param[0] = new SqlParameter("@InvoiceID", objInvoiceID);
                        sqlCommand.Parameters.AddRange(param);
                        SqlDataAdapter sda = new SqlDataAdapter(sqlCommand);
                        sda.Fill(dataTable);
                        sqlConnection.Close();
                    }
                    dataTable.AcceptChanges();
                    Workbook wb = new Workbook();
                    Worksheet worksheet0 = wb.Worksheets[0];
                    worksheet0.Name = "data";
                    wb.Worksheets[0].Import(dataTable, true, 0, 0);
                    int invoiceClientsColumnIndex = dataTable.Columns.IndexOf("InvoiceClients");
                    //int DudateColumnIndex = dataTable.Columns.IndexOf("DueDate");
                    if (invoiceClientsColumnIndex != -1)
                    {
                        worksheet0.Cells[0, invoiceClientsColumnIndex].Value = "InvoiceClient";
                       
                    }

                    //if (dataTable.Columns[DudateColumnIndex].DataType == typeof(DateTime))
                    //{
                    //    // Loop through the rows in the "InvoiceClients" column and format each cell
                    //    for (int rowIndex = 1; rowIndex < dataTable.Rows.Count + 1; rowIndex++)
                    //    {
                    //        DateTime dateValue = (DateTime)dataTable.Rows[rowIndex - 1][invoiceClientsColumnIndex];
                    //        worksheet0.Cells[rowIndex, invoiceClientsColumnIndex].Value = dateValue.ToString("MM/dd/yyyy HH:mm");
                    //    }
                    //}
                    wb.SaveDocument(strExportedPath2);
                    string[] path = strExportedPath2.Split('\\');
                    int arrcount = path.Count();
                    int sc = arrcount - 2;
                    string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                    WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
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


