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
using DevExpress.ExpressApp.Editors;

namespace Modules.BusinessObjects.Assets
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class MaintenanceSetup : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public MaintenanceSetup(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = Library.GetServerTime(Session);
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            //RetireBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            System.Collections.ICollection lstReferenceObjects = Session.CollectReferencingObjects(this);
            if (lstReferenceObjects.Count > 0)
            {
                foreach (var obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.GetType() != typeof(DevExpress.Xpo.Metadata.Helpers.IntermediateObject))
                    {
                        Exception ex = new Exception("Already used can't allow to delete.");
                        throw ex;
                        break;
                    }
                }
            }
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
        DateTime _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue<DateTime>(nameof(CreatedDate), ref _CreatedDate, value); }
        }

        Employee _CreatedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue<Employee>(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        private Labware _InstrumentID;
        [RuleRequiredField]
        [ImmediatePostData]
        public Labware InstrumentID
        {
            get { return _InstrumentID; }
            set { SetPropertyValue<Labware>(nameof(InstrumentID), ref _InstrumentID, value); }
        }
        #region InstrumentName
        private string _InstrumentName;
        [ReadOnly(true)]
        [NonPersistent]
        public string InstrumentName
        {
            get
            {
                if (InstrumentID != null && InstrumentID.AssignedName != null)
                {
                    _InstrumentName = InstrumentID.AssignedName;
                }
                return _InstrumentName;
            }
            set { SetPropertyValue<string>("InstrumentName", ref _InstrumentName, value); }
        }
        #endregion
        //private MaintenanceCategory _Category;
        //[Browsable(false)]
        //public MaintenanceCategory Category
        //{
        //    get { return _Category; }
        //    set { SetPropertyValue<MaintenanceCategory>(nameof(Category), ref _Category, value); }
        //}
        private bool _Active;
        public bool Active
        {
            get { return _Active; }
            set { SetPropertyValue<bool>(nameof(Active), ref _Active, value); }
        }
        private Department _Department;
        [RuleRequiredField]
        [ImmediatePostData]
        public Department Department
        {
            get { return _Department; }
            set { SetPropertyValue<Department>(nameof(Department), ref _Department, value); }
        }
        private string _AssignTo;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string AssignTo
        {
            get { return _AssignTo; }
            set { SetPropertyValue(nameof(AssignTo), ref _AssignTo, value); }
        }
        [Browsable(false)]
        //[ImmediatePostData]
        [NonPersistent]
        public XPCollection<Employee> AssignToDataSource
        {
            get
            {
                return new XPCollection<Employee>(Session, CriteriaOperator.Parse("[UserName] <> 'Administrator' And [UserName] <> 'admin' And [UserName] <> 'Service'"));
            }
        }
        #region ICheckedListBoxItemsProvider
        Dictionary<object, string> ICheckedListBoxItemsProvider.GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "AssignTo" && AssignToDataSource != null && AssignToDataSource.Count > 0)
            {
                foreach (Employee objEmployee in AssignToDataSource.Where(i => i.FullName != null).OrderBy(i => i.FullName).ToList())
                {
                    if (!Properties.ContainsKey(objEmployee.Oid))
                    {
                        Properties.Add(objEmployee.Oid, objEmployee.DisplayName);
                    }
                }
            }
            return Properties;
        }

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }

        #endregion
        private DateTime _RetireDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [ImmediatePostData]
        public DateTime RetireDate
        {
            get { return _RetireDate; }
            set { SetPropertyValue<DateTime>(nameof(RetireDate), ref _RetireDate, value); }
        }

        private Employee _RetireBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Employee RetireBy
        {
            get { return _RetireBy; }
            set { SetPropertyValue<Employee>(nameof(RetireBy), ref _RetireBy, value); }
        }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue<string>(nameof(Remark), ref _Remark, value); }
        }
        private string _Category;
        [NonPersistent]
        [Size(SizeAttribute.Unlimited)]
        public string Category
        {
            get
            {
                if (Department != null)
                {
                    XPCollection<MaintenanceTaskCheckList> objMTCL = new XPCollection<MaintenanceTaskCheckList>(Session, (CriteriaOperator.Parse("[MaintenanceSetup.Oid]=? And [Department.Oid] = ?", Oid, Department.Oid)));
                    if (objMTCL.Count > 0)
                    {
                        _Category = string.Join(";", objMTCL.Cast<MaintenanceTaskCheckList>().Where(i => i.Category != null && i.Category.CategoryName != null).Select(i => i.Category.CategoryName).Distinct().ToList());
                    }
                }
                return _Category;
            }
        }
        private string _TaskDescription;
        [XafDisplayName("Task/Description")]
        [Size(SizeAttribute.Unlimited)]
        [NonPersistent]
        public string TaskDescription
        {
            get
            {
                if (Department != null)
                {
                    XPCollection<MaintenanceTaskCheckList> objMTCL = new XPCollection<MaintenanceTaskCheckList>(Session, (CriteriaOperator.Parse("[MaintenanceSetup.Oid]=? And [Department.Oid] = ?", Oid, Department.Oid)));
                    if (objMTCL.Count > 0)
                    {
                        _TaskDescription = string.Join(";", objMTCL.Cast<MaintenanceTaskCheckList>().Where(i => i.TaskDescription != null).Select(i => i.TaskDescription).Distinct().ToList());
                    }
                }
                return _TaskDescription;
            }
        }
    }
}