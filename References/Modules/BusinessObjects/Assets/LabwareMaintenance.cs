// ================================================================================
// Table Name: [LabwareMaintenance]
// Author: Sunny
// Date: 2016年12月13日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：保养记录，仪器设备的保养记录
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
    /// 表[LabwareMaintenance]的实体类
    /// </summary>
    [DefaultClassOptions]
    [DefaultProperty("MaintainedBy")]
    [XafDisplayName("Instrument Maintenance")]
    public class LabwareMaintenance : BaseObject
    {

        /// <summary>
        /// 初始化类 LabwareMaintenance 的新实例。
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

        #region 所属设备
        private Labware _labware;
        /// <summary>
        /// 所属设备
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

        #region 保养人
        private Employee _maintainedBy;
        /// <summary>
        /// 保养人
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

        #region 保养时间
        private DateTime _maintenanceDate;
        /// <summary>
        /// 保养时间
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

        #region 材料
        private string _material;
        /// <summary>
        /// 材料
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

        #region 创建人
        private CustomSystemUser _createdBy;
        /// <summary>
        /// 创建人
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

        #region 创建时间
        private DateTime _createTime;
        /// <summary>
        /// 创建时间
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

        #region 修改人
        private CustomSystemUser _updatedBy;
        /// <summary>
        /// 修改人
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

        #region 修改时间
        private DateTime _updateTime;
        /// <summary>
        /// 修改时间
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
