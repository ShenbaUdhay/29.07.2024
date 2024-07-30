using DevExpress.ExpressApp.DC;

namespace Modules.BusinessObjects.Hr
{
    #region Quotes
    public enum ChargeType
    {
        Test,
        Parameter
    }
    #endregion

    #region Language
    /// <summary>
    /// 系统语言
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// 中文
        /// </summary>
        Chinese = 0,
        /// <summary>
        /// 英文
        /// </summary>
        English = 1
    }
    #endregion

    #region ContactCategory
    /// <summary>
    /// 员工子表联系信息中的类别
    /// </summary>
    public enum ContactCategory
    {
        /// <summary>
        /// 办公
        /// </summary>
        Office = 0,
        /// <summary>
        /// 个人
        /// </summary>
        Home = 1
    }
    #endregion

    #region MessageCategory
    /// <summary>
    /// 通知公告表中的消息类别
    /// </summary>
    public enum MessageCategory
    {
        /// <summary>
        /// 通知公告
        /// </summary>
        Inform = 0,
        /// <summary>
        /// 业务
        /// </summary>
        Business = 1

    }
    #endregion

    #region Gender
    /// <summary>
    /// Employee-Gender
    /// 员工表-性别
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 男
        /// </summary>
        Male = 1,
        /// <summary>
        /// 女
        /// </summary>
        Female = 2
    }
    #endregion

    #region OperationCategory
    public enum OperationCategory
    {
        Flow = 0,
        Operation = 1,
        Error = 2
    }
    #endregion

    //#region QCRoleCN
    //public enum QCRoleCN
    //{
    //    空白 = 1,
    //    平行 = 2,
    //    加标 = 3,
    //    标准 = 4

    //}
    //#endregion

    //#region QCRoleEN
    //public enum QCRoleEN
    //{
    //    Blank = 1,
    //    Duplicate = 2,
    //    Spike = 3,
    //    Standard = 4
    //}
    //#endregion

    //#region NavigationItem
    //public enum NavigationItem
    //{
    //    MyDesktop = 0,
    //    DataExplorer = 1
    //}
    //#endregion

    #region QueryPanel RadioButtons
    public enum FilterByMonthEN
    {
        _1M = 1,
        _3M = 2,
        _6M = 3,
        _1Y = 4,
        All = 5
    }
    #endregion

    #region Distribution QPanel
    public enum ENMode
    {
        Enter,
        View
    }
    #endregion

    #region Priority
    public enum Priority
    {
        Custom,
        Regular,
        Rush,
        Urgent
    }
    #endregion

    #region Pricetype
    public enum Pricetype
    {
        PerTest,
        PerParameter
    }
    #endregion
    #region Sample
    public enum Samplestatus
    {
        PendingEntry,
        [XafDisplayName("Pending Level 2 Data Review")]
        PendingValidation,
        //[XafDisplayName("Pending Level 3 Data Review")]
        [XafDisplayName("Pending Approval")]
        PendingApproval,
        PendingReporting,
        PendingReportValidation,
        PendingReportApproval,
        Approved,
        PendingReview,
        PendingVerify,
        ReportApproved,
        Reported,
        [XafDisplayName("Level 2 Subout Data Review")]
        SuboutPendingValidation,
        [XafDisplayName("Level 3 Subout Data Review")]
        SuboutPendingApproval
    }
    #endregion

    #region SampleRegistrationSignoffStatus
    public enum SampleRegistrationSignoffStatus
    {
        [XafDisplayName("Pending Submission")]
        PendingSubmit,
        PendingSigningOff,
        PartiallySignedOff,
        Signedoff,
        Submitted
    }
    #endregion
    #region SampleReceiptNotificationStatus
    public enum SampleReceiptNotificationStatus
    {
        InQueue,
        Sent,
        Failed
    }
    #endregion
    #region InvoiceStatus
    public enum InvoiceStatus
    {
        [XafDisplayName("Pending Invoicing")]
        PendingInvoicing
    }
    #endregion
    #region Samplereceiptsendingmethod
    public enum Samplereceiptsendingmethod
    {
        Email,
        Other
    }
    #endregion

    #region ContentType
    public enum TypeofContent
    {
        None,
        SampleCheckin,
        Report,
        Invoice
    }
    #endregion

    public enum ReportStatus
    {
        [XafDisplayName("Pending Level 2 Review")]
        Pending1stReview,
        [XafDisplayName("Pending Level 3 Review")]
        Pending2ndReview,
        PendingPrint,
        PendingDelivery,
        PendingArchive,
        Archived,
        Recalled,
        Rollbacked,
        ReportDelivered
    }

    public enum DeliveryMethod
    {
        Service,
        Delivery,
        Pickup
    }

    public enum RecalledMethod
    {
        Pickup,
        Dropoff
    }

    public enum Status
    {
        PendingSubmit,
        PendingValidation,
        PendingImport,
        Imported,
        Hold,
        Cancel
    }

    #region QC
    public enum QCstatus
    {
        PendingQC,
        QCApplied
    }
    #endregion

    public enum AdministrativePrivilege
    {
        None,
        SystemSupplierAdministrator,
        ClientAdministrator
    }
    public enum ProductClassification
    {

        [XafDisplayName("N/A")]
        None,
        [XafDisplayName("DG")]
        DG,
        [XafDisplayName("Non-DG")]
        NonDG

    }
    public enum Class
    {
        A,
        B,
        C,
        D,
        E
    }
    public enum ScientificDataTypes
    {
        [XafDisplayName("")]
        None,
        [XafDisplayName("bigint")]
        bigint,
        [XafDisplayName("numeric")]
        numeric,
        [XafDisplayName("bit")]
        bit,
        [XafDisplayName("smallint")]
        smallint,
        [XafDisplayName("decimal")]
        decimaltype,
        [XafDisplayName("smallmoney")]
        smallmoney,
        [XafDisplayName("int")]
        inttype,
        [XafDisplayName("tinyint")]
        tinyint,
        [XafDisplayName("money")]
        money,
        [XafDisplayName("float")]
        floattype,
        [XafDisplayName("real")]
        real,
        [XafDisplayName("date")]
        date,
        [XafDisplayName("datetime2")]
        datetime2,
        [XafDisplayName("datetime")]
        datetime,
        [XafDisplayName("datetimeoffset")]
        datetimeoffset,
        [XafDisplayName("smalldatetime")]
        smalldatetime,
        [XafDisplayName("time")]
        time,
        [XafDisplayName("char")]
        chartype,
        [XafDisplayName("varchar(10)")]
        varchar10,
        [XafDisplayName("varchar(50)")]
        varchar50,
        [XafDisplayName("varchar(100)")]
        varchar100,
        [XafDisplayName("text")]
        text,
        [XafDisplayName("nchar")]
        nchar,
        [XafDisplayName("nvarchar(10)")]
        nvarchar10,
        [XafDisplayName("nvarchar(50)")]
        nvarchar50,
        [XafDisplayName("nvarchar(100)")]
        nvarchar100,
        [XafDisplayName("ntext")]
        ntext,
        [XafDisplayName("binary")]
        binary,
        [XafDisplayName("varbinary")]
        varbinary,
        [XafDisplayName("image")]
        image,
        [XafDisplayName("xml")]
        xml,
        [XafDisplayName("timestamp")]
        timestamp
    }
    public enum ScientificDataAction
    {
        None,
        New,
        Edit,
        Delete,

    }

}

