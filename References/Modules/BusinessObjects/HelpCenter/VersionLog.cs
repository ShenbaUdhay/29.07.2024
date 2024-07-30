using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty("VersionID")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class VersionLog : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public VersionLog(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Author = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _VersionID;
        [RuleRequiredField("VersionID", DefaultContexts.Save, "'Version ID'must not be empty")]

        public string VersionID
        {
            get
            {
                return _VersionID;
            }
            set
            {
                SetPropertyValue(nameof(VersionID), ref _VersionID, value);
            }
        }
        private HelpCenter _HelpCenter;
        [Association, VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public HelpCenter HelpCenter
        {
            get
            {
                return _HelpCenter;
            }
            set
            {
                SetPropertyValue(nameof(HelpCenter), ref _HelpCenter, value);
            }
        }
        private DateTime _Date;
        //[ModelDefault("EditMask","dd/MM/yyyy")]
        public DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                SetPropertyValue(nameof(Date), ref _Date, value);
            }
        }
        private Employee _Author;
        public Employee Author
        {
            get
            {
                return _Author;
            }
            set
            {
                SetPropertyValue(nameof(Author), ref _Author, value);
            }
        }
        private bool _InUse;
        public bool InUse
        {
            get
            {
                return _InUse;
            }
            set
            {
                SetPropertyValue(nameof(InUse), ref _InUse, value);
            }
        }
        private DateTime _DateRetire;
        public DateTime DateRetire
        {
            get
            {
                return _DateRetire;
            }
            set
            {
                SetPropertyValue(nameof(DateRetire), ref _DateRetire, value);
            }
        }
        private string _Comment;
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                SetPropertyValue(nameof(Comment), ref _Comment, value);
            }
        }

    }
}