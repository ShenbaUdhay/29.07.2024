using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.XtraEditors.Filtering.Templates;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.TaskManagement;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using Modules.BusinessObjects.SampleManagement;

namespace Modules.BusinessObjects.Setting.DWQRReportTemplateSetup
{
    //public enum DWQRStatus
    //{
    //    [XafDisplayName("Pending Submission")]
    //    PendingSubmission,
    //    [XafDisplayName("Pending Approval")]
    //    PendingApproval,
    //    [XafDisplayName("Pending Delivery")]
    //    PendingDelivery,
    //    Delivered
    //}

    [DefaultClassOptions]

    public class DWQRReportTemplateSetup : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public DWQRReportTemplateSetup(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            //DateTo = DateTime.Now;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);

            if (string.IsNullOrEmpty(TemplateID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(TemplateID, 4))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(DWQRReportTemplateSetup), criteria, null)) + 1).ToString();
                //var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "1")
                {
                    if (tempID.Length == 1)
                    {
                        tempID = "DWRT0" + tempID;
                    }
                    else if(tempID.Length == 2)
                    {
                        tempID = "DWRT" + tempID;
                    }
                }
                else
                {
                    tempID = "DWRT01";
                }

                TemplateID = tempID;
                Active = true;
            }
        }
        #region TemplateID
        private string _TemplateID;
        [ModelDefault("AllowEdit", "False")]
        public string TemplateID
        {
            get { return _TemplateID; }
            set { SetPropertyValue(nameof(TemplateID), ref _TemplateID, value); }
        }
        #endregion

        #region TemplateName
        private string _TemplateName;
        [RuleRequiredField]
        [RuleUniqueValue]        
        public string TemplateName
        {
            get { return _TemplateName; }
            set { SetPropertyValue(nameof(TemplateName), ref _TemplateName, value); }
        }
        #endregion

        #region ReportName
        private string _ReportName;
        [RuleRequiredField]        
        public string ReportName
        {
            get { return _ReportName; }
            set { SetPropertyValue(nameof(ReportName), ref _ReportName, value); }
        }
        #endregion

        #region Category
        private ReportCategory _Category;
        public ReportCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue(nameof(Category), ref _Category, value); }
        }
        #endregion

        #region Frequency
        private string _Frequency;
        public string Frequency
        {
            get { return _Frequency; }
            set { SetPropertyValue(nameof(Frequency), ref _Frequency, value); }
        }
        #endregion

        #region StartDate
        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { SetPropertyValue(nameof(StartDate), ref _StartDate, value); }
        }
        #endregion

        #region EndDate
        private DateTime _EndDate;
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { SetPropertyValue(nameof(EndDate), ref _EndDate, value); }
        }
        #endregion

        #region Active
        private bool _Active;
        [ImmediatePostData]
        public bool Active
        {
            get { return _Active; }
            set { SetPropertyValue(nameof(Active), ref _Active, value); }
        }
        #endregion

        #region RetireDate
        private DateTime _RetireDate;
        [Appearance("HideRetireDate", Visibility = ViewItemVisibility.Hide, Criteria = "Active = false", Context = "DetailView")]
        [ImmediatePostData]
        public DateTime RetireDate
        {
            get { return _RetireDate; }
            set { SetPropertyValue(nameof(RetireDate), ref _RetireDate, value); }
        }
        #endregion

        #region DateFrom
        private DateTime _DateFrom;
        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set { SetPropertyValue(nameof(DateFrom), ref _DateFrom, value); }
        }
        #endregion

        #region DateTo
        private DateTime _DateTo;
        [ImmediatePostData]
        public DateTime DateTo
        {
            get { return _DateTo; }
            set { SetPropertyValue(nameof(DateTo), ref _DateTo, value); }
        }
        #endregion

        #region EmailTo
        private string _EmailTo;
        //[RuleRequiredField] 
        //[RuleRegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", DefaultContexts.Save)] 
        public string EmailTo
        {
            get { return _EmailTo; }
            set { SetPropertyValue(nameof(EmailTo), ref _EmailTo, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #endregion

        //#region Status
        //private DWQRStatus _Status;
        //public DWQRStatus Status
        //{
        //    get { return _Status; }
        //    set { SetPropertyValue(nameof(Status), ref _Status, value); }
        //}
        //#endregion

        #region SampleSitesCollection
        [Association("DWQRReportTemplateSetup-SampleSites")]
        public XPCollection<SampleSites> SampleSites
        {
            get { return GetCollection<SampleSites>(nameof(SampleSites)); }
        } 
        #endregion

        #region ParameterCollection
        [Association("DWQRReportTemplateSetup-Testparameter")]
        public XPCollection<Testparameter> ParameterCollection
        {
            get { return GetCollection<Testparameter>(nameof(ParameterCollection)); }
        } 
        #endregion

        #region CreatedBy
        private CustomSystemUser _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _CreatedBy, value);
            }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                SetPropertyValue("CreatedDate", ref _CreatedDate, value);
            }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser ModifiedBy
        {
            get
            {
                return _ModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref _ModifiedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get
            {
                return _ModifiedDate;
            }
            set
            {
                SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value);
            }
        }
        #endregion
    }
}