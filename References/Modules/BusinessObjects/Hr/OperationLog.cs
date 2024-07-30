// ================================================================================
// Table Name: [OperationLog]
// Author: Sunny
// Date: 2017年01月10日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：操作日志
// ================================================================================
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.Hr
{
    /// <summary>
    /// 表[OperationLog]的实体类
    /// </summary>
    [DefaultClassOptions]
    public class OperationLog : BaseObject
    {

        /// <summary>
        /// 初始化类 OperationLog 的新实例。
        /// </summary>
        public OperationLog(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here.
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            Employee = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            OperationDate = Library.GetServerTime(Session);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            // Place you update code here.
        }

        #region 操作人
        private Employee _employee;
        /// <summary>
        /// 操作人
        /// </summary>
        //[DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public Employee Employee
        {
            get
            {
                return _employee;
            }
            set
            {
                SetPropertyValue("Employee", ref _employee, value);
            }
        }
        #endregion

        #region 操作事项
        private string _operationEvent;
        /// <summary>
        /// 操作事项
        /// </summary>
        [Size(256)]
        public string OperationEvent
        {
            get
            {
                return _operationEvent;
            }
            set
            {
                SetPropertyValue("OperationEvent", ref _operationEvent, value);
            }
        }
        #endregion

        #region 操作时间
        private DateTime _operationDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationDate
        {
            get
            {
                return _operationDate;
            }
            set
            {
                SetPropertyValue("OperationDate", ref _operationDate, value);
            }
        }
        #endregion

        #region 日志类别
        private OperationCategory _category;
        /// <summary>
        /// 日志类别
        /// </summary>
        public OperationCategory Category
        {
            get
            {
                return _category;
            }
            set
            {
                SetPropertyValue("Category", ref _category, value);
            }
        }
        #endregion

        #region 备注
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        [Size(int.MaxValue)]
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                SetPropertyValue("Remark", ref _remark, value);
            }
        }
        #endregion


    }
}
