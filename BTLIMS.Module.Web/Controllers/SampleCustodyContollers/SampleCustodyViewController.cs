using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement.SampleCustody;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Web.Controllers._SampleCustody
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.

    public partial class SampleCustodyViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        List<Guid> NeedToShowObjects = new List<Guid>();
        ModificationsController ModificationController;
        #region Constructor
        public SampleCustodyViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "SampleCustody_ListView;" + "SampleCustody_DetailView;" + "SampleCustody_ListView_Copy_SampleIn;" + "SampleCustody_ListView_Copy_SampleOut;" + "SampleCustody_ListView_Copy_SampleLocations;" + "SampleCustody_ListView_Copy_SampleDisposal;" +
                "SampleCustody_DetailView_Copy_SampleIn;" + "SampleCustody_DetailView_Copy_SampleOut;" + "SampleCustody_DetailView_Copy_SampleDisposal;";
        }
        #endregion
        #region DefaultEvents
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ObjectSpace.Committed += ObjectSpace_Committed;
            ObjectSpace.Committing += ObjectSpace_Committing;
            RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
            Frame.GetController<ShowNavigationItemController>().CustomShowNavigationItem += new EventHandler<CustomShowNavigationItemEventArgs>(ViewController1_CustomShowNavigationItem);
            ModificationController = Frame.GetController<ModificationsController>();
            if (ModificationController != null)
            {
                ModificationController.SaveAndNewAction.Executing += SaveAndNewAction_Executing;
            }
        }

        private void SaveAndNewAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null && (View.Id == "SampleCustody_DetailView_Copy_SampleIn" || View.Id == "SampleCustody_DetailView_Copy_SampleOut" || View.Id == "SampleCustody_DetailView_Copy_SampleDisposal"))
                {
                    SampleCustody obj = (SampleCustody)View.CurrentObject;
                    if (obj.ErrorDescription != null && obj.ErrorDescription != string.Empty)
                    {
                        e.Cancel = true;
                        //throw new UserFriendlyException("Not Saved Read Error Description");
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "readerror"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    if (obj.GivenTo == null && obj.ToStorage == null)
                    {
                        e.Cancel = true;
                        //throw new UserFriendlyException("Not Saved Read Error Description");
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "samplenotsaved"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    obj.LastUpdatedDate = DateTime.Now;
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
                if (View is DetailView && View.Id == "SampleCustody_DetailView_Copy_SampleIn")
                {
                    DashboardViewItem dvSample = ((DetailView)View).FindItem("SavedSampleIn") as DashboardViewItem;
                    if (dvSample != null && dvSample.InnerView != null)
                    {
                        ((ListView)dvSample.InnerView).CollectionSource.Criteria["Filter"] = GroupOperator.And(new BinaryOperator("Mode", 1), new InOperator("Oid", NeedToShowObjects));
                    }
                }
                else if (View is DetailView && View.Id == "SampleCustody_DetailView_Copy_SampleOut")
                {
                    DashboardViewItem dvSample = ((DetailView)View).FindItem("SavedSampleOut") as DashboardViewItem;
                    if (dvSample != null && dvSample.InnerView != null)
                    {
                        ((ListView)dvSample.InnerView).CollectionSource.Criteria["Filter"] = GroupOperator.And(new BinaryOperator("Mode", 2), new InOperator("Oid", NeedToShowObjects));
                    }
                }
                else if (View is ListView && View.Id == "SampleCustody_ListView_Copy_SampleDisposal")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[DisposedDate] IS NOT NULL AND [DisposedBy] IS NOT NULL"); //GroupOperator.And(new BinaryOperator("DisposedDate", 2), new InOperator("Oid", NeedToShowObjects));
                }
                else if (View.Id == "SampleCustody_ListView_Copy_SampleLocations")
                {
                    IObjectSpace objectspace = Application.CreateObjectSpace();
                    List<Guid> UniqueObjects = new List<Guid>();
                    foreach (SampleCustody obj in ((ListView)View).CollectionSource.List)
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("[SampleBottleID]='" + obj.SampleBottleID + "'");// AND Max(LastUpdatedDate)");                                                                                                                    // bool exists = Convert.ToBoolean(ObjectSpace.Evaluate(typeof(SampleCustody), (new AggregateOperand("", Aggregate.Exists)), (criteria)));
                        IList<SampleCustody> ListObject = ObjectSpace.GetObjects<SampleCustody>(criteria);
                        if (ListObject.Count > 1)
                        {
                            string maxDateTime = ListObject.Max(x => x.LastUpdatedDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[SampleBottleID]='" + obj.SampleBottleID + "' AND [LastUpdatedDate]='" + maxDateTime + "'");
                            SampleCustody MaxObject = objectspace.FindObject<SampleCustody>(criteria1, true);
                            if (MaxObject != null)
                            {
                                if (!UniqueObjects.Contains(MaxObject.Oid))
                                    UniqueObjects.Add(MaxObject.Oid);
                            }
                            else
                            {
                                if (!UniqueObjects.Contains(MaxObject.Oid))
                                    UniqueObjects.Add(obj.Oid);
                            }
                        }
                        else
                        {
                            if (!UniqueObjects.Contains(obj.Oid))
                                UniqueObjects.Add(obj.Oid);
                        }
                    }
                    ((ListView)View).CollectionSource.Criteria["filter1"] = new InOperator("Oid", UniqueObjects);
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
        #endregion
        //this method is used for clear the ListObjects when click the Navigation Item
        void ViewController1_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            try
            {
                if (e.ActionArguments.SelectedChoiceActionItem.Id == "SampleIn" || e.ActionArguments.SelectedChoiceActionItem.Id == "SampleOut")
                {
                    NeedToShowObjects.Clear();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        //this method is used for Check the Rule only the Visible Properties in a View
        private void RuleSet_CustomNeedToValidateRule(object sender, CustomNeedToValidateRuleEventArgs e)
        {
            try
            {
                if (View != null && View is DetailView && (View.Id == "SampleCustody_DetailView_Copy_SampleIn" || View.Id == "SampleCustody_DetailView_Copy_SampleOut" || View.Id == "SampleCustody_DetailView_Copy_SampleDisposal"))
                {
                    DetailView dv = View as DetailView;
                    RuleBase ruleBase = e.Rule as RuleBase;
                    if (dv != null && ruleBase != null && dv.ObjectTypeInfo.Type == ruleBase.Properties.TargetType)
                    {
                        List<string> visibleProperties = new List<string>();
                        foreach (PropertyEditor editor in dv.GetItems<PropertyEditor>())
                        {
                            if (editor.Control != null && ((System.Web.UI.Control)editor.Control).Visible)
                            {
                                visibleProperties.Add(editor.PropertyName);
                            }
                        }
                        foreach (string propertyToBeValidated in e.Rule.UsedProperties)
                        {
                            if (!visibleProperties.Contains(propertyToBeValidated))
                            {
                                e.NeedToValidateRule = false;
                                e.NeedToValidateRule = false;
                                e.Handled = true;
                                break;
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
        #region ObjectSpace Events
        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null && View.Id == "SampleCustody_DetailView_Copy_SampleDisposal")
                {
                    SampleCustody obj = (SampleCustody)View.CurrentObject;
                    obj.Where = "Disposed";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                if (View.CurrentObject != null)
                {
                    SampleCustody obj = (SampleCustody)View.CurrentObject;
                    NeedToShowObjects.Add(obj.Oid);
                    OnViewControlsCreated();
                }
                if (View.Id == "SampleCustody_DetailView_Copy_SampleIn" || View.Id == "SampleCustody_DetailView_Copy_SampleOut")
                {
                    if ((Frame.View is DetailView) && (((DetailView)Frame.View).ObjectTypeInfo.Type == typeof(SampleCustody)))
                    {
                        Object newObject = ObjectSpace.CreateObject(typeof(SampleCustody));
                        Frame.GetController<ModificationsController>().Active["AssignNewObject"] = false;
                        View.CurrentObject = newObject;
                        Frame.GetController<ModificationsController>().Active["AssignNewObject"] = true;
                    }
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
                if (e.PropertyName == "SampleBottleID")
                {
                    SampleCustody obj = (SampleCustody)e.Object;
                    if (obj.SampleBottleID != null)
                    {
                        if (CheckSampleID(obj))
                        {
                            if (View.Id == "SampleCustody_DetailView_Copy_SampleIn")
                            {
                                obj.Mode = SampleCustodyMode.In;
                                obj.FromEmployee = getFromEmployee(obj);
                                obj.FromStorage = getFromStorage(obj);
                                obj.DateHandled = DateTime.Now;
                                obj.GivenTo = getGivenTo(obj);
                                obj.ToStorage = getToStorage(obj);
                            }
                            else if (View.Id == "SampleCustody_DetailView_Copy_SampleOut")
                            {
                                obj.Mode = SampleCustodyMode.Out;
                                obj.FromEmployee = getFromEmployee(obj);
                                obj.FromStorage = getFromStorage(obj);
                                obj.DateHandled = DateTime.Now;
                                obj.GivenTo = getGivenTo(obj); ;
                                obj.ToStorage = getToStorage(obj);
                            }
                            else if (View.Id == "SampleCustody_DetailView_Copy_SampleDisposal")
                            {
                                obj.DisposedDate = DateTime.Now;
                                obj.DisposedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            }
                            /*==> Note: View.CurrentObject ==e.Object-->this conditon for check the object integrity
                              because we can change the Current Object at RunTime in ObjectSpace.Commited Event
                              so we must check this condition otherwise it will throw error .*/
                            if ((obj.GivenTo != null || obj.ToStorage != null) && View.CurrentObject == e.Object)
                            {
                                ObjectSpace.CommitChanges();
                            }
                        }
                    }
                }
                if (e.PropertyName == "GivenTo" || e.PropertyName == "ToStorage")
                {
                    SampleCustody obj = (SampleCustody)e.Object;
                    if (obj.GivenTo != null && obj.ToStorage == null && e.OldValue != e.NewValue)
                    {
                        obj.Where = obj.GivenTo.FullName;
                    }
                    else if (obj.ToStorage != null && obj.GivenTo == null && e.OldValue != e.NewValue)
                    {
                        obj.Where = obj.ToStorage.StorageName;
                    }
                    if (obj.GivenTo != null || obj.ToStorage != null && e.OldValue != e.NewValue)
                    {
                        if (obj.SampleBottleID != null && View.CurrentObject == e.Object)
                        {
                            if (ObjectSpace.IsNewObject(View.CurrentObject))
                            {
                                View.ObjectSpace.CommitChanges();
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
        #endregion       
        #region Functions
        private SampleCustody GetLastSavedObject(SampleCustody obj)
        {
            try
            {
                //IObjectSpace objectspace = Application.CreateObjectSpace();
                CriteriaOperator criteria = CriteriaOperator.Parse("[SampleBottleID]='" + obj.SampleBottleID + "'");// AND Max(LastUpdatedDate)");                                                                                                                    // bool exists = Convert.ToBoolean(ObjectSpace.Evaluate(typeof(SampleCustody), (new AggregateOperand("", Aggregate.Exists)), (criteria)));
                IList<SampleCustody> ListObject = ObjectSpace.GetObjects<SampleCustody>(criteria);
                if (ListObject.Count > 0)
                {
                    string maxDateTime = ListObject.Max(x => x.LastUpdatedDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[SampleBottleID]='" + obj.SampleBottleID + "' AND [LastUpdatedDate]='" + maxDateTime + "'");
                    SampleCustody SavedObject = ObjectSpace.FindObject<SampleCustody>(criteria1, false);
                    return SavedObject;
                }
                return null;
                //return null;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private Employee getFromEmployee(SampleCustody obj)
        {
            try
            {
                SampleCustody SavedObject = GetLastSavedObject(obj);
                if (SavedObject != null)
                {
                    if (SavedObject.GivenTo != null)
                    {
                        return SavedObject.GivenTo;
                    }
                    else if (SavedObject.ToStorage != null)
                    {
                        return null;
                    }
                    else//(SavedObject.GivenTo == null && SavedObject.ToStorage == null)
                    {
                        return ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    }
                }
                else
                {
                    return ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private Storage getFromStorage(SampleCustody obj)
        {
            try
            {
                //IObjectSpace objectspace = Application.CreateObjectSpace(typeof(Storage));
                SampleCustody SavedObject = GetLastSavedObject(obj);
                if (SavedObject != null)
                {
                    if (SavedObject.ToStorage != null)
                    {
                        return SavedObject.ToStorage;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private Employee getGivenTo(SampleCustody obj)
        {
            try
            {
                IList<SampleCustody> lstSampleCustody = ObjectSpace.GetObjects<SampleCustody>(new InOperator("Oid", NeedToShowObjects));
                if (lstSampleCustody.Count > 0)
                {
                    string maxDateTime = lstSampleCustody.Max(x => x.LastUpdatedDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    //CriteriaOperator criteria1 = CriteriaOperator.Parse("[SampleBottleID]='" + obj.SampleBottleID + "' AND [LastUpdatedDate]='" + maxDateTime + "'");
                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[LastUpdatedDate]='" + maxDateTime + "'");
                    SampleCustody SavedObject = ObjectSpace.FindObject<SampleCustody>(criteria1, false);
                    //return SavedObject;
                    if (SavedObject != null)
                    {
                        if (SavedObject.GivenTo != null)
                        {
                            return SavedObject.GivenTo;
                        }
                        else if (SavedObject.ToStorage != null)
                        {
                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private Storage getToStorage(SampleCustody obj)
        {
            try
            {
                IList<SampleCustody> lstSampleCustody = ObjectSpace.GetObjects<SampleCustody>(new InOperator("Oid", NeedToShowObjects));
                if (lstSampleCustody.Count > 0)
                {
                    string maxDateTime = lstSampleCustody.Max(x => x.LastUpdatedDate).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[LastUpdatedDate]='" + maxDateTime + "'");
                    SampleCustody SavedObject = ObjectSpace.FindObject<SampleCustody>(criteria1, false);
                    if (SavedObject != null)
                    {
                        if (SavedObject.ToStorage != null)
                        {
                            return SavedObject.ToStorage;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private bool CheckSampleID(SampleCustody obj)
        {
            try
            {
                IObjectSpace objectspace = Application.CreateObjectSpace();
                //if (!obj.SampleBottleID.Contains("-"))
                //{
                //    SetErrorDescription(obj, "Require '-' in the Sample ID");
                //    return false;
                //}
                //else if (!obj.SampleBottleID.Contains(" "))
                //{
                //    SetErrorDescription(obj, "Require a {Space} in the Sample ID");
                //    return false;
                //}

                string[] strSampleSplit = obj.SampleBottleID.Split('-');
                if (strSampleSplit.Length == 2)
                {
                    string[] strBottleSplit = strSampleSplit[1].Split(' ');
                    if (strBottleSplit.Length == 2)
                    {
                        //string strJobID = string.Empty;
                        //int SampleNo = 0;
                        //string strBottleNo = string.Empty;
                        //if (strSampleSplit.Length > 1 && (strSampleSplit[0].Trim().Length > 0 && strSampleSplit[1].Trim().Length > 0))
                        //{
                        //strJobID = strSampleSplit[0];
                        //string[] strBottleSplit = strSampleSplit[1].Split(' ');
                        //SampleNo = Convert.ToInt32(strBottleSplit[0]);
                        //strBottleNo = strBottleSplit[1].ToString();
                        CriteriaOperator criteria = CriteriaOperator.Parse("[SampleLogIn.JobID.JobID]='" + strSampleSplit[0].Trim() + "' AND [SampleLogIn.SampleNo]='" + strBottleSplit[0].Trim() + "' AND [BottleID.BottleId]='" + strBottleSplit[1].Trim() + "'");
                        //SampleBottleTest SBTest = objectspace.FindObject<SampleBottleTest>(criteria, true);
                        //if (SBTest == null)
                        //{
                        //    SetErrorDescription(obj, "SampleID does not exist.");
                        //    return false;
                        //}
                        //}
                        //else
                        //{
                        //    SetErrorDescription(obj, "Not a Valid Sample ID");
                        //    return false;
                        //}
                    }
                    else
                    {
                        SetErrorDescription(obj, "SampleID is not valid, Format should be like [YYYYMMDDXXX-XX A].");
                    }
                }
                else
                {
                    SetErrorDescription(obj, "SampleID is not valid, Format should be like [YYYYMMDDXXX-XX A].");
                }

                SetErrorDescription(obj, string.Empty);
                return true;

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }
        private void SetErrorDescription(SampleCustody obj, string ErrorMsg)
        {
            try
            {
                obj.ErrorDescription = ErrorMsg;
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
