using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using Modules.BusinessObjects.Hr;
using System;

namespace BTLIMS.Module.BusinessObjects
{
    class FuncCurrentCompanyName : ICustomFunctionOperator
    {
        static FuncCurrentCompanyName()
        {
            var instance = new FuncCurrentCompanyName();
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
            if (SecuritySystem.CurrentUser == null || ((Employee)SecuritySystem.CurrentUser).Company == null)
            {
                return "";
            }
            else
            {
                return ((Employee)SecuritySystem.CurrentUser).Company.CompanyName;
            }
        }

        public string Name
        {
            get { return "CurrentCompanyName"; }
        }

        public static void Register()
        {
        }
    }
}
