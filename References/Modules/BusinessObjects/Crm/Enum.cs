namespace Modules.BusinessObjects.Crm
{
    /// <summary>
    /// Customer-CustomerType
    /// 客户管理-单位类型
    /// </summary>
    public enum CustomerType
    {
        受检单位 = 0,
        证书单位 = 1,
        生产单位 = 2,
        综合单位 = 3,
        外包单位 = 4
    }
    /// <summary>
    /// 婚姻状况
    /// </summary>
    public enum MaritalStatus
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 单身
        /// </summary>
        Single = 1,
        /// <summary>
        /// 已婚
        /// </summary>
        Married = 2,
        /// <summary>
        /// 离异
        /// </summary>
        Divorced = 3,
        /// <summary>
        /// 丧偶
        /// </summary>
        Widowed = 4
    }
}