using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxEditors;

namespace LDM.Module.Web.Editors.QueryPanel
{
    public abstract class SerializedListPropertyEditor<T> : ASPxObjectPropertyEditorBase, IComplexViewItem
    {
        private const string EventHandlerKey = "SLQueryPanel";
        public string ClientInsName = string.Empty;
        public string GridListClientInsName = string.Empty;
        //public static char separator = ',';
        //string SelectionChanged = @"function(s, e){ 
        //var grid= s.GetGridView();
        //var values = s.gridView.GetSelectedKeysOnPage();
        //text="";
        //if (values.length > 0) {
        //alert('length is greater then zero');
        //text = values.join('" + separator + "');} s.Text(text);} ";      
        public class ListBoxItem
        {
            public string DisplayText { get; set; }
            public string Value { get; set; }

        }

        public SerializedListPropertyEditor(Type objectType, IModelMemberViewItem info)
                : base(objectType, info) { }

        protected virtual IEnumerable<T> GetDataSource()
        {
            if (Helper.IsPropertyDataSource)
            {
                var dataSource = Helper.CreateCollectionSource(this.CurrentObject);
                var enumerable = dataSource.Collection as IEnumerable;
                return enumerable.Cast<T>();
            }
            else
            {
                return ObjectSpace.GetObjects<T>(CriteriaOperator.Parse(this.Model.ModelMember.DataSourceCriteria));
            }
        }
        protected abstract void AddGridViewColumns(ASPxGridLookup control);
        protected abstract void getGridDataSource(ASPxGridLookup control);

        public IObjectSpace ObjectSpace { get; private set; }
        public ASPxGridLookup DropDownControl { get; private set; }
        public WebLookupEditorHelper Helper { get; private set; }
        protected override WebControl CreateEditModeControlCore()
        {
            //if (DropDownControl != null)
            //{
            //    IList<T> objtp = (IList<T>)DropDownControl.GridView.DataSource;
            //    return DropDownControl;
            //}         


            DropDownControl = new ASPxGridLookup();
            DropDownControl.ValueChanged += DropDownControl_ValueChanged;
            DropDownControl.EnableClientSideAPI = true;
            DropDownControl.ClientInstanceName = "ListPropertyEditor_" + PropertyName;
            DropDownControl.GridView.ClientInstanceName = "GridListPropertyEditor_" + PropertyName;
            ClientInsName = DropDownControl.ClientInstanceName;
            GridListClientInsName = DropDownControl.GridView.ClientInstanceName;
            DropDownControl.ReadOnly = true;
            //AddGridViewColumns(DropDownControl);
            //AddButtons(DropDownControl);
            ////getGridDataSource(DropDownControl);        
            //AddClientSideEvents(DropDownControl);
            DropDownControl.GridView.CustomCallback += GridView_CustomCallback;
            DropDownControl.Init += control_Init;
            DropDownControl.PreRender += control_PreRender;
            DropDownControl.SelectionMode = GridLookupSelectionMode.Multiple;
            DropDownControl.MultiTextSeparator = ",";
            return DropDownControl;
        }

        private void DropDownControl_ValueChanged(object sender, EventArgs e)
        {

            //throw new NotImplementedException();
        }

        private void control_Init(object sender, EventArgs e)
        {
            //string SelectionChanged = "@function(s, e) { lookup.SetText(s.GetSelectedRowCount()); }";
            //control.ClientSideEvents.ValueChanged = SelectionChanged;         
            //AddClientSideEvents((ASPxGridLookup)sender);
            ASPxGridLookup lookup = (ASPxGridLookup)sender;

            AddGridViewColumns(lookup);
            AddButtons(lookup);
            getGridDataSource(lookup);
            //getGridDataSource(DropDownControl);        
            AddClientSideEvents(lookup);
            //ASPxGridView grid = lookup.GridView;
            // grid.ClientSideEvents.SelectionChanged = SelectionChanged;
            //ClientSideEventsHelper.AssignClientHandlerSafe(lookup.GridView, "SelectionChanged", "SLQueryPanel_JobID", EventHandlerKey);
        }

        private void control_PreRender(object sender, EventArgs e)
        {
            AddClientSideEvents((ASPxGridLookup)sender);
            //ASPxGridLookup lookup = (ASPxGridLookup)sender;
            //ASPxGridView grid = lookup.GridView;
            //grid.SelectionChanged += Grid_SelectionChanged;
            //grid.ClientSideEvents.SelectionChanged =;

        }

