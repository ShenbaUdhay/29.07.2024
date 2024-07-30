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
    [DefaultProperty("Analyte")]
    
    public class MCLAndSCLLimits : BaseObject
    { 
        public MCLAndSCLLimits(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            {
                if (Comment != null)
                {
                    AnalyteNotes = true;
                }
            }            
        }

        private Parameter analyte;
        [RuleRequiredField("AnalyteMSCL", DefaultContexts.Save, "'Analyte' must not be empty.")]
        public Parameter Analyte
        {
            get
            {
                return analyte;
            }
            set
            {
                SetPropertyValue("Analyte", ref analyte, value);
            }
        }

        private string mcl;
        public string MCL
        {
            get
            {
                return mcl;
            }
            set
            {
                SetPropertyValue("MCL", ref mcl, value);
            }
        }

        private Unit mclunit;
        public Unit MCLUnit
        {
            get
            {
                return mclunit;
            }
            set
            {
                SetPropertyValue("MCLUnit", ref mclunit, value);
            }
        }

        private string mclg;
        public string MCLG
        {
            get
            {
                return mclg;
            }
            set
            {
                SetPropertyValue("MCLG", ref mclg, value);
            }
        }

        private Unit mclgunit;
        public Unit MCLGUnit
        {
            get
            {
                return mclgunit;
            }
            set
            {
                SetPropertyValue("MCLGUnit", ref mclgunit, value);
            }
        }

        private string scl;
        public string SCL
        {
            get
            {
                return scl;
            }
            set
            {
                SetPropertyValue("SCL", ref scl, value);
            }
        }

        private Unit sclunit;
        public Unit SCLUnit
        {
            get
            {
                return sclunit;
            }
            set
            {
                SetPropertyValue("SCLUnit", ref sclunit, value);
            }
        }

        private string alert;
        public string Alert
        {
            get
            {
                return alert;
            }
            set
            {
                SetPropertyValue("Alert", ref alert, value);
            }
        }

        private Unit alertunit;
        public Unit AlertUnit
        {
            get
            {
                return alertunit;
            }
            set
            {
                SetPropertyValue("AlertUnit", ref alertunit, value);
            }
        }

        private string mrllevel;
        public string MRLLevel
        {
            get
            {
                return mrllevel;
            }
            set
            {
                SetPropertyValue("MRLLevel", ref mrllevel, value);
            }
        }

        private Unit mrlunits;
        public Unit MRLUnits
        {
            get
            {
                return mrlunits;
            }
            set
            {
                SetPropertyValue("MRLUnits", ref mrlunits, value);
            }
        }

        private bool analytenotes;
        [ImmediatePostData(true)]
        [VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool AnalyteNotes
        {
            get 
            { 
                return analytenotes;
            }
            set 
            { 
                SetPropertyValue(nameof(AnalyteNotes), ref analytenotes, value); 
            }
        }

        private string _Comment;
        [VisibleInListView(false)]
        [XafDisplayName("Analyte Notes")]
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
    }
}