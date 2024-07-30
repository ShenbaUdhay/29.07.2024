using DevExpress.Xpo;
using System;
namespace Modules.BusinessObjects.SDA.AlpacaLims
{
    public partial class FlutterSDARRA_SamplingAlternateStation : XPLiteObject
    {
        public FlutterSDARRA_SamplingAlternateStation(Session session) : base(session) { }

        long fuqAlternateStationID;
        [Key(true)]
        public long uqAlternateStationID
        {
            get { return fuqAlternateStationID; }
            set { SetPropertyValue<long>(nameof(uqAlternateStationID), ref fuqAlternateStationID, value); }
        }
        string fJobID;
        [Size(50)]
        public string JobID
        {
            get { return fJobID; }
            set { SetPropertyValue<string>(nameof(JobID), ref fJobID, value); }
        }
        Guid fuqStationID;
        public Guid uqStationID
        {
            get { return fuqStationID; }
            set { SetPropertyValue<Guid>(nameof(uqStationID), ref fuqStationID, value); }
        }
        string fStationID;
        [Size(200)]
        public string StationID
        {
            get { return fStationID; }
            set { SetPropertyValue<string>(nameof(StationID), ref fStationID, value); }
        }
        string fStationName;
        [Size(200)]
        public string StationName
        {
            get { return fStationName; }
            set { SetPropertyValue<string>(nameof(StationName), ref fStationName, value); }
        }
    }
}
