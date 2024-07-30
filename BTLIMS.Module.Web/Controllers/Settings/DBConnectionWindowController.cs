using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace LDM.Module.Web.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class DBConnectionWindowController : WindowController
    {
        MessageTimer timer = new MessageTimer();
        DBConnectioninfo dbconinfo = new DBConnectioninfo();
        public DBConnectionWindowController()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            dbconinfo.strdbconstring = string.Empty;
            // Perform various tasks depending on the target Window.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        private void DBConnection_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if(e.AcceptActionArgs.SelectedObjects.Count ==1)
                {
                    DBConnection dBConnection = (DBConnection)e.AcceptActionArgs.CurrentObject;
                    if (dBConnection != null)
                    {
                        dbconinfo.strdbconstring = "Data Source=" + dBConnection.ServerName.Trim() + ";Initial Catalog=" + dBConnection.DataBaseName.Trim() + ";User ID=" + dBConnection.UserName.Trim() + ";Password=" + dBConnection.Password.Trim() + ";Integrated Security=False;";
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\"));
                        }
                        string strLocalFile = HttpContext.Current.Server.MapPath((@"~\DBConnection.txt"));
                        //if (File.Exists(strLocalFile))
                        {
                            File.WriteAllText(strLocalFile, dbconinfo.strdbconstring);
                        }
                    }                   
                }
                else if (e.AcceptActionArgs.SelectedObjects.Count > 1)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DBConnection_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objectSpace, typeof(DBConnection));
                ListView createlistview = Application.CreateListView("DBConnection_ListView_popup", cs, true);
                ShowViewParameters showViewParameters = new ShowViewParameters(createlistview);
                showViewParameters.CreatedView = createlistview;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                dc.Accepting += DBConnection_Accepting;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void singleChoiceAction1_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {

        }
    }
}
