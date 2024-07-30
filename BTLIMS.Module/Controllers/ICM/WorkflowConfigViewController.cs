using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class WorkflowConfigViewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        ModificationsController mdcSave;
        int levelnumber;
        #endregion

        #region Constructor
        public WorkflowConfigViewController()
        {
            InitializeComponent();
            TargetViewId = "WorkflowConfig_DetailView;" + "OrderingItemSetup_ListView;";
        }
        #endregion

        #region DefaultEvents
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                mdcSave = Frame.GetController<ModificationsController>();
                mdcSave.SaveAction.Executed += SaveAction_Executed;
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged); ;
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
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            mdcSave = Frame.GetController<ModificationsController>();
            mdcSave.SaveAction.Executed -= SaveAction_Executed;
        }
        #endregion

        #region Events
        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "WorkflowConfig_DetailView")
                {
                    WorkflowConfig objWorkflow = (WorkflowConfig)View.CurrentObject;
                    IObjectSpace objectspace = Application.CreateObjectSpace(typeof(WorkflowConfig));
                    CriteriaOperator criteriaWorkFlow = CriteriaOperator.Parse("GCRecord IS Null");
                    IList<WorkflowConfig> objWorkFlow = objectspace.GetObjects<WorkflowConfig>(criteriaWorkFlow);
                    foreach (WorkflowConfig item in objWorkFlow)
                    {
                        if (item.ActivationOn == false)
                        {
                            item.NextLevel = 0;
                            objectspace.CommitChanges();
                        }
                        else
                        {
                            //CriteriaOperator criteria = CriteriaOperator.Parse("GCRecord IS Null and Level<" + objWorkflow.Level + " and ActivationOn = True");
                            CriteriaOperator criteriaLevel = CriteriaOperator.Parse("GCRecord IS Null and Level>" + item.Level + " and ActivationOn = True");
                            List<SortProperty> sorting = new List<SortProperty>();
                            sorting.Add(new SortProperty("Level", DevExpress.Xpo.DB.SortingDirection.Ascending));
                            CollectionSource objcollectionLevel = new CollectionSource(objectspace, typeof(WorkflowConfig));
                            objcollectionLevel.Criteria["flowcriteria"] = criteriaLevel;
                            objcollectionLevel.Sorting = sorting;
                            if (objcollectionLevel.List.Count > 0)
                            {
                                foreach (WorkflowConfig collection in objcollectionLevel.List)
                                {
                                    item.NextLevel = collection.Level;
                                    objectspace.CommitChanges();
                                    break;
                                }
                            }
                            else
                            {
                                item.NextLevel = 0;
                                objectspace.CommitChanges();
                            }

                        }
                    }
                    ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "WorkflowConfig_DetailView")
                {
                    if (e.PropertyName == "ActivationOn" || e.PropertyName == "ActivationOff")
                    {
                        WorkflowConfig objWorkflow = (WorkflowConfig)e.Object;
                        if (e.PropertyName == "ActivationOn")
                        {
                            if (objWorkflow.ActivationOn == true)
                            {
                                objWorkflow.ActivationOff = false;
                            }
                        }
                        if (e.PropertyName == "ActivationOff")
                        {
                            if (objWorkflow.ActivationOff == true)
                            {
                                objWorkflow.ActivationOn = false;

                            }
                        }
                    }
                }
                //if (View != null & View.Id == "OrderingItemSetup_ListView")
                //{

                //    if (e.PropertyName == "OrderingItemOn" || e.PropertyName == "OrderingItemoff")
                //    {
                //        OrderingItemSetup objWorkflow = (OrderingItemSetup)e.Object;
                //        if (e.PropertyName == "OrderingItemOn")
                //        {
                //            if (objWorkflow.OrderingItemon == true)
                //            {
                //                objWorkflow.OrderingItemoff = false;
                //            }
                //        }
                //        if (e.PropertyName == "OrderingItemoff")
                //        {
                //            if (objWorkflow.OrderingItemoff == true)
                //            {
                //                objWorkflow.OrderingItemon = false;

                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        #endregion
    }
}
