﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.QC;
//using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;

namespace LDM.Module.Controllers.Public
{
    public partial class TaskTrackingViewController : ViewController<ListView>, IXafCallbackHandler
    {
        ResourceManager rm;
        string CurrentLanguage = string.Empty;
        MessageTimer timer = new MessageTimer();
        ProjectTrackingStatusFilter objPTInfo = new ProjectTrackingStatusFilter();
        public TaskTrackingViewController()
        {
            InitializeComponent();
            TargetViewId = "Samplecheckin_ListView_Incompletejobs;" + "Samplecheckin_ListView_ProjectTracking;" + "SDMSDCAB_ListView_Tracking;";
            ProjectTrackingDateFilterAction.TargetViewId = "Samplecheckin_ListView_Incompletejobs;" + "Samplecheckin_ListView_ProjectTracking;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {

                //if (View.Id == "Samplecheckin_ListView_Incompletejobs" || View.Id == "Samplecheckin_ListView_ProjectTracking")
                //{
                //    //FilterController filterController = Frame.GetController<FilterController>();
                //    //filterController.FullTextFilterAction.Executing += FullTextFilterAction_Executing;
                //    //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");

                //    if (View.Id == "Samplecheckin_ListView_Incompletejobs" || View.Id == "Samplecheckin_ListView_ProjectTracking")
                //    {
                //        //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", DateTime.Today.AddMonths(-1), DateTime.Now);

                //    }
                //}

                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                //SelectedData sproc = ((XPObjectSpace)(Application.MainWindow.View.ObjectSpace)).Session.ExecuteSproc("getCurrentLanguage", "");
                //CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                CurrentLanguage = "En";
                rm = new ResourceManager("Resources.Tracking", Assembly.Load("App_GlobalResources"));
                if (View.Id == "Samplecheckin_ListView_Incompletejobs" || View.Id == "Samplecheckin_ListView_ProjectTracking")
                {
                    DateTime projectDateFilter = DateTime.MinValue;

                    if (ProjectTrackingDateFilterAction != null && ProjectTrackingDateFilterAction.SelectedItem == null)
                    {
                        DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (ProjectTrackingDateFilterAction.SelectedItem == null)
                    {
                            setting.TaskWorkflow = EnumDateFilter.OneMonth;

                            if (setting.TaskWorkflow == EnumDateFilter.OneMonth)
                            {
                                ProjectTrackingDateFilterAction.SelectedItem = ProjectTrackingDateFilterAction.Items[0];
                                projectDateFilter = DateTime.Today.AddMonths(-1);
                            }
                            else if (setting.TaskWorkflow == EnumDateFilter.ThreeMonth)
                            {
                                ProjectTrackingDateFilterAction.SelectedItem = ProjectTrackingDateFilterAction.Items[1];
                                projectDateFilter = DateTime.Today.AddMonths(-3);
                            }
                            else if (setting.TaskWorkflow == EnumDateFilter.SixMonth)
                            {
                                ProjectTrackingDateFilterAction.SelectedItem = ProjectTrackingDateFilterAction.Items[2];
                                projectDateFilter = DateTime.Today.AddMonths(-6);
                            }
                            else if (setting.TaskWorkflow == EnumDateFilter.OneYear)
                            {
                                ProjectTrackingDateFilterAction.SelectedItem = ProjectTrackingDateFilterAction.Items[3];
                                projectDateFilter = DateTime.Today.AddYears(-1);
                            }
                            else if (setting.TaskWorkflow == EnumDateFilter.TwoYear)
                            {
                                ProjectTrackingDateFilterAction.SelectedItem = ProjectTrackingDateFilterAction.Items[4];
                                projectDateFilter = DateTime.Today.AddYears(-2);
                            }
                            else if (setting.TaskWorkflow == EnumDateFilter.FiveYear)
                            {
                                ProjectTrackingDateFilterAction.SelectedItem = ProjectTrackingDateFilterAction.Items[5];
                                projectDateFilter = DateTime.Today.AddYears(-5);
                            }
                            else if(setting.TaskWorkflow == EnumDateFilter.All)
                            {
                                ProjectTrackingDateFilterAction.SelectedItem = ProjectTrackingDateFilterAction.Items[6];
                                projectDateFilter = new DateTime(1753, 1, 1);
                            }
                        }
                        //if (projectDateFilter != DateTime.MinValue)
                        //{

                        //    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", projectDateFilter, DateTime.Now);
                        //}
                        //else
                        //{
                        //    ((ListView)View).CollectionSource.Criteria.Remove("Filter");
                        //}
                    }

                    //if (ProjectTrackingDateFilterAction.SelectedItem == null)
                    //{
                    //    ProjectTrackingDateFilterAction.SelectedItem = ProjectTrackingDateFilterAction.Items[0];
                    //}
                    //objPTInfo.ptFilterByMonthDate = DateTime.Today.AddMonths(-3);
                    Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    SelectedData sproc = null;
                    sproc = currentSession.ExecuteSproc("ProjectTrackingStatus", new OperandValue(projectDateFilter));
                    List<Guid> lstsmploid = new List<Guid>();
                    //foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                    //{
                    //    if (!lstsmploid.Contains(new Guid(row.Values[0].ToString())))
                    //    {
                    //        lstsmploid.Add(new Guid(row.Values[0].ToString()));
                    //    }
                    //}
                    lstsmploid = sproc.ResultSet[0].Rows.Select(i => new Guid(i.Values[0].ToString())).Distinct().ToList();
                    if (lstsmploid.Count > 0)
                    {
                        ((ListView)View).CollectionSource.Criteria["dateFilter"] = new InOperator("Oid", lstsmploid);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("Oid is null");
                    }
                    ProjectTrackingDateFilterAction.SelectedItemChanged += ProjectTrackingDateFilterAction_SelectedItemChanged;
                    //ProjectTrackingDateFilterAction.SelectedItem = ProjectTrackingDateFilterAction.Items[0];
                    //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", DateTime.Today.AddYears(-3),DateTime.Now);
                }
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void FullTextFilterAction_Executing(object sender, CancelEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "Samplecheckin_ListView_Incompletejobs" || View.Id == "Samplecheckin_ListView_ProjectTracking")
        //        {
        //            if (((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
        //            {
        //                ((ListView)View).CollectionSource.Criteria.Remove("Filter");
        //            }
        //            //((ListView)View).CollectionSource.Criteria["FranctionalFilter"] = CriteriaOperator.Parse("[Item.IsFractional]=? like '%'");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void ProjectTrackingDateFilterAction_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null)
                {

                    if (View.Id == "Samplecheckin_ListView_ProjectTracking" || View.Id == "Samplecheckin_ListView_Incompletejobs")
                    {
                        if (ProjectTrackingDateFilterAction.SelectedItem != null)
                        {
                            if (ProjectTrackingDateFilterAction.SelectedItem.Id == "1M")
                            {
                                //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate,Now())<=3");
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", DateTime.Today.AddMonths(-1), DateTime.Now);
                            }
                          
                            else if (ProjectTrackingDateFilterAction.SelectedItem.Id == "3M")
                            {
                                //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate,Now())<=3");
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", DateTime.Today.AddMonths(-3), DateTime.Now);
                            }
                            else if (ProjectTrackingDateFilterAction.SelectedItem.Id == "6M")
                            {
                                //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate,Now())<=6");
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", DateTime.Today.AddMonths(-6), DateTime.Now);
                            }
                            else if (ProjectTrackingDateFilterAction.SelectedItem.Id == "1Y")
                            {
                                //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffYear(RecievedDate,Now())<=1");
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", DateTime.Today.AddYears(-1), DateTime.Now);
                            }
                            else if (ProjectTrackingDateFilterAction.SelectedItem.Id == "2Y")
                            {
                                // ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffYear(RecievedDate,Now())<=2");
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", DateTime.Today.AddYears(-2), DateTime.Now);
                            }
                            else if (ProjectTrackingDateFilterAction.SelectedItem.Id == "5Y")
                            {
                                ///((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffYear(RecievedDate,Now())<=5");
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", DateTime.Today.AddYears(-5), DateTime.Now);
                            }
                            else if (ProjectTrackingDateFilterAction.SelectedItem.Id == "All")
                            {
                                ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
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

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
                e.PopupControl.CustomizePopupControl += PopupControl_CustomizePopupControl;
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupControl(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupControlEventArgs e)
        {
            try
            {
                e.PopupControl.AllowResize = false;
                e.PopupControl.ShowMaximizeButton = false;
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "SDMSDCAB_ListView_Tracking")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(450);
                    e.Height = new System.Web.UI.WebControls.Unit(450);
                }
                e.Handled = true;
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
            try
            {
                if (View is ListView)
                {
                    ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        if (View.Id == "Samplecheckin_ListView_Incompletejobs" || View.Id == "Samplecheckin_ListView_ProjectTracking")
                        {
                            ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                            selparameter.CallbackManager.RegisterHandler("Tracking", this);
                            gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                            foreach (ColumnWrapper wrapper in gridListEditor.Columns)
                            {
                                IModelColumn columnModel = View.Model.Columns[wrapper.PropertyName];
                                if (columnModel != null && columnModel.PropertyEditorType == typeof(WebProgressPropertyEditor))
                                {
                                    ((ASPxGridViewColumnWrapper)wrapper).Column.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Justify;
                                }
                            }
                        }
                        else if (View.Id == "SDMSDCAB_ListView_Tracking")
                        {
                            gridListEditor.Grid.Load += Grid_Load;
                            gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                            gridListEditor.Grid.HtmlRowPrepared += Grid_HtmlRowPrepared;
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

        private void Grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                if (e.RowType == GridViewRowType.Data)
                {
                    string status = grid.GetRowValues(e.VisibleIndex, "Status").ToString();
                    if (!string.IsNullOrEmpty(status))
                    {
                        if (status == "Done")
                        {
                            e.Row.BackColor = System.Drawing.Color.LightGreen;
                        }
                        else if (status == "Partial")
                        {
                            e.Row.BackColor = System.Drawing.Color.Orange;
                        }
                        else if (status == "Pending")
                        {
                            e.Row.BackColor = System.Drawing.Color.Red;
                            e.Row.ForeColor = System.Drawing.Color.White;
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

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                if (View.Id == "SDMSDCAB_ListView_Tracking" && HttpContext.Current.Session["seljobid"] != null)
                {
                    if (Application.MainWindow.View.Id == "Samplecheckin_ListView_Incompletejobs")
                    {
                        GridViewDataColumn FunctionTypedata_column = new GridViewDataTextColumn();
                        FunctionTypedata_column.FieldName = "FunctionType";
                        FunctionTypedata_column.Caption = rm.GetString("FunctionType_" + CurrentLanguage);
                        FunctionTypedata_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        FunctionTypedata_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        FunctionTypedata_column.ShowInCustomizationForm = false;
                        FunctionTypedata_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                        grid.Columns.Add(FunctionTypedata_column);
                        GridViewDataColumn Statusdata_column = new GridViewDataTextColumn();
                        Statusdata_column.FieldName = "Status";
                        Statusdata_column.Caption = "Status";
                        Statusdata_column.Visible = false;
                        Statusdata_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        Statusdata_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        Statusdata_column.ShowInCustomizationForm = false;
                        Statusdata_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                        grid.Columns.Add(Statusdata_column);
                        DataTable dt = new DataTable();
                        dt.Columns.Add("FunctionType");
                        dt.Columns.Add("Status");
                        Dictionary<double, string[]> statlist = new Dictionary<double, string[]>
                    {
                        { 0, new string[] { "", rm.GetString("Login_" + CurrentLanguage) } },
                        { 5, new string[] { "IndoorInspection", rm.GetString("IndoorInspection_" + CurrentLanguage) } },
                        { 10, new string[] { "ProductSampleMapping", rm.GetString("ProductSampleMapping_" + CurrentLanguage) } },
                        { 22, new string[] { "SamplePreparation", rm.GetString("SamplePreparation_" + CurrentLanguage) } },
                        { 24, new string[] { "PendingEntry", rm.GetString("PendingEntry_" + CurrentLanguage) } },
                        { 37, new string[] { "PendingReview", rm.GetString("PendingReview_" + CurrentLanguage) } },
                        { 49, new string[] { "PendingVerify", rm.GetString("PendingVerify_" + CurrentLanguage) } },
                        { 62, new string[] { "PendingValidation", rm.GetString("PendingValidation_" + CurrentLanguage) } },
                        { 74, new string[] { "PendingApproval", rm.GetString("PendingApproval_" + CurrentLanguage) } },
                        { 87, new string[] { "PendingReporting", rm.GetString("PendingReporting_" + CurrentLanguage) } }
                        //{ 100, new string[] { "Reported", rm.GetString("Reported_" + CurrentLanguage) } }
                    };
                        Dictionary<string, string> checklist = new Dictionary<string, string>
                    {
                        { "PendingReview", "Review" },
                        { "PendingVerify", "Verify" },
                        { "PendingValidation", "REValidate" },
                        { "PendingApproval", "REApprove" }
                    };
                        Dictionary<string, string> spllist = new Dictionary<string, string>
                    {
                        { "IndoorInspection", "Modules.BusinessObjects.SampleManagement.IndoorInspection,Modules" },
                        { "ProductSampleMapping", "Modules.BusinessObjects.SampleManagement.ProductSampleMapping,Modules" },
                    };
                        Dictionary<string, string> prep = new Dictionary<string, string>
                    {
                        { "SamplePreparation", "SamplePreparation_" },
                    };
                        Samplecheckin objsamplecheckin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["seljobid"].ToString())));
                        if (objsamplecheckin != null)
                        {
                            int prepcountPartial = 0;
                            int totsamplecount = Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", objsamplecheckin.Oid)));
                            foreach (KeyValuePair<double, string[]> curlist in statlist)
                            {
                                if (checklist.ContainsKey(curlist.Value[0]))
                                {
                                    if (checklist.TryGetValue(curlist.Value[0], out string checkvalue))
                                    {
                                        DefaultSetting objDefaultSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("" + checkvalue + " = 1"));
                                        if (objDefaultSetting == null)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                DataRow dataRow = dt.NewRow();
                                dataRow["FunctionType"] = curlist.Value[1];
                                if (spllist.ContainsKey(curlist.Value[0]))
                                {
                                    if (spllist.TryGetValue(curlist.Value[0], out string typevalue))
                                    {
                                        Type type = Type.GetType(typevalue);
                                        if (type != null)
                                        {
                                            int totcount = Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(type, CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid]=?", objsamplecheckin.Oid)));
                                            int pendcount = Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(type, CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid]=? and [Status] ='Completed'", objsamplecheckin.Oid)));
                                            if (totcount > 0)
                                            {
                                                if (pendcount > 0)
                                                {
                                                    if (totcount == pendcount)
                                                    {
                                                        dataRow["Status"] = "Done";
                                                    }
                                                    else
                                                    {
                                                        dataRow["Status"] = "Partial";
                                                    }
                                                }
                                                else
                                                {
                                                    dataRow["Status"] = "Pending";
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (prep.ContainsKey(curlist.Value[0]))
                                {
                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                    IList<SampleParameter> objSP = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [SamplePrepBatchID] Is Null ", objsamplecheckin.Oid));
                                    IList<SampleParameter> objSP1 = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [SamplePrepBatchID] Is Not Null ", objsamplecheckin.Oid));
                                    IList<SampleParameter> objSP2 = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [SamplePrepBatchID] Is Null And [SignOff] = True", objsamplecheckin.Oid));
                                    int prepcount = objSP.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0).Count();
                                    int prepcountdone = objSP1.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0).Count();
                                    prepcountPartial = objSP2.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0).Count();
                                    if (prepcount > 0)
                                    {
                                        dataRow["Status"] = "Pending";
                                    }
                                    else if (prepcountdone > 0)
                                    {
                                        dataRow["Status"] = "Done";
                                    }
                                    else if (prepcountPartial > 0)
                                    {
                                        dataRow["Status"] = "Partial";
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (curlist.Key < objsamplecheckin.ProgressStatus)
                                    {
                                        dataRow["Status"] = "Done";
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(curlist.Value[0]))
                                        {
                                            int curstatcount = Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] =?", objsamplecheckin.Oid, curlist.Value[0])));
                                            if (curstatcount == totsamplecount || curstatcount == 0)
                                            {
                                                dataRow["Status"] = "Pending";
                                            }
                                            else
                                            {
                                                dataRow["Status"] = "Partial";
                                            }
                                        }
                                        else
                                        {
                                            dataRow["Status"] = "Pending";
                                        }
                                    }
                                }
                                dt.Rows.Add(dataRow);
                            }
                        }
                        grid.DataSource = dt;
                        grid.DataBind();
                        HttpContext.Current.Session.Remove("seljobid");
                    }
                    else
                    {
                        GridViewDataColumn Testdata_column = new GridViewDataTextColumn();
                        Testdata_column.FieldName = "Test";
                        Testdata_column.Caption = rm.GetString("TestName_" + CurrentLanguage);
                        Testdata_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        Testdata_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        Testdata_column.ShowInCustomizationForm = false;
                        Testdata_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                        grid.Columns.Add(Testdata_column);
                        GridViewDataColumn Completedata_column = new GridViewDataTextColumn();
                        Completedata_column.FieldName = "Complete";
                        Completedata_column.Caption = rm.GetString("Complete_" + CurrentLanguage);
                        Completedata_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        Completedata_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        Completedata_column.ShowInCustomizationForm = false;
                        Completedata_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                        grid.Columns.Add(Completedata_column);
                        GridViewDataColumn Statusdata_column = new GridViewDataTextColumn();
                        Statusdata_column.FieldName = "Status";
                        Statusdata_column.Caption = "Status";
                        Statusdata_column.Visible = false;
                        Statusdata_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        Statusdata_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        Statusdata_column.ShowInCustomizationForm = false;
                        Statusdata_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                        grid.Columns.Add(Statusdata_column);
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Test");
                        dt.Columns.Add("Complete");
                        dt.Columns.Add("Status");
                        Samplecheckin objsamplecheckin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["seljobid"].ToString())));
                        if (objsamplecheckin != null)
                        {
                            IList<SampleParameter> parameters = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", objsamplecheckin.Oid));
                            List<string> LSTTestname = new List<string>();
                            foreach (SampleParameter cursp in parameters.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).OrderBy(i => i.Testparameter.TestMethod.TestName))
                            {
                                if (!LSTTestname.Contains(cursp.Testparameter.TestMethod.TestName))
                                {
                                    LSTTestname.Add(cursp.Testparameter.TestMethod.TestName);
                                }
                            }
                            foreach (string curtn in LSTTestname)
                            {
                                DataRow dataRow = dt.NewRow();
                                dataRow["Test"] = curtn;
                                int totcount = Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Testparameter.TestMethod.TestName] =?", objsamplecheckin.Oid, curtn)));
                                int pendcount = Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Testparameter.TestMethod.TestName] =? and [Status] = 'PendingEntry'", objsamplecheckin.Oid, curtn)));
                                dataRow["Complete"] = (totcount - pendcount).ToString() + "/" + totcount.ToString();
                                if (pendcount == 0)
                                {
                                    dataRow["Status"] = "Done";
                                }
                                else
                                {
                                    dataRow["Status"] = "Pending";
                                }
                                dt.Rows.Add(dataRow);
                            }
                        }
                        grid.DataSource = dt;
                        grid.DataBind();
                        HttpContext.Current.Session.Remove("seljobid");
                    }
                }
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                e.Cell.Attributes.Add("onclick", "event.stopPropagation();");
                if (View.Id == "Samplecheckin_ListView_Incompletejobs" || View.Id == "Samplecheckin_ListView_ProjectTracking")
                {
                    e.Cell.Attributes.Add("ondblclick", "RaiseXafCallback(globalCallbackControl, 'Tracking'," + e.VisibleIndex + " , '', false);");
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
            try
            {
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View.Id == "Samplecheckin_ListView_Incompletejobs" || View.Id == "Samplecheckin_ListView_ProjectTracking")
                {
                    ProjectTrackingDateFilterAction.SelectedItemChanged -= ProjectTrackingDateFilterAction_SelectedItemChanged;
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
            try
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        HttpContext.Current.Session["seljobid"] = gridListEditor.Grid.GetRowValues(int.Parse(parameter), "Oid");
                        string JobID = gridListEditor.Grid.GetRowValues(int.Parse(parameter), "JobID").ToString();
                        NonPersistentObjectSpace nos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(SDMSDCAB));
                        CollectionSource cs = new CollectionSource(nos, typeof(SDMSDCAB));
                        ListView lv = Application.CreateListView("SDMSDCAB_ListView_Tracking", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        showViewParameters.CreatedView.Caption = JobID;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.AcceptAction.Active.SetItemValue("enb", false);
                        dc.CancelAction.Active.SetItemValue("enb", false);
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
    }
}
