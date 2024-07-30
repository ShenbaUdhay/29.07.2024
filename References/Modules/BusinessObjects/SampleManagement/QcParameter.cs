using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Setting;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    [Appearance("CategoryColoredInListView", TargetItems = "Group",
   Context = "ListView", FontColor = "Blue")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCombinationOfPropertiesIsUnique("QcParameter", DefaultContexts.Save, "TestMethod,QCType", SkipNullOrEmptyValues = false)]

    public class QcParameter : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        public QcParameter(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private TestMethod _TestMethod;
        [RuleRequiredField("QCTestMethod", DefaultContexts.Save)]
        [Association]
        public TestMethod TestMethod
        {

            get { return _TestMethod; }
            set { SetPropertyValue("TestMethod", ref _TestMethod, value); }
        }

        private QCType _QCtype;
        [RuleRequiredField("QCParameter.QCType", DefaultContexts.Save)]
        [Association]
        public QCType QCType
        {
            get { return _QCtype; }
            set { SetPropertyValue("QCType", ref _QCtype, value); }
        }

        [ManyToManyAlias("QCTestParameter", "Parameter")]
        public IList<Parameter> Parameters
        {
            get
            {
                return GetList<Parameter>("Parameters");
            }
        }
        #region IListforTestParameter
        [Association, Browsable(false)]

        public IList<QCTestParameter> QCTestParameter
        {
            get
            {
                return GetList<QCTestParameter>("QCTestParameter");
            }
        }
        #endregion IListforTestParameter

        #region NonPersistence
        [NonPersistent]
        public string Method
        {
            get
            {
                if (TestMethod != null)
                {
                    return TestMethod.MethodName.MethodName;
                }
                else
                {
                    return null;
                }
            }
        }


        [NonPersistent]
        public string Matrix
        {
            get
            {
                if (TestMethod != null)
                {
                    return TestMethod.MatrixName.MatrixName;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }
}