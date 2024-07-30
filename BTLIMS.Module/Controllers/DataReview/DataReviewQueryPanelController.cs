using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Web.Controllers.ResultEntry
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DataReviewQueryPanelController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        bool QueryPanel = false;
        viewInfo strviewid = new viewInfo();
        AnalysisDeptUser analysisDeptUser = new AnalysisDeptUser();
        ShowNavigationItemController ShowNavigationController;
        QCResultValidationQueryPanelInfo objQPInfo = new QCResultValidationQueryPanelInfo();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        ASPxPageControl pageControl = null;
        XafCallbackManager callbackManager;
        curlanguage objLanguage = new curlanguage();
        public DataReviewQueryPanelController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "ResultEntry_Validation;" + "ResultEntry_Approval;" + "ResultValidationQueryPanel_DetailView_ResultValidation;"
                + "ResultValidationQueryPanel_DetailView_ResultApproval;" + "SampleParameter_ListView_ResultValidation_QueryPanel;"
                + "SampleParameter_ListView_ResultApproval_QueryPanel;" + "SampleParameter_ListView_Copy_ResultValidation;"
                + "SampleParameter_ListView_Copy_ResultApproval;" + "QCBatch_ListView_ResultValidation;" + "QCBatch_ListView_ResultApproval;"
                + "SampleParameter_ListView_ResultValidation_ABID;" + "SampleParameter_ListView_ResultApproval_ABID;"
                + "SampleParameter_ListView_Copy_QCResultValidation;" + "SampleParameter_ListView_Copy_QCResultApproval;"
                + "ResultValidationQueryPanel_DetailView_ResultApproval_View;" + "ResultValidationQueryPanel_DetailView_ResultValidation_View;"
                + "SampleParameter_ListView_ResultValidation_ABID_Level1Review;" + "SampleParameter_ListView_ResultApproval_ABID_Level2Review;"
                + "SampleParameter_ListView_ResultValidation_QueryPanel_Level1Review;" + "SampleParameter_ListView_ResultApproval_QueryPanel_Level2Review;"
                + "QCBatch_ListView_ResultValidation_Level1Review;" + "QCBatch_ListView_ResultApproval_Level2Review;"
                + "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview;" + "SampleParameter_ListView_Copy_ResultApproval_Level2Review;"
                + "SampleParameter_ListView_Copy_QCResultValidation_Level1Review;" + "SampleParameter_ListView_Copy_QCResultApproval_Level2Review;"
                + "Samplecheckin_ListView_ResultValidation;" + "Samplecheckin_ListView_ResultApproval;" + "Samplecheckin_ListView_ResultValidation_View;" + "Samplecheckin_ListView_ResultApproval_View;";
            dataReviewDateFilterAction.TargetViewId = "ResultValidationQueryPanel_DetailView_ResultValidation;" + "Samplecheckin_ListView_ResultValidation;" + "Samplecheckin_ListView_ResultApproval;"
                + "ResultValidationQueryPanel_DetailView_ResultApproval;" + "ResultValidationQueryPanel_DetailView_ResultApproval_View;" + "ResultValidationQueryPanel_DetailView_ResultValidation_View;" + "Samplecheckin_ListView_ResultValidation_View;" + "Samplecheckin_ListView_ResultApproval_View;";
            SDMS.TargetViewId = "ResultEntry_Validation;" + "ResultEntry_Approval;";
            dataReviewNextAction.TargetViewId = "Samplecheckin_ListView_ResultValidation;" + "Samplecheckin_ListView_ResultApproval;" + "Samplecheckin_ListView_ResultValidation_View;"
                + "Samplecheckin_ListView_ResultApproval_View;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if ((ObjectSpace is NonPersistentObjectSpace) && (View.CurrentObject == null))
                {
                    View.CurrentObject = View.ObjectTypeInfo.CreateInstance();
                    ((DetailView)View).ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                }
                //ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                if (View.Id == "ResultValidationQueryPanel_DetailView_ResultValidation"
                    || View.Id == "ResultValidationQueryPanel_DetailView_ResultApproval"
                    || View.Id == "ResultValidationQueryPanel_DetailView_ResultApproval_View"
                    || View.Id == "ResultValidationQueryPanel_DetailView_ResultValidation_View")
                {
                    objQPInfo.lstJobID = new List<string>();
                    objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    dataReviewDateFilterAction.SelectedItemChanged += DataReviewDateFilterAction_SelectedItemChanged;
                    //DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                    //if (viQC != null)
                    //{
                    //    //viQC.ControlCreated += ViQC_ControlCreated;
                    //}
                    //DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                    //if (viJob != null)
                    //{
                    //    //viJob.ControlCreated += ViJob_ControlCreated;
                    //}
                    //DashboardViewItem viABID = (DashboardViewItem)((DetailView)View).FindItem("viewItemABID");
                    //if (viABID != null)
                    //{
                    //    //viABID.ControlCreated += ViABID_ControlCreated;
                    //}
                    if (dataReviewDateFilterAction.SelectedItem == null)
                    {
                        dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[0];
                    }
                }
                if (View.Id == "Samplecheckin_ListView_ResultValidation" || View.Id == "Samplecheckin_ListView_ResultApproval")
                {
                    objQPInfo.lstJobID = new List<string>();
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (dataReviewDateFilterAction.SelectedItem == null)
                    {
                        if (setting.AnalysisReviewLevel == EnumDateFilter.OneMonth)
                        {
                        dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[0];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.ThreeMonth)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[1];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.SixMonth)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[2];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.OneYear)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[3];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.TwoYear)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[4];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-2);
                    }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.FiveYear)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[5];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-5);
                        }
                        else
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[6];
                            //objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }
                    //if (dataReviewDateFilterAction.SelectedItem == null)
                    //{
                    //    dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[0];
                    //}
                    //objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    //Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    //SelectedData sproc = null;
                    //if (View.Id == "Samplecheckin_ListView_ResultValidation")
                    //{
                    //    sproc = currentSession.ExecuteSproc("GetResultValidationData", new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                    //}
                    //else
                    //{
                    //    sproc = currentSession.ExecuteSproc("GetResultApprovalData", new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                    //}
                    //List<Guid> lstsmploid = new List<Guid>();
                    //List<object> listobj = new List<object>();
                    ////foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                    ////{
                    ////    if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                    ////    {
                    ////        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                    ////    }
                    ////}
                    //lstsmploid = sproc.ResultSet[0].Rows.Where(i => i.Values[1] != null).Select(i => new Guid(i.Values[1].ToString())).Distinct().ToList();
                    //if (lstsmploid.Count > 0)
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                    //}
                    dataReviewDateFilterAction.SelectedItemChanged += DataReviewDateFilterAction_SelectedItemChanged;
                }
                else if (View.Id == "Samplecheckin_ListView_ResultValidation_View" || View.Id == "Samplecheckin_ListView_ResultApproval_View")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (dataReviewDateFilterAction.SelectedItem == null)
                    {
                        if (setting.AnalysisReviewLevel == EnumDateFilter.OneMonth)
                        {
                        dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[0];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.ThreeMonth)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[1];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.SixMonth)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[2];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.OneYear)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[3];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                    }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.TwoYear)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[4];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-2);
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.FiveYear)
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[5];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-5);
                        }
                        else
                        {
                            dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[6];
                            objQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }
                    //if (dataReviewDateFilterAction.SelectedItem == null)
                    //{
                    //    dataReviewDateFilterAction.SelectedItem = dataReviewDateFilterAction.Items[0];
                    //}
                    //objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    //Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    //SelectedData sproc = null;


                    //if (View.Id == "Samplecheckin_ListView_ResultValidation_View")
                    //{
                    //    sproc = currentSession.ExecuteSproc("GetResultValidationDataView", new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                    //}
                    //else
                    //{
                    //    sproc = currentSession.ExecuteSproc("GetResultApprovalDataView", new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                    //}



                    //List<Guid> lstsmploid = new List<Guid>();
                    //List<object> listobj = new List<object>();
                    ////foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                    ////{
                    ////    if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                    ////    {
                    ////        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                    ////    }
                    ////}
                    //lstsmploid = sproc.ResultSet[0].Rows.Where(i => i.Values[1] != null).Select(i => new Guid(i.Values[1].ToString())).Distinct().ToList();
                    //if (lstsmploid.Count > 0)
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                    //}
                    dataReviewDateFilterAction.SelectedItemChanged += DataReviewDateFilterAction_SelectedItemChanged;
                }
                else if (View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel" || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel"
                    || View.Id == "QCBatch_ListView_ResultApproval" || View.Id == "QCBatch_ListView_ResultValidation"
                    || View.Id == "SampleParameter_ListView_ResultApproval_ABID" || View.Id == "SampleParameter_ListView_ResultValidation_ABID"
                    || View.Id == "SampleParameter_ListView_ResultValidation_ABID_Level1Review" || View.Id == "SampleParameter_ListView_ResultApproval_ABID_Level2Review"
                    || View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel_Level1Review" || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel_Level2Review"
                    || View.Id == "QCBatch_ListView_ResultValidation_Level1Review" || View.Id == "QCBatch_ListView_ResultApproval_Level2Review")
                {
                    View.SelectionChanged += View_SelectionChanged;
                }
                else if (View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review")
                {
                    //Doubt
                    bool isAdministrator = false;
                    List<string> lstTests = new List<string>();
                    string strTestMethodsPermissionCriteria = string.Empty;

                    CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        isAdministrator = true;
                    }
                    else
                    {
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                        if (View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview")
                        {
                            lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] = True", currentUser.Oid));
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review")
                        {
                            lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultApproval] = True", currentUser.Oid));
                        }
                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            List<Guid> lstTestsoid = new List<Guid>();
                            strviewid.strtempviewid = View.Id;
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (!lstTests.Contains(testMethod.TestName))
                                    {
                                        lstTests.Add(testMethod.TestName);
                                    }
                                    if (!lstTestsoid.Contains(testMethod.Oid))
                                    {
                                        lstTestsoid.Add(testMethod.Oid);
                                    }
                                }
                            }
                            if (lstTestsoid.Count > 0)
                            {
                                analysisDeptUser.lstAnalysisEmp = new List<Employee>();
                                foreach (Guid objtm in lstTestsoid.ToList())
                                {
                                    TestMethod testMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", objtm));
                                    if (testMethod != null && testMethod.ResultEntryUsers != null)
                                    {
                                        string[] stremparr = testMethod.ResultEntryUsers.Split(',');
                                        foreach (string strempname in stremparr.ToList())
                                        {
                                            Employee employee = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[DisplayName] = ?", strempname));
                                            if (employee != null)
                                            {
                                                if (!analysisDeptUser.lstAnalysisEmp.Contains(employee))
                                                {
                                                    analysisDeptUser.lstAnalysisEmp.Add(employee);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //strTestMethodsPermissionCriteria = "And [Testparameter.TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestMethodOids.Select(i => i.ToString().Replace("'", "''")))) + ") And [GCRecord] IS NULL";
                    }
                    if (string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                    {
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]=True or [UQTESTPARAMETERID.InternalStandard]=True)");
                    }
                    else
                    {
                        //CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]=True or [UQTESTPARAMETERID.InternalStandard]=True)");
                        CriteriaOperator criteria = CriteriaOperator.Parse(objQPInfo.SampleResultValidationQueryFilter);
                        if (isAdministrator)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = criteria;
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = new GroupOperator(GroupOperatorType.And, criteria,
                                new InOperator("Testparameter.TestMethod.TestName", lstTests));
                        }
                    }
                }
                else if (View.Id == "SampleParameter_ListView_Copy_QCResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                    || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review")
                {
                    //doubt
                    bool isAdministrator = false;
                    List<string> lstTests = new List<string>();
                    string strTestMethodsPermissionCriteria = string.Empty;

                    CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        isAdministrator = true;
                    }
                    else
                    {
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                        if (View.Id == "SampleParameter_ListView_Copy_QCResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review")
                        {
                            lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] = True", currentUser.Oid));
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_QCResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review")
                        {
                            lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultApproval] = True", currentUser.Oid));
                        }
                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            List<Guid> lstTestsoid = new List<Guid>();
                            strviewid.strtempviewid = View.Id;
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (!lstTests.Contains(testMethod.TestName))
                                    {
                                        lstTests.Add(testMethod.TestName);
                                    }
                                }
                            }
                            if (lstTestsoid.Count > 0)
                            {
                                analysisDeptUser.lstAnalysisEmp = new List<Employee>();
                                foreach (Guid objtm in lstTestsoid.ToList())
                                {
                                    TestMethod testMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", objtm));
                                    if (testMethod != null && testMethod.ResultEntryUsers != null)
                                    {
                                        string[] stremparr = testMethod.ResultEntryUsers.Split(',');
                                        foreach (string strempname in stremparr.ToList())
                                        {
                                            Employee employee = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[DisplayName] = ?", strempname));
                                            if (employee != null)
                                            {
                                                if (!analysisDeptUser.lstAnalysisEmp.Contains(employee))
                                                {
                                                    analysisDeptUser.lstAnalysisEmp.Add(employee);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //strTestMethodsPermissionCriteria = "And [Testparameter.TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestMethodOids.Select(i => i.ToString().Replace("'", "''")))) + ") And [GCRecord] IS NULL";
                    }

                    if (string.IsNullOrEmpty(objQPInfo.QCResultValidationQueryFilter))
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                    }
                    else
                    {
                        //CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]=True or [UQTESTPARAMETERID.InternalStandard]=True)");

                        CriteriaOperator criteria = CriteriaOperator.Parse(objQPInfo.QCResultValidationQueryFilter);
                        if (isAdministrator)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = criteria;
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = new GroupOperator(GroupOperatorType.And, criteria,
                               new InOperator("Testparameter.TestMethod.TestName", lstTests));
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DataReviewDateFilterAction_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ObjectSpace.GetType() == typeof(XPObjectSpace))
                {
                    Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    if (View != null && dataReviewDateFilterAction != null && dataReviewDateFilterAction.SelectedItem != null && (View.Id == "ResultValidationQueryPanel_DetailView_ResultValidation" || View.Id == "ResultValidationQueryPanel_DetailView_ResultApproval" || View.Id == "ResultValidationQueryPanel_DetailView_ResultApproval_View" || View.Id == "ResultValidationQueryPanel_DetailView_ResultValidation_View")
                        || View.Id == "Samplecheckin_ListView_ResultValidation" || View.Id == "Samplecheckin_ListView_ResultApproval")
                    {
                        //objQPInfo.rgFilterByMonthDate = new DateTime();
                        if (dataReviewDateFilterAction != null && dataReviewDateFilterAction.SelectedItem != null)
                        {
                            if (dataReviewDateFilterAction.SelectedItem.Id == "1M")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "3M")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "6M")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "1Y")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "2Y")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-2);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "5Y")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-5);
                            }
                            else
                            {
                                objQPInfo.rgFilterByMonthDate = new DateTime(1753, 1, 1);
                            }
                        }
                        if (View is DetailView)
                        {
                            DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                            DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                            DashboardViewItem viABID = (DashboardViewItem)((DetailView)View).FindItem("viewItemABID");
                            //View.Refresh();
                            if (viJob != null && viJob.InnerView != null)
                            {
                                if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                                {
                                    ((ListView)viJob.InnerView).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("Samplelogin.JobID.RecievedDate>=? and Samplelogin.JobID.RecievedDate<?", objQPInfo.rgFilterByMonthDate, DateTime.Now);
                                    //((DevExpress.ExpressApp.ListView)viJob.InnerView).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "') And [SignOff] = True");
                                }
                                else
                                {
                                    if (((DevExpress.ExpressApp.ListView)viJob.InnerView).CollectionSource.Criteria.ContainsKey("dateFilter"))
                                    {
                                        ((DevExpress.ExpressApp.ListView)viJob.InnerView).CollectionSource.Criteria.Remove("dateFilter");
                                    }
                                }
                                ((DevExpress.ExpressApp.ListView)viJob.InnerView).Refresh();
                            }
                            if (viQC != null && viQC.InnerView != null)
                            {
                                if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                                {
                                    ((DevExpress.ExpressApp.ListView)viQC.InnerView).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Datecreated] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')");
                                }
                                else
                                {
                                    if (((DevExpress.ExpressApp.ListView)viQC.InnerView).CollectionSource.Criteria.ContainsKey("dateFilter"))
                                    {
                                        ((ListView)viQC.InnerView).CollectionSource.Criteria.Remove("dateFilter");
                                    }
                                }
                                ((DevExpress.ExpressApp.ListView)viJob.InnerView).Refresh();
                            }
                            if (viQC != null && viQC.InnerView != null)
                            {
                                ((DevExpress.ExpressApp.ListView)viQC.InnerView).Refresh();
                            }
                            if (viABID != null && viABID.InnerView != null)
                            {
                                if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                                {
                                    ((DevExpress.ExpressApp.ListView)viABID.InnerView).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[UQABID.CreatedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "') And [SignOff] = True");
                                }
                                else
                                {
                                    if (((DevExpress.ExpressApp.ListView)viABID.InnerView).CollectionSource.Criteria.ContainsKey("dateFilter"))
                                    {
                                        ((DevExpress.ExpressApp.ListView)viABID.InnerView).CollectionSource.Criteria.Remove("dateFilter");
                                    }
                                }
                                ((DevExpress.ExpressApp.ListView)viJob.InnerView).Refresh();
                            }
                            if (viQC != null && viQC.InnerView != null)
                            {
                                ((DevExpress.ExpressApp.ListView)viQC.InnerView).Refresh();
                            }
                            if (viABID != null && viABID.InnerView != null)
                            {
                                ((DevExpress.ExpressApp.ListView)viABID.InnerView).Refresh();
                            }
                        }
                        else if (View.Id == "Samplecheckin_ListView_ResultValidation" || View.Id == "Samplecheckin_ListView_ResultApproval")
                        {

                            bool isAdministrator = false;
                            Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                            if (currentUser != null && currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                            {
                                isAdministrator = true;
                            }
                            if (View.Id == "Samplecheckin_ListView_ResultValidation")
                            {
                                if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                                {
                                    SelectedData sproc = currentSession.ExecuteSproc("GetResultValidationData", new OperandValue(currentUser.DisplayName), new OperandValue(isAdministrator), new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                    List<Guid> lstsmploid = new List<Guid>();
                                    List<object> listobj = new List<object>();
                                    //foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                                    //{
                                    //    if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                    //    {
                                    //        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                    //    }
                                    //}
                                    lstsmploid = sproc.ResultSet[0].Rows.Where(i => i.Values[1] != null).Select(i => new Guid(i.Values[1].ToString())).Distinct().ToList();
                                    if (lstsmploid.Count > 0)
                                    {
                                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                                    }
                                    else
                                    {
                                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                                    }
                                }
                                else
                                {
                                    SelectedData sproc = currentSession.ExecuteSproc("GetResultValidationData", new OperandValue(currentUser.DisplayName), new OperandValue(isAdministrator), new OperandValue("DateFilterRemove"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                    List<Guid> lstsmploid = new List<Guid>();
                                    //foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                                    //{
                                    //    if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                    //    {
                                    //        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                    //    }
                                    //}
                                    lstsmploid = sproc.ResultSet[0].Rows.Where(i => i.Values[1] != null).Select(i => new Guid(i.Values[1].ToString())).Distinct().ToList();
                                    if (lstsmploid.Count > 0)
                                    {
                                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                                    }
                                    else
                                    {
                                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                                    }
                                }
                            }

                            else if (View.Id == "Samplecheckin_ListView_ResultApproval")
                            {
                                if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                                {
                                    SelectedData sproc = currentSession.ExecuteSproc("GetResultApprovalData", new OperandValue(currentUser.DisplayName), new OperandValue(isAdministrator), new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                    List<Guid> lstsmploid = new List<Guid>();
                                    List<object> listobj = new List<object>();
                                    //foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                                    //{
                                    //    if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                    //    {
                                    //        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                    //    }
                                    //}
                                    lstsmploid = sproc.ResultSet[0].Rows.Where(i => i.Values[1] != null).Select(i => new Guid(i.Values[1].ToString())).Distinct().ToList();
                                    if (lstsmploid.Count > 0)
                                    {
                                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                                    }
                                    else
                                    {
                                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                                    }
                                }
                                else
                                {
                                    SelectedData sproc = currentSession.ExecuteSproc("GetResultApprovalData", new OperandValue("DateFilterRemove"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                    List<Guid> lstsmploid = new List<Guid>();
                                    //foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                                    //{
                                    //    if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                    //    {
                                    //        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                    //    }
                                    //}
                                    lstsmploid = sproc.ResultSet[0].Rows.Where(i => i.Values[1] != null).Select(i => new Guid(i.Values[1].ToString())).Distinct().ToList();
                                    if (lstsmploid.Count > 0)
                                    {
                                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                                    }
                                    else
                                    {
                                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                                    }
                                }
                            }

                        }
                    }
                    else if (View.Id == "Samplecheckin_ListView_ResultValidation_View")
                    {
                        if (dataReviewDateFilterAction != null && dataReviewDateFilterAction.SelectedItem != null)
                        {
                            if (dataReviewDateFilterAction.SelectedItem.Id == "1M")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "3M")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "6M")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "1Y")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                            }
                            else
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                            }
                        }
                        if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                        {
                            SelectedData sproc = currentSession.ExecuteSproc("GetResultValidationDataView", new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                            List<Guid> lstsmploid = new List<Guid>();
                            List<object> listobj = new List<object>();
                            foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                            {
                                if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                {
                                    lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                }
                            }
                            //lstsmploid = sproc.ResultSet[0].Rows.Select(i => new Guid(i.Values.ToString())).Distinct().ToList();
                            if (lstsmploid.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                            }
                        }
                        else
                        {
                            SelectedData sproc = currentSession.ExecuteSproc("GetResultValidationDataView", new OperandValue("DateFilterRemove"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                            List<Guid> lstsmploid = new List<Guid>();
                            foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                            {
                                if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                {
                                    lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                }
                            }
                            if (lstsmploid.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                            }
                        }
                    }


                    else if (View.Id == "Samplecheckin_ListView_ResultApproval_View")
                    {
                        if (dataReviewDateFilterAction != null && dataReviewDateFilterAction.SelectedItem != null)
                        {
                            if (dataReviewDateFilterAction.SelectedItem.Id == "1M")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "3M")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "6M")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                            }
                            else if (dataReviewDateFilterAction.SelectedItem.Id == "1Y")
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                            }
                            else
                            {
                                objQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                            }
                        }
                        if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                        {
                            SelectedData sproc = currentSession.ExecuteSproc("GetResultApprovalDataView", new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                            List<Guid> lstsmploid = new List<Guid>();
                            List<object> listobj = new List<object>();
                            foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                            {
                                if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                {
                                    lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                }
                            }
                            //lstsmploid = sproc.ResultSet[0].Rows.Select(i => new Guid(i.Values.ToString())).Distinct().ToList();
                            if (lstsmploid.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                            }
                        }
                        else
                        {
                            SelectedData sproc = currentSession.ExecuteSproc("GetResultApprovalDataView", new OperandValue("DateFilterRemove"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                            List<Guid> lstsmploid = new List<Guid>();
                            foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                            {
                                if (!lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                {
                                    lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                }
                            }
                            if (lstsmploid.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Oid is null");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void View_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel" || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel" || View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel_Level1Review" || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel_Level2Review")
                {
                    if (objQPInfo.lstJobID == null)
                    {
                        objQPInfo.lstJobID = new List<string>();
                    }
                    else
                    {
                        objQPInfo.lstJobID.Clear();
                    }

                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (SampleParameter obj in View.SelectedObjects)
                        {
                            if (objQPInfo.lstJobID != null && !objQPInfo.lstJobID.Contains(obj.Samplelogin.JobID.JobID))
                            {
                                objQPInfo.lstJobID.Add(obj.Samplelogin.JobID.JobID);
                            }
                        }
                    }
                }
                else if (View.Id == "QCBatch_ListView_ResultApproval" || View.Id == "QCBatch_ListView_ResultValidation" || View.Id == "QCBatch_ListView_ResultValidation_Level1Review" || View.Id == "QCBatch_ListView_ResultApproval_Level2Review")
                {
                    if (objQPInfo.lstQCBatchID == null)
                    {
                        objQPInfo.lstQCBatchID = new List<string>();
                    }
                    else
                    {
                        objQPInfo.lstQCBatchID.Clear();
                    }

                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (SpreadSheetEntry_AnalyticalBatch obj in View.SelectedObjects)
                        {
                            if (objQPInfo.lstQCBatchID != null && !objQPInfo.lstJobID.Contains(obj.AnalyticalBatchID))
                            {
                                objQPInfo.lstQCBatchID.Add(obj.AnalyticalBatchID);
                            }
                        }
                    }
                }
                else if (View.Id == "SampleParameter_ListView_ResultApproval_ABID" || View.Id == "SampleParameter_ListView_ResultValidation_ABID" || View.Id == "SampleParameter_ListView_ResultValidation_ABID_Level1Review" || View.Id == "SampleParameter_ListView_ResultApproval_ABID_Level2Review")
                {
                    if (objQPInfo.lstQCBatchID == null)
                    {
                        objQPInfo.lstQCBatchID = new List<string>();
                    }
                    else
                    {
                        objQPInfo.lstQCBatchID.Clear();
                    }

                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (SampleParameter obj in View.SelectedObjects)
                        {
                            if (objQPInfo.lstQCBatchID != null && !objQPInfo.lstJobID.Contains(obj.ABID))
                            {
                                objQPInfo.lstQCBatchID.Add(obj.ABID);
                            }
                        }
                    }
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
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "FilterDataByMonth")
                {
                    objQPInfo.rgFilterByMonthDate = DateTime.Now;
                    if (View.ObjectTypeInfo.Type == typeof(QCResultValidationQueryPanel))
                    {
                        QCResultValidationQueryPanel REQPanel = (QCResultValidationQueryPanel)e.Object;
                        if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._1M))
                        {
                            objQPInfo.rgFilterByMonthDate = objQPInfo.rgFilterByMonthDate.AddMonths(-1);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._3M))
                        {
                            objQPInfo.rgFilterByMonthDate = objQPInfo.rgFilterByMonthDate.AddMonths(-3);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._6M))
                        {
                            objQPInfo.rgFilterByMonthDate = objQPInfo.rgFilterByMonthDate.AddMonths(-6);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._1Y))
                        {
                            objQPInfo.rgFilterByMonthDate = objQPInfo.rgFilterByMonthDate.AddYears(-1);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN.All))
                        {
                            objQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                        }
                    }
                }
                else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "SelectionMode")
                {

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
            try
            {
                if (View != null && View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;

                    if (View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel"
                                    || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel" || View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel_Level1Review"
                                    || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel_Level2Review")
                    {
                        bool isAdministrator = false;
                        List<string> lstTests = new List<string>();
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            if (View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel")
                            {
                                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    isAdministrator = true;
                                }
                                else
                                {
                                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                                    lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] = True", currentUser.Oid));
                                    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                                    {
                                        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                        {
                                            foreach (TestMethod testMethod in departmentChain.TestMethods)
                                            {
                                                if (!lstTests.Contains(testMethod.TestName))
                                                {
                                                    lstTests.Add(testMethod.TestName);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (isAdministrator)
                                {
                                    lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingValidation' And [SignOff] = True And ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample')");
                                }
                                else
                                {
                                    lstview.Criteria = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Status] = 'PendingValidation' And [SignOff] = True And ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample')"),
                                        new InOperator("Testparameter.TestMethod.TestName", lstTests));
                                }
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel")
                            {
                                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    isAdministrator = true;
                                }
                                else
                                {
                                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                                    lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultApproval] = True", currentUser.Oid));
                                    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                                    {
                                        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                        {
                                            foreach (TestMethod testMethod in departmentChain.TestMethods)
                                            {
                                                if (!lstTests.Contains(testMethod.TestName))
                                                {
                                                    lstTests.Add(testMethod.TestName);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (isAdministrator)
                                {
                                    lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingApproval' And [SignOff] = True And ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample')");
                                }
                                else
                                {
                                    lstview.Criteria = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Status]='PendingApproval' And [SignOff] = True And ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample')"),
                                        new InOperator("Testparameter.TestMethod.TestName", lstTests));
                                }
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel_Level1Review")
                            {
                                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    isAdministrator = true;
                                }
                                else
                                {
                                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                                    lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] = True", currentUser.Oid));
                                    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                                    {
                                        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                        {
                                            foreach (TestMethod testMethod in departmentChain.TestMethods)
                                            {
                                                if (!lstTests.Contains(testMethod.TestName))
                                                {
                                                    lstTests.Add(testMethod.TestName);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (isAdministrator)
                                {
                                    lstview.Criteria = CriteriaOperator.Parse("[Status] <> 'PendingEntry' and [Status] <> 'PendingValidation' And [SignOff] = True And ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample')");
                                }
                                else
                                {
                                    lstview.Criteria = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Status] <> 'PendingEntry' and [Status] <> 'PendingValidation' And [SignOff] = True And ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample')"),
                                        new InOperator("Testparameter.TestMethod.TestName", lstTests));
                                }
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel_Level2Review")
                            {
                                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    isAdministrator = true;
                                }
                                else
                                {
                                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                                    lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultApproval] = True", currentUser.Oid));
                                    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                                    {
                                        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                        {
                                            foreach (TestMethod testMethod in departmentChain.TestMethods)
                                            {
                                                if (!lstTests.Contains(testMethod.TestName))
                                                {
                                                    lstTests.Add(testMethod.TestName);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (isAdministrator)
                                {
                                    lstview.Criteria = CriteriaOperator.Parse("[Status] <> 'PendingEntry' and [Status] <> 'PendingValidation' and [Status] <>'PendingApproval' And [SignOff] = True And ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample')");
                                }
                                else
                                {
                                    lstview.Criteria = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Status] <> 'PendingEntry' and [Status] <> 'PendingValidation' and [Status] <>'PendingApproval' And [SignOff] = True And ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample')"),
                                        new InOperator("Testparameter.TestMethod.TestName", lstTests));
                                }
                            }
                            lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> jobid = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                jobid.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", jobid);
                        }

                        if (gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.Load += Grid_Load;
                        }
                        ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                        lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;
                    }
                    else if (View.Id == "QCBatch_ListView_ResultApproval" || View.Id == "QCBatch_ListView_ResultValidation" || View.Id == "QCBatch_ListView_ResultValidation_Level1Review" || View.Id == "QCBatch_ListView_ResultApproval_Level2Review")
                    {
                        bool isAdministrator = false;
                        List<string> lstTests = new List<string>();
                        CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            isAdministrator = true;
                        }
                        else
                        {
                            IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                            if (View.Id == "QCBatch_ListView_ResultValidation" || View.Id == "QCBatch_ListView_ResultValidation_Level1Review")
                            {
                                lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] = True", currentUser.Oid));
                            }
                            else if (View.Id == "QCBatch_ListView_ResultApproval" || View.Id == "QCBatch_ListView_ResultApproval_Level2Review")
                            {
                                lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultApproval] = True", currentUser.Oid));
                            }

                            if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                            {
                                foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                {
                                    foreach (TestMethod testMethod in departmentChain.TestMethods)
                                    {
                                        if (!lstTests.Contains(testMethod.TestName))
                                        {
                                            lstTests.Add(testMethod.TestName);
                                        }
                                    }
                                }
                            }
                            //strTestMethodsPermissionCriteria = "And [Testparameter.TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestMethodOids.Select(i => i.ToString().Replace("'", "''")))) + ") And [GCRecord] IS NULL";
                        }

                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            if (View.Id == "QCBatch_ListView_ResultValidation")
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingValidation' And [SignOff] = True");
                            }
                            else if (View.Id == "QCBatch_ListView_ResultApproval")
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingApproval' And [SignOff] = True");
                            }
                            else if (View.Id == "QCBatch_ListView_ResultValidation_Level1Review")
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[Status] <> 'PendingEntry' and [Status] <> 'PendingValidation' And [SignOff] = True");
                            }
                            else if (View.Id == "QCBatch_ListView_ResultApproval_Level2Review")
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[Status] <> 'PendingEntry' and [Status] <> 'PendingValidation' and [Status] <>'PendingApproval' And [SignOff] = True");
                            }
                            lstview.Properties.Add(new ViewProperty("TQCBatchID", SortDirection.Ascending, "QCBatchID.qcseqdetail.QCBatchID", true, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["TQCBatchID"]);
                            if (isAdministrator)
                            {
                                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("QCBatchID", groups);
                            }
                            else
                            {
                                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new GroupOperator(GroupOperatorType.And, new InOperator("QCBatchID", groups),
                                    new InOperator("Test.TestName", lstTests));
                            }
                            //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse(objQPInfo.SampleResultValidationQueryFilter + "([SubOut] is null Or [SubOut]=False)"),
                            //    new InOperator("QCBatchID", groups));
                        }

                        if (gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.Load += Grid_Load;
                        }
                        ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                        lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;
                    }
                    else if (View.Id == "SampleParameter_ListView_ResultValidation_ABID" || View.Id == "SampleParameter_ListView_ResultApproval_ABID"
                        || View.Id == "SampleParameter_ListView_ResultValidation_ABID_Level1Review" || View.Id == "SampleParameter_ListView_ResultApproval_ABID_Level2Review")
                    {
                        bool isAdministrator = false;
                        List<string> lstTests = new List<string>();
                        CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            isAdministrator = true;
                        }
                        else
                        {
                            IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                            if (View.Id == "SampleParameter_ListView_ResultValidation_ABID" || View.Id == "SampleParameter_ListView_ResultValidation_ABID_Level1Review")
                            {
                                lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] = True", currentUser.Oid));
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultApproval_ABID" || View.Id == "SampleParameter_ListView_ResultApproval_ABID_Level2Review")
                            {
                                lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultApproval] = True", currentUser.Oid));
                            }

                            if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                            {
                                foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                {
                                    foreach (TestMethod testMethod in departmentChain.TestMethods)
                                    {
                                        if (!lstTests.Contains(testMethod.TestName))
                                        {
                                            lstTests.Add(testMethod.TestName);
                                        }
                                    }
                                }
                            }
                        }

                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            if (View.Id == "SampleParameter_ListView_ResultValidation_ABID")
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingValidation' And [SignOff] = True");
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultApproval_ABID")
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingApproval' And [SignOff] = True");
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultValidation_ABID_Level1Review")
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[Status] <> 'PendingEntry' and [Status] <> 'PendingValidation' And [SignOff] = True");
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultApproval_ABID_Level2Review")
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[Status] <> 'PendingEntry' and [Status] <> 'PendingValidation' and [Status] <>'PendingApproval' And [SignOff] = True");
                            }
                            lstview.Properties.Add(new ViewProperty("ABID", SortDirection.Ascending, "UQABID.AnalyticalBatchID", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            if (isAdministrator)
                            {
                                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                            }
                            else
                            {
                                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new GroupOperator(GroupOperatorType.And, new InOperator("Oid", groups),
                                    new InOperator("Testparameter.TestMethod.TestName", lstTests));
                            }
                        }

                        if (gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.Load += Grid_Load;
                        }
                        ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                        lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;
                    }
                    else if (View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                           || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review")
                    {
                        if (gridListEditor != null && gridListEditor.Grid != null && ((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.GetCount() > 0)
                        {
                            gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridListEditor.Grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                            gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            gridListEditor.Grid.JSProperties["cpValidateddateCurrentDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ValidateddateCurrentDate");
                            gridListEditor.Grid.JSProperties["cpValidateddateAnalyzedDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ValidateddateAnalyzedDate");
                            gridListEditor.Grid.JSProperties["cpApprovedDateCurrentDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ApprovedDateCurrentDate");
                            gridListEditor.Grid.JSProperties["cpApprovedDateValidatedDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ApprovedDateValidatedDate");
                            if (View.Id == "SampleParameter_ListView_Copy_ResultValidation")
                            {
                                gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'ValidatedDate' ||  e.focusedColumn.fieldName == 'ValidatedBy.Oid')
                                        {
                                            e.cancel = false;
                                        }
                                        else
                                        {
                                            e.cancel = true;
                                        }    
                                }";
                                gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function (s, e) {
                                window.setTimeout(function () {
                                var dateValidation = s.batchEditApi.GetCellValue(e.visibleIndex, 'ValidatedDate');
                                var dateAnalyzed = s.batchEditApi.GetCellValue(e.visibleIndex, 'AnalyzedDate');
                                var dt = new Date();
                                if (dt != null && dateValidation != null && dateAnalyzed != null)
                                {
                                    if (dateValidation < dateAnalyzed)
                                    {
                                        alert(s.cpValidateddateAnalyzedDatemsg);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ValidatedDate', null);
                                    }
                                    else if (dt < dateValidation)
                                    {
                                        alert(s.cpValidateddateCurrentDatemsg);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ValidatedDate', null);
                                    }
                                }
                                }, 10);
                                }";
                                gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                                gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                    {                        
                        sessionStorage.setItem('ResultValidationFocusedColumn', null); 
                        var fieldName = e.cellInfo.column.fieldName;                       
                        sessionStorage.setItem('ResultValidationFocusedColumn', fieldName);            
                    }";
                                gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            {                      
                            var FocusedColumn = sessionStorage.getItem('ResultValidationFocusedColumn');                                
                            var oid;
                            var text;
                            if(FocusedColumn.includes('.'))
                            {                          
                                if(FocusedColumn ='ValidatedBy.Oid')
                                {
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        {                                                    
                                            if (s.IsRowSelectedOnPage(i)) {
                                            s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                                            }                                                                             
                                        }
                                    }   
                                }    
                             }
                             else
                             {
                                if(FocusedColumn = 'ValidatedDate')
                                {
                                var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                if (e.item.name =='CopyToAllCell')
                                {
                                    for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                    {              
                                        if (s.IsRowSelectedOnPage(i)) {
                                        s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                        }
                                    }
                                }  
                                }
                             }
                         e.processOnServer = false;
                    }";
                            }
                            else if (View.Id == "SampleParameter_ListView_Copy_ResultApproval")
                            {
                                gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'ApprovedDate' ||  e.focusedColumn.fieldName == 'ApprovedBy.Oid')
                                        {
                                            e.cancel = false;           
                                        }
                                        else
                                        {
                                            e.cancel = true;
                                        }    
                                }";
                                gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function (s, e) {
                                window.setTimeout(function () {
                                var dateValidation = s.batchEditApi.GetCellValue(e.visibleIndex, 'ValidatedDate');
                                var dateApproved = s.batchEditApi.GetCellValue(e.visibleIndex, 'ApprovedDate');
                                var dt = new Date();
                                if (dt != null && dateValidation != null && dateApproved != null)
                                {
                                    if (dateApproved < dateValidation)
                                    {
                                        alert(s.cpApprovedDateValidatedDatemsg);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ApprovedDate', null);
                                    }
                                    else if (dt < dateApproved)
                                    {
                                        alert(s.cpApprovedDateCurrentDatemsg);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ApprovedDate', null);
                                    }
                                }
                                }, 10);
                                }";
                                gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                                gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                    {                        
                        sessionStorage.setItem('ResultValidationFocusedColumn', null); 
                        var fieldName = e.cellInfo.column.fieldName;                       
                        sessionStorage.setItem('ResultValidationFocusedColumn', fieldName);            
                    }";
                                gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            {                      
                            var FocusedColumn = sessionStorage.getItem('ResultValidationFocusedColumn');                                
                            var oid;
                            var text;
                            if(FocusedColumn.includes('.'))
                            {                          
                                if(FocusedColumn ='ApprovedBy.Oid')
                                {
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        {                                                    
                                            if (s.IsRowSelectedOnPage(i)) {
                                            s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                                            }                                                                             
                                        }
                                    }   
                                }    
                             }
                             else
                             {        
                                if(FocusedColumn = 'ApprovedDate')
                                {
                                var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                if (e.item.name =='CopyToAllCell')
                                {
                                    for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                    {              
                                        if (s.IsRowSelectedOnPage(i)) {
                                        s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                        }
                                    }
                                }  
                                }                             
                             }
                         e.processOnServer = false;
                    }";
                            }

                            //gridListEditor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                            //gridListEditor.Grid.JSProperties["cpusername"] = SecuritySystem.CurrentUserName;
                            //gridListEditor.Grid.JSProperties["cpPagesize"] = gridListEditor.Grid.SettingsPager.PageSize;
                            //gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = false;
                            //gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e){
                            //    var i = s.cpPagesize * s.GetPageIndex();
                            //    var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                            //    if (e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0)) {
                            //    //for (i; i < totrow; i++) {
                            //    for (var i = 0 ; i < s.GetVisibleRowsOnPage(); i++) {
                            //     if(s.batchEditApi.GetCellValue(i, 'ValidatedDate') != null && s.batchEditApi.HasChanges(i,'ValidatedDate')){
                            //     s.batchEditApi.SetCellValue(i, 'ValidatedDate', null);}
                            //     if(s.batchEditApi.GetCellValue(i, 'ValidatedBy') != null && s.batchEditApi.HasChanges(i,'ValidatedBy')){
                            //     s.batchEditApi.SetCellValue(i, 'ValidatedBy', null);}
                            //    }
                            //    }
                            //    else if(e.visibleIndex == -1 && s.IsRowSelectedOnPage(0)){
                            //    var today = new Date();
                            //    for (var i = 0 ; i < s.GetVisibleRowsOnPage(); i++) { 
                            //    if(s.batchEditApi.GetCellValue(i, 'ValidatedDate') == null && !s.batchEditApi.HasChanges(i,'ValidatedDate')){
                            //    s.batchEditApi.SetCellValue(i, 'ValidatedDate', today);}
                            //    if(s.batchEditApi.GetCellValue(i, 'ValidatedBy') == null && !s.batchEditApi.HasChanges(i,'ValidatedBy')){
                            //    s.batchEditApi.SetCellValue(i, 'ValidatedBy',s.cpuserid, s.cpfullname, false);}
                            //    }
                            //    }
                            //    else{
                            //    if (s.IsRowSelectedOnPage(e.visibleIndex)) {
                            //     var today = new Date();
                            //     if(s.batchEditApi.GetCellValue(e.visibleIndex, 'ValidatedDate') == null && !s.batchEditApi.HasChanges(e.visibleIndex,'ValidatedDate')){
                            //     s.batchEditApi.SetCellValue(e.visibleIndex, 'ValidatedDate', today);}                          
                            //     if(s.batchEditApi.GetCellValue(e.visibleIndex, 'ValidatedBy') == null && !s.batchEditApi.HasChanges(e.visibleIndex,'ValidatedBy')){
                            //     s.batchEditApi.SetCellValue(e.visibleIndex, 'ValidatedBy',s.cpuserid, s.cpfullname, false);}                            
                            //    }
                            //    else{
                            //    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'ValidatedDate') != null && s.batchEditApi.HasChanges(e.visibleIndex,'ValidatedDate')){
                            //     s.batchEditApi.SetCellValue(e.visibleIndex, 'ValidatedDate', null);}                           
                            //     if(s.batchEditApi.GetCellValue(e.visibleIndex, 'ValidatedBy') != null && s.batchEditApi.HasChanges(e.visibleIndex,'ValidatedBy')){
                            //     s.batchEditApi.SetCellValue(e.visibleIndex, 'ValidatedBy', null);}                           
                            //    }
                            //   }  
                            //}";
                            if (View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review")
                            {
                                gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            }
                            if (View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_ResultValidation_Leve1lReview")).PageSize = ((ListView)View).CollectionSource.GetCount();

                            }
                            else if (View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_ResultApproval_Level2Review")).PageSize = ((ListView)View).CollectionSource.GetCount();
                            }
                        }
                    }
                    else if (View.Id == "SampleParameter_ListView_Copy_QCResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                        || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review")
                    {
                        if (gridListEditor != null && gridListEditor.Grid != null && ((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.GetCount() > 0)
                        {
                            gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                            gridListEditor.Grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                            gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            gridListEditor.Grid.JSProperties["cpValidateddateCurrentDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ValidateddateCurrentDate");
                            gridListEditor.Grid.JSProperties["cpValidateddateAnalyzedDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ValidateddateAnalyzedDate");
                            gridListEditor.Grid.JSProperties["cpApprovedDateCurrentDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ApprovedDateCurrentDate");
                            gridListEditor.Grid.JSProperties["cpApprovedDateValidatedDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ApprovedDateValidatedDate");
                            if (View.Id == "SampleParameter_ListView_Copy_QCResultValidation")
                            {
                                gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'ValidatedDate' ||  e.focusedColumn.fieldName == 'ValidatedBy.Oid')
                                        {
                                            e.cancel = false;
                                        }
                                        else
                                        {
                                            e.cancel = true;
                                        }    
                                }";
                                gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function (s, e) {
                                window.setTimeout(function () {
                                var dateValidation = s.batchEditApi.GetCellValue(e.visibleIndex, 'ValidatedDate');
                                var dateAnalyzed = s.batchEditApi.GetCellValue(e.visibleIndex, 'AnalyzedDate');
                                var dt = new Date();
                                if (dt != null && dateValidation != null && dateAnalyzed != null)
                                {
                                    if (dateValidation < dateAnalyzed)
                                    {
                                        alert(s.cpValidateddateAnalyzedDatemsg);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ValidatedDate', null);
                                    }
                                    else if (dt < dateValidation)
                                    {
                                        alert(s.cpValidateddateCurrentDatemsg);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ValidatedDate', null);
                                    }
                                }
                                }, 10);
                                }";
                                gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                                gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                    {                        
                        sessionStorage.setItem('ResultValidationFocusedColumn', null); 
                        var fieldName = e.cellInfo.column.fieldName;                       
                        sessionStorage.setItem('ResultValidationFocusedColumn', fieldName);            
                    }";
                                gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            {                      
                            var FocusedColumn = sessionStorage.getItem('ResultValidationFocusedColumn');                                
                            var oid;
                            var text;
                            if(FocusedColumn.includes('.'))
                            {                          
                                if(FocusedColumn ='ValidatedBy.Oid')
                                {
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        {                                                    
                                            if (s.IsRowSelectedOnPage(i)) {
                                            s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                                            }                                                                             
                                        }
                                    }   
                                }    
                             }
                             else
                             {
                                if(FocusedColumn = 'ValidatedDate')
                                {
                                var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                if (e.item.name =='CopyToAllCell')
                                {
                                    for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                    {              
                                        if (s.IsRowSelectedOnPage(i)) {
                                        s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                        }
                                    }
                                }  
                                }
                             }
                         e.processOnServer = false;
                    }";
                            }
                            else if (View.Id == "SampleParameter_ListView_Copy_QCResultApproval")
                            {
                                gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'ApprovedDate' ||  e.focusedColumn.fieldName == 'ApprovedBy.Oid')
                                        {
                                            e.cancel = false;
                                        }
                                        else
                                        {
                                            e.cancel = true;
                                        }    
                                }";
                                gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function (s, e) {
                                window.setTimeout(function () {
                                var dateValidation = s.batchEditApi.GetCellValue(e.visibleIndex, 'ValidatedDate');
                                var dateApproved = s.batchEditApi.GetCellValue(e.visibleIndex, 'ApprovedDate');
                                var dt = new Date();
                                if (dt != null && dateValidation != null && dateApproved != null)
                                {
                                    if (dateApproved < dateValidation)
                                    {
                                        alert(s.cpApprovedDateValidatedDatemsg);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ApprovedDate', null);
                                    }
                                    else if (dt < dateApproved)
                                    {
                                        alert(s.cpApprovedDateCurrentDatemsg);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ApprovedDate', null);
                                    }
                                }
                                }, 10);
                                }";
                                gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                                gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                    {                        
                        sessionStorage.setItem('ResultValidationFocusedColumn', null); 
                        var fieldName = e.cellInfo.column.fieldName;                       
                        sessionStorage.setItem('ResultValidationFocusedColumn', fieldName);            
                    }";
                                gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            {                      
                            var FocusedColumn = sessionStorage.getItem('ResultValidationFocusedColumn');                                
                            var oid;
                            var text;
                            if(FocusedColumn.includes('.'))
                            {                          
                                if(FocusedColumn ='ApprovedBy.Oid')
                                {
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        {                                                    
                                            if (s.IsRowSelectedOnPage(i)) {
                                            s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                                            }                                                                             
                                        }
                                    }   
                                }    
                             }
                             else
                             {        
                                if(FocusedColumn = 'ApprovedDate')
                                {
                                var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                if (e.item.name =='CopyToAllCell')
                                {
                                    for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                    {              
                                        if (s.IsRowSelectedOnPage(i)) {
                                        s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                        }
                                    }
                                }  
                                }                             
                             }
                         e.processOnServer = false;
                    }";
                            }
                            if (View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review")
                            {
                                gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            }
                            if (View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_QCResultValidation_Level1Review")).PageSize = ((ListView)View).CollectionSource.GetCount();
                            }
                            else if (View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_QCResultApproval_Level2Review")).PageSize = ((ListView)View).CollectionSource.GetCount();
                            }
                        }
                    }
                }
                else if (View.Id == "ResultEntry_Validation" || View.Id == "ResultEntry_Approval")
                {
                    SDMS.Active.SetItemValue("enb", objQPInfo.boolSDMS);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {

            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    //CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse("Oid is null"));
                    if (objLanguage.strcurlanguage != "En")
                    {
                        e.Items.Add("复制到所有单元格", "CopyToAllCell");
                    }
                    else
                    {
                        e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    }
                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";//"navigation_home_16x16";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel" || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel"
                    || View.Id == "QCBatch_ListView_ResultApproval" || View.Id == "QCBatch_ListView_ResultValidation"
                    || View.Id == "SampleParameter_ListView_ResultValidation_ABID" || View.Id == "SampleParameter_ListView_ResultApproval_ABID"
                    || View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel_Level1Review" || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel_Level2Review"
                    || View.Id == "SampleParameter_ListView_ResultValidation_ABID_Level1Review" || View.Id == "SampleParameter_ListView_ResultApproval_ABID_Level2Review"
                    || View.Id == "QCBatch_ListView_ResultValidation_Level1Review" || View.Id == "QCBatch_ListView_ResultApproval_Level2Review")
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    if (gridView != null)
                    {
                        GridViewCommandColumn selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                        if (selectionBoxColumn != null)
                        {
                            selectionBoxColumn.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void LstCurrObj_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel"
                                    || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel"
                                    || View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel_Level1Review"
                                    || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel_Level2Review"
                                    || View.Id == "Samplecheckin_ListView_ResultApproval_View")
                {
                    if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject is SampleParameter)
                    {
                        SampleParameter obj = (SampleParameter)e.InnerArgs.CurrentObject;
                        if (obj != null)
                        {
                            objQPInfo.boolSDMS = false;
                            objQPInfo.SampleResultValidationQueryFilter = "[Samplelogin.JobID.JobID] IN ('" + string.Join(",", obj.Samplelogin.JobID.JobID) + "') And [SignOff] = True";
                            //    objQPInfo.SampleResultValidationQueryFilter = "[QCBatchID] Is Not Null And [QCBatchID.SampleID] Is Not Null And [QCBatchID.SampleID.JobID] Is Not Null And" +
                            //" [QCBatchID.SampleID.JobID.JobID]  IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [QCBatchID] Is Null And [Samplelogin] Is Not Null And [Samplelogin.JobID] Is Not Null And " +
                            //"[Samplelogin.JobID.JobID] =  IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ")";

                            //objQPInfo.QCResultValidationQueryFilter = "[Samplelogin.JobID.JobID] IN ('" + string.Join(",", obj.Samplelogin.JobID.JobID) + "')";
                            //objQPInfo.QCResultValidationQueryFilter = "[QCBatchID.SampleID.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True";
                            objQPInfo.QCResultValidationQueryFilter = "([QCBatchID.SampleID] Is Not Null And [QCBatchID.SampleID.JobID.JobID] IN ('" + string.Join("','", objQPInfo.lstJobID) + "') And [SignOff] = True) Or ([QCBatchID.SampleID] Is Null And Contains([UQABID.Jobid] , '" + obj.Samplelogin.JobID.JobID + "') And [SignOff] = True)";



                            //if (string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter) && objQPInfo.rgFilterByMonthDate.Year != 0001)
                            //{
                            //    objQPInfo.SampleResultValidationQueryFilter = "[Samplelogin.JobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')";
                            //}
                            //else if (!string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter) && objQPInfo.rgFilterByMonthDate.Year != 0001)
                            //{
                            //    objQPInfo.SampleResultValidationQueryFilter = objQPInfo.SampleResultValidationQueryFilter + "AND [Samplelogin.JobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')";
                            //}

                            //if (string.IsNullOrEmpty(objQPInfo.QCResultValidationQueryFilter))
                            //{
                            //    objQPInfo.QCResultValidationQueryFilter = "[Datecreated] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')";
                            //}
                            //else
                            //{
                            //    objQPInfo.QCResultValidationQueryFilter = objQPInfo.QCResultValidationQueryFilter + "AND [Datecreated] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')";
                            //}
                            if (View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Validation", false);
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Approval", false);
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel_Level1Review")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Validation_View", false);
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel_Level2Review")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Approval_View", false);
                            }
                            e.Handled = true;
                        }
                    }
                }
                else if (View.Id == "QCBatch_ListView_ResultApproval" || View.Id == "QCBatch_ListView_ResultValidation" || View.Id == "QCBatch_ListView_ResultValidation_Level1Review"
                         || View.Id == "QCBatch_ListView_ResultApproval_Level2Review")
                {
                    if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject is SpreadSheetEntry_AnalyticalBatch)
                    {
                        SpreadSheetEntry_AnalyticalBatch obj = (SpreadSheetEntry_AnalyticalBatch)e.InnerArgs.CurrentObject;
                        if (obj != null)
                        {
                            objQPInfo.boolSDMS = true;
                            objQPInfo.SampleResultValidationQueryFilter = "[QCBatchID.qcseqdetail.QCBatchID] IN (" + "'" + string.Join("','", objQPInfo.lstQCBatchID) + "'" + ") And [SignOff] = True";
                            objQPInfo.QCResultValidationQueryFilter = objQPInfo.SampleResultValidationQueryFilter;

                            //if (string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter) && objQPInfo.rgFilterByMonthDate.Year != 0001)
                            //{
                            //    objQPInfo.SampleResultValidationQueryFilter = "[Samplelogin.JobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')";
                            //}
                            //else if (!string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter) && objQPInfo.rgFilterByMonthDate.Year != 0001)
                            //{
                            //    objQPInfo.SampleResultValidationQueryFilter = objQPInfo.SampleResultValidationQueryFilter + "AND [Samplelogin.JobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')";
                            //}

                            //if (string.IsNullOrEmpty(objQPInfo.QCResultValidationQueryFilter) && objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                            //{
                            //    objQPInfo.QCResultValidationQueryFilter = "[QCBatchID.qcseqdetail.Datecreated] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')";
                            //}
                            //else if (!string.IsNullOrEmpty(objQPInfo.QCResultValidationQueryFilter) && objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                            //{
                            //    objQPInfo.QCResultValidationQueryFilter = objQPInfo.QCResultValidationQueryFilter + "AND [QCBatchID.qcseqdetail.Datecreated] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')";
                            //}
                            if (View.Id == "QCBatch_ListView_ResultValidation")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Validation", false);
                            }
                            else if (View.Id == "QCBatch_ListView_ResultApproval")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Approval", false);
                            }
                            else if (View.Id == "QCBatch_ListView_ResultValidation_Level1Review")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Validation_View", false);
                            }
                            else if (View.Id == "QCBatch_ListView_ResultApproval_Level2Review")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Approval_View", false);
                            }
                            e.Handled = true;
                        }
                    }
                }
                else if (View.Id == "SampleParameter_ListView_ResultValidation_ABID" || View.Id == "SampleParameter_ListView_ResultApproval_ABID" || View.Id == "SampleParameter_ListView_ResultValidation_ABID_Level1Review" || View.Id == "SampleParameter_ListView_ResultApproval_ABID_Level2Review")
                {
                    if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject is SampleParameter)
                    {
                        SampleParameter obj = (SampleParameter)e.InnerArgs.CurrentObject;
                        if (obj != null)
                        {
                            objQPInfo.boolSDMS = true;
                            objQPInfo.SampleResultValidationQueryFilter = "[UQABID.AnalyticalBatchID] IN (" + "'" + string.Join("','", objQPInfo.lstQCBatchID) + "'" + ") And [SignOff] = True";
                            objQPInfo.QCResultValidationQueryFilter = objQPInfo.SampleResultValidationQueryFilter;
                            if (View.Id == "SampleParameter_ListView_ResultValidation_ABID")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Validation", false);
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultApproval_ABID")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Approval", false);
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultValidation_ABID_Level1Review")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Validation_View", false);
                            }
                            else if (View.Id == "SampleParameter_ListView_ResultApproval_ABID_Level2Review")
                            {
                                e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Approval_View", false);
                            }
                            e.Handled = true;
                        }
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            try
            {
                base.OnDeactivated();
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                if (View.Id == "ResultEntry_Validation" || View.Id == "ResultEntry_Approval")
                {
                    if (SDMS.Active.Contains("enb"))
                    {
                        SDMS.Active.RemoveItem("enb");
                    }
                }

                if (View.Id == "Samplecheckin_ListView_ResultValidation"
                    || View.Id == "Samplecheckin_ListView_ResultApproval")
                {
                    dataReviewDateFilterAction.SelectedItemChanged -= DataReviewDateFilterAction_SelectedItemChanged;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void ProcessAction(string parameter)
        {

        }

        private void SDMS_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(objQPInfo.lstQCBatchID[0]))
                {
                    //SpreadSheetEntry_AnalyticalBatch qC = View.ObjectSpace.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[QCBatchID]=?", objQPInfo.lstQCBatchID[0]));
                    SpreadSheetEntry_AnalyticalBatch qC = View.ObjectSpace.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", objQPInfo.lstQCBatchID[0]));
                    if (qC != null)
                    {
                        //qcbatchinfo.strqcid = objQPInfo.lstQCBatchID[0];
                        //qcbatchinfo.canfilter = true;
                        //qcbatchinfo.strTest = qC.Test.TestName;
                        //qcbatchinfo.OidTestMethod = qC.Method.Oid;
                        ////if (qC.ABID != null)
                        //{
                        //    qcbatchinfo.strAB = qC.AnalyticalBatchID;
                        //    qcbatchinfo.qcstatus = qC.Status;
                        //}
                        //Frame.SetView(Application.CreateDashboardView((NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(SDMSDCSpreadsheet)), "SDMS", true));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void dataReviewNextAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count > 0)
                {
                    objQPInfo.boolSDMS = false;
                    objQPInfo.lstJobID = ((ListView)View).SelectedObjects.Cast<Samplecheckin>().Select(i => i.JobID).Distinct().ToList();
                    //objQPInfo.SampleResultValidationQueryFilter = "[Samplelogin.JobID.JobID] IN ('" + string.Join(",", objQPInfo.lstJobID) + "') And [SignOff] = True";
                    objQPInfo.SampleResultValidationQueryFilter = new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.JobID", objQPInfo.lstJobID), CriteriaOperator.Parse("[SignOff] = True And ([TestHold] = False Or [TestHold] Is null)")).ToString();

                    string strQCCriteria = string.Empty;
                    if (objQPInfo.lstJobID != null && objQPInfo.lstJobID.Count > 0)
                    {
                        foreach (string strJobID in objQPInfo.lstJobID)
                        {
                            if (string.IsNullOrEmpty(strQCCriteria))
                            {
                                strQCCriteria = "Contains([UQABID.Jobid] , '" + strJobID + "')";
                            }
                            else
                            {
                                strQCCriteria = strQCCriteria + "Or Contains([UQABID.Jobid] , '" + strJobID + "')";
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(strQCCriteria))
                    {
                        objQPInfo.QCResultValidationQueryFilter = "([QCBatchID.SampleID] Is Not Null And [QCBatchID.SampleID.JobID.JobID] IN ('" + string.Join("','", objQPInfo.lstJobID) + "') And [SignOff] = True) Or ([QCBatchID.SampleID] Is Null And [UQABID.Jobid] IN ('" + string.Join("','", objQPInfo.lstJobID) + "') And [SignOff] = True And ([TestHold] = False Or [TestHold] Is null))";
                    }
                    else
                    {
                        objQPInfo.QCResultValidationQueryFilter = "([QCBatchID.SampleID] Is Not Null And [QCBatchID.SampleID.JobID.JobID] IN ('" + string.Join("','", objQPInfo.lstJobID) + "') And [SignOff] = True) Or ([QCBatchID.SampleID] Is Null And (" + strQCCriteria + ") And [SignOff] = True And ([TestHold] = False Or [TestHold] Is null))";
                    }
                    if (View.Id == "Samplecheckin_ListView_ResultValidation")
                    {
                        Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Validation", false));
                    }
                    else
                    if (View.Id == "Samplecheckin_ListView_ResultApproval")
                    {
                        Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Approval", false));
                    }
                    else if (View.Id == "Samplecheckin_ListView_ResultValidation_View")
                    {
                        Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Validation_View", false));
                    }
                    else if (View.Id == "Samplecheckin_ListView_ResultApproval_View")
                    {
                        Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Approval_View", false));
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void dataReviewDateFilterAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {

        }
    }
}
