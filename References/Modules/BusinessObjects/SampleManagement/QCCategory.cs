using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SampleManagement
{
    public class QCCategory : BaseObject
    {
        public QCCategory(Session session)
          : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            //CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            //ModifiedDate = DateTime.Now;
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            //ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }

        #region QCCategoryName
        private string _QCCategoryName;
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The QC Category already saved.")]
        [RuleRequiredField("QCCategoryName", DefaultContexts.Save, "QC Category must not be empty")]
        public string QCCategoryName
        {
            get { return _QCCategoryName; }
            set { SetPropertyValue(nameof(QCCategoryName), ref _QCCategoryName, value); }
        }
        #endregion
        //#region Description
        //private string _Description;
        //[Size(SizeAttribute.Unlimited)]
        //public string Description
        //{
        //    get { return _Description; }
        //    set { SetPropertyValue(nameof(Description), ref _Description, value); }
        //}
        //#endregion

        //#region CreatedDate
        //private DateTime _CreatedDate;
        //[ValueConverter(typeof(UtcDateTimeConverter))]
        //public DateTime CreatedDate
        //{
        //    get
        //    {
        //        return _CreatedDate;
        //    }
        //    set
        //    {
        //        SetPropertyValue<DateTime>(nameof(CreatedDate), ref _CreatedDate, value);
        //    }
        //}

        //#endregion

        //#region CreatedBy
        //private Employee _CreatedBy;
        //public Employee CreatedBy
        //{
        //    get
        //    {
        //        return _CreatedBy;
        //    }
        //    set
        //    {
        //        SetPropertyValue<Employee>(nameof(CreatedBy), ref _CreatedBy, value);
        //    }
        //}
        //#endregion

        //#region ModifiedDate
        //private DateTime _ModifiedDate;
        //[ValueConverter(typeof(UtcDateTimeConverter))]
        //public DateTime ModifiedDate
        //{
        //    get
        //    {
        //        return _ModifiedDate;
        //    }
        //    set
        //    {
        //        SetPropertyValue<DateTime>(nameof(ModifiedDate), ref _ModifiedDate, value);
        //    }
        //}
        //#endregion

        //#region ModifiedBy
        //private Employee _ModifiedBy;
        //public Employee ModifiedBy
        //{
        //    get
        //    {
        //        return _ModifiedBy;
        //    }
        //    set
        //    {
        //        SetPropertyValue<Employee>(nameof(ModifiedBy), ref _ModifiedBy, value);
        //    }
        //}
        //#endregion

    }
}
