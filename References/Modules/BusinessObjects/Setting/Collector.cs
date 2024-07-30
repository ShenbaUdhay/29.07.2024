using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.Crm
{
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("UniqueNameCombo", DefaultContexts.Save, "CustomerName,FirstName,LastName", TargetCriteria = "[CustomerName] Is Not Null")]
    //[RuleCombinationOfPropertiesIsUnique("UniqueEmpNameCombo", DefaultContexts.Save, "EmpFullName", TargetCriteria = "[CustomerName] Is Null")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Collector : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        curlanguage curlanguage = new curlanguage();
        public Collector(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            Createdby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
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
                        Exception ex = new Exception("Already used can't allow to delete");
                        throw ex;
                        break;

                    }
                }
            }
        }

        #region FullName
        private string Name;
        [NonPersistent]
        //[RuleUniqueValue]
        public string FullName
        {
            get
            {
                if (EmpFullName == null)
                {
                    if (curlanguage.strcurlanguage == "zh-CN")
                    {
                        Name = string.Format("{0}{1}", LastName, FirstName);
                    }
                    else if (curlanguage.strcurlanguage == "En")
                    {
                        Name = string.Format("{0} {1}", FirstName, LastName);
                    }
                }
                else
                {
                    if (curlanguage.strcurlanguage == "zh-CN")
                    {
                        //Name = string.Format("{0}{1}", EmpFullName.LastName, EmpFullName.FirstName);
                        Name = EmpFullName.DisplayName;
                    }
                    else if (curlanguage.strcurlanguage == "En")
                    {
                        //Name = string.Format("{0} {1}", EmpFullName.FirstName, EmpFullName.LastName);
                        Name = EmpFullName.DisplayName;
                    }
                }
                return Name;
            }
        }
        #endregion

        #region FirstName
        private string _FirstName;
        //[RuleRequiredField(CustomMessageTemplate = "'FirstName' must not be empty" ,TargetCriteria = "[CustomerName] is Not Null" )]
        [RuleRequiredField("FirstName", DefaultContexts.Save, "'First Name' must not be empty")/*,  TargetCriteria = "[CustomerName] is Not Null")*/]

        [ImmediatePostData]
        public string FirstName
        {
            get { return _FirstName; }
            set { if (value == null) value = string.Empty; SetPropertyValue<string>(nameof(FirstName), ref _FirstName, value.Trim()); }
        }
        #endregion

        #region LastName
        private string _LastName;
        [ImmediatePostData]
        public string LastName
        {
            get { return _LastName; }
            set { SetPropertyValue<string>(nameof(LastName), ref _LastName, value); }
        }
        #endregion

        private Employee _EmpFullName;
        [ImmediatePostData]
        [RuleRequiredField(TargetContextIDs = "Collector_DetailView")]
        public Employee EmpFullName
        {
            get { return _EmpFullName; }
            set { SetPropertyValue(nameof(EmpFullName), ref _EmpFullName, value); }
        }

        #region ContactPhone
        private string _ContactPhone;

        public string ContactPhone
        {
            get { return _ContactPhone; }
            set { SetPropertyValue<string>(nameof(ContactPhone), ref _ContactPhone, value); }
        }
        #endregion

        #region IsContractor
        private bool _IsContractor;
        public bool IsContractor
        {
            get { return _IsContractor; }
            set { SetPropertyValue<bool>(nameof(IsContractor), ref _IsContractor, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>(nameof(Comment), ref _Comment, value); }
        }
        #endregion

        private Customer _CustomerName;
        [Association("Customer-Collectors")]
        //[Browsable(false)]
        public Customer CustomerName
        {
            get { return _CustomerName; }
            set { SetPropertyValue("CustomerName", ref _CustomerName, value); }
        }

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue<DateTime>(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region Createdby
        private CustomSystemUser _Createdby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser Createdby
        {
            get { return _Createdby; }
            set { SetPropertyValue<CustomSystemUser>(nameof(Createdby), ref _Createdby, value); }
        }
        #endregion


        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue<DateTime>(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue<CustomSystemUser>(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #endregion

        #region IsFieldUser
        private bool _IsFieldUser;
        public bool IsFieldUser
        {
            get { return _IsFieldUser; }
            set { SetPropertyValue<bool>(nameof(IsFieldUser), ref _IsFieldUser, value); }
        }
        #endregion

        //private Customer clientName;
        //[RuleRequiredField]
        //public Customer ClientName
        //{
        //    get
        //    {
        //        return clientName;
        //    }
        //    set
        //    {
        //        SetPropertyValue("ClientName", ref clientName, value);
        //    }
        //}

        //private Employee collectorName;
        //[RuleRequiredField]
        //public Employee CollectorName
        //{
        //    get
        //    {
        //        return collectorName;
        //    }
        //    set
        //    {
        //        SetPropertyValue("CollectorName", ref collectorName, value);
        //    }
        //}

        private bool dontShow;
        [ImmediatePostData(true)]
        public bool DontShow
        {
            get
            {
                return dontShow;
            }
            set
            {
                SetPropertyValue(nameof(DontShow), ref dontShow, value);
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            {
                //if (CollectorName != null)
                //{
                //    EmpFullName = CollectorName;
                //}
            }
        }

    }
}