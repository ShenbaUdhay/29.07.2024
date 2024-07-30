using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.TestPricing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using static Modules.BusinessObjects.InfoClass.BottlesOrderInfo;
using Component = Modules.BusinessObjects.Setting.Component;

namespace LDM.Module.Controllers.Settings.TestPrice
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TestPriceViewController : ViewController, IXafCallbackHandler
    {
        #region Declaration
        bool Isparameter = false;
        QuotesInfo quotesinfo = new QuotesInfo();
        DataTable tembtable = new DataTable();
        MessageTimer timer = new MessageTimer();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        PricingInfo priceinfo = new PricingInfo();
        Testpricesurchargeinfo testpricesurinfo = new Testpricesurchargeinfo();
        GroupTestPricingInfo objGroupTestPricingInfo = new GroupTestPricingInfo();
        ConstituentInfo constituentInfo = new ConstituentInfo();
        bool Isseleted = false;
        bool isCustomFilter = false;
        #endregion
        #region Constructor
        public TestPriceViewController()
        {
            InitializeComponent();
            TargetViewId = "ConstituentPricing_DetailView;" + "ConstituentPricing_ListView;" + "TestPriceSurcharge_DetailView;" + "TestPriceSurcharge_ListView;" + "PretreatmentPricing_DetailView;"
                + "PretreatmentPricing_ListView;" + "GroupTestSurcharge_DetailView;" + "GroupTestSurcharge_ListView;" + "DefaultPricing_ListView;" + "DefaultPricing_ListView_Copy;"
                + "ConstituentPricingTier_ListView;" + "ItemChargePricing_ListView_Cancaled;" + "ItemChargePricing_DetailView;" + "TestPriceSurcharge_ListView_newlist;" + "TurnAroundTime_ListView_TestPriceSurcharge;"
                + "TestPriceSurcharge_ListView_Edit;" + "DefaultPricing_DetailView;" + "Priority_ListView;" + "Testparameter_LookupListView_ViewPopup;" + "ConstituentPricing_DetailView_Quotes;"
                + "ConstituentPricing_ConstituentPricingTiers_ListView;";
            PriceSaveAction.TargetViewId = "DefaultPricing_ListView_Copy;" + "TestPriceSurcharge_ListView_newlist;";
            Add_Btn.TargetViewId = "ConstituentPricingTier_ListView;" + "ConstituentPricing_ConstituentPricingTiers_ListView;";
            Remove_Btn.TargetViewId = "ConstituentPricingTier_ListView;" + "ConstituentPricing_ConstituentPricingTiers_ListView;";
            PriceCancelAction.TargetViewId = "TestPriceSurcharge_ListView_newlist;";
            Reactivate.TargetViewId = "ItemChargePricing_ListView_Cancaled;";

            //SimpleAction Reactivate = new SimpleAction(this, "ItemchargeReactivated", PredefinedCategory.View)
            //{
            //    Caption = "Reactivate",
            //    ConfirmationMessage = "Do you want to reactivate this Item Charge ?",
            //};
            //Reactivate.TargetViewId = "ItemChargePricing_ListView_Cancaled;";
            //Reactivate.ImageName = "Action_Workflow_Activate";
            //Reactivate.Execute += ReactivateRegistration_Execute;
            //Reactivate.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //Target required Views(via the TargetXXX properties) and create their Actions.
        }
        #endregion
        #region DefaultEvents
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;

                if (View.Id == "ItemChargePricing_ListView_Cancaled")
                {
                    Employee currentUser = SecuritySystem.CurrentUser as Employee;
                    if (currentUser != null && View != null && View.Id != null)
                    {
                        if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                        {
                            objPermissionInfo.CanceledItemChargeIsWrite = false;
                            if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                            {
                                objPermissionInfo.CanceledItemChargeIsWrite = true;
                            }
                            else
                            {
                                foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                                {
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "CanceledItemCharge" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.CanceledItemChargeIsWrite = true;
                                    }
                                }
                            }
                        }
                        if (View.Id == "ItemChargePricing_ListView_Cancaled")
                        {
                            Reactivate.Active["showReactivate"] = objPermissionInfo.CanceledItemChargeIsWrite;
                        }
                    }
                }

                if (View.Id == "ConstituentPricing_DetailView" || View.Id == "TestPriceSurcharge_DetailView" || View.Id == "DefaultPricing_DetailView")
                {
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated += ChangeLayoutGroupCaptionViewController_ItemCreated;
                }
                else if (View.Id == "PretreatmentPricing_DetailView" || View.Id == "PretreatmentPricing_ListView" || View.Id == "GroupTestSurcharge_DetailView" || View.Id == "GroupTestSurcharge_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executed += NewObjectAction_Executed;
                }
                if (View.Id == "TestPriceSurcharge_ListView" || View.Id == "TestPriceSurcharge_DetailView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Execute += NewObjectAction_Execute;
                    if (View.Id == "TestPriceSurcharge_ListView")
                    {
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>().FullTextFilterAction.Executing += FullTextFilterAction_Executing;
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>().CustomGetFullTextSearchProperties += new EventHandler<CustomGetFullTextSearchPropertiesEventArgs>(standardFilterController_CustomGetFullTextSearchProperties);
                    }
                }
                if (View.Id == "TestPriceSurcharge_ListView_newlist")
                {
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>().FullTextFilterAction.Executing += FullTextFilterAction_Executing;
                }
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                if (View is DetailView)
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed += SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executed += SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAction.Executed += SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Execute += SaveAndNewAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Execute += SaveAndCloseAction_Execute;
                }
                //Frame.GetController<NewObjectViewController>().NewObjectAction.Execute += NewObjectAction_Execute;
                if (View.Id == "PretreatmentPricing_DetailView" || View.Id == "TestPriceSurcharge_ListView" || View.Id == "PretreatmentPricing_ListView")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                }
                else if (View.Id == "DefaultPricing_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Execute += NewObjectAction_Execute;
                }
                ////if (View.Id == "TestPriceSurcharge_ListView_newlist")
                ////{
                ////    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                ////    List<TestPriceSurcharge> lsttestpricingsurcharge = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[GCRecord] is Null")).ToList();
                ////    List<Modules.BusinessObjects.Setting.Priority> lstPriority = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Priority>(CriteriaOperator.Parse("")).ToList();
                ////    List<DefaultPricing> lstDefaultPricing = ObjectSpace.GetObjects<DefaultPricing>(CriteriaOperator.Parse("")).ToList();
                ////    List<ConstituentPricing> lstConstituentPricing = ObjectSpace.GetObjects<ConstituentPricing>(CriteriaOperator.Parse("")).ToList();
                ////    List<TestMethod> lstTestmed = new List<TestMethod>();
                ////    if (lstDefaultPricing != null && lstDefaultPricing.Count > 0)
                ////    {
                ////        foreach (DefaultPricing objdefpri in lstDefaultPricing.ToList())
                ////        {
                ////            if (!lstTestmed.Contains(objdefpri.Test))
                ////            {
                ////                lstTestmed.Add(objdefpri.Test);
                ////            }
                ////        }
                ////    }
                ////    if (lstConstituentPricing != null && lstConstituentPricing.Count > 0)
                ////    {
                ////        foreach (ConstituentPricing objconstituepri in lstConstituentPricing.ToList())
                ////        {
                ////            if (!lstTestmed.Contains(objconstituepri.Test))
                ////            {
                ////                lstTestmed.Add(objconstituepri.Test);
                ////            }
                ////        }
                ////    }
                ////    foreach (Modules.BusinessObjects.Setting.Priority objpriority in lstPriority.ToList())
                ////    {
                ////        string strpricecode = string.Empty;
                ////        foreach (TestMethod objTM1 in lstTestmed)
                ////        {
                ////            if (objTM1.IsGroup != true && objTM1 != null && objTM1.MatrixName != null && objTM1.TestName != null && objTM1.MethodName != null)
                ////            {
                ////                List<Component> lstCom = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod] = ?", objTM1.Oid)).ToList();
                ////                if (lstCom.Count > 0)
                ////                {
                ////                    bool Default = true;
                ////                    if (Default == true)
                ////                    {
                ////                        Component objComDefault = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                ////                        TestPriceSurcharge objDPCur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.Oid] = ? And [Test.Oid] = ? And [Method.MethodName.Oid] = ? And [Component.Oid] = ? And [Priority.Oid] = ? And Not IsNullOrEmpty([TAT])", objTM1.MatrixName.Oid, objTM1.Oid, objTM1.MethodName.Oid, objComDefault.Oid, objpriority.Oid));
                ////                        if (objDPCur == null)
                ////                        {
                ////                            TestPriceSurcharge objtps = View.ObjectSpace.CreateObject<TestPriceSurcharge>();
                ////                            DefaultPricing objDP = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                ////                            if (objDP != null && !string.IsNullOrEmpty(objDP.PriceCode))
                ////                            {
                ////                                strpricecode = objDP.PriceCode;
                ////                            }
                ////                            ConstituentPricing objcp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                ////                            if (objcp != null && !string.IsNullOrEmpty(objcp.PriceCode))
                ////                            {
                ////                                strpricecode = objcp.PriceCode;
                ////                            }
                ////                            objtps.PriceCode = strpricecode;
                ////                            objtps.Matrix = objTM1.MatrixName;
                ////                            objtps.Test = objTM1;
                ////                            objtps.Method = View.ObjectSpace.GetObject(objTM1);
                ////                            objtps.Component = View.ObjectSpace.GetObject(objComDefault);
                ////                            objtps.Priority = View.ObjectSpace.GetObject(objpriority);
                ////                            ((ListView)View).CollectionSource.Add(objtps);
                ////                            Default = false;
                ////                        }
                ////                    }
                ////                    foreach (Component objCom in lstCom)
                ////                    {
                ////                        TestPriceSurcharge objDPCur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.Oid] = ? And [Test.Oid] = ? And [Method.MethodName.Oid] = ? And [Component.Oid] = ?  And [Priority.Oid] = ? And Not IsNullOrEmpty([TAT])", objTM1.MatrixName.Oid, objTM1.Oid, objTM1.MethodName.Oid, objCom.Oid, objpriority.Oid));
                ////                        if (objDPCur == null)
                ////                        {
                ////                            TestPriceSurcharge objtps = View.ObjectSpace.CreateObject<TestPriceSurcharge>();
                ////                            DefaultPricing objDP = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                ////                            if (objDP != null && !string.IsNullOrEmpty(objDP.PriceCode))
                ////                            {
                ////                                strpricecode = objDP.PriceCode;
                ////                            }
                ////                            ConstituentPricing objcp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                ////                            if (objcp != null && !string.IsNullOrEmpty(objcp.PriceCode))
                ////                            {
                ////                                strpricecode = objcp.PriceCode;
                ////                            }
                ////                            objtps.PriceCode = strpricecode;
                ////                            objtps.Matrix = objTM1.MatrixName;
                ////                            objtps.Test = objTM1;
                ////                            objtps.Method = View.ObjectSpace.GetObject(objTM1);
                ////                            objtps.Component = View.ObjectSpace.GetObject(objCom);
                ////                            objtps.Priority = View.ObjectSpace.GetObject(objpriority);
                ////                            ((ListView)View).CollectionSource.Add(objtps);
                ////                            Default = false;
                ////                        }
                ////                    }
                ////                }
                ////                else
                ////                {
                ////                    Component objCom = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                ////                    TestPriceSurcharge objDPCur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.Oid] = ? And [Test.Oid] = ? And [Method.MethodName.Oid] = ? And [Component.Oid] = ?  And [Priority.Oid] = ? And Not IsNullOrEmpty([TAT])", objTM1.MatrixName.Oid, objTM1.Oid, objTM1.MethodName.Oid, objCom.Oid, objpriority.Oid));
                ////                    if (objDPCur == null)
                ////                    {
                ////                        TestPriceSurcharge objtps = View.ObjectSpace.CreateObject<TestPriceSurcharge>();
                ////                        DefaultPricing objDP = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                ////                        if (objDP != null && !string.IsNullOrEmpty(objDP.PriceCode))
                ////                        {
                ////                            strpricecode = objDP.PriceCode;
                ////                        }
                ////                        ConstituentPricing objcp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                ////                        if (objcp != null && !string.IsNullOrEmpty(objcp.PriceCode))
                ////                        {
                ////                            strpricecode = objcp.PriceCode;
                ////                        }
                ////                        objtps.PriceCode = strpricecode;
                ////                        objtps.Matrix = objTM1.MatrixName;
                ////                        objtps.Test = objTM1;
                ////                        objtps.Method = View.ObjectSpace.GetObject(objTM1);
                ////                        objtps.Component = View.ObjectSpace.GetObject(objCom);
                ////                        objtps.Priority = View.ObjectSpace.GetObject(objpriority);
                ////                        ((ListView)View).CollectionSource.Add(objtps);
                ////                    }
                ////                }
                ////            }
                ////            else if (objTM1.IsGroup == true && objTM1 != null && objTM1.MatrixName != null && objTM1.TestName != null)
                ////            {
                ////                Component objCom = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                ////                TestPriceSurcharge objDPCur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.Oid] = ? And [Test.Oid] = ? And [Component.Oid] = ?  And [Priority.Oid] = ? And Not IsNullOrEmpty([TAT])", objTM1.MatrixName.Oid, objTM1.Oid, objCom.Oid, objpriority.Oid));
                ////                if (objDPCur == null)
                ////                {
                ////                    TestPriceSurcharge objtps = View.ObjectSpace.CreateObject<TestPriceSurcharge>();
                ////                    DefaultPricing objDP = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                ////                    if (objDP != null && !string.IsNullOrEmpty(objDP.PriceCode))
                ////                    {
                ////                        strpricecode = objDP.PriceCode;
                ////                    }
                ////                    ConstituentPricing objcp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                ////                    if (objcp != null && !string.IsNullOrEmpty(objcp.PriceCode))
                ////                    {
                ////                        strpricecode = objcp.PriceCode;
                ////                    }
                ////                    objtps.PriceCode = strpricecode;
                ////                    objtps.Matrix = objTM1.MatrixName;
                ////                    objtps.Test = objTM1;
                ////                    objtps.Method = View.ObjectSpace.GetObject(objTM1);
                ////                    objtps.Component = View.ObjectSpace.GetObject(objCom);
                ////                    objtps.Priority = View.ObjectSpace.GetObject(objpriority);
                ////                    if (objTM1.IsGroup == true)
                ////                    {
                ////                        objtps.IsGroup = true;
                ////                    }
                ////                    else
                ////                    {
                ////                        objtps.IsGroup = false;
                ////                    }
                ////                    ((ListView)View).CollectionSource.Add(objtps);
                ////                }
                ////            }
                ////        }
                ////    }
                ////    List<TestPriceSurcharge> lsttps = ((ListView)View).CollectionSource.List.Cast<TestPriceSurcharge>().Where(i => i.TAT != null).ToList();
                ////    foreach (TestPriceSurcharge objtps in lsttps.ToList())
                ////    {
                ////        if (objtps != null && !string.IsNullOrEmpty(objtps.TAT))
                ////        {
                ////            lsttps.Remove(objtps);
                ////            ((ListView)View).CollectionSource.Remove(objtps);
                ////            ((ListView)View).Refresh();
                ////        }
                ////    }
                ////}

                if (View.Id == "TestPriceSurcharge_ListView_newlist")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    testpricesurinfo.IsNotRegularSelectAll = false;
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    //List<TestPriceSurcharge> lsttestpricingsurcharge = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[GCRecord] is Null")).ToList();
                    List<TestMethod> lstTest = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[GCRecord] is Null And [MethodName.GCRecord] Is Null And [MatrixName.GCRecord] Is Null")).ToList();
                    List<Modules.BusinessObjects.Setting.Priority> lstPriority = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Priority>(CriteriaOperator.Parse("")).ToList();
                    //List<DefaultPricing> lstDefaultPricing = ObjectSpace.GetObjects<DefaultPricing>(CriteriaOperator.Parse("")).ToList();
                    //List<ConstituentPricing> lstConstituentPricing = ObjectSpace.GetObjects<ConstituentPricing>(CriteriaOperator.Parse("")).ToList();
                    List<TestMethod> lstTestmed = new List<TestMethod>();
                    List<Component> lstcomponent = new List<Component>();
                    //if (lstDefaultPricing != null && lstDefaultPricing.Count > 0)
                    //{
                    //    foreach (DefaultPricing objdefpri in lstDefaultPricing.Where(i=>i.IsGroup== ISGroupType.No).ToList())
                    //    {
                    //        TestMethod objTest = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName]=? And [MethodName.MethodNumber]=?", objdefpri.Matrix.MatrixName, objdefpri.Test.TestName, objdefpri.Method.MethodNumber));
                    //        if (objTest!=null && !lstTestmed.Contains(objTest))
                    //        {
                    //            lstTestmed.Add(objTest);
                    //        }
                    //        if (objdefpri != null && objdefpri.Component != null)
                    //        {
                    //            Component objcomp = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] =? And [TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ?", objdefpri.Component.Components, objdefpri.Matrix.MatrixName, objdefpri.Test.TestName, objdefpri.Method.MethodNumber));
                    //            if (objcomp != null)
                    //            {
                    //                if (!lstcomponent.Contains(objcomp))
                    //                {
                    //                    lstcomponent.Add(objcomp);
                    //                }
                    //            }
                    //        }

                    //    }
                    //    foreach (DefaultPricing objdefpri in lstDefaultPricing.Where(i => i.IsGroup == ISGroupType.Yes).ToList())
                    //    {
                    //        TestMethod objTest = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName]=? And [IsGroup]=True", objdefpri.Matrix.MatrixName, objdefpri.Test.TestName));
                    //        if (objTest != null && !lstTestmed.Contains(objTest))
                    //        {
                    //            lstTestmed.Add(objTest);
                    //        }
                    //        //if (objdefpri != null && objdefpri.Component != null)
                    //        //{
                    //        //    Component objcomp = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] =? And [TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.IsGroup] = True", objdefpri.Component.Components, objdefpri.Matrix.MatrixName, objdefpri.Test.TestName)); //("[Oid] =? And [TestMethod.Oid] =?", objdefpri.Component.Oid, objdefpri.Test.Oid));
                    //        //    if (objcomp != null)
                    //        //    {
                    //        //        if (!lstcomponent.Contains(objcomp))
                    //        //        {
                    //        //            lstcomponent.Add(objcomp);
                    //        //        }
                    //        //    }
                    //        //}

                    //    }
                    //}
                    //if (lstConstituentPricing != null && lstConstituentPricing.Count > 0)
                    //{
                    //    foreach (ConstituentPricing objconstituepri in lstConstituentPricing.ToList())
                    //    {
                    //        TestMethod objTest = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName]=? And [MethodName.MethodNumber]=?", objconstituepri.Matrix.MatrixName, objconstituepri.Test.TestName, objconstituepri.Method.MethodNumber));
                    //        if (objconstituepri !=null && !lstTestmed.Contains(objTest))
                    //        {
                    //            lstTestmed.Add(objTest);
                    //        }
                    //        if (objconstituepri != null && objconstituepri.Component != null)
                    //        {
                    //            Component objcomp = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] =? And [TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ?", objconstituepri.Component.Components, objconstituepri.Matrix.MatrixName, objconstituepri.Test.TestName, objconstituepri.Method.MethodNumber));
                    //            if (objcomp != null)
                    //            {
                    //                if (!lstcomponent.Contains(objcomp))
                    //                {
                    //                    lstcomponent.Add(objcomp);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    foreach (Modules.BusinessObjects.Setting.Priority objpriority in lstPriority.ToList())
                    {
                        foreach (TestMethod objTM1 in lstTest.Where(i => i.MatrixName != null && i.TestName != null).OrderBy(i => i.MatrixName.MatrixName).ThenBy(i => i.TestName).ToList())
                        {
                            //string strpricecode = string.Empty;
                            if (objTM1.IsGroup != true && objTM1 != null && objTM1.MatrixName != null && objTM1.TestName != null && objTM1.MethodName != null)
                            {
                                List<Component> lstCom = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod] = ?", objTM1.Oid)).ToList();
                                if (lstCom.Count > 0)
                                {
                                    bool Default = true;
                                    if (Default == true)
                                    {
                                        Component objComDefault = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                                        TestPriceSurcharge objDPCur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.Prioritys] = ? And Not IsNullOrEmpty([TAT])", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objComDefault.Components, objpriority.Prioritys));
                                        if (objDPCur == null)
                                        {
                                            TestPriceSurcharge objtps = View.ObjectSpace.CreateObject<TestPriceSurcharge>();
                                            objtps.Matrix = objTM1.MatrixName;
                                            objtps.Test = objTM1;
                                            objtps.Method = View.ObjectSpace.GetObject(objTM1);
                                            objtps.Component = View.ObjectSpace.GetObject(objComDefault);
                                            objtps.Priority = View.ObjectSpace.GetObject(objpriority);
                                            ((ListView)View).CollectionSource.Add(objtps);
                                            Default = false;
                                        }
                                    }
                                    foreach (Component objCom in lstCom)
                                    {
                                        if (lstCom.Contains(objCom))
                                        {
                                            TestPriceSurcharge objDPCur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.Prioritys] = ? And Not IsNullOrEmpty([TAT])", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom.Components, objpriority.Prioritys));
                                            if (objDPCur == null)
                                            {
                                                TestPriceSurcharge objtps = View.ObjectSpace.CreateObject<TestPriceSurcharge>();
                                                //DefaultPricing objDP = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom.Oid));
                                                //DefaultPricing objDP = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [Component.Oid] = ? ", objTM1.Oid, objCom.Oid));
                                                //if (objDP != null && !string.IsNullOrEmpty(objDP.PriceCode))
                                                //{
                                                //    strpricecode = objDP.PriceCode;
                                                //    objtps.SurchargePrice = objDP.UnitPrice;
                                                //}
                                                //ConstituentPricing objcp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Oid] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom.Oid));
                                                //ConstituentPricing objcp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [Component.Oid] = ?", objTM1.Oid, objCom.Oid));
                                                //if (objcp != null && !string.IsNullOrEmpty(objcp.PriceCode))
                                                //{
                                                //    strpricecode = objcp.PriceCode;
                                                //}
                                                //if (!string.IsNullOrEmpty(strpricecode))
                                                //{
                                                //    objtps.PriceCode = strpricecode;

                                                //}
                                                objtps.Matrix = objTM1.MatrixName;
                                                objtps.Test = objTM1;
                                                objtps.Method = View.ObjectSpace.GetObject(objTM1);
                                                objtps.Component = View.ObjectSpace.GetObject(objCom);
                                                objtps.Priority = View.ObjectSpace.GetObject(objpriority);
                                                ((ListView)View).CollectionSource.Add(objtps);
                                                Default = false;
                                            }
                                        }
                                        ////TestPriceSurcharge objDPCur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.Oid] = ? And [Test.Oid] = ? And [Method.MethodName.Oid] = ? And [Component.Oid] = ?  And [Priority.Oid] = ? And Not IsNullOrEmpty([TAT])", objTM1.MatrixName.Oid, objTM1.Oid, objTM1.MethodName.Oid, objCom.Oid, objpriority.Oid));
                                        ////if (objDPCur == null)
                                        ////{
                                        ////    TestPriceSurcharge objtps = View.ObjectSpace.CreateObject<TestPriceSurcharge>();
                                        ////    DefaultPricing objDP = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                                        ////    if (objDP != null && !string.IsNullOrEmpty(objDP.PriceCode))
                                        ////    {
                                        ////        strpricecode = objDP.PriceCode;
                                        ////    }
                                        ////    ConstituentPricing objcp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                                        ////    if (objcp != null && !string.IsNullOrEmpty(objcp.PriceCode))
                                        ////    {
                                        ////        strpricecode = objcp.PriceCode;
                                        ////    }
                                        ////    objtps.PriceCode = strpricecode;
                                        ////    objtps.Matrix = objTM1.MatrixName;
                                        ////    objtps.Test = objTM1;
                                        ////    objtps.Method = View.ObjectSpace.GetObject(objTM1);
                                        ////    objtps.Component = View.ObjectSpace.GetObject(objCom);
                                        ////    objtps.Priority = View.ObjectSpace.GetObject(objpriority);
                                        ////    ((ListView)View).CollectionSource.Add(objtps);
                                        ////    Default = false;
                                        ////}
                                    }
                                }
                                else
                                {
                                    Component objCom = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                                    TestPriceSurcharge objDPCur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.Prioritys] = ? And Not IsNullOrEmpty([TAT])", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom.Components, objpriority.Prioritys));
                                    if (objDPCur == null)
                                    {
                                        TestPriceSurcharge objtps = View.ObjectSpace.CreateObject<TestPriceSurcharge>();
                                        //DefaultPricing objDP = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom));
                                        //if (objDP != null && !string.IsNullOrEmpty(objDP.PriceCode))
                                        //{
                                        //    strpricecode = objDP.PriceCode;
                                        //    objtps.SurchargePrice = objDP.UnitPrice;
                                        //}
                                        //ConstituentPricing objcp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Oid] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom));

                                        //if (objcp != null && !string.IsNullOrEmpty(objcp.PriceCode))
                                        //{
                                        //    strpricecode = objcp.PriceCode;
                                        //}
                                        //if (!string.IsNullOrEmpty(strpricecode))
                                        //{

                                        //}
                                        //objtps.PriceCode = strpricecode;
                                        objtps.Matrix = objTM1.MatrixName;
                                        objtps.Test = objTM1;
                                        objtps.Method = View.ObjectSpace.GetObject(objTM1);
                                        objtps.Component = View.ObjectSpace.GetObject(objCom);
                                        objtps.Priority = View.ObjectSpace.GetObject(objpriority);
                                        ((ListView)View).CollectionSource.Add(objtps);
                                    }
                                }
                            }
                            else if (objTM1.IsGroup == true && objTM1 != null && objTM1.MatrixName != null && objTM1.TestName != null)
                            {
                                Component objCom = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                                TestPriceSurcharge objDPCur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Priority.Prioritys] = ? And Not IsNullOrEmpty([TAT])", objTM1.MatrixName.MatrixName, objTM1.TestName, objpriority.Prioritys));
                                if (objDPCur == null)
                                {
                                    TestPriceSurcharge objtps = View.ObjectSpace.CreateObject<TestPriceSurcharge>();
                                    //DefaultPricing objDP = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                                    //if (objDP != null && !string.IsNullOrEmpty(objDP.PriceCode))
                                    //{
                                    //    strpricecode = objDP.PriceCode;
                                    //    objtps.SurchargePrice = objDP.UnitPrice;
                                    //}
                                    //ConstituentPricing objcp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objTM1.Oid));
                                    //if (objcp != null && !string.IsNullOrEmpty(objcp.PriceCode))
                                    //{
                                    //    strpricecode = objcp.PriceCode;
                                    //}
                                    //objtps.PriceCode = strpricecode;
                                    objtps.Matrix = objTM1.MatrixName;
                                    objtps.Test = objTM1;
                                    //objtps.Method = View.ObjectSpace.GetObject(objTM1);

                                    objtps.Priority = View.ObjectSpace.GetObject(objpriority);
                                    if (objTM1.IsGroup == true)
                                    {
                                        objtps.IsGroup = true;
                                    }
                                    else
                                    {
                                        objtps.Component = View.ObjectSpace.GetObject(objCom);
                                        objtps.IsGroup = false;
                                    }
                                    ((ListView)View).CollectionSource.Add(objtps);
                                }
                            }
                        }
                    }
                    testpricesurinfo.lsttpsnewlist = ((ListView)View).CollectionSource.List.Cast<TestPriceSurcharge>().ToList();
                }
                else if (View.Id == "TestPriceSurcharge_ListView")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                }
                //if (View.Id == "DefaultPricing_ListView_Copy")
                //{
                //    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                //    IList<ConstituentPricing> lstconstipricing = ObjectSpace.GetObjects<ConstituentPricing>(CriteriaOperator.Parse("[GCRecord] is Null"));
                //    List<ConstituentPricing> lstTestoid = lstconstipricing.Where(i => i.Matrix != null && i.Test != null && i.Method != null && i.Component != null).ToList();
                //    List<Guid> lstcomponentoid = lstconstipricing.Where(i => i.Matrix != null && i.Test != null && i.Method != null && i.Component != null).Select(i => i.Component.Oid).Distinct().ToList();

                //    List<TestMethod> objTM = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("")).ToList();
                //    IList<DefaultPricing> lstDefaultPrice = ObjectSpace.GetObjects<DefaultPricing>(CriteriaOperator.Parse("[GCRecord] is Null"));
                //    ((ListView)View).CollectionSource.Criteria["Filter1"] = new NotOperator(new InOperator("Oid", lstDefaultPrice.Select(i => i.Oid)));

                //    foreach (TestMethod objTM1 in objTM)
                //    {

                //        List<Component> lstCom = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod] = ?", objTM1.Oid)).ToList();
                //        if (lstCom.Count > 0)
                //        {
                //            bool Default = true;
                //            if (Default == true)
                //            {
                //                Component objComDefault = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                //                if (lstTestoid.FirstOrDefault(i => i.Matrix.MatrixName == objTM1.MatrixName.MatrixName && i.Test.TestName == objTM1.TestName && i.Method.MethodNumber == objTM1.MethodName.MethodNumber && i.Component.Components == "Default") == null)
                //                {
                //                    ConstituentPricing objCp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Oid] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objComDefault));
                //                    DefaultPricing objDPCur = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objComDefault));
                //                    if (objDPCur == null && objCp == null)
                //                    {
                //                        DefaultPricing objDP = View.ObjectSpace.CreateObject<DefaultPricing>();
                //                        objDP.Matrix = objTM1.MatrixName;
                //                        objDP.Test = objTM1;
                //                        objDP.Method = objTM1.MethodName;
                //                        if (objTM1.IsGroup == true)
                //                        {
                //                            objDP.IsGroup = ISGroupType.Yes;
                //                        }
                //                        else
                //                        {
                //                            objDP.Component = objComDefault;
                //                            objDP.IsGroup = ISGroupType.No;
                //                        }
                //                        ((ListView)View).CollectionSource.Add(objDP);
                //                        Default = false;
                //                    }
                //                }
                //            }
                //            foreach (Component objCom in lstCom)
                //            {
                //                Testparameter objtp = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse(" [Component.Oid] = ?", objCom));
                //                if (objtp != null)
                //                {
                //                    ConstituentPricing objCp = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Oid] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom));
                //                    if (objCp == null)
                //                    {
                //                        DefaultPricing objDPCur = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom));
                //                        if (objDPCur == null)
                //                        {
                //                            DefaultPricing objDP = View.ObjectSpace.CreateObject<DefaultPricing>();
                //                            objDP.Matrix = objTM1.MatrixName;
                //                            objDP.Test = objTM1;
                //                            objDP.Method = objTM1.MethodName;
                //                            if (objTM1.IsGroup == true)
                //                            {
                //                                objDP.IsGroup = ISGroupType.Yes;
                //                            }
                //                            else
                //                            {
                //                                objDP.Component = objCom;
                //                                objDP.IsGroup = ISGroupType.No;
                //                            }
                //                            ((ListView)View).CollectionSource.Add(objDP);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (lstTestoid.FirstOrDefault(i => i.Matrix.MatrixName == objTM1.MatrixName.MatrixName && i.Test.TestName == objTM1.TestName && i.Method.MethodNumber == objTM1.MethodName.MethodNumber && i.Component.Components == "Default") == null)
                //            {
                //                Component objCom = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                //                if (objTM1.IsGroup == false && objTM1.MethodName != null && objTM1 != null && objTM1.MatrixName != null)
                //                {
                //                    ConstituentPricing conpricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom));
                //                    DefaultPricing objDPCur = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component] = ? ", objTM1.MatrixName.MatrixName, objTM1.TestName, objTM1.MethodName.MethodNumber, objCom));
                //                    if (objDPCur == null && conpricing == null)
                //                    {
                //                        DefaultPricing objDP = View.ObjectSpace.CreateObject<DefaultPricing>();
                //                        objDP.Matrix = objTM1.MatrixName;
                //                        objDP.Test = objTM1;
                //                        objDP.Method = objTM1.MethodName;
                //                        if (objTM1.IsGroup == true)
                //                        {
                //                            objDP.IsGroup = ISGroupType.Yes;
                //                        }
                //                        else
                //                        {
                //                            objDP.Component = objCom;
                //                            objDP.IsGroup = ISGroupType.No;
                //                        }
                //                        ((ListView)View).CollectionSource.Add(objDP);
                //                    }
                //                }
                //                else
                //                {
                //                    ConstituentPricing conpricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = True ", objTM1.MatrixName.MatrixName, objTM1.TestName));
                //                    DefaultPricing objDPCur = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'Yes' ", objTM1.MatrixName.MatrixName, objTM1.TestName));
                //                    if (objDPCur == null && conpricing == null)
                //                    {
                //                        DefaultPricing objDP = View.ObjectSpace.CreateObject<DefaultPricing>();
                //                        objDP.Matrix = objTM1.MatrixName;
                //                        objDP.Test = objTM1;
                //                        objDP.Method = objTM1.MethodName;
                //                        if (objTM1.IsGroup == true)
                //                        {
                //                            objDP.IsGroup = ISGroupType.Yes;
                //                        }
                //                        else
                //                        {
                //                            objDP.Component = objCom;
                //                            objDP.IsGroup = ISGroupType.No;
                //                        }
                //                        ((ListView)View).CollectionSource.Add(objDP);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                if (View.Id == "ConstituentPricingTier_ListView" || View.Id == "ConstituentPricing_ConstituentPricingTiers_ListView")
                {
                    if (Application.MainWindow.View.Id == "ConstituentPricing_DetailView")
                    {
                        ConstituentPricing objcp = (ConstituentPricing)Application.MainWindow.View.CurrentObject;
                        if (objcp != null)
                        {
                            IList<ConstituentPricingTier> lstcpteir = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing] = ?", objcp.Oid));
                            if (lstcpteir != null && lstcpteir.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[ConstituentPricing] = ?", objcp.Oid);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                            }
                        }
                        if (priceinfo.lstselectremove != null && priceinfo.lstselectremove.Count > 0)
                        {
                            priceinfo.lstselectremove.Clear();
                        }
                    }
                }
                if (View.Id == "ConstituentPricing_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("Hide", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("Hide", false);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void standardFilterController_CustomGetFullTextSearchProperties(object sender, CustomGetFullTextSearchPropertiesEventArgs e)
        {
            try
            {
                if (isCustomFilter)
                {
                    List<string> searchProperties = new List<string>();
                    searchProperties.Add("PriceCode");
                    foreach (string property in searchProperties)
                    {
                        e.Properties.Add(property);
                    }
                    e.Handled = true;
                    isCustomFilter = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void FullTextFilterAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "TestPriceSurcharge_ListView_newlist")
                {
                    ParametrizedAction objFilter = (ParametrizedAction)sender;
                    string newvalue = objFilter.Value.ToString();
                    if (!string.IsNullOrEmpty(newvalue))
                    {
                        DataTable dt = new DataTable("TestPriceDataTable");
                        dt.Columns.Add("Matrix", typeof(string));
                        dt.Columns.Add("Test", typeof(string));
                        dt.Columns.Add("Isgroup", typeof(Boolean));
                        dt.Columns.Add("Method", typeof(string));
                        dt.Columns.Add("Component", typeof(string));
                        dt.Columns.Add("TAT", typeof(string));
                        dt.Columns.Add("Priority", typeof(string));
                        dt.Columns.Add("Surcharge", typeof(string));
                        dt.Columns.Add("SurchargePrice", typeof(string));
                        dt.Columns.Add("Remark", typeof(string));
                        dt.Columns.Add("PrioiritySort", typeof(string));
                        dt.Columns.Add("Oid", typeof(string));

                        foreach (TestPriceSurcharge testPriceSurcharge in ((ListView)View).CollectionSource.List.Cast<TestPriceSurcharge>().ToList())
                        {
                            DataRow newRow = dt.NewRow();
                            newRow["Oid"] = testPriceSurcharge.Oid;
                            if (testPriceSurcharge.Method != null)
                            {
                                newRow["Method"] = testPriceSurcharge.Method.MethodName.MethodNumber;
                            }
                            else
                            {
                                newRow["Method"] = string.Empty;
                            }
                            // newRow["Method"] = testPriceSurcharge.Method.MethodName.MethodNumber;
                            if (testPriceSurcharge.Component != null)
                            {
                                newRow["Component"] = testPriceSurcharge.Component.Components;
                            }
                            else
                            {
                                newRow["Component"] = string.Empty;
                            }
                            //  newRow["createddate"] = testPriceSurcharge.CreatedDate;
                            //  newRow["createdby"] = testPriceSurcharge.Createdby;
                            newRow["Isgroup"] = testPriceSurcharge.IsGroup;
                            newRow["Matrix"] = testPriceSurcharge.Matrix.MatrixName;
                            //   newRow["Modifieddate"] = testPriceSurcharge.ModifiedDate;
                            //   newRow["Modifiedby"] = testPriceSurcharge.Modifiedby;
                            //   newRow["PriceCode"] = testPriceSurcharge.PriceCode;
                            newRow["Priority"] = testPriceSurcharge.Priority.Prioritys;
                            newRow["PrioiritySort"] = testPriceSurcharge.PrioritySort;
                            newRow["Remark"] = testPriceSurcharge.Remark;
                            newRow["Surcharge"] = testPriceSurcharge.Surcharge;
                            newRow["SurchargePrice"] = testPriceSurcharge.SurchargePrice;
                            newRow["TAT"] = testPriceSurcharge.TAT;
                            newRow["Test"] = testPriceSurcharge.Test.TestName;

                            dt.Rows.Add(newRow);

                        }
                        tembtable = dt;
                        string filterExpression = $"Matrix LIKE '%{newvalue}%' OR Component LIKE '%{newvalue}%' OR Priority LIKE '%{newvalue}%' OR Method LIKE'%{newvalue}%' OR Test LIKE'%{newvalue}%' OR PrioiritySort LIKE'%{newvalue}%' OR SurchargePrice LIKE'%{newvalue}%'";
                        DataRow[] filteredRows = tembtable.Select(filterExpression);
                        List<Guid> lstfltoid = new List<Guid>();
                        foreach (DataRow dr in filteredRows)
                        {
                            lstfltoid.Add(new Guid(dr["Oid"].ToString()));
                        }
                        ((ListView)View).CollectionSource.List.Clear();
                        foreach (TestPriceSurcharge objtps in testpricesurinfo.lsttpsnewlist.ToList())
                        {
                            if (lstfltoid.Contains(objtps.Oid))
                            {
                                ((ListView)View).CollectionSource.Add(objtps);
                            }
                        }
                        ((ListView)View).Refresh();
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.List.Clear();
                        foreach (TestPriceSurcharge objtps in testpricesurinfo.lsttpsnewlist.ToList())
                        {
                            ((ListView)View).CollectionSource.Add(objtps);
                        }
                        ((ListView)View).Refresh();
                    }
                    e.Cancel = true;
                }
                else
                {
                    ParametrizedAction objFilter = (ParametrizedAction)sender;
                    if (objFilter != null && !string.IsNullOrEmpty(Convert.ToString(objFilter.Value)) && objFilter.Value.ToString().Contains("-"))
                    {
                        isCustomFilter = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                //if (View.Id == "ConstituentPricing_DetailView")
                //{
                //    ConstituentPricing objconsprice = (ConstituentPricing)Application.MainWindow.View.CurrentObject;
                //    if (objconsprice != null)
                //    {
                //        IObjectSpace os = Application.CreateObjectSpace();
                //        DashboardViewItem dvview = ((DetailView)View).FindItem("ConstituentPricing") as DashboardViewItem;
                //        if (dvview != null && dvview.InnerView != null)
                //        {
                //            foreach (ConstituentPricingTier objconprice in ((ListView)dvview.InnerView).CollectionSource.List)
                //            {
                //                ConstituentPricingTier chkconprice = os.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[TierNo] = ? And [From] = ? And [To] = ? And [TierPrice] = ? And [ConstituentPricing] = ? ", objconprice.TierNo, objconprice.From, objconprice.To, objconprice.TierPrice, objconsprice.Oid));
                //                if (chkconprice == null)
                //                {
                //                    ConstituentPricingTier crtconprice = os.CreateObject<ConstituentPricingTier>();
                //                    crtconprice.TierNo = objconprice.TierNo;
                //                    crtconprice.From = objconprice.From;
                //                    crtconprice.To = objconprice.To;
                //                    crtconprice.TierPrice = objconprice.TierPrice;
                //                    crtconprice.ConstituentPricing = os.GetObject(objconsprice);
                //                }
                //                else
                //                {
                //                    chkconprice.TierNo = objconprice.TierNo;
                //                    chkconprice.From = objconprice.From;
                //                    chkconprice.To = objconprice.To;
                //                    chkconprice.TierPrice = objconprice.TierPrice;
                //                    chkconprice.ConstituentPricing = os.GetObject(objconsprice);
                //                }
                //                os.CommitChanges();
                //            }
                //            os.Refresh();

                //            //ASPxGridListEditor gridlistedit = ((ListView)dvview.InnerView).Editor as ASPxGridListEditor;
                //            //if (gridlistedit != null && gridlistedit.Grid != null)
                //            //{
                //            //    gridlistedit.Grid.UpdateEdit();
                //            //}

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

        private void ChangeLayoutGroupCaptionViewController_ItemCreated(object sender, ItemCreatedEventArgs e)
        {
            try
            {
                if (e.ViewItem is PropertyEditor && ((PropertyEditor)e.ViewItem).PropertyName == "Matrix")
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
                if (e.ViewItem is PropertyEditor && ((PropertyEditor)e.ViewItem).PropertyName == "Test")
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
                if (e.ViewItem is PropertyEditor && ((PropertyEditor)e.ViewItem).PropertyName == "Method")
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
                if (e.ViewItem is PropertyEditor && ((PropertyEditor)e.ViewItem).PropertyName == "Component")
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                //if (View.Id == "ConstituentPricing_DetailView")
                //{
                //    ConstituentPricing objconprice = (ConstituentPricing)View.CurrentObject;
                //    if (objconprice != null && !string.IsNullOrEmpty(objconprice.PriceCode))
                //    {
                //        string[] strarr = objconprice.PriceCode.Split('-');
                //        TestPriceCode objtstpricode = ObjectSpace.FindObject<TestPriceCode>(CriteriaOperator.Parse("[PriceCode] = ?", objconprice.PriceCode));
                //        if (objtstpricode != null)
                //        {
                //            ObjectSpace.Delete(objtstpricode);
                //        }
                //    }
                //}
                //if (View.Id == "ConstituentPricing_ListView")
                //{
                //    foreach (ConstituentPricing objconpri in View.SelectedObjects)
                //    {
                //        string[] strarr = objconpri.PriceCode.Split('-');
                //        TestPriceCode objtstpricode = ObjectSpace.FindObject<TestPriceCode>(CriteriaOperator.Parse("[PriceCode] = ?", objconpri.PriceCode));
                //        if (objtstpricode != null)
                //        {
                //            ObjectSpace.Delete(objtstpricode);
                //        }
                //    }
                //}
                if (View.Id == "PretreatmentPricing_DetailView")
                {
                    PretreatmentPricing objprepprice = (PretreatmentPricing)View.CurrentObject;
                    if (objprepprice != null && !string.IsNullOrEmpty(objprepprice.PriceCode))
                    {
                        string[] strarr = objprepprice.PriceCode.Split('-');
                        TestPriceCode objtstpricode = ObjectSpace.FindObject<TestPriceCode>(CriteriaOperator.Parse("[PriceCode] = ?", objprepprice.PriceCode));
                        if (objtstpricode != null)
                        {
                            ObjectSpace.Delete(objtstpricode);
                        }
                    }
                }
                else if (View.Id == "PretreatmentPricing_ListView")
                {
                    foreach (PretreatmentPricing objpreppri in View.SelectedObjects)
                    {
                        string[] strarr = objpreppri.PriceCode.Split('-');
                        TestPriceCode objtstpricode = ObjectSpace.FindObject<TestPriceCode>(CriteriaOperator.Parse("[PriceCode] = ?", objpreppri.PriceCode));
                        if (objtstpricode != null)
                        {
                            ObjectSpace.Delete(objtstpricode);
                        }
                    }
                }
                //if (View.Id == "TestPriceSurcharge_DetailView")
                //{
                //    TestPriceSurcharge objtstprisur = (TestPriceSurcharge)View.CurrentObject;
                //    if (objtstprisur != null && !string.IsNullOrEmpty(objtstprisur.PriceCode))
                //    {
                //        string[] strarr = objtstprisur.PriceCode.Split('-');
                //        TestPriceCode objtstpricode = ObjectSpace.FindObject<TestPriceCode>(CriteriaOperator.Parse("[PriceCode] = ?", objtstprisur.PriceCode));
                //        if (objtstpricode != null)
                //        {
                //            ObjectSpace.Delete(objtstpricode);
                //        }
                //    }
                //}
                else if (View.Id == "TestPriceSurcharge_ListView")
                {
                    if (View.SelectedObjects.Count == 1)
                    {
                        foreach (TestPriceSurcharge objPrice in View.SelectedObjects)
                        {
                            if (((ListView)View).CollectionSource.List.Cast<TestPriceSurcharge>().FirstOrDefault(i => (i.IsGroup == false && i.Matrix != null && i.Method != null && i.Test != null && i.Component != null && i.Matrix.MatrixName == objPrice.Matrix.MatrixName && i.Test.TestName == objPrice.Test.TestName && i.Method.MethodName.MethodNumber == objPrice.Method.MethodName.MethodNumber && i.Component.Components == objPrice.Component.Components && i.Priority.IsRegular == false && objPrice.Priority.IsRegular == true) || (i.IsGroup == true && i.Matrix != null && i.Test != null && i.Test.TestName == objPrice.Test.TestName && i.Matrix.MatrixName == objPrice.Matrix.MatrixName && i.Priority.IsRegular == false && objPrice.Priority.IsRegular == true)) != null)
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage("To begin delete up a default Price , you must first deleted the surcharge price.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }

                    }
                    else if (View.SelectedObjects.Count != ((ListView)View).CollectionSource.GetCount())
                    {
                        foreach (TestPriceSurcharge objPrice in View.SelectedObjects)
                        {
                            if (((ListView)View).CollectionSource.List.Cast<TestPriceSurcharge>().FirstOrDefault(i => (i.IsGroup == false && i.Matrix != null && i.Method != null && i.Test != null && i.Component != null && i.Matrix.MatrixName == objPrice.Matrix.MatrixName && i.Test.TestName == objPrice.Test.TestName && i.Method.MethodName.MethodNumber == objPrice.Method.MethodName.MethodNumber && i.Component.Components == objPrice.Component.Components && i.Priority.IsRegular == false && objPrice.Priority.IsRegular == true) || (i.IsGroup == true && i.Matrix != null && i.Test != null && i.Test.TestName == objPrice.Test.TestName && i.Matrix.MatrixName == objPrice.Matrix.MatrixName && i.Priority.IsRegular == false && objPrice.Priority.IsRegular == true)) != null)
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage("To begin delete up a default Price , you must first deleted the surcharge price.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }

                    }
                    foreach (TestPriceSurcharge objtstsurpri in View.SelectedObjects)
                    {
                        string[] strarr = objtstsurpri.PriceCode.Split('-');
                        TestPriceCode objtstpricode = ObjectSpace.FindObject<TestPriceCode>(CriteriaOperator.Parse("[PriceCode] = ?", objtstsurpri.PriceCode));
                        if (objtstpricode != null)
                        {
                            ObjectSpace.Delete(objtstpricode);
                        }
                    }
                }
                //if (View.Id == "DefaultPricing_DetailView")
                //{
                //    DefaultPricing objdefprice = (DefaultPricing)View.CurrentObject;
                //    if (objdefprice != null && !string.IsNullOrEmpty(objdefprice.PriceCode))
                //    {
                //        string[] strarr = objdefprice.PriceCode.Split('-');
                //        TestPriceCode objtstpricode = ObjectSpace.FindObject<TestPriceCode>(CriteriaOperator.Parse("[PriceCode] = ?", objdefprice.PriceCode));
                //        if (objtstpricode != null)
                //        {
                //            ObjectSpace.Delete(objtstpricode);
                //        }
                //    }
                //}
                //if (View.Id == "DefaultPricing_ListView")
                //{
                //    foreach (DefaultPricing objdefpri in View.SelectedObjects)
                //    {
                //        string[] strarr = objdefpri.PriceCode.Split('-');
                //        TestPriceCode objtstpricode = ObjectSpace.FindObject<TestPriceCode>(CriteriaOperator.Parse("[PriceCode] = ?", objdefpri.PriceCode));
                //        if (objtstpricode != null)
                //        {
                //            ObjectSpace.Delete(objtstpricode);
                //        }
                //    }
                //}
                ObjectSpace.CommitChanges();
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
                if (View is DetailView)
                {
                    foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible == true))
                    {
                        if (item is ASPxStringPropertyEditor)
                        {
                            ASPxStringPropertyEditor editor = (ASPxStringPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxIntPropertyEditor)
                        {
                            ASPxIntPropertyEditor editor = (ASPxIntPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDoublePropertyEditor)
                        {
                            ASPxDoublePropertyEditor editor = (ASPxDoublePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDecimalPropertyEditor)
                        {
                            ASPxDecimalPropertyEditor editor = (ASPxDecimalPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDateTimePropertyEditor)
                        {
                            ASPxDateTimePropertyEditor editor = (ASPxDateTimePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxLookupPropertyEditor)
                        {
                            ASPxLookupPropertyEditor editor = (ASPxLookupPropertyEditor)item;
                            if (editor != null && editor.DropDownEdit != null && editor.DropDownEdit.DropDown != null)
                            {
                                editor.DropDownEdit.DropDown.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxBooleanPropertyEditor)
                        {
                            ASPxBooleanPropertyEditor editor = (ASPxBooleanPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxEnumPropertyEditor)
                        {
                            ASPxEnumPropertyEditor editor = (ASPxEnumPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxGridLookupPropertyEditor)
                        {
                            ASPxGridLookupPropertyEditor editor = (ASPxGridLookupPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                //if (View.Id == "ConstituentPricing_DetailView_Quotes")
                //{
                //    ConstituentPricing objcp = (ConstituentPricing)View.CurrentObject;
                //    if (objcp != null && objcp.Test != null)
                //    {
                //        DashboardViewItem dvtierprice = ((DetailView)View).FindItem("ConstituentPricing") as DashboardViewItem;
                //        DashboardViewItem dvgrouptest = ((DetailView)View).FindItem("Testmethods") as DashboardViewItem;
                //        DashboardViewItem dvparameter = ((DetailView)View).FindItem("Parameterview") as DashboardViewItem;
                //        if (dvtierprice != null && dvgrouptest != null && dvparameter != null && dvtierprice.InnerView == null && dvgrouptest.InnerView == null && dvparameter.InnerView == null)
                //        {
                //            dvtierprice.CreateControl();
                //        }
                //        else
                //        {
                //            dvtierprice.InnerView.AllowEdit["Disable"] = false;
                //            ((ListView)dvtierprice.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ConstituentPricing.Oid] = ?", objcp.Oid);
                //            ((ListView)dvtierprice.InnerView).Refresh();
                //        }
                //        if (objcp.IsGroup == true)
                //        {
                //            dvgrouptest.InnerView.AllowEdit["Disable"] = false;
                //            ((ListView)dvgrouptest.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", objcp.Test.Oid);
                //            ((ListView)dvgrouptest.InnerView).Refresh();
                //        }
                //        else if (objcp.IsGroup == false)
                //        {
                //            dvparameter.InnerView.AllowEdit["Disable"] = false;
                //            ((ListView)dvparameter.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Test.Oid);
                //            ((ListView)dvparameter.InnerView).Refresh();
                //        }
                //    }
                //}
                if (View.Id == "TestPriceSurcharge_ListView")
                {
                    if (priceinfo.tpsguidlst == null)
                    {
                        priceinfo.tpsguidlst = new List<Guid>();
                    }
                    else if (priceinfo.tpsguidlst != null && priceinfo.tpsguidlst.Count > 0)
                    {
                        priceinfo.tpsguidlst.Clear();
                    }
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("TestPriceSurcharge", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.HtmlDataCellPrepared += Gv_HtmlDataCellPrepared;
                        }
                    }
                }
                else if (View.Id == "DefaultPricing_ListView_Copy" || View.Id == "DefaultPricing_ListView")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("DefaultPriceGroup", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.HtmlDataCellPrepared += Gv_HtmlDataCellPrepared;

                            ////    gv.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            ////    window.setTimeout(function() {                     
                            ////    var unitprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'UnitPrice');
                            ////    var prep1 = s.batchEditApi.GetCellValue(e.visibleIndex, 'Prep1Charge');
                            ////    var prep2 = s.batchEditApi.GetCellValue(e.visibleIndex, 'Prep2Charge');                  
                            ////    var totalprice = unitprice + prep1 + prep2;
                            ////    if(unitprice >= 0)
                            ////    {
                            ////       s.batchEditApi.SetCellValue(e.visibleIndex, 'TotalUnitPrice', totalprice);
                            ////    }                              
                            ////    }, 20); }";
                            ////    gv.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            ////{                  
                            ////    sessionStorage.setItem('TPFocusedColumn', null);  
                            ////    var fieldName = e.cellInfo.column.fieldName;                       
                            ////    sessionStorage.setItem('TPFocusedColumn', fieldName);   
                            ////    //alert(fieldName);
                            ////}";
                        }
                        //gv.JSProperties["NotallowednegativeValue"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue");
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        {
                            var fieldName = e.cellInfo.column.fieldName;
                            sessionStorage.setItem('FocusedColumn', fieldName);
                        }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() { 
                            var fieldName = sessionStorage.getItem('FocusedColumn');
                            if( s.batchEditApi.HasChanges(e.visibleIndex) && fieldName == 'UnitPrice')
                            {
                               var unitprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'UnitPrice');
                               if(unitprice < 0)
                               {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'UnitPrice', 0);
                                    alert('Not allowed negative value.');
                               }
                            }
                           
                            }, 20); }";
                    }
                }
                //if (View.Id == "GroupTestSurcharge_DetailView")
                //{
                //    DashboardViewItem DVgrouptestmethod = ((DetailView)View).FindItem("Testmethods") as DashboardViewItem;
                //    GroupTestSurcharge objgts = (GroupTestSurcharge)View.CurrentObject;
                //    if (objgts != null && objgts.TestGroup != null)
                //    {
                //        GroupTest objgt = ObjectSpace.FindObject<GroupTest>(CriteriaOperator.Parse("[TestGroupName] = ?", objgts.TestGroup.TestGroupName));
                //        if (objgt != null && objgt.TestMethods.Count > 0)
                //        {
                //            List<Guid> lstgtmoid = new List<Guid>();
                //            foreach (TestMethod objtm in objgt.TestMethods.ToList())
                //            {
                //                if (!lstgtmoid.Contains(objtm.Oid))
                //                {
                //                    lstgtmoid.Add(objtm.Oid);
                //                }
                //            }
                //            if (DVgrouptestmethod != null && DVgrouptestmethod.InnerView != null)
                //            {
                //                ((ListView)DVgrouptestmethod.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstgtmoid);
                //            }
                //        }
                //        else if (objgt == null)
                //        {
                //            if (DVgrouptestmethod != null && DVgrouptestmethod.InnerView != null)
                //            {
                //                ((ListView)DVgrouptestmethod.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                //            }
                //        }
                //    }
                //    else if (objgts.TestGroup == null)
                //    {
                //        if (DVgrouptestmethod != null && DVgrouptestmethod.InnerView != null)
                //        {
                //            ((ListView)DVgrouptestmethod.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                //        }
                //    }
                //}
                else if (View.Id == "ConstituentPricingTier_ListView" || View.Id == "ConstituentPricing_ConstituentPricingTiers_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load += Grid_Load;
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        ////                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) 
                        ////                        {
                        ////sessionStorage.setItem('ToValue' + e.visibleIndex, null);
                        ////                            var Rowtodata = s.batchEditApi.GetCellValue(e.visibleIndex, 'To');
                        ////alert(Rowtodata);
                        ////sessionStorage.setItem('ToValue' + e.visibleIndex, Rowtodata);
                        //////                            if (sessionStorage.getItem('ToValue' + e.visibleIndex) == null){  
                        //////alert(Rowtodata);
                        //////                            sessionStorage.setItem('ToValue' + e.visibleIndex, Rowtodata);
                        //////                            } 
                        ////                        }";

                        //gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                        //    window.setTimeout(function() { 
                        //        s.UpdateEdit();
                        //    }, 20); }";
                        ////window.setTimeout(function() { 
                        ////var intfrom = s.batchEditApi.GetCellValue(e.visibleIndex, 'From');
                        ////var intto = s.batchEditApi.GetCellValue(e.visibleIndex, 'To');
                        ////var inttier = s.batchEditApi.GetCellValue(e.visibleIndex, 'TierPrice');
                        ////var intPrep1 = s.batchEditApi.GetCellValue(e.visibleIndex, 'Prep1Charge'); 
                        ////var intPrep2 = s.batchEditApi.GetCellValue(e.visibleIndex, 'Prep2Charge'); 
                        ////if(intfrom > 0 && intto > 0 && inttier >= 0 && intPrep1 >= 0 && intPrep2 >= 0)
                        ////{
                        ////    //var fromto = (intto - intfrom) + 1;
                        ////    //var total = (inttier + intPrep1 +intPrep2) * fromto;
                        ////    var total = (inttier + intPrep1 +intPrep2) 
                        ////    if(total > 0)
                        ////    {
                        ////        s.batchEditApi.SetCellValue(e.visibleIndex, 'TotalTierPrice', total);
                        ////        s.UpdateEdit();
                        ////        //s.timerHandle = setTimeout(function() {  
                        ////        //if (s.batchEditApi.HasChanges()) 
                        ////        //{  
                        ////        //    s.UpdateEdit();  
                        ////        //} }, 20);
                        ////    }
                        ////}
                        ////}, 20); }";
                    }
                    //  Frame.GetController<RefreshController>().RefreshAction.Executed += RefreshAction_Executed;
                }
                else if (View.Id == "TestPriceSurcharge_ListView_newlist")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("TestPriceSurcharge", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditConfirmShowing = @"function(s,e) 
                    { 
                        e.cancel = true;
                    }";
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Load += Grid_Load;
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.HtmlDataCellPrepared += Gv_HtmlDataCellPrepared;
                        }
                        gridListEditor.Grid.JSProperties["cpViewID"] = View.Id;
                        gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                          if (e.visibleIndex != '-1')
                          {
                            s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                             if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                                RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', 'Selected|' + Oidvalue , '', false);    
                             }else{
                                RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', 'UNSelected|' + Oidvalue, '', false);    
                             }
                            }); 
                          }
                          else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                          {        
                            RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', 'Selectall', '', false);     
                          }
                          else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                          {
                            RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', 'UNSelectall', '', false);                        
                          }                      
                        }";
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        {
                            //var fieldName = e.cellInfo.column.fieldName;
                            //sessionStorage.setItem('FocusedColumn', fieldName);
                                if(sessionStorage.getItem('CurrFocusedColumn') == null)
                                {
                                    sessionStorage.setItem('PrevFocusedColumn', e.cellInfo.column.fieldName);
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }
                                else
                                {
                                    var precolumn = sessionStorage.getItem('CurrFocusedColumn');
                                    sessionStorage.setItem('PrevFocusedColumn', precolumn);                           
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }  
                        }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() { 
                            var fieldName = sessionStorage.getItem('PrevFocusedColumn');
                            if(s.batchEditApi.HasChanges(e.visibleIndex) && fieldName=='Surcharge')
                            {
                                if (e.visibleIndex != '-1')
                                 {
                                    s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                                    RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', 'ValuesChange|' + Oidvalue +'|'+  s.cpViewID, '', false); 
                                      }); 
                                 }
                               
                            }
                           else if(s.batchEditApi.HasChanges(e.visibleIndex) && fieldName=='SurchargePrice')
                            {
                                if (e.visibleIndex != '-1')
                                 {
                                    s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                                    RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', 'SurchargePrice|' + Oidvalue +'|'+  s.cpViewID, '', false); 
                                      }); 
                                 }
                               
                            }
                            if( s.batchEditApi.HasChanges(e.visibleIndex) && fieldName == 'SurchargePrice')
                            {
                               var unitprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'SurchargePrice');
                               if(unitprice < 0)
                               {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'SurchargePrice', 0);
                                    alert('Not allowed negative value.');
                               }
                            }
                            }, 20); }";
                        //gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                        //                if( e.focusedColumn.fieldName == 'Surcharge')
                        //                      {
                        //                       var IsDefault = s.batchEditApi.GetCellValue(e.visibleIndex,'Priority.Default');
                        //                       console.log(IsDefault);
                        //                       if(IsDefault==true)
                        //                          {
                        //                             e.cancel = true;
                        //                          }
                        //                          else
                        //                          {
                        //                              e.cancel = false;
                        //                          }
                        //                      }
                        //                  else
                        //                       {
                        //                            e.cancel = false;
                        //                       }
                        //                   }";
                    }
                }
                else if (View.Id == "TurnAroundTime_ListView_TestPriceSurcharge")
                {
                    ASPxGridListEditor gridlist = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlist != null)
                    {
                        if (Isparameter == false)
                        {
                            gridlist.Grid.Load += Grid_Load;
                        }
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("TurnAroundTimeSelection", this);
                        gridlist.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'TurnAroundTimeSelection', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'TurnAroundTimeSelection', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'TurnAroundTimeSelection', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'TurnAroundTimeSelection', 'UNSelectall', '', false);                        
                      }                      
                    }";
                    }
                }
                else if (View.Id == "TestPriceSurcharge_ListView_Edit")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("TestPriceSurcharge", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.HtmlDataCellPrepared += Gv_HtmlDataCellPrepared;
                        }
                    }
                    foreach (TestPriceSurcharge objsurcharge in ((ListView)View).CollectionSource.List)
                    {
                        testpricesurinfo.lsttpsTATedit = objsurcharge.TAT;
                    }
                }
                else if (View.Id == "TestPriceSurcharge_DetailView")
                {
                    TestPriceSurcharge objtps = (TestPriceSurcharge)View.CurrentObject;
                    DashboardViewItem dvtestmethod = ((DetailView)View).FindItem("Testmethods") as DashboardViewItem;
                    if (objtps != null && objtps.Test != null && objtps.Matrix != null && objtps.IsGroup == true)
                    {
                        if (dvtestmethod != null && dvtestmethod.InnerView != null)
                        {
                            List<Guid> lstgmt = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtps.Test.Oid)).Select(i => i.Oid).ToList();
                            if (lstgmt != null && lstgmt.Count > 0)
                            {
                                ((ListView)dvtestmethod.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstgmt);
                            }
                            else
                            {
                                ((ListView)dvtestmethod.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                            }
                        }
                    }
                }
                else if (View.Id == "Priority_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid.Columns["Sort"] != null)
                    {
                        gridListEditor.Grid.Columns["Sort"].CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    }
                }
                //if (View.Id == "DefaultPricing_DetailView")
                //{
                //    DefaultPricing objtps = (DefaultPricing)View.CurrentObject;
                //    if (objtps.Matrix != null && objtps.Test != null && objtps.IsGroup == ISGroupType.Yes)
                //    {
                //        TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [IsGroup] = 'True'", objtps.Matrix.MatrixName, objtps.Test.TestName));
                //        if (objtm != null)
                //        {
                //            DashboardViewItem gview = ((DetailView)View).FindItem("Testmethods") as DashboardViewItem;
                //            if (gview != null && gview.InnerView != null)
                //            {
                //                ((ListView)gview.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid);
                //                ((ListView)gview.InnerView).Refresh();
                //            }
                //        }
                //    }
                //    else if (objtps.Matrix != null && objtps.Test != null && objtps.Method != null && objtps.Component != null)
                //    {
                //        TestMethod objtmparams = ObjectSpace.GetObjectByKey<TestMethod>(objtps.Test.Oid);
                //        //TestMethod objtmparams = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [MethodName.MethodNumber] = ? ", objtps.Matrix.MatrixName, objtps.Test.TestName, objtps.Method.MethodNumber));
                //        if (objtmparams != null)
                //        {
                //            Component objcomp = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtmparams.Oid));
                //            if (objcomp != null)
                //            {
                //                DashboardViewItem gview = ((DetailView)View).FindItem("ParameterView") as DashboardViewItem;
                //                if (gview != null && gview.InnerView != null)
                //                {
                //                    ((ListView)gview.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objtmparams.MatrixName.MatrixName, objtmparams.TestName, objtmparams.MethodName.MethodNumber, objcomp.Components);
                //                    ((ListView)gview.InnerView).Refresh();
                //                }
                //            }
                //            else
                //            {
                //                Component objcompdef = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                //                if (objcompdef != null)
                //                {
                //                    DashboardViewItem gview = ((DetailView)View).FindItem("ParameterView") as DashboardViewItem;
                //                    if (gview != null && gview.InnerView != null)
                //                    {
                //                        ((ListView)gview.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objtmparams.MatrixName.MatrixName, objtmparams.TestName, objtmparams.MethodName.MethodNumber, objcompdef.Components);
                //                        ((ListView)gview.InnerView).Refresh();
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    else if (objtps != null && objtps.Test == null)
                //    {
                //        objtps.IsGroup = ISGroupType.No;
                //    }
                //}
                //if (View.Id == "ConstituentPricing_DetailView" || View.Id == "TestPriceSurcharge_DetailView" || View.Id == "DefaultPricing_DetailView")
                //{
                //    if (View.Id == "ConstituentPricing_DetailView")
                //    {
                //        ConstituentPricing objcp = (ConstituentPricing)View.CurrentObject;
                //        if (objcp != null && objcp.Matrix != null && objcp.Test != null && objcp.Method != null && objcp.Component != null)
                //        {
                //            DashboardViewItem dvparameterview = ((DetailView)View).FindItem("Parameterview") as DashboardViewItem;
                //            if (dvparameterview != null && dvparameterview.InnerView == null)
                //            {
                //                dvparameterview.CreateControl();
                //            }
                //            if (dvparameterview != null && dvparameterview.InnerView != null)
                //            {
                //                List<Guid> lstoid = new List<Guid>();
                //                //List<Testparameter> lsttstpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'  ", objcp.Matrix.MatrixName, objcp.Test.TestName, objcp.Method.MethodNumber, objcp.Component.Components)).ToList();
                //                List<Testparameter> lsttstpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Test.Oid, objcp.Component.Components)).ToList();
                //                foreach (Testparameter objtstpara in lsttstpara.ToList())
                //                {
                //                    if (!lstoid.Contains(objtstpara.Oid))
                //                    {
                //                        lstoid.Add(objtstpara.Oid);
                //                    }
                //                }
                //                if (lstoid.Count > 0)
                //                {
                //                    ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstoid);
                //                }
                //                else
                //                {
                //                    ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                //                }
                //            }
                //        }
                //    }
                //    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated += ChangeLayoutGroupCaptionViewController_ItemCreated;
                //}
                else if (View.Id == "Testparameter_LookupListView_ViewPopup")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RefreshAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "ConstituentPricingTier_ListView" || View.Id == "ConstituentPricing_ConstituentPricingTiers_ListView")
                {
                    ConstituentPricing objconstituent = (ConstituentPricing)Application.MainWindow.View.CurrentObject;
                    if (objconstituent != null)
                    {
                        IList<ConstituentPricingTier> lstcpteir = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing] = ?", objconstituent.Oid));
                        foreach (ConstituentPricingTier objtier in lstcpteir)
                        {
                            ((ListView)View).CollectionSource.Add(objtier);
                            ((ListView)View).Refresh();
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
        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridview = (ASPxGridView)sender;
                if (View.Id == "TestPriceSurcharge_ListView_newlist")
                {
                    if (testpricesurinfo.lsttpssurchargedefTAT != null)
                    {
                        gridview.JSProperties["cpVisibleRowCount"] = gridview.VisibleRowCount;
                    }
                    var selectionBoxColumn = gridview.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                    if (selectionBoxColumn != null)
                    {
                        selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                    }
                    ////for (int i = 0; i <= gridview.VisibleRowCount - 1; i++)
                    ////{
                    ////    string strcontainer = gridview.GetRowValues(i, "Oid").ToString();
                    ////    if (!string.IsNullOrEmpty(strcontainer))
                    ////    {
                    ////        foreach (TestPriceSurcharge objtstsurcharge in testpricesurinfo.lsttpssurchargedefTAT.Cast<TestPriceSurcharge>().Where(a => a.Oid == new Guid(strcontainer)).ToList())
                    ////        {
                    ////            gridview.Selection.SelectRow(i);
                    ////            //if (!testpricesurinfo.lsttpsTAT.Contains(strcontainer.Trim()))
                    ////            //{
                    ////            //    testpricesurinfo.lsttpsTAT.Add(strcontainer.Trim());
                    ////            //}
                    ////        }


                    ////    }
                    ////}
                }
                else if (View.Id == "TurnAroundTime_ListView_TestPriceSurcharge" && Isparameter == false)
                {
                    var selectionBoxColumn = gridview.Columns.OfType<GridViewCommandColumn>().Where(i => i.ShowSelectCheckbox).FirstOrDefault();
                    if (selectionBoxColumn != null)
                    {
                        selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                    }
                    if (!Isseleted)
                    {
                        gridview.JSProperties["cpVisibleRowCount"] = gridview.VisibleRowCount;
                        List<string> lststrcontainer = new List<string>();
                        testpricesurinfo.lsttpsTAT = new List<string>();
                        if (Application.MainWindow.View.Id == "TestPriceSurcharge_ListView_newlist")
                        {
                            TestPriceSurcharge objconset = Application.MainWindow.View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objconset != null && !string.IsNullOrEmpty(objconset.TAT))
                            {
                                string[] strarr = objconset.TAT.Split(';');
                                foreach (string objstr in strarr)
                                {
                                    TurnAroundTime objtat = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT]= ?", objstr.Trim()));
                                    if (objtat != null)
                                    {
                                        lststrcontainer.Add(objtat.TAT.Trim());
                                    }
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(testpricesurinfo.lsttpsTATedit))
                        {
                            string[] strarr = testpricesurinfo.lsttpsTATedit.Split(';');
                            foreach (string objstr in strarr)
                            {
                                lststrcontainer.Add(objstr.Trim());
                            }
                        }
                        for (int i = 0; i <= gridview.VisibleRowCount - 1; i++)
                        {
                            string strcontainer = gridview.GetRowValues(i, "TAT").ToString();
                            if (!string.IsNullOrEmpty(strcontainer) && lststrcontainer.Contains(strcontainer.Trim()))
                            {
                                gridview.Selection.SelectRow(i);
                                if (!testpricesurinfo.lsttpsTAT.Contains(strcontainer.Trim()))
                                {
                                    testpricesurinfo.lsttpsTAT.Add(strcontainer.Trim());
                                }
                            }
                            Isseleted = true;
                        }

                    }
                }
                else if (View.Id == "ConstituentPricingTier_ListView" || View.Id == "ConstituentPricing_ConstituentPricingTiers_ListView")
                {
                    quotesinfo.IsobjChangedproperty = false;
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
            try
            {
                if (View.Id == "TestPriceSurcharge_ListView_newlist")
                {
                    testpricesurinfo.IsNotRegularSelectAll = false;
                    if (testpricesurinfo.lsttpssurchargedefTAT != null)
                    {
                        testpricesurinfo.lsttpssurchargedefTAT.Clear();
                    }
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                else if (View.Id == "ConstituentPricingTier_ListView" || View.Id == "ConstituentPricing_ConstituentPricingTiers_ListView")
                {
                    quotesinfo.IsobjChangedproperty = false;
                }
                else if (View.Id == "PretreatmentPricing_DetailView" || View.Id == "PretreatmentPricing_ListView" || View.Id == "GroupTestSurcharge_DetailView" || View.Id == "GroupTestSurcharge_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executed -= NewObjectAction_Executed;
                }
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                if (View.Id == "TestPriceSurcharge_ListView" || View.Id == "TestPriceSurcharge_DetailView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Execute -= NewObjectAction_Execute;
                    if (View.Id == "TestPriceSurcharge_ListView")
                    {
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>().FullTextFilterAction.Executing -= FullTextFilterAction_Executing;
                        Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>().CustomGetFullTextSearchProperties -= new EventHandler<CustomGetFullTextSearchPropertiesEventArgs>(standardFilterController_CustomGetFullTextSearchProperties);
                    }
                }
                else if (View.Id == "PretreatmentPricing_DetailView" || View.Id == "TestPriceSurcharge_ListView" || View.Id == "PretreatmentPricing_ListView")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                }
                if (View.Id == "TestPriceSurcharge_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        gv.HtmlDataCellPrepared -= Gv_HtmlDataCellPrepared;
                    }
                    if (View.Id == "TurnAroundTime_ListView_TestPriceSurcharge")
                    {
                        Isparameter = false;
                    }
                }
                if (View.Id == "ConstituentPricing_DetailView" || View.Id == "TestPriceSurcharge_DetailView" || View.Id == "DefaultPricing_DetailView")
                {
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated -= ChangeLayoutGroupCaptionViewController_ItemCreated;
                }
                if (View is DetailView)
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed -= SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executed -= SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAction.Executed -= SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAction.Execute -= SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Execute -= SaveAndNewAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Execute -= SaveAndCloseAction_Execute;
                }
                if (View.Id == "TestPriceSurcharge_ListView_newlist")
                {
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>().FullTextFilterAction.Executing -= FullTextFilterAction_Executing;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        #endregion

        #region Events
        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TestPriceSurcharge_DetailView" && !string.IsNullOrEmpty(priceinfo.strTPSpricecode))
                {
                    TestPriceCode crttpc = ObjectSpace.CreateObject<TestPriceCode>();
                    crttpc.PriceCode = priceinfo.strTPSpricecode;
                    priceinfo.strTPSpricecode = string.Empty;
                    ObjectSpace.CommitChanges();
                }
                if (View.Id == "ConstituentPricing_DetailView" && !string.IsNullOrEmpty(priceinfo.strCPpricecode))
                {

                    TestPriceCode crttpc = ObjectSpace.CreateObject<TestPriceCode>();
                    crttpc.PriceCode = priceinfo.strCPpricecode;
                    priceinfo.strCPpricecode = string.Empty;
                    ObjectSpace.CommitChanges();
                }
                if (View.Id == "PretreatmentPricing_DetailView" && !string.IsNullOrEmpty(priceinfo.strPPpricecode))
                {
                    TestPriceCode crttpc = ObjectSpace.CreateObject<TestPriceCode>();
                    crttpc.PriceCode = priceinfo.strPPpricecode;
                    priceinfo.strPPpricecode = string.Empty;
                    ObjectSpace.CommitChanges();
                }
                //if (View.Id == "ConstituentPricing_DetailView")
                //{
                //    DashboardViewItem view = ((DetailView)View).FindItem("ConstituentPricing") as DashboardViewItem;
                //    if (priceinfo.lstselectremove != null && priceinfo.lstselectremove.Count > 0)
                //    {
                //        foreach (ConstituentPricingTier objcontier in priceinfo.lstselectremove.ToList())
                //        {
                //            ((ListView)view.InnerView).ObjectSpace.Delete(objcontier);
                //            ((ListView)view.InnerView).ObjectSpace.CommitChanges();
                //        }
                //        priceinfo.lstselectremove.Clear();
                //    }

                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndNewAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TestPriceSurcharge_DetailView" && !string.IsNullOrEmpty(priceinfo.strTPSpricecode))
                {
                    TestPriceCode crttpc = ObjectSpace.CreateObject<TestPriceCode>();
                    crttpc.PriceCode = priceinfo.strTPSpricecode;
                    priceinfo.strTPSpricecode = string.Empty;
                    ObjectSpace.CommitChanges();
                }
                if (View.Id == "ConstituentPricing_DetailView" && !string.IsNullOrEmpty(priceinfo.strCPpricecode))
                {
                    TestPriceCode crttpc = ObjectSpace.CreateObject<TestPriceCode>();
                    crttpc.PriceCode = priceinfo.strCPpricecode;
                    priceinfo.strCPpricecode = string.Empty;
                    ObjectSpace.CommitChanges();
                }
                if (View.Id == "PretreatmentPricing_DetailView" && !string.IsNullOrEmpty(priceinfo.strPPpricecode))
                {
                    TestPriceCode crttpc = ObjectSpace.CreateObject<TestPriceCode>();
                    crttpc.PriceCode = priceinfo.strPPpricecode;
                    priceinfo.strPPpricecode = string.Empty;
                    ObjectSpace.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndCloseAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TestPriceSurcharge_ListView" && !string.IsNullOrEmpty(priceinfo.strTPSpricecode))
                {
                    TestPriceCode crttpc = ObjectSpace.CreateObject<TestPriceCode>();
                    crttpc.PriceCode = priceinfo.strTPSpricecode;
                    priceinfo.strTPSpricecode = string.Empty;
                    ObjectSpace.CommitChanges();
                }
                if (View.Id == "ConstituentPricing_ListView" && !string.IsNullOrEmpty(priceinfo.strCPpricecode))
                {
                    TestPriceCode crttpc = ObjectSpace.CreateObject<TestPriceCode>();
                    crttpc.PriceCode = priceinfo.strCPpricecode;
                    priceinfo.strCPpricecode = string.Empty;
                    ObjectSpace.CommitChanges();
                }
                if (View.Id == "PretreatmentPricing_ListView" && !string.IsNullOrEmpty(priceinfo.strPPpricecode))
                {
                    TestPriceCode crttpc = ObjectSpace.CreateObject<TestPriceCode>();
                    crttpc.PriceCode = priceinfo.strPPpricecode;
                    priceinfo.strPPpricecode = string.Empty;
                    ObjectSpace.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void NewObjectAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "PretreatmentPricing_DetailView" || View.Id == "PretreatmentPricing_ListView" || View.Id == "GroupTestSurcharge_DetailView" || View.Id == "GroupTestSurcharge_ListView")
                {
                    priceinfo.ISPPNew = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewObjectAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                //if (View.Id == "DefaultPricing_ListView" || View.Id == "DefaultPricing_DetailView")
                //{
                //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                //    CollectionSource cs = new CollectionSource(objectSpace, typeof(DefaultPricing));
                //    ListView listview = Application.CreateListView("DefaultPricing_ListView_Copy", cs, true);
                //    e.ShowViewParameters.CreatedView = listview;
                //}
                if (View.Id == "TestPriceSurcharge_ListView" || View.Id == "TestPriceSurcharge_DetailView")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(TestPriceSurcharge));
                    ListView listview = Application.CreateListView("TestPriceSurcharge_ListView_newlist", cs, true);
                    e.ShowViewParameters.CreatedView = listview;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                string strpricecode = string.Empty;
                string[] strpricecodearr = null;

                if (View.Id == "PretreatmentPricing_DetailView" && View.CurrentObject != null)
                {
                    IObjectSpace ospp = Application.CreateObjectSpace(typeof(PretreatmentPricing));
                    if (priceinfo.ISPPNew == true)
                    {
                        PretreatmentPricing curtpretret = (PretreatmentPricing)View.CurrentObject;
                        if (curtpretret != null && curtpretret.Matrix != null && curtpretret.PrepMethod != null && curtpretret.Test != null)
                        {
                            PretreatmentPricing objpretret = ospp.FindObject<PretreatmentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test] = ? And[PrepMethod.Method.MethodNumber] = ?", curtpretret.Matrix.MatrixName, curtpretret.Test, curtpretret.PrepMethod.Method.MethodNumber));
                            if (objpretret != null)
                            {
                                Application.ShowViewStrategy.ShowMessage("Selected matrix,test,method details already exists", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                priceinfo.ISPPunique = false;
                                priceinfo.ISPPNew = false;
                                e.Cancel = true;
                            }
                        }
                    }
                }
                //if (View.Id == "GroupTestSurcharge_DetailView" && View.CurrentObject != null)
                //{
                //    if (priceinfo.ISPPNew == true)
                //    {
                //        GroupTestSurcharge curtgrptestsur = (GroupTestSurcharge)View.CurrentObject;
                //        if (curtgrptestsur != null)
                //        {
                //            GroupTestSurcharge objpretret = ObjectSpace.FindObject<GroupTestSurcharge>(CriteriaOperator.Parse("[TAT] = ? And [Test] = ? And [TestGroup.TestGroupName] = ?", curtgrptestsur.TAT, curtgrptestsur.TestGroup.TestGroupName));
                //            if (objpretret != null)
                //            {
                //                Application.ShowViewStrategy.ShowMessage("Selected testgroup and TAT details already exists", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //                priceinfo.ISPPunique = false;
                //                priceinfo.ISPPNew = false;
                //                e.Cancel = true;
                //            }
                //        }
                //    }
                //}
                if (View.Id == "TestPriceSurcharge_DetailView" && View.CurrentObject != null)
                {
                    IObjectSpace ostps = Application.CreateObjectSpace(typeof(TestPriceSurcharge));
                    TestPriceSurcharge curttps = (TestPriceSurcharge)View.CurrentObject;
                    if (curttps != null && curttps.PriceCode != null)
                    {
                        List<TestPriceSurcharge> lsttestprice = ostps.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[PriceCode] = ?", curttps.PriceCode)).ToList();
                        foreach (TestPriceSurcharge objtstpricecode in lsttestprice.ToList())
                        {
                            if (curttps.Oid != objtstpricecode.Oid)
                            {
                                if (objtstpricecode.TAT.ToString() == curttps.TAT.ToString())
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectedTATAlreadysave"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    e.Cancel = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                //if (View.Id == "ConstituentPricing_DetailView")
                //{
                //    ConstituentPricing objcp = (ConstituentPricing)View.CurrentObject;
                //    if (objcp != null)
                //    {
                //        if (objcp.Matrix == null && objcp.IsGroup == false)
                //        {
                //            e.Cancel = true;
                //            Application.ShowViewStrategy.ShowMessage("Enter the Matrix, Test, Method, Component", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //        }
                //        else if (objcp.Test == null && objcp.IsGroup == false)
                //        {
                //            e.Cancel = true;
                //            Application.ShowViewStrategy.ShowMessage("Enter the Test, Method, Component", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //        }
                //        else if (objcp.Method == null && objcp.IsGroup == false)
                //        {
                //            e.Cancel = true;
                //            Application.ShowViewStrategy.ShowMessage("Enter the Method, Component", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //        }
                //        else if (objcp.Component == null && objcp.IsGroup == false)
                //        {
                //            e.Cancel = true;
                //            Application.ShowViewStrategy.ShowMessage("Enter the Component", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //        }
                //        else
                //        {
                //            int paramscnt = 0;
                //            bool istierpriceempty = false;
                //            if (objcp.ChargeType == Modules.BusinessObjects.Setting.Quotes.ChargeType.Parameter)
                //            {
                //                List<Testparameter> lstobjtmcnt = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Test.Oid, objcp.Component.Components)).ToList(); //Matrix.MatrixName,objcp.Test.TestName,objcp.Method.MethodNumber,objcp.Component.Components //[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And 
                //                DashboardViewItem dvview = ((DetailView)View).FindItem("ConstituentPricing") as DashboardViewItem;
                //                if (dvview != null && dvview.InnerView != null)
                //                {
                //                    ((ASPxGridListEditor)((ListView)dvview.InnerView).Editor).Grid.UpdateEdit();
                //                    paramscnt = Convert.ToInt32(((ListView)dvview.InnerView).CollectionSource.List.Cast<ConstituentPricingTier>().Max(i => i.To));
                //                    if (paramscnt != lstobjtmcnt.Count)
                //                    {
                //                        e.Cancel = true;
                //                        Application.ShowViewStrategy.ShowMessage("Please check the column To details, Its not match to parameter's count ", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //                    }
                //                    else
                //                    {
                //                        List<ConstituentPricingTier> objconsprice = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ?", objcp.Oid)).ToList();
                //                        foreach (ConstituentPricingTier objDvview in ((ListView)dvview.InnerView).CollectionSource.List.Cast<ConstituentPricingTier>().Where(i => i.TierPrice <= 0).ToList())
                //                        {
                //                            istierpriceempty = true;
                //                            break;
                //                        }
                //                        if (istierpriceempty == true)
                //                        {
                //                            e.Cancel = true;
                //                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EnterPrice"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //                        }
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                DashboardViewItem dvview = ((DetailView)View).FindItem("ConstituentPricing") as DashboardViewItem;
                //                if (dvview != null && dvview.InnerView != null)
                //                {
                //                    ((ASPxGridListEditor)((ListView)dvview.InnerView).Editor).Grid.UpdateEdit();
                //                    if (((ListView)dvview.InnerView).CollectionSource.List.Count > 0)
                //                    {
                //                        foreach (ConstituentPricingTier objDvview in ((ListView)dvview.InnerView).CollectionSource.List.Cast<ConstituentPricingTier>().Where(i => i.TierPrice <= 0).ToList())
                //                        {
                //                            istierpriceempty = true;
                //                            break;
                //                        }
                //                        if (istierpriceempty == true)
                //                        {
                //                            e.Cancel = true;
                //                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EnterPrice"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //                        }
                //                    }
                //                    else
                //                    {
                //                        e.Cancel = true;
                //                        Application.ShowViewStrategy.ShowMessage("Add tier price ", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //                    }
                //                }
                //            }
                //            ////DashboardViewItem view = ((DetailView)View).FindItem("ConstituentPricing") as DashboardViewItem;
                //            ////if (view != null && view.InnerView != null)
                //            ////{
                //            ////    if (((ListView)view.InnerView).CollectionSource.List.Count > 0)
                //            ////    {
                //            ////        ((ASPxGridListEditor)((ListView)view.InnerView).Editor).Grid.UpdateEdit();
                //            ////        foreach (ConstituentPricingTier objRemoveDP1 in ((ListView)view.InnerView).CollectionSource.List.Cast<ConstituentPricingTier>().ToList())
                //            ////        {
                //            ////            if (objRemoveDP1.TotalTierPrice == 0)
                //            ////            {
                //            ////                empty = false;
                //            ////            }
                //            ////        }
                //            ////        if (empty == true)
                //            ////        {
                //            ////            //if (((ListView)view.InnerView).CollectionSource.GetCount() > 0)
                //            ////            //{
                //            ////            //    foreach (ConstituentPricingTier objcontier in ((ListView)view.InnerView).CollectionSource.List)
                //            ////            //    {
                //            ////            //        objcontier.ConstituentPricing = view.InnerView.ObjectSpace.GetObjectByKey<ConstituentPricing>(objcontier.Oid);
                //            ////            //    }
                //            ////            //    ((ListView)view.InnerView).ObjectSpace.CommitChanges();
                //            ////            //    view.InnerView.ObjectSpace.CommitChanges();
                //            ////            //}
                //            ////            if (((ASPxGridListEditor)((ListView)view.InnerView).Editor).Grid != null)
                //            ////            {
                //            ////                ((ASPxGridListEditor)((ListView)view.InnerView).Editor).Grid.UpdateEdit();
                //            ////            }
                //            ////        }
                //            ////        else
                //            ////        {
                //            ////            e.Cancel = true;
                //            ////            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EnterPrice"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //            ////        }
                //            ////    }
                //            ////    else
                //            ////    {
                //            ////        e.Cancel = true;
                //            ////        Application.ShowViewStrategy.ShowMessage("Add tier price ", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //            ////    }
                //            ////}
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

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (base.View != null && e.NewValue != e.OldValue)
                {
                    if (View.Id == "TestPriceSurcharge_DetailView" && View.CurrentObject == e.Object)
                    {
                        TestPriceSurcharge objtps = (TestPriceSurcharge)e.Object;
                        if (e.PropertyName == "Test" && objtps.Matrix != null && objtps.Test != null)
                        {
                            TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [IsGroup] = 'True'", objtps.Matrix.MatrixName, objtps.Test.TestName));
                            if (objtm != null)
                            {
                                DashboardViewItem gview = ((DetailView)View).FindItem("Testmethods") as DashboardViewItem;
                                if (gview != null && gview.InnerView != null)
                                {
                                    ((ListView)gview.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid);
                                    ((ListView)gview.InnerView).Refresh();
                                }
                            }
                        }
                        else if (objtps != null && objtps.Test == null)
                        {
                            objtps.IsGroup = false;
                        }
                    }
                    else if ((View.Id == "TestPriceSurcharge_ListView_Edit" || View.Id == "TestPriceSurcharge_ListView_newlist" || View.Id == "TestPriceSurcharge_ListView") && e.PropertyName == "Surcharge")
                    {
                        TestPriceSurcharge crtsurcharge = (TestPriceSurcharge)e.Object;
                        if (crtsurcharge != null && crtsurcharge.Surcharge < 0)
                        {
                            crtsurcharge.Surcharge = 0;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }
                        //if (crtsurcharge != null)
                        //{
                        //    DefaultPricing objdp = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[PriceCode] = ?", crtsurcharge.PriceCode));
                        //    if (objdp != null)
                        //    {
                        //        decimal surchargeperc = (int)crtsurcharge.Surcharge;
                        //        decimal testprice = objdp.UnitPrice;
                        //        decimal surchargeamt = 0;
                        //        if (surchargeperc > 0)
                        //        {
                        //            surchargeamt = testprice * (surchargeperc / 100);
                        //        }
                        //        crtsurcharge.SurchargePrice = testprice + surchargeamt;
                        //    }
                        //}
                    }
                    //if (View.Id == "GroupTestSurcharge_DetailView" && View.CurrentObject == e.Object && (e.PropertyName == "TestGroup"))
                    //{
                    //    priceinfo.tpsguidlst = new List<Guid>();
                    //    GroupTestSurcharge crtGroupTestSurcharge = (GroupTestSurcharge)e.Object;
                    //    if (crtGroupTestSurcharge != null)
                    //    {
                    //        //if (crtsurcharge.Matrix != null && crtsurcharge.Matrix.MatrixName != null && crtsurcharge.Test != null && crtsurcharge.Test.TestName != null && crtsurcharge.Method != null && crtsurcharge.Method.MethodName != null && crtsurcharge.Method.MethodName.MethodNumber != null && crtsurcharge.Component != null && crtsurcharge.Component.Component != null && crtsurcharge.Component.Component.Components != null)
                    //        if (crtGroupTestSurcharge.TestGroup != null && crtGroupTestSurcharge.TestGroup.TestGroupName != null)
                    //        {
                    //            IList<GroupTestSurcharge> lsttps = ObjectSpace.GetObjects<GroupTestSurcharge>(CriteriaOperator.Parse("[TestGroup.Oid] = ?", crtGroupTestSurcharge.TestGroup.Oid));
                    //            if (lsttps != null && lsttps.Count > 0)
                    //            {
                    //                priceinfo.tpsguidlst = new List<Guid>();
                    //                foreach (GroupTestSurcharge objtps in lsttps.ToList())
                    //                {
                    //                    string[] strtatarr = null;
                    //                    strtatarr = objtps.TAT.Split(';');
                    //                    foreach (string strtatobj in strtatarr.ToList())
                    //                    {
                    //                        if (!priceinfo.tpsguidlst.Contains(new Guid(strtatobj.Trim())))
                    //                        {
                    //                            priceinfo.tpsguidlst.Add(new Guid(strtatobj.Trim()));
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    else if (View.Id == "PretreatmentPricing_DetailView" && View.CurrentObject == e.Object && e.PropertyName == "UnitPrice")
                    {
                        PretreatmentPricing crtsurcharge = (PretreatmentPricing)e.Object;
                        if (crtsurcharge != null && crtsurcharge.UnitPrice < 0)
                        {
                            crtsurcharge.UnitPrice = 0;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    //if (View.Id == "GroupTestSurcharge_DetailView" && View.CurrentObject == e.Object && e.PropertyName == "Surcharge")
                    //{
                    //    GroupTestSurcharge crtsurcharge = (GroupTestSurcharge)e.Object;
                    //    if (crtsurcharge != null && crtsurcharge.Surcharge < 0)
                    //    {
                    //        crtsurcharge.Surcharge = 0;
                    //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    //    }
                    //}
                    //if ((View.Id == "ConstituentPricingTier_ListView" || View.Id == "ConstituentPricing_ConstituentPricingTiers_ListView") && e.Object != null && e.Object.GetType() == typeof(ConstituentPricingTier))
                    //{
                    //    ConstituentPricingTier objCP = (ConstituentPricingTier)e.Object;
                    //    ////if (e.PropertyName == "To")
                    //    ////{
                    //    int from = 0;
                    //    int to = 0;
                    //    bool IsLoop = false;
                    //    if (e.PropertyName == "To" && quotesinfo.IsobjChangedproperty == false)
                    //    {
                    //        quotesinfo.IsobjChangedproperty = true;
                    //        if (objCP.To < objCP.From)
                    //        {
                    //            objCP.To = objCP.From + 1;
                    //            Application.ShowViewStrategy.ShowMessage("To is greater then from", InformationType.Error, 3000, InformationPosition.Top);
                    //        }
                    //    }
                    //    if (e.PropertyName == "From" && quotesinfo.IsobjChangedproperty == false)
                    //    {
                    //        quotesinfo.IsobjChangedproperty = true;
                    //        if (constituentInfo.From > 0)
                    //        {
                    //            if (objCP.From < constituentInfo.From && objCP.To > constituentInfo.From)
                    //            {
                    //                objCP.From = objCP.To - 1;
                    //            }
                    //            if (objCP.From > objCP.To && objCP.To > 0)
                    //            {
                    //                objCP.From = objCP.To - 1;
                    //            }
                    //        }
                    //    }
                    //    ////    ////foreach (ConstituentPricingTier objcptier in ((ListView)View).CollectionSource.List.Cast<ConstituentPricingTier>().OrderBy(i => i.TierNo))
                    //    ////    ////{
                    //    ////    ////    if (from > 0 && to > 0 && IsLoop == false)
                    //    ////    ////    {
                    //    ////    ////        if (to >= objcptier.From)
                    //    ////    ////        {
                    //    ////    ////            IsLoop = true;
                    //    ////    ////            int conpricesub = Convert.ToInt16(objcptier.To - objcptier.From);
                    //    ////    ////            objcptier.From = Convert.ToUInt32(to + 1);
                    //    ////    ////            objcptier.To = Convert.ToUInt32(objcptier.To + (conpricesub));
                    //    ////    ////        }
                    //    ////    ////    }
                    //    ////    ////    else if (IsLoop == true)
                    //    ////    ////    {
                    //    ////    ////        int conpricesub = Convert.ToInt32(objcptier.To - objcptier.From);
                    //    ////    ////        objcptier.From = Convert.ToUInt32(to + 1);
                    //    ////    ////        objcptier.To = Convert.ToUInt32(objcptier.To + conpricesub);
                    //    ////    ////    }
                    //    ////    ////    from = Convert.ToInt32(objcptier.From);
                    //    ////    ////    to = Convert.ToInt32(objcptier.To);
                    //    ////    ////        }

                    //    ////}
                    //    ////ConstituentPricing objconsprice = (ConstituentPricing)Application.MainWindow.View.CurrentObject;
                    //    ////ConstituentPricingTier objCP = (ConstituentPricingTier)e.Object;
                    //    ////if (e.PropertyName == "TierPrice" || e.PropertyName == "Prep1Charge" || e.PropertyName == "Prep2Charge" /*|| e.PropertyName == "TotalTierPrice"*/)
                    //    ////{
                    //    ////    if (objCP.TierPrice >= 0 && objCP.Prep1Charge >= 0 && objCP.Prep2Charge >= 0 && objconsprice != null /*&& objconsprice.ChargeType == Modules.BusinessObjects.Setting.Quotes.ChargeType.Parameter*/)
                    //    ////    {
                    //    ////        if (objCP.TierPrice == 0)
                    //    ////        {
                    //    ////            objCP.Prep1Charge = 0;
                    //    ////            objCP.Prep2Charge = 0;
                    //    ////            objCP.TotalTierPrice = 0;
                    //    ////            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Entertierprice"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //    ////        }
                    //    ////        else
                    //    ////        {
                    //    ////            if (objCP.To > 1)
                    //    ////            {
                    //    ////                int inttosamplecnt = Convert.ToInt32(objCP.To);
                    //    ////                int inttotalprice = Convert.ToInt32(((objCP.To - objCP.From) + 1) * (objCP.TierPrice + objCP.Prep1Charge + objCP.Prep2Charge));
                    //    ////                objCP.TotalTierPrice = Math.Round(Convert.ToDecimal(inttotalprice), 2);
                    //    ////            }
                    //    ////            else
                    //    ////            {
                    //    ////                objCP.TotalTierPrice = 0;
                    //    ////            }
                    //    ////        }
                    //    ////        ////NestedFrame nestedFrame = (NestedFrame)Frame;
                    //    ////        ////CompositeView view = nestedFrame.ViewItem.View;
                    //    ////        ////foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                    //    ////        ////{
                    //    ////        ////    if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                    //    ////        ////    {
                    //    ////        ////        frameContainer.Frame.View.ObjectSpace.Refresh();
                    //    ////        ////    }
                    //    ////        ////}
                    //    ////    }
                    //    ////    else
                    //    ////    {
                    //    ////        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Negativenotallowed"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //    ////        if (e.PropertyName == "UnitPrice")
                    //    ////        {
                    //    ////            objCP.TierPrice = 0;
                    //    ////        }
                    //    ////        else if (e.PropertyName == "Prep1Charge")
                    //    ////        {
                    //    ////            objCP.Prep1Charge = 0;
                    //    ////        }
                    //    ////        else if (e.PropertyName == "Prep2Charge")
                    //    ////        {
                    //    ////            objCP.Prep2Charge = 0;
                    //    ////        }
                    //    ////        objCP.TotalTierPrice = objCP.TierPrice + objCP.Prep1Charge + objCP.Prep2Charge;
                    //    ////    }
                    //    ////}
                    //    ////if (e.PropertyName == "To" && quotesinfo.IsobjChangedproperty == false)
                    //    ////{
                    //    ////    quotesinfo.IsobjChangedproperty = true;
                    //    ////    if (objCP.To < objCP.From)
                    //    ////    {
                    //    ////        objCP.To = objCP.From + 1;
                    //    ////        Application.ShowViewStrategy.ShowMessage("To is greater then from", InformationType.Error, 3000, InformationPosition.Top);
                    //    ////    }
                    //    ////    else
                    //    ////    {
                    //    ////        if (objCP.TierPrice >= 0 && objCP.Prep1Charge >= 0 && objCP.Prep2Charge >= 0 && objconsprice != null/* && objconsprice.ChargeType == Modules.BusinessObjects.Setting.Quotes.ChargeType.Parameter*/)
                    //    ////        {
                    //    ////            if (objCP.To > 0 && objCP.From > 0)
                    //    ////            {
                    //    ////                int intconFromTo = (Convert.ToInt32(objCP.To - objCP.From)) + 1;
                    //    ////                int inttierprepprice = Convert.ToInt32(objCP.TierPrice + objCP.Prep1Charge + objCP.Prep2Charge); ;
                    //    ////                int inttotalprice = inttierprepprice * intconFromTo;
                    //    ////                objCP.TotalTierPrice = Math.Round(Convert.ToDecimal(inttotalprice), 2);
                    //    ////            }
                    //    ////        }
                    //    ////    }
                    //    ////}
                    //    ////if (e.PropertyName == "From" && quotesinfo.IsobjChangedproperty == false)
                    //    ////{
                    //    ////    quotesinfo.IsobjChangedproperty = true;
                    //    ////    if (constituentInfo.From > 0)
                    //    ////    {
                    //    ////        if (objCP.From < constituentInfo.From && objCP.To > constituentInfo.From)
                    //    ////        {
                    //    ////            objCP.From = objCP.To - 1;
                    //    ////        }
                    //    ////        if (objCP.From > objCP.To && objCP.To > 0)
                    //    ////        {
                    //    ////            objCP.From = objCP.To - 1;
                    //    ////        }
                    //    ////    }
                    //    ////    ////foreach (ConstituentPricingTier objcontier in ((ListView)View).CollectionSource.List.Cast<ConstituentPricingTier>().OrderBy(i => i.TierNo))
                    //    ////    ////{
                    //    ////    ////    if (objcontier.From <= objCP.From && objcontier.TierNo < objCP.TierNo)
                    //    ////    ////    {
                    //    ////    ////        objCP.From = Convert.ToUInt16(e.OldValue);
                    //    ////    ////    }
                    //    ////    ////}
                    //    ////    if (objCP.TierPrice >= 0 && objCP.Prep1Charge >= 0 && objCP.Prep2Charge >= 0 && objconsprice != null /*&& objconsprice.ChargeType == Modules.BusinessObjects.Setting.Quotes.ChargeType.Parameter*/)
                    //    ////    {
                    //    ////        if (objCP.From > 0 && objCP.To > 0)
                    //    ////        {
                    //    ////            int intconFromTo = (Convert.ToInt32(objCP.To - objCP.From)) + 1;
                    //    ////            int inttierprepprice = Convert.ToInt32(objCP.TierPrice + objCP.Prep1Charge + objCP.Prep2Charge); ;
                    //    ////            int inttotalprice = inttierprepprice * intconFromTo;
                    //    ////            objCP.TotalTierPrice = Math.Round(Convert.ToDecimal(inttotalprice), 2);
                    //    ////        }
                    //    ////    }
                    //    ////    ////NestedFrame nestedFrame = (NestedFrame)Frame;
                    //    ////    ////CompositeView view = nestedFrame.ViewItem.View;
                    //    ////    ////foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                    //    ////    ////{
                    //    ////    ////    if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                    //    ////    ////    {
                    //    ////    ////        frameContainer.Frame.View.ObjectSpace.Refresh();
                    //    ////    ////    }
                    //    ////    ////}
                    //    ////}
                    //}
                    //if (View.Id == "ConstituentPricing_DetailView" && View.CurrentObject == e.Object)
                    //{
                    //    DashboardViewItem dvparameterview = ((DetailView)View).FindItem("Parameterview") as DashboardViewItem;
                    //    ConstituentPricing objcp = (ConstituentPricing)e.Object;
                    //    if (e.PropertyName == "Matrix")
                    //    {
                    //        objcp.Test = null;
                    //        objcp.Method = null;
                    //        objcp.Component = null;
                    //    }
                    //    if (e.PropertyName == "Test")
                    //    {
                    //        if (objcp != null && objcp.Matrix != null && objcp.Test != null)
                    //        {
                    //            TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [IsGroup] = True", objcp.Matrix.MatrixName, objcp.Test.TestName));
                    //            if (objtm != null)
                    //            {
                    //                objcp.IsGroup = true;
                    //                DashboardViewItem dvgrptest = ((DetailView)View).FindItem("Testmethods") as DashboardViewItem;
                    //                if (dvgrptest != null && dvgrptest.InnerView != null)
                    //                {
                    //                    ((ListView)dvgrptest.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid);
                    //                    ((ListView)dvgrptest.InnerView).Refresh();
                    //                }

                    //            }
                    //            else
                    //            {
                    //                objcp.IsGroup = false;
                    //            }
                    //            ////DefaultPricing objdp = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = true", objcp.Matrix.MatrixName, objcp.Test.TestName));
                    //            ////if (objdp != null)
                    //            ////{
                    //            ////    Application.ShowViewStrategy.ShowMessage("Selected test already added in default pricing", InformationType.Info, timer.Seconds, InformationPosition.Top);
                    //            ////}
                    //            objcp.Method = null;
                    //            objcp.Component = null;
                    //        }
                    //        else if (objcp != null && objcp.Test == null)
                    //        {
                    //            objcp.IsGroup = false;
                    //        }
                    //        if (objcp.IsGroup != true)
                    //        {

                    //        }
                    //    }
                    //    if (e.PropertyName == "Method")
                    //    {
                    //        objcp.Component = null;
                    //    }
                    //    if (e.PropertyName == "Component")
                    //    {
                    //        if ((objcp.Matrix == null || objcp.Test == null || objcp.Method == null || objcp.Component == null) && objcp.ChargeType == Modules.BusinessObjects.Setting.Quotes.ChargeType.Parameter)
                    //        {
                    //            if (dvparameterview != null && dvparameterview.InnerView != null)
                    //            {
                    //                ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //            }
                    //        }
                    //        else if (objcp != null && objcp.Matrix != null && objcp.Test != null && objcp.Method != null && objcp.Component != null && objcp.ChargeType == Modules.BusinessObjects.Setting.Quotes.ChargeType.Parameter)
                    //        {
                    //            if (dvparameterview != null && dvparameterview.InnerView == null)
                    //            {
                    //                dvparameterview.CreateControl();
                    //            }
                    //            if (dvparameterview != null && dvparameterview.InnerView != null)
                    //            {
                    //                List<Guid> lstoid = new List<Guid>();
                    //                //List<Testparameter> lsttstpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'  ", objcp.Matrix.MatrixName, objcp.Test.TestName, objcp.Method.MethodNumber, objcp.Component.Components)).ToList();
                    //                List<Testparameter> lsttstpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Matrix.MatrixName, objcp.Test.TestName, objcp.Method.MethodNumber, objcp.Component.Components)).ToList(); //("[TestMethod.Oid] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Test.Oid, objcp.Component.Components)).ToList();
                    //                foreach (Testparameter objtstpara in lsttstpara.ToList())
                    //                {
                    //                    if (!lstoid.Contains(objtstpara.Oid))
                    //                    {
                    //                        lstoid.Add(objtstpara.Oid);
                    //                    }
                    //                }
                    //                if (lstoid.Count > 0)
                    //                {
                    //                    ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstoid);
                    //                }
                    //                else
                    //                {
                    //                    ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //                }
                    //                ((ListView)dvparameterview.InnerView).Refresh();
                    //            }
                    //        }
                    //        ////if (objcp.Matrix != null && objcp.Test != null && objcp.Method != null && objcp.Component != null)
                    //        ////{
                    //        ////    dvparameterview = ((DetailView)View).FindItem("Parameterview") as DashboardViewItem;
                    //        ////    if (dvparameterview != null && dvparameterview.InnerView == null)
                    //        ////    {
                    //        ////        dvparameterview.CreateControl();
                    //        ////    }
                    //        ////    if (dvparameterview != null && dvparameterview.InnerView != null)
                    //        ////    {
                    //        ////        List<Guid> lstoid = new List<Guid>();
                    //        ////        List<Testparameter> objtestparams = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Matrix.MatrixName, objcp.Test.TestName, objcp.Method.MethodNumber, objcp.Component.Components)).ToList();
                    //        ////        foreach (Testparameter objtstpara in objtestparams.ToList())
                    //        ////        {
                    //        ////            if (!lstoid.Contains(objtstpara.Oid))
                    //        ////            {
                    //        ////                lstoid.Add(objtstpara.Oid);
                    //        ////            }
                    //        ////        }
                    //        ////        if (lstoid.Count > 0)
                    //        ////        {
                    //        ////            ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstoid);
                    //        ////        }
                    //        ////        else
                    //        ////        {
                    //        ////            ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //        ////        }
                    //        ////        ////TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [MethodName.MethodNumber] = ?", objcp.Matrix.MatrixName, objcp.Test.TestName, objcp.Method.MethodNumber));
                    //        ////        ////if (objtm != null)
                    //        ////        ////{
                    //        ////        ////    Component obbjcomp = ObjectSpace.GetObjectByKey<Component>(objtm.Oid);
                    //        ////        ////    if (obbjcomp != null)
                    //        ////        ////    {
                    //        ////        ////        List<Testparameter> objtestparams = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Matrix.MatrixName, objcp.Test.TestName, objcp.Method.MethodNumber, obbjcomp.Components)).ToList();
                    //        ////        ////        foreach (Testparameter objtstpara in objtestparams.ToList())
                    //        ////        ////        {
                    //        ////        ////            if (!lstoid.Contains(objtstpara.Oid))
                    //        ////        ////            {
                    //        ////        ////                lstoid.Add(objtstpara.Oid);
                    //        ////        ////            }
                    //        ////        ////        }
                    //        ////        ////    }
                    //        ////        ////    else
                    //        ////        ////    {
                    //        ////        ////        List<Testparameter> objtestparams = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = 'Default' And [QCType.QCTypeName] = 'Sample'", objcp.Matrix.MatrixName, objcp.Test.TestName, objcp.Method.MethodNumber)).ToList();
                    //        ////        ////        foreach (Testparameter objtstpara in objtestparams.ToList())
                    //        ////        ////        {
                    //        ////        ////            if (!lstoid.Contains(objtstpara.Oid))
                    //        ////        ////            {
                    //        ////        ////                lstoid.Add(objtstpara.Oid);
                    //        ////        ////            }
                    //        ////        ////        }
                    //        ////        ////    }
                    //        ////        ////    if (lstoid.Count > 0)
                    //        ////        ////    {
                    //        ////        ////        ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstoid);
                    //        ////        ////    }
                    //        ////        ////    else
                    //        ////        ////    {
                    //        ////        ////        ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //        ////        ////    }
                    //        ////        ////}

                    //        ////    }
                    //        ////}
                    //    }
                    //    if ((objcp.Matrix == null || objcp.Test == null || objcp.Method == null || objcp.Component == null) && objcp.ChargeType == Modules.BusinessObjects.Setting.Quotes.ChargeType.Parameter)
                    //    {
                    //        if (dvparameterview != null && dvparameterview.InnerView != null)
                    //        {
                    //            ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //        }
                    //    }
                    //    else if (objcp != null && objcp.Matrix != null && objcp.Test != null && objcp.Method != null && objcp.Component != null && objcp.ChargeType == Modules.BusinessObjects.Setting.Quotes.ChargeType.Parameter)
                    //    {
                    //        if (dvparameterview != null && dvparameterview.InnerView == null)
                    //        {
                    //            dvparameterview.CreateControl();
                    //        }
                    //        if (dvparameterview != null && dvparameterview.InnerView != null)
                    //        {
                    //            List<Guid> lstoid = new List<Guid>();
                    //            //List<Testparameter> lsttstpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'  ", objcp.Matrix.MatrixName, objcp.Test.TestName, objcp.Method.MethodNumber, objcp.Component.Components)).ToList();
                    //            //List<Testparameter> lsttstpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Test.Oid, objcp.Component.Components)).ToList();
                    //            List<Testparameter> lsttstpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Matrix.MatrixName, objcp.Test.TestName, objcp.Method.MethodNumber, objcp.Component.Components)).ToList(); //("[TestMethod.Oid] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcp.Test.Oid, objcp.Component.Components)).ToList();
                    //            foreach (Testparameter objtstpara in lsttstpara.ToList())
                    //            {
                    //                if (!lstoid.Contains(objtstpara.Oid))
                    //                {
                    //                    lstoid.Add(objtstpara.Oid);
                    //                }
                    //            }
                    //            if (lstoid.Count > 0)
                    //            {
                    //                ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstoid);
                    //            }
                    //            else
                    //            {
                    //                ((ListView)dvparameterview.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //            }
                    //            ((ListView)dvparameterview.InnerView).Refresh();
                    //        }
                    //    }
                    //}
                    //if ((View.Id == "ConstituentPricingTier_ListView" || View.Id == "ConstituentPricing_ConstituentPricingTiers_ListView") && e.PropertyName == "TierPrice")
                    //{
                    //    ConstituentPricingTier crtconsprice = (ConstituentPricingTier)e.Object;
                    //    if (crtconsprice != null && crtconsprice.TierPrice < 0)
                    //    {
                    //        crtconsprice.TierPrice = 0;
                    //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    //    }
                    //}
                    //if ((View.Id == "DefaultPricing_ListView_Copy" || View.Id == "DefaultPricing_ListView") && e.PropertyName == "UnitPrice")
                    //{
                    //    DefaultPricing crtdefprice = (DefaultPricing)e.Object;
                    //    if (crtdefprice != null && crtdefprice.UnitPrice < 0)
                    //    {
                    //        crtdefprice.UnitPrice = 0;
                    //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    //    }
                    //}
                    else if (View.Id == "ItemChargePricing_DetailView" && View.CurrentObject == e.Object && e.PropertyName == "UnitPrice" || e.PropertyName == "Cancel")
                    {
                        ItemChargePricing objICP = (ItemChargePricing)e.Object;
                        if (e.PropertyName == "UnitPrice")
                        {
                            if (objICP != null && objICP.UnitPrice < 0)
                            {
                                objICP.UnitPrice = 0;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else if (e.PropertyName == "Cancel")
                        {
                            if (objICP.Cancel == true)
                            {
                                objICP.CancelBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objICP.CancelDate = DateTime.Now;
                            }
                            else
                            {
                                objICP.CancelBy = null;
                                objICP.CancelDate = DateTime.MinValue;
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

        private void PriceSaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ////foreach (DefaultPricing objRemoveDP1 in ((ListView)View).CollectionSource.List.Cast<DefaultPricing>().ToList())
                ////{
                ////    if (objRemoveDP1.TotalUnitPrice == 0)
                ////    {
                ////        ((ListView)View).CollectionSource.Remove(objRemoveDP1);
                ////        View.ObjectSpace.RemoveFromModifiedObjects(objRemoveDP1);
                ////    }
                ////}

                //if (View.Id == "DefaultPricing_ListView_Copy")
                //{
                //    IList<DefaultPricing> lstSelectedPrice = View.SelectedObjects.Cast<DefaultPricing>().ToList();
                //    if (View.SelectedObjects.Count > 0)
                //    {
                //        {
                //            foreach (DefaultPricing objUnselected in ((ListView)View).CollectionSource.List.Cast<DefaultPricing>().ToList())
                //            {
                //                if (!lstSelectedPrice.Contains(objUnselected))
                //                {
                //                    View.ObjectSpace.Delete(objUnselected);
                //                }
                //            }
                //            ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                //            foreach (DefaultPricing objPriceCode in lstSelectedPrice.ToList())
                //            {
                //                string strprcode = string.Empty;
                //                int prcode = 0;
                //                string pricecode = string.Empty;
                //                IObjectSpace objectSpace1 = Application.CreateObjectSpace();
                //                DefaultPricing chkdefprice = objectSpace1.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] =? And [Component.Components] = ?", objPriceCode.Matrix.MatrixName, objPriceCode.Test.TestName, objPriceCode.Method.MethodNumber, objPriceCode.Component.Components));
                //                if (chkdefprice == null)
                //                {
                //                    if (objPriceCode.Matrix != null && objPriceCode.Test != null && objPriceCode.Method != null)
                //                    {
                //                        CriteriaOperator estexpression = CriteriaOperator.Parse("Max(PriceCode)");
                //                        CriteriaOperator estfilter = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? ", objPriceCode.Matrix.MatrixName, objPriceCode.Test.TestName, objPriceCode.Method.MethodNumber);
                //                        var max = ((XPObjectSpace)ObjectSpace).Session.Evaluate<DefaultPricing>(estexpression, estfilter);
                //                        var maxconstiprice = ((XPObjectSpace)ObjectSpace).Session.Evaluate<ConstituentPricing>(estexpression, estfilter);
                //                        if (max != null)
                //                        {
                //                            if (max != null)
                //                            {
                //                                strprcode = max.ToString();
                //                            }
                //                            else if (maxconstiprice != null)
                //                            {
                //                                strprcode = maxconstiprice.ToString();
                //                            }
                //                            string[] strarr = strprcode.Split('-');
                //                            if (strarr != null)
                //                            {
                //                                prcode = Convert.ToInt32(strarr[1]);
                //                                strprcode = (prcode + 1).ToString();
                //                                if (!string.IsNullOrEmpty(strprcode) && strprcode.Length == 1)
                //                                {
                //                                    strprcode = "0" + strprcode;
                //                                }

                //                                objPriceCode.PriceCode = strarr[0] + "-" + strprcode;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            CriteriaOperator filter = CriteriaOperator.Parse("Not IsNullOrEmpty([PriceCode])");
                //                            CriteriaOperator expression = CriteriaOperator.Parse("Max(PriceCode)");
                //                            string max1 = (Convert.ToString(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(TestPriceCode), expression, filter))).ToString();
                //                            if (max1 != null && !string.IsNullOrEmpty(max1))
                //                            {
                //                                strprcode = max1.ToString();
                //                                if (!string.IsNullOrEmpty(strprcode))
                //                                {
                //                                    string[] strarr = strprcode.Split('-');
                //                                    if (strarr != null)
                //                                    {
                //                                        prcode = Convert.ToInt32(strarr[0]);
                //                                        strprcode = (prcode + 1).ToString();
                //                                        objPriceCode.PriceCode = strprcode + "-01";
                //                                    }
                //                                }
                //                            }
                //                            else
                //                            {
                //                                objPriceCode.PriceCode = "100-01";
                //                            }
                //                        }
                //                    }
                //                    else if (objPriceCode.Matrix != null && objPriceCode.Test != null)
                //                    {
                //                        CriteriaOperator estexpression = CriteriaOperator.Parse("Max(PriceCode)");
                //                        CriteriaOperator estfilter = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'Yes'", objPriceCode.Matrix.MatrixName, objPriceCode.Test.TestName);
                //                        var max = ((XPObjectSpace)ObjectSpace).Session.Evaluate<DefaultPricing>(estexpression, estfilter);
                //                        if (max != null)
                //                        {
                //                            strprcode = max.ToString();
                //                            string[] strarr = strprcode.Split('-');
                //                            if (strarr != null)
                //                            {
                //                                prcode = Convert.ToInt32(strarr[1]);
                //                                strprcode = (prcode + 1).ToString();
                //                                if (!string.IsNullOrEmpty(strprcode) && strprcode.Length == 1)
                //                                {
                //                                    strprcode = "0" + strprcode;
                //                                }

                //                                objPriceCode.PriceCode = strarr[0] + "-" + strprcode;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            CriteriaOperator filter = CriteriaOperator.Parse("Not IsNullOrEmpty([PriceCode])");
                //                            CriteriaOperator expression = CriteriaOperator.Parse("Max(PriceCode)");
                //                            string max1 = (Convert.ToString(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(TestPriceCode), expression, filter))).ToString();
                //                            if (max1 != null && !string.IsNullOrEmpty(max1))
                //                            {
                //                                strprcode = max1.ToString();
                //                                if (!string.IsNullOrEmpty(strprcode))
                //                                {
                //                                    string[] strarr = strprcode.Split('-');
                //                                    if (strarr != null)
                //                                    {
                //                                        prcode = Convert.ToInt32(strarr[0]);
                //                                        strprcode = (prcode + 1).ToString();
                //                                        objPriceCode.PriceCode = strprcode + "-01";
                //                                    }
                //                                }
                //                            }
                //                            else
                //                            {
                //                                objPriceCode.PriceCode = "100-01";
                //                            }
                //                        }
                //                    }

                //                    TestPriceCode objtpc = View.ObjectSpace.CreateObject<TestPriceCode>();
                //                    objtpc.PriceCode = objPriceCode.PriceCode;
                //                    View.ObjectSpace.CommitChanges();
                //                }
                //                else if (chkdefprice != null && string.IsNullOrEmpty(chkdefprice.PriceCode))
                //                {
                //                    if (objPriceCode.Matrix != null && objPriceCode.Test != null && objPriceCode.Method != null)
                //                    {
                //                        CriteriaOperator estexpression = CriteriaOperator.Parse("Max(PriceCode)");
                //                        CriteriaOperator estfilter = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? ", objPriceCode.Matrix.MatrixName, objPriceCode.Test.TestName, objPriceCode.Method.MethodNumber);
                //                        var max = ((XPObjectSpace)ObjectSpace).Session.Evaluate<DefaultPricing>(estexpression, estfilter);
                //                        var maxconstiprice = ((XPObjectSpace)ObjectSpace).Session.Evaluate<ConstituentPricing>(estexpression, estfilter);
                //                        if (max != null)
                //                        {
                //                            if (max != null)
                //                            {
                //                                strprcode = max.ToString();
                //                            }
                //                            else if (maxconstiprice != null)
                //                            {
                //                                strprcode = maxconstiprice.ToString();
                //                            }
                //                            string[] strarr = strprcode.Split('-');
                //                            if (strarr != null)
                //                            {
                //                                prcode = Convert.ToInt32(strarr[1]);
                //                                strprcode = (prcode + 1).ToString();
                //                                if (!string.IsNullOrEmpty(strprcode) && strprcode.Length == 1)
                //                                {
                //                                    strprcode = "0" + strprcode;
                //                                }

                //                                objPriceCode.PriceCode = strarr[0] + "-" + strprcode;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            CriteriaOperator filter = CriteriaOperator.Parse("Not IsNullOrEmpty([PriceCode])");
                //                            CriteriaOperator expression = CriteriaOperator.Parse("Max(PriceCode)");
                //                            string max1 = (Convert.ToString(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(TestPriceCode), expression, filter))).ToString();
                //                            if (max1 != null && !string.IsNullOrEmpty(max1))
                //                            {
                //                                strprcode = max1.ToString();
                //                                if (!string.IsNullOrEmpty(strprcode))
                //                                {
                //                                    string[] strarr = strprcode.Split('-');
                //                                    if (strarr != null)
                //                                    {
                //                                        prcode = Convert.ToInt32(strarr[0]);
                //                                        strprcode = (prcode + 1).ToString();
                //                                        objPriceCode.PriceCode = strprcode + "-01";
                //                                    }
                //                                }
                //                            }
                //                            else
                //                            {
                //                                objPriceCode.PriceCode = "100-01";
                //                            }
                //                        }
                //                    }
                //                    else if (objPriceCode.Matrix != null && objPriceCode.Test != null)
                //                    {
                //                        CriteriaOperator estexpression = CriteriaOperator.Parse("Max(PriceCode)");
                //                        CriteriaOperator estfilter = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'Yes'", objPriceCode.Matrix.MatrixName, objPriceCode.Test.TestName);
                //                        var max = ((XPObjectSpace)ObjectSpace).Session.Evaluate<DefaultPricing>(estexpression, estfilter);
                //                        if (max != null)
                //                        {
                //                            strprcode = max.ToString();
                //                            string[] strarr = strprcode.Split('-');
                //                            if (strarr != null)
                //                            {
                //                                prcode = Convert.ToInt32(strarr[1]);
                //                                strprcode = (prcode + 1).ToString();
                //                                if (!string.IsNullOrEmpty(strprcode) && strprcode.Length == 1)
                //                                {
                //                                    strprcode = "0" + strprcode;
                //                                }

                //                                objPriceCode.PriceCode = strarr[0] + "-" + strprcode;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            CriteriaOperator filter = CriteriaOperator.Parse("Not IsNullOrEmpty([PriceCode])");
                //                            CriteriaOperator expression = CriteriaOperator.Parse("Max(PriceCode)");
                //                            string max1 = (Convert.ToString(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(TestPriceCode), expression, filter))).ToString();
                //                            if (max1 != null && !string.IsNullOrEmpty(max1))
                //                            {
                //                                strprcode = max1.ToString();
                //                                if (!string.IsNullOrEmpty(strprcode))
                //                                {
                //                                    string[] strarr = strprcode.Split('-');
                //                                    if (strarr != null)
                //                                    {
                //                                        prcode = Convert.ToInt32(strarr[0]);
                //                                        strprcode = (prcode + 1).ToString();
                //                                        objPriceCode.PriceCode = strprcode + "-01";
                //                                    }
                //                                }
                //                            }
                //                            else
                //                            {
                //                                objPriceCode.PriceCode = "100-01";
                //                            }
                //                        }
                //                    }

                //                    TestPriceCode objtpc = View.ObjectSpace.CreateObject<TestPriceCode>();
                //                    objtpc.PriceCode = objPriceCode.PriceCode;
                //                    View.ObjectSpace.CommitChanges();
                //                }

                //            }
                //            View.Close();
                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //        }
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                //    }
                //}

                if (View.Id == "TestPriceSurcharge_ListView_newlist" && View.SelectedObjects.Count > 0)
                {
                    IObjectSpace objsp = Application.CreateObjectSpace();
                    IObjectSpace os = Application.CreateObjectSpace(typeof(TestPriceSurcharge));
                    List<TestPriceSurcharge> lstExistDefaultPrice = os.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[GCRecord] is null")).ToList();
                    IList<TestPriceSurcharge> lstSelectedPrice = View.SelectedObjects.Cast<TestPriceSurcharge>().ToList();
                    foreach (TestPriceSurcharge objUnselected in ((ListView)View).CollectionSource.List.Cast<TestPriceSurcharge>().ToList())
                    {
                        if (!lstSelectedPrice.Contains(objUnselected))
                        {
                            View.ObjectSpace.Delete(objUnselected);
                        }
                    }

                    foreach (TestPriceSurcharge objtestpricesurcharge in lstSelectedPrice.Where(i => i.IsGroup == false))
                    {
                        if (lstExistDefaultPrice.FirstOrDefault(i => i.Matrix != null && i.Test != null && i.Method != null && i.Component != null && i.Matrix.MatrixName == objtestpricesurcharge.Matrix.MatrixName && i.Test.TestName == objtestpricesurcharge.Test.TestName && i.Method.MethodName.MethodNumber == objtestpricesurcharge.Method.MethodName.MethodNumber && i.Component.Components == objtestpricesurcharge.Component.Components && i.Priority.Prioritys == objtestpricesurcharge.Priority.Prioritys) == null)
                        {
                            if (objtestpricesurcharge != null && !string.IsNullOrEmpty(objtestpricesurcharge.TAT))
                            {
                                string pricecode = string.Empty;
                                if (objtestpricesurcharge.Matrix != null && objtestpricesurcharge.Test != null && objtestpricesurcharge.Method != null)
                                {
                                    TestPriceSurcharge objFindPrice = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse(""));
                                    if (objFindPrice == null)
                                    {
                                        pricecode = "100-01-" + objtestpricesurcharge.PrioritySort.ToString();
                                    }
                                    else
                                    {
                                        List<TestPriceSurcharge> lsttstpricesurcharge = os.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? ", objtestpricesurcharge.Matrix.MatrixName, objtestpricesurcharge.Test.TestName, objtestpricesurcharge.Method.MethodName.MethodNumber)).ToList(); //And [Component.Components] = ? , objtestpricesurcharge.Component.Components
                                        if (lsttstpricesurcharge.Count > 0)
                                        {
                                            TestPriceSurcharge lstpr = lsttstpricesurcharge.FirstOrDefault(i => i.Component != null && objtestpricesurcharge != null && objtestpricesurcharge.Component != null && i.Component.Components == objtestpricesurcharge.Component.Components);
                                            if (lstpr != null)
                                            {
                                                pricecode = lstpr.PriceCode.Substring(0, 6);
                                                pricecode = pricecode + "-" + objtestpricesurcharge.PrioritySort.ToString();
                                            }
                                            else
                                            {
                                                lstpr = lsttstpricesurcharge.FirstOrDefault();
                                                pricecode = lstpr.PriceCode.Substring(0, 3);
                                                //string intpricecode=lstpr.PriceCode.Substring(5,7).ToString();
                                                List<string> intpricecode = lstpr.PriceCode.Split('-').ToList();
                                                pricecode = pricecode.ToString() + "-" + (Convert.ToInt32(intpricecode[1]) + 1).ToString("00") + "-" + objtestpricesurcharge.PrioritySort.ToString();
                                            }
                                        }
                                        else
                                        {
                                            pricecode = ((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TestPriceSurcharge), CriteriaOperator.Parse("Max(PriceCode)"), null).ToString();//Convert.ToInt32() + 1;
                                            pricecode = pricecode.Substring(0, 3);
                                            int intpricecode = Convert.ToInt32(pricecode) + 1;
                                            pricecode = intpricecode.ToString() + "-01-" + objtestpricesurcharge.PrioritySort.ToString();
                                        }
                                    }
                                }
                                TestPriceSurcharge objtps = os.CreateObject<TestPriceSurcharge>();
                                //pricecode = ((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TestPriceSurcharge), CriteriaOperator.Parse("Max(PriceCode)"), null).ToString();//Convert.ToInt32() + 1;

                                objtps.PriceCode = pricecode;
                                objtps.Matrix = os.GetObject(objtestpricesurcharge.Matrix);
                                objtps.Test = os.GetObject(objtestpricesurcharge.Test);
                                objtps.Method = os.GetObject(objtestpricesurcharge.Method);
                                objtps.IsGroup = os.GetObject(objtestpricesurcharge.IsGroup);
                                if (objtestpricesurcharge.IsGroup == true)
                                {
                                    objtps.IsGroup = true;
                                }
                                else
                                {
                                    objtps.IsGroup = false;
                                }
                                objtps.TAT = os.GetObject(objtestpricesurcharge.TAT);
                                objtps.Surcharge = os.GetObject(objtestpricesurcharge.Surcharge);
                                objtps.Component = os.GetObject(objtestpricesurcharge.Component);
                                objtps.Priority = os.GetObject(objtestpricesurcharge.Priority);
                                objtps.Remark = os.GetObject(objtestpricesurcharge.Remark);
                                objtps.SurchargePrice = os.GetObject(objtestpricesurcharge.SurchargePrice);
                                os.CommitChanges();
                            }
                            else if (string.IsNullOrEmpty(objtestpricesurcharge.TAT))
                            {
                                Application.ShowViewStrategy.ShowMessage("Select atleast one TAT", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                    foreach (TestPriceSurcharge objTestPriceSur in lstSelectedPrice.Where(i => i.IsGroup == true))
                    {
                        if (lstExistDefaultPrice.FirstOrDefault(i => i.Matrix != null && i.Test != null && i.Matrix.MatrixName == objTestPriceSur.Matrix.MatrixName && i.Test.TestName == objTestPriceSur.Test.TestName && i.Priority.Prioritys == objTestPriceSur.Priority.Prioritys) == null)
                        {
                            if (objTestPriceSur != null && !string.IsNullOrEmpty(objTestPriceSur.TAT))
                            {
                                string pricecode = string.Empty;
                                if (objTestPriceSur.Matrix != null && objTestPriceSur.Test != null)
                                {
                                    TestPriceSurcharge objFindPrice = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse(""));
                                    if (objFindPrice == null)
                                    {
                                        pricecode = "100-01-" + objTestPriceSur.PrioritySort.ToString();
                                    }
                                    else
                                    {
                                        List<TestPriceSurcharge> lsttstpricesurcharge = os.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = True", objTestPriceSur.Matrix.MatrixName, objTestPriceSur.Test.TestName)).ToList(); //And [Component.Components] = ? , objtestpricesurcharge.Component.Components
                                        if (lsttstpricesurcharge.Count > 0)
                                        {
                                            TestPriceSurcharge lstpr = lsttstpricesurcharge.FirstOrDefault(i => i.Test != null && i.Test.TestName == objTestPriceSur.Test.TestName && i.Matrix != null && i.Matrix.MatrixName == objTestPriceSur.Matrix.MatrixName);
                                            if (lstpr != null)
                                            {
                                                pricecode = lstpr.PriceCode.Substring(0, 6);
                                                pricecode = pricecode + "-" + objTestPriceSur.PrioritySort.ToString();
                                            }
                                        }
                                        else
                                        {
                                            pricecode = ((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TestPriceSurcharge), CriteriaOperator.Parse("Max(PriceCode)"), null).ToString();//Convert.ToInt32() + 1;
                                            pricecode = pricecode.Substring(0, 3);
                                            int intpricecode = Convert.ToInt32(pricecode) + 1;
                                            pricecode = intpricecode.ToString() + "-01-" + objTestPriceSur.PrioritySort.ToString();
                                        }
                                    }
                                }
                                TestPriceSurcharge objtps = os.CreateObject<TestPriceSurcharge>();
                                //pricecode = ((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TestPriceSurcharge), CriteriaOperator.Parse("Max(PriceCode)"), null).ToString();//Convert.ToInt32() + 1;

                                objtps.PriceCode = pricecode;
                                objtps.Matrix = os.GetObject(objTestPriceSur.Matrix);
                                objtps.Test = os.GetObject(objTestPriceSur.Test);
                                objtps.Method = os.GetObject(objTestPriceSur.Method);
                                objtps.IsGroup = os.GetObject(objTestPriceSur.IsGroup);
                                if (objTestPriceSur.IsGroup == true)
                                {
                                    objtps.IsGroup = true;
                                }
                                else
                                {
                                    objtps.IsGroup = false;
                                }
                                objtps.TAT = os.GetObject(objTestPriceSur.TAT);
                                objtps.Surcharge = os.GetObject(objTestPriceSur.Surcharge);
                                objtps.Component = os.GetObject(objTestPriceSur.Component);
                                objtps.Priority = os.GetObject(objTestPriceSur.Priority);
                                objtps.Remark = os.GetObject(objTestPriceSur.Remark);
                                objtps.SurchargePrice = os.GetObject(objTestPriceSur.SurchargePrice);
                                os.CommitChanges();
                            }
                            else if (string.IsNullOrEmpty(objTestPriceSur.TAT))
                            {
                                Application.ShowViewStrategy.ShowMessage("Select atleast one TAT", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                    View.Close();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "DefaultPricing_ListView_Copy" || View.Id == "DefaultPricing_ListView")
                {
                    if (e.DataColumn.FieldName == "Test.TestName" && e.CellValue != null)
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'DefaultPriceGroup', 'Test|'+{0}, '', false)", e.VisibleIndex));
                    }
                }
                else if (View.Id == "TestPriceSurcharge_ListView_newlist" || View.Id == "TestPriceSurcharge_ListView_Edit")
                {
                    if (e.DataColumn.FieldName == "Test.TestName" && e.CellValue != null)
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', 'Test|'+{0}, '', false)", e.VisibleIndex));
                    }
                    else if (e.DataColumn.FieldName == "TAT")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', '{0}|{1}', '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                }
                if (View.Id == "TestPriceSurcharge_ListView_Edit")
                {
                    if (e.DataColumn.FieldName == "TAT")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', '{0}|{1}', '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                }
                else if (View.Id == "TestPriceSurcharge_ListView")
                {
                    e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'TestPriceSurcharge', 'Edit|'+{0}, '', false)", e.VisibleIndex));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void ProcessAction(string parameter)
        {
            try
            {
                string[] param = parameter.Split('|');
                ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                //if (param[0] == "Test" && !string.IsNullOrEmpty(parameter) && View.Id == "DefaultPricing_ListView_Copy" || !string.IsNullOrEmpty(parameter) && View.Id == "DefaultPricing_ListView")
                //{
                //    if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                //    {
                //        HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                //        DefaultPricing objdefprice = View.ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                //        if (objdefprice != null && objdefprice.Test != null)
                //        {
                //            TestMethod curTestMethod = ObjectSpace.GetObjectByKey<TestMethod>(objdefprice.Test.Oid);
                //            if (curTestMethod != null && curTestMethod.IsGroup == true)
                //            {
                //                objGroupTestPricingInfo.CurrentOid = curTestMethod.Oid;
                //                //if (param[0] == "Test")
                //                {
                //                    IObjectSpace objspace = Application.CreateObjectSpace();
                //                    CollectionSource cs = new CollectionSource(objspace, typeof(TestMethod));
                //                    List<GroupTestMethod> objGTM = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod] = ?", objGroupTestPricingInfo.CurrentOid)).Cast<GroupTestMethod>().ToList();
                //                    if (objGTM.Count > 0)
                //                    {
                //                        cs.Criteria["TestFilter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objGTM.ToList().Select(x => x.Tests.Oid))) + ")");
                //                    }
                //                    else
                //                    {
                //                        cs.Criteria["TestFilter"] = CriteriaOperator.Parse("1=2");
                //                    }
                //                    ListView createListView = Application.CreateListView("TestMethod_ListView_IsGroup_Copy", cs, false);
                //                    ShowViewParameters showViewParameters = new ShowViewParameters(createListView);
                //                    showViewParameters.Context = TemplateContext.PopupWindow;
                //                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                //                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                //                }
                //            }
                //            else if (curTestMethod != null && curTestMethod.IsGroup == false && objdefprice.Component != null)
                //            {
                //                Component objcomp = ObjectSpace.GetObjectByKey<Component>(objdefprice.Component.Oid);
                //                if (objcomp != null)
                //                {
                //                    IObjectSpace os = Application.CreateObjectSpace(typeof(Testparameter));
                //                    Testparameter objcrtdummy = os.CreateObject<Testparameter>();
                //                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Testparameter));
                //                    cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", curTestMethod.MatrixName.MatrixName, curTestMethod.TestName, curTestMethod.MethodName.MethodNumber, objcomp.Components);
                //                    ListView lvparameter = Application.CreateListView("Testparameter_LookupListView_ViewPopup", cs, false);
                //                    ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                //                    showViewParameters.CreatedView = lvparameter;
                //                    showViewParameters.Context = TemplateContext.PopupWindow;
                //                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                //                    DialogController dc = Application.CreateController<DialogController>();
                //                    dc.SaveOnAccept = false;
                //                    dc.AcceptAction.Active.SetItemValue("OK", false);
                //                    dc.CloseOnCurrentObjectProcessing = false;
                //                    showViewParameters.Controllers.Add(dc);
                //                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                //                }
                //            }
                //        }
                //    }
                //}
                if (View.Id == "TestPriceSurcharge_ListView_newlist" || View.Id == "TestPriceSurcharge_ListView_Edit")
                {
                    if (param[0] == "TAT")
                    {
                        HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objectSpace, typeof(TurnAroundTime));
                        ListView lvcontainer = Application.CreateListView("TurnAroundTime_ListView_TestPriceSurcharge", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(lvcontainer);
                        showViewParameters.CreatedView = lvcontainer;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dctat_Accepting;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (param[0] == "Test")
                    {
                        HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                        TestPriceSurcharge objtestsurprice = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objtestsurprice != null && objtestsurprice.Test != null)
                        {
                            TestMethod curTestMethod = ObjectSpace.GetObjectByKey<TestMethod>(objtestsurprice.Test.Oid);
                            if (curTestMethod != null && curTestMethod.IsGroup == true)
                            {
                                objGroupTestPricingInfo.CurrentOid = curTestMethod.Oid;
                                //if (param[0] == "Test")
                                {
                                    IObjectSpace objspace = Application.CreateObjectSpace();
                                    CollectionSource cs = new CollectionSource(objspace, typeof(TestMethod));
                                    List<GroupTestMethod> objGTM = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod] = ?", objGroupTestPricingInfo.CurrentOid)).Cast<GroupTestMethod>().ToList();
                                    if (objGTM.Count > 0)
                                    {
                                        cs.Criteria["TestFilter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objGTM.ToList().Select(x => x.Tests.Oid))) + ")");
                                    }
                                    else
                                    {
                                        cs.Criteria["TestFilter"] = CriteriaOperator.Parse("1=2");
                                    }
                                    ListView createListView = Application.CreateListView("TestMethod_ListView_IsGroup_Copy", cs, false);
                                    ShowViewParameters showViewParameters = new ShowViewParameters(createListView);
                                    showViewParameters.Context = TemplateContext.PopupWindow;
                                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                }
                            }
                            else if (curTestMethod != null && curTestMethod.IsGroup == false && objtestsurprice.Component != null)
                            {
                                Component objcomp = ObjectSpace.GetObjectByKey<Component>(objtestsurprice.Component.Oid);
                                if (objcomp != null)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace(typeof(Testparameter));
                                    Testparameter objcrtdummy = os.CreateObject<Testparameter>();
                                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Testparameter));
                                    cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", curTestMethod.MatrixName.MatrixName, curTestMethod.TestName, curTestMethod.MethodName.MethodNumber, objcomp.Components);
                                    ListView lvparameter = Application.CreateListView("Testparameter_LookupListView_ViewPopup", cs, false);
                                    ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                                    showViewParameters.CreatedView = lvparameter;
                                    showViewParameters.Context = TemplateContext.PopupWindow;
                                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                    DialogController dc = Application.CreateController<DialogController>();
                                    dc.SaveOnAccept = false;
                                    dc.AcceptAction.Active.SetItemValue("OK", false);
                                    dc.CloseOnCurrentObjectProcessing = false;
                                    showViewParameters.Controllers.Add(dc);
                                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                }
                            }
                        }
                        ////object currentTest = editor.Grid.GetRowValues(int.Parse(param[1]), "Test.TestName");
                        ////TestMethod curGroupTestMethod = View.ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] = ? AND [IsGroup] = True", currentTest));
                        ////if (curGroupTestMethod != null)
                        ////{
                        ////    IObjectSpace objspace = Application.CreateObjectSpace();
                        ////    CollectionSource cs = new CollectionSource(objspace, typeof(TestMethod));
                        ////    List<GroupTestMethod> objGTM = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", curGroupTestMethod.Oid)).Cast<GroupTestMethod>().ToList();
                        ////    if (objGTM.Count > 0)
                        ////    {
                        ////        cs.Criteria["TestFilter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objGTM.ToList().Select(x => x.Tests.Oid))) + ")");
                        ////    }
                        ////    else
                        ////    {
                        ////        cs.Criteria["TestFilter"] = CriteriaOperator.Parse("1=2");
                        ////    }
                        ////    ListView createListView = Application.CreateListView("TestMethod_ListView_IsGroup_Copy", cs, false);
                        ////    ShowViewParameters showViewParameters = new ShowViewParameters(createListView);
                        ////    showViewParameters.Context = TemplateContext.PopupWindow;
                        ////    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        ////    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        ////}
                    }

                    if (testpricesurinfo.lsttpssurchargedefTAT == null)
                    {
                        testpricesurinfo.lsttpssurchargedefTAT = new List<TestPriceSurcharge>();
                    }
                    if (View.Id == "TestPriceSurcharge_ListView_newlist")
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        if (param[0] == "Selected")
                        {
                            TestPriceSurcharge selobj = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                            if (selobj != null && selobj.Priority != null && selobj.Priority.IsRegular == true)
                            {
                                if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                {
                                    testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                    editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                }
                            }
                            else
                            if (selobj != null && selobj.Test != null)
                            {
                                if (testpricesurinfo.lsttpssurchargedefTAT.Count > 0)
                                {
                                    bool IsDefault = false;
                                    bool Islstavl = false;
                                    foreach (TestPriceSurcharge objtestcharge in testpricesurinfo.lsttpssurchargedefTAT.ToList().Cast<TestPriceSurcharge>().Where(i => (i.IsGroup == false && i.Test != null && i.Matrix != null && i.Test.TestName == selobj.Test.TestName && i.Matrix.MatrixName == selobj.Matrix.MatrixName && i.Method.MethodName.MethodNumber == selobj.Method.MethodName.MethodNumber && i.Component == selobj.Component) || (i.IsGroup == true && i.Test.TestName == selobj.Test.TestName && i.Matrix.MatrixName == selobj.Matrix.MatrixName)))
                                    {
                                        Islstavl = true;
                                        if (objtestcharge.Priority != null && objtestcharge.Priority.IsRegular == true)
                                        {
                                            IsDefault = true;
                                            if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                            {
                                                testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                            }
                                            break;
                                        }
                                        if (IsDefault == false)
                                        {
                                            if (selobj.Test != null && selobj.Method != null)
                                            {
                                                TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName, selobj.Method.MethodName.MethodNumber, selobj.Component.Components));
                                                if (deffindobj == null)
                                                {
                                                    testpricesurinfo.IsNotRegularSelectAll = true;
                                                    Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                    editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                                }
                                                else
                                                {
                                                    if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                                    {
                                                        testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                        editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                                    }
                                                }
                                            }
                                            else if (selobj.Test != null && selobj.IsGroup == true)
                                            {
                                                TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName));
                                                if (deffindobj == null)
                                                {
                                                    testpricesurinfo.IsNotRegularSelectAll = true;
                                                    Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                    editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                                }
                                                else
                                                {
                                                    if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                                    {
                                                        testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                        editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (Islstavl == false)
                                    {
                                        if (selobj.Test != null && selobj.Method != null)
                                        {
                                            TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName, selobj.Method.MethodName.MethodNumber, selobj.Component.Components));
                                            if (deffindobj == null)
                                            {
                                                testpricesurinfo.IsNotRegularSelectAll = true;
                                                Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                            }
                                            else
                                            {
                                                if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                                {
                                                    testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                    editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                                }
                                            }
                                        }
                                        else if (selobj.Test != null && selobj.IsGroup == true)
                                        {
                                            TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName));
                                            if (deffindobj == null)
                                            {
                                                testpricesurinfo.IsNotRegularSelectAll = true;
                                                Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                            }
                                            else
                                            {
                                                if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                                {
                                                    testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                    editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                                }
                                            }
                                        }

                                    }
                                }
                                else if (testpricesurinfo.lsttpssurchargedefTAT.Count == 0)
                                {
                                    if (selobj.Test != null && selobj.Method != null)
                                    {
                                        TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName, selobj.Method.MethodName.MethodNumber, selobj.Component.Components));
                                        if (deffindobj == null)
                                        {
                                            testpricesurinfo.IsNotRegularSelectAll = true;
                                            Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                        }
                                    }
                                    else if (selobj.Test != null && selobj.IsGroup == true)
                                    {
                                        TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName));
                                        if (deffindobj == null)
                                        {
                                            testpricesurinfo.IsNotRegularSelectAll = true;
                                            Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                    {
                                        testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                        editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                    }
                                }
                            }
                        }
                        else if (param[0] == "UNSelected")
                        {
                            bool IsRegularUnselect = false;
                            TestPriceSurcharge unselobj = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                            if (unselobj != null && unselobj.Priority != null && unselobj.Priority.IsRegular == true)
                            {
                                if (testpricesurinfo.lsttpssurchargedefTAT.Contains(unselobj))
                                {
                                    testpricesurinfo.lsttpssurchargedefTAT.Remove(unselobj);
                                    editor.Grid.Selection.UnselectRowByKey(unselobj.Oid);
                                }
                                foreach (TestPriceSurcharge objtpsunsel in ((ListView)View).CollectionSource.List.Cast<TestPriceSurcharge>().Where(i => (i.IsGroup == false && i.Test != null && i.Matrix != null && i.Test.TestName == unselobj.Test.TestName && i.Matrix.MatrixName == unselobj.Matrix.MatrixName && i.Method.MethodName.MethodNumber == unselobj.Method.MethodName.MethodNumber && i.Component == unselobj.Component) || (i.IsGroup == true && i.Test.TestName == unselobj.Test.TestName && i.Matrix.MatrixName == unselobj.Matrix.MatrixName)).ToList())
                                {
                                    testpricesurinfo.lsttpssurchargedefTAT.Remove(objtpsunsel);
                                    editor.Grid.Selection.UnselectRowByKey(objtpsunsel.Oid);
                                    IsRegularUnselect = true;
                                }
                                if (IsRegularUnselect == true)
                                {
                                    Application.ShowViewStrategy.ShowMessage("In the selected test must be select default priority first then select other priority", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            //TestPriceSurcharge unselobj = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                            //if (unselobj != null)
                            //{
                            //    testpricesurinfo.lsttpssurchargedefTAT.Remove(unselobj);
                            //    editor.Grid.Selection.UnselectRowByKey(unselobj.Oid);
                            //}
                        }
                        else if (param[0] == "Selectall" && testpricesurinfo.IsNotRegularSelectAll == false)
                        {
                            foreach (TestPriceSurcharge objcon in ((ListView)View).CollectionSource.List)
                            {
                                if (objcon != null && objcon.Priority != null && objcon.Priority.IsRegular == true)
                                {
                                    testpricesurinfo.lsttpssurchargedefTAT.Add(objcon);
                                    editor.Grid.Selection.SelectRowByKey(objcon.Oid);
                                }
                                else
                                if (objcon != null)
                                {
                                    if (testpricesurinfo.lsttpssurchargedefTAT.Count > 0 && testpricesurinfo.lsttpssurchargedefTAT.Cast<TestPriceSurcharge>().Where(i => i.Priority.IsRegular == true) == null)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Please Select the default Priority", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        break;
                                    }
                                    else
                                    {
                                        TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", objcon.Matrix.MatrixName, objcon.Test.TestName, objcon.Method.MethodName.MethodNumber, objcon.Component.Components));
                                        if (deffindobj == null)
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Please Select the default Priority", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (param[0] == "UNSelectall")
                        {
                            //editor.Grid.Selection.UnselectAll();
                            //testpricesurinfo.lsttpssurchargedefTAT.Clear();
                        }
                        else if (param[0] == "ValuesChange")
                        {
                            TestPriceSurcharge selobj = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                            if (selobj != null && selobj.IsGroup == false)
                            {
                                TestPriceSurcharge tstpricesurcharge = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular]=True", selobj.Matrix.MatrixName, selobj.Test.TestName, selobj.Method.MethodName.MethodNumber, selobj.Component.Components));
                                if (tstpricesurcharge != null)
                                {
                                    if (tstpricesurcharge.SurchargePrice == 0 || tstpricesurcharge.SurchargePrice == null)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Please set the regular price first", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                    decimal objSurPer = Convert.ToDecimal(selobj.Surcharge);
                                    decimal objDefaultPrice = Convert.ToDecimal(tstpricesurcharge.SurchargePrice);
                                    decimal obj = objDefaultPrice + (objDefaultPrice * (objSurPer / 100));
                                    selobj.SurchargePrice = Math.Round(obj, 2);
                                }

                            }
                            else if (selobj != null && selobj.IsGroup == true)
                            {
                                TestPriceSurcharge tstpricesurcharge = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup]=True  And [Priority.IsRegular]=True", selobj.Matrix.MatrixName, selobj.Test.TestName));
                                if (tstpricesurcharge != null)
                                {
                                    if (tstpricesurcharge.SurchargePrice == 0 || tstpricesurcharge.SurchargePrice == null)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Please set the regular price first", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                    decimal objSurPer = Convert.ToDecimal(selobj.Surcharge);
                                    decimal objDefaultPrice = Convert.ToDecimal(tstpricesurcharge.SurchargePrice);
                                    decimal obj = objDefaultPrice + (objDefaultPrice * (objSurPer / 100));
                                    selobj.SurchargePrice = Math.Round(obj, 2);
                                }
                            }
                            ((ListView)View).Refresh();
                        }
                        else if (param[0] == "SurchargePrice")
                        {
                            TestPriceSurcharge selobj = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                            if (selobj.Priority.IsRegular == false)
                            {
                                if (selobj != null && selobj.IsGroup == false)
                                {
                                    TestPriceSurcharge tstpricesurcharge = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular]=True", selobj.Matrix.MatrixName, selobj.Test.TestName, selobj.Method.MethodName.MethodNumber, selobj.Component.Components));
                                    if (tstpricesurcharge != null)
                                    {
                                        if (tstpricesurcharge.SurchargePrice == 0 || tstpricesurcharge.SurchargePrice == null)
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Please set the regular price first", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            return;
                                        }
                                        if (selobj.SurchargePrice != null && (Convert.ToDecimal(tstpricesurcharge.SurchargePrice) > Convert.ToDecimal(selobj.SurchargePrice)))
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Enter  Price greaterthan regular price.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            return;
                                        }
                                        decimal objSurPer = Convert.ToDecimal(selobj.SurchargePrice);
                                        decimal objDefaultPrice = Convert.ToDecimal(tstpricesurcharge.SurchargePrice);
                                        decimal obj = ((objSurPer - objDefaultPrice) / objDefaultPrice) * 100;
                                        selobj.Surcharge = Convert.ToInt32(obj);
                                        //decimal obj = objDefaultPrice + (objDefaultPrice * (objSurPer / 100));
                                        //selobj.SurchargePrice = Math.Round(obj, 2);
                                    }

                                }
                                else if (selobj != null && selobj.IsGroup == true)
                                {
                                    TestPriceSurcharge tstpricesurcharge = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup]=True  And [Priority.IsRegular]=True", selobj.Matrix.MatrixName, selobj.Test.TestName));
                                    if (tstpricesurcharge != null)
                                    {
                                        if (tstpricesurcharge.SurchargePrice == 0 || tstpricesurcharge.SurchargePrice == null)
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Please set the regular price first", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            return;
                                        }
                                        if (selobj.SurchargePrice != null && (Convert.ToDecimal(tstpricesurcharge.SurchargePrice) > Convert.ToDecimal(selobj.SurchargePrice)))
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Enter  Price greaterthan regular price.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            return;
                                        }
                                        decimal objSurPer = Convert.ToDecimal(selobj.SurchargePrice);
                                        decimal objDefaultPrice = Convert.ToDecimal(tstpricesurcharge.SurchargePrice);
                                        decimal obj = ((objSurPer - objDefaultPrice) / objDefaultPrice) * 100;
                                        selobj.Surcharge = Convert.ToInt32(obj);
                                    }
                                }
                            ((ListView)View).Refresh();
                            }
                        }
                        if (param[0] == "ValuesChange" || param[0] == "SurchargePrice")
                        {
                            TestPriceSurcharge selobj = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                            if (selobj != null)
                            {
                                if (selobj.Priority != null && selobj.Priority.IsRegular == true)
                                {
                                    if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                    {
                                        testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                        editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                    }
                                }
                                else if (selobj != null && selobj.Test != null)
                                {
                                    if (testpricesurinfo.lsttpssurchargedefTAT.Count > 0)
                                    {
                                        bool IsDefault = false;
                                        bool Islstavl = false;
                                        foreach (TestPriceSurcharge objtestcharge in testpricesurinfo.lsttpssurchargedefTAT.ToList().Cast<TestPriceSurcharge>().Where(i => (i.IsGroup == false && i.Test != null && i.Matrix != null && i.Test.TestName == selobj.Test.TestName && i.Matrix.MatrixName == selobj.Matrix.MatrixName && i.Method.MethodName.MethodNumber == selobj.Method.MethodName.MethodNumber && i.Component == selobj.Component) || (i.IsGroup == true && i.Test.TestName == selobj.Test.TestName && i.Matrix.MatrixName == selobj.Matrix.MatrixName)))
                                        {
                                            Islstavl = true;
                                            if (objtestcharge.Priority != null && objtestcharge.Priority.IsRegular == true)
                                            {
                                                IsDefault = true;
                                                if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                                {
                                                    testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                    editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                                }
                                                break;
                                            }
                                            if (IsDefault == false)
                                            {
                                                if (selobj.Test != null && selobj.Method != null)
                                                {
                                                    TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName, selobj.Method.MethodName.MethodNumber, selobj.Component.Components));
                                                    if (deffindobj == null)
                                                    {
                                                        testpricesurinfo.IsNotRegularSelectAll = true;
                                                        Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                        editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                                    }
                                                    else
                                                    {
                                                        if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                                        {
                                                            testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                            editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                                        }
                                                    }
                                                }
                                                else if (selobj.Test != null && selobj.IsGroup == true)
                                                {
                                                    TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName));
                                                    if (deffindobj == null)
                                                    {
                                                        testpricesurinfo.IsNotRegularSelectAll = true;
                                                        Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                        editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                                    }
                                                    else
                                                    {
                                                        if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                                        {
                                                            testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                            editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (Islstavl == false)
                                        {
                                            if (selobj.Test != null && selobj.Method != null)
                                            {
                                                TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName, selobj.Method.MethodName.MethodNumber, selobj.Component.Components));
                                                if (deffindobj == null)
                                                {
                                                    testpricesurinfo.IsNotRegularSelectAll = true;
                                                    Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                    editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                                }
                                                else
                                                {
                                                    if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                                    {
                                                        testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                        editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                                    }
                                                }
                                            }
                                            else if (selobj.Test != null && selobj.IsGroup == true)
                                            {
                                                TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName));
                                                if (deffindobj == null)
                                                {
                                                    testpricesurinfo.IsNotRegularSelectAll = true;
                                                    Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                    editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                                }
                                                else
                                                {
                                                    if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                                    {
                                                        testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                                        editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    else if (testpricesurinfo.lsttpssurchargedefTAT.Count == 0)
                                    {
                                        if (selobj.Test != null && selobj.Method != null)
                                        {
                                            TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName, selobj.Method.MethodName.MethodNumber, selobj.Component.Components));
                                            if (deffindobj == null)
                                            {
                                                testpricesurinfo.IsNotRegularSelectAll = true;
                                                Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                            }
                                        }
                                        else if (selobj.Test != null && selobj.IsGroup == true)
                                        {
                                            TestPriceSurcharge deffindobj = os.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Priority.IsRegular] = 'True'", selobj.Matrix.MatrixName, selobj.Test.TestName));
                                            if (deffindobj == null)
                                            {
                                                testpricesurinfo.IsNotRegularSelectAll = true;
                                                Application.ShowViewStrategy.ShowMessage("To begin setting up a test price surcharge, you must first select the default priority (General).", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                                editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!testpricesurinfo.lsttpssurchargedefTAT.Contains(selobj))
                                        {
                                            testpricesurinfo.lsttpssurchargedefTAT.Add(selobj);
                                            editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                else if (View.Id == "TurnAroundTime_ListView_TestPriceSurcharge")
                {
                    if (testpricesurinfo.lsttpsTAT == null)
                    {
                        testpricesurinfo.lsttpsTAT = new List<string>();
                    }
                    if (param[0] == "Selected")
                    {
                        bool IsTATadded = false;
                        TurnAroundTime selobj = View.ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                        if (selobj != null && !string.IsNullOrEmpty(selobj.TAT))
                        {
                            string str = (HttpContext.Current.Session["rowid"].ToString());
                            Guid tpsoid = new Guid(HttpContext.Current.Session["rowid"].ToString());
                            if (tpsoid != Guid.Empty)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                TestPriceSurcharge objtstprsur = Application.MainWindow.View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                if (objtstprsur != null)
                                {
                                    if (objtstprsur != null && objtstprsur.Matrix != null && objtstprsur.Test != null && objtstprsur.Method != null && objtstprsur.Component != null)
                                    {
                                        List<TestPriceSurcharge> lsttstpricesurcharge = os.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ?", objtstprsur.Matrix.MatrixName, objtstprsur.Test.TestName, objtstprsur.Method.MethodName.MethodNumber, objtstprsur.Component.Components)).ToList();
                                        if (lsttstpricesurcharge != null && lsttstpricesurcharge.Count > 0)
                                        {
                                            foreach (TestPriceSurcharge objtstprisur in lsttstpricesurcharge.ToList())//((ListView)Application.MainWindow.View).CollectionSource.List.Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())//View.ObjectSpace.GetObjects<TestPriceCode>().Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())
                                            {
                                                if (!string.IsNullOrEmpty(objtstprisur.TAT))
                                                {
                                                    string[] strtatarr = objtstprisur.TAT.Split(';');
                                                    foreach (string strtatoid in strtatarr.ToList())
                                                    {
                                                        if (selobj.TAT.ToString() == strtatoid.Trim())
                                                        {
                                                            IsTATadded = true;
                                                            break;
                                                        }
                                                    }
                                                    if (IsTATadded == true)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (objtstprsur != null && objtstprsur.Matrix != null && objtstprsur.Test != null && objtstprsur.IsGroup == true)
                                    {
                                        List<TestPriceSurcharge> lsttstpricesurcharge = os.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True'", objtstprsur.Matrix.MatrixName, objtstprsur.Test.TestName)).ToList();
                                        if (lsttstpricesurcharge != null && lsttstpricesurcharge.Count > 0)
                                        {
                                            foreach (TestPriceSurcharge objtstprisur in lsttstpricesurcharge.ToList())//((ListView)Application.MainWindow.View).CollectionSource.List.Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())//View.ObjectSpace.GetObjects<TestPriceCode>().Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())
                                            {
                                                if (!string.IsNullOrEmpty(objtstprisur.TAT))
                                                {
                                                    string[] strtatarr = objtstprisur.TAT.Split(';');
                                                    foreach (string strtatoid in strtatarr.ToList())
                                                    {
                                                        if (selobj.TAT.ToString() == strtatoid.Trim())
                                                        {
                                                            IsTATadded = true;
                                                            break;
                                                        }
                                                    }
                                                    if (IsTATadded == true)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    foreach (TestPriceSurcharge objtstprisur in ((ListView)Application.MainWindow.View).CollectionSource.List.Cast<TestPriceSurcharge>().Where(i => (i.IsGroup == false && i.Test != null && i.Test.TestName == objtstprsur.Test.TestName && i.Matrix.MatrixName == objtstprsur.Matrix.MatrixName && i.Method.MethodName.MethodNumber == objtstprsur.Method.MethodName.MethodNumber && i.Component == objtstprsur.Component) || (i.IsGroup == true && i.Test.TestName == objtstprsur.Test.TestName && i.Matrix.MatrixName == objtstprsur.Matrix.MatrixName)).ToList())//View.ObjectSpace.GetObjects<TestPriceCode>().Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())
                                    {
                                        if (!string.IsNullOrEmpty(objtstprisur.TAT))
                                        {
                                            string[] strtatarr = objtstprisur.TAT.Split(';');
                                            foreach (string strtatoid in strtatarr.ToList())
                                            {
                                                if (selobj.TAT.ToString() == strtatoid.Trim())
                                                {
                                                    IsTATadded = true;
                                                    break;
                                                }
                                            }
                                            if (IsTATadded == true)
                                            {
                                                break;
                                            }
                                        }
                                    }


                                    if (IsTATadded == true)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectedTATAlreadysave"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                    }
                                    else
                                    {
                                        editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                                    }
                                }
                            }
                        }
                        if (selobj != null && !string.IsNullOrEmpty(selobj.TAT) && !testpricesurinfo.lsttpsTAT.Contains(selobj.TAT.Trim()) && IsTATadded == false)
                        {
                            testpricesurinfo.lsttpsTAT.Add(selobj.TAT.Trim());
                            editor.Grid.Selection.SelectRowByKey(selobj.Oid);
                        }
                    }
                    else if (param[0] == "UNSelected")
                    {
                        TurnAroundTime unselobj = View.ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                        if (unselobj != null && !string.IsNullOrEmpty(unselobj.TAT) && testpricesurinfo.lsttpsTAT.Contains(unselobj.TAT.Trim()))
                        {
                            testpricesurinfo.lsttpsTAT.Remove(unselobj.TAT.Trim());
                            editor.Grid.Selection.UnselectRowByKey(unselobj.Oid);
                        }
                    }
                    else if (param[0] == "Selectall")
                    {
                        bool IsTATadded = false;
                        foreach (TurnAroundTime selobj in ((ListView)View).CollectionSource.List.Cast<TurnAroundTime>().OrderBy(i => i.TAT).ToList())
                        {
                            //if (objcon != null && !string.IsNullOrEmpty(objcon.TAT) && !testpricesurinfo.lsttpsTAT.Contains(objcon.TAT.Trim()))
                            //{
                            //    testpricesurinfo.lsttpsTAT.Add(objcon.TAT.Trim());
                            //    editor.Grid.Selection.SelectAll();
                            //}
                            if (selobj != null && !string.IsNullOrEmpty(selobj.TAT))
                            {
                                Guid tpsoid = new Guid(HttpContext.Current.Session["rowid"].ToString());
                                if (tpsoid != Guid.Empty)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    TestPriceSurcharge objtstprsur = Application.MainWindow.View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                    if (objtstprsur != null)
                                    {
                                        if (objtstprsur != null && objtstprsur.Matrix != null && objtstprsur.Test != null && objtstprsur.Method != null && objtstprsur.Component != null)
                                        {
                                            List<TestPriceSurcharge> lsttstpricesurcharge = os.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ?", objtstprsur.Matrix.MatrixName, objtstprsur.Test.TestName, objtstprsur.Method.MethodName.MethodNumber, objtstprsur.Component.Components)).ToList();
                                            if (lsttstpricesurcharge != null && lsttstpricesurcharge.Count > 0)
                                            {
                                                foreach (TestPriceSurcharge objtstprisur in lsttstpricesurcharge.ToList())//((ListView)Application.MainWindow.View).CollectionSource.List.Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())//View.ObjectSpace.GetObjects<TestPriceCode>().Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())
                                                {
                                                    if (!string.IsNullOrEmpty(objtstprisur.TAT))
                                                    {
                                                        string[] strtatarr = objtstprisur.TAT.Split(';');
                                                        foreach (string strtatoid in strtatarr.ToList())
                                                        {
                                                            if (selobj.Oid.ToString() == strtatoid.Trim())
                                                            {
                                                                IsTATadded = true;
                                                                break;
                                                            }
                                                        }
                                                        if (IsTATadded == true)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (objtstprsur != null && objtstprsur.Matrix != null && objtstprsur.Test != null && objtstprsur.IsGroup == true)
                                        {
                                            List<TestPriceSurcharge> lsttstpricesurcharge = os.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? [IsGroup] = 'True'", objtstprsur.Matrix.MatrixName, objtstprsur.Test.TestName)).ToList();
                                            if (lsttstpricesurcharge != null && lsttstpricesurcharge.Count > 0)
                                            {
                                                foreach (TestPriceSurcharge objtstprisur in lsttstpricesurcharge.ToList())//((ListView)Application.MainWindow.View).CollectionSource.List.Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())//View.ObjectSpace.GetObjects<TestPriceCode>().Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())
                                                {
                                                    if (!string.IsNullOrEmpty(objtstprisur.TAT))
                                                    {
                                                        string[] strtatarr = objtstprisur.TAT.Split(';');
                                                        foreach (string strtatoid in strtatarr.ToList())
                                                        {
                                                            if (selobj.Oid.ToString() == strtatoid.Trim())
                                                            {
                                                                IsTATadded = true;
                                                                break;
                                                            }
                                                        }
                                                        if (IsTATadded == true)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        foreach (TestPriceSurcharge objtstprisur in ((ListView)Application.MainWindow.View).CollectionSource.List.Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())//View.ObjectSpace.GetObjects<TestPriceCode>().Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())
                                        {
                                            if (!string.IsNullOrEmpty(objtstprisur.TAT))
                                            {
                                                string[] strtatarr = objtstprisur.TAT.Split(';');
                                                foreach (string strtatoid in strtatarr.ToList())
                                                {
                                                    if (selobj.Oid.ToString() == strtatoid.Trim())
                                                    {
                                                        IsTATadded = true;
                                                        break;
                                                    }
                                                }
                                                if (IsTATadded == true)
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                        if (IsTATadded == true)
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectedTATAlreadysave"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            editor.Grid.Selection.UnselectRowByKey(selobj.Oid);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (selobj != null && !string.IsNullOrEmpty(selobj.TAT) && !testpricesurinfo.lsttpsTAT.Contains(selobj.TAT.Trim()) && IsTATadded == false)
                            {
                                testpricesurinfo.lsttpsTAT.Add(selobj.TAT.Trim());
                            }

                        }
                        if (IsTATadded == false)
                        {
                            editor.Grid.Selection.SelectAll();
                        }
                    }
                    else if (param[0] == "UNSelectall")
                    {
                        editor.Grid.Selection.UnselectAll();
                        testpricesurinfo.lsttpsTAT.Clear();
                    }
                    Isparameter = true;
                }
                else if (View.Id == "TestPriceSurcharge_ListView" && param[0] == "Edit")
                {
                    HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(TestPriceSurcharge));
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString()));
                    ListView lvcontainer = Application.CreateListView("TestPriceSurcharge_ListView_Edit", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lvcontainer);
                    showViewParameters.CreatedView = lvcontainer;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += DcTestPriceSurcharge_Edit_Accepting;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DcTestPriceSurcharge_Edit_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dc = (DialogController)sender;
                    if (dc != null && dc.Window != null && dc.Window.View != null && dc.Window.View.Id == "TestPriceSurcharge_ListView_Edit")
                    {
                        ((ListView)dc.Window.View).ObjectSpace.CommitChanges();
                        View.ObjectSpace.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dctat_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                string strTAT = string.Empty;
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    if (testpricesurinfo.lsttpsTAT != null && testpricesurinfo.lsttpsTAT.Count > 0)
                    {
                        testpricesurinfo.lsttpsTAT.Clear();
                    }
                    testpricesurinfo.lsttpsTAT = e.AcceptActionArgs.SelectedObjects.Cast<TurnAroundTime>().Where(i => i.TAT != null).Select(i => i.TAT).ToList();
                }
                foreach (string strtat in testpricesurinfo.lsttpsTAT.ToList())
                {
                    TurnAroundTime objcontainer = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", strtat));
                    if (objcontainer != null)
                    {
                        if (string.IsNullOrEmpty(strTAT))
                        {
                            strTAT = objcontainer.TAT.ToString();
                        }
                        else if (!string.IsNullOrEmpty(strTAT))
                        {
                            strTAT = strTAT + "; " + objcontainer.TAT.ToString();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(strTAT))
                {
                    TestPriceSurcharge objtstprsur = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    if (objtstprsur != null)
                    {
                        objtstprsur.TAT = strTAT;
                    }
                    //bool istatset = false;
                    //TestPriceSurcharge objtstprsur = View.ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    //if (objtstprsur != null)
                    //{
                    //    foreach (TestPriceSurcharge objtstprisur in ((ListView)View).CollectionSource.List.Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())//View.ObjectSpace.GetObjects<TestPriceCode>().Cast<TestPriceSurcharge>().Where(i => i.PriceCode == objtstprsur.PriceCode).ToList())
                    //    {
                    //        if (objtstprsur.Oid != objtstprisur.Oid)
                    //        {
                    //            if (objtstprisur.TAT == strTAT)
                    //            {
                    //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectedTATAlreadysave"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    //                e.Cancel = true;
                    //                istatset = true;
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    //if (istatset == false)
                    //{
                    //    objtstprsur.TAT = strTAT;
                    //}
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select atleast one TAT.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Add_Btn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "ConstituentPricingTier_ListView" || View.Id == "ConstituentPricing_ConstituentPricingTiers_ListView")
                {
                    //Application.MainWindow.View.ObjectSpace.CommitChanges();
                    ConstituentPricing objcp = (ConstituentPricing)Application.MainWindow.View.CurrentObject;
                    if (objcp != null)
                    {
                        ConstituentPricingTier objcpt = ObjectSpace.CreateObject<ConstituentPricingTier>();
                        //ConstituentPricingTier objcpt = Application.MainWindow.View.ObjectSpace.CreateObject<ConstituentPricingTier>();
                        //objcpt.ConstituentPricing = Application.MainWindow.View.ObjectSpace.GetObject(objcp);
                        objcpt.ConstituentPricing = ObjectSpace.GetObject(objcp);
                        int maxTierNo = 0;
                        int maxtoval = 0;
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            for (int i = 0; i <= gridListEditor.Grid.VisibleRowCount; i++)
                            {
                                int cursort = Convert.ToInt32(gridListEditor.Grid.GetRowValues(i, "TierNo"));
                                if (maxTierNo <= cursort)
                                {
                                    maxTierNo = cursort + 1;
                                }
                            }
                            for (int i = 0; i <= gridListEditor.Grid.VisibleRowCount; i++)
                            {
                                int cursort = Convert.ToInt32(gridListEditor.Grid.GetRowValues(i, "To"));
                                if (maxtoval <= cursort)
                                {
                                    maxtoval = cursort + 1;
                                }
                            }
                            objcpt.TierNo = Convert.ToUInt16(maxTierNo);
                            objcpt.From = Convert.ToUInt16(maxtoval);
                            objcpt.To = Convert.ToUInt16(objcpt.From + 1);
                            constituentInfo.From = objcpt.From;
                        }
                        //objcp.ConstituentPricingTiers.Add(objcpt);
                        ((ListView)View).CollectionSource.Add(objcpt);
                        ((ListView)View).Refresh();
                        //Application.MainWindow.View.Refresh();
                        //View.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Remove_Btn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if ((View.Id == "ConstituentPricingTier_ListView" || View.Id == "ConstituentPricing_ConstituentPricingTiers_ListView") && View.SelectedObjects.Count > 0)
                {
                    if (priceinfo.lstselectremove == null)
                    {
                        priceinfo.lstselectremove = new List<ConstituentPricingTier>();
                    }
                    foreach (ConstituentPricingTier objcontier in View.SelectedObjects)
                    {
                        priceinfo.lstselectremove.Add(objcontier);
                        ((ListView)View).CollectionSource.Remove(objcontier);
                        ((ListView)View).Refresh();
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ReactivateRegistration_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ItemChargePricing objICP = View.CurrentObject as ItemChargePricing;
                objICP.Cancel = false;
                View.ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage("Reactivated successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                View.Refresh();
                Application.MainWindow.View.Refresh();
                View.ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion
        private void Cancel_Btn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                View.Close();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
