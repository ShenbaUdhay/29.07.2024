using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class HideAutofilterRowViewController : ViewController<ListView>
    {
        MessageTimer timer = new MessageTimer();
        public HideAutofilterRowViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View != null && View is ListView)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null)
                    {
                        gridlisteditor.Grid.Settings.ShowFilterRow = false;
                        #region Date Format in Miltary Date
                        foreach (WebColumnBase column in gridlisteditor.Grid.VisibleColumns)
                        {
                            if (column.GetType() == typeof(DevExpress.Web.GridViewDataDateColumn) && ((DevExpress.Web.GridViewDataDateColumn)column).PropertiesEdit.DisplayFormatString == "{0:d}")
                            {
                                gridlisteditor.Grid.DataColumns[column.ToString()].PropertiesEdit.DisplayFormatString = "MM/dd/yyyy";
                            }
                            else if (column.GetType() == typeof(DevExpress.Web.GridViewDataDateColumn) && ((DevExpress.Web.GridViewDataDateColumn)column).PropertiesEdit.DisplayFormatString == "MM/dd/yy HH:mm")
                            {
                                gridlisteditor.Grid.DataColumns[column.ToString()].PropertiesEdit.DisplayFormatString = "MM/dd/yyyy HH:mm";
                            }
                            else if (column.GetType() == typeof(DevExpress.Web.GridViewDataDateColumn) && column.Caption != "CollectedDate" && View.Id != "SampleLogIn_ListView_Copy_SampleRegistration")
                            {
                                gridlisteditor.Grid.DataColumns[column.ToString()].PropertiesEdit.DisplayFormatString = "MM/dd/yyyy HH:mm";
                            }
                            if (!column.Name.Contains("Edit") && !column.Name.Contains("Command") && !string.IsNullOrWhiteSpace(column.Caption))
                            {
                                column.Caption = column.Caption.Replace(" ", string.Empty);
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
