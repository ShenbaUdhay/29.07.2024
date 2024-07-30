using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace ICM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Vendor")]
    [RuleCombinationOfPropertiesIsUnique("Vendor.VendorName", DefaultContexts.Save, "Vendor", SkipNullOrEmptyValues = false)]
    public class Vendors : BaseObject
    {
        #region Constructor
        public Vendors(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //Vendorcode += Convert.ToInt32(Session.Evaluate<Vendors>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Max(Vendorcode)"), null)) + 1;
            //if (Vendorcode.Length == 1)
            //{
            //    Vendorcode = "00" + Vendorcode;
            //}
            //else if(Vendorcode.Length == 2)
            //{
            //    Vendorcode = "00" + Vendorcode;
            //}
        }
        #endregion

        #region OnDelete
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {

                //foreach (BaseObject obj in Session.CollectReferencingObjects(this))
                //{
                //    if (obj.Oid != null)
                //    {
                //        Exception ex = new Exception("Already Used Can't allow to Delete");
                //        throw ex;
                //        break;

                //    }
                //}
                foreach (var obj in Session.CollectReferencingObjects(this))
                {

                    if (obj.GetType() != typeof(DevExpress.Xpo.Metadata.Helpers.IntermediateObject) && obj.GetType() != typeof(VendorEvaluation) && obj.GetType() != typeof(Vendorsupload))
                    {
                        Exception ex = new Exception("Already Used Can't allow to Delete");
                        throw ex;
                        break;
                    }
                }
            }
        }
        #endregion

        #region vendorcode
        [Indexed(Unique = true)]
        string fVendorcode;
        public string Vendorcode
        {
            get { return fVendorcode; }
            set { SetPropertyValue<string>("Vendorcode", ref fVendorcode, value); }
        }
        #endregion

        #region vendor
        string fVendor;
        [RuleRequiredField("Vendor", DefaultContexts.Save)]
        //  [RuleStringComparison("RuleStringComparison_Vendor_NotNullOrEmpty", DefaultContexts.Save, StringComparisonType.NotEquals, null)]
        public string Vendor
        {
            get { return fVendor; }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetPropertyValue<string>("Vendor", ref fVendor, value.Trim());
            }
        }
        #endregion

        #region account
        string fAccount;
        //[RuleRequiredField("Account", DefaultContexts.Save)]
        public string Account
        {
            get { return fAccount; }
            set { SetPropertyValue<string>("Account", ref fAccount, value); }
        }
        #endregion

        #region address1
        string fAddress1;
        //[RuleRequiredField("Address1", DefaultContexts.Save)]
        public string Address1
        {
            get { return fAddress1; }
            set { SetPropertyValue<string>("Address1", ref fAddress1, value); }
        }
        #endregion

        #region address2
        string fAddress2;
        public string Address2
        {
            get { return fAddress2; }
            set { SetPropertyValue<string>("Address2", ref fAddress2, value); }
        }
        #endregion

        #region City
        private string _City;
        [ImmediatePostData]
        public string City
        {
            get { return _City; }
            set { SetPropertyValue("City", ref _City, value); }
        }
        #endregion

        #region state
        private string _State;
        [ImmediatePostData]
        [Appearance("StateHide", Visibility = ViewItemVisibility.Hide, Criteria = "[Country.EnglishLongName] = 'United States of America'", Context = "DetailView")]
        [Appearance("StateShow", Visibility = ViewItemVisibility.Show, Criteria = "[Country.EnglishLongName] <> 'United States of America'", Context = "DetailView")]

        public string State
        {
            get { return _State; }
            set { SetPropertyValue("State", ref _State, value); }
        }
        #endregion

        #region stateCB
        private CustomState _StateCB;
        [ImmediatePostData]
        [Appearance("StateCBHide", Visibility = ViewItemVisibility.Hide, Criteria = "[Country.EnglishLongName] <> 'United States of America'", Context = "DetailView")]
        [Appearance("StateCBShow", Visibility = ViewItemVisibility.Show, Criteria = "[Country.EnglishLongName] = 'United States of America'", Context = "DetailView")]

        public CustomState StateCB
        {
            get { return _StateCB; }
            set { SetPropertyValue("StateCB", ref _StateCB, value); }
        }
        #endregion

        #region zipcode
        string fZipCode;
        //[RuleRequiredField("ZipCode", DefaultContexts.Save)]
        public string ZipCode
        {
            get { return fZipCode; }
            set { SetPropertyValue<string>("ZipCode", ref fZipCode, value); }
        }
        #endregion

        #region country
        CustomCountry fCountry;
        [ImmediatePostData]
        public CustomCountry Country
        {
            get { return fCountry; }
            set { SetPropertyValue("Country", ref fCountry, value); }
        }
        #endregion

        #region phone
        string fPhone;
        public string Phone
        {
            get { return fPhone; }
            set { SetPropertyValue<string>("Phone", ref fPhone, value); }
        }
        #endregion

        #region contact
        string fContact;

        public string Contact
        {
            get { return fContact; }
            set { SetPropertyValue<string>("Contact", ref fContact, value); }
        }
        #endregion

        #region fax
        string fFax;
        public string Fax
        {
            get { return fFax; }
            set { SetPropertyValue<string>("Fax", ref fFax, value); }
        }
        #endregion

        #region qualification
        //website
        string fQualification;
        public string Qualification
        {
            get { return fQualification; }
            set { SetPropertyValue<string>("Qualification", ref fQualification, value); }
        }
        #endregion

        #region certificate
        string fCertificate;
        public string Certificate
        {
            get { return fCertificate; }
            set { SetPropertyValue<string>("Certificate", ref fCertificate, value); }
        }
        #endregion

        #region certexpdate
        DateTime fCertExpDate;
        public DateTime CertExpDate
        {
            get { return fCertExpDate; }
            set { SetPropertyValue<DateTime>("CertExpDate", ref fCertExpDate, value); }
        }
        #endregion

        #region upload
        [DevExpress.ExpressApp.DC.Aggregated, Association("Vendors-VendorsFileData")]
        public XPCollection<Vendorsupload> Upload
        {
            get { return GetCollection<Vendorsupload>("Upload"); }
        }
        #endregion

        #region evaluation
        [Association("VendorEvaluationlink", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<VendorEvaluation> Evaluation
        {
            get { return GetCollection<VendorEvaluation>("Evaluation"); }
        }
        #endregion

        #region approvedate
        DateTime fApprovedDate;
        public DateTime ApprovedDate
        {
            get { return fApprovedDate; }
            set { SetPropertyValue<DateTime>("ApprovedDate", ref fApprovedDate, value); }
        }
        #endregion

        #region approvedby
        Employee fApprovedBy;
        public Employee ApprovedBy
        {
            get { return fApprovedBy; }
            set { SetPropertyValue("ApprovedBy", ref fApprovedBy, value); }
        }
        #endregion

        #region retiredate
        DateTime fRetiredDate;
        public DateTime RetiredDate
        {
            get { return fRetiredDate; }
            set { SetPropertyValue<DateTime>("RetiredDate", ref fRetiredDate, value); }
        }
        #endregion

        #region retireby
        Employee fRetiredBy;
        public Employee RetiredBy
        {
            get { return fRetiredBy; }
            set { SetPropertyValue("RetiredBy", ref fRetiredBy, value); }
        }
        #endregion

        #region comment
        string fComment;
        [Size(1000)]
        public string Comment
        {
            get { return fComment; }
            set { SetPropertyValue<string>("Comment", ref fComment, value); }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
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
        //[Appearance("MD9", Enabled = false, Context = "DetailView")]       
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

        #region Email
        string fEmail;
        public string Email
        {
            get { return fEmail; }
            set { SetPropertyValue<string>("Email", ref fEmail, value); }
        }
        #endregion


        #region Website
        string fWebsite;
        public string Website
        {
            get { return fWebsite; }
            set { SetPropertyValue<string>("Website", ref fWebsite, value); }
        }
        #endregion

    }
}