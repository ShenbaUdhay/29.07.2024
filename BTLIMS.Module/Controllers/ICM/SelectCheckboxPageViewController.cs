using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Linq;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SelectCheckboxPageViewController : ViewController<ListView>
    {
        MessageTimer timer = new MessageTimer();

        public SelectCheckboxPageViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.IsBatchMode == true)
                {
                    gridListEditor.Grid.PageIndexChanged += Grid_PageIndexChanged;
                    ((ASPxGridViewContextMenu)gridListEditor.ContextMenuTemplate).ControlsCreated += SelectCheckboxPageViewController1cs_ControlsCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (View.Id != "SampleLogIn_ListView_Copy_SampleRegistration" || View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
                {
                    ((ASPxGridView)sender).PageIndexChanged -= Grid_PageIndexChanged;
                    ((ASPxGridView)sender).Selection.UnselectAll();
                }


            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SelectCheckboxPageViewController1cs_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                ((ASPxGridViewContextMenu)sender).ControlsCreated -= SelectCheckboxPageViewController1cs_ControlsCreated;
                //foreach (GridViewColumn column in ((ASPxGridListEditor)View.Editor).Grid.Columns)
                //{
                //    if (column is GridViewCommandColumn && ((GridViewCommandColumn)column).ShowSelectCheckbox)
                //    {
                //        ((GridViewCommandColumn)column).SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
                //        break;
                //    }
                //}
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    GridViewCommandColumn selectionBoxColumn = gridListEditor.Grid.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                    if (selectionBoxColumn != null)
                    {
                        selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
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
        }
    }
}
