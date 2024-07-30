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

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ICMProject : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ICMProject(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region ProjectName
        private string _ProjectName;
        //[RuleRequiredField]
        [RuleRequiredField("ProjectName", DefaultContexts.Save, "'Project' must not be empty")]

        [RuleUniqueValue]
        public string ProjectName
        {
            get { return _ProjectName; }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetPropertyValue("ProjectName", ref _ProjectName, value.Trim());
            }
        }
        #endregion

        #region Comment
        private string _Comment;
        [Size(1000)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }
        }
        #endregion
    }
}