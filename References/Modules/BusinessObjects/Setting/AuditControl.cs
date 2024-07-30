using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DomainComponent]
    public class AuditControl
    {
        [VisibleInDetailView(false)]
        public Guid ID { get; set; }

        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public byte[] Text { get; set; }
    }
}