using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using System;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    public class ExistingStock : BaseObject
    {
        public ExistingStock(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            //Qty = 1;
            base.AfterConstruction();
        }

        #region itemname
        private Items fItemname;
        [ImmediatePostData]
        [RuleRequiredField("Itemname", DefaultContexts.Save)]
        public Items Itemname
        {
            get { return fItemname; }
            set { SetPropertyValue("Itemname", ref fItemname, value); }
        }
        #endregion

        #region vendor
        private Vendors fVendor;
        //[RuleRequiredField("fVendor", DefaultContexts.Save)]
        public Vendors Vendor
        {
            get { return fVendor; }
            set { SetPropertyValue("Vendor", ref fVendor, value); }
        }
        #endregion

        #region qty
        private int fQty;
        public int Qty
        {
            get { return fQty; }
            set { SetPropertyValue<int>("Qty", ref fQty, value); }
        }
        #endregion

        #region expirydate
        private DateTime fExpiryDate;
        public DateTime ExpiryDate
        {
            get { return fExpiryDate; }
            set { SetPropertyValue<DateTime>("ExpiryDate", ref fExpiryDate, value); }
        }
        #endregion

        #region givento
        private Hr.Employee fgivento;
        public Hr.Employee givento
        {
            get { return fgivento; }
            set { SetPropertyValue("givento", ref fgivento, value); }
        }
        #endregion

        #region storage
        private ICMStorage fStorage;
        public ICMStorage Storage
        {
            get { return fStorage; }
            set { SetPropertyValue("Storage", ref fStorage, value); }
        }
        #endregion

        #region esid
        private string fESID;
        public string ESID
        {
            get { return fESID; }
            set { SetPropertyValue<string>("ESID", ref fESID, value); }
        }
        #endregion

        #region Errorlog
        [NonPersistent]
        private string fErrorlog;
        public string Errorlog
        {
            get { return fErrorlog; }
            set { SetPropertyValue<string>("Errorlog", ref fErrorlog, value); }
        }
        #endregion
    }
}