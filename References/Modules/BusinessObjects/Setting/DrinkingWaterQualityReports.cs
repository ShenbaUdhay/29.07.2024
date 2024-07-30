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
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using Modules.BusinessObjects.SampleManagement;

namespace Modules.BusinessObjects.Setting.DrinkingWaterQualityReports
{

    public enum DWQRStatus
    {
        [XafDisplayName("Pending Submission")]
        PendingSubmission,
        [XafDisplayName("Pending Approval")]
        PendingApproval,
        [XafDisplayName("Pending Delivery")]
        PendingDelivery,
        Delivered
    }

    [DefaultClassOptions]
    
    public class DrinkingWaterQualityReports : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public DrinkingWaterQualityReports(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);

            if (string.IsNullOrEmpty(ReportID))
            {
                string yearMonth = DateTime.Now.ToString("yyMM");
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(ReportID, 8))");
                int sequenceNumber = Convert.ToInt32(Session.Evaluate(typeof(DrinkingWaterQualityReports), criteria, null)) + 1;
                string sequence = sequenceNumber.ToString().PadLeft(3, '0');
                ReportID = "DWQR" + yearMonth + sequence;
                DateCreated = Library.GetServerTime(Session);
                ReportCreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            }
        }

        #region targetMemberName
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "EmailTo" && EmailDataSource != null && EmailDataSource.Count > 0)
            {
                foreach (string objEmailTo in EmailDataSource.OrderBy(i => i).ToList())
                {
                    if (!Properties.ContainsValue(objEmailTo))
                    {
                        Properties.Add(objEmailTo, objEmailTo);
                    }
                }
            }
            return Properties;
        } 
        #endregion

        #region ReportID
        private string _ReportID;
        [ModelDefault("AllowEdit", "False")]
        public string ReportID
        {
            get { return _ReportID; }
            set { SetPropertyValue(nameof(ReportID), ref _ReportID, value); }
        }
        #endregion

        #region TemplateName
        private Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup _TemplateName;
        [Appearance("DisableTemplateName", Enabled = false, Criteria = "Status != 'PendingSubmission'", Context = "DetailView")]
        [RuleRequiredField]        
        [XafDisplayName("Template")]
        public Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup TemplateName
        {
            get { return _TemplateName; }
            set { SetPropertyValue(nameof(TemplateName), ref _TemplateName, value); }
        }
        #endregion

        #region ReportName
        private string _ReportName;
        [Appearance("DisableReportName", Enabled = false, Criteria = "Status != 'PendingSubmission'", Context = "DetailView")]
        [RuleRequiredField]
        [XafDisplayName("Report")]
        public string ReportName
        {
            get { return _ReportName; }
            set { SetPropertyValue(nameof(ReportName), ref _ReportName, value); }
        }
        #endregion

        #region Category
        private ReportCategory _Category;
        [Appearance("DisableCategory", Enabled = false, Criteria = "Status != 'PendingSubmission'", Context = "DetailView")]
        public ReportCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue(nameof(Category), ref _Category, value); }
        }
        #endregion

        #region DateCollectedFrom
        private DateTime _DateCollectedFrom;
        [Appearance("DisableDateCollectedFrom", Enabled = false, Criteria = "Status != 'PendingSubmission'", Context = "DetailView")]
        public DateTime DateCollectedFrom
        {
            get { return _DateCollectedFrom; }
            set { SetPropertyValue(nameof(DateCollectedFrom), ref _DateCollectedFrom, value); }
        }
        #endregion

        #region DateCollectedTo
        private DateTime _DateCollectedTo;
        [Appearance("DisableDateCollectedTo", Enabled = false, Criteria = "Status != 'PendingSubmission'", Context = "DetailView")]
        public DateTime DateCollectedTo
        {
            get { return _DateCollectedTo; }
            set { SetPropertyValue(nameof(DateCollectedTo), ref _DateCollectedTo, value); }
        }
        #endregion

        #region DateCreated
        private DateTime _DateCreated;
        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set { SetPropertyValue(nameof(DateCreated), ref _DateCreated, value); }
        }
        #endregion

        #region ReportCreatedBy
        private CustomSystemUser _ReportCreatedBy;
        public CustomSystemUser ReportCreatedBy
        {
            get
            { return _ReportCreatedBy; }
            set
            { SetPropertyValue("CreatedBy", ref _ReportCreatedBy, value); }
        }
        #endregion

        #region DateApproved
        private DateTime _DateApproved;
        public DateTime DateApproved
        {
            get { return _DateApproved; }
            set { SetPropertyValue(nameof(DateApproved), ref _DateApproved, value); }
        }
        #endregion

        #region ApprovedBy
        private CustomSystemUser _ApprovedBy;
        public CustomSystemUser ApprovedBy
        {
            get
            { return _ApprovedBy; }
            set
            { SetPropertyValue("ApprovedBy", ref _ApprovedBy, value); }
        }
        #endregion

        #region DateDelivered
        private DateTime _DateDelivered;
        public DateTime DateDelivered
        {
            get { return _DateDelivered; }
            set { SetPropertyValue(nameof(DateDelivered), ref _DateDelivered, value); }
        }
        #endregion

        #region DeliveredBy
        private CustomSystemUser _DeliveredBy;
        public CustomSystemUser DeliveredBy
        {
            get
            { return _DeliveredBy; }
            set
            { SetPropertyValue("DeliveredBy", ref _DeliveredBy, value); }
        }
        #endregion

        #region Status
        private DWQRStatus _Status;
        public DWQRStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }
        #endregion

        #region DWQRFileContent
        private byte[] _DWQRFileContent;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public byte[] DWQRFileContent
        {
            get { return _DWQRFileContent; }
            set { SetPropertyValue(nameof(DWQRFileContent), ref _DWQRFileContent, value); }
        }
        #endregion

        #region EmailTo
        private string _EmailTo;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string EmailTo
        {
            get { return _EmailTo; }
            set { SetPropertyValue(nameof(EmailTo), ref _EmailTo, value); }
        }
        #endregion

        #region DrinkingWaterQualityReportsSampleparameter-Relation 

        private XPCollection<SampleParameter> _DWQRSampleParameter;
        [Association("DrinkingWaterQualityReportsSampleparameter", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<SampleParameter> DWQRSampleParameter
        {
            get
            {
                return GetCollection<SampleParameter>("DWQRSampleParameter");
            }
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

        #region EmailDataSource
        [Browsable(false)]
        public List<string> EmailDataSource
        {
            get
            {
                if (TemplateName != null && !string.IsNullOrEmpty(TemplateName.EmailTo))
                {
                    List<string> lstEmailOid = TemplateName.EmailTo.Split(',').ToList();
                    return lstEmailOid;
                }
                else
                {
                    return null;
                }
            }
        } 
        #endregion

        #region ItemsChanged
        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        } 
        #endregion        
    }
}