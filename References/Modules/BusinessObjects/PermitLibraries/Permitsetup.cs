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
    public enum Opreators
    {
        [XafDisplayName("<")] LessThan,
        [XafDisplayName("<=")]LessThanEqual,
        [XafDisplayName(">")]GraterThen,
        [XafDisplayName(">=")] GraterThenEqual,
        [XafDisplayName("<>")] NotEqual,
        [XafDisplayName("=")] Equal,
        Inbetween
    }
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Permitsetup : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Permitsetup(Session session)
            : base(session)
        {
        }
        
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private Opreators _Operator;
        private string _Permit;
        private Unit _Units; 
        private string _Permit2;
        private Unit _Permit2Units;
        private Parameter _Parameter;
        private PermitLibrary _PermitLibrary;
        public Parameter Parameter
        {
            get { return _Parameter; }
            set { SetPropertyValue(nameof(Parameter), ref _Parameter, value); }
        }
        public Opreators Operator
        {
            get { return _Operator; }
            set { SetPropertyValue(nameof(Operator), ref _Operator, value); }
        }
        public string Permit
        {
            get { return _Permit; }
            set { SetPropertyValue(nameof(Permit), ref _Permit, value); }
        } 
        public Unit Units
        {
            get { return _Units; }
            set { SetPropertyValue(nameof(Units), ref _Units, value); }
        } 
        public string Permit2
        {
            get { return _Permit2; }
            set { SetPropertyValue(nameof(Permit2), ref _Permit2, value); }
        } 
        public Unit Permit2Units
        {
            get { return _Permit2Units; }
            set { SetPropertyValue(nameof(Permit2Units), ref _Permit2Units, value); }
        }
        [Association("PermitLibrary-Permits")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public PermitLibrary PermitLibrary
        {
            get { return _PermitLibrary; }
            set { SetPropertyValue(nameof(PermitLibrary), ref _PermitLibrary, value); }
        }
    }
}