using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.ComponentModel;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SetupDeleteValidationViewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        DeleteObjectsViewController DeleteController;
        TestMethodInfo objTMInfo = new TestMethodInfo();
        #endregion

        #region Constructor
        public SetupDeleteValidationViewController()
        {
            InitializeComponent();
            try
            {
                TargetViewId = "Matrix_DetailView;" + "Matrix_ListView;" + "Method_DetailView;" +
                    "Method_ListView;" + "Parameter_DetailView;" + "Parameter_ListView;" + "GroupTest_DetailView;" + "GroupTest_ListView;" +
                    "TestMethod_ListView;" + "TestMethod_DetailView;" + "TestMethod_Parameters_ListView;" + "Contract_ListView;" + "Contract_DetailView;";
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                DeleteController = Frame.GetController<DeleteObjectsViewController>();
                if (DeleteController != null)
                    DeleteController.DeleteAction.Executing += DeleteAction_Executing;
                Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing += UnlinkAction_Executing;
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
            if (View != null && View.CurrentObject != null && View.Id == "TestMethod_DetailView")
            {
                objTMInfo.TestMethodOid = ObjectSpace.GetKeyValueAsString(View.CurrentObject);
            }

        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            try
            {
                if (DeleteController != null)
                {
                    DeleteController.DeleteAction.Executing -= DeleteAction_Executing;
                }
                Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing -= UnlinkAction_Executing;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void UnlinkAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(Parameter) && View.Id == "TestMethod_Parameters_ListView")
                {
                    foreach (Parameter obj in View.SelectedObjects)
                    {
                        var os = Application.CreateObjectSpace();
                        CriteriaOperator criteria = CriteriaOperator.Parse("[Parameter]='" + obj.Oid + "' AND [TestMethod]='" + objTMInfo.TestMethodOid + "'");
                        Testparameter TP = ObjectSpace.FindObject<Testparameter>(criteria, false);
                        if (TP != null)
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("Testparameter='" + TP.Oid + "'");
                            bool exists = Convert.ToBoolean(ObjectSpace.Evaluate(typeof(SampleParameter), (new AggregateOperand("", Aggregate.Exists)), (criteria1)));
                            if (exists == true)
                            {
                                e.Cancel = true;
                                Exception ex = new Exception("Cannot Be Unlink the Parameter.! It is Used to SampleLogIn");
                                throw ex;
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

        private void DeleteAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(Matrix))
                {
                    foreach (Matrix obj in View.SelectedObjects)
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        CriteriaOperator criteria = CriteriaOperator.Parse("[MatrixName]='" + obj.Oid + "'");
                        TestMethod objTM = objSpace.FindObject<TestMethod>(criteria);
                        VisualMatrix objVm = objSpace.FindObject<VisualMatrix>(criteria);
                        if (objVm != null)
                        {
                            e.Cancel = true;
                            Exception ex = new Exception("Cannot Be Delete the Matrix.! It is Used in Sample Matrix");
                            throw ex;
                        }
                        if (objTM != null)
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[TestMethod]='" + objTM.Oid + "'");
                            Testparameter objTP = objSpace.FindObject<Testparameter>(criteria1);
                            if (objTP != null)
                            {
                                e.Cancel = true;
                                Exception ex = new Exception("Cannot Be Delete the Matrix.! It is Used to TestParameter");
                                throw ex;
                            }
                        }

                    }
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Modules.BusinessObjects.Setting.Method))
                {
                    foreach (Modules.BusinessObjects.Setting.Method obj in View.SelectedObjects)
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        CriteriaOperator criteria = CriteriaOperator.Parse("[MethodName]='" + obj.Oid + "'");
                        TestMethod objTM = objSpace.FindObject<TestMethod>(criteria);
                        if (objTM != null)
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[TestMethod]='" + objTM.Oid + "'");
                            Testparameter objTP = objSpace.FindObject<Testparameter>(criteria1);
                            if (objTP != null)
                            {
                                e.Cancel = true;
                                Exception ex = new Exception("Cannot Be Delete the Method.! It is Used to TestParameter");
                                throw ex;
                            }
                        }
                    }
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Parameter))
                {
                    foreach (Parameter obj in View.SelectedObjects)
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        CriteriaOperator criteria1 = CriteriaOperator.Parse("[TestMethod.GCRecord] IS NULL And [Parameter]= ?", obj.Oid);
                        Testparameter objTP = objSpace.FindObject<Testparameter>(criteria1);
                        if (objTP != null)
                        {
                            e.Cancel = true;
                            Exception ex = new Exception("Cannot Be Delete the Parameter.! It is Used to TestParameter");
                            //Exception ex = new Exception("Already used in testparameter,unable to delete parameter.");
                            throw ex;
                        }
                    }
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(TestMethod))
                {
                    foreach (TestMethod obj in View.SelectedObjects)
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        CriteriaOperator criteria = CriteriaOperator.Parse("[TestMethod]='" + obj.Oid + "'");
                        Testparameter objTM = objSpace.FindObject<Testparameter>(criteria);
                        if (objTM != null)
                        {
                            CriteriaOperator criteria1 = CriteriaOperator.Parse("[Testparameter]='" + objTM.Oid + "'");
                            SampleParameter objTP = objSpace.FindObject<SampleParameter>(criteria1);
                            if (objTP != null)
                            {
                                e.Cancel = true;
                                //Exception ex = new Exception("Cannot Be Delete the TestParameter.! It is Used to Samplelogin");
                                Exception ex = new Exception("Already used in samplelogin,unable to delete testparameter.");
                                throw ex;
                            }
                        }
                    }
                }
                ////else if (View != null && View.ObjectTypeInfo.Type == typeof(Contract))
                ////{
                ////    foreach (Contract obj in View.SelectedObjects)
                ////    {
                ////        IObjectSpace objSpace = Application.CreateObjectSpace();
                ////        //Testparameter objTM = objSpace.FindObject<Testparameter>(criteria);
                ////        if (obj.Status != ContractStatus.Saved && obj.Status != ContractStatus.Submitted)
                ////        {
                ////            e.Cancel = true;
                ////            Exception ex = new Exception("Contract registration is reviewed, cannot be deleted.!");
                ////            throw ex;
                ////        }
                ////    }
                ////}
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
