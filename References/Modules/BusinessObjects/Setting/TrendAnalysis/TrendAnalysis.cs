using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.TrendAnalysis
{
    [NonPersistent]
    public class TrendAnalysis : BaseObject, ICheckedListBoxItemsProvider
    {
        public TrendAnalysis(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region From
        private DateTime _From;
        public DateTime From
        {
            get { return _From; }
            set { SetPropertyValue(nameof(From), ref _From, value); }
        }
        #endregion

        #region To
        private DateTime _To;
        public DateTime To
        {
            get 
            { 
                if(_To!=DateTime.MinValue)
                {
                    TimeSpan newTime = new TimeSpan(23, 59, 0);
                    _To = _To + newTime;
                }
                return _To; 
            }
            set { SetPropertyValue(nameof(To), ref _To, value); }
        }
        #endregion

        #region Client
        private Customer _Client;
        [ImmediatePostData(true)]
        public Customer Client
        {
            get { return _Client; }
            set { SetPropertyValue(nameof(Client), ref _Client, value); }
        }
        #endregion

        #region Project
        private Project _Project;
        [ImmediatePostData(true)]
        [DataSourceProperty(nameof(ProjectIDs))]
        public Project Project
        {
            get { return _Project; }
            set { SetPropertyValue(nameof(Project), ref _Project, value); }
        }
        [Browsable(false)]
        public XPCollection<Project> ProjectIDs
        {
            get
            {
                if (Client != null)
                {
                    return new XPCollection<Project>(Session, CriteriaOperator.Parse("[customername.Oid] = ? ", Client.Oid));
                }
                else
                {
                    return new XPCollection<Project>(Session);
                }
            }
        }
        #endregion

        #region Parameter
        private string _Parameter;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Parameter
        {
            get { return _Parameter; }
            set { SetPropertyValue(nameof(Parameter), ref _Parameter, value); }
        }

        [Browsable(false)]
        public XPCollection<Parameter> Parameters
        {
            get
            {
                if (Client != null)
                {
                    IList<Parameter> parameters = new List<Parameter>();
                    List<SampleLogIn> SampleLogIns = new List<SampleLogIn>();
                    if (Project != null)
                    {
                        SampleLogIns = new XPCollection<SampleLogIn>(Session, CriteriaOperator.Parse("[JobID.ClientName.Oid]=? and [JobID.ProjectID.Oid]=?", Client.Oid, Project.Oid)).ToList();
                    }
                    else
                    {
                        SampleLogIns = new XPCollection<SampleLogIn>(Session, CriteriaOperator.Parse("[JobID.ClientName.Oid]=?", Client.Oid)).ToList();
                    }
                    parameters = SampleLogIns.SelectMany(sample => sample.Testparameters ?? Enumerable.Empty<Testparameter>()).Where(testparameter => testparameter.Parameter != null).Select(testparameter => testparameter.Parameter).ToList();
                    //foreach (SampleLogIn sample in SampleLogIns)
                    //{
                    //    if (sample.Testparameters != null)
                    //    {
                    //        foreach (Testparameter testparameter in sample.Testparameters)
                    //        {
                    //            if (testparameter.Parameter != null)
                    //            {
                    //                parameters.Add(testparameter.Parameter);
                    //            }
                    //        }
                    //    }
                    //}
                    if (parameters.Count > 0)
                    {
                        return new XPCollection<Parameter>(Session, CriteriaOperator.Parse("[Oid] in (" + string.Format("'{0}'", string.Join("','", parameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")"));
                    }
                    else
                    {
                        return new XPCollection<Parameter>(Session, CriteriaOperator.Parse("1=2"));
                    }
                }
                else
                {
                    return new XPCollection<Parameter>(Session);
                }
            }
        }

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }

        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> objdic = new Dictionary<object, string>();
            if (targetMemberName == "Parameter" && Parameters != null && Parameters.Count > 0)
            {
                objdic = Parameters.Where(i => i.ParameterName != null).OrderBy(i => i.ParameterName).GroupBy(i => i.Oid).ToDictionary(group => (object)group.Key, group => group.First().ParameterName);
                //foreach (Parameter objCs in Parameters.Where(i => i.ParameterName != null).OrderBy(i => i.ParameterName).ToList())
                //{
                //    if (!objdic.ContainsKey(objCs.Oid))
                //    {
                //        objdic.Add(objCs.Oid, objCs.ParameterName);
                //    }
                //}
            }
            return objdic;
        }
        #endregion

        #region DVParameter
        private string _DVParameter;
        [VisibleInDetailView(false)]
        public string DVParameter
        {
            get { return _DVParameter; }
            set { SetPropertyValue(nameof(DVParameter), ref _DVParameter, value); }
        }
        #endregion

        #region Units
        private string _Units;
        [VisibleInDetailView(false)]
        public string Units
        {
            get { return _Units; }
            set { SetPropertyValue(nameof(Units), ref _Units, value); }
        }
        #endregion

        #region Average
        private string _Average;
        [VisibleInDetailView(false)]
        public string Average
        {
            get { return _Average; }
            set { SetPropertyValue(nameof(Average), ref _Average, value); }
        }
        #endregion

        #region Minimum
        private string _Minimum;
        [VisibleInDetailView(false)]
        public string Minimum
        {
            get { return _Minimum; }
            set { SetPropertyValue(nameof(Minimum), ref _Minimum, value); }
        }
        #endregion

        #region Maximum
        private string _Maximum;
        [VisibleInDetailView(false)]
        public string Maximum
        {
            get { return _Maximum; }
            set { SetPropertyValue(nameof(Maximum), ref _Maximum, value); }
        }
        #endregion

        #region STDV
        private string _STDV;
        [VisibleInDetailView(false)]
        public string STDV
        {
            get { return _STDV; }
            set { SetPropertyValue(nameof(STDV), ref _STDV, value); }
        }
        #endregion    

        #region PointCount
        private string _PointCount;
        [VisibleInDetailView(false)]
        public string PointCount
        {
            get { return _PointCount; }
            set { SetPropertyValue(nameof(PointCount), ref _PointCount, value); }
        }
        #endregion

        #region GeographicSelector
        private SampleLogIn _GeographicSelector;
        [DataSourceProperty(nameof(GeographicSelectors))]
        public SampleLogIn GeographicSelector
        {
            get { return _GeographicSelector; }
            set { SetPropertyValue(nameof(GeographicSelector), ref _GeographicSelector, value); }
        }

        [Browsable(false)]
        public XPCollection<SampleLogIn> GeographicSelectors
        {
            get
            {
                XPView testsView = new XPView(Session, typeof(SampleLogIn));
                testsView.Properties.Add(new ViewProperty("TSamplingLocation", SortDirection.Ascending, "SamplingLocation", true, true));
                testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                List<object> groups = testsView.Cast<ViewRecord>().Select(rec => rec["Toid"]).ToList();
                //List<object> groups = new List<object>();
                //foreach (ViewRecord rec in testsView)
                //    groups.Add(rec["Toid"]);
                return new XPCollection<SampleLogIn>(Session, new InOperator("Oid", groups));
            }
        }
        #endregion

        #region MinimumScale
        private Nullable<double> _MinimumScale;
        [VisibleInListView(false)]
        public Nullable<double> MinimumScale
        {
            get { return _MinimumScale; }
            set { SetPropertyValue(nameof(MinimumScale), ref _MinimumScale, value); }
        }
        #endregion

        #region MaximumScale
        private Nullable<double> _MaximumScale;
        [VisibleInListView(false)]
        public Nullable<double> MaximumScale
        {
            get { return _MaximumScale; }
            set { SetPropertyValue(nameof(MaximumScale), ref _MaximumScale, value); }
        }
        #endregion
        #region ScaleSize
        private Nullable<double> _ScaleSize;
        [VisibleInListView(false)]
        public Nullable<double> ScaleSize
        {
            get { return _ScaleSize; }
            set { SetPropertyValue(nameof(ScaleSize), ref _ScaleSize, value); }
        }
        #endregion
    }
}