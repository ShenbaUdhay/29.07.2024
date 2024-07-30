using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labmaster.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SampleBottleAllocationViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        TaskManagementInfo TMInfo = new TaskManagementInfo();
        SamplingInfo SInfo = new SamplingInfo();
        COCSettingsInfo COCInfo = new COCSettingsInfo();
        public SampleBottleAllocationViewController()
        {
            InitializeComponent();
            TargetViewId = "SampleBottleAllocation_ListView_Sampleregistration;" + "SampleBottleAllocation_ListView_Taskregistration;" + "SampleBottleAllocation_ListView_COCSettings;"+ "COCSettings;"
                + "SamplingProposal;" + "SampleRegistration;";
            ADDTests.TargetViewId = "SampleBottleAllocation_ListView_Sampleregistration;" + "SampleBottleAllocation_ListView_Taskregistration;" + "SampleBottleAllocation_ListView_COCSettings;";
            RemoveTests.TargetViewId = "SampleBottleAllocation_ListView_Sampleregistration" + "SampleBottleAllocation_ListView_Taskregistration" + "SampleBottleAllocation_ListView_COCSettings";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void ADDTests_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string strsharedtest = string.Empty;
                if (Application.MainWindow.View.Id == "SampleRegistration")
                {
                    DashboardViewItem DVavailabletest = ((DashboardView)Application.MainWindow.View).FindItem("AvailableTest") as DashboardViewItem;
                    DashboardViewItem DVBottleAllocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                    if(DVavailabletest != null && DVavailabletest.InnerView != null && DVBottleAllocation != null && DVBottleAllocation.InnerView != null && DVavailabletest.InnerView.SelectedObjects.Count > 0 && DVBottleAllocation.InnerView.SelectedObjects.Count > 0)
                    {
                        foreach(SampleParameter objsmppara in DVavailabletest.InnerView.SelectedObjects)
                        {
                            foreach(SampleBottleAllocation objsmplbottle in DVBottleAllocation.InnerView.SelectedObjects)
                            {
                                strsharedtest = objsmplbottle.SharedTests;
                                objsmplbottle.SharedTests = strsharedtest + ", " + objsmppara.Testparameter.TestMethod.TestName;
                                DVBottleAllocation.InnerView.ObjectSpace.CommitChanges();
                                DVBottleAllocation.InnerView.ObjectSpace.Refresh();
                            }
                        }
                        SampleRegistrationAvailabletests();
                        Application.ShowViewStrategy.ShowMessage("Test added successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else if(DVavailabletest.InnerView.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select available test checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                    else if(DVBottleAllocation.InnerView.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select bottle allocation checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }

                else if (Application.MainWindow.View.Id == "SamplingProposal")
                {
                    DashboardViewItem DVavailabletest = ((DashboardView)Application.MainWindow.View).FindItem("AvailableTest") as DashboardViewItem;
                    DashboardViewItem DVBottleAllocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                    if (DVavailabletest != null && DVavailabletest.InnerView != null && DVBottleAllocation != null && DVBottleAllocation.InnerView != null && DVavailabletest.InnerView.SelectedObjects.Count > 0 && DVBottleAllocation.InnerView.SelectedObjects.Count > 0)
                    {
                        foreach (SamplingTest objsmppara in DVavailabletest.InnerView.SelectedObjects)
                        {
                            foreach (SampleBottleAllocation objsmplbottle in DVBottleAllocation.InnerView.SelectedObjects)
                            {
                                strsharedtest = objsmplbottle.SharedTests;
                                objsmplbottle.SharedTests = strsharedtest + ", " + objsmppara.Testparameter.TestMethod.TestName;
                                DVBottleAllocation.InnerView.ObjectSpace.CommitChanges();
                                DVBottleAllocation.InnerView.ObjectSpace.Refresh();
                            }
                        }
                        TaskRegistrationAvailabletests();
                        DashboardViewItem lvBottleAllocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                        DashboardViewItem lvavailabletest = ((DashboardView)Application.MainWindow.View).FindItem("AvailableTest") as DashboardViewItem;
                        if (lvBottleAllocation != null && lvBottleAllocation.InnerView != null)
                        {
                            ((ListView)lvBottleAllocation.InnerView).Refresh();
                        }
                        if (lvavailabletest != null && lvavailabletest.InnerView != null)
                        {
                            ((ListView)lvavailabletest.InnerView).Refresh();
                        }
                        Application.ShowViewStrategy.ShowMessage("Test added successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else if (DVavailabletest.InnerView.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select available test checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                    else if (DVBottleAllocation.InnerView.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select bottle allocation checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
                else if (Application.MainWindow.View.Id == "COCSettings")
                {
                    DashboardViewItem DVavailabletest = ((DashboardView)Application.MainWindow.View).FindItem("AvailableTest") as DashboardViewItem;
                    DashboardViewItem DVBottleAllocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                    if (DVavailabletest != null && DVavailabletest.InnerView != null && DVBottleAllocation != null && DVBottleAllocation.InnerView != null && DVavailabletest.InnerView.SelectedObjects.Count > 0 && DVBottleAllocation.InnerView.SelectedObjects.Count > 0)
                    {
                        foreach (COCSettingsTest objsmppara in DVavailabletest.InnerView.SelectedObjects)
                        {
                            foreach (SampleBottleAllocation objsmplbottle in DVBottleAllocation.InnerView.SelectedObjects)
                            {
                                strsharedtest = objsmplbottle.SharedTests;
                                objsmplbottle.SharedTests = strsharedtest + ", " + objsmppara.Testparameter.TestMethod.TestName;
                                DVBottleAllocation.InnerView.ObjectSpace.CommitChanges();
                                DVBottleAllocation.InnerView.ObjectSpace.Refresh();
                            }
                        }
                        COCSettingsAvailabletests();
                        Application.ShowViewStrategy.ShowMessage("Test added successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else if (DVavailabletest.InnerView.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select available test checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                    else if (DVBottleAllocation.InnerView.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select bottle allocation checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RemoveTests_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (Application.MainWindow.View.Id == "SampleRegistration")
                {
                    DashboardViewItem DVBottleAllocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                    if (DVBottleAllocation != null && DVBottleAllocation.InnerView != null && DVBottleAllocation.InnerView.SelectedObjects.Count == 1)
                    {

                    }
                    else if (DVBottleAllocation.InnerView.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select bottle allocation checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                    else if (DVBottleAllocation.InnerView.SelectedObjects.Count > 1)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select only one bottle allocation checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
                else if (Application.MainWindow.View.Id == "SamplingProposal")
                {
                    DashboardViewItem DVBottleAllocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                    if (DVBottleAllocation != null && DVBottleAllocation.InnerView != null && DVBottleAllocation.InnerView.SelectedObjects.Count == 1)
                    {

                    }
                    else if (DVBottleAllocation.InnerView.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select bottle allocation checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                    else if (DVBottleAllocation.InnerView.SelectedObjects.Count > 1)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select only one bottle allocation checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
                else if (Application.MainWindow.View.Id == "COCSettings")
                {
                    DashboardViewItem DVBottleAllocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                    if (DVBottleAllocation != null && DVBottleAllocation.InnerView != null && DVBottleAllocation.InnerView.SelectedObjects.Count == 1)
                    {

                    }
                    else if(DVBottleAllocation.InnerView.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select bottle allocation checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                    else if(DVBottleAllocation.InnerView.SelectedObjects.Count > 1)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select only one bottle allocation checkbox", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void TaskRegistrationAvailabletests()
        {
            List<Guid> lsttestparaguid = new List<Guid>();
            Sampling objsampling = ObjectSpace.FindObject<Sampling>(CriteriaOperator.Parse("[Oid] = ?", SInfo.SamplingGuid));
            DashboardViewItem DVAvailableTest = ((DashboardView)Application.MainWindow.View).FindItem("AvailableTest") as DashboardViewItem;
            IList<SamplingTest> objsmpltest = ObjectSpace.GetObjects<SamplingTest>(CriteriaOperator.Parse("[Sampling] = ?", objsampling.Oid)).ToList();
            IList<SampleBottleAllocation> lstsmplallocation = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[TaskRegistration.Oid] = ?", objsampling.Oid));
            foreach (SampleBottleAllocation objsmpl in lstsmplallocation.ToList())
            {
                if (!string.IsNullOrEmpty(objsmpl.SharedTests))
                {
                    string[] arrParent = objsmpl.SharedTests.Split(',');
                    foreach (var strtst in arrParent.ToList())
                    {
                        //SamplingTest objsmptst = ObjectSpace.FindObject<SamplingTest>(CriteriaOperator.Parse("[Testparameter.TestMethod.TestName] = ? And [Sampling.Oid] = ?", strtst, objsampling.Oid));

                        foreach (SamplingTest objsmpltes in objsmpltest.Cast<SamplingTest>().Where(i => i.Testparameter.TestMethod.TestName == strtst.Trim()).ToList())
                        {
                            if (!lsttestparaguid.Contains(objsmpltes.Oid))
                            {
                                lsttestparaguid.Add(objsmpltes.Oid);
                            }
                        }
                    }
                }
            }
            if (lsttestparaguid != null && lsttestparaguid.Count > 0)
            {
                ((ListView)DVAvailableTest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", lsttestparaguid.Select(i => i.ToString().Replace("'", "''")))) + ") And [Sampling.Oid] = ?", objsampling.Oid);
            }
            else
            {
                ((ListView)DVAvailableTest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Sampling.Oid] = ?", objsampling.Oid);
            }
        }

        private void SampleRegistrationAvailabletests()
        {
            List<Guid> lsttestparaguid = new List<Guid>();
            Samplecheckin objsamplecheckin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("JobID=" + SRInfo.strJobID));
            Modules.BusinessObjects.SampleManagement.SampleLogIn objsampling = ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("JobID=?", objsamplecheckin.Oid));
            DashboardViewItem DVAvailableTest = ((DashboardView)Application.MainWindow.View).FindItem("AvailableTest") as DashboardViewItem;
            IList<SampleParameter> objsmpltest = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Samplelogin.Oid=?", objsampling.Oid)).ToList();
            IList<SampleBottleAllocation> lstsmplallocation = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid] = ?", objsampling.Oid));
            foreach (SampleBottleAllocation objsmpl in lstsmplallocation.ToList())
            {
                if (!string.IsNullOrEmpty(objsmpl.SharedTests))
                {
                    string[] arrParent = objsmpl.SharedTests.Split(',');
                    foreach (var strtst in arrParent.ToList())
                    {
                        //SamplingTest objsmptst = ObjectSpace.FindObject<SamplingTest>(CriteriaOperator.Parse("[Testparameter.TestMethod.TestName] = ? And [Sampling.Oid] = ?", strtst, objsampling.Oid));

                        foreach (SampleParameter objsmpltes in objsmpltest.Cast<SampleParameter>().Where(i => i.Testparameter.TestMethod.TestName == strtst).ToList())
                        {
                            if (!lsttestparaguid.Contains(objsmpltes.Oid))
                            {
                                lsttestparaguid.Add(objsmpltes.Oid);
                            }
                        }
                    }
                }
            }
            if (lsttestparaguid != null && lsttestparaguid.Count > 0)
            {
                ((ListView)DVAvailableTest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", lsttestparaguid.Select(i => i.ToString().Replace("'", "''")))) + ") And [Samplelogin.Oid] = ?", objsampling.Oid);
            }
            else
            {
                ((ListView)DVAvailableTest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Samplelogin.Oid] = ?", objsampling.Oid);
            }
        }

        private void COCSettingsAvailabletests()
        {
            List<Guid> lsttestparaguid = new List<Guid>();
            COCSettings objcocid = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.COCOid));
            COCSettingsSamples objsampling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[COC.Oid] = ?", objcocid.Oid));
            DashboardViewItem DVAvailableTest = ((DashboardView)Application.MainWindow.View).FindItem("AvailableTest") as DashboardViewItem;
            IList<COCSettingsTest> objsmpltest = ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid] = ?", objsampling.Oid)).ToList();
            IList<SampleBottleAllocation> lstsmplallocation = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", objsampling.Oid));
            foreach (SampleBottleAllocation objsmpl in lstsmplallocation.ToList())
            {
                if (!string.IsNullOrEmpty(objsmpl.SharedTests))
                {
                    string[] arrParent = objsmpl.SharedTests.Split(',');
                    foreach (var strtst in arrParent.ToList())
                    {
                        //SamplingTest objsmptst = ObjectSpace.FindObject<SamplingTest>(CriteriaOperator.Parse("[Testparameter.TestMethod.TestName] = ? And [Sampling.Oid] = ?", strtst, objsampling.Oid));

                        foreach (COCSettingsTest objsmpltes in objsmpltest.Cast<COCSettingsTest>().Where(i => i.Testparameter.TestMethod.TestName == strtst).ToList())
                        {
                            if (!lsttestparaguid.Contains(objsmpltes.Oid))
                            {
                                lsttestparaguid.Add(objsmpltes.Oid);
                            }
                        }
                    }
                }
            }
            if (lsttestparaguid != null && lsttestparaguid.Count > 0)
            {
                ((ListView)DVAvailableTest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", lsttestparaguid.Select(i => i.ToString().Replace("'", "''")))) + ") And [Samples.Oid] = ?", objsampling.Oid);
            }
            else
            {
                ((ListView)DVAvailableTest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Samples.Oid] = ?", objsampling.Oid);
            }
        }
    }
}
