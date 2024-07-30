using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.BarCodeSampleCustody;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.Barcode_Sample_Custody
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SampleInOutHistoryController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public SampleInOutHistoryController()
        {
            InitializeComponent();
            TargetViewId = "SampleCustodyTest_DetailView_SampleDisposal;" + "SampleCustodyTest_DetailView_SampleLocation;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void SAHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SampleCustodyTest_DetailView_SampleDisposal")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    SampleCustodyTest objToShow = objspace.CreateObject<SampleCustodyTest>();
                    if (objToShow != null)
                    {
                        DetailView CreateDetailView = Application.CreateDetailView(objspace, "SampleCustodyTest_DetailView_SampleDisposal_History", false, objToShow);
                        //objRQPInfo.CurrentViewID = CreateDetailView.Id;
                        CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                        Frame.SetView(CreateDetailView);
                        //e.Handled = true;
                    }
                }
                if (View.Id == "SampleCustodyTest_DetailView_SampleLocation")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    SampleCustodyTest objToShow = objspace.CreateObject<SampleCustodyTest>();
                    if (objToShow != null)
                    {
                        DetailView CreateDetailView = Application.CreateDetailView(objspace, "SampleCustodyTest_DetailView_SampleLocation_History", false, objToShow);
                        CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                        Frame.SetView(CreateDetailView);
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
