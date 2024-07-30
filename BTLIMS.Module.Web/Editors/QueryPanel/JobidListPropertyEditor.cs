using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Web;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;

namespace LDM.Module.Web.Editors.QueryPanel
{
    [PropertyEditor(typeof(string), false)]
    public class JobidListPropertyEditor : SerializedListPropertyEditor<Samplecheckin>
    {
        //string SelectionChanged = "@function(s, e){ s.SetText(GetSelectedFieldValues(s.KeyFieldName)); }";
        public JobidListPropertyEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info) { }

        //protected override string GetDisplayText(Samplecheckin sc)
        //{
        //    return string.Format("{0}", sc.JobID);
        //}

        //protected override string GetValue(Samplecheckin sc)
        //{
        //    return string.Format("{0}",sc.JobID);
        //}
        protected override void getGridDataSource(DevExpress.Web.ASPxGridLookup control)
        {
            IList<Samplecheckin> sclst = ObjectSpace.GetObjects<Samplecheckin>();
            control.DataSource = sclst;
            control.DataBind();
        }
        protected override void AddGridViewColumns(DevExpress.Web.ASPxGridLookup control)
        {
            control.KeyFieldName = "JobID";
            control.GridView.KeyFieldName = "JobID";
            GridViewCommandColumn cmdcolumn = new GridViewCommandColumn();
            cmdcolumn.ShowSelectCheckbox = true;
            cmdcolumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
            control.Columns.Add(cmdcolumn);

            GridViewDataTextColumn JobID = new GridViewDataTextColumn();
            JobID.Caption = "JobID";
            JobID.FieldName = "JobID";
            JobID.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            control.Columns.Add(JobID);

            GridViewDataTextColumn ClientName = new GridViewDataTextColumn();
            ClientName.Caption = "ClientName";
            ClientName.FieldName = "ClientName.CustomerName";
            control.Columns.Add(ClientName);
            //DropDownControl.SelectionMode = GridLookupSelectionMode.Multiple;
            //control.ClientSideEvents.ValueChanged = SelectionChanged;

            //DropDownControl.Text = "Chai, Chang, Ikura";

        }
        //private IList<Samplecheckin>getDataSource(IList<Samplecheckin> sc)
        //{
        //    return sc.Select(s => s.JobID)
        //}
    }
}
