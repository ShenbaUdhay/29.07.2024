using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SamplingManagement;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Image : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Image(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Sort = 0;
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
        #region ImageUpload
        private byte[] _ImageUpload;
        /// <summary>
        /// 照片
        /// </summary>
        ///  
        [RuleRequiredField("Image.ImageUpload", DefaultContexts.Save)]
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 100)]
        public byte[] ImageUpload
        {
            get
            {
                return _ImageUpload;
            }
            set
            {
                SetPropertyValue("ImageUpload", ref _ImageUpload, value);
            }
        }
        #endregion

        //private Samplecheckin _JobID;

        //public Samplecheckin JobID
        //{
        //    get { return _JobID; }
        //    set { SetPropertyValue("JobID", ref _JobID, value); }
        //}
        private string _Name;
        [RuleRequiredField(DefaultContexts.Save)]

        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }
        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }
        }
        private int _Sort;
        [RuleRange(0, int.MaxValue, CustomMessageTemplate = "Sort value must be non-negative.")]
        public int Sort
        {
            get { return _Sort; }
            set { SetPropertyValue("Sort", ref _Sort, value); }
        }
        [Association("SampleCheckinImage", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<Samplecheckin> Samplecheckins
        {
            get { return GetCollection<Samplecheckin>("Samplecheckins"); }
        }

        //[Association("SamplingProposalImage", UseAssociationNameAsIntermediateTableName = true)]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //public XPCollection<SamplingProposal> SamplingProposals
        //{
        //    get { return GetCollection<SamplingProposal>("SamplingProposals"); }
        //}

    }
}