using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
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

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    //[RuleCombinationOfPropertiesIsUnique("QCType", DefaultContexts.Save, "QCTypeName", SkipNullOrEmptyValues = false)]
    public class QCType : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public QCType(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            //QCRole = QCRoleCN.空白;
            //QCRootRole = QCRoleCN.空白;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (var obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.GetType() != typeof(DevExpress.Xpo.Metadata.Helpers.IntermediateObject))
                    {
                        Exception ex = new Exception("Unable to delete since the data already used in another form");
                        throw ex;
                        break;

                    }
                }
            }
        }

        #region QCType
        private string _QCType;
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The QC Type already saved.")]
        [RuleRequiredField("QCTypeName", DefaultContexts.Save, "QC Type must not be empty")]
        public string QCTypeName
        {
            get { return _QCType; }
            set { SetPropertyValue("QCType", ref _QCType, value); }
        }
        #endregion



        #region Comment
        private string _Comment;
        [Size(1000)]
        public String Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }

        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("MD5", Enabled = false, Context = "DetailView")]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }

        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("MB5", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }

        }
        #endregion

        #region Sort
        private int _Sort;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [ImmediatePostData]
        public int Sort
        {
            get { return _Sort; }
            set { SetPropertyValue("Sort", ref _Sort, value); }

        }
        #endregion

        #region IListforTestParameter

        [Association("TestMethodQCType")]

        public XPCollection<TestMethod> TestMethods
        {
            get
            {
                return GetCollection<TestMethod>(nameof(TestMethods));
            }
        }
        #endregion IListforTestParameter

        #region IListforQCParameter
        [Association, Browsable(false)]
        public IList<QcParameter> QCParameter
        {
            get
            {
                return GetList<QcParameter>("QCParameter");
            }
        }
        #endregion


        #region IListforQCParameter
        [Association, Browsable(false)]
        public IList<Testparameter> Testparameter
        {
            get
            {
                return GetList<Testparameter>("Testparameter");
            }
        }
        #endregion


        //Index for the QCType listview in the TestParameter Edit PopupWindow
        private int _index;
        [NonPersistent]
        [VisibleInDashboards(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int index
        {
            get
            {
                if (QCTypeName == "Sample")
                {
                    index = 0;
                }
                else
                {
                    index = 1;
                }
                return _index;
            }
            set
            {
                SetPropertyValue<int>(nameof(index), ref _index, value);
            }
        }


        #region initialqctypes
        [Association("Initialqctype-QCType")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public XPCollection<TestMethod> Initialqctypes
        {
            get { return GetCollection<TestMethod>(nameof(Initialqctypes)); }
        }
        #endregion
        #region sampleqctypes
        [Association("Sampleqctype-QCType")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public XPCollection<TestMethod> Sampleqctypes
        {
            get { return GetCollection<TestMethod>(nameof(Sampleqctypes)); }
        }
        #endregion
        #region closingqctypes
        [Association("Closingqctype-QCType")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public XPCollection<TestMethod> closingqctypes
        {
            get { return GetCollection<TestMethod>(nameof(closingqctypes)); }
        }
        #endregion

        #region SequenceOrder
        private int _SequenceOrder;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public int SequenceOrder
        {
            get { return _SequenceOrder; }
            set { SetPropertyValue("SequenceOrder", ref _SequenceOrder, value); }
        }
        #endregion

        #region QcSource
        private QCSource _qCSource;
        public QCSource QCSource
        {
            get { return _qCSource; }
            set { SetPropertyValue(nameof(QCSource), ref _qCSource, value); }
        }
        #endregion

        #region QCRole
        private QcRole _QcRole;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public QcRole QcRole
        {
            get { return _QcRole; }
            set { SetPropertyValue(nameof(QcRole), ref _QcRole, value); }
        }
        #endregion

        #region QcRootRole
        private QCRootRole _QCRootRole;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public QCRootRole QCRootRole
        {
            get { return _QCRootRole; }
            set { SetPropertyValue(nameof(QCRootRole), ref _QCRootRole, value); }
        }
        #endregion
    }
}