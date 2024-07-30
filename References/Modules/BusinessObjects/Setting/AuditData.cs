using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class AuditData : XPBaseObject
    {
        public AuditData(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        [Key(AutoGenerate = true), Browsable(false)]
        public int Oid { get; set; }

        private Guid _Source;
        public Guid Source
        {
            get { return _Source; }
            set { SetPropertyValue("Source", ref _Source, value); }
        }

        private OperationType _OperationType;
        public OperationType OperationType
        {
            get { return _OperationType; }
            set { SetPropertyValue("OperationType", ref _OperationType, value); }
        }

        private string _FormName;
        public string FormName
        {
            get { return _FormName; }
            set { SetPropertyValue(nameof(FormName), ref _FormName, value); }
        }

        private string _ID;
        public string ID
        {
            get { return _ID; }
            set { SetPropertyValue(nameof(ID), ref _ID, value); }
        }

        private string _PropertyName;
        public string PropertyName
        {
            get { return _PropertyName; }
            set { SetPropertyValue(nameof(PropertyName), ref _PropertyName, value); }
        }

        private string _Oldvalue;
        [Size(SizeAttribute.Unlimited)]
        public string Oldvalue
        {
            get { return _Oldvalue; }
            set { SetPropertyValue(nameof(Oldvalue), ref _Oldvalue, value); }
        }

        private string _Newvalue;
        [Size(SizeAttribute.Unlimited)]
        public string Newvalue
        {
            get { return _Newvalue; }
            set { SetPropertyValue(nameof(Newvalue), ref _Newvalue, value); }
        }

        [NonPersistent]
        public bool IsComment
        {
            get { return !string.IsNullOrEmpty(Comment); }
        }

        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }

        private Employee _CreatedBy;
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }

        private DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }

        private bool _CommentProcessed;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool CommentProcessed
        {
            get { return _CommentProcessed; }
            set { SetPropertyValue(nameof(PropertyName), ref _CommentProcessed, value); }
        }
    }

    public enum OperationType
    {
        ValueChanged = 0,
        Rollback = 1,
        Deleted = 2,
        Created = 3,
    }
}
