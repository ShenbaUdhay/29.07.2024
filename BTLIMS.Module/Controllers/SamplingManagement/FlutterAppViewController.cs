using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.AlpacaLims;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SDA.AlpacaLims;
using Modules.BusinessObjects.TaskManagement;
using System;

namespace LDM.Module.Controllers.SamplingManagement
{
    public partial class FlutterAppViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        //List<Tuple<string, string>> collist = new List<Tuple<string, string>>{
        //            new Tuple<string,string>("Conductivity", "Conductivity_uS_cm_"),
        //            new Tuple<string,string>("pH", "pH_StandardUnits_"),
        //            new Tuple<string,string>("Flow", "Flow_CFS_"),
        //            new Tuple<string,string>("Temperature", "Temprature_C_"),
        //        };

        public FlutterAppViewController()
        {
            InitializeComponent();
        }

        public void deletesample(UnitOfWork uow, Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin)
        {
            try
            {
                FlutterSDARRA_FieldDataEntryInfo obj = uow.FindObject<FlutterSDARRA_FieldDataEntryInfo>(CriteriaOperator.Parse("[SampleID]=?", objSampleLogin.SampleID));
                if (obj != null)
                {
                    uow.Delete(obj);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void deletesample(IObjectSpace os, Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin)
        {
            try
            {
                FlutterSDARRA_FieldDataEntryInfo obj = os.FindObject<FlutterSDARRA_FieldDataEntryInfo>(CriteriaOperator.Parse("[SampleID]=?", objSampleLogin.SampleID));
                if (obj != null)
                {
                    os.Delete(obj);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void insertsample(UnitOfWork uow, Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin)
        {
            try
            {
                FlutterSDARRA_FieldDataEntryInfo sample = uow.FindObject<FlutterSDARRA_FieldDataEntryInfo>(CriteriaOperator.Parse("[SampleID]=?", objSampleLogin.SampleID));
                if (sample == null)
                {
                    FlutterSDARRA_FieldDataEntryInfo objflu = new FlutterSDARRA_FieldDataEntryInfo(uow);
                    if (objSampleLogin.StationLocation != null)
                    {
                        objflu.uqStationID = objSampleLogin.StationLocation.Oid;
                        objflu.Station = objSampleLogin.StationLocation.SiteName;
                        objflu.StationID = objSampleLogin.StationLocation.SiteID;
                        insertalternatestation(uow, objSampleLogin.StationLocation, objSampleLogin);
                    }
                    if (objSampleLogin.JobID.ProjectID != null)
                    {
                        objflu.ProjectID = objSampleLogin.JobID.ProjectID.ProjectId;
                        objflu.ProjectName = objSampleLogin.JobID.ProjectID.ProjectName;
                    }
                    objflu.uqSampleID = objSampleLogin.Oid;
                    objflu.JobID = objSampleLogin.JobID.JobID;
                    objflu.SampleID = objSampleLogin.SampleID;
                    objflu.ElementName = "DW";
                    objflu.Blended = objSampleLogin.Blended;
                    objflu.Latitude = objSampleLogin.Latitude;
                    objflu.Longitude = objSampleLogin.Longitude;
                    objflu.PlannedLatitude = objSampleLogin.PlannedLatitude;
                    objflu.PlannedLongitude = objSampleLogin.PlannedLongitude;
                    //objflu.SampleName = objSampleLogin.SampleName;
                    objflu.CollectedBy = null;
                    if (objSampleLogin.JobID.ClientName != null)
                    {
                        objflu.ClientName = objSampleLogin.JobID.ClientName.DisplayName;
                    }
                    objflu.Temprature_C_ = objSampleLogin.Temp;
                    objflu.Humidity___ = objSampleLogin.Humidity;
                    objflu.ClientSampleID = objSampleLogin.ClientSampleID;
                    //objflu.SampleBottleID = "A";
                    objflu.TestSummary = objSampleLogin.FieldTestSummary;
                    objflu.ModifiedDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(objSampleLogin.AlternativeStationOid))
                    {
                        foreach (string guid in objSampleLogin.AlternativeStationOid.Split(new[] { "; " }, StringSplitOptions.None))
                        {
                            SampleSites sites = ObjectSpace.FindObject<SampleSites>(CriteriaOperator.Parse("[Oid]=?", new Guid(guid)));
                            if (sites != null)
                            {
                                insertalternatestation(uow, sites, objSampleLogin);
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

        private void insertalternatestation(UnitOfWork uow, SampleSites sites, Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin)
        {
            try
            {
                FlutterSDARRA_SamplingAlternateStation site = uow.FindObject<FlutterSDARRA_SamplingAlternateStation>(CriteriaOperator.Parse("[uqStationID]=? and [JobID]=?", sites.Oid, objSampleLogin.JobID.JobID));
                if (site == null)
                {
                    FlutterSDARRA_SamplingAlternateStation objsta = new FlutterSDARRA_SamplingAlternateStation(uow);
                    objsta.uqStationID = sites.Oid;
                    objsta.StationName = sites.SiteName;
                    objsta.StationID = sites.SiteID;
                    objsta.JobID = objSampleLogin.JobID.JobID;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void insertsample(IObjectSpace os, Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin)
        {
            try
            {
                FlutterSDARRA_FieldDataEntryInfo sample = os.FindObject<FlutterSDARRA_FieldDataEntryInfo>(CriteriaOperator.Parse("[SampleID]=?", objSampleLogin.SampleID));
                if (sample == null)
                {
                    FlutterSDARRA_FieldDataEntryInfo objflu = os.CreateObject<FlutterSDARRA_FieldDataEntryInfo>();
                    if (objSampleLogin.StationLocation != null)
                    {
                        objflu.uqStationID = objSampleLogin.StationLocation.Oid;
                        objflu.Station = objSampleLogin.StationLocation.SiteName;
                        objflu.StationID = objSampleLogin.StationLocation.SiteID;
                        insertalternatestation(os, objSampleLogin.StationLocation, objSampleLogin);
                    }
                    if (objSampleLogin.JobID.ProjectID != null)
                    {
                        objflu.ProjectID = objSampleLogin.JobID.ProjectID.ProjectId;
                        objflu.ProjectName = objSampleLogin.JobID.ProjectID.ProjectName;
                    }
                    objflu.uqSampleID = objSampleLogin.Oid;
                    objflu.JobID = objSampleLogin.JobID.JobID;
                    objflu.SampleID = objSampleLogin.SampleID;
                    objflu.ElementName = "DW";
                    objflu.Blended = objSampleLogin.Blended;
                    objflu.Latitude = objSampleLogin.Latitude;
                    objflu.Longitude = objSampleLogin.Longitude;
                    objflu.PlannedLatitude = objSampleLogin.PlannedLatitude;
                    objflu.PlannedLongitude = objSampleLogin.PlannedLongitude;
                    //objflu.SampleName = objSampleLogin.SampleName;
                    objflu.CollectedBy = null;
                    if (objSampleLogin.JobID.ClientName != null)
                    {
                        objflu.ClientName = objSampleLogin.JobID.ClientName.DisplayName;
                    }
                    objflu.Temprature_C_ = objSampleLogin.Temp;
                    objflu.Humidity___ = objSampleLogin.Humidity;
                    objflu.ClientSampleID = objSampleLogin.ClientSampleID;
                    //objflu.SampleBottleID = "A";
                    objflu.TestSummary = objSampleLogin.FieldTestSummary;
                    objflu.ModifiedDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(objSampleLogin.AlternativeStationOid))
                    {
                        foreach (string guid in objSampleLogin.AlternativeStationOid.Split(new[] { "; " }, StringSplitOptions.None))
                        {
                            SampleSites sites = ObjectSpace.FindObject<SampleSites>(CriteriaOperator.Parse("[Oid]=?", new Guid(guid)));
                            if (sites != null)
                            {
                                insertalternatestation(os, sites, objSampleLogin);
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

        private void insertalternatestation(IObjectSpace os, SampleSites sites, Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin)
        {
            try
            {
                FlutterSDARRA_SamplingAlternateStation site = os.FindObject<FlutterSDARRA_SamplingAlternateStation>(CriteriaOperator.Parse("[uqStationID]=? and [JobID]=?", sites.Oid, objSampleLogin.JobID.JobID));
                if (site == null)
                {
                    FlutterSDARRA_SamplingAlternateStation objsta = os.CreateObject<FlutterSDARRA_SamplingAlternateStation>();
                    objsta.uqStationID = sites.Oid;
                    objsta.StationName = sites.SiteName;
                    objsta.StationID = sites.SiteID;
                    objsta.JobID = objSampleLogin.JobID.JobID;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        //public void processsubmitteddata()
        //{
        //    try
        //    {
        //        IObjectSpace os = Application.CreateObjectSpace();
        //        foreach (FlutterSDARRA_FieldDataEntryInfo datas in os.GetObjects<FlutterSDARRA_FieldDataEntryInfo>(CriteriaOperator.Parse("[Issubmitted] = True And [SubmitProcessed] <> True")).ToList())
        //        {
        //            Modules.BusinessObjects.SampleManagement.SampleLogIn logIn = os.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[Oid]=?", datas.uqSampleID));
        //            if (logIn != null && logIn.JobID.Index.UqIndexID == 29 && logIn.SamplingStatus == SamplingStatus.PendingCompletion)
        //            {
        //                if (datas.Uncollected == true)
        //                {
        //                    logIn.SamplingStatus = SamplingStatus.Completed;
        //                    SampleStatus status = os.FindObject<SampleStatus>(CriteriaOperator.Parse("Samplestatus = 'Uncollected'"));
        //                    if (status != null)
        //                    {
        //                        foreach (SampleBottleAllocation bottles in os.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=?", logIn.Oid)).ToList())
        //                        {
        //                            bottles.SampleStatus = status;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (logIn.StationLocation.Oid != datas.uqStationID)
        //                    {
        //                        logIn.StationLocation = os.GetObjectByKey<SampleSites>(datas.uqStationID);
        //                    }
        //                    logIn.Latitude = datas.Latitude;
        //                    logIn.Longitude = datas.Longitude;
        //                    logIn.Temp = datas.StationTemperature;
        //                    logIn.Humidity = datas.Humidity___;
        //                    logIn.stationcommment = datas.StationComment;
        //                    logIn.EnteredDate = datas.CollectedDate;
        //                    logIn.LastUpdatedDate = datas.CollectedDate.ToString();
        //                    logIn.MonitoredDate = datas.CollectedDate;
        //                    logIn.CollectDate = datas.CollectedDate;
        //                    Collector collector = os.FindObject<Collector>(CriteriaOperator.Parse("[Oid]=?", datas.CollectedBy));
        //                    if (collector != null)
        //                    {
        //                        logIn.Collector = collector;
        //                        Employee empcollector = os.FindObject<Employee>(CriteriaOperator.Parse("[FirstName]=? and [LastName]=?", collector.FirstName, collector.LastName));
        //                        if (empcollector != null)
        //                        {
        //                            logIn.EnteredBy = empcollector;
        //                            logIn.LastUpdatedBy = empcollector.DisplayName;
        //                            logIn.MonitoredBy = empcollector;
        //                        }
        //                        else
        //                        {
        //                            Employee empfield = os.FindObject<Employee>(CriteriaOperator.Parse("[DisplayName]='Field'"));
        //                            if (empfield != null)
        //                            {
        //                                logIn.EnteredBy = empfield;
        //                                logIn.LastUpdatedBy = empfield.DisplayName;
        //                                logIn.MonitoredBy = empfield;
        //                            }
        //                        }
        //                    }
        //                    logIn.TestSummary = datas.TestSummary;
        //                    logIn.samplecomment = datas.SampleComment;

        //                    foreach (SampleParameter parameter in os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=? And [Testparameter.TestMethod.IsFieldTest] = True", logIn.Oid)).ToList())
        //                    {
        //                        XPMemberInfo sproperty;
        //                        var proname = collist.Where(a => a.Item1 == parameter.Testparameter.TestMethod.TestName.ToString()).Select(a => a.Item2).FirstOrDefault();
        //                        if (proname != null)
        //                        {
        //                            sproperty = datas.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name.ToUpper() == proname.Replace(" ", "").ToUpper()).FirstOrDefault();
        //                        }
        //                        else
        //                        {
        //                            sproperty = datas.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name.ToUpper() == parameter.Testparameter.TestMethod.TestName.Replace(" ", "").ToUpper()).FirstOrDefault();
        //                        }
        //                        if (sproperty != null && sproperty.GetValue(datas) != null)
        //                        {
        //                            parameter.Result = sproperty.GetValue(datas).ToString();
        //                        }
        //                    }
        //                    SamplingStatus status = SamplingStatus.PendingValidation;
        //                    IList<DefaultSetting> FDdefsetting = View.ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Sampling Management' And Not IsNullOrEmpty([NavigationItemName])"));
        //                    if (FDdefsetting != null && FDdefsetting.Count > 0)
        //                    {
        //                        DefaultSetting FD1defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "FieldDataReview1").FirstOrDefault();
        //                        DefaultSetting FD2defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "FieldDataReview2").FirstOrDefault();
        //                        if (FD1defsetting != null && FD2defsetting != null)
        //                        {
        //                            if (FD1defsetting.Select == false && FD2defsetting.Select == true)
        //                            {
        //                                status = SamplingStatus.PendingApproval;
        //                            }
        //                        }
        //                    }
        //                    logIn.SamplingStatus = status;
        //                }
        //            }
        //            datas.SubmitProcessed = true;
        //        }
        //        if (os.IsModified)
        //        {
        //            os.CommitChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
    }
}
