using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.Setting
{
    [RuleCombinationOfPropertiesIsUnique("SampleRepChainRule", DefaultContexts.Save, "PrepMethod.Oid, TestMethod.Oid", SkipNullOrEmptyValues = true)]
    [DefaultClassOptions]
    public class SamplePreparationChain : BaseObject
    {
        public SamplePreparationChain(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region PrepType
        private PrepType _PrepType;
        public PrepType PrepType
        {
            get
            {
                return _PrepType;
            }
            set
            {
                SetPropertyValue<PrepType>(nameof(PrepType), ref _PrepType, value);
            }
        }
        #endregion

        #region PrepMethod
        private Method _PrepMethod;
        [RuleRequiredField]
        public Method PrepMethod
        {
            get
            {
                return _PrepMethod;
            }
            set
            {
                SetPropertyValue(nameof(PrepMethod), ref _PrepMethod, value);

            }
        }
        #endregion

        #region Template
        private SamplePrepTemplates _Template;
        public SamplePrepTemplates Template
        {
            get
            {
                return _Template;
            }
            set
            {
                SetPropertyValue(nameof(Template), ref _Template, value);

            }
        }
        #endregion

        #region Department
        private Department _Department;
        public Department Department
        {
            get
            {
                return _Department;
            }
            set
            {
                SetPropertyValue(nameof(Department), ref _Department, value);

            }
        }
        #endregion

        #region SamplePreparationChainTestMethods
        private TestMethod _TestMethod;
        //[Association("SamplePreparationChainTestMethods", UseAssociationNameAsIntermediateTableName = true)]
        [Association("SamplePreparationChainTestMethods")]
        public TestMethod TestMethod
        {
            get
            {
                return _TestMethod;
            }
            set
            {
                SetPropertyValue<TestMethod>(nameof(TestMethod), ref _TestMethod, value);
            }
        }
        #endregion

        //#region PrepMethods
        //[Association("SamplePreparationChains-PrepMethod")]
        //public XPCollection<Method> PrepMethods
        //{
        //    get
        //    {
        //        return GetCollection<Method>(nameof(PrepMethods));
        //    }
        //}
        //#endregion

        #region Users
        [Association("SamplePreparationChainUsers", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Employee> Users
        {
            get
            {
                return GetCollection<Employee>(nameof(Users));
            }
        }
        #endregion

        #region Instruments
        [Association("SamplePreparationChain-Labware")]
        public XPCollection<BusinessObjects.Assets.Labware> Instruments
        {
            get { return GetCollection<BusinessObjects.Assets.Labware>(nameof(Instruments)); }
        }
        #endregion
    }

    public enum PrepType
    {
        TCLP,
        Digestion
    }
}