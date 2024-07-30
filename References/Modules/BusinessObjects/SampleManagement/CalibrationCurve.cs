using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    [NonPersistent]
    public class CalibrationCurve : BaseObject
    {
        public CalibrationCurve(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        string _RegressionCurveConstant;
        //[Size(SizeAttribute.Unlimited)]
        public string RegressionCurveConstant
        {
            get { return _RegressionCurveConstant; }
            set { SetPropertyValue<string>(nameof(RegressionCurveConstant), ref _RegressionCurveConstant, value); }
        }
    }
}