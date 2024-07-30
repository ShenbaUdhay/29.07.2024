using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    //[RuleCombinationOfPropertiesIsUnique("Items", DefaultContexts.Save, "items,Specification,Vendor,VendorCatName,Amount,AmountUnit", SkipNullOrEmptyValues = false)]
    //[RuleCombinationOfPropertiesIsUnique("Items", DefaultContexts.Save, "items,Specification,Vendor", SkipNullOrEmptyValues = false)]
    [RuleCombinationOfPropertiesIsUnique("Items", DefaultContexts.Save, "items,Specification,Vendor,VendorCatName", SkipNullOrEmptyValues = false)]
    //[RuleCombinationOfPropertiesIsUnique("Items", DefaultContexts.Save, "items", SkipNullOrEmptyValues = false)]
    //[Appearance("ItemsBackColor", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "Items_ListView_Copy;",
    //Criteria = "IsNullOrEmpty([ItemCode]) And IsNullOrEmpty([Errorlog])", BackColor = "#d7f9c7")]
    public class Items : BaseObject
    {
        #region Construcotr
        Session sessionuser;
        ////itemsvaluemanager packageunits = new itemsvaluemanager();
        public Items(Session session) : base(session)
        {
            sessionuser = session;
        }
        #endregion

        #region DefaultMethods
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);

            //if (string.IsNullOrEmpty(ItemCode) && !packageunits.IsSavingImportedItems)
            //{
            //    ItemCode = (Convert.ToInt32(Session.Evaluate<Items>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Max(ItemCode)"), null)) + 1).ToString();
            //    if (ItemCode.Length == 1)
            //    {
            //        ItemCode = "1000" + ItemCode;
            //    }
            //    else if (ItemCode.Length == 2)
            //    {
            //        ItemCode = "100" + ItemCode;
            //    } 
            //    else if (ItemCode.Length == 3)
            //    {
            //        ItemCode = "10" + ItemCode;
            //    } 
            //    else if (ItemCode.Length == 4)
            //    {
            //        ItemCode = "1" + ItemCode;
            //    } 
            //}
        }
        protected override void OnSaved()
        {
            base.OnSaved();
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            ItemUnit = 1;
            // UnitPrice = 1;
            //ItemCode += Convert.ToInt32(Session.Evaluate<Items>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Max(ItemCode)"), null)) + 1;
            //if (ItemCode.Length == 1)
            //{
            //    ItemCode = "00" + ItemCode;
            //}
            //else if (ItemCode.Length == 2)
            //{
            //    ItemCode = "0" + ItemCode;
            //}
            fIsLabLT = true;
            fIsVendorLT = true;

        }
        #endregion

        #region OnDelete
        protected override void OnDeleting()
        {
            base.OnDeleting();
            //if (Session.CollectReferencingObjects(this).Count > 0)
            //{
            //    foreach (BaseObject obj in Session.CollectReferencingObjects(this))
            //    {
            //        if (obj.Oid != null)
            //        {
            //            Exception ex = new Exception("Already Used Can't allow to Delete");
            //            throw ex;
            //            break;

            //        }
            //    }
            //}
            System.Collections.ICollection lstReferenceObjects = Session.CollectReferencingObjects(this);
            if (lstReferenceObjects.Count > 0)
            {
                foreach (var obj in Session.CollectReferencingObjects(this))
                {
                    //if (obj.GetType() == typeof(BaseObject))
                    //{
                    //    if (((BaseObject)obj).Oid != null)
                    //    {
                    //        Exception ex = new Exception("Already Used Can't allow to Delete");
                    //        throw ex;
                    //        break;
                    //    } 
                    //}
                    //else 
                    if (obj.GetType() != typeof(DevExpress.Xpo.Metadata.Helpers.IntermediateObject) && obj.GetType() != typeof(SubItems) && obj.GetType() != typeof(ItemsUpload) && obj.GetType() != typeof(NFPAGHSKey))
                    {
                        Exception ex = new Exception("Already Used Can't allow to Delete");
                        throw ex;
                        break;
                    }
                }
            }
        }
        #endregion

        #region itemcode
        [Indexed(Unique = true)]
        private string fItemCode;
        //[Appearance("Vendor",  = false, Criteria = "Status <> 'PendingReview'", Context = "DetailView")]
        public string ItemCode
        {
            get { return fItemCode; }
            set { SetPropertyValue<string>("ItemCode", ref fItemCode, value); }
        }
        #endregion

        #region items
        string fitems;
        [RuleRequiredField("items", DefaultContexts.Save)]
        //  [RuleStringComparison("RuleStringComparison_Items_NotNullOrEmpty", DefaultContexts.Save, StringComparisonType.NotEquals, null)]
        [XafDisplayName("Item")]
        public string items
        {
            get { return fitems; }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetPropertyValue<string>("items", ref fitems, value.Trim());
            }
        }
        #endregion

        #region itemdescription
        string fItemDescription;
        [Size(1000)]
        public string ItemDescription
        {
            get { return fItemDescription; }
            set { SetPropertyValue<string>("ItemDescription", ref fItemDescription, value); }
        }
        #endregion

        #region specification
        string fSpecification;
        [XafDisplayName("Description")]
        //[RuleRequiredField("Specification", DefaultContexts.Save)]
        public string Specification
        {
            get { return fSpecification; }
            set { SetPropertyValue<string>("Specification", ref fSpecification, value); }
        }
        #endregion

        #region category
        private Category fCategory;
        //[RuleRequiredField("Category", DefaultContexts.Save)]
        public Category Category
        {
            get { return fCategory; }
            set { SetPropertyValue("Category", ref fCategory, value); }
        }
        #endregion

        #region grade
        private Grades fGrade;
        //[RuleRequiredField("Grade1", DefaultContexts.Save)]
        public Grades Grade
        {
            get { return fGrade; }
            set { SetPropertyValue("Grade", ref fGrade, value); }
        }
        #endregion

        #region unit
        private Packageunits fUnit;
        [RuleRequiredField("Packageunits", DefaultContexts.Save, "Pack Unit must not be empty")]
        [XafDisplayName("PackUnit")]
        public Packageunits Unit
        {
            get { return fUnit; }
            set { SetPropertyValue("Unit", ref fUnit, value); }
        }
        #endregion

        #region Unitprice   
        private double fUnitPrice;
        [RuleRequiredField("Unitprice", DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [RuleRange(1, 100000)]
        public double UnitPrice
        {
            get { return fUnitPrice; }
            set { SetPropertyValue<double>("UnitPrice", ref fUnitPrice, value); }
        }
        #endregion

        #region stockqty
        private int fStockQty;
        [RuleRange(0, 100000)]
        public int StockQty
        {
            get { return fStockQty; }
            set { SetPropertyValue<int>("StockQty", ref fStockQty, value); }
        }
        #endregion

        #region itemunit
        private int fItemUnit;
        [RuleRange(0, 10000)]
        //[RuleRequiredField("ItemUnit", DefaultContexts.Save)]
        public int ItemUnit
        {
            get { return fItemUnit; }
            set { SetPropertyValue<int>("ItemUnit", ref fItemUnit, value); }
        }
        #endregion

        #region alertqty
        private int fAlertQty;
        [RuleRange(0, 10000)]
        //[RuleRequiredField("AlertQty", DefaultContexts.Save)]
        public int AlertQty
        {
            get { return fAlertQty; }
            set { SetPropertyValue<int>("AlertQty", ref fAlertQty, value); }
        }
        #endregion

        #region subitemname
        [Association("SubItemslink", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<SubItems> SubItemsName
        {
            get { return GetCollection<SubItems>("SubItemsName"); }
        }
        #endregion

        #region vendorlt
        private Boolean fIsVendorLT;
        public Boolean IsVendorLT
        {
            get { return fIsVendorLT; }
            set { SetPropertyValue<Boolean>("IsVendorLT", ref fIsVendorLT, value); }
        }
        #endregion
        #region IsFractional
        private Boolean _IsFractional;
        [ImmediatePostData]
        public Boolean IsFractional
        {
            get { return _IsFractional; }
            set { SetPropertyValue<Boolean>("IsFractional", ref _IsFractional, value); }
        }
        #endregion
        #region lablt
        private Boolean fIsLabLT;
        public Boolean IsLabLT
        {
            get { return fIsLabLT; }
            set { SetPropertyValue<Boolean>("IsLabLT", ref fIsLabLT, value); }
        }
        #endregion

        #region toxic
        private Boolean fIsToxic;
        public Boolean IsToxic
        {
            get { return fIsToxic; }
            set { SetPropertyValue<Boolean>("IsToxic", ref fIsToxic, value); }
        }
        #endregion

        #region upload
        [DevExpress.ExpressApp.DC.Aggregated, Association("Items-ItemsFileData")]
        public XPCollection<ItemsUpload> Upload
        {
            get { return GetCollection<ItemsUpload>("Upload"); }
        }
        #endregion

        #region comment
        string fComment;
        [Size(1000)]
        public string Comment
        {
            get { return fComment; }
            set { SetPropertyValue<string>("Comment", ref fComment, value); }
        }
        #endregion

        #region retiredate
        DateTime fRetireDate;
        public DateTime RetireDate
        {
            get { return fRetireDate; }
            set { SetPropertyValue<DateTime>("RetireDate", ref fRetireDate, value); }
        }
        #endregion

        #region Manufacturer
        private Manufacturer _Manufacturer;
        public Manufacturer Manufacturer
        {
            get { return _Manufacturer; }
            set { SetPropertyValue("Manufacturer", ref _Manufacturer, value); }
        }
        #endregion

        #region Mfritemname
        private string _MfritemName;
        public string MfritemName
        {
            get { return _MfritemName; }
            set { SetPropertyValue("MfritemName", ref _MfritemName, value); }
        }
        #endregion

        #region MfrcatNum
        private string _MfrcatNum;
        public string MfrcatNum
        {
            get { return _MfrcatNum; }
            set { SetPropertyValue("MfrcatNum", ref _MfrcatNum, value); }
        }
        #endregion

        #region Vendor
        private Vendors _Vendor;
        [RuleRequiredField("ItemVendor", DefaultContexts.Save)]
        public Vendors Vendor
        {
            get { return _Vendor; }
            set { SetPropertyValue("Vendor", ref _Vendor, value); }
        }
        #endregion

        #region VendorItemName
        private string _VendorItemName;
        public string VendorItemName
        {
            get { return _VendorItemName; }
            set { SetPropertyValue("VendorItemName", ref _VendorItemName, value); }
        }
        #endregion

        #region VendorCatName
        private string _VendorCatName;
        ////[RuleRequiredField("VendorCatName", DefaultContexts.Save)]
        public string VendorCatName
        {
            get { return _VendorCatName; }
            set { SetPropertyValue("VendorCatName", ref _VendorCatName, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public Employee ModifiedBy
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

        #region Item Number
        private string _ItemNumber;
        [Browsable(false)]
        public string ItemNumber
        {
            get { return _ItemNumber; }
            set { SetPropertyValue("ItemNumber", ref _ItemNumber, value); }
        }
        #endregion

        ////private ProductCode _ASIProductCode;
        ////// [Browsable(false)]
        ////public ProductCode ASIProductCode
        ////{

        ////    get
        ////    {
        ////        return _ASIProductCode;
        ////    }
        ////    set
        ////    {
        ////        SetPropertyValue<ProductCode>("ASIProductCode", ref _ASIProductCode, value);
        ////    }
        ////}

        #region Matrix
        private Matrix _Matrix;
        [Browsable(false)]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue("Matrix", ref _Matrix, value); }
        }
        #endregion

        #region NFPABlue
        private string _NFPABlue;
        [Browsable(false)]
        public string NFPABlue
        {
            get { return _NFPABlue; }
            set { SetPropertyValue("NFPABlue", ref _NFPABlue, value); }
        }
        #endregion

        #region NFPARed
        private string _NFPARed;
        [Browsable(false)]
        public string NFPARed
        {
            get { return _NFPARed; }
            set { SetPropertyValue("NFPARed", ref _NFPARed, value); }
        }
        #endregion

        #region NFPAYellow
        private string _NFPAYellow;
        [Browsable(false)]
        public string NFPAYellow
        {
            get { return _NFPAYellow; }
            set { SetPropertyValue("NFPAYellow", ref _NFPAYellow, value); }
        }
        #endregion

        #region NFPAWhite
        private string _NFPAWhite;
        [Browsable(false)]
        public string NFPAWhite
        {
            get { return _NFPAWhite; }
            set { SetPropertyValue("NFPAWhite", ref _NFPAWhite, value); }
        }
        #endregion

        #region ExportedItems
        private bool _bolExportedItems;
        [Browsable(false)]
        public bool bolExportedItems
        {
            get { return _bolExportedItems; }
            set { SetPropertyValue<bool>("bolExportedItems", ref _bolExportedItems, value); }
        }
        #endregion

        #region ResaleItems
        private bool _bolResaleItems;
        public bool bolResaleItems
        {
            get { return _bolResaleItems; }
            set { SetPropertyValue<bool>("bolResaleItems", ref _bolResaleItems, value); }
        }
        #endregion

        [NonPersistent]
        private bool _IsASIProduct;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool IsASIProduct
        {
            get
            {
                if (Vendor != null && Vendor.Vendorcode == "007")
                {
                    _IsASIProduct = true;
                }
                else
                {
                    _IsASIProduct = false;
                }
                return _IsASIProduct;
            }
        }
        #region NFPA
        [Association("NFPAItems", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<NFPAGHSKey> NFPAGHSKeys
        {
            get { return GetCollection<NFPAGHSKey>("NFPAGHSKeys"); }
        }
        #endregion

        private string _Amount;
        [RuleRequiredField(TargetCriteria = "IsFractional=True")]
        [XafDisplayName("Amount")]
        [ImmediatePostData]
        [Appearance("Amounthide", Visibility = ViewItemVisibility.Hide, Criteria = "IsFractional=false", Context = "DetailView")]
        [Appearance("Amountshow", Visibility = ViewItemVisibility.Show, Criteria = "IsFractional=true", Context = "DetailView")]
        public string Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetPropertyValue("Amount", ref _Amount, value.Trim());
            }
        }

        private Unit _AmountUnit;
        [RuleRequiredField(TargetCriteria = "IsFractional=True")]
        [XafDisplayName("AmountUnit")]

        [ImmediatePostData]
        [Appearance("AmountUnithide", Visibility  = ViewItemVisibility.Hide, Criteria = "IsFractional=false", Context = "DetailView")]
        [Appearance("AmountUnitshow", Visibility = ViewItemVisibility.Show, Criteria = "IsFractional=true", Context = "DetailView")]
        public Unit AmountUnit
        {
            get
            {
                return _AmountUnit;
            }
            set { SetPropertyValue("AmountUnit", ref _AmountUnit, value); }
        }
        private string _NpAmount;
        [NonPersistent]
        [ImmediatePostData]
        [Appearance("NPAmountShow", Visibility = ViewItemVisibility.Show, Criteria = "IsFractional=false", Context = "DetailView")]
        [Appearance("NPAmountHide", Visibility = ViewItemVisibility.Hide, Criteria = "IsFractional=true", Context = "DetailView")]
        [XafDisplayName("Amount")]
        public string NpAmount
        {
            get 
            {
                return _NpAmount; 
            }
            set
            {
                SetPropertyValue("Amount", ref _NpAmount, value.Trim());
            }
        }

        private Unit _NPAmountUnit;
        [NonPersistent]
        [ImmediatePostData]
        [Appearance("NpAmountUnitShow", Visibility = ViewItemVisibility.Show, Criteria = "IsFractional=false", Context = "DetailView")]
        [Appearance("NPAmountUnitHide", Visibility = ViewItemVisibility.Hide, Criteria = "IsFractional=true", Context = "DetailView")]
        [XafDisplayName("Amount Units")]
        public Unit NPAmountUnit
        {
            get 
            {
                return _NPAmountUnit; 
            }
            set { SetPropertyValue("NPAmountUnit", ref _NPAmountUnit, value); }
        }

        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public double Totamount
        {
            get
            {
                if (!string.IsNullOrEmpty(Amount) && Amount != null && StockQty != 0)
                {
                    return Convert.ToDouble(Amount) * StockQty;
                }
                return 0;
            }
        }

        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public string Storage
        {
            get
            {
                var strstorage = string.Empty;
                if (ItemCode != null)
                {
                    string[] storage = sessionuser.Query<Distribution>().Where(p => p.Item.Oid == Oid && (p.Status == Distribution.LTStatus.PendingConsume || p.Status == Distribution.LTStatus.PendingDispose) && !string.IsNullOrEmpty(p.Storage.storage)).Select(p => p.Storage.storage).Distinct().ToArray();
                    //string[] storage = sessionuser.Query<Distribution>().Where(p => p.Item.ItemCode == ItemCode && p.Status == Distribution.LTStatus.PendingConsume && !string.IsNullOrEmpty(p.Storage.storage)).Select(p => p.Storage.storage).Distinct().ToArray();
                    if (storage.Length > 0)
                    {
                        strstorage = String.Join(", ", storage);
                    }
                }
                return strstorage;
            }
        }

        #region ReminderDaysInAdvance
        private int _ReminderDaysInAdvance;
        public int ReminderDaysInAdvance
        {
            get { return _ReminderDaysInAdvance; }
            set { SetPropertyValue("ReminderDaysInAdvance", ref _ReminderDaysInAdvance, value); }
        }
        #endregion

        #region Department
        private Department _Department;

        public Department Department
        {
            get { return _Department; }
            set { SetPropertyValue("Department", ref _Department, value); }
        }
        #endregion

        #region Errorlog
        [NonPersistent]
        private string fErrorlog;
        public string Errorlog
        {
            get { return fErrorlog; }
            set { SetPropertyValue<string>("Errorlog", ref fErrorlog, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }
        #endregion

        private string _Size;
        public string Size
        {
            get { return _Size; }
            set { SetPropertyValue("Size", ref _Size, value); }
        }

        #region additionalqty
        private uint fAdditionalQty;
        //[NonPersistent]
        [ImmediatePostData]
        [RuleRange(0, 10000)]
        public uint AdditionalQty
        {
            get { return fAdditionalQty; }
            set { SetPropertyValue<uint>("AdditionalQty", ref fAdditionalQty, value); }
        }
        #endregion

        //private string _ProductCode;
        //[VisibleInListView(false),VisibleInDetailView(false),VisibleInLookupListView(false)]
        //public string ProductCode
        //{
        //    get { return _ProductCode; }
        //    set { SetPropertyValue("ProductCode", ref _ProductCode, value); }
        //}

        //#region Many To Many for TestMethod
        //[Association]
        //public XPCollection<TestMethod> TestMethods
        //{
        //    get { return GetCollection<TestMethod>(nameof(TestMethods)); }
        //}
        //#endregion

        private string _Tests;
        [NonPersistent]
        public string Tests
        {
            //get { return string.Join(",", Linkparameters.Select(i => i.LinkTestMethod.TestName).Distinct().ToList()); }
            get
            {
                if (TestMethods.Count > 0)
                {
                    return string.Join(",", TestMethods.Where(i => i != null).Select(i => i.TestName).Distinct().ToList());
                }
                else
                {
                    return string.Empty;
                }
            }
            set { SetPropertyValue("Tests", ref _Tests, value); }
        }

        //#region ManyToManyAlias
        //[Association("Items-TestMethodLink"), Browsable(false)]
        //public IList<Linkparameter> Linkparameters
        //{
        //    get
        //    {
        //        return GetList<Linkparameter>("Linkparameters");
        //    }
        //}
        //[ManyToManyAlias("Linkparameters", "LinkTestMethod")]
        //public IList<TestMethod> TestMethods
        //{
        //    get
        //    {
        //        return GetList<TestMethod>("TestMethods");
        //    }
        //} 
        //#endregion

        [Association("Items-TestMethodLinks")]
        public IList<ItemTestMethodLink> Linkparameters
        {
            get
            {
                return GetList<ItemTestMethodLink>("Linkparameters");
            }
        }

        [ManyToManyAlias("Linkparameters", "LinkTestMethod")]
        public IList<TestMethod> TestMethods
        {
            get
            {
                return GetList<TestMethod>("TestMethods");
            }
        }


    }
}