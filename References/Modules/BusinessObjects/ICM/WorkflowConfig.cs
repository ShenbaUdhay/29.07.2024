using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class WorkflowConfig : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Consturtor
        public WorkflowConfig(Session session)
            : base(session)
        {

        }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            Level = 1;
            ActivationOn = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #endregion

        #region Level
        private int _Level;
        [RuleUniqueValue]
        [RuleRange(1, 6)]
        [RuleRequiredField("Workflowlevel", DefaultContexts.Save)]
        public int Level
        {
            get { return _Level; }
            set { SetPropertyValue("Level", ref _Level, value); }
        }
        #endregion

        #region User
        [Association("WorkflowConfigUser", UseAssociationNameAsIntermediateTableName = true)]
        // [RuleRequiredField("workflowuser", DefaultContexts.Save)]
        public XPCollection<CustomSystemUser> User
        {
            get { return GetCollection<CustomSystemUser>("User"); }

        }
        #endregion

        #region Description
        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
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
        //[Appearance("MD9", Enabled = false, Context = "DetailView")]       
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

        #region ActivationOn
        private bool _ActivationOn;
        [ImmediatePostData(true)]
        public bool ActivationOn
        {
            get { return _ActivationOn; }
            set { SetPropertyValue("ActivationOn", ref _ActivationOn, value); }
        }
        #endregion

        #region ActivationOff
        private bool _ActivationOff;
        [ImmediatePostData(true)]
        public bool ActivationOff
        {
            get { return _ActivationOff; }
            set { SetPropertyValue("ActivationOff", ref _ActivationOff, value); }
        }
        #endregion

        #region Budget
        private double _Budget;
        public double Budget
        {
            get { return _Budget; }
            set { SetPropertyValue("Budget", ref _Budget, value); }
        }
        #endregion

        #region NextLevel
        private int _NextLevel;
        public int NextLevel
        {
            get { return _NextLevel; }
            set { SetPropertyValue("NextLevel", ref _NextLevel, value); }
        }
        #endregion
    }
}