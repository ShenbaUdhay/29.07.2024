using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Office.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Web.ASPxRichEdit;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace LDM.Module.Controllers.SampleRegistration
{
    public partial class AuditViewController : ViewController
    {
        CharacterProperties cp;
        Color gcolor = (Color)new ColorConverter().ConvertFromString("#009900");
        public AuditViewController()
        {
            InitializeComponent();
            TargetViewId = "Samplecheckin_DetailView_Copy_SampleRegistration;" + "AuditControl_DetailView;";
            Audit.TargetViewId = "Samplecheckin_DetailView_Copy_SampleRegistration";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.Id == "AuditControl_DetailView")
            {
                ASPxRichTextPropertyEditor RichText = ((DetailView)View).FindItem("Text") as ASPxRichTextPropertyEditor;
                if (RichText != null)
                {
                    RichText.ControlCreated += RichText_ControlCreated;
                }
            }
        }

        void RichText_ControlCreated(object sender, System.EventArgs e)
        {
            ASPxRichEdit RichEdit = ((ASPxRichTextPropertyEditor)sender).ASPxRichEditControl;
            if (RichEdit != null)
            {
                RichEdit.RibbonMode = RichEditRibbonMode.None;
                RichEdit.ClientSideEvents.Init = "function(s, e) { s.SetFullscreenMode(true); } ";
                RichEdit.ClientSideEvents.SelectionChanged = "function(s, e) { s.selection.intervals = [{ start: 0, length: 0 }]; }";
                AuditControl audit = (AuditControl)View.CurrentObject;
                if (audit != null)
                {
                    createdoc(audit);
                }
            }
        }

        void createdoc(AuditControl audit)
        {
            using (RichEditDocumentServer wordProcessor = new RichEditDocumentServer())
            {
                using (IObjectSpace os = Application.CreateObjectSpace())
                {
                    IList<AuditData> list = os.GetObjects<AuditData>(CriteriaOperator.Parse("[Source]=?", audit.ID));
                    if (list.Count > 0)
                    {
                        foreach (AuditData data in list.OrderByDescending(a => a.CreatedDate))
                        {
                            createtable(wordProcessor.Document, data);
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                            wordProcessor.SaveDocument(ms, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
                            ms.Position = 0;
                            if (ms.Length > 0)
                            {
                                audit.Text = ms.ToArray();
                            }
                        }
                    }
                }
            }
        }

        void createtable(Document doc, AuditData data)
        {
            Table table = doc.Tables.Create(doc.Range.End, 7, 1, AutoFitBehaviorType.AutoFitToWindow);
            table.BeginUpdate();
            SetTableProperties(table);
            InsertHeaderData(doc, table, data);
            InsertTableData(doc, table, data);
            table.EndUpdate();
            doc.Paragraphs.Append();
        }

        void SetTableProperties(Table table)
        {
            table.TopPadding = table.BottomPadding = 10.0F;
            var borders = table.Borders;
            borders.Top.LineColor = borders.Bottom.LineColor = borders.Left.LineColor = borders.Right.LineColor = gcolor;
            table.HorizontalAlignment = TableHorizontalAlignment.Center;
        }

        void InsertHeaderData(Document doc, Table table, AuditData data)
        {
            var range = doc.InsertText(table[0, 0].Range.Start, data.FormName);
            tablestyle("green", doc, range, false, true);
            var range1 = doc.InsertText(range.End, " - " + data.OperationType.ToString());
            tablestyle("green", doc, range1, false, true);
            table[0, 0].Borders.Bottom.LineStyle = TableBorderLineStyle.None;
            var range2 = doc.InsertText(table[1, 0].Range.Start, "by ");
            tablestyle("green", doc, range2);
            var range3 = doc.InsertText(range2.End, data.CreatedBy.DisplayName.Trim());
            tablestyle("blue", doc, range3);
            var range4 = doc.InsertText(range3.End, " on " + data.CreatedDate.ToString("yyyy-MM-dd HH:mm") + ", " + (DateTime.Now.Date - data.CreatedDate.Date).TotalDays.ToString() + " days ago");
            tablestyle("green", doc, range4);
            table[1, 0].Borders.Bottom.LineStyle = TableBorderLineStyle.None;
        }

        void InsertTableData(Document doc, Table table, AuditData data)
        {
            inserttabledata(doc, table[2, 0], "ID", data.ID);
            inserttabledata(doc, table[3, 0], "Property Name", data.PropertyName);
            inserttabledata(doc, table[4, 0], "Old Value", data.Oldvalue);
            inserttabledata(doc, table[5, 0], "New Value", data.Newvalue);
            inserttabledata(doc, table[6, 0], "Comment", data.Comment, true);
        }

        void inserttabledata(Document doc, TableCell table, string Fieldname, string Value, bool showborder = false)
        {
            var range = doc.InsertText(table.Range.Start, Fieldname);
            tablestyle("", doc, range, true);
            var range1 = doc.InsertText(range.End, " : " + Value);
            tablestyle("", doc, range1);
            table.Borders.Bottom.LineStyle = showborder ? TableBorderLineStyle.Single : TableBorderLineStyle.None;
        }

        void tablestyle(string type, Document doc, DocumentRange range, bool underline = false, bool Size = false)
        {
            cp = doc.BeginUpdateCharacters(range);
            cp.Bold = Size;
            cp.ForeColor = type == "green" ? gcolor : type == "blue" ? Color.Blue : Color.Black;
            cp.Underline = underline ? UnderlineType.Single : UnderlineType.None;
            doc.EndUpdateCharacters(cp);
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (View.Id == "AuditControl_DetailView")
            {
                ASPxRichTextPropertyEditor RichText = ((DetailView)View).FindItem("Text") as ASPxRichTextPropertyEditor;
                if (RichText != null)
                {
                    RichText.ControlCreated -= RichText_ControlCreated;
                }
            }
        }

        private void Audit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            NonPersistentObjectSpace os = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(AuditControl));
            if (os != null)
            {
                AuditControl obj = os.CreateObject<AuditControl>();
                if (obj != null)
                {
                    obj.ID = ((Samplecheckin)View.CurrentObject).Oid;
                    DetailView createdView = Application.CreateDetailView(os, "AuditControl_DetailView", true, obj);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    createdView.Caption = "Audit Trail";
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.AcceptAction.Active.SetItemValue("AcceptAction", false);
                    dc.CancelAction.Active.SetItemValue("CancelAction", false);
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
            }
        }
    }
}
