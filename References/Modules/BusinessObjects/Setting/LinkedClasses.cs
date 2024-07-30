using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DomainComponent]
    //[NonPersistent]
    public class LinkedClasses : BaseObject, ICheckedListBoxItemsProvider
    {
        public LinkedClasses(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region NavigationItem
        private NavigationItem _NavigationItem;
        [Association("NavigationItemLinkedClasses")]
        public NavigationItem NavigationItem
        {
            get
            {
                return _NavigationItem;
            }
            set
            {
                SetPropertyValue<NavigationItem>(nameof(NavigationItem), ref _NavigationItem, value);
            }
        }
        #endregion

        #region ClassName
        private string _ClassName;

        public event EventHandler ItemsChanged;

        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public string ClassName
        {
            get
            {
                return _ClassName;
            }
            set
            {
                SetPropertyValue<string>(nameof(ClassName), ref _ClassName, value);
            }
        }
        #endregion

        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> properties = new Dictionary<object, string>();
            if (targetMemberName == "ClassName" && ClassDataSource != null && ClassDataSource.Count > 0)
            {
                foreach (NavigationItem objlab in ClassDataSource.OrderBy(a => a.NavigationModelClass).ToList())
                {
                    if (!properties.ContainsValue(objlab.NavigationModelClass))
                    {
                        properties.Add(objlab.Oid, objlab.NavigationModelClass);
                    }
                }
            }
            return properties;
        }
        #endregion

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<NavigationItem> ClassDataSource
        {
            get
            {
                return new XPCollection<NavigationItem>(Session, CriteriaOperator.Parse(""));
            }
        }

    }
}