using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.Seting
{
    [DefaultClassOptions]
    public class KeyValue : BaseObject
    {
        public KeyValue(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region 公司
        private Company _company;
        /// <summary>
        /// 公司
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Company Company
        {
            get
            {
                return _company;
            }
            set
            {
                SetPropertyValue("Company", ref _company, value);
            }
        }
        #endregion

        #region 类型
        private KeyType _keyType;
        [RuleRequiredField("KeyValue.KeyType", DefaultContexts.Save)]
        public KeyType KeyType
        {
            get { return _keyType; }
            set { SetPropertyValue("KeyType", ref _keyType, value); }
        }
        #endregion

        #region 编码
        private string _number;
        /// <summary>
        /// 编码
        /// </summary>
        public string Number
        {
            get { return _number; }
            set { SetPropertyValue("Number", ref _number, value); }
        }
        #endregion

        #region 名称
        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
        [RuleUniqueValue]
        [RuleRequiredField("KeyValue.Name", DefaultContexts.Save)]
        [Size(128)]
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }
        #endregion

        #region 是否默认
        private bool _isDefault;
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
            set { SetPropertyValue("IsDefault", ref _isDefault, value); }
        }
        #endregion

        #region 备注
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        [ModelDefault("RowCount", "1")]
        [Size(512)]
        public string Remark
        {
            get { return _remark; }
            set { SetPropertyValue("Remark", ref _remark, value); }
        }
        #endregion
    }
}