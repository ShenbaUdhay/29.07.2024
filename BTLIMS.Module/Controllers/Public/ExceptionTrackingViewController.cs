using DevExpress.ExpressApp;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.IO;
using System.Web;
using System.Windows;

namespace LDM.Module.Controllers.Public
{
    public partial class ExceptionTrackingViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public ExceptionTrackingViewController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        public void InsertException(string errmsg, string errstack, string controllername, string funcname, string viewid)
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                ExceptionTracking exception = objectSpace.CreateObject<ExceptionTracking>();
                exception.ErrorMessage = errmsg;
                exception.StackTrace = errstack;
                exception.ControllerName = controllername;
                exception.FunctionName = funcname;
                exception.ViewID = viewid;
                exception.LoginBy = objectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                exception.LoginDate = DateTime.Now;
                objectSpace.CommitChanges();
                //return true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void InsertAspxException(string errmsg, string errstack, string controllername, string funcname)
        {
            try
            {
                objectspaceinfo objinfo = new objectspaceinfo();
                IObjectSpace objectSpace = objinfo.tempobjspace;
                ExceptionTracking exception = objectSpace.CreateObject<ExceptionTracking>();
                exception.ErrorMessage = errmsg;
                exception.StackTrace = errstack;
                exception.ControllerName = controllername;
                exception.FunctionName = funcname;
                exception.LoginBy = objectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                exception.LoginDate = DateTime.Now;
                objectSpace.CommitChanges();
                MessageBox.Show(errmsg);
                //WebWindow.CurrentRequestWindow.RegisterClientScript("XafMessageBox", "alert('" + errmsg + "');");
                //Application = objinfo.tempapplication;
                //Application.ShowViewStrategy.ShowMessage(errmsg, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void Logtime(string msg, string time)
        {
            try
            {
                string message = "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += string.Format("Process: {0}", msg);
                message += Environment.NewLine;
                message += string.Format("Elapsed Time: {0}", time);
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                string path = HttpContext.Current.Server.MapPath("~/Log.txt");
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
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
