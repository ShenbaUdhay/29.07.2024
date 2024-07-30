using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    public enum OptionSetups
    {
        Yes = 1,
        No = 0
    }
    public enum YesNoFilters
    {
        [XafDisplayName("Yes")]
        Yes,
        [XafDisplayName("No")]
        No
    }
    public enum ReportIDDigit
    {
        [XafDisplayName("2")]
        Two,
        [XafDisplayName("3")]
        Three
    }
    public enum YearFormats
    {
        [XafDisplayName("yy")]
        yy,
        [XafDisplayName("yyyy")]
        yyyy
    }
    public enum ReportIDFormatOption
    {
        [XafDisplayName("Date Format")]
        Yes,
        [XafDisplayName("JobID Format")]
        No
    }

    [DefaultClassOptions]
    public class ReportIDFormat : BaseObject
    {
        public ReportIDFormat(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            CreatedDate = DateTime.Now;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            //ReportIDDigit = ReportIDDigit.Three;

        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }
        private ReportIDFormatOption _ReportIDFormatOption;
        [Appearance("ReportIDFormatOption", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public ReportIDFormatOption ReportIDFormatOption
        {
            get { return _ReportIDFormatOption; }
            set { SetPropertyValue("ReportIDFormatOption", ref _ReportIDFormatOption, value); }
        }
        private YesNoFilters _Prefixs;
        [Appearance("Prefix2", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YesNoFilters Prefixs
        {
            get { return _Prefixs; }
            set { SetPropertyValue("Prefixs", ref _Prefixs, value); }
        }

        [Appearance("PrefixValue2", Visibility = ViewItemVisibility.Hide, Criteria = "!Prefixs = false", Context = "DetailView")]
        [Appearance("PrefixsValueNew", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ImmediatePostData]
        public string PrefixsValue
        {
            get { return GetPropertyValue<string>("PrefixsValue"); }
            set { SetPropertyValue<string>("PrefixsValue", value); }
        }

        private YesNoFilters _Year;
        [Appearance("Year2", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [Appearance("Year3", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!ReportIDFormatOption = false", AppearanceItemType = "ViewItem")]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YesNoFilters Year
        {
            get { return _Year; }
            set { SetPropertyValue("Year", ref _Year, value); }
        }

        private YearFormats _YearFormat;
        [Appearance("YearFormat2", Visibility = ViewItemVisibility.Hide, Criteria = "!Year = false", Context = "DetailView")]
        [Appearance("YearFormatNew2", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [Appearance("YearFormat3", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!ReportIDFormatOption = false", AppearanceItemType = "ViewItem")]

        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YearFormats YearFormat
        {
            get { return _YearFormat; }
            set { SetPropertyValue("YearFormat", ref _YearFormat, value); }
        }

        private YesNoFilters _Month;
        [Appearance("Month2", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [Appearance("Month3", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!ReportIDFormatOption = false", AppearanceItemType = "ViewItem")]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YesNoFilters Month
        {
            get { return _Month; }
            set { SetPropertyValue("Month", ref _Month, value); }
        }

        private string _MonthFormat;
        [Appearance("MonthFormat2", Visibility = ViewItemVisibility.Hide, Criteria = "!Month = false", Context = "DetailView")]
        [Appearance("MonthFormatNew2", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [Appearance("MonthFormat3", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!ReportIDFormatOption = false", AppearanceItemType = "ViewItem")]
        [ModelDefault("AllowEdit", "False")]
        [ImmediatePostData]
        public string MonthFormat
        {
            get { return "mm"; }
            set { SetPropertyValue("MonthFormat", ref _MonthFormat, value); }
        }

        private YesNoFilters _Day;
        [Appearance("Day2", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [Appearance("Day3", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!ReportIDFormatOption = false", AppearanceItemType = "ViewItem")]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YesNoFilters Day
        {
            get { return _Day; }
            set { SetPropertyValue("Day", ref _Day, value); }
        }

        private string _DayFormat;
        [Appearance("DayFormat2", Visibility = ViewItemVisibility.Hide, Criteria = "!Day = false", Context = "DetailView")]
        [Appearance("DayFormatNew2", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [Appearance("DayFormat3", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!ReportIDFormatOption = false", AppearanceItemType = "ViewItem")]
        [ModelDefault("AllowEdit", "False")]
        [ImmediatePostData]
        public string DayFormat
        {
            get { return "dd"; }
            set { SetPropertyValue("DayFormat", ref _DayFormat, value); }
        }

        private uint _SequentialNumber;
        [Appearance("SequentialNumber2", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ImmediatePostData]
        public uint SequentialNumber
        {
            get { return _SequentialNumber; }
            set { SetPropertyValue(nameof(SequentialNumber), ref _SequentialNumber, value); }
        }

        private uint _NumberStart;
        [Appearance("NumberStart2", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ImmediatePostData]
        public uint NumberStart
        {
            get
            {
                return _NumberStart;
            }
            set { SetPropertyValue(nameof(NumberStart), ref _NumberStart, value); }
        }

        private string _ReportIDFormatDisplay;
        [ImmediatePostData]
        [ModelDefault("AllowEdit", "False")]
        public string ReportIDFormatDisplay
        {
            get
            {
                var curdate = DateTime.Now;
                _ReportIDFormatDisplay = string.Empty;
                if (Prefixs == YesNoFilters.Yes)
                {
                    _ReportIDFormatDisplay = PrefixsValue;
                }
                if (ReportIDFormatOption == ReportIDFormatOption.No)
                {
                    using (UnitOfWork uow = new UnitOfWork())
                    {
                        XPQuery<Samplecheckin> sampleCheckinsQuery = new XPQuery<Samplecheckin>(uow);
                        string maxJobID = sampleCheckinsQuery.Max(x => x.JobID);
                        _ReportIDFormatDisplay += maxJobID;
                        // Now you have the maximum JobID stored in maxJobID
                    }
                }
                if (ReportIDFormatOption == ReportIDFormatOption.Yes)
                {
                    if (Year == YesNoFilters.Yes)
                    {
                        _ReportIDFormatDisplay += curdate.ToString(YearFormat.ToString());
                    }
                    if (Month == YesNoFilters.Yes)
                    {
                        _ReportIDFormatDisplay += curdate.ToString(MonthFormat.ToUpper());
                    }
                    if (Day == YesNoFilters.Yes)
                    {
                        _ReportIDFormatDisplay += curdate.ToString(DayFormat);
                    }
                }
                if (SequentialNumber > 0)
                {
                    if (NumberStart > 0)
                    {
                        _ReportIDFormatDisplay = _ReportIDFormatDisplay.PadRight(Convert.ToInt32(_ReportIDFormatDisplay.Length + (SequentialNumber - NumberStart.ToString().Length)), '0') + NumberStart;
                    }
                    else
                    {
                        _ReportIDFormatDisplay = _ReportIDFormatDisplay.PadRight(Convert.ToInt32(_ReportIDFormatDisplay.Length + (SequentialNumber - 1)), '0') + "1";
                    }
                }
                return _ReportIDFormatDisplay;
            }
            set
            {
                SetPropertyValue(nameof(ReportIDFormatDisplay), ref _ReportIDFormatDisplay, value);
            }
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
        //private ReportIDDigit _ReportIDDigit;
        //[VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        //public ReportIDDigit ReportIDDigit
        //{
        //    get { return _ReportIDDigit; }
        //    set { SetPropertyValue(nameof(ReportIDDigit), ref _ReportIDDigit, value); }
        //}

        //private string _ReportIDFormatDisplay;
        //[ImmediatePostData]
        //[ModelDefault("AllowEdit", "False")]
        //[VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        //public string ReportIDFormatDisplay
        //{
        //    get
        //    {
        //        if (ReportIDFormatDisplay != null)
        //        {
        //            if (ReportIDDigit == ReportIDDigit.Two)
        //            {
        //                _ReportIDFormatDisplay = ReportIDFormatDisplay + "-" + "01";
        //            }
        //            else if (ReportIDDigit == ReportIDDigit.Three)
        //            {
        //                _ReportIDFormatDisplay = ReportIDFormatDisplay + "-" + "001";
        //            }
        //        }
        //        return _ReportIDFormatDisplay;
        //    }
        //    set
        //    {
        //        SetPropertyValue(nameof(ReportIDFormatDisplay), ref _ReportIDFormatDisplay, value);
        //    }
        //}

        private bool _Dynamic;
        [ImmediatePostData]
        public bool Dynamic
        {
            get { return _Dynamic; }
            set { SetPropertyValue(nameof(Dynamic), ref _Dynamic, value); }
        }
    }
}