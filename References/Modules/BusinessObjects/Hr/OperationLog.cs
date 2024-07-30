// ================================================================================
// Table Name: [OperationLog]
// Author: Sunny
// Date: 2017��01��10��
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption��������־
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
    /// ��[OperationLog]��ʵ����
    /// </summary>
    [DefaultClassOptions]
    public class OperationLog : BaseObject
    {

        /// <summary>
        /// ��ʼ���� OperationLog ����ʵ����
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

        #region ������
        private Employee _employee;
        /// <summary>
        /// ������
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

        #region ��������
        private string _operationEvent;
        /// <summary>
        /// ��������
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

        #region ����ʱ��
        private DateTime _operationDate;
        /// <summary>
        /// ����ʱ��
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

        #region ��־���
        private OperationCategory _category;
        /// <summary>
        /// ��־���
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

        #region ��ע
        private string _remark;
        /// <summary>
        /// ��ע
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
