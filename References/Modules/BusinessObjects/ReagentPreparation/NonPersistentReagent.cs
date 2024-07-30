using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.ReagentPreparation
{
    public enum PrepSelectTypes
    {
        [XafDisplayName("Chemical Reagent Prep")]
        ChemicalReagentPrep,
        [XafDisplayName("Calibration Set Prep")]
        CalibrationSetPrep,
        [XafDisplayName("Micro Media and Reagent Prep")]
        MicroMediaAndReagentPrep,
        //Standardization
    }
    [DomainComponent]
    public class NonPersistentReagent
    {
        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]
        public Guid Oid { get; set; }
        [Size(SizeAttribute.Unlimited)]
        public string Formula { get; set; }
        public string Variable { get; set; }
        public string Unit { get; set; }
        [VisibleInListView(false),VisibleInDetailView(false),VisibleInDashboards(false)]
        public PrepSelectTypes PrepSelectType { get; set; }

    }
    [DomainComponent]
    public class NPReagentPrep
    {

    }
}