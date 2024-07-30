using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Labmaster.Module.Controllers.SamplingManagement
{
    public partial class FieldDataReviewController : ViewController
    {
        FielddataEntryReviewInfo FDERInfo = new FielddataEntryReviewInfo();
        MessageTimer timer = new MessageTimer();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        SamplingFieldConfigurationInfo SFCInfo = new SamplingFieldConfigurationInfo();
        curlanguage objLanguage = new curlanguage();

        public FieldDataReviewController()
        {
            InitializeComponent();
            TargetViewId = "Samplecheckin_ListView_FieldDataReview1;"
                + "Samplecheckin_DetailView_FieldDataReview1;"
                + "SampleLogIn_ListView_FieldDataReview1_Sampling;"
                + "SampleLogIn_ListView_FieldDataReview1_Station;"
                + "SampleParameter_ListView_FieldDataReview1;"

                + "Samplecheckin_ListView_FieldDataReview2;"
                + "Samplecheckin_DetailView_FieldDataReview2;"
                + "SampleLogIn_ListView_FieldDataReview2_Sampling;"
                + "SampleLogIn_ListView_FieldDataReview2_Station;"
                + "SampleParameter_ListView_FieldDataReview2;";
            FieldDataValidated.TargetViewId = "Samplecheckin_DetailView_FieldDataReview1;";
            FieldDataRollBack.TargetViewId = "Samplecheckin_DetailView_FieldDataReview1;";
            FieldDataApproved.TargetViewId = "Samplecheckin_DetailView_FieldDataReview2;";
            FieldDataApprovedRollback.TargetViewId = "Samplecheckin_DetailView_FieldDataReview2;";
            FieldDataReviewRecord.TargetViewId = "Samplecheckin_ListView_FieldDataReview2;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                FieldDataApproved.Active.SetItemValue("valApproved", false);
                FieldDataApprovedRollback.Active.SetItemValue("valApproveRollback", false);
                FieldDataRollBack.Active.SetItemValue("valValidateRollBack", false);
                FieldDataValidated.Active.SetItemValue("valValidate", false);
                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                {
                    FieldDataValidated.Active.SetItemValue("valValidate", true);
                    FieldDataRollBack.Active.SetItemValue("valValidateRollBack", true);
                    FieldDataApproved.Active.SetItemValue("valApproved", true);
                    FieldDataApprovedRollback.Active.SetItemValue("valApproveRollback", true);
                    FDERInfo.ReviewWrite = true;
                }
                else
                {
                    if (objnavigationRefresh.ClickedNavigationItem == "FieldDataReview1")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "FieldDataReview1" && i.Write == true) != null)
                            {
                                FieldDataRollBack.Active.SetItemValue("valValidateRollBack", true);
                                FieldDataValidated.Active.SetItemValue("valValidate", true);
                            }
                        }
                    }
                    else if (objnavigationRefresh.ClickedNavigationItem == "FieldDataReview2")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "FieldDataReview2" && i.Write == true) != null)
                            {
                                FDERInfo.ReviewWrite = true;
                                FieldDataApproved.Active.SetItemValue("valApproved", true);
                                FieldDataApprovedRollback.Active.SetItemValue("valApproveRollback", true);
                            }
                        }
                    }
                }

                if (View.Id == "Samplecheckin_ListView_FieldDataReview1")
                {
                    List<object> OidTask = new List<object>();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[Samplelogin.SamplingStatus] = 'PendingValidation'");
                        lstview.Properties.Add(new ViewProperty("group", SortDirection.Ascending, "Samplelogin.JobID.Oid", true, true));
                        foreach (ViewRecord Vrec in lstview)
                            OidTask.Add(Vrec["group"]);
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", OidTask);
                }
                else if (View.Id == "Samplecheckin_ListView_FieldDataReview2")
                {
                    List<object> OidTask = new List<object>();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[Samplelogin.SamplingStatus] = 'PendingApproval'");
                        lstview.Properties.Add(new ViewProperty("group", SortDirection.Ascending, "Samplelogin.JobID.Oid", true, true));
                        foreach (ViewRecord Vrec in lstview)
                            OidTask.Add(Vrec["group"]);
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", OidTask);

                    if (FieldDataReviewRecord.Items.Count == 0)
                    {
                        ChoiceActionItem item1 = new ChoiceActionItem();
                        ChoiceActionItem item2 = new ChoiceActionItem();
                        if (objLanguage.strcurlanguage == "En")
                        {
                            FieldDataReviewRecord.Items.Add(new ChoiceActionItem("Pending FieldData Review2 Record", item1));
                            FieldDataReviewRecord.Items.Add(new ChoiceActionItem("FieldData Reviewed Record", item2));
                        }
                        else
                        {
                            FieldDataReviewRecord.Items.Add(new ChoiceActionItem("待现场批准记录", item1));
                            FieldDataReviewRecord.Items.Add(new ChoiceActionItem("现场审核记录", item2));
                        }
                    }
                    FDERInfo.strMode = "Enter";
                    FieldDataReviewRecord.SelectedIndex = 0;
                }
                if (View.Id == "Samplecheckin_ListView_FieldDataReview1" || View.Id == "Samplecheckin_ListView_FieldDataReview2")
                {
                    ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listProcessController != null)
                    {
                        listProcessController.CustomProcessSelectedItem += ProcessListViewRowController_CustomProcessSelectedItem;
                    }
                }
                else if (View.Id == "Samplecheckin_DetailView_FieldDataReview1" || View.Id == "Samplecheckin_DetailView_FieldDataReview2")
                {
                    DashboardViewItem viStation = ((DetailView)View).FindItem("StationInformation") as DashboardViewItem;
                    if (viStation != null)
                    {
                        viStation.ControlCreated += ViStation_ControlCreated;
                    }
                }
                //if (View.Id == "Samplecheckin_DetailView_FieldDataReview2")
                //{
                //    if (FDERInfo.ReviewWrite)
                //    {
                //        if (FDERInfo.strMode == "Enter")
                //        {
                //            FieldDataApproved.Active.SetItemValue("valApproved", true);
                //            FieldDataApprovedRollback.Active.SetItemValue("valApproveRollback", false);
                //        }
                //        else if (FDERInfo.strMode == "View")
                //        {
                //            FieldDataApproved.Active.SetItemValue("valApproved", false);
                //            FieldDataApprovedRollback.Active.SetItemValue("valApproveRollback", true);
                //        }
                //    }
                //}

                if (View.Id == "SampleLogIn_ListView_FieldDataReview1_Sampling" || View.Id == "SampleLogIn_ListView_FieldDataReview2_Sampling")
                {
                    if (FDERInfo.TaskRegistrationOid != null)
                    {
                        IList<SampleParameter> lstss = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? AND [Samplelogin.SamplingStatus] <> 'PendingCompletion' And [Samplelogin.SamplingStatus] <> 'Completed'", FDERInfo.TaskRegistrationOid));
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In (" + string.Format("'{0}'", string.Join("','", lstss.Select(i => i.Samplelogin.Oid.ToString().Replace("'", "''")))) + ")");
                    }
                }
                else if (View.Id == "SampleParameter_ListView_FieldDataReview1" || View.Id == "SampleParameter_ListView_FieldDataReview2")
                {
                    if (FDERInfo.TaskRegistrationOid != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? AND [Samplelogin.SamplingStatus] <> 'PendingCompletion' And [Samplelogin.SamplingStatus] <> 'Completed' And [Testparameter.TestMethod.IsFieldTest] = True And [Samplelogin.Collector] Is Not Null And [Samplelogin.CollectDate] Is Not Null", FDERInfo.TaskRegistrationOid);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ViStation_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_DetailView_FieldDataReview1" || View.Id == "Samplecheckin_DetailView_FieldDataReview2")
                {
                    DashboardViewItem viStation = ((DetailView)View).FindItem("StationInformation") as DashboardViewItem;
                    if (viStation != null && viStation.InnerView != null)
                    {
                        IList<SampleParameter> lstss = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? AND [Samplelogin.SamplingStatus] <> 'PendingCompletion' And [Samplelogin.SamplingStatus] <> 'Completed'", FDERInfo.TaskRegistrationOid));
                        List<SampleLogIn> distinctStation = lstss.Select(a => a.Samplelogin).GroupBy(p => new { p.Matrix, p.StationLocation }).Select(g => g.First()).ToList();
                        List<Guid> objOid = distinctStation.Select(i => i.Oid).ToList();
                        if (objOid.Count > 0)
                        {
                            ((ListView)viStation.InnerView).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", objOid);
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

        private void ProcessListViewRowController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject.GetType() == typeof(Samplecheckin))
                {
                    if (((Samplecheckin)e.InnerArgs.CurrentObject).Oid != null)
                    {
                        FDERInfo.TaskRegistrationOid = ((Samplecheckin)e.InnerArgs.CurrentObject).Oid;
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
                if (View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load += Grid_Load;
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
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (View.Id == "SampleLogIn_ListView_FieldDataReview1_Sampling" || View.Id == "SampleLogIn_ListView_FieldDataReview2_Sampling")
                {
                    if (SFCInfo.lstSamplingColumn == null)
                    {
                        Samplecheckin objTasks = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("Oid=?", FDERInfo.TaskRegistrationOid));
                        if (gridListEditor != null && objTasks != null)
                        {
                            SFCInfo.lstSamplingColumn = new List<SamplingFieldConfiguration>();
                            if (!string.IsNullOrEmpty(objTasks.SampleMatries))
                            {
                                List<string> lstSMOid = objTasks.SampleMatries.Split(';').ToList();
                                foreach (string strOid in lstSMOid)
                                {
                                    VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));
                                    if (objVM != null && objVM.SamplingFieldConfiguration.Count > 0)
                                    {
                                        IList<SamplingFieldConfiguration> objFiledSample = objVM.SamplingFieldConfiguration.Where(i => i.FieldClass == FieldClass.Sampling).ToList();
                                        if (objFiledSample != null && objFiledSample.Count > 0)
                                        {
                                            foreach (SamplingFieldConfiguration objField in objFiledSample)
                                            {
                                                if (SFCInfo.lstSamplingColumn.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                                {
                                                    SFCInfo.lstSamplingColumn.Add(objField);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    customisefields(gridListEditor, SFCInfo.lstSamplingColumn);
                }
                else if (View.Id == "SampleLogIn_ListView_FieldDataReview1_Station" || View.Id == "SampleLogIn_ListView_FieldDataReview2_Station")
                {
                    if (SFCInfo.lstStationColumn == null)
                    {
                        Samplecheckin objTasks = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("Oid=?", FDERInfo.TaskRegistrationOid));
                        if (gridListEditor != null && objTasks != null)
                        {
                            SFCInfo.lstStationColumn = new List<SamplingFieldConfiguration>();
                            if (!string.IsNullOrEmpty(objTasks.SampleMatries))
                            {
                                List<string> lstSMOid = objTasks.SampleMatries.Split(';').ToList();
                                foreach (string strOid in lstSMOid)
                                {
                                    VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));

                                    if (objVM != null && objVM.SamplingFieldConfiguration.Count > 0)
                                    {
                                        IList<SamplingFieldConfiguration> objFiledStation = objVM.SamplingFieldConfiguration.Where(i => i.FieldClass == FieldClass.Station).ToList();
                                        if (objFiledStation != null && objFiledStation.Count > 0)
                                        {
                                            foreach (SamplingFieldConfiguration objField in objFiledStation)
                                            {
                                                if (SFCInfo.lstStationColumn.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                                {
                                                    SFCInfo.lstStationColumn.Add(objField);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    customisefields(gridListEditor, SFCInfo.lstStationColumn);
                }
                else if (View.Id == "SampleParameter_ListView_FieldDataReview1" || View.Id == "SampleParameter_ListView_FieldDataReview2")
                {
                    if (SFCInfo.lstTestColumn == null)
                    {
                        Samplecheckin objTasks = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("Oid=?", FDERInfo.TaskRegistrationOid));
                        if (gridListEditor != null && objTasks != null)
                        {
                            SFCInfo.lstTestColumn = new List<SamplingFieldConfiguration>();
                            if (!string.IsNullOrEmpty(objTasks.SampleMatries))
                            {
                                List<string> lstSMOid = objTasks.SampleMatries.Split(';').ToList();
                                foreach (string strOid in lstSMOid)
                                {
                                    VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));
                                    if (objVM != null && objVM.SamplingFieldConfiguration.Count > 0)
                                    {
                                        IList<SamplingFieldConfiguration> objFiledTest = objVM.SamplingFieldConfiguration.Where(i => i.FieldClass == FieldClass.Test).ToList();
                                        if (objFiledTest != null && objFiledTest.Count > 0)
                                        {
                                            foreach (SamplingFieldConfiguration objField in objFiledTest)
                                            {
                                                if (SFCInfo.lstTestColumn.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                                {
                                                    SFCInfo.lstTestColumn.Add(objField);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    customisefields(gridListEditor, SFCInfo.lstTestColumn);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        void customisefields(ASPxGridListEditor gridListEditor, List<SamplingFieldConfiguration> lstFields)
        {
            gridListEditor.Grid.ClearSort();
            foreach (GridViewColumn column in gridListEditor.Grid.Columns.Cast<GridViewColumn>().ToList())
            {
                if (column.Name == "SelectionCommandColumn")
                {
                    gridListEditor.Grid.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                }
                else
                {
                    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                    if (columnInfo != null)
                    {
                        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                        if (lstFields != null)
                        {
                            if ((View.Id == "SampleLogIn_ListView_FieldDataReview1_Station" || View.Id == "SampleLogIn_ListView_FieldDataReview2_Station") && columnInfo.Model.Id == "SampleID")
                            {
                                gridListEditor.Grid.SortBy(column, 0);
                                column.Visible = false;
                            }
                            else
                            {
                                SamplingFieldConfiguration curField = lstFields.FirstOrDefault(i => i.FieldID.Trim().ToLower() == columnInfo.Model.Id.Trim().ToLower());
                                if (curField != null)
                                {
                                    column.Visible = true;
                                    if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                                    {
                                        column.Caption = curField.FieldCustomCaption;
                                    }
                                    else
                                    {
                                        column.Caption = curField.FieldCaption;
                                    }
                                    if (curField.SortOrder > 0)
                                    {
                                        column.SetColVisibleIndex(curField.SortOrder + 1);
                                        if (curField.Freeze)
                                        {
                                            gridListEditor.Grid.SortBy(column, curField.SortOrder);
                                        }
                                    }
                                    if (curField.Freeze && gridListEditor.Grid.Columns[columnInfo.Model.Id] != null)
                                    {
                                        gridListEditor.Grid.Columns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                                    }
                                    if (curField.Width > 0)
                                    {
                                        column.Width = curField.Width;
                                    }
                                    modelColumn.AllowEdit = !curField.IsReadonly;
                                }
                                else
                                {
                                    modelColumn.Remove();
                                    gridListEditor.Grid.Columns.Remove(column);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (View.Id == "Samplecheckin_ListView_FieldDataReview1" || View.Id == "Samplecheckin_ListView_FieldDataReview2")
            {
                ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
                if (listProcessController != null)
                {
                    listProcessController.CustomProcessSelectedItem -= ProcessListViewRowController_CustomProcessSelectedItem;
                }
            }
            else if (View.Id == "Samplecheckin_DetailView_FieldDataReview1" || View.Id == "Samplecheckin_DetailView_FieldDataReview2")
            {
                FDERInfo.TaskRegistrationOid = null;
                DashboardViewItem viStation = ((DetailView)View).FindItem("StationInformation") as DashboardViewItem;
                if (viStation != null)
                {
                    viStation.ControlCreated -= ViStation_ControlCreated;
                }
            }

            if (View.Id == "Samplecheckin_ListView_FieldDataReview2")
            {
                if (FieldDataReviewRecord.Items.Count > 0)
                {
                    FieldDataReviewRecord.Items.Clear();
                }
            }
        }

        private void checkstatus(Samplecheckin objSamplecheckin, IObjectSpace os)
        {
            bool fieldtest = false;
            bool normaltest = false;
            objSamplecheckin = os.GetObjectByKey<Samplecheckin>(objSamplecheckin.Oid);
            IList<SampleLogIn> objSamplelogin = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? And [Testparameter.TestMethod.IsFieldTest] = True", objSamplecheckin.Oid), true).Cast<SampleParameter>().Select(a => a.Samplelogin).Distinct().ToList();
            if (objSamplelogin != null)
            {
                if (objSamplelogin.Count == objSamplelogin.Where(i => i.SamplingStatus == SamplingStatus.Completed).Count())
                {
                    fieldtest = true;
                }
            }
            IList<SampleBottleAllocation> samples = os.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.JobID.Oid]=?", objSamplecheckin.Oid));
            if (samples != null)
            {
                if (samples.Count == samples.Where(i => i.SampleTransferStatus == SamplingTransferStatus.Submitted).Count())
                {
                    normaltest = true;
                }
            }
            if (fieldtest && normaltest)
            {
                StatusDefinition objStatus = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 10"));
                if (objStatus != null)
                {
                    objSamplecheckin.Index = objStatus;
                }
            }
        }

        private void rollbackstatus(Samplecheckin objSamplecheckin, IObjectSpace os)
        {
            objSamplecheckin = os.GetObjectByKey<Samplecheckin>(objSamplecheckin.Oid);
            StatusDefinition objStatus = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 29"));
            if (objStatus != null)
            {
                objSamplecheckin.Index = objStatus;
            }
        }

        private void FieldDataValidated_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Samplecheckin objSamplecheckin = View.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid] = ?", FDERInfo.TaskRegistrationOid));
                if (objSamplecheckin != null)
                {
                    DashboardViewItem tasksSamplingInfo = ((DetailView)View).FindItem("SamplingInformation") as DashboardViewItem;
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView == null)
                    {
                        tasksSamplingInfo.CreateControl();
                        tasksSamplingInfo.InnerView.CreateControls();
                    }
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView != null && ((ListView)tasksSamplingInfo.InnerView).SelectedObjects.Count > 0)
                    {
                        SamplingStatus status = SamplingStatus.PendingApproval;
                        IList<DefaultSetting> FDdefsetting = View.ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Sampling Management' And Not IsNullOrEmpty([NavigationItemName])"));
                        if (FDdefsetting != null && FDdefsetting.Count > 0)
                        {
                            DefaultSetting FD2defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "FieldDataReview2").FirstOrDefault();
                            if (FD2defsetting != null)
                            {
                                if (FD2defsetting.Select == false)
                                {
                                    status = SamplingStatus.Completed;
                                }
                            }
                        }

                        Employee curemployee = ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        if (curemployee != null)
                        {
                            foreach (SampleLogIn objSampling in ((ListView)tasksSamplingInfo.InnerView).SelectedObjects)
                            {
                                objSampling.SamplingStatus = status;
                                objSampling.SamplingValidatedBy = curemployee;
                                objSampling.SamplingValidatedDate = DateTime.Now;
                                if (status == SamplingStatus.Completed)
                                {
                                    objSampling.SamplingApprovedBy = curemployee;
                                    objSampling.SamplingApprovedDate = DateTime.Now;
                                    if (objSampling.SampleStatus == null || !objSampling.SampleStatus.Samplinghold)
                                    {
                                        foreach (SampleParameter sp in tasksSamplingInfo.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=? And [Testparameter.TestMethod.IsFieldTest] = True", objSampling.Oid)))
                                        {
                                            sp.IsTransferred = true;
                                            sp.ValidatedBy = curemployee;
                                            sp.ApprovedBy = curemployee;
                                            sp.ValidatedDate = DateTime.Now;
                                            sp.ApprovedDate = DateTime.Now;
                                            sp.Status = Samplestatus.PendingReporting;
                                        }
                                    }
                                    checkstatus(objSamplecheckin, tasksSamplingInfo.InnerView.ObjectSpace);
                                }
                            }
                        }
                        ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultvalidate"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        e.ShowViewParameters.CreatedView = Application.CreateListView("Samplecheckin_ListView_FieldDataReview1", new CollectionSource(Application.CreateObjectSpace(), typeof(Samplecheckin)), true);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select the sampleID to validate", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FieldDataRollBack_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(SampleLogIn));
                SampleLogIn obj = os.CreateObject<SampleLogIn>();
                DetailView createdView = Application.CreateDetailView(os, "SampleLogIn_DetailView_FieldDataEntry_Rollback", true, obj);
                createdView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += RollBack_Accepting;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RollBack_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                SampleLogIn Sampling = (SampleLogIn)e.AcceptActionArgs.CurrentObject;
                if (Sampling != null && !string.IsNullOrEmpty(Sampling.SamplingRollbackReason))
                {
                    DashboardViewItem tasksSamplingInfo = ((DetailView)View).FindItem("SamplingInformation") as DashboardViewItem;
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView == null)
                    {
                        tasksSamplingInfo.CreateControl();
                        tasksSamplingInfo.InnerView.CreateControls();
                    }
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView != null && ((ListView)tasksSamplingInfo.InnerView).SelectedObjects.Count > 0)
                    {
                        foreach (SampleLogIn objSampling in ((ListView)tasksSamplingInfo.InnerView).SelectedObjects)
                        {
                            objSampling.SamplingStatus = SamplingStatus.PendingCompletion;
                            objSampling.SamplingRollbackReason = Sampling.SamplingRollbackReason;
                            objSampling.SamplingRollbackedBy = ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objSampling.SamplingRollbackedDate = DateTime.Now;
                        }
                        rollbackstatus(((SampleLogIn)((ListView)tasksSamplingInfo.InnerView).CollectionSource.List[0]).JobID, tasksSamplingInfo.InnerView.ObjectSpace);
                        ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        ListView CreateListView = Application.CreateListView("Samplecheckin_ListView_FieldDataReview1", new CollectionSource(Application.CreateObjectSpace(), typeof(Samplecheckin)), true);
                        Frame.SetView(CreateListView);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select the sampleID to rollback", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FieldDataApproved_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Samplecheckin objSamplecheckin = View.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid] = ?", FDERInfo.TaskRegistrationOid));
                if (objSamplecheckin != null)
                {
                    DashboardViewItem tasksSamplingInfo = ((DetailView)View).FindItem("SamplingInformation") as DashboardViewItem;
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView == null)
                    {
                        tasksSamplingInfo.CreateControl();
                        tasksSamplingInfo.InnerView.CreateControls();
                    }
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView != null && ((ListView)tasksSamplingInfo.InnerView).SelectedObjects.Count > 0)
                    {
                        Employee curemployee = ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        if (curemployee != null)
                        {
                            foreach (SampleLogIn objSampling in ((ListView)tasksSamplingInfo.InnerView).SelectedObjects)
                            {
                                objSampling.SamplingStatus = SamplingStatus.Completed;
                                objSampling.SamplingApprovedBy = curemployee;
                                objSampling.SamplingApprovedDate = DateTime.Now;
                                if (objSampling.SampleStatus == null || !objSampling.SampleStatus.Samplinghold)
                                {
                                    foreach (SampleParameter sp in tasksSamplingInfo.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=? And [Testparameter.TestMethod.IsFieldTest] = True", objSampling.Oid)))
                                    {
                                        sp.IsTransferred = true;
                                        sp.ValidatedBy = curemployee;
                                        sp.ApprovedBy = curemployee;
                                        sp.ValidatedDate = DateTime.Now;
                                        sp.ApprovedDate = DateTime.Now;
                                        sp.Status = Samplestatus.PendingReporting;
                                    }
                                }
                                checkstatus(objSamplecheckin, tasksSamplingInfo.InnerView.ObjectSpace);
                            }
                        }
                        ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "approvesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        e.ShowViewParameters.CreatedView = Application.CreateListView("Samplecheckin_ListView_FieldDataReview2", new CollectionSource(Application.CreateObjectSpace(), typeof(Samplecheckin)), true);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select the sampleID to approve", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FieldDataApprovedRollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(SampleLogIn));
                SampleLogIn obj = os.CreateObject<SampleLogIn>();
                DetailView createdView = Application.CreateDetailView(os, "SampleLogIn_DetailView_FieldDataEntry_Rollback", true, obj);
                createdView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += ApprovedRollBack_Accepting;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ApprovedRollBack_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                SampleLogIn Sampling = (SampleLogIn)e.AcceptActionArgs.CurrentObject;
                if (Sampling != null && !string.IsNullOrEmpty(Sampling.SamplingRollbackReason))
                {
                    DashboardViewItem tasksSamplingInfo = ((DetailView)View).FindItem("SamplingInformation") as DashboardViewItem;
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView == null)
                    {
                        tasksSamplingInfo.CreateControl();
                        tasksSamplingInfo.InnerView.CreateControls();
                    }
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView != null && ((ListView)tasksSamplingInfo.InnerView).SelectedObjects.Count > 0)
                    {
                        SamplingStatus status = SamplingStatus.PendingValidation;
                        IList<DefaultSetting> FDdefsetting = View.ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Sampling Management' And Not IsNullOrEmpty([NavigationItemName])"));
                        if (FDdefsetting != null && FDdefsetting.Count > 0)
                        {
                            DefaultSetting FD1defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "FieldDataReview1").FirstOrDefault();
                            if (FD1defsetting != null)
                            {
                                if (FD1defsetting.Select == false)
                                {
                                    status = SamplingStatus.PendingCompletion;
                                }
                            }
                        }

                        foreach (SampleLogIn objSampling in ((ListView)tasksSamplingInfo.InnerView).SelectedObjects)
                        {
                            objSampling.SamplingStatus = status;
                            objSampling.SamplingRollbackReason = Sampling.SamplingRollbackReason;
                            objSampling.SamplingRollbackedBy = ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objSampling.SamplingRollbackedDate = DateTime.Now;
                            objSampling.SamplingValidatedBy = null;
                            objSampling.SamplingValidatedDate = null;
                            objSampling.SamplingApprovedBy = null;
                            objSampling.SamplingApprovedDate = null;
                        }
                        rollbackstatus(((SampleLogIn)((ListView)tasksSamplingInfo.InnerView).CollectionSource.List[0]).JobID, tasksSamplingInfo.InnerView.ObjectSpace);
                        ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        ListView createListView = Application.CreateListView("Samplecheckin_ListView_FieldDataReview2", new CollectionSource(Application.CreateObjectSpace(), typeof(Samplecheckin)), true);
                        Frame.SetView(createListView);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select the sampleID to rollback", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void FieldDataReviewFilter_Excecute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedChoiceActionItem.Id == "Pending FieldData Review2 Record" || e.SelectedChoiceActionItem.Id == "待现场批准记录")
                {
                    FDERInfo.strMode = "Enter";
                    //if (FDERInfo.strMode == "Enter")
                    //{
                    //    FieldDataApproved.Active.SetItemValue("valApproved", true);
                    //    FieldDataApprovedRollback.Active.SetItemValue("valApproveRollback", false);
                    //}
                    List<object> OidTask = new List<object>();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[Samplelogin.SamplingStatus] = 'PendingApproval'");
                        lstview.Properties.Add(new ViewProperty("group", SortDirection.Ascending, "Samplelogin.JobID.Oid", true, true));
                        foreach (ViewRecord Vrec in lstview)
                            OidTask.Add(Vrec["group"]);
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", OidTask);
                }
                else if (e.SelectedChoiceActionItem.Id == "FieldData Reviewed Record" || e.SelectedChoiceActionItem.Id == "现场审核记录")
                {
                    FDERInfo.strMode = "View";
                    //if (FDERInfo.strMode == "View")
                    //{
                    //    FieldDataApproved.Active.SetItemValue("valApproved", false);
                    //    FieldDataApprovedRollback.Active.SetItemValue("valApproveRollback", true);
                    //}
                    List<object> OidTask = new List<object>();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleLogIn)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[SamplingApprovedBy] Is Not Null And [SamplingApprovedDate] Is Not Null");
                        lstview.Properties.Add(new ViewProperty("group", SortDirection.Ascending, "JobID.Oid", true, true));
                        foreach (ViewRecord Vrec in lstview)
                            OidTask.Add(Vrec["group"]);
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", OidTask);
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
