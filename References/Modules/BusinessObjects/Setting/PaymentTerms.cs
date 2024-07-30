using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
   
    public class PaymentTerms : BaseObject
    {
        public PaymentTerms(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #region Terms
        private uint _Terms;
        [RuleRequiredField]
        [RuleUniqueValue]
        public uint  Terms
        {
            get { return _Terms; }
            set { SetPropertyValue(nameof(Terms), ref _Terms, value); }
        }
        #endregion
    }
}