// ================================================================================
// Table Name: [Department]
// Author: Sunny
// Date: 2016年12月13日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：部门
// ================================================================================
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Hr
{
    /// <summary>
    /// 表[Department]的实体类
    /// </summary>
    [DefaultClassOptions]

    public class Department : BaseObject, IHCategory
    {

        /// <summary>
        /// 初始化类 Department 的新实例。
        /// </summary>
        /// 
        #region Constructor
        public Department(Session session) : base(session) { }
        #endregion

        #region Events
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreateTime = Library.GetServerTime(Session);
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;
            //排序
            var maxSort = Session.Evaluate<Department>(CriteriaOperator.Parse("MAX(Sort)"), null);
            int sort = 1;
            if (maxSort != null)
            {
                sort = int.Parse(maxSort.ToString()) + 1;
            }
            Sort = sort;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
        }
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

        #region 公司
        private Company _company;
        /// <summary>
        /// 公司
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Company-Departments")]
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

        #region 部门编码
        private string _departmentCode;
        /// <summary>
        /// 部门编码
        /// </summary>
        [Size(32)]
        //[RuleRequiredField("Department.DepartmentCode", DefaultContexts.Save)]
        [Browsable(false)]
        public string DepartmentCode
        {
            get
            {
                return _departmentCode;
            }
            set
            {
                SetPropertyValue("DepartmentCode", ref _departmentCode, value);
            }
        }
        #endregion

        #region 部门名称
        private string _departmentName;
        /// <summary>
        /// 部门名称
        /// </summary>
        [Size(128)]
        [RuleUniqueValue]
        [RuleRequiredField("Department.Name", DefaultContexts.Save, "Department must not be empty")]
        public string Name
        {
            get
            {
                return _departmentName;
            }
            set
            {
                SetPropertyValue("Name", ref _departmentName, value);
            }
        }
        #endregion

        #region 上层组织
        private Department _parent;
        /// <summary>
        /// 上层组织
        /// </summary>
        [Persistent, Association("HCategoryParent-HCategoryChild")]
        //  [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public Department Parent
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

        #region DepartmentEmployee
        [Association("Department-Employees")]
        public XPCollection<Employee> Employees
        {
            get { return GetCollection<Employee>("Employees"); }
        }
        #endregion

        #region 接口
        [NonPersistent]
        [RuleFromBoolProperty("DepartmentCircularReferences", DefaultContexts.Save, "Circular refrerence detected. To correct this error, set the Parent property to another value.", UsedProperties = "Parent")]
        [Browsable(false)]
        public bool IsValid
        {
            get
            {
                Department currentObj = Parent;
                while (currentObj != null)
                {
                    if (currentObj == this)
                    {
                        return false;
                    }
                    currentObj = currentObj.Parent;
                }
                return true;
            }
        }

        [Association("HCategoryParent-HCategoryChild")]
        [ModelDefault("Caption", "子部门")]
        public XPCollection<Department> Children
        {
            get { return GetCollection<Department>("Children"); }
        }
        IBindingList ITreeNode.Children
        {
            get { return Children as IBindingList; }
        }
        ITreeNode IHCategory.Parent
        {
            get { return Parent as IHCategory; }
            set { Parent = value as Department; }
        }
        ITreeNode ITreeNode.Parent
        {
            get { return Parent as ITreeNode; }
        }
        #endregion
    }
}
