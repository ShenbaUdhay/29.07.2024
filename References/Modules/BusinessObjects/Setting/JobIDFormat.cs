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
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting

{
    public enum OptionSetup
    {
        Yes = 1,
        No = 0
    }
    public enum YesNoFilter
    {
        [XafDisplayName("Yes")]
        Yes,
        [XafDisplayName("No")]
        No
    }
    public enum SampleIDDigit
    {
        [XafDisplayName("2")]
        Two,
        [XafDisplayName("3")]
        Three
    }
    public enum YearFormat
    {
        [XafDisplayName("yy")]
        yy,
        [XafDisplayName("yyyy")]
        yyyy
    }

    [DefaultClassOptions]
    public class JobIDFormat : BaseObject
    {
        public JobIDFormat(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = DateTime.Now;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            SampleIDDigit = SampleIDDigit.Three;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        private YesNoFilter _Prefix;
        [Appearance("Prefix", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YesNoFilter Prefix
        {
            get { return _Prefix; }
            set { SetPropertyValue("Prefix", ref _Prefix, value); }
        }

        [Appearance("PrefixValue", Visibility = ViewItemVisibility.Hide, Criteria = "!Prefix = false", Context = "DetailView")]
        [Appearance("PrefixValueNew", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ImmediatePostData]
        public string PrefixValue
        {
            get { return GetPropertyValue<string>("PrefixValue"); }
            set { SetPropertyValue<string>("PrefixValue", value); }
        }

        private YesNoFilter _Year;
        [Appearance("Year", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YesNoFilter Year
        {
            get { return _Year; }
            set { SetPropertyValue("Year", ref _Year, value); }
        }

        private YearFormat _YearFormat;
        [Appearance("YearFormat", Visibility = ViewItemVisibility.Hide, Criteria = "!Year = false", Context = "DetailView")]
        [Appearance("YearFormatNew", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YearFormat YearFormat
        {
            get { return _YearFormat; }
            set { SetPropertyValue("YearFormat", ref _YearFormat, value); }
        }

        private YesNoFilter _Month;
        [Appearance("Month", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YesNoFilter Month
        {
            get { return _Month; }
            set { SetPropertyValue("Month", ref _Month, value); }
        }

        private string _MonthFormat;
        [Appearance("MonthFormat", Visibility = ViewItemVisibility.Hide, Criteria = "!Month = false", Context = "DetailView")]
        [Appearance("MonthFormatNew", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ModelDefault("AllowEdit", "False")]
        [ImmediatePostData]
        public string MonthFormat
        {
            get { return "mm"; }
            set { SetPropertyValue("MonthFormat", ref _MonthFormat, value); }
        }

        private YesNoFilter _Day;
        [Appearance("Day", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public YesNoFilter Day
        {
            get { return _Day; }
            set { SetPropertyValue("Day", ref _Day, value); }
        }

        private string _DayFormat;
        [Appearance("DayFormat", Visibility = ViewItemVisibility.Hide, Criteria = "!Day = false", Context = "DetailView")]
        [Appearance("DayFormatNew", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ModelDefault("AllowEdit", "False")]
        [ImmediatePostData]
        public string DayFormat
        {
            get { return "dd"; }
            set { SetPropertyValue("DayFormat", ref _DayFormat, value); }
        }

        private uint _SequentialNumber;
        [Appearance("SequentialNumber", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ImmediatePostData]
        public uint SequentialNumber
        {
            get { return _SequentialNumber; }
            set { SetPropertyValue(nameof(SequentialNumber), ref _SequentialNumber, value); }
        }

        private uint _NumberStart;
        [Appearance("NumberStart", Context = "DetailView", Criteria = "Dynamic = false", AppearanceItemType = "ViewItem", Enabled = false)]
        [ImmediatePostData]
        public uint NumberStart
        {
            get
            {
                return _NumberStart;
            }
            set { SetPropertyValue(nameof(NumberStart), ref _NumberStart, value); }
        }

        private string _JobIDFormatDisplay;
        [ImmediatePostData]
        [ModelDefault("AllowEdit", "False")]
        public string JobIDFormatDisplay
        {
            get
            {
                var curdate = DateTime.Now;
                _JobIDFormatDisplay = string.Empty;
                if (Prefix == YesNoFilter.Yes)
                {
                    _JobIDFormatDisplay = PrefixValue;
                }
                if (Year == YesNoFilter.Yes)
                {
                    _JobIDFormatDisplay += curdate.ToString(YearFormat.ToString());
                }
                if (Month == YesNoFilter.Yes)
                {
                    _JobIDFormatDisplay += curdate.ToString(MonthFormat.ToUpper());
                }
                if (Day == YesNoFilter.Yes)
                {
                    _JobIDFormatDisplay += curdate.ToString(DayFormat);
                }
                if (SequentialNumber > 0)
                {
                    if (NumberStart > 0)
                    {
                        _JobIDFormatDisplay = _JobIDFormatDisplay.PadRight(Convert.ToInt32(_JobIDFormatDisplay.Length + (SequentialNumber - NumberStart.ToString().Length)), '0') + NumberStart;
                    }
                    else
                    {
                        _JobIDFormatDisplay = _JobIDFormatDisplay.PadRight(Convert.ToInt32(_JobIDFormatDisplay.Length + (SequentialNumber - 1)), '0') + "1";
                    }
                }
                return _JobIDFormatDisplay;
            }
            set
            {
                SetPropertyValue(nameof(JobIDFormatDisplay), ref _JobIDFormatDisplay, value);
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
        private SampleIDDigit _SampleIDDigit;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public SampleIDDigit SampleIDDigit
        {
            get { return _SampleIDDigit; }
            set { SetPropertyValue(nameof(SampleIDDigit), ref _SampleIDDigit, value); }
        }

        private string _SampleIDFormatDisplay;
        [ImmediatePostData]
        [ModelDefault("AllowEdit", "False")]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public string SampleIDFormatDisplay
        {
            get
            {
                if (JobIDFormatDisplay != null)
                {
                    if (SampleIDDigit == SampleIDDigit.Two)
                    {
                        _SampleIDFormatDisplay = JobIDFormatDisplay + "-" + "01";
                    }
                    else if (SampleIDDigit == SampleIDDigit.Three)
                    {
                        _SampleIDFormatDisplay = JobIDFormatDisplay + "-" + "001";
                    }
                }
                return _SampleIDFormatDisplay;
            }
            set
            {
                SetPropertyValue(nameof(SampleIDFormatDisplay), ref _SampleIDFormatDisplay, value);
            }
        }

        private bool _Dynamic;
        [ImmediatePostData]
        public bool Dynamic
        {
            get { return _Dynamic; }
            set { SetPropertyValue(nameof(Dynamic), ref _Dynamic, value); }
        }
    }
}