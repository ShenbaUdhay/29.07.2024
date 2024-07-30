using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Scheduler.Web;
using DevExpress.Web.ASPxScheduler;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Linq;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CalendarTabViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public CalendarTabViewController()
        {
            InitializeComponent();
            TargetViewId = "ReminderActivity_ListView;"+ "TaskRecurranceSetup_ListView;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "ReminderActivity_ListView")
                {
                    DateTime dtStartDate = DateTime.Now;
                    foreach (ReminderActivity objAct in ((ListView)View).CollectionSource.List.Cast<ReminderActivity>().ToList())
                    {
                        if (objAct.StartOn.Date != objAct.EndOn.Date)
                        {
                            dtStartDate = objAct.StartOn;
                            do
                            {
                                dtStartDate = dtStartDate.AddDays(1);
                                ReminderActivity objActivity = ObjectSpace.CreateObject<ReminderActivity>();
                                objActivity.StartOn = dtStartDate;
                                objActivity.Subject = objAct.Subject;
                                objActivity.EndOn = objAct.EndOn;
                                objActivity.Location = objAct.Location;
                                objActivity.CreatedBy = objAct.CreatedBy;
                                objActivity.Description = objAct.Description;
                                ((ListView)View).CollectionSource.Add(objActivity);
                            }
                            while (dtStartDate.Date != objAct.EndOn.Date);
                            View.Refresh();
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
            try
            {
                base.OnViewControlsCreated();
                if ((View.Id == "ReminderActivity_ListView" || View.Id == "TaskRecurranceSetup_ListView") && ((ListView)View).Editor != null && ((ListView)View).Editor is ASPxSchedulerListEditor)
                {
                    ASPxSchedulerListEditor objEditor = (ASPxSchedulerListEditor)((ListView)View).Editor;
                    if (objEditor != null)
                    {
                        ASPxScheduler objScheduler = objEditor.SchedulerControl;
                        objScheduler.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Month;
                        //objScheduler.Init += ObjScheduler_Init
                        //objScheduler.DayView.Enabled = false;

                        //objScheduler.WorkWeekView.Enabled = false;
                        //objScheduler.WeekView.Enabled = false;
                        //objScheduler.MonthView.Enabled = true;
                        //objScheduler.TimelineView.Enabled = true;
                        //objScheduler.AgendaView.Enabled = true;
                        //objScheduler.MonthView.Enabled = true;
                        ////objScheduler.DayView.NavigationButtonVisibility = DevExpress.XtraScheduler.NavigationButtonVisibility.Never;
                        ////objScheduler.OptionsBehavior.ShowViewSelector = false;
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
