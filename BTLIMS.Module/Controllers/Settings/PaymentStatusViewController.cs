using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PaymentStatusViewController : ViewController, IXafCallbackHandler
    {
        ICallbackManagerHolder Default;
        MessageTimer timer = new MessageTimer();
        public PaymentStatusViewController()
        {
            InitializeComponent();
            TargetViewId = "PaymentStatus_DetailView";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "PaymentStatus_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "PaymentStatus_DetailView" && e.PropertyName == "Default" && e.OldValue != e.NewValue)
                {
                    PaymentStatus objPayment = (PaymentStatus)e.Object;
                    PaymentStatus tatobj = View.ObjectSpace.FindObject<PaymentStatus>(CriteriaOperator.Parse("[Default] = True And [Oid]<>?", objPayment.Oid));
                    if (objPayment != null && objPayment.Default && tatobj!=null)
                    {
                        WebWindow.CurrentRequestWindow.RegisterClientScript("OpenMessage", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Default has been already enabled, Do you want this Status to be enabled as default'); {0}", Default.CallbackManager.GetScript("OpenMessage", "openconfirm")));
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
            try
            {
                if (View.Id == "PaymentStatus_DetailView")
                {
                    Default = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    Default.CallbackManager.RegisterHandler("OpenMessage", this);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            if (View.Id == "PaymentStatus_DetailView")
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
                    PaymentStatus selectedObject = View.CurrentObject as PaymentStatus;
                    if (selectedObject != null)
                    {
                        PaymentStatus tatobj = View.ObjectSpace.FindObject<PaymentStatus>(CriteriaOperator.Parse("[Default] = True And [Oid]<>?", selectedObject.Oid));

                        if (tatobj != null)
                        {

                            tatobj.Default = false;
                        }
                    }
                }
                else
                {
                    PaymentStatus selectedObject = View.CurrentObject as PaymentStatus;
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
