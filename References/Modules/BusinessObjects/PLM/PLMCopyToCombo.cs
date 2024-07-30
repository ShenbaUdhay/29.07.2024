using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.BusinessObjects.PLM
{
    [NonPersistent]
    public class PLMCopyToCombo : BaseObject, ICheckedListBoxItemsProvider
    {
        PLMInfo plmInfo = new PLMInfo();
        public PLMCopyToCombo(Session session) : base(session) { }

        private string _SourceSample;
        [EditorAlias("ComboBoxEditor")]
        public string SourceSample
        {
            get { return _SourceSample; }
            set { SetPropertyValue(nameof(SourceSample), ref _SourceSample, value); }
        }

        private string _CopyTo;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string CopyTo
        {
            get { return _CopyTo; }
            set { SetPropertyValue(nameof(CopyTo), ref _CopyTo, value); }
        }

        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "CopyTo")
            {
                if (SourceSample != null)
                {
                    Properties = plmInfo.lstPLMSte.ToDictionary(a => a.Key, a => a.Value);
                    var item = Properties.FirstOrDefault(a => a.Value == SourceSample);
                    if (item.Value != null)
                    {
                        Properties.Remove(item.Key);
                    }
                }
                else
                {
                    Properties = plmInfo.lstPLMSte;
                }
            }
            return Properties.OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
        }
        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        #endregion
    }
}