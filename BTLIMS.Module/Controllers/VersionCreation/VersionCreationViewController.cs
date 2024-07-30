using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Drawing;
using System.Web.UI.WebControls;

namespace LDM.Module.Controllers.VersionCreationViewController
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VersionCreationViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();

        public VersionCreationViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "VersionControl_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("save", false);
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("saveclose", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("savenew", false);
                    Frame.GetController<ModificationsController>().CancelAction.Active.SetItemValue("cancel", false);
                    if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                    {
                        ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
                    }
                }
                //using (IObjectSpace os = CreateObjectSpace())
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    System.Reflection.Assembly assem;
                    System.Reflection.AssemblyName assemname;
                    System.Version assemVersion;
                    assem = System.Reflection.Assembly.GetExecutingAssembly();
                    assemname = assem.GetName();
                    assemVersion = assemname.Version;
                    var strXafVersionNumber = assemname.Version.ToString();
                    VersionControlInfo objVersion = new VersionControlInfo();
                    string VersionNumber = objVersion.VersionNumber;
                    Modules.BusinessObjects.Setting.VersionControl curtverno = os.FindObject<Modules.BusinessObjects.Setting.VersionControl>(CriteriaOperator.Parse("[VersionNumber] = ? And IsNullOrEmpty([XAFVersionNumber])", VersionNumber));
                    if (curtverno != null)
                    {
                        curtverno.XAFVersionNumber = strXafVersionNumber;
                        os.CommitChanges();
                    }
                    //Modules.BusinessObjects.Setting.VersionControl curtverno = os.FindObject<Modules.BusinessObjects.Setting.VersionControl>(CriteriaOperator.Parse("[VersionNumber] = ?", VersionNumber));
                    //if (curtverno == null)
                    //{
                    //    Modules.BusinessObjects.Setting.VersionControl crtverno = os.CreateObject<Modules.BusinessObjects.Setting.VersionControl>();
                    //    if (!string.IsNullOrEmpty(VersionNumber))
                    //    {
                    //        crtverno.VersionNumber = VersionNumber;
                    //    }
                    //    crtverno.XAFVersionNumber = strXafVersionNumber;
                    //    PermissionPolicyUser chkuser = os.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse("[UserName] = 'Administrator' And [GCRecord] Is Null"));
                    //    if (chkuser != null)
                    //    {
                    //        Employee chkemp = os.FindObject<Employee>(CriteriaOperator.Parse("[Oid] = ?", chkuser.Oid));
                    //        if (chkemp != null)
                    //        {
                    //            crtverno.ReleasedBy = os.GetObject<Employee>(chkemp);
                    //        }
                    //        else
                    //        {
                    //            crtverno.ReleasedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        crtverno.ReleasedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    //    }
                    //    crtverno.DateReleased = DateTime.Now;
                    //    os.CommitChanges();
                    //}

                }
                // Perform various tasks depending on the target View.
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (View.Id == "VersionControl_DetailView")
                {
                    if (View is DetailView)
                    {
                        foreach (ViewItem item in ((DetailView)View).Items)
                        {
                            if (item is ASPxStringPropertyEditor)
                            {
                                ASPxStringPropertyEditor editor = (ASPxStringPropertyEditor)item;
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
                }
                if (View.Id == "VersionControl_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid.Columns["InlineEditCommandColumn"] != null)
                    {
                        gridListEditor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                    }
                }
                else if (View.Id == "VersionControl_Posts_ListView_Copy")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden; 
                    }
                }
                // Access and customize the target View control.
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
            try
            {
            base.OnDeactivated();
                if (View.Id == "VersionControl_DetailView")
                {
                    if (Frame.GetController<ModificationsController>().SaveAction.Active.Contains("save"))
                    {
                        Frame.GetController<ModificationsController>().SaveAction.Active.RemoveItem("save"); 
                    }
                    if (Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.Contains("saveclose"))
                    {
                        Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.RemoveItem("saveclose"); 
                    }
                    if (Frame.GetController<ModificationsController>().SaveAndNewAction.Active.Contains("savenew"))
                    {
                        Frame.GetController<ModificationsController>().SaveAndNewAction.Active.RemoveItem("savenew"); 
                    }
                    if (Frame.GetController<ModificationsController>().CancelAction.Active.Contains("cancel"))
                    {
                        Frame.GetController<ModificationsController>().CancelAction.Active.RemoveItem("cancel"); 
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
