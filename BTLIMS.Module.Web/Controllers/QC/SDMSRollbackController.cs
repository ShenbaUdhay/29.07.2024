using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Web.Controllers.QC
{
    public partial class SDMSRollbackController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        public SDMSRollbackController()
        {
            InitializeComponent();
            TargetViewId = "SDMSRollback_ListView";
            SDMSRollbackDateFilterAction.TargetViewId = "SDMSRollback_ListView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            SDMSRollbackDateFilterAction.SelectedItemChanged += SDMSRollbackDateFilterAction_SelectedItemChanged;
            qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-1);
            if (SDMSRollbackDateFilterAction.SelectedItem == null)
            {
                SDMSRollbackDateFilterAction.SelectedItem = SDMSRollbackDateFilterAction.Items[0];
            }
        }

        private void SDMSRollbackDateFilterAction_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && SDMSRollbackDateFilterAction != null && SDMSRollbackDateFilterAction.SelectedItem != null && View.Id == "SDMSRollback_ListView")
                {
                    if (SDMSRollbackDateFilterAction.SelectedItem.Id == "1M")
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    }
                    else if (SDMSRollbackDateFilterAction.SelectedItem.Id == "3M")
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-3);
                    }
                    else if (SDMSRollbackDateFilterAction.SelectedItem.Id == "6M")
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-6);
                    }
                    else if (SDMSRollbackDateFilterAction.SelectedItem.Id == "1Y")
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddYears(-1);
                    }
                    else
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.MinValue;
                    }

                    if (qcbatchinfo.qcFilterByMonthDate != DateTime.MinValue)
                    {
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[RollbackDate] BETWEEN('" + qcbatchinfo.qcFilterByMonthDate + "', '" + DateTime.Now + "')");
                    }
                    else
                    {
                        if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey("dateFilter"))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"].IsNull();
                        }
                    }
                   ((DevExpress.ExpressApp.ListView)View).Refresh();
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
    }
}
