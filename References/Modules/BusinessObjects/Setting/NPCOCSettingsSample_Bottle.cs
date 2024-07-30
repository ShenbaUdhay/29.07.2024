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

namespace Modules.BusinessObjects.Setting
{
    [NonPersistent]
    public class NPCOCSettingsSample_Bottle : BaseObject, ICheckedListBoxItemsProvider
    {
        COCSettingsRegistrationInfo COCsr = new COCSettingsRegistrationInfo();
        public NPCOCSettingsSample_Bottle(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private COCSettingsSamples _From;
        [DataSourceProperty("SampleIDDataSource")]
        [ImmediatePostData(true)]
        public COCSettingsSamples From
        {
            get
            {
                return _From;
            }
            set
            {
                SetPropertyValue<COCSettingsSamples>("From", ref _From, value);
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
                foreach (COCSettingsSamples objlab in SampleIDDataSource)
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
        public XPCollection<COCSettingsSamples> SampleIDDataSource
        {
            get
            {
                return new XPCollection<COCSettingsSamples>(Session, CriteriaOperator.Parse("[COCID.COC_ID] = ?", COCsr.strCOCID), new SortProperty("SampleID", SortingDirection.Ascending));
            }
        }
    }
}