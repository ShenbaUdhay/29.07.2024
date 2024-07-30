using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    public class ReportPackage : BaseObject
    {
        public ReportPackage(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = Library.GetServerTime(Session);
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }

        private string _PackageName;
        [RuleRequiredField]
        [System.ComponentModel.DisplayName("PackageName")]
        [ImmediatePostData]
        public string PackageName
        {
            get { return _PackageName; }
            set { SetPropertyValue<string>(nameof(PackageName), ref _PackageName, value); }
        }

        private string _ReportName;
        [System.ComponentModel.DisplayName("TemplateName")]
        public string ReportName
        {
            get { return _ReportName; }
            set { SetPropertyValue<string>(nameof(ReportName), ref _ReportName, value); }
        }
        private int _ReportId;
        [System.ComponentModel.DisplayName("TemplateID")]
        public int ReportId
        {
            get { return _ReportId; }
            set { SetPropertyValue<int>(nameof(ReportId), ref _ReportId, value); }
        }
        private string _CustomCaption;
        [ImmediatePostData]
        public string CustomCaption
        {
            get { return _CustomCaption; }
            set { SetPropertyValue<string>(nameof(CustomCaption), ref _CustomCaption, value); }
        }
        private int _sort;
        [ImmediatePostData]
        public int sort
        {
            get { return _sort; }
            set { SetPropertyValue<int>(nameof(sort), ref _sort, value); }
        }

        private bool _PageDisplay;
        public bool PageDisplay
        {
            get { return _PageDisplay; }
            set { SetPropertyValue<bool>(nameof(PageDisplay), ref _PageDisplay, value); }
        }
        private bool _IsActive;
        //[System.ComponentModel.DisplayName("PageCountInclude")]
        public bool IsActive
        {
            get { return _IsActive; }
            set { SetPropertyValue<bool>(nameof(IsActive), ref _IsActive, value); }
        }
        private ReportCategory _Category;
        public ReportCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue<ReportCategory>(nameof(Category), ref _Category, value); }
        }
        private ReportType _ReportType;
        public ReportType ReportType
        {
            get { return _ReportType; }
            set { SetPropertyValue<ReportType>(nameof(ReportType), ref _ReportType, value); }
        }
        private Employee _UserAccess;
        public Employee UserAccess
        {
            get { return _UserAccess; }
            set { SetPropertyValue<Employee>(nameof(UserAccess), ref _UserAccess, value); }
        }
        private bool _PageCount;
        [System.ComponentModel.DisplayName("PageCountInclude")]
        public bool PageCount
        {
            get { return _PageCount; }
            set { SetPropertyValue<bool>(nameof(PageCount), ref _PageCount, value); }
        }

        DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue<DateTime>(nameof(CreatedDate), ref _CreatedDate, value); }
        }

        Employee _CreatedBy;
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue<Employee>(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        string _NPUserDefinedReportName;
        [NonPersistent]
        public string NPUserDefinedReportName
        {
            get
            {
                if (!string.IsNullOrEmpty(UserDefinedReportName))
                {
                    _NPUserDefinedReportName = UserDefinedReportName;
                }
                else
                {
                    _NPUserDefinedReportName = ReportName;
                }
                return _NPUserDefinedReportName;
            }
        }

        string _UserDefinedReportName;

        public string UserDefinedReportName
        {
            get { return _UserDefinedReportName; }
            set { SetPropertyValue<string>(nameof(UserDefinedReportName), ref _UserDefinedReportName, value); }
        }

        //private tbl_Public_CustomReportDesignerDetails _Reports;
        //public tbl_Public_CustomReportDesignerDetails Reports
        //{
        //    get { return _Reports; }
        //    set { SetPropertyValue<tbl_Public_CustomReportDesignerDetails>(nameof(Reports),ref _Reports,value); }
        //}

        //private CustomReportDesignerDetails _ReportDesignerDetails;
        //public CustomReportDesignerDetails ReportDesignerDetails
        //{
        //    get { return _ReportDesignerDetails; }
        //    set { SetPropertyValue<CustomReportDesignerDetails>(nameof(ReportDesignerDetails),ref _ReportDesignerDetails,value); }
        //}

        //[NonPersistent]
        //public IList<tbl_Public_CustomReportDesignerDetails> Reports
        //{
        //    get; set;
        //}

    }
}