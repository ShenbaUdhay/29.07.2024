using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Office.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class HelpCenterController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        HelpCenterInfo hcInfo = new HelpCenterInfo();
        //bool manualFAQ = false;
        public HelpCenterController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "HelpCenter_ListView;" + "HelpCenter;" + "HelpCenter_DetailView_FAQ_Articles;" + "HelpCenter_ListView_FAQ_Articles;" + "HelpCenter_Copy;" + "HelpCenter_HelpCenterAttachments_ListView;" + "HelpCenter_HelpCenterAttachments_ListView_Download;"
                + "HelpCenter_ListView_Articles;" + "HelpCenter_DetailView_Article;" + "UserManualEditorCategory_ListView;" + "UserManualEditorCategory_DetailView;" + "CustomReportBuilder_DetailView;" + "HelpCenter_ListView_Articles_Manual;";
            FullText.TargetViewId = "HelpCenter_ListView_Articles;";
            ArticalRelease.TargetViewId = "HelpCenter_DetailView_FAQ_Articles;" + "HelpCenter_ListView_FAQ_Articles;";
            HelpCenterArtical.TargetViewId = "HelpCenter_ListView_Articles;";
            ArticalCategory.TargetViewId = "HelpCenter_Copy;";
            downloadAction.TargetViewId = "HelpCenter_ListView_Articles;" + "HelpCenter_ListView_Articles_Manual;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                HelpCenterArtical.Active.SetItemValue("valArticle", false);
                if (View.Id == "UserManualEditorCategory_DetailView" || View.Id == "HelpCenter_DetailView_FAQ_Articles")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Execute += SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Execute += SaveAction_Execute;
                }
                if (View.Id == "HelpCenter_ListView")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is null");
                    ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;

                }
                if (View.Id == "HelpCenter_DetailView_FAQ_Articles")
                {
                    FileDataPropertyEditor FilePropertyEditor = ((DetailView)View).FindItem("Upload") as FileDataPropertyEditor;
                    if (FilePropertyEditor != null)
                    {
                        FilePropertyEditor.ControlCreated += FilePropertyEditor_ControlCreated;
                    }
                    ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                }
                else if (View.Id == "HelpCenter_DetailView_Article")
                {
                    string strheight = System.Web.HttpContext.Current.Request.Cookies.Get("height").Value;
                    ASPxRichTextPropertyEditor richTextPropertyEditor = ((DetailView)View).FindItem("Article") as ASPxRichTextPropertyEditor;
                    if (richTextPropertyEditor != null)
                    {
                        if (!string.IsNullOrEmpty(strheight) && int.TryParse(strheight, out int a))
                        {
                            richTextPropertyEditor.Height = Convert.ToInt32(strheight) - (Convert.ToInt32(strheight) * 30 / 100);
                            //editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                        //richTextPropertyEditor.Height = 680;
                    }
                }
                if (View.Id == "UserManualEditorCategory_DetailView" || View.Id == "UserManualEditorCategory_ListView" || View.Id == "HelpCenter_ListView_FAQ_Articles" || View.Id == "HelpCenter_DetailView_FAQ_Articles" || View.Id == "HelpCenter_HelpCenterAttachments_ListView")
                {
                    DeleteObjectsViewController objDelete = Frame.GetController<DeleteObjectsViewController>();
                    objDelete.DeleteAction.Executing += DeleteAction_Executing;
                }
                if (View.Id == "CustomReportBuilder_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                if (View.Id == "HelpCenter_Copy")
                {
                    ArticalCategory.SelectedItemChanged += ArticalCategory_SelectedItemChanged;
                    hcInfo.manualFAQ = true;

                    //ArticalCategory.SelectedItem = ArticalCategory.Items[0];
                    //hcInfo.SelectedCategory = ArticalCategory.SelectedItem.Id;
                    if (hcInfo.SelectedCategory == "Manual")
                    {
                        ArticalCategory.SelectedItem = ArticalCategory.Items[1];
                        hcInfo.SelectedCategory = ArticalCategory.SelectedItem.Id;
                    }
                    else
                    {
                        ArticalCategory.SelectedItem = ArticalCategory.Items[0];
                        hcInfo.SelectedCategory = ArticalCategory.SelectedItem.Id;
                    }
                    //ArticalCategory.SelectedItem = ArticalCategory.Items[0];                    
                    //hcInfo.SelectedCategory = ArticalCategory.SelectedItem.Id;
                    DashboardViewItem vilstReports = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView_Manual") as DashboardViewItem;
                    if (vilstReports != null)
                    {
                        if (vilstReports.Control == null)
                        {
                            vilstReports.CreateControl();
                        }
                        ((Control)vilstReports.Control).Visible = false;
                    }
                    DashboardViewItem vilstFAQ = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView") as DashboardViewItem;
                    if (vilstFAQ != null)
                    {
                        if (vilstFAQ.Control == null)
                        {
                            vilstFAQ.CreateControl();
                        }
                        ((Control)vilstFAQ.Control).Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (e.CurrentObject != null)
                {
                    if (View.ObjectTypeInfo.Type == typeof(UserManualEditorCategory))
                    {
                        UserManualEditorCategory category = (UserManualEditorCategory)e.CurrentObject;
                        if (category != null)
                        {
                            string strHelpcenterConString = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("AlpacaLimsHelpCenterConnectionStrings"))["AlpacaLimsHelpCenterConnectionString"];
                            if (!string.IsNullOrEmpty(strHelpcenterConString))
                            {
                                SqlConnection sqlconnection = new SqlConnection(strHelpcenterConString);
                                SqlCommand catchksqlcmd = new SqlCommand("Select * from UserManualEditorCategory Where GCRecord Is NUll And Oid = '" + category.Oid + "'", sqlconnection);
                                sqlconnection.Open();
                                DataTable dt = new DataTable();
                                SqlDataAdapter sda = new SqlDataAdapter(catchksqlcmd);
                                sda.Fill(dt);
                                if (dt != null && dt.Rows.Count == 0)
                                {
                                    SqlCommand catinsertcmd = new SqlCommand("Insert into UserManualEditorCategory (Oid,Category,Comment,CreatedDate,ModifiedDate) values(N'" + category.Oid + "','" + category.Category + "','" + category.Comment + "','" + category.CreatedDate + "','" + category.ModifiedDate + "')", sqlconnection);
                                    SqlDataReader dataReader = catinsertcmd.ExecuteReader();
                                }
                                else if (dt != null && dt.Rows.Count > 0)
                                {
                                    SqlCommand catinsertcmd = new SqlCommand("Update UserManualEditorCategory Set Category = '" + category.Category + "',Comment ='" + category.Comment + "',CreatedDate ='" + category.CreatedDate + "',ModifiedDate = '" + category.ModifiedDate + "' Where Oid = '" + category.Oid + "'", sqlconnection);
                                    SqlDataReader dataReader = catinsertcmd.ExecuteReader();
                                }
                                sqlconnection.Close();
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("Helpcenter connection details not available.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            }
                        }
                    }
                    else if (View.ObjectTypeInfo.Type == typeof(HelpCenter))
                    {
                        HelpCenter objhelpcent = (HelpCenter)e.CurrentObject;
                        if (objhelpcent != null)
                        {
                            string strHelpcenterConString = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("AlpacaLimsHelpCenterConnectionStrings"))["AlpacaLimsHelpCenterConnectionString"];
                            if (!string.IsNullOrEmpty(strHelpcenterConString))
                            {
                                string strcategory = null;
                                if (objhelpcent.Category != null)
                                {
                                    strcategory = "'" + objhelpcent.Category.Oid.ToString() + "'";
                                }
                                else
                                {
                                    strcategory = "Null";
                                }
                                SqlConnection sqlconnection = new SqlConnection(strHelpcenterConString);
                                SqlCommand hpchksqlcmd = new SqlCommand("Select * from HelpCenter Where GCRecord Is NUll And Oid = '" + objhelpcent.Oid + "'", sqlconnection);
                                sqlconnection.Open();
                                DataTable dt = new DataTable();
                                SqlDataAdapter sda = new SqlDataAdapter(hpchksqlcmd);
                                sda.Fill(dt);
                                if (dt != null && dt.Rows.Count == 0)
                                {
                                    int hcstyle = (int)objhelpcent.Style;
                                    string strhcinsert = "Insert into HelpCenter(Oid,Topic,Article,Size,Module,BusinessObject,DateReleased,Active,ModifiedDate,Category,Style) values(N'" + objhelpcent.Oid + "', '" + objhelpcent.Topic + "','" + objhelpcent.Article + "', '" + objhelpcent.Size + "', '" + objhelpcent.Module + "', '" + objhelpcent.BusinessObject + "', '" + objhelpcent.DateReleased + "', " + Convert.ToInt32(objhelpcent.Active) + ", '" + objhelpcent.ModifiedDate + "', " + strcategory + ", " + hcstyle + ")";
                                    SqlCommand hcinsertcmd = new SqlCommand(strhcinsert, sqlconnection);
                                    SqlDataReader hcinsertsqlDataReader = hcinsertcmd.ExecuteReader();
                                    hcinsertsqlDataReader.Close();
                                    SqlCommand hpattchksqlcmd = new SqlCommand("Select * from HelpCenterAttachments Where Topic = '" + objhelpcent.Oid + "' And GCRecord Is Null", sqlconnection);
                                    DataTable hlpattdt = new DataTable();
                                    SqlDataAdapter hlpattada = new SqlDataAdapter(hpattchksqlcmd);
                                    hlpattada.Fill(hlpattdt);
                                    if (hlpattdt != null && hlpattdt.Rows.Count == 0)
                                    {
                                        if (objhelpcent.HelpCenterAttachments != null && objhelpcent.HelpCenterAttachments.Count > 0)
                                        {
                                            foreach (HelpCenterAttachments objhcatt in objhelpcent.HelpCenterAttachments.ToList())
                                            {
                                                if (objhcatt.File != null)
                                                {
                                                    SqlCommand filedatachksqlcmd = new SqlCommand("Select * from FileData Where Oid ='" + objhcatt.File.Oid + "' And GCRecord Is Null", sqlconnection);
                                                    DataTable dtFD = new DataTable();
                                                    SqlDataAdapter FDada = new SqlDataAdapter(filedatachksqlcmd);
                                                    FDada.Fill(dtFD);
                                                    if (dt != null && dt.Rows.Count == 0)
                                                    {
                                                        string strfiledata = "insert into FileData(Oid,[size],[FileName],Content) Values(N'" + objhcatt.File.Oid + "','" + objhcatt.File.Size + "','" + objhcatt.File.FileName + "',Cast('" + objhcatt.File.Content + "'As varbinary(max)))"; //Cast('" + objhcatt.File.Content + "'As varbinary(max))
                                                        SqlCommand hcattFDinsertsqlcmd = new SqlCommand(strfiledata, sqlconnection);
                                                        SqlDataReader hcattFDinsertsqlDataReader = hcattFDinsertsqlcmd.ExecuteReader();
                                                        hcattFDinsertsqlDataReader.Close();
                                                    }
                                                }
                                                string strhcattinsertcmd = "Insert Into HelpCenterAttachments(Oid,Topic,Title,Size,[File],Sort,[Date],CreatedDate,ModifiedDate)values(N'" + objhcatt.Oid + "','" + objhcatt.Topic.Oid + "','" + objhcatt.Title + "','" + objhcatt.Size + "','" + objhcatt.File.Oid + "','" + objhcatt.Sort + "','" + objhcatt.Date + "','" + objhcatt.CreatedDate + "','" + objhcatt.ModifiedDate + "')";
                                                SqlCommand hcattinsertsqlcmd = new SqlCommand(strhcattinsertcmd, sqlconnection);
                                                SqlDataReader hcattinsertsqlDataReader = hcattinsertsqlcmd.ExecuteReader();
                                                hcattinsertsqlDataReader.Close();
                                            }
                                        }
                                    }
                                }
                                else if (dt != null && dt.Rows.Count > 0)
                                {
                                    int hcstyle = (int)objhelpcent.Style;
                                    string strhcinsert = "Update HelpCenter Set Topic = '" + objhelpcent.Topic + "', Article = '" + objhelpcent.Article + "', Size = '" + objhelpcent.Size + "', Module = '" + objhelpcent.Module + "', BusinessObject = '" + objhelpcent.BusinessObject + "', DateReleased = '" + objhelpcent.DateReleased + "', Active = " + Convert.ToInt32(objhelpcent.Active) + ", ModifiedDate = '" + objhelpcent.ModifiedDate + "', Category = " + strcategory + ", Style = '" + hcstyle + "' Where Oid = '" + objhelpcent.Oid + "'";
                                    SqlCommand hcinsertcmd = new SqlCommand(strhcinsert, sqlconnection);
                                    SqlDataReader hcinsertsqlDataReader = hcinsertcmd.ExecuteReader();
                                    hcinsertsqlDataReader.Close();
                                    SqlCommand hpattchksqlcmd = new SqlCommand("Select * from HelpCenterAttachments Where Topic = '" + objhelpcent.Oid + "' And GCRecord Is Null", sqlconnection);
                                    DataTable hlpattdt = new DataTable();
                                    SqlDataAdapter hlpattada = new SqlDataAdapter(hpattchksqlcmd);
                                    hlpattada.Fill(hlpattdt);
                                    if (hlpattdt != null && hlpattdt.Rows.Count == 0)
                                    {
                                        if (objhelpcent.HelpCenterAttachments != null && objhelpcent.HelpCenterAttachments.Count > 0)
                                        {
                                            foreach (HelpCenterAttachments objhcatt in objhelpcent.HelpCenterAttachments.ToList())
                                            {
                                                if (objhcatt.File != null)
                                                {
                                                    SqlCommand filedatachksqlcmd = new SqlCommand("Select * from FileData Where Oid ='" + objhcatt.File.Oid + "' And GCRecord Is Null", sqlconnection);
                                                    DataTable dtFD = new DataTable();
                                                    SqlDataAdapter FDada = new SqlDataAdapter(filedatachksqlcmd);
                                                    FDada.Fill(dtFD);
                                                    if (dtFD != null && dtFD.Rows.Count == 0)
                                                    {
                                                        string strfiledata = "insert into FileData(Oid,[size],[FileName],Content) Values(N'" + objhcatt.File.Oid + "','" + objhcatt.File.Size + "','" + objhcatt.File.FileName + "',Cast('" + objhcatt.File.Content + "'As varbinary(max)))"; //Cast('" + objhcatt.File.Content + "'As varbinary(max))
                                                        SqlCommand hcattFDinsertsqlcmd = new SqlCommand(strfiledata, sqlconnection);
                                                        SqlDataReader hcattFDinsertsqlDataReader = hcattFDinsertsqlcmd.ExecuteReader();
                                                        hcattFDinsertsqlDataReader.Close();
                                                    }
                                                }
                                                string strhcattinsertcmd = "Insert Into HelpCenterAttachments(Oid,Topic,Title,Size,[File],Sort,[Date],CreatedDate,ModifiedDate)values(N'" + objhcatt.Oid + "','" + objhcatt.Topic.Oid + "','" + objhcatt.Title + "','" + objhcatt.Size + "','" + objhcatt.File.Oid + "','" + objhcatt.Sort + "','" + objhcatt.Date + "','" + objhcatt.CreatedDate + "','" + objhcatt.ModifiedDate + "')";
                                                SqlCommand hcattinsertsqlcmd = new SqlCommand(strhcattinsertcmd, sqlconnection);
                                                SqlDataReader hcattinsertsqlDataReader = hcattinsertsqlcmd.ExecuteReader();
                                                hcattinsertsqlDataReader.Close();
                                            }
                                        }
                                    }
                                    else if (hlpattdt != null && hlpattdt.Rows.Count > 0)
                                    {
                                        if (objhelpcent.HelpCenterAttachments != null && objhelpcent.HelpCenterAttachments.Count > 0)
                                        {
                                            foreach (HelpCenterAttachments objhcatt in objhelpcent.HelpCenterAttachments.ToList())
                                            {
                                                if (objhcatt.File != null)
                                                {
                                                    SqlCommand filedatachksqlcmd = new SqlCommand("Select * from FileData Where Oid ='" + objhcatt.File.Oid + "'", sqlconnection);
                                                    DataTable dtFD = new DataTable();
                                                    SqlDataAdapter FDada = new SqlDataAdapter(filedatachksqlcmd);
                                                    FDada.Fill(dtFD);
                                                    if (dtFD != null && dtFD.Rows.Count == 0)
                                                    {
                                                        string strfiledata = "insert into FileData(Oid,[size],[FileName],Content) Values(N'" + objhcatt.File.Oid + "','" + objhcatt.File.Size + "','" + objhcatt.File.FileName + "',Cast('" + objhcatt.File.Content + "'As varbinary(max)))"; //Cast('" + objhcatt.File.Content + "'As varbinary(max))
                                                        SqlCommand hcattFDinsertsqlcmd = new SqlCommand(strfiledata, sqlconnection);
                                                        SqlDataReader hcattFDinsertsqlDataReader = hcattFDinsertsqlcmd.ExecuteReader();
                                                        hcattFDinsertsqlDataReader.Close();
                                                    }
                                                }
                                                SqlCommand hpattupdatechksqlcmd = new SqlCommand("Select * from HelpCenterAttachments Where Oid ='" + objhcatt.Oid + "' And GCRecord Is Null", sqlconnection);
                                                DataTable hlpattdt1 = new DataTable();
                                                SqlDataAdapter hlpattada1 = new SqlDataAdapter(hpattupdatechksqlcmd);
                                                hlpattada1.Fill(hlpattdt1);
                                                if (hlpattdt1 != null && hlpattdt1.Rows.Count == 0)
                                                {
                                                    string strhcattinsertcmd = "Insert Into HelpCenterAttachments(Oid,Topic,Title,Size,[File],Sort,[Date],CreatedDate,ModifiedDate)values(N'" + objhcatt.Oid + "','" + objhcatt.Topic.Oid + "','" + objhcatt.Title + "','" + objhcatt.Size + "','" + objhcatt.File.Oid + "','" + objhcatt.Sort + "','" + objhcatt.Date + "','" + objhcatt.CreatedDate + "','" + objhcatt.ModifiedDate + "')";
                                                    SqlCommand hcattinsertsqlcmd = new SqlCommand(strhcattinsertcmd, sqlconnection);
                                                    SqlDataReader hcattinsertsqlDataReader = hcattinsertsqlcmd.ExecuteReader();
                                                    hcattinsertsqlDataReader.Close();
                                                }
                                                else if (hlpattdt1 != null && hlpattdt1.Rows.Count > 0)
                                                {
                                                    string strhcattinsertcmd = "Update HelpCenterAttachments Set Topic ='" + objhcatt.Topic.Oid + "',Title ='" + objhcatt.Title + "',Size='" + objhcatt.Size + "',[File]='" + objhcatt.File.Oid + "',Sort='" + objhcatt.Sort + "',[Date]='" + objhcatt.Date + "',CreatedDate='" + objhcatt.CreatedDate + "',ModifiedDate='" + objhcatt.ModifiedDate + "' Where Oid ='" + objhcatt.Oid + "'";
                                                    SqlCommand hcattinsertsqlcmd = new SqlCommand(strhcattinsertcmd, sqlconnection);
                                                    SqlDataReader hcattinsertsqlDataReader = hcattinsertsqlcmd.ExecuteReader();
                                                    hcattinsertsqlDataReader.Close();
                                                }
                                            }
                                        }
                                    }
                                }
                                sqlconnection.Close();
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("Helpcenter connection details not available.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
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

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View.Id == "CustomReportBuilder_DetailView")
                {
                    if (e.PropertyName == "Module")
                    {
                        Modules.BusinessObjects.SampleManagement.CustomReportBuilder objHelp = (Modules.BusinessObjects.SampleManagement.CustomReportBuilder)e.Object;
                        BusinessObjectCustomstringComboPropertyEditor customStringCombo = ((DetailView)View).FindItem("BusinessObject") as BusinessObjectCustomstringComboPropertyEditor;
                        if (customStringCombo != null && objHelp != null && !string.IsNullOrEmpty(objHelp.Module))
                        {
                            if (((ASPxComboBox)customStringCombo.Editor).Items.Count > 0)
                            {
                                ((ASPxComboBox)customStringCombo.Editor).Items.Clear();
                            }
                            SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("SelectBusinessObject_Sp", new OperandValue(objHelp.Module));
                            if (sprocReport.ResultSet != null)
                            {
                                foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                                {
                                    ((ASPxComboBox)customStringCombo.Editor).Items.Add(row.Values[0].ToString());
                                }
                            }

                            //dni.CreateControl();
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

        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "UserManualEditorCategory_ListView")
                {
                    foreach (UserManualEditorCategory objCategory in View.SelectedObjects)
                    {
                        HelpCenter objFieldSetup = View.ObjectSpace.FindObject<HelpCenter>(CriteriaOperator.Parse("[Category.Oid] = ?", objCategory.Oid));
                        if (objFieldSetup != null)

                        {
                            Application.ShowViewStrategy.ShowMessage("Category cannot be deleted! It has been linked in another forms.", InformationType.Error, 5000, InformationPosition.Top);
                        }
                        else
                        {
                            if (View is ListView && View.SelectedObjects.Count > 0)
                            {
                                foreach (UserManualEditorCategory category in View.SelectedObjects)
                                {
                                    if (category != null)
                                    {
                                        string strHelpcenterConString = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("AlpacaLimsHelpCenterConnectionStrings"))["AlpacaLimsHelpCenterConnectionString"];
                                        if (!string.IsNullOrEmpty(strHelpcenterConString))
                                        {
                                            SqlConnection sqlconnection = new SqlConnection(strHelpcenterConString);
                                            SqlCommand catchksqlcmd = new SqlCommand("Select * from UserManualEditorCategory Where GCRecord Is NUll And Oid = '" + category.Oid + "'", sqlconnection);
                                            sqlconnection.Open();
                                            DataTable dt = new DataTable();
                                            SqlDataAdapter sda = new SqlDataAdapter(catchksqlcmd);
                                            sda.Fill(dt);
                                            if (dt != null && dt.Rows.Count > 0)
                                            {
                                                SqlCommand catinsertcmd = new SqlCommand("Update UserManualEditorCategory Set GCRecord = '12345' Where Oid = '" + category.Oid + "'", sqlconnection);
                                                SqlDataReader dataReader = catinsertcmd.ExecuteReader();
                                                dataReader.Close();
                                            }
                                            sqlconnection.Close();
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Helpcenter connection details not available.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        }
                                    }
                                }

                            }
                            ObjectSpace.Delete(objCategory);
                            ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    ObjectSpace.Refresh();
                }

                if (View.Id == "UserManualEditorCategory_DetailView")
                {
                    UserManualEditorCategory objCategory = View.CurrentObject as UserManualEditorCategory;

                    HelpCenter objFieldSetup = View.ObjectSpace.FindObject<HelpCenter>(CriteriaOperator.Parse("[Category.Oid] = ?", objCategory.Oid));
                    if (objFieldSetup != null)
                    {
                        Application.ShowViewStrategy.ShowMessage("Category cannot be deleted! It has been linked in another forms.", InformationType.Error, 5000, InformationPosition.Top);
                    }
                    else
                    {
                        if (View is DetailView && View.CurrentObject != null)
                        {
                            UserManualEditorCategory category = (UserManualEditorCategory)View.CurrentObject;
                            if (category != null)
                            {
                                string strHelpcenterConString = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("AlpacaLimsHelpCenterConnectionStrings"))["AlpacaLimsHelpCenterConnectionString"];
                                if (!string.IsNullOrEmpty(strHelpcenterConString))
                                {
                                    SqlConnection sqlconnection = new SqlConnection(strHelpcenterConString);
                                    SqlCommand catchksqlcmd = new SqlCommand("Select * from UserManualEditorCategory Where GCRecord Is NUll And Oid = '" + category.Oid + "'", sqlconnection);
                                    sqlconnection.Open();
                                    DataTable dt = new DataTable();
                                    SqlDataAdapter sda = new SqlDataAdapter(catchksqlcmd);
                                    sda.Fill(dt);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        SqlCommand catinsertcmd = new SqlCommand("Update UserManualEditorCategory Set GCRecord = '12345' Where Oid = '" + category.Oid + "'", sqlconnection);
                                        SqlDataReader dataReader = catinsertcmd.ExecuteReader();
                                        dataReader.Close();
                                    }
                                    sqlconnection.Close();
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Helpcenter connection details not available.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                }
                            }
                        }
                        ObjectSpace.Delete(objCategory);
                        ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }

                    ObjectSpace.Refresh();
                }

                if (View.Id == "HelpCenter_ListView_FAQ_Articles" && View.SelectedObjects.Count > 0)
                {
                    string strHelpcenterConString = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("AlpacaLimsHelpCenterConnectionStrings"))["AlpacaLimsHelpCenterConnectionString"];
                    if (!string.IsNullOrEmpty(strHelpcenterConString))
                    {
                        SqlConnection sqlconnection = new SqlConnection(strHelpcenterConString);
                        sqlconnection.Open();
                        foreach (HelpCenter objhpc in View.SelectedObjects)
                        {
                            SqlCommand catchksqlcmd = new SqlCommand("Select * from HelpCenter Where GCRecord Is NUll And Oid = '" + objhpc.Oid + "'", sqlconnection);
                            DataTable dt = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(catchksqlcmd);
                            sda.Fill(dt);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                SqlCommand catinsertcmd = new SqlCommand("Update HelpCenter Set GCRecord = '12345' Where Oid = '" + objhpc.Oid + "'", sqlconnection);
                                SqlDataReader dataReader = catinsertcmd.ExecuteReader();
                                dataReader.Close();
                            }
                        }
                        sqlconnection.Close();
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Helpcenter connection details not available.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
                if (View.Id == "HelpCenter_DetailView_FAQ_Articles" && View.CurrentObject != null)
                {
                    HelpCenter objhlpcenter = (HelpCenter)View.CurrentObject;
                    if (objhlpcenter != null)
                    {
                        string strHelpcenterConString = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("AlpacaLimsHelpCenterConnectionStrings"))["AlpacaLimsHelpCenterConnectionString"];
                        if (!string.IsNullOrEmpty(strHelpcenterConString))
                        {
                            SqlConnection sqlconnection = new SqlConnection(strHelpcenterConString);
                            sqlconnection.Open();
                            SqlCommand catchksqlcmd = new SqlCommand("Select * from HelpCenter Where GCRecord Is NUll And Oid = '" + objhlpcenter.Oid + "'", sqlconnection);
                            DataTable dt = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(catchksqlcmd);
                            sda.Fill(dt);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                SqlCommand catinsertcmd = new SqlCommand("Update HelpCenter Set GCRecord = '12345' Where Oid = '" + objhlpcenter.Oid + "'", sqlconnection);
                                SqlDataReader dataReader = catinsertcmd.ExecuteReader();
                                dataReader.Close();
                            }
                            sqlconnection.Close();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Helpcenter connection details not available.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                if (View.Id == "HelpCenter_HelpCenterAttachments_ListView" && View.SelectedObjects.Count > 0)
                {
                    string strHelpcenterConString = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("AlpacaLimsHelpCenterConnectionStrings"))["AlpacaLimsHelpCenterConnectionString"];
                    if (!string.IsNullOrEmpty(strHelpcenterConString))
                    {
                        SqlConnection sqlconnection = new SqlConnection(strHelpcenterConString);
                        sqlconnection.Open();
                        foreach (HelpCenterAttachments objhpcatt in View.SelectedObjects)
                        {
                            SqlCommand catchksqlcmd = new SqlCommand("Select * from HelpCenterAttachments Where GCRecord Is NUll And Title = '" + objhpcatt.Title + "'" + "And Topic = '" + objhpcatt.Topic.Oid + "'", sqlconnection);
                            DataTable dt = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(catchksqlcmd);
                            sda.Fill(dt);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                SqlCommand catinsertcmd = new SqlCommand("Update HelpCenterAttachments Set GCRecord = '12345' Where Title = '" + objhpcatt.Title + "'" + "And Topic = '" + objhpcatt.Topic.Oid + "'", sqlconnection);
                                SqlDataReader dataReader = catinsertcmd.ExecuteReader();
                                dataReader.Close();
                            }
                        }
                        sqlconnection.Close();
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Helpcenter connection details not available.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
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
                if (View.Id == "HelpCenter_DetailView_FAQ_Articles")
                {
                    if (e.PropertyName == "Module")
                    {
                        HelpCenter objHelp = (HelpCenter)e.Object;
                        BusinessObjectCustomstringComboPropertyEditor customStringCombo = ((DetailView)View).FindItem("BusinessObject") as BusinessObjectCustomstringComboPropertyEditor;
                        if (customStringCombo != null && objHelp != null && !string.IsNullOrEmpty(objHelp.Module))
                        {
                            if (((ASPxComboBox)customStringCombo.Editor).Items.Count > 0)
                            {
                                ((ASPxComboBox)customStringCombo.Editor).Items.Clear();
                            }
                            SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("SelectBusinessObject_Sp", new OperandValue(objHelp.Module));
                            if (sprocReport.ResultSet != null)
                            {
                                foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                                {
                                    ((ASPxComboBox)customStringCombo.Editor).Items.Add(row.Values[0].ToString());
                                }
                            }

                            //dni.CreateControl();
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

        private void FilePropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                FileDataEdit FileControl = ((FileDataPropertyEditor)sender).Editor;
                if (FileControl != null)
                {
                    FileControl.UploadControlCreated += FileControl_UploadControlCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FileControl_UploadControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxUploadControl FileUploadControl = ((FileDataEdit)sender).UploadControl;
                FileUploadControl.ValidationSettings.AllowedFileExtensions = new String[] { ".pdf" };
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
                if (View.Id == "HelpCenter_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.ShowColumnHeaders = false;
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    }
                }
                else if (View.Id == "HelpCenter")
                {
                    DashboardViewItem dvItem = ((DashboardView)View).FindItem("dvHelpCenterListView") as DashboardViewItem;
                    if (dvItem != null && dvItem.InnerView != null && FullText.Value != null && !string.IsNullOrEmpty(FullText.Value.ToString()))
                    {
                        ((ListView)dvItem.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Contains([Topic], ?)", FullText.Value.ToString());
                        ((Control)dvItem.Control).Visible = true;
                    }
                    else
                    {
                        ((Control)dvItem.Control).Visible = false;
                    }
                }
                else
                if (View.Id == "HelpCenter_ListView_Articles" || View.Id == "HelpCenter_ListView_Articles_Manual")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    View.CurrentObject = null;
                    gridListEditor.Grid.SettingsPager.PageSizeItemSettings.Visible = false;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;

                    //((ListView)View).CollectionSource.Criteria["FAQFilter"] = CriteriaOperator.Parse("[Active] = True And [Category.Category] = 'FAQ'");
                    if (gridListEditor.Grid.Columns["downloadAction"] != null)
                    {
                        gridListEditor.Grid.Columns["downloadAction"].Visible = true;
                        gridListEditor.Grid.Columns["downloadAction"].Caption = "Video";
                        if (gridListEditor.Grid.Columns["downloadAction"].VisibleIndex < gridListEditor.Columns.Max(i => i.VisibleIndex))
                        {
                            gridListEditor.Grid.Columns["downloadAction"].VisibleIndex = gridListEditor.Columns.Max(i => i.VisibleIndex) + 1;
                        }
                    }
                }
                if (View.Id == "HelpCenter_Copy")
                {
                    if (ArticalCategory.SelectedItem != null)
                    {
                        if (ArticalCategory.SelectedItem.Id == "FAQ")
                        {
                            DashboardViewItem vilstReports = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView_Manual") as DashboardViewItem;
                            if (vilstReports != null)
                            {
                                if (vilstReports.Control == null)
                                {
                                    vilstReports.CreateControl();
                                }
                                ((Control)vilstReports.Control).Visible = false;
                            }
                            DashboardViewItem vilstFAQ = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView") as DashboardViewItem;
                            if (vilstFAQ != null)
                            {
                                if (vilstFAQ.Control == null)
                                {
                                    vilstFAQ.CreateControl();
                                }
                                ((Control)vilstFAQ.Control).Visible = true;
                            }
                        }
                        else if (ArticalCategory.SelectedItem.Id == "Manual")
                        {
                            DashboardViewItem vilstReports = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView") as DashboardViewItem;
                            if (vilstReports != null)
                            {
                                if (vilstReports.Control == null)
                                {
                                    vilstReports.CreateControl();
                                }
                                ((Control)vilstReports.Control).Visible = false;
                            }
                            DashboardViewItem vilstManual = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView_Manual") as DashboardViewItem;
                            if (vilstManual != null)
                            {
                                if (vilstManual.Control == null)
                                {
                                    vilstManual.CreateControl();
                                }
                                ((Control)vilstManual.Control).Visible = true;
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

        private void ArticalCategory_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "HelpCenter_Copy" && View != null && View != null && !View.IsDisposed)
                {
                    if (ArticalCategory.SelectedItem != null)
                    {
                        hcInfo.SelectedCategory = ArticalCategory.SelectedItem.Id;
                        hcInfo.SelectedIndex = ArticalCategory.SelectedIndex;
                        if (ArticalCategory.SelectedItem.Id == "FAQ")
                        {
                            DashboardViewItem vilstReports = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView_Manual") as DashboardViewItem;
                            if (vilstReports != null && vilstReports.Control != null)
                            {
                                ((Control)vilstReports.Control).Visible = false;
                            }

                            DashboardViewItem vilstFAQ = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView") as DashboardViewItem;
                            if (vilstFAQ != null && vilstFAQ.Control != null)
                            {
                                ((Control)vilstFAQ.Control).Visible = true;
                            }
                        }
                        else if (ArticalCategory.SelectedItem.Id == "Manual")
                        {
                            DashboardViewItem vilstReports = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView") as DashboardViewItem;
                            if (vilstReports != null && vilstReports.Control != null)
                            {
                                ((Control)vilstReports.Control).Visible = false;
                            }
                            DashboardViewItem vilstManual = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView_Manual") as DashboardViewItem;
                            if (vilstManual != null && vilstManual.Control != null)
                            {
                                ((Control)vilstManual.Control).Visible = true;
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

        private void LstCurrObj_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject is HelpCenter)
                {
                    HelpCenter obj = (HelpCenter)e.InnerArgs.CurrentObject;
                    if (obj != null)
                    {
                        MemoryStream ms = new MemoryStream();
                        NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                        PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                        obj.Upload.SaveToStream(ms);
                        objToShow.PDFData = ms.ToArray();
                        objToShow.ViewID = "Articles";

                        DetailView CreatedDetailView = Application.CreateDetailView(Popupos, "PDFPreview_DetailView_Articles", false, objToShow);
                        CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        //showViewParameters.CreatedView.Caption = "PDFViewer";
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.AcceptAction.Active.SetItemValue("disable", false);
                        dc.CancelAction.Active.SetItemValue("disable", false);
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        e.Handled = true;
                    }
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
            try
            {
                if (View.Id == "UserManualEditorCategory_DetailView" || View.Id == "HelpCenter_DetailView_FAQ_Articles")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Execute -= SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Execute -= SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Execute -= SaveAction_Execute;
                }
                if (View.Id == "UserManualEditorCategory_DetailView" || View.Id == "UserManualEditorCategory_ListView" || View.Id == "HelpCenter_ListView_FAQ_Articles" || View.Id == "HelpCenter_DetailView_FAQ_Articles" || View.Id == "HelpCenter_HelpCenterAttachments_ListView")
                {
                    DeleteObjectsViewController objDelete = Frame.GetController<DeleteObjectsViewController>();
                    objDelete.DeleteAction.Executing -= DeleteAction_Executing;
                }
                if (View.Id == "HelpCenter_ListView")
                {
                    ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    lstCurrObj.CustomProcessSelectedItem -= LstCurrObj_CustomProcessSelectedItem;
                }
                else if (View.Id == "HelpCenter_DetailView_FAQ_Articles")
                {
                    ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                }

                if (View.Id == "HelpCenter_Copy")
                {
                    ArticalCategory.SelectedItemChanged -= ArticalCategory_SelectedItemChanged;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FullText_Execute_1(object sender, ParametrizedActionExecuteEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.ParameterCurrentValue.ToString()))
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Contains([Topic], ?) Or Contains([BusinessObject], ?) Or Contains([Module], ?)", e.ParameterCurrentValue.ToString(), e.ParameterCurrentValue.ToString(), e.ParameterCurrentValue.ToString());
                }
                else if (((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
                {
                    ((ListView)View).CollectionSource.Criteria.Remove("Filter");
                }
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ReleaseArtical_Excecute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    foreach (HelpCenter objHelpCenter in View.SelectedObjects)
                    {
                        objHelpCenter.Active = true;
                        objHelpCenter.DateReleased = DateTime.Now;
                        objHelpCenter.ReleasedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        ObjectSpace.CommitChanges();
                    }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "RelesedSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                     .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Artical_Excecute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                HelpCenter objHelpCenter = objectSpace.GetObject((HelpCenter)View.CurrentObject);
                DetailView dv = Application.CreateDetailView(objectSpace, "HelpCenter_DetailView_Article", true, objHelpCenter);
                ShowViewParameters showViewParameters = new ShowViewParameters();
                showViewParameters.CreatedView = dv;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.AcceptAction.Active.SetItemValue("Ok", false);
                dc.CancelAction.Active.SetItemValue("Cancel", false);
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                    .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void downloadAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                HelpCenter ftpHelpCenter = (HelpCenter)e.CurrentObject;
                if (ftpHelpCenter != null)
                {
                    if (ArticalCategory.SelectedItem != null)
                    {
                        hcInfo.SelectedCategory = ArticalCategory.SelectedItem.Id;
                    }
                    else if (!string.IsNullOrEmpty(hcInfo.SelectedCategory))
                    {
                        ArticalCategory.SelectedItem = ArticalCategory.Items.FirstOrDefault(i => i.Id == hcInfo.SelectedCategory);
                    }
                    if (ftpHelpCenter.HelpCenterAttachments.Count > 1)
                    {
                        CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(HelpCenterAttachments));
                        cs.Criteria["filter"] = CriteriaOperator.Parse("[Topic.Oid] = ?", ftpHelpCenter.Oid);
                        ListView listview = Application.CreateListView("HelpCenter_HelpCenterAttachments_ListView_Download", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                        //showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("disable", false);
                        dc.CancelAction.Active.SetItemValue("disable", false);
                        dc.ViewClosed += Dc_ViewClosed;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        //e.ShowViewParameters.Context = TemplateContext.PopupWindow;
                        //e.ShowViewParameters.CreatedView = listview;
                    }
                    else if (ftpHelpCenter.HelpCenterAttachments.Count == 1)
                    {
                        HelpCenterAttachments objHelpCenterAttachment = ftpHelpCenter.HelpCenterAttachments.FirstOrDefault();
                        if (objHelpCenterAttachment != null)
                        {
                            MemoryStream ms = new MemoryStream();
                            objHelpCenterAttachment.File.SaveToStream(ms);
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\VideoReports\")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\VideoReports\"));
                            }
                            string strLocalFile = HttpContext.Current.Server.MapPath((@"~\VideoReports\" + objHelpCenterAttachment.File.FileName));
                            if (File.Exists(strLocalFile))
                            {
                                File.Delete(strLocalFile);
                            }
                            File.WriteAllBytes(strLocalFile, ms.ToArray());

                            DownloadHttpHandler dtcp = new DownloadHttpHandler();
                            dtcp.Bytes = ms.ToArray();
                            string strExtension = Path.GetExtension(objHelpCenterAttachment.File.FileName);
                            dtcp.FileName = objHelpCenterAttachment.File.FileName;
                            if (string.IsNullOrEmpty(dtcp.FileName))
                            {
                                dtcp.FileName = ftpHelpCenter.Topic + "(1)" + strExtension;
                            }
                            dtcp.ContentType = "application/" + strExtension.Replace(".", string.Empty);
                            dtcp.FilePath = strLocalFile;
                            dtcp.ProcessRequest(HttpContext.Current);
                            //    if (File.Exists(strLocalFile))
                            //    {
                            //        File.Delete(strLocalFile);
                            //    }
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

        public void downloadfile()
        {
            try
            {
                objectspaceinfo objinfo = new objectspaceinfo();
                HelpCenterInfo helpCenterInfo = new HelpCenterInfo();
                IObjectSpace objectSpace = objinfo.tempobjspace;
                HelpCenter ftpHelpCenter = objectSpace.FindObject<HelpCenter>(CriteriaOperator.Parse("[Topic] = ?", helpCenterInfo.StrTopic));
                if (ftpHelpCenter != null)
                {
                    if (ftpHelpCenter.HelpCenterAttachments.Count == 1)
                    {
                        HelpCenterAttachments objHelpCenterAttachment = ftpHelpCenter.HelpCenterAttachments.FirstOrDefault();
                        if (objHelpCenterAttachment != null)
                        {
                            MemoryStream ms = new MemoryStream();
                            objHelpCenterAttachment.File.SaveToStream(ms);
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\VideoReports\")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\VideoReports\"));
                            }
                            string strLocalFile = HttpContext.Current.Server.MapPath((@"~\VideoReports\" + objHelpCenterAttachment.File.FileName));
                            if (File.Exists(strLocalFile))
                            {
                                File.Delete(strLocalFile);
                            }
                            File.WriteAllBytes(strLocalFile, ms.ToArray());

                            DownloadHttpHandler dtcp = new DownloadHttpHandler();
                            dtcp.Bytes = ms.ToArray();
                            string strExtension = Path.GetExtension(objHelpCenterAttachment.File.FileName);
                            dtcp.FileName = objHelpCenterAttachment.File.FileName;
                            if (string.IsNullOrEmpty(dtcp.FileName))
                            {
                                dtcp.FileName = ftpHelpCenter.Topic + "(1)" + strExtension;
                            }
                            dtcp.ContentType = "application/" + strExtension.Replace(".", string.Empty);
                            dtcp.FilePath = strLocalFile;
                            dtcp.ProcessRequest(HttpContext.Current);
                        }
                    }
                    else if (ftpHelpCenter.HelpCenterAttachments.Count > 1)
                    {
                        foreach (HelpCenterAttachments objfileattach in ftpHelpCenter.HelpCenterAttachments.ToList())
                        {
                            if (objfileattach.Title == helpCenterInfo.StrDownloadTopic)
                            {
                                HelpCenterAttachments objHelpCenterAttachment = objfileattach;
                                if (objHelpCenterAttachment != null)
                                {
                                    MemoryStream ms = new MemoryStream();
                                    objHelpCenterAttachment.File.SaveToStream(ms);
                                    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\VideoReports\")) == false)
                                    {
                                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\VideoReports\"));
                                    }
                                    string strLocalFile = HttpContext.Current.Server.MapPath((@"~\VideoReports\" + objHelpCenterAttachment.File.FileName));
                                    if (File.Exists(strLocalFile))
                                    {
                                        File.Delete(strLocalFile);
                                    }
                                    File.WriteAllBytes(strLocalFile, ms.ToArray());

                                    DownloadHttpHandler dtcp = new DownloadHttpHandler();
                                    dtcp.Bytes = ms.ToArray();
                                    string strExtension = Path.GetExtension(objHelpCenterAttachment.File.FileName);
                                    dtcp.FileName = objHelpCenterAttachment.File.FileName;
                                    if (string.IsNullOrEmpty(dtcp.FileName))
                                    {
                                        dtcp.FileName = ftpHelpCenter.Topic + "(1)" + strExtension;
                                    }
                                    dtcp.ContentType = "application/" + strExtension.Replace(".", string.Empty);
                                    dtcp.FilePath = strLocalFile;
                                    dtcp.ProcessRequest(HttpContext.Current);
                                }
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

        private void Dc_ViewClosed(object sender, EventArgs e)
        {
            try
            {
                if (Application.MainWindow.View.Id == "HelpCenter_Copy")
                {
                    if (!string.IsNullOrEmpty(hcInfo.SelectedCategory) && ArticalCategory.SelectedItem == null)
                    {
                        ArticalCategory.SelectedItem = ArticalCategory.Items.FirstOrDefault(i => i.Id == hcInfo.SelectedCategory);
                    }
                    else if (ArticalCategory.SelectedItem == null)
                    {
                        ArticalCategory.SelectedIndex = 0;
                    }

                    if (ArticalCategory.SelectedItem.Id == "FAQ")
                    {
                        DashboardViewItem vilstReports = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView_Manual") as DashboardViewItem;
                        if (vilstReports != null)
                        {
                            if (vilstReports.Control == null)
                            {
                                vilstReports.CreateControl();
                            }
                            ((Control)vilstReports.Control).Visible = false;
                            vilstReports.Refresh();
                        }
                        DashboardViewItem vilstFAQ = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView") as DashboardViewItem;
                        if (vilstFAQ != null)
                        {
                            if (vilstFAQ.Control == null)
                            {
                                vilstFAQ.CreateControl();
                            }
                            ((Control)vilstFAQ.Control).Visible = true;
                            vilstFAQ.Refresh();
                        }
                    }
                    else if (ArticalCategory.SelectedItem.Id == "Manual")
                    {
                        DashboardViewItem vilstReports = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView") as DashboardViewItem;
                        if (vilstReports != null)
                        {
                            if (vilstReports.Control == null)
                            {
                                vilstReports.CreateControl();
                            }
                            ((Control)vilstReports.Control).Visible = false;
                        }
                        DashboardViewItem vilstManual = ((DashboardView)Application.MainWindow.View).FindItem("dvHelpCenterListView_Manual") as DashboardViewItem;
                        if (vilstManual != null)
                        {
                            if (vilstManual.Control == null)
                            {
                                vilstManual.CreateControl();
                            }
                            ((Control)vilstManual.Control).Visible = true;
                        }
                    }
                    Application.MainWindow.View.ObjectSpace.Refresh();
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
