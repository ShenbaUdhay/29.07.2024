using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;

namespace Labmaster.Module.Web.Editors
{
    [PropertyEditor(typeof(bool), "CustomBoolPropertyEditor", false)]
    public class ASPxBooleanPropertyEditorEx : ASPxBooleanPropertyEditor
    {
        public ASPxBooleanPropertyEditorEx(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        public override string Caption
        {
            get
            {
                if(this.Model.Caption=="Password")
                {
                    return this.Model.Caption + ":*";
                }
                return this.Model.Caption + ":";
            }
        }
        private static void SetCaption(object control)
        {
            if (control is ASPxCheckBox)
            {
                ((ASPxCheckBox)control).Text = string.Empty;
            }
        }
        protected override void SetupControl(System.Web.UI.WebControls.WebControl control)
        {
            base.SetupControl(control);
            SetCaption(control);
        }
        protected override System.Web.UI.WebControls.WebControl CreateEditModeControlCore()
        {
            var control = base.CreateEditModeControlCore();
            control.Load += control_Load;
            return control;
        }
        void control_Load(object sender, EventArgs e)
        {
            SetCaption(sender);
        }
    }
    //    [DefaultClassOptions]
    //    [PropertyEditor(typeof(bool), true)]
    //    //[ImageName("BO_Contact")]
    //    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //    //[Persistent("DatabaseTableName")]
    //    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    //    public class AspxBooleanPropertyEditor : BaseObject
    //    {


    //        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
    //        public AspxBooleanPropertyEditor(Session session)
    //            : base(session)
    //        {

    //        }
    //        public override void AfterConstruction()
    //        {
    //            base.AfterConstruction();
    //            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
    //        }
    //       // public ASPxBooleanPropertyEditorEx(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
    //        //private string _PersistentProperty;
    //        //[XafDisplayName("My display name"), ToolTip("My hint message")]
    //        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
    //        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
    //        //public string PersistentProperty {
    //        //    get { return _PersistentProperty; }
    //        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
    //        //}

    //        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
    //        //public void ActionMethod() {
    //        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
    //        //    this.PersistentProperty = "Paid";
    //        //}
    //    }
}