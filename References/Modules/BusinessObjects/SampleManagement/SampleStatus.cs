using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    [DefaultProperty("Samplestatus")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SampleStatus : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleStatus(Session session)
            : base(session)
        {
        }
        public enum colors
        {
            None,
            Blue,
            Cyan,
            Green,
            LightBlue,
            LightGreen,
            LightYellow,
            Red,
            Skyblue,
            Yellow

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region Samplestatus
        private string _Samplestatus;
        [ImmediatePostData]
        [RuleRequiredField("Samplestatus1", DefaultContexts.Save,"'Sample status must not be empty'")]
        public string Samplestatus
        {
            get { return _Samplestatus; }
            set { SetPropertyValue("Samplestatus", ref _Samplestatus, value); }
            
        }
        #endregion

        #region hold
        private bool _Hold;
        [ImmediatePostData]
        public bool Hold
        {
            get { return _Hold; }
            set { SetPropertyValue("Hold", ref _Hold, value); }
        }
        #endregion

        #region Samplinghold
        private bool _Samplinghold;
        [ImmediatePostData]
        public bool Samplinghold
        {
            get { return _Samplinghold; }
            set { SetPropertyValue("Samplinghold", ref _Samplinghold, value); }
        }
        #endregion

        #region description
        private string _Description;
        [ImmediatePostData]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue("Samplestatus", ref _Description, value); }

        }
        #endregion

        #region Colors
        private colors _Color;
        [ImmediatePostData]
        public colors Color
        {
            get { return _Color; }
            set { SetPropertyValue("Color", ref _Color, value); }
        }
        #endregion
    }
}