using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.SDMS
{
    public partial class SpreadSheetBuilder_TestParameter : XPLiteObject//BaseObject
    {
        public SpreadSheetBuilder_TestParameter(Session session) : base(session) { }

        long fuqID;
        [Key(true)]
        public long uqID
        {
            get { return fuqID; }
            set { SetPropertyValue<long>(nameof(uqID), ref fuqID, value); }
        }
        int fTemplateID;
        public int TemplateID
        {
            get { return fTemplateID; }
            set { SetPropertyValue<int>(nameof(TemplateID), ref fTemplateID, value); }
        }

        //private SpreadSheetBuilder_TemplateInfo _Template;
        //public SpreadSheetBuilder_TemplateInfo Template
        //{
        //    get { return _Template; }
        //    set { SetPropertyValue<SpreadSheetBuilder_TemplateInfo>(nameof(Template), ref _Template, value); }
        //}
        Guid fTestMethodID;
        public Guid TestMethodID
        {
            get { return fTestMethodID; }
            set { SetPropertyValue<Guid>(nameof(TestMethodID), ref fTestMethodID, value); }
        }
        Guid fTestParameterID;
        public Guid TestParameterID
        {
            get { return fTestParameterID; }
            set { SetPropertyValue<Guid>(nameof(TestParameterID), ref fTestParameterID, value); }
        }
        string fParameter;
        [Size(50)]
        public string Parameter
        {
            get { return fParameter; }
            set { SetPropertyValue<string>(nameof(Parameter), ref fParameter, value); }
        }
        string fSupportingParameter;
        [Size(500)]
        public string SupportingParameter
        {
            get { return fSupportingParameter; }
            set { SetPropertyValue<string>(nameof(SupportingParameter), ref fSupportingParameter, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}