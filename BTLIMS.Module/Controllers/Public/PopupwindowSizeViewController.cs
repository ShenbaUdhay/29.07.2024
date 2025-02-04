﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PopupwindowSizeViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        QuotesInfo quotesinfo = new QuotesInfo();
        int itemcnt = 0;
        bool IsPopup = false;
        public PopupwindowSizeViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (Frame.GetType() == typeof(DevExpress.ExpressApp.Web.PopupWindow))
                {
                    if (View is DetailView)
                    {
                        foreach (ViewItem item in ((DetailView)View).Items)
                        {
                            itemcnt++;
                        }
                        IsPopup = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
                e.PopupControl.CustomizePopupControl += PopupControl_CustomizePopupControl;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupControl(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupControlEventArgs e)
        {
            try
            {
                if (View != null && e.PopupFrame.View.Id == "SampleBottleAllocation_DetailView_STPassword" || e.PopupFrame.View.Id == "PLMCopyToCombo_DetailView")
                {
                    e.PopupControl.AllowResize = false;
                    e.PopupControl.ShowMaximizeButton = false;
                }
                else if(e.PopupFrame.View!=null && e.PopupFrame.View.Id== "AuditData_DetailView")
                {
                    if(((DetailView)e.PopupFrame.View).ViewEditMode== ViewEditMode.Edit)
                    {
                    e.PopupControl.ShowCloseButton = false;
                    }
                }
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" || e.PopupFrame.View.Id == "COCSettingsSampleRegistration")
                {
                    e.PopupFrame.View.Caption = "Samples";
                    e.Width = new System.Web.UI.WebControls.Unit(1200);
                    e.Height = new System.Web.UI.WebControls.Unit(700);
                    e.Handled = true;
                }
                if (e.PopupFrame.View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration")
                {
                    e.PopupFrame.View.Caption = "Tests";
                    e.Width = new System.Web.UI.WebControls.Unit(1200);
                    e.Height = new System.Web.UI.WebControls.Unit(700);
                    e.Handled = true;
                }
                if (e.PopupFrame.View.Id == "COCSettingsBottleAllocation_DetailView_Copy_SampleRegistration")
                {
                    e.PopupFrame.View.Caption = "Containers";
                    e.Width = new System.Web.UI.WebControls.Unit(1210);
                    e.Height = new System.Web.UI.WebControls.Unit(520);
                    e.Handled = true;
                }
                if (e.PopupFrame.View.Id == "COCTest")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1400);
                    e.Height = new System.Web.UI.WebControls.Unit(648);
                    e.Handled = true;
                }
                if (e.PopupFrame.View.Id == "COC_CopyNoOfSamples_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(800);
                    e.Height = new System.Web.UI.WebControls.Unit(400);
                    e.Handled = true;
                }
                else if (e.PopupFrame.View.Id == "ItemsFileUpload_DetailView")
                {
                    e.Width = 600;
                    e.Height = 200;
                    e.Handled = true;
                }
                else if (e.PopupFrame.View.Id == "AuditData_ListView")
                {
                    e.Width = 1200;
                    e.Height = 680;
                    e.Handled = true;
                }
                else if (e.PopupFrame.View.Id == "AuditData_DetailView")
                {
                    e.Width = 650;
                    e.Height = 325;
                    e.Handled = true;
                }
                //else if (e.PopupFrame.View.Id == "COCSample_DetailView")
                //{
                //    e.Width = 270;
                //    e.Height = 200;
                //    e.Handled = true;
                //}

                if (View != null && View.Id == "CRMQuotes_DetailView" && View.CurrentObject != null)
                {
                    CRMQuotes popupcurtquote = (CRMQuotes)View.CurrentObject;
                    if (popupcurtquote != null)
                    {
                        quotesinfo.popupcurtquote = popupcurtquote;
                    }
                }
                if (View != null && e.PopupFrame != null && e.PopupFrame.View != null && e.PopupFrame.View.ObjectTypeInfo != null && e.PopupFrame.View.ObjectTypeInfo.Type == typeof(ItemChargePricing))
                {
                    e.Width = 1200;
                    e.Height = 590;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "InvoicingAddress_DetailView")
                {
                    e.Width = 1200;
                    e.Height = 500;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "InvoicingContact_DetailView")
                {
                    e.Width = 1200;
                    e.Height = 500;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "ReportingContact_DetailView")
                {
                    e.Width = 1200;
                    e.Height = 500;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "SampleSites_DetailView_Client")
                {
                    e.Width = 1200;
                    e.Height = 500;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Schedular_DetailView")
                {
                    e.Width = 1200;
                    e.Height = 500;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "CRMQuote_DetailView_CRMProspects")
                {
                    e.Width = 1200;
                    e.Height = 500;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "DummyClass_DetailView_Reasons")
                {
                    e.PopupFrame.View.Caption = "Reason";
                    e.Width = 800;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "CRMProspects_Quote_ListView")
                {
                    e.Width = 1200;
                    e.Height = 700;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Notes_DetailView")
                {
                    e.Width = 800;
                    e.Height = 400;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "CloseCRMProspects_popup_DetailView_Copy" && Application.MainWindow.View != null && Application.MainWindow.View.Id == "CRMProspects_DetailView")
                {
                    e.Width = 600;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "ItemsFileUpload_DetailView" && Application.MainWindow.View != null && Application.MainWindow.View.Id == "CRMProspects_ListView")
                {
                    e.Width = 600;
                    e.Height = 300;
                    e.Handled = true;
                }
                else

                if (View != null && e.PopupFrame.View.Id == "TestFileUpload_DetailView")
                {
                    e.Width = 600;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Parameter_ListView_ContractLab")
                {
                    e.Width = 600;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "NPCOCSettingsSample_Bottle_DetailView")
                {
                    e.Width = 600;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "NPSamplingSample_Bottle_DetailView")
                {
                    e.Width = 600;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "SL_CopyNoOfSamples_DetailView")
                {
                    e.Width = 600;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "COCSettingsSamples_LookupListView_Copy_COCSamples_Copy_CopyTest")
                {
                    e.Width = 600;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "DummyClass_ListView_COCSettingsSample")
                {
                    e.Width = 600;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "DummyClass_ListView_Sampling")
                {
                    e.Width = 600;
                    e.Height = 500;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "CertifiedTests_DetailView")
                {
                    e.PopupFrame.View.Caption = "Add Test";
                    e.Width = 700;
                    e.Height = 400;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Contact_DetailView_popup")
                {
                    //e.PopupFrame.View.Caption = "Add Test";
                    e.Width = 800;
                    e.Height = 800;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Customer_LookupListView")
                {
                    //e.PopupFrame.View.Caption = "Add Test";
                    e.Width = 1200;
                    e.Height = 800;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Customer_DetailView_SampleRegistration")
                {
                    //e.PopupFrame.View.Caption = "Add Test";
                    e.Width = 1100;
                    e.Height = 700;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "CalibrationTrending_DetailView")
                {
                    e.PopupFrame.View.Caption = "Trending";
                    e.Width = 1400;
                    e.Height = 600;
                    e.Handled = true;
                }
                //if (View != null && View.Id == "eNotificationContentTemplate_DetailView_Copy")
                //{
                //    e.Width = 600;
                //    e.Height = 600;
                //    e.Handled = true;
                //}
                else
                if (View != null && e.PopupFrame.View.Id == "SourceOpportunity_DetailView")
                {
                    e.Width = 600;
                    e.Height = 200;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "PaymentMethod_DetailView")
                {
                    e.Width = 600;
                    e.Height = 200;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Attachment_DetailView")
                {
                    e.Width = 700;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Project_DetailView")
                {
                    e.Width = 1000;
                    e.Height = 300;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Notes_DetailView_Client_CallLog_Popup")
                {
                    e.Width = 1000;
                    e.Height = 400;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Notes_DetailView_Prospect_CallLog_Popup")
                {
                    e.Width = 885;
                    e.Height = 400;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "PrepMethod_DetailView")
                {
                    e.Width = 900;
                    e.Height = 600;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "SubOutContractLab_LookupListView")
                {
                    e.Width = 1057;
                    e.Height = 400;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "SampleConditionCheckComment_DetailView")
                {
                    e.Width = 600;
                    e.Height = 320;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Topic_DetailView_prospect")
                {
                    e.Width = 600;
                    e.Height = 320;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Region_DetailView_popup")
                {
                    e.Width = 600;
                    e.Height = 320;
                    e.Handled = true;
                }
                else
                if (View != null && e.PopupFrame.View.Id == "Category_DetailView_popup")
                {
                    e.Width = 600;
                    e.Height = 320;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "TrendAnalysis_ListView_Chart")
                {
                    e.Width = 1200;
                    e.Height = 600;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "DailyQCChart_DetailView")
                {
                    e.Width = 1300;
                    e.Height = 600;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "Labware_ListView_DailyQC")
                {
                    e.Width = 600;
                    e.Height = 650;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "EDDCategory_DetailView_popup")
                {
                    e.Width = 500;
                    e.Height = 320;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "EDDQueryBuilder_LookupListView")
                {
                    e.Width = 700;
                    e.Height = 600;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "EDDBuilder_DetailView")
                {
                    e.Width = 1000;
                    e.Height = 500;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "EDDQueryBuilder_DetailView_SheetEDD")
                {
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    if (!string.IsNullOrEmpty(strscreenwidth))
                    {
                        e.Width = Convert.ToInt32(strscreenwidth) - (Convert.ToInt32(strscreenwidth) * 40 / 100); 
                    }
                    string strscreenheight = System.Web.HttpContext.Current.Request.Cookies.Get("screenheight").Value;
                    if (!string.IsNullOrEmpty(strscreenheight))
                    {
                        e.Height = Convert.ToInt32(strscreenheight) - (Convert.ToInt32(strscreenheight) * 10 / 100);
                    }
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "TurnAroundTime_ListView_Quotes")
                {
                    e.Width = 700;
                    e.Height = 600;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "Priority_ListView_Invoice")
                {
                    e.Width = 700;
                    e.Height = 600;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "LabwareCertificate_DetailView")
                {
                    e.Width = 1105;
                    e.Height = 300;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "MetrcIncomingDetData_ListView")
                {
                    e.Width = 1200;
                    e.Height = 400;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "TestPriceSurcharge_ListView_Invoice")
                {
                    e.Width = 700;
                    e.Height = 500;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "TrendAnalysis_DetailView_Charts")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(70);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(66);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "Sampling_ListView_SourceSample")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(50);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(75);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "Sampling_LookupListView_CopyTest_SampleID")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(50);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(75);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "DOC_DetailView_Copy_DV")
                {
                    e.Width = 795;
                    e.Height = 600;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "SampleParameter_ListView_Copy_DOC")
                {
                    e.Width = 1428;
                    e.Height = 557;
                    e.Handled = true;
                }
                else if (View != null && (e.PopupFrame.View.Id == "DOC_DetailView_Deleting_Reason"||e.PopupFrame.View.Id== "DOC_DetailView_RollBack_Reason"))
                {
                    e.Width = 600;
                    e.Height = 394;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "TaskRecurranceSetup_DetailView")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(50);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(65);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "Collector_DetailView_Sampling")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(60);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(75);
                    e.Handled = true;
                }
                else if (View != null && View.Id == "DWQRReportTemplateSetup_DetailView_DateRange")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(25);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(40);
                    e.Handled = true;
                }
                else if (View != null && View.Id == "SampleSites_LookupListView_DWQR")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(70);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(98);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "SampleSites_LookupListView_Sampling" || e.PopupFrame.View.Id== "SampleSites_LookupListView_Sampling_StationLocation" || e.PopupFrame.View.Id == "SampleSites_LookupListView_CocSetting")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(70);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(95);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View != null && e.PopupFrame.View.Id == "Reporting_DetailView_Revision")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(35);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(30);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View != null && e.PopupFrame.View.Id == "NpSampleSourceSetup_DetailView")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(30);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(30);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View != null && e.PopupFrame.View.Id == "ReagentPrepClassify")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(45);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(72);
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "SampleBottleAllocation_DetailView_STPassword")
                {
                    e.Width = 650;
                    e.Height = 300;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "DefinitionCategory_DetailView")
                {
                    e.Width = 500;
                    e.Height = 230;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "SDMSDCSpreadsheet_DetailView_EDDReportGenerator")
                {
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    if (!string.IsNullOrEmpty(strscreenwidth))
                    {
                        e.Width = Convert.ToInt32(strscreenwidth) - (Convert.ToInt32(strscreenwidth) * 20 / 100);
                    }
                    string strscreenheight = System.Web.HttpContext.Current.Request.Cookies.Get("screenheight").Value;
                    if (!string.IsNullOrEmpty(strscreenheight))
                    {
                        e.Height = Convert.ToInt32(strscreenheight) - (Convert.ToInt32(strscreenheight) * 40 / 100);
                    }
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "EDDReportGenerator_DetailView_popup")
                {
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    if (!string.IsNullOrEmpty(strscreenwidth))
                    {
                        e.Width = Convert.ToInt32(strscreenwidth) - (Convert.ToInt32(strscreenwidth) * 50 / 100);
                    }
                    string strscreenheight = System.Web.HttpContext.Current.Request.Cookies.Get("screenheight").Value;
                    if (!string.IsNullOrEmpty(strscreenheight))
                    {
                        e.Height = Convert.ToInt32(strscreenheight) - (Convert.ToInt32(strscreenheight) * 50 / 100);
                    }
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "PLMCopyToCombo_DetailView")
                {
                    e.Width = 600;
                    e.Height = 450;
                    e.Handled = true;
                }
                else if (View != null && e.PopupFrame.View.Id == "PLM")
                {
                    e.Width = System.Web.UI.WebControls.Unit.Percentage(85);
                    e.Height = System.Web.UI.WebControls.Unit.Percentage(85);
                    e.Handled = true;
                }
                //if (Frame.GetType() == typeof(DevExpress.ExpressApp.Web.PopupWindow))
                //{                //    if (View is DetailView)
                //    {
                //        foreach (ViewItem item in ((DetailView)View).Items)
                //        {
                //            itemcnt++;
                //        }
                //        IsPopup = true;
                //    }
                //}
                //if (itemcnt < 2)
                //{
                //    e.Width = 800;
                //    e.Height = 200;
                //}
                //else if (itemcnt < 6)
                //{
                //    e.Width = 800;
                //    e.Height = 300;
                //}
                //e.Handled = true;
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
            IsPopup = false;
        }
    }
}
