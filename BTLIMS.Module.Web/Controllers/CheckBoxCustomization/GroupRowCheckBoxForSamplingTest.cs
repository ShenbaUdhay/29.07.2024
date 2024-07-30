using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SamplingManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LDM.Module.Web.Controllers.CheckBoxCustomization
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GroupRowCheckBoxForSamplingParameter : ViewController<ListView>
    {
        #region Declaration
        public const string keyFieldName = "Oid";
        private System.ComponentModel.IContainer components;
        public const string gridId = "SamplingParameter_ListView_SamplingProposal";
        MessageTimer timer = new MessageTimer();
        #endregion

        #region Constructor
        public GroupRowCheckBoxForSamplingParameter()
        {
            InitializeComponent();
            TargetViewId = "SamplingParameter_ListView_SamplingProposal;";
            TargetViewNesting = Nesting.Any;
            TargetViewType = ViewType.ListView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        #endregion
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (Grid != null)
            {
                // Add checkboxes to group rows
                Grid.ClientInstanceName = gridId;
                Grid.Templates.GroupRowContent = new CheckboxGroupRowContentTemplate(gridId);
                Grid.CustomCallback += Grid_CustomCallback;
                Grid.HtmlRowPrepared += Grid_HtmlRowPrepared;
            }
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        #region Functions
        private ASPxGridView Grid
        {
            get
            {
                ASPxGridListEditor editor = View != null ? (View as ListView).Editor as ASPxGridListEditor : null;
                if (editor != null)
                    return editor.Grid;
                return null;
            }
        }
        #endregion
        #region Events
        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string[] parameters = e.Parameters.Split(';');

                int index = int.Parse(parameters[0]);
                string fieldname = parameters[1];
                bool isGroupRowSelected = bool.Parse(parameters[2]);
                ReadOnlyCollection<GridViewDataColumn> groupedCols = Grid.GetGroupedColumns();
                if (groupedCols[groupedCols.Count - 1].FieldName == fieldname)
                {
                    // Checked groupcolumn is the lowest level groupcolumn;
                    // we can apply original recursive checking here
                    Grid.ExpandRow(index, true); // Expand grouped column for consistent behaviour
                    for (int childIndex = 0; childIndex < Grid.GetChildRowCount(index); childIndex++)
                    {
                        var key = (Grid.GetChildRow(index, childIndex) as SamplingParameter).Oid;
                        Grid.Selection.SetSelectionByKey(key, isGroupRowSelected);
                    }
                }
                else
                {
                    // checked row is not the lowest level groupcolumn:
                    // we will find the Datarows that are to be checked recursively by iterating all rows
                    // and compare the fieldvalues of the fields described by the checked groupcolumn
                    // and all its parent groupcolumns. Rows that match these criteria are to the checked.
                    // CAVEAT: only expanded rows can be iterated, so we will have to expand clicked row recursivly before iterating the grid

                    // Get index of current grouped column
                    int checkedGroupedColumnIndex = -1;
                    foreach (GridViewDataColumn gcol in groupedCols)
                    {
                        if (gcol.FieldName == fieldname)
                        {
                            checkedGroupedColumnIndex = groupedCols.IndexOf(gcol);
                            break;
                        }
                    }
                    //Build dictionary with checked groupcolumn and its parent groupcolumn fieldname and values
                    SamplingParameter checkedDataRow = Grid.GetRow(index) as SamplingParameter;
                    Dictionary<string, object> dictParentFieldNamesValues = new Dictionary<string, object>();
                    string parentFieldName;
                    object parentKeyValue;
                    for (int i = checkedGroupedColumnIndex; i >= 0; i--)
                    {
                        // find parent groupcols and parentkeyvalue
                        GridViewDataColumn pcol = groupedCols[i];
                        parentFieldName = pcol.FieldName;
                        parentKeyValue = checkedDataRow.Parent;
                        dictParentFieldNamesValues.Add(parentFieldName, parentKeyValue);
                    }

                    bool isChildDataRowOfClickedGroup;
                    Grid.ExpandRow(index, true); // Expand grouped column for consistent behaviour
                    for (int i = 0; i <= Grid.VisibleRowCount - 1; i++)
                    {
                        SamplingParameter row = Grid.GetRow(i) as SamplingParameter;
                        // Check whether row does belong to checked group all the parent groups of the clicked group
                        isChildDataRowOfClickedGroup = true;
                        foreach (KeyValuePair<string, object> kvp in dictParentFieldNamesValues)
                        {
                            parentFieldName = kvp.Key;
                            parentKeyValue = kvp.Value;
                            if (row.Parent.Equals(parentKeyValue) == false)
                            {
                                isChildDataRowOfClickedGroup = false;
                                break;
                                //Iterated row does not belong to at least one parentgroup of the clicked groupbox; do not change selection state for this row
                            }
                        }
                        if (isChildDataRowOfClickedGroup == true)
                        {
                            // Row meets all the criteria for belonging to the clicked group and all parents of the clicked group:
                            // change selection state
                            Grid.Selection.SetSelectionByKey(row.Oid, isGroupRowSelected);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        void Grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (e.RowType == GridViewRowType.Group)
                {
                    ASPxCheckBox checkBox = Grid.FindGroupRowTemplateControl(e.VisibleIndex, "checkBox") as ASPxCheckBox;
                    if (checkBox != null)
                    {
                        checkBox.Checked = GetChecked(e.VisibleIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected bool GetChecked(int visibleIndex)
        {
            try
            {
                for (int childIndex = 0; childIndex < Grid.GetChildRowCount(visibleIndex); childIndex++)
                {
                    //var key = Grid.GetChildRowValues(visibleIndex, childIndex, keyFieldName);
                    var key = (Grid.GetChildRow(visibleIndex, childIndex) as SamplingParameter).Oid;
                    bool isRowSelected = Grid.Selection.IsRowSelectedByKey(key);
                    if (!isRowSelected)
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }

        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // GroupRowCheckBoxForTestParameter

        }
    }
    #endregion

}

