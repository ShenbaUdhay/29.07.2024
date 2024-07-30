using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace LDM.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DataCenterViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        private string constr;
        AnalyticalBatchInfo analyticalbatchinfo = new AnalyticalBatchInfo();


        public DataCenterViewController()
        {
            InitializeComponent();
            TargetViewId = "Collector_ListView_ClientCollectors;" + "TestMethod_PrepMethods_ListView_DataCenter;" + "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults;" + "TestMethod_TestGuides_ListView_DataCenter;" + "Project_ListView_Copy_DC;"
                + "Contact_ListView_Copy_DC;" + "Testparameter_ListView_Test_SampleParameter_DataCenter;" + "UserRightDC_ListView;"+ "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC;"+ "CompliantInitiation_ListView_DataCenter;"+ "Reporting_ListView_Datacenter;"+ "Invoicing_ListView_DataCenter;"
                + "Samplecheckin_ListView_DataCenter;"+ "Samplecheckin_Notes_ListView_DataCenter;"+ "SampleLogIn_ListView_DataCenter;"+ "SampleParameter_ListView_DataCenter;"+ "NonConformityInitiation_ListView_DataCenter;"+ "COCSettings_ListView_DataCenter;"
                + "SampleParameter_ListView_Copy_DC_ResultQC;"+ "SampleParameter_ListView_Copy_DC_ResultSample;"+ "CRMQuotes_ListView_DataCenter;"+ "LoginLog_ListView_DataCenter;"+ "QCType_ListView_DC;";
            Datacenterdatefilter.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC;" + "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults;"+ "Collector_ListView_ClientCollectors;"+ "Reporting_ListView_Datacenter;"+ "Invoicing_ListView_DataCenter;"
                + "Samplecheckin_ListView_DataCenter;"+ "Samplecheckin_Notes_ListView_DataCenter;"+ "SampleLogIn_ListView_DataCenter;"+ "SampleParameter_ListView_DataCenter;"+ "NonConformityInitiation_ListView_DataCenter;"+ "COCSettings_ListView_DataCenter;"
                + "SampleParameter_ListView_Copy_DC_ResultQC;"+ "SampleParameter_ListView_Copy_DC_ResultSample;"+ "CRMQuotes_ListView_DataCenter;"+ "LoginLog_ListView_DataCenter;";

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                // Perform various tasks depending on the target View.
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults"||View.Id== "Collector_ListView_ClientCollectors"
                    ||View.Id== "Reporting_ListView_Datacenter" || View.Id== "Invoicing_ListView_DataCenter"|| View.Id== "Samplecheckin_ListView_DataCenter"|| View.Id== "Samplecheckin_Notes_ListView_DataCenter"
                    || View.Id== "SampleLogIn_ListView_DataCenter"|| View.Id== "SampleParameter_ListView_DataCenter"||View.Id== "NonConformityInitiation_ListView_DataCenter"||View.Id== "COCSettings_ListView_DataCenter"
                    ||View.Id== "SampleParameter_ListView_Copy_DC_ResultQC"||View.Id== "SampleParameter_ListView_Copy_DC_ResultSample"||View.Id== "CRMQuotes_ListView_DataCenter"||View.Id== "LoginLog_ListView_DataCenter")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (Datacenterdatefilter.SelectedItem == null)
                    {
                       
                        if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                        {
                            if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC"||View.Id== "Collector_ListView_ClientCollectors"||View.Id== "NonConformityInitiation_ListView_DataCenter"
                                || View.Id == "COCSettings_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC"|| View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 1"); 
                            }
                            else if (View.Id == "Reporting_ListView_Datacenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReportedDate, Now()) <= 1"); 
                            } 
                            else if (View.Id == "Invoicing_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced, Now()) <= 1"); 
                            }
                            else if (View.Id == "Samplecheckin_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate, Now()) <= 1"); 
                            }
                            else if (View.Id == "Samplecheckin_Notes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ModifiedDate, Now()) <= 1"); 
                            }
                            else if (View.Id == "SampleLogIn_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(JobID.RecievedDate, Now()) <= 1"); 
                            }
                            else if (View.Id == "CRMQuotes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 1"); 
                            } 
                            else if (View.Id == "LoginLog_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(LoginDateTime, Now()) <= 1"); 
                            }
                            else if (View.Id == "SampleParameter_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultSample")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 1"); 
                            }
                            Datacenterdatefilter.SelectedItem = Datacenterdatefilter.Items[0];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                        {
                            if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC"||View.Id== "Collector_ListView_ClientCollectors" || View.Id == "NonConformityInitiation_ListView_DataCenter" 
                                || View.Id == "COCSettings_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC"|| View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 3"); 
                            } 
                            else if (View.Id == "Reporting_ListView_Datacenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReportedDate, Now()) <= 3"); 
                            }
                            else if (View.Id == "Invoicing_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced, Now()) <= 3"); 
                            } 
                            else if (View.Id == "Samplecheckin_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate, Now()) <= 3"); 
                            } 
                            else if (View.Id == "Samplecheckin_Notes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ModifiedDate, Now()) <= 3"); 
                            } 
                            else if (View.Id == "SampleLogIn_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(JobID.RecievedDate, Now()) <= 3"); 
                            
                            }
                            else if (View.Id == "CRMQuotes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 3"); 
                            } 
                            else if (View.Id == "LoginLog_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(LoginDateTime, Now()) <= 3"); 
                            }
                            else if (View.Id == "SampleParameter_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultSample")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 3"); 
                            }
                            Datacenterdatefilter.SelectedItem = Datacenterdatefilter.Items[1];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                        {
                            if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC"||View.Id== "Collector_ListView_ClientCollectors" || View.Id == "NonConformityInitiation_ListView_DataCenter" 
                                || View.Id == "COCSettings_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 6"); 
                            }
                            else if (View.Id == "Reporting_ListView_Datacenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReportedDate, Now()) <= 6"); 
                            }
                            else if (View.Id == "Invoicing_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced, Now()) <= 6"); 
                            }
                            else if (View.Id == "Samplecheckin_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate, Now()) <= 6"); 
                            }
                            else if (View.Id == "Samplecheckin_Notes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ModifiedDate, Now()) <= 6"); 
                            } 
                            else if (View.Id == "SampleLogIn_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(JobID.RecievedDate, Now()) <= 6"); 
                            } 
                            else if (View.Id == "CRMQuotes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 6"); 
                            } 
                            else if (View.Id == "LoginLog_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(LoginDateTime, Now()) <= 6"); 
                            }
                            else if (View.Id == "SampleParameter_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultSample")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 6"); 
                            }
                            Datacenterdatefilter.SelectedItem = Datacenterdatefilter.Items[2];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                        {
                            if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC" || View.Id == "Collector_ListView_ClientCollectors" || View.Id == "NonConformityInitiation_ListView_DataCenter"
                                || View.Id == "COCSettings_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 1"); 
                            }
                            else if (View.Id == "Reporting_ListView_Datacenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReportedDate, Now()) <= 1"); 
                            }
                            else if (View.Id == "Invoicing_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DateInvoiced, Now()) <= 1"); 

                            }
                            else if (View.Id == "Samplecheckin_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RecievedDate, Now()) <= 1"); 
                            }
                            else if (View.Id == "Samplecheckin_Notes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ModifiedDate, Now()) <= 1"); 
                            }
                            else if (View.Id == "SampleLogIn_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(JobID.RecievedDate, Now()) <= 1"); 
                            }
                            else if (View.Id == "CRMQuotes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 1"); 
                            } 
                            else if (View.Id == "LoginLog_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(LoginDateTime, Now()) <= 1"); 
                            }
                            else if (View.Id == "SampleParameter_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultSample")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 1"); 
                            }
                            Datacenterdatefilter.SelectedItem = Datacenterdatefilter.Items[3];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                        {
                            if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC" || View.Id == "Collector_ListView_ClientCollectors" || View.Id == "NonConformityInitiation_ListView_DataCenter" 
                                || View.Id == "COCSettings_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 2"); 
                            }
                            else if (View.Id == "Reporting_ListView_Datacenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReportedDate, Now()) <= 2"); 
                            }
                            else if (View.Id == "Invoicing_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DateInvoiced, Now()) <= 2"); 
                            }
                            else if (View.Id == "Samplecheckin_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RecievedDate, Now()) <= 2"); 
                            }
                            else if (View.Id == "Samplecheckin_Notes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ModifiedDate, Now()) <= 2"); 
                            }
                            else if (View.Id == "SampleLogIn_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(JobID.RecievedDate, Now()) <= 2"); 
                            }  
                            else if (View.Id == "CRMQuotes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 2"); 
                            } 
                            else if (View.Id == "LoginLog_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(LoginDateTime, Now()) <= 2"); 
                            } 
                            else if (View.Id == "SampleParameter_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultSample")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 2"); 
                            }
                            Datacenterdatefilter.SelectedItem = Datacenterdatefilter.Items[4];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                        {
                            if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC" || View.Id == "Collector_ListView_ClientCollectors" || View.Id == "NonConformityInitiation_ListView_DataCenter"
                                || View.Id == "COCSettings_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 5"); 
                            }
                            else if (View.Id == "Reporting_ListView_Datacenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReportedDate, Now()) <= 5"); 
                            } 
                            else if (View.Id == "Invoicing_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DateInvoiced, Now()) <= 5"); 
                            }
                            else if (View.Id == "Samplecheckin_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RecievedDate, Now()) <= 5"); 
                            } 
                            else if (View.Id == "Samplecheckin_Notes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ModifiedDate, Now()) <= 5"); 
                            } 
                            else if (View.Id == "SampleLogIn_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(JobID.RecievedDate, Now()) <= 5"); 
                            }
                            else if (View.Id == "CRMQuotes_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 5"); 
                            } 
                            else if (View.Id == "LoginLog_ListView_DataCenter")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(LoginDateTime, Now()) <= 5"); 
                            } 
                            else if (View.Id == "SampleParameter_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultSample")
                            {
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 5"); 
                            }
                            Datacenterdatefilter.SelectedItem = Datacenterdatefilter.Items[5];
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria.Remove("Distinct1");
                            Datacenterdatefilter.SelectedItem = Datacenterdatefilter.Items[6];
                        }
                    }
                    //Datacenterdatefilter.SelectedItem = Datacenterdatefilter.Items[0];
                    //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate,Now())<=3");
                }

                constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //SpreadSheetEntry_AnalyticalBatch objabid = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                if (View.Id == "SpreadSheetBuilder_SequencePattern_ListView_DataCenter")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[uqTestMethodID.GCRecord] Is Null");
                }
                if (View.Id == "UserRightDC_ListView")
                {
                    Session currentSession = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)(this.ObjectSpace)).Session;
                    SelectedData sproc = currentSession.ExecuteSproc("UserNavigationPremission_Sp", "");
                    if (sproc.ResultSet != null)
                    {
                        foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                        {
                            Modules.BusinessObjects.Hr.UserRightDC objURD = ObjectSpace.CreateObject<UserRightDC>();

                            objURD.FullName = row.Values[0].ToString();
                            Employee objE = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse(" [FullName] = ?", objURD.FullName));
                            if (objE != null)
                            {
                                objURD.UserName = objE.UserName;
                            }
                            objURD.Role = row.Values[1].ToString();
                            objURD.NavigationItem = row.Values[2].ToString();
                            if (Convert.ToInt32(row.Values[3]) == 1)
                            {
                                objURD.Create = true;
                            }
                            else
                            {
                                objURD.Create = false;
                            }
                            if (Convert.ToInt32(row.Values[4]) == 1)
                            {
                                objURD.Read = true;
                            }
                            else
                            {
                                objURD.Read = false;
                            }
                            if (Convert.ToInt32(row.Values[5]) == 1)
                            {
                                objURD.Write = true;
                            }
                            else
                            {
                                objURD.Write = false;
                            }
                            if (Convert.ToInt32(row.Values[6]) == 1)
                            {
                                objURD.Delete = true;
                            }
                            else
                            {
                                objURD.Delete = false;
                            }
                            ((ListView)View).CollectionSource.Add(objURD);

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
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                // Access and customize the target View control.

                if (View != null && View.Id == "Collector_ListView_ClientCollectors")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[CustomerName] is Not NULL And [CustomerName.GCRecord] is NULL");
                }
                else
                if (View != null && View.Id == "Project_ListView_Copy_DC")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[customername] is Not NULL And [customername.GCRecord] is NULL");
                }
                else
                if (View != null && View.Id == "Contact_ListView_Copy_DC")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Customer] is Not NULL And [Customer.GCRecord] is NULL");
                }
                else
                if (View != null && View.Id == "TestMethod_PrepMethods_ListView_DataCenter")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod] is Not NULL And [TestMethod.GCRecord] is NULL");
                }
                else
                if (View != null && View.Id == "TestMethod_TestGuides_ListView_DataCenter")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod] is Not NULL And [TestMethod.GCRecord] is NULL");
                }
                else
                if (View != null && View.Id == "Testparameter_ListView_Test_SampleParameter_DataCenter")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod] is Not NULL And [TestMethod.GCRecord] is NULL");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Datacenterdatefilter_SelectedItemChanged(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)
                {

                    if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC" || View.Id == "Collector_ListView_ClientCollectors" || View.Id == "NonConformityInitiation_ListView_DataCenter"
                        || View.Id == "COCSettings_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC")
                    {
                        if (e.SelectedChoiceActionItem.Id == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 3 And [CreatedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 6 And [CreatedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 2 And [CreatedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[CreatedDate] Is Not Null");
                        }
                    }
                    else if(View.Id== "Reporting_ListView_Datacenter")
                    {
                        if (e.SelectedChoiceActionItem.Id == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(ReportedDate, Now()) <= 1 And [ReportedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(ReportedDate, Now()) <= 3 And [ReportedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(ReportedDate, Now()) <= 6 And [ReportedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(ReportedDate, Now()) <= 1 And [ReportedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(ReportedDate, Now()) <= 2 And [ReportedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[ReportedDate] Is Not Null");
                        }
                    }
                    else if(View.Id== "Invoicing_ListView_DataCenter")
                    {
                        if (e.SelectedChoiceActionItem.Id == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced, Now()) <= 1 And [DateInvoiced] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced, Now()) <= 3 And [DateInvoiced] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateInvoiced, Now()) <= 6 And [DateInvoiced] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateInvoiced, Now()) <= 1 And [DateInvoiced] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateInvoiced, Now()) <= 2 And [DateInvoiced] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[DateInvoiced] Is Not Null");
                        }
                    }
                    else if(View.Id== "Samplecheckin_ListView_DataCenter")
                    {
                        if (e.SelectedChoiceActionItem.Id == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate, Now()) <= 1 And [RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate, Now()) <= 3 And [RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate, Now()) <= 6 And [RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(RecievedDate, Now()) <= 1 And [RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(RecievedDate, Now()) <= 2 And [RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[RecievedDate] Is Not Null");
                        }
                    }
                    else if(View.Id== "Samplecheckin_Notes_ListView_DataCenter")
                    {
                        if (e.SelectedChoiceActionItem.Id == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(ModifiedDate, Now()) <= 1 And [ModifiedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(ModifiedDate, Now()) <= 3 And [ModifiedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(ModifiedDate, Now()) <= 6 And [ModifiedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(ModifiedDate, Now()) <= 1 And [ModifiedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(ModifiedDate, Now()) <= 2 And [ModifiedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[ModifiedDate] Is Not Null");
                        }
                    }
                    else if(View.Id== "SampleLogIn_ListView_DataCenter")
                    {
                        if (e.SelectedChoiceActionItem.Id == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(JobID.RecievedDate, Now()) <= 1 And [JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(JobID.RecievedDate, Now()) <= 3 And [JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(JobID.RecievedDate, Now()) <= 6 And [JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(JobID.RecievedDate, Now()) <= 1 And [JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(JobID.RecievedDate, Now()) <= 2 And [JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[JobID.RecievedDate] Is Not Null");
                        }
                    }
                    else if(View.Id== "CRMQuotes_ListView_DataCenter")
                    {
                        if (e.SelectedChoiceActionItem.Id == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 1 And [QuotedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 3 And [QuotedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 6 And [QuotedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 1 And [QuotedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 2 And [QuotedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[QuotedDate] Is Not Null");
                        }
                    }
                    else if(View.Id== "CRMQuotes_ListView_DataCenter")
                    {
                        if (e.SelectedChoiceActionItem.Id == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(LoginDateTime, Now()) <= 1 And [LoginDateTime] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(LoginDateTime, Now()) <= 3 And [LoginDateTime] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(LoginDateTime, Now()) <= 6 And [LoginDateTime] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(LoginDateTime, Now()) <= 1 And [LoginDateTime] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(LoginDateTime, Now()) <= 2 And [LoginDateTime] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[LoginDateTime] Is Not Null");
                        }
                    }
                    else if(View.Id== "SampleParameter_ListView_DataCenter" || View.Id == "SampleParameter_ListView_Copy_DC_ResultSample")
                    {
                        if (e.SelectedChoiceActionItem.Id == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 1 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 3 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 6 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 1 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 2 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (e.SelectedChoiceActionItem.Id == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] Is Not Null");
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

        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults")
                {
                    e.Handled = true;
                    SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)((ListView)View).CurrentObject;
                    qcbatchinfo.canfilter = true;
                    qcbatchinfo.strTest = qC.Test.TestName;
                    if (qC.AnalyticalBatchID != null)
                    {
                        qcbatchinfo.QCBatchOid = qC.Oid;
                        qcbatchinfo.strqcid = qcbatchinfo.strqcbatchid = qC.AnalyticalBatchID;
                        if (qC.Test != null)
                        {
                            qcbatchinfo.OidTestMethod = qC.Test.Oid;
                        }
                    }
                    qcbatchinfo.strAB = qC.AnalyticalBatchID;
                    qcbatchinfo.qcstatus = qC.Status;
                    Frame.SetView(Application.CreateDashboardView((NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(SDMSDCSpreadsheet)), "SDMS", true));
                    qcbatchinfo.dtsample = new DataTable { TableName = "RawDataTableDataSource" };
                    Getdtsamplesource(qcbatchinfo.QCBatchOid);

                    foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                    {
                        column.ColumnName = column.ColumnName.ToUpper();
                    }
                    qcbatchinfo.strMode = "View";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Getdtsamplesource(Guid? oid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("LDMSPREADSHEETENTRY_SELECT_SP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] param = new SqlParameter[1];
                        //param[0] = new SqlParameter("@QCBATCHID", qcid);
                        param[0] = new SqlParameter("@QCBATCHID", oid);
                        cmd.Parameters.AddRange(param);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(qcbatchinfo.dtsample);
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
            base.OnDeactivated();
        }
    }
}
