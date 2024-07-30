using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;
//using Modules.BusinessObjects.Biz;

namespace Modules.BusinessObjects.Libraries
{
    public static class Library
    {
        /// <summary>
        /// 临时变量，用于存储登陆用户的系统语言
        /// 系统语言来自，公司维度的语言设置，用户可以设置自己的语言
        /// </summary>
        public static string Language;

        #region 获取服务器时间
        /// <summary>
        /// 获取务器时间,参数为：UnitOfWork
        /// </summary>
        /// <param name="uow">UnitOfWork的实例</param>
        /// <returns></returns>
        public static DateTime GetServerTime(UnitOfWork uow)
        {
            return GetServerTime((Session)uow);
        }
        /// <summary>
        /// 获取务器时间,参数为：XPObjectSpace
        /// </summary>
        /// <param name="objSpace">objSpace的实例</param>
        /// <returns></returns>
        public static DateTime GetServerTime(XPObjectSpace objSpace)
        {
            return GetServerTime(objSpace.Session);
        }
        /// <summary>
        /// 获取务器时间,参数为：XPObjectSpace
        /// </summary>
        /// <param name="objSpace">objSpace的实例</param>
        /// <returns></returns>
        public static DateTime GetServerTime(IObjectSpace objSpace)
        {
            //return GetServerTime((Session)objSpace);
            return GetServerTime((Session)((XPObjectSpace)objSpace).Session);
        }
        /// <summary>
        /// 获取务器时间,参数为：Session
        /// </summary>
        /// <param name="session">Session</param>
        /// <returns></returns>
        public static DateTime GetServerTime(Session session)
        {
            var funcNow = new FunctionOperator(FunctionOperatorType.Now);
            var serverTime = DateTime.Now;//(DateTime)session.Evaluate(typeof(XPObjectType), funcNow, null);

            return serverTime;


            //DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            //DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
            //                                                 // TimeZoneInfo localZone = getTimeZone();

            //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(getTimeZone());
            //DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
            //return localTime;

        }
        #endregion



        /// <summary>
        /// 格式化日期:yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeFormat(DateTime date)
        {
            return DateTime.Parse(date.ToString("yyyy-MM-dd"));
        }

        public static DateTime GetDateTime
        {
            get { return DateTime.Now; }
        }
        /// <summary>
        /// 验证日期格式是否正确
        /// </summary>
        /// <param name="dt">日期的字符串格式</param>
        /// <returns></returns>
        public static bool IsDateTime(string dt)
        {
            DateTime dtOut;
            return DateTime.TryParse(dt, out dtOut);
        }

        public static object GetCurrentUserCompany(IObjectSpace objSpace)
        {
            if (objSpace != null && SecuritySystem.CurrentUserId != null)
            {
                return objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;
            }
            return null;
        }
        ///// <summary>
        ///// 返回枚举对应语言的Caption
        ///// </summary>
        ///// <param name="approvalResult"></param>
        ///// <returns></returns>
        //public static string GetEnumApprovalResult(ApprovalResult approvalResult)
        //{
        //    var currentUser = SecuritySystem.CurrentUser as Employee;
        //    if (currentUser != null)
        //    {
        //        //取得员工所在公司的统一语言，如果个人设置了语言，则以个人为准
        //        var currentLanguage = "zh-CN";
        //        var language = currentUser.IsEnabledLanguage ? currentUser.Language.ToString() : currentUser.Company.Language.ToString();
        //        if (!string.IsNullOrWhiteSpace(language))
        //        {
        //            currentLanguage = language == "Chinese" ? "zh-CN" : "EN";
        //        }

        //        switch (currentLanguage)
        //        {
        //            case "zh-CN":
        //                switch (approvalResult)
        //                {
        //                    case ApprovalResult.Pass:
        //                        return "通过";
        //                        break;
        //                    case ApprovalResult.Audit:
        //                        return "重审核";
        //                        break;
        //                    case ApprovalResult.Registration:
        //                        return "重结果登记";
        //                        break;
        //                }
        //                break;

