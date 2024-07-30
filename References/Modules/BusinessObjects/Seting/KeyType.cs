using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
//using Modules.BusinessObjects.Assets;

namespace Modules.BusinessObjects.Seting
{
    [DefaultClassOptions]
    public class KeyType : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public KeyType(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;
            var maxSort = Session.Evaluate<KeyType>(CriteriaOperator.Parse("MAX(TypeNumber)"), null);
            int number = 1;
            if (maxSort != null)
            {
                number = int.Parse(maxSort.ToString()) + 1;
            }
            TypeNumber = number;
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

        #region 编码
        private int _typeNumber;
        /// <summary>
        /// 编码
        /// </summary>
        [Size(8)]
        public int TypeNumber
        {
            get { return _typeNumber; }
            set { SetPropertyValue("TypeNumber", ref _typeNumber, value); }
        }
        #endregion

        #region 名称
        private string _typeName;
        /// <summary>
        /// 名称
        /// </summary>
        [RuleUniqueValue]
        [RuleRequiredField("TypeName", DefaultContexts.Save)]
        [Size(64)]
        public string TypeName
        {
            get { return _typeName; }
            set { SetPropertyValue("TypeName", ref _typeName, value); }
        }
        #endregion

        #region 业务类别(计量/质检)
        //private BizCategory _bizCategory;
        ///// <summary>
        ///// 业务类别(计量/质检)
        ///// </summary>
        //public BizCategory BizCategory
        //{
        //    get
        //    {
        //        return _bizCategory;
        //    }
        //    set
        //    {
        //        SetPropertyValue("BizCategory", ref _bizCategory, value);
        //    }
        //}
        #endregion

    }
}