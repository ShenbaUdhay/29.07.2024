using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Module.BusinessObjects.Settings;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [XafDisplayName("Documents")]
    [RuleCombinationOfPropertiesIsUnique("NameAndCategory", DefaultContexts.Save, "Name, Category")]
    [Appearance("downloadmanualapp", Context = "ListView", TargetItems = "Download", Criteria = "[Download] = 'N/A'", FontColor = "Red")]
    [Appearance("downloadmanualavailable", Context = "ListView", TargetItems = "Download", Criteria = "[Download] = 'Available'", FontColor = "Blue", FontStyle = System.Drawing.FontStyle.Underline)]
    [Appearance("DateRetiredShow", AppearanceItemType = "ViewItem", TargetItems = "DateRetired", Criteria = "[IsRetire] = True", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("DateRetiredhide", AppearanceItemType = "ViewItem", TargetItems = "DateRetired", Criteria = "[IsRetire] = false", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("ReasonShow", AppearanceItemType = "ViewItem", TargetItems = "ReasonForRetiring", Criteria = "[IsRetire] = True", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("Reasonhide", AppearanceItemType = "ViewItem", TargetItems = "ReasonForRetiring", Criteria = "[IsRetire] = false", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("RetiredByShow", AppearanceItemType = "ViewItem", TargetItems = "RetiredBy", Criteria = "[IsRetire] = True", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("RetiredByhide", AppearanceItemType = "ViewItem", TargetItems = "RetiredBy", Criteria = "[IsRetire] = false", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("hideIsRetire", AppearanceItemType = "ViewItem", TargetItems = "IsRetire", Criteria = "[IsNew] = True", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("hidenewRetiredBy", AppearanceItemType = "ViewItem", TargetItems = "RetiredBy", Criteria = "[IsNew] = True", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("hidenewReasonForRetiring", AppearanceItemType = "ViewItem", TargetItems = "ReasonForRetiring", Criteria = "[IsNew] = True", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("hidenewDateRetired", AppearanceItemType = "ViewItem", TargetItems = "DateRetired", Criteria = "[IsNew] = True", Context = "Manual_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    public class Manual : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        MessageTimer timer = new MessageTimer();
        public Manual(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            DateRetired = DateTime.Now;
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            if (string.IsNullOrEmpty(DocumentID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(DocumentID, 3))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(Manual), criteria, null)) + 1).ToString();
                //var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "1")
                {
                    tempID = "DOC" + tempID;
                }
                else
                {
                    tempID = "DOC" + "1001";
                }

                DocumentID = tempID;

            }


            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            Author = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }
        private string _Name;
        [RuleRequiredField]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        private string _SubName;
        public string SubName
        {
            get { return _SubName; }
            set { SetPropertyValue("SubName", ref _SubName, value); }
        }

        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }

        private string _Product;
        public string Product
        {
            get { return _Product; }
            set { SetPropertyValue("Product", ref _Product, value); }
        }

        private string _VersionNumber;
        public string VersionNumber
        {
            get { return _VersionNumber; }
            set { SetPropertyValue("VersionNumber", ref _VersionNumber, value); }
        }

        [NonPersistent]
        public string RevNumber
        {
            get
            {
                if (Attachments.Where(i => i.IsActive == true).Count() == 0)
                {
                    return string.Empty;
                }
                else if (Attachments.Count > 0)
                {
                    //return Attachments.OrderByDescending(i => i.CreatedDate).FirstOrDefault().RevNumber;
                    return Attachments.OrderByDescending(i => i.IsActive).FirstOrDefault().RevNumber;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string _DocumentID;
        [ModelDefault("AllowEdit", "False")]
        public string DocumentID
        {
            get { return _DocumentID; }
            set { SetPropertyValue("DocumentID", ref _DocumentID, value); }
        }

        private bool _IsRetire;
        [ImmediatePostData]
        //[VisibleInListView(TargetCriteria = "[Document ID] Is Null And [Rev Number] Is Null")]
        public bool IsRetire
        {
            get { return _IsRetire; }
            set { SetPropertyValue("IsRetire", ref _IsRetire, value); }
        }

        private DateTime _DateRetired;
        [RuleRequiredField(TargetCriteria = "[IsRetire] = True")]
        public DateTime DateRetired
        {
            get { return _DateRetired; }
            set { SetPropertyValue("DateRetired", ref _DateRetired, value); }
        }

        private DateTime _DateReleased;
        public DateTime DateReleased
        {
            get { return _DateReleased; }
            set { SetPropertyValue("DateReleased", ref _DateReleased, value); }
        }

        private Employee _Owner;
        public Employee Owner
        {
            get { return _Owner; }
            set { SetPropertyValue("Owner", ref _Owner, value); }
        }
        private Employee _Author;
        public Employee Author
        {
            get { return _Author; }
            set { SetPropertyValue("Author", ref _Author, value); }
        }

        private FileData _Attachment;
        public virtual FileData Attachment
        {
            get { return _Attachment; }
            set { SetPropertyValue<FileData>("Attachment", ref _Attachment, value); }
        }

        private FileData _WordAttachment;
        public virtual FileData WordAttachment
        {
            get { return _WordAttachment; }
            set { SetPropertyValue<FileData>("WordAttachment", ref _WordAttachment, value); }
        }

        private string _Remark;
        [Size(1000)]
        public string Remark { get { return _Remark; } set { SetPropertyValue<string>("Remark", ref _Remark, value); } }

        private string _ReasonForRetiring;
        [Size(1000)]
        [RuleRequiredField(TargetCriteria = "[IsRetire] = True")]
        public string ReasonForRetiring
        {
            get { return _ReasonForRetiring; }
            set { SetPropertyValue<string>("ReasonForRetiring", ref _ReasonForRetiring, value); }
        }

        [Association("Manual-Attachment")]
        public XPCollection<DocumentAttachment> Attachments
        {
            get { return GetCollection<DocumentAttachment>("Attachments"); }
        }

        private DocumentCategory _Category;
        [RuleRequiredField]
        public DocumentCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue("Category", ref _Category, value); }
        }

        [NonPersistent]
        //[NonPersistent]
        public string Download
        {
            get
            {
                if (Attachments != null)
                {
                    if (Attachments.Where(i => i.IsActive == true).Count() == 0)
                    {
                        return "UnAvailable";
                    }
                    else if (Attachments.Count > 0)
                    {
                        return "Available";
                    }
                    else
                    {
                        return "N/A";
                    }
                }
                else
                {
                    return "N/A";
                }

            }

        }
        private Employee _RetiredBy;
        [RuleRequiredField(TargetCriteria = "[IsRetire] = True")]
        public Employee RetiredBy
        {
            get
            {
                if (IsRetire)
                {
                    _RetiredBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                }
                return _RetiredBy;
            }
            set { SetPropertyValue("RetiredBy", ref _RetiredBy, value); }
        }
        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }

        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }

        private DateTime _ModifiedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }

        private Employee _ModifiedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }
        }

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool IsNew
        {
            get
            {
                if (Attachments.Count > 0)
                {
                    return Session.IsNewObject(this);
                }
                else
                {
                    IsRetire = false;
                    ReasonForRetiring = null;
                    return true;
                }
            }
        }
    }
}