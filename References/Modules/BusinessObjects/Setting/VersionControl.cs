using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class VersionControl : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public VersionControl(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ReleasedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            DateReleased = Library.GetServerTime(Session);
            // Version Number
            //string path = HttpContext.Current.Server.MapPath(@"~\Properties\AssemblyInfo.cs");
            //if (File.Exists(path))
            //{
            //    string[] readText = File.ReadAllLines(path);
            //    string[] versionInfoLines = readText.Where(t => t.Contains("[assembly: AssemblyVersion")).Cast<string>().ToArray();
            //    if (versionInfoLines != null && versionInfoLines.Length > 0)
            //    {
            //        string strVersionNumber = versionInfoLines[0].Substring(versionInfoLines[0].IndexOf('(') + 2, versionInfoLines[0].LastIndexOf(')') - versionInfoLines[0].IndexOf('(') - 3);
            //        VersionNumber = strVersionNumber;
            //    }
            //}

            VersionControlInfo objVersion = new VersionControlInfo();
            VersionNumber = objVersion.VersionNumber;

            //XAFVersion Number
            System.Reflection.Assembly assem;
            System.Reflection.AssemblyName assemname;
            System.Version assemVersion;
            assem = System.Reflection.Assembly.GetExecutingAssembly();
            assemname = assem.GetName();
            assemVersion = assemname.Version;
            var strXafVersionNumber = assemname.Version.ToString();
            XAFVersionNumber = strXafVersionNumber;

            //Size
            //string strProjectFolderPath =string.Empty;
            //strProjectFolderPath = path.Replace("\\BTLIMS.Web\\Properties\\AssemblyInfo.cs", "");
            //strProjectFolderPath = path.Replace("\\Web.config", "");
            //DirectoryInfo di = new DirectoryInfo(strProjectFolderPath);
            //long allProjectSize = di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
            //Size = AutoFileSize(allProjectSize);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        #region VersionNumber
        private string _VersionNumber;
        [RuleUniqueValue]
        [RuleRequiredField("VersionNumber", DefaultContexts.Save, "Version Number must not to be empty")]
        public string VersionNumber
        {
            get
            {
                return _VersionNumber;
            }
            set
            {
                SetPropertyValue("VersionNumber", ref _VersionNumber, value);
            }
        }
        #endregion

        #region XAFVersionNumber
        private string _XAFVersionNumber;
        public string XAFVersionNumber
        {
            get
            {
                return _XAFVersionNumber;
            }
            set
            {
                SetPropertyValue("XAFVersionNumber", ref _XAFVersionNumber, value);
            }
        }
        #endregion

        #region Size
        private string _Size;
        public string Size
        {
            get
            {
                return _Size;
            }
            set
            {
                SetPropertyValue("Size", ref _Size, value);
            }
        }
        #endregion

        #region Description 
        private string _Description;
        //[Size(1000)]
        [Size(SizeAttribute.Unlimited)]
        [RuleRequiredField("VersionDescription", DefaultContexts.Save, "Description must not to be empty")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetPropertyValue("Description ", ref _Description, value);
            }
        }
        #endregion

        #region Date Released 
        private DateTime _DateReleased;
        public DateTime DateReleased
        {
            get
            {
                return _DateReleased;
            }
            set
            {
                SetPropertyValue("DateReleased ", ref _DateReleased, value);
            }
        }
        #endregion

        //#region Tested By
        //private Employee _TestedBy;
        //public Employee TestedBy
        //{
        //    get
        //    {
        //        return _TestedBy;
        //    }
        //    set
        //    {
        //        SetPropertyValue("TestedBy ", ref _TestedBy, value);
        //    }
        //}
        //#endregion

        #region VersionTestedBy
        private string _VersionTestedBy;
        public string VersionTestedBy
        {
            get
            {
                return _VersionTestedBy;
            }
            set
            {
                SetPropertyValue<string>(nameof(VersionTestedBy), ref _VersionTestedBy, value);
            }
        }
        #endregion

        #region Date Tested 
        private DateTime _DateTested;
        public DateTime DateTested
        {
            get
            {
                return _DateTested;
            }
            set
            {
                SetPropertyValue("DateTested ", ref _DateTested, value);
            }
        }
        #endregion

        #region Released By
        private CustomSystemUser _ReleasedBy;
        public CustomSystemUser ReleasedBy
        {
            get
            {
                return _ReleasedBy;
            }
            set
            {
                SetPropertyValue("ReleasedBy ", ref _ReleasedBy, value);
            }
        }
        #endregion

        #region VersionReleasedBy
        private string _VersionReleasedBy;
        public string VersionReleasedBy
        {
            get
            {
                if (string.IsNullOrEmpty(_VersionReleasedBy) && ReleasedBy != null)
                {
                    _VersionReleasedBy = ReleasedBy.UserName;
                }
                return _VersionReleasedBy;
            }
            set
            {
                SetPropertyValue<string>(nameof(VersionReleasedBy), ref _VersionReleasedBy, value);
            }
        }
        #endregion


        #region AutoFileSize
        public string AutoFileSize(long number)
        {
            double tmp = number;
            string suffix = " B ";
            if (tmp > 1024) { tmp = tmp / 1024; suffix = " KB"; }
            if (tmp > 1024) { tmp = tmp / 1024; suffix = " MB"; }
            if (tmp > 1024) { tmp = tmp / 1024; suffix = " GB"; }
            if (tmp > 1024) { tmp = tmp / 1024; suffix = " TB"; }
            return tmp.ToString("n") + suffix;
        }
        #endregion

        [Association("VersionLog-Posts")]
        public XPCollection<VersionPosts> Posts
        {
            get
            {
                return GetCollection<VersionPosts>(nameof(Posts));
            }
        }

        #region TicketID
        private string _TicketID;
        [RuleUniqueValue]
        public string TicketID
        {
            get
            {
                return _TicketID;
            }
            set
            {
                SetPropertyValue("TicketID", ref _TicketID, value);
            }
        }
        #endregion

        #region VersionReleaseYear
        [VisibleInListView(false),VisibleInDetailView(false),VisibleInLookupListView(false)]
        public int VersionReleaseYear
        {
            get
            {
                if (!string.IsNullOrEmpty(VersionNumber) && VersionNumber.Contains(".") && VersionNumber.Split('.').Length > 0)
                {
                return Convert.ToInt32(VersionNumber.Split('.')[0]);
            }
                else if(DateReleased != DateTime.MinValue)
                {
                    return Convert.ToInt32(DateReleased.ToString("yy"));
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion

        #region VersionReleaseMonth
        [VisibleInListView(false),VisibleInDetailView(false),VisibleInLookupListView(false)]
        public int VersionReleaseMonth
        {
            get
            {
                if (!string.IsNullOrEmpty(VersionNumber) && VersionNumber.Contains(".") && VersionNumber.Split('.').Length > 1)
                {
                return Convert.ToInt32(VersionNumber.Split('.')[1]);
            }
                else if (DateReleased != DateTime.MinValue)
                {
                    return Convert.ToInt32(DateReleased.ToString("MM"));
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion

        #region VersionReleaseNo
        [VisibleInListView(false),VisibleInDetailView(false),VisibleInLookupListView(false)]
        public int VersionReleaseNo
        {
            get
            {
                //return Convert.ToInt32(VersionNumber.Split('.')[2]);
                if (!string.IsNullOrEmpty(VersionNumber) && VersionNumber.Contains(".") && VersionNumber.Split('.').Length > 2)
                {
                    return Convert.ToInt32(VersionNumber.Split('.')[1]);
                }
                else if (DateReleased != DateTime.MinValue)
                {
                    return Convert.ToInt32(DateReleased.ToString("dd"));
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
    }
}