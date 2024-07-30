using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
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
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;

namespace LDM.Module.Web.Controllers.SamplingProposal
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SamplingProposalViewController : ViewController
    {
        ASPxGridListEditor GridListEditor;
        MessageTimer timer = new MessageTimer();
        ResourceManager rm;
        curlanguage objLanguage = new curlanguage();
        SamplingManagementInfo objSmInfo = new SamplingManagementInfo();
        public SamplingProposalViewController()
        {
            InitializeComponent();
            TargetViewId = "SamplingProposal_DetailView;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (objSmInfo.lstTestgridviewrow == null)
                {
                    objSmInfo.lstTestgridviewrow = new List<string>();
                }
                rm = new ResourceManager("Resources.SDMS", Assembly.Load("App_GlobalResources"));
                if (View.Id == "SamplingProposal_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View.Id == "SamplingProposal_DetailView")
                {
                    if (View is DetailView)
                    {
                        Modules.BusinessObjects.SamplingManagement.SamplingProposal objSample = (Modules.BusinessObjects.SamplingManagement.SamplingProposal)View.CurrentObject;
                        foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "Test" && i.GetType() == typeof(AspxGridLookupCustomEditor)))
                        {
                            AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                gridLookup.GridView.CustomCallback += GridView_CustomCallback;
                                gridLookup.GridView.Load += GridView_Load;
                                Modules.BusinessObjects.SamplingManagement.SamplingProposal SC = (Modules.BusinessObjects.SamplingManagement.SamplingProposal)View.CurrentObject;
                                if (SC != null)
                                {
                                    if (gridLookup != null)
                                    {
                                        gridLookup.GridViewProperties.Settings.ShowFilterRow = true;
                                        if (!View.ObjectSpace.IsNewObject(SC))
                                        {
                                            List<Sampling> lstSamples = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", SC.Oid)).ToList();
                                            if (lstSamples.Count > 0)
                                            {
                                                objSmInfo.strNPTest = "Disable";
                                            }
                                            else
                                            {
                                                objSmInfo.strNPTest = string.Empty;
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
                                        if (objSmInfo.isNoOfSampleDisable)
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
                                                    string strtestview = objTest.TestMethod.TestName.ToString() + "_" + objTest.TestMethod.MethodName.MethodNumber.ToString();

                                                    if (!string.IsNullOrEmpty(SC.NPTest))
                                                    {
                                                        string[] strtestarr = SC.NPTest.Split(';');
                                                        if (strtestarr.Contains(strtestview))
                                                        {
                                                            objSmInfo.lstTestgridviewrow.Add(i.ToString());
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
                                        objSmInfo.dtTest = table;
                                        gridLookup.GridView.DataBind();
                                        ;
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
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
           
            base.OnDeactivated();
            if (View.Id == "SamplingProposal_DetailView")
            {
                objSmInfo.strNPTest = string.Empty;
                objSmInfo.lstTestgridviewrow = null;
                objSmInfo.strtempNPTEST = string.Empty;
                Frame.GetController<ModificationsController>().SaveAction.Execute -= SaveAction_Execute;
            }

        }
        private void SaveAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.SamplingManagement.SamplingProposal objSample = (Modules.BusinessObjects.SamplingManagement.SamplingProposal)View.CurrentObject;
                foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "Test" && i.GetType() == typeof(AspxGridLookupCustomEditor)))
                {
                    AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                    if (propertyEditor != null && propertyEditor.Editor != null)
                    {
                        ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                        Modules.BusinessObjects.SamplingManagement.SamplingProposal SC = (Modules.BusinessObjects.SamplingManagement.SamplingProposal)View.CurrentObject;
                        if (SC != null)
                        {
                            if (gridLookup != null)
                            {
                                if (!View.ObjectSpace.IsNewObject(SC))
                                {
                                    List<Sampling> lstSamples = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", SC.Oid)).ToList();
                                    if (lstSamples.Count > 0)
                                    {
                                        objSmInfo.strNPTest = "Disable";
                                        gridLookup.GridView.ClientSideEvents.RowClick = "function(s,e){e.cancel = true;}";
                                    }
                                    else
                                    {
                                        objSmInfo.strNPTest = string.Empty;
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
                //if (objSample != null)
                //{
                //    IObjectSpace objSpace = Application.CreateObjectSpace();
                //    objSample = objSpace.GetObjectByKey<Modules.BusinessObjects.SamplingManagement.SamplingProposal>(objSample.Oid);
                //    TaskJobIDAutomation objTaskAutomation = objSpace.FindObject<TaskJobIDAutomation>(CriteriaOperator.Parse("[SRID] =? ", objSample));
                //    if (objTaskAutomation == null)
                //    {
                //        TaskJobIDAutomation objTask = objSpace.CreateObject<TaskJobIDAutomation>();
                //        objTask.SRID = objSample;
                //        objSpace.CommitChanges();
                //    }
                //}

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
                foreach (string strtest in objSmInfo.lstTestgridviewrow.Distinct().ToList())
                {
                    grid.Selection.SelectRow(Convert.ToInt32(strtest));
                }

                if (objSmInfo.lstTestgridviewrow.Count > 0)
                {
                    List<string> lsttest = new List<string>();
                    foreach (string strtest in objSmInfo.lstTestgridviewrow.Distinct().ToList())
                    {
                        lsttest.Add(grid.GetRowValues(Convert.ToInt32(strtest), "Test").ToString());
                    }
                    objSmInfo.strtempNPTEST = string.Join(";", lsttest);
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
                if (objSmInfo.strNPTest != null || objSmInfo.strNPTest != string.Empty)
                {
                    if (objSmInfo.strNPTest == "Disable")
                    {
                        e.Enabled = false;
                    }
                }
            }
        }
        private void GridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                var grid = sender as ASPxGridView;
                foreach (string strtest in objSmInfo.lstTestgridviewrow.Distinct().ToList())
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
        private void GridLookup_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                //if(grid != null)
                {
                    string str = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("TextView").OrderBy(i => i));

                    if (((Modules.BusinessObjects.SamplingManagement.SamplingProposal)View.CurrentObject).NPTest != str)
                    {
                        if (string.IsNullOrEmpty(objSmInfo.strtempNPTEST))
                        {
                            ((Modules.BusinessObjects.SamplingManagement.SamplingProposal)View.CurrentObject).NPTest = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("TextView").OrderBy(i => i));
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
