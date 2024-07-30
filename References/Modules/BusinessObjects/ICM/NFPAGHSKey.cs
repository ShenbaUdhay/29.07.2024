using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class NFPAGHSKey : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public NFPAGHSKey(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region CategoryCode
        private string _CategoryCode;
        public string CategoryCode
        {
            get { return _CategoryCode; }
            set { SetPropertyValue("CategoryCode", ref _CategoryCode, value); }
        }
        #endregion

        #region CategoryDescription
        private string _CategoryDescription;
        public string CategoryDescription
        {
            get { return _CategoryDescription; }
            set { SetPropertyValue("CategoryDescription", ref _CategoryDescription, value); }
        }
        #endregion

        #region Level
        private string _Level;
        public string Level
        {
            get { return _Level; }
            set { SetPropertyValue("Level", ref _Level, value); }
        }
        #endregion

        #region LevelDescription
        private string _LevelDescription;
        public string LevelDescription
        {
            get { return _LevelDescription; }
            set { SetPropertyValue("LevelDescription", ref _LevelDescription, value); }
        }
        #endregion

        #region Items
        private XPCollection<Items> _Items;
        [Association("NFPAItems", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[RuleRequiredField("VisualMatrixName2", DefaultContexts.Save)]
        public XPCollection<Items> Items
        {
            get
            {
                return GetCollection<Items>("Items");

            }
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

    }
}