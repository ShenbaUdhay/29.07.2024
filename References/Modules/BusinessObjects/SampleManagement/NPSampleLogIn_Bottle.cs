using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modules.BusinessObjects.SampleManagement
{
    [NonPersistent]
    public class NPSampleLogIn_Bottle : BaseObject, ICheckedListBoxItemsProvider
    {
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        public NPSampleLogIn_Bottle(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private SampleLogIn _From;
        [DataSourceProperty("SampleIDDataSource")]
        [ImmediatePostData(true)]
        public SampleLogIn From
        {
            get
            {
                return _From;
            }
            set
            {
                SetPropertyValue<SampleLogIn>("From", ref _From, value);
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
                foreach (SampleLogIn objlab in SampleIDDataSource)
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
        public XPCollection<SampleLogIn> SampleIDDataSource
        {
            get
            {
                return new XPCollection<SampleLogIn>(Session, CriteriaOperator.Parse("[JobID.JobID] = ?", SRInfo.strJobID), new SortProperty("SampleID", SortingDirection.Ascending));
            }
        }
    }
}
