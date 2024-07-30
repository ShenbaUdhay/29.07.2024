using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using LDM.Module.Controllers.SamplingManagement;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.PLM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.SampleLogIn
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SampleLoginCopyTestGroupTestViewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        List<Guid> TPOid = new List<Guid>();
        string SLOid = string.Empty;
        string FocusedJobID = string.Empty;
        SampleLogInInfo objSLInfo = new SampleLogInInfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        Modules.BusinessObjects.SampleManagement.SampleLogIn objParentSL;
        #endregion

        #region Constructor
        public SampleLoginCopyTestGroupTestViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "SampleLogIn_Testparameters_ListView;" + "SampleLogIn_LookupListView_Copy_SampleLogin_Copy_CopyTest;" + "SampleLogIn_DetailView;" + "GroupTest_ListView_Copy_Samplelogin;" + "SampleLogIn_ListView_Copy_SampleRegistration";
            SL_CopyTest.TargetViewId = "SampleLogIn_Testparameters_ListView;" + "SampleLogIn_ListView_Copy_SampleRegistration";
            SL_GroupTest.TargetViewId = "SampleLogIn_Testparameters_ListView";
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();


        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                SL_CopyTest.Executing += SL_CopyTest_Executing;
                if (View != null && View.CurrentObject != null && View.Id == "SampleLogIn_DetailView")
                {
                    objSLInfo.SLOid = ObjectSpace.GetKeyValueAsString((Modules.BusinessObjects.SampleManagement.SampleLogIn)View.CurrentObject);
                    if (((DetailView)View).ViewEditMode == DevExpress.ExpressApp.Editors.ViewEditMode.View)
                    {
                        SL_CopyTest.Active.SetItemValue("SL_CopyTest", false);
                        SL_GroupTest.Active.SetItemValue("SL_GroupTest", false);
                    }
                    else
                    {
                        SL_CopyTest.Active.SetItemValue("SL_CopyTest", true);
                        SL_GroupTest.Active.SetItemValue("SL_GroupTest", true);
                    }
                    ((DetailView)View).FindItem("Testparameters").Refresh();
                }
                else if (View != null && View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                {
                    if (objPermissionInfo.SampleRegIsWrite == false)
                    {
                        SL_CopyTest.Active["showCopyTest"] = false;
                    }
                    else
                    {
                        SL_CopyTest.Active["showCopyTest"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SL_CopyTest_Executing(object sender, System.ComponentModel.CancelEventArgs e)
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

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        #endregion

        #region Events
        private void SL_CopyTest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                List<string> FocusedMatrix = new List<string>();
                if(View.ObjectSpace.ModifiedObjects.Count>0)
                {
                    View.ObjectSpace.CommitChanges();
                }
                ////if (View != null && View.Id == "SampleLogIn_Testparameters_ListView" && View.SelectedObjects.Count > 0)
                ////{
                ////    foreach (Testparameter objtp in View.SelectedObjects)
                ////    {
                ////        TPOid.Add(objtp.Oid);
                ////        FocusedMatrix.Add(objtp.TestMethod.MatrixName.MatrixName.ToString());
                ////    }
                ////    IObjectSpace objspace = Application.CreateObjectSpace();
                ////    object objToShow = objspace.CreateObject(typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn));
                ////    if (objToShow != null)
                ////    {
                ////        CollectionSource cs = new CollectionSource(objspace, typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn));
                ////        cs.Criteria.Clear();
                ////        if (View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" && !string.IsNullOrEmpty(objSLInfo.SLOid))
                ////        {
                ////            cs.Criteria["filter1"] = CriteriaOperator.Parse("[JobID.JobID]='" + objSLInfo.focusedJobID + "' and Oid <> ?", new Guid(objSLInfo.SLOid));
                ////        }
                ////        else
                ////        {
                ////            cs.Criteria["filter1"] = CriteriaOperator.Parse("[JobID.JobID]='" + objSLInfo.focusedJobID + "'");
                ////        }
                ////        cs.Criteria["filter2"] = new InOperator("VisualMatrix.MatrixName.MatrixName", FocusedMatrix);
                ////        ListView CreateListView = Application.CreateListView("SampleLogIn_LookupListView_Copy_SampleLogin_Copy_CopyTest", cs, false);
                ////        e.Size = new Size(750, 500);
                ////        e.View = CreateListView;
                ////        View.CurrentObject = CreateListView;
                ////    }
                ////}
                if (View != null && View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" && View.SelectedObjects.Count == 1)
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn objtp in View.SelectedObjects)
                    {
                        Modules.BusinessObjects.SampleManagement.SampleLogIn objSample = objspace.GetObject(objtp);
                        if (objSample != null)
                        {
                            foreach (Testparameter testparameter in objSample.Testparameters)
                            {
                                TPOid.Add(testparameter.Oid);
                                FocusedMatrix.Add(testparameter.TestMethod.MatrixName.MatrixName.ToString());
                            }
                            objSLInfo.SLOid = objtp.Oid.ToString();
                        }
                    }
                    object objToShow = objspace.CreateObject(typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn));
                    if (objToShow != null)
                    {
                        ////ListView CreateListView = Application.CreateListView("SampleLogIn_LookupListView_Copy_SampleLogin_Copy_CopyTest", cs, false);
                        ////e.Size = new Size(750, 500);
                        ////e.View = CreateListView;
                        ////View.CurrentObject = CreateListView;
                        CollectionSource cs = new CollectionSource(objspace, typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn));
                        cs.Criteria.Clear();
                        if (View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" && !string.IsNullOrEmpty(objSLInfo.SLOid))
                        {
                            cs.Criteria["filter1"] = CriteriaOperator.Parse("[JobID.JobID]='" + objSLInfo.focusedJobID + "' and Oid <> ?", new Guid(objSLInfo.SLOid));
                        }
                        else
                        {
                            cs.Criteria["filter1"] = CriteriaOperator.Parse("[JobID.JobID]='" + objSLInfo.focusedJobID + "'");
                        }
                        cs.Criteria["filter2"] = new InOperator("VisualMatrix.MatrixName.MatrixName", FocusedMatrix);
                        ListView dvbottleAllocation = Application.CreateListView("SampleLogIn_LookupListView_Copy_SampleLogin_Copy_CopyTest", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(dvbottleAllocation);
                        showViewParameters.CreatedView = dvbottleAllocation;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += CopyTest_Accepting;
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

        private void CopyTest_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count>0)
                {
                if (e.AcceptActionArgs.SelectedObjects.Cast<Modules.BusinessObjects.SampleManagement.SampleLogIn>().Where(a => a.JobID.IsSampling && a.StationLocation == null).Any())
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage("Assign the station location to the samples", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }
                //ObjectSpace.CommitChanges();
                IObjectSpace os = Application.CreateObjectSpace();
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
                IList<Testparameter> objtp = null;
                Samplecheckin objJobId = null;
                if (TPOid.Count > 0)
                {
                    //objtp = os.GetObjects<Testparameter>(new InOperator("Oid", TPOid));
                    XPClassInfo TestParameterinfo;
                    TestParameterinfo = uow.GetClassInfo(typeof(Testparameter));
                    objtp = uow.GetObjects(TestParameterinfo, new InOperator("Oid", TPOid), null, int.MaxValue, false, true).Cast<Testparameter>().ToList();
                    CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + objSLInfo.SLOid + "'");
                    objParentSL = uow.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(criteria);
                }
                List<Guid> lstSampleOid = new List<Guid>();
                foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn obj in e.AcceptActionArgs.SelectedObjects)
                {
                    if (objtp != null && obj.Testparameters != null)
                    {

                        lstSampleOid.Add(obj.Oid);
                        if (objJobId == null)
                        {
                            objJobId = obj.JobID;
                        }
                        CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + obj.Oid + "'");
                        Modules.BusinessObjects.SampleManagement.SampleLogIn objSL = uow.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(criteria);
                        IList<SampleParameter> objsp = (IList<SampleParameter>)objParentSL.SampleParameter;
                        if (objsp != null)
                        {
                            if (objSL.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                if (objSL.JobID.IsSampling)
                                {
                                        //IList<SampleParameter> parameters = uow.GetObjects(uow.GetClassInfo(typeof(SampleParameter)), CriteriaOperator.Parse("[Samplelogin.Oid]=?", objParentSL.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                                        //if (parameters.Where(a => a.Testparameter != null && a.Testparameter.TestMethod != null && a.Testparameter.TestMethod.IsFieldTest == true).Count() > 0)
                                        //{
                                        Frame.GetController<FlutterAppViewController>().insertsample(uow, objSL);
                                        //}
                                }
                            }
                                IList<SampleParameter> lsts = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Samplelogin.Oid=? ", objSL.Oid));
                                foreach (SampleParameter item in lsts)
                                {
                                    var parametersToRemove1 = objSL.Testparameters.Where(objTestSL => !objtp.Contains(objTestSL) && (!string.IsNullOrEmpty(item.uqQCBatchID) || item.PrepMethodCount > 0 || !string.IsNullOrEmpty(item.Result))).ToList();
                                    foreach (var objtestparameter in parametersToRemove1)
                                    {
                                        SampleParameter objsampleparameter = ObjectSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("Testparameter.Oid=? ", objtestparameter.Oid));
                                        if (objsampleparameter != null)
                                        {
                                            if (!string.IsNullOrEmpty(objsampleparameter.uqQCBatchID))
                                            {
                                                Application.ShowViewStrategy.ShowMessage("In this " + objSL.SampleID + " a test with a created QCbatchID was found, so the CopyToTest function did not copy it. ", InformationType.Info, timer.Seconds, InformationPosition.Top);
                                                return;
                                            }
                                            else if (objsampleparameter.PrepMethodCount > 0)
                                            {
                                                Application.ShowViewStrategy.ShowMessage("In this " + objSL.SampleID + " a test with a created PrepBatchID was found, so the CopyToTest function did not copy it. ", InformationType.Info, timer.Seconds, InformationPosition.Top);
                                                return;
                                            }
                                            else if (!string.IsNullOrEmpty(objsampleparameter.Result))
                                            {
                                                Application.ShowViewStrategy.ShowMessage("In this " + objSL.SampleID + " a test with a created result  was found, so the CopyToTest function did not copy it. ", InformationType.Info, timer.Seconds, InformationPosition.Top);
                                                return;
                                            }
                                        }
                                    }

                                    var parametersToRemove = objSL.Testparameters.Where(objTestSL => !objtp.Contains(objTestSL) && string.IsNullOrEmpty(item.uqQCBatchID) && string.IsNullOrEmpty(item.PrepBatchID) && string.IsNullOrEmpty(item.Result)).ToList();
                                    foreach (var parameterToRemove in parametersToRemove)
                                    {
                                        objSL.Testparameters.Remove(parameterToRemove);
                                    }
                                }
                                foreach (Testparameter objtestperam in objtp)
                            {
                                if (!objSL.Testparameters.Contains(objtestperam))
                                {

                                    foreach (SampleParameter sp in objsp)
                                    {

                                        if (sp != null)
                                        {
                                            if (objtestperam.Oid == sp.Testparameter.Oid)
                                            {
                                                objSL.Testparameters.Add(objtestperam);
                                                if (sp.IsGroup == true && sp.GroupTest != null)
                                                   {
                                                     SampleParameter sample = objSL.SampleParameter.FirstOrDefault<SampleParameter>(i => i.Testparameter.Oid == sp.Testparameter.Oid);
                                                      if (sample != null)
                                                        {
                                                            sample.IsGroup = true;
                                                            sample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(sp.GroupTest.Oid);
                                                        }
                                                   }
                                            }

                                        }
                                    }
                                }
                                //if (objSL.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                                //{
                                //    Frame.GetController<AuditlogViewController>().insertauditdata(uow, objSL.JobID.Oid, OperationType.Created, "Sample Registration", objSL.SampleID, "Test", "", objtestperam.TestMethod.TestName + " | " + objtestperam.Parameter.ParameterName, "");
                                //}
                            }
                            foreach (Testparameter objtestperam in objtp)
                            {
                                if (!objSL.Testparameters.Contains(objtestperam))
                                {
                                    foreach (SampleParameter sp in objsp)
                                    {
                                        if (sp != null)
                                        {
                                            if (sp.Testparameter != null && objtestperam.Oid == sp.Testparameter.Oid)
                                            {
                                                objSL.Testparameters.Add(objtestperam);

                                            }
                                            if (sp.TestHold == true)
                                            {

                                                objSL.Testparameters.Where(i => i.SampleParameter.Count > 0).SelectMany(i => i.SampleParameter).ToList().ForEach(i => i.TestHold = true);

                                            }
                                                if (sp.IsGroup == true && sp.GroupTest != null)
                                                {
                                                    SampleParameter sample = objSL.SampleParameter.FirstOrDefault<SampleParameter>(i => i.Testparameter.Oid == sp.Testparameter.Oid);
                                                    if (sample != null)
                                                    {
                                                        sample.IsGroup = true;
                                                        sample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(sp.GroupTest.Oid);
                                                    }
                                                }
                                        }
                                    }
                                }
                                if (objSL.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    Frame.GetController<AuditlogViewController>().insertauditdata(uow, objSL.JobID.Oid, OperationType.Created, "Sample Registration", objSL.SampleID, "Test", "", objtestperam.TestMethod.TestName + " | " + objtestperam.Parameter.ParameterName, "");
                                }
                            }
                            objSL.Save();
                            uow.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "copysuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
 							if (e.AcceptActionArgs.SelectedObjects.Count == 1)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\EditTestMessageGroup", "CopiedTestsAppliedSuccessfully"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                if (e.AcceptActionArgs.SelectedObjects.Count > 1)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\EditTestMessageGroup", "CopiedTestsAppliedSuccessfully>1"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                        }
                        //Frame.GetController<SampleRegistration.SampleRegistrationViewController>().AssignBottlesToSamples(uow, obj.JobID.JobID, objSL.Oid);
                        Frame.GetController<SampleRegistration.SampleRegistrationViewController>().AssignBottleAllocationToSamples(uow, objSL.Oid);
                        Modules.BusinessObjects.SampleManagement.SampleLogIn objSample = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objSL.Oid);
                        objSample.TestSummary = string.Join("; ", new XPQuery<SampleParameter>(uow).Where(i => i.Samplelogin.Oid == objSample.Oid && i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.TestName).Distinct().ToList());
                        objSample.FieldTestSummary = string.Join(", ", new XPQuery<SampleParameter>(uow).Where(i => i.Samplelogin.Oid == objSample.Oid && i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsFieldTest == true).Select(i => i.Testparameter.TestMethod.TestName).Distinct().ToList());
                        IList<SampleParameter> childsampleparemter = uow.GetObjects(uow.GetClassInfo(typeof(SampleParameter)), CriteriaOperator.Parse("[Samplelogin]=?", objSample.Oid), null, int.MaxValue, false, true).Cast<SampleParameter>().ToList();

                        foreach (SampleParameter Parentsp in objsp)
                        {
                            if (Parentsp != null)
                            {
                                foreach (SampleParameter CopiedSpPara in childsampleparemter)
                                {
                                    if (CopiedSpPara != null && Parentsp.Testparameter.Oid == CopiedSpPara.Testparameter.Oid)
                                    {
                                        if (Parentsp.TAT != null)
                                        {
                                            CopiedSpPara.TAT = uow.GetObjectByKey<TurnAroundTime>(Parentsp.TAT.Oid);
                                        }
                                        CopiedSpPara.SubOut = Parentsp.SubOut;
                                    }
                                }
                            }
                        }
                        foreach (SampleParameter Parentsp in objsp.Where(i => i.TestHold == true).ToList())
                        {
                            if (Parentsp != null)
                            {
                                foreach (SampleParameter CopiedSpPara in childsampleparemter)
                                {
                                    if (CopiedSpPara != null && Parentsp.Testparameter.Oid == CopiedSpPara.Testparameter.Oid)
                                    {
                                        CopiedSpPara.TestHold = Parentsp.TestHold;
                                        break;
                                    }
                                }
                            }
                        }
                        uow.CommitChanges();
                    }
                }
                if (TPOid.Count > 0)
                {
                    TPOid.Clear();
                }

                if (objJobId != null && lstSampleOid != null && lstSampleOid.Count > 0 && objJobId.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                {
                    if (objJobId != null && objJobId.ProjectCategory != null && (objJobId.ProjectCategory.CategoryName == "PT" || objJobId.ProjectCategory.CategoryName == "DOC" || objJobId.ProjectCategory.CategoryName == "MDL"))
                    {
                        PTStudyLog Objstudylog = uow.FindObject<PTStudyLog>(CriteriaOperator.Parse("[SampleCheckinJobID.JobID]= ?", objJobId.JobID));
                        if (Objstudylog != null)
                        {
                            foreach (Guid objSampleLogInNew in lstSampleOid.ToList())
                            {
                                XPClassInfo sampleParameterinfo;
                                sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                                IList<SampleParameter> lstSampleParam = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.Oid]=?", objSampleLogInNew), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                                foreach (SampleParameter objParam in lstSampleParam)
                                {
                                    if (uow.Query<PTStudyLogResults>().FirstOrDefault(i => i.SampleID != null && i.SampleID.Oid == objParam.Oid) == null)
                                    {
                                        PTStudyLogResults objPTRes = new PTStudyLogResults(uow);
                                        SampleParameter objParameter = uow.GetObjectByKey<SampleParameter>(objParam.Oid);
                                        objPTRes.PTStudyLog = Objstudylog;
                                        objPTRes.SampleID = objParameter;
                                        if (objParam.Samplelogin != null)
                                        {
                                            objPTRes.SampleLogin = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objParam.Samplelogin.Oid);
                                        }
                                        objPTRes.Save();
                                    }
                                }
                            }
                        }
                    }
                }
                //Application.MainWindow.GetController<RegistrationSignOffController>().PendingSigningOffJobIDCount();

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
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\EditTestMessageGroup", "SelectSampleToApplyCopiedTest"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SL_GroupTest_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                Guid TestGroupName;
                Guid SLOid = new Guid();
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                if (View != null)
                {
                    //foreach (GroupTest obj in e.PopupWindowViewSelectedObjects)
                    //{
                    //    if (obj != null)
                    //    {
                    //        TestGroupName = obj.Oid;
                    //        CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + objSLInfo.SLOid + "'");
                    //        Modules.BusinessObjects.SampleManagement.SampleLogIn objSL = ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(criteria);
                    //        CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]='" + TestGroupName + "'");
                    //        IList<GroupTest> objTP = ObjectSpace.GetObjects<GroupTest>(criteria1);
                    //        SLOid = objSL.Oid;
                    //        if (objTP != null)
                    //        {
                    //            foreach (GroupTest tp in objTP)
                    //            {
                    //                foreach (TestMethod testmethod in tp.TestMethods)
                    //                {
                    //                    if ((testmethod.RetireDate == DateTime.MinValue || testmethod.RetireDate > DateTime.Now) && (testmethod.MethodName.RetireDate == DateTime.MinValue || testmethod.MethodName.RetireDate > DateTime.Now))
                    //                    {
                    //                        foreach (Testparameter testparam in testmethod.TestParameter)
                    //                        {
                    //                            if (objSL != null && !objSL.Testparameters.Contains(testparam))
                    //                            {
                    //                                if (testparam.TestMethod.MatrixName.MatrixName == objSLInfo.SLVisualMatrixName)
                    //                                {
                    //                                    testparam.TestGroup = tp.TestGroupName;
                    //                                    objSL.Testparameters.Add(testparam);
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SL_GroupTest_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                //    IObjectSpace objspace = Application.CreateObjectSpace();
                //    object objToShow = objspace.CreateObject(typeof(GroupTest));
                //    if (objToShow != null)
                //    {
                //        CollectionSource cs = new CollectionSource(objspace, typeof(GroupTest));
                //        cs.Criteria.Clear();
                //        List<object> GroupTestName = new List<object>();
                //        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(GroupTestMethod)))
                //        {
                //            lstview.Criteria = CriteriaOperator.Parse("[TestMethods.MatrixName.MatrixName]='" + objSLInfo.SLVisualMatrixName + "'");
                //            lstview.Properties.Add(new ViewProperty("GroupTestName", SortDirection.Ascending, "GroupTests.TestGroupName", true, true));
                //            foreach (ViewRecord Vrec in lstview)
                //                GroupTestName.Add(Vrec["GroupTestName"]);
                //        }
                //        cs.Criteria["filter"] = new InOperator("TestGroupName", GroupTestName); ;
                //        ListView CreateListView = Application.CreateListView("GroupTest_ListView_Copy_Samplelogin", cs, false);
                //        e.Size = new Size(750, 500);
                //        e.View = CreateListView;
                //    }
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
