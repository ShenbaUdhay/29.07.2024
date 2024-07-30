using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class DBConnection : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public DBConnection(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Title
        private string _Title;
        [RuleRequiredField(DefaultContexts.Save)]
        public string Title
        {
            get { return _Title; }
            set { SetPropertyValue(nameof(Title), ref _Title, value); }
        }
        #endregion

        #region ServerName
        private string _ServerName;
        [RuleRequiredField(DefaultContexts.Save)]
        public string ServerName
        {
            get { return _ServerName; }
            set { SetPropertyValue(nameof(ServerName), ref _ServerName, value); }
        }
        #endregion

        #region DataBaseName
        private string _DataBaseName;
        [RuleRequiredField(DefaultContexts.Save)]
        public string DataBaseName
        {
            get { return _DataBaseName; }
            set { SetPropertyValue(nameof(DataBaseName), ref _DataBaseName, value); }
        }
        #endregion

        #region UserName
        private string _UserName;
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Username")]
        public string UserName
        {
            get { return _UserName; }
            set { SetPropertyValue(nameof(UserName), ref _UserName, value); }
        }
        #endregion

        #region Password
        private string _Password;
        [RuleRequiredField(DefaultContexts.Save)]
        public string Password
        {
            get { return _Password; }
            set { SetPropertyValue(nameof(Password), ref _Password, value); }
        }
        #endregion

        #region IsDefault
        private bool _IsDefault;
        public bool IsDefault
        {
            get { return _IsDefault; }
            set { SetPropertyValue(nameof(IsDefault), ref _IsDefault, value); }
        }
        #endregion
    }
}