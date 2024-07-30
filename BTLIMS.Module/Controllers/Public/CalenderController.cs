using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Scheduler.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CalenderController : ObjectViewController<ListView, ReminderActivity>
    {
        MessageTimer timer = new MessageTimer();
        //SchedulerColorSchema objColorSchema;
        List<System.Drawing.Color> SchemaColor;
        List<Tuple<DateTime, System.Drawing.Color>> activityColor;
        public CalenderController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();

                if (SchemaColor == null)
                {
                    SchemaColor = new List<System.Drawing.Color>();
                    SchemaColor.Add(System.Drawing.Color.DarkRed);
                    SchemaColor.Add(System.Drawing.Color.Green);
                    SchemaColor.Add(System.Drawing.Color.Blue);
                    SchemaColor.Add(System.Drawing.Color.DarkCyan);
                    SchemaColor.Add(System.Drawing.Color.OrangeRed);
                    SchemaColor.Add(System.Drawing.Color.DeepPink);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

            // Perform various tasks depending on the target View.

        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                ASPxSchedulerListEditor listEditor = View.Editor as ASPxSchedulerListEditor;
                if (listEditor != null)
                {
                    DevExpress.Web.ASPxScheduler.ASPxScheduler scheduler = listEditor.SchedulerControl as DevExpress.Web.ASPxScheduler.ASPxScheduler;
                    if (scheduler != null)
                    {
                        scheduler.WeekView.Enabled = false;
                        scheduler.WorkWeekView.Enabled = false;
                        scheduler.DayView.Enabled = false;
                        //scheduler.Views.DayView.VisibleTime =
                        //new TimeOfDayInterval(new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0));
                        //scheduler.ActiveViewChanging += Scheduler_ActiveViewChanging;
                        //heduler.ActiveViewChanged += Scheduler_ActiveViewChanged;
                        //heduler.InitAppointmentDisplayText += Scheduler_InitAppointmentDisplayText;
                        //scheduler.HtmlTimeCellPrepared += Scheduler_HtmlTimeCellPrepared;
                        //heduler.CustomizeElementStyle += Scheduler_CustomizeElementStyle;
                        //scheduler.ResourceColorSchemas.Add(new DevExpress.Web.ASPxScheduler.SchedulerColorSchema(System.Drawing.Color.FromArgb(255, 255, 213)));
                        //ASPxSchedulerStorage objStorage = scheduler.Storage as ASPxSchedulerStorage;
                        //ASPxAppointmentStorage objAppointment = objStorage.Appointments as ASPxAppointmentStorage;
                        //scheduler.OptionsBehavior.ShowViewVisibleInterval = false;
                        scheduler.AppointmentViewInfoCustomizing += Scheduler_AppointmentViewInfoCustomizing;
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


        private void Scheduler_CustomizeElementStyle(object sender, CustomizeElementStyleEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Scheduler_HtmlTimeCellPrepared(object handler, ASPxSchedulerTimeCellPreparedEventArgs e)
        {
            //int w = e.Cell.ColumnSpan;
            //var r = e.Cell.Width;
            //e.Cell.Width = new System.Web.UI.WebControls.Unit(1000);

            //e.Cell.Height = new System.Web.UI.WebControls.Unit(500);
            //e.Cell.Width = new System.Web.UI.WebControls.Unit(700);

            // e.Cell.CssClass = "dx-scheduler-cell-sizes-horizontal";

            //try
            //{

            //    if (e.View  != null && e.View is DevExpress.Web.ASPxScheduler.MonthView)
            //    {




            //    }
            //}
            //catch (Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}

        }

        private void Scheduler_InitAppointmentDisplayText(object sender, AppointmentDisplayTextEventArgs e)
        {
            ////e.Text = "xxx";
        }

        private void Scheduler_AppointmentViewInfoCustomizing(object sender, DevExpress.Web.ASPxScheduler.AppointmentViewInfoCustomizingEventArgs e)
        {
            try
            {
                //e.ViewInfo.Appearance.BackColor = schema.Cell;
                //e.ViewInfo.AppointmentStyle.BackColor = System.Drawing.Color.AliceBlue;
                //e.ViewInfo.AppointmentStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                //e.ViewInfo.ShowBell = true;
                //e.ViewInfo.StatusDisplayType = AppointmentStatusDisplayType.Bounds;
                DevExpress.Web.ASPxScheduler.ASPxScheduler scheduler = sender as DevExpress.Web.ASPxScheduler.ASPxScheduler;
                //scheduler.WorkWeekView.Enabled = false;
                scheduler.DayView.VisibleTime = new TimeOfDayInterval(new TimeSpan(0, 0, 0), new TimeSpan(1, 0, 0));
                ASPxSchedulerStorage objStorage = scheduler.Storage as ASPxSchedulerStorage;
                ASPxAppointmentStorage objAppointment = objStorage.Appointments as ASPxAppointmentStorage;
                if (activityColor == null)
                {
                    activityColor = new List<Tuple<DateTime, System.Drawing.Color>>();
                    if (objAppointment != null)
                    {
                        int countColor = -1;
                        foreach (var app in objAppointment.Items.ToList().OrderBy(x => x.End).Select(x => x.End).Distinct())
                        {
                            if (countColor == -1 && SchemaColor != null)
                            {
                                countColor = SchemaColor.Count - 1;
                            }
                            if (app != null)
                            {
                                activityColor.Add(Tuple.Create(app.Date, SchemaColor[countColor]));
                                countColor = countColor - 1;

                            }
                        }
                    }

                }
                DevExpress.Web.ASPxScheduler.Drawing.AppointmentViewInfo objInfo = e.ViewInfo as DevExpress.Web.ASPxScheduler.Drawing.AppointmentViewInfo;
                if (objInfo != null)
                {
                    ReminderActivity myEvent = this.ObjectSpace.GetObjectByKey<ReminderActivity>(objInfo.Appointment.Id);
                    if (myEvent != null)
                    {
                        e.ViewInfo.ShowStartTime = false;
                        e.ViewInfo.ShowEndTime = false;
                        if (myEvent.EndOn < DateTime.Today)
                        {
                            e.ViewInfo.AppointmentStyle.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {

                            foreach (var i in activityColor)
                            {
                                if (i.Item1.Date == myEvent.EndOn.Date)
                                {
                                    e.ViewInfo.AppointmentStyle.ForeColor = i.Item2;
                                    break;
                                }
                            }
                        }


                    }
                }
                if (scheduler.ActiveViewType == SchedulerViewType.Month)
                {
                    e.ViewInfo.AppointmentStyle.Font.Size = FontUnit.XSmall;
                    e.ViewInfo.AppointmentStyle.VerticalAlign = VerticalAlign.Top;
                    e.ViewInfo.AppointmentStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Scheduler_ActiveViewChanged(object sender, EventArgs e)
        {
            try
            {
                //ASPxSchedulerListEditor listEditor = sender as ASPxSchedulerListEditor;
                //DevExpress.Web.ASPxScheduler.ASPxScheduler scheduler = sender as DevExpress.Web.ASPxScheduler.ASPxScheduler;
                //if (scheduler != null)
                //{

                //    if (scheduler.ActiveViewType == SchedulerViewType.Day)
                //    {
                //       //scheduler.Start = DateTime.Today.AddDays(-1);

                //    }
                //    else if (scheduler.ActiveViewType == SchedulerViewType.Week)
                //    {
                //       // scheduler.Start = DateTime.Today.AddDays(-1);
                //    }
                //    else if (scheduler.ActiveViewType == SchedulerViewType.Month)
                //    {

                //    }

                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Scheduler_ActiveViewChanging(object sender, DevExpress.Web.ASPxScheduler.ActiveViewChangingEventArgs e)
        {
            try
            {
                //    ASPxSchedulerListEditor listEditor = View.Editor as ASPxSchedulerListEditor;
                //    if (listEditor != null)
                //    {
                //        DevExpress.Web.ASPxScheduler.ASPxScheduler scheduler = listEditor.SchedulerControl as DevExpress.Web.ASPxScheduler.ASPxScheduler;

                //        if (e.NewView.Type == SchedulerViewType.Day)
                //        {
                //            Application.ShowViewStrategy.ShowMessage("Day");
                //        }
                //        else if (e.NewView.Type == SchedulerViewType.Week)
                //        {
                //            Application.ShowViewStrategy.ShowMessage("Week");
                //        }
                //        else if (e.NewView.Type == SchedulerViewType.Month)
                //        {
                //            Application.ShowViewStrategy.ShowMessage("Month");
                //        }

                //    }
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
