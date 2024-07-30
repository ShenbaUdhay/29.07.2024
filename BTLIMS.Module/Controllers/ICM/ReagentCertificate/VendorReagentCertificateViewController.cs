using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl;
using LDM.Module.Controllers.Public;
using LDM.Module.Controllers.Public.FTPSetup;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using Rebex.Net;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.ICM.ReagentCertificate
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VendorReagentCertificateViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        ShowNavigationItemController ShowNavigationController;
        FtpInfo objFTPInfo = new FtpInfo();
        VendorReagentInfo vendorreagentInfo = new VendorReagentInfo();
        //private readonly object showNew;

        public VendorReagentCertificateViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //TargetViewId = "VendorReagentCertificate_DV;"+ "VendorReagentCertificate_ListView;"+ "VendorReagentCertificate_ListView_ViewCertificateLog;"+ "VendorReagentCertificate_DetailView;";
            TargetViewId = "VendorReagentCertificate_DetailView;" + "VendorReagentCertificate_ListView;" + "VendorReagentCertificate_Upload_ListView;";
            ShowViewModeVRC.TargetViewId = "VendorReagentCertificate_ListView";
            ShowNew.TargetViewId = "VendorReagentCertificate_Upload_ListView";

            VRCDateFilter.TargetViewId = "VendorReagentCertificate_ListView";

        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                // Perform various tasks depending on the target View.
                Frame.GetController<VendorReagentCertificateViewController>().Actions["ShowViewModeVRC"].Active.SetItemValue("", true);
                Frame.GetController<VendorReagentCertificateViewController>().Actions["VRCDateFilter"].Active.SetItemValue("", false);
                if (View != null && View.Id == "VendorReagentCertificate_DetailView")
                {
                    View.ObjectSpace.Committing += ObjectSpace_Committing;
                    View.ObjectSpace.Committed += ObjectSpace_Committed;
                }
                if (View.Id == "VendorReagentCertificate_ListView")
                {
                    if (VRCDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        VRCDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        VRCDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        VRCDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        VRCDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        VRCDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        VRCDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        VRCDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    //VRCDateFilter.SelectedIndex = 0;
                    //DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    //if (VRCDateFilter.SelectedItem == null)
                    //{
                    //    if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                    //    {
                    //        VRCDateFilter.SelectedItem = VRCDateFilter.Items[0];
                    //        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 1");
                    //    }
                    //    else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                    //    {
                    //        VRCDateFilter.SelectedItem = VRCDateFilter.Items[1];
                    //        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 3");
                    //    }
                    //    else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                    //    {
                    //        VRCDateFilter.SelectedItem = VRCDateFilter.Items[2];
                    //        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 6");
                    //    }
                    //    else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                    //    {
                    //        VRCDateFilter.SelectedItem = VRCDateFilter.Items[3];
                    //        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffYear(LoadedDate, Now()) <= 1");
                    //    }
                    //    else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                    //    {
                    //        VRCDateFilter.SelectedItem = VRCDateFilter.Items[4];
                    //        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffYear(LoadedDate, Now()) <= 2");
                    //    }
                    //    else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                    //    {
                    //        VRCDateFilter.SelectedItem = VRCDateFilter.Items[5];
                    //        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffYear(LoadedDate, Now()) <= 5");
                    //    }
                    //    else if(setting.InventoryWorkFlow == EnumDateFilter.All)
                    //    {
                    //        VRCDateFilter.SelectedItem = VRCDateFilter.Items[6];
                    //        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[LoadedDate] IS Not NULL AND [LoadedBy] IS Not NULL");
                    //    }                        
                    //    //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    //}
                    VRCDateFilter.Execute += VRCDateFilter_Execute;
                    //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 3");
                }

                //if (View is DashboardView)
                //{
                //    DashboardViewItem AddCertificate = ((DashboardView)View).FindItem("VendorReagentCertificate") as DashboardViewItem;
                //    DashboardViewItem ViewCertificateLog = ((DashboardView)View).FindItem("ViewCertificateLog") as DashboardViewItem;
                //    if (AddCertificate != null)
                //    {
                //        AddCertificate.ControlCreated += AddCertificate_ControlCreated;
                //    }
                //    if (ViewCertificateLog != null)
                //    {
                //        ViewCertificateLog.ControlCreated += ViewCertificateLog_ControlCreated;
                //    }                
                //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 3");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "VendorReagentCertificate_DetailView")
                {
                    VendorReagentCertificate VendorRC = (VendorReagentCertificate)View.CurrentObject;
                    if (VendorRC.Upload.Count == 0)
                    {
                        VendorRC.LoadedDate = null;
                        VendorRC.LoadedBy = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            //throw new NotImplementedException();
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                //if(View !=null && View.Id== "VendorReagentCertificate_DetailView" && View.CurrentObject !=null)
                //{
                //    ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                //    {
                //        if (parent.Id == "InventoryManagement")
                //        {
                //            foreach (ChoiceActionItem child in parent.Items)
                //            {
                //                if (child.Id == "Operations")
                //                {
                //                    foreach (ChoiceActionItem subchild in child.Items)
                //                    {
                //                        if (subchild.Id == "VendorReagentCertificate")
                //                        {
                //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                //                            var count = objectSpace.GetObjectsCount(typeof(VendorReagentCertificate), CriteriaOperator.Parse("[LoadedDate] IS NULL AND [LoadedBy] IS NULL"));
                //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                            if (count > 0)
                //                            {
                //                                subchild.Caption = cap[0] + " (" + count + ")";
                //                            }
                //                            else
                //                            {
                //                                subchild.Caption = cap[0];
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    string LocalPath = string.Empty;
                //    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\VendorReagentCertificates")) == false)
                //    {
                //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\VendorReagentCertificates"));
                //    }
                //    VendorReagentCertificate VendorRC = (VendorReagentCertificate)View.CurrentObject;
                //    //UploadFileToFTP(VendorRC);     //if we want to save the Vendor Certificate into FTP nw can use this Method.     
                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //}
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
                // Access and customize the target View control.
                if (View != null && View.Id == "VendorReagentCertificate_DetailView")
                {
                    VendorReagentCertificate vendorRC = (VendorReagentCertificate)View.CurrentObject;
                    if (vendorRC != null && vendorRC.LoadedDate == null && vendorRC.LoadedBy == null)
                    {
                        vendorRC.LoadedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        vendorRC.LoadedDate = Library.GetServerTime(ObjectSpace);
                    }
                }
                if (VRCDateFilter.Active == false)
                {
                    if (View != null && View.Id == "VendorReagentCertificate_ListView")
                    {
                        //((ListView)View).CollectionSource.Criteria["Filter"]=CriteriaOperator.Parse("[LoadedDate] BTWEEN ")
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[LoadedDate] IS NULL AND [LoadedBy] IS NULL");
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

            VRCDateFilter.Execute -= VRCDateFilter_Execute;

        }
        private void UploadFileToFTP(VendorReagentCertificate VendorRC)
        {
            try
            {
                string LocalPath = string.Empty;
                //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\VendorReagentCertificates")) == false)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\VendorReagentCertificates"));
                }
                if (VendorRC.Upload != null)
                {
                    foreach (FileUploadCollectionVRC FileVRC in VendorRC.Upload.ToList())
                    {
                        LocalPath = HttpContext.Current.Server.MapPath(@"~\VendorReagentCertificates\" + FileVRC.File.FileName + "");
                        MemoryStream ms = new MemoryStream();
                        FileVRC.File.SaveToStream(ms);
                        ms.Position = 0;
                        using (FileStream file = new FileStream(LocalPath, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            ms.CopyTo(file);
                        }
                        objFTPInfo.ReadXmlFile_FTPConc();
                        Rebex.Net.Ftp _FTP = objFTPInfo.GetFTPConnection();
                        if (_FTP.State == FtpState.Ready)
                        {
                            if (!_FTP.DirectoryExists("//VendorReagentCertificates"))
                            {
                                _FTP.CreateDirectory("//VendorReagentCertificates");
                            }
                            //_FTP.PutFile(ms, "//VendorReagentCertificates" + VendorRC.Upload.FileName + "");
                            if (!_FTP.FileExists("//VendorReagentCertificates//" + FileVRC.File.FileName + ""))
                            {
                                _FTP.PutFile(LocalPath, "//VendorReagentCertificates//" + FileVRC.File.FileName + "");
                            }
                            //else
                            //{
                            //    Application.ShowViewStrategy.ShowMessage("File Already Uploaded to FTP");
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

        private void ShowViewModeVRC_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "VendorReagentCertificate_ListView")
                {
                    ObjectSpace.Refresh();
                    Frame.GetController<VendorReagentCertificateViewController>().Actions["ShowViewModeVRC"].Active.SetItemValue("", false);
                    Frame.GetController<VendorReagentCertificateViewController>().Actions["VRCDateFilter"].Active.SetItemValue("", true);
                    ((ListView)View).CollectionSource.Criteria.Clear();
                    //VRCDateFilter.SelectedIndex = 1;
                    //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 3");
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                    {
                        VRCDateFilter.SelectedIndex = 0;
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 1");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                    {
                    VRCDateFilter.SelectedIndex = 1;
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 3");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                    {
                        VRCDateFilter.SelectedIndex = 2;
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 6");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                    {
                        VRCDateFilter.SelectedIndex = 3;
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffYear(LoadedDate, Now()) <= 1");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                    {
                        VRCDateFilter.SelectedIndex = 4;
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffYear(LoadedDate, Now()) <= 2");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                    {
                        VRCDateFilter.SelectedIndex = 5;
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffYear(LoadedDate, Now()) <= 5");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.All)
                    {
                        VRCDateFilter.SelectedIndex = 6;
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[LoadedDate] IS Not NULL AND [LoadedBy] IS Not NULL");
                    }
                    //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[LoadedDate] BETWEEN('" + DateTime.Now.AddMonths(-3) + "', '" + DateTime.Now + "')");
                    //VRCDateFilter.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ShowNew_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                FileUploadCollectionVRC objFileUpload = os.CreateObject<FileUploadCollectionVRC>();
                DetailView dv = Application.CreateDetailView(os, "FileUploadCollectionVRC_DetailView", false, objFileUpload);
                dv.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                vendorreagentInfo.FileOid = Guid.NewGuid();
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                dc.Accepting += Dc_Accepting;
                dc.AcceptAction.Executed += AcceptAction_Executed;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.CurrentObject != null && e.AcceptActionArgs.CurrentObject is FileUploadCollectionVRC)
                {
                    //vendorreagentInfo.FileOid = ((FileUploadCollectionVRC)e.AcceptActionArgs.CurrentObject).Oid;
                    FileUploadCollectionVRC objPopupWindow = (FileUploadCollectionVRC)e.AcceptActionArgs.CurrentObject;
                    if (objPopupWindow != null)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        FileUploadCollectionVRC obj = os.CreateObject<FileUploadCollectionVRC>();
                        //obj.File = os.GetObject<FileData>(objPopupWindow.File);
                        if (objPopupWindow.File != null)
                        {
                            FileData objFile = os.CreateObject<FileData>();
                            objFile.FileName = objPopupWindow.File.FileName;
                            objFile.Content = objPopupWindow.File.Content;
                            obj.File = objFile;
                            os.CommitChanges();
                            vendorreagentInfo.FileOid = obj.Oid;
                            os.Dispose();
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

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (Application.MainWindow.View.Id == "VendorReagentCertificate_DetailView")
                {
                    VendorReagentCertificate objVendorCertificate = (VendorReagentCertificate)Application.MainWindow.View.CurrentObject;
                    if (objVendorCertificate != null)
                    {
                        FileUploadCollectionVRC objFileUpload = Application.MainWindow.View.ObjectSpace.GetObjectByKey<FileUploadCollectionVRC>(vendorreagentInfo.FileOid);
                        if (objFileUpload != null)
                        {
                            objVendorCertificate.Upload.Add(objFileUpload);
                            Application.MainWindow.View.Refresh();
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

        private void VRCDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedChoiceActionItem.Id == "1M")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 1");
                }
                else if (e.SelectedChoiceActionItem.Id == "3M")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 3");
                }
                else if (e.SelectedChoiceActionItem.Id == "6M")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffMonth(LoadedDate, Now()) <= 6");
                }
                else if (e.SelectedChoiceActionItem.Id == "1Y")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffYear(LoadedDate, Now()) <= 1");
                }
                else if (e.SelectedChoiceActionItem.Id == "2Y")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffYear(LoadedDate, Now()) <= 2");
                }
                else if (e.SelectedChoiceActionItem.Id == "5Y")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("DateDiffYear(LoadedDate, Now()) <= 5");
                }
                else if (e.SelectedChoiceActionItem.Id == "ALL")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[LoadedDate] IS Not NULL AND [LoadedBy] IS Not NULL");
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
