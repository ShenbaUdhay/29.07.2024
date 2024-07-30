using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Validation;

namespace Modules.BusinessObjects.Login
{
    [DomainComponent]
    public class LoginOneTimePin
    {
        public string OTP { get; set; }
        [RuleRequiredField]
        public string NewPassword { get; set; }
        [RuleRequiredField]
        public string ConfirmPassword { get; set; }
    }
}