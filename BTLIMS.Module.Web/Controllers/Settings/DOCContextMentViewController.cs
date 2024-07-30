using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace LDM.Module.Web.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DOCContextMentViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();

        public DOCContextMentViewController()
        {
            InitializeComponent();
            TargetViewId = "DOC_DetailView_Copy_DV;";
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
            try
            {
                if (View.Id == "DOC_DetailView_Copy_DV")
                {
                    if (View is DetailView)
                    {
                        Modules.BusinessObjects.Setting.DOC objSample = (Modules.BusinessObjects.Setting.DOC)View.CurrentObject;
                        foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "QCBatches" && i.GetType() == typeof(AspxGridLookupCustomEditor)))
                        {
                            AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                Modules.BusinessObjects.Setting.DOC SC = (Modules.BusinessObjects.Setting.DOC)View.CurrentObject;
                                if (SC != null)
                                {
                                    if (gridLookup != null)
                                    {
                                        gridLookup.GridViewProperties.Settings.ShowFilterRow = true;
                                        gridLookup.JSProperties["cpQBID"] = SC.QCBatches;
                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpQBID);
                                            }";
                                        gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                        gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                        gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                        gridLookup.ValueChanged += GridLookup_ValueChanged;

                                        gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                        gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "QBID" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "QCType" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Date" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "SysSampleCode" });
                                        gridLookup.GridView.Columns["QBID"].Width = 120;
                                        gridLookup.GridView.Columns["QCType"].Width = 100;
                                        gridLookup.GridView.Columns["Date"].Width = 150;
                                        gridLookup.GridView.Columns["SysSampleCode"].Width = 150;
                                        gridLookup.GridView.Columns["QCType"].Caption = "QCType";
                                        gridLookup.GridView.Columns["SysSampleCode"].Caption = "SysSampleCode";
                                        gridLookup.GridView.KeyFieldName = "QBID;SysSampleCode";
                                        gridLookup.TextFormatString = "{0}";
                                        DataTable table = new DataTable();
                                        table.Columns.Add("QBID");
                                        table.Columns.Add("QCType");
                                        table.Columns.Add("Date");
                                        table.Columns.Add("SysSampleCode");
                                        //gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                        //gridLookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                        //gridLookup.GridView.Settings.VerticalScrollableHeight = 200;
                                        if (SC.Method != null)
                                        {
                                            Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                            UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                            XPClassInfo testMatrixInfo;
                                            testMatrixInfo = uow.GetClassInfo(typeof(SampleParameter));
                                            List<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>
                                             (CriteriaOperator.Parse("[UQABID] Is Not Null " +
                                             "AND [Testparameter.TestMethod] Is Not Null" +
                                             " And [Testparameter.TestMethod.MatrixName] Is Not Null " +
                                             "And [Testparameter.TestMethod.MethodName] Is Not Null " +
                                             "AND [Testparameter.TestMethod.MatrixName.Oid] = ? " +
                                             "AND [Testparameter.TestMethod.TestName] = ? " +
                                             "AND [Testparameter.TestMethod.MethodName.Oid] = ? " +
                                             "And [QCBatchID]  Is Not Null" +
                                             " And [QCBatchID.QCType]  Is Not Null" +
                                             " And [QCBatchID.QCType.QCTypeName] = 'LCS'" +
                                             //" And [DOCDetail] Is Null" +
                                             "And ([Status] = 'PendingReportValidation' " +
                                             "Or [Status] = 'PendingReporting'" +
                                             "Or [Status] = 'PendingReportApproval'" +
                                             "Or [Status] = 'Reported'" +
                                             "Or [Status]='ReportApproved')" +
                                             "AND [DOCDetail] Is Null"
                                             , SC.Matrix.Oid, SC.Test.TestName, SC.Method.Oid)).Distinct().ToList();
                                            if (SC.TrainingStartDate != null || SC.TrainingEndDate != null)
                                            {
                                                if (SC.TrainingStartDate > SC.TrainingEndDate)
                                                {
                                                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "lessFromdate"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                                    SC.QCBatches = null;
                                                    propertyEditor.Refresh();
                                                    return;
                                                }
                                                else if(SC.TrainingStartDate == null || SC.TrainingEndDate == null)
                                                {
                                                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DOC", "validdate"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                                    SC.QCBatches = null;
                                                    propertyEditor.Refresh();
                                                    return;
                                                }
                                                else 
                                                {
                                                    if (SC.TrainingEndDate != DateTime.MinValue)
                                                    {
                                                        DateTime nonNullableDateTime = SC.TrainingEndDate.Value;
                                                        if (nonNullableDateTime != DateTime.MinValue)
                                                        {
                                                            TimeSpan timeSpan = nonNullableDateTime.Subtract(SC.TrainingEndDate.Value);
                                                            if (timeSpan==TimeSpan.Zero)
                                                            {
                                                                TimeSpan newTime = new TimeSpan(23, 59, 0);
                                                                SC.TrainingEndDate = SC.TrainingEndDate + newTime;  
                                                            }
                                                        }
                                                    }
                                                    //lstSamples1 = lstSamples1
                                                    lstSamples1 = lstSamples1.Where(i => i.UQABID != null && SC.TrainingStartDate <= i.UQABID.CreatedDate && i.UQABID.CreatedDate <= SC.TrainingEndDate).ToList();
                                                    //lstSamples1 = lstSamples1.Where(i=> SC.TrainingStartDate <= i.UQABID.CreatedDate && i.UQABID.CreatedDate>=SC.TrainingEndDate).ToList();
                                                }
                                            }
                                            if (lstSamples1 != null && lstSamples1.Count > 0)
                                            {
                                                int i = 0;
                                                foreach (SampleParameter objTest in lstSamples1.GroupBy(p => new { p.UQABID, p.QCBatchID }).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.UQABID).ToList())
                                                {
                                                    table.Rows.Add(new object[] { objTest.UQABID.AnalyticalBatchID, " " + objTest.QCBatchID.QCType.QCTypeName, " " + Convert.ToDateTime(objTest.CreatedDate).ToString("MM/dd/yy") , " " + objTest.SysSampleCode });

                                                    i++;
                                                }
                                                DataView dv = new DataView(table);
                                                dv.Sort = "QBID Desc";
                                                table = dv.ToTable();
                                            }

                                        }
                                        gridLookup.GridView.DataSource = table;
                                        //SRInfo.dtTest = table;
                                        gridLookup.GridView.DataBind();
                                    }
                                }
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
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
            // Access and customize the target View control.
        }

        private void GridLookup_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ((Modules.BusinessObjects.Setting.DOC)View.CurrentObject).QCBatches = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("QBID"));
                //((SpreadSheetEntry_AnalyticalBatch)View.CurrentObject).NPJobid = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("JobID"));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        //private void GridView_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ASPxGridView grid = sender as ASPxGridView;
        //        foreach (string strtest in SRInfo.lstTestgridviewrow.Distinct().ToList())
        //        {
        //            grid.Selection.SelectRow(Convert.ToInt32(strtest));
        //        }

        //        if (SRInfo.lstTestgridviewrow.Count > 0)
        //        {
        //            List<string> lsttest = new List<string>();
        //            foreach (string strtest in SRInfo.lstTestgridviewrow.Distinct().ToList())
        //            {
        //                lsttest.Add(grid.GetRowValues(Convert.ToInt32(strtest), "Test").ToString());
        //            }
        //            SRInfo.strtempNPTEST = string.Join(";", lsttest);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void GridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    try
        //    {
        //        var grid = sender as ASPxGridView;
        //        foreach (string strtest in SRInfo.lstTestgridviewrow.Distinct().ToList())
        //        {
        //            grid.Selection.SelectRow(Convert.ToInt32(strtest));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
