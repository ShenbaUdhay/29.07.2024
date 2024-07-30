using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    public class ItemsUpload : FileAttachmentBase
    {
        #region Constructor
        public ItemsUpload(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region Refrence
        protected Items refrence;
        [Persistent, Association("Items-ItemsFileData")]
        public Items Refrence
        {
            get { return refrence; }
            set
            {
                SetPropertyValue("Refrence", ref refrence, value);
            }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser ModifiedBy
        {
            get
            {
                return fModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref fModifiedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime fModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get
            {
                return fModifiedDate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref fModifiedDate, value);
            }
        }
        #endregion      
    }
}