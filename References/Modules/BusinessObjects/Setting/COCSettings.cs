using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Assets;
//using Modules.BusinessObjects.ContractManagement;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SamplingManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(strCOCID))]
    [XafDisplayName("Pre-Log Settings")]
    public class COCSettings : Event, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        COCSettingsRegistrationInfo COCsr = new COCSettingsRegistrationInfo();
        COCSettingsRegistrationInfo objInfo = new COCSettingsRegistrationInfo();

        public COCSettings(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            CreatedDate = Library.GetServerTime(Session);
            CreatedBy = (Employee)Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            NumberOfSample = 1;
            NoOfSamples = 1;
            IsAlpacCOCid = 1;
            objInfo.bolNewCOCID = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (COC_ID == null)
            {
                //string strPrefix = DateTime.Now.ToString("yyMM");
                if (string.IsNullOrEmpty(COC_ID))
                {
                    //CriteriaOperator criteria = CriteriaOperator.Parse("MAX(TOINT(SUBSTRING(COC_ID, 3)))");
                    //string tempid = (Convert.ToInt32(Session.Evaluate(typeof(COCSettings), criteria, null)) + 1).ToString("0000");
                    //COC_ID = tempid;

                    CriteriaOperator criteria = CriteriaOperator.Parse("MAX(COC_ID)");
                    int maxSubstringValue = Convert.ToInt32(Session.Evaluate(typeof(COCSettings), criteria, null)) + 1;
                    string tempid = maxSubstringValue.ToString("000");
                    COC_ID = tempid;
                }



                //int ID = Convert.ToInt32(Session.Evaluate(typeof(COCSettings), CriteriaOperator.Parse("MAX(COC_ID)"), CriteriaOperator.Parse("")));
                //string str = Convert.ToString(ID);
                ////string num = "001";                
                //if (str != "")
                //{

                //    //string EmpID = str.Remove(0, 4);
                //    string cocid = ID.ToString().Substring(str.Length - 3); 
                //    //string TempID = EmpID;
                //    int TempID = int.Parse(cocid);
                //    TempID = TempID + 1;
                //    if (TempID.ToString().Length == 1)
                //    {
                //        COC_ID = "00" + TempID;
                //    }
                //    else if (TempID.ToString().Length == 2)
                //    {
                //        COC_ID = "0" + TempID;
                //    }
                //    else
                //    {
                //        COC_ID = TempID.ToString();
                //    }
                //    //string id = TempID.ToString().PadLeft(2, '0');
                //    //COC_ID =  id;
                //}



                //if (ID.ToString().Length == 1)
                //{
                //    COC_ID = "00" + ID;
                //    COC_ID = COC_ID + 1;
                //    //if (!ID.ToString())
                //    //{
                //    //    COC_ID = strPrefix + "00" + ID;
                //    //}
                //    //else
                //    //{
                //    //    COC_ID = ID.ToString();
                //    //}
                //}
                //else if (ID.ToString().Length == 2)
                //{
                //    COC_ID = "0" + ID;
                //    COC_ID = COC_ID + 1;
                //    //if (!ID.ToString().Contains(strPrefix))
                //    //{
                //    //    COC_ID = strPrefix + "0" + ID;
                //    //}
                //    //else
                //    //{
                //    //    COC_ID = ID.ToString();
                //    //}
                //}
                //else
                //{
                //    COC_ID = ID.ToString();
                //    COC_ID = COC_ID + 1;
                //    //if (!ID.ToString().Contains(strPrefix))
                //    //{
                //    //    COC_ID = strPrefix + ID.ToString();
                //    //}
                //    //else
                //    //{
                //    //    COC_ID = ID.ToString();
                //    //}
                //}
                //COC_ID = strPrefix +
                //    DistributedIdGeneratorHelper.Generate(Session, strPrefix, string.Empty).ToString().PadLeft(3, '0');
            }
            if (NextUpdateDate == DateTime.MinValue)
            {
                NextUpdateDate = StartOn;
            }
        }

        #region COC_ID
        private string _cOC_ID;
        [ReadOnly(true)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [Appearance("COCID", Enabled = false, Context = "DetailView")]

        public string COC_ID { get => _cOC_ID; set => SetPropertyValue(nameof(COC_ID), ref _cOC_ID, value); }
        #endregion

        #region strCOCID
        private string _strCOCID;
        [DevExpress.Xpo.DisplayName(nameof(COC_ID))]
        [NonPersistent]
        [ReadOnly(true)]
        public string strCOCID
        {
            get
            {
                if (!string.IsNullOrEmpty(COC_ID))
                {
                    _strCOCID = "COC" + COC_ID;
                }
                return _strCOCID;
            }
        }
        #endregion

        #region COCName
        private string _COCName;
        [RuleRequiredField("_COCName", DefaultContexts.Save,"'COC Name' must not be empty")]
        //[RuleUniqueValue]
        public string COCName
        {
            get
            {
                return _COCName;
            }
            set
            {
                SetPropertyValue<string>(nameof(COCName), ref _COCName, value);
            }
        }
        #endregion

        #region ClientName
        private Customer _ClientName;
        [RuleRequiredField("ClientName2", DefaultContexts.Save,"'Client' must not be empty")]
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
                    return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", ClientName.Oid));
                }
                else
                {
                    return null;
                }
            }
        }

        #region Contact
        private Contact _ClientContact;
        [DataSourceProperty("Contacts")]
        public Contact ClientContact
        {
            get { return _ClientContact; }
            set { SetPropertyValue(nameof(ClientContact), ref _ClientContact, value); }
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
            set { SetPropertyValue<string>("License ", ref _License, value); }

        }
        #endregion

        #region InspectionCategory
        //private InspectCategory _InspectionCategory;
        ////[RuleRequiredField]
        //[BrowsableAttribute(false)]
        //public InspectCategory InspectionCategory
        //{
        //    get { return _InspectionCategory; }
        //    set { SetPropertyValue<InspectCategory>(nameof(InspectionCategory), ref _InspectionCategory, value); }
        //}
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

        #region QuoteID
        private CRMQuotes _QuoteID;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DataSourceProperty("QuotesDataSource")]
        [ImmediatePostData]
        public CRMQuotes QuoteID
        {
            get {
                if (ClientName == null)
                {
                    _QuoteID = null;
                }
                return _QuoteID; }
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

        #region ProjectId
        private Project _ProjectID;
        [ImmediatePostData(true)]
        [RuleRequiredField]
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
            set { SetPropertyValue<string>("ProjectName", ref _ProjectName, value); }
        }
        #endregion

        #region Project Location
        private string _projectLocation;
        [NonPersistent]
        [Size(300)]
        [ModelDefault("RowCount", "1")]
        public string ProjectLocation
        {
            get
            {
                if (ProjectID != null)
                {
                    _projectLocation = ProjectID.ProjectLocation;
                }
                else
                {
                    _projectLocation = string.Empty;
                }
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

        #region ProjectManager
        private Employee _ProjectManager;
        public Employee ProjectManager
        {
            get { return _ProjectManager; }
            set { SetPropertyValue<Employee>("ProjectManager", ref _ProjectManager, value); }
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
                return new XPCollection<VisualMatrix>(Session, CriteriaOperator.Parse("[IsRetired] <> True Or [IsRetired] Is Null"));
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
                //    if (!Properties.ContainsKey(objSampleMatrix.Oid))
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

        [Browsable(false)]
        [NonPersistent]
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
        [DevExpress.ExpressApp.DC.Aggregated, Association("COCSettings_CustomDueDate")]
        public XPCollection<CustomDueDate> CustomDueDates
        {
            get { return GetCollection<CustomDueDate>(nameof(CustomDueDates)); }
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

                if (!IsLoading && COC_ID != null && COCsr.isNoOfSampleDisable)
                {
                    _NoOfSamples = Convert.ToUInt32(Session.Evaluate(typeof(COCSettingsSamples), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[COCID.Oid] = ?", Oid)));
                }
                else
                if (COCsr.IsSamplePopupClose == true)
                {
                    // _NoOfSamples = Convert.ToUInt32(Session.Evaluate(typeof(COCSettingsSamples), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[COCID.Oid] = ?", Oid)));
                    // SRInfo.IsSamplePopupClose = false;
                    uint samplecnt = Convert.ToUInt32(Session.Evaluate(typeof(COCSettingsSamples), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[COCID.Oid] = ?", Oid)));
                    if (samplecnt == 0)
                    {
                        _NoOfSamples = 1;
                    }
                    else
                    {
                        _NoOfSamples = samplecnt;
                    }
                }
                return _NoOfSamples;
            }
            set
            {
                SetPropertyValue<uint>("NoOfSamples", ref _NoOfSamples, value);
            }
        }
        #endregion

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
        private string _checkedsamplematrix;
        [NonPersistent]
        public string checkedsamplematrix
        {
            get
              {
                if (SampleMatries != null)
                {

                    List<Guid> ids = SampleMatries.Split(';').Select(i => new Guid(i)).ToList();
                    if (ids != null)
                    {
                        XPCollection<VisualMatrix> lstTests = new XPCollection<VisualMatrix>(Session, new InOperator("Oid", ids));

                        if (lstTests!=null& lstTests.Count > 0)
                        {
                            List<string> sample = lstTests.Select(i => i.VisualMatrixName).Distinct().ToList();
                            string str = string.Join(",", sample);
                            _checkedsamplematrix = str;
                        }
                  
                   }
                }
                  return _checkedsamplematrix; 
                }
            set { SetPropertyValue(nameof(checkedsamplematrix), ref _checkedsamplematrix, value); }
        }

        private string _SampleCategory;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string SampleCategory
        {
            get { return _SampleCategory; }
            set { SetPropertyValue(nameof(SampleCategory), ref _SampleCategory, value); }
        }

        #region NumberOfSample
        private int _NumberOfSample;
        public int NumberOfSample
        {
            get { return _NumberOfSample; }
            set { SetPropertyValue<int>("NumberOfSample", ref _NumberOfSample, value); }
        }
        #endregion        

        #region SiteMapID
        private string _SiteMapID;
        public string SiteMapID
        {
            get { return _SiteMapID; }
            set { SetPropertyValue<string>(nameof(SiteMapID), ref _SiteMapID, value); }
        }
        #endregion

        #region FileUpload
        private FileData _FileUpload;
        //[RuleRequiredField]
        public FileData FileUpload
        {
            get { return _FileUpload; }
            set { SetPropertyValue<FileData>(nameof(FileUpload), ref _FileUpload, value); }
        }
        #endregion
        //#region ClientName
        //private Customer _Client;
        //[RuleRequiredField]
        //[ImmediatePostData(true)]
        //public Customer Client
                //{
        //    get
                //    {
        //        return _Client;
                //    }
        //    set
                //    { 
        //        SetPropertyValue("Client", ref _Client, value);
        //        //if(!IsLoading)
        //        //{
        //        //    if (value == null)
        //        //    {

        //        //    }
        //        //    else
        //        //    { 
        //        //        ClientAddress = value.Address;
        //        //        ClientAddress2 = value.Address1;
        //        //        ClientPhone = value.OtherPhone;
        //        //    }
        //        //}
                //    }
                //}
        //#endregion

        #region Comment
        private string _Comment;
        [Size(1000)]
        //[Browsable(false)]
        [XafDisplayName("Remark")]
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                SetPropertyValue(nameof(Comment), ref _Comment, value);
            }
        }
        #endregion

        #region SamplingAddress
        private string _SamplingAddress;
        [Size(1000)]
        //[Browsable(false)]
        public string SamplingAddress
        {
            get
            {
                return _SamplingAddress;
            }
            set
            {
                SetPropertyValue(nameof(SamplingAddress), ref _SamplingAddress, value);
            }
        }
        #endregion        

        #region SampleMatrix
        private VisualMatrix _SampleMatrix;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public VisualMatrix SampleMatrix
        {
            get { return _SampleMatrix; }
            set { SetPropertyValue(nameof(SampleMatrix), ref _SampleMatrix, value); }
        }
        #endregion

        #region ReportTemplate
        private string _ReportTemplate;
        [EditorAlias("ReportTemplatePropertyEditor")]
        public string ReportTemplate
        {
            get { return _ReportTemplate; }
            set { SetPropertyValue(nameof(ReportTemplate), ref _ReportTemplate, value); }
        }
        #endregion

        #region COC Template
        private string _COCTemplate;
        [EditorAlias("COCTemplatePropertyEditor")]
        public string COCTemplate
        {
            get { return _COCTemplate; }
            set { SetPropertyValue(nameof(_COCTemplate), ref _COCTemplate, value); }
        }
        #endregion

        #region EDDTemplate
        private EDDBuilder _EDDTemplate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public EDDBuilder EDDTemplate
        {
            get { return _EDDTemplate; }
            set { SetPropertyValue(nameof(EDDTemplate), ref _EDDTemplate, value); }
        }
        #endregion

        #region Active
        private bool _Active;
        public bool Active
        {
            get { return _Active; }
            set { SetPropertyValue(nameof(Active), ref _Active, value); }
        }
        #endregion

        #region Retire
        private bool _Retire;
        public bool Retire
        {
            get { return _Retire; }
            set { SetPropertyValue(nameof(Retire), ref _Retire, value); }
        }
        #endregion

        #region RetireDate
        private DateTime _RetireDate;
        public DateTime RetireDate
        {
            get { return _RetireDate; }
            set { SetPropertyValue("RetireDate", ref _RetireDate, value); }
        }
        #endregion

        #region Attachment
        [Association("COCSettings-Attachment")]
        public XPCollection<Attachment> Attachment
        {
            get { return GetCollection<Attachment>("Attachment"); }
        }
        #endregion

        #region Note
        [Association("COCSettings-Note")]
        public XPCollection<Notes> Note
        {
            get { return GetCollection<Notes>("Note"); }
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

        private TurnAroundTime _TAT;
        [ImmediatePostData(true)]

        //[RuleRequiredField("Enter The TAT1", DefaultContexts.Save)]
        public TurnAroundTime TAT
        {
            get
            {
                //if (_TAT == null)
                //{
                //    var tatobject = new XPCollection<TurnAroundTime>(Session, CriteriaOperator.Parse("[Default] = True")).ToList();
                //    _TAT = tatobject.FirstOrDefault();
                //}
                return _TAT;



            }
            set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        }

        private Modules.BusinessObjects.Assets.Labware _BalanceID;
        public Modules.BusinessObjects.Assets.Labware BalanceID
        {
            get { return _BalanceID; }
            set { SetPropertyValue(nameof(BalanceID), ref _BalanceID, value); }
        }
        //#region ItemChargePricingCollection
        //[DevExpress.ExpressApp.DC.Aggregated, Association("COCSettings-COCItemChargePrice")]
        //public XPCollection<COCSettingsItemChargePricing> COCItemCharges
        //{
        //    get { return GetCollection<COCSettingsItemChargePricing>(nameof(COCItemCharges)); }
        //}
        //#endregion
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
        #region IsAlpacCOCid
        private int _IsAlpacCOCid;
        [Browsable(false)]
        public int IsAlpacCOCid
        {
            get { return _IsAlpacCOCid; }
            set { SetPropertyValue("IsAlpacCOCid", ref _IsAlpacCOCid, value); }
        }
        #endregion

        [Association("COCSettings-CopyTo")]
        public XPCollection<TaskRecurranceSetup> CopyTo
        {
            get { return GetCollection<TaskRecurranceSetup>("CopyTo"); }
        }


        private DateTime _NextUpdateDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime NextUpdateDate
        {
            get
            {
                return _NextUpdateDate;
            }
            set { SetPropertyValue<DateTime>(nameof(NextUpdateDate), ref _NextUpdateDate, value); }
        }
        private DateTime _LastUpdateDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime LastUpdateDate
        {
            get { return _LastUpdateDate; }
            set { SetPropertyValue<DateTime>(nameof(LastUpdateDate), ref _LastUpdateDate, value); }
        }
        private uint _BeforEventDays;
        [XafDisplayName("#Days In Advance")]
        [VisibleInListView(false), VisibleInLookupListView(false)]
        public uint BeforEventDays
        {
            get { return _BeforEventDays; }
            set { SetPropertyValue<uint>(nameof(BeforEventDays), ref _BeforEventDays, value); }
        }

        //#region EmailReportTo
        //private Contact _EmailReportTo;
        //[DataSourceProperty("EmailReportToDataSource")]
        //[VisibleInListView(false), VisibleInLookupListView(false),VisibleInDetailView(false)]
        //[ImmediatePostData(true)]
        //public Contact EmailReportTo
        //{
        //    get { return _EmailReportTo; }
        //    set { SetPropertyValue<Contact>(nameof(EmailReportTo), ref _EmailReportTo, value); }
        //}
        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<Contact> EmailReportToDataSource
        //{
        //    get
        //    {
        //        if (ClientName != null)
        //        {
        //            XPCollection<Contact> listprojects = new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ? And IsReport =true ", ClientName.Oid));
        //            return listprojects;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        //#endregion
        //#region EmailInvoiceTo
        //private Contact _EmailInvoiceTo;
        //[DataSourceProperty("InvoiceReportToDataSource")]
        //[VisibleInListView(false), VisibleInLookupListView(false), VisibleInDetailView(false)]
        //public Contact EmailInvoiceTo
        //{
        //    get { return _EmailInvoiceTo; }
        //    set { SetPropertyValue<Contact>(nameof(EmailInvoiceTo), ref _EmailInvoiceTo, value); }
        //}

        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<Contact> InvoiceReportToDataSource
        //{
        //    get
        //    {
        //        if (ClientName != null)
        //        {
        //            XPCollection<Contact> listprojects = new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ? And IsInvoice =true ", ClientName.Oid));
        //            return listprojects;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        //#endregion

        //#region ContactNumber
        //private string _ContactNumber;
        //[NonPersistent]
        //[VisibleInListView(false), VisibleInLookupListView(false), VisibleInDetailView(false)]
        //public string ContactNumber
        //{
        //    get {
        //        if (EmailReportTo != null)
        //        {
        //            _ContactNumber = EmailReportTo.MobilePhone;
        //        }
        //        else
        //        {
        //            _ContactNumber = null;
        //        }
        //       return  _ContactNumber; }
        //}
        //#endregion

        #region DaysinAdvance
        private int _DaysinAdvance;
        [ImmediatePostData]
        public int DaysinAdvance
        {
            get
            {
                return _DaysinAdvance;
            }
            set
            {
                SetPropertyValue("DaysinAdvance", ref _DaysinAdvance, value);
            }
        }
        #endregion
    }
}