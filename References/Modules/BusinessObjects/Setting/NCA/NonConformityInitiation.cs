using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting.NCAID
{
    [DefaultClassOptions]
    [Appearance("ShowCorrectiveActionVerifications", AppearanceItemType = "LayoutItem", TargetItems = "Item6_Copy", Criteria = "[IsPermission] = True", Context = "NonConformityInitiation_DetailView_PendingVerification", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideCorrectiveActionVerifications", AppearanceItemType = "LayoutItem", TargetItems = "Item6_Copy", Criteria = "[IsPermission] = False", Context = "NonConformityInitiation_DetailView_PendingVerification", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    public class NonConformityInitiation : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public NonConformityInitiation(Session session)
            : base(session)
        {
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            Initiator = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            if (string.IsNullOrEmpty(NCAID))
            {
                string MaxID = string.Empty;
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(NCAID, 5))");
                int tempID = Convert.ToInt32(Session.Evaluate(typeof(NonConformityInitiation), criteria, null));
                var curdate = DateTime.Now.ToString("yy");
                if (tempID > 0)
                {
                    MaxID = (tempID + 1).ToString();

                    if (MaxID.Length == 1)
                    {
                        MaxID = "NCA" + curdate.ToString() + "00" + MaxID;
                    }
                    else if (MaxID.Length == 2)
                    {
                        MaxID = "NCA" + curdate.ToString() + "0" + MaxID;
                    }
                    else
                    {
                        MaxID = "NCA" + curdate.ToString() + MaxID;
                    }
                }
                else
                {
                    MaxID = "NCA" + curdate.ToString() + "001";
                }

                NCAID = MaxID;

            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        public enum NCAStatus
        {
            PendingSubmission,
            PendingVerification,
            Closed
        }
        private string _NCAID;
        public string NCAID
        {
            get { return _NCAID; }
            set { SetPropertyValue(nameof(NCAID), ref _NCAID, value); }
        }
        private DateTime _DateInitiated;
        public DateTime DateInitiated
        {
            get { return _DateInitiated; }
            set { SetPropertyValue(nameof(DateInitiated), ref _DateInitiated, value); }
        }
        private Employee _Initiator;
        public Employee Initiator
        {
            get { return _Initiator; }
            set { SetPropertyValue(nameof(Initiator), ref _Initiator, value); }
        }
        private string _Subject;
        public string Subject
        {
            get { return _Subject; }
            set { SetPropertyValue(nameof(Subject), ref _Subject, value); }
        }
        private Employee _AssignedTo;
        public Employee AssignedTo
        {
            get { return _AssignedTo; }
            set { SetPropertyValue(nameof(AssignedTo), ref _AssignedTo, value); }
        }
        private string _Responsible;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Responsible
        {
            get { return _Responsible; }
            set { SetPropertyValue(nameof(Responsible), ref _Responsible, value); }
        }
        private DateTime _InvestigateDueDate;
        [ImmediatePostData(true)]
        public DateTime InvestigateDueDate
        {
            get { return _InvestigateDueDate; }
            set { SetPropertyValue(nameof(InvestigateDueDate), ref _InvestigateDueDate, value); }
        }
        private bool _IsPermission;
        //[ImmediatePostData(true)]
        [NonPersistent]
        [Browsable(false)]
        public bool IsPermission
        {
            get { return _IsPermission; }
            set { SetPropertyValue(nameof(IsPermission), ref _IsPermission, value); }
        }
        private int _InvestigateDaysRemaining;
        //[ImmediatePostData(true)]
        public int InvestigateDaysRemaining
        {
            get
            {
                if (InvestigateDueDate != DateTime.MinValue)
                {
                    _InvestigateDaysRemaining = (InvestigateDueDate - DateTime.Today).Days;
                }
                else
                {
                    _InvestigateDaysRemaining = 0;
                }
                return _InvestigateDaysRemaining;
            }
            set { SetPropertyValue(nameof(InvestigateDaysRemaining), ref _InvestigateDaysRemaining, value); }
        }
        private DateTime _CorrectiveActionDueDate;
        [ImmediatePostData(true)]
        public DateTime CorrectiveActionDueDate
        {
            get { return _CorrectiveActionDueDate; }
            set { SetPropertyValue(nameof(CorrectiveActionDueDate), ref _CorrectiveActionDueDate, value); }
        }
        private int _CorrectiveDaysRemaining;
        //[ImmediatePostData(true)]
        public int CorrectiveDaysRemaining
        {
            get
            {
                if (CorrectiveActionDueDate != DateTime.MinValue)
                {
                    _CorrectiveDaysRemaining = (CorrectiveActionDueDate - DateTime.Today).Days;
                }
                else
                {
                    _CorrectiveDaysRemaining = 0;
                }
                return _CorrectiveDaysRemaining;
            }
            set { SetPropertyValue(nameof(CorrectiveDaysRemaining), ref _CorrectiveDaysRemaining, value); }
        }
        private NCAStatus _Status;
        public NCAStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }
        private string _AffectedDepartment;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string AffectedDepartment
        {
            get { return _AffectedDepartment; }
            set { SetPropertyValue(nameof(AffectedDepartment), ref _AffectedDepartment, value); }
        }
        private Customer _AffectedClients;
        public Customer AffectedClients
        {
            get { return _AffectedClients; }
            set { SetPropertyValue(nameof(AffectedClients), ref _AffectedClients, value); }
        }
        private ProblemCategory _ProblemCategory;
        public ProblemCategory ProblemCategory
        {
            get { return _ProblemCategory; }
            set { SetPropertyValue(nameof(ProblemCategory), ref _ProblemCategory, value); }
        }
        private string _ReasonDiscovered;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string ReasonDiscovered
        {
            get { return _ReasonDiscovered; }
            set { SetPropertyValue(nameof(ReasonDiscovered), ref _ReasonDiscovered, value); }
        }
        private string _IncidentSpecifics;
        [Size(SizeAttribute.Unlimited)]
        public string IncidentSpecifics
        {
            get { return _IncidentSpecifics; }
            set { SetPropertyValue(nameof(IncidentSpecifics), ref _IncidentSpecifics, value); }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Employee> ResponsibleDataSource
        {
            get
            {
                return new XPCollection<Employee>(Session, CriteriaOperator.Parse("[UserName] <> 'Administrator' And [UserName] <> 'admin' And [UserName] <> 'Service'"));
            }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Department> AffectedDepartmentDataSource
        {
            get
            {
                return new XPCollection<Department>(Session, CriteriaOperator.Parse(""));
            }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<NCAReason> ReasonDiscoveredDataSource
        {
            get
            {
                return new XPCollection<NCAReason>(Session, CriteriaOperator.Parse(""));
            }
        }
        #region ICheckedListBoxItemsProvider
        Dictionary<object, string> ICheckedListBoxItemsProvider.GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "Responsible" && ResponsibleDataSource != null && ResponsibleDataSource.Count > 0)
            {
                foreach (Employee objEmployee in ResponsibleDataSource.Where(i => i.DisplayName != null).OrderBy(i => i.DisplayName).ToList())
                {
                    if (!Properties.ContainsKey(objEmployee.Oid))
                    {
                        Properties.Add(objEmployee.Oid, objEmployee.DisplayName);
                    }
                }
            }
            if (targetMemberName == "AffectedDepartment" && AffectedDepartmentDataSource != null && AffectedDepartmentDataSource.Count > 0)
            {
                foreach (Department objDepartment in AffectedDepartmentDataSource.Where(i => i.Name != null).OrderBy(i => i.Name).ToList())
                {
                    if (!Properties.ContainsKey(objDepartment.Oid))
                    {
                        Properties.Add(objDepartment.Oid, objDepartment.Name);
                    }
                }
            }
            if (targetMemberName == "ReasonDiscovered" && ReasonDiscoveredDataSource != null && ReasonDiscoveredDataSource.Count > 0)
            {
                foreach (NCAReason objNCAReason in ReasonDiscoveredDataSource.Where(i => i.Reason != null).OrderBy(i => i.Reason).ToList())
                {
                    if (!Properties.ContainsKey(objNCAReason.Oid))
                    {
                        Properties.Add(objNCAReason.Oid, objNCAReason.Reason);
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
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
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
        [Association("NonConformityInitiation-CorrectiveActionLog")]
        public XPCollection<CorrectiveActionLog> CorrectiveActionLogs
        {
            get { return GetCollection<CorrectiveActionLog>("CorrectiveActionLogs"); }
        }
        [Association("NonConformityInitiation-CorrectiveActionVerification")]
        public XPCollection<CorrectiveActionVerification> CorrectiveActionVerifications
        {
            get { return GetCollection<CorrectiveActionVerification>("CorrectiveActionVerifications"); }
        }
    }
}