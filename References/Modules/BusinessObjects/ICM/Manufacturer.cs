using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.ICM
{
    [RuleCombinationOfPropertiesIsUnique("UniqueManufacturer", DefaultContexts.Save, "ManufacturerName;", "Manufacturer with the same name already exists.", SkipNullOrEmptyValues = false)]
    [DefaultClassOptions]
    [DefaultProperty("ManufacturerName")]

    public class Manufacturer : BaseObject
    {
        #region Consturcotr
        public Manufacturer(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }
        #endregion

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

        #region ManufacturerName
        private string fManufacturerName;
        [RuleRequiredField("Manufacturer", DefaultContexts.Save, "'Manufacturer must not be empty'")]
        // [RuleStringComparison("RuleStringComparison_Manufactures_NotNullOrEmpty", DefaultContexts.Save, StringComparisonType.NotEquals, null)]
        //[RuleUniqueValue]
        public string ManufacturerName
        {
            get { return fManufacturerName; }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetPropertyValue("Manufacturer", ref fManufacturerName, value.Trim());
            }
        }
        #endregion

        //#region ManufacturerAddress
        //private string _Address;
        //[Size(1024)]
        //public string ManufacturerAddress
        //{
        //    get
        //    {
        //        return _Address;
        //    }
        //    set
        //    {
        //        SetPropertyValue(nameof(ManufacturerAddress), ref _Address, value);
        //    }
        //}
        //#endregion

        #region Description
        string _Description;
        [Size(1000)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue<string>("Description", ref _Description, value); }
        }
        #endregion

        //#region Comment
        //string _Comment;
        //[Size(1000)]
        //public string Comment
        //{
        //    get { return _Comment; }
        //    set { SetPropertyValue<string>("Comment", ref _Comment, value); }
        //}
        //#endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }
        }

        #endregion

    }
}