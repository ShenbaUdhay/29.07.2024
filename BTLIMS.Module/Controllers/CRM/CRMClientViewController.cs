using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;

namespace LDM.Module.Controllers.CRM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CRMClientViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public CRMClientViewController()
        {
            InitializeComponent();
            TargetViewId = "Customer_DetailView_CRM;" + "Customer_DetailView_ClosedCRM;" + "Customer_ListView_Closed;";
            ReActivate.TargetViewId = "Customer_DetailView_ClosedCRM;";
            ReActivate.TargetObjectsCriteria = "[IsClose] = True";
            DeActivate.TargetViewId = "Customer_DetailView_CRM;";
            DeActivate.TargetObjectsCriteria = "[IsClose] = False";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "Customer_ListView_Closed")
                {
                    ListViewProcessCurrentObjectController listview = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listview != null)
                    {
                        listview.CustomProcessSelectedItem += Listview_CustomProcessSelectedItem;
                    }
                }
                //else if(View.Id== "Customer_DetailView_ClosedCRM")
                //{
                //    if(((DetailView)View).ViewEditMode==ViewEditMode.View)
                //    {
                //        ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Listview_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (e.InnerArgs.CurrentObject != null)
                {
                    Customer objcust = (Customer)e.InnerArgs.CurrentObject;
                    if (objcust != null)
                    {
                        DetailView dvcust = Application.CreateDetailView(ObjectSpace, "Customer_DetailView_ClosedCRM", false, objcust);
                        dvcust.ViewEditMode = ViewEditMode.View;
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
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            if (View.Id == "Customer_ListView_Closed")
            {
                ListViewProcessCurrentObjectController listview = Frame.GetController<ListViewProcessCurrentObjectController>();
                if (listview != null)
                {
                    listview.CustomProcessSelectedItem -= Listview_CustomProcessSelectedItem;
                }
            }
        }

        private void ReActivate_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Customer_DetailView_ClosedCRM")
                {
                    Customer objcust = (Customer)View.CurrentObject;
                    if (objcust != null)
                    {
                        //IObjectSpace os = Application.CreateObjectSpace();
                        DummyClass objdummycls = ObjectSpace.CreateObject<DummyClass>();
                        DetailView createdView = Application.CreateDetailView(ObjectSpace, "DummyClass_DetailView_Reasons", false, objdummycls);
                        ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                        createdView.ViewEditMode = ViewEditMode.Edit;
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += CRMReActivate_Accepting;
                        dc.AcceptAction.Executed += AcceptAction_Executed;
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "Customer_DetailView_CRM")
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/LDMMessages", "Deactivatesuccess"), InformationType.Success, 3000, InformationPosition.Top);
                }
                else if (View.Id == "Customer_DetailView_ClosedCRM")
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages/LDMMessages", "reactivatesuccess"), InformationType.Success, 3000, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CRMReActivate_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                DummyClass objCurrentObject = (DummyClass)e.AcceptActionArgs.CurrentObject;
                if (objCurrentObject != null && !string.IsNullOrEmpty(objCurrentObject.Reason) && !string.IsNullOrWhiteSpace(objCurrentObject.Reason))
                {
                    Customer objcust = (Customer)View.CurrentObject;
                    if (objcust != null)
                    {
                        objcust.IsClose = false;
                        objcust.CloseReason = objCurrentObject.Reason;
                        ObjectSpace.CommitChanges();
                    }
                    ObjectSpace.Refresh();
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EnterReason"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeActivate_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Customer_DetailView_CRM")
                {
                    Customer objcust = (Customer)View.CurrentObject;
                    if (objcust != null)
                    {
                        //IObjectSpace os = Application.CreateObjectSpace();
                        DummyClass objdummycls = ObjectSpace.CreateObject<DummyClass>();
                        DetailView createdView = Application.CreateDetailView(ObjectSpace, "DummyClass_DetailView_Reasons", false, objdummycls);
                        ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                        createdView.ViewEditMode = ViewEditMode.Edit;
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += CRMDeActivate_Accepting;
                        dc.AcceptAction.Executed += AcceptAction_Executed;
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CRMDeActivate_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                DummyClass objCurrentObject = (DummyClass)e.AcceptActionArgs.CurrentObject;
                if (objCurrentObject != null && !string.IsNullOrEmpty(objCurrentObject.Reason) && !string.IsNullOrWhiteSpace(objCurrentObject.Reason))
                {
                    Customer objcust = (Customer)View.CurrentObject;
                    if (objcust != null)
                    {
                        objcust.IsClose = true;
                        objcust.CloseReason = objCurrentObject.Reason;
                        ObjectSpace.CommitChanges();
                    }
                    ObjectSpace.Refresh();
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EnterReason"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
