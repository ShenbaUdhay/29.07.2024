using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Dashboard
{
    //停止使用-转到展现层的用户控件-2017-04-28
    [DefaultClassOptions]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SpecificSymbol : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Consturctor
        public SpecificSymbol(Session session)
            : base(session)
        {
        }
        #endregion

        #region DefaultEvent
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            Up0 = "0";
        }
        #endregion

        #region 特殊符号
        public string Up0 { get { return "⁰"; } set { } }
        public string Up1 { get { return "¹"; } set { } }
        public string Up2 { get { return "²"; } set { } }
        public string Up3 { get { return "³"; } set { } }
        public string Up4 { get { return "⁴"; } set { } }
        public string Up5 { get { return "⁵"; } set { } }
        public string Up6 { get { return "⁶"; } set { } }
        public string Up7 { get { return "⁷"; } set { } }
        public string Up8 { get { return "⁸"; } set { } }
        public string Up9 { get { return "⁹"; } set { } }
        public string Up10 { get { return "⁺"; } set { } }
        public string Up11 { get { return "⁻"; } set { } }
        public string Up12 { get { return "⁼"; } set { } }
        public string Up13 { get { return "ⁿ"; } set { } }
        public string Below0 { get { return "₀"; } set { } }
        public string Below1 { get { return "₁"; } set { } }
        public string Below2 { get { return "₂"; } set { } }
        public string Below3 { get { return "₃"; } set { } }
        public string Below4 { get { return "₄"; } set { } }
        public string Below5 { get { return "₅"; } set { } }
        public string Below6 { get { return "₆"; } set { } }
        public string Below7 { get { return "₇"; } set { } }
        public string Below8 { get { return "₈"; } set { } }
        public string Below9 { get { return "₉"; } set { } }
        public string Below10 { get { return "₊"; } set { } }
        public string Below11 { get { return "₋"; } set { } }
        public string Below12 { get { return "₌"; } set { } }
        public string Middle0 { get { return "′"; } set { } }
        public string Middle1 { get { return "″"; } set { } }
        public string Middle2 { get { return "℃"; } set { } }
        public string Middle3 { get { return "℉"; } set { } }
        public string Middle4 { get { return "Ω"; } set { } }
        public string Middle5 { get { return "φ"; } set { } }
        public string Middle6 { get { return "Ø"; } set { } }
        public string Middle7 { get { return "π"; } set { } }
        public string Middle8 { get { return "℉"; } set { } }
        public string Middle9 { get { return "※"; } set { } }
        public string Middle10 { get { return "±"; } set { } }
        public string Middle11 { get { return "×"; } set { } }
        public string Middle12 { get { return "≤"; } set { } }
        public string Middle13 { get { return "≥"; } set { } }
        public string Middle14 { get { return ">"; } set { } }
        public string Middle15 { get { return "<"; } set { } }
        public string Middle16 { get { return "△"; } set { } }
        public string Middle17 { get { return "‰"; } set { } }
        public string Middle18 { get { return "r/min"; } set { } }
        public string Middle19 { get { return "N•m"; } set { } }
        public string Middle20 { get { return "g•m"; } set { } }
        public string Middle21 { get { return "%H0"; } set { } }
        public string Middle22 { get { return "%"; } set { } }
        public string Middle23 { get { return "MPa"; } set { } }
        public string Middle24 { get { return "m"; } set { } }
        public string Middle25 { get { return "cm"; } set { } }
        public string Middle26 { get { return "mL"; } set { } }
        public string Middle27 { get { return "g"; } set { } }
        public string Middle28 { get { return "kg"; } set { } }
        public string Middle29 { get { return "L"; } set { } }
        public string Middle30 { get { return "μ"; } set { } }
        public string Middle31 { get { return "μg"; } set { } }
        public string Middle32 { get { return "/"; } set { } }
        public string Middle33 { get { return "M₋₁"; } set { } }
        public string Middle34 { get { return "M₁"; } set { } }
        public string Middle35 { get { return "M₂"; } set { } }
        public string Middle36 { get { return "M₃"; } set { } }
        public string Middle37 { get { return "M₄"; } set { } }
        public string Middle38 { get { return "F₁"; } set { } }
        public string Middle39 { get { return "F₂"; } set { } }
        public string Middle40 { get { return "F₃"; } set { } }
        public string Middle41 { get { return "F₄"; } set { } }
        public string Middle42 { get { return "E₂"; } set { } }
        public string Middle43 { get { return "U₉₅"; } set { } }
        public string Middle44 { get { return "10¹"; } set { } }
        public string Middle45 { get { return "10⁻¹"; } set { } }
        public string Middle46 { get { return "mL"; } set { } }
        public string Middle47 { get { return "g"; } set { } }
        public string Middle48 { get { return "kg"; } set { } }
        public string Middle49 { get { return "L"; } set { } }
        public string Middle50 { get { return "μ"; } set { } }
        public string Middle51 { get { return "μg"; } set { } }
        public string Middle52 { get { return "/"; } set { } }
        #endregion
    }
}