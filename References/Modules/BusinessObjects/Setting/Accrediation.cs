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

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty("lAccrediation")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Accrediation : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Accrediation(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        
        private string _lAccrediation;
        [ImmediatePostData]
        [RuleRequiredField("Accrediation must not be empty.", DefaultContexts.Save)]

        [RuleUniqueValue]
        [XafDisplayName("Accrediation")]
        public string lAccrediation
        {
            get { return _lAccrediation; }
            set { SetPropertyValue<string>(nameof(lAccrediation), ref _lAccrediation, value); }
        }

        private string _CertificateNumber;
        [ImmediatePostData]
        public string CertificateNumber
        {
            get { return _CertificateNumber; }
            set { SetPropertyValue<string>(nameof(CertificateNumber), ref _CertificateNumber, value); }
        }

        private Boolean _DefaultAccrediation;
        [ImmediatePostData]
        public Boolean DefaultAccrediation
        {
            get { return _DefaultAccrediation; }
            set { SetPropertyValue<Boolean>(nameof(DefaultAccrediation), ref _DefaultAccrediation, value); }
        }
    }
}