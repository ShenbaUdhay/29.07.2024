using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ItemStockQtyEditorController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        StockQtyEditInfo objStockQtyEditInfo = new StockQtyEditInfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();

        public ItemStockQtyEditorController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "ItemStockQtyEdit;" + "Items_ListView;" + "Items_ListView_StockQtyEdit;" + "Distribution_ListView_StockQtyEdit;" + "Items_DetailView;";
            StockQtyEdit.TargetViewId = "Items_ListView;" + "Items_DetailView;";
            AddItems.TargetViewId = "ItemStockQtyEdit;";
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

                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.ICMStockqtyedit = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {

                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Items" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ICMStockqtyedit = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                StockQtyEdit.Active.SetItemValue("ShowStockQtyEdit", objPermissionInfo.ICMStockqtyedit);
                // Perform various tasks depending on the target View.
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View.Id == "ItemStockQtyEdit")
                {
                    objStockQtyEditInfo.ItemIsPopulated = false;
                    DashboardViewItem dviItems = ((DashboardView)View).FindItem("viItems") as DashboardViewItem;
                    if (dviItems != null)
                    {
                        dviItems.ControlCreated += DviItems_ControlCreated;
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

        private void PopupControl_CustomizePopupWindowSize(object sender, CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "ItemStockQtyEdit" || View.Id == "Items_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(730);
                    e.Height = new System.Web.UI.WebControls.Unit(700);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DviItems_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                if (!objStockQtyEditInfo.ItemIsPopulated)
                {
                    DashboardViewItem dviItems = (DashboardViewItem)sender;
                    if (dviItems != null && dviItems.InnerView != null)
                    {
                        IList<Guid> lstItemsOid = Application.MainWindow.View.SelectedObjects.Cast<Items>().Select(i => i.Oid).ToList();
                        ((ListView)dviItems.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstItemsOid);
                        objStockQtyEditInfo.ItemIsPopulated = true;
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
                // Access and customize the target View control.
                if (View.Id == "Items_ListView_StockQtyEdit" || View.Id == "Distribution_ListView_StockQtyEdit")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 125;
                        gridListEditor.Grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        if (View.Id == "Items_ListView_StockQtyEdit")
                        {
                            gridListEditor.Grid.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            gridListEditor.Grid.Columns["ItemCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                            gridListEditor.Grid.Columns["items"].FixedStyle = GridViewColumnFixedStyle.Left;
                            //  gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
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
            try
            {
                // Unsubscribe from previously subscribed events and release other references and resources.
                base.OnDeactivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AddItems_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

                DashboardViewItem dviItems = ((DashboardView)View).FindItem("viItems") as DashboardViewItem;
                DashboardViewItem dviDistribution = ((DashboardView)View).FindItem("viDistribution") as DashboardViewItem;
                if (dviItems != null && dviDistribution != null)
                {
                    if (dviItems.InnerView == null)
                    {
                        dviItems.CreateControl();
                    }
                    if (dviDistribution.InnerView == null)
                    {
                        dviDistribution.CreateControl();
                    }
                    if (((ListView)dviItems.InnerView).SelectedObjects.Count > 0)
                    {
                        ((ListView)dviItems.InnerView).Refresh();
                        ((ListView)dviDistribution.InnerView).RefreshDataSource();
                        //ASPxGridListEditor gridListEditor = ((ListView)dviItems.InnerView).Editor as ASPxGridListEditor;
                        //if (gridListEditor != null && gridListEditor.Grid != null)
                        //{
                        //    gridListEditor.Grid.UpdateEdit();
                        //}
                        List<Items> lstItems = ((ListView)dviItems.InnerView).SelectedObjects.Cast<Items>().ToList();
                        string curdate = DateTime.Now.ToString("yy");
                        int intLTNO = 0;
                        CriteriaOperator LTExpressionWithoutSequence = CriteriaOperator.Parse("Max(LT)");
                        CriteriaOperator LTCriteriaWithoutSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Not Contains([LT], '_')", curdate);

                        CriteriaOperator LTExpressionWithSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 0, 6))");
                        CriteriaOperator LTCriteriaWithSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Contains([LT], '_')", curdate);

                        int templtwithoutsequence = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Distribution), LTExpressionWithoutSequence, LTCriteriaWithoutSequence)) + 1;
                        int templtwithsequence = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Distribution), LTExpressionWithSequence, LTCriteriaWithSequence)) + 1;

                        if (templtwithoutsequence == 1 && templtwithsequence == 1)
                        {
                            intLTNO = Convert.ToInt32(curdate + "0001");
                        }
                        else
                        {
                            intLTNO = (templtwithoutsequence > templtwithsequence) ? templtwithoutsequence : templtwithsequence;
                        }

                        List<Guid> lstOid = new List<Guid>();
                        IObjectSpace os = ((ListView)dviDistribution.InnerView).ObjectSpace;
                        List<Distribution> lstDistribution = ((ListView)dviDistribution.InnerView).CollectionSource.List.Cast<Distribution>().ToList();
                        int itemCount = 0;
                        foreach (Items objItem in lstItems)
                        {
                            List<Distribution> tempDistribution = lstDistribution.Where(i => i.Item.Oid == objItem.Oid).ToList();
                            if (tempDistribution != null && tempDistribution.Count == objItem.AdditionalQty)
                            {
                                itemCount++;
                            }
                        }
                        if (lstItems.Count == itemCount)
                        {
                            Application.ShowViewStrategy.ShowMessage(@"Items are already added.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                            return;
                        }

                        foreach (Items objItem in lstItems)
                        {
                            if (!lstOid.Contains(objItem.Oid))
                            {
                                if (lstOid.Count > 0)
                                {
                                    intLTNO++;
                                }
                                lstOid.Add(objItem.Oid);
                            }

                            if (objItem.AdditionalQty == 1)
                            {
                                Distribution objDistribution = os.CreateObject<Distribution>();
                                objDistribution.Item = os.GetObject<Items>(objItem);
                                objDistribution.ItemCode = objItem.ItemCode;
                                objDistribution.Vendor = os.GetObject<Vendors>(objItem.Vendor);
                                objDistribution.LT = intLTNO.ToString();
                                objDistribution.Status = Distribution.LTStatus.PendingConsume;
                                ((ListView)dviDistribution.InnerView).CollectionSource.Add(objDistribution);
                            }
                            else
                            {
                                for (int i = 1; i <= objItem.AdditionalQty; i++)
                                {
                                    Distribution objDistribution = os.CreateObject<Distribution>();
                                    objDistribution.Item = os.GetObject<Items>(objItem);
                                    objDistribution.ItemCode = objItem.ItemCode;
                                    objDistribution.Vendor = os.GetObject<Vendors>(objItem.Vendor);
                                    //if (objDistribution.ExpiryDate <= DateTime.Now.AddDays(7))
                                    //{
                                    //    objDistribution.Status = Distribution.LTStatus.PendingDispose;
                                    //}
                                    //else
                                    //{
                                    //    objDistribution.Status = Distribution.LTStatus.PendingConsume;
                                    //}
                                    if (i < 10)
                                    {
                                        objDistribution.LT = intLTNO + "_0" + i;
                                    }
                                    else
                                    {
                                        objDistribution.LT = intLTNO + "_" + i;
                                    }
                                    ((ListView)dviDistribution.InnerView).CollectionSource.Add(objDistribution);
                                }
                            }
                        }
                        ((ListView)dviDistribution.InnerView).Refresh();
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void StockQtyEdit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                DashboardView dashboard = Application.CreateDashboardView(os, "ItemStockQtyEdit", false);
                ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                dc.Accepting += Dc_Accepting;
                dc.AcceptAction.ExecuteCompleted += AcceptAction_ExecuteCompleted;
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AcceptAction_ExecuteCompleted(object sender, ActionBaseEventArgs e)
        {
            try
            {
                Application.MainWindow.View.ObjectSpace.Refresh();
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

                DevExpress.ExpressApp.SystemModule.DialogController dc = (DevExpress.ExpressApp.SystemModule.DialogController)sender;
                if (dc != null && dc.Frame.View is DashboardView)
                {
                    DashboardViewItem dviDistribution = ((DashboardView)dc.Frame.View).FindItem("viDistribution") as DashboardViewItem;
                    DashboardViewItem dviItems = ((DashboardView)dc.Frame.View).FindItem("viItems") as DashboardViewItem;
                    if (dviDistribution != null)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)dviDistribution.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.UpdateEdit();
                        }
                        ((ListView)dviDistribution.InnerView).Refresh();
                        if (((ListView)dviDistribution.InnerView).CollectionSource.List.Count > 0)
                        {
                            List<Distribution> lstDistributions = ((ListView)dviDistribution.InnerView).CollectionSource.List.Cast<Distribution>().ToList();
                            List<Items> lstItems = ((ListView)dviItems.InnerView).CollectionSource.List.Cast<Items>().ToList();

                            if (lstDistributions != null)
                            {
                                if (lstDistributions.FirstOrDefault(i => i.Item.IsVendorLT && string.IsNullOrEmpty(i.VendorLT)) != null)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "emptyvendorlt"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    e.Cancel = true;
                                    return;
                                }
                                else if (lstDistributions.FirstOrDefault(i => i.Storage == null) != null)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectstorage"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    e.Cancel = true;
                                    return;
                                }
                                else if (lstDistributions.FirstOrDefault(i => i.ExpiryDate == null) != null)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "expirydate"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    e.Cancel = true;
                                    return;
                                }

                                foreach (Items objItem in lstItems)
                                {
                                    List<Distribution> tempDistribution = lstDistributions.Where(i => i.Item.Oid == objItem.Oid).ToList();
                                    if (tempDistribution != null && tempDistribution.Count > 0)
                                    {
                                        objItem.StockQty = objItem.StockQty + tempDistribution.Count;
                                        foreach (Distribution objDistribution in lstDistributions)
                                        {
                                            if (objDistribution.ExpiryDate >= DateTime.Today)
                                            {
                                                objDistribution.Status = Distribution.LTStatus.PendingConsume;
                                            }
                                            else
                                            {
                                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ExpirationDatelessthancurrentdateissue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                e.Cancel = true;
                                                return;
                                            }
                                        }
                                    }
                                }
                                if (lstDistributions.FirstOrDefault(i => i.ExpiryDate >= DateTime.Today && i.Storage != null && !string.IsNullOrEmpty(i.VendorLT)) != null)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Saved Successfully.", InformationType.Success, 3000, InformationPosition.Top);
                                }

                                ((ListView)dviDistribution.InnerView).ObjectSpace.CommitChanges();
                                ((ListView)dviItems.InnerView).ObjectSpace.CommitChanges();
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(@"Items not added.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                e.Cancel = true;
            }
        }

    }
}
