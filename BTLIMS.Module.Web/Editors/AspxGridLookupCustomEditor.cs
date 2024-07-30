using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors
{
    [PropertyEditor(typeof(string), false)]
    public partial class AspxGridLookupCustomEditor : ASPxPropertyEditor
    {
        public AspxGridLookupCustomEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override WebControl CreateEditModeControlCore()
        {
            return new ASPxGridLookup();
        }
    }
}
