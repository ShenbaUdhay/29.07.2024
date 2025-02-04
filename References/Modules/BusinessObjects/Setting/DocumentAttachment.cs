﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class DocumentAttachment : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        DocumentAttachmentInfo objDocInfo = new DocumentAttachmentInfo();
        public DocumentAttachment(Session session)
            : base(session)
        {

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Manual = Session.GetObjectByKey<Manual>(objDocInfo.DocumentIDOid);

            if (objDocInfo.rowCount > -1)
            {
                RevNumber = "Rev" + (objDocInfo.rowCount + 1);
            }
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
        protected override void OnSaving()
        {
            base.OnSaving();

            if (IsActive && Manual != null)
            {
                Manual.Attachments.Where(i => i.IsActive && i.Oid != Oid).ToList().ForEach(j => j.IsActive = false);
            }

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            Date = DateTime.Now;
            Author = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }

        private string _Name;
        //[RuleRequiredField]
        public string Name
        {
            get
            {
                return _Name;
            }
            set { SetPropertyValue("Name", ref _Name, value); }
        }
        private FileData _Attachment;
        [RuleRequiredField]
        public virtual FileData Attachment
        {
            get { return _Attachment; }
            set { SetPropertyValue<FileData>("Attachment", ref _Attachment, value); }
        }

        private string _RevNumber;
        [ModelDefault("AllowEdit", "False")]
        public string RevNumber
        {
            get { return _RevNumber; }
            set { SetPropertyValue("RevNumber", ref _RevNumber, value); }
        }
        private string _Comment;
        [Size(1000)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue("Date", ref _Date, value); }
        }

        private Employee _Author;
        public Employee Author
        {
            get { return _Author; }
            set { SetPropertyValue(nameof(Author), ref _Author, value); }
        }

        private bool _IsActive;
        [ImmediatePostData]
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                SetPropertyValue("IsActive", ref _IsActive, value);
                if (IsActive && Manual != null)
                {
                    Manual.Attachments.Where(i => i.IsActive && i.Oid != Oid).ToList().ForEach(j => j.IsActive = false);
                }
            }
        }

        private Manual _Manual;
        [VisibleInDetailView(false), VisibleInListView(false)]
        [DevExpress.Xpo.Association("Manual-Attachment")]
        public Manual Manual
        {
            get { return _Manual; }
            set { SetPropertyValue("Manual", ref _Manual, value); }
        }

        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }


        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]

        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }


        private DateTime _ModifiedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }

        private Employee _ModifiedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }
        }
    }
}