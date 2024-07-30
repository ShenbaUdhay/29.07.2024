using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.XtraReports.UI;
using DevExpress.XtraScheduler;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting.Labware;

namespace LDM.Module.Controllers.Settings.Labware
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class LabwareMaintenanceViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        bool msg = false;
        SchedularInfo objS = new SchedularInfo();
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        private Stream newms;
        curlanguage objLanguage = new curlanguage();
        string strLWID = string.Empty;
        public LabwareMaintenanceViewController()
        {
            InitializeComponent();
            TargetViewId = "TaskCheckList_ListView;" + "MaintenanceTaskCheckList_DetailView_MaintenanceQueue;" + "MaintenanceSetup_DetailView;" + "MaintenanceTaskCheckList_ListView_MaintenanceSetup;" + "TaskCheckList_ListView_Copy;"
                 + "MaintenanceTaskCheckList_ListView;" + "MaintenanceTaskCheckList_ListView_MaintenanceLog;" + "MaintenanceTaskCheckList_ListView_MaintenanceQueue;" + "Schedular_DetailView;"+ "Labware_ListView;";
            ChecklistShow.TargetViewId = "MaintenanceTaskCheckList_ListView;";
            ChecklistHide.TargetViewId = "MaintenanceTaskCheckList_ListView;";
            LabwareBarcode.TargetViewId = "Labware_ListView";
            LabwareBarcodeBig.TargetViewId = "Labware_ListView";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "MaintenanceTaskCheckList_DetailView_MaintenanceQueue")
                {
                    MaintenanceTaskCheckList objMTCL = View.CurrentObject as MaintenanceTaskCheckList;
                    objMTCL.MaintainedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    objMTCL.MaintainedDate = DateTime.Now;
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executing += SaveAction_Executing;
                    Frame.GetController<RefreshController>().RefreshAction.Executed += RefreshAction_Executed;
                }
                if (View.Id == "MaintenanceSetup_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Execute += SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Execute += SaveAction_Execute;
                }
                if (View.Id == "Schedular_DetailView" || View.Id == "MaintenanceTaskCheckList_DetailView_MaintenanceQueue")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                #region Scheduler
                if (View.Id == "MaintenanceTaskCheckList_ListView_MaintenanceQueue")
                {
                    List<MaintenanceTaskCheckList> lstMTC = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[MaintenanceStatus] = 'PendingQueue'And [MaintenanceSetup.Active] = True")).ToList();
                    List<string> lststr = new List<string>();
                    List<Guid> lstOid = new List<Guid>();
                    foreach (MaintenanceTaskCheckList objMTC in lstMTC.OrderByDescending(i => i.NextMaintainDate))
                    {

                        if (!lststr.Contains(objMTC.MaintenanceId))
                        {
                            lstOid.Add(objMTC.Oid);
                            lststr.Add(objMTC.MaintenanceId);
                        }
                    }
                    List<MaintenanceTaskCheckList> lstMTC1 = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(new InOperator("Oid", lstOid)).ToList();
                    foreach (MaintenanceTaskCheckList objMTC in lstMTC1)
                    {
                        DateTime DateToMaintain = new DateTime();
                        RecurrenceInfo info = new RecurrenceInfo();
                        info.FromXml(objMTC.RecurrenceInfoXml);
                        DateToMaintain = objMTC.DateToMaintain;
                        objS.NextDate = objMTC.NextMaintainDate;
                        for (int i = 0; i < 1; i++)
                        {
                            if (info.Type == RecurrenceType.Daily)
                            {
                                if (info.WeekDays == DevExpress.XtraScheduler.WeekDays.WorkDays)
                                {
                                    if (objS.NextDate == DateTime.MinValue)
                                    {
                                        if (DateToMaintain.DayOfWeek == DayOfWeek.Friday)
                                        {
                                            objS.NextDate = objMTC.NextMaintainDate = objMTC.DateToMaintain.AddDays(3);
                                            ObjectSpace.CommitChanges();
                                        }
                                        else if (DateToMaintain.DayOfWeek == DayOfWeek.Saturday)
                                        {
                                            DateToMaintain = objMTC.DateToMaintain = objMTC.DateToMaintain.AddDays(2);
                                            objS.NextDate = objMTC.NextMaintainDate = objMTC.DateToMaintain.AddDays(1);
                                            ObjectSpace.CommitChanges();
                                        }
                                        else if (DateToMaintain.DayOfWeek == DayOfWeek.Sunday)
                                        {
                                            DateToMaintain = objMTC.DateToMaintain = objMTC.DateToMaintain.AddDays(1);
                                            objS.NextDate = objMTC.NextMaintainDate = objMTC.DateToMaintain.AddDays(1);
                                            ObjectSpace.CommitChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    if (objS.NextDate == DateTime.MinValue)
                                    {
                                        objS.NextDate = objMTC.NextMaintainDate = objMTC.DateToMaintain.AddDays(info.Periodicity);
                                        ObjectSpace.CommitChanges();
                                    }
                                }
                                if (objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                {
                                    i = -1;
                                    DateToMaintain = objS.NextDate;
                                    if (info.WeekDays == DevExpress.XtraScheduler.WeekDays.WorkDays)
                                    {
                                        if (objS.NextDate.DayOfWeek == DayOfWeek.Friday)
                                        {
                                            objS.NextDate = objS.NextDate.AddDays(3);
                                        }
                                        else
                                        {
                                            objS.NextDate = objS.NextDate.AddDays(1);
                                        }
                                    }
                                    else
                                    {
                                        objS.NextDate = objS.NextDate.AddDays(info.Periodicity);
                                    }
                                    NextRecurrence(objMTC, DateToMaintain);
                                }

                            }
                            else if (info.Type == RecurrenceType.Weekly)
                            {
                                bool Sunday = false;
                                bool Monday = false;
                                bool Tuesday = false;
                                bool Wednesday = false;
                                bool Thursday = false;
                                bool Friday = false;
                                bool Saturday = false;
                                string LastDayofWeek = null;
                                if (objS.NextDate == DateTime.MinValue)
                                {
                                    objS.NextDate = DateToMaintain;
                                }
                                string[] Days = info.WeekDays.ToString().Split(',');
                                foreach (string D in Days)
                                {
                                    string Day = D.Replace(" ", "");
                                    if (Day == "Sunday")
                                    {
                                        Sunday = true;
                                        if (Days[Days.Count() - 1] == D)
                                        {
                                            LastDayofWeek = "Sunday";
                                        }
                                    }
                                    if (Day == "Monday")
                                    {
                                        Monday = true;
                                        if (Days[Days.Count() - 1] == D)
                                        {
                                            LastDayofWeek = "Monday";
                                        }
                                    }
                                    if (Day == "Tuesday")
                                    {
                                        Tuesday = true;
                                        if (Days[Days.Count() - 1] == D)
                                        {
                                            LastDayofWeek = "Tuesday";
                                        }
                                    }
                                    if (Day == "Wednesday")
                                    {
                                        Wednesday = true;
                                        if (Days[Days.Count() - 1] == D)
                                        {
                                            LastDayofWeek = "Wednesday";
                                        }
                                    }
                                    if (Day == "Thursday")
                                    {
                                        Thursday = true;
                                        if (Days[Days.Count() - 1] == D)
                                        {
                                            LastDayofWeek = "Thursday";
                                        }
                                    }
                                    if (Day == "Friday")
                                    {
                                        Friday = true;
                                        if (Days[Days.Count() - 1] == D)
                                        {
                                            LastDayofWeek = "Friday";
                                        }
                                    }
                                    if (Day == "Saturday")
                                    {
                                        Saturday = true;
                                        if (Days[Days.Count() - 1] == D)
                                        {
                                            LastDayofWeek = "Saturday";
                                        }
                                    }
                                }
                                DateTime DT = new DateTime();
                                DateTime DT1 = new DateTime();
                                do
                                {
                                    for (int a = 1; a <= 7; a++)
                                    {
                                        if (objS.NextDate.DayOfWeek == DayOfWeek.Monday && Monday == true && objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                        {
                                            if (objMTC.NextMaintainDate == DateTime.MinValue)
                                            {
                                                if (DT == DateTime.MinValue)
                                                {
                                                    DT = objS.NextDate;
                                                    if (LastDayofWeek == "Monday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    objMTC.DateToMaintain = DT;
                                                    objMTC.NextMaintainDate = objS.NextDate;
                                                    ObjectSpace.CommitChanges();
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Monday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                if (DT1 == DateTime.MinValue)
                                                {
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Monday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    DateToMaintain = DT1;
                                                    NextRecurrence(objMTC, DateToMaintain);
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Monday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }

                                            }
                                        }
                                        else if (objS.NextDate.DayOfWeek == DayOfWeek.Tuesday && Tuesday == true && objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                        {
                                            if (objMTC.NextMaintainDate == DateTime.MinValue)
                                            {
                                                if (DT == DateTime.MinValue)
                                                {
                                                    DT = objS.NextDate;
                                                    if (LastDayofWeek == "Tuesday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    objMTC.DateToMaintain = DT;
                                                    objMTC.NextMaintainDate = objS.NextDate;
                                                    ObjectSpace.CommitChanges();
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Tuesday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (DT1 == DateTime.MinValue)
                                                {
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Tuesday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    DateToMaintain = DT1;
                                                    NextRecurrence(objMTC, DateToMaintain);
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Tuesday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }

                                            }
                                        }
                                        else if (objS.NextDate.DayOfWeek == DayOfWeek.Wednesday && Wednesday == true && objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                        {
                                            if (objMTC.NextMaintainDate == DateTime.MinValue)
                                            {
                                                if (DT == DateTime.MinValue)
                                                {
                                                    DT = objS.NextDate;
                                                    if (LastDayofWeek == "Wednesday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    objMTC.DateToMaintain = DT;
                                                    objMTC.NextMaintainDate = objS.NextDate;
                                                    ObjectSpace.CommitChanges();
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Wednesday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (DT1 == DateTime.MinValue)
                                                {
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Wednesday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    DateToMaintain = DT1;
                                                    NextRecurrence(objMTC, DateToMaintain);
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Wednesday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }

                                                }

                                            }
                                        }
                                        else if (objS.NextDate.DayOfWeek == DayOfWeek.Thursday && Thursday == true && objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                        {
                                            if (objMTC.NextMaintainDate == DateTime.MinValue)
                                            {
                                                if (DT == DateTime.MinValue)
                                                {
                                                    DT = objS.NextDate;
                                                    if (LastDayofWeek == "Thursday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    objMTC.DateToMaintain = DT;
                                                    objMTC.NextMaintainDate = objS.NextDate;
                                                    ObjectSpace.CommitChanges();
                                                    if (LastDayofWeek == "Thursday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                    DT1 = DateTime.MinValue;
                                                }
                                            }
                                            else
                                            {
                                                if (DT1 == DateTime.MinValue)
                                                {
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Thursday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    DateToMaintain = DT1;
                                                    NextRecurrence(objMTC, DateToMaintain);
                                                    if (LastDayofWeek == "Thursday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                    DT1 = DateTime.MinValue;
                                                }

                                            }
                                        }
                                        else if (objS.NextDate.DayOfWeek == DayOfWeek.Friday && Friday == true && objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                        {
                                            if (objMTC.NextMaintainDate == DateTime.MinValue)
                                            {
                                                if (DT == DateTime.MinValue)
                                                {
                                                    DT = objS.NextDate;
                                                    if (LastDayofWeek == "Friday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    objMTC.DateToMaintain = DT;
                                                    objMTC.NextMaintainDate = objS.NextDate;
                                                    ObjectSpace.CommitChanges();
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Friday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                if (DT1 == DateTime.MinValue)
                                                {
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Friday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    DateToMaintain = DT1;
                                                    NextRecurrence(objMTC, DateToMaintain);
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Friday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }

                                                }

                                            }
                                        }
                                        else if (objS.NextDate.DayOfWeek == DayOfWeek.Saturday && Saturday == true && objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                        {
                                            if (objMTC.NextMaintainDate == DateTime.MinValue)
                                            {
                                                if (DT == DateTime.MinValue)
                                                {
                                                    DT = objS.NextDate;
                                                    if (LastDayofWeek == "Saturday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    objMTC.DateToMaintain = DT;
                                                    objMTC.NextMaintainDate = objS.NextDate;
                                                    ObjectSpace.CommitChanges();
                                                    if (LastDayofWeek == "Saturday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                    DT1 = DateTime.MinValue;
                                                }
                                            }
                                            else
                                            {
                                                if (DT1 == DateTime.MinValue)
                                                {
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Saturday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    DateToMaintain = DT1;
                                                    NextRecurrence(objMTC, DateToMaintain);
                                                    if (LastDayofWeek == "Saturday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                    DT1 = DateTime.MinValue;
                                                }

                                            }
                                        }
                                        else if (objS.NextDate.DayOfWeek == DayOfWeek.Sunday && Sunday == true && objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                        {
                                            if (objMTC.NextMaintainDate == DateTime.MinValue)
                                            {
                                                if (DT == DateTime.MinValue)
                                                {
                                                    DT = objS.NextDate;
                                                    if (LastDayofWeek == "Sunday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    objMTC.DateToMaintain = DT;
                                                    objMTC.NextMaintainDate = objS.NextDate;
                                                    ObjectSpace.CommitChanges();
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Sunday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                if (DT1 == DateTime.MinValue)
                                                {
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Sunday")
                                                    {
                                                        a = 1;
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }
                                                }
                                                else
                                                {
                                                    DateToMaintain = DT1;
                                                    NextRecurrence(objMTC, DateToMaintain);
                                                    DT1 = objS.NextDate;
                                                    if (LastDayofWeek == "Sunday")
                                                    {
                                                        objS.NextDate = objS.NextDate.AddDays((info.Periodicity - 1) * 7);
                                                    }

                                                }

                                            }
                                        }

                                        objS.NextDate = objS.NextDate.AddDays(1);

                                    }
                                }
                                while (objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now);
                            }
                            else if (info.Type == RecurrenceType.Monthly)
                            {

                                if (info.WeekOfMonth == WeekOfMonth.None)
                                {
                                    if (objS.NextDate == DateTime.MinValue)
                                    {
                                        objS.NextDate = objMTC.NextMaintainDate = new DateTime(objMTC.StartOn.Year, objMTC.StartOn.Month, info.DayNumber);
                                        if (objMTC.StartOn <= objS.NextDate)
                                        {
                                            objMTC.DateToMaintain = objS.NextDate;
                                            objS.NextDate = objMTC.NextMaintainDate = objS.NextDate.AddMonths(info.Periodicity);
                                            ObjectSpace.CommitChanges();
                                        }
                                        else
                                        {
                                            objS.NextDate = objMTC.NextMaintainDate = objS.NextDate.AddMonths(info.Periodicity);
                                            objMTC.DateToMaintain = objS.NextDate;
                                            objS.NextDate = objMTC.NextMaintainDate = objS.NextDate.AddMonths(info.Periodicity);
                                            ObjectSpace.CommitChanges();
                                        }
                                    }
                                    if (objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                    {
                                        i = -1;
                                        DateToMaintain = objS.NextDate;
                                        objS.NextDate = objS.NextDate.AddMonths(info.Periodicity);
                                        NextRecurrence(objMTC, DateToMaintain);
                                    }
                                }
                                else
                                {
                                    if (objS.NextDate == DateTime.MinValue)
                                    {
                                        if (info.WeekOfMonth == WeekOfMonth.First)
                                        {
                                            DateTime DT = new DateTime();
                                            DT = objS.NextDate = new DateTime(objMTC.StartOn.Year, objMTC.StartOn.Month, 01);
                                            if (objMTC.StartOn >= objS.NextDate)
                                            {
                                                objS.NextDate = objS.NextDate.AddDays(7);
                                                if (objMTC.StartOn >= objS.NextDate)
                                                {
                                                    objS.NextDate = DT.AddMonths(info.Periodicity);
                                                }
                                                else
                                                {
                                                    int day = Convert.ToInt32(objS.NextDate.Day - objMTC.StartOn.Day);
                                                    objS.NextDate = objS.NextDate.AddDays(-day);
                                                }
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Second)
                                        {
                                            DateTime DT = new DateTime();
                                            DT = objS.NextDate = new DateTime(objMTC.StartOn.Year, objMTC.StartOn.Month, 08);
                                            if (objMTC.StartOn >= objS.NextDate)
                                            {
                                                objS.NextDate = objS.NextDate.AddDays(7);
                                                if (objMTC.StartOn >= objS.NextDate)
                                                {
                                                    objS.NextDate = DT.AddMonths(info.Periodicity);
                                                }
                                                else
                                                {
                                                    int day = Convert.ToInt32(objS.NextDate.Day - objMTC.StartOn.Day);
                                                    objS.NextDate = objS.NextDate.AddDays(-day);
                                                }
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Third)
                                        {
                                            DateTime DT = new DateTime();
                                            DT = objS.NextDate = new DateTime(objMTC.StartOn.Year, objMTC.StartOn.Month, 15);
                                            if (objMTC.StartOn >= objS.NextDate)
                                            {
                                                objS.NextDate = objS.NextDate.AddDays(7);
                                                if (objMTC.StartOn >= objS.NextDate)
                                                {
                                                    objS.NextDate = DT.AddMonths(info.Periodicity);
                                                }
                                                else
                                                {
                                                    int day = Convert.ToInt32(objS.NextDate.Day - objMTC.StartOn.Day);
                                                    objS.NextDate = objS.NextDate.AddDays(-day);
                                                }
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                                        {
                                            DateTime DT = new DateTime();
                                            DT = objS.NextDate = new DateTime(objMTC.StartOn.Year, objMTC.StartOn.Month, 22);
                                            if (objMTC.StartOn >= objS.NextDate)
                                            {
                                                objS.NextDate = objS.NextDate.AddDays(7);
                                                if (objMTC.StartOn >= objS.NextDate)
                                                {
                                                    objS.NextDate = DT.AddMonths(info.Periodicity);
                                                }
                                                else
                                                {
                                                    int day = Convert.ToInt32(objS.NextDate.Day - objMTC.StartOn.Day);
                                                    objS.NextDate = objS.NextDate.AddDays(-day);
                                                }
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Last)
                                        {
                                            DateTime DT = new DateTime();
                                            DT = objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                            objS.NextDate = objS.NextDate.AddDays(1);
                                            if (objS.NextDate.Month == objMTC.StartOn.Month)
                                            {
                                                if (objMTC.StartOn >= objS.NextDate)
                                                {
                                                    objS.NextDate = objS.NextDate.AddDays(7);
                                                    if (objMTC.StartOn >= objS.NextDate)
                                                    {
                                                        objS.NextDate = DT.AddMonths(info.Periodicity);
                                                    }
                                                }
                                                else
                                                {
                                                    int day = Convert.ToInt32(objS.NextDate.Day - objMTC.StartOn.Day);
                                                    objS.NextDate = objS.NextDate.AddDays(-day);
                                                }
                                            }
                                            else
                                            {
                                                DT = objS.NextDate = new DateTime(objMTC.StartOn.Year, objMTC.StartOn.Month, 22);
                                                if (objMTC.StartOn >= objS.NextDate)
                                                {
                                                    objS.NextDate = objS.NextDate.AddDays(7);
                                                    if (objMTC.StartOn >= objS.NextDate)
                                                    {
                                                        objS.NextDate = DT.AddMonths(info.Periodicity);
                                                    }
                                                }
                                                else
                                                {
                                                    int day = Convert.ToInt32(objS.NextDate.Day - objMTC.StartOn.Day);
                                                    objS.NextDate = objS.NextDate.AddDays(-day);
                                                }
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                    }
                                    if (objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                    {
                                        i = -1;
                                        DateToMaintain = objS.NextDate;
                                        objS.NextDate = objS.NextDate.AddMonths(info.Periodicity);
                                        if (info.WeekOfMonth == WeekOfMonth.First)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 01);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Second)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 08);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Third)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 15);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 22);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Last)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                            objS.NextDate = objS.NextDate.AddDays(1);
                                            if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                                            {
                                                objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                                            }
                                        }
                                        MonthlyYearly(info, objMTC);
                                        NextRecurrence(objMTC, DateToMaintain);
                                    }
                                }
                            }
                            else if (info.Type == RecurrenceType.Yearly)
                            {
                                if (info.WeekOfMonth == WeekOfMonth.None)
                                {
                                    if (objS.NextDate == DateTime.MinValue)
                                    {
                                        objS.NextDate = objMTC.NextMaintainDate = new DateTime(objMTC.StartOn.Year, info.Month, info.DayNumber);
                                        if (objMTC.StartOn <= objS.NextDate)
                                        {
                                            objMTC.DateToMaintain = objS.NextDate;
                                            objS.NextDate = objMTC.NextMaintainDate = objS.NextDate.AddYears(info.Periodicity);
                                            ObjectSpace.CommitChanges();
                                        }
                                        else
                                        {
                                            objS.NextDate = objMTC.NextMaintainDate = objS.NextDate.AddYears(info.Periodicity);
                                            objMTC.DateToMaintain = objS.NextDate;
                                            objS.NextDate = objMTC.NextMaintainDate = objS.NextDate.AddYears(info.Periodicity);
                                            ObjectSpace.CommitChanges();
                                        }
                                    }
                                    if (objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                    {
                                        i = -1;
                                        DateToMaintain = objS.NextDate;
                                        objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                                        NextRecurrence(objMTC, DateToMaintain);
                                    }
                                }
                                else
                                {
                                    if (objS.NextDate == DateTime.MinValue)
                                    {
                                        if (info.WeekOfMonth == WeekOfMonth.First)
                                        {
                                            objS.NextDate = new DateTime(objMTC.StartOn.Year, info.Month, 01);
                                            if (objMTC.StartOn >= objS.NextDate)
                                            {
                                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Second)
                                        {
                                            objS.NextDate = new DateTime(objMTC.StartOn.Year, info.Month, 08);
                                            if (objMTC.StartOn >= objS.NextDate)
                                            {
                                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Third)
                                        {
                                            objS.NextDate = new DateTime(objMTC.StartOn.Year, info.Month, 15);
                                            if (objMTC.StartOn >= objS.NextDate)
                                            {
                                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                                        {
                                            objS.NextDate = new DateTime(objMTC.StartOn.Year, info.Month, 22);
                                            if (objMTC.StartOn >= objS.NextDate)
                                            {
                                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Last)
                                        {
                                            objS.NextDate = new DateTime(objMTC.StartOn.Year, info.Month, 28);
                                            objS.NextDate = objS.NextDate.AddDays(1);
                                            if (objS.NextDate.Month == info.Month)
                                            {
                                                if (objMTC.StartOn >= objS.NextDate)
                                                {
                                                    objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                                                }
                                            }
                                            else
                                            {
                                                objS.NextDate = new DateTime(objMTC.StartOn.Year, info.Month, 22);
                                                if (objMTC.StartOn >= objS.NextDate)
                                                {
                                                    objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                                                }
                                            }
                                            MonthlyYearly(info, objMTC);
                                        }
                                    }
                                    if (objS.NextDate <= objMTC.EndOn && DateToMaintain <= DateTime.Now)
                                    {
                                        i = -1;
                                        DateToMaintain = objS.NextDate;
                                        objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                                        if (info.WeekOfMonth == WeekOfMonth.First)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 01);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Second)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 08);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Third)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 15);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 22);
                                        }
                                        else if (info.WeekOfMonth == WeekOfMonth.Last)
                                        {
                                            objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                            objS.NextDate = objS.NextDate.AddDays(1);
                                            if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                                            {
                                                objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                                            }
                                        }
                                        MonthlyYearly(info, objMTC);
                                        NextRecurrence(objMTC, DateToMaintain);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                if (View.Id == "MaintenanceTaskCheckList_ListView_MaintenanceLog")
                {
                    IList<MaintenanceTaskCheckList> lstMTC = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[MaintenanceStatus] = 'Maintained'"));
                    List<Guid> lisGuid = new List<Guid>();
                    List<string> lisId = new List<string>();
                    foreach (MaintenanceTaskCheckList objMCT in lstMTC.Distinct())
                    {
                        if (!lisId.Contains(objMCT.MaintenanceId))
                        {
                            lisGuid.Add(objMCT.Oid);
                            lisId.Add(objMCT.MaintenanceId);
                        }
                    }
                    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", lisGuid);
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                if (View.Id == "MaintenanceTaskCheckList_ListView")
                {
                    MaintenanceSetup objMS = Application.MainWindow.View.CurrentObject as MaintenanceSetup;
                    if (objMS != null)
                    {
                        IList<MaintenanceTaskCheckList> objCross = new List<MaintenanceTaskCheckList>();
                        if (objMS.Department != null)
                        {
                            objCross = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse(" [MaintenanceSetup.Oid] = ? And [Department.Oid] = ?", objMS.Oid, objMS.Department.Oid));
                        }
                        else
                        {
                            objCross = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[MaintenanceSetup.Oid] = ?", objMS.Oid));
                        }
                        List<Guid> lisGuid = new List<Guid>();
                        List<string> lstMaintenance = new List<string>();
                        foreach (MaintenanceTaskCheckList objMTC in objCross)
                        {
                            if (!lstMaintenance.Contains(objMTC.MaintenanceId))
                            {
                                lstMaintenance.Add(objMTC.MaintenanceId);
                                lisGuid.Add(objMTC.Oid);
                            }
                        }
                      ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lisGuid);
                    }
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                }
                if (View.Id == "TaskCheckList_ListView_Copy")
                {
                    MaintenanceSetup objMS = Application.MainWindow.View.CurrentObject as MaintenanceSetup;
                    if (objMS != null && objMS.Department != null)
                    {
                        IList<TaskCheckList> objCrossCL = ObjectSpace.GetObjects<TaskCheckList>(CriteriaOperator.Parse("[Department.Oid] = ?", objMS.Department.Oid));
                        IList<MaintenanceTaskCheckList> objCross = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[MaintenanceSetup.Oid] = ?", objMS.Oid));
                        if (objCrossCL.Count > 0)
                        {
                            if (objCross.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria["Filter"] = new GroupOperator(GroupOperatorType.And, (CriteriaOperator.Parse("Not[Oid] In(" + string.Format("'{0}'", string.Join("','", objCross.Select(i => i.TaskChecklist.Oid.ToString().Replace("'", "''")))) + ")")), (CriteriaOperator.Parse("[Department.Oid] = ?", objMS.Department.Oid)));
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Department.Oid] = ?", objMS.Department.Oid);
                            }
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                        }

                    }

                }
                if (View.Id == "MaintenanceTaskCheckList_ListView_MaintenanceSetup")
                {
                    IList<MaintenanceTaskCheckList> objCross = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[MaintenanceStatus] = 'PendingQueue'"));
                    List<Guid> lisGuid = new List<Guid>();
                    List<string> lstMaintenance = new List<string>();
                    foreach (MaintenanceTaskCheckList objMTC in objCross)
                    {
                        if (!lstMaintenance.Contains(objMTC.MaintenanceId))
                        {
                            lstMaintenance.Add(objMTC.MaintenanceId);
                            lisGuid.Add(objMTC.Oid);
                        }
                    }
                    ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lisGuid);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
            if (View.Id == "MaintenanceSetup_DetailView")
            {
                MaintenanceSetup objMS = Application.MainWindow.View.CurrentObject as MaintenanceSetup;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                MaintenanceSetup objMTS1 = objectSpace.GetObject<MaintenanceSetup>(objMS);
                if (objMTS1 != null)
                {
                    IList<MaintenanceTaskCheckList> objMTC = objectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse(" [MaintenanceSetup.Oid] = ? And [Department.Oid] <> ?", objMTS1.Oid, objMTS1.Department.Oid));
                    if (objMTC.Count > 0)
                    {
                        objectSpace.Delete(objMTC);
                        objectSpace.CommitChanges();
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

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
            if (View.Id == "MaintenanceSetup_DetailView" && e.Object != null)
            {
                MaintenanceSetup objMS = (MaintenanceSetup)e.Object;
                if (e.PropertyName == "Department" && e.Object.GetType() == typeof(MaintenanceSetup))
                {

                        if (objMS != null && objMS.Department != null)
                    {
                        IList<MaintenanceTaskCheckList> objCross = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[MaintenanceStatus] = 'PendingQueue' And [MaintenanceSetup.Oid] = ? And [Department.Oid] = ?", objMS.Oid, objMS.Department.Oid));
                        List<Guid> lisGuid = new List<Guid>();
                        List<string> lstMaintenance = new List<string>();
                        foreach (MaintenanceTaskCheckList objMTC in objCross)
                        {
                            if (!lstMaintenance.Contains(objMTC.MaintenanceId))
                            {
                                lstMaintenance.Add(objMTC.MaintenanceId);
                                lisGuid.Add(objMTC.Oid);

                            }
                        }
                        DashboardViewItem listResultview = ((DetailView)Application.MainWindow.View).FindItem("TaskChecklist") as DashboardViewItem;
                        ((ListView)listResultview.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lisGuid);
                        listResultview.InnerView.ObjectSpace.Refresh();
                        //foreach (MaintenanceTaskCheckList LstSetUp in ((ListView)listResultview.InnerView).CollectionSource.List.Cast<MaintenanceTaskCheckList>().ToList())
                        //{                            
                        //    listResultview.InnerView.ObjectSpace.Delete(LstSetUp);
                        //    listResultview.InnerView.ObjectSpace.Refresh();
                        //}

                        ////listResultview.InnerView.ObjectSpace.CommitChanges();                        
                    }
                }
                if (e.PropertyName == "RetireDate")
                {
                    if (objMS.RetireDate != DateTime.MinValue)
                    {
                        objMS.RetireBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    }
                    else
                    {
                        objMS.RetireBy = null;
                    }
                }
            }
            if (View.Id == "Schedular_DetailView" && e.Object != null)
            {

                Schedular objMS = (Schedular)e.Object;
                if (e.PropertyName == "StartOn")
                {
                    if (objMS.StartOn < DateTime.Today)
                    {
                            Application.ShowViewStrategy.ShowMessage("Enter a valid date.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        objMS.StartOn = DateTime.Now;
                    }
                }
                else if (e.PropertyName == "EndOn")
                {
                    if (objMS.EndOn < objMS.StartOn)
                    {
                            Application.ShowViewStrategy.ShowMessage("Enter a valid date.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        objMS.EndOn = objMS.StartOn;
                    }
                }
                //else if (e.PropertyName == "RecurrenceInfoXml")
                //{
                //    RecurrenceInfo currentinfo = new RecurrenceInfo();
                //    currentinfo.FromXml(objMS.RecurrenceInfoXml);
                //    MaintenanceSetup objMaintenanceSetup = Application.MainWindow.View.CurrentObject as MaintenanceSetup;
                //    if (objMS != null)
                //    {
                //        IList<MaintenanceTaskCheckList> objCross = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[MaintenanceSetup.Oid] = ? And [Department.Oid] = ?", objMaintenanceSetup.Oid, objMaintenanceSetup.Department.Oid));
                //        foreach (MaintenanceTaskCheckList objMTC in objCross)
                //        {
                //            if (objMS.RecurrenceInfoXml!=null&& objMTC.RecurrenceInfoXml != null)
                //            {
                //                RecurrenceInfo info = new RecurrenceInfo();
                //                info.FromXml(objMTC.RecurrenceInfoXml);
                //                if (info.Type == currentinfo.Type)
                //                {
                //                    msg = true;
                //                    objMS.RecurrenceInfoXml = null;
                //                    return;
                //                } 
                //            }
                //        }
                //    }
                //}
                //if(msg==true)
                //{
                //    Application.ShowViewStrategy.ShowMessage("Already Used the RecurrenceType", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //    msg = false;
                //}
            }
            if (View.Id == "MaintenanceTaskCheckList_DetailView_MaintenanceQueue" && e.Object !=null && e.PropertyName == "Skip" && e.NewValue != e.OldValue)
                {
                    MaintenanceTaskCheckList objNullReason = View.CurrentObject as MaintenanceTaskCheckList;
                    if (objNullReason != null && objNullReason.Skip == false)
                    {
                        objNullReason.SkipReason = null;
                    }
                }

        }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RefreshAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
            if (View.Id == "MaintenanceTaskCheckList_DetailView_MaintenanceQueue")
            {
                MaintenanceTaskCheckList objMTCL = View.CurrentObject as MaintenanceTaskCheckList;
                objMTCL.MaintainedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                objMTCL.MaintainedDate = DateTime.Now;
            }
        }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void LabwareBarcode_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Labware_ListView")
                {

                    //if (LTBarcodeReport.SelectedItem != null && LTBarcodeReport.SelectedItem.ToString() != string.Empty)
                    if (View.SelectedObjects.Count > 0)
                    {



                        strLWID = string.Empty;

                        foreach (Modules.BusinessObjects.Assets.Labware obj in View.SelectedObjects.Cast<Modules.BusinessObjects.Assets.Labware>().Where(i => !string.IsNullOrEmpty(i.LabwareID)))
                        {
                            if (obj.LabwareID == null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "pendingdistribute"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                return;
                            }

                            if (strLWID == string.Empty)
                            {
                                strLWID = "'" + obj.LabwareID + "'";
                            }
                            else
                            {
                                strLWID = strLWID + ",'" + obj.LabwareID + "'";
                            }
                        }

                        string strTempPath = Path.GetTempPath();
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\"));
                        }
                        string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\" + timeStamp + ".pdf");
                        XtraReport xtraReport = new XtraReport();

                        ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();

                        DynamicReportBusinessLayer.BLCommon.SetDBConnection(ObjReportDesignerInfo.LDMSQLServerName, ObjReportDesignerInfo.LDMSQLDatabaseName, ObjReportDesignerInfo.LDMSQLUserID, ObjReportDesignerInfo.LDMSQLPassword);
                        //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                        ObjReportingInfo.strlabwarebarcode = strLWID;
                        //xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(LTBarcodeReport.SelectedItem.ToString(), ObjReportingInfo, false);
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("PrintLabwareBarcode", ObjReportingInfo, false);
                        //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                        xtraReport.ExportToPdf(strExportedPath);
                        string[] path = strExportedPath.Split('\\');
                        int arrcount = path.Count();
                        int sc = arrcount - 3;
                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                        //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void LabwareBarcodeBig_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Labware_ListView")
                {

                    //if (LTBarcodeReport.SelectedItem != null && LTBarcodeReport.SelectedItem.ToString() != string.Empty)
                    if (View.SelectedObjects.Count > 0)
                    {



                        strLWID = string.Empty;

                        foreach (Modules.BusinessObjects.Assets.Labware obj in View.SelectedObjects.Cast<Modules.BusinessObjects.Assets.Labware>().Where(i => !string.IsNullOrEmpty(i.LabwareID)))
                        {
                            if (obj.LabwareID == null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "pendingdistribute"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                return;
                            }

                            if (strLWID == string.Empty)
                            {
                                strLWID = "'" + obj.LabwareID + "'";
                            }
                            else
                            {
                                strLWID = strLWID + ",'" + obj.LabwareID + "'";
                            }
                        }

                        string strTempPath = Path.GetTempPath();
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\"));
                        }
                        string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\" + timeStamp + ".pdf");
                        XtraReport xtraReport = new XtraReport();

                        ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();

                        DynamicReportBusinessLayer.BLCommon.SetDBConnection(ObjReportDesignerInfo.LDMSQLServerName, ObjReportDesignerInfo.LDMSQLDatabaseName, ObjReportDesignerInfo.LDMSQLUserID, ObjReportDesignerInfo.LDMSQLPassword);
                        //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                        ObjReportingInfo.strlabwarebarcode = strLWID;
                        //xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(LTBarcodeReport.SelectedItem.ToString(), ObjReportingInfo, false);
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("PrintLabwareBarcode(Big)", ObjReportingInfo, false);
                        //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                        xtraReport.ExportToPdf(strExportedPath);
                        string[] path = strExportedPath.Split('\\');
                        int arrcount = path.Count();
                        int sc = arrcount - 3;
                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                        //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void NextRecurrence(MaintenanceTaskCheckList objMTC, DateTime DateToMaintain)
        {
            try
            {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            MaintenanceTaskCheckList objMTS = objectSpace.CreateObject<MaintenanceTaskCheckList>();
            objMTS.MaintenanceSetup = objectSpace.GetObject(objMTC.MaintenanceSetup);
            objMTS.Department = objectSpace.GetObject(objMTC.Department);
            objMTS.Category = objectSpace.GetObject(objMTC.Category);
            objMTS.TaskChecklist = objectSpace.GetObject(objMTC.TaskChecklist);
            objMTS.Category = objectSpace.GetObject(objMTC.Category);
            objMTS.TaskDescription = objMTC.TaskDescription;
            objMTS.AssignTo = objMTC.AssignTo;
            objMTS.DateToMaintain = DateToMaintain;
            objMTS.NextMaintainDate = objS.NextDate;
            objMTS.StartOn = objMTC.StartOn;
            objMTS.EndOn = objMTC.EndOn;
            objMTS.RecurrenceInfoXml = objMTC.RecurrenceInfoXml;
            objMTS.MaintenanceId = objMTC.MaintenanceId;
            objectSpace.CommitChanges();
        }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void MonthlyYearly(RecurrenceInfo info, MaintenanceTaskCheckList objMTC)
        {
            try
            {
            DateTime DT = new DateTime();
            for (int a = 0; a < 7; a++)
            {
                if (objS.NextDate.DayOfWeek == DayOfWeek.Monday && info.WeekDays == WeekDays.Monday)
                {
                    a = 7;
                    if (objMTC.NextMaintainDate == DateTime.MinValue)
                    {
                        if (DT == DateTime.MinValue)
                        {
                            DT = objS.NextDate;
                            a = 0;
                            if (info.Type == RecurrenceType.Monthly)
                            {
                                objS.NextDate = objS.NextDate.AddMonths(info.Periodicity);
                            }
                            else
                            {
                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                            }
                            if (info.WeekOfMonth == WeekOfMonth.First)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 01);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Second)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 08);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Third)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 15);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 22);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Last)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                objS.NextDate = objS.NextDate.AddDays(1);
                                if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                                {
                                    objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                                }
                            }
                        }
                        else
                        {
                            objMTC.DateToMaintain = DT;
                            objMTC.NextMaintainDate = objS.NextDate;
                            ObjectSpace.CommitChanges();
                        }
                    }
                }
                else if (objS.NextDate.DayOfWeek == DayOfWeek.Tuesday && info.WeekDays == WeekDays.Tuesday)
                {
                    a = 7;
                    if (objMTC.NextMaintainDate == DateTime.MinValue)
                    {
                        if (DT == DateTime.MinValue)
                        {
                            DT = objS.NextDate;
                            a = 0;
                            if (info.Type == RecurrenceType.Monthly)
                            {
                                objS.NextDate = objS.NextDate.AddMonths(info.Periodicity);
                            }
                            else
                            {
                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                            }
                            if (info.WeekOfMonth == WeekOfMonth.First)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 01);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Second)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 08);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Third)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 15);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 22);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Last)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                objS.NextDate = objS.NextDate.AddDays(1);
                                if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                                {
                                    objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                                }
                            }
                        }
                        else
                        {
                            objMTC.DateToMaintain = DT;
                            objMTC.NextMaintainDate = objS.NextDate;
                            ObjectSpace.CommitChanges();
                        }
                    }

                }
                else if (objS.NextDate.DayOfWeek == DayOfWeek.Wednesday && info.WeekDays == WeekDays.Wednesday)
                {
                    a = 7;
                    if (objMTC.NextMaintainDate == DateTime.MinValue)
                    {
                        if (DT == DateTime.MinValue)
                        {
                            DT = objS.NextDate;
                            a = 0;
                            if (info.Type == RecurrenceType.Monthly)
                            {
                                objS.NextDate = objS.NextDate.AddMonths(info.Periodicity);
                            }
                            else
                            {
                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                            }
                            if (info.WeekOfMonth == WeekOfMonth.First)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 01);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Second)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 08);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Third)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 15);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 22);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Last)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                objS.NextDate = objS.NextDate.AddDays(1);
                                if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                                {
                                    objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                                }
                            }
                        }
                        else
                        {
                            objMTC.DateToMaintain = DT;
                            objMTC.NextMaintainDate = objS.NextDate;
                            ObjectSpace.CommitChanges();
                        }
                    }

                }
                else if (objS.NextDate.DayOfWeek == DayOfWeek.Thursday && info.WeekDays == WeekDays.Thursday)
                {
                    a = 7;
                    if (objMTC.NextMaintainDate == DateTime.MinValue)
                    {
                        if (DT == DateTime.MinValue)
                        {
                            DT = objS.NextDate;
                            a = 0;
                            if (info.Type == RecurrenceType.Monthly)
                            {
                                objS.NextDate = objS.NextDate.AddMonths(info.Periodicity);
                            }
                            else
                            {
                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                            }
                            if (info.WeekOfMonth == WeekOfMonth.First)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 01);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Second)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 08);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Third)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 15);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 22);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Last)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                objS.NextDate = objS.NextDate.AddDays(1);
                                if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                                {
                                    objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                                }
                            }
                        }
                        else
                        {
                            objMTC.DateToMaintain = DT;
                            objMTC.NextMaintainDate = objS.NextDate;
                            ObjectSpace.CommitChanges();
                        }
                    }

                }
                else if (objS.NextDate.DayOfWeek == DayOfWeek.Friday && info.WeekDays == WeekDays.Friday)
                {
                    a = 7;
                    if (objMTC.NextMaintainDate == DateTime.MinValue)
                    {
                        if (DT == DateTime.MinValue)
                        {
                            DT = objS.NextDate;
                            a = 0;
                            if (info.Type == RecurrenceType.Monthly)
                            {
                                objS.NextDate = objS.NextDate.AddMonths(info.Periodicity);
                            }
                            else
                            {
                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                            }
                            if (info.WeekOfMonth == WeekOfMonth.First)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 01);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Second)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 08);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Third)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 15);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 22);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Last)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                objS.NextDate = objS.NextDate.AddDays(1);
                                if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                                {
                                    objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                                }
                            }
                        }
                        else
                        {
                            objMTC.DateToMaintain = DT;
                            objMTC.NextMaintainDate = objS.NextDate;
                            ObjectSpace.CommitChanges();
                        }
                    }

                }
                else if (objS.NextDate.DayOfWeek == DayOfWeek.Saturday && info.WeekDays == WeekDays.Saturday)
                {
                    a = 7;
                    if (objMTC.NextMaintainDate == DateTime.MinValue)
                    {
                        if (DT == DateTime.MinValue)
                        {
                            DT = objS.NextDate;
                            a = 0;
                            if (info.Type == RecurrenceType.Monthly)
                            {
                                objS.NextDate = objS.NextDate.AddMonths(info.Periodicity);
                            }
                            else
                            {
                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                            }
                            if (info.WeekOfMonth == WeekOfMonth.First)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 01);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Second)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 08);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Third)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 15);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 22);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Last)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                objS.NextDate = objS.NextDate.AddDays(1);
                                if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                                {
                                    objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                                }
                            }
                        }
                        else
                        {
                            objMTC.DateToMaintain = DT;
                            objMTC.NextMaintainDate = objS.NextDate;
                            ObjectSpace.CommitChanges();
                        }
                    }

                }
                else if (objS.NextDate.DayOfWeek == DayOfWeek.Sunday && info.WeekDays == WeekDays.Sunday)
                {
                    a = 7;
                    if (objMTC.NextMaintainDate == DateTime.MinValue)
                    {
                        if (DT == DateTime.MinValue)
                        {
                            DT = objS.NextDate;
                            a = 0;
                            if (info.Type == RecurrenceType.Monthly)
                            {
                                objS.NextDate = objS.NextDate.AddMonths(info.Periodicity);
                            }
                            else
                            {
                                objS.NextDate = objS.NextDate.AddYears(info.Periodicity);
                            }
                            if (info.WeekOfMonth == WeekOfMonth.First)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 01);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Second)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 08);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Third)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 15);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 22);
                            }
                            else if (info.WeekOfMonth == WeekOfMonth.Last)
                            {
                                objS.NextDate = new DateTime(objS.NextDate.Year, objS.NextDate.Month, 28);
                                objS.NextDate = objS.NextDate.AddDays(1);
                                if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                                {
                                    objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                                }
                            }
                        }
                        else
                        {
                            objMTC.DateToMaintain = DT;
                            objMTC.NextMaintainDate = objS.NextDate;
                            ObjectSpace.CommitChanges();
                        }
                    }

                }
                if (a != 7)
                {
                    objS.NextDate = objS.NextDate.AddDays(1);
                    if (info.WeekOfMonth == WeekOfMonth.Last)
                    {
                        if (objS.NextDate.Month != objMTC.NextMaintainDate.Month)
                        {
                            objS.NextDate = new DateTime(objMTC.NextMaintainDate.Year, objMTC.NextMaintainDate.Month, 22);
                            a = 0;
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

        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            MaintenanceTaskCheckList objMTS1 = e.InnerArgs.CurrentObject as MaintenanceTaskCheckList;
            CollectionSource cs = new CollectionSource(objectSpace, typeof(MaintenanceTaskCheckList));
            cs.Criteria["Filter"] = CriteriaOperator.Parse("[MaintenanceId]=?", objMTS1.MaintenanceId);
            e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateListView("MaintenanceTaskCheckList_ListView_MaintenanceQueue_History", cs, false);
            e.InnerArgs.ShowViewParameters.CreatedView.Caption = "Maintenance Log" + "-" + objMTS1.MaintenanceSetup.InstrumentID.LabwareName;
            e.Handled = true;
        }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
            MaintenanceTaskCheckList objMTS1 = View.CurrentObject as MaintenanceTaskCheckList;
            objMTS1.MaintenanceStatus = InstrumentMaintenanceStatus.Maintained;
            //ObjectSpace.CommitChanges();
            //View.Close();
                Application.ShowViewStrategy.ShowMessage("Maintained successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                if (View.Id == "MaintenanceTaskCheckList_ListView")
                {
                    MaintenanceTaskCheckList objMTS1 = View.CurrentObject as MaintenanceTaskCheckList;
                    MaintenanceTaskCheckList objMTS = objectSpace.GetObject<MaintenanceTaskCheckList>(objMTS1);
                    DetailView createDetailView = Application.CreateDetailView(objectSpace, "MaintenanceTaskCheckList_DetailView", true, objMTS);
                    ShowViewParameters showViewParameters = new ShowViewParameters(createDetailView);
                    createDetailView.ViewEditMode = ViewEditMode.Edit;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    showViewParameters.CreatedView.Caption = "Task Checklist";
                    createDetailView.ViewEditMode = ViewEditMode.Edit;
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                //else
                //{
                //    MaintenanceTaskCheckList objMTS = View.CurrentObject as MaintenanceTaskCheckList;
                //    MaintenanceSetup objMS = objectSpace.GetObject<MaintenanceSetup>(objMTS.MaintenanceSetup);
                //    DetailView createDetailView = Application.CreateDetailView(objectSpace, objMS);
                //    createDetailView.ViewEditMode = ViewEditMode.Edit;
                //    Frame.SetView(createDetailView);
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewObjectAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                MaintenanceSetup objMS = objectSpace.CreateObject<MaintenanceSetup>();
                DetailView createDetailView = Application.CreateDetailView(objectSpace, objMS);
                createDetailView.ViewEditMode = ViewEditMode.Edit;
                Frame.SetView(createDetailView);
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

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();

                Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
                Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing -= SaveAction_Executing;
                Frame.GetController<ModificationsController>().SaveAndNewAction.Executing -= SaveAction_Executing;
                Frame.GetController<RefreshController>().RefreshAction.Executed -= RefreshAction_Executed;
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
				ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                Schedular objMTCL = e.AcceptActionArgs.CurrentObject as Schedular;
                if (!string.IsNullOrEmpty(objMTCL.RecurrenceInfoXml))
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    IList<MaintenanceTaskCheckList> objCrossCL = os.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[TaskChecklist.Oid] In(" + string.Format("'{0}'", string.Join("','", objS.lstTaskCheckList.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")"));
                    foreach (MaintenanceTaskCheckList objMTL in objCrossCL)
                    {
                        objMTL.Subject = objMTCL.Subject;
                        RecurrenceInfo info = new RecurrenceInfo();
                        info.FromXml(objMTCL.RecurrenceInfoXml);
                        //info.Start = info.Start.Date.AddTicks(region.Start.TimeOfDay.Ticks);
                        //region.RecurrenceInfo = 
                        objMTL.RecurrenceInfoXml = info.ToXml();
                        objMTL.StartOn = objMTCL.StartOn;
                        objMTL.EndOn = objMTCL.EndOn;
                        objMTL.AllDay = objMTCL.AllDay;
                    }
                    os.CommitChanges();
                    DashboardViewItem listResultview = ((DetailView)Application.MainWindow.View).FindItem("TaskChecklist") as DashboardViewItem;
                    if (listResultview.InnerView.Id == "MaintenanceTaskCheckList_ListView")
                    {
                        MaintenanceSetup objMS = Application.MainWindow.View.CurrentObject as MaintenanceSetup;
                        if (objMS != null)
                        {
                            IList<MaintenanceTaskCheckList> objCross = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[MaintenanceStatus] = 'PendingQueue' And [MaintenanceSetup.Oid] = ? And [Department.Oid] = ?", objMS.Oid, objMS.Department.Oid));
                            List<Guid> lisGuid = new List<Guid>();
                            List<string> lstMaintenance = new List<string>();
                            foreach (MaintenanceTaskCheckList objMTC in objCross)
                            {
                                if (!lstMaintenance.Contains(objMTC.MaintenanceId))
                                {
                                    lstMaintenance.Add(objMTC.MaintenanceId);
                                    lisGuid.Add(objMTC.Oid);
                                }
                            }
                          ((ListView)listResultview.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lisGuid);
                        }
                        listResultview.InnerView.ObjectSpace.Refresh();
                    }
                    Application.ShowViewStrategy.ShowMessage("Saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    objS.ViewClose = true;
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage("Please note that recurrence must not be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChecklistShow_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Application.MainWindow.View.ObjectSpace.CommitChanges();
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(TaskCheckList));
                ListView CreateListView = Application.CreateListView("TaskCheckList_ListView_Copy", cs, false);
                ShowViewParameters showViewParameters = new ShowViewParameters(CreateListView);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                showViewParameters.CreatedView.Caption = "Task Checklist";
                DialogController dc = Application.CreateController<DialogController>();
                dc.Accepting += Dc_List_Accepting;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_List_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                objS.lstTaskCheckList = e.AcceptActionArgs.SelectedObjects.Cast<TaskCheckList>().ToList();
                MaintenanceSetup objMS = Application.MainWindow.View.CurrentObject as MaintenanceSetup;
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    foreach (TaskCheckList obje in e.AcceptActionArgs.SelectedObjects)
                    {
                        MaintenanceTaskCheckList objMCheck = ObjectSpace.CreateObject<MaintenanceTaskCheckList>();
                        objMCheck.Category = ObjectSpace.GetObject(obje.Category);

                        CriteriaOperator estexpression = CriteriaOperator.Parse("Max(MaintenanceId)");
                        int max = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate<MaintenanceTaskCheckList>(estexpression, CriteriaOperator.Parse("")));
                        if (max > 0)
                        {
                            objMCheck.MaintenanceId = (max + 1).ToString();
                        }
                        else
                        {
                            objMCheck.MaintenanceId = "101";
                        }
                        objMCheck.TaskDescription = obje.TaskDescription;
                        objMCheck.Department = ObjectSpace.GetObject(obje.Department);
                        //objMCheck.Schedular = objS.Schedular;
                        objMCheck.TaskChecklist = ObjectSpace.GetObject(obje);
                        objMCheck.MaintenanceSetup = ObjectSpace.GetObject(objMS);
                        ObjectSpace.CommitChanges();
                    }
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    Schedular objMTL = objspace.CreateObject<Schedular>();
                    DetailView CreateDetailView = Application.CreateDetailView(objspace, "Schedular_DetailView", true, objMTL);
                    CreateDetailView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = CreateDetailView;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.Accepting += Dc_Accepting;
                    dc.ViewClosing += Dc_ViewClosing;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Please select an item to proceed.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_ViewClosing(object sender, EventArgs e)
        {
            try
            {
            View view = (View)sender as View;

            if (view.Id == "Schedular_DetailView")
            {
                Schedular objMTCL = view.CurrentObject as Schedular;
                if (objS.ViewClose == false)
                {
                    IList<MaintenanceTaskCheckList> objCrossCL = ObjectSpace.GetObjects<MaintenanceTaskCheckList>(CriteriaOperator.Parse("[TaskChecklist.Oid] In(" + string.Format("'{0}'", string.Join("','", objS.lstTaskCheckList.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")"));
                    ObjectSpace.Delete(objCrossCL);
                    ObjectSpace.CommitChanges();
                        View.Refresh();
                }
                else
                {
                    objS.ViewClose = false;
                }
            }
        }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SetConnectionString()
        {
            try
            {
                AppSettingsReader config = new AppSettingsReader();
                string serverType, server, database, user, password;
                string[] connectionstring = ObjReportDesignerInfo.WebConfigConn.Split(';');
                ObjReportDesignerInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChecklistHide_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                foreach (MaintenanceTaskCheckList objMTL in e.SelectedObjects)
                {
                    View.ObjectSpace.Delete(objMTL);
                    View.ObjectSpace.CommitChanges();
                }

                Application.ShowViewStrategy.ShowMessage("Unlinked successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
