using DevExpress.DashboardWeb;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LDM.Module.Web.Controllers.Dashboard
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class ShowDetailViewFromDashboardIView : ObjectViewController<DetailView, IDashboardData>, IXafCallbackHandler
    {
        private const string HandlerName = "WebShowDetailViewFromDashboardController";
        private ParametrizedAction openDetailViewAction;
        SampleCheckInInfo objSCInfo = new SampleCheckInInfo();
        ResultEntryQueryPanelInfo objQPInfo = new ResultEntryQueryPanelInfo();
        MessageTimer timer = new MessageTimer();
        public ShowDetailViewFromDashboardIView()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
            openDetailViewAction = new ParametrizedAction(this, "Dashboard_OpenDetailView", "Dashboard", typeof(string));
            openDetailViewAction = new ParametrizedAction(this, "Result_View", "ListView", typeof(string));

            openDetailViewAction.Caption = "OpenDetailView";
            openDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            openDetailViewAction.Execute += OpenDetailViewAction_Execute;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            try
            {
                WebDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
                if (dashboardViewerViewItem != null)
                {
                    if (dashboardViewerViewItem.DashboardDesigner != null)
                    {
                        CustomizeDashboardViewer(dashboardViewerViewItem.DashboardDesigner);
                    }
                    dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;
                }
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                WebDashboardViewerViewItem dashboardViewerViewItem = sender as WebDashboardViewerViewItem;
                CustomizeDashboardViewer(dashboardViewerViewItem.DashboardDesigner);
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CustomizeDashboardViewer(ASPxDashboard dashboardControl)
        {
            try
            {
                XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                callbackManager.RegisterHandler(HandlerName, this);
                string widgetScript = @"function getOid(s, e) {{
                                        function findMeasure(measure) {{
                                            return measure.DataMember === 'Oid';
                                        }}
                                        function findMeasureStatus(measure) {{
                                            return measure.DataMember === 'Status';
                                        }}
                                        if (e.ItemName.includes('gridDashboardItem')) {{
                                             var itemData = e.GetData(),
                                                dataSlice = itemData.GetSlice(e.GetAxisPoint()),
                                                oidMeasure = dataSlice.GetMeasures().find(findMeasure).Id,
                                                StatusMeasure = dataSlice.GetMeasures().find(findMeasureStatus).Id,
                                                statusValue = dataSlice.GetMeasureValue(StatusMeasure);
                                                measureValue = dataSlice.GetMeasureValue(oidMeasure);
                                                {0}
                                        }}
                                    }}";
                dashboardControl.ClientSideEvents.ItemClick = string.Format(widgetScript, callbackManager.GetScript(HandlerName, "measureValue.GetValue()+','+statusValue.GetValue()"));
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void ProcessAction(string parameter)
        {
            var Ty = parameter.GetType();
            openDetailViewAction.DoExecute(parameter);
        }
        private void OpenDetailViewAction_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            try
            {
                string[] splitString = e.ParameterCurrentValue.ToString().Split(',');
                if (splitString.Count() > 0)
                {
                    string strOid = splitString[0].ToString();
                    string strStatus = splitString[1].ToString();
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    if (View != null && View.CurrentObject != null && ((DashboardData)View.CurrentObject).Title == "My BackLog")
                    {

                        //if (TestMethod != null)
                        //{
                        if (strStatus == "0") //"Pending Entry")
                        {
                            string TestName = string.Empty;
                            TestMethod TestMethod = objectSpace.FindObject<TestMethod>(new BinaryOperator("Oid", strOid));
                            if (TestMethod != null)
                            {
                                TestName = TestMethod.TestName;
                            }

                            CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleParameter));
                            //cs.Criteria["filter"] = CriteriaOperator.Parse("[Oid]=NULL");
                            //ListView listview = Application.CreateListView("RawDataResultEntry_ListView", cs, true);
                            //e.ShowViewParameters.CreatedView = listview;
                            //RedirectToSDMS("SDMS", strOid, strStatus);
                            //return;

                            ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultEntry", cs, true);
                            listview.CollectionSource.Criteria["filter"] = GroupOperator.And(new BinaryOperator("Testparameter.TestMethod.TestName", TestName), new BinaryOperator("Status", strStatus), new BinaryOperator("SubOut", "0"));
                            //objQPInfo.ResultEntryQueryFilter = "[Testparameter.TestMethod.TestName] == '" + TestName + "' AND [Status]='"+ strStatus +"'";
                            objQPInfo.FromDashboard = true;//for restrict the default criteria applying from the ResultEntry Views in ResultEntryQueryPanelWebViewController
                            e.ShowViewParameters.CreatedView = listview;
                            return;
                        }
                        else if (strStatus == "Pending Review")
                        {
                            CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleParameter));
                            cs.Criteria["filter"] = CriteriaOperator.Parse("[Oid]=NULL");
                            ListView listview = Application.CreateListView("RawDataResultEntry_ListView", cs, true);

                            e.ShowViewParameters.CreatedView = listview;
                            RedirectToSDMS("RawDataResultReview", strOid, strStatus);
                            return;

                        }
                        else if (strStatus == "Pending Verify")
                        {
                            CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleParameter));
                            cs.Criteria["filter"] = CriteriaOperator.Parse("[Oid]=NULL");
                            ListView listview = Application.CreateListView("RawDataResultEntry_ListView", cs, true);

                            e.ShowViewParameters.CreatedView = listview;
                            RedirectToSDMS("RawDataResultVerify", strOid, strStatus);
                            return;

                        }
                        //else if (strStatus == "Pending Validation")
                        //{
                        //    ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultValidation", cs, true);
                        //    listview.CollectionSource.Criteria["filter"] = GroupOperator.And(new BinaryOperator("Testparameter.TestMethod.TestName", TestName), new BinaryOperator("Status", strStatus));
                        //    //objQPInfo.ResultEntryQueryFilter = "[Testparameter.TestMethod.TestName] == '" + TestName + "' AND [Status]='" + strStatus + "'";
                        //    objQPInfo.FromDashboard = true;//for restrict the default criteria applying from the ResultEntry Views in ResultEntryQueryPanelWebViewController
                        //    e.ShowViewParameters.CreatedView = listview;
                        //    return;
                        //}
                        //else if (strStatus == "Pending Approval")
                        //{
                        //    ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultApproval", cs, true);
                        //    listview.CollectionSource.Criteria["filter"] = GroupOperator.And(new BinaryOperator("Testparameter.TestMethod.TestName", TestName), new BinaryOperator("Status", strStatus));
                        //    //objQPInfo.ResultEntryQueryFilter = "[Testparameter.TestMethod.TestName] == '" + TestName + "' AND [Status]='" + strStatus + "'";
                        //    objQPInfo.FromDashboard = true;//for restrict the default criteria applying from the ResultEntry Views in ResultEntryQueryPanelWebViewController
                        //    e.ShowViewParameters.CreatedView = listview;
                        //    return;
                        //}                         
                        //else
                        //    {
                        //    TestMethod TestMethod = objectSpace.FindObject<TestMethod>(new BinaryOperator("Oid", strOid));
                        //    if (TestMethod != null)
                        //    {
                        //        string TestName = TestMethod.TestName;
                        //        CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleParameter));
                        //        ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultEntry", cs, true);
                        //        listview.CollectionSource.Criteria["filter"] = GroupOperator.And(new BinaryOperator("Testparameter.TestMethod.TestName", TestName), new BinaryOperator("Status", strStatus));
                        //        //objQPInfo.ResultEntryQueryFilter = "[Testparameter.TestMethod.TestName] == '" + TestName + "' AND [Status]='" + strStatus + "'";
                        //        objQPInfo.FromDashboard = true;//for restrict the default criteria applying from the ResultEntry Views in ResultEntryQueryPanelWebViewController
                        //        e.ShowViewParameters.CreatedView = listview;
                        //        return;
                        //    }             
                        //}
                        else if (strStatus == "SampleLogIn")
                        {

                            IObjectSpace os = Application.CreateObjectSpace();
                            Samplecheckin SCObj = os.FindObject<Samplecheckin>(new BinaryOperator("Oid", strOid));
                            //SampleLogIn SLObj = os.CreateObject<SampleLogIn>();
                            objSCInfo.JobID = SCObj.JobID;//assign the newly created object to the Info Variable its initiates the Default values like sampleID,Received date etc..
                                                          //if (SLObj != null)
                                                          //{
                            DetailView detailview = Application.CreateDetailView(os, "Samplecheckin_DetailView_Copy_SampleRegistration", true, SCObj);
                            detailview.ViewEditMode = ViewEditMode.Edit;
                            e.ShowViewParameters.CreatedView = detailview;
                            return;
                            //}

                        }
                    }
                    TestMethod contact = objectSpace.FindObject<TestMethod>(new BinaryOperator("Oid", e.ParameterCurrentValue.ToString()));
                    if (contact != null)
                    {
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, contact, View);
                        return;
                    }
                    Customer customer = objectSpace.FindObject<Customer>(new BinaryOperator("Oid", e.ParameterCurrentValue.ToString()));
                    if (customer != null)
                    {
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, customer, View);
                        return;
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
            try
            {
                WebDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
                if (dashboardViewerViewItem != null)
                {
                    dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
                }
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void RedirectToSDMS(string NavigationID, string Oid, string status)
        {
            try
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(SecuritySystem.CurrentUserName);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                var encname = Convert.ToBase64String(resultArray, 0, resultArray.Length);

                bool boolWrite = false;
                bool boolDelete = false;
                var Write = "";
                var Delete = "";
                IObjectSpace ObjectSpace = Application.CreateObjectSpace();
                if (NavigationID == "SDMS" || NavigationID == "RawDataResultReview" || NavigationID == "RawDataResultVerify")
                {
                    Employee currentUser = SecuritySystem.CurrentUser as Employee;
                    CriteriaOperator criteria = null;
                    criteria = CriteriaOperator.Parse("[NavigationId]='" + NavigationID + "'and [GCRecord] is NULL");
                    Modules.BusinessObjects.Setting.NavigationItem objNavigation = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(criteria);

                    if (objNavigation != null)
                    {
                        CriteriaOperator criteria1 = CriteriaOperator.Parse("[User]='" + currentUser.Oid + "' and [GCRecord] is NULL");
                        UserNavigationPermission objUserNavigationPermission = ObjectSpace.FindObject<UserNavigationPermission>(criteria1);

                        if (objUserNavigationPermission != null)
                        {
                            CriteriaOperator criteria2 = CriteriaOperator.Parse("[NavigationItem]='" + objNavigation.Oid + "' and [UserNavigationPermission]='" + objUserNavigationPermission.Oid + "'and [GCRecord] is NULL");
                            UserNavigationPermissionDetails objNavigationPermission = ObjectSpace.FindObject<UserNavigationPermissionDetails>(criteria2);
                            boolWrite = objNavigationPermission.Write;
                            boolDelete = objNavigationPermission.Delete;
                        }
                    }
                    byte[] inputAr = UTF8Encoding.UTF8.GetBytes(boolWrite.ToString());
                    TripleDESCryptoServiceProvider tripleDE = new TripleDESCryptoServiceProvider();
                    tripleDE.Key = UTF8Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
                    tripleDE.Mode = CipherMode.ECB;
                    tripleDE.Padding = PaddingMode.PKCS7;
                    ICryptoTransform Transform = tripleDE.CreateEncryptor();
                    byte[] result = Transform.TransformFinalBlock(inputAr, 0, inputAr.Length);
                    tripleDE.Clear();
                    Write = Convert.ToBase64String(result, 0, result.Length);


                    byte[] inputDelete = UTF8Encoding.UTF8.GetBytes(boolDelete.ToString());
                    TripleDESCryptoServiceProvider tripleDelete = new TripleDESCryptoServiceProvider();
                    tripleDelete.Key = UTF8Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
                    tripleDelete.Mode = CipherMode.ECB;
                    tripleDelete.Padding = PaddingMode.PKCS7;
                    ICryptoTransform TransformDelete = tripleDelete.CreateEncryptor();
                    byte[] resultDelete = TransformDelete.TransformFinalBlock(inputDelete, 0, inputDelete.Length);
                    tripleDelete.Clear();
                    Delete = Convert.ToBase64String(resultDelete, 0, resultDelete.Length);
                    var url = ConfigurationManager.AppSettings["SDMSurl"];
                    if (NavigationID == "RawDataResultReview")
                    {
                        url = ConfigurationManager.AppSettings["SDMSurlreview"];
                    }
                    else if (NavigationID == "RawDataResultVerify")
                    {
                        url = ConfigurationManager.AppSettings["SDMSurlverify"];
                    }
                    WebApplication.Redirect(url + "?url=" + HttpContext.Current.Request.Url.AbsoluteUri + "&user=" + encname + "&Write=" + Write + "&Delete=" + Delete + "&ABID=" + Oid);
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
