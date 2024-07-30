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

namespace Modules.BusinessObjects.Report
{
    [DefaultClassOptions]
   
    public class EmailForms : BaseObject
    { 
        public EmailForms(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #region FormName
        private string _FormName;
        public string FormName
        {
            get { return _FormName; }
            set { SetPropertyValue("FormName", ref _FormName, value); }
        }
        #endregion
    }
}