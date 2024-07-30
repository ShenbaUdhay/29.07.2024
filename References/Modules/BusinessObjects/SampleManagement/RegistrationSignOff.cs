using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    [NonPersistent]
    [DomainComponent]
    public class RegistrationSignOff : BaseObject
    {
        public RegistrationSignOff(Session session)
           : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        //public string JobID { get; set; }
        //public string SampleID { get; set; }
        //public string SampleName { get; set; }
        //public string TestName { get; set; }
        //public string BottleID { get; set; }
        //public Employee ReceivedBy { get; set; }
        //public DateTime ReceivedDate { get; set; }
        //public Employee SignOffBy { get; set; }
        //public DateTime SignOffDate { get; set; }
        #region JobID
        private string _JobID;
        public string JobID
        {
            get { return _JobID; }
            set { SetPropertyValue("JobID", ref _JobID, value); }
        }
        #endregion
        #region SampleID
        private string _SampleID;
        public string SampleID
        {
            get { return _SampleID; }
            set { SetPropertyValue("SampleID", ref _SampleID, value); }
        }
        #endregion
        #region SampleName
        private string _SampleName;
        public string SampleName
        {
            get { return _SampleName; }
            set { SetPropertyValue("SampleName", ref _SampleName, value); }
        }
        #endregion
        #region TestName
        private string _TestName;
        public string TestName
        {
            get { return _TestName; }
            set { SetPropertyValue("TestName", ref _TestName, value); }
        }
        #endregion
        #region BottleID
        private string _BottleID;
        public string BottleID
        {
            get { return _BottleID; }
            set { SetPropertyValue("BottleID", ref _BottleID, value); }
        }
        #endregion
        #region ReceivedBy
        private string _ReceivedBy;
        public string ReceivedBy
        {
            get { return _ReceivedBy; }
            set { SetPropertyValue("ReceivedBy", ref _ReceivedBy, value); }
        }
        #endregion
        #region ReceivedDate
        private DateTime _ReceivedDate;
        public DateTime ReceivedDate
        {
            get { return _ReceivedDate; }
            set { SetPropertyValue("ReceivedDate", ref _ReceivedDate, value); }
        }
        #endregion
        #region SignOffBy
        private string _SignOffBy;
        public string SignOffBy
        {
            get { return _SignOffBy; }
            set { SetPropertyValue("SignOffBy", ref _SignOffBy, value); }
        }
        #endregion
        #region SignOffDate
        private Nullable<DateTime> _SignOffDate;
        public Nullable<DateTime> SignOffDate
        {
            get { return _SignOffDate; }
            set { SetPropertyValue("SignOffDate", ref _SignOffDate, value); }
        }
        #endregion
        #region SampleLogin
        private SampleLogIn _SampleLogin;
        public SampleLogIn SampleLogin
        {
            get { return _SampleLogin; }
            set { SetPropertyValue("SampleLogin", ref _SampleLogin, value); }
        }
        #endregion
        #region TestMethod
        private string _TestMethod;
        public string TestMethod
        {
            get { return _TestMethod; }
            set { SetPropertyValue("TestMethod", ref _TestMethod, value); }
        }
        #endregion
    }
}