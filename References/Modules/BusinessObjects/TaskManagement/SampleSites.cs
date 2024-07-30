using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.DWQRReportTemplateSetup;
using Modules.BusinessObjects.Setting.SamplesSite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.TaskManagement
{
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("SampleSiteUnique", DefaultContexts.Save, targetProperties: "Client,SiteName,Address", CustomMessageTemplate = "A Site Name with the same data already exists")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SampleSites : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleSites(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            EnteredBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            EnteredDate = Library.GetServerTime(Session);
        }

        protected override void OnSaving()
        {
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            if (string.IsNullOrEmpty(SiteID))
            {
                LastUpdatedDate = Library.GetServerTime(Session);
                LastUpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(SiteID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(SampleSites), criteria, null)) + 1).ToString("00000");
                SiteID = "SS" + tempID;
            }
        }

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
        //#region Address1
        //private string _Address1;
        //[Size(2000)]
        //public string Address1
        //{
        //    get { return _Address1; }
        //    set { SetPropertyValue(nameof(Address1), ref _Address1, value); }
        //}
        //#endregion
        #region Description
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }
        #endregion
        #region SiteName
        private string _SiteName;
        //[RuleRequiredField]
        //[RuleUniqueValue]
        public string SiteName
        {
            get { return _SiteName; }
            set { SetPropertyValue(nameof(SiteName), ref _SiteName, value); }
        }
        #endregion
        #region SamplePointID 
        private string _SamplePointID;
        //[RuleRequiredField]
        //[RuleUniqueValue]
        public string SamplePointID
        {
            get { return _SamplePointID; }
            set { SetPropertyValue(nameof(SamplePointID), ref _SamplePointID, value); }
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
        #region ProjectID
        private Project _ProjectID;
        //[DataSourceProperty("ProjectDataSource", DataSourcePropertyIsNullMode.SelectNothing)]
        [ImmediatePostData(true)]
        public Project ProjectID
        {
            get { return _ProjectID; }
            set { SetPropertyValue(nameof(ProjectID), ref _ProjectID, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Project> ProjectDataSource
        {
            get
            {
                if (Client != null)
                {
                    XPCollection<Project> listprojects = new XPCollection<Project>(Session, CriteriaOperator.Parse("[customername.Oid] = ? ", Client.Oid));
                    return listprojects;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region ProjectName
        private string _ProjectName;
        [NonPersistent]
        [ImmediatePostData]
        public string ProjectName
        {
            get
            {
                if (Client != null)
                {
                    XPCollection<COCSettingsSamples> lstOfProjects = new XPCollection<COCSettingsSamples>(Session, CriteriaOperator.Parse("[COCID.ClientName.Oid] = ? And [StationLocation.Oid] = ?", Client.Oid, Oid));
                    _ProjectName = string.Join(", ", lstOfProjects.Where(c => c.COCID.ProjectID != null && c.COCID.ProjectID.ProjectName != null).Select(c => c.COCID.ProjectID.ProjectName).Distinct());
                }
                return _ProjectName;
            }
            set { SetPropertyValue(nameof(ProjectName), ref _ProjectName, value); }
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
        private string _SiteNameArchived;
        public string SiteNameArchived
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
        //#region SamplingPointID
        //private string _SamplingPointID;
        //public string SamplingPointID
        //{
        //    get { return _SamplingPointID; }
        //    set { SetPropertyValue(nameof(SamplingPointID), ref _SamplingPointID, value); }
        //}
        //#endregion
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

        #region SampleType
        private SampleType _SampleType;
        public SampleType SampleType
        {
            get { return _SampleType; }
            set { SetPropertyValue(nameof(SampleType), ref _SampleType, value); }
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
        private CustomSystemUser _EnteredBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser EnteredBy
        {
            get { return _EnteredBy; }
            set { SetPropertyValue(nameof(EnteredBy), ref _EnteredBy, value); }
        }
        #endregion
        #region EnteredDate
        private DateTime _EnteredDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime EnteredDate
        {
            get { return _EnteredDate; }
            set { SetPropertyValue(nameof(EnteredDate), ref _EnteredDate, value); }
        }
        #endregion
        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #endregion
        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region LastUpdatedBy
        private CustomSystemUser _LastUpdatedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser LastUpdatedBy
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
        private string _StationLocation;
        public string StationLocation

        {
            get { return _StationLocation; }
            set { SetPropertyValue(nameof(StationLocation), ref _StationLocation, value); }
        }
        #endregion

        #region Depth
        private string _Depth;
        public string Depth

        {
            get { return _Depth; }
            set { SetPropertyValue(nameof(Depth), ref _Depth, value); }
        }
        #endregion
        #region SampleID
        private string _SampleID;
        public string SampleID

        {
            get { return _SampleID; }
            set { SetPropertyValue(nameof(SampleID), ref _SampleID, value); }
        }
        #endregion
        #region SampleNo
        private string _SampleNo;
        public string SampleNo
        {
            get { return _SampleNo; }
            set { SetPropertyValue(nameof(SampleNo), ref _SampleNo, value); }
        }
        #endregion
        #region Latitude
        private string _Latitude;
        public string Latitude
        {
            get { return _Latitude; }
            set { SetPropertyValue(nameof(Latitude), ref _Latitude, value); }
        }
        #endregion
        #region Longitude
        private string _Longitude;
        public string Longitude
        {
            get { return _Longitude; }
            set { SetPropertyValue(nameof(Longitude), ref _Longitude, value); }
        }
        #endregion
        #region SampleMatrix
        private VisualMatrix _SampleMatrix;
        public VisualMatrix SampleMatrix
        {
            get { return _SampleMatrix; }
            set { SetPropertyValue(nameof(SampleMatrix), ref _SampleMatrix, value); }
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

        #region Qty
        private uint _Qty;
        public uint Qty
        {
            get { return _Qty; }
            set { SetPropertyValue(nameof(Qty), ref _Qty, value); }
        }
        #endregion

        #region Collector
        private Collector _Collector;
        [DataSourceProperty(nameof(CollectorDataSource))]
        public Collector Collector
        {
            get { return _Collector; }
            set { SetPropertyValue<Collector>("Collector", ref _Collector, value); }
        }

        [NonPersistent]
        public XPCollection<Collector> CollectorDataSource
        {
            get
            {
                if (Client != null)
                {
                    return new XPCollection<Collector>(Session, CriteriaOperator.Parse("[CustomerName] Is Null or [CustomerName.Oid] = ?", Client.Oid));
                }
                else
                {
                    return new XPCollection<Collector>(Session, CriteriaOperator.Parse("[CustomerName] Is Null"));
                }
            }
        }
        #endregion

        #region CollectorPhone
        private string _CollectorPhone;
        public string CollectorPhone
        {
            get
            {
                if (Collector != null)
                {
                    _CollectorPhone = Collector.ContactPhone;
                }
                return _CollectorPhone;
            }
            set { SetPropertyValue(nameof(CollectorPhone), ref _CollectorPhone, value); }
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

        //#region FreeColorine
        //private string _FreeColorine;
        //[XafDisplayName("FreeColorine(mg/L)")]
        //public string FreeColorine
        //{
        //    get { return _FreeColorine; }
        //    set { SetPropertyValue(nameof(FreeColorine), ref _FreeColorine, value); }
        //}
        //#endregion

        //#region TotalChlorine
        //private string _TotalChlorine;
        //[XafDisplayName("TotalChlorine(mg/L)")]
        //public string TotalChlorine
        //{
        //    get { return _TotalChlorine; }
        //    set { SetPropertyValue(nameof(TotalChlorine), ref _TotalChlorine, value); }
        //}
        //#endregion

        #region RejectionCriteria
        private string _RejectionCriteria;
        [XafDisplayName("RejectionCriteria#")]
        public string RejectionCriteria
        {
            get { return _RejectionCriteria; }
            set { SetPropertyValue(nameof(RejectionCriteria), ref _RejectionCriteria, value); }
        }
        #endregion

        #region Customer
        private Customer _Customer;
        [Association("Customer-SampleSites")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Customer Customer

        {
            get { return _Customer; }
            set { SetPropertyValue(nameof(Customer), ref _Customer, value); }
        }
        #endregion

        #region DWQRTemplateSetup
        [Association("DWQRReportTemplateSetup-SampleSites")]
        public XPCollection<DWQRReportTemplateSetup> DWQRReportTemplateSetups
        {
            get { return GetCollection<DWQRReportTemplateSetup>(nameof(DWQRReportTemplateSetups)); }
        }
        #endregion

        //#region ReportID
        //private string _ReportID;
        //[ModelDefault("AllowEdit", "False")]
        //[NonPersistent]
        //public string ReportID
        //{
        //    get {
        //        if (_ReportID == null)
        //        {
        //            _ReportID = DWQRReportTemplateSetups.Select(j => j.ReportID).FirstOrDefault();
        //        }
        //        return _ReportID; 
        //    }
        //    set { SetPropertyValue(nameof(ReportID), ref _ReportID, value); }
        //}
        //#endregion
    }
}