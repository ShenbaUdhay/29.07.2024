// ================================================================================
// Table Name: [InspectCategory]
// Author: Sunny
// Date: 2016年12月13日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：检定类型
// ================================================================================
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;

namespace Modules.BusinessObjects.Assets
{
    /// <summary>
    /// Labware-InspectCategory
    /// 子表：仪器设备-检定类型
    /// </summary>
    [DefaultClassOptions]

    public class InspectCategory : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InspectCategory(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;
        }

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

        #region 检定类型
        private string _name;
        /// <summary>
        /// 检定类型名称
        /// </summary>
        [RuleUniqueValue]
        [RuleRequiredField("InspectCategory.Name", DefaultContexts.Save)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetPropertyValue("Name", ref _name, value);
            }
        }
        #endregion

        #region 上次检定日期
        private DateTime _lastTime;
        /// <summary>
        /// 上次检定日期
        /// </summary>
        public DateTime LastTime
        {
            get
            {
                return _lastTime;
            }
            set
            {
                SetPropertyValue("LastTime", ref _lastTime, value);
            }
        }
        #endregion

        #region 下次检定日期
        private DateTime _nextTime;
        /// <summary>
        /// 下次检定日期
        /// </summary>
        public DateTime NextTime
        {
            get
            {
                return _nextTime;
            }
            set
            {
                SetPropertyValue("NextTime", ref _nextTime, value);
            }
        }
        #endregion

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
    }
}