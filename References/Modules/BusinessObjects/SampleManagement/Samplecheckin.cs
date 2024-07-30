using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using Image = Modules.BusinessObjects.Setting.Image;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty("JobID")]
    [Appearance("showClauses", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "Clauses;MethodNumber;", Criteria = "[IndoorInspection] = True Or [OutdoorInspection] = True", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("hideClauses", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "Clauses;MethodNumber;", Criteria = "[IndoorInspection] = False And [OutdoorInspection] = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("DropOffSamplesView", AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide, TargetItems = "DateExpect;COCSource;ProjectManager;", Criteria = "[IsSampling] = 'False'", Context = "DetailView")]
    [Appearance("SamplingSamplesView", AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide, TargetItems = "RelinquishedBy;DateCollected;Collector;ManifestNumbers;RecievedDate;RecievedBy,TimeCollected", Criteria = "[IsSampling] = 'True'", Context = "DetailView")]
    public class Samplecheckin : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public const string ProgressPropertyAlias = "ProgressStatus";
        viewInfo strviewid = new viewInfo();
        curlanguage curlanguage = new curlanguage();
        SampleRegistrationInfo objinfo = new SampleRegistrationInfo();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        ResultEntryQueryPanelInfo objQPInfo = new ResultEntryQueryPanelInfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        //List<Guid> assignedValidationTestMethods = new List<Guid>();
        //List<Guid> assignedApprovalTestMethods = new List<Guid>();
        //List<Guid> assignedTestMethods = new List<Guid>();
        Dictionary<double, string[]> statlist = new Dictionary<double, string[]>
                    {
                        { 0, new string[] { "", "Login" } },
                        { 5, new string[] { "IndoorInspection", "Indoor Inspection" } },
                        { 10, new string[] { "ProductSampleMapping", "Product and Sample Mapping" } },
                        { 22, new string[] { "SamplePreparation", "Sample Preparation" } },
                        { 24, new string[] { "PendingEntry", "Analysis" } },
                        { 37, new string[] { "PendingReview", "SDMS Review" } },
                        { 49, new string[] { "PendingVerify", "SDMS Verify" } },
                        { 62, new string[] { "PendingValidation", "Result Validation" } },
                        { 74, new string[] { "PendingApproval", "Result Approval" } },
                        { 87, new string[] { "PendingReporting", "Reporting" } }
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
        public Samplecheckin(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            if (!SRInfo.IsSampling)
            {
                RecievedDate = Library.GetServerTime(Session);
                RecievedBy = (Employee)Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                if (TAT == null)
                {
                    TAT = new XPCollection<TurnAroundTime>(Session, CriteriaOperator.Parse("[Default] = True")).ToList().FirstOrDefault();
                }
            }
            CreatedDate = Library.GetServerTime(Session);
            CreatedBy = (Employee)Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            NumberOfSample = 1;
            NoOfSamples = 1;
            objinfo.bolNewJobID = true;
            IsAlpacJobid = 1;
            StatusDefinition objStatus = Session.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] =9"));
            if (objStatus != null)
            {
                Index = objStatus;
            }
            PaymentStatus objPayment = Session.FindObject<PaymentStatus>(CriteriaOperator.Parse("[Default] =True "));
            if (objPayment != null)
            {
                PaymentStatus = objPayment;
            }
        }
        protected override void OnSaving()
        {
            //if (Convert.ToBoolean(Session.Evaluate(typeof(Samplecheckin), null, CriteriaOperator.Parse("[Oid] = ?", Oid))) == false)
            //{
            //    if (string.IsNullOrEmpty(JobID))
            //    {
            //        JobIDFormat obj = Session.FindObject<JobIDFormat>(CriteriaOperator.Parse(""));
            //        if (obj != null && obj.Dynamic == false)
            //        {
            //            var curdate = DateTime.Now;
            //            string strjobid = string.Empty;
            //            int formatlen = 0;

            //            if (obj.Year == YesNoFilter.Yes)
            //            {
            //                strjobid += curdate.ToString(obj.YearFormat.ToString());
            //                formatlen = obj.YearFormat.ToString().Length;
            //            }
            //            if (obj.Month == YesNoFilter.Yes)
            //            {
            //                strjobid += curdate.ToString(obj.MonthFormat.ToUpper());
            //                formatlen = formatlen + obj.MonthFormat.Length;
            //            }
            //            if (obj.Day == YesNoFilter.Yes)
            //            {
            //                strjobid += curdate.ToString(obj.DayFormat);
            //                formatlen = formatlen + obj.DayFormat.Length;
            //            }
            //            CriteriaOperator sam = obj.PrefixValue != null ? CriteriaOperator.Parse("Max(SUBSTRING(JobID, " + obj.PrefixValue.ToString().Length + "))") : CriteriaOperator.Parse("Max(SUBSTRING(JobID, 0))");
            //            string tempid = (Convert.ToInt32(Session.Evaluate(typeof(Samplecheckin), sam, null)) + 1).ToString();
            //            if (tempid != "1")
            //            {
            //                var predate = tempid.Substring(0, formatlen);
            //                if (predate == strjobid)
            //                {
            //                    if (obj.Prefix == YesNoFilter.Yes)
            //                    {
            //                        if (!string.IsNullOrEmpty(obj.PrefixValue))
            //                        {
            //                            strjobid = obj.PrefixValue + tempid;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        strjobid = tempid;
            //                    }
            //                }
            //                else
            //                {
            //                    if (obj.Prefix == YesNoFilter.Yes)
            //                    {
            //                        if (!string.IsNullOrEmpty(obj.PrefixValue))
            //                        {
            //                            strjobid = obj.PrefixValue + strjobid;
            //                        }
            //                    }
            //                    if (obj.SequentialNumber > 1)
            //                    {
            //                        if (obj.NumberStart > 0)
            //                        {
            //                            strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (obj.SequentialNumber - obj.NumberStart.ToString().Length)), '0') + obj.NumberStart;
            //                        }
            //                        else
            //                        {
            //                            strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (obj.SequentialNumber - 1)), '0') + "1";
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (obj.NumberStart > 0 && obj.NumberStart < 10)
            //                        {
            //                            strjobid = strjobid + obj.NumberStart;
            //                        }
            //                        else
            //                        {
            //                            strjobid = strjobid + "1";
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (obj.Prefix == YesNoFilter.Yes)
            //                {
            //                    if (!string.IsNullOrEmpty(obj.PrefixValue))
            //                    {
            //                        strjobid = obj.PrefixValue + strjobid;
            //                    }
            //                }
            //                if (obj.SequentialNumber > 1)
            //                {
            //                    if (obj.NumberStart > 0)
            //                    {
            //                        strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (obj.SequentialNumber - obj.NumberStart.ToString().Length)), '0') + obj.NumberStart;
            //                    }
            //                    else
            //                    {
            //                        strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (obj.SequentialNumber - 1)), '0') + "1";
            //                    }
            //                }
            //                else
            //                {
            //                    if (obj.NumberStart > 0 && obj.NumberStart < 10)
            //                    {
            //                        strjobid = strjobid + obj.NumberStart;
            //                    }
            //                    else
            //                    {
            //                        strjobid = strjobid + "1";
            //                    }
            //                }
            //            }
            //            JobID = strjobid;
            //        }
            //    }
            //}
        }

        #region JobID
        private string _JobID;
        [RuleUniqueValue]
        [RuleRequiredField("JobID", DefaultContexts.Save)]
        [Appearance("JobID", Context = "DetailView")]
        public string JobID
        {
            get
            {
                return _JobID;
            }
            set
            {
                SetPropertyValue<string>("JobID", ref _JobID, value);
            }
        }
        #endregion

        #region RecievedDate

        private DateTime _RecievedDate;
        //[RuleRequiredField("RecievedDate1", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public DateTime RecievedDate
        {
            get { return _RecievedDate; }
            set { SetPropertyValue<DateTime>("RecievedDate", ref _RecievedDate, value); }

        }
        #endregion

        #region RecievedBy
        private Employee _RecievedBy;
        //[RuleRequiredField("RecievedBy1", DefaultContexts.Save)]
        public Employee RecievedBy
        {
            get { return _RecievedBy; }
            set { SetPropertyValue<Employee>("RecievedBy", ref _RecievedBy, value); }
        }
        #endregion

        #region ClientName
        private Customer _ClientName;
        [RuleRequiredField("ClientName1", DefaultContexts.Save, CustomMessageTemplate="Client must not be empty.")]
        [ImmediatePostData(true)]
        public Customer ClientName
        {
            get
            {
                return _ClientName;
            }
            set
            {
                SetPropertyValue("ClientName", ref _ClientName, value);
            }
        }
        #endregion

        #region ClientAddress
        private string _ClientAddress;
        [ImmediatePostData(true)]
        [Appearance("CA", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        public string ClientAddress
        {
            get
            {
                if (ClientName != null)
                {
                    _ClientAddress = ClientName.Address;
                }
                else
                {
                    _ClientAddress = string.Empty;
                }
                return _ClientAddress;
            }
            set { SetPropertyValue<string>("ClientAddress", ref _ClientAddress, value); }

        }
        #endregion

        #region ClientAddress2
        private string _ClientAddress2;
        [ReadOnly(true)]
        [Appearance("CA2", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        //[DataSourceProperty("Customer.Address1", DataSourcePropertyIsNullMode.SelectNothing)]
        public string ClientAddress2
        {
            get { return _ClientAddress2; }
            set { SetPropertyValue<string>("ClientAddress2", ref _ClientAddress2, value); }

        }
        #endregion

        #region ClientPhone
        private string _ClientPhone;
        [ReadOnly(true)]
        [NonPersistent]
        [Appearance("CA1", Enabled = false, Context = "DetailView")]
        public string ClientPhone
        {
            get
            {
                if (ClientName != null)
                {
                    _ClientPhone = ClientName.OfficePhone;
                }
                else
                {
                    _ClientPhone = string.Empty;
                }
                return _ClientPhone;
            }
            set { SetPropertyValue(nameof(ClientPhone), ref _ClientPhone, value); }

        }
        #endregion
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Contact> Contacts
        {
            get
            {
                if (ClientName != null && ClientName.Oid != null)
                {
                    return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ? and IsReport =true", ClientName.Oid));
                }
                else
                {
                    return null;
                }
            }
        }
        private Contact _ClientContact;
        [DataSourceProperty("Contacts")]
        public Contact ClientContact
        {
            get { return _ClientContact; }
            set { SetPropertyValue(nameof(ClientContact), ref _ClientContact, value); }
        }

        #region Licence
        private string _License;
        [NonPersistent]
        public string License
        {
            get
            {
                if (ClientName != null)
                {
                    _License = ClientName.LicenseNumber;
                }
                return _License;
            }
            //set { SetPropertyValue<string>("License ", ref _License, value); }

        }
        #endregion
        #region ProjectId
        private Project _ProjectID;
        [ImmediatePostData(true)]
        [RuleRequiredField("projectid1",DefaultContexts.Save)]
        public Project ProjectID
        {
            get { return _ProjectID; }
            set
            {
                SetPropertyValue(nameof(ProjectID), ref _ProjectID, value);
            }
        }
        #endregion

        #region ProjectName
        private string _ProjectName;
        [ReadOnly(true)]
        [NonPersistent]
        [Appearance("CA3", Enabled = false, Context = "DetailView")]
        public string ProjectName
        {
            get
            {
                if (ProjectID != null && ProjectID.ProjectName != null)
                {
                    _ProjectName = ProjectID.ProjectName;
                }
                else
                {
                    _ProjectName = string.Empty;
                }
                return _ProjectName;
            }
            //set { SetPropertyValue<string>("ProjectName", ref _ProjectName, value); }
        }
        #endregion

        #region Project Location
        private string _projectLocation;
        [NonPersistent]
        [Size(300)]
        [ModelDefault("RowCount", "1")]
        [Appearance("ProjectLocation", Context = "DetailView", Criteria = "ProjectID is null", AppearanceItemType = "ViewItem", Enabled = false)]
        public string ProjectLocation
        {
            get
            {
                //if (ProjectID != null)
                //{
                //    _projectLocation = ProjectID.ProjectLocation;
                //}

                ////else
                ////{
                ////    _projectLocation = string.Empty;
                ////}
                return _projectLocation;
            }
            set => SetPropertyValue(nameof(ProjectLocation), ref _projectLocation, value);
        }
        #endregion

        #region ProjectCategory
        private ProjectCategory _ProjectCategory;
        public ProjectCategory ProjectCategory
        {
            get { return _ProjectCategory; }
            set { SetPropertyValue<ProjectCategory>("ProjectCategory", ref _ProjectCategory, value); }
        }
        #endregion
        #region ProjectSource
        private string _ProjectSource;
        public string ProjectSource
        {
            get { return _ProjectSource; }
            set { SetPropertyValue<string>("ProjectSource", ref _ProjectSource, value); }
        }
        #endregion
        #region ProjectOverview
        private string _ProjectOverview;
        public string ProjectOverview
        {
            get { return _ProjectOverview; }
            set { SetPropertyValue<string>("ProjectOverview", ref _ProjectOverview, value); }
        }
        #endregion
        #region ProjectCity
        private string _ProjectCity;
        public string ProjectCity
        {
            get { return _ProjectCity; }
            set { SetPropertyValue<string>("ProjectCity", ref _ProjectCity, value); }
        }
        #endregion

        #region BroughtBy
        private BroughtBy _BroughtBy;
        public BroughtBy BroughtBy
        {
            get { return _BroughtBy; }
            set { SetPropertyValue<BroughtBy>("BroughtBy", ref _BroughtBy, value); }

        }
        #endregion

        //#region InspectedClient
        //private string _InspectedClient;
        //[Size(SizeAttribute.DefaultStringMappingFieldSize)]
        //public string InspectedClient
        //{
        //    get { return _InspectedClient; }
        //    set { SetPropertyValue(nameof(InspectedClient), ref _InspectedClient, value); }
        //}
        //#endregion

        //#region InspectedClientAddress
        //private string _InspectedClientAddress;
        //[Size(SizeAttribute.DefaultStringMappingFieldSize)]
        //public string InspectedClientAddress
        //{
        //    get { return _InspectedClientAddress; }
        //    set { SetPropertyValue(nameof(InspectedClientAddress), ref _InspectedClientAddress, value); }
        //}
        //#endregion

        #region Manufacturer
        private string _Manufacturer;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { SetPropertyValue(nameof(Manufacturer), ref _Manufacturer, value); }
        }
        #endregion

        #region ManufacturerAddress
        private string _ManufacturerAddress;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ManufacturerAddress
        {
            get { return _ManufacturerAddress; }
            set { SetPropertyValue(nameof(ManufacturerAddress), ref _ManufacturerAddress, value); }
        }
        #endregion

        #region TradeMark
        private string _TradeMark;
        public string TradeMark
        {
            get { return _TradeMark; }
            set { SetPropertyValue(nameof(TradeMark), ref _TradeMark, value); }
        }
        #endregion

        #region ModelNumber
        private string _ModelNumber;
        public string ModelNumber
        {
            get { return _ModelNumber; }
            set { SetPropertyValue(nameof(ModelNumber), ref _ModelNumber, value); }
        }
        #endregion

        #region SampleName
        private string _SampleName;
        public string SampleName
        {
            get { return _SampleName; }
            set { SetPropertyValue(nameof(SampleName), ref _SampleName, value); }
        }
        #endregion

        #region SampleGrade
        private string _SampleGrade;
        public string SampleGrade
        {
            get { return _SampleGrade; }
            set { SetPropertyValue(nameof(SampleGrade), ref _SampleGrade, value); }
        }
        #endregion

        //#region SampleQuantity
        //private int _SampleQuantity;
        //public int SampleQuantity
        //{
        //    get
        //    {
        //        if (_SampleQuantity == 0)
        //        {
        //            _SampleQuantity = 1;
        //        }
        //        return _SampleQuantity;
        //    }
        //    set
        //    {
        //        SetPropertyValue<int>("SampleQuantity", ref _SampleQuantity, value);
        //    }
        //}
        //#endregion

        //#region SampleCheckin-VisualMatrix Relation 

        //private XPCollection<VisualMatrix> _VisualMatrixName;
        //[Association("SamplecheckinVisualMatrix", UseAssociationNameAsIntermediateTableName = true)]
        ////[RuleRequiredField("VisualMatrixName2", DefaultContexts.Save)]
        //public XPCollection<VisualMatrix> VisualMatrixName
        //{

        //    get
        //    {
        //        return GetCollection<VisualMatrix>("VisualMatrixName");

        //    }


        //}
        //#endregion

        //#region SampleCheckin-SampleCategory Relation
        //[Association("SampleCheckinSampleCategory", UseAssociationNameAsIntermediateTableName = true)]
        //public XPCollection<SampleCategory> SampleCategoryName
        //{
        //    get { return GetCollection<SampleCategory>("SampleCategoryName"); }
        //}
        //#endregion
        #region DueDate
        private Nullable<DateTime> _DueDate;
        //[RuleRequiredField("Enter The DueDate", DefaultContexts.Save)]
        //[ImmediatePostData]
        public Nullable<DateTime> DueDate
        {
            get { return _DueDate; }
            set { SetPropertyValue<Nullable<DateTime>>("DueDate", ref _DueDate, value); }
        }
        #endregion

        #region DateRequested
        private DateTime _DateRequested;
        public DateTime DateRequested
        {
            get { return _DateRequested; }
            set { SetPropertyValue<DateTime>("DateRequested", ref _DateRequested, value); }
        }
        #endregion


        private StatusDefinition _Index;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public StatusDefinition Index
        {
            get
            {
                return _Index;
            }
            set
            {
                SetPropertyValue<StatusDefinition>("Index", ref _Index, value);
            }
        }


        #region DateManufactured
        private DateTime _DateManufactured;
        public DateTime DateManufactured
        {
            get { return _DateManufactured; }
            set { SetPropertyValue<DateTime>("DateManufactured", ref _DateManufactured, value); }
        }
        #endregion

        #region PresevitiveCode
        private string _PresevitiveCode;
        public string PresevitiveCode
        {
            get { return _PresevitiveCode; }
            set { SetPropertyValue<string>("PresevitiveCode", ref _PresevitiveCode, value); }
        }
        #endregion

        #region CoolerId
        private string _CoolerId;
        public string CoolerId
        {
            get { return _CoolerId; }
            set { SetPropertyValue<string>("CoolerId", ref _CoolerId, value); }
        }
        #endregion

        #region CoolerTemp
        private string _CoolerTemp;
        public string CoolerTemp
        {
            get { return _CoolerTemp; }
            set { SetPropertyValue<string>("CoolerTemp", ref _CoolerTemp, value); }
        }
        #endregion

        #region NumberOfSample
        private int _NumberOfSample;
        public int NumberOfSample
        {
            get { return _NumberOfSample; }
            set { SetPropertyValue<int>("NumberOfSample", ref _NumberOfSample, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        [Appearance("MB9", Enabled = false, Context = "DetailView")]
        public Employee ModifiedBy
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
        [Appearance("MD9", Enabled = false, Context = "DetailView")]
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
        private XPObjectSpace ObjectSpace;

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

        #region Comment

        private bool _Comments;
        public bool Comments
        {
            get
            {
                return _Comments;
            }
            set
            {
                SetPropertyValue("Comment", ref _Comments, value);
            }
        }


        #endregion

        #region ProjectManager

        private Employee _ProjectManager;
        public Employee ProjectManager
        {
            get
            {
                return _ProjectManager;
            }
            set
            {
                SetPropertyValue("ProjectManager", ref _ProjectManager, value);
            }
        }


        #endregion

        #region Pre-InvoiceReport

        private string _PreInvoiceReport;
        public string PreInvoiceReport
        {
            get
            {
                return _PreInvoiceReport;
            }
            set
            {
                SetPropertyValue("PreInvoiceReport", ref _PreInvoiceReport, value);
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
                SetPropertyValue("Hold", ref _Hold, value);
            }
        }
        #endregion

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Contact> ContactsDataSource
        {
            get
            {
                if (ClientName != null && ClientName.Oid != null)
                {
                    XPCollection<Contact> lstconatcts = new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", ClientName.Oid));
                    if (lstconatcts.Count > 0)
                    {
                        if (lstconatcts != null && lstconatcts.Count == 1)
                        {
                            return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", ClientName.Oid));
                        }
                        else
                        {
                            return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", ClientName.Oid));
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
        }
        #region EmailList

        private Contact _EmailList;
        [DataSourceProperty("ContactsDataSource")]
        [NonPersistent]
        [ImmediatePostData]
        public Contact EmailList
        {
            get
            {
                if (ContactsDataSource != null && ContactsDataSource.Count == 1)
                {
                    _EmailList = ContactsDataSource.FirstOrDefault();
                }
                return _EmailList;
            }
            set
            {
                SetPropertyValue("EmailList", ref _EmailList, value);
            }
        }
        #endregion

        #region Email
        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                SetPropertyValue("Email", ref _Email, value);
            }
        }
        #endregion

        #region DateTimeSent

        private string _DateTimeSent;
        public string DateTimeSent
        {
            get
            {
                return _DateTimeSent;
            }
            set
            {
                SetPropertyValue("DateTimeSent", ref _DateTimeSent, value);
            }
        }
        #endregion

        #region SendBy

        private Employee _SendBy;
        public Employee SendBy
        {
            get
            {
                return _SendBy;
            }
            set
            {
                SetPropertyValue("SendBy", ref _SendBy, value);
            }
        }
        #endregion

        #region SendDate

        private Nullable<DateTime> _SendDate;
        public Nullable<DateTime> SendDate
        {
            get
            {
                return _SendDate;
            }
            set
            {
                SetPropertyValue("SendDate", ref _SendDate, value);
            }
        }
        #endregion
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

        #region SendingMethod

        private Samplereceiptsendingmethod _SendingMethod;
        public Samplereceiptsendingmethod SendingMethod
        {
            get
            {
                return _SendingMethod;
            }
            set
            {
                SetPropertyValue("SendingMethod", ref _SendingMethod, value);
            }
        }
        #endregion

        #region DateTimeSignedOff

        private DateTime _DateTimeSignedOff;
        public DateTime DateTimeSignedOff
        {
            get
            {
                return _DateTimeSignedOff;
            }
            set
            {
                SetPropertyValue("DateTimeSignedOff", ref _DateTimeSignedOff, value);
            }
        }
        #endregion
        #region DateTimeReceived

        private DateTime _DateTimeReceived;
        //[ImmediatePostData(true)]
        public DateTime DateTimeReceived
        {
            get
            {
                return _DateTimeReceived;
            }
            set
            {
                SetPropertyValue("DateTimeReceived", ref _DateTimeReceived, value);
            }
        }
        #endregion

        #region COCAttached

        private string _COCAttached;
        public string COCAttached
        {
            get
            {
                return _COCAttached;
            }
            set
            {
                SetPropertyValue("COCAttached", ref _COCAttached, value);
            }
        }
        #endregion

        #region TestDescription
        private string _TestDescription;
        [Size(1000)]
        public string TestDescription
        {
            get { return _TestDescription; }
            set { SetPropertyValue("TestDescription", ref _TestDescription, value); }
        }


        #endregion
        #region SampleImage
        private byte[] _SampleImage;
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 100)]
        public byte[] SampleImage
        {
            get
            {
                return _SampleImage;
            }
            set
            {
                SetPropertyValue("SampleImage", ref _SampleImage, value);
            }
        }
        #endregion

        private FileData _UploadCoc;

        public FileData UploadCOC
        {
            get { return _UploadCoc; }
            set { SetPropertyValue("UploadCOC", ref _UploadCoc, value); }
        }


        #region NoOfDaysRemaining

        private int _NoOfDaysRemaining;
        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public int NoOfDaysRemaining
        {
            get
            {
                if (DueDate != null && DueDate.HasValue)
                {
                    _NoOfDaysRemaining = Convert.ToInt32((DueDate.Value - DateTime.Now.Date).TotalDays);
                }
                return _NoOfDaysRemaining;
            }
            //set
            //{
            //    SetPropertyValue<int>("NoOfDaysRemaining", ref _NoOfDaysRemaining, value);
            //}
        }
        #endregion

        #region NoOfSamples

        private uint _NoOfSamples;
        //This Property for sample login.
        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public uint NoOfSamples
        {
            get
            {

                if (!IsLoading && JobID != null && SRInfo.isNoOfSampleDisable)
                {
                    _NoOfSamples = Convert.ToUInt32(Session.Evaluate(typeof(SampleLogIn), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid] = ?", Oid)));
                    //SelectedData sproc = Session.ExecuteSproc("GetSampleID", new OperandValue(JobID));
                    //// SampleLogIn sl = new SampleLogIn(currentSession);
                    //if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
                    //{
                    //    int SNo = 0;
                    //    //objSampleLogIn.SampleID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                    //   // SNo = Convert.ToInt32(sproc.ResultSet[1].Rows[0].Values[0].ToString().Split(new char[] { '-' }).GetValue(1));
                    //    _NoOfSamples = SNo - 1;
                    //}
                    //_NoOfSamples = SampleNo;
                }
                //else
                //if (SRInfo.IsSamplePopupClose == true)
                //{
                //    uint count = Convert.ToUInt32(Session.Evaluate(typeof(SampleLogIn), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid] = ?", Oid)));
                //    if (count != 0)
                //    {
                //        _NoOfSamples = count;
                //    }
                //    // SRInfo.IsSamplePopupClose = false;
                //}
                return _NoOfSamples;
            }
            set
            {
                SetPropertyValue<uint>("NoOfSamples", ref _NoOfSamples, value);
            }
        }
        #endregion


        #region SampleCheckin-Image Relation
        [Association("SampleCheckinImage", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Image> ImageUpload
        {
            get { return GetCollection<Image>("ImageUpload"); }
        }
        #endregion

        #region GroupSampleLogin
        [Association("SampleCheckinTest", UseAssociationNameAsIntermediateTableName = true)]

        public XPCollection<Testparameter> Testparameters
        {
            get { return GetCollection<Testparameter>("Testparameters"); }
        }

        private VisualMatrix _GroupVisualMatrix;
        [ImmediatePostData]
        public VisualMatrix GroupVisualMatrix
        {
            get { return _GroupVisualMatrix; }
            set { SetPropertyValue("GroupVisualMatrix", ref _GroupVisualMatrix, value); }
        }

        #endregion

        private string _ClientSampleID;
        [NonPersistent]
        public string ClientSampleID
        {
            get { return _ClientSampleID; }
            set { SetPropertyValue("ClientSampleID", ref _ClientSampleID, value); }
        }

        private DateTime _CollectionDate;
        [NonPersistent]
        public DateTime CollectionDate
        {
            get { return _CollectionDate; }
            set { SetPropertyValue("CollectionDate", ref _CollectionDate, value); }
        }

        #region TimeCollected
        private TimeSpan _CollectionTime;
        //[ImmediatePostData(true)]
        //[XafDisplayName("CollectedTime")]
        public TimeSpan CollectionTime
        {
            get { return _CollectionTime; }
            set { SetPropertyValue("CollectionTime", ref _CollectionTime, value); }
        }
        private string _TimeCollected;
        [ImmediatePostData(true)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
       // [NonPersistent]
        public string TimeCollected
        {
            get { return _TimeCollected; }
            set { SetPropertyValue("TimeCollected", ref _TimeCollected, value); }
        }
        #endregion
        #region Collector
        private Collector _Collector;
        [DataSourceProperty(nameof(CollectorDataSource))]
        [ImmediatePostData(true)]
        public Collector Collector
        {
            get { return _Collector; }
            set { SetPropertyValue("Collector", ref _Collector, value); }
        }

        public XPCollection<Collector> CollectorDataSource
        {
            get
            {
                if (ClientName != null)
                {
                    return  new XPCollection<Collector>(Session, CriteriaOperator.Parse("[CustomerName.Oid] = ?", ClientName.Oid), new SortProperty("FirstName", SortingDirection.Ascending));
                    //XPView lstview = new XPView(Session, typeof(Collector));
                    //lstview.Criteria = new InOperator("Oid", names.Select(i => i.Oid));
                    //lstview.Properties.Add(new ViewProperty("TCustomerName", SortDirection.Ascending, "FirstName", true, true));
                    //lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //List<object> groups = new List<object>();
                    //foreach (ViewRecord rec in lstview)
                    //    groups.Add(rec["Toid"]);
                    //return new XPCollection<Collector>(Session, new InOperator("Oid", groups));
                }
                else
                {
                    return new XPCollection<Collector>(Session, CriteriaOperator.Parse("Oid is null"));
                }
            }
        }
        #endregion
        private string _SampleLocation;
        [NonPersistent]
        public string SampleLocation
        {
            get { return _SampleLocation; }
            set { SetPropertyValue("SampleLocation", ref _SampleLocation, value); }
        }

        private Contact _ContactName;
        public Contact ContactName
        {
            get { return _ContactName; }
            set
            {
                SetPropertyValue("ContactName", ref _ContactName, value);
            }
        }
        public XPCollection<Contact> InvoiceContacts
        {
            get
            {
                if (ClientName != null && ClientName.Oid != null)
                {
                    return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ? and IsInvoice = True ", ClientName.Oid));
                }
                else
                {
                    return null;
                }
            }
        }
        private Contact _InvoiceContact;
        public Contact InvoiceContact
        {
            get { return _InvoiceContact; }
            set
            {
                SetPropertyValue("InvoiceContact", ref _InvoiceContact, value);
            }
        }

        private string _PurchaseOrder;
        public string PurchaseOrder
        {
            get { return _PurchaseOrder; }
            set { SetPropertyValue("PurchaseOrder", ref _PurchaseOrder, value); }
        }

        private string _SalesOrder;

        public string SalesOrder
        {
            get { return _SalesOrder; }
            set { SetPropertyValue("SaleseOrder", ref _SalesOrder, value); }
        }

        #region InspectionCategory
        private InspectCategory _InspectionCategory;
        //[RuleRequiredField]
        public InspectCategory InspectionCategory
        {
            get { return _InspectionCategory; }
            set { SetPropertyValue<InspectCategory>(nameof(InspectionCategory), ref _InspectionCategory, value); }
        }
        #endregion

        #region Status
        SampleRegistrationSignoffStatus _status;
        [ReadOnly(true)]
        public SampleRegistrationSignoffStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }
        #endregion
        #region Status
        SampleReceiptNotificationStatus _MailStatus;
        [ReadOnly(true)]
        public SampleReceiptNotificationStatus MailStatus { get => _MailStatus; set => SetPropertyValue(nameof(MailStatus), ref _MailStatus, value); }
        #endregion    

        #region Status
        ReportStatus _ReportStatus;
        [ReadOnly(true)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public ReportStatus ReportStatus { get => _ReportStatus; set => SetPropertyValue(nameof(ReportStatus), ref _ReportStatus, value); }
        #endregion


        #region InspectionStandard
        private string _InspectionStandard;
        [ModelDefault("RowCount", "3")]
        [Size(SizeAttribute.Unlimited)]
        public string InspectionStandard
        {
            get
            {
                return _InspectionStandard;
            }
            set
            {
                SetPropertyValue(nameof(InspectionStandard), ref _InspectionStandard, value);
            }
        }
        #endregion

        #region COCSource
        private Modules.BusinessObjects.Setting.COCSettings _COCSource;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Modules.BusinessObjects.Setting.COCSettings COCSource
        {
            get { return _COCSource; }
            set { SetPropertyValue(nameof(COCSource), ref _COCSource, value); }
        }
        #endregion
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Project> ProjectDataSource
        {
            get
            {
                if (ClientName != null)
                {
                    XPCollection<Project> listprojects = new XPCollection<Project>(Session, CriteriaOperator.Parse("[customername.Oid] = ? ", ClientName.Oid));
                    return listprojects;
                }
                else
                {
                    return null;
                }
            }
        }
        #region upload
        [DevExpress.ExpressApp.DC.Aggregated, Association("Sample-SampleUpload")]
        public XPCollection<SampleUpload> Photos
        {
            get { return GetCollection<SampleUpload>(nameof(Photos)); }
        }
        #endregion
        #region SampleCount
        private int _SampleCount;
        [NonPersistent]
        [VisibleInDashboards(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [XafDisplayName("#Samples")]
        public int SampleCount
        {
            get
            {
                if (strviewid.strtempviewid == "Samplecheckin_ListView_Copy_RegistrationSigningOff")
                {
                    return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [SignOff] = False", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();

                }
                //else if (strviewid.strtempviewid == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History")
                //{
                //    return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [SignOff] = True", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                //    //_SampleCount = Convert.ToInt32(Session.Evaluate(typeof(SampleLogIn), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid] = ? And ([SubOut] Is Null Or [SubOut] = False) And [JobID.Status] == 'Signedoff'", Oid)));
                //}
                else if (strviewid.strtempviewid == "Samplecheckin_ListView_Copy_Registration_History")
                {
                    _SampleCount = Convert.ToInt32(Session.Evaluate(typeof(SampleLogIn), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid] = ?", Oid)));
                }
                else if (strviewid.strtempresultentryviewid == "Samplecheckin_ListView_ResultEntry")
                {
                    return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Samplelogin.JobID.Status] = 'Signedoff' And [Status] = 'PendingEntry' And [SignOff] = True", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(); //([Status] = 'PendingReview' Or [Status] = 'PendingVerify' Or 
                }
                else if (strviewid.strtempresultentryviewid == "Samplecheckin_ListView_ResultView")
                {
                    return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Samplelogin.JobID.Status] = 'Signedoff' And [SignOff] = True", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(); //([Status] = 'PendingReview' Or [Status] = 'PendingVerify' Or 
                }
                else
                {
                    //  _SampleCount = Convert.ToInt32(Session.Evaluate(typeof(SampleLogIn), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid] = ? And [JobID.Status] = 'PendingSubmit'", Oid)));
                    _SampleCount = Convert.ToInt32(Session.Evaluate(typeof(SampleLogIn), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid] = ?", Oid)));
                }
                return _SampleCount;
            }
        }
        #endregion

        #region TestsCount
        private int _TestsCount;
        [NonPersistent]
        [VisibleInDashboards(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [XafDisplayName("#Tests")]
        public int TestsCount
        {
            get
            {
                if (strviewid.strtempviewid == "Samplecheckin_ListView_Copy_RegistrationSigningOff"/* || strviewid.strtempviewid == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History"*/)
                {
                    List<string> strtemp = new List<string>();
                    List<SampleBottleAllocation> lstBottles = new List<SampleBottleAllocation>();
                    if (strviewid.strtempviewid == "Samplecheckin_ListView_Copy_RegistrationSigningOff")
                    {
                        lstBottles = new XPCollection<SampleBottleAllocation>(Session, CriteriaOperator.Parse("[SampleRegistration.JobID.Oid] = ? And [SignOffBy] Is  Null And [SignOffDate] Is  Null", Oid)).ToList();
                    }
                    else
                    {
                        lstBottles = new XPCollection<SampleBottleAllocation>(Session, CriteriaOperator.Parse("[SampleRegistration.JobID.Oid] = ? And [SignOffBy] Is Not Null And [SignOffDate] Is Not Null", Oid)).ToList();
                    }
                    foreach (SampleBottleAllocation obj in lstBottles.ToList())
                    {
                        if (!strtemp.Contains(obj.TestMethod.TestName))
                        {
                            strtemp.Add(obj.TestMethod.TestName);
                        }
                    }
                    _TestsCount = strtemp.Count;
                }
                else
                {
                    var ListSamples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ?", Oid));
                    if (ListSamples.Count > 0)
                    {
                        _TestsCount = ListSamples.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.TestName != null).Select(i => i.Testparameter.TestMethod.TestName).Distinct().Count();
                    }
                }
                return _TestsCount;
            }
        }
        #endregion

        #region ContainerCount
        private int _ContainerCount;
        [NonPersistent]
        [VisibleInDashboards(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [XafDisplayName("#Containers")]
        public int ContainerCount
        {
            get
            {
                _ContainerCount = Convert.ToInt32(Session.Evaluate(typeof(SampleLogIn), CriteriaOperator.Parse("SUM(Qty)"), CriteriaOperator.Parse("[JobID.Oid] = ?", Oid)));
                //_ContainerCount = Convert.ToInt32(Session.Evaluate(typeof(SampleLogIn), CriteriaOperator.Parse("SUM(Qty)"), CriteriaOperator.Parse("[SampleRegistration.JobID.Oid] = ?", Oid)));
                return _ContainerCount;
            }
        }
        #endregion

        #region IndoorInspection
        private bool _IndoorInspection;
        [ImmediatePostData]
        public bool IndoorInspection
        {
            get
            {
                return _IndoorInspection;
            }
            set
            {
                SetPropertyValue<bool>(nameof(IndoorInspection), ref _IndoorInspection, value);
            }
        }
        #endregion

        #region OutdoorInspection
        private bool _OutdoorInspection;
        [ImmediatePostData]
        public bool OutdoorInspection
        {
            get
            {
                return _OutdoorInspection;
            }
            set
            {
                SetPropertyValue<bool>(nameof(OutdoorInspection), ref _OutdoorInspection, value);
            }
        }
        #endregion

        #region MethodNumber
        private Modules.BusinessObjects.Setting.Method _MethodNumber;
        //[ImmediatePostData]
        public Modules.BusinessObjects.Setting.Method MethodNumber
        {
            get
            {
                return _MethodNumber;
            }
            set
            {
                SetPropertyValue<Modules.BusinessObjects.Setting.Method>(nameof(MethodNumber), ref _MethodNumber, value);
            }
        }
        #endregion

        //#region Samplecheckins
        //[Association("Samplecheckins-Clause")]
        //public XPCollection<Setting.ClauseInspectionSettings> Clauses
        //{
        //    get
        //    {
        //        return GetCollection<Setting.ClauseInspectionSettings>(nameof(Clauses));
        //    }
        //}
        //#endregion

        #region ReportID
        private string _ReportID;
        public string ReportID
        {
            get { return _ReportID; }
            set { SetPropertyValue<string>(nameof(ReportID), ref _ReportID, value); }
        }
        #endregion

        #region SampleColor
        private string _SampleColor;
        public string SampleColor
        {
            get { return _SampleColor; }
            set { SetPropertyValue<string>(nameof(SampleColor), ref _SampleColor, value); }
        }
        #endregion

        #region EvaluationStandard
        private string _EvaluationStandard;
        public string EvaluationStandard
        {
            get { return _EvaluationStandard; }
            set { SetPropertyValue<string>(nameof(EvaluationStandard), ref _EvaluationStandard, value); }
        }
        #endregion

        #region ClientDataSource
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Customer> ClientDataSource
        {
            get
            {
                if (ProjectID != null)
                {
                    var lstCustomers = new XPCollection<Customer>(Session, CriteriaOperator.Parse("[Projects][[ProjectName] = ?]", ProjectID.ProjectName));
                    if (lstCustomers.Count > 0)
                    {
                        return new XPCollection<Customer>(Session, CriteriaOperator.Parse("[Projects][[ProjectName] = ?]", ProjectID.ProjectName));
                    }
                    else
                    {
                        return new XPCollection<Customer>(Session, CriteriaOperator.Parse("[Oid] is not null"));
                    }
                }
                else
                {
                    return new XPCollection<Customer>(Session, CriteriaOperator.Parse("[Oid] is not null"));
                }
            }
        }
        #endregion

        #region Unit
        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set { SetPropertyValue<string>(nameof(Unit), ref _Unit, value); }
        }
        #endregion

        #region ProductionBatch
        private string _ProductionBatch;
        public string ProductionBatch
        {
            get { return _ProductionBatch; }
            set { SetPropertyValue<string>(nameof(ProductionBatch), ref _ProductionBatch, value); }
        }
        #endregion

        #region ProgressStatus
        [EditorAlias(ProgressPropertyAlias)]
        [NonPersistent]
        public double ProgressStatus
        {
            get
            {
                if(JobID != null && !Session.IsObjectsSaving)
                {
                    Samplecheckin samplecheckin = Session.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", JobID));
                    var objProgress = Session.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Oid] = ?", samplecheckin.Index));
                    if (objProgress != null)
                    {
                        if (Index.Status == "Registration Submitted")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "TCLP Prepared")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Sample Prepared")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Entered")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "SDMS Result Entered")
                        {
                            return objProgress.Progress;
                        }

                        else if (Index.Status == "Level 2 Batch reviewed")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Level 3 Batch reviewed")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Approved")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Reported")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Report Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Report Delivered")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoiced")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoice Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoice Delivered")
                        {
                            return objProgress.Progress;
                        }

                    }

                }

                return 0;
               
                //int objSampleParameter = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", Oid)));
                //if (objSampleParameter > 0)
                //{
                //    //int PendingEntry = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] = 'PendingEntry'", Oid)));
                //    int Pendingsingingoff = Convert.ToInt32(Session.Evaluate(typeof(Samplecheckin), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID]=? and [Status] = 'PendingSigningOff'", JobID)));
                //    //int Analisis = Convert.ToInt32(Session.Evaluate(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[qcbatchID.Jobid] = ? and [AnalyticalBatchID] <> 'null'", JobID)));
                //    int Analisis = Convert.ToInt32(Session.Evaluate(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Jobid] = ? and [AnalyticalBatchID] <> 'null'", JobID)));
                //    //double IndoorInspection = Convert.ToDouble(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Samplelogin.JobID.IndoorInspection] = True", Oid)));
                //    int PendingReview = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] = 'PendingReview'", Oid)));
                //    int PendingVerify = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] = 'PendingVerify'", Oid)));
                //    int PendingValidation = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] = 'PendingValidation'", Oid)));
                //    int PendingApproval = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] = 'PendingApproval'", Oid)));
                //    int PendingReporting = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] = 'PendingReporting'", Oid)));
                //    XPCollection<SampleParameter> lstTests = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? ", Oid));
                //    int prepcount = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0).Count();
                //    int prepmethodcount = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0 && i.SignOff == true).Count();
                //    int prepmethodcountDone = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0 && i.SignOff == true).Count();
                //    //int PendingReporting1 = XafTypesInfo.Instance.FindTypeInfo(SampleParameter).FindMember(PropertyName).IsList
                //    int Reported = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] = 'Reported'", Oid)));
                //    if (objSampleParameter == Reported)
                //    {
                //        return 100;
                //    }
                //    if (objSampleParameter == PendingReporting  /*|| (PendingReporting > 0 && PendingApproval == 0 && PendingValidation == 0 && PendingVerify == 0 && PendingReview == 0 && PendingEntry == 0)*/)
                //    {
                //        if (prepmethodcountDone > 0)
                //        {
                //            return 88.8;
                //        }
                //        else
                //        {
                //            return 87.5;
                //        }

                //    }
                //    else if (objSampleParameter == PendingApproval /* || (PendingApproval > 0 && PendingValidation == 0 && PendingVerify == 0 && PendingReview == 0 && PendingEntry == 0)*/)
                //    {

                //        if (prepmethodcountDone > 0)
                //        {
                //            return 77.7;
                //        }
                //        else
                //        {
                //            return 75;
                //        }
                //    }
                //    else if (objSampleParameter == PendingValidation /* || (PendingValidation > 0 && PendingVerify == 0 && PendingReview == 0 && PendingEntry == 0)*/)
                //    {

                //        if (prepmethodcountDone > 0)
                //        {
                //            return 66.6;
                //        }
                //        else
                //        {
                //            return 62.5;
                //        }
                //    }
                //    else if (objSampleParameter == PendingVerify /* || (PendingVerify > 0 && PendingReview == 0 && PendingEntry == 0)*/)
                //    {
                //        if (prepmethodcountDone > 0)
                //        {
                //            return 55.5;
                //        }
                //        else
                //        {
                //            return 50;
                //        }
                //    }
                //    else if (objSampleParameter == PendingReview)
                //    {

                //        if (prepmethodcountDone > 0)
                //        {
                //            return 44.4;
                //        }
                //        else
                //        {
                //            return 37.5;
                //        }
                //    }
                //    else if (Pendingsingingoff == 0 && Analisis > 0)
                //    {
                //        if (prepmethodcountDone > 0)
                //        {
                //            return 33.3;
                //        }
                //        else
                //        {
                //            return 25;
                //        }
                //    }
                //    else if (prepmethodcountDone > 0)
                //    {
                //        return 22.2;
                //    }
                //    else
                //    {
                //        if (prepmethodcount > 0 || prepcount > 0)
                //        {
                //            return 11.1;
                //        }
                //        else
                //        {
                //            return 12.5;
                //        }
                //    }
                //}
                //else
                //{
                //    return 12.5;
                //}
            }
        }
        #endregion
        [EditorAlias(ProgressPropertyAlias)]
        [NonPersistent]
        public double ProjectTrackingStatus
        {
            get
            {

                if (JobID != null)
                {
                    Samplecheckin samplecheckin = Session.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", JobID));
                    var objProgress = Session.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Oid] = ?", samplecheckin.Index));
                    if (objProgress != null)
                    {
                        if (Index.Status == "Registration Submitted")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "TCLP Prepared")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Sample Prepared")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Entered")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "SDMS Result Entered")
                        {
                            return objProgress.Progress;
                        }

                        else if (Index.Status == "Level 2 Batch reviewed")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Level 3 Batch reviewed")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Approved")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Reported")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Report Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Report Delivered")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoiced")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoice Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoice Delivered")
                        {
                            return objProgress.Progress;
                        }

                    }

                }
                return 0;

                //double objSampleParameter = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", Oid)));
                //if (objSampleParameter > 0)
                //{
                //    double PendingEntry = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] != 'PendingEntry'", Oid)));
                //    return Math.Round((PendingEntry / objSampleParameter) * 100);
                //}
                //else
                //{
                //    return 0;
                //}
            }
        }


        #region NPJobStatus
        private double _NPJobStatus;
        [NonPersistent]

        public double JobStatus
        {
            get
            {
                if (!Session.IsObjectsSaving)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("FunctionType");
                    dt.Columns.Add("Status");
                    int objdone = 0;
                    int prepcountPartial = 0;
                    int totsamplecount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", Oid)));
                    foreach (KeyValuePair<double, string[]> curlist in statlist)
                    {
                        if (checklist.ContainsKey(curlist.Value[0]))
                        {
                            if (checklist.TryGetValue(curlist.Value[0], out string checkvalue))
                            {
                                DefaultSetting objDefaultSetting = Session.FindObject<DefaultSetting>(CriteriaOperator.Parse("" + checkvalue + " = 1"));
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
                                    int totcount = Convert.ToInt32(Session.Evaluate(type, CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid]=?", Oid)));
                                    int pendcount = Convert.ToInt32(Session.Evaluate(type, CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[JobID.Oid]=? and [Status] ='Completed'", Oid)));
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
                            XPCollection<SampleParameter> objSPs = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [SamplePrepBatchID] Is Null ", Oid));
                            XPCollection<SampleParameter> objSP1 = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [SamplePrepBatchID] Is Not Null ", Oid));
                            XPCollection<SampleParameter> objSP2 = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [SamplePrepBatchID] Is Null And [SignOff] = True", Oid));
                            int prepcounts = objSPs.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0).Count();
                            int prepcountdone = objSP1.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0).Count();
                            prepcountPartial = objSP2.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0).Count();
                            if (prepcounts > 0)
                            {
                                dataRow["Status"] = "Pending";
                            }
                            else if (prepcountdone > 0)
                            {
                                dataRow["Status"] = "Done";
                                objdone++;

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
                            if (curlist.Key < ProgressStatus)
                            {
                                dataRow["Status"] = "Done";
                                objdone++;

                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(curlist.Value[0]))
                                {
                                    int curstatcount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] =?", Oid, curlist.Value[0])));
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
                        if (!prep.ContainsKey(curlist.Value[0]))
                        {
                            dt.Rows.Add(dataRow);
                        }
                        else
                        {
                            XPCollection<SampleParameter> objSPs = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [SamplePrepBatchID] Is Null ", Oid));
                            int prepcounts = objSPs.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0).Count();
                            if (prepcounts > 0)
                            {
                                dt.Rows.Add(dataRow);
                            }
                        }
                        int totalCount = dt.Rows.Count;
                        double totalcounts = (double)(totalCount);
                        double totalobjdone = (double)(objdone);
                       _NPJobStatus = Math.Round((totalobjdone * 100) / (double)totalcounts);
                        //_NPJobStatus = (double)((objdone * 100) / totalCount);

                   }
                }
                return _NPJobStatus;
            }
            set { SetPropertyValue<double>(nameof(JobStatus), ref _NPJobStatus, value); }
        }
        #endregion

        private double _ProjectJobStatus;
        [NonPersistent]

        public double ProjectJobStatus
        {
            get
            {
                DataTable dt = new DataTable();
                int objdoneCount = 0;
                dt.Columns.Add("Test");
                dt.Columns.Add("Complete");
                dt.Columns.Add("Status");
                //Samplecheckin objsamplecheckin = Session.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?"));
                    IList<SampleParameter> parameters = Session.GetObjects(Session.GetClassInfo(typeof(SampleParameter)), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", Oid), null, int.MaxValue, false, true).Cast<SampleParameter>().ToList(); 
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
                        int totcount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Testparameter.TestMethod.TestName] =?",Oid, curtn)));
                        int pendcount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Testparameter.TestMethod.TestName] =? and [Status] = 'PendingEntry'", Oid, curtn)));
                        dataRow["Complete"] = (totcount - pendcount).ToString() + "/" + totcount.ToString();

                        if (pendcount == 0)
                        {
                            dataRow["Status"] = "Done";
                             objdoneCount++;
                        }
                        else
                        {
                            dataRow["Status"] = "Pending";
                        }
                    dt.Rows.Add(dataRow);
                    int totalCounts = dt.Rows.Count;
                    _ProjectJobStatus = (double)((objdoneCount * 100) / totalCounts);

                }

                return  _ProjectJobStatus;

            }
        }





        [NonPersistent]
        public string Reported
        {
            get
            {
                int objSampleParameter = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", Oid)));
                if (objSampleParameter > 0)
                {
                    int Reported = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] = 'Reported'", Oid)));
                    if (objSampleParameter == Reported)
                    {
                        if (curlanguage.strcurlanguage == "En")
                        {
                            return "Reported";
                        }
                        else
                        {
                            return "已报告";
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
        }
        #region RelinquishedBy
        private BroughtBy _RelinquishedBy;
        public BroughtBy RelinquishedBy
        {
            get { return _RelinquishedBy; }
            set { SetPropertyValue<BroughtBy>(nameof(RelinquishedBy), ref _RelinquishedBy, value); }
        }
        #endregion
        #region ManifestNumbers
        private uint _ManifestNumbers;
        public uint ManifestNumbers
        {
            get { return _ManifestNumbers; }
            set { SetPropertyValue<uint>(nameof(ManifestNumbers), ref _ManifestNumbers, value); }
        }
        #endregion
        #region SampleTemperature(°C)
        private string _SampleTemperature;
        public string SampleTemperature
        {
            get { return _SampleTemperature; }
            set { SetPropertyValue<string>(nameof(SampleTemperature), ref _SampleTemperature, value); }
        }
        #endregion
        #region BatchID
        private string _BatchID;
        public string BatchID
        {
            get { return _BatchID; }
            set { SetPropertyValue<string>(nameof(BatchID), ref _BatchID, value); }
        }
        #endregion
        #region PackageNo
        private string _PackageNo;
        public string PackageNo
        {
            get { return _PackageNo; }
            set { SetPropertyValue<string>(nameof(PackageNo), ref _PackageNo, value); }
        }
        #endregion
        #region PreviousWeight(g)
        private string _PreviousWeight;
        public string PreviousWeight
        {
            get { return _PreviousWeight; }
            set { SetPropertyValue<string>(nameof(PreviousWeight), ref _PreviousWeight, value); }
        }
        #endregion
        #region CurrentWeight(g)
        private string _CurrentWeight;
        public string CurrentWeight
        {
            get { return _CurrentWeight; }
            set { SetPropertyValue<string>(nameof(CurrentWeight), ref _CurrentWeight, value); }
        }
        #endregion
        #region Remark
        private string _Remark;
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue<string>(nameof(Remark), ref _Remark, value); }
        }
        #endregion
        private TurnAroundTime _TAT;
        [ImmediatePostData(true)]

        //[RuleRequiredField("TAT1", DefaultContexts.Save)]
        public TurnAroundTime TAT
        {
            get
            {
                return _TAT;



            }
            set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        }
        private string _ReportDeliveryMethod;
        public string ReportDeliveryMethod
        {
            get { return _ReportDeliveryMethod; }
            set { SetPropertyValue(nameof(ReportDeliveryMethod), ref _ReportDeliveryMethod, value); }
        }
        private string _ReportDeliveryAddress;
        public string ReportDeliveryAddress
        {
            get { return _ReportDeliveryAddress; }
            set { SetPropertyValue(nameof(ReportDeliveryAddress), ref _ReportDeliveryAddress, value); }
        }
        private string _ReportSpecification;
        public string ReportSpecification
        {
            get { return _ReportSpecification; }
            set { SetPropertyValue(nameof(ReportSpecification), ref _ReportSpecification, value); }
        }

        private bool _PendingResult;
        [NonPersistent]
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        public bool PendingResult
        {
            get
            {
                if (JobID != null)
                {

                }
                int Result = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and Not IsNullOrEmpty([ResultNumeric])", Oid)));
                int Total = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", Oid)));
                if (Result != Total)
                {
                    _PendingResult = true;
                }
                else
                {
                    _PendingResult = false;
                }
                return _PendingResult;
            }
        }
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(SampleMatries))
                {
                    List<string> lstSM = new List<string>();
                    List<string> lstSMOid = SampleMatries.Split(';').ToList();
                    if (lstSMOid != null)
                    {
                        foreach (string objOid in lstSMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                VisualMatrix objVM = Session.GetObjectByKey<VisualMatrix>(new Guid(objOid.Trim()));
                                if (objVM != null && !lstSM.Contains(objVM.MatrixName.MatrixName))
                                {
                                    lstSM.Add(objVM.MatrixName.MatrixName);
                                }
                            }

                        }
                    }
                    return new XPCollection<TestMethod>(Session, new InOperator("MatrixName.MatrixName", lstSM));
                }
                else
                {
                    return null;
                }
            }
        }
        #region TestSummary
        private string _TestSummary;
        public string TestSummary
        {
            get { return _TestSummary; }
            set { SetPropertyValue<string>(nameof(TestSummary), ref _TestSummary, value); }
        }
        #endregion
        //#region ContractTitle
        ////private string _ContractTitle;
        //public string ContractTitle
        //{
        //    get
        //    {
        //        if (Tasks != null && Tasks.Count > 0)
        //        {
        //            Tasks task = Tasks.FirstOrDefault();
        //            if (task != null && task.ContractTitle.ContractTitle != null)
        //            {
        //                return task.ContractTitle.ContractTitle;
        //            }
        //            else
        //            {
        //                return string.Empty;
        //            }
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //        //return _ContractTitle;
        //    }
        //    //set { SetPropertyValue<Contract>(nameof(ContractTitle), ref _ContractTitle, value); }
        //}
        //#endregion
        #region NoStations
        private int _NoStations;
        public int NoStations
        {
            get { return _NoStations; }
            set { SetPropertyValue<int>(nameof(NoStations), ref _NoStations, value); }
        }
        #endregion
        #region SamplingEquipment
        private string _SamplingEquipment;
        public string SamplingEquipment
        {
            get { return _SamplingEquipment; }
            set { SetPropertyValue<string>(nameof(SamplingEquipment), ref _SamplingEquipment, value); }
        }
        #endregion
        private Employee _RollbackedBy;
        public Employee RollbackedBy
        {
            get { return _RollbackedBy; }
            set { SetPropertyValue(nameof(RollbackedBy), ref _RollbackedBy, value); }
        }
        private DateTime _RollbackedDate;
        public DateTime RollbackedDate
        {
            get { return _RollbackedDate; }
            set { SetPropertyValue(nameof(RollbackedDate), ref _RollbackedDate, value); }
        }
        private String _RollbackReason;
        [Size(SizeAttribute.Unlimited)]
        public String RollbackReason
        {
            get { return _RollbackReason; }
            set { SetPropertyValue(nameof(_RollbackReason), ref _RollbackReason, value); }
        }
        //#region Tasks
        //[DevExpress.ExpressApp.DC.Aggregated, Association("Tasks_SampleCheckin")]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //public XPCollection<Tasks> Tasks
        //{
        //    get { return GetCollection<Tasks>(nameof(Tasks)); }
        //}
        //#endregion
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<SampleCategory> SampleCategorys
        {
            get
            {
                return new XPCollection<SampleCategory>(Session, CriteriaOperator.Parse(""));
            }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<VisualMatrix> SampleMatrixes
        {
            get
            {
                XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session);
                return new XPCollection<VisualMatrix>(Session,new GroupOperator(GroupOperatorType.And,CriteriaOperator.Parse("[IsRetired] <> True Or [IsRetired] Is Null"),new InOperator("MatrixName.Oid", tests.Where(i => i.MatrixName != null).Select(i => i.MatrixName.Oid).Distinct().ToList())));
            }
        }
        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "SampleMatries" && SampleMatrixes != null && SampleMatrixes.Count > 0)
            {
                Properties = SampleMatrixes.OrderBy(i => i.VisualMatrixName).ToDictionary(x => (Object)x.Oid, x => x.VisualMatrixName);
                //foreach (VisualMatrix objSampleMatrix in SampleMatrixes.Where(i => i.VisualMatrixName != null).OrderBy(i => i.VisualMatrixName).ToList())
                //{
                //    if (!Properties.ContainsKey(objSampleMatrix.Oid) && lstmatrix.Contains(objSampleMatrix.MatrixName.Oid))
                //    {
                //        Properties.Add(objSampleMatrix.Oid, objSampleMatrix.VisualMatrixName);
                //    }
                //}
            }
            if (targetMemberName == "SampleCategory" && SampleCategorys != null && SampleCategorys.Count > 0)
            {
                Properties = SampleCategorys.Where(i => i.SampleCategoryName != null).OrderBy(i => i.SampleCategoryName).ToDictionary(x => (object)x.Oid, x => x.SampleCategoryName);
                //foreach (SampleCategory objCategory in SampleCategorys.Where(i => i.SampleCategoryName != null).OrderBy(i => i.SampleCategoryName).ToList())
                //{
                //    if (!Properties.ContainsKey(objCategory.Oid))
                //    {
                //        Properties.Add(objCategory.Oid, objCategory.SampleCategoryName);
                //    }
                //}
            }
            if (targetMemberName == "TestName" && TestDataSource != null && TestDataSource.Count > 0)
            {
                foreach (TestMethod objTest in TestDataSource.Where(i => i.TestName != null).OrderBy(i => i.TestName).ToList())
                {
                    if (!Properties.ContainsValue(objTest.TestName))
                    {
                        Properties.Add(objTest.Oid, objTest.TestName);
                    }
                }
            }
            return Properties;
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
        private string _SampleMatries;
        [RuleRequiredField]
        [XafDisplayName("Sample Matrix")]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string SampleMatries
        {
            get { return _SampleMatries; }
            set { SetPropertyValue(nameof(SampleMatries), ref _SampleMatries, value); }
        }
        private string _SampleCategory;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string SampleCategory
        {
            get { return _SampleCategory; }
            set { SetPropertyValue(nameof(SampleCategory), ref _SampleCategory, value); }
        }
        [Association("SampleRegistration-SampleConditionCheck")]
        public XPCollection<SampleConditionCheck> SampleConditionCheck
        {
            get { return GetCollection<SampleConditionCheck>("SampleConditionCheck"); }
        }
        #region Attachments
        [Association("SampleRegistration-Attachments")]
        public XPCollection<Attachment> Attachments
        {
            get { return GetCollection<Attachment>("Attachments"); }
        }
        #endregion
        #region Notes
        [DevExpress.ExpressApp.DC.Aggregated, Association("Notes_SampleCheckIn")]
        public XPCollection<Notes> Notes
        {
            get { return GetCollection<Notes>(nameof(Notes)); }
        }
        #endregion
        private string _TestName;
        //[RuleRequiredField]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [ImmediatePostData]
        public string TestName
        {
            get
            {
                if (string.IsNullOrEmpty(SampleMatries))
                {
                    _TestName = null;
                }
                return _TestName;
            }
            set { SetPropertyValue(nameof(TestName), ref _TestName, value); }
        }
        #region CustomDueDateCollection
        [DevExpress.ExpressApp.DC.Aggregated, Association("SampleCheckIn_CustomDueDate")]
        public XPCollection<CustomDueDate> CustomDueDates
        {
            get { return GetCollection<CustomDueDate>(nameof(CustomDueDates)); }
        }
        #endregion
        //#region ItemChargePricingCollection
        //[DevExpress.ExpressApp.DC.Aggregated, Association("SampleCheckIn_ItemChargePricing")]
        //public XPCollection<ItemChargePricing> ItemChargePricings
        //{
        //    get { return GetCollection<ItemChargePricing>(nameof(ItemChargePricings)); }
        //}
        //#endregion
        private Labware _BalanceID;
        public Labware BalanceID
        {
            get { return _BalanceID; }
            set { SetPropertyValue(nameof(BalanceID), ref _BalanceID, value); }
        }
        private string _ReasonforWeighing;
        [Size(SizeAttribute.Unlimited)]
        public string ReasonforWeighing
        {
            get { return _ReasonforWeighing; }
            set { SetPropertyValue(nameof(ReasonforWeighing), ref _ReasonforWeighing, value); }
        }
        #region BalanceCalibrationID
        private string _BalanceCalibrationID;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string BalanceCalibrationID
        {
            get { return _BalanceCalibrationID; }
            set { SetPropertyValue(nameof(BalanceCalibrationID), ref _BalanceCalibrationID, value); }
        }
        #endregion
        #region ReportTemplate
        private string _ReportTemplate;
        [EditorAlias("ReportTemplatePropertyEditor")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string ReportTemplate
        {
            get
            {
                return _ReportTemplate;
            }
            set { SetPropertyValue(nameof(ReportTemplate), ref _ReportTemplate, value); }
        }
        #endregion

        #region QuoteID
        private CRMQuotes _QuoteID;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DataSourceProperty("QuotesDataSource")]
        [ImmediatePostData]
        public CRMQuotes QuoteID
        {
            get { return _QuoteID; }
            set { SetPropertyValue(nameof(QuoteID), ref _QuoteID, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<CRMQuotes> QuotesDataSource
        {
            get
            {
                if (ClientName != null)
                {
                    XPCollection<CRMQuotes> lstquotes = new XPCollection<CRMQuotes>(Session, CriteriaOperator.Parse("[Client.Oid]=? And [ExpirationDate] >= ? And [Status] = 'Reviewed'", ClientName.Oid, DateTime.Now.Date));
                    List<Guid> lstquotesOid = new List<Guid>();
                    List<string> lstquoteID = new List<string>();
                    foreach (CRMQuotes objOid in lstquotes)
                    {
                        if (!lstquoteID.Contains(objOid.QuoteID))
                        {
                            lstquoteID.Add(objOid.QuoteID);
                            lstquotesOid.Add(objOid.Oid);
                        }
                    }
                    XPView testsView = new XPView(Session, typeof(CRMQuotes));
                    testsView.Criteria = new InOperator("Oid", lstquotes.Select(i => i.Oid));
                    testsView.Properties.Add(new ViewProperty("TQuoteID", SortDirection.Ascending, "QuoteID", true, true));
                    testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    List<object> groups = new List<object>();
                    foreach (ViewRecord rec in testsView)
                        groups.Add(rec["Toid"]);
                    return new XPCollection<CRMQuotes>(Session, new InOperator("Oid", groups));
                }
                else
                {
                    return null;
                }
            }

        }
        #endregion

        //#region COCID
        //private COCSettings _COCID;
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[DataSourceProperty("COCDataSource")]
        //[ImmediatePostData]
        //public COCSettings COCID
        //{
        //    get { return _COCID; }
        //    set { SetPropertyValue(nameof(COCID), ref _COCID, value); }
        //}

        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<COCSettings> COCDataSource
        //{
        //    get
        //    {
        //        if (ClientName != null)
        //        {
        //            XPCollection<COCSettings> lstquotes = new XPCollection<COCSettings>(Session, CriteriaOperator.Parse("[ClientName.Oid]=? And [RetireDate] >= ?", ClientName.Oid, DateTime.Now.Date));
        //            List<Guid> lstquotesOid = new List<Guid>();
        //            List<string> lstquoteID = new List<string>();
        //            foreach (COCSettings objOid in lstquotes)
        //            {
        //                if (!lstquoteID.Contains(objOid.COC_ID))
        //                {
        //                    lstquoteID.Add(objOid.COC_ID);
        //                    lstquotesOid.Add(objOid.Oid);
        //                }
        //            }
        //            XPView testsView = new XPView(Session, typeof(COCSettings));
        //            testsView.Criteria = new InOperator("Oid", lstquotes.Select(i => i.Oid));
        //            testsView.Properties.Add(new ViewProperty("TCOCID", SortDirection.Ascending, "COC_ID", true, true));
        //            testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
        //            List<object> groups = new List<object>();
        //            foreach (ViewRecord rec in testsView)
        //                groups.Add(rec["Toid"]);
        //            return new XPCollection<COCSettings>(Session, new InOperator("Oid", groups));
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //}
        //#endregion

        #region InvoiceStatus
        private Modules.BusinessObjects.Hr.InvoiceStatus _InvoiceStatus;
        [XafDisplayName("Status")]
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Modules.BusinessObjects.Hr.InvoiceStatus InvoiceStatus
        {
            get
            {
                return _InvoiceStatus;
            }
            set { SetPropertyValue(nameof(InvoiceStatus), ref _InvoiceStatus, value); }
        }
        #endregion

        #region SampleParameterStatus
        private SampleParameter _SampleParameterStatus;
        [XafDisplayName("Status")]
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public SampleParameter SampleParameterStatus
        {
            get
            {
                if (strviewid.strtempresultentryviewid == "Samplecheckin_ListView_ResultEntry")
                {
                    _SampleParameterStatus = Session.FindObject<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Status] = 'PendingEntry'", Oid));
                }
                else if (strviewid.strtempresultentryviewid == "Samplecheckin_ListView_ResultView")
                {
                    _SampleParameterStatus = Session.FindObject<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Status] >= 'PendingEntry'", Oid));
                }
                else if (strviewid.strtempresultentryviewid == "Samplecheckin_ListView_Copy_Reporting")
                {
                    _SampleParameterStatus = Session.FindObject<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Status] >= 'PendingReporting'", Oid));
                }
                return _SampleParameterStatus;
            }
            set { SetPropertyValue(nameof(Samplestatus), ref _SampleParameterStatus, value); }
        }
        #endregion
        #region BatchInvoice
        #region DateReceived From
        private DateTime _DateReceivedFrom;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("DateReceivedFrom", Context = "DetailView")]
        public DateTime DateReceivedFrom
        {
            get { return _DateReceivedFrom; }
            set
            {
                SetPropertyValue("DateReceivedFrom", ref _DateReceivedFrom, value);
            }
        }
        #endregion
        #region DateReceived To
        private DateTime _DateReceivedTo;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("DateReceivedTo", Context = "DetailView")]
        public DateTime DateReceivedTo
        {
            get { return _DateReceivedTo; }
            set
            {
                SetPropertyValue("DateReceivedTo", ref _DateReceivedTo, value);
            }
        }
        #endregion
        #region DateReportedFrom
        private DateTime _DateReportedFrom;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("DateReportedFrom", Context = "DetailView")]
        public DateTime DateReportedFrom
        {
            get { return _DateReportedFrom; }
            set
            {
                SetPropertyValue("DateReportedFrom", ref _DateReportedFrom, value);
            }
        }
        #endregion
        #region DateReportedTo
        private DateTime _DateReportedTo;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("DateReportedTo", Context = "DetailView")]
        public DateTime DateReportedTo
        {
            get 
            { 
                if(_DateReportedTo== DateTime.MinValue)
                {
                    _DateReportedTo= Library.GetServerTime(Session);
                }
                return _DateReportedTo; 
            }
            set
            {
                SetPropertyValue("DateReportedTo", ref _DateReportedTo, value);
            }
        }
        #endregion
        #endregion

        private Samplestatus _NPSampleStatus;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [NonPersistent]
        public Samplestatus NPSampleStatus
        {
            get
            {
                if (objnavigationRefresh.ClickedNavigationItem == "Result Validation")
                {
                    return Samplestatus.PendingValidation;
                }
                else if (objnavigationRefresh.ClickedNavigationItem == "Result Approval")
                {
                    return Samplestatus.PendingApproval;
                }
                else
                {
                    return _NPSampleStatus;
                }
            }
        }
        #region ItemChargePricingCollection
        [DevExpress.ExpressApp.DC.Aggregated, Association("SampleCheckin-SCItemChargePrice")]
        public XPCollection<SampleCheckinItemChargePricing> SCItemCharges
        {
            get { return GetCollection<SampleCheckinItemChargePricing>(nameof(SCItemCharges)); }
        }
        #endregion
        private string _NPTest;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        [Size(SizeAttribute.Unlimited)]
        //[NonPersistent]
        public string NPTest
        {
            get
            {
                return _NPTest;
            }
            set { SetPropertyValue("NPTest", ref _NPTest, value); }
        }

        private string _Test;
        [ImmediatePostData(true)]
        [XafDisplayName("Test Name")]
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public string Test
        {
            get { return _Test; }
            set { SetPropertyValue("Test", ref _Test, value); }
        }
        private string _PO;
        [XafDisplayName("PO #")]
        public string PO
        {
            get { return _PO; }
            set { SetPropertyValue("PO", ref _PO, value); }
        }
        #region IsAlpacJobid
        private int _IsAlpacJobid;
        [Browsable(false)]
        public int IsAlpacJobid
        {
            get { return _IsAlpacJobid; }
            set { SetPropertyValue("IsAlpacJobid", ref _IsAlpacJobid, value); }
        }
        #endregion
        #region DateCollected
        private Nullable<DateTime> _DateCollected;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        [ImmediatePostData]
        //[RuleRequiredField("DateCollected1",DefaultContexts.Save)]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public Nullable<DateTime> DateCollected
        {
            get { return _DateCollected; }
            set { SetPropertyValue("DateCollected", ref _DateCollected, value); }
        }
        #endregion
        #region PaymentStatus
        private PaymentStatus _PaymentStatus;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false),VisibleInLookupListView(false)]
        [ModelDefault("LookupProperty", "Status")]
        public PaymentStatus PaymentStatus
        {
            get { return _PaymentStatus; }
            set { SetPropertyValue("PaymentStatus", ref _PaymentStatus, value); }
        }
        #endregion
        private bool _Isinvoicesummary;
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        public bool Isinvoicesummary
        {
            get { return _Isinvoicesummary; }
            set
            {
                SetPropertyValue("Isinvoicesummary", ref _Isinvoicesummary, value);
            }
        }
        #region TestEditSampleLogIn
        private SampleLogIn _TestEditSampleLogIn;
        [NonPersistent]
        [DataSourceProperty(nameof(TestEditDataSource))]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        [ImmediatePostData]
        public SampleLogIn TestEditSampleLogIn
        {
            get { return _TestEditSampleLogIn; }
            set { SetPropertyValue("TestEditSampleLogIn", ref _TestEditSampleLogIn, value); }
        }
        #endregion
        [NonPersistent]
        [Browsable(false)]
        public XPCollection<SampleLogIn> TestEditDataSource
        {
            get
            {
                XPCollection<SampleLogIn> lstSamples = new XPCollection<SampleLogIn>(Session);
                lstSamples.Criteria = CriteriaOperator.Parse("[JobID.Oid] = ? ", Oid);
                return lstSamples;
            }
        }
        #region ReportedBy
        private Employee _ReportedBy;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ReportedBy
        {
            get
            { 
                if(_ReportedBy==null)
                {
                    _ReportedBy= Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                }
                return _ReportedBy; 
            }
            set
            {
                SetPropertyValue("ReportedBy", ref _ReportedBy, value);
            }
        }
        #endregion
        #region ReportTemplates
        private ReportPackage _ReportTemplates;
        [NonPersistent]
        [ImmediatePostData]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public ReportPackage ReportTemplates
        {
            get { return _ReportTemplates; }
            set
            {
                SetPropertyValue("ReportTemplates", ref _ReportTemplates, value);
            }
        }
        #endregion
        #region ReportID
        private string _BatchReportID;
        [NonPersistent]
        public string BatchReportID
        {
            get { return _BatchReportID; }
            set { SetPropertyValue<string>(nameof(BatchReportID), ref _BatchReportID, value); }
        }
        #endregion
        //#region RegistrationID
        //private SamplingProposal _RegistrationID;
        //[VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        //public SamplingProposal RegistrationID
        //{
        //    get { return _RegistrationID; }
        //    set { SetPropertyValue("RegistrationID", ref _RegistrationID, value); }
        //}
        //#endregion

        #region JobIDFormat
        private int _JobIDFormat;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public int JobIDFormat
        {
            get { return _JobIDFormat; }
            set { SetPropertyValue("JobIDFormat", ref _JobIDFormat, value); }
        }
        #endregion

        #region SamplingStatus
        [NonPersistent]
        public string SamplingStatus
        {
            get
            {
                if (objnavigationRefresh.ClickedNavigationItem == "FieldDataEntry")
                {
                    return "Pending Completion";
                }
                else if (objnavigationRefresh.ClickedNavigationItem == "FieldDataReview1")
                {
                    return "Pending Validation";
                }
                else if (objnavigationRefresh.ClickedNavigationItem == "FieldDataReview2")
                {
                    return "Pending Approval";
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region DateExpect
        private DateTime _DateExpect;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        [RuleRequiredField("DateExpect1",DefaultContexts.Save)]
        public DateTime DateExpect
        {
            get { return _DateExpect; }
            set { SetPropertyValue(nameof(DateExpect), ref _DateExpect, value); }
        }
        #endregion

        #region IsSampling
        [Browsable(false)]
        private bool _IsSampling;
        public bool IsSampling
        {
            get { return _IsSampling; }
            set { SetPropertyValue(nameof(IsSampling), ref _IsSampling, value); }
        }
        #endregion

        public string RegistrationType
        {
            get
            {
                if (IsSampling)
                    return "Pre-scheduled";
                else
                    return "Regular";
            }
        }
    }
}