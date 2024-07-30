using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.Drawing;

namespace Modules.BusinessObjects.Setting
{
    //public enum EnumCategory
    //{
    //    Manual,
    //    FAQ
    //}
    public enum EnumStyle
    {
        Normal,
        NoSpacing,
        Heading1,
        Heading2,
        Title,
        Subtitle,
        SubtleEmphasis,
        Emphasis,
        IntenseEmphasis,
        Strong,
        Quote,
        IntenseQuote,
        SubtleReference,
        IntenseReference,
        BookTitle,
        ListParagraph
    }
    [DefaultClassOptions]


    public class HelpCenter : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public HelpCenter(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            Author = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            DateReleased = DateTime.Now;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }
        private string _Topic;
        //[Appearance("HelpCenterInListView", AppearanceItemType = "ViewItem", TargetItems = "Topic", Criteria = "Topic = 'Article'", Context = "ListView", FontColor = "Blue", Priority = 1)]
        //[Appearance("HelpCenterInListView", AppearanceItemType = "ViewItem", TargetItems = "Topic", Context = "HelpCenter_ListView_Articles", Criteria = "[Category.Category] = 'Manual'", FontStyle = FontStyle.Underline,FontColor = "Blue", Priority = 1)]
        [Appearance("HelpCenterInListView", AppearanceItemType = "ViewItem", TargetItems = "Topic", Context = "HelpCenter_ListView_FAQ_Articles", FontStyle = FontStyle.Underline, FontColor = "Blue", Priority = 1)]
        [Appearance("HelpCenterInListManual", AppearanceItemType = "ViewItem", TargetItems = "Topic", Context = "HelpCenter_ListView_Articles_Manual", FontStyle = FontStyle.Underline, FontColor = "Blue", Priority = 1)]
        [Appearance("HelpCenterInListFAQ", AppearanceItemType = "ViewItem", TargetItems = "Topic", Context = "HelpCenter_ListView_Articles", Criteria = "[Category.Category] = 'FAQ'", FontStyle = FontStyle.Regular, FontColor = "Blue", Priority = 1)]

        [RuleRequiredField]
        public string Topic
        {
            get
            {
                return _Topic;
            }
            set
            {
                SetPropertyValue(nameof(Topic), ref _Topic, value);
            }
        }

        private string _Article;
        [Size(SizeAttribute.Unlimited)]

        public string Article
        {
            get
            {
                return _Article;
            }
            set
            {
                SetPropertyValue(nameof(Article), ref _Article, value);
            }
        }

        public string NPArticle
        {
            get
            {
                System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
                if (rtBox != null)
                {
                    rtBox.Rtf = Article;
                    return rtBox.Text;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private FileData _Upload;
        [ImmediatePostData]
        public FileData Upload
        {
            get
            {
                return _Upload;
            }
            set
            {
                SetPropertyValue(nameof(Upload), ref _Upload, value);
            }
        }
        private string _Size;
        public string Size
        {
            get
            {
                if (Upload != null)
                {
                    var totalSizeKB = Upload.Size / Math.Pow(1024, 1);
                    var totalSizeMB = Upload.Size / Math.Pow(1024, 2);
                    var totalSizeGB = Upload.Size / Math.Pow(1024, 3);
                    if (totalSizeMB < 1024)
                    {
                        _Size = Math.Round(totalSizeKB) + " KB";
                    }
                    else if (totalSizeGB < 1024)
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
            set
            {
                SetPropertyValue(nameof(Size), ref _Size, value);
            }
        }

        private string _Module;
        [EditorAlias("ModuleNamePropertyEditor")]
        [ImmediatePostData]
        public string Module
        {
            get
            {
                return _Module;
            }
            set
            {
                SetPropertyValue(nameof(Module), ref _Module, value);
            }
        }
        private string _BusinessObject;

        [EditorAlias("BusinessObjectPropertyEditor")]
        //[ImmediatePostData]
        public string BusinessObject
        {
            get
            {
                return _BusinessObject;
            }
            set
            {
                SetPropertyValue(nameof(BusinessObject), ref _BusinessObject, value);
            }
        }

        private DateTime _DateReleased;
        public DateTime DateReleased
        {
            get
            {
                return _DateReleased;
            }
            set
            {
                SetPropertyValue(nameof(DateReleased), ref _DateReleased, value);
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
        private bool _Active;

        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                SetPropertyValue(nameof(Active), ref _Active, value);
            }
        }

        #region ModifiedBy
        private Employee _modifiedby;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ModifiedBy
        {
            get
            {
                return _modifiedby;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref _modifiedby, value);
            }
        }
        #endregion


        #region ModifiedDate
        private DateTime _modifieddate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get
            {
                return _modifieddate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref _modifieddate, value);
            }
        }
        #endregion
        #region ReleasedBy
        private Employee _ReleasedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ReleasedBy
        {
            get
            {
                return _ReleasedBy;
            }
            set
            {
                SetPropertyValue("ReleasedBy", ref _ReleasedBy, value);
            }
        }
        #endregion
        #region Category
        private UserManualEditorCategory _Category;
        public UserManualEditorCategory Category
        {
            get
            {
                return _Category;
            }
            set
            {
                SetPropertyValue("Category", ref _Category, value);
            }
        }
        #endregion
        #region Style
        private EnumStyle _Style;
        public EnumStyle Style
        {
            get
            {
                return _Style;
            }
            set
            {
                SetPropertyValue("Style", ref _Style, value);
            }
        }
        #endregion
        [Association]
        public XPCollection<VersionLog> VersionLogs
        {
            get
            {
                return GetCollection<VersionLog>(nameof(VersionLogs));
            }
        }

        [Association]
        public XPCollection<HelpCenterAttachments> HelpCenterAttachments
        {
            get
            {
                return GetCollection<HelpCenterAttachments>(nameof(HelpCenterAttachments));
            }
        }



    }

}