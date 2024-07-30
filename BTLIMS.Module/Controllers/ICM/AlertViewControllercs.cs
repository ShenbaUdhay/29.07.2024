using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Linq;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AlertViewControllercs : ViewController
    {
        MessageTimer timer = new MessageTimer();
        requisitionquerypanelinfo obj = new requisitionquerypanelinfo();
        public AlertViewControllercs()
        {
            InitializeComponent();
            TargetViewId = "Distribution_ListView_Copy_ExpirationAlert;" + "Items_ListView_Copy_StockAlert;" + "Items_ListView_Copy_StockWatch;";
            OrderAction.TargetViewId = "Items_ListView_Copy_StockAlert;";
            // + "NotificationsObject_Notifications_ListView";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            //ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
            //tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
            // Perform various tasks depending on the target View.
        }

        //private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        //{
        //    if (View.Id == "NotificationsObject_Notifications_ListView") {
        //        Notification iCMAlert = (Notification)e.InnerArgs.CurrentObject;
        //        string[] icm = iCMAlert.Subject.Split(new[] { " - " }, StringSplitOptions.None);
        //        IObjectSpace objspace = Application.CreateObjectSpace();
        //        CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
        //        cs.Criteria["Filtervendor"] = CriteriaOperator.Parse("[LT]=?", icm[1]);
        //        e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateListView("Distribution_ListView", cs,true);
        //        e.Handled = true;
        //    }
        //}

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                DateTime TodayDate = DateTime.Now;
                TodayDate = TodayDate.AddDays(7);

                if (View != null && View.Id == "Distribution_ListView_Copy_ExpirationAlert")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 'PendingDispose' OR [Status] == 'PendingConsume') And [ExpiryDate] <= ?", TodayDate);
                    //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("LT IS NOT NULL and  ExpiryDate<=?", TodayDate);
                }
                if (View != null && View.Id == "Items_ListView_Copy_StockAlert")
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[StockQty] <= [AlertQty]");

                }
                if (View != null && View.Id == "Items_ListView_Copy_StockWatch")
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[StockQty] > 0");

                }
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ASPxGridView gv = gridListEditor.Grid;
                //gv.Columns[0].Visible = false;
                if (gridListEditor.Grid != null)
                {
                    gridListEditor.Grid.Load += Grid_Load;
                }

                // Access and customize the target View control.
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
        }

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Distribution_ListView_Copy_ExpirationAlert" || View.Id == "Items_ListView_Copy_StockAlert" || View.Id == "Items_ListView_Copy_StockWatch")
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                    selectionBoxColumn.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void OrderAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Items_ListView_Copy_StockAlert")
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(Requisition));
                    Items objitems = (Items)e.CurrentObject;
                    obj.Items.Add(objitems.items);
                    obj.fromStockAlert = true;
                    obj.SelectedItem = objitems;
                    //Requisition objReq = os.CreateObject<Requisition>();
                    //objReq.Item = os.GetObject<Items>(objitems);
                    //if (objitems.Vendor != null)
                    //{
                    //    objReq.Vendor = os.GetObjectByKey<Vendors>(objitems.Vendor.Oid);
                    //}
                    //if (objitems.Manufacturer != null)
                    //{
                    //    objReq.Manufacturer = os.GetObjectByKey<Modules.BusinessObjects.ICM.Manufacturer>(objitems.Manufacturer.Oid);
                    //}
                    //objReq.UnitPrice = objitems.UnitPrice;
                    //if (objitems.VendorCatName != null)
                    //{
                    //    objReq.Catalog = objitems.VendorCatName;
                    //}
                    //IList<Employee> obj1 = os.GetObjects<Employee>(CriteriaOperator.Parse("Oid = ?", objReq.RequestedBy.Oid));
                    //foreach (var rec in obj1)
                    //{
                    //    if (rec.Department != null)
                    //    {
                    //        objReq.department = rec.Department;
                    //    }
                    //}
                    //objReq.ExpPrice = Math.Round(objReq.OrderQty * objitems.UnitPrice, 2, MidpointRounding.ToEven);
                    CollectionSource cs = new CollectionSource(os, typeof(Requisition));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([RQID])");
                    //cs.Add(objReq);
                    Frame.SetView(Application.CreateListView("Requisition_ListViewEntermode", cs, true));
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
