using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCombinationOfPropertiesIsUnique("Parameter", DefaultContexts.Save, "ParameterName", SkipNullOrEmptyValues = false)]
    public class Parameter : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Parameter(Session session)
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
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            if (string.IsNullOrEmpty(ParameterCode))
            {
                SelectedData sproc = Session.ExecuteSproc("GetParameterCode", "");
                if (sproc.ResultSet[0].Rows[0] != null)
                    ParameterCode = sproc.ResultSet[0].Rows[0].Values[0].ToString();
            }
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (BaseObject obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.Oid != null)
                    {
                        Exception ex = new Exception("Already Used Can't allow to Delete");
                        throw ex;
                        break;

                    }
                }
            }
        }


        #region Code
        private string fParameterCode;
        //[Browsable(false)]
        public string ParameterCode
        {
            get
            {
                return fParameterCode;
            }
            set
            {
                SetPropertyValue("ParameterCode", ref fParameterCode, value);
            }
        }
        #endregion

        #region Name
        private string fParameterName;
        //[Browsable(false)]
        //[RuleRequiredField("ParameterName", DefaultContexts.Save)]
        [RuleRequiredField("ParameterName", DefaultContexts.Save, "'Parameter' must not to be empty.")]

        public string ParameterName
        {
            get
            {
                return fParameterName;
            }
            set
            {
                if (value != null)
                SetPropertyValue("ParameterName", ref fParameterName, value);
            }
        }
        #endregion

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
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        [Appearance("MB3", Enabled = false, Context = "DetailView")]
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
        [Appearance("MD3", Enabled = false, Context = "DetailView")]
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

        #region Synonym
        private string _Synonym;
        public string Synonym
        {
            get { return _Synonym; }
            set { SetPropertyValue("Synonym", ref _Synonym, value); }
        }
        #endregion

        #region Formula
        private string _Formula;
        public string Formula
        {
            get { return _Formula; }
            set { SetPropertyValue("Formula", ref _Formula, value); }
        }
        #endregion

        #region MW
        private string _MW;
        public string MW
        {
            get { return _MW; }
            set { SetPropertyValue("MW", ref _MW, value); }
        }
        #endregion

        #region Equivalent
        private string _Equivalent;
        public string Equivalent
        {
            get { return _Equivalent; }
            set { SetPropertyValue("Equivalent", ref _Equivalent, value); }
        }
        #endregion

        #region BoilingPoint
        private string _BoilingPoint;
        public string BoilingPoint
        {
            get { return _BoilingPoint; }
            set { SetPropertyValue("BoilingPoint", ref _BoilingPoint, value); }
        }
        #endregion

        #region MeltingPoint
        private string _MeltingPoint;
        public string MeltingPoint
        {
            get { return _MeltingPoint; }
            set { SetPropertyValue("MeltingPoint", ref _MeltingPoint, value); }
        }
        #endregion

        #region FlashPoint
        private string _FlashPoint;
        public string FlashPoint
        {
            get { return _FlashPoint; }
            set { SetPropertyValue("FlashPoint", ref _FlashPoint, value); }
        }
        #endregion

        #region CAS
        private string _CAS;
        public string CAS
        {
            get { return _CAS; }
            set { SetPropertyValue("CAS", ref _CAS, value); }
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


        #region MSDS
        private FileData _MSDS;

        public FileData MSDS
        {
            get { return _MSDS; }
            set { SetPropertyValue("MSDS", ref _MSDS, value); }
        }
        #endregion

        #region IListforTestParameter
        [Association, Browsable(true)]
        public IList<Testparameter> TestParameter
        {
            get
            {
                return GetList<Testparameter>("TestParameter");
            }
        }
        #endregion IListforTestParameter

        #region TestMethod
        [ManyToManyAlias("TestParameter", "Parameter")]
        public IList<TestMethod> TestMethods
        {
            get
            {
                return GetList<TestMethod>("TestMethods");
            }
        }
        #endregion

        #region Testparameter
        private Testparameter _Testparameter;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public Testparameter Testparameter
        {
            get
            {
                if (TestMethods != null)
                {
                    Testparameter objTestparameter = Session.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod] = ?", Oid));
                    if (objTestparameter != null)
                    {
                        if (objTestparameter != null)
                        {
                            _Testparameter = objTestparameter;
                        }
                        else
                        {
                            _Testparameter = null;
                        }
                    }
                }
                return _Testparameter;
            }
            set { SetPropertyValue("Testparameter", ref _Testparameter, value); }
        }
        #endregion
        #region TestMethodparameter

        [Association("TestMethodparameter")]

        public XPCollection<TestMethod> TestMethodsparameter
        {
            get
            {
                return GetCollection<TestMethod>(nameof(TestMethodsparameter));
            }
        }
        #endregion 
        [ManyToManyAlias("QCTestParameter", "Parameter")]
        public IList<QcParameter> QcParameter
        {
            get
            {
                return GetList<QcParameter>("QcParameter");
            }
        }

        [Association, Browsable(false)]
        public IList<QCTestParameter> QCTestParameter
        {
            get
            {
                return GetList<QCTestParameter>("QCTestParameter");
            }
        }

        #region Analytecode 
        private string _Analytecode;
        public string Analytecode
        {
            get { return _Analytecode; }
            set { SetPropertyValue("Analytecode", ref _Analytecode, value); }
        }
        #endregion 

        #region Limit 
        private bool _Limit;
        public bool Limit
        {
            get { return _Limit; }
            set { SetPropertyValue("Limit", ref _Limit, value); }
        }
        #endregion 


    }
}