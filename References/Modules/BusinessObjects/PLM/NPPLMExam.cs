using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.PLM
{
    [DefaultClassOptions]
    [NonPersistent]
    public class NPPLMExam : BaseObject
    {
        public NPPLMExam(Session session) : base(session) { }
        #region Type_Isotropic
        private bool _Type_Isotropic;
        public bool Type_Isotropic
        {
            get { return _Type_Isotropic; }
            set { SetPropertyValue(nameof(Type_Isotropic), ref _Type_Isotropic, value); }
        }
        #endregion
        #region Type_Anisotropic
        private bool _Type_Anisotropic;
        public bool Type_Anisotropic
        {
            get { return _Type_Anisotropic; }
            set { SetPropertyValue(nameof(Type_Anisotropic), ref _Type_Anisotropic, value); }
        }
        #endregion
        #region Pleochroism_Yes
        private bool _Pleochroism_Yes;
        public bool Pleochroism_Yes
        {
            get { return _Pleochroism_Yes; }
            set { SetPropertyValue(nameof(Pleochroism_Yes), ref _Pleochroism_Yes, value); }
        }
        #endregion
        #region Pleochroism_No
        private bool _Pleochroism_No;
        public bool Pleochroism_No
        {
            get { return _Pleochroism_No; }
            set { SetPropertyValue(nameof(Pleochroism_No), ref _Pleochroism_No, value); }
        }
        #endregion
        #region Color
        private string _Color;
        public string Color
        {
            get { return _Color; }
            set { SetPropertyValue(nameof(Color), ref _Color, value); }
        }
        #endregion
        #region Morphology_Bundles
        private bool _Morphology_Bundles;
        public bool Morphology_Bundles
        {
            get { return _Morphology_Bundles; }
            set { SetPropertyValue(nameof(Morphology_Bundles), ref _Morphology_Bundles, value); }
        }
        #endregion
        #region Morphology_Curved
        private bool _Morphology_Curved;
        public bool Morphology_Curved
        {
            get { return _Morphology_Curved; }
            set { SetPropertyValue(nameof(Morphology_Curved), ref _Morphology_Curved, value); }
        }
        #endregion
        #region Morphology_Ribbon
        private bool _Morphology_Ribbon;
        public bool Morphology_Ribbon
        {
            get { return _Morphology_Ribbon; }
            set { SetPropertyValue(nameof(Morphology_Ribbon), ref _Morphology_Ribbon, value); }
        }
        #endregion
        #region Morphology_Splayed
        private bool _Morphology_Splayed;
        public bool Morphology_Splayed
        {
            get { return _Morphology_Splayed; }
            set { SetPropertyValue(nameof(_Morphology_Splayed), ref _Morphology_Splayed, value); }
        }
        #endregion
        #region Morphology_Straight
        private bool _Morphology_Straight;
        public bool Morphology_Straight
        {
            get { return _Morphology_Straight; }
            set { SetPropertyValue(nameof(Morphology_Straight), ref _Morphology_Straight, value); }
        }
        #endregion
        #region Morphology_Wavey
        private bool _Morphology_Wavey;
        public bool Morphology_Wavey
        {
            get { return _Morphology_Wavey; }
            set { SetPropertyValue(nameof(Morphology_Wavey), ref _Morphology_Wavey, value); }
        }
        #endregion
        #region Extinction_Oblique
        private bool _Extinction_Oblique;
        public bool Extinction_Oblique
        {
            get { return _Extinction_Oblique; }
            set { SetPropertyValue(nameof(Extinction_Oblique), ref _Extinction_Oblique, value); }
        }
        #endregion
        #region Extinction_Parallel
        private bool _Extinction_Parallel;
        public bool Extinction_Parallel
        {
            get { return _Extinction_Parallel; }
            set { SetPropertyValue(nameof(Extinction_Parallel), ref _Extinction_Parallel, value); }
        }
        #endregion
        #region Extinction_Undulose
        private bool _Extinction_Undulose;
        public bool Extinction_Undulose
        {
            get { return _Extinction_Undulose; }
            set { SetPropertyValue(nameof(Extinction_Undulose), ref _Extinction_Undulose, value); }
        }
        #endregion
        #region Extinction_Wavey
        private bool _Extinction_Wavey;
        public bool Extinction_Wavey
        {
            get { return _Extinction_Wavey; }
            set { SetPropertyValue(nameof(Extinction_Wavey), ref _Extinction_Wavey, value); }
        }
        #endregion
        #region Elonggation_Negative
        private bool _Elonggation_Negative;
        public bool Elonggation_Negative
        {
            get { return _Elonggation_Negative; }
            set { SetPropertyValue(nameof(Elonggation_Negative), ref _Elonggation_Negative, value); }
        }
        #endregion
        #region Elonggation_Positive
        private bool _Elonggation_Positive;
        public bool Elonggation_Positive
        {
            get { return _Elonggation_Positive; }
            set { SetPropertyValue(nameof(Elonggation_Positive), ref _Elonggation_Positive, value); }
        }
        #endregion
        #region Elonggation_Both
        private bool _Elonggation_Both;
        public bool Elonggation_Both
        {
            get { return _Elonggation_Both; }
            set { SetPropertyValue(nameof(Elonggation_Both), ref _Elonggation_Both, value); }
        }
        #endregion
        #region Gamma
        private string _Gamma;
        public string Gamma
        {
            get { return _Gamma; }
            set { SetPropertyValue(nameof(Gamma), ref _Gamma, value); }
        }
        #endregion
        #region Alpha
        private string _Alpha;
        public string Alpha
        {
            get { return _Alpha; }
            set { SetPropertyValue(nameof(Alpha), ref _Alpha, value); }
        }
        #endregion
        #region LambdaPara
        private string _LambdaPara;
        public string LambdaPara
        {
            get { return _LambdaPara; }
            set { SetPropertyValue(nameof(LambdaPara), ref _LambdaPara, value); }
        }
        #endregion
        #region LambdaPerb
        private string _LambdaPerb;
        public string LambdaPerb
        {
            get { return _LambdaPerb; }
            set { SetPropertyValue(nameof(LambdaPerb), ref _LambdaPerb, value); }
        }
        #endregion
        #region RIOil
        private string _RIOil;
        public string RIOil
        {
            get { return _RIOil; }
            set { SetPropertyValue(nameof(RIOil), ref _RIOil, value); }
        }
        #endregion
        #region Parallel
        private string _Parallel;
        public string Parallel
        {
            get { return _Parallel; }
            set { SetPropertyValue(nameof(Parallel), ref _Parallel, value); }
        }
        #endregion
        #region Perpendicular
        private string _Perpendicular;
        public string Perpendicular
        {
            get { return _Perpendicular; }
            set { SetPropertyValue(nameof(Perpendicular), ref _Perpendicular, value); }
        }
        #endregion
    }
}