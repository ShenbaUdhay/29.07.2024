// ================================================================================
// Table Name: [City]
// Author: Sunny
// Date: 2016年12月15日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：市
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
    /// 市
    /// </summary>
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("City", DefaultContexts.Save, "CityName", "City name must unique", SkipNullOrEmptyValues = false)]
    public class City : BaseObject
    {
        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Consturctor
        public City(Session session)
            : base(session)
        {
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

        #region  DefaultEvents
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #endregion

        #region CityNumber
        private string _cityNumber;
        /// <summary>
        /// 城市编号
        /// </summary>
        //[RuleRequiredField("City.CityNumber", DefaultContexts.Save)]
        [Size(8)]
        [Browsable(false)]
        public string CityNumber
        {
            get { return _cityNumber; }
            set { SetPropertyValue("CityNumber", ref _cityNumber, value); }
        }
        #endregion

        #region CityName
        private string _cityName;
        /// <summary>
        /// 城市名称
        /// </summary>
        //[RuleRequiredField("City.CityName", DefaultContexts.Save)]
        [RuleRequiredField("City.CityName", DefaultContexts.Save, "'City' must not to be empty.")]

        [Size(128)]
        public string CityName
        {
            get { return _cityName; }
            set { SetPropertyValue("CityName", ref _cityName, value); }
        }
        #endregion

        #region State
        private CustomState _state;
        //[RuleRequiredField("City.State", DefaultContexts.Save)]
        /// <summary>
        /// 所属省、州
        /// </summary>
        [Association("State-Cities")]

        public CustomState State
        {
            get { return _state; }
            set { SetPropertyValue("State", ref _state, value); }
        }
        #endregion

        #region CityArea
        /// <summary>
        /// 城市下的区
        /// </summary>
        [Association("City_Areas", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Area> Areas
        {
            get
            {
                return GetCollection<Area>("Areas");
            }
        }
        #endregion
    }
}