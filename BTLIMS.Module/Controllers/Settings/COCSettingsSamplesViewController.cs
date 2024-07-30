using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.SamplesSite;
using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class COCSettingsSamplesViewController : ViewController, IXafCallbackHandler
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        bool bolRefresh = false;
        COCSettingsSampleInfo objCOCSampleinfo = new COCSettingsSampleInfo();
        COCSettingsRegistrationInfo COCsr = new COCSettingsRegistrationInfo();
        CopyNoOfSamplesPopUp objCopySampleInfo = new CopyNoOfSamplesPopUp();
        COCSettingsSampleCheckInInfo objCOCInfo = new COCSettingsSampleCheckInInfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        #endregion
        public COCSettingsSamplesViewController()
        {
            InitializeComponent();
            TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration;" + "COCSettingsSamples_DetailView;" + "COCSettingsSamples_ListView;" + "Testparameter_LookupListView_Copy_COCSample;" + "Testparameter_LookupListView_Copy_COCSample_Copy;" + "Testparameter_LookupListView_Copy_COCSample_Copy_Parameter;"
                + "SampleSites_LookupListView_CocSetting;" + "COCSettingsSampleRegistration;";
            CopySamples.TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration;" + "COCSettingsSamples_DetailView;" + "COCSettingsSamples_ListView;";
            SaveCOCSettingSamples.TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration;";

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if (/*View.Id == "COCSettingsSampleRegistration"*/View.ObjectTypeInfo!=null && View.ObjectTypeInfo.Type == typeof(COCSettingsSamples))
            {
                View.SelectionChanged += new EventHandler(View_SelectionChanged);
            }
            else if (View.Id == "SampleSites_LookupListView_CocSetting")
            {
                View.ControlsCreated += View_ControlsCreated;
            }
            else if (View.Id == "COCSettingsSampleRegistration")
            {
                View.Closing += View_Closing;
            }
            //Frame.GetController<COCSettingsViewController>().Actions["SR_SLDetailViewNew"].Active.SetItemValue("Show", false);
            //Frame.GetController<COCSettingsViewController>().Actions["SampleRegistrationSL_Save"].Active.SetItemValue("Show", false);
            if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
            {
                if (objPermissionInfo.COCSettingsIsWrite == false)
                {
                    CopySamples.Active["showCopySamples"] = false;
                    //Btn_Add_Collector.Active["showAddCollector"] = false;
                }
                else
                {
                    CopySamples.Active["showCopySamples"] = true;
                    //Btn_Add_Collector.Active["showAddCollector"] = true;
                }
            }
            CopySamples.Executing += CopySamples_Executing;
        }

        private void View_Closing(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem lvSamples = ((DashboardView)View).FindItem("COCSettingsSample") as DashboardViewItem;
                if (lvSamples != null && lvSamples.InnerView != null && lvSamples.InnerView.ObjectSpace.ModifiedObjects.Count > 0)
                {
                    lvSamples.InnerView.ObjectSpace.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                if (View.Id== "SampleSites_LookupListView_CocSetting")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        if (HttpContext.Current.Session["StationLocation"] != null)
                        {
                            string str = HttpContext.Current.Session["StationLocation"].ToString();
                            SampleSites site = ObjectSpace.FindObject<SampleSites>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["StationLocation"].ToString())));
                            if (site != null)
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(site.Oid);
                            }
                        }
                    } 
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void View_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(COCSettingsSamples))
                {
                    if (View.CurrentObject != null)
                    {
                        COCSettingsSamples cocSam = (COCSettingsSamples)View.CurrentObject;
                        if (cocSam.VisualMatrix != null)
                        {
                            objCOCSampleinfo.COCVisualMatrixName = cocSam.VisualMatrix.MatrixName.MatrixName;
                        }
                        if (cocSam.COCID != null)
                        {
                            objCOCSampleinfo.COCID = View.ObjectSpace.GetKeyValue(View.CurrentObject).ToString();
                            objCOCSampleinfo.focusedCOCID = cocSam.COCID.COC_ID;
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
            base.OnViewControlsCreated();
            if (View.Id == "Testparameter_LookupListView_Copy_COCSample")
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e){ 
                s.SetWidth(400); 
                s.RowClick.ClearHandlers();
                }";
                gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
            }
            else if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
            {
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesController");
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                selparameter.CallbackManager.RegisterHandler("GridColumnPopup", this);
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                }
               
            }
            else if (View.Id == "SampleSites_LookupListView_CocSetting")
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 410;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                }
            }
        }

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" && objPermissionInfo.COCSettingsIsWrite)
                {
                    if (e.DataColumn.FieldName == "StationLocation.Oid"||e.DataColumn.FieldName == "StationLocationName")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'GridColumnPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            if (/*View.Id == "COCSettingsSampleRegistration"*/View.ObjectTypeInfo!=null&& View.ObjectTypeInfo.Type == typeof(COCSettingsSamples))
            {
                View.SelectionChanged -= new EventHandler(View_SelectionChanged);
            }
            if (View != null && View.Id == "COCSettingsSamples_DetailView")
            {
                objCOCSampleinfo.COCVisualMatrixName = string.Empty;
            }
            else if (View.Id == "COCSettingsSampleRegistration")
            {
                View.Closing -= View_Closing;
            }
            CopySamples.Executing -= CopySamples_Executing;

        }
        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "COCID")
                {
                    if (View.ObjectTypeInfo!=null && View.ObjectTypeInfo.Type == typeof(COCSettingsSamples))
                    {
                        COCSettingsSamples objCOCSampleLogIn = (COCSettingsSamples)e.Object;

                        if (objCOCSampleLogIn.COCID != null)
                        {
                            objCOCSampleinfo.focusedCOCID = objCOCSampleLogIn.COCID.COC_ID;
                            objCOCSampleLogIn.ProjectID = objCOCSampleLogIn.COCID.ProjectID;
                            Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;

                            SelectedData sproc = currentSession.ExecuteSproc("GetCOCSampleID", new OperandValue(objCOCSampleLogIn.COCID.COC_ID));
                            if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
                            {
                                objCOCSampleLogIn.SampleNo = Convert.ToInt32(sproc.ResultSet[1].Rows[0].Values[0].ToString());
                                objCOCSampleLogIn.SampleID = string.Format("{0}{1}{2}", objCOCSampleLogIn.COCID.COC_ID, "-", objCOCSampleLogIn.SampleNo.ToString());
                            }
                            if (objCOCSampleLogIn.QCType != null)
                            {
                                objCOCSampleLogIn.QCType = null;
                            }
                        }
                    }
                }
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "ProjectID")
                {
                    if (View.ObjectTypeInfo.Type == typeof(COCSettingsSamples))
                    {
                        COCSettingsSamples objCOCSampleLogIn = (COCSettingsSamples)e.Object;
                        if (objCOCSampleLogIn.ProjectID != null)
                        {
                            objCOCSampleLogIn.ProjectName = objCOCSampleLogIn.ProjectID.ProjectName;
                        }
                        else
                        {
                            objCOCSampleLogIn.ProjectName = string.Empty;
                        }

                    }
                }
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "VisualMatrix")
                {
                    bool bolTestparam = false;

                    if (View.ObjectTypeInfo.Type == typeof(COCSettingsSamples))
                    {
                        COCSettingsSamples objCOCSampleLogIn = (COCSettingsSamples)e.Object;
                        if (objCOCSampleLogIn.VisualMatrix != null)
                        {
                            string strOldVisualMatrix = objCOCSampleLogIn.VisualMatrix.VisualMatrixName.ToString();
                            if (CheckIFExists(objCOCSampleLogIn))
                            {
                                var objectSpace = Application.CreateObjectSpace();
                                var objLogin = objectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("Oid = ?", objCOCSampleLogIn.Oid));
                                if (objLogin == null)
                                {
                                    for (int i = objCOCSampleLogIn.Testparameters.Count - 1; i >= 0; i--)
                                    {
                                        objCOCSampleLogIn.Testparameters.Remove(objCOCSampleLogIn.Testparameters[i]);
                                        bolRefresh = true;
                                    }
                                }
                                else
                                {
                                    foreach (Testparameter objTestParam in objCOCSampleLogIn.Testparameters)
                                    {
                                        var osTestParam = Application.CreateObjectSpace();
                                        IList<COCSettingsTest> lstSampleParam = (IList<COCSettingsTest>)objTestParam.COCSettingsTests;
                                        {
                                            foreach (var li in lstSampleParam)
                                            {
                                                if (objTestParam == li.Testparameter)
                                                {
                                                    if (objCOCSampleLogIn.Oid.ToString() == li.COCSettingsSamples.Oid.ToString())
                                                    {
                                                        if (li.Result != null && li.Result != string.Empty)
                                                        // if (li.Result != null)
                                                        {
                                                            bolTestparam = true;
                                                            try
                                                            {
                                                                throw new UserFriendlyException(string.Format("Result Already Entered Cannot allow to change the Sample Matrix"));
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                DevExpress.ExpressApp.Web.ErrorHandling.Instance.SetPageError(ex);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    if (bolTestparam == false)
                                    {
                                        for (int i = objCOCSampleLogIn.Testparameters.Count - 1; i >= 0; i--)
                                        {
                                            objCOCSampleLogIn.Testparameters.Remove(objCOCSampleLogIn.Testparameters[i]);
                                            bolRefresh = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objCOCSampleinfo.COCVisualMatrixName = objCOCSampleLogIn.VisualMatrix.MatrixName.MatrixName;
                            }
                            if (bolRefresh == true)
                            {
                                View.Refresh();
                                bolRefresh = false;
                            }
                        }
                        else
                        {
                            objCOCSampleinfo.COCVisualMatrixName = string.Empty;
                        }
                    }

                }

                if (View != null && View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" && /*e.GetType() == typeof(COCSettingsSamples) &&*/ e.PropertyName == "Containers")
                {
                    if (View.ObjectTypeInfo.Type == typeof(COCSettingsSamples))
                    {
                        COCSettingsSamples objSampleLogIn = (COCSettingsSamples)e.Object;
                        if (objSampleLogIn.Containers <= 0)
                        {
                            objSampleLogIn.Containers = 1;
                            Application.ShowViewStrategy.ShowMessage("Containers Should not be less than 1", InformationType.Warning, 3000, InformationPosition.Top);
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
        private bool CheckIFExists(COCSettingsSamples objCOCSL)
        {
            try
            {
                if (objCOCSL.Testparameters.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }


        private void COCSampleViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                if (View is DetailView && View.ObjectTypeInfo!=null && View.ObjectTypeInfo.Type == typeof(COCSettingsSamples))
                {
                    if (objCOCInfo.COCID != string.Empty)
                    {

                        COCSettingsSamples cocS = (COCSettingsSamples)View.CurrentObject;
                        if (cocS != null)
                        {
                            COCSettings sc = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("[COC_ID]='" + objCOCInfo.COCID + "'"));
                            cocS.COCID = sc;
                            ObjectChangedEventArgs ee = new ObjectChangedEventArgs(cocS.COCID, "COCID", null, sc);
                            objectSpace_ObjectChanged(cocS.COCID, ee);
                            objCOCInfo.COCID = string.Empty;
                        }
                    }
                    object obj = View.CurrentObject;
                    if (View.CurrentObject != null)
                    {
                        objCOCSampleinfo.COCID = View.ObjectSpace.GetKeyValue(View.CurrentObject).ToString();
                        COCSettingsSamples sl = (COCSettingsSamples)View.CurrentObject;
                        if (sl.COCID != null)
                        {
                            CopySamples.Enabled["enable"] = true;
                            objCOCSampleinfo.focusedCOCID = sl.COCID.COC_ID;
                        }

                        else
                        {
                            CopySamples.Enabled["enable"] = false;
                        }
                    }

                }
                if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Testparameter) && View.Id == "Testparameter_LookupListView_Copy_COCSample")
                {

                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName]='" + objCOCSampleinfo.COCVisualMatrixName + "' AND [TestMethod.GCRecord] IS NULL AND (([TestMethod.RetireDate] IS NULL OR [TestMethod.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')) AND " +
                       " ([TestMethod.MethodName.RetireDate] IS NULL OR [TestMethod.MethodName.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "') AND ([Parameter.RetireDate] IS NULL OR [Parameter.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" + "AND([RetireDate] IS NULL OR[RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" +
                        "AND ([InternalStandard] == False or [InternalStandard] IS NULL ) AND ([Surroagate] == False or [Surroagate] IS NULL)");


                }

                if (View.Id == "Testparameter_LookupListView_Copy_COCSample_Copy")
                {
                    DashboardViewItem TestViewSubChild = ((NestedFrame)Frame).ViewItem.View.FindItem("TestViewSubChild") as DashboardViewItem;
                    if ((COCsr.lstTestParameter == null || COCsr.lstTestParameter.Count == 0) && (TestViewSubChild != null && ((ListView)TestViewSubChild.InnerView).CollectionSource.GetCount() == 0))
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                }
                //if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(COCSettingsSamples) && View.Id == "SampleLogIn_LookupListView_Copy_SampleLogin")
                //{
                //    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[COCID.COC_ID]=='" + objCOCSampleinfo.focusedCOCID + "' AND [GCRecord] IS NULL");
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopySamples_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    gridListEditor.Grid.UpdateEdit();
                }
                IObjectSpace objspace = Application.CreateObjectSpace();
                SL_CopyNoOfSamples copyNoOfSamples = objspace.CreateObject<SL_CopyNoOfSamples>();
                DetailView dvcopysample = Application.CreateDetailView(objspace, "SL_CopyNoOfSamples_DetailView", false, copyNoOfSamples);
                dvcopysample.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(dvcopysample);
                showViewParameters.CreatedView = dvcopysample;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += CopySamples_Accepting;
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
        private void CopySamples_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (objCOCSampleinfo.COCID != string.Empty)
                {
                    if (objCopySampleInfo.NoOfSamples > 0)
                    {
                        objCopySampleInfo.Msgflag = false;
                        bool DBAccess = false;
                        string CocID = string.Empty;
                        int SampleNo = 0;
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)(objectSpace)).Session;
                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                        if (View != null && View.CurrentObject != null && View.ObjectTypeInfo.Type == typeof(COCSettingsSamples))
                        {
                            COCSettingsSamples objcocSampleOld = (COCSettingsSamples)View.CurrentObject;
                            List<COCSettingsBottleAllocation> smplold = uow.Query<COCSettingsBottleAllocation>().Where(i => i.COCSettingsRegistration != null && i.COCSettingsRegistration.Oid == objcocSampleOld.Oid).ToList();
                            VisualMatrix visualMatrix = null;

                            COCSettings objJobId = uow.GetObjectByKey<COCSettings>(objcocSampleOld.COCID.Oid);
                            if (objcocSampleOld.VisualMatrix != null)
                            {
                                visualMatrix = uow.GetObjectByKey<VisualMatrix>(objcocSampleOld.VisualMatrix.Oid);
                            }
                            for (int i = 1; i <= objCopySampleInfo.NoOfSamples; i++)
                            {
                                COCSettingsSamples objcocSLNew = new COCSettingsSamples(uow);
                                objcocSLNew.COCID = objJobId;
                                if (DBAccess == false)
                                {
                                    SelectedData sproc = currentSession.ExecuteSproc("GetCOCSampleID", new OperandValue(objcocSLNew.COCID.COC_ID.ToString()));
                                    if (sproc.ResultSet[1].Rows[0].Values[0] != null)
                                    {
                                        objCOCSampleinfo.SampleID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                        SampleNo = Convert.ToInt32(objCOCSampleinfo.SampleID);
                                        DBAccess = true;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                objCOCSampleinfo.boolCopySamples = true;
                                objcocSLNew.SampleNo = SampleNo;
                                objcocSLNew.Test = true;
                                if (visualMatrix != null)
                                {
                                    objcocSLNew.VisualMatrix = visualMatrix;
                                }
                                if (objcocSampleOld.SampleType != null)
                                {
                                    objcocSLNew.SampleType = uow.GetObjectByKey<SampleType>(objcocSampleOld.SampleType.Oid);
                                }
                                objcocSLNew.Qty = objcocSampleOld.Qty;
                                if (objcocSampleOld.Storage != null)
                                {
                                    objcocSLNew.Storage = uow.GetObjectByKey<Storage>(objcocSampleOld.Storage.Oid);
                                }
                                objcocSLNew.Preservetives = objcocSampleOld.Preservetives;
                                objcocSLNew.SamplingLocation = objcocSampleOld.SamplingLocation;
                                if (objcocSampleOld.QCType != null)
                                {
                                    objcocSLNew.QCType = uow.GetObjectByKey<QCType>(objcocSampleOld.QCType.Oid);
                                }
                                if (objcocSampleOld.QCSource != null)
                                {
                                    objcocSLNew.QCSource = uow.GetObjectByKey<COCSettingsSamples>(objcocSampleOld.QCSource.Oid);
                                }
                                if (objcocSampleOld.Client != null)
                                {
                                    objcocSLNew.Client = uow.GetObjectByKey<Customer>(objcocSampleOld.Client);
                                }
                                if (objcocSampleOld.Department != null)
                                {
                                    objcocSLNew.Department = uow.GetObjectByKey<Department>(objcocSampleOld.Department);
                                }
                                if (objcocSampleOld.Department != null)
                                {
                                    objcocSLNew.ProjectID = uow.GetObjectByKey<Project>(objcocSampleOld.ProjectID);
                                }
                                if (objcocSampleOld.PreserveCondition != null)
                                {
                                    objcocSLNew.PreserveCondition = uow.GetObjectByKey<PreserveCondition>(objcocSampleOld.PreserveCondition);
                                }
                                if (objcocSampleOld.StorageID != null)
                                {

                                    objcocSLNew.StorageID = uow.GetObjectByKey<Storage>(objcocSampleOld.StorageID);
                                }
                                //objcocSLNew.CollectDate = objcocSampleOld.CollectDate;
                                //objcocSLNew.CollectTime = objcocSampleOld.CollectTime;
                                objcocSLNew.FlowRate = objcocSampleOld.FlowRate;
                                objcocSLNew.TimeStart = objcocSampleOld.TimeStart;
                                objcocSLNew.TimeEnd = objcocSampleOld.TimeEnd;
                                objcocSLNew.Time = objcocSampleOld.Time;
                                objcocSLNew.Volume = objcocSampleOld.Volume;
                                objcocSLNew.Address = objcocSampleOld.Address;
                                objcocSLNew.AreaOrPerson = objcocSampleOld.AreaOrPerson;
                                if (objcocSampleOld.BalanceID != null)
                                {
                                    objcocSLNew.BalanceID = uow.GetObjectByKey<Modules.BusinessObjects.Assets.Labware>(objcocSampleOld.BalanceID.Oid);
                                }
                                objcocSLNew.AssignTo = objcocSampleOld.AssignTo;
                                objcocSLNew.Barp = objcocSampleOld.Barp;
                                objcocSLNew.BatchID = objcocSampleOld.BatchID;
                                objcocSLNew.BatchSize = objcocSampleOld.BatchSize;
                                objcocSLNew.BatchSize_pc = objcocSampleOld.BatchSize_pc;
                                objcocSLNew.BatchSize_Units = objcocSampleOld.BatchSize_Units;
                                objcocSLNew.Blended = objcocSampleOld.Blended;
                                objcocSLNew.BottleQty = objcocSampleOld.BottleQty;
                                objcocSLNew.BuriedDepthOfGroundWater = objcocSampleOld.BuriedDepthOfGroundWater;
                                objcocSLNew.ChlorineFree = objcocSampleOld.ChlorineFree;
                                objcocSLNew.ChlorineTotal = objcocSampleOld.ChlorineTotal;
                                objcocSLNew.City = objcocSampleOld.City;
                                //objcocSLNew.CollectorPhone = objcocSampleOld.CollectorPhone;
                                //objcocSLNew.CollectTimeDisplay = objcocSampleOld.CollectTimeDisplay;
                                objcocSLNew.CompositeQty = objcocSampleOld.CompositeQty;
                                objcocSLNew.DateEndExpected = objcocSampleOld.DateEndExpected;
                                objcocSLNew.DateStartExpected = objcocSampleOld.DateStartExpected;
                                objcocSLNew.Depth = objcocSampleOld.Depth;
                                objcocSLNew.Description = objcocSampleOld.Description;
                                objcocSLNew.DischargeFlow = objcocSampleOld.DischargeFlow;
                                objcocSLNew.DischargePipeHeight = objcocSampleOld.DischargePipeHeight;
                                objcocSLNew.DO = objcocSampleOld.DO;
                                //objcocSLNew.DueDate = objcocSampleOld.DueDate;
                                objcocSLNew.Emission = objcocSampleOld.Emission;
                                objcocSLNew.EndOfRoad = objcocSampleOld.EndOfRoad;
                                objcocSLNew.EquipmentModel = objcocSampleOld.EquipmentModel;
                                objcocSLNew.EquipmentName = objcocSampleOld.EquipmentName;
                                objcocSLNew.FacilityID = objcocSampleOld.FacilityID;
                                objcocSLNew.FacilityName = objcocSampleOld.FacilityName;
                                objcocSLNew.FacilityType = objcocSampleOld.FacilityType;
                                objcocSLNew.FinalForm = objcocSampleOld.FinalForm;
                                objcocSLNew.FinalPackaging = objcocSampleOld.FinalPackaging;
                                objcocSLNew.FlowRate = objcocSampleOld.FlowRate;
                                objcocSLNew.FlowRateCubicMeterPerHour = objcocSampleOld.FlowRateCubicMeterPerHour;
                                objcocSLNew.FlowRateLiterPerMin = objcocSampleOld.FlowRateLiterPerMin;
                                objcocSLNew.FlowVelocity = objcocSampleOld.FlowVelocity;
                                objcocSLNew.ForeignMaterial = objcocSampleOld.ForeignMaterial;
                                objcocSLNew.Frequency = objcocSampleOld.Frequency;
                                objcocSLNew.GISStatus = objcocSampleOld.GISStatus;
                                objcocSLNew.GravelContent = objcocSampleOld.GravelContent;
                                objcocSLNew.GrossWeight = objcocSampleOld.GrossWeight;
                                objcocSLNew.GroupSample = objcocSampleOld.GroupSample;
                                objcocSLNew.Hold = objcocSampleOld.Hold;
                                objcocSLNew.Humidity = objcocSampleOld.Humidity;
                                objcocSLNew.IceCycle = objcocSampleOld.IceCycle;
                                objcocSLNew.Increments = objcocSampleOld.Increments;
                                objcocSLNew.Interval = objcocSampleOld.Interval;
                                objcocSLNew.IsActive = objcocSampleOld.IsActive;
                                //objcocSLNew.IsNotTransferred = objcocSampleOld.IsNotTransferred;
                                objcocSLNew.ItemName = objcocSampleOld.ItemName;
                                objcocSLNew.KeyMap = objcocSampleOld.KeyMap;
                                objcocSLNew.LicenseNumber = objcocSampleOld.LicenseNumber;
                                objcocSLNew.ManifestNo = objcocSampleOld.ManifestNo;
                                objcocSLNew.MonitoryingRequirement = objcocSampleOld.MonitoryingRequirement;
                                objcocSLNew.NoOfCollectionsEachTime = objcocSampleOld.NoOfCollectionsEachTime;
                                objcocSLNew.NoOfPoints = objcocSampleOld.NoOfPoints;
                                objcocSLNew.Notes = objcocSampleOld.Notes;
                                objcocSLNew.OriginatingEntiry = objcocSampleOld.OriginatingEntiry;
                                objcocSLNew.OriginatingLicenseNumber = objcocSampleOld.OriginatingLicenseNumber;
                                objcocSLNew.PackageNumber = objcocSampleOld.PackageNumber;
                                objcocSLNew.ParentSampleDate = objcocSampleOld.ParentSampleDate;
                                objcocSLNew.ParentSampleID = objcocSampleOld.ParentSampleID;
                                objcocSLNew.PiecesPerUnit = objcocSampleOld.PiecesPerUnit;
                                objcocSLNew.Preservetives = objcocSampleOld.Preservetives;
                                objcocSLNew.ProjectName = objcocSampleOld.ProjectName;
                                objcocSLNew.PurifierSampleID = objcocSampleOld.PurifierSampleID;
                                objcocSLNew.PWSID = objcocSampleOld.PWSID;
                                if (objcocSampleOld.PWSSystemName!=null)
                                {
                                    objcocSLNew.PWSSystemName =uow.GetObjectByKey<PWSSystem>(objcocSampleOld.PWSSystemName.Oid);
                                } 
                                objcocSLNew.RegionNameOfSection = objcocSampleOld.RegionNameOfSection;
                                objcocSLNew.RejectionCriteria = objcocSampleOld.RejectionCriteria;
                                objcocSLNew.RepeatLocation = objcocSampleOld.RepeatLocation;
                                objcocSLNew.RetainedWeight = objcocSampleOld.RetainedWeight;
                                objcocSLNew.RiverWidth = objcocSampleOld.RiverWidth;
                                objcocSLNew.RushSample = objcocSampleOld.RushSample;
                                objcocSLNew.SampleAmount = objcocSampleOld.SampleAmount;
                                objcocSLNew.SampleCondition = objcocSampleOld.SampleCondition;
                                objcocSLNew.SampleDescription = objcocSampleOld.SampleDescription;
                                objcocSLNew.SampleImage = objcocSampleOld.SampleImage;
                                objcocSLNew.SampleName = objcocSampleOld.SampleName;
                                objcocSLNew.SamplePointID = objcocSampleOld.SamplePointID;
                                objcocSLNew.SamplePointType = objcocSampleOld.SamplePointType;
                                objcocSLNew.SampleSource = objcocSampleOld.SampleSource;
                                objcocSLNew.SampleTag = objcocSampleOld.SampleTag;
                                objcocSLNew.SampleWeight = objcocSampleOld.SampleWeight;
                                objcocSLNew.SamplingAddress = objcocSampleOld.SamplingAddress;
                                objcocSLNew.SamplingEquipment = objcocSampleOld.SamplingEquipment;
                                objcocSLNew.SamplingLocation = objcocSampleOld.SamplingLocation;
                                objcocSLNew.SamplingProcedure = objcocSampleOld.SamplingProcedure;
                                objcocSLNew.SequenceTestSampleID = objcocSampleOld.SequenceTestSampleID;
                                objcocSLNew.SequenceTestSortNo = objcocSampleOld.SequenceTestSortNo;
                                objcocSLNew.ServiceArea = objcocSampleOld.ServiceArea;
                                objcocSLNew.SiteCode = objcocSampleOld.SiteCode;
                                objcocSLNew.SiteDescription = objcocSampleOld.SiteDescription;
                                objcocSLNew.SiteID = objcocSampleOld.SiteID;
                                objcocSLNew.SiteNameArchived = objcocSampleOld.SiteNameArchived;
                                objcocSLNew.SiteUserDefinedColumn1 = objcocSampleOld.SiteUserDefinedColumn1;
                                objcocSLNew.SiteUserDefinedColumn2 = objcocSampleOld.SiteUserDefinedColumn2;
                                objcocSLNew.SiteUserDefinedColumn3 = objcocSampleOld.SiteUserDefinedColumn3;
                                objcocSLNew.SubOut = objcocSampleOld.SubOut;
                                objcocSLNew.SystemType = objcocSampleOld.SystemType;
                                objcocSLNew.TargetMGTHC_CBD_mg_pc = objcocSampleOld.TargetMGTHC_CBD_mg_pc;
                                objcocSLNew.TargetMGTHC_CBD_mg_unit = objcocSampleOld.TargetMGTHC_CBD_mg_unit;
                                objcocSLNew.TargetPotency = objcocSampleOld.TargetPotency;
                                objcocSLNew.TargetUnitWeight_g_pc = objcocSampleOld.TargetUnitWeight_g_pc;
                                objcocSLNew.TargetUnitWeight_g_unit = objcocSampleOld.TargetUnitWeight_g_unit;
                                objcocSLNew.TargetWeight = objcocSampleOld.TargetWeight;
                                objcocSLNew.Time = objcocSampleOld.Time;
                                objcocSLNew.TimeEnd = objcocSampleOld.TimeEnd;
                                objcocSLNew.TimeStart = objcocSampleOld.TimeStart;
                                objcocSLNew.TotalSamples = objcocSampleOld.TotalSamples;
                                objcocSLNew.TotalTimes = objcocSampleOld.TotalTimes;
                                if (objcocSampleOld.TtimeUnit != null)
                                {
                                    objcocSLNew.TtimeUnit = uow.GetObjectByKey<Unit>(objcocSampleOld.TtimeUnit.Oid);
                                }
                                objcocSLNew.WaterType = objcocSampleOld.WaterType;
                                objcocSLNew.ZipCode = objcocSampleOld.ZipCode;
                                if (objcocSampleOld.ModifiedBy != null)
                                {
                                    objcocSLNew.ModifiedBy = uow.GetObjectByKey<Modules.BusinessObjects.Hr.CustomSystemUser>(objcocSampleOld.ModifiedBy.Oid);
                                }
                                objcocSLNew.ModifiedDate = objcocSampleOld.ModifiedDate;
                                objcocSLNew.Comment = objcocSampleOld.Comment;
                                objcocSLNew.Latitude = objcocSampleOld.Latitude;
                                objcocSLNew.Longitude = objcocSampleOld.Longitude;
                                List<Testparameter> lsttp = uow.Query<Testparameter>().Where(j => j.QCType.QCTypeName == "Sample" && j.COCSettingsSample.Where(a => a.Oid == objcocSampleOld.Oid).Count() > 0).ToList();
                                foreach (var objLineA in lsttp)
                                {
                                    objcocSLNew.Testparameters.Add(uow.GetObjectByKey<Testparameter>(objLineA.Oid));
                                }
                                foreach (var objSampleparameter in objcocSampleOld.COCSettingsTests.Where(a => a.IsGroup == true && a.GroupTest != null).ToList())
                                {
                                    COCSettingsTest sample = objcocSLNew.COCSettingsTests.FirstOrDefault<COCSettingsTest>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                                    if (objSampleparameter.GroupTest != null && sample != null)
                                    {
                                        sample.IsGroup = true;
                                        sample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objSampleparameter.GroupTest.Oid);
                                    }
                                }
                                foreach (var objSampleparameter in objcocSampleOld.COCSettingsTests.Where(a => a.SubOut == true).ToList())
                                {
                                    COCSettingsTest sample = objcocSLNew.COCSettingsTests.FirstOrDefault<COCSettingsTest>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                                    if (sample != null)
                                    {
                                        sample.SubOut = true;
                                    }
                                }
                                objcocSLNew.Save();
                                SampleNo++;
                                if (smplold != null && smplold.Count > 0)
                                {
                                    foreach (COCSettingsBottleAllocation smpl in smplold.ToList())
                                    {
                                        COCSettingsBottleAllocation smplnew = new COCSettingsBottleAllocation(uow);
                                        smplnew.COCSettingsRegistration = objcocSLNew;
                                        smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(smpl.TestMethod.Oid);
                                        smplnew.BottleID = smpl.BottleID;
                                        if (smpl.Containers != null)
                                        {
                                            smplnew.Containers = uow.GetObjectByKey<Modules.BusinessObjects.Setting.Container>(smpl.Containers.Oid);
                                        }
                                        if (smpl.Preservative != null)
                                        {
                                            smplnew.Preservative = uow.GetObjectByKey<Preservative>(smpl.Preservative.Oid);
                                        }
                                        if (smpl.StorageID != null)
                                        {
                                            smplnew.StorageID = uow.GetObjectByKey<Storage>(smpl.StorageID.Oid);
                                        }
                                        if (smpl.StorageCondition != null)
                                        {
                                            smplnew.StorageCondition = uow.GetObjectByKey<PreserveCondition>(smpl.StorageCondition.Oid);
                                        }
                                    }
                                }
                            }
                            uow.CommitChanges();
                        }
                        //SelectedData updateStatusProc = currentSession.ExecuteSproc("StatusUpdate_SP");
                        objCopySampleInfo.NoOfSamples = 0;
                        objCOCSampleinfo.boolCopySamples = false;
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                            {
                                if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                                {
                                    //frameContainer.Frame.View.ObjectSpace.Refresh();
                                    if (frameContainer.Frame.View is DetailView)
                                    {
                                        frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
                                    }
                                    else
                                    {
                                        (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
                                    }
                                    frameContainer.Frame.View.Refresh();
                                }
                            }
                        }
                        View.Refresh();
                        View.RefreshDataSource();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CopySamples_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count == 0)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                else if (View != null && View.SelectedObjects.Count > 1)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    string[] param = parameter.Split('|');
                    if (param[0] == "StationLocation.Oid"||param[0]== "StationLocationName")
                    {
                        if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                        {
                            object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["StationLocation"] = editor.Grid.GetRowValues(int.Parse(param[1]), "StationLocation.Oid");
                            IObjectSpace os = Application.CreateObjectSpace();
                            COCSettingsSamples objSample = os.GetObjectByKey<COCSettingsSamples>(currentOid);
                            if (objSample != null)
                            {
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(labwareObjectSpace, typeof(SampleSites));
                                cs.Criteria["Filter1"] = CriteriaOperator.Parse("[Client.Oid] = ?", objSample.COCID.ClientName.Oid);
                                showViewParameters.CreatedView = Application.CreateListView("SampleSites_LookupListView_CocSetting", cs, false);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.AcceptAction.Execute += AcceptAction_Execute_StationLocation;
                                dc.Accepting += Dc_Accepting_StationLocation;
                                dc.SaveOnAccept = false;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting_StationLocation(object sender, DialogControllerAcceptingEventArgs e)
        {
          try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 1)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AcceptAction_Execute_StationLocation(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session["rowid"] != null)
                {
                    COCSettingsSamples objsampling = ((ListView)View).CollectionSource.List.Cast<COCSettingsSamples>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                    SampleSites objSite = e.SelectedObjects.Cast<SampleSites>().FirstOrDefault();
                    if (objSite != null)
                    {
                        objSite = View.ObjectSpace.GetObjectByKey<SampleSites>(objSite.Oid);
                        if (objsampling != null)
                        {
                            objsampling.StationLocation = objSite;
                            objsampling.PWSID = objSite.PWSID;
                            objsampling.KeyMap = objSite.KeyMap;
                            objsampling.Address = objSite.Address;
                            objsampling.SamplePointID = objSite.SamplePointID;
                            objsampling.SamplePointType = objSite.SamplePointType;
                            objsampling.StationLocationName = objSite.SiteName;
                            if (objSite.SystemType!=null)
                            {
                                objsampling.SystemType = View.ObjectSpace.GetObjectByKey<SystemTypes>(objSite.SystemType.Oid);
                            }
                            objsampling.SamplingAddress = objSite.SamplingAddress;
                            if (objSite.PWSSystemName!=null)
                            {
                                objsampling.PWSSystemName = View.ObjectSpace.GetObjectByKey<PWSSystem>(objSite.PWSSystemName.Oid); 
                            }
                            objsampling.RejectionCriteria = objSite.RejectionCriteria;
                            if (objSite.WaterType!=null)
                            {
                                objsampling.WaterType = View.ObjectSpace.GetObjectByKey<WaterTypes>(objSite.WaterType.Oid);
                            }
                            objsampling.ParentSampleID = objSite.ParentSampleID;
                            objsampling.ParentSampleDate = objSite.ParentSampleDate;
                        }
                    }
                    else
                    {
                        objsampling.StationLocationName = null;
                        objsampling.StationLocation = null;
                        objsampling.PWSID = null;
                        objsampling.KeyMap = null;
                        objsampling.Address = null;
                        objsampling.SamplePointID = null;
                        objsampling.SamplePointType = null;
                        objsampling.SystemType = null;
                        objsampling.PWSSystemName = null;
                        objsampling.RejectionCriteria = null;
                        objsampling.WaterType = null;
                        objsampling.ParentSampleID = null;
                        objsampling.ParentSampleDate = null;
                        objsampling.SamplingAddress = null;
                    }
                 ((ListView)View).Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Save_Samples(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (((ListView)View).CollectionSource.List.Cast<COCSettingsSamples>().FirstOrDefault(i => i.StationLocation == null) != null)
                {
                    Application.ShowViewStrategy.ShowMessage("Enter the station location.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }
                else
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.UpdateEdit();
                        if (View.ObjectSpace.ModifiedObjects.Count > 0)
                        {
                            View.ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
