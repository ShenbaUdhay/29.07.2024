using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Modules.BusinessObjects.Hr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Module.BusinessObjects.Accounts
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EmailSetting : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EmailSetting(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            EnteredBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }


        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        private Employee _CreatedBy;

        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }

        }

        private bool _AssignedTo;
        public bool AssigendTo
        {
            get { return _AssignedTo; }
            set { SetPropertyValue("AssigendTo", ref _AssignedTo, value); }
        }

        private EmailAction _EmailAction;
        [DataSourceProperty("EmailSettings", DataSourcePropertyIsNullMode.SelectNothing)]
        [RuleRequiredField]
        public EmailAction EmailAction
        {
            get { return _EmailAction; }
            set { SetPropertyValue("EmailAction", ref _EmailAction, value); }
        }

        [ManyToManyAlias("ActionEmail", "EmailAction")]
        public IList<EmailAction> EmailActions
        {
            get
            {
                return GetList<EmailAction>("EmailActions");
            }

        }

        [Association, Browsable(false)]
        public IList<ActionEmail> ActionEmail
        {
            get
            {
                return GetList<ActionEmail>("ActionEmail");
            }
        }

        [ManyToManyAlias("EmailUser", "Employee")]
        public IList<Employee> DCUser
        {
            get
            {
                return GetList<Employee>("Employee");
            }

        }

        [Association, Browsable(false)]
        public IList<EmailUser> EmailUser
        {
            get
            {
                return GetList<EmailUser>("EmailUser");
            }
        }

        private Employee _EnteredBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee EnteredBy
        {
            get { return _EnteredBy; }
            set { SetPropertyValue("EnteredBy", ref _EnteredBy, value); }
        }

        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
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

        [NonPersistent]

        public XPCollection<EmailAction> EmailSettings
        {
            get
            {
                bool objEmailAction = false;
                XPCollection<EmailSetting> emailSettings = new XPCollection<EmailSetting>(Session);
                if (emailSettings != null && emailSettings.Count > 0)
                {
                    foreach (EmailSetting objEmail in emailSettings)
                    {
                        if (objEmail.EmailAction != null)
                        {
                            objEmailAction = true;
                        }
                        else
                        {
                            objEmailAction = false;
                        }
                    }
                    if (objEmailAction == true)
                    {
                        List<Guid> lstOid = emailSettings.Select(i => i.EmailAction.Oid).ToList();
                        if (lstOid != null && lstOid.Count > 0)
                        {
                            string strCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", lstOid.Select(i => i.ToString().Replace("'", "''")))) + ")";
                            return new XPCollection<EmailAction>(Session, CriteriaOperator.Parse(strCriteria));
                        }
                        else
                        {
                            return new XPCollection<EmailAction>(Session);
                        }
                    }
                    else
                    {
                        return new XPCollection<EmailAction>(Session);
                    }

                }
                else
                {
                    return new XPCollection<EmailAction>(Session);
                }
            }
        }

    }
}