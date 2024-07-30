using DevExpress.ExpressApp;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ICMRollbackReasonViewController : ViewController
    {

        #region Declaration
        MessageTimer timer = new MessageTimer();
        ICMinfo objRollBackReason = new ICMinfo();
        #endregion

        #region Constructor
        public ICMRollbackReasonViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ICMRollBack);
        }
        #endregion

        #region Default Methods
        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View != null && View.CurrentObject != null)
                {
                    ICMRollBack obj = (ICMRollBack)View.CurrentObject;
                    if (obj != null)
                    {
                        objRollBackReason.RollBackReason = obj.Reason;
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
            try
            {
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Event
        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Reason")
                {
                    if (View.ObjectTypeInfo.Type == typeof(ICMRollBack))
                    {
                        ICMRollBack objReason = (ICMRollBack)e.Object;
                        objRollBackReason.RollBackReason = objReason.Reason;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion
    }
}
