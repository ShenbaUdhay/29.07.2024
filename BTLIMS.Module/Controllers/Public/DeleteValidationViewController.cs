using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using LDM.Module.Controllers.SampleRegistration;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.PLM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Web.Controllers.Reporting
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class WebReportingDeleteObjectViewController : WebDeleteObjectsViewController
    {
        MessageTimer timer = new MessageTimer();
        ShowNavigationItemController ShowNavigationController;
        requisitionquerypanelinfo objreq = new requisitionquerypanelinfo();
        #region Constructor
        public WebReportingDeleteObjectViewController()
        {
            InitializeComponent();
        }
        #endregion

        #region DefaultEvents
        protected override void OnActivated()
        {
            base.OnActivated();
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

        #region Function
        protected override void Delete(SimpleActionExecuteEventArgs args)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(Labware))
                {
                    foreach (Labware item in View.SelectedObjects)
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        SpreadSheetEntry_AnalyticalBatch sampleAnalyticalBatch = objectSpace.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("Contains([Instrument], ?)", item.Oid.ToString()));
                        SamplePrepBatch samplePrepBatch = objectSpace.FindObject<SamplePrepBatch>(CriteriaOperator.Parse("Contains([Instrument], ?)", item.Oid.ToString()));
                        if (samplePrepBatch != null)
                        {
                            Exception ex = new Exception("Already used can't allow to delete");
                            throw ex;
                        }
                        else if (sampleAnalyticalBatch != null)
                        {
                            Exception ex = new Exception("Already used can't allow to delete");
                            throw ex;
                        }
                        else
                        {
                            base.Delete(args);
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                if (View != null && View.ObjectTypeInfo.Type == typeof(Modules.BusinessObjects.SampleManagement.Reporting))
                {
                    var objecttodelete = ObjectSpace.GetObjectsToDelete(true);
                    foreach (Modules.BusinessObjects.SampleManagement.Reporting obj in args.SelectedObjects)
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + obj.Oid + "'");
                        Modules.BusinessObjects.SampleManagement.Reporting obj1 = objSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(criteria);
                        if (obj1 != null)
                        {
                            if (obj1.ReportApprovedBy != null && obj1.ReportApprovedDate != DateTime.MinValue)
                            {
                                obj1.ReportApprovedBy = null;
                                DateTime? dt = null;
                                obj1.ReportApprovedDate = Convert.ToDateTime(dt);
                                objSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            else if (obj1.ReportValidatedBy != null && obj1.ReportValidatedDate != DateTime.MinValue)
                            {
                                obj1.ReportValidatedBy = null;
                                DateTime? dt = null;
                                obj1.ReportValidatedDate = Convert.ToDateTime(dt);
                                objSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                objSpace.Delete(obj1);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                objSpace.CommitChanges();
                            }
                        }
                    }
                    ObjectSpace.Refresh();
                }
                else if(View !=null && (View.Id == "Accrediation_ListView"))
                {
                    if(View.SelectedObjects.Count > 0)
                    {
                        //if (View.SelectedObjects.Count > 0)
                        //{
                        //    List<Accrediation> ls = View.SelectedObjects.Cast<Accrediation>().ToList();
                        //    var lstaccrediations = ls.Where(i => i.lAccrediation != null).Select(i => i.lAccrediation).ToList();
                        //    if (lstaccrediations.Count > 0)
                        //    {
                        //        CriteriaOperator criteria = new InOperator("lAccrediation", lstaccrediations);
                        //        Testparameter testparameter = View.ObjectSpace.FindObject<Testparameter>(criteria);
                        //        if (testparameter != null)
                        //        {
                        //            Exception ex = new Exception("Already used can't allow to delete");
                        //            throw ex;
                        //        }
                        //        else
                        //        {
                        //            ObjectSpace.Delete(ls);
                        //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        //            ObjectSpace.CommitChanges();
                        //        }
                        //    }
                        //}

                        List<Accrediation> ls = View.SelectedObjects.Cast<Accrediation>().ToList();
                        var lstaccrediations = ls.Where(i=>i.lAccrediation!=null).Select(i=>i.lAccrediation).ToList();
                        if(lstaccrediations.Count > 0)
                        {
                            CriteriaOperator criteria = CriteriaOperator.Parse("IsNullOrEmpty([GCRecord])");
                            IList<Testparameter> testparameter = View.ObjectSpace.GetObjects<Testparameter>(criteria);
                            // List<string> findaccrediation = testparameter.Where(i => i.lAccrediation != null).Select(i => i.lAccrediation).ToList();
                           var findaccrediation = testparameter.Where(i => i.lAccrediation != null).SelectMany(i => i.lAccrediation.Split(';')).ToList();
                            foreach(string laccrdiation in lstaccrediations)
                            {
                                // var accresult = findaccrediation.Where(i => i.Contains(laccrdiation)).Distinct().ToList();
                                var accresult = findaccrediation.Any(i => i.Split(';').Any(a => a.Trim().Equals(laccrdiation.Trim())));
                                if (accresult)
                            {
                                Exception ex = new Exception("Already used can't allow to delete");
                                throw ex;
                            }
                            else
                            {
                                ObjectSpace.Delete(ls);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                ObjectSpace.CommitChanges();
                            }
                        }
                            
                            
                        }
                    }
                }
                else if (View != null && (View.Id == "Accrediation_DetailView"))
                {
                    //Accrediation laccrediation = View.CurrentObject as Accrediation;
                    //if(laccrediation != null && laccrediation.lAccrediation != null)
                    //{
                    //    string acr = laccrediation.lAccrediation;
                    //    //CriteriaOperator criteria = new InOperator("lAccrediation", laccrediation.lAccrediation);

                    //    Testparameter testparameter = View.ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[lAccrediation] Like ?", laccrediation.lAccrediation));
                    //    if (testparameter != null)
                    //    {
                    //        Exception ex = new Exception("Already used can't allow to delete");
                    //        throw ex;
                    //    }
                    //    else
                    //    {
                    //        ObjectSpace.Delete(laccrediation);
                    //        base.Delete(args);
                    //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    //        ObjectSpace.CommitChanges();

                    //    }

                    //}
                    Accrediation laccrediation = View.CurrentObject as Accrediation;
                    if(laccrediation != null && laccrediation.lAccrediation != null)
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("IsNullOrEmpty([GCRecord])");
                        IList<Testparameter> testparameter = View.ObjectSpace.GetObjects<Testparameter>(criteria);
                        // List<string> findaccrediation = testparameter.Where(i => i.lAccrediation != null).Select(i => i.lAccrediation).ToList();
                        var findaccrediation = testparameter.Where(i => i.lAccrediation != null).SelectMany(i => i.lAccrediation.Split(';')).ToList();
                        var accresult = findaccrediation.Where(i => i.Contains(laccrediation.lAccrediation)).ToList();
                        if(accresult.Count > 0)
                        {
                            Exception ex = new Exception("Already used can't allow to delete");
                            throw ex;
                        }
                        else
                        {
                            ObjectSpace.Delete(laccrediation);
                            ObjectSpace.CommitChanges();
                            base.Delete(args);
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                        }
                    }
                   
                }
                else if (View != null && (View.Id == "SampleLogIn_ListView" || View.Id == "SampleLogIn_ListView_Copy_SampleRegistration"))
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        //bool IsDeleted = false;
                        //View.RefreshDataSource();
                        List<SampleLogIn> lstSampleLogin = View.SelectedObjects.Cast<SampleLogIn>().ToList();
                        IObjectSpace os = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)os).Session;
                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                        foreach (SampleLogIn objSampleLogin in lstSampleLogin)
                        {
                            SampleLogIn obj = uow.GetObjectByKey<SampleLogIn>(objSampleLogin.Oid);
                            if (obj.Testparameters.Count > 0)
                            {
                                int qcCount = View.ObjectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.QC.QCBatchSequence), CriteriaOperator.Parse("[SampleID.Oid] = ?", obj.Oid));
                                int sdmsCount = View.ObjectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.SpreadSheetEntry), CriteriaOperator.Parse("[uqSampleParameterID] Is Not Null And [uqSampleParameterID.Samplelogin] Is Not Null And [uqSampleParameterID.Samplelogin.Oid] = ?", obj.Oid));
                                if (qcCount == 0 && sdmsCount == 0)
                                {
                                    bool IsDeleted = false;
                                    List<SampleParameter> lstSampleParameters = obj.SampleParameter.Cast<SampleParameter>().ToList();
                                    foreach (SampleParameter objSampleParam in lstSampleParameters)
                                    {
                                        SampleParameter sampleParam = uow.GetObjectByKey<SampleParameter>(objSampleParam.Oid);
                                        if (sampleParam.AnalyzedDate != null && sampleParam.AnalyzedDate != DateTime.MinValue)
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletetest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        }
                                        else
                                        {
                                            CriteriaOperator criteriaSample = CriteriaOperator.Parse("[uqSampleParameterID]='" + sampleParam.Oid + "'");
                                            SpreadSheetEntry objSample = uow.FindObject<SpreadSheetEntry>(criteriaSample);
                                            if (sampleParam.QCBatchID != null)
                                            {
                                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteqctest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                            }
                                            else if (objSample != null)
                                            {
                                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletesdmstest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                            }
                                            else
                                            {
                                                Frame.GetController<SampleRegistrationViewController>().DeletePTStudyLogTest(uow, sampleParam);
                                                uow.Delete(uow.GetObjectByKey<SampleParameter>(sampleParam.Oid));
                                                IsDeleted = true;
                                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                            }

                                        }
                                    }
                                    if (IsDeleted)
                                    {
                                        uow.CommitChanges();
                                    }
                                    int spCount = View.ObjectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.SampleParameter), CriteriaOperator.Parse("[Samplelogin.Oid] = ?", obj.Oid));
                                    if (spCount == 0)
                                    {
                                        XPClassInfo BottleSetupinfo;
                                        ////BottleSetupinfo = uow.GetClassInfo(typeof(BottleSetup));
                                        ////IList<BottleSetup> lstBottles = uow.GetObjects(BottleSetupinfo, CriteriaOperator.Parse("Contains([SampleContainer], ?)", obj.SampleID), new SortingCollection(), 0, 0, false, true).Cast<BottleSetup>().ToList();
                                        ////if (lstBottles != null && lstBottles.Count > 0)
                                        ////{
                                        ////    foreach (BottleSetup objBottleSetup in lstBottles.ToList())
                                        ////    {
                                        ////        BottleSetup objBottle = uow.GetObjectByKey<BottleSetup>(objBottleSetup.Oid);
                                        ////        List<string> lstContainers = objBottle.SampleContainer.Split(',').Where(i => i.Trim().StartsWith(obj.SampleID)).ToList();
                                        ////        if (lstContainers != null && lstContainers.Count > 0)
                                        ////        {
                                        ////            foreach (string strContainer in lstContainers)
                                        ////            {
                                        ////                objBottle.SampleContainer = objBottle.SampleContainer.Replace(strContainer, string.Empty);
                                        ////            }

                                        ////            if (objBottle.SampleContainer.StartsWith(", "))
                                        ////            {
                                        ////                objBottle.SampleContainer = objBottle.SampleContainer.Substring(2);
                                        ////            }
                                        ////            else
                                        ////            if (objBottle.SampleContainer.StartsWith(","))
                                        ////            {
                                        ////                objBottle.SampleContainer = objBottle.SampleContainer.Substring(1);
                                        ////            }
                                        ////            if (objBottle.SampleContainer.EndsWith(","))
                                        ////            {
                                        ////                objBottle.SampleContainer = objBottle.SampleContainer.Substring(0, objBottle.SampleContainer.Length - 1);
                                        ////            }
                                        ////        }
                                        ////        if (string.IsNullOrEmpty(objBottle.SampleContainer))
                                        ////        {
                                        ////            uow.Delete(objBottle);
                                        ////        }
                                        ////    }
                                        ////}
                                        XPClassInfo SampleBottleAllocationinfo;
                                        SampleBottleAllocationinfo = uow.GetClassInfo(typeof(SampleBottleAllocation));
                                        IList<SampleBottleAllocation> lstbottleAllocation = uow.GetObjects(SampleBottleAllocationinfo, CriteriaOperator.Parse("SampleRegistration=?", obj.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleBottleAllocation>().ToList();
                                        if (lstbottleAllocation.Count > 0)
                                        {
                                            foreach (SampleBottleAllocation objSamplebottleAll in lstbottleAllocation.ToList())
                                            {
                                                SampleBottleAllocation objbottleAll = uow.GetObjectByKey<SampleBottleAllocation>(objSamplebottleAll.Oid);
                                                uow.Delete(objbottleAll);
                                            }
                                        }
                                        if (obj.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                                        {
                                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, obj.JobID.Oid, OperationType.Deleted, "Sample Registration", obj.JobID.JobID, "Samples", obj.SampleID, "", "");
                                        }
                                        uow.Delete(obj);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletesample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletesample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else
                            {
                                //base.Delete(args);
                                if (obj.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    Frame.GetController<AuditlogViewController>().insertauditdata(uow, obj.JobID.Oid, OperationType.Deleted, "Sample Registration", obj.JobID.JobID, "Samples", obj.SampleID, "", "");
                                }
                                uow.Delete(obj);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        uow.CommitChanges();
                        if (Application.MainWindow.View is DashboardView)
                        {
                            DashboardViewItem dvSampleCheckin = ((DashboardView)Application.MainWindow.View).FindItem("SampleCheckin") as DashboardViewItem;
                            if (dvSampleCheckin != null && dvSampleCheckin.InnerView != null)
                            {
                                Samplecheckin objCurrent = (Samplecheckin)dvSampleCheckin.InnerView.CurrentObject;
                                if (objCurrent != null)
                                {
                                    List<SampleLogIn> lstSamples = uow.Query<SampleLogIn>().Where(i => i.JobID != null && i.JobID.Oid == objCurrent.Oid).ToList();
                                    if (lstSamples.Count == 0)
                                    {
                                        Samplecheckin objCurre = uow.GetObjectByKey<Samplecheckin>(objCurrent.Oid);
                                        if (objCurre != null)
                                        {
                                            objCurre.Status = SampleRegistrationSignoffStatus.PendingSubmit;
                                            StatusDefinition objStatus = uow.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] =9"));
                                            if (objStatus != null)
                                            {
                                                objCurre.Index = objStatus;
                                            }
                                            uow.CommitChanges();
                                        }
                                    }
                                }
                            }
                        }
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            if (nestedFrame != null)
                            {
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
                        }
                        Frame.GetController<SampleRegistrationViewController>().UpdateStatusInJobID();
                        Application.MainWindow.View.ObjectSpace.Refresh();
                    }
                    //Application.MainWindow.GetController<SampleRegistrationViewController>().GetDispositionNavigationItemCount();
                }
                else if (View != null && View.Id == "COCSettings_ListView")
                {
                    if (View.Id == "COCSettings_ListView" && View.SelectedObjects.Count > 0)
                    {
                        foreach (COCSettings objsampling in View.SelectedObjects)
                        {
                            IList<COCSettingsSamples> lstCOCSettingsSamples = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COCID.Oid] = ?", objsampling.Oid));
                            foreach (COCSettingsSamples lstSample in lstCOCSettingsSamples.ToList())
                            {
                                IList<COCSettingsBottleAllocation> lstsmplbtlalloc = ObjectSpace.GetObjects<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid] = ?", lstSample.Oid));
                                if (lstsmplbtlalloc != null && lstsmplbtlalloc.Count > 0)
                                {
                                    ObjectSpace.Delete(lstsmplbtlalloc);
                                    ObjectSpace.CommitChanges();
                                }
                                IList<COCSettingsTest> lstsamplingTest = ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[COCSettingsSamples.Oid] = ?", lstSample.Oid));
                                if (lstsamplingTest != null && lstsamplingTest.Count > 0)
                                {
                                    ObjectSpace.Delete(lstsamplingTest);
                                    ObjectSpace.CommitChanges();
                                }
                                ObjectSpace.Delete(lstSample);
                                ObjectSpace.CommitChanges();
                            }
                            ObjectSpace.Delete(objsampling);
                            ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        ObjectSpace.Refresh();
                        View.Refresh();
                    }
                }
                else if (View != null && View.Id == "SampleParameter_ListView_Copy_SampleRegistration")
                {
                    List<Tuple<string, Guid, Guid>> lstDeletedSampleParameters = new List<Tuple<string, Guid, Guid>>();
                    bool IsDeleted = false;
                    var os = Application.CreateObjectSpace();
                    foreach (SampleParameter obj in View.SelectedObjects)
                    {
                        if (obj.AnalyzedDate != null && obj.AnalyzedDate != DateTime.MinValue)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletetest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            CriteriaOperator criteriaSample = CriteriaOperator.Parse("[uqSampleParameterID]='" + obj.Oid + "'");
                            SpreadSheetEntry objSample = ObjectSpace.FindObject<SpreadSheetEntry>(criteriaSample);
                            if (objSample != null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletesdmstest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else if (obj.QCBatchID != null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteqctest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else if (obj.SignOff)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "testcannotremove"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                //base.Delete(args);
                                if (obj != null && obj.Samplelogin != null && obj.Samplelogin.JobID != null && obj.Samplelogin.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit &&
                               obj.Samplelogin.JobID.ProjectCategory != null && (obj.Samplelogin.JobID.ProjectCategory.CategoryName == "PT" || obj.Samplelogin.JobID.ProjectCategory.CategoryName == "DOC" || obj.Samplelogin.JobID.ProjectCategory.CategoryName == "MDL"))
                                {
                                    PTStudyLogResults objOldPTStudyTest = os.FindObject<PTStudyLogResults>(CriteriaOperator.Parse("[SampleID] = ? And [SampleLogin] = ?", obj.Oid, obj.Samplelogin));
                                    if (objOldPTStudyTest != null)
                                    {
                                        os.Delete(objOldPTStudyTest);
                                    }
                                }
                                os.Delete(os.GetObject<SampleParameter>(obj));
                                if (lstDeletedSampleParameters.FirstOrDefault(i => i.Item2 == obj.Samplelogin.Oid && i.Item3 == obj.Testparameter.TestMethod.Oid) == null)
                                {
                                    Tuple<string, Guid, Guid> tupDeletedSampleTest = new Tuple<string, Guid, Guid>(obj.Samplelogin.SampleID, obj.Samplelogin.Oid, obj.Testparameter.TestMethod.Oid);
                                    lstDeletedSampleParameters.Add(tupDeletedSampleTest);
                                }
                                IsDeleted = true;
                            }
                        }
                    }
                    IList<SampleParameter> distinctSample = ((ListView)View).SelectedObjects.Cast<SampleParameter>().ToList().GroupBy(p => new { p.Testparameter.TestMethod, p.Samplelogin }).Select(g => g.First()).ToList();
                    foreach (SampleParameter objs in distinctSample)
                    {
                        SampleBottleAllocation objAllocation = os.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? and [TestMethod.Oid]=?", objs.Samplelogin.Oid, objs.Testparameter.TestMethod.Oid));
                        if (objAllocation != null)
                        {
                            os.Delete(objAllocation);
                            IsDeleted = true;
                        }

                    }
                    if (IsDeleted == true)
                    {
                        os.CommitChanges();

                        foreach (Tuple<string, Guid, Guid> tupDeletedSampleTest in lstDeletedSampleParameters)
                        {
                            IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid] = ? And [Testparameter.TestMethod.Oid] = ?", tupDeletedSampleTest.Item2, tupDeletedSampleTest.Item3));
                            if (lstSamples != null && lstSamples.Count == 0)
                            {
                                ////IList<BottleSetup> lstBottles = os.GetObjects<BottleSetup>(CriteriaOperator.Parse("Contains([SampleContainer], ?) and [Test][[Oid] = ?]", tupDeletedSampleTest.Item1, tupDeletedSampleTest.Item3));
                                ////if (lstBottles != null && lstBottles.Count > 0)
                                ////{
                                ////    foreach (BottleSetup objBottle in lstBottles.ToList())
                                ////    {
                                ////        List<string> lstContainers = objBottle.SampleContainer.Split(',').Where(i => i.Trim().StartsWith(tupDeletedSampleTest.Item1)).ToList();
                                ////        if (lstContainers != null && lstContainers.Count > 0)
                                ////        {
                                ////            List<string> lstSampleID = objBottle.SampleContainer.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Split('(')[0]).Distinct().ToList();
                                ////            if (lstSampleID != null && lstSampleID.Count > 0)
                                ////            {
                                ////                foreach (string strSampleID in lstSampleID)
                                ////                {
                                ////                    bool CanRemoveContainer = true;
                                ////                    foreach (TestMethod objTest in objBottle.Test.ToList())
                                ////                    {
                                ////                        //SampleParameter objSample = os.FirstOrDefault<SampleParameter>(i => i.Samplelogin.SampleID == strSampleID && i.Testparameter.TestMethod.Oid == objTest.Oid);
                                ////                        SampleParameter objSample = os.FindObject<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID] = ? and [Testparameter.TestMethod.Oid] = ? and [Samplelogin.GCRecord] is null", strSampleID, objTest.Oid));
                                ////                        if (objSample != null)
                                ////                        {
                                ////                            CanRemoveContainer = false;
                                ////                            //break;
                                ////                        }
                                ////                        SampleParameter objSampleParam = os.FindObject<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? and [Bottle.Oid] = ?", objTest.Oid, objBottle.Oid));
                                ////                        if (objSampleParam == null)
                                ////                        {
                                ////                            objBottle.Test.Remove(objTest);
                                ////                            if (objBottle.TestGroup.Contains(objTest.TestName + ","))
                                ////                            {
                                ////                                objBottle.TestGroup = objBottle.TestGroup.Replace(objTest.TestName + ",", string.Empty);
                                ////                            }
                                ////                            else
                                ////                            if (objBottle.TestGroup.Contains("," + objTest.TestName))
                                ////                            {
                                ////                                objBottle.TestGroup = objBottle.TestGroup.Replace("," + objTest.TestName, string.Empty);
                                ////                            }
                                ////                            else
                                ////                            {
                                ////                                objBottle.TestGroup = objBottle.TestGroup.Replace(objTest.TestName, string.Empty);
                                ////                            }
                                ////                            if (objBottle.TestGroup.Contains(",,"))
                                ////                            {
                                ////                                objBottle.TestGroup = objBottle.TestGroup.Replace(",,", ",");
                                ////                            }
                                ////                        }
                                ////                    }
                                ////                    if (CanRemoveContainer)
                                ////                    {
                                ////                        foreach (string strContainer in lstContainers.Where(i => i.StartsWith(strSampleID)))
                                ////                        {
                                ////                            objBottle.SampleContainer = objBottle.SampleContainer.Replace(strContainer, string.Empty);
                                ////                        }
                                ////                    }
                                ////                }
                                ////            }

                                ////            if (objBottle.SampleContainer.StartsWith(", "))
                                ////            {
                                ////                objBottle.SampleContainer = objBottle.SampleContainer.Substring(2);
                                ////            }
                                ////            else
                                ////            if (objBottle.SampleContainer.StartsWith(","))
                                ////            {
                                ////                objBottle.SampleContainer = objBottle.SampleContainer.Substring(1);
                                ////            }
                                ////            if (objBottle.SampleContainer.EndsWith(","))
                                ////            {
                                ////                objBottle.SampleContainer = objBottle.SampleContainer.Substring(0, objBottle.SampleContainer.Length - 1);
                                ////            }
                                ////        }
                                ////        if (string.IsNullOrEmpty(objBottle.SampleContainer) || objBottle.Test.Count > 0)
                                ////        {
                                ////            os.Delete(objBottle);
                                ////        }
                                ////        os.CommitChanges();
                                ////    }
                                ////}
                            }
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            if (nestedFrame != null)
                            {
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
                        }

                    }

                }
                else if (View != null && View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration")
                {
                    List<Tuple<string, Guid, Guid>> lstDeletedSampleParameters = new List<Tuple<string, Guid, Guid>>();
                    bool IsDeleted = false;
                    var os = Application.CreateObjectSpace();
                    foreach (COCSettingsTest obj in View.SelectedObjects)
                    {
                        if (obj != null)
                        {
                            //base.Delete(args);
                            os.Delete(os.GetObject<COCSettingsTest>(obj));
                            if (lstDeletedSampleParameters.FirstOrDefault(i => i.Item2 == obj.COCSettingsSamples.Oid && i.Item3 == obj.Testparameter.TestMethod.Oid) == null)
                            {
                                Tuple<string, Guid, Guid> tupDeletedSampleTest = new Tuple<string, Guid, Guid>(obj.COCSettingsSamples.SampleID, obj.COCSettingsSamples.Oid, obj.Testparameter.TestMethod.Oid);
                                lstDeletedSampleParameters.Add(tupDeletedSampleTest);
                            }
                            IsDeleted = true;

                        }
                    }
                    IList<COCSettingsTest> distinctSample = ((ListView)View).SelectedObjects.Cast<COCSettingsTest>().ToList().GroupBy(p => new { p.Testparameter.TestMethod, p.COCSettingsSamples }).Select(g => g.First()).ToList();
                    foreach (COCSettingsTest objs in distinctSample)
                    {
                        COCSettingsBottleAllocation objAllocation = os.FindObject<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid]=? and [TestMethod.Oid]=?", objs.COCSettingsSamples.Oid, objs.Testparameter.TestMethod.Oid));
                        if (objAllocation != null)
                        {
                            os.Delete(objAllocation);
                            IsDeleted = true;
                        }

                    }
                    if (IsDeleted == true)
                    {
                        os.CommitChanges();

                        //foreach (Tuple<string, Guid, Guid> tupDeletedSampleTest in lstDeletedSampleParameters)
                        //{
                        //    IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid] = ? And [Testparameter.TestMethod.Oid] = ?", tupDeletedSampleTest.Item2, tupDeletedSampleTest.Item3));
                        //    if (lstSamples != null && lstSamples.Count == 0)
                        //    {

                        //    }
                        //}
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            if (nestedFrame != null)
                            {
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
                        }

                    }

                }
                else if (View != null && (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration"))
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        List<COCSettingsSamples> lstcocSampleLogin = View.SelectedObjects.Cast<COCSettingsSamples>().ToList();
                        IObjectSpace os = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)os).Session;
                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                        foreach (COCSettingsSamples objcocSampleLogin in lstcocSampleLogin)
                        {
                            COCSettingsSamples obj = uow.GetObjectByKey<COCSettingsSamples>(objcocSampleLogin.Oid);
                            if (obj.Testparameters.Count > 0)
                            {
                                bool IsDeleted = false;
                                List<COCSettingsTest> lstSampleParameters = obj.COCSettingsTests.Cast<COCSettingsTest>().ToList();
                                foreach (COCSettingsTest objSampleParam in lstSampleParameters)
                                {
                                    COCSettingsTest sampleParam = uow.GetObjectByKey<COCSettingsTest>(objSampleParam.Oid);
                                    uow.Delete(uow.GetObjectByKey<COCSettingsTest>(sampleParam.Oid));
                                    IsDeleted = true;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                if (IsDeleted)
                                {
                                    uow.CommitChanges();
                                }
                                int spCount = View.ObjectSpace.GetObjectsCount(typeof(COCSettingsTest), CriteriaOperator.Parse("[COCSettingsSamples.Oid] = ?", obj.Oid));
                                if (spCount == 0)
                                {
                                    XPClassInfo BottleSetupinfo;
                                    XPClassInfo SampleBottleAllocationinfo;
                                    SampleBottleAllocationinfo = uow.GetClassInfo(typeof(COCSettingsBottleAllocation));
                                    IList<COCSettingsBottleAllocation> lstbottleAllocation = uow.GetObjects(SampleBottleAllocationinfo, CriteriaOperator.Parse("COCSettingsRegistration=?", obj.Oid), new SortingCollection(), 0, 0, false, true).Cast<COCSettingsBottleAllocation>().ToList();
                                    if (lstbottleAllocation.Count > 0)
                                    {
                                        foreach (COCSettingsBottleAllocation objSamplebottleAll in lstbottleAllocation.ToList())
                                        {
                                            COCSettingsBottleAllocation objbottleAll = uow.GetObjectByKey<COCSettingsBottleAllocation>(objSamplebottleAll.Oid);
                                            uow.Delete(objbottleAll);
                                        }
                                    }
                                    uow.Delete(obj);
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletesample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else
                            {
                                //base.Delete(args);
                                uow.Delete(obj);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        uow.CommitChanges();
                        if (Application.MainWindow.View is DashboardView)
                        {
                            DashboardViewItem dvSampleCheckin = ((DashboardView)Application.MainWindow.View).FindItem("COCSettings") as DashboardViewItem;
                            if (dvSampleCheckin != null && dvSampleCheckin.InnerView != null)
                            {
                                COCSettings objCurrent = (COCSettings)dvSampleCheckin.InnerView.CurrentObject;
                                if (objCurrent != null)
                                {
                                    List<COCSettingsSamples> lstSamples = uow.Query<COCSettingsSamples>().Where(i => i.COCID != null && i.COCID.Oid == objCurrent.Oid).ToList();
                                    if (lstSamples.Count == 0)
                                    {
                                        COCSettings objCurre = uow.GetObjectByKey<COCSettings>(objCurrent.Oid);
                                        if (objCurre != null)
                                        {
                                            uow.CommitChanges();
                                        }
                                    }
                                }
                            }
                        }
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            if (nestedFrame != null)
                            {
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
                        }
                        //Frame.GetController<SampleRegistrationViewController>().UpdateStatusInJobID();
                        Application.MainWindow.View.ObjectSpace.Refresh();
                    }
                    //Application.MainWindow.GetController<SampleRegistrationViewController>().GetDispositionNavigationItemCount();
                }
                else if (View != null && View.Id == "SampleLogIn_DetailView")
                {
                    if (View.CurrentObject != null)
                    {
                        SampleLogIn obj = (SampleLogIn)View.CurrentObject;
                        if (obj.Testparameters.Count > 0)
                        {
                            Exception ex = new Exception(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletesample"));
                            throw ex;
                        }
                        else
                        {
                            base.Delete(args);
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else if (View != null && (View.Id == "Samplecheckin_ListView" || View.Id == "Samplecheckin_ListView_Copy_Registration" || View.Id == "Samplecheckin_ListView_Copy_Registration_History"))
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        Session currentsession = ((XPObjectSpace)ObjectSpace).Session;
                        foreach (Samplecheckin obj in View.SelectedObjects)
                        {
                            if (obj.JobID != null && obj.JobID.Length > 0)
                            {
                                //IndoorInspection objIndoorInspection = ObjectSpace.FindObject<IndoorInspection>(CriteriaOperator.Parse("[JobID.JobID] = ?",obj.JobID));
                                //if (objIndoorInspection == null)
                                //{
                                //    string output = string.Empty;
                                //    SelectedData sproc = currentsession.ExecuteSproc("CheckSamples", new OperandValue(obj.JobID));
                                //    if (sproc != null)
                                //    {
                                //        output = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                //        if (output == "True")
                                //        {
                                //            Exception ex = new Exception(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletejobid"));
                                //            throw ex;
                                //        }
                                //        else
                                //        {
                                //            base.Delete(args);
                                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        base.Delete(args);
                                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                //    }
                                //    //break; 
                                //}
                                //else
                                //{
                                //    Exception ex = new Exception(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletejobid_Copy"));
                                //    throw ex;
                                //}

                                string output = string.Empty;
                                SelectedData sproc = currentsession.ExecuteSproc("CheckSamples", new OperandValue(obj.JobID));
                                if (sproc != null)
                                {
                                    output = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                    if (output == "True")
                                    {
                                        Exception ex = new Exception(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletejobid"));
                                        throw ex;
                                    }
                                    else
                                    {
                                        if(obj.IsSampling)
                                        {
                                            obj.COCSource = null;
                                        }
                                        base.Delete(args);
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                                else
                                {
                                    if (obj.IsSampling)
                                    {
                                        obj.COCSource = null;
                                    }
                                    base.Delete(args);
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                            }
                        }

                    }

                }
                else if (View != null && (View.Id == "Samplecheckin_DetailView" || View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration" || View.Id == "Samplecheckin_DetailView_Copy_RegistrationSigningOff"))
                {
                    if (View.CurrentObject != null)
                    {
                        Session currentsession = ((XPObjectSpace)ObjectSpace).Session;
                        Samplecheckin obj = (Samplecheckin)View.CurrentObject;
                        if (obj.JobID != null && obj.JobID.Length > 0)
                        {
                            string output = string.Empty;
                            SelectedData sproc = currentsession.ExecuteSproc("CheckSamples", new OperandValue(obj.JobID));
                            if (sproc != null)
                            {
                                output = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                if (output == "True")
                                {
                                    Exception ex = new Exception(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletejobid"));
                                    throw ex;
                                }
                                else
                                {
                                    if (obj.IsSampling)
                                    {
                                        obj.COCSource = null;
                                    }
                                    base.Delete(args);
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else
                            {
                                if (obj.IsSampling)
                                {
                                    obj.COCSource = null;
                                }
                                base.Delete(args);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }

                    }
                }
                else if (View != null && View.Id == "Requisition_ListView" || View.Id == "Requisition_DetailView")
                {
                    bool bolDelete = false;
                    int intCount = 0;
                    if (View.SelectedObjects.Count > 0)
                    {
                        Session currentsession = ((XPObjectSpace)ObjectSpace).Session;
                        foreach (Requisition item in View.SelectedObjects)
                        {
                            intCount = intCount + 1;
                            if (item.Status != Requisition.TaskStatus.PendingReview)
                            {
                                Exception ex = new Exception(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletereviewed"));
                                throw ex;
                            }
                            if (View.SelectedObjects.Count == intCount)
                            {
                                bolDelete = true;
                            }
                        }
                        if (bolDelete == true)
                        {
                            base.Delete(args);
                            ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                            foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                            {
                                if (parent.Id == "ICM" || parent.Id == "InventoryManagement")
                                {
                                    foreach (ChoiceActionItem child in parent.Items)
                                    {
                                        if (child.Id == "Operations")
                                        {
                                            foreach (ChoiceActionItem subchild in child.Items)
                                            {
                                                if (subchild.Id == "Review")
                                                {
                                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                    var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                    if (count > 0)
                                                    {
                                                        subchild.Caption = cap[0] + " (" + count + ")";
                                                    }
                                                    else
                                                    {
                                                        subchild.Caption = cap[0];
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else if (View != null && View.Id == "Requisition_ListView_Entermode")
                {
                    foreach (Requisition item in View.SelectedObjects)
                    {
                        if (item.RQID == null)
                        {
                            ((ListView)View).CollectionSource.Remove(item);
                            objreq.Items.Remove(item.Item.items);
                        }
                    }
                }
                ////else if (View != null && View.Id == "IndoorInspection_ListView")
                ////{
                ////    if (View.SelectedObjects.Count > 0)
                ////    {
                ////        List<string> lstJobID = View.SelectedObjects.Cast<IndoorInspection>().Select(i => i.JobID.JobID).Distinct().ToList();
                ////        List<Guid> lstIndoorInspectionOid = View.SelectedObjects.Cast<IndoorInspection>().Select(i => i.Oid).Distinct().ToList();
                ////        bool IsDeleted = false;
                ////        base.Delete(args);
                ////        IObjectSpace os = Application.CreateObjectSpace();
                ////        IList<ProductSampleMapping> lstProductSampleMappings = os.GetObjects<ProductSampleMapping>(new InOperator("JobID.JobID", lstJobID));
                ////        IList<IndoorInspectionSamples> lstInspectionSamples = os.GetObjects<IndoorInspectionSamples>(new InOperator("JobID.Oid", lstIndoorInspectionOid));
                ////        if (lstInspectionSamples != null && lstInspectionSamples.Count > 0)
                ////        {
                ////            os.Delete(lstInspectionSamples);
                ////            IsDeleted = true;
                ////        }
                ////        if (lstProductSampleMappings != null && lstProductSampleMappings.Count > 0)
                ////        {
                ////            os.Delete(lstProductSampleMappings);
                ////            IsDeleted = true;
                ////        }
                ////        if (IsDeleted)
                ////        {
                ////            os.CommitChanges();
                ////        }
                ////        os.Dispose();
                ////        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                ////    }
                ////}
                ////else if (View.Id == "IndoorInspection_DetailView" && View.CurrentObject != null)
                ////{
                ////    IndoorInspection objIndoorInspection = (IndoorInspection)View.CurrentObject;
                ////    if (objIndoorInspection != null && objIndoorInspection.JobID != null)
                ////    {
                ////        IObjectSpace os = Application.CreateObjectSpace();
                ////        string strJobID = objIndoorInspection.JobID.JobID;
                ////        Guid inspectionOid = objIndoorInspection.Oid;
                ////        bool IsDeleted = false;
                ////        base.Delete(args);
                ////        IList<IndoorInspectionSamples> lstInspectionSamples = os.GetObjects<IndoorInspectionSamples>(CriteriaOperator.Parse("[JobID.Oid] = ?", inspectionOid));
                ////        if (lstInspectionSamples != null && lstInspectionSamples.Count > 0)
                ////        {
                ////            os.Delete(lstInspectionSamples);
                ////            IsDeleted = true;
                ////        }
                ////        IList<ProductSampleMapping> lstProductSampleMappings = os.GetObjects<ProductSampleMapping>(CriteriaOperator.Parse("[JobID.JobID] = ?", strJobID));
                ////        if (lstProductSampleMappings != null && lstProductSampleMappings.Count > 0)
                ////        {
                ////            os.Delete(lstProductSampleMappings);
                ////            IsDeleted = true;
                ////        }
                ////        if (IsDeleted)
                ////        {
                ////            os.CommitChanges();
                ////        }
                ////        os.Dispose();
                ////        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                ////    }
                ////}
                ////else if (View.Id == "SampleWeighingBatch_ListView")
                ////{
                ////    if (View.SelectedObjects.Count > 0)
                ////    {
                ////        List<Guid> lstBatchOid = View.SelectedObjects.Cast<SampleWeighingBatch>().Select(i => i.Oid).Distinct().ToList();
                ////        bool IsDeleted = false;
                ////        base.Delete(args);
                ////        IObjectSpace os = Application.CreateObjectSpace();
                ////        IList<SampleWeighingBatchSequence> lstBatchSamples = os.GetObjects<SampleWeighingBatchSequence>(new InOperator("[SampleWeighingBatchDetail.Oid] = ?", lstBatchOid));
                ////        List<Guid> lstSequence = lstBatchSamples.Select(i => i.Oid).Distinct().ToList();
                ////        if (lstBatchSamples == null && lstBatchSamples.Count > 0)
                ////        {
                ////            os.Delete(lstBatchSamples);
                ////            IsDeleted = true;
                ////        }
                ////        IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>(new InOperator("[SampleWeighingBatchID.Oid] = ?", lstSequence));
                ////        foreach (SampleParameter param in lstSamples)
                ////        {
                ////            param.SampleWeighingBatchID = null;
                ////        }
                ////        if (IsDeleted)
                ////        {
                ////            os.CommitChanges();
                ////        }
                ////        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                ////    }
                ////}
                //else if (View != null && View.Id == "SampleParameter_ListView_Copy_SampleRegistration")
                //{
                //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                //    foreach (SampleParameter sample in View.SelectedObjects)
                //    {
                //        IList<SampleParameter> objSL = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin]='" + sample.Samplelogin.Oid + "'"));
                //        if (objSL.Count == 1)
                //        {
                //            sample.Samplelogin.Test = false;
                //        }
                //        objectSpace.Delete(objectSpace.GetObjectByKey<SampleParameter>(sample.Oid));
                //        objectSpace.CommitChanges();
                //    }
                //    ObjectSpace.CommitChanges();
                //    NestedFrame nestedFrame = (NestedFrame)Frame;
                //    CompositeView view = nestedFrame.ViewItem.View;
                //    foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                //    {
                //        if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                //        {
                //            frameContainer.Frame.View.ObjectSpace.Refresh();
                //        }
                //    }
                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //}
                else if (View != null && View.Id == "City_Areas_ListView")
                {
                    base.Delete(args);
                    ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Topic))
                {
                    if (View is ListView && View.SelectedObjects.Count > 0)
                    {
                        foreach (Topic objtp in View.SelectedObjects)
                        {
                            IList<CRMProspects> cRMProspects = ObjectSpace.GetObjects<CRMProspects>(CriteriaOperator.Parse("[Topic] = ?", objtp)).ToList();
                            if (cRMProspects != null && cRMProspects.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteTopic"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else
                            {
                                ObjectSpace.Delete(objtp);
                                ObjectSpace.CommitChanges();
                                base.Delete(args);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                    }
                    else
                    if (View is DetailView && View.CurrentObject != null)
                    {
                        Topic objtp = (Topic)View.CurrentObject;
                        if (objtp != null)
                        {
                            IList<CRMProspects> cRMProspects = ObjectSpace.GetObjects<CRMProspects>(CriteriaOperator.Parse("[Topic] = ?", objtp)).ToList();
                            if (cRMProspects != null && cRMProspects.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteTopic"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else
                            {
                                ObjectSpace.Delete(objtp);
                                ObjectSpace.CommitChanges();
                                base.Delete(args);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                    }
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Customer))
                {
                    if (View is ListView && View.SelectedObjects.Count > 0)
                    {
                        foreach (Customer objCustomer in View.SelectedObjects.Cast<Customer>().ToList())
                        {
                            IList<Samplecheckin> lstSC = ObjectSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse("[ClientName.Oid] = ?", objCustomer.Oid));
                            IList<Contact> lstContact = ObjectSpace.GetObjects<Contact>(CriteriaOperator.Parse("[Customer.Oid] = ?", objCustomer.Oid));
                            IList<CRMQuotes> lstQuote = ObjectSpace.GetObjects<CRMQuotes>(CriteriaOperator.Parse("[Client.Oid] = ?", objCustomer.Oid));
                            IList<Project> lstproject = ObjectSpace.GetObjects<Project>(CriteriaOperator.Parse("[customername.Oid] = ?", objCustomer.Oid));
                            if (lstSC != null && lstSC.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteCustomerSR"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else if (lstContact != null && lstContact.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteCustomerContact"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else if (lstQuote != null && lstQuote.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteCustomerQuote"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else if (lstproject != null && lstproject.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteCustomerProject"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else
                            {
                                ObjectSpace.Delete(objCustomer);
                                ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                            }
                        }
                    }
                    else if (View is DetailView && View.CurrentObject != null)
                    {
                        IList<Samplecheckin> lstSC = ObjectSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse("[ClientName.Oid] = ?", ((Customer)View.CurrentObject).Oid));
                        IList<Contact> lstContact = ObjectSpace.GetObjects<Contact>(CriteriaOperator.Parse("[Customer.Oid] = ?", ((Customer)View.CurrentObject).Oid));
                        IList<CRMQuotes> lstQuote = ObjectSpace.GetObjects<CRMQuotes>(CriteriaOperator.Parse("[Client.Oid] = ?", ((Customer)View.CurrentObject).Oid));
                        IList<Project> lstproject = ObjectSpace.GetObjects<Project>(CriteriaOperator.Parse("[customername.Oid] = ?", ((Customer)View.CurrentObject).Oid));
                        if (lstSC != null && lstSC.Count > 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteCustomerSR"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        else if (lstContact != null && lstContact.Count > 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteCustomerContact"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        else if (lstQuote != null && lstQuote.Count > 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteCustomerQuote"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        else if (lstproject != null && lstproject.Count > 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteCustomerProject"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        else
                        {
                            ObjectSpace.Delete((Customer)View.CurrentObject);
                            ObjectSpace.CommitChanges();
                            base.Delete(args);
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                        }
                    }
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Contact))
                {
                    if (View is ListView && View.SelectedObjects.Count > 0)
                    {
                        foreach (Contact objContact in View.SelectedObjects.Cast<Contact>().ToList())
                        {
                            IList<Samplecheckin> lstSC = ObjectSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse("[ContactName.Oid] = ?", objContact.Oid));
                            //IList<Customer> lstContact = ObjectSpace.GetObjects<Customer>(CriteriaOperator.Parse("[Contacts][[Oid] = ?]", objContact.Oid));
                            IList<CRMQuotes> lstQuote = ObjectSpace.GetObjects<CRMQuotes>(CriteriaOperator.Parse("[PrimaryContact.Oid] = ?", objContact.Oid));
                            if (lstSC != null && lstSC.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteContactSR"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            //else if (lstContact != null && lstContact.Count > 0)
                            //{
                            //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteContactCustomer"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            //    return;
                            //}
                            else if (lstQuote != null && lstQuote.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteContactQuote"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else
                            {
                                ObjectSpace.Delete(objContact);
                                ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                            }
                        }
                    }
                    else if (View is DetailView && View.CurrentObject != null)
                    {
                        if (Frame is NestedFrame)
                        {
                            IList<Samplecheckin> lstSC = ObjectSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse("[ContactName.Oid] = ?", ((Contact)View.CurrentObject).Oid));
                            IList<Customer> lstContact = ObjectSpace.GetObjects<Customer>(CriteriaOperator.Parse("[Contacts][[Oid] = ?]", ((Contact)View.CurrentObject).Oid));
                            IList<CRMQuotes> lstQuote = ObjectSpace.GetObjects<CRMQuotes>(CriteriaOperator.Parse("[PrimaryContact.Oid] = ?", ((Contact)View.CurrentObject).Oid));
                            if (lstSC != null && lstSC.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteContactSR"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            //else if (lstContact != null && lstContact.Count > 0)
                            //{
                            //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteContactCustomer"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            //    return;
                            //}
                            else if (lstQuote != null && lstQuote.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteContactQuote"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else
                            {
                                ObjectSpace.Delete((Contact)View.CurrentObject);
                                ObjectSpace.CommitChanges();
                                base.Delete(args);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                            }
                        }
                        else if (Application.MainWindow.View.ObjectTypeInfo != typeof(Customer))
                        {
                            IList<Samplecheckin> lstSC = ObjectSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse("[ContactName.Oid] = ?", ((Contact)View.CurrentObject).Oid));
                            IList<Customer> lstContact = ObjectSpace.GetObjects<Customer>(CriteriaOperator.Parse("[Contacts][[Oid] = ?]", ((Contact)View.CurrentObject).Oid));
                            IList<CRMQuotes> lstQuote = ObjectSpace.GetObjects<CRMQuotes>(CriteriaOperator.Parse("[PrimaryContact.Oid] = ?", ((Contact)View.CurrentObject).Oid));
                            if (lstSC != null && lstSC.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteContactSR"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            //else if (lstContact != null && lstContact.Count > 0)
                            //{
                            //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteContactCustomer"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            //    return;
                            //}
                            else if (lstQuote != null && lstQuote.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteContactQuote"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else
                            {
                                ObjectSpace.Delete((Contact)View.CurrentObject);
                                ObjectSpace.CommitChanges();
                                base.Delete(args);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                            }
                        }
                    }
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(TestMethod))
                {
                    if (View is DetailView && View.CurrentObject != null)
                    {
                        IList<SampleParameter> lstSampleParameter = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ?", ((TestMethod)View.CurrentObject).Oid));
                        if (lstSampleParameter != null && lstSampleParameter.Count > 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletetestmethod"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            IList<Testparameter> lstTestParams = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", ((TestMethod)View.CurrentObject).Oid));
                            IList<Component> lstTestComps = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", ((TestMethod)View.CurrentObject).Oid));
                            foreach (Testparameter param in lstTestParams.ToList())
                            {
                                ObjectSpace.Delete(param);
                            }
                            foreach (Component comp in lstTestComps.ToList())
                            {
                                ObjectSpace.Delete(comp);
                            }
                            base.Delete(args);
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else if (View is ListView && View.SelectedObjects.Count > 0)
                    {
                        foreach (TestMethod objTest in View.SelectedObjects.Cast<TestMethod>().ToList())
                        {
                            IList<SampleParameter> lstSampleParameter = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ?", objTest.Oid));
                            if (lstSampleParameter != null && lstSampleParameter.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletetestmethod"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                IList<Testparameter> lstTestParams = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objTest.Oid));
                                IList<Component> lstTestComps = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", ((TestMethod)View.CurrentObject).Oid));
                                foreach (Testparameter param in lstTestParams.ToList())
                                {
                                    ObjectSpace.Delete(param);
                                }
                                foreach (Component comp in lstTestComps.ToList())
                                {
                                    ObjectSpace.Delete(comp);
                                }
                                ObjectSpace.Delete(objTest);
                                ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                    }
                }
                //else if (View != null && View.ObjectTypeInfo.Type == typeof(Customer))
                //{
                //    if (View.SelectedObjects.Count > 0)
                //    {
                //        foreach ()
                //        {

                //        }
                //    }
                //}
                //else if (View != null && View.Id == "SampleParameter_ListView_Copy_ResultEntry")
                //{
                //    if (View.SelectedObjects.Count > 0)
                //    {

                //        foreach (SampleParameter objsp in args.SelectedObjects)
                //        {
                //            if (objsp != null)
                //            {
                //                if (objsp.Result != null && objsp.Result.Length > 0)
                //                {
                //                    objsp.Result = string.Empty;
                //                    objsp.AnalyzedBy = null;
                //                    objsp.AnalyzedDate = null;
                //                    objsp.EnteredBy = null;
                //                    objsp.EnteredDate = null;
                //                    ObjectSpace.CommitChanges();
                //                }
                //            }
                //        }
                //        Application.ShowViewStrategy.ShowMessage("Result deleted successfully.", InformationType.Success, 1500, InformationPosition.Top);
                //        ObjectSpace.Refresh();
                //    }
                //}
                //else if (View != null && View.ObjectTypeInfo.Type == typeof(Modules.BusinessObjects.QC.QCBatch))
                //{
                //    if (View.SelectedObjects.Count > 0)
                //    {
                //        foreach (Modules.BusinessObjects.QC.QCBatch objQCBatch in View.SelectedObjects.Cast<Modules.BusinessObjects.QC.QCBatch>().ToList())
                //        {
                //            IList<SampleParameter> lstSampleParameter = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail.Oid] = ? and [Status] > 'PendingEntry'", objQCBatch.Oid));
                //            if (lstSampleParameter != null && lstSampleParameter.Count > 0)
                //            {
                //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteqcbatch"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                //            }
                //            else
                //            {
                //                for (int i = 0; i < lstSampleParameter.Count; i++)
                //                {
                //                    if (lstSampleParameter[i] != null)
                //                    {
                //                        lstSampleParameter[i].QCBatchID = null;
                //                    }
                //                }
                //                IList<Modules.BusinessObjects.QC.QCBatchSequence> lstQC = ObjectSpace.GetObjects<Modules.BusinessObjects.QC.QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail.Oid] = ?", objQCBatch.Oid));
                //                ObjectSpace.Delete(lstQC);

                //                //foreach (Modules.BusinessObjects.QC.QCBatchSequence param in lstQC.ToList())
                //                //{
                //                //    ObjectSpace.Delete(param);
                //                //}
                //                ObjectSpace.CommitChanges();
                //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //            }
                //        }
                //    }
                //}
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Modules.BusinessObjects.SampleManagement.SpreadSheetEntry_AnalyticalBatch))
                {
                    if (View is ListView)
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            foreach (Modules.BusinessObjects.SampleManagement.SpreadSheetEntry_AnalyticalBatch objABID in View.SelectedObjects.Cast<Modules.BusinessObjects.SampleManagement.SpreadSheetEntry_AnalyticalBatch>().ToList())
                            {
                                IList<SampleParameter> lstSampleParameter = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[UQABID.Oid] = ? and [Status] > 'PendingEntry' and [Samplelogin] is Not null ", objABID.Oid));
                                if (lstSampleParameter != null && lstSampleParameter.Count > 0)
                                {
                                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteanalyticalbatch"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteqcbatch"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                                else
                                {
                                    IList<SampleParameter> delSampleParameter = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[UQABID.Oid] = ? and [Status] = 'PendingEntry' and [Samplelogin] is Not null ", objABID.Oid));

                                    if (delSampleParameter != null && delSampleParameter.Count > 0)
                                    {
                                        for (int i = 0; i < delSampleParameter.Count; i++)
                                        {
                                            if (delSampleParameter[i] != null)
                                            {
                                                delSampleParameter[i].UQABID = null;
                                                delSampleParameter[i].ABID = string.Empty;
                                                delSampleParameter[i].Result = string.Empty;
                                                delSampleParameter[i].FinalResult = string.Empty;
                                                delSampleParameter[i].ResultNumeric = string.Empty;
                                                delSampleParameter[i].QCBatchID = null;
                                                delSampleParameter[i].QCSort = 0;
                                            }
                                        }
                                    }
                                    IList<Modules.BusinessObjects.SampleManagement.SpreadSheetEntry> lstQC = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID.Oid] = ?", objABID.Oid));
                                    ObjectSpace.Delete(lstQC);
                                    //foreach (Modules.BusinessObjects.SampleManagement.SpreadSheetEntry param in lstQC.ToList())
                                    //{
                                    //    ObjectSpace.Delete(param);
                                    //}
                                    ObjectSpace.Delete(objABID);
                                    ObjectSpace.CommitChanges();
                                    //base.Delete(args);
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                            }
                        }
                    }
                    else if (View is DetailView)
                    {
                        Modules.BusinessObjects.SampleManagement.SpreadSheetEntry_AnalyticalBatch objABID = (Modules.BusinessObjects.SampleManagement.SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                        if (objABID != null)
                        {
                            IList<SampleParameter> lstSampleParameter = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[UQABID.Oid] = ? and [Status] > 'PendingEntry' and [Samplelogin] is Not null", objABID.Oid));
                            if (lstSampleParameter != null && lstSampleParameter.Count > 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteanalyticalbatch"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                IList<SampleParameter> delSampleParameter = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[UQABID.Oid] = ? and [Status] = 'PendingEntry' and [Samplelogin] is Not null", objABID.Oid));
                                if (delSampleParameter != null && delSampleParameter.Count > 0)
                                {
                                    for (int i = 0; i < delSampleParameter.Count; i++)
                                    {
                                        if (delSampleParameter[i] != null)
                                        {
                                            delSampleParameter[i].UQABID = null;
                                            delSampleParameter[i].ABID = string.Empty;
                                            delSampleParameter[i].Result = string.Empty;
                                            delSampleParameter[i].FinalResult = string.Empty;
                                            delSampleParameter[i].ResultNumeric = string.Empty;
                                            delSampleParameter[i].QCBatchID = null;
                                            delSampleParameter[i].QCSort = 0;
                                        }
                                    }
                                }
                                IList<Modules.BusinessObjects.SampleManagement.SpreadSheetEntry> lstQC = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID.Oid] = ?", objABID.Oid));
                                ObjectSpace.Delete(lstQC);
                                //foreach (Modules.BusinessObjects.SampleManagement.SpreadSheetEntry param in lstQC.ToList())
                                //{
                                //    ObjectSpace.Delete(param);
                                //}
                                ObjectSpace.CommitChanges();
                                base.Delete(args);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                    }
                }
                ////else if (View != null && View.Id == "SubOutSampleRegistrations_ListView_ViewMode")
                ////{
                ////    IObjectSpace os = Application.CreateObjectSpace();
                ////    int DeleteCount = 0;
                ////    int TotalCount = View.SelectedObjects.Count;
                ////    foreach (SubOutSampleRegistrations ObjSubouts in View.SelectedObjects)
                ////    {
                ////        SubOutSampleRegistrations objSubOut = os.GetObject((ObjSubouts));
                ////        if (objSubOut != null && objSubOut.SampleParameter != null)
                ////        {
                ////            IList<SampleParameter> objParam = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SuboutSample.Oid] = ?", objSubOut.Oid));
                ////            int objParamCount = objSubOut.SampleParameter.Where(i => i.Status == Samplestatus.PendingEntry).Count();
                ////            if (objParam != null && objParam.Count > 0)
                ////            {
                ////                if (objParamCount == objParam.Count)
                ////                {
                ////                    foreach (SampleParameter obj in objParam)
                ////                    {
                ////                        obj.ResultNumeric = null;
                ////                        obj.Result = null;
                ////                        obj.Units = null;
                ////                        obj.DF = null;
                ////                        obj.LOQ = null;
                ////                        obj.RptLimit = null;
                ////                        obj.UQL = null;
                ////                        obj.MDL = null;
                ////                        obj.SpikeAmount = 0;
                ////                        obj.Rec = null;
                ////                        obj.RPD = null;
                ////                        obj.RecHCLimit = null;
                ////                        obj.RecLCLimit = null;
                ////                        obj.RPDHCLimit = null;
                ////                        obj.RPDLCLimit = null;
                ////                        obj.AnalyzedBy = null;
                ////                        obj.AnalyzedDate = null;
                ////                        obj.IsExportedSuboutResult = false;
                ////                        objSubOut.SampleParameter.Remove(obj);
                ////                    }
                ////                    os.Delete(objSubOut);
                ////                    os.CommitChanges();
                ////                    DeleteCount = DeleteCount + 1;
                ////                }
                ////                else
                ////                {
                ////                    foreach (SampleParameter obj in objParam)
                ////                    {
                ////                        if (obj.Status == Samplestatus.PendingEntry)
                ////                        {
                ////                            obj.ResultNumeric = null;
                ////                            obj.Result = null;
                ////                            obj.Units = null;
                ////                            obj.DF = null;
                ////                            obj.LOQ = null;
                ////                            obj.RptLimit = null;
                ////                            obj.UQL = null;
                ////                            obj.MDL = null;
                ////                            obj.SpikeAmount = 0;
                ////                            obj.Rec = null;
                ////                            obj.RPD = null;
                ////                            obj.RecHCLimit = null;
                ////                            obj.RecLCLimit = null;
                ////                            obj.RPDHCLimit = null;
                ////                            obj.RPDLCLimit = null;
                ////                            obj.AnalyzedBy = null;
                ////                            obj.AnalyzedDate = null;
                ////                            obj.IsExportedSuboutResult = false;
                ////                            objSubOut.SampleParameter.Remove(obj);
                ////                        }
                ////                    }
                ////                    os.CommitChanges();
                ////                }
                ////            }

                ////        }
                ////    }
                ////    os.Refresh();
                ////    View.ObjectSpace.Refresh();
                ////    if (DeleteCount == TotalCount)
                ////    {
                ////        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                ////    }
                ////    else
                ////    {
                ////        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteSubout"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                ////    }
                ////}
                else if (View != null && View.Id == "SamplePrepBatch_DetailView_Copy" || View.Id == "SamplePrepBatch_DetailView_Copy_History")
                {
                    Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch obj = (Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch)View.CurrentObject;
                    if (obj != null)
                    {
                        if (ObjectSpace.FirstOrDefault<SampleParameter>(i => i.PrepBatchID != null && i.PrepBatchID.Contains(obj.Oid.ToString()) && (i.QCBatchID != null || i.UQABID != null)) == null)
                        {
                            List<string> lstMatrixOid = obj.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                            List<string> lstTestOid = obj.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                            List<string> lstMethdOid = obj.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                            List<string> lstTest = new List<string>();
                            if (lstTestOid != null)
                            {
                                foreach (string objOid in lstTestOid)
                                {
                                    if (!string.IsNullOrEmpty(objOid))
                                    {
                                        TestMethod objTest = ObjectSpace.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                        if (objTest != null && !lstTest.Contains(objTest.TestName))
                                        {
                                            lstTest.Add(objTest.TestName);
                                        }
                                    }

                                }
                            }
                            List<string> lstMatrix = new List<string>();
                            if (lstMatrixOid != null)
                            {
                                foreach (string objOid in lstMatrixOid)
                                {
                                    if (!string.IsNullOrEmpty(objOid))
                                    {
                                        Matrix objTest = ObjectSpace.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                        if (objTest != null && !lstMatrix.Contains(objTest.MatrixName))
                                        {
                                            lstMatrix.Add(objTest.MatrixName);
                                        }
                                    }

                                }
                            }
                            List<string> lstMethod = new List<string>();
                            if (lstMethdOid != null)
                            {
                                foreach (string objOid in lstMethdOid)
                                {
                                    if (!string.IsNullOrEmpty(objOid))
                                    {
                                        Method objTest = ObjectSpace.GetObjectByKey<Method>(new Guid(objOid.Trim()));
                                        if (lstMethod != null && !lstMethod.Contains(objTest.MethodNumber))
                                        {
                                            lstMethod.Add(objTest.MethodNumber);
                                        }
                                    }

                                }
                            }
                            List<TestMethod> lsts = ObjectSpace.GetObjects<TestMethod>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[PrepMethods][].Count() > 0"), new InOperator("MatrixName.MatrixName", lstMatrix),
                    new InOperator("TestName", lstTest), new InOperator("MethodName.MethodNumber", lstMethod))).ToList();
                            if ((lsts.FirstOrDefault(i => i.PrepMethods.Count == 2) != null) && obj.Sort == 1)
                            {
                                bool IsDelete = true;
                                foreach (TestMethod objtestmethod in lsts)
                                {
                                    if (ObjectSpace.FirstOrDefault<SampleParameter>(i => i.PrepBatchID.Contains(obj.Oid.ToString()) && objtestmethod.PrepMethods.Count == 2 && i.IsPrepMethodComplete == true && i.Testparameter != null
                                    && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.Oid == objtestmethod.Oid) != null)
                                    {
                                        if (ObjectSpace.FirstOrDefault<SampleParameter>(i => i.PrepBatchID.Contains(obj.Oid.ToString()) && i.IsPrepMethodComplete == true && i.PrepMethodCount == objtestmethod.PrepMethods.Count) != null)
                                        {
                                            IsDelete = false;
                                            break;
                                        }
                                    }
                                }
                                if (IsDelete)
                                {
                                    IList<SamplePrepBatchSequence> seq = ObjectSpace.GetObjects<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail]=?", obj.Oid));
                                    IList<Samplecheckin> samplecheck = seq.Where(i=>i.SampleID!=null).Select(i => i.SampleID.JobID).Distinct().ToList();
                                    ObjectSpace.Delete(seq);
                                    IList<SampleParameter> objsampleParameters = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Contains([PrepBatchID], ?)", obj.Oid.ToString()));
                                    if (obj.Sort == 1)
                                    {
                                        foreach (SampleParameter sample in objsampleParameters.ToList())
                                        {
                                            sample.PrepBatchID = null;
                                            sample.PrepMethodCount = 0;
                                            sample.IsPrepMethodComplete = false;
                                        }
                                    }
                                    else if (obj.Sort == 2)
                                    {
                                        foreach (SampleParameter sample in objsampleParameters.ToList())
                                        {
                                            List<string> lst = sample.PrepBatchID.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                            if (lst.Contains(obj.Oid.ToString()))
                                            {
                                                lst.Remove(obj.Oid.ToString());
                                            }
                                            sample.PrepBatchID = lst.FirstOrDefault();
                                            sample.PrepMethodCount = 1;
                                            sample.IsPrepMethodComplete = false;
                                        }
                                    }


                                    ObjectSpace.Delete(obj);
                                    ObjectSpace.CommitChanges();
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    Samplecheckin samplecheckin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", obj.Jobid));
                                    if (samplecheckin != null)
                                    {
                                        List<SampleParameter> lstSampleParam = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.TestMethod.PrepMethods][].Count() > 0", samplecheckin.Oid)).ToList();
                                        Samplecheckin samplecheckin1 = os.GetObjectByKey<Samplecheckin>(samplecheckin.Oid);
                                        if (lstSampleParam.FirstOrDefault(i => i.PrepMethodCount == 0 && i.IsPrepMethodComplete == false) != null)
                                        {
                                            StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=10"));
                                            if (status != null)
                                            {
                                                samplecheckin1.Index = status;
                                            }
                                        }
                                        else if (lstSampleParam.FirstOrDefault(i => i.PrepMethodCount > 0 && i.IsPrepMethodComplete == false) != null)
                                        {
                                            StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=15"));
                                            if (status != null)
                                            {
                                                samplecheckin1.Index = status;
                                            }
                                        }
                                        os.CommitChanges();
                                    }

                                    View.Close();
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteprepbatch"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }

                            }
                            else
                            {
                                IList<SamplePrepBatchSequence> seq = ObjectSpace.GetObjects<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail]=?", obj.Oid));
                                IList<Samplecheckin> samplecheck = seq.Where(i=>i.SampleID!=null).Select(i => i.SampleID.JobID).Distinct().ToList();
                                ObjectSpace.Delete(seq);
                                IList<SampleParameter> objsampleParameters = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Contains([PrepBatchID], ?)", obj.Oid.ToString()));
                                if (obj.Sort == 1)
                                {
                                    foreach (SampleParameter sample in objsampleParameters.ToList())
                                    {
                                        sample.PrepBatchID = null;
                                        sample.PrepMethodCount = 0;
                                        sample.IsPrepMethodComplete = false;
                                    }
                                }
                                else if (obj.Sort == 2)
                                {
                                    foreach (SampleParameter sample in objsampleParameters.ToList())
                                    {
                                        List<string> lst = sample.PrepBatchID.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                        if (lst.Contains(obj.Oid.ToString()))
                                        {
                                            lst.Remove(obj.Oid.ToString());
                                        }
                                        sample.PrepBatchID = lst.FirstOrDefault();
                                        sample.PrepMethodCount = 1;
                                        sample.IsPrepMethodComplete = false;
                                    }
                                }

                                ObjectSpace.Delete(obj);
                                ObjectSpace.CommitChanges();

                                IObjectSpace os = Application.CreateObjectSpace();
                                Samplecheckin samplecheckin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", obj.Jobid));
                                if (samplecheckin != null)
                                {
                                    List<SampleParameter> lstSampleParam = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.TestMethod.PrepMethods][].Count() > 0", samplecheckin.Oid)).ToList();
                                    Samplecheckin samplecheckin1 = os.GetObjectByKey<Samplecheckin>(samplecheckin.Oid);
                                    if (lstSampleParam.FirstOrDefault(i => i.PrepMethodCount == 0 && i.IsPrepMethodComplete == false) != null)
                                    {
                                        StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=10"));
                                        if (status != null)
                                        {
                                            samplecheckin1.Index = status;
                                        }
                                    }
                                    else if (lstSampleParam.FirstOrDefault(i => i.PrepMethodCount > 0 && i.IsPrepMethodComplete == false) != null)
                                    {
                                        StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=15"));
                                        if (status != null)
                                        {
                                            samplecheckin1.Index = status;
                                        }
                                    }
                                    os.CommitChanges();
                                }


                                View.Close();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }

                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteprepbatch"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }


                    }
                    //base.Delete(args);
                }
                else if (View != null && View.Id == "SamplePrepBatch_ListView")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch obj in View.SelectedObjects.Cast<SamplePrepBatch>().ToList())
                        {
                            if (ObjectSpace.FirstOrDefault<SampleParameter>(i => i.PrepBatchID != null && i.PrepBatchID.Contains(obj.Oid.ToString()) && (i.QCBatchID != null || i.UQABID != null)) == null)
                            {
                                List<string> lstMatrixOid = obj.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                List<string> lstTestOid = obj.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                List<string> lstMethdOid = obj.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                List<string> lstTest = new List<string>();
                                if (lstTestOid != null)
                                {
                                    foreach (string objOid in lstTestOid)
                                    {
                                        if (!string.IsNullOrEmpty(objOid))
                                        {
                                            TestMethod objTest = ObjectSpace.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                            if (objTest != null && !lstTest.Contains(objTest.TestName))
                                            {
                                                lstTest.Add(objTest.TestName);
                                            }
                                        }

                                    }
                                }
                                List<string> lstMatrix = new List<string>();
                                if (lstMatrixOid != null)
                                {
                                    foreach (string objOid in lstMatrixOid)
                                    {
                                        if (!string.IsNullOrEmpty(objOid))
                                        {
                                            Matrix objTest = ObjectSpace.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                            if (objTest != null && !lstMatrix.Contains(objTest.MatrixName))
                                            {
                                                lstMatrix.Add(objTest.MatrixName);
                                            }
                                        }

                                    }
                                }
                                List<string> lstMethod = new List<string>();
                                if (lstMethdOid != null)
                                {
                                    foreach (string objOid in lstMethdOid)
                                    {
                                        if (!string.IsNullOrEmpty(objOid))
                                        {
                                            Method objTest = ObjectSpace.GetObjectByKey<Method>(new Guid(objOid.Trim()));
                                            if (lstMethod != null && !lstMethod.Contains(objTest.MethodNumber))
                                            {
                                                lstMethod.Add(objTest.MethodNumber);
                                            }
                                        }

                                    }
                                }
                                List<TestMethod> lsts = ObjectSpace.GetObjects<TestMethod>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[PrepMethods][].Count() > 0"), new InOperator("MatrixName.MatrixName", lstMatrix),
                        new InOperator("TestName", lstTest), new InOperator("MethodName.MethodNumber", lstMethod))).ToList();
                                if ((lsts.FirstOrDefault(i => i.PrepMethods.Count == 2) != null) && obj.Sort == 1)
                                {
                                    bool IsDelete = true;
                                    foreach (TestMethod objtestmethod in lsts)
                                    {
                                        if (ObjectSpace.FirstOrDefault<SampleParameter>(i => i.PrepBatchID.Contains(obj.Oid.ToString()) && objtestmethod.PrepMethods.Count == 2 && i.IsPrepMethodComplete == true && i.Testparameter != null
                                        && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.Oid == objtestmethod.Oid) != null)
                                        {
                                            if (ObjectSpace.FirstOrDefault<SampleParameter>(i => i.PrepBatchID.Contains(obj.Oid.ToString()) && i.IsPrepMethodComplete == true && i.PrepMethodCount == objtestmethod.PrepMethods.Count) != null)
                                            {
                                                IsDelete = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (IsDelete)
                                    {
                                        IList<SamplePrepBatchSequence> seq = ObjectSpace.GetObjects<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail]=?", obj.Oid));
                                        IList<Samplecheckin> samplecheck = seq.Where(i=>i.SampleID!= null && i.SampleID.JobID != null).Select(i => i.SampleID.JobID).Distinct().ToList();
                                        ObjectSpace.Delete(seq);
                                        IList<SampleParameter> objsampleParameters = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Contains([PrepBatchID], ?)", obj.Oid.ToString()));
                                        if (obj.Sort == 1) 
                                        {
                                            foreach (SampleParameter sample in objsampleParameters.ToList())
                                            {
                                                sample.PrepBatchID = null;
                                                sample.PrepMethodCount = 0;
                                                sample.IsPrepMethodComplete = false;
                                                sample.OSSync = true;
                                            }
                                        }
                                        else if (obj.Sort == 2)
                                        {
                                            foreach (SampleParameter sample in objsampleParameters.ToList())
                                            {
                                                List<string> lst = sample.PrepBatchID.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                                if (lst.Contains(obj.Oid.ToString()))
                                                {
                                                    lst.Remove(obj.Oid.ToString());
                                                }
                                                sample.PrepBatchID = lst.FirstOrDefault();
                                                sample.PrepMethodCount = 1;
                                                sample.IsPrepMethodComplete = false;
                                                sample.OSSync = true;
                                            }
                                        }
                                        

                                        ObjectSpace.Delete(obj);
                                        ObjectSpace.CommitChanges();
                                        IObjectSpace os = Application.CreateObjectSpace();
                                        Samplecheckin samplecheckin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", obj.Jobid));
                                        IList<Notes> notes = os.GetObjects<Notes>(CriteriaOperator.Parse("[Samplecheckin.Oid] =? AND [NoteSource] <> 'Sample Registration' ", samplecheckin.Oid));
                                        os.Delete(notes);
                                        if (samplecheckin != null)
                                        {
                                            List<SampleParameter> lstSampleParam = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.TestMethod.PrepMethods][].Count() > 0", samplecheckin.Oid)).ToList();
                                            Samplecheckin samplecheckin1 = os.GetObjectByKey<Samplecheckin>(samplecheckin.Oid);
                                            if (lstSampleParam.FirstOrDefault(i => i.PrepMethodCount == 0 && i.IsPrepMethodComplete == false) != null)
                                            {
                                                StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=10"));
                                                if (status != null)
                                                {
                                                    samplecheckin1.Index = status;
                                                }
                                            }
                                            else if (lstSampleParam.FirstOrDefault(i => i.PrepMethodCount > 0 && i.IsPrepMethodComplete == false) != null)
                                            {
                                                StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=15"));
                                                if (status != null)
                                                {
                                                    samplecheckin1.Index = status;
                                                }
                                            }
                                            os.CommitChanges();
                                        }
                                        //View.Close();
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteprepbatch"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }

                                }
                                else
                                {
                                    IList<SamplePrepBatchSequence> seq = ObjectSpace.GetObjects<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail]=?", obj.Oid));
                                    IList<Samplecheckin> samplecheck = seq.Where(i => i.SampleID != null).Select(i => i.SampleID.JobID).Distinct().ToList();
                                    ObjectSpace.Delete(seq);
                                    IList<SampleParameter> objsampleParameters = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Contains([PrepBatchID], ?)", obj.Oid.ToString()));
                                    if (obj.Sort == 1)
                                    {
                                        foreach (SampleParameter sample in objsampleParameters.ToList())
                                        {
                                            sample.PrepBatchID = null;
                                            sample.PrepMethodCount = 0;
                                            sample.IsPrepMethodComplete = false;
                                            sample.OSSync = true;
                                        }
                                    }
                                    else if (obj.Sort == 2)
                                    {
                                        foreach (SampleParameter sample in objsampleParameters.ToList())
                                        {
                                            List<string> lst = sample.PrepBatchID.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                            if (lst.Contains(obj.Oid.ToString()))
                                            {
                                                lst.Remove(obj.Oid.ToString());
                                            }
                                            sample.PrepBatchID = lst.FirstOrDefault();
                                            sample.PrepMethodCount = 1;
                                            sample.IsPrepMethodComplete = false;
                                            sample.OSSync = true;
                                        }
                                    }
                                    ObjectSpace.Delete(obj);
                                    ObjectSpace.CommitChanges();
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    Samplecheckin samplecheckin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", obj.Jobid));
                                    List<SampleParameter> lstSampleParam = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.TestMethod.PrepMethods][].Count() > 0", samplecheckin.Oid)).ToList();
                                    Samplecheckin samplecheckin1 = os.GetObjectByKey<Samplecheckin>(samplecheckin.Oid);

                                    if (lstSampleParam.FirstOrDefault(i => i.PrepMethodCount == 0 && i.IsPrepMethodComplete == false) != null)
                                    {
                                        StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=10"));
                                        if (status != null)
                                        {
                                            samplecheckin1.Index = status;
                                        }
                                    }
                                    else if (lstSampleParam.FirstOrDefault(i => i.PrepMethodCount > 0 && i.IsPrepMethodComplete == false) != null)
                                    {
                                        StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=15"));
                                        if (status != null)
                                        {
                                            samplecheckin1.Index = status;
                                        }
                                    }
                                    os.CommitChanges();





                                    //View.Close();
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                //IList<SamplePrepBatchSequence> seq = ObjectSpace.GetObjects<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail]=?", obj.Oid));
                                //ObjectSpace.Delete(seq);
                                //IList<SampleParameter> objsampleParameters = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SamplePrepBatchID.SamplePrepBatchDetail]=?", obj.Oid));
                                //foreach (SampleParameter sample in objsampleParameters.ToList())
                                //{
                                //    sample.SamplePrepBatchID = null;
                                //}
                                //ObjectSpace.Delete(obj);
                                //ObjectSpace.CommitChanges();
                                //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeleteprepbatch"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                }
                //else if (View != null && View.Id == "SamplingProposal_ListView")
                //{
                //    if (View.SelectedObjects.Count > 0)
                //    {
                //        foreach (SamplingProposal objTask in View.SelectedObjects)
                //        {
                //            List<Sampling> lstSamples = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", objTask.Oid)).ToList();
                //            if (lstSamples.Count > 0)
                //            {
                //                Application.ShowViewStrategy.ShowMessage("The RegistrationID cannot be deleted! it is used in sample login.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //            }
                //            else
                //            {
                //                base.Delete(args);
                //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                //    }
                //}
                //else if (View != null && (View.Id == "SamplingProposal_DetailView"))
                //{
                //    if (View.CurrentObject != null)
                //    {
                //        SamplingProposal objSamplingProposal = (SamplingProposal)View.CurrentObject;
                //        List<Sampling> lstSamples = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", objSamplingProposal.Oid)).ToList();
                //        if (lstSamples.Count > 0)
                //        {
                //            Application.ShowViewStrategy.ShowMessage("The RegistrationID cannot be deleted! it is used in sample login.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //        }
                //        else
                //        {
                //            base.Delete(args);
                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //        }
                //    }
                //}
                //else if (View != null && View.Id == "Sampling_ListView_SamplingProposal")
                //{
                //    if (View.SelectedObjects.Count > 0)
                //    {
                //        foreach (Sampling objSampling in View.SelectedObjects)
                //        {
                //            if (objSampling != null && objSampling.SamplingProposal != null)
                //            {
                //                if (objSampling.Testparameters.Count > 0)
                //                {
                //                    List<SamplingParameter> lstSamplingTests = objSampling.SamplingParameter.Cast<SamplingParameter>().ToList();
                //                    if (lstSamplingTests.Count > 0)
                //                    {
                //                        ObjectSpace.Delete(lstSamplingTests);
                //                    }
                //                    ObjectSpace.Delete(objSampling);
                //                    if (objSampling.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                //                    {
                //                        Frame.GetController<AuditlogViewController>().insertauditdata(ObjectSpace, objSampling.SamplingProposal.Oid, OperationType.Deleted, "Sampling Proposal", objSampling.SamplingProposal.RegistrationID, "Samples", objSampling.SampleID, "", "");
                //                    }
                //                }
                //                else
                //                {
                //                    ObjectSpace.Delete(objSampling);
                //                    if (objSampling.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                //                    {
                //                        Frame.GetController<AuditlogViewController>().insertauditdata(ObjectSpace, objSampling.SamplingProposal.Oid, OperationType.Deleted, "Sampling Proposal", objSampling.SamplingProposal.RegistrationID, "Samples", objSampling.SampleID, "", "");
                //                    }
                //                }
                //            }
                //        }
                //        ObjectSpace.CommitChanges();
                //        ObjectSpace.Refresh();
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                //    }
                //}
               

                else if (View.ObjectTypeInfo.Type == typeof(Purchaseorder))
                {
                    base.Delete(args);
                    ObjectSpace.CommitChanges();
                    ObjectSpace.Refresh();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    base.Delete(args);
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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
