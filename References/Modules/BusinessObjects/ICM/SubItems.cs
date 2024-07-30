using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    public class SubItems : BaseObject
    {
        #region Constructor
        public SubItems(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region subitemsname
        string fSubItemsName;
        [RuleRequiredField("SubItemsName", DefaultContexts.Save)]
        public string SubItemsName
        {
            get { return fSubItemsName; }
            set { SetPropertyValue<string>("SubItemsName", ref fSubItemsName, value); }
        }
        #endregion

        #region concentration
        string fConcentration;
        [RuleRequiredField("Concentration", DefaultContexts.Save)]
        public string Concentration
        {
            get { return fConcentration; }
            set { SetPropertyValue<string>("Concentration", ref fConcentration, value); }
        }
        #endregion

        #region remark
        string fRemark;
        public string Remark
        {
            get { return fRemark; }
            set { SetPropertyValue<string>("Remark", ref fRemark, value); }
        }
        #endregion

        #region subitemslink
        [Association("SubItemslink", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<Items> items
        {
            get { return GetCollection<Items>("items"); }
        }
        #endregion


    }
}