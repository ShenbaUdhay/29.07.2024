using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Web.Controllers.CheckBoxCustomization
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class HideCheckBoxWebViewController : ViewController<ListView>
    {
        MessageTimer timer = new MessageTimer();
        public HideCheckBoxWebViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "BroughtBy_LookupListView_Copy_SampleCheckIn;" + "Customer_LookupListView_Copy_SampleCheckin;" + "Contact_LookupListView_Copy_SampleCheckin;" + "Project_LookupListView_Copy_SampleCheckIn;" + "Employee_LookupListView_Copy_SampleCheckIn;"
                 + "Employee_LookupListView_Copy_SampleLogin;" + "Samplecheckin_LookupListView_Copy_SampleLogin;" + "SampleLogIn_LookupListView_Copy_SampleLogin;" + "QCType_LookupListView_Copy_SampleLogin;" + "SampleType_LookupListView_Copy_SampleLogin;";


        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
                if (View != null)
                {
                    gridListEditor.CanSelectRows = false;
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
