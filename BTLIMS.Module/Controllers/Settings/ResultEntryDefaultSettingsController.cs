using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;

namespace LDM.Module.Controllers.Settings
{
    public partial class ResultEntryDefaultSettingsController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public ResultEntryDefaultSettingsController()
        {
            InitializeComponent();
            TargetViewId = "ResultEntryDefaultSettings_DetailView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ModificationsController modificationcontroller = Frame.GetController<ModificationsController>();
                if (modificationcontroller != null)
                {
                    //modificationcontroller .SaveAction.Executed += SaveAction_Executed;
                    modificationcontroller .SaveAction.Executing += SaveAction_Executing;
                    //modificationcontroller.SaveAndCloseAction.Executed += SaveAction_Executed;
                    modificationcontroller.SaveAndCloseAction.Executing += SaveAction_Executing;
                }
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                NewObjectViewController newController = Frame.GetController<NewObjectViewController>();
                newController.NewObjectAction.Active["RemoveNewButton"] = false;
                DeleteObjectsViewController deleteObjectsView = Frame.GetController<DeleteObjectsViewController>();
                deleteObjectsView.DeleteAction.Active["RemoveDeleteButton"] = false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject != null && View.ObjectTypeInfo.Type == typeof(ResultEntryDefaultSettings))
                {
                    ResultEntryDefaultSettings objResult = (ResultEntryDefaultSettings)View.CurrentObject;
                    IObjectSpace os = View.ObjectSpace;
                    DefaultSetting defaultSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));

                    Modules.BusinessObjects.Setting.DefaultSetting RVdefsetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Validation' And [GCRecord] is Null"));
                    Modules.BusinessObjects.Setting.DefaultSetting RAdefsetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Approval' And [GCRecord] is Null"));
                    DefaultSetting SDMSLeve2defsetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'RawDataLevel2BatchReview' And [GCRecord] is Null"));
                    DefaultSetting SDMSLeve3defsetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'RawDataLevel3BatchReview' And [GCRecord] is Null"));
                    DefaultSetting objQuoteReviewDefSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Quotes' And [NavigationItemNameID] = 'QuotesReview' And [GCRecord] is Null"));
                    if (defaultSetting != null)
                    {
                        if (objResult.SDMS == ResultEntryDefaultSettings.YesNoFilter.Yes)
                        {
                            defaultSetting.Select = true;
                        }
                        else
                        {
                            defaultSetting.Select = false;
                        }
                        if (objResult.Leve2DataReview == ResultEntryDefaultSettings.YesNoFilter.No)
                        {
                            defaultSetting.Review = EnumRELevelSetup.No;
                            defaultSetting.Verify = EnumRELevelSetup.No;
                            SDMSLeve2defsetting.Select = false;
                            SDMSLeve3defsetting.Select = false;
                        }
                        else if (objResult.Leve3DataReview == ResultEntryDefaultSettings.YesNoFilter.No)
                        {
                            defaultSetting.Verify = EnumRELevelSetup.No;
                            SDMSLeve3defsetting.Select = false;
                        }
                        if (objResult.Leve3DataReview == ResultEntryDefaultSettings.YesNoFilter.Yes)
                        {
                            defaultSetting.Review = EnumRELevelSetup.Yes;
                            defaultSetting.Verify = EnumRELevelSetup.Yes;
                            SDMSLeve2defsetting.Select = true;
                            SDMSLeve3defsetting.Select = true;
                        }
                        else if (objResult.Leve2DataReview == ResultEntryDefaultSettings.YesNoFilter.Yes)
                        {
                            defaultSetting.Review = EnumRELevelSetup.Yes;
                            SDMSLeve2defsetting.Select = true;
                        }

                        if (RVdefsetting != null)
                        {
                            if (objResult.ResultValidation == ResultEntryDefaultSettings.YesNoFilter.No)
                            {
                                RVdefsetting.Select = false;
                                defaultSetting.REValidate = EnumRELevelSetup.No;
                            }
                            else if (objResult.ResultValidation == ResultEntryDefaultSettings.YesNoFilter.Yes)
                            {
                                RVdefsetting.Select = true;
                                defaultSetting.REValidate = EnumRELevelSetup.Yes;
                            }
                        }

                        if (RAdefsetting != null)
                        {
                            if (objResult.ResultApproval == ResultEntryDefaultSettings.YesNoFilter.No)
                            {
                                RAdefsetting.Select = false;
                                defaultSetting.REApprove = EnumRELevelSetup.No;
                            }
                            else if (objResult.ResultApproval == ResultEntryDefaultSettings.YesNoFilter.Yes)
                            {
                                RAdefsetting.Select = true;
                                defaultSetting.REApprove = EnumRELevelSetup.Yes;
                            }
                        }
                    }
                        if (objQuoteReviewDefSetting != null)
                        {
                            if (objResult.QuoteReview == ResultEntryDefaultSettings.YesNoFilter.No)
                            {
                                objQuoteReviewDefSetting.Select = false;
                            }
                            else
                            {
                                objQuoteReviewDefSetting.Select = true;
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

        //private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        //{
        //    try
        //    {
        //        if (View != null && View.CurrentObject != null && View.ObjectTypeInfo.Type == typeof(ResultEntryDefaultSettings))
        //        {
        //            ResultEntryDefaultSettings objResult =(ResultEntryDefaultSettings)View.CurrentObject; 
        //            IObjectSpace os=Application.CreateObjectSpace();
        //            DefaultSetting defaultSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
               
        //            Modules.BusinessObjects.Setting.DefaultSetting RVdefsetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Validation' And [GCRecord] is Null"));
        //            Modules.BusinessObjects.Setting.DefaultSetting RAdefsetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Approval' And [GCRecord] is Null"));
        //            DefaultSetting SDMSLeve2defsetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'RawDataLevel2BatchReview' And [GCRecord] is Null"));
        //            DefaultSetting SDMSLeve3defsetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'RawDataLevel3BatchReview' And [GCRecord] is Null"));
        //            DefaultSetting objQuoteReviewDefSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Quotes' And [NavigationItemNameID] = 'QuotesReview' And [GCRecord] is Null"));
        //            if (defaultSetting != null)
        //            {
        //                if (objResult.Leve2DataReview == ResultEntryDefaultSettings.YesNoFilter.No)
        //                {
        //                    defaultSetting.Review = EnumRELevelSetup.No;
        //                    defaultSetting.Verify = EnumRELevelSetup.No;
        //                    SDMSLeve2defsetting.Select = false;
        //                    SDMSLeve3defsetting.Select = false;
        //                }
        //                else if (objResult.Leve3DataReview == ResultEntryDefaultSettings.YesNoFilter.No)
        //                {
        //                    defaultSetting.Verify = EnumRELevelSetup.No;
        //                    SDMSLeve3defsetting.Select = false;
        //                }
        //                if (objResult.Leve3DataReview == ResultEntryDefaultSettings.YesNoFilter.Yes)
        //                {
        //                    defaultSetting.Review = EnumRELevelSetup.Yes;
        //                    defaultSetting.Verify = EnumRELevelSetup.Yes;
        //                    SDMSLeve2defsetting.Select = true;
        //                    SDMSLeve3defsetting.Select = true;
        //                }
        //                else if (objResult.Leve2DataReview == ResultEntryDefaultSettings.YesNoFilter.Yes)
        //                {
        //                    defaultSetting.Review = EnumRELevelSetup.Yes;
        //                    SDMSLeve2defsetting.Select = true;
        //                }

        //                if (RVdefsetting != null)
        //                {
        //                    if (objResult.ResultValidation == ResultEntryDefaultSettings.YesNoFilter.No)
        //                    {
        //                        RVdefsetting.Select = false;
        //                        defaultSetting.REValidate = EnumRELevelSetup.No;
        //                    }
        //                    else if (objResult.ResultValidation == ResultEntryDefaultSettings.YesNoFilter.Yes)
        //                    {
        //                        RVdefsetting.Select = true;
        //                        defaultSetting.REValidate = EnumRELevelSetup.Yes;
        //                    }
        //                }

        //                if (RAdefsetting != null)
        //                {
        //                    if (objResult.ResultApproval  == ResultEntryDefaultSettings.YesNoFilter.No)
        //                    {
        //                        RAdefsetting .Select = false;
        //                        defaultSetting.REApprove = EnumRELevelSetup.No;
        //                    }
        //                    else if (objResult.ResultApproval == ResultEntryDefaultSettings.YesNoFilter.Yes)
        //                    {
        //                        RAdefsetting.Select = true;
        //                        defaultSetting.REApprove = EnumRELevelSetup.Yes;
        //                    }
        //                }
        //            }
        //            if (objQuoteReviewDefSetting != null)
        //            {
        //                if (objResult.QuoteReview == ResultEntryDefaultSettings.YesNoFilter.No)
        //                {
        //                    objQuoteReviewDefSetting.Select = false;
        //                }
        //                else
        //                {
        //                    objQuoteReviewDefSetting.Select = true;
        //                }
        //            }
        //            os.CommitChanges();
                
        //            Application.ShowViewStrategy.ShowMessage("Saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (e.Object != null && e.PropertyName != null && e.Object.GetType() == typeof(ResultEntryDefaultSettings))
                {
                    ResultEntryDefaultSettings objResult = (ResultEntryDefaultSettings)e.Object;
                    //if (e.PropertyName == "ResultValidation" && e.NewValue.ToString() == "No")
                    //{
                    //    objResult.ResultApproval = (ResultEntryDefaultSettings.YesNoFilter)YesNoFilter.No;
                    //}
                    /*else*/ 
                    if (e.PropertyName == "Leve2DataReview" && e.NewValue.ToString() == "No")
                    {
                        objResult.Leve3DataReview = (ResultEntryDefaultSettings.YesNoFilter)YesNoFilter.No;
                    }
                    else if (e.PropertyName == "SDMS" && e.NewValue.ToString() == "No")
                    {
                    objResult.Leve2DataReview = (ResultEntryDefaultSettings.YesNoFilter)YesNoFilter.No;
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
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            try
            {
                ModificationsController modificationcontroller = Frame.GetController<ModificationsController>();
                if (modificationcontroller != null)
                {
                    //modificationcontroller.SaveAction.Executed -= SaveAction_Executed;
                    modificationcontroller.SaveAction.Executing -= SaveAction_Executing;
                    //modificationcontroller.SaveAndCloseAction.Executed -= SaveAction_Executed;
                    modificationcontroller.SaveAndCloseAction.Executing -= SaveAction_Executing;
                }
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                NewObjectViewController newController = Frame.GetController<NewObjectViewController>();
                newController.NewObjectAction.Active.RemoveItem("RemoveNewButton");
                DeleteObjectsViewController deleteObjectsView = Frame.GetController<DeleteObjectsViewController>();
                deleteObjectsView.DeleteAction.Active.RemoveItem("RemoveDeleteButton");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
