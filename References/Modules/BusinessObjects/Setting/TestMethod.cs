﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SDMS;
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

    [Appearance("ShowCopyFromTest", AppearanceItemType = "LayoutItem",
 TargetItems = "Grp_CopyFromTest", Criteria = "CanCopyTest = True",
 Context = "TestMethod_DetailView_Copy", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideCopyFromTest", AppearanceItemType = "LayoutItem",
 TargetItems = "Grp_CopyFromTest", Criteria = "CanCopyTest = False",
 Context = "TestMethod_DetailView_Copy", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("ShowTestmethods", AppearanceItemType = "LayoutItem",
 TargetItems = "Group", Criteria = "IsGroup = True",
 Context = "TestMethod_DetailView_Copy", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestmethods", AppearanceItemType = "LayoutItem",
 TargetItems = "Group", Criteria = "IsGroup = False",
 Context = "TestMethod_DetailView_Copy", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("ShowCopyTestmethods", AppearanceItemType = "LayoutItem",
 TargetItems = "Method", Criteria = "IsGroup = False",
 Context = "TestMethod_DetailView_Copy", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideCopyTestmethods", AppearanceItemType = "LayoutItem",
 TargetItems = "Method", Criteria = "IsGroup = True",
 Context = "TestMethod_DetailView_Copy", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("ShowTestmethodsTM", AppearanceItemType = "LayoutItem",
 TargetItems = "Item18", Criteria = "IsGroup = True",
 Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestmethodsTM", AppearanceItemType = "LayoutItem",
 TargetItems = "Item18", Criteria = "IsGroup = False",
 Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("ShowTestmethodsTPD", AppearanceItemType = "LayoutItem",
 TargetItems = "Item5", Criteria = "IsGroup = False",
 Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestmethodsTPD", AppearanceItemType = "LayoutItem",
 TargetItems = "Item5", Criteria = "IsGroup = True",
 Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("ShowTestmethodsMD", AppearanceItemType = "LayoutItem",
 TargetItems = "Item6", Criteria = "IsGroup = False",
 Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestmethodsMD", AppearanceItemType = "LayoutItem",
 TargetItems = "Item6", Criteria = "IsGroup = True",
 Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("ShowCopyFromTestSettings", AppearanceItemType = "LayoutItem",
 TargetItems = "Grp_CopySettings", Criteria = "CanCopyTest = True And Method is not null",
 Context = "TestMethod_DetailView_Copy", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideCopyFromTestSettings", AppearanceItemType = "LayoutItem",
 TargetItems = "Grp_CopySettings", Criteria = "CanCopyTest = False Or Method is null",
 Context = "TestMethod_DetailView_Copy", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ///*SamplingMethodTab*/
    //[Appearance("ShowTestSampleTab", AppearanceItemType = "LayoutItem", TargetItems = "Item11", Criteria = "[SamplingMethodTab] = True", Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    //[Appearance("HideTestSampleTab", AppearanceItemType = "LayoutItem", TargetItems = "Item11", Criteria = "[SamplingMethodTab] = False", Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    /*Surrogates*/
    [Appearance("ShowTestSurrogates", AppearanceItemType = "LayoutItem", TargetItems = "Item13", Criteria = "[Surrogatestab] = True", Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestSurrogates", AppearanceItemType = "LayoutItem", TargetItems = "Item13", Criteria = "[Surrogatestab] = False", Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    /*InternalStandards*/
    [Appearance("ShowTestInternalStandards", AppearanceItemType = "LayoutItem", TargetItems = "Item14", Criteria = "[InternalStandardsTab] = True", Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestInternalStandards", AppearanceItemType = "LayoutItem", TargetItems = "Item14", Criteria = "[InternalStandardsTab] = False", Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    /*Components*/
    [Appearance("ShowTestComponents", AppearanceItemType = "LayoutItem", TargetItems = "Item15", Criteria = "[Componentstab] = True", Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestComponents", AppearanceItemType = "LayoutItem", TargetItems = "Item15", Criteria = "[Componentstab] = False", Context = "TestMethod_DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [RuleCombinationOfPropertiesIsUnique("TestMethod", DefaultContexts.Save, "MatrixName,TestName,MethodName,Component", SkipNullOrEmptyValues = false)]

    public class TestMethod : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        TestmethodQctypeinfo objtmqc = new TestmethodQctypeinfo();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();


        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TestMethod(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);

        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(TestCode))
            {
                SelectedData sproc = Session.ExecuteSproc("GetTestuqID", "");
                if (sproc.ResultSet[0].Rows[0] != null)
                    TestCode = sproc.ResultSet[0].Rows[0].Values[0].ToString();
            }
        }


        #region TestSampleTab
        private bool _SamplingMethodTab;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool SamplingMethodTab
        {
            get
            {
                XPCollection<TabControls> lsttabcontrol = new XPCollection<TabControls>(Session, CriteriaOperator.Parse("[GCRecord] is Null"));
                foreach (TabControls objtabcontrol in lsttabcontrol.Cast<TabControls>().Where(i => i.TabName == "Sampling Method").ToList())
                {
                    if (objtabcontrol.IsVisible == false)
                    {
                        _SamplingMethodTab = false;
                    }
                    else
                    {
                        SamplingMethodTab = true;
                    }
                }
                return _SamplingMethodTab;
            }
            set
            {
                SetPropertyValue(nameof(SamplingMethodTab), ref _SamplingMethodTab, value);
            }
        }
        #endregion

        #region TestSurrogates
        private bool _Surrogatestab;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool Surrogatestab
        {
            get
            {
                XPCollection<TabControls> lsttabcontrol = new XPCollection<TabControls>(Session, CriteriaOperator.Parse("[GCRecord] is Null"));
                foreach (TabControls objtabcontrol in lsttabcontrol.Cast<TabControls>().Where(i => i.TabName == "Surrogates").ToList())
                {
                    if (objtabcontrol.IsVisible == false)
                    {
                        _Surrogatestab = false;
                    }
                    else
                    {
                        _Surrogatestab = true;
                    }
                }
                return _Surrogatestab;
            }
            set
            {
                SetPropertyValue(nameof(Surrogatestab), ref _Surrogatestab, value);
            }
        }
        #endregion

        #region TestInternalStandardsTab
        private bool _InternalStandardsTab;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool InternalStandardsTab
        {
            get
            {
                XPCollection<TabControls> lsttabcontrol = new XPCollection<TabControls>(Session, CriteriaOperator.Parse("[GCRecord] is Null"));
                foreach (TabControls objtabcontrol in lsttabcontrol.Cast<TabControls>().Where(i => i.TabName == "Internal Standards").ToList())
                {
                    if (objtabcontrol.IsVisible == false)
                    {
                        _InternalStandardsTab = false;
                    }
                    else
                    {
                        _InternalStandardsTab = true;
                    }
                }
                return _InternalStandardsTab;
            }
            set
            {
                SetPropertyValue(nameof(InternalStandardsTab), ref _InternalStandardsTab, value);
            }
        }
        #endregion

        #region Componentstab
        private bool _Componentstab;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool Componentstab
        {
            get
            {
                XPCollection<TabControls> lsttabcontrol = new XPCollection<TabControls>(Session, CriteriaOperator.Parse("[GCRecord] is Null"));
                foreach (TabControls objtabcontrol in lsttabcontrol.Cast<TabControls>().Where(i => i.TabName == "Components").ToList())
                {
                    if (objtabcontrol.IsVisible == false)
                    {
                        _Componentstab = false;
                    }
                    else
                    {
                        _Componentstab = true;
                    }
                }
                return _Componentstab;
            }
            set
            {
                SetPropertyValue(nameof(Componentstab), ref _Componentstab, value);
            }
        }
        #endregion


        #region Matrix
        private Matrix fMatrix;
        //[Browsable(false)]
        [RuleRequiredField("MatrixName2", DefaultContexts.Save)]
        public Matrix MatrixName
        {
            get
            {
                return fMatrix;
            }
            set
            {
                SetPropertyValue("MatrixName", ref fMatrix, value);
            }
        }
        #endregion

        #region Test
        private string fTest;
        //[Browsable(false)]
        //[RuleUniqueValue]
        [RuleRequiredField("TestName2", DefaultContexts.Save)]
        [EditorAlias("StringTestComoboxPropertyEditor")]
        public string TestName
        {
            get
            {
                return fTest;
            }
            set
            {
                SetPropertyValue("TestName", ref fTest, value);
            }
        }
        #endregion

        #region Method
        private Method fMethod;
        //[Browsable(false)]
        [ImmediatePostData]
        //[RuleRequiredField/*("MethodName", DefaultContexts.Save)*/]
        [Appearance("MethodmedHide", Visibility = ViewItemVisibility.Hide, Criteria = "IsGroup=True", Context = "DetailView")]
        [Appearance("MethodmedShow", Visibility = ViewItemVisibility.Show, Criteria = "IsGroup=False", Context = "DetailView")]
        public Method MethodName
        {
            get
            {
                if (IsGroup == true)
                {
                    fMethod = null;
                }
                return fMethod;
            }
            set
            {
                SetPropertyValue("MethodName", ref fMethod, value);
            }
        }
        #endregion

        #region Component
        private string _Component;
        [NonPersistent]
        public string Component
        {
            get
            {
                if (Oid != null)
                {
                    Testparameter objtp = Session.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] =? And [Component.Components] = 'Default'", Oid));
                    if (objtp != null && objtp.Component != null && objtp.Component.Components != null)
                    {
                        _Component = objtp.Component.Components;
                    }
                }
                return _Component;
            }
            set { SetPropertyValue("Component", ref _Component, value); }
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

        #region TestCode
        private string _TestCode;

        public string TestCode
        {
            get { return _TestCode; }
            set { SetPropertyValue("TestCode", ref _TestCode, value); }
        }
        #endregion
        #region Technologies
        private Technology _Technologies;
        [XafDisplayName("Technology")]
        public Technology Technologies
        {
            get { return _Technologies; }
            set { SetPropertyValue(nameof(Technologies), ref _Technologies, value); }
        }
        #endregion
        #region IsGroup
        private bool _IsGroup;
        [ImmediatePostData]
        public bool IsGroup
        {
            get
            {
                return _IsGroup;
            }
            set
            {
                SetPropertyValue("IsGroup", ref _IsGroup, value);
            }
        }
        #endregion

        [ManyToManyAlias("TestParameter", "Parameter")]
        public IList<Parameter> Parameters
        {
            get
            {
                return GetList<Parameter>("Parameters");
            }
        }

        #region Comment
        private string fComment;
        [Size(1000)]
        //[Browsable(false)]

        public string Comment
        {
            get
            {
                return fComment;
            }
            set
            {
                SetPropertyValue("Comment", ref fComment, value);
            }
        }
        #endregion

        #region ModifiedBy
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        private CustomSystemUser fModifiedBy;
        //[Browsable(false)]
        [Appearance("MB9", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get
            {
                return fModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref fModifiedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime fModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        [Appearance("MD9", Enabled = false, Context = "DetailView")]
        public DateTime ModifiedDate
        {
            get
            {
                return fModifiedDate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref fModifiedDate, value);
            }
        }
        #endregion
        #region IListforTestParameter
        [Association, Browsable(false)]

        public IList<Testparameter> TestParameter
        {
            get
            {
                return GetList<Testparameter>("TestParameter");
            }
        }
        #endregion IListforTestParameter

        #region PreservativeCollection

        //[Association(@"TestMethodReferencesPreservative")]
        //public XPCollection<Preservative> Preservatives
        //{
        //    get
        //    {
        //return GetCollection<Preservative>(nameof(Preservatives));
        //    }
        //}

        private Preservative _Preservative;

        public Preservative Preservative
        {
            get { return _Preservative; }
            set { SetPropertyValue("Preservative", ref _Preservative, value); }
        }

        #endregion

        private XPCollection<AuditDataItemPersistent> auditTrail;
        public XPCollection<AuditDataItemPersistent> AuditTrail
        {
            get
            {
                if (auditTrail == null)
                {
                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditTrail;
            }
        }

        private string _Category;

        public string Category
        {
            get { return _Category; }
            set { SetPropertyValue("Category", ref _Category, value); }
        }

        [NonPersistent]
        public string Instrument
        {
            get
            {
                if (Labwares.Count > 0)
                {
                    return String.Join(";", Labwares.Select(i => i.AssignedName));
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string SamplePrepInstrument
        {
            get
            {
                if (SamplePrepLabwares.Count > 0)
                {
                    return String.Join(";", SamplePrepLabwares.Select(i => i.AssignedName));
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string NPInstrument
        {
            get
            {
                if (!string.IsNullOrEmpty(objtmqc.applicationviewid) && objtmqc.applicationviewid == "TestMethod_ListView_TestInstrument")
                {
                    if (!string.IsNullOrEmpty(objtmqc.strinstruments))
                    {
                        return objtmqc.strinstruments;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else if (!string.IsNullOrEmpty(objtmqc.applicationviewid) && objtmqc.applicationviewid == "TestMethod_ListView_SamplePrep_TestInstrument")
                {

                    if (!string.IsNullOrEmpty(objtmqc.strinstruments))
                    {
                        return objtmqc.strinstruments;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        //[NonPersistent]
        //public string Instrument
        //{
        //    get
        //    {
        //        if (Labwares.Count > 0)
        //        {
        //            return String.Join(";", Labwares.Select(i => i.AssignedName));
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //}
        #region ManyToManyRelations

        #endregion

        #region ManyToManyRelations

        [Association, Browsable(false)]
        public IList<GroupTestMethod> GroupTestMethod
        {
            get
            {
                return GetList<GroupTestMethod>("GroupTestMethod");
            }
        }
        //[ManyToManyAlias("GroupTestMethod", "GroupTests")]
        //public IList<GroupTest> GroupTests
        //{
        //    get
        //    {
        //        return GetList<GroupTest>("GroupTests");
        //    }
        //}

        #endregion

        #region IListforQCType
        //[ManyToManyAlias("SampleParameter", "SampleLogIn")]
        //[ManyToManyAlias("QCParameter", "QCType")]
        //[ManyToManyAlias("TestParameter", "QCType")]
        [Association("TestMethodQCType")]
        //public IList<QCType> QCTypes
        //{
        //    get
        //    {
        //        return GetList<QCType>("QCTypes");
        //    }
        //}
        public XPCollection<QCType> QCTypes
        {
            get
            {
                return GetCollection<QCType>(nameof(QCTypes));
            }
        }
        #endregion IListforSampleLogin

        #region department

        private Department fDepartmentname;
        [ImmediatePostData]
        public Department Departmentname
        {
            get
            {
                return fDepartmentname;
            }
            set
            {
                SetPropertyValue("Departmentname", ref fDepartmentname, value);
            }
        }

        #endregion

        #region surrogates

        [Association("TestMethodparameter")]

        public XPCollection<Parameter> surrogates
        {
            get
            {
                return GetCollection<Parameter>(nameof(surrogates));
            }
        }
        #endregion

        #region IListforQCParameter
        [Association, Browsable(false)]
        public IList<QcParameter> QCParameter
        {
            get
            {
                return GetList<QcParameter>("QCParameter");
            }
        }
        #endregion

        [NonPersistent]
        private bool _SubOut;
        public bool SubOut
        {
            get { return _SubOut; }
            set { SetPropertyValue("SubOut", ref _SubOut, value); }
        }

        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int NoSamples
        {
            //Code commented  To allow a non sdms test also
            //get
            //{
            //    //return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [QCBatchID] Is Null", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
            //    if (Session.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID] =?", Oid)) != null)
            //    {
            //        //DefaultSetting objDefault = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[SamplePreparation]=1")).FirstOrDefault();
            //        DefaultSetting objDefault = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True")).FirstOrDefault();
            //        DefaultSetting objDefaultmodule = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparationRootNode'AND [Select]=True")).FirstOrDefault();
            //        if (objDefault != null && objDefaultmodule != null && PrepMethods.Count> 0)
            //        {
            //           return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [UQABID] Is Null And [QCBatchID] Is Null AND [SamplePrepBatchID] IS NOT NULL", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
            //        }
            //        else
            //        {
            //            return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [UQABID] Is Null And [QCBatchID] Is Null", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
            //        }
            //        //return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [QCBatchID] Is Null", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
            //        //return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [UQABID] Is Null And [QCBatchID] Is Null", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
            //    }
            //    else
            //    {
            //        return 0;
            //    }
            //}

            get
            {
                return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True  And IsNullOrEmpty([ResultNumeric]) And [IsTransferred] = true And [UQABID] Is Null And [QCBatchID] Is Null And ([SubOut] Is Null Or [SubOut] = False)  And [GCRecord] Is NULL And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False)", Oid)).Where(p => p.Samplelogin != null && p.TestHold == false).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();

                //DefaultSetting objDefault = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[SamplePreparation]=1")).FirstOrDefault();
                //DefaultSetting objDefault = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True")).FirstOrDefault();
                //DefaultSetting objDefaultmodule = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparationRootNode'AND [Select]=True")).FirstOrDefault();
                //if (objDefault != null && objDefaultmodule != null && PrepMethods.Count > 0)
                //{
                //    //return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [UQABID] Is Null And [QCBatchID] Is Null AND [SamplePrepBatchID] IS NOT NULL", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                //    return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And IsNullOrEmpty([ResultNumeric]) And [Samplelogin.IsNotTransferred] = false And [UQABID] Is Null And [QCBatchID] Is Null AND [IsPrepMethodComplete]  = True And [GCRecord] Is NULL", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                //}
                //else
                //{
                //    return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True  And IsNullOrEmpty([ResultNumeric]) And [Samplelogin.IsNotTransferred] = false And [UQABID] Is Null And [QCBatchID] Is Null And [GCRecord] Is NULL", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                //}
                //return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [QCBatchID] Is Null", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                //return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [UQABID] Is Null And [QCBatchID] Is Null", Oid)).Where(p => p.Samplelogin != null).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
            }
        }

        //[NonPersistent]
        //[Association("SamplingBottlesetupTest", UseAssociationNameAsIntermediateTableName = true)]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]

        //public XPCollection<SamplingBottleSetup> SamplingBottlesetup
        //{
        //    get { return GetCollection<SamplingBottleSetup>("SamplingBottlesetup"); }
        //}
        //[NonPersistent]
        //[Association("BottlesetupTest", UseAssociationNameAsIntermediateTableName = true)]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]

        //public XPCollection<BottleSetup> Bottlesetup
        //{
        //    get { return GetCollection<BottleSetup>("Bottlesetup"); }
        //}

        //[NonPersistent]
        //[Association("COCBottlesetupTest", UseAssociationNameAsIntermediateTableName = true)]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]

        //public XPCollection<COCBottleSetup> COCBottlesetup
        //{
        //    get { return GetCollection<COCBottleSetup>("COCBottlesetup"); }
        //}

        #region AnalysisDepartmentUsers
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("AnalysisDepartmentChainTestMethods", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<AnalysisDepartmentChain> DepartmentChains
        {
            get
            {
                return GetCollection<AnalysisDepartmentChain>(nameof(DepartmentChains));
            }
        }
        #endregion

        #region SamplePreparationChainTestMethods
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Association("SamplePreparationChainTestMethods", UseAssociationNameAsIntermediateTableName = true)]
        [Association("SamplePreparationChainTestMethods")]
        public XPCollection<SamplePreparationChain> SamplePreparationChains
        {
            get
            {
                return GetCollection<SamplePreparationChain>(nameof(SamplePreparationChains));
            }
        }
        #endregion

        #region ResultValidationUsers
        [NonPersistent]
        public string ResultValidationUsers
        {
            get
            {
                if (DepartmentChains != null && DepartmentChains.Count > 0)
                {
                    List<string> userlist = DepartmentChains.Where(i => i.ResultValidation == true).Select(i => i.Employee.DisplayName).OrderBy(i => i).ToList();
                    if (userlist != null && userlist.Count > 0)
                    {
                        return string.Join(",", userlist);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region ResultApprovalUsers
        [NonPersistent]
        public string ResultApprovalUsers
        {
            get
            {
                if (DepartmentChains != null && DepartmentChains.Count > 0)
                {
                    List<string> userlist = DepartmentChains.Where(i => i.ResultApproval == true).Select(i => i.Employee.DisplayName).OrderBy(i => i).ToList();
                    if (userlist != null && userlist.Count > 0)
                    {
                        return string.Join(",", userlist);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion


        #region ResultEntryUsers
        [NonPersistent]
        public string ResultEntryUsers
        {
            get
            {
                if (DepartmentChains != null && DepartmentChains.Count > 0)
                {
                    List<string> userlist = DepartmentChains.Where(i => i.ResultEntry == true).Select(i => i.Employee.DisplayName).OrderBy(i => i).ToList();
                    if (userlist != null && userlist.Count > 0)
                    {
                        return string.Join(",", userlist);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        [Association("TestMethod-Labware")]
        public XPCollection<BusinessObjects.Assets.Labware> Labwares
        {
            get { return GetCollection<BusinessObjects.Assets.Labware>(nameof(Labwares)); }
        }
        [Association("SamplePrepTestMethod-Labware")]
        public XPCollection<BusinessObjects.Assets.Labware> SamplePrepLabwares
        {
            get { return GetCollection<BusinessObjects.Assets.Labware>(nameof(SamplePrepLabwares)); }
        } 
        [Association("FieldInstrument-Labware")]
        public XPCollection<BusinessObjects.Assets.Labware> FieldInstrument
        {
            get { return GetCollection<BusinessObjects.Assets.Labware>(nameof(FieldInstrument)); }
        }

        #region IsFieldTest
        private bool fIsFieldTest;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsFieldTest
        {
            get
            {
                return fIsFieldTest;
            }
            set
            {
                SetPropertyValue("IsFieldTest", ref fIsFieldTest, value);
            }
        }
        #endregion

        #region InstrumentAnalysis
        private string fInstrumentAnalysis;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string InstrumentAnalysis
        {
            get
            {
                return fInstrumentAnalysis;
            }
            set
            {
                SetPropertyValue("InstrumentAnalysis", ref fInstrumentAnalysis, value);
            }
        }
        #endregion
        #region SamplingMethodCollection
        [Association("SamplingMethod-Method")]
        public XPCollection<Method> SamplingMethods
        {
            get { return GetCollection<Method>(nameof(SamplingMethods)); }
        }
        #endregion
        [Association("TestMethod-ItemsLinks")]
        public IList<ItemTestMethodLink> Linkparameters
        {
            get
            {
                return GetList<ItemTestMethodLink>("Linkparameters");
            }
        }

        [ManyToManyAlias("Linkparameters", "LinkItems")]
        public IList<Items> Item
        {
            get
            {
                return GetList<Items>("Item");
            }
        }
        [Association("TestMethod-Preservative")]
        public XPCollection<Preservative> Preservatives
        {
            get { return GetCollection<Preservative>("Preservatives"); }
        }

        public bool IsSDMSTest
        {
            get
            {
                //if (Session.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID] =?", Oid)) != null)
                //{
                //    return true;
                //}
                //else
                //{
                return false;
                //}
            }
        }
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        public string Template
        {
            get
            {
                SpreadSheetBuilder_TestParameter template = Session.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID] =?", Oid));
                if (template != null)
                {
                    SpreadSheetBuilder_TemplateInfo objTemplate = Session.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID] =?", template.TemplateID));
                    if (objTemplate != null)
                    {
                        return objTemplate.TemplateName;
                    }
                    else
                    {
                        return "N/A";
                    }
                }
                else
                {
                    return "N/A";
                }
            }
        }


        #region QCtypecombo

        private QCType _QCtypesCombo;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        [ImmediatePostData]
        [DataSourceProperty(nameof(lstQCTypes))]
        public QCType QCtypesCombo
        {
            get { return _QCtypesCombo; }
            set { SetPropertyValue("QCtypesCombo", ref _QCtypesCombo, value); }
        }
        [NonPersistent]
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public XPCollection<QCType> lstQCTypes
        {
            get
            {
                if (objtmqc.objtmQCType != null)
                {
                    List<Guid> ids = objtmqc.objtmQCType.Select(i => new Guid(i.Oid.ToString())).ToList();
                    return new XPCollection<QCType>(Session, new InOperator("Oid", ids));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region SequenceSetupFields

        #region FrequencyofQCAnalysis
        private int _FrequencyofQCAnalysis;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public int FrequencyofQCAnalysis
        {
            get { return _FrequencyofQCAnalysis; }
            set { SetPropertyValue("FrequencyofQCAnalysis", ref _FrequencyofQCAnalysis, value); }
        }
        #endregion

        #region InstrumentFieldTesting
        private string fInstrumentFieldTesting;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string InstrumentFieldTesting
        {
            get
            {
                return fInstrumentFieldTesting;
            }
            set
            {
                SetPropertyValue("InstrumentFieldTesting", ref fInstrumentFieldTesting, value);
            }
        }
        #endregion

        #region InsertDuplicateSample
        private int _InsertDuplicateSample;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public int InsertDuplicateSample
        {
            get { return _InsertDuplicateSample; }
            set { SetPropertyValue("InsertDuplicateSample", ref _InsertDuplicateSample, value); }
        }
        #endregion

        #region Insertspikesample
        private int _Insertspikesample;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public int Insertspikesample
        {
            get { return _Insertspikesample; }
            set { SetPropertyValue("Insertspikesample", ref _Insertspikesample, value); }
        }
        #endregion

        #region initialqctype
        [Association("Initialqctype-QCType")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public XPCollection<QCType> Initialqctype
        {
            get { return GetCollection<QCType>(nameof(Initialqctype)); }
        }
        #endregion
        #region sampleqctype

        [Association("Sampleqctype-QCType")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public XPCollection<QCType> Sampleqctype
        {
            get { return GetCollection<QCType>(nameof(Sampleqctype)); }
        }
        #endregion
        #region closingqctype
        [Association("Closingqctype-QCType")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public XPCollection<QCType> Closingqctype
        {
            get { return GetCollection<QCType>(nameof(Closingqctype)); }
        }
        #endregion

        #region closingtypebool
        private bool _IsClosingQC;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public bool IsClosingQC
        {
            get { return _IsClosingQC; }
            set
            {
                SetPropertyValue(nameof(IsClosingQC), ref _IsClosingQC, value);
            }
        }
        #endregion
        #endregion

        #region CopyTestFields

        private bool _CanCopyTest;
        [NonPersistent]
        [ImmediatePostData]
        public bool CanCopyTest
        {
            get
            {
                return _CanCopyTest;
            }
            set
            {
                SetPropertyValue<bool>(nameof(CanCopyTest), ref _CanCopyTest, value);
            }
        }

        #region Test
        private TestMethod _Test;
        [NonPersistent]
        [ImmediatePostData]
        //[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [DataSourceProperty(nameof(TestDataSource))]
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
                        if (IsGroup == true)
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = True", Matrix.MatrixName);
                            lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestName", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                        }
                        else if (IsGroup == false)
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And([IsGroup] <> True Or [IsGroup] Is Null)", Matrix.MatrixName);
                            lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestName", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                        }
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
        [NonPersistent]
        [ImmediatePostData]
        //[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [DataSourceProperty(nameof(MethodDataSource))]
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

        #region Matrix
        private Matrix _Matrix;
        [NonPersistent]
        [ImmediatePostData]
        //[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [DataSourceProperty(nameof(MatrixDataSource))]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue("Matrix", ref _Matrix, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Matrix> MatrixDataSource
        {
            get
            {
                if (Matrix == null /*&& Matrix != null && Matrix != null*/)
                {
                    XPCollection<Matrix> matrixs = new XPCollection<Matrix>(Session, CriteriaOperator.Parse("Not IsNullOrEmpty([MatrixName])"));
                    List<Guid> lstmethod = new List<Guid>();
                    List<string> ids = matrixs.Select(i => i.MatrixName.ToString()).Distinct().ToList();
                    foreach (string objids in ids.ToList())//tests.Where(a => a.TestName !=null).Distinct())
                    {
                        Matrix objtm = Session.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName] = ?", objids));
                        lstmethod.Add(objtm.Oid);
                    }
                    List<Guid> ids1 = lstmethod.Select(i => new Guid(i.ToString())).ToList();
                    return new XPCollection<Matrix>(Session, new InOperator("Oid", ids1));
                    //return tests;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion 
        #endregion


        [Association("TestMethod-TestGuides")]
        public XPCollection<TestGuide> TestGuides
        {
            get
            {
                return GetCollection<TestGuide>(nameof(TestGuides));
            }
        }
        [Association("TestMethod-PrepMethods")]
        public XPCollection<PrepMethod> PrepMethods
        {
            get
            {
                return GetCollection<PrepMethod>(nameof(PrepMethods));
            }
        }


        #region samples
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int NoOfPrepSamples
        {
            get
            {
                DefaultSetting objDefault = new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True")).FirstOrDefault();
                if (objDefault != null && PrepMethods.Count > 0)
                {
                    return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [IsTransferred] = true And [SamplePrepBatchID] Is Null And [Status] = 'PendingEntry' And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False)", Oid)).Where(p => p.Samplelogin != null && p.TestHold == false).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count();
                }
                else
                {
                    return 0;
                }

            }
        }
        #endregion

        #region LinkedItems
        [VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string LinkedItems
        {
            get
            {
                if (Item != null && Item.Count > 0)
                {
                    return string.Join(",", Item.Select(i => i.items).OrderBy(i => i).Distinct().ToList());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region IsPLMTest
        public bool IsPLMTest
        {
            get
            {
                if (Session.FindObject<DataEntry>(CriteriaOperator.Parse("[BOSourceCodeCaption.BOSourceCodeCaption] = 'Asbestos_PLM' And Contains([TestNameOid], ?)", Oid.ToString())) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
