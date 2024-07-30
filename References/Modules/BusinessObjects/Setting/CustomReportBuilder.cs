using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CustomReportBuilder : ReportDataV2
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CustomReportBuilder(Session session)
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
            if (string.IsNullOrEmpty(TemplateID))
            {
                CriteriaOperator estexpression = CriteriaOperator.Parse("Max(TemplateID)");
                CriteriaOperator estfilter = CriteriaOperator.Parse(" ");
                var objtemp = Session.Evaluate<CustomReportBuilder>(estexpression, estfilter);
                if (objtemp != null)
                {
                    int tempid = Convert.ToInt32(objtemp) + 1;
                    TemplateID = tempid.ToString();
                }
                else
                {
                    TemplateID = "1001";
                }
            }
        }



        #region ReportDesignerName
        string _ReportDesignerName;
        [XafDisplayName("TemplateName")]
        public string ReportDesignerName
        {
            get { return _ReportDesignerName; }
            set { SetPropertyValue<string>(nameof(ReportDesignerName), ref _ReportDesignerName, value); }
        }
        #endregion

        //private string _TemplateName;
        //public string TemplateName
        //{
        //    get { return _TemplateName; }
        //    set { SetPropertyValue<string>(nameof(TemplateName), ref _TemplateName, value); }
        //}
        private string _TemplateID;

        public string TemplateID
        {
            get { return _TemplateID; }
            set { SetPropertyValue<string>(nameof(TemplateID), ref _TemplateID, value); }
        }
        private string _CustomCaption;
        public string CustomCaption
        {
            get { return _CustomCaption; }
            set { SetPropertyValue<string>(nameof(CustomCaption), ref _CustomCaption, value); }
        }
        private bool _Active;
        public bool Active
        {
            get { return _Active; }
            set { SetPropertyValue<bool>(nameof(Active), ref _Active, value); }
        }
        private ReportCategory _Category;
        public ReportCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue<ReportCategory>(nameof(Category), ref _Category, value); }
        }
        private ReportType _ReportType;
        public ReportType ReportType
        {
            get { return _ReportType; }
            set { SetPropertyValue<ReportType>(nameof(ReportType), ref _ReportType, value); }
        }
        private Employee _UserAccess;
        public Employee UserAccess
        {
            get { return _UserAccess; }
            set { SetPropertyValue<Employee>(nameof(UserAccess), ref _UserAccess, value); }
        }

        #region Notepad
        string _Notepad;
        public string Notepad
        {
            get { return _Notepad; }
            set { SetPropertyValue<string>(nameof(Notepad), ref _Notepad, value); }
        }
        #endregion

        #region ViewDataSource
        string _ViewDataSource;
        public string ViewDataSource
        {
            get { return _ViewDataSource; }
            set { SetPropertyValue<string>(nameof(ViewDataSource), ref _ViewDataSource, value); }
        }
        #endregion

        #region AllView
        string _AllView;
        public string AllView
        {
            get { return _AllView; }
            set { SetPropertyValue<string>(nameof(AllView), ref _AllView, value); }
        }
        #endregion

        //#region BusinessObjectName
        //Modules.BusinessObjects.Setting.NavigationItem _BusinessObjectName;
        //public Modules.BusinessObjects.Setting.NavigationItem BusinessObjectName
        //{
        //    get { return _BusinessObjectName; }
        //    set { SetPropertyValue<Modules.BusinessObjects.Setting.NavigationItem>(nameof(BusinessObjectName), ref _BusinessObjectName, value); }
        //}
        //#endregion
        private string _Module;
        [XafDisplayName("Navigation")]
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

        //#region BusinessObjectName
        //Modules.BusinessObjects.Setting.NavigationItem _NavigationItem;
        //public Modules.BusinessObjects.Setting.NavigationItem NavigationItem
        //{
        //    get { return _NavigationItem; }
        //    set { SetPropertyValue<Modules.BusinessObjects.Setting.NavigationItem>(nameof(NavigationItem), ref _NavigationItem, value); }
        //}
        //#endregion

        #region ShowReportID
        bool _ShowReportID;
        public bool ShowReportID
        {
            get { return _ShowReportID; }
            set { SetPropertyValue<bool>(nameof(ShowReportID), ref _ShowReportID, value); }
        }
        #endregion


        byte[] _ReportLayout;
        [Size(SizeAttribute.Unlimited)]
        public byte[] ReportLayout
        {
            get { return _ReportLayout; }
            set { SetPropertyValue<byte[]>(nameof(ReportLayout), ref _ReportLayout, value); }
        }

        string _ReportXml;
        [Size(SizeAttribute.Unlimited)]
        public string ReportXml
        {
            get { return _ReportXml; }
            set { SetPropertyValue<string>(nameof(ReportXml), ref _ReportXml, value); }
        }
        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>(nameof(Comment), ref _Comment, value); }
        }
    }
}