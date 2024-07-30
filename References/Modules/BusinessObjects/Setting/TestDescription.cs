using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SampleManagement
{
    [NonPersistent]
    public class TestDescription : BaseObject
    {
        public TestDescription(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region Description
        private string fDescription;
        [Size(1000)]
        public string Description
        {
            get
            {
                return fDescription;
            }
            set
            {
                SetPropertyValue("Comment", ref fDescription, value);
            }
        }
        #endregion
    }
}