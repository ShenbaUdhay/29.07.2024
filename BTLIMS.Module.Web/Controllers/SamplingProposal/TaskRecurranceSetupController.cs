using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Scheduler.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Web;
using DevExpress.Web.ASPxScheduler.Controls;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraScheduler;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;

namespace LDM.Module.Web.Controllers.SamplingProposal
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TaskRecurranceSetupController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        List<Tuple<DateTime, System.Drawing.Color>> activityColor;

        public TaskRecurranceSetupController()
        {
            InitializeComponent();
            TargetViewId = "COCSettings_DetailView_Copy_SampleRegistration;" + "TaskRecurranceSetup_DetailView;" + "TaskRecurranceSetup_ListView;" + "TaskSchedulerEventList_ListView_Copy;";
            TaskScheduler.TargetViewId = "COCSettings_DetailView_Copy_SampleRegistration";
            Save.TargetViewId = "TaskRecurranceSetup_DetailView;";
            Save.Executed += Save_Executed;
            Save.ConfirmationMessage = "Do you want to Save?";
            TaskAgenda.TargetViewId = "TaskRecurranceSetup_ListView;";

        }

       

        protected override void OnActivated()
        {
            base.OnActivated();
            if(View.Id == "TaskRecurranceSetup_DetailView")
            {
                View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            }
            ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                //if (e.PropertyName == "StartOn")
                //{
                //    TaskRecurranceSetup taskRecurrance = View.CurrentObject as TaskRecurranceSetup;
                //    if(taskRecurrance.StartOn == DateTime.Today || taskRecurrance.StartOn < DateTime.Today)
                //    {
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "StartoN"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                //        return;

                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        }
        }

        private void PopupControl_CustomizePopupWindowSize(object sender, CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if(e.PopupFrame != null &&e.PopupFrame.View.Id  == "TaskSchedulerEventList_ListView")
                {
                    e.Width = 1320;
                    e.Height = 700;
                    e.Handled = true;
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
            if (View!= null && View.Id == "TaskRecurranceSetup_DetailView")
            {
                PropertyEditor recurrenceEditor = FindRecurrencePropertyEditor();
                if (recurrenceEditor != null)
                {
                    ASPxSchedulerRecurrenceInfoPropertyEditor propertyEditor = (ASPxSchedulerRecurrenceInfoPropertyEditor)recurrenceEditor;
                    ASPxSchedulerRecurrenceInfoEdit editor = (ASPxSchedulerRecurrenceInfoEdit)propertyEditor.Editor;
                    if(editor!=null)
                    {
                        editor.RecurrencePopupControlCreated += Editor_RecurrencePopupControlCreated;
                        AppointmentRecurrenceControl recurrenceControl = editor.RecurrenceControl;
                    }
                 
                }
            }
            if (View != null && View.Id == "TaskRecurranceSetup_ListView")
            {
                ASPxSchedulerListEditor listEditor = ((ListView)View).Editor as ASPxSchedulerListEditor;
                if (listEditor != null)
                {
                    DevExpress.Web.ASPxScheduler.ASPxScheduler scheduler = listEditor.SchedulerControl as DevExpress.Web.ASPxScheduler.ASPxScheduler;
                    if (scheduler != null)
                    {
                                              
                        scheduler.AppointmentViewInfoCustomizing += Scheduler_AppointmentViewInfoCustomizing;
                    }

                }
            }
        }

        private void Scheduler_AppointmentViewInfoCustomizing(object sender, DevExpress.Web.ASPxScheduler.AppointmentViewInfoCustomizingEventArgs e)
        {
            DevExpress.Web.ASPxScheduler.Drawing.AppointmentViewInfo objInfo = e.ViewInfo as DevExpress.Web.ASPxScheduler.Drawing.AppointmentViewInfo;
            if (objInfo != null)
            {
           
                TaskRecurranceSetup myEvent = this.ObjectSpace.GetObjectByKey<TaskRecurranceSetup>(objInfo.Appointment.RecurrencePattern.Id);
                if (myEvent != null)
                {                   
                    RecurrenceInfo info = new RecurrenceInfo();
                    info.FromXml(myEvent.RecurrenceInfoXml);
                    // if (myEvent.EndOn < DateTime.Today || myEvent.EndOn.Date == DateTime.Today.Date)
                    if(e.ViewInfo.Appointment.Start  == DateTime.Today)
                    {
                        e.ViewInfo.AppointmentStyle.ForeColor = System.Drawing.Color.DarkRed;
                    }
                    DateTime today = DateTime.Today;
                    IEnumerable<DateTime> selectedDays = Enumerable.Range(1, 7).Select(offset => today.AddDays(offset));
                    List<DateTime> selectedDaysList = selectedDays.ToList();

                    if(selectedDaysList.Contains(e.ViewInfo.Appointment.Start))
                    {
                        e.ViewInfo.AppointmentStyle.ForeColor = System.Drawing.Color.Orange;
                    }

                    //if (Convert.ToInt32(info.Type) == 0)
                    //{
                    //    e.ViewInfo.AppointmentStyle.ForeColor = System.Drawing.Color.DarkRed;
                    //}

                    //if (Convert.ToInt32(info.Type)==1)
                    //{
                    // e.ViewInfo.AppointmentStyle.ForeColor = System.Drawing.Color.Orange;
                    //}
            
        }
            }
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void TaskScheduler_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                COCSettings objCOCSetting= (COCSettings)View.CurrentObject;
                DetailView dvTaskRecurrance=null;
                IObjectSpace objSpace = Application.CreateObjectSpace();
                objCOCSetting = objSpace.GetObjectByKey<COCSettings>(objCOCSetting.Oid);
                TaskRecurranceSetup objTaskRecurrance = objSpace.FindObject<TaskRecurranceSetup>(CriteriaOperator.Parse("[COCSettings] =? ", objCOCSetting));
                if (objTaskRecurrance != null)
                {
                     dvTaskRecurrance = Application.CreateDetailView(objSpace, "TaskRecurranceSetup_DetailView", true, objTaskRecurrance);
                }
                else
                {
                    TaskRecurranceSetup objTask = objSpace.CreateObject<TaskRecurranceSetup>();
                    dvTaskRecurrance = Application.CreateDetailView(objSpace, "TaskRecurranceSetup_DetailView", true, objTask);
                    objTask.COCSettings = objCOCSetting;
                    objTask.StartOn = DateTime.Today;
                    objTask.EndOn = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 0);
                    if (objCOCSetting.ProjectID != null)
                    {
                        objTask.Subject = objCOCSetting.ProjectID.ProjectId;
                    }
                    
                }
                dvTaskRecurrance.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(dvTaskRecurrance);
                showViewParameters.CreatedView = dvTaskRecurrance;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.AcceptAction.Active["save"] = false;
                dc.CancelAction.Active["CancelBtn"] = false;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                dc.AcceptAction.ConfirmationMessage = "Do you want to Save?";
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void Save_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                IObjectSpace objSpace = Application.CreateObjectSpace();
                TaskRecurranceSetup objTask =  (TaskRecurranceSetup)View.CurrentObject;
                if (objTask.RecurrenceInfoXml != null)
                {
                    List<DateTime> eventDates = new List<DateTime>();
                    RecurrenceInfo info = new RecurrenceInfo();
                    info.FromXml(objTask.RecurrenceInfoXml);
                    string strValue = info.Type.ToString();
                    DateTime startDate = info.Start;
                    DateTime endDate = info.End;
                    OccurrenceCalculator calculator = OccurrenceCalculator.CreateInstance(info);
                    int occurrenceIndex = 0;
                    DateTime recurrenceEndDate = info.End;
                    DateTime occurrenceStart = calculator.CalcOccurrenceStartTime(occurrenceIndex);

                    List<DateTime> recurringDates = new List<DateTime>();
                    recurringDates.Add(occurrenceStart);

                    while (occurrenceStart.Date < recurrenceEndDate.Date)
                    {
                        occurrenceIndex++;
                        occurrenceStart = calculator.CalcOccurrenceStartTime(occurrenceIndex);
                        recurringDates.Add(occurrenceStart);
                    }
                    IList<TaskSchedulerEventList> objTaskScheduler = objSpace.GetObjects<TaskSchedulerEventList>(CriteriaOperator.Parse("[TaskSchedulerID.Oid] =? ", objTask.Oid));
                    IObjectSpace objSpacenew = Application.CreateObjectSpace();
                    foreach (TaskSchedulerEventList objTaskEvent in objTaskScheduler)
                    {
                        objSpacenew.Delete(objSpacenew.GetObject(objTaskEvent));

                    }
                    objSpacenew.CommitChanges();
                    foreach (DateTime recurringDate in recurringDates)
                    {
                        TaskSchedulerEventList objTaskschedulerEvent = objSpace.CreateObject<TaskSchedulerEventList>();
                        objTaskschedulerEvent.TaskSchedulerID = objSpace.GetObject(objTask);
                        objTaskschedulerEvent.StartDate = recurringDate;
                        objTaskschedulerEvent.RecurranceType = strValue;
                        objTaskschedulerEvent.EndDate = new DateTime(recurringDate.Year, recurringDate.Month, recurringDate.Day, 23, 59, 0);
                        objSpace.CommitChanges();
                    }
                    if (objTask != null)
                    {
                        COCSettings objCOCID = objSpace.FindObject<COCSettings>(CriteriaOperator.Parse("[Oid] =? ", objTask.COCSettings.Oid));
                //        if (objSample.Status == RegistrationStatus.Submitted || objSample.Status == RegistrationStatus.PendingSubmission)
                //        {
                //            objSample.Status = RegistrationStatus.Scheduled;
                //            objSpace.CommitChanges();
                //        }                       
                        TaskJobIDAutomation objTaskAutomation = objSpace.FindObject<TaskJobIDAutomation>(CriteriaOperator.Parse("[COCID] =? ", objTask.COCSettings.Oid));
                          if (objTaskAutomation == null)
                        {
                            TaskJobIDAutomation objTasks = objSpace.CreateObject<TaskJobIDAutomation>();
                            objTasks.COCID = objCOCID;
                            objTasks.DaysinAdvance = 1;
                            objSpace.CommitChanges();
                        }
                }
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
                {
                    (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
                }
                
            }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
       
                private PropertyEditor FindRecurrencePropertyEditor()
                 {
                    try
                    {
                foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "RecurrenceInfoXml"))
                {
                    return (PropertyEditor)item;
                }
                return null; // Return null if the recurrence property editor is not found
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private void Editor_RecurrencePopupControlCreated(object sender, RecurrencePopupControlCreatedEventArgs e)
        {
            try
            {
                ASPxSchedulerRecurrenceInfoEdit editor = (ASPxSchedulerRecurrenceInfoEdit)sender;
                AppointmentRecurrenceControl recurrenceControl = editor.RecurrenceControl;
                recurrenceControl.Load += RecurrenceControl_Load;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void RecurrenceControl_Load(object sender, EventArgs e)
        {
            try
            {
                AppointmentRecurrenceControl recurrenceControl = (AppointmentRecurrenceControl)sender;
                var noEnd = (ASPxRadioButton)recurrenceControl.FindControl("RangeCtl").FindControl("DeNoEnd");
                noEnd.ClientVisible = false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void Save_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ObjectSpace.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TaskAgenda_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objectSpace, typeof(TaskSchedulerEventList));
                ListView listView = Application.CreateListView("TaskSchedulerEventList_ListView", cs,true);
                ShowViewParameters showViewParameters = new ShowViewParameters(listView);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                showViewParameters.CreatedView.Caption = "Scheduled Task Tracking";
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                dc.AcceptAction.Active.SetItemValue("enb", false);
                dc.CancelAction.Active.SetItemValue("enb", false);
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
       
    }
}
