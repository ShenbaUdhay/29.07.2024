using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.SDA
{
    [DefaultClassOptions]
    public class SDATemplate : XPLiteObject
    {
        public SDATemplate(Session session) : base(session) { }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(TemplateID))
            {
                int id = Convert.ToInt32(Session.Evaluate(typeof(SDATemplate), CriteriaOperator.Parse("max(TemplateID)"), null)) + 1;
                if (id < 10)
                {
                    TemplateID = "00" + id;
                }
                else if (id < 100)
                {
                    TemplateID = "0" + id;
                }
                else
                {
                    TemplateID = id.ToString();
                }
            }
        }

        int fuqSDATemplateID;
        [Key(true)]
        public int uqSDATemplateID
        {
            get { return fuqSDATemplateID; }
            set { SetPropertyValue<int>(nameof(uqSDATemplateID), ref fuqSDATemplateID, value); }
        }

        string fTemplateID;
        public string TemplateID
        {
            get { return fTemplateID; }
            set { SetPropertyValue<string>(nameof(TemplateID), ref fTemplateID, value); }
        }

        string fTemplateName;
        [RuleUniqueValue, RuleRequiredField]
        public string TemplateName
        {
            get { return fTemplateName; }
            set { SetPropertyValue<string>(nameof(TemplateName), ref fTemplateName, value); }
        }

        TemplateCategory fCategory;
        public TemplateCategory Category
        {
            get { return fCategory; }
            set { SetPropertyValue<TemplateCategory>(nameof(Category), ref fCategory, value); }
        }

        TemplateElement fElement;
        [ImmediatePostData]
        public TemplateElement Element
        {
            get { return fElement; }
            set { SetPropertyValue<TemplateElement>(nameof(Element), ref fElement, value); }
        }

        bool fIsActive;
        public bool IsActive
        {
            get { return fIsActive; }
            set { SetPropertyValue<bool>(nameof(IsActive), ref fIsActive, value); }
        }

        string fComment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return fComment; }
            set { SetPropertyValue<string>(nameof(Comment), ref fComment, value); }
        }

        [Association("SDATemplateFields")]
        public XPCollection<SDATemplateDetail> SDATemplateFields
        {
            get
            {
                return GetCollection<SDATemplateDetail>(nameof(SDATemplateFields));
            }
        }
    }

    public enum TemplateCategory
    {
        [DevExpress.Xpo.DisplayName("Field Entry")]
        FieldEntry
    }

    public enum TemplateElement
    {
        Stream = 0,
        Lake = 1,
        Liquid = 2
    }
}