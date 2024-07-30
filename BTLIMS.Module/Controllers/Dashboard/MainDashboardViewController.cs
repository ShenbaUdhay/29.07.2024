using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using System;
using System.Drawing;

namespace BTLIMS.Module.Controllers.Dashboard
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class MainDashboardViewController : ViewController
    {
        #region Constructor
        public MainDashboardViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "MainDashBoard;" + "TodayJobID_ListView;" + "UpComingDeliveries_ListView;";
        }
        #endregion
        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                // Perform various tasks depending on the target View.  
                if (View != null && View is DashboardView)
                {
                    foreach (StaticTextViewItem item in ((DashboardView)View).GetItems<StaticTextViewItem>())
                    {
                        item.ControlCreated += item_ControlCreated;
                    }
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 1500, InformationPosition.Top);
            }

        }
        void item_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                StaticTextViewItem item = (StaticTextViewItem)sender;
                item.Label.Appearance.Font = new Font(item.Label.Font.FontFamily, 15);
                item.Label.Appearance.BackColor = Color.Transparent;
                item.Label.Appearance.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 1500, InformationPosition.Top);
            }
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
            try
            {
                if (View != null && View is DashboardView)
                {
                    foreach (StaticTextViewItem item in ((DashboardView)View).GetItems<StaticTextViewItem>())
                    {
                        item.ControlCreated -= item_ControlCreated;
                    }
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 1500, InformationPosition.Top);
            }
        }
        #endregion
        #region Events
        private void MainDashboardViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View is ListView)
                {

                    if (View.Id == "TodayJobID_ListView")
                    {
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[JobID.RecievedDate] BETWEEN ('" + DateTime.Now.Date + "','" + DateTime.Now + "')");
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[RecievedDate] BETWEEN ('" + DateTime.Now.Date + "','" + DateTime.Now + "')");

                    }
                    if (View.Id == "UpComingDeliveries_ListView")
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DueDate] BETWEEN ('" + DateTime.Now.Date + "','" + DateTime.Now.AddDays(3) + "')");

                    }

                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 1500, InformationPosition.Top);
            }
        }
        #endregion
    }
}
