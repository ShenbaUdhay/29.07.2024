using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty("Software")]
    //[XafDisplayName("Parser Setup")]

    public class InstrumentSoftware : BaseObject, ICheckedListBoxItemsProvider
    {
        public InstrumentSoftware(Session session)
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

        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (BaseObject obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.Oid != null)
                    {
                        Exception ex = new Exception("Please note that this item can not be deleted since it has referenced already.");
                        throw ex;
                        break;

                    }
                }
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString()))
                return;

            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);

            if (string.IsNullOrEmpty(ParserID))
            {
                // Find the maximum existing ParserID
                CriteriaOperator parserCriteria = CriteriaOperator.Parse("Max(SUBSTRING(ParserID, 2))");
                object maxParserID = Session.Evaluate(typeof(InstrumentSoftware), parserCriteria, null);

                int nextParserIDNumber = 1;
                if (maxParserID != null && maxParserID != DBNull.Value)
                {
                    // Increment the maximum ID by 1 to get the next ID number
                    nextParserIDNumber = Convert.ToInt32(maxParserID) + 1;
                }

                // Format the next ParserID
                string nextParserID = $"P{nextParserIDNumber:D3}";

                ParserID = nextParserID;
            }

            if (string.IsNullOrEmpty(OrganizationID))
            {
                // Find the maximum existing OrganizationID
                CriteriaOperator orgCriteria = CriteriaOperator.Parse("Max(SUBSTRING(OrganizationID, 4))");
                object maxOrgID = Session.Evaluate(typeof(InstrumentSoftware), orgCriteria, null);

                int nextOrgIDNumber = 1;
                if (maxOrgID != null && maxOrgID != DBNull.Value)
                {
                    // Increment the maximum ID by 1 to get the next ID number
                    nextOrgIDNumber = Convert.ToInt32(maxOrgID) + 1;
                }

                // Format the next OrganizationID
                string nextOrgID = $"ORG{nextOrgIDNumber:D3}";

                OrganizationID = nextOrgID;
            }
        }
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "LinkInstrument" && LinkInstrumentDataSource != null && LinkInstrumentDataSource.Count > 0)
            {
                foreach (Modules.BusinessObjects.Assets.Labware objLinkInstrument in LinkInstrumentDataSource.Where(i => i.LabwareName != null).OrderBy(i => i.LabwareName).ToList())
                {
                    if (!Properties.ContainsValue(objLinkInstrument.LabwareName))
                    {
                        Properties.Add(objLinkInstrument.Oid, objLinkInstrument.LabwareName);
                    }
                }
            }
            return Properties;
        }

        #region ParserID
        private string _ParserID;
        [ModelDefault("AllowEdit", "False")]
        public string ParserID
        {
            get { return _ParserID; }
            set { SetPropertyValue(nameof(ParserID), ref _ParserID, value); }
        }
        #endregion

        #region ParserName
        private string _ParserName;
        [RuleUniqueValue]
        public string ParserName 
        {
            get { return _ParserName; }
            set { SetPropertyValue(nameof(ParserName), ref _ParserName, value); }
        }
        #endregion

        #region OrganizationID
        private string _OrganizationID;
        [ModelDefault("AllowEdit", "False")]
        public string OrganizationID
        {
            get { return _OrganizationID; }
            set { SetPropertyValue(nameof(OrganizationID), ref _OrganizationID, value); }
        }
        #endregion

        #region Software
        private string _Software;
        [RuleRequiredField, RuleUniqueValue]
        [XafDisplayName("Software Name")]
        public string Software
        {
            get { return _Software; }
            set { SetPropertyValue(nameof(Software), ref _Software, value); }
        }
        #endregion

        #region SoftwareVersion
        private string _SoftwareVersion;
        public string SoftwareVersion
        {
            get { return _SoftwareVersion; }
            set { SetPropertyValue(nameof(SoftwareVersion), ref _SoftwareVersion, value); }
        }
        #endregion

        #region LinkInstrument
        private string _LinkInstrument;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string LinkInstrument
        {
            get { return _LinkInstrument; }
            set { SetPropertyValue(nameof(LinkInstrument), ref _LinkInstrument, value); }
        }
        #endregion

        #region Description
        private string _Description;
        //[EditorAlias(EditorAliases.RichTextPropertyEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }
        #endregion

        #region ModelNo
        private string _ModelNo;
        public string ModelNo
        {
            get { return _ModelNo; }
            set { SetPropertyValue(nameof(ModelNo), ref _ModelNo, value); }
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

        #region DateRetired
        private DateTime _DateRetired;
        [ModelDefault("AllowEdit", "False")]
        public DateTime DateRetired
        {
            get
            {
                return _DateRetired;
            }
            set
            {
                SetPropertyValue("DateRetired", ref _DateRetired, value);
            }
        }
        #endregion

        #region InstrumentFile
        private FileData _InstrumentFile;
        [Size(SizeAttribute.Unlimited)]
        [FileTypeFilter("All Files|*.*")]
        public FileData InstrumentFile
        {
            get { return _InstrumentFile; }
            set { SetPropertyValue(nameof(InstrumentFile), ref _InstrumentFile, value); }
        } 
        #endregion

        #region ParserCode
        private FileData _ParserCode;
        [Size(SizeAttribute.Unlimited)]
        [FileTypeFilter("All Files|*.*")]
        public FileData ParserCode
        {
            get { return _ParserCode; }
            set { SetPropertyValue(nameof(ParserCode), ref _ParserCode, value); }
        } 
        #endregion

        #region LinkInstrumentDataSource
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Modules.BusinessObjects.Assets.Labware> LinkInstrumentDataSource
        {
            get
            {
                return new XPCollection<Modules.BusinessObjects.Assets.Labware>(Session, CriteriaOperator.Parse(""));
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