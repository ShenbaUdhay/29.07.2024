using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace BTLIMS.Module.Controllers.Dashboard
{
    public partial class SortViewController : ViewController<ListView>
    {
        MessageTimer timer = new MessageTimer();
        #region Constructor
        public SortViewController()
        {
            InitializeComponent();
            TargetViewId = "TodayJobID_ListView";
        }
        #endregion
        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                foreach (IModelColumn column in View.Model.Columns)
                {
                    if (View != null && View.Id == "TodayJobID_ListView")
                    {
                        if (column.PropertyName == "JobID")
                        {
                            column.SortIndex = 0;
                            column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                        }
                        else if (column.PropertyName == "SampleNo")
                        {
                            column.SortIndex = 1;
                            column.SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                        }
                        else
                        {
                            column.SortIndex = -1;
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
