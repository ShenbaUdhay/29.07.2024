using DevExpress.Spreadsheet.Functions;
using ReportMultilanguage.LIMSFormula;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LDM.Module.BusinessObjects
{
    public class SignificantNotation : ICustomFunction
    {
        const string functionName = "SGFNT";
        readonly ParameterInfo[] functionParameters;

        public SignificantNotation()
        {
            // Missing optional parameters do not result in an error message.
            this.functionParameters = new ParameterInfo[] { new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
            new ParameterInfo(ParameterType.Value, ParameterAttributes.Required)
           };
        }

        public string Name { get { return functionName; } }
        ParameterInfo[] IFunction.Parameters { get { return functionParameters; } }
        ParameterType IFunction.ReturnType { get { return ParameterType.Value; } }
        bool IFunction.Volatile { get { return false; } }

        public string GetName(CultureInfo culture)
        {
            return functionName;
        }

        ParameterValue IFunction.Evaluate(IList<ParameterValue> parameters, EvaluationContext context)
        {
            ParameterValue ValueA;
            ParameterValue ValueB;

            if (parameters.Count == 2)
            {
                ValueA = parameters[0];
                ValueB = parameters[1];

                //return FormatSF(Convert.ToDouble(ValueA.NumericValue), Convert.ToInt16(ValueB.NumericValue), ValueC == "1" ? true : false);
                LIMSFormula objSignFicant = new LIMSFormula();
                return objSignFicant.FormatSF(Convert.ToDouble(ValueA.NumericValue), Convert.ToInt16(ValueB.NumericValue), true);
            }
            return 0;
        }
    }
}
