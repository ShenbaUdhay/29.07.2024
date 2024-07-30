using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Libraries
{
    [NonPersistent]
    [ModelDefault("Caption", "��ʾ")]
    public class TextMessage
    {
        private readonly string _message;

        [ModelDefault("Caption", " ")]
        [Size(SizeAttribute.Unlimited)]
        public string Message
        {
            get { return _message; }
        }

        public TextMessage(string message)
        {
            _message = message;
        }
    }
}
