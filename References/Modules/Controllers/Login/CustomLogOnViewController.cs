using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Win.Templates;
using DevExpress.XtraBars;
using Modules.BusinessObjects.InfoClass;
using Modules.ConnectionForm;
using System;
using System.Drawing;

namespace Modules.Controllers.Hr
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomLogOnViewController : ViewController<DetailView>
    {
        // SimpleAction Connection=null;
        #region Constructor
        MessageTimer timer = new MessageTimer();
        curlanguage objLanguage = new curlanguage();

        public CustomLogOnViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "CustomLogonParameters_DetailView";
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                // Perform various tasks depending on the target View.
                foreach (StaticTextViewItem item in View.GetItems<StaticTextViewItem>())
                {
                    item.ControlCreated += item_ControlCreated;
                }
                //IObjectSpace os = Application.CreateObjectSpace();
                //string CurrentLang = string.Empty;
                //Session currentSession = ((XPObjectSpace)(os)).Session;
                //SelectedData sproc = currentSession.ExecuteSproc("getCurrentLanguage", "");
                //if (sproc.ResultSet[1].Rows[0].Values[0] != null)
                //{
                //    CurrentLang = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                if (objLanguage.strcurlanguage == "En")
                {
                    Application.SetFormattingCulture("EN-US");
                }
                else
                {
                    Application.SetFormattingCulture("zh");
                }
                //}
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            //Application.CustomizeTemplate += Application_CustomizeTemplate;

        }
        protected override void OnViewChanged()
        {
            try
            {
                base.OnViewChanged();
                if (View != null && View.Id == "CustomLogonParameters_DetailView")
                {
                    if (Frame.Context == TemplateContext.PopupWindow)
                    {
                        ((System.Windows.Forms.Control)((PopupFormBase)Frame.Template).ViewSiteControl).BackColor = Color.Transparent;
                        ((PopupFormBase)Frame.Template).InitialMinimumSize = new Size(500, 300);
                        ((PopupFormBase)Frame.Template).BackColor = Color.White;
                        ((PopupFormBase)Frame.Template).AllowFormSkin = true;
                        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");

                        BarManager bm = new BarManager();
                        Bar bar = new Bar();
                        BarDockControl barDockControlTop = new BarDockControl();
                        bm.Bars.Add(bar);
                        bm.Form = ((PopupFormBase)Frame.Template);

                        bar.BarName = "Tools";
                        bar.DockCol = 0;
                        bar.DockRow = 0;
                        bar.DockStyle = BarDockStyle.Top;

                        //IObjectSpace os = Application.CreateObjectSpace();
                        //string CurrentLang = string.Empty;
                        //Session currentSession = ((XPObjectSpace)(os)).Session;
                        //SelectedData sproc = currentSession.ExecuteSproc("getCurrentLanguage", "");
                        //if (sproc.ResultSet[1].Rows[0].Values[0] != null)
                        //{
                        //    CurrentLang = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                        //    // Application.SetFormattingCulture(CurrentLang);
                        //}
                        BarButtonItem bbi = new BarButtonItem();
                        //if (CurrentLang == "zh-CN")
                        if (objLanguage.strcurlanguage != "En")
                        {
                            bbi.Caption = "服务器连接";
                        }
                        else
                        {
                            bbi.Caption = "Connection";
                        }
                        bbi.Id = 0;
                        bbi.Name = "Connection";
                        bbi.ItemClick += Bbi_ItemClick;
                        bm.Items.Add(bbi);
                        bar.AddItem(bbi);
                    }
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            try
            {
                foreach (StaticTextViewItem item in View.GetItems<StaticTextViewItem>())
                {
                    item.ControlCreated -= item_ControlCreated;
                }
                base.OnDeactivated();
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void Bbi_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmConnection obj = new frmConnection(true);
                obj.Show();
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        void item_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                StaticTextViewItem item = (StaticTextViewItem)sender;
                if (item.Id == "txtCompleteLabSolution")
                {
                    item.Label.Height = 10;

                    item.Label.Appearance.Font = new System.Drawing.Font(item.Label.Font.FontFamily, 15, FontStyle.Bold);
                    //item.Label.Appearance.Font=new Font()
                    item.Label.Appearance.ForeColor = Color.Orange;
                }
                else
                {
                    item.Label.Appearance.Font = new System.Drawing.Font(item.Label.Font.FontFamily, 25, FontStyle.Bold);
                    //item.Label.Appearance.Font=new Font()
                    item.Label.Appearance.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion
    }
}
