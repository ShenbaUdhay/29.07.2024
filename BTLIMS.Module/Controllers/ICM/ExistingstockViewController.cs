using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.ICM
{
    public partial class ExistingstockViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        ShowNavigationItemController ShowNavigationController;
        requisitionquerypanelinfo objreq = new requisitionquerypanelinfo();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        ICMinfo ICMInfo = new ICMinfo();
        Existingstockquerypanelinfo objes = new Existingstockquerypanelinfo();
        string strLT = string.Empty;

        public ExistingstockViewController()
        {
            InitializeComponent();
            TargetViewId = "ExistingStock_ListView;" + "Distribution_ListView_Existingstock;" + "ExistingStock_LookupListView;" + "Existingstockquerypanel_DetailView;" + "Distribution_ListView_Existingstockview;";
            AddItem.TargetViewId = "ExistingStock_ListView";
            Existstockdelete.TargetViewId = "ExistingStock_ListView";
            StockSave.TargetViewId = "ExistingStock_ListView";
            //ShowLT.TargetViewId = "ExistingStock_ListView";
            LTPreviewReportES.TargetViewId = "Distribution_ListView_Existingstockview";
            //ExistingstockFilter.TargetViewId = "ExistingStock_ListView;" + "Distribution_ListView_Existingstockview;";
            ExistingstockRollback.TargetViewId = "Distribution_ListView_Existingstockview";
            LTBarcodeReportES.TargetViewId = "Distribution_ListView_Existingstockview;";
            LTBarcodeReportES.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            var item = new ChoiceActionItem();
            LTBarcodeReportES.Items.Add(new ChoiceActionItem("", item));
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id != null && View.Id == "ExistingStock_ListView")
                {
                    if (objes.Esidlist != null)
                    {
                        objes.Esidlist.Clear();
                    }
                }
                if (View != null && View.Id == "Distribution_ListView_Existingstock" || View.Id == "Distribution_ListView_Existingstockview")
                {
                    if (LTBarcodeReportES.Items.Count == 1)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        LTBarcodeReportES.Items.Add(new ChoiceActionItem("LTBarCode", item1));
                        LTBarcodeReportES.Items.Add(new ChoiceActionItem("LTBarCodeBig", item2));
                    }
                }
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                if (View != null && View.CurrentObject != null && View.Id == "Existingstockquerypanel_DetailView")
                {
                    Existingstockquerypanel QPanel = (Existingstockquerypanel)View.CurrentObject;
                    objes.rgMode = QPanel.Mode.ToString();
                }
                //objes.ISnew = true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (base.View != null && base.View.Id == "Existingstockquerypanel_DetailView")
                {
                    if (View != null && View.CurrentObject == e.Object && e.PropertyName == "ESID")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Existingstockquerypanel))
                        {
                            Existingstockquerypanel ESPanel = (Existingstockquerypanel)e.Object;
                            if (ESPanel.ESID != null)
                            {
                                if (ESPanel.Mode.Equals(ENMode.View))
                                {
                                    if (objes.ExistingstockFilter != string.Empty)
                                    {
                                        objes.ExistingstockFilter = objes.ExistingstockFilter + "AND [ESID] == '" + ESPanel.ESID.ESID + "'";
                                    }
                                    else
                                    {
                                        objes.ExistingstockFilter = "[ESID] == '" + ESPanel.ESID.ESID + "'";
                                    }
                                }
                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Mode")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Existingstockquerypanel))
                        {
                            Existingstockquerypanel DPanel = (Existingstockquerypanel)e.Object;
                            if (DPanel != null)
                            {
                                objes.rgMode = DPanel.Mode.ToString();
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
            base.OnViewControlsCreated();
            try
            {
                if (View.Id != null && View.Id == "ExistingStock_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    //gridListEditor.Grid.RowValidating += Grid_RowValidating;
                    //gridListEditor.Grid.Load += Grid_Load;
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    if (objes.ExistingstockFilter == null || objes.ExistingstockFilter == string.Empty)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ESID] Is NULL");
                    }

                    //if (objes.rgMode == ENMode.View.ToString())
                    //{
                    //    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) { 
                    //   if(s.batchEditApi.GetCellValue(e.visibleIndex, 'ESID') != null){
                    //   e.cancel = true;
                    //   }}";
                    //}
                    //else
                    //{

                    //if ((e.NewValues["givento.Oid"] == null && e.NewValues["Storage.Oid"] == null)   e.NewValues["ExpiryDate"] == null || Convert.ToInt32(e.NewValues["Qty"]) <= 0)


                    string js = @"                                         
                            if (window.ASPxClientGridView) {{
                            var allGirds = ASPx.GetControlCollection().GetControlsByType(ASPxClientGridView);
                            }}
                            var errstat = 0;
                            var errmsg;
                            for (var a in allGirds) {{                  
                            for (var i = 0; i <= allGirds[a].GetVisibleRowsOnPage() - 1; i++) {{
                            allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', null);
                            if ((allGirds[a].batchEditApi.GetCellValue(i, 'givento.Oid') == null && allGirds[a].batchEditApi.GetCellValue(i, 'Storage.Oid') == null) || allGirds[a].batchEditApi.GetCellValue(i, 'Qty') <= 0 || allGirds[a].batchEditApi.GetCellValue(i, 'ExpiryDate') == null) {{                                                            
                            errmsg = 'Select ';
                            if((allGirds[a].batchEditApi.GetCellValue(i, 'givento.Oid') == null && allGirds[a].batchEditApi.GetCellValue(i, 'Storage.Oid') == null)){{
                            errmsg = errmsg + 'givento or storage';
                            }}
                            if(allGirds[a].batchEditApi.GetCellValue(i, 'Qty') <= 0){{
                            if(errmsg.length > 7){{
                            errmsg = errmsg + ',quantity';
                            }}else{{
                            errmsg = errmsg + 'quantity ';
                            }}
                            }}
                            if(allGirds[a].batchEditApi.GetCellValue(i, 'ExpiryDate') == null){{
                            if(errmsg.length > 7){{
                            errmsg = errmsg + ',expiry date';
                            }}else{{
                            errmsg = errmsg + 'expiry date';
                            }}
                            }}
                            allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', errmsg);     
                            errstat = 1                          
                            }}                            
                            }}                   
                            }}
                            {0}
                            ";

                    ICallbackManagerHolder holder = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    holder.CallbackManager.RegisterHandler("sam", this);
                    StockSave.SetClientScript(string.Format(CultureInfo.InvariantCulture, js, holder.CallbackManager.GetScript("sam", "errstat")), false);


                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s, e){ 
                            for (var i = 0; i <= s.GetVisibleRowsOnPage() - 1; i++) {
                            if (s.batchEditApi.GetCellValue(i, 'Errorlog') != null) {
                                s.GetRow(i).style.backgroundColor='OrangeRed';   
                                setTimeout(function () {
                                var width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
                                window.scrollTo(width, 0);
                            }, 2);
                            }}
                            }";

                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {
                       if(s.batchEditApi.GetCellValue(e.visibleIndex, 'ESID') == null){
                       if(e.focusedColumn.fieldName == 'ExpiryDate' || e.focusedColumn.fieldName == 'Qty' || e.focusedColumn.fieldName == 'givento.Oid' || e.focusedColumn.fieldName == 'Storage.Oid')
                       {
                       e.cancel = false;                  
                       }
                       else{e.cancel = true;
                       }}
                       else{
                       e.cancel = true
                       }
                       }";

                    gridListEditor.Grid.ClientSideEvents.CustomButtonClick = @"function(s ,e){ 
                        s.batchEditApi.ResetChanges(e.visibleIndex);     
                        }";

                    //gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) {
                    //   if (!s.IsRowSelectedOnPage(e.visibleIndex)){
                    //   if(s.batchEditApi.GetCellValue(e.visibleIndex, 'ESID') == null){
                    //   s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                    //   s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                    //   s.batchEditApi.SetCellValue(e.visibleIndex, 'Itemname.StockQty', null);
                    //   s.batchEditApi.SetCellValue(e.visibleIndex, 'Vendor', null);
                    //   s.batchEditApi.SetCellValue(e.visibleIndex, 'ExpiryDate', null);
                    //   s.batchEditApi.SetCellValue(e.visibleIndex, 'Qty', '0');
                    //   s.batchEditApi.SetCellValue(e.visibleIndex, 'Itemname', null);
                    //   }}
                    //   }";

                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {                    
                            window.setTimeout(function() {                      
                            var Storage = s.batchEditApi.GetCellValue(e.visibleIndex, 'Storage');
                            var givento = s.batchEditApi.GetCellValue(e.visibleIndex, 'givento');                    
                            if(sessionStorage.getItem('selecttype' + e.visibleIndex) == null){                    
                            if(Storage != null){
                            s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                            sessionStorage.setItem('selecttype' + e.visibleIndex, 'Storage');                                                  
                            }
                            else if(givento != null){
                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                            sessionStorage.setItem('selecttype' + e.visibleIndex, 'givento');                                                  
                            }
                            } else {
                            var select = sessionStorage.getItem('selecttype' + e.visibleIndex);
                            if(select == 'givento' && Storage != null){
                            s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                            sessionStorage.setItem('selecttype' + e.visibleIndex, 'Storage');                                                  
                            }
                            else if(select == 'Storage' && givento != null){
                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                            sessionStorage.setItem('selecttype' + e.visibleIndex, 'givento');                                                  
                            }} 
                            //if((s.batchEditApi.GetCellValue(e.visibleIndex, 'givento.Oid') != null || s.batchEditApi.GetCellValue(e.visibleIndex, 'Storage.Oid') != null) && s.batchEditApi.GetCellValue(e.visibleIndex, 'Itemname.Oid') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'Vendor.Oid') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'ExpiryDate') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty') != null){
                            //sessionStorage.setItem('Validate' + e.visibleIndex, 'True');
                            //}
                            //else{
                            //sessionStorage.setItem('Validate' + e.visibleIndex, 'False');
                            //}
                            }, 20);}";
                    //}
                }
                else if (View != null && View.Id == "ExistingStock_LookupListView")
                {
                    if (objes.rgMode == "View")
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not IsNullOrEmpty([ESID])");
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ESID] = 'Emptygrid'");
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void Grid_Load(object sender, EventArgs e)
        //{
        //    //ASPxGridView gridView = sender as ASPxGridView;
        //    //gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
        //    //var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
        //    //selectionBoxColumn.SelectCheckBoxPosition = GridSelectCheckBoxPosition.Left;
        //    //selectionBoxColumn.FixedStyle = GridViewColumnFixedStyle.Left;
        //    //selectionBoxColumn.VisibleIndex = 0;
        //    //selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
        //}

        //private void Grid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        //{
        //    try
        //    {
        //        if ((e.NewValues["givento.Oid"] == null && e.NewValues["Storage.Oid"] == null) || e.NewValues["Itemname.Oid"] == null || e.NewValues["Vendor.Oid"] == null || e.NewValues["ExpiryDate"] == null || Convert.ToInt32(e.NewValues["Qty"]) <= 0)
        //        {
        //            e.RowError = "All Fields must be filled";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                strLT = string.Empty;
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void StockNew_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        ExistingStock objextt = ObjectSpace.CreateObject<ExistingStock>();
        //        ((ListView)View).CollectionSource.Add(objextt);
        //        View.Refresh();
        //        objes.ExistingstockFilter = "Nothing";
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void StockSave_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //try
        //{
        //    ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
        //    foreach (ExistingStock objext in View.SelectedObjects)
        //    {
        //        if ((objext.givento != null || objext.Storage != null) && objext.Itemname != null && objext.Vendor != null && objext.ExpiryDate != null && objext.Qty >= 1)
        //        {
        //            CriteriaOperator escriteria = CriteriaOperator.Parse("Max(SUBSTRING(ESID, 2))");
        //            string tempes = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Distribution), escriteria, null)) + 1).ToString();
        //            var escurdate = DateTime.Now.ToString("yyMMdd");
        //            if (tempes != "1")
        //            {
        //                var predate = tempes.Substring(0, 6);
        //                if (predate == escurdate)
        //                {
        //                    tempes = "ES" + tempes;
        //                }
        //                else
        //                {
        //                    tempes = "ES" + escurdate + "01";
        //                }
        //            }
        //            else
        //            {
        //                tempes = "ES" + escurdate + "01";
        //            }
        //            objext.ESID = tempes;
        //            objext.Itemname.StockQty = objext.Itemname.StockQty + objext.Qty;
        //            ObjectSpace.CommitChanges();
        //            for (int i = 1; i <= objext.Qty; i++)
        //            {
        //                IObjectSpace os = Application.CreateObjectSpace();
        //                Distribution newobj = os.CreateObject<Distribution>();
        //                if (objext.Itemname.items != null)
        //                {
        //                    newobj.Item = os.GetObjectByKey<Items>(objext.Itemname.Oid);
        //                }
        //                if (objext.Vendor != null)
        //                {
        //                    newobj.Vendor = os.GetObjectByKey<Vendors>(objext.Vendor.Oid);
        //                }
        //                if (objext.ExpiryDate != null)
        //                {
        //                    newobj.ExpiryDate = objext.ExpiryDate;
        //                }
        //                CriteriaOperator ltcriteria = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
        //                string templt = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(Distribution), ltcriteria, null)) + 1).ToString();
        //                var curdate = DateTime.Now.ToString("yy");
        //                if (templt != "1")
        //                {
        //                    var predate = templt.Substring(0, 2);
        //                    if (predate == curdate)
        //                    {
        //                        templt = "LT" + templt;
        //                    }
        //                    else
        //                    {
        //                        templt = "LT" + curdate + "0001";
        //                    }
        //                }
        //                else
        //                {
        //                    templt = "LT" + curdate + "0001";
        //                }
        //                newobj.LT = templt;
        //                newobj.ESID = tempes;
        //                if (objext.Storage != null)
        //                {
        //                    newobj.Storage = os.GetObjectByKey<ICMStorage>(objext.Storage.Oid);
        //                    newobj.Status = Distribution.LTStatus.PendingConsume;
        //                    if (newobj.ExpiryDate <= DateTime.Now.AddDays(7) && newobj.ExpiryDate != null)
        //                    {
        //                        IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
        //                        ICMAlert objdisp = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + newobj.LT));
        //                        if (objdisp == null)
        //                        {
        //                            ICMAlert obj1 = space.CreateObject<ICMAlert>();
        //                            obj1.Subject = "Expiry Alert - " + newobj.LT;
        //                            obj1.StartDate = DateTime.Now;
        //                            obj1.DueDate = DateTime.Now.AddDays(7);
        //                            obj1.RemindIn = TimeSpan.FromMinutes(5);
        //                            obj1.Description = "Nice";
        //                            space.CommitChanges();
        //                        }
        //                        if (newobj.ExpiryDate <= DateTime.Now)
        //                        {
        //                            newobj.Status = Distribution.LTStatus.PendingDispose;
        //                        }
        //                    }
        //                }
        //                else if (objext.givento != null)
        //                {
        //                    newobj.Status = Distribution.LTStatus.Consumed;
        //                    newobj.ConsumptionBy = os.GetObjectByKey<Employee>(objext.givento.Oid);
        //                    newobj.ConsumptionDate = DateTime.Now;
        //                    newobj.EnteredBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
        //                    newobj.EnteredDate = DateTime.Now;
        //                    ConsumptionHistory objconshis = os.CreateObject<ConsumptionHistory>();
        //                    objconshis.Distribution = newobj;
        //                    objconshis.ConsumptionBy = os.GetObjectByKey<Employee>(objext.givento.Oid);
        //                    objconshis.ConsumptionDate = DateTime.Now;
        //                    objconshis.EnteredBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
        //                    objconshis.EnteredDate = DateTime.Now;
        //                    objconshis.Consumed = true;
        //                }
        //                newobj.DistributionDate = DateTime.Now;
        //                newobj.GivenBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
        //                os.CommitChanges();
        //            }
        //            if (objext.Itemname.StockQty >= objext.Itemname.AlertQty)
        //            {
        //                IObjectSpace objspace = Application.CreateObjectSpace();
        //                IList<ICMAlert> alertlist = objspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Low Stock - " + objext.Itemname.items + "(" + objext.Itemname.ItemCode + ")"));
        //                if (alertlist != null)
        //                {
        //                    foreach (ICMAlert item in alertlist)
        //                    {
        //                        item.AlarmTime = null;
        //                        item.RemindIn = null;
        //                    }
        //                    objspace.CommitChanges();
        //                }
        //            }
        //            else
        //            {
        //                IObjectSpace objspace1 = Application.CreateObjectSpace();
        //                ICMAlert objdisp1 = objspace1.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + objext.Itemname.items + "(" + objext.Itemname.ItemCode + ")"));
        //                if (objdisp1 == null)
        //                {
        //                    IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
        //                    ICMAlert obj1 = space.CreateObject<ICMAlert>();
        //                    obj1.Subject = "Low Stock - " + objext.Itemname.items + "(" + objext.Itemname.ItemCode + ")";
        //                    obj1.StartDate = DateTime.Now;
        //                    obj1.DueDate = DateTime.Now.AddDays(7);
        //                    obj1.RemindIn = TimeSpan.FromMinutes(5);
        //                    obj1.Description = "Nice";
        //                    space.CommitChanges();
        //                }
        //            }
        //              ((ASPxGridListEditor)((ListView)View).Editor).UnselectRowByKey(objext.Oid);
        //        }
        //    }
        //    NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
        //    module.ShowNotificationsWindow = false;
        //    module.NotificationsService.Refresh();
        //    ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
        //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
        //    {
        //        if (parent.Id == "ICM")
        //        {
        //            foreach (ChoiceActionItem child in parent.Items)
        //            {
        //                if (child.Id == "Operations")
        //                {
        //                    foreach (ChoiceActionItem subchild in child.Items)
        //                    {
        //                        //if (subchild.Id == "ConsumptionItems")
        //                        //{
        //                        //    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                        //    var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[ConsumptionBy] Is Null And [ConsumptionDate] Is Null And [Status] = 'PendingConsume' And ([ExpiryDate] Is Null or [ExpiryDate] > ?)", DateTime.Now));
        //                        //    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                        //    if (count > 0)
        //                        //    {
        //                        //        subchild.Caption = cap[0] + " (" + count + ")";
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        subchild.Caption = cap[0];
        //                        //    }
        //                        //}
        //                        if (subchild.Id == "DisposalItems")
        //                        {
        //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[DisposedBy] Is Null And [DisposedDate] Is Null And [ExpiryDate] <= ?", DateTime.Now));
        //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                            if (count > 0)
        //                            {
        //                                subchild.Caption = cap[0] + " (" + count + ")";
        //                            }
        //                            else
        //                            {
        //                                subchild.Caption = cap[0];
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //objes.ISnew = true;
        //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, 1500, InformationPosition.Top);
        //}
        //catch (Exception ex)
        //{
        //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //}
        //}

        //private void StockCancel_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        //WebWindow.CurrentRequestWindow.RegisterClientScript("Cancel", "grid.CancelEdit();");
        //        ((ASPxGridListEditor)((ListView)View).Editor).Grid.CancelEdit();
        //        ObjectSpace.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void ShowLT_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (View.SelectedObjects.Count == 1)
        //        {
        //            foreach (ExistingStock obj in View.SelectedObjects)
        //            {
        //                if (obj.ESID != null)
        //                {
        //                    IObjectSpace objspace = Application.CreateObjectSpace();
        //                    CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
        //                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[ESID] = ?", obj.ESID);
        //                    if (cs != null)
        //                    {
        //                        ListView CreateListView = Application.CreateListView("Distribution_ListView_Existingstock", cs, false);
        //                        ShowViewParameters showViewParameters = new ShowViewParameters();
        //                        showViewParameters.CreatedView = CreateListView;
        //                        showViewParameters.Context = TemplateContext.PopupWindow;
        //                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
        //                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
        //                    }
        //                }
        //                else
        //                {
        //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Invalidoperation"), InformationType.Error, 1500, InformationPosition.Top);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, 1500, InformationPosition.Top);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void LTPreviewReportES_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if ((View.Id == "Distribution_ListView_Existingstock" || View.Id == "Distribution_ListView_Existingstockview") && View.SelectedObjects.Count > 0)
                {
                    if (LTBarcodeReportES.SelectedItem != null && LTBarcodeReportES.SelectedItem.ToString() != string.Empty)
                    {
                        foreach (Distribution obj in View.SelectedObjects)
                        {
                            if (obj.LT == null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "pendingdistribute"), InformationType.Info, 3000, InformationPosition.Top);
                                return;
                            }

                            if (strLT == string.Empty)
                            {
                                strLT = "'" + obj.LT.ToString() + "'";
                            }
                            else
                            {
                                strLT = strLT + ",'" + obj.LT.ToString() + "'";
                            }
                        }
                        string strTempPath = Path.GetTempPath();
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\"));
                        }
                        string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\" + timeStamp + ".pdf");
                        XtraReport xtraReport = new XtraReport();

                        objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();

                        DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                        //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                        ObjReportingInfo.strLT = strLT;
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(LTBarcodeReportES.SelectedItem.ToString(), ObjReportingInfo, false);
                        //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                        xtraReport.ExportToPdf(strExportedPath);
                        string[] path = strExportedPath.Split('\\');
                        int arrcount = path.Count();
                        int sc = arrcount - 3;
                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                    }
                    strLT = string.Empty;
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

        //private void ExistingstockFilter_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        ((ListView)View).CollectionSource.Criteria.Clear();
        //        int RowCount = 0;
        //        if (View != null && View.Id == "ExistingStock_ListView" || View.Id == "Distribution_ListView_Existingstockview")
        //        {
        //            if (objes.ExistingstockFilter == string.Empty)
        //            {
        //                if (objes.rgMode == "View")
        //                {
        //                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ESID] IS Null");
        //                }
        //                else
        //                {
        //                    IObjectSpace objspace = Application.CreateObjectSpace();
        //                    CollectionSource cs = new CollectionSource(objspace, typeof(ExistingStock));
        //                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[ESID] Is NULL");
        //                    Frame.SetView(Application.CreateListView("ExistingStock_ListView", cs, false));
        //                }
        //            }
        //            else if (objes.ExistingstockFilter != string.Empty)
        //            {
        //                if (objes.rgMode == "View")
        //                {
        //                    LTBarcodeReportES.Category = "Unspecified";
        //                    LTPreviewReportES.Category = "Unspecified";
        //                    IObjectSpace objspace = Application.CreateObjectSpace();
        //                    CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
        //                    cs.Criteria["Filter"] = CriteriaOperator.Parse(objes.ExistingstockFilter);
        //                    Frame.SetView(Application.CreateListView("Distribution_ListView_Existingstockview", cs, false));
        //                }
        //                RowCount = ((ListView)View).CollectionSource.GetCount();
        //                if (RowCount == 0)
        //                {
        //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Invalid"), InformationType.Info, 1500, InformationPosition.Top);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void ExistingstockFilter_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        //{
        //    try
        //    {
        //        objes.ExistingstockFilter = string.Empty;
        //        IObjectSpace objspace = Application.CreateObjectSpace();
        //        object objToShow = objspace.CreateObject(typeof(Existingstockquerypanel));
        //        if (objToShow != null)
        //        {
        //            DetailView CreateDetailView = Application.CreateDetailView(objspace, objToShow);
        //            CreateDetailView.ViewEditMode = ViewEditMode.Edit;
        //            e.View = CreateDetailView;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void ExistingstockRollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count >= 1)
                {
                    foreach (Distribution objdis in View.SelectedObjects)
                    {
                        if (objdis.Status == Distribution.LTStatus.Consumed || objdis.Status == Distribution.LTStatus.Disposed)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackfail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                    foreach (Distribution objdis in View.SelectedObjects)
                    {
                        IObjectSpace space = Application.CreateObjectSpace();
                        ExistingStock objes = space.FindObject<ExistingStock>(CriteriaOperator.Parse("[ESID] = ? ", objdis.ESID));
                        if (objes.Qty > 1)
                        {
                            objes.Qty -= 1;
                            objes.Itemname.StockQty -= 1;
                        }
                        else if (objes.Qty == 1)
                        {
                            objes.Itemname.StockQty -= 1;
                            space.Delete(objes);
                        }
                        IList<ICMAlert> alertlist = space.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Expiry Alert - " + objdis.LT));
                        if (alertlist != null)
                        {
                            foreach (ICMAlert item in alertlist)
                            {
                                item.AlarmTime = null;
                                item.RemindIn = null;
                            }
                        }
                        if (objdis.Item.StockQty <= objdis.Item.AlertQty)
                        {
                            ICMAlert objdisp1 = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + objdis.Item.items + "(" + objdis.Item.ItemCode + ")"));
                            if (objdisp1 == null)
                            {
                                ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                obj1.Subject = "Low Stock - " + objdis.Item.items + "(" + objdis.Item.ItemCode + ")";
                                obj1.StartDate = DateTime.Now;
                                obj1.DueDate = DateTime.Now.AddDays(7);
                                obj1.RemindIn = TimeSpan.FromMinutes(5);
                                obj1.Description = "Nice";
                            }
                        }
                        space.CommitChanges();
                        ObjectSpace.Delete(objdis);
                        space.CommitChanges();
                        ObjectSpace.CommitChanges();
                    }
                    View.Refresh();
                    NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                    module.ShowNotificationsWindow = false;
                    module.NotificationsService.Refresh();
                    ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    {
                        if (parent.Id == "ICM")
                        {
                            foreach (ChoiceActionItem child in parent.Items)
                            {
                                if (child.Id == "Operations")
                                {
                                    foreach (ChoiceActionItem subchild in child.Items)
                                    {
                                        //if (subchild.Id == "ConsumptionItems")
                                        //{
                                        //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        //    var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[ConsumptionBy] Is Null And [ConsumptionDate] Is Null And [Status] = 'PendingConsume' And ([ExpiryDate] Is Null or [ExpiryDate] > ?)", DateTime.Now));
                                        //    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        //    if (count > 0)
                                        //    {
                                        //        subchild.Caption = cap[0] + " (" + count + ")";
                                        //    }
                                        //    else
                                        //    {
                                        //        subchild.Caption = cap[0];
                                        //    }
                                        //}
                                        if (subchild.Id == "DisposalItems")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[DisposedBy] Is Null And [DisposedDate] Is Null And [ExpiryDate] <= ?", DateTime.Now));
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + count + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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

        private void AddItem_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                IList<ExistingStock> exstlist = ObjectSpace.GetObjects<ExistingStock>(CriteriaOperator.Parse("IsNullOrEmpty([ESID])"), true);
                if (exstlist.Count == 0)
                {
                    ((ListView)View).CollectionSource.Criteria.Clear();
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ESID] Is NULL");
                }
                foreach (Items obj in e.PopupWindowViewSelectedObjects)
                {
                    objreq.Items.Add(obj.items);
                    ExistingStock objnewreq = ObjectSpace.CreateObject<ExistingStock>();
                    objnewreq.Itemname = ObjectSpace.GetObject<Items>(obj);
                    if (obj.Vendor != null)
                    {
                        objnewreq.Vendor = ObjectSpace.GetObjectByKey<Vendors>(obj.Vendor.Oid);
                    }
                    ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(objnewreq));
                }
                View.Refresh();
                objes.ExistingstockFilter = "Nothing";
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AddItem_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Items));
                ListView CreateListView = Application.CreateListView("Items_LookupListView", cs, false);
                e.View = CreateListView;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Existstockdelete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ExistingStock item = (ExistingStock)e.CurrentObject;
                if (item.ESID == null)
                {
                    ((ListView)View).CollectionSource.Remove(item);
                    ObjectSpace.RemoveFromModifiedObjects(item);
                    objreq.Items.Remove(item.Itemname.items);
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
                if (parameter == "0")
                {
                    IList<ExistingStock> exstlist = ObjectSpace.GetObjects<ExistingStock>(CriteriaOperator.Parse("IsNullOrEmpty([ESID])"), true);
                    if (exstlist.Count >= 1)
                    {
                        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                        foreach (ExistingStock objext in exstlist)
                        {
                            if ((objext.givento != null || objext.Storage != null) && objext.Itemname != null && objext.Vendor != null && objext.ExpiryDate != null && objext.Qty >= 1)
                            {
                                CriteriaOperator escriteria = CriteriaOperator.Parse("Max(SUBSTRING(ESID, 2))");
                                string tempes = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Distribution), escriteria, null)) + 1).ToString();
                                var escurdate = DateTime.Now.ToString("yyMMdd");
                                if (tempes != "1")
                                {
                                    var predate = tempes.Substring(0, 6);
                                    if (predate == escurdate)
                                    {
                                        tempes = "ES" + tempes;
                                    }
                                    else
                                    {
                                        tempes = "ES" + escurdate + "01";
                                    }
                                }
                                else
                                {
                                    tempes = "ES" + escurdate + "01";
                                }
                                objext.ESID = tempes;
                                objext.Itemname.StockQty = objext.Itemname.StockQty + objext.Qty;
                                ObjectSpace.CommitChanges();
                                for (int i = 1; i <= objext.Qty; i++)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    Distribution newobj = os.CreateObject<Distribution>();
                                    if (objext.Itemname.items != null)
                                    {
                                        newobj.Item = os.GetObjectByKey<Items>(objext.Itemname.Oid);
                                    }
                                    if (objext.Vendor != null)
                                    {
                                        newobj.Vendor = os.GetObjectByKey<Vendors>(objext.Vendor.Oid);
                                    }
                                    if (objext.ExpiryDate != null)
                                    {
                                        newobj.ExpiryDate = objext.ExpiryDate;
                                    }
                                    CriteriaOperator ltcriteria = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                                    string templt = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(Distribution), ltcriteria, null)) + 1).ToString();
                                    var curdate = DateTime.Now.ToString("yy");
                                    if (templt != "1")
                                    {
                                        var predate = templt.Substring(0, 2);
                                        if (predate == curdate)
                                        {
                                            templt = "LT" + templt;
                                        }
                                        else
                                        {
                                            templt = "LT" + curdate + "0001";
                                        }
                                    }
                                    else
                                    {
                                        templt = "LT" + curdate + "0001";
                                    }
                                    newobj.LT = templt;
                                    newobj.ESID = tempes;
                                    if (objes.Esidlist == null)
                                    {
                                        objes.Esidlist = new List<string>();
                                    }
                                    objes.Esidlist.Add(tempes);
                                    if (objext.Storage != null)
                                    {
                                        newobj.Storage = os.GetObjectByKey<ICMStorage>(objext.Storage.Oid);
                                        newobj.Status = Distribution.LTStatus.PendingConsume;
                                        if (newobj.ExpiryDate <= DateTime.Now.AddDays(7) && newobj.ExpiryDate != null)
                                        {
                                            IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                                            ICMAlert objdisp = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + newobj.LT));
                                            if (objdisp == null)
                                            {
                                                ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                                obj1.Subject = "Expiry Alert - " + newobj.LT;
                                                obj1.StartDate = DateTime.Now;
                                                obj1.DueDate = DateTime.Now.AddDays(7);
                                                obj1.RemindIn = TimeSpan.FromMinutes(5);
                                                obj1.Description = "Nice";
                                                space.CommitChanges();
                                            }
                                            if (newobj.ExpiryDate <= DateTime.Now)
                                            {
                                                newobj.Status = Distribution.LTStatus.PendingDispose;
                                            }
                                        }
                                    }
                                    else if (objext.givento != null)
                                    {
                                        newobj.Status = Distribution.LTStatus.Consumed;
                                        newobj.ConsumptionBy = os.GetObjectByKey<Employee>(objext.givento.Oid);
                                        newobj.ConsumptionDate = DateTime.Now;
                                        newobj.EnteredBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                        newobj.EnteredDate = DateTime.Now;
                                        ConsumptionHistory objconshis = os.CreateObject<ConsumptionHistory>();
                                        objconshis.Distribution = newobj;
                                        objconshis.ConsumptionBy = os.GetObjectByKey<Employee>(objext.givento.Oid);
                                        objconshis.ConsumptionDate = DateTime.Now;
                                        objconshis.EnteredBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                        objconshis.EnteredDate = DateTime.Now;
                                        objconshis.Consumed = true;
                                    }
                                    newobj.DistributionDate = DateTime.Now;
                                    newobj.GivenBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                    os.CommitChanges();
                                }
                                if (objext.Itemname.StockQty >= objext.Itemname.AlertQty)
                                {
                                    IObjectSpace objspace = Application.CreateObjectSpace();
                                    IList<ICMAlert> alertlist = objspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Low Stock - " + objext.Itemname.items + "(" + objext.Itemname.ItemCode + ")"));
                                    if (alertlist != null)
                                    {
                                        foreach (ICMAlert item in alertlist)
                                        {
                                            item.AlarmTime = null;
                                            item.RemindIn = null;
                                        }
                                        objspace.CommitChanges();
                                    }
                                }
                                else
                                {
                                    IObjectSpace objspace1 = Application.CreateObjectSpace();
                                    ICMAlert objdisp1 = objspace1.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + objext.Itemname.items + "(" + objext.Itemname.ItemCode + ")"));
                                    if (objdisp1 == null)
                                    {
                                        IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                                        ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                        obj1.Subject = "Low Stock - " + objext.Itemname.items + "(" + objext.Itemname.ItemCode + ")";
                                        obj1.StartDate = DateTime.Now;
                                        obj1.DueDate = DateTime.Now.AddDays(7);
                                        obj1.RemindIn = TimeSpan.FromMinutes(5);
                                        obj1.Description = "Nice";
                                        space.CommitChanges();
                                    }
                                }
                                  ((ASPxGridListEditor)((ListView)View).Editor).UnselectRowByKey(objext.Oid);
                            }
                        }
                        objreq.Items.Clear();
                        NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                        module.ShowNotificationsWindow = false;
                        module.NotificationsService.Refresh();
                        ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                        foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                        {
                            if (parent.Id == "ICM")
                            {
                                foreach (ChoiceActionItem child in parent.Items)
                                {
                                    if (child.Id == "Operations")
                                    {
                                        foreach (ChoiceActionItem subchild in child.Items)
                                        {
                                            //if (subchild.Id == "ConsumptionItems")
                                            //{
                                            //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            //    var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[ConsumptionBy] Is Null And [ConsumptionDate] Is Null And [Status] = 'PendingConsume' And ([ExpiryDate] Is Null or [ExpiryDate] > ?)", DateTime.Now));
                                            //    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            //    if (count > 0)
                                            //    {
                                            //        subchild.Caption = cap[0] + " (" + count + ")";
                                            //    }
                                            //    else
                                            //    {
                                            //        subchild.Caption = cap[0];
                                            //    }
                                            //}
                                            if (subchild.Id == "DisposalItems")
                                            {
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[DisposedBy] Is Null And [DisposedDate] Is Null And [ExpiryDate] <= ?", DateTime.Now));
                                                var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                if (count > 0)
                                                {
                                                    subchild.Caption = cap[0] + " (" + count + ")";
                                                }
                                                else
                                                {
                                                    subchild.Caption = cap[0];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //objes.ISnew = true;                        
                        //LTBarcodeReportES.Category = "Unspecified";
                        //LTPreviewReportES.Category = "Unspecified";
                        IObjectSpace objspace2 = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace2, typeof(Distribution));
                        cs.Criteria["Filter"] = new InOperator("ESID", objes.Esidlist);
                        Frame.SetView(Application.CreateListView("Distribution_ListView_Existingstockview", cs, false));
                        //View.Refresh();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "shippingnull"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Existingstockview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
                cs.Criteria["Filter"] = CriteriaOperator.Parse("[ESID] Is Not NULL");
                Frame.SetView(Application.CreateListView("Distribution_ListView_Existingstockview", cs, false));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
