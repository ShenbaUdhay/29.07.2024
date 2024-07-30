using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Modules.BusinessObjects.Hr;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Accounting.Receivables
{
    [DefaultClassOptions]
    [DefaultProperty("RegionName")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Region : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Region(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedDate = DateTime.Now;
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }

        #region Name
        private string _Name;
        [RuleRequiredField("Region",DefaultContexts.Save, "'RegionName must not be empty'")]
        public string RegionName
        {
            get { return _Name; }
            set { SetPropertyValue(nameof(RegionName), ref _Name, value); }
        }
        #endregion
        #region Description
        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                SetPropertyValue<DateTime>(nameof(CreatedDate), ref _CreatedDate, value);
            }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        public Employee CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetPropertyValue<Employee>(nameof(CreatedBy), ref _CreatedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime ModifiedDate
        {
            get
            {
                return _ModifiedDate;
            }
            set
            {
                SetPropertyValue<DateTime>(nameof(ModifiedDate), ref _ModifiedDate, value);
            }
        }
        #endregion

        #region ModifiedBy
        private Employee _ModifiedBy;
        public Employee ModifiedBy
        {
            get
            {
                return _ModifiedBy;
            }
            set
            {
                SetPropertyValue<Employee>(nameof(ModifiedBy), ref _ModifiedBy, value);
            }
        }
        #endregion
    }
}