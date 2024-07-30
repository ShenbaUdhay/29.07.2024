using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors.QueryPanel
{
    [PropertyEditor(typeof(Samplecheckin), false)]
    public class JobIDLookupListViewPropertyEditor : ASPxPropertyEditor, IComplexViewItem
    {
        private Dictionary<ASPxGridLookup, string> controlsHash = new Dictionary<ASPxGridLookup, string>();
        public IObjectSpace ObjectSpace { get; private set; }
        public WebLookupEditorHelper Helper { get; private set; }

        private ASPxGridLookup lookup = null;
        private WebLookupEditorHelper _helper;

        private EditButton newButton;
        private EditButton editButton;
        private EditButton clearButton;

        //private PopupWindowShowAction editObjectAction;
        //private PopupWindowShowAction newObjectAction;

        private IObjectSpace _objectSpace;
        private WebApplication _application;

        private bool needRaiseValueChanged = false;
        private object storedChangedValue;
        public JobIDLookupListViewPropertyEditor(Type objectType, IModelMemberViewItem info)
                : base(objectType, info) { }

        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            //_captionService = new Services.Captions.Base.CaptionService(((XPObjectSpace)objectSpace).Session);
            this._application = WebApplication.Instance;
            if (objectSpace == null)
            {
                this._objectSpace = application.CreateObjectSpace();
            }
            else
            {
                this._objectSpace = objectSpace;
            }
            //_session = ((XPObjectSpace)this._objectSpace).Session;
            _helper = new WebLookupEditorHelper(application, objectSpace, MemberInfo.MemberTypeInfo, Model);
        }
        public ASPxGridLookup DropDownControl { get; private set; }
        protected override WebControl CreateEditModeControlCore()
        {
            if (lookup != null && lookup.Text != string.Empty)
            {
                return lookup;
            }
            if (lookup != null && lookup.GridView.IsCallback == true)
            {
                return lookup;
            }
            if (lookup != null && lookup.DataSource != null && lookup.GridView.Selection.Count > 0)
            {
                return lookup;
            }

            //if (lookup != null && lookup.Text != string.Empty) return lookup;


            controlsHash.Clear();
            //if (((ASPxGridLookup)GridView.IsCallback)
            //if (lookup !=null)
            //{
            //    if (lookup.GridView.IsCallback) return lookup;
            //}          
            lookup = new ASPxGridLookup();
            lookup.ValueChanged += Lookup_ValueChanged;
            lookup.Load += Lookup_Load;
            lookup.GridView.Init += GridView_Init;

            lookup.ID = "glJobID";
            lookup.EnableClientSideAPI = true;
            lookup.ClientInstanceName = "glJobID";
            lookup.ReadOnly = true;
            AddGridViewColumns(lookup);
            AddButtons(lookup);
            AddClientSideEvents(lookup);
            //getGridDataSource(lookup);
            //AddClientSideEvents(DropDownControl);          
            lookup.SelectionMode = GridLookupSelectionMode.Multiple;
            lookup.TextFormatString = "{0}";
            lookup.MultiTextSeparator = ",";
            //DropDownControl.KeyFieldName = "JobID";

            //lookup.GridView.SettingsPager.PageSize = Model.Application.Options.LookupSmallCollectionItemCount;
            //lookup.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            lookup.AutoPostBack = false;
            lookup.GridView.AutoGenerateColumns = false;
            lookup.GridView.Settings.ShowFilterRow = true;


            lookup.GridView.SettingsPager.EnableAdaptivity = true;
            lookup.KeyFieldName = MemberInfo.MemberTypeInfo.KeyMember.Name;
            lookup.ClientSideEvents.GotFocus = "function(s,e){ s.SelectAll(); e.handled = true; e.processOnServer = false; }";
            //DropDownControl.Init += control_Init;
            lookup.GridView.CustomCallback += GridView_CustomCallback;
            controlsHash.Add(lookup, lookup.ClientInstanceName);
            //DropDownControl.DataBound += GridView_DataBound;
            //DropDownControl.GridView.SelectionChanged += GridView_SelectionChanged;
            //controlsHash.Add(DropDownControl, DropDownControl.ClientInstanceName);

            //string viewName = $"{this.MemberInfo.MemberType.Name}_lookuplistview";

            //string viewName = $"{this.MemberInfo.MemberType.Name}_LookupListView_Copy_SLQueryPanel";
            //if (View != null)
            //{
            //    var modelView = (IModelListView)View.Model.Application.Views.FirstOrDefault(x => x.Id.ToLower().Equals(viewName.ToLower()));
            //    GridViewCommandColumn cmdcolumn = new GridViewCommandColumn();
            //    cmdcolumn.ShowSelectCheckbox = true;
            //    cmdcolumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
            //    lookup.GridView.Columns.Add(cmdcolumn);
            //    foreach (var column in modelView.Columns)
            //    {
            //        string fullPropName = column.PropertyName;
            //        if (!string.IsNullOrWhiteSpace(column.LookupProperty))
            //        {
            //            fullPropName += $".{column.LookupProperty}";
            //        }
            //        GridViewColumn col = new GridViewDataColumn(fullPropName)
            //        {
            //            Caption = column.Caption
            //        };
            //        lookup.GridView.Columns.Add(col);
            //    }


            //    lookup.GridView.KeyFieldName = MemberInfo.MemberTypeInfo.KeyMember.Name;
            //}


            ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager.PreRender += CallbackManager_PreRender;
            // return lookup;

            return lookup;
        }

        private void GridView_PageIndexChanged(object sender, EventArgs e)
        {

        }

        private void CallbackManager_PreRender(object sender, EventArgs e)
        {
            if (needRaiseValueChanged)
            {
                EditValueChangedHandler(this, new EventArgs());
                needRaiseValueChanged = false;
            }
        }
        private void Lookup_ValueChanged(object sender, EventArgs e)
        {
            if (((ASPxGridLookup)sender).GridView.IsCallback)
            {
                needRaiseValueChanged = true;
                storedChangedValue = View.ObjectSpace.GetObjectByKey(MemberInfo.MemberType, lookup.Value);
            }
        }
        private void GridView_Init(object sender, EventArgs e)
        {
            lookup.GridView.Init -= GridView_Init;
            //Todo: Memory overflow sometimes when clicking on record in StockMutation listview (serveral times)
            lookup.GridView.DataBind();
            object lookupValue = null;
            if (needRaiseValueChanged)
            {
                lookupValue = storedChangedValue;
            }
            else
            {
                lookupValue = PropertyValue;
            }
            _objectSpace.ReloadObject(PropertyValue);
            lookup.Value = lookupValue != null ? _objectSpace.GetKeyValue(lookupValue) : null;
        }
        private void Lookup_Load(object sender, EventArgs e)
        {
            AddClientSideEvents((ASPxGridLookup)sender);
            // ASPxButtonEditBase buttons = (ASPxButtonEditBase)sender;
            // ButtonEditClientSideEventsBase clientSideEvents = (ButtonEditClientSideEventsBase)buttons.GetClientSideEvents();
            // //string showEditModalScript = _application.PopupWindowManager.GetShowPopupWindowScript(editObjectAction, null, buttons.ClientID, false, true, false, false);
            // //string showNewModalScript = _application.PopupWindowManager.GetShowPopupWindowScript(newObjectAction, null, buttons.ClientID, false, newObjectAction.IsSizeable, false, false, "function() { window.buttonEditAlreadyClicked = false; window.canInitiateImmediatePostData = true;}");
            // string clearButtonScript = $"var processOnServer = false;var lookupControl = ASPx.GetControlCollection().Get('{buttons.ClientID}'); if (lookupControl.GetValue() != null) {{ lookupControl.SetText(''); processOnServer = lookupControl.RaiseValueChangedEvent(); }} e.processOnServer = processOnServer; ";
            // //string editButtonScript = $"var lookupControl = ASPx.GetControlCollection().Get('{buttons.ClientID}'); if(lookupControl.GetValue() != null) {{ {showEditModalScript} e.handled = true; e.processOnServer = false; }}";
            // //string newButtonScript = "window.canInitiateImmediatePostData = false; e.handled = true; e.processOnServer = false; " + showNewModalScript;
            // //string script = $"function(s,e) {{ if(e.buttonIndex == 0) {{ if(!window.buttonEditAlreadyClicked) {{ window.buttonEditAlreadyClicked = true; {newButtonScript} }} }} if(e.buttonIndex == 1) {{ {clearButtonScript} }} if(e.buttonIndex == 2) {{ {editButtonScript} }} }} ";
            // //clientSideEvents.ButtonClick = script;

            //string script = $"function(s,e) {{if(e.buttonIndex == 0) {{ {clearButtonScript} }} }} ";


            // clientSideEvents.ButtonClick = script;

        }
        protected override void ReadEditModeValueCore()
        {
            //this._session = ((XPObjectSpace)WebApplication.Instance.CreateObjectSpace()).Session;
            //var dataSource = new XPCollection(nwObjSpace.Session, GetUnderlyingType());

            //Notes
            //CollectionSource cannot be used here. GridView.DataSource Expects IList, IEnumerable
            //XPServerCollectionSource : can be used but has issues with filtering based on values in currentobject           
            XPCollection dataSource = new XPCollection(((XPObjectSpace)this._objectSpace).Session, GetUnderlyingType());
            if (!string.IsNullOrWhiteSpace(Model.DataSourceCriteria))
            {
                if (!Model.DataSourceCriteria.Contains("@"))
                {
                    dataSource.CriteriaString = Model.DataSourceCriteria;
                }
                else
                {
                    //throw exception
                }
            }
            if (!string.IsNullOrWhiteSpace(Model.DataSourceCriteriaProperty))
            {
                dataSource.Criteria = (CriteriaOperator)this.CurrentObject.GetType().GetProperty(Model.DataSourceCriteriaProperty).GetValue(this.CurrentObject);
            }
            lookup.GridView.Selection.BeginSelection();
            lookup.GridView.Selection.UnselectAll();
            lookup.GridView.DataSource = dataSource;
            lookup.GridView.Selection.EndSelection();
        }
        protected override object GetControlValueCore()
        {
            object result = null;
            if (lookup != null && lookup.Value != null && needRaiseValueChanged)
            {
                result = storedChangedValue;
            }
            return result;
        }
        //protected override void WriteValueCore()
        //{
        //    if (lookup != null)
        //    {
        //        IList<string> value = (IList<string>)PropertyValue;
        //        value.Clear();
        //        foreach (var v in lookup.GridView.GetSelectedFieldValues(MemberInfo.MemberTypeInfo.KeyMember.Name))
        //        {
        //            value.Add((string)v);
        //        }
        //    }
        //}

        protected override void WriteValueCore()
        {
            base.WriteValueCore();
        }
        protected override void ReadViewModeValueCore()
        {
            base.ReadViewModeValueCore();
        }
        //protected override void ReadViewModeValueCore()
        //{
        //    Label viewModeControl = InplaceViewModeEditor as Label;
        //    if (viewModeControl != null)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        IList<string> value = (IList<string>)PropertyValue;
        //        foreach (var v in value)
        //        {
        //            if (sb.Length > 0) sb.Append(", ");
        //            sb.Append(_objectSpace.GetObjects<Samplecheckin>().ToList().FirstOrDefault(x => x.JobID == v).JobID);
        //        }
        //        viewModeControl.Text = sb.ToString();
        //    }
        //}
        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if (lookup != null)
            {
                lookup.ValueChanged -= EditValueChangedHandler;
                lookup.Load -= Lookup_Load;
            }
            //if (unwireEventsOnly == false)
            //{
            //    if (lookup != null && lookup.GridView != null)
            //        lookup.GridView.DataSource = null;
            //}
            base.BreakLinksToControl(unwireEventsOnly);
        }

        private bool CanCreate()
        {
            return true;
        }

        private bool CanEdit()
        {
            return Model.AllowEdit;
        }

        private bool CanClear()
        {
            return Model.AllowClear;
        }

        private void SetButtonVisibility(EditButton button, bool visibility)
        {
            button.Visible = visibility;
        }

        private void AddClientSideEvents(ASPxGridLookup control)
        {
            string js = @"function(s,e) { 
                                // clear button
                                if (e.buttonIndex == 0) { 
                                    s.GetGridView().PerformCallback('clear'); 
                                    s.SetText(''); 
                                }                               
                                e.processOnServer = false;
                            }";

            control.ClientSideEvents.ButtonClick = js;
        }
        protected void AddGridViewColumns(DevExpress.Web.ASPxGridLookup lookup)
        {
            string viewName = $"{this.MemberInfo.MemberType.Name}_LookupListView_Copy_SLQueryPanel";
            if (View != null)
            {
                var modelView = (IModelListView)View.Model.Application.Views.FirstOrDefault(x => x.Id.ToLower().Equals(viewName.ToLower()));
                GridViewCommandColumn cmdcolumn = new GridViewCommandColumn();
                cmdcolumn.ShowSelectCheckbox = true;
                cmdcolumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
                lookup.GridView.Columns.Add(cmdcolumn);
                foreach (var column in modelView.Columns)
                {
                    string fullPropName = column.PropertyName;
                    if (!string.IsNullOrWhiteSpace(column.LookupProperty))
                    {
                        fullPropName += $".{column.LookupProperty}";
                    }
                    GridViewColumn col = new GridViewDataColumn(fullPropName)
                    {
                        Caption = column.Caption
                    };
                    lookup.GridView.Columns.Add(col);
                }


                lookup.GridView.KeyFieldName = MemberInfo.MemberTypeInfo.KeyMember.Name;
            }

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
        //protected override object GetControlValueCore()
        //{
        //    if (ViewEditMode == ViewEditMode.Edit && Editor != null)
        //    {
        //        var editor = this.Editor as ASPxGridLookup;

        //        if (editor.Value != null)
        //        {
        //            return ObjectSpace.GetObjectByKey(MemberInfo.MemberType, editor.Value);
        //        }

        //        return null;
        //    }

        //    return MemberInfo.GetValue(CurrentObject);
        //}
        private void AddButtons(ASPxGridLookup control)
        {
            clearButton = new EditButton();
            ASPxImageHelper.SetImageProperties(clearButton.Image, "Action_Clear_12x12");
            SetButtonVisibility(clearButton, CanClear());
            lookup.Buttons.Add(clearButton);

            //EditButton clearButton = new EditButton();
            //clearButton.ToolTip = CaptionHelper.GetLocalizedText("DialogButtons", "Clear");
            //ASPxImageHelper.SetImageProperties(clearButton.Image, "Editor_Clear");
            //control.Buttons.Add(clearButton);

            //EditButton addButton = new EditButton();
            //addButton.ToolTip = CaptionHelper.GetLocalizedText("DialogButtons", "Add");
            //addButton.Text = "Add";
            ////ASPxImageHelper.SetImageProperties(addButton.Image, "Editor_Add");
            //control.Buttons.Add(addButton);
            //control.ButtonClick += Control_ButtonClick;
        }

        private void Control_ButtonClick(object source, ButtonEditClickEventArgs e)
        {

        }
        //private void AddClientSideEvents(ASPxGridLookup control)
        //{
        //    string js = @"function(s,e) { 
        //                        // clear button
        //                        if (e.buttonIndex == 0) { 
        //                            s.GetGridView().PerformCallback('clear'); 
        //                            s.SetText(''); 
        //                        }
        //                        // add button
        //                        else if (e.buttonIndex == 1) { 
        //                            // open new window and pass in script to run call back when finished                                     
        //                            alert('Values');
        //                            s.ConfirmCurrentSelection()
        //                            alert('Values1');  
        //                           var lst= glJobID.GetGridView().GetSelectedFieldValues('Oid'); 
        //                            alert(lst);                 
        //                            glJobID.GetGridView().PerformCallback('Add'); 
        //                        }
        //                        // total hack, but after creating a new object we cause a button click and pass in bogus button id (9)
        //                        else if (e.buttonIndex == 9) {
        //                           s.GetGridView().PerformCallback('newObjId=' + window.ddLookupResult); 
        //                        } 
        //                        e.processOnServer = false;
        //                    }";

        //    //string jss = @"function(s,e) {                              

        //    //                        s.ConfirmCurrentSelection()
        //    //                        alert('Values1');  
        //    //                       var lst= glJobID.GetGridView().GetSelectedFieldValues('JobID');                                            
        //    //                        glJobID.GetGridView().PerformCallback('Add'); 
        //    //                    e.processOnServer = false;
        //    //                }";
        //    //string SelectionChanged = "@function(s, e) { s.SetText(s.GetSelectedRowCount());  e.processOnServer = false; }";

        //    control.ClientSideEvents.ButtonClick = js;
        //    //control.GridView.ClientSideEvents.SelectionChanged= jss;
        //    //control.ClientSideEvents.ValueChanged = "alert('Value Changed ');";

        //}
        //protected override void SetImmediatePostDataScript(string script)
        //{
        //    foreach (ASPxGridLookup gridlookup in controlsHash.Keys)
        //    {
        //        gridlookup.ClientSideEvents.ValueChanged = script;
        //    }
        //    // base.SetImmediatePostDataScript(script);
        //}

        //protected override void ApplyReadOnly()
        //{
        //    if (DropDownControl != null)
        //    {
        //        DropDownControl.Enabled = AllowEdit;
        //    }
        //}

        //public override void BreakLinksToControl(bool unwireEventsOnly)
        //{
        //    if (DropDownControl != null)
        //    {
        //        //DropDownControl.ValueChanged -= ExtendedEditValueChangedHandler;
        //    }
        //    base.BreakLinksToControl(unwireEventsOnly);
        //}
        //protected override void Dispose(bool disposing)
        //{
        //    try
        //    {
        //        if (disposing)
        //        {
        //            if (DropDownControl != null)
        //            {
        //                DropDownControl.Dispose();
        //                DropDownControl = null;
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        base.Dispose(disposing);
        //    }
        //}
    }
}
