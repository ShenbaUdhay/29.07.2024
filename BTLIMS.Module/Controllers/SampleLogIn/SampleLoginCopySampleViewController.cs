using DevExpress.ExpressApp;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Windows.Forms;

namespace BTLIMS.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CopyNoOfSamplesViewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        CopyNoOfSamplesPopUp objCopySampleInfo = new CopyNoOfSamplesPopUp();
        #endregion

        #region Constructor
        public CopyNoOfSamplesViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(SL_CopyNoOfSamples);
        }
        #endregion

        #region  DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
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
            try
            {
                // Access and customize the target View control.
                if (View != null && View.CurrentObject != null)
                {
                    SL_CopyNoOfSamples obj = (SL_CopyNoOfSamples)View.CurrentObject;
                    if (obj != null)
                    {
                        objCopySampleInfo.NoOfSamples = obj.NoOfSamples;
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

        #region Events
        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "NoOFSamples")
                {
                    if (View.ObjectTypeInfo.Type == typeof(SL_CopyNoOfSamples))
                    {
                        SL_CopyNoOfSamples ObjCopyNoOfSamples = (SL_CopyNoOfSamples)e.Object;
                        objCopySampleInfo.NoOfSamples = ObjCopyNoOfSamples.NoOfSamples;
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



