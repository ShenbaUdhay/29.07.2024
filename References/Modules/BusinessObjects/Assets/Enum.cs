namespace Modules.BusinessObjects.Assets
{
    /// <summary>
    /// 业务类别
    /// </summary>
    public enum BizCategory
    {
        /// <summary>
        /// 计量
        /// </summary>
        Measure = 0,
        /// <summary>
        /// 质检
        /// </summary>
        QualityInspection = 1
    }

    public enum InstrumentMaintenanceStatus
    {
        PendingQueue,
        Maintained
    }
    ///// <summary>
    ///// Labware-TrackingMode
    ///// 仪器设备-溯源方式
    ///// </summary>
    //public enum TrackingMode
    //{
    //    /// <summary>
    //    /// 自检
    //    /// </summary>
    //    SelfTest,
    //    /// <summary>
    //    /// 送检
    //    /// </summary>
    //    Inspect,
    //    /// <summary>
    //    /// 不溯源
    //    /// </summary>
    //    NoTraceability
    //}

    ///// <summary>
    ///// Labware-Status
    ///// 仪器设备-设备状态
    ///// </summary>
    //public enum LabwareStatus
    //{
    //    /// <summary>
    //    /// 正常
    //    /// </summary>
    //    Normal,
    //    /// <summary>
    //    /// 溯源中
    //    /// </summary>
    //    Traceability,
    //    /// <summary>
    //    /// 报废
    //    /// </summary>
    //    Scrap
    //}
    ///// <summary>
    ///// Labware-BizCategory
    ///// 仪器设备-业务类别
    ///// </summary>
    //public enum BizCategory
    //{
    //    /// <summary>
    //    /// 计量
    //    /// </summary>
    //    Measure,
    //    /// <summary>
    //    /// 质检
    //    /// </summary>
    //    QualityInspection
    //}
    ///// <summary>
    ///// LabwareCertificate-CertificateCategory
    ///// 仪器设备证书-证书类别
    ///// </summary>
    //public enum CertificateCategory
    //{
    //    /// <summary>
    //    /// 检定证书
    //    /// </summary>
    //    Verification,
    //    /// <summary>
    //    /// 校准证书
    //    /// </summary>
    //    Calibration,
    //    /// <summary>
    //    /// 检测报告
    //    /// </summary>
    //    Examining,
    //    /// <summary>
    //    /// 测试报告
    //    /// </summary>
    //    Test,
    //    /// <summary>
    //    /// 结果通知书
    //    /// </summary>
    //    Notices,
    //    /// <summary>
    //    /// 检验报告
    //    /// </summary>
    //    Checkout
    //}
}