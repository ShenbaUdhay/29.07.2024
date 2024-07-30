// ================================================================================
// Table Name: [Area]
// Author: Sunny
// Date: 2016年12月15日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：区
// ================================================================================
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace Modules.BusinessObjects.Hr
{
    /// <summary>
    /// 区
    /// </summary>
    [RuleCombinationOfPropertiesIsUnique("Area", DefaultContexts.Save, "AreaName", "Area name must unique", SkipNullOrEmptyValues = false)]
    [DefaultClassOptions]
    public class Area : BaseObject
    {
        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Constructor
        public Area(Session session)
            : base(session)
        {
        }
        #endregion

        #region DefaultEvent
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #endregion

        #region AreaNumber
        private string _areaNumber;
        /// <summary>
        /// 区编号
        /// </summary>
        //[RuleRequiredField("Area.AreaNumber", DefaultContexts.Save)]
        [Size(8)]
        [Browsable(false)]
        public string AreaNumber
        {
            get { return _areaNumber; }
            set { SetPropertyValue("AreaNumber", ref _areaNumber, value); }
        }
        #endregion

        #region AreaName
        private string _areaName;
        /// <summary>
        /// 区名称
        /// </summary>
        [RuleRequiredField("Area.AreaName", DefaultContexts.Save , "'Area' must not to be empty.")]
        [Size(128)]
        public string AreaName
        {
            get { return _areaName; }
            set { SetPropertyValue("AreaName", ref _areaName, value); }
        }
        #endregion

        #region Citry
        //private City _city;
        /// <summary>
        /// 所属城市
        /// </summary>
        [Association("City_Areas", UseAssociationNameAsIntermediateTableName = true)]
        //[RuleRequiredField("Area.City", DefaultContexts.Save)]
        public XPCollection<City> City
        {
            get
            {
                return GetCollection<City>("City");
            }
            //get { return _city; }
            //set { SetPropertyValue("City", ref _city, value); }
        }
        #endregion

    }
}