using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AuditlogsummaryViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public AuditlogsummaryViewController()
        {
            InitializeComponent();
            TargetViewId = "AuditData_ListView_Sampleregistration;"
                + "AuditData_ListView_Test;"
                + "AuditData_ListView_SampleSites;"
                + "AuditData_ListView_ReportTracking;"
                + "AuditData_ListView_SampleTransfer;"
                + "AuditData_ListView_FieldDataEntry;";
                //+ "AuditData_DetailView;";
            STFilter.TargetViewId = "AuditData_ListView_Sampleregistration;"
                + "AuditData_ListView_Test;"
                + "AuditData_ListView_SampleSites;"
                + "AuditData_ListView_ReportTracking;"
                + "AuditData_ListView_SampleTransfer;"
                + "AuditData_ListView_FieldDataEntry;";
            //SimpleAction BtnFilter = new SimpleAction(this, "btnFilter", PredefinedCategory.Unspecified)
            //{
            //    Caption = "Data Filter"
            //};
            //BtnFilter.TargetViewId = "AuditData_ListView_Sampleregistration;";
            //BtnFilter.Execute += BtnFilter_Execute;
        }

        //private void BtnFilter_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //    AuditData objAudit = (AuditData)objectSpace.CreateObject(typeof(AuditData));
        //    DetailView detailtview = Application.CreateDetailView(objectSpace, "AuditData_DetailView_SampleRegistration", true, objAudit);
        //    detailtview.Caption = "Data Filter";
        //    ShowViewParameters showViewParameters = new ShowViewParameters(detailtview);
        //    showViewParameters.Context = TemplateContext.PopupWindow;
        //    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
        //    DialogController dc = Application.CreateController<DialogController>();
        //    dc.Accepting += Dc_Accepting;
        //    dc.AcceptAction.Active.SetItemValue("disable", true);
        //    dc.CancelAction.Active.SetItemValue("disable", true);
        //    showViewParameters.Controllers.Add(dc);
        //    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
        //}

        //private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        protected override void OnActivated()
        {
            base.OnActivated();            
            if (View.Id == "AuditData_ListView_Sampleregistration")
            {
                if (STFilter != null && STFilter.SelectedItem == null && View.Id == "AuditData_ListView_Sampleregistration")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (STFilter.SelectedItem == null)
                    {
                        if (setting.SampleTransfer == EnumDateFilter.OneMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[0];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.ThreeMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[1];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.SixMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[2];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.OneYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[3];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.TwoYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[4];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.FiveYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[5];
                        }
                        else
                        {
                            STFilter.SelectedItem = STFilter.Items[6];
                        }
                    }
                    STFilter.SelectedItemChanged += STFilter_SelectedItemChanged;
                }
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[FormName] = 'Sample Registration' OR [FormName] = 'SampleBottleAllocation'");
            }
            else if (View.Id == "AuditData_ListView_FieldDataEntry")
            {
                if (STFilter != null && STFilter.SelectedItem == null && View.Id == "AuditData_ListView_FieldDataEntry")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (STFilter.SelectedItem == null)
                    {
                        if (setting.SampleTransfer == EnumDateFilter.OneMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[0];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.ThreeMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[1];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.SixMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[2];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.OneYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[3];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.TwoYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[4];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.FiveYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[5];
                        }
                        else
                        {
                            STFilter.SelectedItem = STFilter.Items[6];
                        }
                    }
                    STFilter.SelectedItemChanged += STFilter_SelectedItemChanged;
                }
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[FormName] = 'Field Data Entry' OR [FormName] = 'Sample Registration'");
            }
            else if (View.Id == "AuditData_ListView_Test")
            {
                if (STFilter != null && STFilter.SelectedItem == null && View.Id == "AuditData_ListView_Test")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (STFilter.SelectedItem == null)
                    {
                        if (setting.SampleTransfer == EnumDateFilter.OneMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[0];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.ThreeMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[1];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.SixMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[2];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.OneYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[3];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.TwoYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[4];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.FiveYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[5];
                        }
                        else
                        {
                            STFilter.SelectedItem = STFilter.Items[6];
                        }
                    }
                    STFilter.SelectedItemChanged += STFilter_SelectedItemChanged;
                }
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[FormName] = 'Tests' OR [FormName] = 'TestGuide'");
            }
            else if (View.Id == "AuditData_ListView_SampleTransfer")
            {
                if (STFilter != null && STFilter.SelectedItem == null && View.Id == "AuditData_ListView_SampleTransfer")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (STFilter.SelectedItem == null)
                    {
                        if (setting.SampleTransfer == EnumDateFilter.OneMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[0];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.ThreeMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[1];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.SixMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[2];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.OneYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[3];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.TwoYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[4];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.FiveYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[5];
                        }
                        else
                        {
                            STFilter.SelectedItem = STFilter.Items[6];
                        }
                    }
                    STFilter.SelectedItemChanged += STFilter_SelectedItemChanged;
                }
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[FormName] = 'Sample Transfer' OR [FormName] = 'Sample Registration' OR " +
                    "[FormName] = 'SampleBottleAllocation'");
            }
            else if (View.Id == "AuditData_ListView_ReportTracking")
            {
                if (STFilter != null && STFilter.SelectedItem == null && View.Id == "AuditData_ListView_ReportTracking")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (STFilter.SelectedItem == null)
                    {
                        if (setting.SampleTransfer == EnumDateFilter.OneMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[0];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.ThreeMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[1];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.SixMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[2];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.OneYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[3];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.TwoYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[4];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.FiveYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[5];
                        }
                        else
                        {
                            STFilter.SelectedItem = STFilter.Items[6];
                        }
                    }
                    STFilter.SelectedItemChanged += STFilter_SelectedItemChanged;
                }
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[FormName] = 'Report Tracking'");
            }
            else if (View.Id == "AuditData_ListView_SampleSites")
            {
                if (STFilter != null && STFilter.SelectedItem == null && View.Id == "AuditData_ListView_SampleSites")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (STFilter.SelectedItem == null)
                    {
                        if (setting.SampleTransfer == EnumDateFilter.OneMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[0];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.ThreeMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[1];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.SixMonth)
                        {
                            STFilter.SelectedItem = STFilter.Items[2];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.OneYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[3];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.TwoYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[4];
                        }
                        else if (setting.SampleTransfer == EnumDateFilter.FiveYear)
                        {
                            STFilter.SelectedItem = STFilter.Items[5];
                        }
                        else
                        {
                            STFilter.SelectedItem = STFilter.Items[6];
                        }
                    }
                    STFilter.SelectedItemChanged += STFilter_SelectedItemChanged;
                }
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[FormName] = 'Sample Transfer' OR [FormName] = 'Sample Registration' OR " +
                    "[FormName] = 'SampleBottleAllocation' OR [FormName] = 'Tests' OR [FormName] = 'TestGuide' OR [FormName] = 'Sample Parameter' OR [FormName] = 'PrepMethod' OR " +
                    "[FormName] = 'Report Tracking' OR [FormName] = 'Notes' OR [FormName] = 'Field Data Entry'");
            }
        }
        private void STFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View is ListView)
                {
                    DateTime srDateFilter = DateTime.MinValue;
                    if (STFilter != null && STFilter.SelectedItem != null)
                    {
                        if (STFilter.SelectedItem.Id == "1M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-1);
                        }
                        else if (STFilter.SelectedItem.Id == "3M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-3);
                        }
                        else if (STFilter.SelectedItem.Id == "6M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-6);
                        }
                        else if (STFilter.SelectedItem.Id == "1Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-1);
                        }
                        else if (STFilter.SelectedItem.Id == "2Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-2);
                        }
                        else if (STFilter.SelectedItem.Id == "5Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-5);
                        }
                    }
                    if (srDateFilter != DateTime.MinValue)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("CreatedDate >= ?", srDateFilter);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria.Remove("Filter");
                    }
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
