using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class AnalysisDepartmentChain : BaseObject//, ICheckedListBoxItemsProvider
    {
        public AnalysisDepartmentChain(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region Employee
        private Employee _Employee;
        public Employee Employee
        {
            get
            {
                return _Employee;
            }
            set
            {
                SetPropertyValue<Employee>(nameof(Employee), ref _Employee, value);
            }
        }
        #endregion

        //#region SamplePrep1
        //private bool _SamplePrep1;
        //public bool SamplePrep1
        //{
        //    get
        //    {
        //        return _SamplePrep1;
        //    }
        //    set
        //    {
        //        SetPropertyValue<bool>(nameof(SamplePrep1), ref _SamplePrep1, value);
        //    }
        //}
        //#endregion

        //#region SamplePrep2
        //private bool _SamplePrep2;
        //public bool SamplePrep2
        //{
        //    get
        //    {
        //        return _SamplePrep2;
        //    }
        //    set
        //    {
        //        SetPropertyValue<bool>(nameof(SamplePrep2), ref _SamplePrep2, value);
        //    }
        //}
        //#endregion

        #region ResultEntry
        private bool _ResultEntry;
        public bool ResultEntry
        {
            get
            {
                return _ResultEntry;
            }
            set
            {
                SetPropertyValue<bool>(nameof(ResultEntry), ref _ResultEntry, value);
            }
        }
        #endregion

        //#region TestMethod
        //private TestMethod _TestMethod;
        //public TestMethod TestMethod
        //{
        //    get
        //    {
        //        return _TestMethod;
        //    }
        //    set
        //    {
        //        SetPropertyValue<TestMethod>(nameof(TestMethod),ref _TestMethod,value);
        //    }
        //}
        //#endregion

        #region AnalysisDepartmentUsers
        [Association("AnalysisDepartmentChainTestMethods", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<TestMethod> TestMethods
        {
            get
            {
                return GetCollection<TestMethod>(nameof(TestMethods));
            }
        }
        #endregion

        #region ResultValidation
        private bool _ResultValidation;
        public bool ResultValidation
        {
            get
            {
                return _ResultValidation;
            }
            set
            {
                SetPropertyValue<bool>(nameof(ResultValidation), ref _ResultValidation, value);
            }
        }
        #endregion

        #region ResultApproval
        private bool _ResultApproval;
        public bool ResultApproval
        {
            get
            {
                return _ResultApproval;
            }
            set
            {
                SetPropertyValue<bool>(nameof(ResultApproval), ref _ResultApproval, value);
            }
        }
        #endregion

        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<Employee> UsersDataSource
        //{
        //    get
        //    {
        //        return new XPCollection<Employee>(Session);
        //    }
        //}

        //#region ICheckedListBoxItemsProvider Members
        //public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        //{
        //    Dictionary<object, string> properties = new Dictionary<object, string>();
        //    if ((targetMemberName == nameof(SamplePrep1) || targetMemberName == nameof(SamplePrep2) || targetMemberName == nameof(ResultEntry)) && UsersDataSource != null && UsersDataSource.Count > 0)
        //    {
        //        foreach (Employee objUser in UsersDataSource.ToList())
        //        {
        //            if (!properties.ContainsKey(objUser.Oid))
        //            {
        //                properties.Add(objUser.Oid, objUser.UserName);
        //            }
        //        }
        //    }
        //    return properties;
        //}

        //public event EventHandler ItemsChanged;
        //protected void OnItemsChanged()
        //{
        //    if (ItemsChanged != null)
        //    {
        //        ItemsChanged(this, new EventArgs());
        //    }
        //}
        //#endregion
    }
}