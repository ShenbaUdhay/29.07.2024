using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SubContractLab : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SubContractLab(Session session)
            : base(session)
        {

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            SetupDate = Library.GetServerTime(Session); ;
            SetupBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private string _SubLabName;
        [RuleUniqueValue]
        [RuleRequiredField("SubLabName", DefaultContexts.Save)]
        public string SubLabName
        {
            get { return _SubLabName; }
            set { SetPropertyValue("SubLabName", ref _SubLabName, value); }
        }

        private string _Account;
        public string Account
        {
            get { return _Account; }
            set { SetPropertyValue("Account", ref _Account, value); }
        }
        private string _Address1;
        public string Address1
        {
            get { return _Address1; }
            set { SetPropertyValue("Address1", ref _Address1, value); }
        }

        private string _Address2;
        public string Address2
        {
            get { return _Address2; }
            set { SetPropertyValue("Address2", ref _Address2, value); }
        }

        private City _City;
        [DataSourceProperty("State.Cities", DataSourcePropertyIsNullMode.SelectNothing)]
        public City City
        {
            get { return _City; }
            set { SetPropertyValue("City", ref _City, value); }
        }

        private CustomState _State;
        [ImmediatePostData(true)]
        [DataSourceProperty("Country.States", DataSourcePropertyIsNullMode.SelectNothing)]
        public CustomState State
        {
            get { return _State; }
            set { SetPropertyValue("State", ref _State, value); }
        }

        private string _Zip;
        public string Zip
        {
            get { return _Zip; }
            set { SetPropertyValue("Zip", ref _Zip, value); }
        }

        private CustomCountry _Country;
        [ImmediatePostData(true)]
        public CustomCountry Country
        {
            get { return _Country; }
            set { SetPropertyValue("Country", ref _Country, value); }
        }

        private string _CertificateNo;
        public string CertificateNo
        {
            get { return _CertificateNo; }
            set { SetPropertyValue("CertificateNo", ref _CertificateNo, value); }
        }


        private string _AccountBank;
        public string AccountBank
        {
            get { return _AccountBank; }
            set { SetPropertyValue("AccountBank", ref _AccountBank, value); }
        }


        private string _AccountNumber;
        public string AccountNumber
        {
            get { return _AccountNumber; }
            set { SetPropertyValue("AccountNumber", ref _AccountNumber, value); }
        }

        private string _ContactName;
        [RuleRequiredField("ContactName", DefaultContexts.Save)]
        public string ContactName
        {
            get { return _ContactName; }
            set { SetPropertyValue("ContactName", ref _ContactName, value); }
        }


        private string _ContactPhone;
        [RuleRequiredField("ContactPhone", DefaultContexts.Save)]
        public string ContactPhone
        {
            get { return _ContactPhone; }
            set { SetPropertyValue("ContactPhone", ref _ContactPhone, value); }
        }

        private string _Fax;
        public string Fax
        {
            get { return _Fax; }
            set { SetPropertyValue("Fax", ref _Fax, value); }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { SetPropertyValue("Email", ref _Email, value); }
        }

        private string _WebSite;
        public string WebSite
        {
            get { return _WebSite; }
            set { SetPropertyValue("WebSite", ref _WebSite, value); }
        }
        private string _Note;
        public string Note
        {
            get { return _Note; }
            set { SetPropertyValue("Note", ref _Note, value); }
        }


        private CustomSystemUser _SetupBy;
        public CustomSystemUser SetupBy
        {
            get { return _SetupBy; }
            set { SetPropertyValue("SetupBy", ref _SetupBy, value); }
        }

        private DateTime? _SetupDate;
        public DateTime? SetupDate
        {
            get { return _SetupDate; }
            set { SetPropertyValue("SetupDate", ref _SetupDate, value); }
        }

        private DateTime? _RetireDate;
        public DateTime? RetireDate
        {
            get { return _RetireDate; }
            set { SetPropertyValue("RetireDate", ref _RetireDate, value); }
        }

        private string _RetireReason;
        public string RetireReason
        {
            get { return _RetireReason; }
            set { SetPropertyValue("RetireReason", ref _RetireReason, value); }
        }

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]

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
    }
}