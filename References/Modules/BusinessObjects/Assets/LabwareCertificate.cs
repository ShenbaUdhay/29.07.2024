// ================================================================================
// Table Name: [LabwareCertificate]
// Author: Sunny
// Date: 2016��12��13��
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption���豸֤�飬�����豸��֤�飬�����豸Ҳ��Ҫ���ڼ��
// ================================================================================
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Seting;
using System;

namespace Modules.BusinessObjects.Assets
{
    /// <summary>
    /// ��[LabwareCertificate]��ʵ����
    /// </summary>
    [DefaultClassOptions]
    [XafDisplayName("Instrument Certificate")]
    public class LabwareCertificate : BaseObject
    {

        /// <summary>
        /// ��ʼ���� LabwareCertificate ����ʵ����
        /// </summary>
        public LabwareCertificate(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreateTime = Library.GetServerTime(Session);
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
        }

        #region �����豸
        private Labware _labware;
        /// <summary>
        /// �����豸
        /// </summary>
        [Association("Labware-Certificates")]
        [RuleRequiredField("Labware", DefaultContexts.Save, "'Category must not be empty'")]
        [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public Labware Labware
        {
            get
            {
                return _labware;
            }
            set
            {
                SetPropertyValue("Labware", ref _labware, value);
            }
        }
        #endregion

        #region ֤����
        private string _certificateNumber;
        /// <summary>
        /// ֤����
        /// </summary>
        [Size(64)]
        [RuleUniqueValue]
        //[RuleRequiredField("LabwareCertificate.CertificateNumber", DefaultContexts.Save)]
        public string CertificateNumber
        {
            get
            {
                return _certificateNumber;
            }
            set
            {
                SetPropertyValue("CertificateNumber", ref _certificateNumber, value);
            }
        }
        #endregion

        #region ֤������
        private string _certificateName;
        /// <summary>
        /// ֤������
        /// </summary>
        [Size(128)]
        //[RuleRequiredField("LabwareCertificate.CertificateName", DefaultContexts.Save)]
        public string CertificateName
        {
            get
            {
                return _certificateName;
            }
            set
            {
                SetPropertyValue("CertificateName", ref _certificateName, value);
            }
        }
        #endregion

        #region �춨���
        private KeyValue _certificateCategory;
        /// <summary>
        /// �춨���
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='20'")]
        public KeyValue CertificateCategory
        {
            get
            {
                return _certificateCategory;
            }
            set
            {
                SetPropertyValue("CertificateCategory", ref _certificateCategory, value);
            }
        }
        #endregion

        #region ��Ч����
        private DateTime _dateIssued;
        /// <summary>
        /// ��Ч����
        /// </summary>
        public DateTime DateIssued
        {
            get
            {
                return _dateIssued;
            }
            set
            {
                SetPropertyValue("DateIssued", ref _dateIssued, value);
            }
        }
        #endregion

        #region ��Ч��ֹ
        private DateTime _expirationDate;
        /// <summary>
        /// ��Ч��ֹ
        /// </summary>
        public DateTime ExpirationDate
        {
            get
            {
                return _expirationDate;
            }
            set
            {
                SetPropertyValue("ExpirationDate", ref _expirationDate, value);
            }
        }
        #endregion

        #region ������
        private CustomSystemUser _createdBy;
        /// <summary>
        /// ������
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser CreatedBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _createdBy, value);
            }
        }
        #endregion

        #region ����ʱ��
        private DateTime _createTime;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
            set
            {
                SetPropertyValue("CreateTime", ref _createTime, value);
            }
        }
        #endregion

        #region �޸���
        private CustomSystemUser _updatedBy;
        /// <summary>
        /// �޸���
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser UpdatedBy
        {
            get
            {
                return _updatedBy;
            }
            set
            {
                SetPropertyValue("UpdatedBy", ref _updatedBy, value);
            }
        }
        #endregion

        #region �޸�ʱ��
        private DateTime _updateTime;
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime UpdateTime
        {
            get
            {
                return _updateTime;
            }
            set
            {
                SetPropertyValue("UpdateTime", ref _updateTime, value);
            }
        }
        #endregion

        #region Title
        private string _Title;
        [RuleRequiredField]
        [RuleUniqueValue]
        public string Title
        {
            get { return _Title; }
            set { SetPropertyValue(nameof(Title), ref _Title, value); }
        }
        #endregion

        #region ID
        private string _ID;
        public string ID
        {
            get { return _ID; }
            set { SetPropertyValue(nameof(ID), ref _ID, value); }
        }
        #endregion

        #region Number
        private string _Number;
        public string Number
        {
            get { return _Number; }
            set { SetPropertyValue(nameof(Number), ref _Number, value); }
        }
        #endregion

        #region IssuingAgency
        private string _IssuingAgency;
        public string IssuingAgency
        {
            get { return _IssuingAgency; }
            set { SetPropertyValue(nameof(IssuingAgency), ref _IssuingAgency, value); }
        }
        #endregion

        #region Attachment
        private FileData _Attachment;
        public FileData Attachment
        {
            get { return _Attachment; }
            set { SetPropertyValue(nameof(Attachment), ref _Attachment, value); }
        }
        #endregion

        #region Active
        private bool _Active;
        public bool Active
        {
            get { return _Active; }
            set { SetPropertyValue(nameof(Active), ref _Active, value); }
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

        //#region LabwareID
        //private Labware _LabwareID;
        //public Labware LabwareID
        //{
        //    get { return _LabwareID; }
        //    set { SetPropertyValue(nameof(LabwareID), ref _LabwareID, value); }
        //}
        //#endregion

    }
}
