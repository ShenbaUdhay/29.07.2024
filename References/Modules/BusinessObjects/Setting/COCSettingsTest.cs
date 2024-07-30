using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]

    public class COCSettingsTest : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        //private string CurrentLanguage;
        curlanguage curlanguage = new curlanguage();

        public COCSettingsTest(Session session)
            : base(session)
        {
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            //SelectedData sproc = session.ExecuteSproc("getCurrentLanguage", "");
            //CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).  
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
        }

        #region CreatedDate

        private DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue<DateTime>("CreatedDate", ref _CreatedDate, value); }

        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue<Employee>("CreatedBy", ref _CreatedBy, value); }
        }
        #endregion

        #region SampleArea
        private string _SampleArea;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string SampleArea
        {

            get { return _SampleArea; }

            set { SetPropertyValue<string>("SampleArea", ref _SampleArea, value); }
        }
        #endregion

        #region FinalVolume
        private string _FinalVolume;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string FinalVolume
        {

            get { return _FinalVolume; }

            set { SetPropertyValue<string>("FinalVolume", ref _FinalVolume, value); }
        }
        #endregion

        #region ugsample
        private string _ugsample;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ugsample
        {

            get { return _ugsample; }

            set { SetPropertyValue<string>("ugsample", ref _ugsample, value); }
        }
        #endregion

        #region ugft
        private string _ugft;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ugft
        {

            get { return _ugft; }

            set { SetPropertyValue<string>("ugft", ref _ugft, value); }
        }
        #endregion

        #region ugmcube
        private string _ugmcube;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ugmcube
        {

            get { return _ugmcube; }

            set { SetPropertyValue<string>("ugmcube", ref _ugmcube, value); }
        }
        #endregion

        #region TWA
        private string _TWA;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string TWA
        {

            get { return _TWA; }

            set { SetPropertyValue<string>("TWA", ref _TWA, value); }
        }
        #endregion

        #region FinalRptLimit
        private string _FinalRptLimit;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string FinalRptLimit
        {

            get { return _FinalRptLimit; }

            set { SetPropertyValue<string>("FinalRptLimit", ref _FinalRptLimit, value); }
        }
        #endregion

        #region FinalRptLimitpercentage
        private string _FinalRptLimitpercentage;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string FinalRptLimitpercentage
        {

            get { return _FinalRptLimitpercentage; }

            set { SetPropertyValue<string>("FinalRptLimitpercentage", ref _FinalRptLimitpercentage, value); }
        }
        #endregion

        #region COCSettingsSamples      
        private COCSettingsSamples _COCSettingsSamples;
        [Association]
        public COCSettingsSamples COCSettingsSamples
        {
            get { return _COCSettingsSamples; }
            set { SetPropertyValue("COCSettingsSamples", ref _COCSettingsSamples, value); }
        }
        #endregion

        #region TestParameter
        private Testparameter _Testparameter;
        [Association]
        public Testparameter Testparameter
        {
            get { return _Testparameter; }
            set { SetPropertyValue("Testparameter", ref _Testparameter, value); }
        }
        #endregion TestParameter

        #region Result

        private string _Result;
        //[ImmediatePostData]
        public string Result
        {
            get
            {
                if (_Result == null && Testparameter != null /*&& SignOff == true*/)
                {

                    return Testparameter.DefaultResult;
                }
                return _Result;
            }
            set
            {
                SetPropertyValue("Result", ref _Result, value);
                //OnChanged("ResultNumeric");
            }
        }
        #endregion Result

        #region EnteredDate
        private DateTime? _EnteredDate;
        public DateTime? EnteredDate
        {
            get
            {
                return _EnteredDate;
            }
            set
            {
                SetPropertyValue<DateTime?>("EnteredDate", ref _EnteredDate, value);
            }
        }
        #endregion

        #region EnteredBy
        private Employee _EnteredBy;
        public Employee EnteredBy
        {
            get
            { //_EnteredBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _EnteredBy;
            }
            set
            {
                SetPropertyValue<Employee>("EnteredBy", ref _EnteredBy, value);
            }

        }


        #endregion

        #region Units
        private Unit _Units;
        public Unit Units
        {
            get
            {
                if ((_Units == null) && (_Testparameter != null))
                {
                    return Testparameter.DefaultUnits;
                }
                return _Units;
            }
            set { SetPropertyValue<Unit>("Units", ref _Units, value); }
        }
        //private string _Units;
        //public string Units
        //{
        //    get { return _Units; }
        //    set { SetPropertyValue<string>("Units", ref _Units, value); }
        //}

        #endregion

        #region DF
        private string _DF;
        public string DF
        {
            get { return _DF; }

            set { SetPropertyValue<string>("DF", ref _DF, value); }
        }

        #endregion

        #region ResultNumeric        
        private string _ResultNumeric;
        //[ImmediatePostData(true)]
        public string ResultNumeric
        {

            get { return _ResultNumeric; }
            set { SetPropertyValue<string>("ResultNumeric", ref _ResultNumeric, value); }
        }
        #endregion       

        #region Rec
        private string _Rec;
        public string Rec
        {
            get { return _Rec; }

            set { SetPropertyValue<string>("Rec", ref _Rec, value); }
        }
        #endregion

        #region RPD
        private string _RPD;
        public string RPD
        {
            get { return _RPD; }

            set { SetPropertyValue<string>("RPD", ref _RPD, value); }
        }
        #endregion

        #region MDL
        private string _MDL;
        public string MDL
        {
            get
            {
                if (_MDL == null && Testparameter != null)
                {
                    return Testparameter.MDL;
                }
                return _MDL;
            }

            set { SetPropertyValue<string>("MDL", ref _MDL, value); }
        }
        #endregion

        #region RptLimit
        private string _RptLimit;
        public string RptLimit
        {
            get
            {
                if (_RptLimit == null && Testparameter != null)
                {
                    return Testparameter.RptLimit;
                }
                return _RptLimit;
            }
            set { SetPropertyValue<string>("RptLimit", ref _RptLimit, value); }
        }
        #endregion RptLimit

        #region AssignedValue
        private string _AssignedValue;
        public string AssignedValue
        {
            get { return _AssignedValue; }

            set { SetPropertyValue<string>("AssignedValue", ref _AssignedValue, value); }
        }
        #endregion AssignedValue

        #region IsExported
        private bool _IsExported;
        public bool IsExported
        {
            get { return _IsExported; }
            set { SetPropertyValue<bool>("IsExported", ref _IsExported, value); }
        }
        #endregion

        public enum Priority
        {
            [ImageName("State_Priority_Low")]
            Low = 0,
            [ImageName("State_Priority_Normal")]
            Normal = 1,
            [ImageName("State_Priority_High")]
            High = 2
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestGuide> TestGuides
        {
            get
            {
                if (Testparameter != null && Testparameter.TestMethod != null && Testparameter.TestMethod.TestGuides != null)
                {
                    XPCollection<TestGuide> listGuides = new XPCollection<TestGuide>(Session, CriteriaOperator.Parse("[TestMethod] = ? ", Testparameter.TestMethod.Oid));
                    return listGuides;
                }
                else
                {
                    return null;
                }
            }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Preservative> Preservatives
        {
            get
            {
                if (TestGuides != null && TestGuides.Count > 0)
                {
                    XPCollection<Preservative> lstpreservative = new XPCollection<Preservative>(Session);
                    IList<Guid> ListOid = TestGuides.Where(i => i.Preservative != null).Select(i => i.Preservative.Oid).ToList();
                    lstpreservative.Criteria = new InOperator("Oid", ListOid);
                    return lstpreservative;
                }
                else
                {
                    return null;
                }
            }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Modules.BusinessObjects.Setting.Container> ContainerTypes
        {
            get
            {
                if (TestGuides != null && TestGuides.Count > 0)
                {
                    XPCollection<Modules.BusinessObjects.Setting.Container> lstContainer = new XPCollection<Modules.BusinessObjects.Setting.Container>(Session);
                    IList<Guid> ListOid = TestGuides.Where(i => i.Container != null).Select(i => i.Container.Oid).ToList();
                    lstContainer.Criteria = new InOperator("Oid", ListOid);
                    return lstContainer;
                }
                else
                {
                    return null;
                }
            }
        }

        private Preservative _Preservative;
        //  [DataSourceProperty("Preservatives")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Preservative Preservative
        {
            get
            {
                return _Preservative;
            }
            set { SetPropertyValue("Preservative", ref _Preservative, value); }
        }

        private Modules.BusinessObjects.Setting.Container _ContainerType;
        //   [DataSourceProperty("ContainerTypes")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Modules.BusinessObjects.Setting.Container ContainerType
        {
            get
            {

                return _ContainerType;
            }
            set { SetPropertyValue("ContainerType", ref _ContainerType, value); }
        }

        private uint _Container;
        [XafDisplayName("#Container")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public uint Container
        {
            get { return _Container; }
            set { SetPropertyValue("Container", ref _Container, value); }
        }

        private string _HoldTime;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string HoldTime
        {
            get
            {

                if (TestGuides != null && TestGuides.Count > 1)
                {
                    string minValue = TestGuides.Where(i => i.HoldingTimeBeforePrep != null && i.HoldingTimeBeforePrep.HoldingTime != null).Select(i => i.HoldingTimeBeforePrep.HoldingTime).Min();
                    if (minValue != null)
                    {
                        _HoldTime = TestGuides.Where(i => i.HoldingTimeBeforePrep != null && i.HoldingTimeBeforePrep.HoldingTime != null && i.HoldingTimeBeforePrep.HoldingTime == minValue).Select(i => i.HoldingTimeBeforePrep.HoldingTime).FirstOrDefault();
                    }
                }
                {
                    if (TestGuides != null && TestGuides.Count == 1)
                    {
                        _HoldTime = TestGuides.Where(i => i.HoldingTimeBeforePrep != null && i.HoldingTimeBeforePrep.HoldingTime != null).Select(i => i.HoldingTimeBeforePrep.HoldingTime).FirstOrDefault();
                    }
                }
                return _HoldTime;
            }
            set { SetPropertyValue("HoldTime", ref _HoldTime, value); }
        }

        #region IsGroup
        private bool _IsGroup;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public bool IsGroup
        {
            get { return _IsGroup; }
            set { SetPropertyValue("IsGroup", ref _IsGroup, value); }
        }
        #endregion

        #region GroupTest
        private GroupTestMethod _GroupTest;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public GroupTestMethod GroupTest
        {
            get { return _GroupTest; }
            set { SetPropertyValue("GroupTest", ref _GroupTest, value); }
        }
        #endregion

        #region ClientName
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string ClientName
        {
            get
            {
                if (COCSettingsSamples != null && COCSettingsSamples.COCID != null && COCSettingsSamples.COCID.ClientName != null)
                {
                    return COCSettingsSamples.COCID.ClientName.CustomerName;
                }
                //else
                //if (QCBatchID != null && QCBatchID.SampleID != null && QCBatchID.SampleID.JobID != null && QCBatchID.SampleID.JobID.ClientName != null)
                //{
                //    return QCBatchID.SampleID.JobID.ClientName.CustomerName;
                //}
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion 

        private int _SuboutSamplesCount;
        [VisibleInListView(false)]
        [XafDisplayName("#Samples")]
        [VisibleInDetailView(false)]
        [NonPersistent]
        [VisibleInLookupListView(false)]
        public int SuboutSamplesCount
        {
            get
            {
                if (COCSettingsSamples != null && COCSettingsSamples.COCID != null && !string.IsNullOrEmpty(COCSettingsSamples.COCID.strCOCID) && Testparameter != null && Testparameter.TestMethod != null
                     && Testparameter.TestMethod.TestName != null && Testparameter.TestMethod.MatrixName != null && !string.IsNullOrEmpty(Testparameter.TestMethod.MatrixName.MatrixName)
                     && Testparameter.TestMethod.MethodName != null && !string.IsNullOrEmpty(Testparameter.TestMethod.MethodName.MethodName) && !string.IsNullOrEmpty(Testparameter.TestMethod.MethodName.MethodNumber))
                {
                    _SuboutSamplesCount = Convert.ToInt32(Session.Evaluate(typeof(COCSettingsTest), CriteriaOperator.Parse("Count()"),
                        CriteriaOperator.Parse("/*[Samplelogin.JobID.JobID]=? And */[SuboutSample] Is Null And [Testparameter.TestMethod.MatrixName.MatrixName]=? And" +
                        " [Testparameter.TestMethod.TestName]=? And /*[Testparameter.TestMethod.MethodName.MethodName]=? And*/ " +
                        "[Testparameter.TestMethod.MethodName.MethodNumber]=? And [SubOut] = True",
                        /*Samplelogin.JobID.JobID,*/ Testparameter.TestMethod.MatrixName.MatrixName, Testparameter.TestMethod.TestName,
                       /* Testparameter.TestMethod.MethodName.MethodName,*/ Testparameter.TestMethod.MethodName.MethodNumber)));
                }
                return _SuboutSamplesCount;
            }
            set { SetPropertyValue("SuboutSamplesCount", ref _SuboutSamplesCount, value); }
        }

        private int _SuboutSamplesClients;
        [VisibleInListView(false)]
        [XafDisplayName("#Clients")]
        [VisibleInDetailView(false)]
        [NonPersistent]
        [VisibleInLookupListView(false)]
        public int SuboutSamplesClients
        {
            get
            {
                if (Testparameter != null && Testparameter.TestMethod != null)
                {
                    XPCollection<COCSettingsTest> lstSamples = new XPCollection<COCSettingsTest>(Session);
                    lstSamples.Criteria = CriteriaOperator.Parse("[COCSettingsSamples] Is Not Null And [COCSettingsSamples.COCID] Is Not Null And [COCSettingsSamples.COCID.ClientName] Is Not Null And [SuboutSample] Is Null And [SubOut] = True And [Testparameter.TestMethod.Oid] = ?",
                         Testparameter.TestMethod.Oid);
                    if (lstSamples != null && lstSamples.Count > 0)
                    {
                        _SuboutSamplesClients = lstSamples.Where(i => i.COCSettingsSamples != null && i.COCSettingsSamples.COCID != null && i.COCSettingsSamples.COCID.ClientName != null).Select(i => i.COCSettingsSamples.COCID.ClientName.Oid).Distinct().Count();
                    }
                }
                return _SuboutSamplesClients;
            }
            set { SetPropertyValue("SuboutSamplesClients", ref _SuboutSamplesClients, value); }
        }

        //#region SuboutSample

        //private SubOutSampleRegistrations _SuboutSample;
        //[Association("SubOutSampleTest")]
        //public SubOutSampleRegistrations SuboutSample
        //{
        //    get { return _SuboutSample; }
        //    set { SetPropertyValue<SubOutSampleRegistrations>("SuboutSample", ref _SuboutSample, value); }
        //}
        //#endregion

        #region SubOut
        private bool _subOut;

        public bool SubOut
        {
            get { return _subOut; }
            set { SetPropertyValue("SubOut", ref _subOut, value); }
        }
        #endregion

        #region SubLabName
        private SubContractLab _SubLabName;
        public SubContractLab SubLabName
        {
            get { return _SubLabName; }
            set { SetPropertyValue<SubContractLab>("SubLabName", ref _SubLabName, value); }
        }
        #endregion

        #region SubOutBy
        private Employee _SubOutBy;
        public Employee SubOutBy
        {
            get { return _SubOutBy; }
            set { SetPropertyValue<Employee>("SubOutBy", ref _SubOutBy, value); }
        }
        #endregion

        #region SubOutDate 
        private DateTime? _SubOutDate;
        public DateTime? SubOutDate
        {
            get { return _SubOutDate; }
            set { SetPropertyValue<DateTime?>("SubOutDate", ref _SubOutDate, value); }
        }
        #endregion

        private QCType _SuboutQCType;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public QCType SuboutQCType
        {
            get { return _SuboutQCType; }
            set { SetPropertyValue("SuboutQCType", ref _SuboutQCType, value); }
        }

        #region Settings
        //[Browsable(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInDashboards(false)]
        [NonPersistent]
        public COCSettings Settings
        {
            get
            {
                if (COCSettingsSamples != null)
                {
                    return COCSettingsSamples.COCID;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        ////private COCBottleSetup _Bottle;
        ////[VisibleInListView(false)]
        ////[VisibleInDetailView(false)]
        ////[VisibleInLookupListView(false)]
        ////public COCBottleSetup Bottle
        ////{
        ////    get { return _Bottle; }
        ////    set { SetPropertyValue("Bottle", ref _Bottle, value); }
        ////}

        private int _DupBottle;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public int DupBottle
        {
            get { return _DupBottle; }
            set { SetPropertyValue("DupBottle", ref _DupBottle, value); }
        }


        ////#region TestGroup
        ////private GroupTest _TestGroup;
        ////public GroupTest TestGroup
        ////{
        ////    get
        ////    {
        ////        return _TestGroup;
        ////    }
        ////    set
        ////    {
        ////        SetPropertyValue<GroupTest>("TestGroup", ref _TestGroup, value);
        ////    }
        ////}
        ////#endregion

        [NonPersistent]
        public string Parent
        {
            get
            {
                if (COCSettingsSamples != null && Testparameter != null)
                {
                    if (Testparameter.TestMethod != null)
                    {
                        if (curlanguage.strcurlanguage == "En")
                        {
                            if (Testparameter.IsGroup == false)
                            {
                                return string.Format("SampleID:{0} Martix:{1} Test:{2} Method:{3}: Component:{4}:", COCSettingsSamples.SampleID, Testparameter.TestMethod.MatrixName.MatrixName, Testparameter.TestMethod.TestName, Testparameter.TestMethod.MethodName.MethodNumber, Testparameter.Component.Components);
                            }
                            else
                            {
                                return string.Format("SampleID:{0} Martix:{1} Test:{2} Method:{3}: Component:{4}:", COCSettingsSamples.SampleID, Testparameter.TestMethod.MatrixName.MatrixName, Testparameter.TestMethod.TestName, Testparameter.TestMethod.MethodName, Testparameter.Component);
                            }
                        }
                        else
                        {
                            return string.Format("样品编号:{0} 基质:{1} 监测项目:{2} 方法:{3}:", COCSettingsSamples.SampleID, Testparameter.TestMethod.MatrixName.MatrixName, Testparameter.TestMethod.TestName, Testparameter.TestMethod.MethodName.MethodName);
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }


            }
            //set { SetPropertyValue("MatrixTestMethod", ref _TestGroup, value); }
        }

        //#region MethodName
        //private string _MethodName;
        //public string MethodName
        //{
        //    get { return _MethodName; }
        //    set { SetPropertyValue<string>(nameof(MethodName), ref _MethodName, value); }
        //}
        //#endregion

        //#region MethodNumber
        //private string _MethodNumber;
        //public string MethodNumber
        //{
        //    get { return _MethodNumber; }
        //    set { SetPropertyValue<string>(nameof(MethodNumber), ref _MethodNumber, value); }
        //}
        //#endregion


    }
}