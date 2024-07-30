using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace ICM.Module.BusinessObjects
{
    public class Vendorsupload : FileAttachmentBase
    {
        #region Constructor
        public Vendorsupload(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region refrence
        protected Vendors refrence;
        [Persistent, Association("Vendors-VendorsFileData")]
        public Vendors Refrence
        {
            get { return refrence; }
            set
            {
                SetPropertyValue("Refrence", ref refrence, value);
            }
        }
        #endregion
    }
}