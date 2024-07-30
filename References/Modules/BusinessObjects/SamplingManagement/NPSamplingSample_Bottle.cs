using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Modules.BusinessObjects.InfoClass;
using DevExpress.ExpressApp.Editors;
using DevExpress.Xpo.DB;

namespace Modules.BusinessObjects.SamplingManagement
{
    [NonPersistent]
    public class NPSamplingSample_Bottle : BaseObject, ICheckedListBoxItemsProvider
    {
        SamplingManagementInfo SMInfo = new SamplingManagementInfo();
        public NPSamplingSample_Bottle(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private Sampling _From;
        [DataSourceProperty("SampleIDDataSource")]
        [ImmediatePostData(true)]
        public Sampling From
        {
            get
            {
                return _From;
            }
            set
            {
                SetPropertyValue<Sampling>("From", ref _From, value);
            }
        }

        private string _To;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string To
        {
            get
            {
                return _To;
            }
            set
            {
                SetPropertyValue<string>("To", ref _To, value);
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
            Dictionary<object, string> properties = new Dictionary<object, string>();
            if (targetMemberName == "To" && SampleIDDataSource != null && SampleIDDataSource.Count > 0)
            {
                foreach (Sampling objlab in SampleIDDataSource)
                {
                    if (!properties.ContainsKey(objlab.Oid) && !string.IsNullOrEmpty(objlab.SampleID) && From != null && objlab.SampleID != From.SampleID)
                    {
                        properties.Add(objlab.Oid, objlab.SampleID);
                    }
                }
            }
            return properties;
        }


        [Browsable(false)]
        public XPCollection<Sampling> SampleIDDataSource
        {
            get
            {
                return new XPCollection<Sampling>(Session, CriteriaOperator.Parse("[SamplingProposal.RegistrationID] = ?", SMInfo.strJobID), new SortProperty("SampleID", SortingDirection.Ascending));
            }
        }
    }
}