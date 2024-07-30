using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DecimalPlaceViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public DecimalPlaceViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "Requisition_DetailView;" + "Items_ListView;" + "Items_DetailView;" + "Requisition_ListViewEntermode;" + "Requisition_ListView_Purchaseorder_Mainview";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            try
            {
                if (View is DetailView)
                {
                    foreach (var item in ((DetailView)View).GetItems<ASPxDoublePropertyEditor>())
                    {
                        if (item.ViewEditMode == ViewEditMode.Edit)
                        {
                            ASPxSpinEdit spinEdit = item.Editor;
                            if (spinEdit != null)
                            {
                                spinEdit.DecimalPlaces = 2;
                            }

                        }
                    }
                }
                if (View is ListView)
                {
                    ASPxGridView gridView = ((ListView)View).Editor.Control as ASPxGridView;

                    GridViewColumn column = null;
                    if (View != null && View.Id == "Requisition_ListView_Purchaseorder_Mainview")
                    {
                        column = (GridViewColumn)gridView.Columns["TotalPrice"];
                    }
                    else
                    {
                        column = (GridViewColumn)gridView.Columns["UnitPrice"];
                    }
                    if (column is GridViewDataSpinEditColumn)
                    {
                        var col = (GridViewDataSpinEditColumn)column;
                        col.PropertiesSpinEdit.DecimalPlaces = 2;
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
        }
    }
}
