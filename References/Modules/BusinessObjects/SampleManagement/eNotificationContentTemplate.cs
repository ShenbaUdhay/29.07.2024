using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class eNotificationContentTemplate : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public eNotificationContentTemplate(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) ;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        #region Subject
        private string _Subject;
        public string Subject
        {
            get { return _Subject; }
            set { SetPropertyValue(nameof(Subject), ref _Subject, value); }
        }
        #endregion

        #region Body
        private string _Body;
        [Size(SizeAttribute.Unlimited)]
        //[Editor(RichTextMailMergeData)]
        public string Body
        {
            get { return _Body; }
            set { SetPropertyValue(nameof(Body), ref _Body, value); }
        }
        #endregion

        #region SampleCheckin
        private Samplecheckin _SampleCheckin;
        public Samplecheckin SampleCheckin
        {
            get { return _SampleCheckin; }
            set { SetPropertyValue(nameof(Samplecheckin), ref _SampleCheckin, value); }
        }
        #endregion

        #region Report
        private Reporting _Reporting;
        public Reporting Reporting
        {
            get { return _Reporting; }
            set { SetPropertyValue(nameof(Reporting), ref _Reporting, value); }
        }
        #endregion

        #region Invoice
        private Modules.BusinessObjects.Setting.Invoicing.Invoicing _Invoice;
        public Modules.BusinessObjects.Setting.Invoicing.Invoicing Invoice
        {
            get { return _Invoice; }
            set { SetPropertyValue(nameof(Invoice), ref _Invoice, value); }
        }
        #endregion

        #region ContentType
        private TypeofContent _ContentType;
        public TypeofContent ContentType
        {
            get { return _ContentType; }
            set { SetPropertyValue(nameof(ContentType), ref _ContentType, value); }
        }
        #endregion


        //#region Image
        //private byte[] _Image;
        //public byte[] Image
        //{
        //    get { return _Image; }
        //    set { SetPropertyValue(nameof(Image), ref _Image, value); }
        //}
        //#endregion

        //#region JobId
        //private string _JobId;
        //public string JobId
        //{
        //    get { return _JobId; }
        //    set { SetPropertyValue(nameof(JobId), ref _JobId, value); }
        //}
        //#endregion

        //#region ProjectId
        //private string _ProjectId;
        //public string ProjectId
        //{
        //    get { return _ProjectId; }
        //    set { SetPropertyValue(nameof(ProjectId), ref _ProjectId, value); }
        //}
        //#endregion

        //#region TAT
        //private string _TAT;
        //public string TAT
        //{
        //    get { return _TAT; }
        //    set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        //}
        //#endregion

        //#region ProjectName
        //private string _ProjectName;
        //public string ProjectName
        //{
        //    get { return _ProjectName; }
        //    set { SetPropertyValue(nameof(ProjectName), ref _ProjectName, value); }
        //}
        //#endregion

        //#region ReceivedDate
        //private DateTime _ReceivedDate;
        //public DateTime ReceivedDate
        //{
        //    get { return _ReceivedDate; }
        //    set { SetPropertyValue(nameof(ReceivedDate), ref _ReceivedDate, value); }
        //} 
        //#endregion

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
        private CustomSystemUser _CreatedBy;
        [Browsable(false)]
        public CustomSystemUser CreatedBy
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
        private CustomSystemUser _ModifiedBy;
        [Browsable(false)]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }

        #endregion



    }
}