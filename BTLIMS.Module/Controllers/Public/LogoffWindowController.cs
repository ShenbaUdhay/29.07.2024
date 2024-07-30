using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class LogoffWindowController : WindowController
    {
        public static DateTime LastTimeIteration;
        MessageTimer timer = new MessageTimer();
        System.Windows.Forms.Timer LogOffTimer = new System.Windows.Forms.Timer();
        public LogoffWindowController()
        {
            InitializeComponent();
            LastTimeIteration = DateTime.Now;
            LogOffTimer.Interval = 60_000;
            LogOffTimer.Start();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                LogOffTimer.Tick += new EventHandler(LogOffTimer_Tick);
                if (Window is WinWindow)
                {
                    ((WinWindow)Window).KeyDown += WinWindow_KeyDown;
                }
                // Perform various tasks depending on the target Window.
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void WinWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                //your code
                LogOffTimer.Stop();
                if (LastTimeIteration.AddSeconds(60) < DateTime.Now)
                {
                    Application.LogOff();
                }
                LogOffTimer.Start();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        protected override void OnDeactivated()
        {
            try
            {
                // Unsubscribe from previously subscribed events and release other references and resources.
                base.OnDeactivated();
                LogOffTimer.Tick -= new EventHandler(LogOffTimer_Tick);
                if (Window is WinWindow)
                {
                    ((WinWindow)Window).KeyDown -= WinWindow_KeyDown;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void LogOffTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                LogOffTimer.Stop();
                if (LastTimeIteration.AddSeconds(60) < DateTime.Now)
                {
                    Application.LogOff();
                }
                LogOffTimer.Start();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
