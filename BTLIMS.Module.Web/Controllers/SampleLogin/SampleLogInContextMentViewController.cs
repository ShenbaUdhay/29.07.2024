using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Controllers.SampleLogInWeb
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SampleLogInContextMentViewController : ViewController
    {
        ASPxGridListEditor GridListEditor;
        MessageTimer timer = new MessageTimer();
        ResourceManager rm;
        //string CurrentLanguage = string.Empty;
        curlanguage objLanguage = new curlanguage();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        Samplecheckin objSamplecheckin = null;
        string[] NoNeed = { "TAT", "Recieved By", "Received Date" };
        public SampleLogInContextMentViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "SampleLogIn_ListView;" + "Samplecheckin_DetailView_Copy_SampleRegistration;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (SRInfo.lstTestgridviewrow == null)
                {
                    SRInfo.lstTestgridviewrow = new List<string>();
                }
                rm = new ResourceManager("Resources.SDMS", Assembly.Load("App_GlobalResources"));
                if (View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;
                    if (View != null &&((DetailView)View).LayoutManager != null && ((DetailView)View).LayoutManager is WebLayoutManager)
                    {
                        objSamplecheckin = (Samplecheckin)View.CurrentObject;
                        ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated += ViewController1_ItemCreated;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.                   
        }
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration")
                {
                    if (e.PropertyName == "ProjectID")
                    {
                        AspxStringComoboxPropertyEditor ProjectLoc = ((DetailView)View).FindItem("ProjectLocation") as AspxStringComoboxPropertyEditor;
                        if (ProjectLoc != null && ProjectLoc.Editor != null)
                        {
                            ((ASPxComboBox)ProjectLoc.Editor).Items.Clear();

                            Samplecheckin objCheckin = (Samplecheckin)View.CurrentObject;
                            if (objCheckin != null && objCheckin.ClientName != null && objCheckin.ProjectID != null)
                            {

                                SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("GetProjectLocationByProjectID", new OperandValue(objCheckin.ClientName.CustomerName), new OperandValue(objCheckin.ProjectID.ProjectId));
                                if (sprocReport.ResultSet != null)
                                {
                                    foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                                    {
                                        if (row.Values[0] != null)
                                            ((ASPxComboBox)ProjectLoc.Editor).Items.Add(row.Values[0].ToString());
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
        private void ViewController1_ItemCreated(object sender, ItemCreatedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject != null && e.TemplateContainer != null && e.TemplateContainer is LayoutItemTemplateContainer)
                {
                    string strCaption = ((LayoutItemTemplateContainer)e.TemplateContainer).Caption;
                    if (objSamplecheckin.IsSampling)
                    {
                        if (((LayoutItemTemplateContainer)e.TemplateContainer).Caption!=null && (((LayoutItemTemplateContainer)e.TemplateContainer).Caption.Contains("TAT") || ((LayoutItemTemplateContainer)e.TemplateContainer).Caption.Contains("Received By") || ((LayoutItemTemplateContainer)e.TemplateContainer).Caption.Contains("Received Date") || ((LayoutItemTemplateContainer)e.TemplateContainer).Caption.Contains("Job ID")))
                        {
                            ((LayoutItemTemplateContainer)e.TemplateContainer).Caption= strCaption.Replace("*", "");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(strCaption) && strCaption.Length > 0 && strCaption.Substring(strCaption.Length - 1) == "*")
                            {
                                if (e.TemplateContainer.CaptionControl != null)
                                {
                                    CustomizeCaptionControl(e.TemplateContainer.CaptionControl);
                                }
                                else
                                {
                                    e.TemplateContainer.Load += TemplateContainer_Load;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (((LayoutItemTemplateContainer)e.TemplateContainer).Caption != null && (((LayoutItemTemplateContainer)e.TemplateContainer).Caption.Contains("Project ID") || ((LayoutItemTemplateContainer)e.TemplateContainer).Caption.Contains("Job ID")))
                        {
                            ((LayoutItemTemplateContainer)e.TemplateContainer).Caption = strCaption.Replace("*", "");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(strCaption) && strCaption.Length > 0 && strCaption.Substring(strCaption.Length - 1) == "*")
                            {
                                if (e.TemplateContainer.CaptionControl != null)
                                {
                                    CustomizeCaptionControl(e.TemplateContainer.CaptionControl);
                                }
                                else
                                {
                                    e.TemplateContainer.Load += TemplateContainer_Load;
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
        private void TemplateContainer_Load(object sender, EventArgs e)
        {
            try
            {
                if (sender != null && sender is LayoutItemTemplateContainerBase)
                {
                    LayoutItemTemplateContainerBase templateControler = (LayoutItemTemplateContainerBase)sender;
                    if (templateControler != null)
                    {
                        templateControler.Load -= TemplateContainer_Load;
                        if (templateControler != null && templateControler.CaptionControl != null)
                        {
                            CustomizeCaptionControl(templateControler.CaptionControl);
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
        private void CustomizeCaptionControl(WebControl captionControl)
        {
            try
            {
                if (captionControl != null)
                {
                    captionControl.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                Samplecheckin objSample = (Samplecheckin)View.CurrentObject;
                foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "Test" && i.GetType() == typeof(AspxGridLookupCustomEditor)))
                {
                    AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                    if (propertyEditor != null && propertyEditor.Editor != null)
                    {
                        ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                        Samplecheckin SC = (Samplecheckin)View.CurrentObject;
                        if (SC != null)
                        {
                            if (gridLookup != null)
                            {
                                if (!View.ObjectSpace.IsNewObject(SC))
                                {
                                    List<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstSamples = View.ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("JobID.Oid=?", SC.Oid)).ToList();
                                    //IList<SampleParameter> parameters = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Samplelogin.JobID.Oid=?", SC.Oid));
                                    if (lstSamples.Count > 0) //&& string.IsNullOrEmpty(SC.NPTest)
                                    {
                                        SRInfo.strNPTest = "Disable";
                                        gridLookup.GridView.ClientSideEvents.RowClick = "function(s,e){e.cancel = true;}";
                                    }
                                    else
                                    {
                                        SRInfo.strNPTest = string.Empty;
                                        gridLookup.GridView.ClientSideEvents.RowClick = "function(s,e){e.cancel = false;}";
                                    }
                                }
                            }
                        }
                    }
                    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                    {
                        propertyEditor.Editor.BackColor = Color.LightYellow;
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
            // Access and customize the target View control.           
            try
            {
                if (View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration")
                {
                    if (View is DetailView)
                    {
                        Samplecheckin objSample = (Samplecheckin)View.CurrentObject;
                        foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "Test" && i.GetType() == typeof(AspxGridLookupCustomEditor)))
                        {
                            AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                //gridLookup.GridView.CustomCallback += GridView_CustomCallback;
                                //gridLookup.GridView.Load += GridView_Load;
                                Samplecheckin SC = (Samplecheckin)View.CurrentObject;
                                if (SC != null)
                                {
                                    if (gridLookup != null)
                                    {
                                        gridLookup.GridViewProperties.Settings.ShowFilterRow = true;
                                        if (!View.ObjectSpace.IsNewObject(SC))
                                        {
                                            List<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstSamples = View.ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("JobID.Oid=?", SC.Oid)).ToList();
                                            //IList<SampleParameter> parameters = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Samplelogin.JobID.Oid=?", SC.Oid));
                                            if (lstSamples.Count > 0) //&& string.IsNullOrEmpty(SC.NPTest)
                                            {
                                                SRInfo.strNPTest = "Disable";
                                            }
                                            else
                                            {
                                                SRInfo.strNPTest = string.Empty;
                                            }
                                        }
                                        gridLookup.JSProperties["cpTest"] = SC.NPTest;
                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpTest);
                                            }";
                                        gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                        gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                        gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                        gridLookup.ValueChanged += GridLookup_ValueChanged;
                                        gridLookup.GridView.CommandButtonInitialize += GridView_CommandButtonInitialize;
                                        gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                        gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Test" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Method" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "IsGroup" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "TestName" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "TextView" });
                                        gridLookup.GridView.Columns["Test"].Width = 120;
                                        gridLookup.GridView.Columns["Method"].Width = 200;
                                        gridLookup.GridView.Columns["IsGroup"].Width = 100;
                                        gridLookup.GridView.VisibleColumns["TestName"].Visible = false;
                                        gridLookup.GridView.VisibleColumns["TextView"].Visible = false;
                                        gridLookup.GridView.KeyFieldName = "TextView";
                                        if (SRInfo.isNoOfSampleDisable)
                                        {
                                            gridLookup.GridView.ClientSideEvents.RowClick = "function(s,e){e.cancel = true;}";
                                        }
                                        else
                                        {
                                            gridLookup.GridView.ClientSideEvents.RowClick = "function(s,e){e.cancel = false;}";
                                        }
                                        gridLookup.TextFormatString = "{4}";
                                        DataTable table = new DataTable();
                                        table.Columns.Add("Test");
                                        table.Columns.Add("Method");
                                        table.Columns.Add("IsGroup", typeof(bool));
                                        table.Columns.Add("TestName");
                                        table.Columns.Add("TextView");
                                        gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                        gridLookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                        gridLookup.GridView.Settings.VerticalScrollableHeight = 200;
                                        if (SC.SampleMatries != null && !string.IsNullOrEmpty(SC.SampleMatries))
                                        {
                                            List<string> lstMatrix = SC.SampleMatries.Split(';').ToList().Select(i => i.Trim()).ToList();
                                            Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                            UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                            XPClassInfo testMatrixInfo;
                                            testMatrixInfo = uow.GetClassInfo(typeof(VisualMatrix));
                                            List<VisualMatrix> lstVisuaMatrix = uow.GetObjects(testMatrixInfo, new InOperator("Oid", lstMatrix.Select(i => new Guid(i.Trim()))), null, int.MaxValue, false, true).Cast<VisualMatrix>().ToList();
                                            //IList<VisualMatrix> lstVisuaMatrix = ObjectSpace.GetObjects<VisualMatrix>(new InOperator("Oid", lstMatrix.Select(i => new Guid(i.Trim()))));
                                            XPClassInfo testParameterInfo;
                                            testParameterInfo = uow.GetClassInfo(typeof(Testparameter));
                                            IList<Testparameter> lstTests = uow.GetObjects(testParameterInfo, new GroupOperator(GroupOperatorType.And, new InOperator("TestMethod.MatrixName.Oid", lstVisuaMatrix.Select(i => i.MatrixName)), CriteriaOperator.Parse("TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null")), new SortingCollection(), 0, 0, false, true).Cast<Testparameter>().ToList();
                                            //IList<Testparameter> lstTests = ObjectSpace.GetObjects<Testparameter>(new GroupOperator(GroupOperatorType.And, new InOperator("TestMethod.MatrixName.Oid", lstVisuaMatrix.Select(i => i.MatrixName)), CriteriaOperator.Parse("TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null")));
                                            if (lstTests != null && lstTests.Count > 0)
                                            {
                                                int i = 0;
                                                foreach (Testparameter objTest in lstTests.Where(a => a.TestMethod != null && a.IsGroup == false && a.TestMethod.MatrixName != null && a.TestMethod.MethodName != null).GroupBy(p => new { p.TestMethod.TestName, p.TestMethod.MethodName.MethodNumber, p.TestMethod.IsGroup }).Select(grp => grp.FirstOrDefault()).OrderBy(a => a.TestMethod.TestName).ThenBy(a => a.TestMethod.MethodName.MethodNumber).ToList())
                                                {
                                                    table.Rows.Add(new object[] { objTest.TestMethod.TestName, objTest.TestMethod.MethodName.MethodNumber, objTest.TestMethod.IsGroup, objTest.TestMethod.TestName + "|" + objTest.TestMethod.MethodName.MethodNumber + "|" + objTest.TestMethod.IsGroup, objTest.TestMethod.TestName + "|" + objTest.TestMethod.MethodName.MethodNumber });
                                                    if (!string.IsNullOrEmpty(SC.NPTest))
                                                    {
                                                        string[] strtestarr = SC.NPTest.Split(';');
                                                    string strtestview = objTest.TestMethod.TestName.ToString() + "_" + objTest.TestMethod.MethodName.MethodNumber.ToString();
                                                    if (strtestarr.Contains(strtestview))
                                                    {
                                                        SRInfo.lstTestgridviewrow.Add(i.ToString());
                                                    }
                                                    }
                                                    i++;
                                                }
                                                foreach (Testparameter objTest in lstTests.Where(a => a.TestMethod != null && a.IsGroup == true && a.TestMethod.MatrixName != null && a.TestMethod.TestName != null).GroupBy(p => new { p.TestMethod.TestName, p.TestMethod.IsGroup }).Select(grp => grp.FirstOrDefault()).OrderBy(a => a.TestMethod.TestName).ThenBy(a => a.TestMethod.IsGroup).ToList())
                                                {
                                                    table.Rows.Add(new object[] { objTest.TestMethod.TestName, " ", objTest.TestMethod.IsGroup, objTest.TestMethod.TestName + "|" + " " + "|" + objTest.TestMethod.IsGroup, objTest.TestMethod.TestName });
                                                }
                                                DataView dv = new DataView(table);
                                                dv.Sort = "Test Asc";
                                                table = dv.ToTable();
                                            }
                                        }
                                        gridLookup.GridView.DataSource = table;
                                        SRInfo.dtTest = table;
                                        gridLookup.GridView.DataBind();
                                    }
                                }
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        AspxStringComoboxPropertyEditor ProjectLoc = ((DetailView)View).FindItem("ProjectLocation") as AspxStringComoboxPropertyEditor;
                        if (ProjectLoc != null && ProjectLoc.Editor != null)
                        {
                            ((ASPxComboBox)ProjectLoc.Editor).Items.Clear();

                            Samplecheckin objCheckin = (Samplecheckin)View.CurrentObject;
                            if (objCheckin != null && objCheckin.ClientName != null && objCheckin.ProjectID != null)
                            {

                                SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("GetProjectLocationByProjectID", new OperandValue(objCheckin.ClientName.CustomerName), new OperandValue(objCheckin.ProjectID.ProjectId));
                                if (sprocReport.ResultSet != null)
                                {
                                    foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                                    {
                                        if (row.Values[0] != null)
                                            ((ASPxComboBox)ProjectLoc.Editor).Items.Add(row.Values[0].ToString());
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

        private void GridView_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                foreach (string strtest in SRInfo.lstTestgridviewrow.Distinct().ToList())
                {
                    grid.Selection.SelectRow(Convert.ToInt32(strtest));
                }

                if (SRInfo.lstTestgridviewrow.Count > 0)
                {
                    List<string> lsttest = new List<string>();
                    foreach (string strtest in SRInfo.lstTestgridviewrow.Distinct().ToList())
                    {
                        lsttest.Add(grid.GetRowValues(Convert.ToInt32(strtest), "Test").ToString());
                    }
                    SRInfo.strtempNPTEST = string.Join(";", lsttest);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                var grid = sender as ASPxGridView;
                foreach (string strtest in SRInfo.lstTestgridviewrow.Distinct().ToList())
                {
                    grid.Selection.SelectRow(Convert.ToInt32(strtest));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
            {
                if (SRInfo.strNPTest != null || SRInfo.strNPTest != string.Empty)
                {
                    if (SRInfo.strNPTest == "Disable")
                    {
                        e.Enabled = false;
                    }
                    //else
                    //{
                    //    var test = SRInfo.dtTest.Rows[e.VisibleIndex]["TextView"].ToString();
                    //    if (test != null && SRInfo.strNPTest.Split(';').Contains(test))
                    //    {
                    //        e.Enabled = false;
                    //    }
                    //}
                }
            }
        }

        private void GridLookup_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                //if(grid != null)
                {
                    if (SRInfo.BoolReset)
                    {
                        SRInfo.count++;
                        ((ASPxGridLookup)sender).GridView.Selection.UnselectAll();
                        if (SRInfo.count==2)
                        {
                            SRInfo.count = 0;
                            SRInfo.BoolReset = false; 
                        }
                    }
                    string str = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("TextView").OrderBy(i => i));
                    if(string.IsNullOrEmpty(str))
                    {
                        str = null;
                    }
                    if (((Samplecheckin)View.CurrentObject).NPTest != str)
                    {
                        //if (string.IsNullOrEmpty(SRInfo.strtempNPTEST))
                        //{
                            ((Samplecheckin)View.CurrentObject).NPTest = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("TextView").OrderBy(i => i));
                        //}
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
            base.OnDeactivated();
            if (View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration")
            {
                SRInfo.strNPTest = string.Empty;
                SRInfo.lstTestgridviewrow = null;
                SRInfo.strtempNPTEST = string.Empty;
                SRInfo.BoolReset = false;
                Frame.GetController<ModificationsController>().SaveAction.Execute -= SaveAction_Execute;
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                if (View != null && ((DetailView)View).LayoutManager != null && ((DetailView)View).LayoutManager is WebLayoutManager)
                {
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated -= ViewController1_ItemCreated;
                }
            }
        }
    }
}
