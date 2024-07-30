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

namespace Modules.BusinessObjects.Setting.SamplesSite
{
    [DefaultClassOptions]
    [XafDefaultProperty(nameof(WaterType))]
    public class WaterTypes : BaseObject
    { 
        public WaterTypes(Session session)
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
        private string _WaterType;
        [RuleRequiredField]
        [RuleUniqueValue]
        public string WaterType
        {
            get { return _WaterType; }
            set { SetPropertyValue(nameof(WaterType), ref _WaterType, value.Trim()); }
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