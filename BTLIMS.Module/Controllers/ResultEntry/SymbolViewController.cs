using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;

namespace Modules.Controllers.Biz
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SymbolController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        #region Constructor
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        ShowNavigationItemController ShowNavigationController;
        public SymbolController()
        {
            InitializeComponent();
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                SymbolAction.Active.SetItemValue("sybol", false);
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
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        #endregion

        #region PopupEvents
        private void SymbolAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                var objSpace = Application.CreateObjectSpace();
                if (objSpace != null)
                {
                    e.View = Application.CreateDashboardView(objSpace, "SpecificSymbolDashboard", true); //SpecificSymbolDashboard展现层定义的UserControl-SpecificSymbol.ascx
                }
                e.DialogController.AcceptAction.Active.SetItemValue("DialogOk", false);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SymbolAction_CustomizeControl(object sender, CustomizeControlEventArgs e)
        {
            try
            {
                //if (View is DashboardView && View.Id == "ResultEntryDV")
                //{
                //    SymbolAction.Caption = "Units";
                //}
                //else
                //{
                //    SymbolAction.Caption = "Symbol";
                //}
                if (objnavigationRefresh.Refresh == "1")
                {
                    objnavigationRefresh.Refresh = "0";
                    ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    {
                        if (parent.Id == "Reporting")
                        {
                            foreach (ChoiceActionItem child in parent.Items)
                            {
                                if (child.Id == "Custom Reporting")
                                {
                                    int count = 0;
                                    IObjectSpace objSpace = Application.CreateObjectSpace();
                                    using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                    {
                                        lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingReporting' AND ([NotReport] Is Null Or [NotReport] = False)");
                                        lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                        List<object> jobid = new List<object>();
                                        foreach (ViewRecord rec in lstview)
                                            jobid.Add(rec["Toid"]);
                                        count = jobid.Count;
                                    }
                                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    if (count > 0)
                                    {
                                        child.Caption = cap[0] + " (" + count + ")";
                                    }
                                    else
                                    {
                                        child.Caption = cap[0];
                                    }
                                }
                            }
                        }
                        else if (parent.Id == "Data Review")
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            Session currentSession = ((XPObjectSpace)(os)).Session;
                            SelectedData sproc = currentSession.ExecuteSproc("Spreadsheetentry_SelectResultBatchReview_sp", new OperandValue(Convert.ToDateTime("1/1/1753 12:00:00")), new OperandValue(DateTime.Now.Date));
                            var Reviewcount = 0;
                            var Verifycount = 0;
                            foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                            {
                                if (row.Values[13] != null)
                                {
                                    if (row.Values[13].ToString() == "Pending Review")
                                    {
                                        Reviewcount += 1;
                                    }
                                    else if (row.Values[13].ToString() == "Pending Verify")
                                    {
                                        Verifycount += 1;
                                    }
                                }
                            }
                            foreach (ChoiceActionItem child in parent.Items)
                            {
                                if (child.Id == "RawDataResultReview")
                                {
                                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    if (Reviewcount > 0)
                                    {
                                        child.Caption = cap[0] + " (" + Reviewcount + ")";
                                    }
                                    else
                                    {
                                        child.Caption = cap[0];
                                    }
                                }
                                else if (child.Id == "RawDataResultVerify")
                                {
                                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    if (Verifycount > 0)
                                    {
                                        child.Caption = cap[0] + " (" + Verifycount + ")";
                                    }
                                    else
                                    {
                                        child.Caption = cap[0];
                                    }
                                }
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
        #endregion
    }
}
