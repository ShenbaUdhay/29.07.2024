using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Modules.BusinessObjects.Setting
{
    public enum Content
    {
       [XafDisplayName("Sample Registration")]
        Samplecheckin,
        [XafDisplayName("Subout Registration")]
        SuboutSample,
        [XafDisplayName("Reporting")]
        Reporting,
        [XafDisplayName("Invoice")]
        Invoice
    }
    [DefaultClassOptions]
    [DefaultProperty("Contents")]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EmailContentTemplate : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EmailContentTemplate(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) ;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        #region Subject
        private string _Subject;
        public string Subject
        {
            get { return _Subject; }
            set { SetPropertyValue(nameof(Subject), ref _Subject, value); }
        }
        #endregion

        #region Body
        private string _Body;
        [Size(SizeAttribute.Unlimited)]
        //[Editor(RichTextMailMergeData)]
        public string Body
        {
            get { return _Body; }
            set { SetPropertyValue(nameof(Body), ref _Body, value); }
        }
        #endregion


        #region SampleCheckin
        private bool _Samplecheckin;
        public bool Samplecheckin
        {
            get { return _Samplecheckin; }
            set { SetPropertyValue(nameof(Samplecheckin), ref _Samplecheckin, value); }
        }
        #endregion

        #region Report
        private bool _Reporting;
        public bool Reporting
        {
            get { return _Reporting; }
            set { SetPropertyValue(nameof(Reporting), ref _Reporting, value); }
        }
        #endregion

        #region Invoice
        private bool _Invoice;
        public bool Invoice
        {
            get { return _Invoice; }
            set { SetPropertyValue(nameof(Invoice), ref _Invoice, value); }
        }
        #endregion

        private Content _Contents;
        [RuleRequiredField("Contents", DefaultContexts.Save, "'This content has been alread added.'")]
        [RuleUniqueValue]
        public Content Contents
        {
            get { return _Contents; }
            set { SetPropertyValue(nameof(Contents), ref _Contents, value); }
        }



        #region ContentType
        private DataSourceEmailTemplate _Datasource;
        [NonPersistent]
        public DataSourceEmailTemplate Datasource
        {
            get { return _Datasource; }
            set { SetPropertyValue(nameof(Datasource), ref _Datasource, value); }
        }
        #endregion 

        #region SubOutSample
        private bool _SubOutSample;
        public bool SubOutSample
        {
            get { return _SubOutSample; }
            set { SetPropertyValue(nameof(SubOutSample), ref _SubOutSample, value); }
        }
        #endregion



        #region CreatedDate
        private DateTime _CreatedDate;
        [Browsable(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private CustomSystemUser _CreatedBy;
        [Browsable(false)]
        public CustomSystemUser CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [Browsable(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [Browsable(false)]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }

        #endregion
    }
}