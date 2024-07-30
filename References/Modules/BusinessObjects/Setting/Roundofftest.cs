using DevExpress.ExpressApp.DC;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DomainComponent]
    public class Roundofftest
    {
        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]
        public Guid Oid { get; set; }
        public string SigFigures { get; set; }
        public string CutOff { get; set; }
        public string Decimals { get; set; }
        public string DefaultResult { get; set; }
        public string RptLimit { get; set; }
        public string SciNotationDecimals { get; set; }
        public string NumericResult1 { get; set; }
        public string NumericResult2 { get; set; }
        public string NumericResult3 { get; set; }
        public string NumericResult4 { get; set; }
        public string DisplayResult1 { get; set; }
        public string DisplayResult2 { get; set; }
        public string DisplayResult3 { get; set; }
        public string DisplayResult4 { get; set; }
        public string ScientificResult1 { get; set; }
        public string ScientificResult2 { get; set; }
        public string ScientificResult3 { get; set; }
        public string ScientificResult4 { get; set; }
    }
}