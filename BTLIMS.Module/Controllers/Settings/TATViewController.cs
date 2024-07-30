using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.ComponentModel;
using System.Globalization;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TATViewController : ViewController, IXafCallbackHandler
    {
        ModificationsController mdcsaveaction, mdcsavenewaction, mdcsavecloseaction;
        MessageTimer timer = new MessageTimer();
        ICallbackManagerHolder sheet;
        public TATViewController()
        {
            InitializeComponent();
            TargetViewId = "TurnAroundTime_DetailView;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "TurnAroundTime_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    mdcsaveaction = Frame.GetController<ModificationsController>();
                    mdcsaveaction.SaveAction.Executing += SaveAction_Executing;
                    mdcsaveaction.SaveAction.Executed += SaveAction_Executed;
                    //mdcSave.SaveAction.Executed += SaveAction_Executed;
                    mdcsavecloseaction = Frame.GetController<ModificationsController>();
                    mdcsavecloseaction.SaveAndCloseAction.Executing += SaveAction_Executing;
                    mdcsavecloseaction.SaveAndCloseAction.Executed += SaveAction_Executed;
                    mdcsavenewaction = Frame.GetController<ModificationsController>();
                    mdcsavenewaction.SaveAndNewAction.Executing += SaveAction_Executing;
                    mdcsavenewaction.SaveAndNewAction.Executed += SaveAction_Executed;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }

        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                ////IList<TurnAroundTime> lstTAT = ObjectSpace.GetObjects<TurnAroundTime>(CriteriaOperator.Parse("GCRecord is null"));

                ////int sort = 1;
                ////foreach (TurnAroundTime objectTAT in lstTAT.ToList())
                ////{
                ////    objectTAT.Sort = 0;
                ////    ObjectSpace.CommitChanges();
                ////}
                ////foreach (TurnAroundTime objectTAT in lstTAT.ToList())
                ////{
                ////    var count = ObjectSpace.GetObjectsCount(typeof(TurnAroundTime), CriteriaOperator.Parse("[Count] <= ?", objectTAT.Count));
                ////    objectTAT.Sort = count;
                ////    ObjectSpace.CommitChanges();
                ////}

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
                if (View != null && View.Id == "TurnAroundTime_DetailView")
                {
                    TurnAroundTime objTAT1 = (TurnAroundTime)View.CurrentObject;

                    if (View.CurrentObject == e.Object && e.PropertyName == "TAT")
                    {
                        string strTAT = string.Empty;
                        int temptat = 0;
                        int temptime = 0;
                        TurnAroundTime objTAT = (TurnAroundTime)e.Object;
                        if (objTAT.TAT != null)
                        {
                            if (objTAT.TAT.ToUpper().Contains("DAYS"))
                            {
                                strTAT = objTAT.TAT.ToUpper().Replace("DAYS", "");
                                temptime = 24;
                            }
                            else if (objTAT.TAT.ToUpper().Contains("DAY"))
                            {
                                strTAT = objTAT.TAT.ToUpper().Replace("DAY", "");
                                temptime = 24;
                            }
                            if (objTAT.TAT.ToUpper().Contains("WEEKS"))
                            {
                                strTAT = objTAT.TAT.ToUpper().Replace("WEEKS", "");
                                temptime = 24 * 7;
                            }
                            else if (objTAT.TAT.ToUpper().Contains("WEEK"))
                            {
                                strTAT = objTAT.TAT.ToUpper().Replace("WEEK", "");
                                temptime = 24 * 7;
                            }
                            if (objTAT.TAT.ToUpper().Contains("MONTHS"))
                            {
                                strTAT = objTAT.TAT.ToUpper().Replace("MONTHS", "");
                                temptime = 24 * 30;
                            }
                            else if (objTAT.TAT.ToUpper().Contains("MONTH"))
                            {
                                strTAT = objTAT.TAT.ToUpper().Replace("MONTH", "");
                                temptime = 24 * 30;
                            }
                            if (objTAT.TAT.ToUpper().Contains("YEARS"))
                            {
                                strTAT = objTAT.TAT.ToUpper().Replace("YEARS", "");
                                temptime = 24 * 365;
                            }
                            else if (objTAT.TAT.ToUpper().Contains("YEAR"))
                            {
                                strTAT = objTAT.TAT.ToUpper().Replace("YEAR", "");
                                temptime = 24 * 365;
                            }
                            strTAT = strTAT.Trim();
                            int numericValue;
                            bool isNumber = int.TryParse(strTAT, out numericValue);
                            if (isNumber == true)
                            {
                                temptat = Convert.ToInt32(strTAT);
                                objTAT.Count = temptat * temptime;
                            }
                        }


                    }
                    TurnAroundTime tatobj = View.ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[Default] = True And [Oid]<>?", objTAT1.Oid));
                    if (View.CurrentObject == e.Object && objTAT1.Default && tatobj != null)
                    {
                        WebWindow.CurrentRequestWindow.RegisterClientScript("Openspreadsheet", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Default has been already enabled, Do you want this TAT to be enabled as default'); {0}", sheet.CallbackManager.GetScript("openspreadsheet", "openconfirm")));

                    }

                }
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
                if (View.Id == "TurnAroundTime_DetailView")
                {
                    TurnAroundTime objTAT = (TurnAroundTime)View.CurrentObject;
                    if (objTAT.TAT != null && objTAT.Count == 0)
                    {
                        string strTAT = string.Empty;
                        int temptat = 0;
                        int temptime = 0;
                        if (objTAT.TAT.ToUpper().Contains("DAYS"))
                        {
                            strTAT = objTAT.TAT.ToUpper().Replace("DAYS", "");
                            temptime = 24;
                        }
                        else if (objTAT.TAT.ToUpper().Contains("DAY"))
                        {
                            strTAT = objTAT.TAT.ToUpper().Replace("DAY", "");
                            temptime = 24;
                        }
                        if (objTAT.TAT.ToUpper().Contains("WEEKS"))
                        {
                            strTAT = objTAT.TAT.ToUpper().Replace("WEEKS", "");
                            temptime = 24 * 7;
                        }
                        else if (objTAT.TAT.ToUpper().Contains("WEEK"))
                        {
                            strTAT = objTAT.TAT.ToUpper().Replace("WEEK", "");
                            temptime = 24 * 7;
                        }
                        if (objTAT.TAT.ToUpper().Contains("MONTHS"))
                        {
                            strTAT = objTAT.TAT.ToUpper().Replace("MONTHS", "");
                            temptime = 24 * 30;
                        }
                        else if (objTAT.TAT.ToUpper().Contains("MONTH"))
                        {
                            strTAT = objTAT.TAT.ToUpper().Replace("MONTH", "");
                            temptime = 24 * 30;
                        }
                        if (objTAT.TAT.ToUpper().Contains("YEARS"))
                        {
                            strTAT = objTAT.TAT.ToUpper().Replace("YEARS", "");
                            temptime = 24 * 365;
                        }
                        else if (objTAT.TAT.ToUpper().Contains("YEAR"))
                        {
                            strTAT = objTAT.TAT.ToUpper().Replace("YEAR", "");
                            temptime = 24 * 365;
                        }
                        strTAT = strTAT.Trim();
                        int numericValue;
                        bool isNumber = int.TryParse(strTAT, out numericValue);
                        if (isNumber == true)
                        {
                            temptat = Convert.ToInt32(strTAT);
                            objTAT.Count = temptat * temptime;
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
            sheet = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
            sheet.CallbackManager.RegisterHandler("openspreadsheet", this);
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (View.Id == "TurnAroundTime_DetailView")
            {
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            }

        }

        public void ProcessAction(string parameter)
        {
            try
            {
                if (parameter == "true")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    TurnAroundTime selectedObject = View.CurrentObject as TurnAroundTime;
                    if (selectedObject != null)
                    {
                        TurnAroundTime tatobj = View.ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[Default] = True And [Oid]<>?", selectedObject.Oid));

                        if (tatobj != null)
                        {

                            tatobj.Default = false;
                        }
                    }
                }
                else
                {
                    TurnAroundTime selectedObject = View.CurrentObject as TurnAroundTime;
                    if (selectedObject != null)
                    {
                        selectedObject.Default = false;
                    }
                }

            }

            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
