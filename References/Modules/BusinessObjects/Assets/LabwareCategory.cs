using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace Modules.BusinessObjects.Assets
{
    [DefaultClassOptions]
    [DefaultProperty("CategoryName")]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class LabwareCategory : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public LabwareCategory(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #region Category
        private string _CategoryName;
        [RuleRequiredField("LabwareCategory", DefaultContexts.Save, "'Instrument Category must not be empty'")]
        [RuleUniqueValue]
        [XafDisplayName("Instrument Category")]
        public string CategoryName
        {
            get
            {
                if (_CategoryName != null)
                {
                    return _CategoryName.Trim();
                }
                return _CategoryName;
            }

            set { SetPropertyValue(nameof(CategoryName), ref _CategoryName, value/*.Trim()*/); }
        }
        #endregion
        //private Labware _Labware;
        //[Association("Category-Labware", UseAssociationNameAsIntermediateTableName = true)]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public Labware Labware
        //{
        //    get { return _Labware; }
        //    set { SetPropertyValue(nameof(Labware), ref _Labware, value); }
        //}
        #region Labware
        //[Association("LabwareCategoryLabware", UseAssociationNameAsIntermediateTableName = true)]
        //public XPCollection<Labware> Labwares
        //{
        //    get { return GetCollection<Labware>("Labwares"); }
        //}
        #endregion
    }
}