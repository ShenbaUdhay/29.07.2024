using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.XtraScheduler;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting.Labware;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.BusinessObjects.Assets
{
    [DefaultClassOptions]
    [Appearance("ShowTestmethods", AppearanceItemType = "LayoutItem", TargetItems = "SkipReason", Criteria = "Skip = True", Context = "MaintenanceTaskCheckList_DetailView_MaintenanceQueue", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestmethods", AppearanceItemType = "LayoutItem", TargetItems = "SkipReason", Criteria = "Skip = False", Context = "MaintenanceTaskCheckList_DetailView_MaintenanceQueue", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    public class MaintenanceTaskCheckList : Event
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public MaintenanceTaskCheckList(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = Library.GetServerTime(Session);
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
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
            if (DateToMaintain == DateTime.MinValue && RecurrenceInfoXml != null)
            {
                DateToMaintain = StartOn;
            }
        }

        private MaintenanceCategory _Category;
        public MaintenanceCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue<MaintenanceCategory>(nameof(Category), ref _Category, value); }
        }
        private Department _Department;
        public Department Department
        {
            get { return _Department; }
            set { SetPropertyValue<Department>(nameof(Department), ref _Department, value); }
        }
        private string _TaskDescription;
        [XafDisplayName("Task/Description")]
        [Size(SizeAttribute.Unlimited)]
        public string TaskDescription
        {
            get { return _TaskDescription; }
            set { SetPropertyValue<string>(nameof(TaskDescription), ref _TaskDescription, value); }
        }
        DateTime _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue<DateTime>(nameof(CreatedDate), ref _CreatedDate, value); }
        }

        CustomSystemUser _CreatedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue<CustomSystemUser>(nameof(CreatedBy), ref _CreatedBy, value); }
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
        private string _AssignTo;
        [NonPersistent]
        [Size(SizeAttribute.Unlimited)]
        public string AssignTo
        {
            get
            {
                if (MaintenanceSetup != null && MaintenanceSetup.AssignTo != null)
                {
                    List<string> lstSM = new List<string>();
                    List<string> lstAssignTo = MaintenanceSetup.AssignTo.Split(';').ToList();
                    if (lstAssignTo != null)
                    {
                        foreach (string objOid in lstAssignTo)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Employee objE = Session.GetObjectByKey<Employee>(new Guid(objOid.Trim()));
                                if (objE != null && !lstSM.Contains(objE.UserName))
                                {
                                    lstSM.Add(objE.UserName);
                                }
                            }

                        }
                    }
                    _AssignTo = string.Join(";", lstSM.Cast<string>());
                }
                return _AssignTo;
            }
            set { SetPropertyValue(nameof(AssignTo), ref _AssignTo, value); }
        }
        #region TaskCheckList      
        private TaskCheckList _TaskChecklist;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public TaskCheckList TaskChecklist
        {
            get { return _TaskChecklist; }
            set { SetPropertyValue("TaskChecklist", ref _TaskChecklist, value); }
        }
        #endregion
        #region MaintenanceSetup      
        private MaintenanceSetup _MaintenanceSetup;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public MaintenanceSetup MaintenanceSetup
        {
            get { return _MaintenanceSetup; }
            set { SetPropertyValue("MaintenanceSetup", ref _MaintenanceSetup, value); }
        }
        #endregion
        private string _ActionDescription;
        [Size(SizeAttribute.Unlimited)]
        public string ActionDescription
        {
            get { return _ActionDescription; }
            set { SetPropertyValue<string>(nameof(ActionDescription), ref _ActionDescription, value); }
        }
        private FileData _Attachment;
        public FileData Attachment
        {
            get { return _Attachment; }
            set { SetPropertyValue<FileData>(nameof(Attachment), ref _Attachment, value); }
        }
        private DateTime _MaintainedDate;
        public DateTime MaintainedDate
        {
            get { return _MaintainedDate; }
            set { SetPropertyValue<DateTime>(nameof(MaintainedDate), ref _MaintainedDate, value); }
        }
        private Employee _MaintainedBy;
        public Employee MaintainedBy
        {
            get { return _MaintainedBy; }
            set { SetPropertyValue<Employee>(nameof(MaintainedBy), ref _MaintainedBy, value); }
        }
        private bool _Skip;
        [ImmediatePostData]
        public bool Skip
        {
            get { return _Skip; }
            set { SetPropertyValue<bool>(nameof(Skip), ref _Skip, value); }
        }
        private SkipReason _SkipReason;
        public SkipReason SkipReason
        {
            get { return _SkipReason; }
            set { SetPropertyValue<SkipReason>(nameof(SkipReason), ref _SkipReason, value); }
        }
        private DateTime _NextMaintainDate;
        public DateTime NextMaintainDate
        {
            get
            {
                return _NextMaintainDate;
            }
            set { SetPropertyValue<DateTime>(nameof(NextMaintainDate), ref _NextMaintainDate, value); }
        }
        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>(nameof(Comment), ref _Comment, value); }
        }
        private InstrumentMaintenanceStatus _MaintenanceStatus;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public InstrumentMaintenanceStatus MaintenanceStatus
        {
            get { return _MaintenanceStatus; }
            set { SetPropertyValue<InstrumentMaintenanceStatus>(nameof(MaintenanceStatus), ref _MaintenanceStatus, value); }
        }
        private string _Frequency;
        [NonPersistent]
        public string Frequency
        {
            get
            {
                if (RecurrenceInfoXml != null)
                {
                    RecurrenceInfo info = new RecurrenceInfo();
                    info.FromXml(RecurrenceInfoXml);
                    _Frequency = info.Type.ToString();
                }
                return _Frequency;
            }

        }
        private string _MaintenanceId;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string MaintenanceId
        {
            get { return _MaintenanceId; }
            set { SetPropertyValue<string>(nameof(MaintenanceId), ref _MaintenanceId, value); }
        }
        private DateTime _DateToMaintain;
        public DateTime DateToMaintain
        {
            get { return _DateToMaintain; }
            set { SetPropertyValue<DateTime>(nameof(DateToMaintain), ref _DateToMaintain, value); }
        }
    }
}