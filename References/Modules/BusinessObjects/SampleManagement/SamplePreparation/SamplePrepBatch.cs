using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Method = Modules.BusinessObjects.Setting.Method;

namespace Modules.BusinessObjects.SampleManagement.SamplePreparation
{
    public enum PrepBatchQueueStatus
    {
        [XafDisplayName("Pending Tier 1 Prep")]
        PendingTier1Prep,
        [XafDisplayName("Pending Tier 2 Prep")]
        PendingTier2Prep,
        [XafDisplayName("Pending Result Entry")]
        PendingResultEntry,
        [XafDisplayName("Result Completed")]
        ResultCompleted
    }

    [DefaultClassOptions]
    public class SamplePrepBatch : BaseObject, ICheckedListBoxItemsProvider
    {
        public SamplePrepBatch(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            Datecreated = Libraries.Library.GetServerTime(Session);
        }

        #region PrepBatchID
        private string _PrepBatchID;
        public string PrepBatchID
        {
            get { return _PrepBatchID; }
            set { SetPropertyValue(nameof(PrepBatchID), ref _PrepBatchID, value); }
        }
        #endregion

        #region Datecreated
        private DateTime _Datecreated;
        public DateTime Datecreated
        {
            get { return _Datecreated; }
            set { SetPropertyValue(nameof(Datecreated), ref _Datecreated, value); }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion

        #region Matrix
        private string _Matrix;
        //[ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public string Matrix
        {
            get
            {
                return _Matrix;
            }
            set
            {
                SetPropertyValue(nameof(Matrix), ref _Matrix, value);
                Method = null;
                Test = null;
            }
        }
        #endregion

        #region Test
        private string _Test;
        [DataSourceProperty(nameof(TestDataSource))]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        //[ImmediatePostData]
        public string Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        #endregion

        #region Method
        private string _Method;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [DataSourceProperty(nameof(MethodDataSource))]
        [Size(SizeAttribute.Unlimited)]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        //[ImmediatePostData]
        public string Method
        {
            get { return _Method; }
            set { SetPropertyValue(nameof(Method), ref _Method, value); }
        }
        #endregion

        #region Instrument
        private string _Instrument;
        //[DataSourceProperty(nameof(InstrumentDataSource))]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Instrument
        {
            get { return _Instrument; }
            set { SetPropertyValue("Instrument", ref _Instrument, value); }
        }
        #endregion
        private string _NPInstrument;
        [VisibleInListView(false)]
        [NonPersistent]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [Appearance("ABNPInstrument", Visibility = ViewItemVisibility.Hide, Criteria = "!ISShown", Context = "DetailView")]
        public string NPInstrument
        {
            get
            {
                if (!Session.IsNewObject(this) && !string.IsNullOrEmpty(Instrument))
                {
                    _NPInstrument = Instrument;
                }
                return _NPInstrument;
            }
            set { SetPropertyValue("NPInstrument", ref _NPInstrument, value); }
        }
        private string _strInstrument;
        [Appearance("Instrument", Visibility = ViewItemVisibility.Hide, Criteria = "ISShown", Context = "DetailView")]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public string strInstrument
        {
            get
            {
                //if (!string.IsNullOrEmpty(Instrument))
                //{
                //    //XPCollection<Labware> lstInstruments = new XPCollection<Labware>(Session);
                //    //lstInstruments.Criteria = new InOperator("Oid", Instrument.Split(';').Select(i => new Guid(i.Trim())));
                //    //if (lstInstruments != null && lstInstruments.Count > 0)
                //    //{
                //    //    _strInstrument = string.Join(",", lstInstruments.Select(i => i.AssignedName).Distinct().OrderBy(i => i).ToList());
                //    //}
                //}
                return _strInstrument;
            }
            set { SetPropertyValue("strInstrument", ref _strInstrument, value); }
        }
        #region Temperature
        private string _Temperature;
        public string Temperature
        {
            get { return _Temperature; }
            set { SetPropertyValue(nameof(Temperature), ref _Temperature, value); }
        }
        #endregion

        #region Humidity
        private string _Humidity;
        public string Humidity
        {
            get { return _Humidity; }
            set { SetPropertyValue(nameof(Humidity), ref _Humidity, value); }
        }
        #endregion

        //#region JobID
        //private string _Jobid;
        ////[Appearance("Jobid", Visibility = ViewItemVisibility.Hide, Criteria = "ISShown", Context = "DetailView")]
        //[EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[Size(SizeAttribute.Unlimited)]
        //[RuleRequiredField]
        //public string Jobid
        //{
        //    get { return _Jobid; }
        //    set { SetPropertyValue("Jobid", ref _Jobid, value); }
        //}
        //#endregion

        #region CustomDataSources
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                //if (Matrix != null && Test == null && Matrix != null)
                //if (Matrix != null)
                //{
                //    //XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]=? and [PrepMethods][].Count() > 0", Matrix.MatrixName));
                //    //return tests;
                //}
                //else
                //{
                //    return null;
                //}
                if (Matrix != null)
                {
                    if (!string.IsNullOrEmpty(Matrix))
                    {
                        List<string> lstSM = new List<string>();
                        List<string> lstSMOid = Matrix.Split(';').ToList();
                        if (lstSMOid != null)
                        {
                            foreach (string objOid in lstSMOid)
                            {
                                if (!string.IsNullOrEmpty(objOid))
                                {
                                    Matrix objVM = Session.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                    if (objVM != null && !lstSM.Contains(objVM.MatrixName))
                                    {
                                        lstSM.Add(objVM.MatrixName);
                                    }
                                }

                            }
                        }
                        //return new XPCollection<TestMethod>(Session,new InOperator("MatrixName.MatrixName", lstSM));
                        return new XPCollection<TestMethod>(Session,new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[IsFieldTest] Is Null Or [IsFieldTest] = False"), new InOperator("MatrixName.MatrixName", lstSM)));

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

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> MethodDataSource
        {
            get
            {
                if (Test != null && Matrix != null && !string.IsNullOrEmpty(Matrix))
                {
                    List<string> lstSM = new List<string>();
                    List<string> lstSMOid = Matrix.Split(';').ToList();
                    if (lstSMOid != null)
                    {
                        foreach (string objOid in lstSMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Matrix objVM = Session.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                if (objVM != null && !lstSM.Contains(objVM.MatrixName))
                                {
                                    lstSM.Add(objVM.MatrixName);
                                }
                            }

                        }
                    }
                    List<string> lstTest = new List<string>();
                    List<string> lstTestOid = Test.Split(';').ToList();
                    if (lstTestOid != null)
                    {
                        foreach (string objOid in lstTestOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                TestMethod objTM = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                if (objTM != null && !lstTest.Contains(objTM.TestName))
                                {
                                    lstTest.Add(objTM.TestName);
                                }
                            }

                        }
                    }
                    //return new XPCollection<TestMethod>(Session, new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[PrepMethods][].Count() > 0"), new InOperator("MatrixName.MatrixName", lstSM),
                    //    new InOperator("TestName",lstTest)));
                    return new XPCollection<TestMethod>(Session, new GroupOperator(GroupOperatorType.And, new InOperator("MatrixName.MatrixName", lstSM),
                        new InOperator("TestName", lstTest)));

                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName]=? and [PrepMethods][].Count() > 0", Test.TestName, Matrix.MatrixName));
                }
                else
                {
                    return null;
                }
            }
        }

        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<SampleParameter> JobidDataSource
        //{
        //    get
        //    {
        //        if (Method != null && string.IsNullOrEmpty(PrepBatchID))
        //        {
        //            TestMethod objtestmethod = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?",Method.Oid));
        //            if(objtestmethod != null && objtestmethod.PrepMethods.Count > 0)
        //            {
        //                return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? AND [SamplePrepBatchID] IS NULL", Method.Oid));
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Labware> InstrumentDataSource
        {
            get
            {
                return new XPCollection<Labware>(Session, CriteriaOperator.Parse(""));
            }
        }
        #endregion
        #region MatrixDataSource
        [Browsable(false)]
        //[ImmediatePostData]
        [NonPersistent]
        public XPCollection<Matrix> SampleMatrixes
        {
            get
            {
                return new XPCollection<Matrix>(Session, CriteriaOperator.Parse(""));
            }
        }
        #endregion
        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> properties = new Dictionary<object, string>();

            if (targetMemberName == "Instrument" && Test != null && Matrix != null && Method != null)
            {
                List<string> lstTest = new List<string>();
                List<string> lstTestOid = Test.Split(';').ToList();
                if (lstTestOid != null)
                {
                    foreach (string objOid in lstTestOid)
                    {
                        if (!string.IsNullOrEmpty(objOid))
                        {
                            TestMethod objTest = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                            if (objTest != null && !lstTest.Contains(objTest.TestName))
                            {
                                lstTest.Add(objTest.TestName);
                            }
                        }

                    }
                }
                List<string> lstMatrix = new List<string>();
                List<string> lstMatrixOid = Matrix.Split(';').ToList();
                if (lstMatrixOid != null)
                {
                    foreach (string objOid in lstMatrixOid)
                    {
                        if (!string.IsNullOrEmpty(objOid))
                        {
                            Matrix objTest = Session.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                            if (objTest != null && !lstMatrix.Contains(objTest.MatrixName))
                            {
                                lstMatrix.Add(objTest.MatrixName);
                            }
                        }

                    }
                }
                List<string> lstMethod = new List<string>();
                List<string> lstMethodOid = Method.Split(';').ToList();
                if (lstMethodOid != null)
                {
                    foreach (string objOid in lstMethodOid)
                    {
                        if (!string.IsNullOrEmpty(objOid))
                        {
                            Method objTest = Session.GetObjectByKey<Method>(new Guid(objOid.Trim()));
                            if (objTest != null && !lstMethod.Contains(objTest.MethodNumber))
                            {
                                lstMethod.Add(objTest.MethodNumber);
                            }
                        }

                    }
                }
                XPCollection<TestMethod> lst = new XPCollection<TestMethod>(Session, new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[PrepMethods][].Count() > 0"), new InOperator("MatrixName.MatrixName", lstMatrix),
                        new InOperator("TestName", lstTest), new InOperator("MethodName.MethodNumber", lstMethod)));
                foreach (TestMethod objTest in lst.ToList())
                {
                    foreach (Labware objlab in objTest.Labwares.OrderBy(a => a.AssignedName).ToList())
                    {
                        if (!properties.ContainsKey(objlab.Oid) && !string.IsNullOrEmpty(objlab.AssignedName))
                        {
                            properties.Add(objlab.Oid, objlab.AssignedName);
                        }
                    }
                }
                if (Instrument != null)
                {
                    string[] strinstrument = Instrument.Split(';');
                    foreach (string strobj in strinstrument)
                    {
                        Labware objlab = Session.FindObject<Labware>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strobj)));
                        if (!properties.ContainsKey(objlab.Oid) && !string.IsNullOrEmpty(objlab.AssignedName))
                        {
                            properties.Add(objlab.Oid, objlab.AssignedName);
                        }
                    }
                }
            }
            if (targetMemberName == "Test" && Matrix != null && TestDataSource != null && TestDataSource.Count > 0)
            {
                foreach (TestMethod objTest in TestDataSource.Where(i => i.TestName != null).OrderBy(i => i.TestName).ToList())
                {
                    if (!properties.ContainsKey(objTest.Oid))
                    {
                        properties.Add(objTest.Oid, objTest.TestName);
                    }
                }
            }
            if (targetMemberName == "Matrix" && SampleMatrixes != null && SampleMatrixes.Count > 0)
            {
                properties = SampleMatrixes.OrderBy(i => i.MatrixName).ToDictionary(x => (Object)x.Oid, x => x.MatrixName);
            }
            if (targetMemberName == "Method" && Matrix != null && Test != null && MethodDataSource != null && MethodDataSource.Count > 0)
            {
                //foreach (TestMethod objTest in TestDataSource.Where(i => i.MethodName != null && i.MethodName.MethodNumber != null).OrderBy(i => i.MethodName.MethodNumber).ToList())
                //{
                //    if (!properties.ContainsValue(objTest.MethodName.MethodNumber))
                //    {
                //        properties.Add(objTest.Oid, objTest.MethodName.MethodNumber);
                //    }
                //}
                properties = MethodDataSource.Where(i => i.MethodName != null).OrderBy(i => i.MethodName.MethodNumber).ToDictionary(x => (Object)x.MethodName.Oid, x => x.MethodName.MethodNumber);

            }
            return properties;
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

        #region SamplePrepBatchlink
        [Association("SamplePrepBatchlink")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<SamplePrepBatchSequence> SamplePrepBatchSeqDetail
        {
            get { return GetCollection<SamplePrepBatchSequence>(nameof(SamplePrepBatchSeqDetail)); }
        }
        #endregion

        #region Reagents
        [Association("SamplePrepBatchReagents", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Reagent> Reagents
        {
            get
            {
                return GetCollection<Reagent>(nameof(Reagents));
            }
        }
        #endregion

        #region Instruments
        [Association("SamplePrepBatchInstruments", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Labware> Instruments
        {
            get
            {
                return GetCollection<Labware>(nameof(Instruments));
            }
        }
        #endregion

        #region Remarks
        private string _Remarks;
        [Size(SizeAttribute.Unlimited)]
        public string Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                SetPropertyValue(nameof(Remarks), ref _Remarks, value);
            }
        }
        #endregion
        #region Tier
        private uint _Tier;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        public uint Tier
        {
            get
            {
                return _Tier;
            }
            set
            {
                SetPropertyValue(nameof(Tier), ref _Tier, value);
            }
        }
        #endregion
        #region PrepTypes
        private PrepTypes _PrepType;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        public PrepTypes PrepType
        {
            get { return _PrepType; }
            set { SetPropertyValue(nameof(PrepTypes), ref _PrepType, value); }
        }
        #endregion
        private string _NPJobid;
        [NonPersistent]
        [Appearance("PrepNPJobid", Visibility = ViewItemVisibility.Hide, Criteria = "!ISShown", Context = "DetailView")]
        [RuleRequiredField("PrepNPJobid", DefaultContexts.Save)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string NPJobid
        {
            get { return _NPJobid; }
            set { SetPropertyValue("NPJobid", ref _NPJobid, value); }
        }

        private string _Jobid;
        [Appearance("PrepJobid", Visibility = ViewItemVisibility.Hide, Criteria = "ISShown", Context = "DetailView")]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string Jobid
        {
            get { return _Jobid; }
            set { SetPropertyValue("Jobid", ref _Jobid, value); }
        }

        private bool _ISShown;
        [NonPersistent]
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        public bool ISShown
        {
            get { return _ISShown; }
            set { SetPropertyValue("ISShown", ref _ISShown, value); }
        }
        #region sort
        private uint _Sort;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        public uint Sort
        {
            get { return _Sort; }
            set { SetPropertyValue("Sort", ref _Sort, value); }
        }
        #endregion

        #region PrepBatchStatus
        private PrepBatchQueueStatus _PrepBatchStatus;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        [NonPersistent]
        public PrepBatchQueueStatus PrepBatchStatus
        {
            get
            {
                if (!Session.IsObjectsSaving)
                {
                    XPCollection<SampleParameter> lstSampleParams = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("Contains([PrepBatchID], ?)", Oid.ToString()));
                    
                    if (lstSampleParams.FirstOrDefault(i => !i.IsPrepMethodComplete) != null)
                    {
                        if (Tier == 1)
                        {
                            _PrepBatchStatus = PrepBatchQueueStatus.PendingTier2Prep;
                        }
                        else
                        {
                            _PrepBatchStatus = PrepBatchQueueStatus.PendingTier1Prep;
                        }
                    }
                    else if (lstSampleParams.FirstOrDefault(i => string.IsNullOrEmpty(i.Result)) == null)
                    {
                        _PrepBatchStatus = PrepBatchQueueStatus.ResultCompleted;
                    }
                    else
                    {
                        _PrepBatchStatus = PrepBatchQueueStatus.PendingResultEntry;
                    }                    
                }
                return _PrepBatchStatus;
            }
            set { SetPropertyValue("PrepBatchStatus", ref _PrepBatchStatus, value); }
        }
        #endregion
    }
}