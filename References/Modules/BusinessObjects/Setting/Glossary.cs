using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty("Term")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Glossary : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Glossary(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(Code))
            {
                string strCode = (Convert.ToInt32(Session.Evaluate<Glossary>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Max(Code)"), null)) + 1).ToString();
                if (strCode.Length == 1)
                {
                    Code = "00" + strCode;
                }
                else if (strCode.Length == 2)
                {
                    Code = "0" + strCode;
                }
                else
                {
                    Code = strCode;
                }
            }
        }

        #region Term
        string _Term;
        [RuleRequiredField("Term", DefaultContexts.Save)]
        public string Term
        {
            get { return _Term; }
            set { SetPropertyValue<string>(nameof(Term), ref _Term, value); }
        }
        #endregion

        #region definition
        string _Definition;
        [Size(SizeAttribute.Unlimited)]
        public string Definition
        {
            get { return _Definition; }
            set { SetPropertyValue<string>(nameof(Definition), ref _Definition, value); }
        }
        #endregion


        #region synonym
        string _Synonym;
        public string Synonym
        {
            get { return _Synonym; }
            set { SetPropertyValue<string>(nameof(Synonym), ref _Synonym, value); }
        }
        #endregion

        #region Symbol
        string _Symbol;
        public string Symbol
        {
            get { return _Symbol; }
            set { SetPropertyValue<string>(nameof(Symbol), ref _Symbol, value); }
        }
        #endregion

        #region Abbreviation
        string _Abbreviation;
        public string Abbreviation
        {
            get { return _Abbreviation; }
            set { SetPropertyValue<string>(nameof(Abbreviation), ref _Abbreviation, value); }
        }
        #endregion

        #region cn
        string _CN;
        public string CN
        {
            get { return _CN; }
            set { SetPropertyValue<string>(nameof(CN), ref _CN, value); }
        }
        #endregion

        #region Code
        string _Code;
        public string Code
        {
            get { return _Code; }
            set { SetPropertyValue<string>(nameof(Code), ref _Code, value); }
        }
        #endregion
    }
}