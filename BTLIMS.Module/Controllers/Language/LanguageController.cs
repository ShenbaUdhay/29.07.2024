using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;

namespace BTLIMS.Module.Controllers.Language
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class LanguageController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        #region Declaration
        ModificationsController modificationController;
        GridListEditor gridlisteditor;
        #endregion

        #region Constructor
        public LanguageController()
        {
            InitializeComponent();
            TargetViewId = "CurrentLanguage_DetailView;" + "CurrentLanguage_ListView;";
        }
        #endregion

        #region DeafultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ObjectSpace.Committed += ObjectSpace_Committed;
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
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
                if (View != null & (View.Id == "CurrentLanguage_ListView" || View.Id == "CurrentLanguage_DetailView"))
                {

                    if (e.PropertyName == "English" || e.PropertyName == "Chinese")
                    {
                        CurrentLanguage objCurrentLanguage = (CurrentLanguage)e.Object;
                        if (e.PropertyName == "English")
                        {
                            if (objCurrentLanguage.English == true)
                            {
                                objCurrentLanguage.Chinese = false;
                            }
                        }
                        if (e.PropertyName == "Chinese")
                        {
                            if (objCurrentLanguage.Chinese == true)
                            {
                                objCurrentLanguage.English = false;

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

        private void ObjectSpace_Committed(object sender, System.EventArgs e)
        {
            try
            {
                string Culture = string.Empty;
                IObjectSpace os = Application.CreateObjectSpace();
                Session currentSession = ((XPObjectSpace)(os)).Session;
                SelectedData sproc = currentSession.ExecuteSproc("getCurrentLanguage", "");
                Culture = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                if (Culture == "En")
                {
                    Application.SetFormattingCulture("EN-US");
                    SelectedData sproc1 = currentSession.ExecuteSproc("Language_Update_SP", new OperandValue("English"));
                }
                else
                {
                    Application.SetFormattingCulture("zh");
                    SelectedData sproc1 = currentSession.ExecuteSproc("Language_Update_SP", new OperandValue("Chinese"));
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
            ObjectSpace.Committed -= ObjectSpace_Committed;
        }
        #endregion

    }
}
