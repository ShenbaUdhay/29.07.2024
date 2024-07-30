using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Pdf;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DxSample.Module.Web.Editors
{
    [PropertyEditor(typeof(byte[]), false)]
    public class PdfPropertyEditor : ASPxPropertyEditor
    {
        public PdfPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
            DocumentProcessor = new PdfDocumentProcessor();
        }

        public ASPxDataView DataView { get; private set; }

        private PdfDocumentProcessor DocumentProcessor { get; set; }


        protected override WebControl CreateViewModeControlCore()
        {
            DataView = new ASPxDataView();
            //DataView.PagerSettings.EnableAdaptivity = true;
            DataView.PagerSettings.EndlessPagingMode = DataViewEndlessPagingMode.OnScroll;
            DataView.SettingsTableLayout.ColumnCount = 1;
            DataView.SettingsTableLayout.RowsPerPage = 1;
            //DataView.PagerSettings.ShowNumericButtons = true;
            //DataView.PagerSettings.AllButton.Visible = true;
            DataView.ItemStyle.Paddings.Padding = new Unit(0, UnitType.Pixel);
            DataView.ItemTemplate = new DocumentItemTemplate(this);
            return DataView;
        }

        protected override WebControl CreateEditModeControlCore()
        {
            return CreateViewModeControlCore();
        }

        protected override void ReadViewModeValueCore()
        {
            try
            {
                byte[] value = (byte[])PropertyValue;
                if (value == null || value.Length == 0)
                {
                    DocumentProcessor.CloseDocument();
                }
                else
                {
                    using (var ms = new MemoryStream(value))
                    {
                        DocumentProcessor.LoadDocument(ms, true);
                    }
                }
            }
            catch (ArgumentException) { }
            finally
            {
                BindDataView();
            }
        }

        private void BindDataView()
        {
            List<PdfPageItem> data = new List<PdfPageItem>();
            if (DocumentProcessor.Document != null)
            {
                for (int pageNumber = 1; pageNumber <= DocumentProcessor.Document.Pages.Count; pageNumber++)
                {
                    data.Add(new PdfPageItem()
                    {
                        PageNumber = pageNumber
                    });
                }
            }
            DataView.DataSource = data;
            DataView.DataBind();
        }

        void image_DataBinding(object sender, EventArgs e)
        {
            ASPxBinaryImage image = sender as ASPxBinaryImage;
            if (DocumentProcessor.Document == null)
            {
                image.ContentBytes = new byte[0];
            }
            else
            {
                DataViewItemTemplateContainer container = image.NamingContainer as DataViewItemTemplateContainer;
                int pageNumber = (int)container.EvalDataItem("PageNumber");

                using (Bitmap bitmap = DocumentProcessor.CreateBitmap(pageNumber, 900))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        image.ContentBytes = stream.ToArray();
                    }
                }
            }
        }

        protected override void ReadEditModeValueCore()
        {
            ReadViewModeValueCore();
        }

        private class PdfPageItem
        {
            public int PageNumber { get; set; }
        }

        private class DocumentItemTemplate : ITemplate
        {
            private PdfPropertyEditor Owner;

            public DocumentItemTemplate(PdfPropertyEditor owner)
            {
                this.Owner = owner;
            }

            #region ITemplate Members

            void ITemplate.InstantiateIn(Control container)
            {
                var image = new ASPxBinaryImage();
                image.DataBinding += Owner.image_DataBinding;
                container.Controls.Add(image);
            }
            #endregion
        }

    }
}
