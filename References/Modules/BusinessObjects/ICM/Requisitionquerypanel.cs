using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using System;

namespace Modules.BusinessObjects.ICM
{
    [NonPersistent]
    public class Requisitionquerypanel : BaseObject
    {
        #region Constructor
        public Requisitionquerypanel(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region RequestedDateFrom
        private DateTime? _RequestedDateFrom;
        public DateTime? RequestedDateFrom
        {
            get { return _RequestedDateFrom; }
            set
            {
                SetPropertyValue<DateTime?>("RequestedDateFrom", ref _RequestedDateFrom, value);
            }
        }
        #endregion

        #region RequestedDateTo
        private DateTime? _RequestedDateTo;
        public DateTime? RequestedDateTo
        {
            get { return _RequestedDateTo; }
            set
            {
                SetPropertyValue<DateTime?>("RequestedDateTo", ref _RequestedDateTo, value);
            }
        }
        #endregion

        #region specification
        private Requisition _Specification;
        public Requisition Specification
        {
            get { return _Specification; }
            set
            {
                SetPropertyValue<Requisition>("Specification", ref _Specification, value);
            }
        }
        #endregion

        #region itemname
        private Requisition _ItemName;
        public Requisition ItemName
        {
            get { return _ItemName; }
            set
            {
                SetPropertyValue<Requisition>("ItemName", ref _ItemName, value);
            }
        }
        #endregion

        #region category
        private Requisition _Category;
        public Requisition category
        {
            get { return _Category; }
            set
            {
                SetPropertyValue<Requisition>("category", ref _Category, value);
            }
        }
        #endregion

        #region requisitionnumber
        private Requisition _Requisitionnumber;
        public Requisition Requisitionnumber
        {
            get { return _Requisitionnumber; }
            set
            {
                SetPropertyValue<Requisition>("Requisitionnumber", ref _Requisitionnumber, value);
            }
        }
        #endregion

        #region requestedby
        private Requisition _Requestedby;
        public Requisition Requestedby
        {
            get { return _Requestedby; }
            set
            {
                SetPropertyValue<Requisition>("Requestedby", ref _Requestedby, value);
            }
        }
        #endregion

        #region resetall
        private Boolean _ResetAll;
        public Boolean ResetAll
        {
            get { return _ResetAll; }
            set
            {
                SetPropertyValue<Boolean>("ResetAll", ref _ResetAll, value);
            }
        }
        #endregion

    }
}