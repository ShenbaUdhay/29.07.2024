// ================================================================================
// Table Name: [State]
// Author: Sunny
// Date: 2016年12月15日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：省
// ================================================================================
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Hr
{
    /// <summary>
    /// 省、州，同Province
    /// </summary>
    //[RuleCombinationOfPropertiesIsUnique("CustomState", DefaultContexts.Save, "LongName", "State name must unique", SkipNullOrEmptyValues = false)]
    [DefaultClassOptions]
    public class CustomState : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Constructor
        public CustomState(Session session)
            : base(session)
        {
        }
        #endregion

        #region Events
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #endregion

        #region OnDelete
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (BaseObject obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.Oid != null)
                    {
                        Exception ex = new Exception("Already Used Can't allow to Delete");
                        throw ex;
                        break;

                    }
                }
            }
        }
        #endregion


        #region 编号
        private string _stateNumber;
        /// <summary>
        /// 省、州编号
        /// </summary>
        //[RuleRequiredField("CustomState.StateNumber", DefaultContexts.Save)]
        [Size(8)]
        [Browsable(false)]
        public string StateNumber
        {
            get { return _stateNumber; }
            set { SetPropertyValue("StateNumber", ref _stateNumber, value); }
        }
        #endregion

        #region 名称
        private string _longName;
        /// <summary>
        /// 省、州名称
        /// </summary>
        [DevExpress.Xpo.DisplayName("State")]
        [RuleRequiredField("CustomState.LongName", DefaultContexts.Save)]
        [RuleUniqueValue]
        [Size(128)]
        public string LongName
        {
            get { return _longName; }
            set { SetPropertyValue("LongName", ref _longName, value); }
        }
        #endregion

        #region 简称
        private string _shortName;
        /// <summary>
        /// 简称
        /// </summary>
        //[RuleRequiredField("CustomState.ShortName", DefaultContexts.Save)]
        [RuleUniqueValue]
        [Size(16)]
        public string ShortName
        {
            get { return _shortName; }
            set { SetPropertyValue("ShortName", ref _shortName, value); }
        }
        #endregion

        #region 国家
        private CustomCountry _country;
        /// <summary>
        /// 所属国家
        /// </summary>
        [Association("Country-States")]
        //[RuleRequiredField("CustomState.Country", DefaultContexts.Save)]
        public CustomCountry Country
        {
            get { return _country; }
            set { SetPropertyValue("Country", ref _country, value); }
        }
        #endregion

        #region 城市
        /// <summary>
        /// 国家下的城市
        /// </summary>
        [Association("State-Cities")]
        public XPCollection<City> Cities
        {
            get
            {
                return GetCollection<City>("Cities");
            }
        }
        #endregion


    }
}