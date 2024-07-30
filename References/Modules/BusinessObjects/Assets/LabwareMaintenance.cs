// ================================================================================
// Table Name: [LabwareMaintenance]
// Author: Sunny
// Date: 2016��12��13��
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption��������¼�������豸�ı�����¼
// ================================================================================
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Assets
{
    /// <summary>
    /// ��[LabwareMaintenance]��ʵ����
    /// </summary>
    [DefaultClassOptions]
    [DefaultProperty("MaintainedBy")]
    [XafDisplayName("Instrument Maintenance")]
    public class LabwareMaintenance : BaseObject
    {

        /// <summary>
        /// ��ʼ���� LabwareMaintenance ����ʵ����
        /// </summary>
        public LabwareMaintenance(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreateTime = Library.GetServerTime(Session);
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
            MaintenanceDate = DateTime.Now;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
        }

        #region �����豸
        private Labware _labware;
        /// <summary>
        /// �����豸
        /// </summary>
        [Association("Labware-Maintenances")]
        [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        [RuleRequiredField]
        [XafDisplayName("Instrument")]
        [RuleUniqueValue]
        public Labware Labware
        {
            get
            {
                return _labware;
            }
            set
            {
                SetPropertyValue("Labware", ref _labware, value);
            }
        }
        #endregion

        #region ������
        private Employee _maintainedBy;
        /// <summary>
        /// ������
        /// </summary>
        [RuleRequiredField("LabwareMaintenance.MaintainedBy", DefaultContexts.Save , "'MaintainedBy must not be empty'")]
        public Employee MaintainedBy
        {
            get
            {
                return _maintainedBy;
            }
            set
            {
                SetPropertyValue("MaintainedBy", ref _maintainedBy, value);
            }
        }
        #endregion

        #region ����ʱ��
        private DateTime _maintenanceDate;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime MaintenanceDate
        {
            get
            {
                return _maintenanceDate;
            }
            set
            {
                SetPropertyValue("MaintenanceDate", ref _maintenanceDate, value);
            }
        }
        #endregion

        #region ����
        private string _material;
        /// <summary>
        /// ����
        /// </summary>
        [Size(2048)]
        public string Material
        {
            get
            {
                return _material;
            }
            set
            {
                SetPropertyValue("Material", ref _material, value);
            }
        }
        #endregion

        #region ������
        private CustomSystemUser _createdBy;
        /// <summary>
        /// ������
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser CreatedBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _createdBy, value);
            }
        }
        #endregion

        #region ����ʱ��
        private DateTime _createTime;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
            set
            {
                SetPropertyValue("CreateTime", ref _createTime, value);
            }
        }
        #endregion

        #region �޸���
        private CustomSystemUser _updatedBy;
        /// <summary>
        /// �޸���
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser UpdatedBy
        {
            get
            {
                return _updatedBy;
            }
            set
            {
                SetPropertyValue("UpdatedBy", ref _updatedBy, value);
            }
        }
        #endregion

        #region �޸�ʱ��
        private DateTime _updateTime;
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime UpdateTime
        {
            get
            {
                return _updateTime;
            }
            set
            {
                SetPropertyValue("UpdateTime", ref _updateTime, value);
            }
        }
        #endregion

    }
}
