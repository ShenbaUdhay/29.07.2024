using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("User")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class UserNavigationPermission : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public UserNavigationPermission(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private CustomSystemUser _User;
        [RuleRequiredField]
        // 
        public CustomSystemUser User
        {
            get { return _User; }
            set { SetPropertyValue("User", ref _User, value); }
        }

        [Association, Browsable(false)]
        public IList<UserNavigationPermissionDetails> UserNavigationPermissionDetails
        {
            get
            {
                return GetList<UserNavigationPermissionDetails>("UserNavigationPermissionDetails");
            }
        }

        [ManyToManyAlias("UserNavigationPermissionDetails", "NavigationItem")]
        public IList<NavigationItem> NavigationItems
        {
            get
            {
                return GetList<NavigationItem>("NavigationItems");
            }
        }


    }
}