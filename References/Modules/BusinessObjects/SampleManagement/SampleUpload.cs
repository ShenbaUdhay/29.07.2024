using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SampleUpload : BaseObject//FileAttachmentBase
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleUpload(Session session)
            : base(session)
        {
        }

        #region DefaultMethods
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region ID
        int _ID;
        [ReadOnly(true)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public int ID
        {
            get { return _ID; }
            set { SetPropertyValue<int>(nameof(ID), ref _ID, value); }
        }
        #endregion

        #region SamplePhoto
        private byte[] _image;
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 400)]
        //[ImageEditor(ListViewImageEditorCustomHeight = 50)]
        [RuleRequiredField]
        public byte[] SamplePhoto
        {
            get { return _image; }
            set { SetPropertyValue<byte[]>(nameof(SamplePhoto), ref _image, value); }
        }
        #endregion

        #region Name
        private string _Name;
        [RuleRequiredField]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>(nameof(Name), ref _Name, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>(nameof(Comment), ref _Comment, value); }
        }
        #endregion

        #region Sample
        protected Samplecheckin _Sample;
        [Persistent, Association("Sample-SampleUpload")]
        public Samplecheckin Sample
        {
            get { return _Sample; }
            set
            {
                SetPropertyValue(nameof(Sample), ref _Sample, value);
                if (value != null && ID == 0)
                {
                    ID = Convert.ToInt32(Session.Evaluate(typeof(SampleUpload), CriteriaOperator.Parse("MAX(ID)"), CriteriaOperator.Parse("[Sample.Oid]=?", Sample.Oid))) + 1;
                }
            }
        }
        #endregion

        //#region IndoorInspection
        //protected IndoorInspection _IndoorInspection;
        //[Persistent, Association("IndoorInspection-SampleUpload")]
        //public IndoorInspection IndoorInspection
        //{
        //    get
        //    {
        //        return _IndoorInspection;
        //    }
        //    set
        //    {
        //        SetPropertyValue(nameof(IndoorInspection), ref _IndoorInspection, value);
        //        if (value != null && ID == 0)
        //        {
        //            ID = Convert.ToInt32(Session.Evaluate(typeof(SampleUpload), CriteriaOperator.Parse("MAX(ID)"), CriteriaOperator.Parse("[IndoorInspection.Oid]=?", IndoorInspection.Oid))) + 1;
        //        }
        //    }
        //}
        //#endregion

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
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