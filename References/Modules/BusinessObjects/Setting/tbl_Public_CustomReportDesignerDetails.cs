using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{

    public partial class tbl_Public_CustomReportDesignerDetails : XPBaseObject, ICheckedListBoxItemsProvider
    {

        public tbl_Public_CustomReportDesignerDetails(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }



        int fcoluqCustomReportDesignerID;
        [Key(true)]
        [System.ComponentModel.DisplayName("TemplateID")]
        public int coluqCustomReportDesignerID
        {
            get { return fcoluqCustomReportDesignerID; }
            set { SetPropertyValue<int>(nameof(coluqCustomReportDesignerID), ref fcoluqCustomReportDesignerID, value); }
        }

        string fcolCustomReportDesignerName;
        [System.ComponentModel.DisplayName("TemplateName")]
        public string colCustomReportDesignerName
        {
            get { return fcolCustomReportDesignerName; }
            set { SetPropertyValue<string>(nameof(colCustomReportDesignerName), ref fcolCustomReportDesignerName, value); }
        }

        string fcolCustomReportDesignerLayOut;
        [Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        public string colCustomReportDesignerLayOut
        {
            get { return fcolCustomReportDesignerLayOut; }
            set { SetPropertyValue<string>(nameof(colCustomReportDesignerLayOut), ref fcolCustomReportDesignerLayOut, value); }
        }

        string fcolTableSchema;
        [Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        public string colTableSchema
        {
            get { return fcolTableSchema; }
            set { SetPropertyValue<string>(nameof(colTableSchema), ref fcolTableSchema, value); }
        }

        string fcolNormalColumn;
        [Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        public string colNormalColumn
        {
            get { return fcolNormalColumn; }
            set { SetPropertyValue<string>(nameof(colNormalColumn), ref fcolNormalColumn, value); }
        }

        string fcolRowColumn;
        [Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        public string colRowColumn
        {
            get { return fcolRowColumn; }
            set { SetPropertyValue<string>(nameof(colRowColumn), ref fcolRowColumn, value); }
        }

        string fcolUniqueColumn;
        [Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        public string colUniqueColumn
        {
            get { return fcolUniqueColumn; }
            set { SetPropertyValue<string>(nameof(colUniqueColumn), ref fcolUniqueColumn, value); }
        }

        string fcolCustomReportDesignerUserDefinedName;
        [Browsable(false)]
        public string colCustomReportDesignerUserDefinedName
        {
            get { return fcolCustomReportDesignerUserDefinedName; }
            set { SetPropertyValue<string>(nameof(colCustomReportDesignerUserDefinedName), ref fcolCustomReportDesignerUserDefinedName, value); }
        }

        short fcoluqCompReportCategoryID;
        [Browsable(false)]
        public short coluqCompReportCategoryID
        {
            get { return fcoluqCompReportCategoryID; }
            set { SetPropertyValue<short>(nameof(coluqCompReportCategoryID), ref fcoluqCompReportCategoryID, value); }
        }

        short fcoluqCompReportReportTypeID;
        [Browsable(false)]
        public short coluqCompReportReportTypeID
        {
            get { return fcoluqCompReportReportTypeID; }
            set { SetPropertyValue<short>(nameof(coluqCompReportReportTypeID), ref fcoluqCompReportReportTypeID, value); }
        }

        string fcolDataSourceQuery;
        [Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        public string colDataSourceQuery
        {
            get { return fcolDataSourceQuery; }
            set { SetPropertyValue<string>(nameof(colDataSourceQuery), ref fcolDataSourceQuery, value); }
        }

        int fcolVerticalColumnCount;
        [Browsable(false)]
        public int colVerticalColumnCount
        {
            get { return fcolVerticalColumnCount; }
            set { SetPropertyValue<int>(nameof(colVerticalColumnCount), ref fcolVerticalColumnCount, value); }
        }

        bool fNeedSignOff;
        [Browsable(false)]
        public bool NeedSignOff
        {
            get { return fNeedSignOff; }
            set { SetPropertyValue<bool>(nameof(NeedSignOff), ref fNeedSignOff, value); }
        }

        bool fFinalReport;
        //[Browsable(false)]

        [ColumnDbDefaultValue("((0))")]
        public bool FinalReport
        {
            get { return fFinalReport; }
            set { SetPropertyValue<bool>(nameof(FinalReport), ref fFinalReport, value); }
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

        private bool _AllowMultipleJOBID;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public bool AllowMultipleJOBID
        {
            get { return _AllowMultipleJOBID; }
            set { SetPropertyValue<bool>(nameof(AllowMultipleJOBID), ref _AllowMultipleJOBID, value); }
        }

        private string _UserAccess;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string UserAccess
        {
            get { return _UserAccess; }
            set { SetPropertyValue<string>(nameof(UserAccess), ref _UserAccess, value); }
        }
        [Browsable(false)]
        public XPCollection<Employee> Users
        {
            get
            {
                return new XPCollection<Employee>(Session, CriteriaOperator.Parse(""));
            }
        }
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "UserAccess" && Users != null && Users.Count > 0)
            {
                foreach (Employee objEmp in Users)
                {
                    if (!Properties.ContainsKey(objEmp.Oid))
                    {
                        Properties.Add(objEmp.Oid, objEmp.FullName);
                    }
                }

            }
            return Properties;
        }
        private DefaultSetting _Module;
        [XafDisplayName("Navigation")]
        //[EditorAlias("ModuleNamePropertyEditor")]
        [ImmediatePostData]
        [DataSourceProperty("ModuleDataSource")]
        public DefaultSetting Module
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
        [NonPersistent]
        [Browsable(false)]
        public XPCollection<DefaultSetting> ModuleDataSource
        {
            get
            {
                List<string> strModule = new List<string>();
                XPCollection<DefaultSetting> lstTests = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse(""));
                foreach (DefaultSetting objDS in lstTests)
                {
                    if (objDS != null && objDS.ModuleName != null && objDS.ModuleName != string.Empty)
                    {
                        if (!strModule.Contains(objDS.ModuleName))
                        {
                            strModule.Add(objDS.ModuleName);
                        }
                    }
                }
                return new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[ModuleName] In(" + string.Format("'{0}'", string.Join("','", strModule)) + ") AND [IsModule] = True AND [Select] = True"));
                //return new XPCollection<DefaultSetting>(Session, new InOperator("[ModuleName] = ?", strModule));
            }
        }



        private DefaultSetting _BusinessObject;
        [DataSourceProperty("BusinessObjectDataSource")]
        [RuleRequiredField]
        //[EditorAlias("BusinessObjectPropertyEditor")]
        //[ImmediatePostData]
        public DefaultSetting BusinessObject
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
        [NonPersistent]
        [Browsable(false)]
        public XPCollection<DefaultSetting> BusinessObjectDataSource
        {
            get
            {

                if (Module != null)
                {
                    List<string> strModule = new List<string>();
                    XPCollection<DefaultSetting> lstTests = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[IsModule] = False AND [Select] = True AND [ModuleName]=?", Module.ModuleName));
                    foreach (DefaultSetting objDS in lstTests)
                    {
                        if (objDS != null && objDS.ModuleName != null && objDS.ModuleName != string.Empty)
                        {
                            if (!strModule.Contains(objDS.ModuleName))
                            {
                                strModule.Add(objDS.ModuleName);
                            }
                        }
                    }
                    return new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[ModuleName] In(" + string.Format("'{0}'", string.Join("','", strModule)) + ") AND [IsModule] = False AND [Select] = True"));
                }
                else
                {
                    return null;
                }
                //return new XPCollection<DefaultSetting>(Session, new InOperator("[ModuleName] = ?", strModule));
            }
        }
        #region ShowReportID
        bool _ShowReportID;
        public bool ShowReportID
        {
            get { return _ShowReportID; }
            set { SetPropertyValue<bool>(nameof(ShowReportID), ref _ShowReportID, value); }
        }
        #endregion

        #region COCSettings
        bool _COCSettings;
        public bool COCSettings
        {
            get { return _COCSettings; }
            set { SetPropertyValue<bool>(nameof(COCSettings), ref _COCSettings, value); }
        }
        #endregion

        private string _Comment;

        public event EventHandler ItemsChanged;

        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>(nameof(Comment), ref _Comment, value); }
        }


    }

}
