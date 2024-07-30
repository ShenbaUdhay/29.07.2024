using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class DataEntry : BaseObject
    {
        public DataEntry(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedBy = Session.GetObjectByKey<PermissionPolicyUser>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<PermissionPolicyUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
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
                    }
                }
            }
        }

        #region BOSourceCodeCaption
        private UnconventionalAnalysisDataEntryConfigBOSourceCodeCaption _BOSourceCodeCaption;
        public UnconventionalAnalysisDataEntryConfigBOSourceCodeCaption BOSourceCodeCaption
        {
            get { return _BOSourceCodeCaption; }
            set { SetPropertyValue(nameof(BOSourceCodeCaption), ref _BOSourceCodeCaption, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [Browsable(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private PermissionPolicyUser _CreatedBy;
        [Browsable(false)]
        public PermissionPolicyUser CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [Browsable(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private PermissionPolicyUser _ModifiedBy;
        [Browsable(false)]
        public PermissionPolicyUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #endregion       

        #region TestMethod
        private string _Test;
        [Size(SizeAttribute.Unlimited)]
        public string Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        #endregion

        #region TestNameoid
        private string _TestNameOid;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [Browsable(false)]
        public string TestNameOid
        {
            get { return _TestNameOid; }
            set { SetPropertyValue(nameof(TestNameOid), ref _TestNameOid, value); }
        }
        #endregion
    }
}