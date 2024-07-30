using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Web;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class HideActionViewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        ChooseThemeController chooseTheme;
        WebNewObjectViewController quickCreate;
        #endregion

        #region Constructor
        public HideActionViewController()
        {
            InitializeComponent();
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                chooseTheme = Frame.GetController<ChooseThemeController>();
                quickCreate = Frame.GetController<WebNewObjectViewController>();
                //chooseTheme.ChooseThemeAction.Active.SetItemValue("HideChooseTheme", false);
                quickCreate.QuickCreateAction.Active.SetItemValue("HideQuickCreate", false);
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
                if (View.Id == "Labware_ListView" || View.Id == "Labware_ListView_Instrument" || View.Id == "Labware_ListView_Balance_Instrument" || View.Id == "SamplePrepBatch_Instruments_ListView" || View.Id == "Labware_LookupListView")
                {
                    Frame.GetController<DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentController>().DownloadAction.Active["ShowDownloadAction"] = false;
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
                if (View.Id == "Labware_ListView")
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    if (e.CommandCellType == GridViewTableCommandCellType.Data)
                    {
                        if (e.CommandColumn.Name == "Download")
                        {
                            string status = gridView.GetRowValuesByKeyValue(e.KeyValue, "Status").ToString();
                            if (status != null && status == "Processing")
                            {
                                ////e.Cell.Controls[0].Visible = false;
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

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        #endregion
    }
}