        //            case "EN":
        //                switch (approvalResult)
        //                {
        //                    case ApprovalResult.Pass:
        //                        return ApprovalResult.Pass.ToString();
        //                        break;
        //                    case ApprovalResult.Audit:
        //                        return ApprovalResult.Audit.ToString();
        //                        break;
        //                    case ApprovalResult.Registration:
        //                        return ApprovalResult.Registration.ToString();
        //                        break;
        //                }
        //                break;
        //        }
        //    }
        //    return "";
        //}
        ///// <summary>
        ///// 返回枚举对应语言的Caption
        ///// </summary>
        ///// <param name="approvalResult"></param>
        ///// <returns></returns>
        //public static string GetEnumAuditResult(AuditResult auditResult)
        //{
        //    //取得员工所在公司的统一语言，如果个人设置了语言，则以个人为准
        //    var currentLanguage = GetCurrentSysLanguage();
        //    switch (currentLanguage)
        //    {
        //        case "zh-CN":
        //            switch (auditResult)
        //            {
        //                case AuditResult.Pass:
        //                    return "通过";
        //                    break;
        //                    break;
        //                case AuditResult.Registration:
        //                    return "重结果登记";
        //                    break;
        //            }
        //            break;

        //        case "EN":
        //            switch (auditResult)
        //            {
        //                case AuditResult.Pass:
        //                    return AuditResult.Pass.ToString();
        //                    break;
        //                case AuditResult.Registration:
        //                    return AuditResult.Registration.ToString();
        //                    break;
        //            }
        //            break;
        //    }

        //    return "";
        //}
        //public static string GetEnumComment(Comment comment)
        //{
        //    var currentLanguage = GetCurrentSysLanguage();
        //    switch (currentLanguage)
        //    {
        //        case "zh-CN":
        //            switch (comment)
        //            {
        //                case Comment.Nothing:
        //                    return "无";
        //                    break;
        //                case Comment.FormatError:
        //                    return "格式、数据录入错误";
        //                    break;
        //                case Comment.DataError:
        //                    return "原始数据错误";
        //                    break;
        //            }
        //            break;

        //        case "EN":
        //            switch (comment)
        //            {
        //                case Comment.Nothing:
        //                    return Comment.Nothing.ToString();
        //                    break;
        //                case Comment.FormatError:
        //                    return Comment.FormatError.ToString();
        //                    break;
        //                case Comment.DataError:
        //                    return Comment.DataError.ToString();
        //                    break;
        //            }
        //            break;
        //    }

        //    return "";
        //}

        /// <summary>
        /// 判断当前系统语言是否为中文,true为中文
        /// </summary>
        /// <returns></returns>
        public static bool IsCurrentSysLanguageCn()
        {
            var currentUser = SecuritySystem.CurrentUser as Employee;
            if (currentUser != null)
            {
                //取得员工所在公司的统一语言，如果个人设置了语言，则以个人为准
                var language = currentUser.IsEnabledLanguage ? currentUser.Language.ToString() : currentUser.Company.Language.ToString();
                if (!string.IsNullOrWhiteSpace(language) && language == "Chinese")
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public static string GetCurrentSysLanguage()
        {
            var currentLanguage = "zh-CN";
            var currentUser = SecuritySystem.CurrentUser as Employee;
            if (currentUser != null)
            {
                //取得员工所在公司的统一语言，如果个人设置了语言，则以个人为准

                var language = currentUser.IsEnabledLanguage ? currentUser.Language.ToString() : currentUser.Company.Language.ToString();
                if (!string.IsNullOrWhiteSpace(language))
                {
                    currentLanguage = language == "Chinese" ? "zh-CN" : "EN";
                }
            }
            return currentLanguage;
        }
    }
}