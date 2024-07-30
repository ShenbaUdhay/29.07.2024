using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using Modules.BusinessObjects.Hr;
using System;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    //[FileAttachmentAttribute("Upload")]
    [Appearance("VendorReagentCertificateBackColor", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "VendorReagentCertificate_ListView;VendorReagentCertificate_ListView_ViewCertificateLog;",
    Criteria = "[Upload].Count()>0", BackColor = "#5cd65c", FontColor = "White")]
    public class VendorReagentCertificate : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public VendorReagentCertificate(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).      
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
        //#region lt
        //private string _LT;
        //public string LT
        //{
        //    get { return _LT; }
        //    set { SetPropertyValue<string>("LT", ref _LT, value); }
        //}
        //#endregion
        #region vendorlt
        private string _VendorLT;
        public string VendorLT
        {
            get { return _VendorLT; }
            set { SetPropertyValue<string>("VendorLT", ref _VendorLT, value); }
        }
        #endregion
        //#region N/A
        //private bool _NA;
        //public bool NA
        //{
        //    get { return _NA; }
        //    set { SetPropertyValue<bool>("NA", ref _NA, value); }

        //}
        //#endregion
        #region vendor
        private Vendors _Vendor;
        //[RuleRequiredField("fVendor", DefaultContexts.Save)]
        public Vendors Vendor
        {
            get { return _Vendor; }
            set { SetPropertyValue("Vendor", ref _Vendor, value); }
        }
        #endregion
        #region Item
        private Items _Item;
        public Items Item
        {
            get { return _Item; }
            set { SetPropertyValue("Item", ref _Item, value); }
        }
        #endregion
        #region Catelog
        private string _Catelog;
        public string Catelog
        {
            get { return _Catelog; }
            set { SetPropertyValue("Catelog", ref _Catelog, value); }
        }
        #endregion


        //#region Preview
        //private string _Preview;
        //public string Preview
        //{
        //    get { return _Preview; }
        //    set { SetPropertyValue("Preview", ref _Preview, value); }
        //}
        //#endregion
        #region Requestor
        private Employee _Requestor;
        public Employee Requestor
        {
            get { return _Requestor; }
            set { SetPropertyValue<Employee>("Requestor", ref _Requestor, value); }
        }
        #endregion
        #region Department
        private string _Department;
        public string Department
        {
            get { return _Department; }
            set { SetPropertyValue<string>("Department", ref _Department, value); }
        }
        #endregion
        #region LoadedBy
        private Employee _LoadedBy;
        public Employee LoadedBy
        {
            get { return _LoadedBy; }
            set { SetPropertyValue<Employee>("LoadedBy", ref _LoadedBy, value); }
        }
        #endregion
        #region LoadedBy
        private DateTime? _LoadedDate;
        public DateTime? LoadedDate
        {
            get { return _LoadedDate; }
            set { SetPropertyValue<DateTime?>("LoadedDate", ref _LoadedDate, value); }
        }
        #endregion
        #region rqid
        private string _RQID;
        public string RQID
        {
            get { return _RQID; }
            set { SetPropertyValue<string>("RQID", ref _RQID, value); }
        }
        #endregion
        #region poid
        private Purchaseorder _POID;
        public Purchaseorder POID
        {
            get { return _POID; }
            set { SetPropertyValue<Purchaseorder>("POID", ref _POID, value); }
        }
        #endregion

        #region receiveid
        private string _ReceiveID;
        public string ReceiveID
        {
            get { return _ReceiveID; }
            set { SetPropertyValue<string>("ReceiveID", ref _ReceiveID, value); }
        }
        #endregion
        #region Comments
        private string _Comments;
        [Size(1000)]
        public string Comments
        {
            get { return _Comments; }
            set { SetPropertyValue<string>("Comments", ref _Comments, value); }
        }
        #endregion

        // private Requisition _Requisition;
        [NonPersistent]
        [VisibleInDetailView(false)]
        public Requisition Requisition
        {
            get
            {
                if (POID != null)
                {
                    return Session.FindObject<Requisition>(CriteriaOperator.Parse("[Item] =? and [POID] =?", Item, POID));
                    //return _Requisition;
                }
                return null;
            }
            //set
            //{
            //    SetPropertyValue<Requisition>(nameof(Requisition), ref _Requisition, value);
            //}
        }




        //#region FileSourcePath
        //private string _FileSourcePath;
        //public string FileSourcePath
        //{
        //    get { return _FileSourcePath; }
        //    set { SetPropertyValue<string>("FileSourcePath", ref _FileSourcePath, value); }
        //}
        //#endregion
        //#region Destination
        //private string _Destination;
        //public string Destination
        //{
        //    get { return _Destination; }
        //    set { SetPropertyValue<string>("Destination", ref _Destination, value); }
        //}
        //#endregion

        //#region Upload
        //private FileData _Upload;

        //public FileData Upload
        //{
        //    get { return _Upload; }
        //    set { SetPropertyValue("Upload", ref _Upload, value); }
        //}
        //#endregion
        [Association("VendorReagentCertificate_FileUploadCollectionVRC", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<FileUploadCollectionVRC> Upload
        {
            get
            {
                return GetCollection<FileUploadCollectionVRC>("Upload");
            }
        }

        [Association("VendorReagentCertificate_Distribution")]
        public XPCollection<Distribution> LT
        {
            get
            {
                return GetCollection<Distribution>("LT");
            }
        }


    }

    public class FileUploadCollectionVRC : FileAttachmentBase
    {
        public FileUploadCollectionVRC(Session session) : base(session)
        {
        }

        [Association("VendorReagentCertificate_FileUploadCollectionVRC", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<VendorReagentCertificate> VendorReagentCertificate
        {
            get
            {
                return GetCollection<VendorReagentCertificate>("VendorReagentCertificate");
            }
        }

        //public static explicit operator FileUploadCollection(XPCollection<FileUploadCollection> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}