using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.ICM
{
    public partial class IcmsortController : ViewController<ListView>
    {
        MessageTimer timer = new MessageTimer();
        #region Constructor
        public IcmsortController()
        {
            InitializeComponent();
            TargetViewId = "Requisition_ListView;" + "Requisition_ListView_Tracking;" + "Items_ListView_Copy_StockWatch;" + "Items_ListView_Copy_StockAlert;" + "Distribution_ListView_Copy_ExpirationAlert;";
        }
        #endregion

        #region Default Methods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if ((base.View != null && base.View.Id == "Requisition_ListView" || base.View.Id == "Requisition_ListView_Tracking" || View.Id == "Requisition_ListView_History"))
                {
                    foreach (IModelColumn column in View.Model.Columns)
                    {
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                        if (column.PropertyName == "RequestedDate")
                        {
                            column.SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                        }
                    }
                }
                if (base.View != null && base.View.Id == "Items_ListView_Copy_StockWatch")
                {
                    foreach (IModelColumn column in View.Model.Columns)
                    {
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                        if (column.PropertyName == "ItemCode")
                        {
                            column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                        }
                    }
                }
                if (base.View != null && base.View.Id == "Items_ListView_Copy_StockAlert")
                {
                    foreach (IModelColumn column in View.Model.Columns)
                    {
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                        if (column.PropertyName == "ItemCode")
                        {
                            column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                        }
                    }
                }
                if (base.View != null && base.View.Id == "Distribution_ListView_Copy_ExpirationAlert")
                {
                    foreach (IModelColumn column in View.Model.Columns)
                    {
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                        if (column.PropertyName == "ItemCode")
                        {
                            column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
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
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        #endregion
    }
}
