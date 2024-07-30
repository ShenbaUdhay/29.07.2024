using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.SampleManagement;

namespace Modules.BusinessObjects.QC
{
    [DefaultClassOptions]
    public class QCBatchInstrument : BaseObject
    {
        public QCBatchInstrument(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private SpreadSheetEntry_AnalyticalBatch _QCBatchID;
        public SpreadSheetEntry_AnalyticalBatch QCBatchID
        {
            get { return _QCBatchID; }
            set { SetPropertyValue("QCBatchID", ref _QCBatchID, value); }
        }

        private Labware _LabwareID;
        public Labware LabwareID
        {
            get { return _LabwareID; }
            set { SetPropertyValue("LabwareID", ref _LabwareID, value); }
        }
    }
}