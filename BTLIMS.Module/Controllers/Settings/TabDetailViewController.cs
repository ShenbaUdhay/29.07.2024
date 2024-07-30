using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace Labmaster.Module.Controllers.SamplingManagement
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TabDetailViewController : ViewController<DetailView>, IXafCallbackHandler
    {
        Testpriceinfo testpriceinfo = new Testpriceinfo();
        Tabinfo objtabinfo = new Tabinfo();
        MessageTimer timer = new MessageTimer();
        bool IsPageCreated = false;
        ASPxPageControl pageControl;
        public TabDetailViewController()
        {
            InitializeComponent();
            TargetViewId = "TestPrice_DetailView;" + "Tasks_DetailView_FieldDataEntry;" + "Tasks_DetailView_FieldDataReview1;" + "Tasks_DetailView_FieldDataReview2;" + "Invoicing_DetailView;"
                + "PTStudyLog_DetailView;" + "Invoicing_DetailView_Queue;" + "TabFieldConfiguration_DetailView_TestTab;" + "NotebookBuilder_DetailView;" + "Invoicing_DetailView_PreInvoiceDetails;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                //((WebLayoutManager)View.LayoutManager).PageControlCreated += TabViewController_PageControlCreated;
                //if (View.Id == "TestPrice_DetailView")
                //{
                //    Modules.BusinessObjects.Setting.TestPrice objtp = (Modules.BusinessObjects.Setting.TestPrice)View.CurrentObject;
                //    testpriceinfo.testpricetypeinfo = objtp.PriceType.ToString();
                //}
                if (View.Id == "Tasks_DetailView_FieldDataEntry" || View.Id == "Tasks_DetailView_FieldDataReview1"
                    || View.Id == "Tasks_DetailView_FieldDataReview2" || View.Id == "Invoicing_DetailView"
                    || View.Id == "PTStudyLog_DetailView" || View.Id == "Invoicing_DetailView_Queue" || View.Id == "TabFieldConfiguration_DetailView_TestTab"
                    || View.Id == "NotebookBuilder_DetailView" || View.Id == "Invoicing_DetailView_PreInvoiceDetails")
                {
                    ((WebLayoutManager)View.LayoutManager).PageControlCreated += OnPageControlCreated;
                }
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
                // Perform various tasks depending on the target View.
                //{
                //    if (View.CurrentObject == e.Object && e.PropertyName == "PriceType")
                //    {
                //        Modules.BusinessObjects.Setting.TestPrice objtm = (Modules.BusinessObjects.Setting.TestPrice)View.CurrentObject;
                //        testpriceinfo.testpricetypeinfo = objtm.PriceType.ToString();
                //        //((WebLayoutManager)View.LayoutManager).PageControlCreated += TabViewController_PageControlCreated;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TabViewController_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            try
            {
                if (testpriceinfo.testpricetypeinfo == "PerTest")
                {
                    if (e.Model.Id == "Item3")
                    {
                        e.PageControl.Visible = true;
                    }
                    if (e.Model.Id == "Item6")
                    {
                        e.PageControl.Visible = false;
                    }
                }

                else //(objtp.PriceType == Pricetype.PerParameter)
                {
                    if (e.Model.Id == "Item3")
                    {
                        e.PageControl.Visible = false;
                    }
                    if (e.Model.Id == "Item6")
                    {
                        e.PageControl.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void OnPageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            try
            {
                if (View.Id == "Tasks_DetailView_FieldDataEntry" || View.Id == "Tasks_DetailView_FieldDataReview1"
                    || View.Id == "Tasks_DetailView_FieldDataReview2")
                {
                    e.PageControl.ClientSideEvents.Init = "function(s,e){s.SetActiveTabIndex(1);}";
                    ((WebLayoutManager)View.LayoutManager).PageControlCreated -= OnPageControlCreated;
                }
                else if (View.Id == "Invoicing_DetailView" || View.Id == "PTStudyLog_DetailView" || View.Id == "Invoicing_DetailView_Queue" || View.Id == "Invoicing_DetailView_PreInvoiceDetails")
                {
                    e.PageControl.ClientSideEvents.Init = "function(s,e){s.SetActiveTabIndex(0);}";
                    ((WebLayoutManager)View.LayoutManager).PageControlCreated -= OnPageControlCreated;
                }
                else if (View.Id == "TabFieldConfiguration_DetailView_TestTab")
                {
                    pageControl = e.PageControl;
                    //if(IsPageCreated == false)
                    //{
                    //    IsPageCreated = true;
                    //    e.PageControl.ClientSideEvents.Init = "function(s,e){s.SetActiveTabIndex(0);}"; //{RaiseXafCallback(globalCallbackControl, 'TabName', 'TabClick|SimpleEditors' , '', false);}";
                    //}
                    //else
                    {
                        e.PageControl.ClientSideEvents.TabClick = "function(s,e){ var tabname = e.tab.name; RaiseXafCallback(globalCallbackControl, 'TabName', 'TabClick|' + tabname , '', false);}"; //var tabname = s.GetActiveTab().name; alert(tabname); 
                    }
                    ((WebLayoutManager)View.LayoutManager).PageControlCreated -= OnPageControlCreated;
                    // WebWindow.CurrentRequestWindow.RegisterClientScript("CalTabName", "RaiseXafCallback(globalCallbackControl, 'TabName', 'QuotesListAmount', false);");
                    //e.PageControl.ClientSideEvents.Init = "function(s,e){ if (s.GetActiveTab().name == '" + phoneNumbersGroupName + "') { s.SetActiveTab(s.GetTabByName('" + activeTabGroupName + "')); }}";
                    //((WebLayoutManager)View.LayoutManager).PageControlCreated -= OnPageControlCreated;

                }
                else if (View.Id == "NotebookBuilder_DetailView")
                {
                    e.PageControl.ClientSideEvents.Init = "function(s,e){s.SetActiveTabIndex(0);}";
                    ((WebLayoutManager)View.LayoutManager).PageControlCreated -= OnPageControlCreated;
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
            try
            {
                base.OnViewControlsCreated();
                if (View.Id == "TabFieldConfiguration_DetailView_TestTab")
                {
                    ((WebLayoutManager)View.LayoutManager).PageControlCreated += OnPageControlCreated;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("TabName", this);
                }
                // Access and customize the target View control.
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void ProcessAction(string parameter)
        {
            try
            {
                string[] paramsarr = parameter.Split('|');
                if (paramsarr[0] == "TabClick")
                {
                    objtabinfo.strtabname = paramsarr[1].ToString();
                    if (paramsarr[1] == "SampleParameters")
                    {
                        pageControl.ActiveTabIndex = 0;
                    }
                    else
                    if (paramsarr[1] == "Surrogates")
                    {
                        pageControl.ActiveTabIndex = 1;
                    }
                    else
                    if (paramsarr[1] == "InternalStandards")
                    {
                        pageControl.ActiveTabIndex = 2;
                    }
                    else
                    if (paramsarr[1] == "QCParameters")
                    {
                        pageControl.ActiveTabIndex = 3;
                    }
                    else
                    if (paramsarr[1] == "Components")
                    {
                        pageControl.ActiveTabIndex = 4;
                    }
                    else
                    if (paramsarr[1] == "QCParameterDefaults")
                    {
                        pageControl.ActiveTabIndex = 5;
                    }
                }
                View.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                if (View.Id == "TabFieldConfiguration_DetailView_TestTab")
                {
                    IsPageCreated = false;
                }
                if (View.Id == "Tasks_DetailView_FieldDataEntry" || View.Id == "Tasks_DetailView_FieldDataReview1"
               || View.Id == "Tasks_DetailView_FieldDataReview2")
                {
                    ((WebLayoutManager)View.LayoutManager).PageControlCreated -= OnPageControlCreated;
                }
                //((WebLayoutManager)View.LayoutManager).PageControlCreated -= TabViewController_PageControlCreated;
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
