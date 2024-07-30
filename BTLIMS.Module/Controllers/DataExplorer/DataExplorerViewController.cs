using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraGrid.Views.Grid;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace BTLIMS.Module.Controllers.DataExplorer
{
    public partial class DataExplorerViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        #region Constructor
        public DataExplorerViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "DESampleInfo_ListView";
        }
        #endregion
        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        #endregion
        #region Events
        private void DataExplorerViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                if (View != null)
                {
                    if (View is ListView)
                    {
                        GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
                        if (listEditor != null)
                        {
                            Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                            SelectedData sproc = currentSession.ExecuteSproc("SampleInfo_SP");
                            if (sproc.ResultSet[0].Rows[0].Values[0].ToString() != null)
                            {
                                GridView gridView = listEditor.GridView;
                                listEditor.DataSource = sproc.ResultSet[0];
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
        #endregion
    }
}
