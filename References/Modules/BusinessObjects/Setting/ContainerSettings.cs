using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ContainerSettings : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ContainerSettings(Session session)
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
        #region CPCode
        private string _CPCode;
        [XafDisplayName("CSCode")]
        public string CPCode
        {
            get { return _CPCode; }
            set { SetPropertyValue("CPCode", ref _CPCode, value); }
        }
        #endregion

        #region Matrix
        private Matrix _Matrix;
        [ImmediatePostData]
        //[DataSourceProperty(nameof(MatrixDataSource))]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue("Matrix", ref _Matrix, value); }
        }

        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<Matrix> MatrixDataSource
        //{
        //    get
        //    {
        //        if (Matrix == null /*&& Matrix != null && Matrix != null*/)
        //        {
        //            XPCollection<Matrix> matrixs = new XPCollection<Matrix>(Session, CriteriaOperator.Parse("Not IsNullOrEmpty([MatrixName])"));
        //            List<Guid> lstmethod = new List<Guid>();
        //            List<string> ids = matrixs.Select(i => i.MatrixName.ToString()).Distinct().ToList();
        //            foreach (string objids in ids.ToList())//tests.Where(a => a.TestName !=null).Distinct())
        //            {
        //                Matrix objtm = Session.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName] = ?", objids));
        //                lstmethod.Add(objtm.Oid);
        //            }
        //            List<Guid> ids1 = lstmethod.Select(i => new Guid(i.ToString())).ToList();
        //            return new XPCollection<Matrix>(Session, new InOperator("Oid", ids1));
        //            //return tests;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        #endregion 

        #region Test
        private TestMethod _Test;
        [ImmediatePostData]
        // [DataSourceProperty(nameof(TestDataSource))]
        public TestMethod Test
        {
            get { return _Test; }
            set { SetPropertyValue("Test", ref _Test, value); }
        }


        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (Matrix != null && Test == null)
                {
                    List<object> groups = new List<object>();
                    using (XPView lstview = new XPView(Session, typeof(TestMethod)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And([IsGroup] <> True Or [IsGroup] Is Null)", Matrix.MatrixName);
                        lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                    }
                    if (groups.Count == 0)
                    {
                        XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]=?", Matrix.MatrixName));
                        return tests;
                    }
                    else
                    {
                        XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
                        return tests;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion 

        #region Method
        private TestMethod _Method;
        [ImmediatePostData]
        //[DataSourceProperty(nameof(MethodDataSource))]
        public TestMethod Method
        {
            get { return _Method; }
            set { SetPropertyValue("Method", ref _Method, value); }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<TestMethod> MethodDataSource
        {
            get
            {
                if (Test != null && Method == null && Matrix != null)
                {
                    return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName]=?", Test.TestName, Matrix.MatrixName));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Container
        private string _Container;
        [Size(SizeAttribute.Unlimited)]
        [RuleRequiredField("select container", DefaultContexts.Save)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string Container
        {
            get { return _Container; }
            set { SetPropertyValue("Container", ref _Container, value); }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Container> ContainerDatasource
        {
            get
            {
                if (Test != null && Matrix != null && Method != null)
                {
                    List<Guid> lstguid = new List<Guid>();
                    TestMethod objtm = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And  [TestName] = ? And [MethodName.MethodNumber] = ?", Matrix.MatrixName, Test.TestName, Method.MethodName.MethodNumber));
                    if (objtm != null)
                    {
                        XPCollection<TestGuide> tstguide = new XPCollection<TestGuide>(Session, CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid));
                        List<string> ids = tstguide.Select(i => i.Container.ContainerName.ToString()).Distinct().ToList();
                        foreach (string ojtstrCon in ids.ToList())
                        {
                            Container objcontainer = Session.FindObject<Container>(CriteriaOperator.Parse("[ContainerName] = ?", ojtstrCon));
                            lstguid.Add(objcontainer.Oid);
                        }
                        return new XPCollection<Container>(Session, new InOperator("Oid", lstguid));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region DefaultContainer
        private Container _DefaultContainer;
        [ImmediatePostData]
        [DataSourceProperty(nameof(DContainerDataSource))]
        public Container DefaultContainer
        {
            get { return _DefaultContainer; }
            set { SetPropertyValue("DefaultContainer", ref _DefaultContainer, value); }
        }


        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Container> DContainerDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(Container))
                {
                    List<Guid> lstcontainer = new List<Guid>();
                    string[] strconarr = Container.Split(';');
                    if (strconarr != null && strconarr.Length == 1)
                    {
                        foreach (string strcon in strconarr)
                        {
                            lstcontainer.Add(new Guid(strcon.Trim()));
                        }
                        if (lstcontainer.Count > 0)
                        {
                            lstcontainer.Sort();
                            XPCollection<Container> tests = new XPCollection<Container>(Session, new InOperator("Oid", lstcontainer));
                            return tests;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    if (strconarr != null && strconarr.Length > 1)
                    {
                        foreach (string strcon in strconarr)
                        {
                            lstcontainer.Add(new Guid(strcon.Trim()));
                        }
                        if (lstcontainer.Count > 0)
                        {
                            lstcontainer.Sort();
                            XPCollection<Container> tests = new XPCollection<Container>(Session, new InOperator("Oid", lstcontainer));
                            return tests;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion 

        #region Preservative
        private string _Preservative;
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string Preservative
        {
            get { return _Preservative; }
            set { SetPropertyValue("Preservative", ref _Preservative, value); }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Preservative> PreservativeDatasource
        {
            get
            {
                if (Test != null && Matrix != null && Method != null)
                {
                    List<Guid> lstguid = new List<Guid>();
                    TestMethod objtm = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And  [TestName] = ? And [MethodName.MethodNumber] = ?", Matrix.MatrixName, Test.TestName, Method.MethodName.MethodNumber));
                    if (objtm != null)
                    {
                        XPCollection<TestGuide> tstguide = new XPCollection<TestGuide>(Session, CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid));
                        List<string> ids = tstguide.Select(i => i.Preservative.PreservativeName.ToString()).Distinct().ToList();
                        foreach (string ojtstrprev in ids.ToList())
                        {
                            Preservative objpreser = Session.FindObject<Preservative>(CriteriaOperator.Parse("[PreservativeName] = ?", ojtstrprev));
                            lstguid.Add(objpreser.Oid);
                        }
                        return new XPCollection<Preservative>(Session, new InOperator("Oid", lstguid));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "Preservative" && PreservativeDatasource != null && PreservativeDatasource.Count > 0)
            {
                foreach (Preservative objtat in PreservativeDatasource.Where(i => i.PreservativeName != null).OrderBy(i => i.PreservativeName).ToList())
                {
                    if (!Properties.ContainsKey(objtat.Oid) && !string.IsNullOrEmpty(objtat.PreservativeName))
                    {
                        Properties.Add(objtat.Oid, objtat.PreservativeName);
                    }
                }
            }
            if (targetMemberName == "Container" && ContainerDatasource != null && ContainerDatasource.Count > 0)
            {
                foreach (Container objtat in ContainerDatasource.Where(i => i.ContainerName != null).OrderBy(i => i.ContainerName).ToList())
                {
                    if (!Properties.ContainsKey(objtat.Oid) && !string.IsNullOrEmpty(objtat.ContainerName))
                    {
                        Properties.Add(objtat.Oid, objtat.ContainerName);
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

        #region HTBeforePrep
        private HoldingTimes _HTBeforePrep;
        public HoldingTimes HTBeforePrep
        {
            get { return _HTBeforePrep; }
            set { SetPropertyValue("HTBeforePrep", ref _HTBeforePrep, value); }
        }
        #endregion

        #region HTBeforeAnalysis
        private HoldingTimes _HTBeforeAnalysis;
        public HoldingTimes HTBeforeAnalysis
        {
            get { return _HTBeforeAnalysis; }
            set { SetPropertyValue("HTBeforeAnalysis", ref _HTBeforeAnalysis, value); }
        }
        #endregion 

        #region SetPreTimeAsAnalysisTime
        private bool _SetPreTimeAsAnalysisTime;
        public bool SetPreTimeAsAnalysisTime
        {
            get { return _SetPreTimeAsAnalysisTime; }
            set { SetPropertyValue("SetPreTimeAsAnalysisTime", ref _SetPreTimeAsAnalysisTime, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #endregion
    }
}