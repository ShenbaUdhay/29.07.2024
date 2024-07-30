using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Metrc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace LDM.Module.Controllers.Metrc
{
    public partial class MetrcController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        Metrcinfo metrcinfo = new Metrcinfo();
        string VendorKey;
        string UserKey;
        public MetrcController()
        {
            InitializeComponent();
            TargetViewId = "MetrcIncoming_DetailView;" + "MetrcFacility_LookupListView;" + "MetrcIncomingData_ListView;" + "MetrcIncomingDetData_ListView;";
            getIncoming.TargetViewId = "MetrcIncoming_DetailView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "MetrcIncoming_DetailView")
                {
                    MetrcIncoming metrc = (MetrcIncoming)View.CurrentObject;
                    metrc.From = DateTime.Parse("2021-10-12");
                    metrc.To = DateTime.Parse("2021-10-13");
                    metrcinfo.dtincdatasource = metrcinfo.dtincdetdatasource = new DataTable();
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
            try
            {
                if (View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        if (View.Id == "MetrcIncomingData_ListView")
                        {
                            ICallbackManagerHolder holder = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                            holder.CallbackManager.RegisterHandler("itemname", this);
                            gridListEditor.Grid.Load += Grid_Load;
                            gridListEditor.Grid.ClientSideEvents.CustomButtonClick = @"function(s,e) { RaiseXafCallback(globalCallbackControl, 'itemname', e.buttonID + '|' + e.visibleIndex, '', false); }";
                            gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;

                        }
                        else if (View.Id == "MetrcIncomingDetData_ListView")
                        {
                            gridListEditor.Grid.Load += Grid_Load;
                            gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
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

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                e.Cell.Attributes.Add("onclick", "event.stopPropagation();");
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
                ASPxGridView grid = sender as ASPxGridView;
                if (grid != null)
                {
                    if (View.Id == "MetrcIncomingData_ListView")
                    {
                        getdata(grid);
                    }
                    else if (View.Id == "MetrcIncomingDetData_ListView")
                    {
                        getdetdata(grid);
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
            base.OnDeactivated();
        }

        public BindingList<MetrcFacility> getfacility()
        {
            try
            {
                getkey();
                metrcinfo.dtFacilitydatasource = new BindingList<MetrcFacility>();
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{VendorKey}:{UserKey}")));
                    var response = Task.Run(() => client.GetAsync("https://sandbox-api-mi.metrc.com/facilities/v2/")).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string content = Task.Run(() => response.Content.ReadAsStringAsync()).Result;
                        if (!string.IsNullOrEmpty(content))
                        {
                            JArray data = JArray.Parse(content);
                            foreach (JObject a in data)
                            {
                                metrcinfo.dtFacilitydatasource.Add(new MetrcFacility() { Facility = a.GetValue("Name").ToString(), LicenseNumber = a.GetValue("License")["Number"].ToString() });
                            }
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(response.StatusCode.ToString(), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    return metrcinfo.dtFacilitydatasource;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return new BindingList<MetrcFacility>();
            }
        }

        private void getIncoming_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                MetrcIncoming metrc = (MetrcIncoming)View.CurrentObject;
                if (metrc != null && metrc.Facility != null)
                {
                    getkey();
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{VendorKey}:{UserKey}")));
                        var response = Task.Run(() => client.GetAsync(string.Format("https://sandbox-api-mi.metrc.com/transfers/v2/incoming?lastModifiedStart={0}&lastModifiedEnd={1}&licenseNumber={2}", metrc.From.ToString("yyyy-MM-dd"), metrc.To.ToString("yyyy-MM-dd"), metrc.Facility.LicenseNumber.Trim()))).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string content = Task.Run(() => response.Content.ReadAsStringAsync()).Result;
                            if (!string.IsNullOrEmpty(content))
                            {
                                metrcinfo.dtincdatasource = new DataTable();
                                JObject data = JObject.Parse(content);
                                foreach (var a in data.Properties().AsJEnumerable().Select(a => a.Value).First())
                                {
                                    var jTokenProperties = a.Children().OfType<JProperty>();
                                    DataRow dr = metrcinfo.dtincdatasource.NewRow();
                                    foreach (JProperty property in jTokenProperties)
                                    {
                                        if (!metrcinfo.dtincdatasource.Columns.Contains(property.Name))
                                        {
                                            metrcinfo.dtincdatasource.Columns.Add(property.Name);
                                        }
                                        dr[property.Name] = property.Value.ToString();
                                    }
                                    metrcinfo.dtincdatasource.Rows.Add(dr);
                                }
                                DashboardViewItem viewItem = ((DetailView)View).FindItem("IncomingData") as DashboardViewItem;
                                if (viewItem != null && viewItem.InnerView != null)
                                {
                                    ASPxGridListEditor gridListEditor = ((ListView)viewItem.InnerView).Editor as ASPxGridListEditor;
                                    if (gridListEditor != null && gridListEditor.Grid != null)
                                    {
                                        getdata(gridListEditor.Grid);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(response.StatusCode.ToString(), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                //else
                //{
                //    Application.ShowViewStrategy.ShowMessage("Facility must not be empty", InformationType.Error, 5000, InformationPosition.Top);

                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void insertdata()
        {
            IObjectSpace os = Application.CreateObjectSpace(typeof(MetrcIncomingHistory));
            foreach (DataRow dr in metrcinfo.dtincdatasource.Rows)
            {
                var data = JsonConvert.SerializeObject(dr);
                var hash = data.GetHashCode();
                MetrcIncomingHistory metrchis = os.FindObject<MetrcIncomingHistory>(CriteriaOperator.Parse("[hash]=?", hash));
                if (metrchis == null)
                {
                    MetrcIncomingHistory metrc = os.CreateObject<MetrcIncomingHistory>();
                    metrc.data = data;
                    metrc.hash = hash;
                }
            }
            if (os.IsModified)
            {
                os.CommitChanges();
            }
        }

        private void getdata(ASPxGridView grid)
        {
            try
            {
                if (metrcinfo.dtincdatasource != null && metrcinfo.dtincdatasource.Rows.Count > 0)
                {
                    insertdata();
                    grid.Columns.Clear();
                    GridViewCommandColumn column = new GridViewCommandColumn();
                    GridViewCommandColumnCustomButton detcolumn = new GridViewCommandColumnCustomButton();
                    detcolumn.ID = "Details";
                    detcolumn.Text = "Details";
                    column.CustomButtons.Add(detcolumn);
                    grid.Columns.Add(column);
                    foreach (DataColumn col in metrcinfo.dtincdatasource.Columns)
                    {
                        GridViewDataColumn data_column = new GridViewDataTextColumn();
                        data_column.FieldName = col.ColumnName;
                        data_column.Caption = col.ColumnName;
                        data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        data_column.ShowInCustomizationForm = false;
                        data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                        grid.Columns.Add(data_column);
                    }
                    grid.KeyFieldName = "Id";
                    grid.DataSource = metrcinfo.dtincdatasource;
                    grid.DataBind();
                    grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) 
                        {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth(totusablescr - 75); 
                        }                                            
                    }";
                }
                else
                {
                    DataTable dttemp = new DataTable();
                    dttemp.Columns.Add("NoData");
                    GridViewDataColumn data_column = new GridViewDataTextColumn();
                    data_column.FieldName = "NoData";
                    data_column.Caption = " ";
                    data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    data_column.ShowInCustomizationForm = false;
                    data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                    grid.Columns.Add(data_column);
                    grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
                    grid.ClientSideEvents.Init = null;
                    grid.DataSource = dttemp;
                    grid.DataBind();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void getdetdata(ASPxGridView grid)
        {
            try
            {
                if (metrcinfo.dtincdetdatasource != null && metrcinfo.dtincdetdatasource.Rows.Count > 0)
                {
                    grid.Columns.Clear();
                    foreach (DataColumn col in metrcinfo.dtincdetdatasource.Columns)
                    {
                        GridViewDataColumn data_column = new GridViewDataTextColumn();
                        data_column.FieldName = col.ColumnName;
                        data_column.Caption = col.ColumnName;
                        data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        data_column.ShowInCustomizationForm = false;
                        data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                        grid.Columns.Add(data_column);
                    }
                    grid.KeyFieldName = "PackageId";
                    grid.DataSource = metrcinfo.dtincdetdatasource;
                    grid.DataBind();
                    grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                }
                else
                {
                    DataTable dttemp = new DataTable();
                    dttemp.Columns.Add("NoData");
                    GridViewDataColumn data_column = new GridViewDataTextColumn();
                    data_column.FieldName = "NoData";
                    data_column.Caption = " ";
                    data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    data_column.ShowInCustomizationForm = false;
                    data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                    grid.Columns.Add(data_column);
                    grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
                    grid.ClientSideEvents.Init = null;
                    grid.DataSource = dttemp;
                    grid.DataBind();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void getkey()
        {
            try
            {
                if (WebConfigurationManager.AppSettings["MetrcVendorV2"] != null)
                {
                    VendorKey = WebConfigurationManager.AppSettings["MetrcVendorV2"];
                }
                if (WebConfigurationManager.AppSettings["MetrcUser"] != null)
                {
                    UserKey = WebConfigurationManager.AppSettings["MetrcUser"];
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
            if (!string.IsNullOrEmpty(parameter))
            {
                string[] cmd = parameter.Split('|');
                if (cmd[0] == "Details")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        string DeliveryID = gridListEditor.Grid.GetRowValues(int.Parse(cmd[1]), "DeliveryId").ToString();
                        if (!string.IsNullOrEmpty(DeliveryID))
                        {
                            getincomingdetailsdata(DeliveryID);
                            ListView CreatedListView = Application.CreateListView(typeof(MetrcIncomingDetData), false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(CreatedListView);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = DeliveryID;
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                }
            }
        }

        private void getincomingdetailsdata(string id)
        {
            getkey();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{VendorKey}:{UserKey}")));
                var response = Task.Run(() => client.GetAsync(string.Format("https://sandbox-api-mi.metrc.com/transfers/v2/deliveries/{0}/packages", id))).Result;
                if (response.IsSuccessStatusCode)
                {
                    string content = Task.Run(() => response.Content.ReadAsStringAsync()).Result;
                    if (!string.IsNullOrEmpty(content))
                    {
                        metrcinfo.dtincdetdatasource = new DataTable();
                        JObject data = JObject.Parse(content);
                        foreach (var a in data.Properties().AsJEnumerable().Select(a => a.Value).First())
                        {
                            var jTokenProperties = a.Children().OfType<JProperty>();
                            DataRow dr = metrcinfo.dtincdetdatasource.NewRow();
                            foreach (JProperty property in jTokenProperties)
                            {
                                if (!metrcinfo.dtincdetdatasource.Columns.Contains(property.Name))
                                {
                                    metrcinfo.dtincdetdatasource.Columns.Add(property.Name);
                                }
                                dr[property.Name] = property.Value.ToString();
                            }
                            metrcinfo.dtincdetdatasource.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(response.StatusCode.ToString(), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
        }
    }
}