        protected override object GetControlValueCore()
        {
            if (ViewEditMode == ViewEditMode.Edit && Editor != null)
            {
                var editor = this.Editor as ASPxGridLookup;

                if (editor.Value != null)
                {
                    return objectSpace.GetObjectByKey(MemberInfo.MemberType, editor.Value);
                }

                return null;
            }

            return MemberInfo.GetValue(CurrentObject);
        }
        private void AddButtons(ASPxGridLookup control)
        {
            EditButton clearButton = new EditButton();
            clearButton.ToolTip = CaptionHelper.GetLocalizedText("DialogButtons", "Clear");
            ASPxImageHelper.SetImageProperties(clearButton.Image, "Editor_Clear");
            control.Buttons.Add(clearButton);

            EditButton addButton = new EditButton();
            addButton.ToolTip = CaptionHelper.GetLocalizedText("DialogButtons", "Add");
            addButton.Text = "Add";
            //ASPxImageHelper.SetImageProperties(addButton.Image, "Editor_Add");
            control.Buttons.Add(addButton);
        }


        private void AddClientSideEvents(ASPxGridLookup control)
        {
            string js = @"function(s,e) { 
                                // clear button
                                if (e.buttonIndex == 0) { 
                                    s.GetGridView().PerformCallback('clear'); 
                                    s.SetText(''); 
                                }
                                // add button
                                else if (e.buttonIndex == 1) { 
                                    // open new window and pass in script to run call back when finished                                     
                                    alert('Values');
                                    s.ConfirmCurrentSelection()
                                    alert('Values1');  
                                    var lst= s.GetGridView().GetSelectedFieldValues(s.GetGridView().KeyFieldName);
                                    alert('lst') ;                                                       
                                s.GetGridView().PerformCallback('Add'); 
                                }
                                // total hack, but after creating a new object we cause a button click and pass in bogus button id (9)
                                else if (e.buttonIndex == 9) {
                                   s.GetGridView().PerformCallback('newObjId=' + window.ddLookupResult); 
                                } 
                                e.processOnServer = false;
                            }";
            //string SelectionChanged = "@function(s, e) { s.SetText(s.GetSelectedRowCount());  e.processOnServer = false; }";
            control.ClientSideEvents.ButtonClick = js;
            //control.ClientSideEvents.ValueChanged = "alert('Value Changed ');";

        }
        void GridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var control = sender as ASPxGridView;
            if (e.Parameters == "clear")
            {
                control.Selection.UnselectAll();
                control.FocusedRowIndex = -1;
                ((ASPxGridLookup)this.Editor).Value = null;
                WriteValue();
            }
            else if (e.Parameters == "Add")
            {
                var lst = control.GetSelectedFieldValues(control.KeyFieldName);
                string str = string.Empty;
                ((ASPxGridLookup)this.Editor).Value = lst.Count();
                WriteValue();
            }
        }
        protected override void ApplyReadOnly()
        {
            if (DropDownControl != null)
            {
                DropDownControl.Enabled = AllowEdit;
            }
        }

        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if (DropDownControl != null)
            {
                DropDownControl.ValueChanged -= ExtendedEditValueChangedHandler;
            }
            base.BreakLinksToControl(unwireEventsOnly);
        }

        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            Helper = new WebLookupEditorHelper(application, objectSpace, MemberInfo.MemberTypeInfo, Model);
            ObjectSpace = objectSpace;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    //if (_ListBoxTemplate != null)
                    //{
                    //    _ListBoxTemplate.Dispose();
                    //    _ListBoxTemplate = null;
                    //}
                    if (DropDownControl != null)
                    {
                        DropDownControl.Dispose();
                        DropDownControl = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }



        //public class SerializedListBoxTemplate : ASPxListBox, ITemplate
        //{
        //    public SerializedListBoxTemplate()
        //    {
        //        SelectionMode = ListEditSelectionMode.CheckColumn;
        //        EnableClientSideAPI = true;
        //        Width = Unit.Percentage(100.0);
        //        Height = 300;
        //    }

        //    private string _DropDownId;
        //    private char _SeparatorChar = ',';

        //    public void InstantiateIn(Control container)
        //    {
        //        InitClientSideEvents();
        //        container.Controls.Add(this);
        //    }

        //    private void InitClientSideEvents()
        //    {
        //        ClientSideEvents.Init =
        //            @"function (s, args) {
        //                var listBox = ASPxClientControl.Cast(s);
        //                listBox.autoResizeWithContainer = true;
        //            }";

        //        ClientSideEvents.ValueChanged =
        //            @"function (s, args) {
        //                var listBox = ASPxClientControl.Cast(s);
        //                var checkComboBox = ASPxClientControl.Cast(" + _DropDownId + @");
        //                var selectedItems = listBox.GetSelectedItems();
        //                var values = [];
        //                for(var i = 0; i < selectedItems.length; i++)
        //                    values.push(selectedItems[i].value);
        //                checkComboBox.SetText(values.join('" + _SeparatorChar + @"'));
        //        //    }";
        //    }

        //    public void SetDropDownId(string id)
        //    {
        //        _DropDownId = id;
        //    }

        //    public void SetSeparatorChar(char separatorChar)
        //    {
        //        _SeparatorChar = separatorChar;
        //    }

        //    public void SetValue(string value)
        //    {
        //        foreach (ListEditItem item in Items)
        //        {
        //            item.Selected = value.Contains(item.Value.ToString());
        //        }
        //    }
        //}
    }
}

