using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.Setting.SDMS
{
    public partial class SpreadSheetBuilder_InitialQCTestRun : XPLiteObject
    {
        public SpreadSheetBuilder_InitialQCTestRun(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        Guid fuqInitialQCTestRunID;
        [Key(true)]
        public Guid uqInitialQCTestRunID
        {
            get { return fuqInitialQCTestRunID; }
            set { SetPropertyValue<Guid>(nameof(uqInitialQCTestRunID), ref fuqInitialQCTestRunID, value); }
        }
        int fuqSequencePatternID;
        public int uqSequencePatternID
        {
            get { return fuqSequencePatternID; }
            set { SetPropertyValue<int>(nameof(uqSequencePatternID), ref fuqSequencePatternID, value); }
        }
        QCType fuqQCTypeID;
        public QCType uqQCTypeID
        {
            get { return fuqQCTypeID; }
            set { SetPropertyValue<QCType>(nameof(uqQCTypeID), ref fuqQCTypeID, value); }
        }
        int fOrder;
        [ImmediatePostData(true)]
        public int Order
        {
            get { return fOrder; }
            set { SetPropertyValue<int>(nameof(Order), ref fOrder, value); }
        }
    }
}