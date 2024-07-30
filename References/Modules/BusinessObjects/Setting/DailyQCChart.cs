using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [NonPersistent]
    [DomainComponent]
    public class DailyQCChart : BaseObject
    {
        public DailyQCChart(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        #region From
        private DateTime _From;
        public DateTime From
        {
            get { return _From; }
            set { SetPropertyValue(nameof(From), ref _From, value); }
        }
        #endregion

        #region To
        private DateTime _To;
        public DateTime To
        {
            get { return _To; }
            set { SetPropertyValue(nameof(To), ref _To, value); }
        }
        #endregion

        #region ReferenceValue
        private string _ReferenceValue;
        public string ReferenceValue
        {
            get { return _ReferenceValue; }
            set { SetPropertyValue(nameof(ReferenceValue), ref _ReferenceValue, value); }
        }
        #endregion

        #region Count
        private string _Count;
        public string Count
        {
            get { return _Count; }
            set { SetPropertyValue(nameof(Count), ref _Count, value); }
        }
        #endregion

        #region Mean
        private string _Mean;
        public string Mean
        {
            get { return _Mean; }
            set { SetPropertyValue(nameof(Mean), ref _Mean, value); }
        }
        #endregion

        #region STD
        private string _STD;
        public string STD
        {
            get { return _STD; }
            set { SetPropertyValue(nameof(STD), ref _STD, value); }
        }
        #endregion

        #region LCL
        private string _LCL;
        public string LCL
        {
            get { return _LCL; }
            set { SetPropertyValue(nameof(LCL), ref _LCL, value); }
        }
        #endregion

        #region UCL
        private string _UCL;
        public string UCL
        {
            get { return _UCL; }
            set { SetPropertyValue(nameof(UCL), ref _UCL, value); }
        }
        #endregion
    }
}