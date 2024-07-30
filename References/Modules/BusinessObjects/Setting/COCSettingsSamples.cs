using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.Setting.SamplesSite;
using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class COCSettingsSamples : BaseObject, ICheckedListBoxItemsProvider
    {
        COCSettingsSampleInfo objCOCSampleinfo = new COCSettingsSampleInfo();
        viewInfo strviewid = new viewInfo();

        public COCSettingsSamples(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            EnteredBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            EnteredDate = Library.GetServerTime(Session);
            Qty = 1;
            Containers = 1;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            //SampleID = SampleID.Remove(0, 9);
            //SampleNo = SampleID.Remove(0, 10);
            base.OnSaving();
            LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            LastUpdatedDate = Library.GetServerTime(Session);
            if (!string.IsNullOrEmpty(SampleWeight) && string.IsNullOrEmpty(RetainedWeight))
            {
                RetainedWeight = SampleWeight;
            }
            
            if (string.IsNullOrEmpty(ClientSampleID))
            {
                ClientSampleID = "Sample" + SampleNo.ToString();
            }
        }

        #region COCSettings
        private COCSettings _COCID;
        public COCSettings COCID
        {
            get { return _COCID; }
            set { SetPropertyValue<COCSettings>(nameof(COCID), ref _COCID, value); }
        }
        #endregion
        //#region COC
        //private COCSettings _COC;
        //public COCSettings COC
        //{
        //    get { return _COC; }
        //    set { SetPropertyValue<COCSettings>(nameof(COC), ref _COC, value); }
        //}
        //#endregion

        #region SampleID
        private string _SampleID;
        [Appearance("SampleID", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        [ImmediatePostData]
        public string SampleID
        {
            get
            {
                if (SampleNo > 0)
                {
                    if (objCOCSampleinfo.SampleIDDigit == SampleIDDigit.Three)
                    {
                        if (SampleNo.ToString().Length == 1)
                        {
                            return string.Format("{0}{1}{2}", COCID, "-00", SampleNo.ToString());
                        }
                        else if (SampleNo.ToString().Length == 2)
                        {
                            return string.Format("{0}{1}{2}", COCID, "-0", SampleNo.ToString());
                        }
                        else
                        {
                            return string.Format("{0}{1}{2}", COCID, "-", SampleNo.ToString());
                        }
                    }
                    else
                    {
                        if (SampleNo.ToString().Length == 1)
                        {
                            return string.Format("{0}{1}{2}", COCID, "-0", SampleNo.ToString());
                        }
                        else
                        {
                            return string.Format("{0}{1}{2}", COCID, "-", SampleNo.ToString());
                        }
                    }

                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                SetPropertyValue<string>("SampleID", ref _SampleID, value);
            }
        }
        #endregion

        #region SequenceTestSampleID
        private string _SequenceTestSampleID;
        [NonPersistent]
        [XafDisplayName("SampleID")]
        [VisibleInDetailView(false)]
        public string SequenceTestSampleID
        {
            get
            {
                return _SequenceTestSampleID;
            }
            set
            {
                SetPropertyValue<string>("SequenceTestSampleID", ref _SequenceTestSampleID, value);
            }
        }
        #endregion

        #region SequenceTestSortNo
        private int _SequenceTestSortNo;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public int SequenceTestSortNo
        {
            get
            {
                return _SequenceTestSortNo;
            }
            set
            {
                SetPropertyValue<int>("SequenceTestSortNo", ref _SequenceTestSortNo, value);
            }
        }
        #endregion

        #region SampleNo
        private int _SampleNo;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ImmediatePostData]
        public int SampleNo
        {
            get
            {
                return _SampleNo;
            }
            set
            {
                SetPropertyValue<int>("SampleNo", ref _SampleNo, value);
            }
        }
        #endregion

        #region ClientSampleID
        private string _ClientSampleID;
        [Appearance("ClientSampleID", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        public string ClientSampleID
        {
            get { return _ClientSampleID; }
            set { SetPropertyValue<string>(nameof(ClientSampleID), ref _ClientSampleID, value); }
        }
        #endregion

        #region VisualMatrix
        private VisualMatrix _VisualMatrix;
        [ImmediatePostData]
        [DataSourceProperty("VisualMatrixs")]
        public VisualMatrix VisualMatrix
        {
            get
            {
                return _VisualMatrix;
            }
            set { SetPropertyValue<VisualMatrix>("VisualMatrix", ref _VisualMatrix, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<VisualMatrix> VisualMatrixs
        {
            get
            {
                if (COCID != null && !string.IsNullOrEmpty(COCID.SampleMatries))
                {
                    List<Guid> ids = COCID.SampleMatries.Split(';').Select(i => new Guid(i)).ToList();
                    return new XPCollection<VisualMatrix>(Session, new InOperator("Oid", ids));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region SampleType
        private SampleType _SampleType;
        [Appearance("SampleType3", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        public SampleType SampleType
        {
            get { return _SampleType; }
            set { SetPropertyValue<SampleType>(nameof(SampleType), ref _SampleType, value); }
        }
        #endregion

        #region SiteName
        //private SampleSites _SiteName;
        ////[DataSourceProperty("SampleSitesDataSource")]
        //[ImmediatePostData(true)]
        //public SampleSites SiteName
        //{
        //    get { return _SiteName; }
        //    set { SetPropertyValue<SampleSites>(nameof(SiteName), ref _SiteName, value); }
        //}
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<SampleSites> SampleSitesDataSource
        {
            get
            {
                if (COCID != null)
                {
                    XPCollection<SampleSites> listprojects = new XPCollection<SampleSites>(Session, CriteriaOperator.Parse("[Client.Oid] = ? ", COCID.ClientName.Oid));
                    return listprojects;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        //#region SiteName
        //private SampleSites _SiteName;
        ////[DataSourceProperty("SampleSitesDataSource")]
        //[ImmediatePostData(true)]
        //public SampleSites SiteName
        //{
        //    get { return _SiteName; }
        //    set { SetPropertyValue<SampleSites>(nameof(SiteName), ref _SiteName, value); }
        //}
        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<SampleSites> SampleSitesDataSource
        //{
        //    get
        //    {
        //        if (COC != null)
        //        {
        //            XPCollection<SampleSites> listprojects = new XPCollection<SampleSites>(Session, CriteriaOperator.Parse("[Client.Oid] = ? ", COC.Oid));
        //            return listprojects;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        //#endregion

        #region Qty
        private uint _Qty;
        //[Appearance("Qty", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        //[RuleRange(1, uint.MaxValue, CustomMessageTemplate = "Qty must be greater than 0")]
        public uint Qty
        {
            get { return _Qty; }
            set { SetPropertyValue<uint>("Qty", ref _Qty, value); }
        }
        #endregion

        #region Storage
        private Storage _Storage;
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        [Appearance("Storage", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        public Storage Storage
        {
            get { return _Storage; }
            set { SetPropertyValue<Storage>("Storage", ref _Storage, value); }
        }
        #endregion

        #region Preservetives
        private string _Preservetives;
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        [Appearance("Preservetives", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        public string Preservetives
        {
            get { return _Preservetives; }
            set { SetPropertyValue<string>("Preservetives", ref _Preservetives, value); }
        }
        #endregion

        #region SamplingLocation
        private string _SamplingLocation;
        //[ImmediatePostData(true)]
        [Appearance("SamplingLocation", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        public string SamplingLocation
        {
            get
            {
                return _SamplingLocation;
            }
            set { SetPropertyValue<string>("SamplingLocation", ref _SamplingLocation, value); }
        }
        #endregion

        #region QCType
        private QCType _QCType;
        [ImmediatePostData(true)]
        [Appearance("QCType3", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        public QCType QCType
        {
            get { return _QCType; }
            set { SetPropertyValue<QCType>("QCType", ref _QCType, value); }
        }
        #endregion

        #region QCSource
        private COCSettingsSamples _QCSource;
        [Appearance("QCSource2", Enabled = false, Criteria = "QCType.QCSource.QCTypeName <> 'Sample'", Context = "DetailView")]
        public COCSettingsSamples QCSource
        {
            get { return _QCSource; }
            set { SetPropertyValue("QCSource", ref _QCSource, value); }
        }
        #endregion

        #region Collector
        private Collector _Collector;
        [Appearance("Collector2", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        [DataSourceProperty(nameof(CollectorDataSource))]
        public Collector Collector
        {
            get
            {
                return _Collector;
            }
            set { SetPropertyValue<Collector>("Collector", ref _Collector, value); }
        }
        #region CollectorPhone
        private string _CollectorPhone;
        public string CollectorPhone
        {
            get
            {
                return _CollectorPhone;
            }
            set { SetPropertyValue(nameof(CollectorPhone), ref _CollectorPhone, value); }
        }
        #endregion
        [NonPersistent]
        public XPCollection<Collector> CollectorDataSource
        {
            get
            {
                if (COCID != null && COCID.ClientName != null)
                {
                    return new XPCollection<Collector>(Session, CriteriaOperator.Parse("[CustomerName] Is Null or [CustomerName.Oid] = ?", COCID.ClientName.Oid));
                }
                else
                {
                    return new XPCollection<Collector>(Session, CriteriaOperator.Parse("[CustomerName] Is Null"));
                }
            }
        }
        #endregion

        #region CollectDate
        //private DateTime _CollectDate;
        //public DateTime CollectDate
        //{
        //    get { return _CollectDate; }
        //    set { SetPropertyValue<DateTime>("CollectDate", ref _CollectDate, value); }
        //}
        #endregion

        #region CollectTime
        //private TimeSpan? ConverTextToStoredValue(string value)
        //{
        //    if (value != null)
        //    {
        //        TimeSpan? time = TimeSpan.Parse(value);
        //        return time;
        //    }
        //    return TimeSpan.Zero;
        //}

        //private string ConvertStoredValueToText(TimeSpan? collectTime)
        //{
        //    if (collectTime != null && collectTime != TimeSpan.Zero)
        //    {
        //        string strTime = collectTime.Value.ToString(@"hh\:mm");
        //        return strTime;
        //    }
        //    return null;
        //}

        #region CollectTime
        //private TimeSpan? _CollectTime;
        ////[ModelDefault("EditMaskType", "DateTime")]
        ////[ModelDefault("EditMask", @"H:mm")]
        ////[ModelDefault("DisplayFormat", "{0:h\\:mm}")]
        //public TimeSpan? CollectTime
        //{
        //    get { return _CollectTime; }
        //    set { SetPropertyValue("CollectTime", ref _CollectTime, value); }
        //}
        #endregion
        #endregion

        #region FlowRate
        private string _FlowRate;
        [Appearance("FlowRate", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        //[ImmediatePostData]
        [Size(int.MaxValue)]
        public string FlowRate
        {
            get { return _FlowRate; }
            set { SetPropertyValue<string>("FlowRate", ref _FlowRate, value); }
        }
        #endregion

        #region Length
        private string _Length;
        [Appearance("Length", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        //[ImmediatePostData]
        public string Length
        {
            get { return _Length; }
            set { SetPropertyValue<string>("Length", ref _Length, value); }
        }
        #endregion

        #region Width
        private string _Width;
        [Appearance("Width", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        //[ImmediatePostData]
        public string Width
        {
            get { return _Width; }
            set { SetPropertyValue<string>("Width", ref _Width, value); }
        }
        #endregion

        #region TimeStart
        private Nullable<DateTime> _TimeStart;
        [Appearance("TimeStart", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        //[ImmediatePostData]
        public Nullable<DateTime> TimeStart
        {
            get { return _TimeStart; }
            set { SetPropertyValue<Nullable<DateTime>>("TimeStart", ref _TimeStart, value); }
        }
        #endregion

        #region TimeEnd
        private Nullable<DateTime> _TimeEnd;
        [Appearance("TimeEnd", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        //[ImmediatePostData]
        public Nullable<DateTime> TimeEnd
        {
            get { return _TimeEnd; }
            set { SetPropertyValue<Nullable<DateTime>>("TimeEnd", ref _TimeEnd, value); }
        }
        #endregion

        #region StartFlow
        private string _StartFlow;
        [Appearance("StartFlow(lpm)", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        [ImmediatePostData]
        public string StartFlow
        {
            get { return _StartFlow; }
            set { SetPropertyValue<string>("StartFlow", ref _StartFlow, value); }
        }
        #endregion

        #region EndFlow
        private string _EndFlow;
        [Appearance("EndFlow(lpm)", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        [ImmediatePostData]
        public string EndFlow
        {
            get { return _EndFlow; }
            set { SetPropertyValue<string>("EndFlow", ref _EndFlow, value); }
        }
        #endregion

        #region Volume
        private string _Volume;
        [Appearance("Volume", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        //[ImmediatePostData]
        //[Size(int.MaxValue)]
        public string Volume
        {
            get { return _Volume; }
            set { SetPropertyValue<string>("Volume", ref _Volume, value); }
        }
        #endregion

        #region Collection
        [ManyToManyAlias("COCSettingsTests", "Testparameter")]
        public IList<Testparameter> Testparameters
        {
            get
            {
                return GetList<Testparameter>("Testparameters");
            }
            //set
            //{
            //    return _testparameters;
            //}
        }

        [Association, Browsable(false)]
        public IList<COCSettingsTest> COCSettingsTests
        {
            get
            {
                return GetList<COCSettingsTest>("COCSettingsTests");
            }
        }
        #endregion

        #region ProjectID
        private Project _ProjectID;
        [NonPersistent]
        [ImmediatePostData]
        [Appearance("COCProjectID", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        [ReadOnly(false)]
        public Project ProjectID
        {
            get
            {
                return _ProjectID;
            }
            set
            {
                SetPropertyValue<Project>("ProjectID", ref _ProjectID, value);
            }
        }
        #endregion

        #region ProjectName
        private string _ProjectName;
        [NonPersistent]
        [ImmediatePostData]
        [Appearance("COCProjectName", Enabled = false, Context = "DetailView")]
        public string ProjectName
        {
            get { return _ProjectName; }
            set { SetPropertyValue<string>("ProjectName", ref _ProjectName, value); }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        [Appearance("MB10", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get
            {
                return fModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref fModifiedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime fModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        [Appearance("MD10", Enabled = false, Context = "DetailView")]
        public DateTime ModifiedDate
        {
            get
            {
                return fModifiedDate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref fModifiedDate, value);
            }
        }
        #endregion

        #region Comment
        private string fComment;

        [Size(1000)]
        //[Browsable(false)]
        public string Comment
        {
            get
            {
                return fComment;
            }
            set
            {
                SetPropertyValue("Comment", ref fComment, value);
            }
        }


        #endregion

        #region Time
        private string _Time;
        [Appearance("Time", Enabled = false, Criteria = "COCID IS NULL", Context = "DetailView")]
        //[ImmediatePostData]
        [Size(int.MaxValue)]
        public string Time
        {
            get { return _Time; }
            set { SetPropertyValue<string>("Time", ref _Time, value); }
        }
        #endregion

        #region Longitude
        private string _Longitude;
        public string Longitude
        {
            get
            {
                return _Longitude;
            }
            set { SetPropertyValue("Longitude", ref _Longitude, value); }
        }
        #endregion

        #region Latitude
        private string _Latitude;
        public string Latitude
        {
            get
            {
                return _Latitude;
            }
            set { SetPropertyValue("Latitude", ref _Latitude, value); }
        }
        #endregion

        #region SubOut
        private bool _SubOut;
        public bool SubOut
        {
            get
            {
                return _SubOut;
            }
            set { SetPropertyValue<bool>(nameof(SubOut), ref _SubOut, value); }
        }
        #endregion

        #region Test
        private bool _Test;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public bool Test
        {
            get { return _Test; }
            set { SetPropertyValue("Test", ref _Test, value); }
        }

        #endregion

        //#region Source
        //private COCSettingsSamples _SettingsSource;
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //public COCSettingsSamples SettingsSource
        //{
        //    get { return _SettingsSource; }
        //    set { SetPropertyValue<COCSettingsSamples>(nameof(SettingsSource), ref _SettingsSource, value); }
        //}
        //#endregion

        #region COntainers
        uint _containers;
        [DevExpress.Xpo.DisplayName("#Containers")]
        public uint Containers
        {
            get => _containers;
            set => SetPropertyValue(nameof(Containers), ref _containers, value);
        }
        #endregion

        #region DaysKept
        //private int _DaysKept;
        //public int DaysKept
        //{
        //    get
        //    {
        //        if (CollectDate != null && CollectDate != DateTime.MinValue)
        //        {
        //            //TimeSpan diff = DateTime.Today - Convert.ToDateTime(CollectDate.ToString("MM/dd/yyyy"));
        //            TimeSpan diff = DateTime.Today - CollectDate;
        //            _DaysKept = diff.Days;
        //        }
        //        else
        //        if (ModifiedDate != null && ModifiedDate != DateTime.MinValue)
        //        {
        //            //TimeSpan diff = DateTime.Today - Convert.ToDateTime(ModifiedDate.ToString("MM/dd/yyyy"));
        //            TimeSpan diff = DateTime.Today - ModifiedDate;
        //            _DaysKept = diff.Days;
        //        }
        //        return _DaysKept;
        //    }
        //}
        #endregion

        #region StorageID
        private Storage _StorageID;
        public Storage StorageID
        {
            get
            {
                return _StorageID;
            }
            set
            {
                SetPropertyValue<Storage>(nameof(StorageID), ref _StorageID, value);
            }
        }
        #endregion

        #region PreserveCondition
        private PreserveCondition _PreserveCondition;
        public PreserveCondition PreserveCondition
        {
            get
            {
                return _PreserveCondition;
            }
            set
            {
                SetPropertyValue<PreserveCondition>(nameof(PreserveCondition), ref _PreserveCondition, value);
            }
        }
        #endregion

        #region ContainerID
        public string ContainerID
        {
            get
            {
                return "";
            }
        }
        #endregion

        #region CanChangeVisualMatrix
        [ImmediatePostData]
        public string CanChangeVisualMatrix
        {
            get
            {
                if (Testparameters != null && Testparameters.Count > 0)
                {
                    //return "CannotChange";
                    if (Convert.ToInt32(Session.Evaluate(typeof(COCSettingsTest), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[COCSettingsSamples.Oid] = ?", Oid))) > 0)
                    {
                        return "CannotChange";
                    }
                    else
                    {
                        return "DeleteSampleParamsAndChange";
                    }
                }
                else
                {
                    return "Change";
                }
            }
        }
        #endregion

        #region PurifierSampleID
        private string _PurifierSampleID;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        //[DevExpress.Persistent.Validation.RuleRequiredField]
        public string PurifierSampleID
        {
            get { return _PurifierSampleID; }
            set { SetPropertyValue(nameof(PurifierSampleID), ref _PurifierSampleID, value); }
        }
        #endregion

        #region SamplePretreatmentBatchID
        private SamplePretreatmentBatchSequence _SamplePretreatmentBatchID;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public SamplePretreatmentBatchSequence SamplePretreatmentBatchID
        {
            get
            {
                return _SamplePretreatmentBatchID;
            }
            set
            {
                SetPropertyValue<SamplePretreatmentBatchSequence>(nameof(SamplePretreatmentBatchID), ref _SamplePretreatmentBatchID, value);
            }
        }
        #endregion

        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> properties = new Dictionary<object, string>();
            //if (targetMemberName == "PurifierSampleID" && PurifierSampleIDDataSource != null && PurifierSampleIDDataSource.Count > 0)
            //{
            //    foreach (IndoorInspectionSamples objsample in PurifierSampleIDDataSource.OrderBy(a => a.PurifierSampleID).ToList())
            //    {
            //        if (!properties.ContainsKey(objsample.Oid))
            //        {
            //            properties.Add(objsample.Oid, objsample.PurifierSampleID);
            //        }
            //    }
            //}
            return properties;
        }

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        #endregion

        #region SampleAmount
        private string _SampleAmount;
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        public string SampleAmount
        {
            get { return _SampleAmount; }
            set { SetPropertyValue(nameof(SampleAmount), ref _SampleAmount, value); }
        }
        #endregion

        #region AreaOrPerson
        private string _AreaOrPerson;
        public string AreaOrPerson
        {
            get
            {
                return _AreaOrPerson;
            }
            set
            {
                SetPropertyValue<string>(nameof(AreaOrPerson), ref _AreaOrPerson, value);
            }
        }
        #endregion

        #region AuthorizedBy
        private CustomSystemUser _AuthorizedBy;
        public CustomSystemUser AuthorizedBy
        {
            get
            {
                return _AuthorizedBy;
            }
            set
            {
                SetPropertyValue(nameof(AuthorizedBy), ref _AuthorizedBy, value);
            }
        }
        #endregion

        #region AuthorizedDate
        private DateTime _AuthorizedDate;
        public DateTime AuthorizedDate
        {
            get
            {
                return _AuthorizedDate;
            }
            set
            {
                SetPropertyValue(nameof(AuthorizedDate), ref _AuthorizedDate, value);
            }
        }
        #endregion

        #region Barp
        private string _Barp;
        public string Barp
        {
            get
            {
                return _Barp;
            }
            set
            {
                SetPropertyValue<string>(nameof(Barp), ref _Barp, value);
            }
        }
        #endregion

        #region BottleQty
        private int _BottleQty;
        public int BottleQty
        {
            get
            {
                return _BottleQty;
            }
            set
            {
                SetPropertyValue<int>(nameof(BottleQty), ref _BottleQty, value);
            }
        }
        #endregion

        #region BuriedDepthOfGroundWater
        private string _BuriedDepthOfGroundWater;
        public string BuriedDepthOfGroundWater
        {
            get
            {
                return _BuriedDepthOfGroundWater;
            }
            set
            {
                SetPropertyValue<string>(nameof(BuriedDepthOfGroundWater), ref _BuriedDepthOfGroundWater, value);
            }
        }
        #endregion

        #region ChlorineFree
        private string _ChlorineFree;
        [XafDisplayName("FreeColorine(mg/L)")]
        public string ChlorineFree
        {
            get
            {
                return _ChlorineFree;
            }
            set
            {
                SetPropertyValue<string>(nameof(ChlorineFree), ref _ChlorineFree, value);
            }
        }
        #endregion

        #region ChlorineTotal
        private string _ChlorineTotal;
        [XafDisplayName("TotalChlorine(mg/L)")]
        public string ChlorineTotal
        {
            get
            {
                return _ChlorineTotal;
            }
            set
            {
                SetPropertyValue<string>(nameof(ChlorineTotal), ref _ChlorineTotal, value);
            }
        }
        #endregion

        #region CompositeQty
        private int _CompositeQty;
        public int CompositeQty
        {
            get
            {
                return _CompositeQty;
            }
            set
            {
                SetPropertyValue<int>(nameof(CompositeQty), ref _CompositeQty, value);
            }
        }
        #endregion

        #region DateStartExpected
        private DateTime _DateStartExpected;
        public DateTime DateStartExpected
        {
            get
            {
                return _DateStartExpected;
            }
            set
            {
                SetPropertyValue(nameof(DateStartExpected), ref _DateStartExpected, value);
            }
        }
        #endregion

        #region DateEndExpected
        private DateTime _DateEndExpected;
        public DateTime DateEndExpected
        {
            get
            {
                return _DateEndExpected;
            }
            set
            {
                SetPropertyValue(nameof(DateEndExpected), ref _DateEndExpected, value);
            }
        }
        #endregion

        #region Department
        private Department _Department;
        public Department Department
        {
            get
            {
                return _Department;
            }
            set
            {
                SetPropertyValue(nameof(Department), ref _Department, value);
            }
        }
        #endregion

        #region Depth
        private string _Depth;
        public string Depth
        {
            get
            {
                return _Depth;
            }
            set
            {
                SetPropertyValue(nameof(Depth), ref _Depth, value);
            }
        }
        #endregion

        #region Description
        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetPropertyValue(nameof(Description), ref _Description, value);
            }
        }
        #endregion

        #region DischargeFlow
        private string _DischargeFlow;
        public string DischargeFlow
        {
            get
            {
                return _DischargeFlow;
            }
            set
            {
                SetPropertyValue(nameof(DischargeFlow), ref _DischargeFlow, value);
            }
        }
        #endregion

        #region DischargePipeHeight
        private string _DischargePipeHeight;
        public string DischargePipeHeight
        {
            get
            {
                return _DischargePipeHeight;
            }
            set
            {
                SetPropertyValue(nameof(DischargePipeHeight), ref _DischargePipeHeight, value);
            }
        }
        #endregion

        #region DO
        private string _DO;
        public string DO
        {
            get
            {
                return _DO;
            }
            set
            {
                SetPropertyValue(nameof(DO), ref _DO, value);
            }
        }
        #endregion

        //#region DueDate
        //private DateTime _DueDate;
        //public DateTime DueDate
        //{
        //    get
        //    {
        //        if (COCID != null && COCID.DueDate != null)
        //        {
        //            _DueDate = (DateTime)COCID.DueDate;
        //        }
        //        return _DueDate;
        //    }
        //    set
        //    {
        //        SetPropertyValue(nameof(DueDate), ref _DueDate, value);
        //    }
        //}
        //#endregion

        #region Emission
        private string _Emission;
        public string Emission
        {
            get
            {
                return _Emission;
            }
            set
            {
                SetPropertyValue(nameof(Emission), ref _Emission, value);
            }
        }
        #endregion

        #region EndOfRoad
        private string _EndOfRoad;
        public string EndOfRoad
        {
            get
            {
                return _EndOfRoad;
            }
            set
            {
                SetPropertyValue(nameof(EndOfRoad), ref _EndOfRoad, value);
            }
        }
        #endregion

        #region EquipmentName
        private string _EquipmentName;
        public string EquipmentName
        {
            get
            {
                return _EquipmentName;
            }
            set
            {
                SetPropertyValue(nameof(EquipmentName), ref _EquipmentName, value);
            }
        }
        #endregion

        #region EquipmentModel
        private string _EquipmentModel;
        public string EquipmentModel
        {
            get
            {
                return _EquipmentModel;
            }
            set
            {
                SetPropertyValue(nameof(EquipmentModel), ref _EquipmentModel, value);
            }
        }
        #endregion

        #region FlowRateLiterPerMin
        private string _FlowRateLiterPerMin;
        public string FlowRateLiterPerMin
        {
            get
            {
                return _FlowRateLiterPerMin;
            }
            set
            {
                SetPropertyValue(nameof(FlowRateLiterPerMin), ref _FlowRateLiterPerMin, value);
            }
        }
        #endregion

        #region FlowRateCubicMeterPerHour
        private string _FlowRateCubicMeterPerHour;
        public string FlowRateCubicMeterPerHour
        {
            get
            {
                return _FlowRateCubicMeterPerHour;
            }
            set
            {
                SetPropertyValue(nameof(FlowRateCubicMeterPerHour), ref _FlowRateCubicMeterPerHour, value);
            }
        }
        #endregion

        #region FlowVelocity
        private string _FlowVelocity;
        public string FlowVelocity
        {
            get
            {
                return _FlowVelocity;
            }
            set
            {
                SetPropertyValue(nameof(FlowVelocity), ref _FlowVelocity, value);
            }
        }
        #endregion

        #region ForeignMaterial
        private string _ForeignMaterial;
        public string ForeignMaterial
        {
            get
            {
                return _ForeignMaterial;
            }
            set
            {
                SetPropertyValue(nameof(ForeignMaterial), ref _ForeignMaterial, value);
            }
        }
        #endregion

        #region Frequency
        private string _Frequency;
        public string Frequency
        {
            get
            {
                return _Frequency;
            }
            set
            {
                SetPropertyValue(nameof(Frequency), ref _Frequency, value);
            }
        }
        #endregion

        #region GISStatus
        private string _GISStatus;
        public string GISStatus
        {
            get
            {
                return _GISStatus;
            }
            set
            {
                SetPropertyValue(nameof(GISStatus), ref _GISStatus, value);
            }
        }
        #endregion

        #region GravelContent
        private string _GravelContent;
        public string GravelContent
        {
            get
            {
                return _GravelContent;
            }
            set
            {
                SetPropertyValue(nameof(GravelContent), ref _GravelContent, value);
            }
        }
        #endregion

        #region GroupSample
        private string _GroupSample;
        public string GroupSample
        {
            get
            {
                return _GroupSample;
            }
            set
            {
                SetPropertyValue(nameof(GroupSample), ref _GroupSample, value);
            }
        }
        #endregion

        #region Hold
        private bool _Hold;
        public bool Hold
        {
            get
            {
                return _Hold;
            }
            set
            {
                SetPropertyValue(nameof(Hold), ref _Hold, value);
            }
        }
        #endregion

        #region Humidity
        private string _Humidity;
        public string Humidity
        {
            get
            {
                return _Humidity;
            }
            set
            {
                SetPropertyValue(nameof(Humidity), ref _Humidity, value);
            }
        }
        #endregion

        #region IceCycle
        private string _IceCycle;
        public string IceCycle
        {
            get
            {
                return _IceCycle;
            }
            set
            {
                SetPropertyValue(nameof(IceCycle), ref _IceCycle, value);
            }
        }
        #endregion

        #region RegionNameOfSection
        private string _RegionNameOfSection;
        public string RegionNameOfSection
        {
            get
            {
                return _RegionNameOfSection;
            }
            set
            {
                SetPropertyValue(nameof(RegionNameOfSection), ref _RegionNameOfSection, value);
            }
        }
        #endregion

        #region RiverWidth
        private string _RiverWidth;
        public string RiverWidth
        {
            get
            {
                return _RiverWidth;
            }
            set
            {
                SetPropertyValue(nameof(RiverWidth), ref _RiverWidth, value);
            }
        }
        #endregion

        #region SampleName
        private string _SampleName;
        [Browsable(false)]
        public string SampleName
        {
            get
            {
                return _SampleName;
            }
            set
            {
                SetPropertyValue(nameof(SampleName), ref _SampleName, value);
            }
        }
        #endregion

        #region SamplePointID
        private string _SamplePointID;
        public string SamplePointID
        {
            get
            {
                return _SamplePointID;
            }
            set
            {
                SetPropertyValue(nameof(SamplePointID), ref _SamplePointID, value);
            }
        }
        #endregion

        #region SampleSource
        private string _SampleSource;
        public string SampleSource
        {
            get
            {
                return _SampleSource;
            }
            set
            {
                SetPropertyValue(nameof(SampleSource), ref _SampleSource, value);
            }
        }
        #endregion

        #region Section
        private string _Section;
        public string Section
        {
            get
            {
                return _Section;
            }
            set { SetPropertyValue(nameof(Section), ref _Section, value); }
        }
        #endregion
        ////#region SiteName
        ////private SampleSites _SiteName;
        ////[DataSourceProperty("SampleSitesDataSource")]
        ////[ImmediatePostData(true)]
        ////public SampleSites SiteName
        ////{
        ////    get { return _SiteName; }
        ////    set { SetPropertyValue<SampleSites>(nameof(SiteName), ref _SiteName, value); }
        ////}
        ////[Browsable(false)]
        ////[NonPersistent]
        ////public XPCollection<SampleSites> SampleSitesDataSource
        ////{
        ////    get
        ////    {
        ////        if (COC != null)
        ////        {
        ////            XPCollection<SampleSites> listprojects = new XPCollection<SampleSites>(Session, CriteriaOperator.Parse("[Client.Oid] = ? ", COC.Client.Oid));
        ////            return listprojects;
        ////        }
        ////        else
        ////        {
        ////            return null;
        ////        }
        ////    }
        ////}
        ////#endregion

        #region SiteDescription
        private string _SiteDescription;
        public string SiteDescription
        {
            get
            {
                return _SiteDescription;
            }
            set { SetPropertyValue(nameof(SiteDescription), ref _SiteDescription, value); }
        }
        #endregion

        #region SoilColor
        private string _SoilColor;
        public string SoilColor
        {
            get
            {
                return _SoilColor;
            }
            set { SetPropertyValue(nameof(SoilColor), ref _SoilColor, value); }
        }
        #endregion

        #region SoilQuality
        private string _SoilQuality;
        public string SoilQuality
        {
            get
            {
                return _SoilQuality;
            }
            set { SetPropertyValue(nameof(SoilQuality), ref _SoilQuality, value); }
        }
        #endregion

        #region SoilMoisture
        private string _SoilMoisture;
        public string SoilMoisture
        {
            get
            {
                return _SoilMoisture;
            }
            set { SetPropertyValue(nameof(SoilMoisture), ref _SoilMoisture, value); }
        }
        #endregion

        #region SortOrder
        private int _SortOrder;
        public int SortOrder
        {
            get
            {
                return _SortOrder;
            }
            set { SetPropertyValue(nameof(SortOrder), ref _SortOrder, value); }
        }
        #endregion

        #region SplitQty
        private int _SplitQty;
        public int SplitQty
        {
            get
            {
                return _SplitQty;
            }
            set { SetPropertyValue(nameof(SplitQty), ref _SplitQty, value); }
        }
        #endregion

        #region StandardVol
        private string _StandardVol;
        public string StandardVol
        {
            get
            {
                return _StandardVol;
            }
            set { SetPropertyValue(nameof(StandardVol), ref _StandardVol, value); }
        }
        #endregion

        #region StartOfRoad
        private string _StartOfRoad;
        public string StartOfRoad
        {
            get
            {
                return _StartOfRoad;
            }
            set { SetPropertyValue(nameof(StartOfRoad), ref _StartOfRoad, value); }
        }
        #endregion
        #region Station
        private string _Station;
        public string Station
        {
            get
            {
                return _Station;
            }
            set { SetPropertyValue(nameof(Station), ref _Station, value); }
        }
        #endregion

        #region NoOfPoints
        private int _NoOfPoints;
        public int NoOfPoints
        {
            get
            {
                return _NoOfPoints;
            }
            set
            {
                SetPropertyValue<int>(nameof(NoOfPoints), ref _NoOfPoints, value);
            }
        }
        #endregion

        #region NoOfCollectionsEachTime
        private int _NoOfCollectionsEachTime;
        public int NoOfCollectionsEachTime
        {
            get
            {
                return _NoOfCollectionsEachTime;
            }
            set
            {
                SetPropertyValue<int>(nameof(NoOfCollectionsEachTime), ref _NoOfCollectionsEachTime, value);
            }
        }
        #endregion

        #region TotalTimes
        private int _TotalTimes;
        public int TotalTimes
        {
            get
            {
                return _TotalTimes;
            }
            set
            {
                SetPropertyValue<int>(nameof(TotalTimes), ref _TotalTimes, value);
            }
        }
        #endregion

        #region TotalSamples
        private int _TotalSamples;
        public int TotalSamples
        {
            get
            {
                return _TotalSamples;
            }
            set
            {
                SetPropertyValue<int>(nameof(TotalSamples), ref _TotalSamples, value);
            }
        }
        #endregion

        #region Interval
        private int _Interval;
        public int Interval
        {
            get
            {
                return _Interval;
            }
            set
            {
                SetPropertyValue<int>(nameof(Interval), ref _Interval, value);
            }
        }
        #endregion

        #region TtimeUnit
        private Unit _TtimeUnit;
        public Unit TtimeUnit
        {
            get
            {
                return _TtimeUnit;
            }
            set
            {
                SetPropertyValue<Unit>(nameof(TtimeUnit), ref _TtimeUnit, value);
            }
        }
        #endregion

        #region SamplingEquipment
        [VisibleInDetailView(false)]
        private string _SamplingEquipment;
        public string SamplingEquipment
        {
            get { return _SamplingEquipment; }
            set { SetPropertyValue<string>(nameof(SamplingEquipment), ref _SamplingEquipment, value); }
        }
        #endregion

        #region SampleDescription
        [VisibleInDetailView(false)]
        private string _SampleDescription;
        public string SampleDescription
        {
            get { return _SampleDescription; }
            set { SetPropertyValue<string>(nameof(SampleDescription), ref _SampleDescription, value); }
        }
        #endregion

        #region SampleCondition
        [VisibleInDetailView(false)]
        private string _SampleCondition;
        public string SampleCondition
        {
            get { return _SampleCondition; }
            set { SetPropertyValue<string>(nameof(SampleCondition), ref _SampleCondition, value); }
        }
        #endregion

        #region SampleDescription
        [VisibleInDetailView(false)]
        private string _SamplingProcedure;
        public string SamplingProcedure
        {
            get { return _SamplingProcedure; }
            set { SetPropertyValue<string>(nameof(SamplingProcedure), ref _SamplingProcedure, value); }
        }
        #endregion

        #region AssignTo
        [VisibleInDetailView(false)]
        private string _AssignTo;
        [ImmediatePostData]
        public string AssignTo
        {
            get { return _AssignTo; }
            set { SetPropertyValue<string>(nameof(AssignTo), ref _AssignTo, value); }
        }
        #endregion

        #region DateAssigned
        [VisibleInDetailView(false)]
        private DateTime _DateAssigned;
        public DateTime DateAssigned
        {
            get { return _DateAssigned; }
            set { SetPropertyValue<DateTime>(nameof(DateAssigned), ref _DateAssigned, value); }
        }
        #endregion

        #region AssignTo
        [VisibleInDetailView(false)]
        private Employee _AssignedBy;
        public Employee AssignedBy
        {
            get { return _AssignedBy; }
            set { SetPropertyValue<Employee>(nameof(AssignedBy), ref _AssignedBy, value); }
        }
        #endregion

        #region TestSummary
        [VisibleInDetailView(false)]
        private string _TestSummary;
        public string TestSummary
        {
            get { return _TestSummary; }
            set { SetPropertyValue<string>(nameof(TestSummary), ref _TestSummary, value); }
        }
        #endregion

        #region FieldTestSummary
        [VisibleInDetailView(false)]
        private string _FieldTestSummary;
        [Size(SizeAttribute.Unlimited)]
        public string FieldTestSummary
        {
            get { return _FieldTestSummary; }
            set { SetPropertyValue<string>(nameof(FieldTestSummary), ref _FieldTestSummary, value); }
        }
        #endregion

        //#region IsNotTransferred
        //private bool _IsNotTransferred;
        //[VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        //public bool IsNotTransferred
        //{
        //    get
        //    {
        //        return _IsNotTransferred;
        //    }
        //    set
        //    {
        //        SetPropertyValue(nameof(IsNotTransferred), ref _IsNotTransferred, value);
        //    }
        //}
        //#endregion

        #region SampleWeight
        private string _SampleWeight;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string SampleWeight
        {
            get { return _SampleWeight; }
            set { SetPropertyValue(nameof(SampleWeight), ref _SampleWeight, value); }
        }
        #endregion

        #region RetainedWeight
        private string _RetainedWeight;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string RetainedWeight
        {
            get
            {
                if (!string.IsNullOrEmpty(SampleWeight) && string.IsNullOrEmpty(_RetainedWeight))
                {
                    _RetainedWeight = SampleWeight;
                }
                return _RetainedWeight;
            }
            set { SetPropertyValue(nameof(RetainedWeight), ref _RetainedWeight, value); }
        }
        #endregion

        #region GrossWeight
        private string _GrossWeight;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string GrossWeight
        {
            get { return _GrossWeight; }
            set { SetPropertyValue(nameof(GrossWeight), ref _GrossWeight, value); }
        }
        #endregion

        #region BatchID
        private string _BatchID;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string BatchID
        {
            get
            {
                if (COCID != null && COCID.BatchID != null)
                {
                    _BatchID = COCID.BatchID;
                }
                return _BatchID;
            }
            set { SetPropertyValue(nameof(BatchID), ref _BatchID, value); }
        }
        #endregion

        #region BatchSize
        private string _BatchSize;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string BatchSize
        {
            get { return _BatchSize; }
            set { SetPropertyValue(nameof(BatchSize), ref _BatchSize, value); }
        }
        #endregion

        #region PackageNumber
        private string _PackageNumber;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string PackageNumber
        {
            get
            {
                if (COCID != null && COCID.PackageNo != null)
                {
                    _PackageNumber = COCID.PackageNo;
                }
                return _PackageNumber;
            }
            set { SetPropertyValue(nameof(PackageNumber), ref _PackageNumber, value); }
        }
        #endregion

        #region SampleTag
        private string _SampleTag;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string SampleTag
        {
            get { return _SampleTag; }
            set { SetPropertyValue(nameof(SampleTag), ref _SampleTag, value); }
        }
        #endregion

        #region SampleImage
        private byte[] _SampleImage;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public byte[] SampleImage
        {
            get { return _SampleImage; }
            set { SetPropertyValue(nameof(SampleImage), ref _SampleImage, value); }
        }
        #endregion

        #region BalanceID
        private Modules.BusinessObjects.Assets.Labware _BalanceID;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Modules.BusinessObjects.Assets.Labware BalanceID
        {
            get
            {
                if (COCID != null && COCID.BalanceID != null)
                {
                    _BalanceID = COCID.BalanceID;
                }
                return _BalanceID;
            }
            set { SetPropertyValue(nameof(BalanceID), ref _BalanceID, value); }
        }
        #endregion

        #region ItemName
        private string _ItemName;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ItemName
        {
            get { return _ItemName; }
            set { SetPropertyValue(nameof(ItemName), ref _ItemName, value); }
        }
        #endregion

        #region ManifestNo
        private uint _ManifestNo;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public uint ManifestNo
        {
            get { return _ManifestNo; }
            set { SetPropertyValue(nameof(ManifestNo), ref _ManifestNo, value); }
        }
        #endregion

        #region OriginatingEntiry
        private string _OriginatingEntiry;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string OriginatingEntiry
        {
            get { return _OriginatingEntiry; }
            set { SetPropertyValue(nameof(OriginatingEntiry), ref _OriginatingEntiry, value); }
        }
        #endregion

        #region OriginatingLicenseNumber
        private uint _OriginatingLicenseNumber;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public uint OriginatingLicenseNumber
        {
            get { return _OriginatingLicenseNumber; }
            set { SetPropertyValue(nameof(OriginatingLicenseNumber), ref _OriginatingLicenseNumber, value); }
        }
        #endregion

        #region BatchSize(pc)
        private double _BatchSize_pc;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public double BatchSize_pc
        {
            get { return _BatchSize_pc; }
            set { SetPropertyValue(nameof(BatchSize_pc), ref _BatchSize_pc, value); }
        }
        #endregion

        #region BatchSize(Units)
        private double _BatchSize_Units;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public double BatchSize_Units
        {
            get { return _BatchSize_Units; }
            set { SetPropertyValue(nameof(BatchSize_Units), ref _BatchSize_Units, value); }
        }
        #endregion

        #region PiecesPerUnit(pc/unit)
        private double _PiecesPerUnit;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public double PiecesPerUnit
        {
            get { return _PiecesPerUnit; }
            set { SetPropertyValue(nameof(PiecesPerUnit), ref _PiecesPerUnit, value); }
        }
        #endregion

        #region TargetMGTHC:CBD(mg/pc)
        private double _TargetMGTHC_CBD_mg_pc;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public double TargetMGTHC_CBD_mg_pc
        {
            get { return _TargetMGTHC_CBD_mg_pc; }
            set { SetPropertyValue(nameof(TargetMGTHC_CBD_mg_pc), ref _TargetMGTHC_CBD_mg_pc, value); }
        }
        #endregion

        #region TargetMGTHC:CBD(mg/unit)
        private double _TargetMGTHC_CBD_mg_unit;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public double TargetMGTHC_CBD_mg_unit
        {
            get { return _TargetMGTHC_CBD_mg_unit; }
            set { SetPropertyValue(nameof(TargetMGTHC_CBD_mg_unit), ref _TargetMGTHC_CBD_mg_unit, value); }
        }
        #endregion

        #region TargetUnitWeight(g/pc)
        private string _TargetUnitWeight_g_pc;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string TargetUnitWeight_g_pc
        {
            get { return _TargetUnitWeight_g_pc; }
            set { SetPropertyValue(nameof(TargetUnitWeight_g_pc), ref _TargetUnitWeight_g_pc, value); }
        }
        #endregion

        #region TargetUnitWeight(g/unit)
        private string _TargetUnitWeight_g_unit;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string TargetUnitWeight_g_unit
        {
            get { return _TargetUnitWeight_g_unit; }
            set { SetPropertyValue(nameof(TargetUnitWeight_g_unit), ref _TargetUnitWeight_g_unit, value); }
        }
        #endregion

        #region TargetPotency(mg)
        private string _TargetPotency;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string TargetPotency
        {
            get { return _TargetPotency; }
            set { SetPropertyValue(nameof(TargetPotency), ref _TargetPotency, value); }
        }
        #endregion

        #region TargetWeight(g)
        private string _TargetWeight;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string TargetWeight
        {
            get { return _TargetWeight; }
            set { SetPropertyValue(nameof(TargetWeight), ref _TargetWeight, value); }
        }
        #endregion

        #region FinalPackaging
        private uint _FinalPackaging;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public uint FinalPackaging
        {
            get { return _FinalPackaging; }
            set { SetPropertyValue(nameof(FinalPackaging), ref _FinalPackaging, value); }
        }
        #endregion

        #region FinalForm
        private string _FinalForm;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string FinalForm
        {
            get { return _FinalForm; }
            set { SetPropertyValue(nameof(FinalForm), ref _FinalForm, value); }
        }
        #endregion

        #region RushSample
        private string _RushSample;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string RushSample
        {
            get { return _RushSample; }
            set { SetPropertyValue(nameof(RushSample), ref _RushSample, value); }
        }
        #endregion

        #region Increments
        private string _Increments;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string Increments
        {
            get { return _Increments; }
            set { SetPropertyValue(nameof(Increments), ref _Increments, value); }
        }
        #endregion

        #region LicenseNumber
        private string _LicenseNumber;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string LicenseNumber
        {
            get { return _LicenseNumber; }
            set { SetPropertyValue(nameof(LicenseNumber), ref _LicenseNumber, value); }
        }
        #endregion

        #region Notes
        private string _Notes;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string Notes
        {
            get { return _Notes; }
            set { SetPropertyValue(nameof(Notes), ref _Notes, value); }
        }
        #endregion

        #region SiteID
        private string _SiteID;
        public string SiteID
        {
            get { return _SiteID; }
            set { SetPropertyValue(nameof(SiteID), ref _SiteID, value); }
        }
        #endregion 

        #region Client
        private Customer _Client;
        [ImmediatePostData]
        public Customer Client
        {
            get { return _Client; }
            set { SetPropertyValue(nameof(Client), ref _Client, value); }
        }
        #endregion

        #region Address
        private string _Address;
        [ImmediatePostData]
        [Size(2000)]
        public string Address
        {
            get
            {
                if (Client != null && Client.Address != null && Client.Address1 != null)
                {
                    _Address = Client.Address + ", " + Client.Address1;
                }
                else if (Client != null && Client.Address != null)
                {
                    _Address = Client.Address;
                }
                return _Address;
            }
            set { SetPropertyValue(nameof(Address), ref _Address, value); }
        }
        #endregion

        #region Blended 
        private bool _Blended;
        //[RuleRequiredField]
        //[RuleUniqueValue]
        public bool Blended
        {
            get { return _Blended; }
            set { SetPropertyValue(nameof(Blended), ref _Blended, value); }
        }
        #endregion

        #region PWSSystemName 
        private PWSSystem _PWSSystemName;
        //[RuleRequiredField]
        //[RuleUniqueValue]
        public PWSSystem PWSSystemName
        {
            get { return _PWSSystemName; }
            set { SetPropertyValue(nameof(PWSSystemName), ref _PWSSystemName, value); }
        }
        #endregion

        #region SystemType 
        private SystemTypes _SystemType;
        //[RuleRequiredField]
        //[RuleUniqueValue]
        public SystemTypes SystemType
        {
            get { return _SystemType; }
            set { SetPropertyValue(nameof(SystemType), ref _SystemType, value); }
        }
        #endregion

        #region SiteCode
        private string _SiteCode;
        //[RuleUniqueValue]
        //[RuleRequiredField]
        public string SiteCode
        {
            get { return _SiteCode; }
            set { SetPropertyValue(nameof(SiteCode), ref _SiteCode, value); }
        }
        #endregion

        #region PWSID
        private string _PWSID;
        public string PWSID
        {
            get { return _PWSID; }
            set { SetPropertyValue(nameof(PWSID), ref _PWSID, value); }
        }
        #endregion

        #region KeyMap
        private string _KeyMap;
        public string KeyMap
        {
            get { return _KeyMap; }
            set { SetPropertyValue(nameof(KeyMap), ref _KeyMap, value); }
        }
        #endregion

        #region ServiceArea
        private string _ServiceArea;
        public string ServiceArea
        {
            get { return _ServiceArea; }
            set { SetPropertyValue(nameof(ServiceArea), ref _ServiceArea, value); }
        }
        #endregion

        #region SiteNameArchived
        private bool _SiteNameArchived;
        public bool SiteNameArchived
        {
            get { return _SiteNameArchived; }
            set { SetPropertyValue(nameof(SiteNameArchived), ref _SiteNameArchived, value); }
        }
        #endregion

        #region IsActive
        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set { SetPropertyValue(nameof(IsActive), ref _IsActive, value); }
        }
        #endregion

        #region City
        private string _City;
        [ImmediatePostData]
        public string City
        {
            get
            {
                if (Client != null && Client.City != null)
                {
                    _City = Client.City.CityName;
                }
                return _City;
            }
            set { SetPropertyValue(nameof(City), ref _City, value); }
        }
        #endregion

        #region State
        private string _State;
        [ImmediatePostData]
        public string State
        {
            get
            {
                if (Client != null && Client.State != null && !string.IsNullOrEmpty(Client.State.ShortName))
                {
                    _State = Client.State.ShortName;
                }
                else if (Client != null && Client.State != null && !string.IsNullOrEmpty(Client.State.LongName))
                {
                    _State = Client.State.LongName;
                }
                return _State;
            }
            set { SetPropertyValue(nameof(State), ref _State, value); }
        }
        #endregion

        #region ZipCode
        private string _ZipCode;
        [ImmediatePostData]
        public string ZipCode
        {
            get
            {
                if (Client != null && Client.Zip != null)
                {
                    _ZipCode = Client.Zip;
                }
                return _ZipCode;
            }
            set { SetPropertyValue(nameof(ZipCode), ref _ZipCode, value); }
        }
        #endregion

        #region FacilityID
        private string _FacilityID;
        public string FacilityID
        {
            get { return _FacilityID; }
            set { SetPropertyValue(nameof(FacilityID), ref _FacilityID, value); }
        }
        #endregion

        #region FacilityName
        private string _FacilityName;
        public string FacilityName
        {
            get { return _FacilityName; }
            set { SetPropertyValue(nameof(FacilityName), ref _FacilityName, value); }
        }
        #endregion

        #region FacilityType
        private string _FacilityType;
        public string FacilityType
        {
            get { return _FacilityType; }
            set { SetPropertyValue(nameof(FacilityType), ref _FacilityType, value); }
        }
        #endregion

        #region SamplePointType
        private string _SamplePointType;
        public string SamplePointType
        {
            get { return _SamplePointType; }
            set { SetPropertyValue(nameof(SamplePointType), ref _SamplePointType, value); }
        }
        #endregion

        #region WaterType
        private WaterTypes _WaterType;
        public WaterTypes WaterType
        {
            get { return _WaterType; }
            set { SetPropertyValue(nameof(WaterType), ref _WaterType, value); }
        }
        #endregion

        #region MonitoryingRequirement
        private string _MonitoryingRequirement;
        public string MonitoryingRequirement
        {
            get { return _MonitoryingRequirement; }
            set { SetPropertyValue(nameof(MonitoryingRequirement), ref _MonitoryingRequirement, value); }
        }
        #endregion

        #region SiteUserDefinedColumn1
        private string _SiteUserDefinedColumn1;
        [XafDisplayName("CollectorUserID")]
        public string SiteUserDefinedColumn1
        {
            get { return _SiteUserDefinedColumn1; }
            set { SetPropertyValue(nameof(SiteUserDefinedColumn1), ref _SiteUserDefinedColumn1, value); }
        }
        #endregion

        #region SiteUserDefinedColumn2
        private string _SiteUserDefinedColumn2;
        public string SiteUserDefinedColumn2
        {
            get { return _SiteUserDefinedColumn2; }
            set { SetPropertyValue(nameof(SiteUserDefinedColumn2), ref _SiteUserDefinedColumn2, value); }
        }
        #endregion

        #region SiteUserDefinedColumn3
        private string _SiteUserDefinedColumn3;
        public string SiteUserDefinedColumn3
        {
            get { return _SiteUserDefinedColumn3; }
            set { SetPropertyValue(nameof(SiteUserDefinedColumn3), ref _SiteUserDefinedColumn3, value); }
        }
        #endregion

        #region CreatedBy
        private CustomSystemUser _CreatedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region EnteredBy
        private Employee _EnteredBy;
        public Employee EnteredBy
        {
            get { return _EnteredBy; }
            set { SetPropertyValue(nameof(EnteredBy), ref _EnteredBy, value); }
        }
        #endregion

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

        #region LastUpdatedBy
        private Employee _LastUpdatedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Employee LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { SetPropertyValue(nameof(LastUpdatedBy), ref _LastUpdatedBy, value); }
        }
        #endregion

        #region LastUpdatedDate
        private DateTime _LastUpdatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { SetPropertyValue(nameof(LastUpdatedDate), ref _LastUpdatedDate, value); }
        }
        #endregion

        #region SamplingAddress
        private string _SamplingAddress;
        public string SamplingAddress

        {
            get { return _SamplingAddress; }
            set { SetPropertyValue(nameof(SamplingAddress), ref _SamplingAddress, value); }
        }
        #endregion

        #region StationLocation
        private SampleSites _StationLocation;
        public SampleSites StationLocation

        {
            get { return _StationLocation; }
            set { SetPropertyValue(nameof(StationLocation), ref _StationLocation, value); }
        }
        #endregion
        #region StationLocationName
        private string _StationLocationName;
        public string StationLocationName
        {
            get { return _StationLocationName; }
            set { SetPropertyValue(nameof(StationLocationName), ref _StationLocationName, value); }
        }
        #endregion
        #region ParentSampleID
        private string _ParentSampleID;
        public string ParentSampleID
        {
            get { return _ParentSampleID; }
            set { SetPropertyValue(nameof(ParentSampleID), ref _ParentSampleID, value); }
        }
        #endregion

        #region ParentSampleDate
        private string _ParentSampleDate;
        public string ParentSampleDate
        {
            get { return _ParentSampleDate; }
            set { SetPropertyValue(nameof(ParentSampleDate), ref _ParentSampleDate, value); }
        }
        #endregion

        #region RepeatLocation
        private string _RepeatLocation;
        public string RepeatLocation
        {
            get { return _RepeatLocation; }
            set { SetPropertyValue(nameof(RepeatLocation), ref _RepeatLocation, value); }
        }
        #endregion


        #region RejectionCriteria
        private string _RejectionCriteria;
        [XafDisplayName("RejectionCriteria#")]
        public string RejectionCriteria
        {
            get { return _RejectionCriteria; }
            set { SetPropertyValue(nameof(RejectionCriteria), ref _RejectionCriteria, value); }
        }
        #endregion

        #region Matrix
        private Matrix _Matrix;
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Matrix), ref _Matrix, value); }
        }
        #endregion

        #region WaterSource
        private string _WaterSource;
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string WaterSource
        {
            get
            {
                return _WaterSource;
            }
            set { SetPropertyValue(nameof(WaterSource), ref _WaterSource, value); }
        }
        #endregion

        //#region TestCategory
        //private string _TestCategory;
        //public string TestCategory
        //{
        //    get { return _TestCategory; }
        //    set { SetPropertyValue<string>(nameof(TestCategory), ref _TestCategory, value); }
        //}
        //#endregion


        //#region Tests
        //[Association("COCSettingsSampleTest", UseAssociationNameAsIntermediateTableName = true)]
        //public XPCollection<COCSettingsTest> Tests
        //{
        //    get { return GetCollection<COCSettingsTest>(nameof(Tests)); }
        //}
        //#endregion






        //#region Time(Min)
        //private DateTime _Timemin;
        //public DateTime Timemin
        //{
        //    get { return _Timemin; }
        //    set { SetPropertyValue<DateTime>("Timemin", ref _Timemin, value); }
        //}
        //#endregion       





        //#region SamplingPointID
        //private string _SamplingPointID;
        //public string SamplingPointID
        //{
        //    get { return _SamplingPointID; }
        //    set { SetPropertyValue(nameof(SamplingPointID), ref _SamplingPointID, value); }
        //}
        //#endregion     

        #region 'Remark'
        private string _Remark;
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }
        #endregion

        #region 'Sample Status'
        private string _SampleStatus;
        public string SampleStatus
        {
            get
            {
                return _SampleStatus;
            }
            set { SetPropertyValue(nameof(SampleStatus), ref _SampleStatus, value); }
        }
        #endregion

        #region 'ProjectNumber'
        private string _ProjectNumber;
        public string ProjectNumber
        {
            get
            {
                return _ProjectNumber;
            }
            set { SetPropertyValue(nameof(ProjectNumber), ref _ProjectNumber, value); }
        }
        #endregion

        #region 'PreviousSample'
        private string _PreviousSample;
        public string PreviousSample
        {
            get
            {
                return _PreviousSample;
            }
            set { SetPropertyValue(nameof(PreviousSample), ref _PreviousSample, value); }
        }
        #endregion

        #region 'PreviousCollection'
        private string _PreviousCollection;
        public string PreviousCollection
        {
            get
            {
                return _PreviousCollection;
            }
            set { SetPropertyValue(nameof(PreviousCollection), ref _PreviousCollection, value); }
        }
        #endregion

        #region 'SamplingEvent'
        private string _SamplingEvent;
        public string SamplingEvent
        {
            get
            {
                return _SamplingEvent;
            }
            set { SetPropertyValue(nameof(SamplingEvent), ref _SamplingEvent, value); }
        }
        #endregion

        #region 'LoginBy'
        private Employee _LoginBy;
        public Employee LoginBy
        {
            get
            {
                return _LoginBy;
            }
            set { SetPropertyValue(nameof(LoginBy), ref _LoginBy, value); }
        }
        #endregion

        #region 'LoginDateTime'
        private DateTime _LoginDateTime;
        public DateTime LoginDateTime
        {
            get
            {
                return _LoginDateTime;
            }
            set { SetPropertyValue(nameof(LoginDateTime), ref _LoginDateTime, value); }
        }
        #endregion
        #region ComplianceProject
        private bool _ComplianceProject;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public bool ComplianceProject
        {
            get { return _ComplianceProject; }
            set { SetPropertyValue("ComplianceProject", ref _ComplianceProject, value); }
        }
        #endregion

    }
}