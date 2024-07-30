using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]

    [DefaultProperty("File")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class HelpCenterAttachments : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public HelpCenterAttachments(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = DateTime.Now;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            Date = DateTime.Now;
            Author = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);

        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        private HelpCenter _Topic;
        [Browsable(false), Association]
        public HelpCenter Topic
        {
            get { return _Topic; }
            set { SetPropertyValue(nameof(Topic), ref _Topic, value); }
        }

        private string _Title;
        [RuleRequiredField]
        public string Title
        {
            get { return _Title; }
            set { SetPropertyValue(nameof(Title), ref _Title, value); }
        }

        private string _Size;
        public string Size
        {
            get
            {
                if (File != null)
                {
                    var totalSizeKB = File.Size / Math.Pow(1024, 1);
                    var totalSizeMB = File.Size / Math.Pow(1024, 2);
                    var totalSizeGB = File.Size / Math.Pow(1024, 3);
                    if (totalSizeKB < 1024)
                    {
                        _Size = Math.Round(totalSizeKB) + " KB";
                    }
                    else if (totalSizeMB < 1024)
                    {
                        _Size = Math.Round(totalSizeMB) + " MB";
                    }
                    else
                    {
                        _Size = Math.Round(totalSizeGB) + " GB";
                    }

                }
                return _Size;
            }
            set { SetPropertyValue(nameof(Size), ref _Size, value); }
        }

        private FileData _File;
        [ImmediatePostData]
        [RuleRequiredField]
        public FileData File
        {
            get { return _File; }
            set { SetPropertyValue(nameof(File), ref _File, value); }
        }

        private int _Sort;
        public int Sort
        {
            get { return _Sort; }
            set { SetPropertyValue(nameof(Sort), ref _Sort, value); }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue(nameof(Date), ref _Date, value); }
        }

        private Employee _Author;
        public Employee Author
        {
            get { return _Author; }
            set { SetPropertyValue(nameof(Author), ref _Author, value); }
        }

        private CustomSystemUser _CreatedBy;
        [Browsable(false)]
        public CustomSystemUser CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        private CustomSystemUser _ModifiedBy;
        [Browsable(false)]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }

        private DateTime _CreatedDate;

        [Browsable(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }

        private DateTime _ModifiedDate;

        [Browsable(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }

    }
}