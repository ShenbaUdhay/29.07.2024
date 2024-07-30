using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement.SampleCustody;
using System;

namespace LDM.Module.Web.Controllers._SampleCustody
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    //this controller used for show the DetailView from the Navigation Item Click
    public partial class ShowDetailViewFromNavigationWindowController : WindowController
    {
        MessageTimer timer = new MessageTimer();
        public ShowDetailViewFromNavigationWindowController()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            Frame.GetController<ShowNavigationItemController>().CustomShowNavigationItem += new EventHandler<CustomShowNavigationItemEventArgs>(ShowDetailView_CustomShowNavigationItem);

        }
        void ShowDetailView_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            try
            {
                if (e.ActionArguments.SelectedChoiceActionItem.Id == "SampleIn" || e.ActionArguments.SelectedChoiceActionItem.Id == "SampleOut")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SampleCustody));
                    SampleCustody obj = objectSpace.CreateObject<SampleCustody>();
                    if (e.ActionArguments.SelectedChoiceActionItem.Id == "SampleIn")
                    {
                        //NeedToShowObjects.Clear();
                        DetailView dv = Application.CreateDetailView(objectSpace, "SampleCustody_DetailView_Copy_SampleIn", true, obj);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        e.ActionArguments.ShowViewParameters.CreatedView = dv;
                        e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
                        e.Handled = true;
                    }
                    else if (e.ActionArguments.SelectedChoiceActionItem.Id == "SampleOut")
                    {
                        DetailView dv = Application.CreateDetailView(objectSpace, "SampleCustody_DetailView_Copy_SampleOut", true, obj);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        e.ActionArguments.ShowViewParameters.CreatedView = dv;
                        e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
