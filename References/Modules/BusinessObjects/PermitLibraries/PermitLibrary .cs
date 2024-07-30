using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Modules.BusinessObjects.Setting.PermitLibraries
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PermitLibrary : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PermitLibrary(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            {
                if (string.IsNullOrEmpty(PermitID))
                {
                    CriteriaOperator orgCriteria = CriteriaOperator.Parse("Max(SUBSTRING(PermitID, 4))");
                    object maxOrgID = Session.Evaluate(typeof(PermitLibrary), orgCriteria, null);
                    int nextOrgIDNumber = 1;
                    if (maxOrgID != null && maxOrgID != DBNull.Value)
                    {
                        nextOrgIDNumber = Convert.ToInt32(maxOrgID) + 1;
                    }
                    string nextOrgID = $"P{nextOrgIDNumber:D3}";
                    PermitID = nextOrgID;
                }
            }
            //if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            //SubmittedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            //CreatedDate = Library.GetServerTime(Session);
        }
        private string _PermitName;
        private PermitTypes _PermitType;
        private string _PermitNumber;
        private VisualMatrix _SampleMatrix;
        private DateTime _EffectiveDate;
        private DateTime _EndDate;
        private string _Remark;
        private bool _Retire;
        private string _PermitID;

        public string PermitID
        {
            get { return _PermitID; }
            set { SetPropertyValue(nameof(PermitID), ref _PermitID, value); }
        }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PermitName
        {
            get { return _PermitName; }
            set { SetPropertyValue(nameof(PermitName), ref _PermitName, value); }
        }
        public PermitTypes PermitType
        {
            get { return _PermitType; }
            set { SetPropertyValue(nameof(PermitType), ref _PermitType, value); }
        }   
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PermitNumber
        {
            get { return _PermitNumber; }
            set { SetPropertyValue(nameof(PermitNumber), ref _PermitNumber, value); }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public VisualMatrix SampleMatrix
        {
            get { return _SampleMatrix; }
            set { SetPropertyValue(nameof(SampleMatrix), ref _SampleMatrix, value); }
        }

        public DateTime EffectiveDate
        {
            get { return _EffectiveDate; }
            set { SetPropertyValue(nameof(EffectiveDate), ref _EffectiveDate, value); }
        }

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { SetPropertyValue(nameof(EndDate), ref _EndDate, value); }
        }
        public bool Retire
        {
            get { return _Retire; }
            set { SetPropertyValue(nameof(Retire), ref _Retire, value); }
        }
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }
        [Association("PermitLibrary-Permits")]
        public XPCollection<Permitsetup> Permitsetups
        {
            get { return GetCollection<Permitsetup>("Permitsetups"); }
        }

    }
}