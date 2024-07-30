using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class RoleNavigationPermission : BaseObject
    {
        public RoleNavigationPermission(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (Sort == 0)
            {
                Sort = Convert.ToInt32(Session.Evaluate(typeof(RoleNavigationPermission), CriteriaOperator.Parse("MAX(Sort)"), null)) + 1;
            }
            if (Session.IsNewObject(this) && AdministrativePrivilege == AdministrativePrivilege.ClientAdministrator && RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationModelClass == typeof(Employee).ToString() && i.NavigationItem.NavigationView.EndsWith("ListView")) == null)
            {
                RoleNavigationPermissionDetails empDetails = new RoleNavigationPermissionDetails(Session);
                empDetails.NavigationItem = Session.FindObject<NavigationItem>(CriteriaOperator.Parse("[NavigationModelClass] = ? AND EndsWith([NavigationView], 'ListView')", typeof(Employee).ToString()));
                empDetails.RoleNavigationPermission = this;
                empDetails.Navigate = true;
                empDetails.Read = true;
                empDetails.Create = true;
                empDetails.Write = true;
                empDetails.Delete = true;
            }
            if (AdministrativePrivilege == AdministrativePrivilege.SystemSupplierAdministrator)
            {
                IsAdministrative = true;
            }
        }

        #region OnDelete
        protected override void OnDeleting()
        {
            base.OnDeleting();
            System.Collections.ICollection lstReferenceObjects = Session.CollectReferencingObjects(this);
            if (lstReferenceObjects.Count > 0)
            {
                foreach (var obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.GetType() != typeof(RoleNavigationPermissionDetails))
                    {
                        Exception ex = new Exception("Already used can't allow to delete");
                        throw ex;
                    }
                }
            }
            else
            {
                Employee objEmployee = Session.FindObject<Employee>(CriteriaOperator.Parse("[RolePermissions][[Oid] = ?]", Oid));
                if (objEmployee != null)
                {
                    Exception ex = new Exception("Already Used Can't allow to Delete");
                    throw ex;
                }
            }
        }
        #endregion

        #region RoleName
        private string _RoleName;
        [RuleRequiredField("RoleNavigationPermission_RuleRequiredField", DefaultContexts.Save,"'Name' must not be empty")]
        [RuleUniqueValue("RoleNavigationPermission_RuleUniqueValue", DefaultContexts.Save, "The role with the entered Name was already registered within the system")]
        public string RoleName
        {
            get
            {
                return _RoleName;
            }
            set
            {
                SetPropertyValue<string>(nameof(RoleName), ref _RoleName, value);
            }
        }
        #endregion

        #region IsAdministrative
        private bool _IsAdministrative;
        public bool IsAdministrative
        {
            get
            {
                return _IsAdministrative;
            }
            set
            {
                SetPropertyValue<bool>(nameof(IsAdministrative), ref _IsAdministrative, value);
            }
        }
        #endregion

        //public bool CanEditModel { get; set; }

        #region PermissionPolicy
        private SecurityPermissionPolicy _PermissionPolicy;
        [VisibleInListView(false)]
        public SecurityPermissionPolicy PermissionPolicy
        {
            get
            {
                return _PermissionPolicy;
            }
            set
            {
                SetPropertyValue<SecurityPermissionPolicy>(nameof(PermissionPolicy), ref _PermissionPolicy, value);
            }
        }
        #endregion

        #region RoleNavigationPermissionDetails
        //[Association, Browsable(false)]
        [Association]
        public IList<RoleNavigationPermissionDetails> RoleNavigationPermissionDetails
        {
            get
            {
                return GetList<RoleNavigationPermissionDetails>("RoleNavigationPermissionDetails");
            }
        }
        #endregion

        #region Sort
        private int _Sort;
        public int Sort
        {
            get
            {
                return _Sort;
            }
            set
            {
                SetPropertyValue<int>(nameof(Sort), ref _Sort, value);
            }
        }
        #endregion

        #region Employees
        [VisibleInDetailView(false)]
        [Association("EmployeeRoleNavigationPermission", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Employee> Employees
        {
            get
            {
                return GetCollection<Employee>(nameof(Employees));
            }
        }
        #endregion

        #region AdministrativePrivilege
        private AdministrativePrivilege _AdministrativePrivilege;
        [ImmediatePostData]
        public AdministrativePrivilege AdministrativePrivilege
        {
            get
            {
                return _AdministrativePrivilege;
            }
            set
            {
                SetPropertyValue(nameof(AdministrativePrivilege), ref _AdministrativePrivilege, value);
                if (value == AdministrativePrivilege.SystemSupplierAdministrator)
                {
                    IsAdministrative = true;
                }
                else
                {
                    IsAdministrative = false;
                }
            }
        }
        #endregion

    }
}