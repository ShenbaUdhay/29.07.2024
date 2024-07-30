using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    public enum PrepStatus
    {
        [XafDisplayName("Pending Tier 1 Prep")]
        PendingTier1Prep,
        [XafDisplayName("Pending Tier 2 Prep")]
        PendingTier2Prep,
        Completed

    }
    [DefaultClassOptions]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).

    // [RuleCombinationOfPropertiesIsUnique("PrepMethod", DefaultContexts.Save, "Method,TestMethod", SkipNullOrEmptyValues = false)]
    public class PrepMethod : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        TestInfo testinfo = new TestInfo();
        public PrepMethod(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }


        #region Method
        private Method fMethod;
        [RuleRequiredField("Method1", DefaultContexts.Save)]
        [DataSourceProperty("MethodDataSource")]
        [ImmediatePostData]
        //[RuleUniqueValue]
        public Method Method
        {
            get
            {
                return fMethod;
            }
            set
            {
                SetPropertyValue(nameof(Method), ref fMethod, value);
            }
        }
        #endregion

        #region Tier
        private uint fTier;
        public uint Tier
        {
            get
            {
                return fTier;
            }
            set
            {
                SetPropertyValue(nameof(Tier), ref fTier, value);
            }
        }
        #endregion

        #region PrepType
        private PrepTypes fPrepType;
        [RuleRequiredField("PrepType", DefaultContexts.Save)]
        public PrepTypes PrepType
        {
            get { return fPrepType; }
            set { SetPropertyValue("PrepType", ref fPrepType, value); }
        }
        #endregion

        #region Department
        private Department fDepartment;
        //[Browsable(false)]
        [ImmediatePostData]
        public Department Department
        {
            get
            {
                return fDepartment;
            }
            set
            {
                SetPropertyValue(nameof(Department), ref fDepartment, value);
            }
        }
        #endregion

        #region Instrument
        private string fInstrument;
        [DataSourceProperty("LabwareDataSource")]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [ImmediatePostData]
        public string Instrument
        {
            get
            {
                return fInstrument;
            }
            set
            {
                SetPropertyValue(nameof(Instrument), ref fInstrument, value);
            }
        }
        #endregion

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Modules.BusinessObjects.Assets.Labware> LabwareDataSource
        {
            get
            {
                if (Department != null /*&& Instrument == null*/)
                {
                    XPCollection<Modules.BusinessObjects.Assets.Labware> objLabware = new XPCollection<Modules.BusinessObjects.Assets.Labware>(Session, CriteriaOperator.Parse("Contains([Department], ?)", Department.Oid.ToString().Replace(" ", "")));// "[DepartmentUsed.Name] =?", Department.Name));
                    foreach (Modules.BusinessObjects.Assets.Labware test in objLabware.ToList())
                    {
                        objLabware.Add(test);
                    }
                    return objLabware;
                }
                else
                {
                    return null;
                }
            }
        }

        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "Instrument" && LabwareDataSource != null && LabwareDataSource.Count > 0)
            {
                foreach (Modules.BusinessObjects.Assets.Labware objLabware in LabwareDataSource.Where(i => i.LabwareName != null).OrderBy(i => i.LabwareName).ToList())
                {
                    if (!Properties.ContainsKey(objLabware.Oid))
                    {
                        Properties.Add(objLabware.Oid, objLabware.LabwareName);
                    }
                }
            }
            return Properties;
        }
        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        #endregion

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Method> MethodDataSource
        {
            get
            {

                XPCollection<Method> objmethod = new XPCollection<Method>(Session, CriteriaOperator.Parse("[Oid] Is Not Null"));
                return objmethod;
            }
        }

        #region ActiveDate
        private DateTime _ActiveDate;
        public DateTime ActiveDate
        {
            get { return _ActiveDate; }
            set { SetPropertyValue("ActiveDate", ref _ActiveDate, value); }
        }
        #endregion

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
                SetPropertyValue(nameof(Description), ref fDescription, value);
            }
        }
        #endregion

        #region RetireDate
        private DateTime _RetireDate;
        public DateTime RetireDate
        {
            get { return _RetireDate; }
            set { SetPropertyValue("RetireDate", ref _RetireDate, value); }
        }
        #endregion

        private TestMethod _TestMethod;
        [Association("TestMethod-PrepMethods")]
        public TestMethod TestMethod
        {
            get { return _TestMethod; }
            set { SetPropertyValue("TestMethod", ref _TestMethod, value); }
        }

        //#region Manay-to-Many Collection Sampleparameter
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false),VisibleInLookupListView(false)]
        //[Association("Sampleparameter-PrepMethod")]
        //public XPCollection<SampleParameter> Sampleparameters
        //{
        //    get
        //    {
        //        return GetCollection<SampleParameter>(nameof(Sampleparameters));
        //    }
        //}
        //#endregion
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        private uint _Sort;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public uint Sort
        {
            get { return _Sort; }
            set { SetPropertyValue(nameof(Sort), ref _Sort, value); }
        }
        #region samples
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("#Samples")]
        public int NoOfPrepSamples
        {
            get
            {
                DefaultSetting objDefault = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True")).FirstOrDefault();
                if (objDefault != null && TestMethod != null && TestMethod.PrepMethods.Count > 0)
                {
                    if (Sort == 1)
                    {
                        return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("([Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [IsPrepMethodComplete]  = False And [Status] = 'PendingEntry' And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount] = 0 )", TestMethod.Oid)).Where(p => p.Samplelogin != null && p.TestHold==false).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                    }
                    else if (Sort == 2)
                    {
                        return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("([Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [IsPrepMethodComplete]  = False And [Status] = 'PendingEntry' And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount] = 1 )", TestMethod.Oid)).Where(p => p.Samplelogin != null && p.TestHold == false).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                    }
                    else
                    {
                        return 0;
                    }
                    //[Testparameter.TestMethod.Prep Methods][[Tier] = 1u]

                    //OR([Testparameter.TestMethod.PrepMethods][].Count() = 2 And(([PrepMethodCount] = 0 And[Testparameter.TestMethod.PrepMethods][[Tier] = 1])  ) )
                    //OR([PrepMethodCount] = 1 And[Testparameter.TestMethod.PrepMethods][[Tier] = 2u])
                    //return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [IsPrepMethodComplete]  = False And [Status] = 'PendingEntry' And ([Testparameter.TestMethod.PrepMethods][].Count() > 0 And ([Testparameter.TestMethod.PrepMethods][].Count() = 1 And [PrepMethodCount] = 0) OR ([Testparameter.TestMethod.PrepMethods][].Count() = 2 And (([PrepMethodCount] =0 And [Testparameter.TestMethod.PrepMethods][[Tier] = 1u]) OR ([PrepMethodCount] =1 And [Testparameter.TestMethod.PrepMethods][[Tier] = 2u]) ) ) ) ", TestMethod.Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                    //return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [IsPrepMethodComplete]  = False And [Status] = 'PendingEntry' And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And (([Testparameter.TestMethod.PrepMethods][].Count() = 1 And [PrepMethodCount] = 0) OR  ([Testparameter.TestMethod.PrepMethods][].Count() = 2 And  (([PrepMethodCount] = 0 And [SamplePrepBatchID] Is Null) OR ([PrepMethodCount] = 1 And [SamplePrepBatchID]  Is Not Null) ) )  ) ", TestMethod.Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                    //return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("([Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [IsPrepMethodComplete]  = False And [Status] = 'PendingEntry' And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [Testparameter.TestMethod.PrepMethods][[TestMethod.PrepMethods][[Tier] = 1]].Count() = 2 And [PrepMethodCount] = 0 )", TestMethod.Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();

                }
                else
                {
                    return 0;
                }

            }
        }
        #endregion
        #region PreparationStatus
        private PrepStatus _PreparationStatus;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        [NonPersistent]
        public PrepStatus PreparationStatus
        {

            get
            {
                if (Tier == 1)
                {
                    _PreparationStatus = PrepStatus.PendingTier1Prep;
                }
                else if (Tier == 2)
                {
                    _PreparationStatus = PrepStatus.PendingTier2Prep;
                }
                return _PreparationStatus;
            }

        }
        #endregion
    }
}