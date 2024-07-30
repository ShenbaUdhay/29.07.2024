using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System.Resources;
using System.Reflection;
using LDM.Module.Controllers.Public;
using DevExpress.Web;
using System.Drawing;
using System.Data;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using LDM.Module.Web.Editors;

namespace LDM.Module.Web.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class COCSettingsContextMentViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        ResourceManager rm;
        COCSettingsRegistrationInfo COCsr = new COCSettingsRegistrationInfo();

        public COCSettingsContextMentViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "COCSettings_DetailView_Copy_SampleRegistration";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                rm = new ResourceManager("Resources.SDMS", Assembly.Load("App_GlobalResources"));
                if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;
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
                COCSettings objSample = (COCSettings)View.CurrentObject;
                foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "Test" && i.GetType() == typeof(AspxGridLookupCustomEditor)))
                {
                    AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                    if (propertyEditor != null && propertyEditor.Editor != null)
                    {
                        ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                        COCSettings COCsc = (COCSettings)View.CurrentObject;
                        if (COCsc != null)
                        {
                            if (gridLookup != null)
                            {
                                if (!View.ObjectSpace.IsNewObject(COCsc))
                                {
                                    List<COCSettingsSamples> lstSamples = View.ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COCID.Oid=?", COCsc.Oid)).ToList();
                                    //IList<SampleParameter> parameters = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Samplelogin.JobID.Oid=?", SC.Oid));
                                    if (lstSamples.Count > 0) //&& string.IsNullOrEmpty(SC.NPTest)
                                    {
                                        COCsr.strNPTest = "Disable";
                                        gridLookup.GridView.ClientSideEvents.RowClick = "function(s,e){e.cancel = true;}";
                                    }
                                    else
                                    {
                                        COCsr.strNPTest = string.Empty;
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
                if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {
                    if (View is DetailView)
                    {
                        COCSettings objSample = (COCSettings)View.CurrentObject;
                        foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "Test" && i.GetType() == typeof(AspxGridLookupCustomEditor)))
                        {
                            AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                COCSettings cocSC = (COCSettings)View.CurrentObject;
                                if (cocSC != null)
                                {
                                    if (gridLookup != null)
                                    {
                                        if (!View.ObjectSpace.IsNewObject(cocSC))
                                        {
                                            List<COCSettingsSamples> lstSamples = View.ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COCID.Oid=?", cocSC.Oid)).ToList();
                                            if (lstSamples.Count > 0)
                                            {
                                                COCsr.strNPTest = "Disable";
                                            }
                                            else
                                            {
                                                COCsr.strNPTest = string.Empty;
                                            }
                                        }
                                        gridLookup.JSProperties["cpTest"] = cocSC.NPTest;
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
                                        if (COCsr.isNoOfSampleDisable)
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
                                        if (cocSC.SampleMatries != null && !string.IsNullOrEmpty(cocSC.SampleMatries))
                                        {
                                            List<string> lstMatrix = cocSC.SampleMatries.Split(';').ToList().Select(i => i.Trim()).ToList();
                                            Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                            UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                            XPClassInfo testMatrixInfo;
                                            testMatrixInfo = uow.GetClassInfo(typeof(VisualMatrix));
                                            List<VisualMatrix> lstVisuaMatrix = uow.GetObjects(testMatrixInfo, new InOperator("Oid", lstMatrix.Select(i => new Guid(i.Trim()))), null, int.MaxValue, false, true).Cast<VisualMatrix>().ToList();
                                            XPClassInfo testParameterInfo;
                                            testParameterInfo = uow.GetClassInfo(typeof(Testparameter));
                                            IList<Testparameter> lstTests = uow.GetObjects(testParameterInfo, new GroupOperator(GroupOperatorType.And, new InOperator("TestMethod.MatrixName.Oid", lstVisuaMatrix.Select(i => i.MatrixName)), CriteriaOperator.Parse("TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null")), new SortingCollection(), 0, 0, false, true).Cast<Testparameter>().ToList();
                                            if (lstTests != null && lstTests.Count > 0)
                                            {
                                                foreach (Testparameter objTest in lstTests.Where(a => a.TestMethod != null && a.IsGroup == false && a.TestMethod.MatrixName != null && a.TestMethod.MethodName != null).GroupBy(p => new { p.TestMethod.TestName, p.TestMethod.MethodName.MethodNumber, p.TestMethod.IsGroup }).Select(grp => grp.FirstOrDefault()).OrderBy(a => a.TestMethod.TestName).ThenBy(a => a.TestMethod.MethodName.MethodNumber).ToList())
                                                {
                                                    table.Rows.Add(new object[] { objTest.TestMethod.TestName, objTest.TestMethod.MethodName.MethodNumber, objTest.TestMethod.IsGroup, objTest.TestMethod.TestName + "_" + objTest.TestMethod.MethodName.MethodNumber + "_" + objTest.TestMethod.IsGroup, objTest.TestMethod.TestName + "_" + objTest.TestMethod.MethodName.MethodNumber });
                                                }
                                                foreach (Testparameter objTest in lstTests.Where(a => a.TestMethod != null && a.IsGroup == true && a.TestMethod.MatrixName != null && a.TestMethod.TestName != null).GroupBy(p => new { p.TestMethod.TestName, p.TestMethod.IsGroup }).Select(grp => grp.FirstOrDefault()).OrderBy(a => a.TestMethod.TestName).ThenBy(a => a.TestMethod.IsGroup).ToList())
                                                {
                                                    table.Rows.Add(new object[] { objTest.TestMethod.TestName, " ", objTest.TestMethod.IsGroup, objTest.TestMethod.TestName + "_" + " " + "_" + objTest.TestMethod.IsGroup, objTest.TestMethod.TestName });
                                                }
                                                DataView dv = new DataView(table);
                                                dv.Sort = "Test Asc";
                                                table = dv.ToTable();
                                            }
                                        }
                                        gridLookup.GridView.DataSource = table;
                                        COCsr.dtTest = table;
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
        private void GridView_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
            {
                if (COCsr.strNPTest != null || COCsr.strNPTest != string.Empty)
                {
                    if (COCsr.strNPTest == "Disable")
                    {
                        e.Enabled = false;
                    }

                }
            }
        }

        private void GridLookup_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                string str = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("TextView").OrderBy(i => i));
                if (((COCSettings)View.CurrentObject).NPTest != str)
                {
                    ((COCSettings)View.CurrentObject).NPTest = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("TextView").OrderBy(i => i));
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
            if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
            {
                COCsr.strNPTest = string.Empty;
                Frame.GetController<ModificationsController>().SaveAction.Execute -= SaveAction_Execute;
            }
        }
    }
}
