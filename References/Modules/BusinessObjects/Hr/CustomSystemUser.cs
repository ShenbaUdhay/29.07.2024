using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.Hr
{
    [DefaultClassOptions]
    public class CustomSystemUser : PermissionPolicyUser
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CustomSystemUser(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

        }
        protected override void OnSaving()
        {
            base.OnSaving();
        }
        [Association("WorkflowConfigUser", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<WorkflowConfig> WorkflowConfig
        {
            get { return GetCollection<WorkflowConfig>("WorkflowConfig"); }

        }

        public static explicit operator CustomSystemUser(string v)
        {
            throw new NotImplementedException();
        }
    }
}