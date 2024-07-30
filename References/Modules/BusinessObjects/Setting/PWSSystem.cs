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
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [XafDefaultProperty(nameof(PWSSystemName))]
    public class PWSSystem : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PWSSystem(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedDate = Library.GetServerTime(Session);
            CreatedBy = (Employee)Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);

        } 
        private string _PWSSystemName;
        [RuleRequiredField]
        [RuleUniqueValue]
        public string PWSSystemName
        {
            get { return _PWSSystemName; }
            set { SetPropertyValue(nameof(PWSSystemName), ref _PWSSystemName, value.Trim()); }
        }
        #region CreatedDate

        private DateTime _CreatedDate;
        [Browsable(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue<DateTime>("CreatedDate", ref _CreatedDate, value); }

        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        [Browsable(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue<Employee>("CreatedBy", ref _CreatedBy, value); }
        }
        #endregion
    }
}