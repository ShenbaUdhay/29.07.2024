using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Method = Modules.BusinessObjects.Setting.Method;

namespace Modules.BusinessObjects.EDD
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EDDReportGenerator : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        EDDInfo objEDDInfo = new EDDInfo();
        public EDDReportGenerator(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        public event EventHandler ItemsChanged;

        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> objdic = new Dictionary<object, string>();
            //if (targetMemberName == "Client" && Clients != null && Clients.Count > 0)
            //{
            //    foreach (Customer objCs in Clients.Where(i => i.CustomerName != null).OrderBy(i => i.CustomerName).ToList())
            //    {
            //        if (!objdic.ContainsKey(objCs.Oid))
            //        {
            //            objdic.Add(objCs.Oid, objCs.CustomerName);
            //        }
            //    }
            //}
            //if (targetMemberName == "JobID" && JobIDs != null && JobIDs.Count > 0)
            //{
            //    foreach (Samplecheckin objSc in JobIDs.Where(i => i.JobID != null).OrderBy(i => i.JobID).ToList())
            //    {
            //        if (!objdic.ContainsKey(objSc.Oid))
            //        {
            //            objdic.Add(objSc.Oid, objSc.JobID);
            //        }
            //    }
            //}
            //if (targetMemberName == "ProjectID" && ProjectIDs != null && ProjectIDs.Count > 0)
            //{
            //    foreach (Project objPt in ProjectIDs.Where(i => i.ProjectId != null).OrderBy(i => i.ProjectId).ToList())
            //    {
            //        if (!objdic.ContainsKey(objPt.Oid))
            //        {
            //            objdic.Add(objPt.Oid, objPt.ProjectId);
            //        }
            //    }
            //}
            if (targetMemberName == "ProjectName" && ProjectNames != null && ProjectNames.Count > 0)
            {
                foreach (Project objPt in ProjectNames.Where(i => i.ProjectName != null).OrderBy(i => i.ProjectName).ToList())
                {
                    if (!objdic.ContainsKey(objPt.Oid)&& !objdic.ContainsValue(objPt.ProjectName))
                    {
                        objdic.Add(objPt.Oid, objPt.ProjectName);
                    }
                }
            }
            if (targetMemberName == "SampleCategory" && SampleCategorys != null && SampleCategorys.Count > 0)
            {
                foreach (SampleCategory objSct in SampleCategorys.Where(i => i.SampleCategoryName != null).OrderBy(i => i.SampleCategoryName).ToList())
                {
                    if (!objdic.ContainsKey(objSct.Oid)&&!objdic.ContainsValue(objSct.SampleCategoryName))
                    {
                        objdic.Add(objSct.Oid, objSct.SampleCategoryName);
                    }
                }
            }
            if (targetMemberName == "Test" && Tests != null && Tests.Count > 0)
            {
                foreach (TestMethod objTm in Tests.Where(i => i.TestName != null).OrderBy(i => i.TestName).ToList())
                {
                    if (!objdic.ContainsKey(objTm.Oid) && !objdic.ContainsValue(objTm.TestName))
                    {
                        objdic.Add(objTm.Oid, objTm.TestName);
                    }
                }
            }
            if (targetMemberName == "Method" && Methods != null && Methods.Count > 0)
            {
                foreach (Method objMd in Methods.Where(i => i.MethodNumber != null).OrderBy(i => i.MethodNumber).ToList())
                {
                    if (!objdic.ContainsKey(objMd.Oid) && !objdic.ContainsValue(objMd.MethodNumber))
                    {
                        objdic.Add(objMd.Oid, objMd.MethodNumber);
                    }
                }
            }
            return objdic;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(EddReportID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(EddReportID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(EDDReportGenerator), criteria, null)) + 1).ToString();
                var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "1")
                {
                    var predate = tempID.Substring(0, 6);
                    if (predate == curdate)
                    {
                        tempID = "ED" + tempID;
                    }
                    else
                    {
                        tempID = "ED" + curdate + "001";
                    }
                }
                else
                {
                    tempID = "ED" + curdate + "001";
                }
                EddReportID = tempID;
            }
        }


        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        #region EDDReportID
        private string _EddReportID;
        public string EddReportID
        {
            get { return _EddReportID; }
            set { SetPropertyValue(nameof(EddReportID), ref _EddReportID, value); }
        }
        #endregion

        #region EddTemplate
        private EDDBuilder _EddTemplate;
        [ImmediatePostData]
        [RuleRequiredField]
        public EDDBuilder EddTemplate
        {
            get { return _EddTemplate; }
            set { SetPropertyValue(nameof(EddTemplate), ref _EddTemplate, value); }
        }
        #endregion

        #region EDDQueryBuilder
        private EDDQueryBuilder _EDDQueryBuilder;
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DataSourceProperty("EDDQueryBuilderDataSource")]
        public EDDQueryBuilder EDDQueryBuilder
        {
            get { return _EDDQueryBuilder; }
            set { SetPropertyValue(nameof(EDDQueryBuilder), ref _EDDQueryBuilder, value); }
        }
        [Browsable(false)]
        public XPCollection<EDDQueryBuilder> EDDQueryBuilderDataSource
        {
            get
            {
                //XPCollection<EDDQueryBuilder> lsteDDQueryBuilders = new XPCollection<EDDQueryBuilder>(Session);
                //if (lsteDDQueryBuilders != null && lsteDDQueryBuilders.Count > 0 && objEDDInfo.EddBuildOid != null && objEDDInfo.EddBuildOid != Guid.Empty)
                //{
                //    return new XPCollection<EDDQueryBuilder>(Session, CriteriaOperator.Parse("[EDDBuilder.Oid] = ?", objEDDInfo.EddBuildOid));
                //}
                if (EddTemplate != null)
                {
                    return new XPCollection<EDDQueryBuilder>(Session, CriteriaOperator.Parse("[EDDBuilder.Oid] = ?", EddTemplate.Oid));
                }
                else
                {
                    return new XPCollection<EDDQueryBuilder>(Session, CriteriaOperator.Parse("[Oid] is NULL"));
                }
            }
        }
        #endregion

        #region Client
        private string _Client;
        [ImmediatePostData]
        public string Client
        {
            get { return _Client; }
            set { SetPropertyValue(nameof(Client), ref _Client, value); }
        }
        private string _NPClient;
        [VisibleInListView(false)]
        [NonPersistent]
        //[Appearance("ABNPJobid", Visibility = ViewItemVisibility.Hide, Criteria = "!ISShown", Context = "DetailView")]
        public string NPClient
        {
            get
            {
                if (!Session.IsNewObject(this) && !string.IsNullOrEmpty(Client))
                {
                    _NPClient = Client;
                }
                return _NPClient;
            }
            set { SetPropertyValue("NPClient", ref _NPClient, value); }
        }
        //public XPCollection<Customer> Clients
        //{
        //    get
        //    {
        //        if (EddTemplate != null && !string.IsNullOrEmpty(JobID))
        //        {
        //            List<string> lstStr = new List<string>();
        //            List<string> lstStrOid = JobID.Split(';').ToList();
        //            if (lstStrOid != null)
        //            {
        //                foreach (string objOid in lstStrOid)
        //                {
        //                    Samplecheckin objSCI = Session.GetObjectByKey<Samplecheckin>(new Guid(objOid.Trim()));
        //                    if (objSCI != null && objSCI.ClientName != null)
        //                    {
        //                        Customer objCtm = Session.GetObjectByKey<Customer>(objSCI.ClientName.Oid);
        //                        if (objCtm != null && !lstStr.Contains(objCtm.CustomerName))
        //                        {
        //                            lstStr.Add(objCtm.CustomerName);
        //                        }
        //                    }

        //                }
        //            }
        //            return new XPCollection<Customer>(Session, new InOperator("CustomerName", lstStr));
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        #endregion

        #region ALLTemplateEDDClient
        private string _ALLTemplateEDDClient;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        [ImmediatePostData]
        public string ALLTemplateEDDClient
        {
            get { return _ALLTemplateEDDClient; }
            set { SetPropertyValue(nameof(ALLTemplateEDDClient), ref _ALLTemplateEDDClient, value); }
        }
        #endregion

        #region ProjectID
        private string _ProjectID;
        [ImmediatePostData]
        //[EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string ProjectID
        {
            get { return _ProjectID; }
            set { SetPropertyValue(nameof(ProjectID), ref _ProjectID, value); }
        }
        private string _NPProjectID;
        [VisibleInListView(false)]
        [NonPersistent]
        //[Appearance("ABNPJobid", Visibility = ViewItemVisibility.Hide, Criteria = "!ISShown", Context = "DetailView")]
        public string NPProjectID
        {
            get
            {
                if (!Session.IsNewObject(this) && !string.IsNullOrEmpty(ProjectID))
                {
                    _NPProjectID = ProjectID;
                }
                return _NPClient;
            }
            set { SetPropertyValue("NPProjectID", ref _NPProjectID, value); }
        }
        //public XPCollection<Project> ProjectIDs
        //{
        //    //get
        //    //{
        //    //    if (EddTemplate != null && !string.IsNullOrEmpty(ALLTemplateEDDProjectID))
        //    //    {
        //    //        List<Guid> lstStr = new List<Guid>();
        //    //        List<string> lstStrOid = ALLTemplateEDDProjectID.Split(';').ToList();
        //    //        if (lstStrOid != null)
        //    //        {
        //    //            foreach (string objOid in lstStrOid)
        //    //            {
        //    //                if (!string.IsNullOrEmpty(objOid))
        //    //                {
        //    //                    Project objSCI = Session.GetObjectByKey<Project>(new Guid(objOid.Trim()));
        //    //                    if (objSCI != null && !lstStr.Contains(objSCI.Oid))
        //    //                    {
        //    //                        lstStr.Add(objSCI.Oid);
        //    //                    }
        //    //                }

        //    //            }
        //    //        }
        //    //        return new XPCollection<Project>(Session, new InOperator("Oid", lstStr));
        //    //    }
        //    //    else
        //    //    {
        //    //        return null;
        //    //    }
        //    //}
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(JobID))
        //        {
        //            List<string> lstSM = new List<string>();
        //            List<string> lstSMOid = JobID.Split(';').ToList();
        //            if (lstSMOid != null)
        //            {
        //                foreach (string objOid in lstSMOid)
        //                {
        //                    if (!string.IsNullOrEmpty(objOid))
        //                    {
        //                        Samplecheckin objSCI = Session.GetObjectByKey<Samplecheckin>(new Guid(objOid.Trim()));
        //                        if (objSCI != null && objSCI.ProjectID != null)
        //                        {
        //                            Project objP = Session.GetObjectByKey<Project>(objSCI.ProjectID.Oid);
        //                            if (objP != null && !lstSM.Contains(objP.ProjectId))
        //                            {
        //                                lstSM.Add(objP.ProjectId);
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //            return new XPCollection<Project>(Session, new InOperator("ProjectId", lstSM));
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        #endregion

        #region ALLTemplateEDDProjectID
        private string _ALLTemplateEDDProjectID;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        [ImmediatePostData]
        public string ALLTemplateEDDProjectID
        {
            get { return _ALLTemplateEDDProjectID; }
            set { SetPropertyValue(nameof(ALLTemplateEDDProjectID), ref _ALLTemplateEDDProjectID, value); }
        }
        #endregion

        #region ProjectName
        private string _ProjectName;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string ProjectName
        {
            get { return _ProjectName; }
            set { SetPropertyValue(nameof(ProjectName), ref _ProjectName, value); }
        }
        public XPCollection<Project> ProjectNames
        {
            get
            {
                if (EddTemplate != null && !string.IsNullOrEmpty(ALLTemplateEDDProjectName))
                {
                    List<Guid> lstStr = new List<Guid>();
                    List<string> lstStrOid = ALLTemplateEDDProjectName.Split(';').ToList();
                    if (lstStrOid != null)
                    {
                        foreach (string objOid in lstStrOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Project objSCI = Session.GetObjectByKey<Project>(new Guid(objOid.Trim()));
                                if (objSCI != null && !lstStr.Contains(objSCI.Oid))
                                {
                                    lstStr.Add(objSCI.Oid);
                                }
                            }

                        }
                    }
                    return new XPCollection<Project>(Session, new InOperator("Oid", lstStr));
                }
                else
                {
                    return null;
                }
            }
            //get
            //{
            //    if (!string.IsNullOrEmpty(ProjectID))
            //    {
            //        List<string> lstStr = new List<string>();
            //        List<string> lstStrOid = ProjectID.Split(';').ToList();
            //        if (lstStrOid != null)
            //        {
            //            foreach (string objOid in lstStrOid)
            //            {
            //                if (!string.IsNullOrEmpty(objOid))
            //                {
            //                    Samplecheckin objSCI = Session.GetObjectByKey<Samplecheckin>(new Guid(objOid.Trim()));
            //                    if (objSCI != null && objSCI.ProjectID != null && objSCI.ProjectName != null)
            //                    {
            //                        Project objP = Session.GetObjectByKey<Project>(objSCI.ProjectID.Oid);
            //                        if (objP != null && !lstStr.Contains(objP.ProjectName))
            //                        {
            //                            lstStr.Add(objP.ProjectName);
            //                        }
            //                    }
            //                }

            //            }
            //        }
            //        return new XPCollection<Project>(Session, new InOperator("ProjectName", lstStr));
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
        }
        #endregion

        #region ALLTemplateEDDProjectName
        private string _ALLTemplateEDDProjectName;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string ALLTemplateEDDProjectName
        {
            get { return _ALLTemplateEDDProjectName; }
            set { SetPropertyValue(nameof(ALLTemplateEDDProjectName), ref _ALLTemplateEDDProjectName, value); }
        }
        #endregion

        #region Test
        private string _Test;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [ImmediatePostData]
        public string Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        public XPCollection<TestMethod> Tests
        {
            get
            {
                if (EddTemplate != null && !string.IsNullOrEmpty(ALLTemplateEDDTest))
                {
                    List<Guid> lstStr = new List<Guid>();
                    List<string> lstStrOid = ALLTemplateEDDTest.Split(';').ToList();
                    if (lstStrOid != null)
                    {
                        foreach (string objOid in lstStrOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                TestMethod objSCI = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                if (objSCI != null && !lstStr.Contains(objSCI.Oid))
                                {
                                    lstStr.Add(objSCI.Oid);
                                }
                            }

                        }
                    }
                    return new XPCollection<TestMethod>(Session, new InOperator("Oid", lstStr));
                }
                else
                {
                    return null;
                }
            }
            //get
            //{
            //    if (!string.IsNullOrEmpty(JobID))
            //    {
            //        List<string> lstSM = new List<string>();
            //        List<Guid> lstoid = new List<Guid>();
            //        List<string> lstSMOid = JobID.Split(';').ToList();
            //        if (lstSMOid != null)
            //        {

            //            foreach (string objOid in lstSMOid)
            //            {
            //                XPCollection<SampleParameter> lstTests = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID]=?", new Guid(objOid.Trim())));
            //                //XPCollection<SampleParameter> lstTests = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", new Guid(objOid.Trim())));
            //                foreach (SampleParameter objSP in lstTests)
            //                {
            //                    Testparameter objTP = Session.GetObjectByKey<Testparameter>(objSP.Testparameter.Oid);
            //                    if (objTP != null && objTP.TestMethod != null)
            //                    {
            //                        TestMethod objTM = Session.GetObjectByKey<TestMethod>(objTP.TestMethod.Oid);
            //                        if (objTM != null && !lstSM.Contains(objTM.TestName))
            //                        {
            //                            lstSM.Add(objTM.TestName);
            //                            lstoid.Add(objTM.Oid);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        return new XPCollection<TestMethod>(Session, new InOperator("Oid", lstoid));
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
        }
        #endregion

        #region ALLTemplateEDDTest
        private string _ALLTemplateEDDTest;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string ALLTemplateEDDTest
        {
            get { return _ALLTemplateEDDTest; }
            set { SetPropertyValue(nameof(ALLTemplateEDDTest), ref _ALLTemplateEDDTest, value); }
        }
        #endregion

        #region Method
        private string _Method;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Method
        {
            get { return _Method; }
            set { SetPropertyValue(nameof(Method), ref _Method, value); }
        }
        public XPCollection<Method> Methods
        {
            get
            {
                if (EddTemplate != null && !string.IsNullOrEmpty(ALLTemplateEDDMethod))
                {
                    List<Guid> lstStr = new List<Guid>();
                    List<string> lstStrOid = ALLTemplateEDDMethod.Split(';').ToList();
                    if (lstStrOid != null)
                    {
                        foreach (string objOid in lstStrOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Method objSCI = Session.GetObjectByKey<Method>(new Guid(objOid.Trim()));
                                if (objSCI != null && !lstStr.Contains(objSCI.Oid))
                                {
                                    lstStr.Add(objSCI.Oid);
                                }
                            }

                        }
                    }
                    return new XPCollection<Method>(Session, new InOperator("Oid", lstStr));
                }
                else
                {
                    return null;
                }
            }
            //get
            //{
            //    if (!string.IsNullOrEmpty(JobID))
            //    {
            //        List<string> lstSM = new List<string>();
            //        List<string> lstSMOid = JobID.Split(';').ToList();
            //        if (lstSMOid != null)
            //        {

            //            foreach (string objOid in lstSMOid)
            //            {
            //                XPCollection<SampleParameter> lstTests = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", new Guid(objOid.Trim())));
            //                foreach (SampleParameter objSP in lstTests)
            //                {
            //                    Testparameter objTP = Session.GetObjectByKey<Testparameter>(objSP.Testparameter.Oid);
            //                    if (objTP != null && objTP.TestMethod != null)
            //                    {
            //                        TestMethod objTM = Session.GetObjectByKey<TestMethod>(objTP.TestMethod.Oid);
            //                        if (!string.IsNullOrEmpty(Test))
            //                        {
            //                            List<string> lstTMOid = Test.Split(';').ToList();
            //                            foreach (string strTMOid in lstTMOid)
            //                            {
            //                                TestMethod objTM1 = Session.GetObjectByKey<TestMethod>(new Guid(strTMOid.Trim()));
            //                                if (objTM != null && !lstSM.Contains(objTM.MethodName.MethodNumber) && objTM1.TestName == objTM.TestName)
            //                                {
            //                                    lstSM.Add(objTM.MethodName.MethodNumber);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        return new XPCollection<Method>(Session, new InOperator("MethodNumber", lstSM));
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
        }
        #endregion

        #region ALLTemplateEDDMethod
        private string _ALLTemplateEDDMethod;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string ALLTemplateEDDMethod
        {
            get { return _ALLTemplateEDDMethod; }
            set { SetPropertyValue(nameof(ALLTemplateEDDMethod), ref _ALLTemplateEDDMethod, value); }
        }
        #endregion

        #region DateCreated
        private DateTime _DateCreated;
        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set { SetPropertyValue(nameof(DateCreated), ref _DateCreated, value); }
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

        #region JobID
        private string _JobID;
        [ImmediatePostData]
        //[EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string JobID
        {
            get { return _JobID; }
            set { SetPropertyValue(nameof(JobID), ref _JobID, value); }
        }
        #endregion
        private string _NPJobid;
        [VisibleInListView(false)]
        [NonPersistent]
        //[Appearance("ABNPJobid", Visibility = ViewItemVisibility.Hide, Criteria = "!ISShown", Context = "DetailView")]
        public string NPJobid
        {
            get
            {
                if (!Session.IsNewObject(this) && !string.IsNullOrEmpty(JobID))
                {
                    _NPJobid = JobID;
                }
                return _NPJobid;
            }
            set { SetPropertyValue("NPJobid", ref _NPJobid, value); }
        }
        //private bool _ISShown;
        //[NonPersistent]
        //[ImmediatePostData]
        //public bool ISShown
        //{
        //    get { return _ISShown; }
        //    set { SetPropertyValue("ISShown", ref _ISShown, value); }
        //}
        #region ALLEDDTemplateJobID
        private string _ALLEDDTemplateJobID;
        [ImmediatePostData]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string ALLEDDTemplateJobID
        {
            get { return _ALLEDDTemplateJobID; }
            set { SetPropertyValue(nameof(ALLEDDTemplateJobID), ref _ALLEDDTemplateJobID, value); }
        }
        public XPCollection<Samplecheckin> JobIDs
        {
            get
            {

                if (EddTemplate != null && !string.IsNullOrEmpty(ALLEDDTemplateJobID))
                {
                    List<Guid> lstStr = new List<Guid>();
                    List<string> lstStrOid = ALLEDDTemplateJobID.Split(';').ToList();
                    if (lstStrOid != null)
                    {
                        foreach (string objOid in lstStrOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Samplecheckin objSCI = Session.GetObjectByKey<Samplecheckin>(new Guid(objOid.Trim()));
                                if (objSCI != null && !lstStr.Contains(objSCI.Oid))
                                {
                                    lstStr.Add(objSCI.Oid);
                                }
                            }

                        }
                    }
                    return new XPCollection<Samplecheckin>(Session, new InOperator("Oid", lstStr));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region SampleCategory
        private string _SampleCategory;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string SampleCategory
        {
            get { return _SampleCategory; }
            set { SetPropertyValue(nameof(SampleCategory), ref _SampleCategory, value); }
        }
        #endregion

        #region ALLTemplateEDDSampleCategory
        private string _ALLTemplateEDDSampleCategory;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string ALLTemplateEDDSampleCategory
        {
            get { return _ALLTemplateEDDSampleCategory; }
            set { SetPropertyValue(nameof(ALLTemplateEDDSampleCategory), ref _ALLTemplateEDDSampleCategory, value); }
        }
        public XPCollection<SampleCategory> SampleCategorys
        {
            get
            {
                if (EddTemplate != null && !string.IsNullOrEmpty(ALLTemplateEDDSampleCategory))
                {
                    List<Guid> lstStr = new List<Guid>();
                    List<string> lstStrOid = ALLTemplateEDDSampleCategory.Split(';').ToList();
                    if (lstStrOid != null)
                    {
                        foreach (string objOid in lstStrOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                SampleCategory objSCI = Session.GetObjectByKey<SampleCategory>(new Guid(objOid.Trim()));
                                if (objSCI != null && !lstStr.Contains(objSCI.Oid))
                                {
                                    lstStr.Add(objSCI.Oid);
                                }
                            }

                        }
                    }
                    return new XPCollection<SampleCategory>(Session, new InOperator("Oid", lstStr));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region DateReceivedFrom
        private DateTime _DateReceivedFrom;
        public DateTime DateReceivedFrom
        {
            get { return _DateReceivedFrom; }
            set { SetPropertyValue(nameof(DateReceivedFrom), ref _DateReceivedFrom, value); }
        }
        #endregion

        #region DateReceivedTo
        private DateTime _DateReceivedTo;
        public DateTime DateReceivedTo
        {
            get { return _DateReceivedTo; }
            set { SetPropertyValue(nameof(DateReceivedTo), ref _DateReceivedTo, value); }
        }
        #endregion

        #region DateCollectedFrom
        private DateTime _DateCollectedFrom;
        public DateTime DateCollectedFrom
        {
            get { return _DateCollectedFrom; }
            set { SetPropertyValue(nameof(DateCollectedFrom), ref _DateCollectedFrom, value); }
        }
        #endregion

        #region DateCollectedTo
        private DateTime _DateCollectedTo;
        public DateTime DateCollectedTo
        {
            get { return _DateCollectedTo; }
            set { SetPropertyValue(nameof(DateCollectedTo), ref _DateCollectedTo, value); }
        }
        #endregion 
        #region IsDateCollected
        private bool _IsDateCollected;
        public bool IsDateCollected
        {
            get { return _IsDateCollected; }
            set { SetPropertyValue(nameof(IsDateCollected), ref _IsDateCollected, value); }
        }
        #endregion
        #region IsDateReceived
        private bool _IsDateReceived;
        public bool IsDateReceived
        {
            get { return _IsDateReceived; }
            set { SetPropertyValue(nameof(IsDateReceived), ref _IsDateReceived, value); }
        }
        #endregion

        #region Report
        private byte[] _Report;
        [Size(SizeAttribute.Unlimited)]
        [Browsable(false)]
        public byte[] Report
        {
            get { return _Report; }
            set { SetPropertyValue(nameof(Report), ref _Report, value); }
        }
        #endregion
        #region ExcelFile
        private FileData _ExcelFile;
        [Size(SizeAttribute.Unlimited)]
        [Browsable(false)]
        public FileData ExcelFile
        {
            get { return _ExcelFile; }
            set { SetPropertyValue(nameof(ExcelFile), ref _ExcelFile, value); }
        }
        #endregion
    }
}