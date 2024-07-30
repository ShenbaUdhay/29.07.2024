using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty("Status")]




    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class StatusDefinition : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public StatusDefinition(Session session)
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
            if (UqIndexID == 0)
            {
                UqIndexID = Convert.ToInt32(Session.Evaluate(typeof(StatusDefinition), CriteriaOperator.Parse("MAX(UqIndexID)"), null)) + 1;
            }

        }
        private string _Status;
        [RuleRequiredField("Statusde", DefaultContexts.Save, "'Status' must not be empty.")]
        [RuleUniqueValue]
        public string Status
        {
            get { return _Status; }
            set { SetPropertyValue<string>("Status", ref _Status, value); }

        }

        private int _Index;
        [RuleRequiredField]
        [RuleUniqueValue]
        public int Index
        {
            get { return _Index; }
            set { SetPropertyValue<int>("Index", ref _Index, value); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }  
        private int _Progress;
        public int Progress
        {
            get { return _Progress; }
            set { SetPropertyValue<int>("Progress", ref _Progress, value); }
        }
        private int _UqIndexID;
       [Browsable (false)]
       [RuleUniqueValue]
        public int UqIndexID
        {
            get { return _UqIndexID; }
            set { SetPropertyValue<int>("UqIndexID", ref _UqIndexID, value); }
        }
    }
}