using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class SamplePrepTemplates : BaseObject
    {
        public SamplePrepTemplates(Session session)
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
            if (string.IsNullOrEmpty(TemplateID))
            {
                string strID = (Convert.ToInt32(Session.Evaluate(typeof(SamplePrepTemplates), CriteriaOperator.Parse("MAX(TemplateID)"), null)) + 1).ToString();
                if (strID.Length == 1)
                {
                    strID = "00" + strID;
                }
                else if (strID.Length == 2)
                {
                    strID = "0" + strID;
                }
                TemplateID = strID;
            }
        }

        #region TemplateID
        private string _TemplateID;
        public string TemplateID
        {
            get
            {
                return _TemplateID;
            }
            set
            {
                SetPropertyValue<string>(nameof(TemplateID), ref _TemplateID, value);
            }
        }
        #endregion

        #region TemplateName
        private string _TemplateName;
        [RuleRequiredField]
        [RuleUniqueValue]
        public string TemplateName
        {
            get
            {
                return _TemplateName;
            }
            set
            {
                SetPropertyValue<string>(nameof(TemplateName), ref _TemplateName, value);
            }
        }
        #endregion

        #region VisualMatrix
        private VisualMatrix _VisualMatrix;
        public VisualMatrix VisualMatrix
        {
            get
            {
                return _VisualMatrix;
            }
            set
            {
                SetPropertyValue<VisualMatrix>(nameof(VisualMatrix), ref _VisualMatrix, value);
            }
        }
        #endregion

        #region IsActive
        private bool _IsActive;
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                SetPropertyValue<bool>(nameof(IsActive), ref _IsActive, value);
            }
        }
        #endregion

        //#region IsPretreatment
        //private bool _IsPretreatment;
        //[ImmediatePostData]
        //public bool IsPretreatment
        //{
        //    get
        //    {
        //        return _IsPretreatment;
        //    }
        //    set
        //    {
        //        SetPropertyValue<bool>(nameof(IsPretreatment),ref _IsPretreatment, value);
        //    }
        //}
        //#endregion

        //#region PrepMethod
        //private PrepMethod1 _PrepMethod;
        //[Appearance("HidePrepMethod", Visibility = ViewItemVisibility.Hide, Criteria = "IsPretreatment", Context = "DetailView")]
        //[Appearance("ShowPrepMethod", Visibility = ViewItemVisibility.Show, Criteria = "!IsPretreatment", Context = "DetailView")]
        //[Appearance("DisablePrepMethod", Enabled = false, Criteria = "IsPretreatment", Context = "ListView")]
        //[Appearance("EnablePrepMethod", Enabled = true, Criteria = "!IsPretreatment", Context = "ListView")]
        //public PrepMethod1 PrepMethod
        //{
        //    get { return _PrepMethod; }
        //    set { SetPropertyValue<PrepMethod1>(nameof(PrepMethod), ref _PrepMethod, value); }
        //}
        //#endregion

        #region Comment
        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>(nameof(Comment), ref _Comment, value); }
        }
        #endregion

        #region SelectedFields
        [Association("SamplePrepTemplates-SamplePrepTemplateFields")]
        public XPCollection<SamplePrepTemplateFields> SelectedFields
        {
            get { return GetCollection<SamplePrepTemplateFields>(nameof(SelectedFields)); }
        }
        #endregion

    }
}