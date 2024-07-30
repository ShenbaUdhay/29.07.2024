// ================================================================================
// Table Name: [Position]
// Author: Sunny
// Date: 2016年12月13日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：职务
// ================================================================================
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Hr
{
    /// <summary>
    /// 表[Position]的实体类
    /// </summary>
    [DefaultClassOptions]
    //[RuleCombinationOfPropertiesIsUnique("Position", DefaultContexts.Save, "PositionName", SkipNullOrEmptyValues = false)]
    public class Position : BaseObject
    {

        /// <summary>
        /// 初始化类 Position 的新实例。
        /// </summary>
        public Position(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;

            //排序
            var maxSort = Session.Evaluate<Position>(CriteriaOperator.Parse("MAX(Sort)"), null);
            int sort = 1;
            if (maxSort != null)
            {
                sort = int.Parse(maxSort.ToString()) + 1;
            }
            Sort = sort;
        }
        #region OnDeleting
        protected override void OnDeleting()
        {
            base.OnDeleting();
            System.Collections.ICollection lstReferenceObjects = Session.CollectReferencingObjects(this);
            if (lstReferenceObjects.Count > 0)
            {
                foreach (var obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.GetType() != typeof(DevExpress.Xpo.Metadata.Helpers.IntermediateObject))
                    {
                        Exception ex = new Exception("Already used can't allow to delete");
                        throw ex;
                        break;
                    }
                }
            }
        }
        #endregion

        //#region OnDelete
        //protected override void OnDeleting()
        //{
        //    base.OnDeleting();
        //    if (Session.CollectReferencingObjects(this).Count > 0)
        //    {
        //        foreach (BaseObject obj in Session.CollectReferencingObjects(this))
        //        {
        //            if (obj.Oid != null)
        //            {
        //                Exception ex = new Exception("Already Used Can't allow to Delete");
        //                throw ex;
        //                break;

        //            }
        //        }
        //    }
        //}
        //#endregion

        #region 职务编码
        private string _positionCode;
        /// <summary>
        /// 职务编码
        /// </summary>
        [Size(32)]
        //[RuleRequiredField("Position.PositionCode", DefaultContexts.Save)]
        [Browsable(false)]
        public string PositionCode
        {
            get
            {
                return _positionCode;
            }
            set
            {
                SetPropertyValue("PositionCode", ref _positionCode, value);
            }
        }
        #endregion

        #region 职务名称
        private string _positionName;
        /// <summary>
        /// 职务名称
        /// </summary>
        [Size(64)]
        [RuleRequiredField("Position.PositionName", DefaultContexts.Save,"'Position' must not be empty")]
        public string PositionName
        {
            get
            {
                return _positionName;
            }
            set
            {
                SetPropertyValue("PositionName", ref _positionName, value);
            }
        }
        #endregion

        #region 排序
        private int _sort;
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort
        {
            get
            {
                return _sort;
            }
            set
            {
                SetPropertyValue("Sort", ref _sort, value);
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
        [Association("Company-Positions")]
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
