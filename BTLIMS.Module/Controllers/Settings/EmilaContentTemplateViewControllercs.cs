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
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class EmilaContentTemplateViewControllercs : ViewController
    {
        MessageTimer timer = new MessageTimer();

        public EmilaContentTemplateViewControllercs()
        {
            InitializeComponent();
            TargetViewId = "EmailContentTemplate_DetailView;";
            Addvalues.TargetViewId = "EmailContentTemplate_DetailView;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            if(View.Id== "EmailContentTemplate_DetailView")
            {
                //ListView viAvailableFields = ((DetailView)View).FindItem("Datasource1") as ListView;
                //DashboardViewItem lstqctype = (ASPxLookupPropertyEditor)((DetailView)View).FindItem("Datasource1");
                DashboardViewItem lstqctype = ((DetailView)View).FindItem("Datasource") as DashboardViewItem;
                if (lstqctype != null)
                {
                    if (lstqctype.InnerView == null)
                    {
                        lstqctype.CreateControl(); 
                    }

                    ListView liAvailableFields = lstqctype.InnerView as ListView;
                    IObjectSpace objectSpace = liAvailableFields.ObjectSpace;

                    if (liAvailableFields != null)
                    {
                        string[] arrayvalues = new string[] { "[IMG]", "[SuboutID]","[ProjectID]","[ProjectName]","[TAT]", "[ReceivedDate]","[JobID]" };

                        foreach (string objtp in arrayvalues)
                        {
                            DataSourceEmailTemplate newItem = objectSpace.CreateObject<DataSourceEmailTemplate>();
                            newItem.DataSource = objtp;
                            liAvailableFields.CollectionSource.Add(newItem);

                        }
                    }
                }
                if (View is DetailView detailView )
                {
                    detailView.ControlsCreated += DetailView_ControlsCreated;
                }






            }
            // Perform various tasks depending on the target View.
        }
        private void Addvalues_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "EmailContentTemplate_DetailView")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    DashboardViewItem lstqctype = ((DetailView)View).FindItem("Datasource") as DashboardViewItem;
                    ListView liAvailableFields = lstqctype.InnerView as ListView;
                    if(liAvailableFields.SelectedObjects != null)
                    {
                        EmailContentTemplate crtobj = (EmailContentTemplate)Application.MainWindow.View.CurrentObject;

                        foreach (DataSourceEmailTemplate objDatasource in liAvailableFields.SelectedObjects)
                        {
                            string mainBodyContent = crtobj.Body;

                            string imgContent = objDatasource.DataSource;
                            int lastClosingTagIndex = mainBodyContent.LastIndexOf("</span>");
                            //int lastClosingTagIndex = mainBodyContent.("</span>");
                            string updatedMainBodyContent = $"{mainBodyContent.Substring(0, lastClosingTagIndex)} {imgContent} {mainBodyContent.Substring(lastClosingTagIndex)}";

                            crtobj.Body = updatedMainBodyContent;
                            //var selectedValue = objDatasource.DataSource;
                            //var page = WebWindow.CurrentRequestPage;
                            //if (page != null)
                            //{
                            //    string script = $"addDataSourceValue('{selectedValue}');";
                            //    //page.ClientScript.RegisterStartupScript(GetType(), "InvokeAddDataSourceValueScript", script, true);
                            //    page.ClientScript.RegisterStartupScript(GetType(), "InvokeAddDataSourceValueScript", script, true);
                            //    crtobj.Body = page;
                            //}

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
        private void DetailView_ControlsCreated(object sender, EventArgs e)
        {
            var detailView = (DetailView)sender;
            var page = WebWindow.CurrentRequestPage;
            if (page != null)
            {
                RegisterClientScript(page);
            }
        }

        private void RegisterClientScript(System.Web.UI.Page page)
        {

            string script = @"
                function insertAtCursor(text) {
                    var editor = document.getElementById('<%# myLink.ClientID %>');
                    var editorDoc = editor.contentDocument || editor.contentWindow.document;
                    var selection = editorDoc.getSelection();
                    var range = selection.getRangeAt(0);
                    
                    range.deleteContents();
                    range.insertNode(editorDoc.createTextNode(text));

                    range.setStartAfter(range.endContainer);
                    selection.removeAllRanges();
                    selection.addRange(range);
                }

                function addDataSourceValue(value) {
                    insertAtCursor(value);
                    alert('Value added: ' + value);
                }
            ";
            page.ClientScript.RegisterStartupScript(GetType(), "AddDataSourceValueScript", script, true);
        }
    





        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            //if (View.Id == "EmailContentTemplate_DetailView")
            //{
            //    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
            //    selparameter.CallbackManager.RegisterHandler(View.Id, this);
            //    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
            //    if (gridlisteditor != null && gridlisteditor.Grid != null)
            //    {
            //        //string strheight = System.Web.HttpContext.Current.Request.Cookies.Get("height").Value;
            //        //gridlisteditor.Grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strheight) - (Convert.ToInt32(strheight) * 33 / 100);
            //        //gridlisteditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            //        //gridlisteditor.Grid.Load += Grid_Load;
            //        gridlisteditor.SelectionOnDoubleClick = true;
            //        gridlisteditor.Grid.ClientSideEvents.RowDblClick = @"function(s,e) { 
            //                s.GetRowValues(e.visibleIndex, 'Datasoruce', function(Value) {      
            //                    RaiseXafCallback(globalCallbackControl, 'TempInfo', 'DSblclck|'+ Value, '', false);                         
            //                }); 
            //            }";
            //    }
            //}
            // Access and customize the target View control.
        }


        public void ProcessAction(string parameter)
        {
            try
            {

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
    }
}
