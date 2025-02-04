// ================================================================================
// Table Name: [Labware]
// Author: Sunny
// Date: 2016年12月13日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：仪器设备,用于检测的仪器
// ================================================================================
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.Seting;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Assets
{
    /// <summary>
    /// 表[Labware]的实体类
    /// </summary>
    [DefaultClassOptions]
    [XafDisplayName("Instrument")]
    [FileAttachment("Attachment")]
    [DefaultProperty("LabwareName")]  

    public class Labware : BaseObject, ICheckedListBoxItemsProvider
    {
        /// <summary>
        /// 初始化类 Labware 的新实例。
        /// </summary>
        public Labware(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreateTime = Library.GetServerTime(Session);
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;

            UncertaintyImage = "<div style='text-align: center;vertical-align:middle;font-family:宋体;font-size: 9pt; line-height:26px;'></div>";

            if (string.IsNullOrEmpty(LabwareID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(LabwareID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(Labware), criteria, null)) + 1).ToString("0000");
                LabwareID = "IN" + tempID;
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            System.Collections.ICollection lstReferenceObjects = Session.CollectReferencingObjects(this);
            if (lstReferenceObjects.Count > 0)
            {
                foreach (var obj in Session.CollectReferencingObjects(this))
                {
                    //if (obj.GetType() == typeof(DevExpress.Xpo.Metadata.Helpers.IntermediateObject)||obj.GetType()==typeof(DevExpress.Persistent.BaseImpl.BaseObject))
                    if(obj.GetType() != typeof(Labware))
                    {
                        Exception ex = new Exception("Please note that this item can not be deleted since it has referenced already.");
                        throw ex;
                        break;
                    }
                }
            }
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
        }
        /// <summary>
        /// 设备名称+型号规格+计量范围 
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public string FullName
        {
            get
            {
                if (LabwareName != null && Specification != null && MeasuringRange != null)
                {
                    return LabwareName + "-" + Specification + "-" + MeasuringRange;
                }
                else if (LabwareName != null && Specification != null)
                {
                    return LabwareName + "-" + Specification;
                }
                else if (LabwareName != null)
                {
                    return LabwareName;

                }
                else
                {
                    return null;
                }

            }
        }



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

        #region 设备名称
        private string _labwareName;
        /// <summary>
        /// 设备名称
        /// </summary>
        [Size(128)]
        [RuleUniqueValue]
        [RuleRequiredField("Labware.LabwareName", DefaultContexts.Save, "'Instrument Number' must not to be empty.")]
        [XafDisplayName("Instrument Number")]
        public string LabwareName
        {
            get
            {
                return _labwareName;
            }
            set
            {
                SetPropertyValue("LabwareName", ref _labwareName, value);
            }
        }
        #endregion

        #region 设备授予名
        private string _assignedName;
        /// <summary>
        /// 设备授予名
        /// </summary>
        [Size(128)]
        public string AssignedName
        {
            get
            {
                return _assignedName;
            }
            set
            {
                SetPropertyValue("AssignedName", ref _assignedName, value);
            }
        }
        #endregion

        #region 出厂编号
        private string _factoryNumber;
        /// <summary>
        /// 出厂编号
        /// </summary>
        [Size(256)]
        public string FactoryNumber
        {
            get
            {
                return _factoryNumber;
            }
            set
            {
                SetPropertyValue("FactoryNumber", ref _factoryNumber, value);
            }
        }
        #endregion

        #region 设备编号
        private string _labwareID;
        /// <summary>
        /// 设备编号
        /// </summary>
        [Size(256)]
        //[RuleRequiredField("Labware.LabwareID", DefaultContexts.Save, "Instrument ID must not to be empty.")]
        public string LabwareID
        {
            get
            {
                return _labwareID;
            }
            set
            {
                SetPropertyValue("LabwareID", ref _labwareID, value);
            }
        }
        #endregion

        #region 系列号
        private string _serialNumber;
        /// <summary>
        /// 系列号
        /// </summary>
        [Size(256)]
        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                SetPropertyValue("SerialNumber", ref _serialNumber, value);
            }
        }
        #endregion

        #region 固定资产编号
        private string _assetsNumber;
        /// <summary>
        /// 固定资产编号
        /// </summary>
        [Size(64)]
        public string AssetsNumber
        {
            get
            {
                return _assetsNumber;
            }
            set
            {
                SetPropertyValue("AssetsNumber", ref _assetsNumber, value);
            }
        }
        #endregion

        #region 档案号
        private string _fileNumber;
        /// <summary>
        /// 档案号
        /// </summary>
        [Size(64)]
        public string FileNumber
        {
            get
            {
                return _fileNumber;
            }
            set
            {
                SetPropertyValue("FileNumber", ref _fileNumber, value);
            }
        }
        #endregion

        #region 型号规格
        private string _specification;
        /// <summary>
        /// 型号规格
        /// </summary>
        [Size(32)]
        public string Specification
        {
            get
            {
                return _specification;
            }
            set
            {
                SetPropertyValue("Specification", ref _specification, value);
            }
        }
        #endregion

        #region 内部类别
        private KeyValue _internalCategory;
        /// <summary>
        /// 内部类别
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='2'")]
        public KeyValue InternalCategory
        {
            get
            {
                return _internalCategory;
            }
            set
            {
                SetPropertyValue("InternalCategory", ref _internalCategory, value);
            }
        }
        #endregion

        #region 计量范围
        private string _measuringRange;
        /// <summary>
        /// 计量范围
        /// </summary>
        [Size(32)]
        public string MeasuringRange
        {
            get
            {
                return _measuringRange;
            }
            set
            {
                SetPropertyValue("MeasuringRange", ref _measuringRange, value);
            }
        }
        #endregion

        #region 不确定度或准确度
        private string _uncertainty;
        /// <summary>
        /// 不确定度或准确度
        /// </summary>
        [Size(128)]
        public string Uncertainty
        {
            get
            {
                return _uncertainty;
            }
            set
            {
                SetPropertyValue("Uncertainty", ref _uncertainty, value);
            }
        }
        #endregion

        #region 不确定度或准确度图片
        private string _uncertaintyImage;
        /// <summary>
        /// 不确定度或准确度图片
        /// </summary>
        [Size(int.MaxValue)]
        public string UncertaintyImage
        {
            get
            {
                return _uncertaintyImage;
            }
            set
            {
                SetPropertyValue("UncertaintyImage", ref _uncertaintyImage, value);
            }
        }
        #endregion

        #region 制造厂家
        private string _manufacturer;
        /// <summary>
        /// 制造厂家
        /// </summary>
        [Size(64)]
        public string Manufacturer
        {
            get
            {
                return _manufacturer;
            }
            set
            {
                SetPropertyValue("Manufacturer", ref _manufacturer, value);
            }
        }
        #endregion

        #region 购进日期
        private string _purchasingDate;
        /// <summary>
        /// 购进日期
        /// </summary>
        public string PurchasingDate
        {
            get
            {
                return _purchasingDate;
            }
            set
            {
                SetPropertyValue("PurchasingDate", ref _purchasingDate, value);
            }
        }
        #endregion

        #region 验收日期
        private string _acceptanceDate;
        /// <summary>
        /// 验收日期
        /// </summary>
        public string AcceptanceDate
        {
            get
            {
                return _acceptanceDate;
            }
            set
            {
                SetPropertyValue("AcceptanceDate", ref _acceptanceDate, value);
            }
        }
        #endregion

        #region 启用日期
        private string _startDate;
        /// <summary>
        /// 启用日期
        /// </summary>
        public string StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                SetPropertyValue("StartDate", ref _startDate, value);
            }
        }
        #endregion

        #region 归档日期
        private DateTime _filingDate;
        /// <summary>
        /// 归档日期
        /// </summary>
        public DateTime FilingDate
        {
            get
            {
                return _filingDate;
            }
            set
            {
                SetPropertyValue("FilingDate", ref _filingDate, value);
            }
        }
        #endregion

        #region 设备价格
        private string _price;
        /// <summary>
        /// 设备价格
        /// </summary>
        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                SetPropertyValue("Price", ref _price, value);
            }
        }
        #endregion

        #region 安装地点
        private string _location;
        /// <summary>
        /// 安装地点
        /// </summary>
        //[DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='3'")]
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                SetPropertyValue("Location", ref _location, value);
            }
        }
        #endregion

        #region 使用部门
        private Department _departmentUsed;
        /// <summary>
        /// 使用部门
        /// </summary>
        //[DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public Department DepartmentUsed
        {
            get
            {
                return _departmentUsed;
            }
            set
            {
                SetPropertyValue("DepartmentUsed", ref _departmentUsed, value);
            }
        }
        #endregion

        #region 设备负责人
        private Employee _custodian;
        /// <summary>
        /// 设备负责人
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public Employee Custodian
        {
            get
            {
                return _custodian;
            }
            set
            {
                SetPropertyValue("Custodian", ref _custodian, value);
            }
        }
        #endregion

        #region 有效期至
        private DateTime _expirationDate;
        /// <summary>
        /// 有效期至
        /// </summary>
        public DateTime ExpirationDate
        {
            get
            {
                return _expirationDate;
            }
            set
            {
                SetPropertyValue("ExpirationDate", ref _expirationDate, value);
            }
        }
        #endregion

        #region 溯源方式
        private KeyValue _trackingMode;
        /// <summary>
        /// 溯源方式
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='4'")]
        public KeyValue TrackingMode
        {
            get
            {
                return _trackingMode;
            }
            set
            {
                SetPropertyValue("TrackingMode", ref _trackingMode, value);
            }
        }
        #endregion

        #region 计量单位
        private string _unitOfMeasure;
        /// <summary>
        /// 计量单位
        /// </summary>
        [Size(32)]
        public string UnitOfMeasure
        {
            get
            {
                return _unitOfMeasure;
            }
            set
            {
                SetPropertyValue("UnitOfMeasure", ref _unitOfMeasure, value);
            }
        }
        #endregion

        #region 计量费用
        private string _fee;
        /// <summary>
        /// 计量费用
        /// </summary>
        public string Fee
        {
            get
            {
                return _fee;
            }
            set
            {
                SetPropertyValue("Fee", ref _fee, value);
            }
        }
        #endregion

        #region 设备状态
        private KeyValue _status;
        /// <summary>
        /// 设备状态
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='5'")]
        public KeyValue Status
        {
            get
            {
                return _status;
            }
            set
            {
                SetPropertyValue("Status", ref _status, value);
            }
        }
        #endregion

        #region 下次计量日期
        private DateTime _nextMeasureDate;
        /// <summary>
        /// 下次计量日期
        /// </summary>
        public DateTime NextMeasureDate
        {
            get
            {
                return _nextMeasureDate;
            }
            set
            {
                SetPropertyValue("NextMeasureDate", ref _nextMeasureDate, value);
            }
        }
        #endregion

        #region 下次核查日期
        private DateTime _nextCheckDate;
        /// <summary>
        /// 下次核查日期
        /// </summary>
        public DateTime NextCheckDate
        {
            get
            {
                return _nextCheckDate;
            }
            set
            {
                SetPropertyValue("NextCheckDate", ref _nextCheckDate, value);
            }
        }
        #endregion

        #region 下次保养日期
        private DateTime _nextMaintenanceDate;
        /// <summary>
        /// 下次保养日期
        /// </summary>
        public DateTime NextMaintenanceDate
        {
            get
            {
                return _nextMaintenanceDate;
            }
            set
            {
                SetPropertyValue("NextMaintenanceDate", ref _nextMaintenanceDate, value);
            }
        }
        #endregion

        #region 检定类别
        private KeyValue _certificateCategory;
        /// <summary>
        /// 检定类别
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='20'")]
        public KeyValue CertificateCategory
        {
            get
            {
                return _certificateCategory;
            }
            set
            {
                SetPropertyValue("CertificateCategory", ref _certificateCategory, value);
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

        #region 检定频率
        private string _frequency;
        /// <summary>
        /// 检定频率
        /// </summary>
        public string Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                SetPropertyValue("Frequency", ref _frequency, value);
            }
        }
        #endregion

        #region 签发日期
        private DateTime _dateIssued;
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime DateIssued
        {
            get
            {
                return _dateIssued;
            }
            set
            {
                SetPropertyValue("DateIssued", ref _dateIssued, value);
            }
        }
        #endregion

        #region 退役日期
        private bool _isValid;
        /// <summary>
        /// 退役日期
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                SetPropertyValue("IsValid", ref _isValid, value);
            }
        }
        #endregion

        #region 退役原因
        private string _retireReason;
        /// <summary>
        /// 退役原因
        /// </summary>
        [ModelDefault("RowCount", "1")]
        [Size(2048)]
        public string RetireReason
        {
            get
            {
                return _retireReason;
            }
            set
            {
                SetPropertyValue("RetireReason", ref _retireReason, value);
            }
        }
        #endregion

        #region 附件
        private FileData _attachment;
        /// <summary>
        /// 附件
        /// </summary>
        public FileData Attachment
        {
            get
            {
                return _attachment;
            }
            set
            {
                SetPropertyValue("Attachment", ref _attachment, value);
            }
        }
        #endregion

        #region 备注
        private string _remarks;
        /// <summary>
        /// 备注
        /// </summary>
        [Size(int.MaxValue)]
        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                SetPropertyValue("Remarks", ref _remarks, value);
            }
        }
        #endregion

        #region 业务类别(计量/质检)
        private BizCategory _bizCategory;
        /// <summary>
        /// 业务类别(计量/质检)
        /// </summary>
        public BizCategory BizCategory
        {
            get
            {
                return _bizCategory;
            }
            set
            {
                SetPropertyValue("BizCategory", ref _bizCategory, value);
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

        /// <summary>
        /// 设备证书
        /// </summary>
        [Association("Labware-Certificates")]
        public XPCollection<LabwareCertificate> Certificates
        {
            get { return GetCollection<LabwareCertificate>("Certificates"); }
        }

        /// <summary>
        /// 保养记录
        /// </summary>
        [Association("Labware-Maintenances")]
        public XPCollection<LabwareMaintenance> Maintenances
        {
            get { return GetCollection<LabwareMaintenance>("Maintenances"); }
        }
        /////// <summary>
        /////// 检测项目
        /////// </summary>
        //////[Association("Labwares-TestItems")]
        //////public XPCollection<MeasureTestItem> TestItems
        //////{
        //////    get { return GetCollection<MeasureTestItem>("TestItems"); }
        //////}

        [Association("Labware-Attachments")]
        public XPCollection<Attachment> Attachments
        {
            get { return GetCollection<Attachment>("Attachments"); }
        }

        //#region Instruments
        //[Association("PrepMethodLabware-Labware")]
        //[VisibleInDetailView(false)]
        //public XPCollection<PrepMethodLabware> PrepMethods
        //{
        //    get { return GetCollection<PrepMethodLabware>(nameof(PrepMethods)); }
        //}
        //#endregion
        [Association("TestMethod-Labware")]
        public XPCollection<TestMethod> TestMethods
        {
            get { return GetCollection<TestMethod>(nameof(TestMethods)); }
        }

        [Association("SamplePrepTestMethod-Labware")]
        public XPCollection<TestMethod> SamplePrepTestsinstruments
        {
            get { return GetCollection<TestMethod>(nameof(SamplePrepTestsinstruments)); }
        } 
        
        [Association("FieldInstrument-Labware")]
        public XPCollection<TestMethod> FieldTestsinstruments
        {
            get { return GetCollection<TestMethod>(nameof(FieldTestsinstruments)); }
        }

        [VisibleInDetailView(false)]
        [Association("SamplePrepBatchInstruments", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<SamplePrepBatch> SamplePrepBatchs
        {
            get { return GetCollection<SamplePrepBatch>(nameof(SamplePrepBatchs)); }
        }


        [VisibleInDetailView(false)]
        [Association("SamplePreparationChain-Labware")]
        public XPCollection<SamplePreparationChain> SamplePreparationChains
        {
            get { return GetCollection<SamplePreparationChain>(nameof(SamplePreparationChains)); }
        }

        [VisibleInDetailView(false)]
        [Association("AnalyticalBatchInstruments", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<SpreadSheetEntry_AnalyticalBatch> SpreadSheetEntry_AnalyticalBatchs
        {
            get { return GetCollection<SpreadSheetEntry_AnalyticalBatch>(nameof(SpreadSheetEntry_AnalyticalBatchs)); }
        }

        #region Active
        private bool _Active;
        public bool Active
        {
            get { return _Active; }
            set { SetPropertyValue(nameof(Active), ref _Active, value); }
        }
        #endregion

        #region MaintenanceFrequency
        private string _MaintenanceFrequency;
        public string MaintenanceFrequency
        {
            get { return _MaintenanceFrequency; }
            set { SetPropertyValue(nameof(MaintenanceFrequency), ref _MaintenanceFrequency, value); }
        }
        #endregion

        #region Department
        private string _Department;
        //[ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Department
        {
            get { return _Department; }
            set { SetPropertyValue("Department", ref _Department, value); }
        }
        public XPCollection<Department> Departments
        {
            get
            {
                return new XPCollection<Department>(Session, CriteriaOperator.Parse(""));
            }
        }
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> objdic = new Dictionary<object, string>();

            if (targetMemberName == "Department" && Departments != null && Departments.Count > 0)
            {
                foreach (Department objdpt in Departments.Where(i => i.Name != null).OrderBy(i => i.Name).ToList())
                {
                    if (!objdic.ContainsKey(objdpt.Oid))
                    {
                        objdic.Add(objdpt.Oid, objdpt.Name);
                    }
                }
            }
            return objdic;
        }
        #endregion


        #region Category
        private LabwareCategory _Category;
        [RuleRequiredField("labwarecat", DefaultContexts.Save,"'Category must not be empty'")]
        public LabwareCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue(nameof(Category), ref _Category, value); }
        }
        #endregion

        #region RetireDate
        private DateTime _RetiredDate;
        public DateTime RetiredDate
        {
            get
            {
                return _RetiredDate;
            }
            set { SetPropertyValue("RetiredDate", ref _RetiredDate, value); }
        }
        #endregion


        #region Container
        private string _Container;

        public event EventHandler ItemsChanged;

        [Size(SizeAttribute.Unlimited)]
        public string Container
        {
            get { return _Container; }
            set { SetPropertyValue(nameof(Container), ref _Container, value); }
        }
        #endregion
    }
}
