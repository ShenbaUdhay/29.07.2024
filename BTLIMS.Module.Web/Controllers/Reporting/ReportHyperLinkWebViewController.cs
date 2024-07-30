using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;
using System.Web.UI.WebControls;

namespace HyperLinkPropertyEditor.Web
{
    [PropertyEditor(typeof(System.String), "HyperLinkStringPropertyEditor", false)]
    public class WebHyperLinkStringPropertyEditor : ASPxPropertyEditor
    {
        #region Declaration
        ////Dennis TODO: This is to be setup via the Model Editor at the ViewItems | PropertyEditors | HyperLinkStringPropertyEditor level once.
        ////public const string UrlEmailMask = @"(((http|https|ftp)\://)?[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;amp;%\$#\=~])*)|([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})";
        #endregion

        #region Constructor
        public WebHyperLinkStringPropertyEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info)
        {
            this.CancelClickEventPropagation = true;
        }
        #endregion

        #region Function
        protected override WebControl CreateEditModeControlCore()
        {
            try
            {
                if (AllowEdit.ResultValue)
                {
                    ASPxTextBox textBox = RenderHelper.CreateASPxTextBox();
                    textBox.ID = "textBox";
                    textBox.TextChanged += EditValueChangedHandler;
                    return textBox;
                }
                else
                {
                    return CreateViewModeControlCore();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        protected override WebControl CreateViewModeControlCore()
        {
            try
            {
                ASPxHyperLink hyperlink = RenderHelper.CreateASPxHyperLink();
                hyperlink.ID = "hyperlink";
                return hyperlink;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        protected override void ReadEditModeValueCore()
        {
            base.ReadEditModeValueCore();
            if (ASPxEditor is ASPxHyperLink)
            {
                SetupHyperLink((ASPxHyperLink)ASPxEditor);
            }
        }
        protected override void ReadViewModeValueCore()
        {
            base.ReadViewModeValueCore();
            ASPxHyperLink hyperlink = (ASPxHyperLink)InplaceViewModeEditor;
            SetupHyperLink(hyperlink);
        }
        private void SetupHyperLink(ASPxHyperLink hyperlink)
        {
            string url = Convert.ToString(PropertyValue);
            hyperlink.Text = url;
            hyperlink.NavigateUrl = string.Empty;
        }
        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if (ASPxEditor is ASPxTextBox)
            {
                ((ASPxTextBox)ASPxEditor).TextChanged -= EditValueChangedHandler;
            }
            base.BreakLinksToControl(unwireEventsOnly);
        }
        protected override void ApplyReadOnly()
        {
            base.ApplyReadOnly();
            if (ASPxEditor is ASPxHyperLink)
            {
                ASPxEditor.ClientEnabled = true;
            }
        }
        public override bool CanFormatPropertyValue
        {
            get { return false; }
        }
        #endregion
    }
}
