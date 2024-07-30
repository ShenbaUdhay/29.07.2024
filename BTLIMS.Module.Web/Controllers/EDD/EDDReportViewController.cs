using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.EDD;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Method = Modules.BusinessObjects.Setting.Method;

namespace LDM.Module.Web.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class EDDReportViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        public EDDReportViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "EDDReportGenerator_DetailView_popup;";
            ClearEDD.TargetViewId = "EDDReportGenerator_DetailView_popup;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            try
            {
                if (View.Id == "EDDReportGenerator_DetailView_popup")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View.Id == "EDDReportGenerator_DetailView_popup")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    EDDBuilder objED = os.CreateObject<EDDBuilder>();
                    EDDReportGenerator objERG = View.CurrentObject as EDDReportGenerator;
                    if (objERG.EddTemplate != null)
                    {
                        EDDBuilder objed = View.ObjectSpace.GetObject(objERG.EddTemplate);
                        string strQuery = string.Empty;
                        List<string> lstFillName = new List<string>();

                        if (e.PropertyName == "EddTemplate")
                        {
                            if(e.OldValue!=e.NewValue)
                            {
                                objERG.ALLEDDTemplateJobID = null;
                                objERG.ALLTemplateEDDClient = null;
                                objERG.ALLTemplateEDDMethod = null;
                                objERG.ALLTemplateEDDProjectID = null;
                                objERG.ALLTemplateEDDProjectName = null;
                                objERG.ALLTemplateEDDSampleCategory = null;
                                objERG.JobID = null;
                                objERG.Client = null;
                                objERG.ProjectID = null;
                                objERG.ProjectName = null;
                                objERG.Test = null;
                                objERG.Method = null;
                                objERG.SampleCategory = null;
                                objERG.DateCollectedFrom = DateTime.MinValue;
                                objERG.DateCollectedTo = DateTime.MinValue;
                                objERG.DateReceivedFrom = DateTime.MinValue;
                                objERG.DateReceivedTo = DateTime.MinValue;
                            }
                            lstFillName.Add("JobID");
                            lstFillName.Add("ClientName");
                            lstFillName.Add("ProjectID");
                            lstFillName.Add("ProjectName");
                            lstFillName.Add("SampleCategory");
                            lstFillName.Add("Test");
                            lstFillName.Add("Method");
                            lstFillName.Add("DateReceived");
                            lstFillName.Add("DateCollected");
                            List<Guid> lststringeddbuilder = objERG.EddTemplate.EDDQueryBuilders.Cast<EDDQueryBuilder>().Select(i => i.Oid).ToList();
                            List<EDDFieldEditor> lstEDDFE = ObjectSpace.GetObjects<EDDFieldEditor>(new GroupOperator(GroupOperatorType.And, new InOperator("FieldName", lstFillName), new InOperator("EDDQueryBuilder.Oid", lststringeddbuilder))).ToList(); ///*, (CriteriaOperator.Parse("[EddBuilder]=?", objERG.EddTemplate))*/)).ToList();
                            foreach (string FillName in lstEDDFE.Select(i => i.FieldName))
                            {
                                DataSet ds = new DataSet();
                                ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                SetConnectionString();
                                if (objed.EDDQueryBuilders != null && objed.EDDQueryBuilders.Count > 0)
                                {
                                    using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn))
                                    {
                                        var paramjobid = string.Empty;
                                        foreach (EDDQueryBuilder objquerybuilder in objed.EDDQueryBuilders.OrderBy(i=>i.QueryName).ToList())
                                        {
                                            DataTable dt = new DataTable();
                                            using (SqlCommand cmd = new SqlCommand("EDD_RG_FillComboByFilter_SP", con))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                SqlParameter[] param = new SqlParameter[13];
                                                param[0] = new SqlParameter("@Query", objquerybuilder.QueryBuilder);
                                                param[1] = new SqlParameter("@FillName", FillName);
                                                param[2] = new SqlParameter("@JobID", DBNull.Value);
                                                param[3] = new SqlParameter("@ClientName", DBNull.Value);
                                                param[4] = new SqlParameter("@ProjectID", DBNull.Value);
                                                param[5] = new SqlParameter("@ProjectName", DBNull.Value);
                                                param[6] = new SqlParameter("@SampleCategoryName", DBNull.Value);
                                                param[7] = new SqlParameter("@TestName", DBNull.Value);
                                                param[8] = new SqlParameter("@MethodNumber", DBNull.Value);
                                                param[9] = new SqlParameter("@DateReceivedFrom", DBNull.Value);
                                                param[10] = new SqlParameter("@DateReceivedTo", DBNull.Value);
                                                param[11] = new SqlParameter("@DateCollectedFrom", DBNull.Value);
                                                param[12] = new SqlParameter("@DateCollectedTo", DBNull.Value);
                                                cmd.Parameters.AddRange(param);
                                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                                {
                                                    sda.Fill(dt);
                                                    ds.Tables.Add(dt);
                                                }
                                                List<DataRow> rows = (from DataRow row in dt.Rows select row).ToList();
                                                if (FillName == "JobID" && dt != null && dt.Columns.Contains("JobID"))
                                                {
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        IList<Samplecheckin> obj = new List<Samplecheckin>();
                                                        if (dt.Columns["JobID"].DataType == typeof(Guid))
                                                        {
                                                            obj = ObjectSpace.GetObjects<Samplecheckin>(new InOperator("Oid", rows.Select(x => x.ItemArray[0])));
                                                        }
                                                        else
                                                        {
                                                            obj = ObjectSpace.GetObjects<Samplecheckin>(new InOperator("JobID", rows.Select(x => x.ItemArray[0])));
                                                        }
                                                        paramjobid = String.Join("; ", obj.Select(x => String.Join("; ", x.Oid)).Distinct());
                                                        string[] strsamplchkoid = paramjobid.Split(';');
                                                        objERG.ALLEDDTemplateJobID = string.Join("; ", strsamplchkoid.Distinct());
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("NPJobid");
                                                            if (item is AspxGridLookupCustomEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                if (!string.IsNullOrEmpty(objERG.ALLEDDTemplateJobID))
                                                                {
                                                                    editor.Enabled = true; 
                                                                }
                                                                else
                                                                {
                                                                    editor.Enabled = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("JobID");
                                                            if (item is AspxGridLookupCustomEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                editor.Enabled = false;
                                                            }
                                                        }
                                                    }
                                                   
                                                }
                                                if (FillName == "ClientName" && dt != null)
                                                {
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        IList<Customer> obj = ObjectSpace.GetObjects<Customer>(new InOperator("CustomerName", rows.Select(x => x.ItemArray[0])));
                                                        var paramclient = String.Join("; ", obj.Select(x => String.Join("; ", x.Oid)));
                                                        objERG.ALLTemplateEDDClient = paramclient;
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("NPClient");
                                                            if (item is AspxGridLookupCustomEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDClient))
                                                                {
                                                                    editor.Enabled = true;
                                                                }
                                                                else
                                                                {
                                                                    editor.Enabled = false;
                                                                }
                                                            }
                                                        } 
                                                    }
                                                    else
                                                    {
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("NPClient");
                                                            if (item is AspxGridLookupCustomEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                editor.Enabled = false;
                                                            }
                                                        }

                                                    }
                                                }
                                                if (FillName == "ProjectID" && dt != null && dt.Rows.Count > 0)
                                                {
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        IList<Project> obj = ObjectSpace.GetObjects<Project>(new InOperator("ProjectId", rows.Select(x => x.ItemArray[0])));
                                                        var paramprojectid = String.Join("; ", obj.Select(x => String.Join("; ", x.Oid)));
                                                        objERG.ALLTemplateEDDProjectID = paramprojectid;
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("NPProjectID");
                                                            if (item is AspxGridLookupCustomEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDProjectID))
                                                                {
                                                                    editor.Enabled = true;
                                                                }
                                                                else
                                                                {
                                                                    editor.Enabled = false;
                                                                }
                                                            }
                                                        } 
                                                    }
                                                    else
                                                    {
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("NPProjectID");
                                                            if (item is AspxGridLookupCustomEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                editor.Enabled = false;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (FillName == "ProjectName" && dt != null)
                                                {
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        IList<Project> obj = ObjectSpace.GetObjects<Project>(new InOperator("ProjectName", rows.Select(x => x.ItemArray[0])));
                                                        var paramprojectname = String.Join("; ", obj.Select(x => String.Join("; ", x.Oid)));
                                                        objERG.ALLTemplateEDDProjectName = paramprojectname;
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("ProjectName");
                                                            if (item is ASPxCheckedLookupStringPropertyEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDProjectName))
                                                                {
                                                                    editor.Enabled = true; 
                                                                }
                                                                else
                                                                {
                                                                    editor.Enabled = false;
                                                                }
                                                            }
                                                        } 
                                                    }
                                                    else
                                                    {
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("ProjectName");
                                                            if (item is ASPxCheckedLookupStringPropertyEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                editor.Enabled = false;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (FillName == "SampleCategory" && dt != null)
                                                {
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        IList<SampleCategory> obj = ObjectSpace.GetObjects<SampleCategory>(new InOperator("SampleCategoryName", rows.Select(x => x.ItemArray[0])));
                                                        var paramsamplecategoryname = String.Join("; ", obj.Select(x => String.Join("; ", x.Oid)));
                                                        objERG.ALLTemplateEDDSampleCategory = paramsamplecategoryname;
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("SampleCategory");
                                                            if (item is ASPxCheckedLookupStringPropertyEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDSampleCategory))
                                                                {
                                                                    editor.Enabled = true;
                                                                }
                                                                else
                                                                {
                                                                    editor.Enabled = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("SampleCategory");
                                                            if (item is ASPxCheckedLookupStringPropertyEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                editor.Enabled = false;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (FillName == "Test" && dt != null)
                                                {
                                                    if(dt.Rows.Count > 0)
                                                    { 
                                                    IList<TestMethod> obj = ObjectSpace.GetObjects<TestMethod>(new InOperator("TestName", rows.Select(x => x.ItemArray[0])));
                                                    var paramtest = String.Join("; ", obj.Select(x => String.Join("; ", x.Oid)));
                                                    objERG.ALLTemplateEDDTest = paramtest;
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("Test");
                                                            if (item is ASPxCheckedLookupStringPropertyEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDTest))
                                                                {
                                                                    editor.Enabled = true;
                                                                }
                                                                else
                                                                {
                                                                    editor.Enabled = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("Test");
                                                            if (item is ASPxCheckedLookupStringPropertyEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                editor.Enabled = false;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (FillName == "Method" && dt != null)
                                                {
                                                    if(dt.Rows.Count > 0)
                                                    { 
                                                    IList<Method> obj = ObjectSpace.GetObjects<Method>(new InOperator("MethodNumber", rows.Select(x => x.ItemArray[0])));
                                                    var parammethodname = String.Join("; ", obj.Select(x => String.Join("; ", x.Oid)));
                                                    objERG.ALLTemplateEDDMethod = parammethodname;
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("Method");
                                                            if (item is ASPxCheckedLookupStringPropertyEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDMethod))
                                                                {
                                                                    editor.Enabled = true;
                                                                }
                                                                else
                                                                {
                                                                    editor.Enabled = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var detailView = View as DetailView;
                                                        if (detailView != null)
                                                        {
                                                            ViewItem item = detailView.FindItem("Method");
                                                            if (item is ASPxCheckedLookupStringPropertyEditor propertyEditor && propertyEditor.Editor is ASPxGridLookup editor)
                                                            {
                                                                editor.Enabled = false;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (FillName == "DateCollected" && dt != null && dt.Columns.Contains("DateCollected") && dt.Rows.Count > 0)
                                                {
                                                    objERG.IsDateCollected = true;
                                                    var detailView = View as DetailView;
                                                    if (detailView != null)
                                                    {
                                                        ViewItem item = detailView.FindItem("DateCollectedFrom");
                                                        ViewItem item1 = detailView.FindItem("DateCollectedTo");
                                                        if (item is ASPxDateTimePropertyEditor propertyEditor && propertyEditor.Editor != null)
                                                        {
                                                            propertyEditor.AllowEdit.SetItemValue("DateCollected", true);
                                                        }
                                                        if (item1 is ASPxDateTimePropertyEditor propertyEditor1 && propertyEditor1.Editor != null)
                                                        {
                                                            propertyEditor1.AllowEdit.SetItemValue("DateCollected", true);
                                                        }
                                                    }

                                                }
                                                else if(FillName == "DateCollected")
                                                {
                                                    objERG.IsDateCollected = false;
                                                    var detailView = View as DetailView;
                                                    if (detailView != null)
                                                    {
                                                        ViewItem item = detailView.FindItem("DateCollectedFrom");
                                                        ViewItem item1 = detailView.FindItem("DateCollectedTo");
                                                        if (item is ASPxDateTimePropertyEditor propertyEditor && propertyEditor.Editor != null)
                                                        {
                                                            propertyEditor.AllowEdit.SetItemValue("DateCollected", false);
                                                        }
                                                        if (item1 is ASPxDateTimePropertyEditor propertyEditor1 && propertyEditor1.Editor != null)
                                                        {
                                                            propertyEditor1.AllowEdit.SetItemValue("DateCollected", false);
                                                        }
                                                    }
                                                }
                                                if (FillName == "DateReceived" && dt != null && dt.Columns.Contains("DateReceived") && dt.Rows.Count > 0)
                                                {
                                                    objERG.IsDateReceived = true;
                                                    var detailView = View as DetailView;
                                                    if (detailView != null)
                                                    {
                                                        ViewItem item = detailView.FindItem("DateReceivedFrom");
                                                        ViewItem item1 = detailView.FindItem("DateReceivedTo");
                                                        if (item is ASPxDateTimePropertyEditor propertyEditor && propertyEditor.Editor != null)
                                                        {
                                                            propertyEditor.AllowEdit.SetItemValue("DateReceived", true);
                                                        }
                                                        if (item1 is ASPxDateTimePropertyEditor propertyEditor1 && propertyEditor1.Editor != null)
                                                        {
                                                            propertyEditor1.AllowEdit.SetItemValue("DateReceived", true);
                                                        }
                                                    }
                                                }
                                                else if(FillName == "DateReceived")
                                                {
                                                    objERG.IsDateReceived = false;
                                                    var detailView = View as DetailView;
                                                    if (detailView != null)
                                                    {
                                                        ViewItem item = detailView.FindItem("DateReceivedFrom");
                                                        ViewItem item1 = detailView.FindItem("DateReceivedTo");
                                                        if (item is ASPxDateTimePropertyEditor propertyEditor && propertyEditor.Editor != null)
                                                        {
                                                            propertyEditor.AllowEdit.SetItemValue("DateReceived", false);
                                                        }
                                                        if (item1 is ASPxDateTimePropertyEditor propertyEditor1 && propertyEditor1.Editor != null)
                                                        {
                                                            propertyEditor1.AllowEdit.SetItemValue("DateReceived", false);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
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

        private void GridLookup_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                ASPxGridLookup grid = sender as ASPxGridLookup;
                if (grid!=null&& grid.GridView!=null)
                {
                    if (grid.GridView.KeyFieldName=="JobID")
                    {
                        ((EDDReportGenerator)View.CurrentObject).JobID = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("JobID"));
                    }
                    if (grid.GridView.KeyFieldName== "Client")
                    {
                        ((EDDReportGenerator)View.CurrentObject).Client = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("Client"));
                    } 
                    if (grid.GridView.KeyFieldName== "ProjectID")
                    {
                        ((EDDReportGenerator)View.CurrentObject).ProjectID = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("ProjectID"));
                    } 
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View.Id == "EDDReportGenerator_DetailView_popup")
                {
                    if (View is DetailView)
                    {
                        EDDReportGenerator objERG = (EDDReportGenerator)View.CurrentObject;
                        foreach (ViewItem item in ((DetailView)View).Items.Where(i => (i.Id == "NPJobid" || i.Id == "NPClient" || i.Id == "NPProjectID") && i.GetType() == typeof(AspxGridLookupCustomEditor)))
                        {
                            AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                //Modules.BusinessObjects.Setting.DOC SC = (Modules.BusinessObjects.Setting.DOC)View.CurrentObject;
                                if (objERG != null)
                                {
                                    if (gridLookup != null && item.Id== "NPJobid")
                                    {
                                        gridLookup.JSProperties["cpJobID"] = objERG.JobID;
                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            }";
                                        gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                        gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                        gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                        gridLookup.ValueChanged += GridLookup_ValueChanged;
                                        gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                        gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "JobID"});
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Client" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Address" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "ProjectID" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "ProjectName" });
                                        gridLookup.GridView.Columns["JobID"].Width = 100;
                                        gridLookup.GridView.Columns["Client"].Width = 150;
                                        gridLookup.GridView.Columns["Address"].Width = 150;
                                        gridLookup.GridView.Columns["ProjectID"].Width = 100;
                                        gridLookup.GridView.Columns["ProjectName"].Width = 100;
                                        gridLookup.GridView.KeyFieldName = "JobID";
                                        gridLookup.TextFormatString = "{0}";
                                        DataTable table = new DataTable();
                                        table.Columns.Add("JobID");
                                        table.Columns.Add("Client");
                                        table.Columns.Add("Address");
                                        table.Columns.Add("ProjectID");
                                        table.Columns.Add("ProjectName");
                                        //gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                        //gridLookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                        //gridLookup.GridView.Settings.VerticalScrollableHeight = 200;
                                        if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.ALLEDDTemplateJobID))
                                        {
                                            gridLookup.Enabled = true;
                                            int i = 0;
                                            string[] guidStrings = objERG.ALLEDDTemplateJobID.Split(';');
                                            List<Guid> lstOid = guidStrings.Where(guidString => Guid.TryParse(guidString, out _)).Select(guidString => Guid.Parse(guidString)).Distinct().ToList();
                                            IList<Samplecheckin> lstSC = ObjectSpace.GetObjects<Samplecheckin>(new InOperator("Oid", lstOid));
                                            foreach (Samplecheckin objSC in lstSC.OrderByDescending(a => a.JobID).ToList())
                                            {
                                                table.Rows.Add(new object[]{objSC.JobID,objSC.ClientName != null ? " " + objSC.ClientName.CustomerName : string.Empty,objSC.ClientAddress != null ? " " + objSC.ClientAddress : string.Empty,
                                                   objSC.ProjectID != null ? " " + objSC.ProjectID.ProjectId : string.Empty,objSC.ProjectID != null ? " " + objSC.ProjectID.ProjectName : string.Empty});
                                                i++;
                                            }
                                            DataView dv = new DataView(table);
                                            dv.Sort = "JobID Desc";
                                            table = dv.ToTable();
                                        }
                                        else if(string.IsNullOrEmpty(objERG.ALLEDDTemplateJobID))
                                        {
                                            gridLookup.Enabled = false;
                                        }
                                        gridLookup.GridView.DataSource = table;
                                        gridLookup.GridView.DataBind();
                                    }
                                    else if(gridLookup != null && item.Id == "NPClient")
                                    {
                                        gridLookup.GridViewProperties.Settings.ShowFilterRow = true;
                                        gridLookup.JSProperties["cpClient"] = objERG.JobID;
                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpClient);
                                            }";
                                        gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                        gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                        gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                        gridLookup.ValueChanged += GridLookup_ValueChanged;
                                        gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                        gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Client" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Oid" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Address" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Phone" });
                                        gridLookup.GridView.Columns["Client"].Width = 150;
                                        gridLookup.GridView.Columns["Address"].Width = 150;
                                        gridLookup.GridView.Columns["Phone"].Width = 150;
                                        gridLookup.GridView.Columns["Oid"].Visible = false;
                                        gridLookup.GridView.KeyFieldName = "Oid";
                                        gridLookup.TextFormatString = "{0}";
                                        DataTable table = new DataTable();
                                        table.Columns.Add("Client");
                                        table.Columns.Add("Oid");
                                        table.Columns.Add("Address");
                                        table.Columns.Add("Phone");
                                        //gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                        //gridLookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                        //gridLookup.GridView.Settings.VerticalScrollableHeight = 200;
                                        if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.ALLTemplateEDDClient))
                                        {
                                            gridLookup.Enabled = true;
                                            int i = 0;
                                            string[] guidStrings = objERG.ALLTemplateEDDClient.Split(';');
                                            List<Guid> lstOid = guidStrings.Where(guidString => Guid.TryParse(guidString, out _)).Select(guidString => Guid.Parse(guidString)).Distinct().ToList();
                                            IList<Customer> lstSC = ObjectSpace.GetObjects<Customer>(new InOperator("Oid", lstOid));
                                            foreach (Customer objSC in lstSC.OrderBy(a => a.CustomerName).ToList())
                                            {
                                                table.Rows.Add(new object[]{objSC.CustomerName != null ? " " + objSC.CustomerName:string.Empty,objSC.Oid,objSC.Address != null ? " " + objSC.Address : string.Empty,
                                                    objSC.MobilePhone != null ? " " + objSC.MobilePhone : string.Empty});
                                                i++;
                                            }
                                            DataView dv = new DataView(table);
                                            dv.Sort = "Client Desc";
                                            table = dv.ToTable();
                                        }
                                        else if(string.IsNullOrEmpty(objERG.ALLTemplateEDDClient))
                                        {
                                            gridLookup.Enabled = false;
                                        }
                                        gridLookup.GridView.DataSource = table;
                                        gridLookup.GridView.DataBind();
                                    }
                                    else if(gridLookup != null && item.Id == "NPProjectID")
                                    {
                                        gridLookup.GridViewProperties.Settings.ShowFilterRow = true;
                                        gridLookup.JSProperties["cpProject"] = objERG.JobID;
                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpProject);
                                            }";
                                        gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                        gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                        gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                        gridLookup.ValueChanged += GridLookup_ValueChanged;
                                        gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                        gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "ProjectID" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "ProjectName" });
                                        gridLookup.GridView.Columns["ProjectID"].Width = 150;
                                        gridLookup.GridView.Columns["ProjectName"].Width = 150;
                                        gridLookup.GridView.KeyFieldName = "ProjectID";
                                        gridLookup.TextFormatString = "{0}";
                                        DataTable table = new DataTable();
                                        table.Columns.Add("ProjectID");
                                        table.Columns.Add("ProjectName");
                                        //gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                        //gridLookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                        //gridLookup.GridView.Settings.VerticalScrollableHeight = 200;
                                        if (objERG.EddTemplate!=null && !string.IsNullOrEmpty(objERG.ALLTemplateEDDProjectID))
                                        {
                                            gridLookup.Enabled = true;
                                            int i = 0;
                                            string[] guidStringsAllProject = objERG.ALLTemplateEDDProjectID.Split(';');
                                            List<Guid> lstOidproject = guidStringsAllProject.Where(guidString => Guid.TryParse(guidString, out _)).Select(guidString => Guid.Parse(guidString)).Distinct().ToList();
                                            IList<Project> lstSC = ObjectSpace.GetObjects<Project>(new InOperator("Oid", lstOidproject));
                                            foreach (Project objSC in lstSC.GroupBy(p => new { p.ProjectId, p.ProjectName }).Select(grp => grp.FirstOrDefault()).OrderBy(a => a.ProjectId).ToList())
                                            {
                                                table.Rows.Add(new object[]{objSC.ProjectId != null ? " " + objSC.ProjectId:string.Empty,objSC.ProjectName != null ? " " + objSC.ProjectName : string.Empty,});
                                                i++;
                                            }
                                            DataView dv = new DataView(table);
                                            dv.Sort = "ProjectID Desc";
                                            table = dv.ToTable();
                                        }
                                        else if(string.IsNullOrEmpty(objERG.ALLTemplateEDDProjectID))
                                        {
                                            gridLookup.Enabled = false;
                                        }
                                        gridLookup.GridView.DataSource = table;
                                        gridLookup.GridView.DataBind();
                                    }
                                }
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
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
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        private void SetConnectionString()
        {
            try
            {
                AppSettingsReader config = new AppSettingsReader();
                string serverType, server, database, user, password;
                string[] connectionstring = ObjReportDesignerInfo.WebConfigConn.Split(';');
                ObjReportDesignerInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLUserID = connectionstring[3].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLPassword = connectionstring[4].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ClearEDD_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                EDDReportGenerator objERG = (EDDReportGenerator)e.CurrentObject;
                if (objERG != null)
                {
                    objERG.EddTemplate = null;
                    objERG.ALLEDDTemplateJobID = null;
                    objERG.ALLTemplateEDDClient = null;
                    objERG.ALLTemplateEDDMethod = null;
                    objERG.ALLTemplateEDDProjectID = null;
                    objERG.ALLTemplateEDDProjectName = null;
                    objERG.ALLTemplateEDDSampleCategory = null;
                    objERG.JobID = null;
                    objERG.Client = null;
                    objERG.ProjectID = null;
                    objERG.ProjectName = null;
                    objERG.Test = null;
                    objERG.Method = null;
                    objERG.SampleCategory = null;
                    objERG.DateCollectedFrom = DateTime.MinValue;
                    objERG.DateCollectedTo = DateTime.MinValue;
                    objERG.DateReceivedFrom = DateTime.MinValue;
                    objERG.DateReceivedTo = DateTime.MinValue;
                    if (View is DetailView)
                    {
                        foreach (ViewItem item in ((DetailView)View).Items)
                        {
                            if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                            {
                                ASPxCheckedLookupStringPropertyEditor propertyeditor = item as ASPxCheckedLookupStringPropertyEditor;
                                if (propertyeditor != null && propertyeditor.AllowEdit && propertyeditor.Editor != null)
                                {
                                    ASPxGridLookup editor = (ASPxGridLookup)propertyeditor.Editor;
                                    if (editor != null)
                                    {
                                        editor.Enabled = false;
                                    }
                                }
                            }
                            if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                            {
                                ASPxDateTimePropertyEditor propertyeditor = item as ASPxDateTimePropertyEditor;
                                if (propertyeditor != null && propertyeditor.AllowEdit && propertyeditor.Editor != null)
                                {
                                    propertyeditor.AllowEdit.SetItemValue("DateReceived", false);
                                    propertyeditor.AllowEdit.SetItemValue("DateCollected", true);
                                }

                            }
                            if (item.GetType() == typeof(AspxGridLookupCustomEditor))
                            {
                                AspxGridLookupCustomEditor propertyeditor = item as AspxGridLookupCustomEditor;
                                if (propertyeditor != null && propertyeditor.AllowEdit && propertyeditor.Editor != null)
                                {
                                    ASPxGridLookup editor = (ASPxGridLookup)propertyeditor.Editor;
                                    if (editor != null)
                                    {
                                        editor.Enabled = false;
                                    }
                                }
                            }
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
    }
}
