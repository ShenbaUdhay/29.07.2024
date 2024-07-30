// ================================================================================
// Class Name: [FunctionCurrentUserName]
// Author: Sunny
// Date: 2016年12月15日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：自定义函数参数，用于系统参数调用，需要在Module.cs下进行注册
//             返回当前用户名
//             调用：1.在模型编辑器的ListView：Criteria下：[Company] = CurrentCompanyOid()
//                   2.在类中，[RuleCriteria("Company.Oid = CurrentCompanyOid()")]，有待测试
// ================================================================================
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using Modules.BusinessObjects.Hr;
using System;

namespace BTLIMS.Module.BusinessObjects
{
    /// <summary>
    /// 返回当前用户名
    /// </summary>
    public class FuncCurrentUserName : ICustomFunctionOperator
    {
        static FuncCurrentUserName()
        {
            var instance = new FuncCurrentUserName();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }
        public Type ResultType(params Type[] operands)
        {
            return typeof(object);
        }

        public object Evaluate(params object[] operands)
        {
            return SecuritySystem.CurrentUser == null ? "" : ((Employee)SecuritySystem.CurrentUser).UserName;
        }

        public string Name
        {
            get { return "CurrentUserName"; }
        }

        public static void Register()
        {
        }
    }
}