// ================================================================================
// Table Name: [Navigation]
// Author: Sunny
// Date: 2017年03月23日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：导航表，用于关联角色表
// ================================================================================
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Hr
{
    [DefaultClassOptions]
    public class Navigation : BaseObject
    {
        public Navigation(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region 导航编码
        private string _navigationId;
        /// <summary>
        /// 导航编码
        /// </summary>
        [Size(1024)]
        [RuleRequiredField("Navigation.NavigationId", DefaultContexts.Save)]
        public string NavigationId
        {
            get
            {
                return _navigationId;
            }
            set
            {
                SetPropertyValue("NavigationId", ref _navigationId, value);
            }
        }
        #endregion

        #region 导航名称
        private string _navigationName;
        /// <summary>
        /// 导航名称
        /// </summary>
        [Size(128)]
        [RuleRequiredField("Navigation.NavigationName", DefaultContexts.Save)]
        public string NavigationName
        {
            get
            {
                return _navigationName;
            }
            set
            {
                SetPropertyValue("NavigationName", ref _navigationName, value);
            }
        }
        #endregion

        #region 上级
        private Navigation _parent;
        /// <summary>
        /// 导航名称
        /// </summary>
        public Navigation Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                SetPropertyValue("Parent", ref _parent, value);
            }
        }
        #endregion

        #region 角色
        /// <summary>
        /// 角色
        /// </summary>
        [Association("Roles-Navigations")]
        public XPCollection<CustomSystemRole> Roles
        {
            get { return GetCollection<CustomSystemRole>("Roles"); }
        }

        #endregion


    }
}