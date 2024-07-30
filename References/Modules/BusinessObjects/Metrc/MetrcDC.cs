using DevExpress.ExpressApp.DC;
using System;

namespace Modules.BusinessObjects.Metrc
{
    [DomainComponent]
    public class MetrcIncoming
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public MetrcFacility Facility { get; set; }
    }

    [DomainComponent]
    public class MetrcFacility
    {
        public string Facility { get; set; }
        public string LicenseNumber { get; set; }
    }

    [DomainComponent]
    public class MetrcIncomingData { }

    [DomainComponent]
    public class MetrcIncomingDetData { }
}