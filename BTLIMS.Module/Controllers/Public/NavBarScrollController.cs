using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;

namespace LDM.Module.Controllers.Public
{
    public partial class NavBarScrollController : WindowController
    {
        MessageTimer timer = new MessageTimer();

        public NavBarScrollController()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.Execute += ShowNavigationItemAction_Execute;
                Frame.GetController<ShowNavigationItemController>().CustomShowNavigationItem += new EventHandler<CustomShowNavigationItemEventArgs>(ViewController1_CustomShowNavigationItem);
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        void ViewController1_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            try
            {
                if (e.ActionArguments.SelectedChoiceActionItem.Model.GetType().Name == "ModelNavigationItem")
                {
                    IModelListView model = ((IModelNavigationItem)e.ActionArguments.SelectedChoiceActionItem.Model).View as IModelListView;
                    if (model != null && model.Id == "DefaultSetting_ListView")
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        DefaultSetting defaultsetting = objectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse(""));
                        e.ActionArguments.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, "DefaultSetting_DetailView", true, defaultsetting);
                        e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
                        e.Handled = true;
                    }
                    if (model != null && model.Id == "DefaultSetting_ListView_DateFilter")
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        //DefaultSetting objectToShow = objectSpace.CreateObject<DefaultSetting>();
                        //XPCollection<DefaultSetting> xpc = new XPCollection<DefaultSetting>(objectSpace
                        DefaultSetting defaultsetting = objectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse(""));
                        e.ActionArguments.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, "DefaultSetting_DetailView_Copy", true, defaultsetting);
                        e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
                        e.Handled = true;
                    }
                    if (model != null && model.Id == "TabFieldConfiguration_ListView")
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        //DefaultSetting objectToShow = objectSpace.CreateObject<DefaultSetting>();
                        //XPCollection<DefaultSetting> xpc = new XPCollection<DefaultSetting>(objectSpace
                        //TabFieldConfiguration dvtabconfig = objectSpace.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse(""));
                        TabFieldConfiguration dvtabconfig = objectSpace.CreateObject<TabFieldConfiguration>();
                        e.ActionArguments.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, "TabFieldConfiguration_DetailView_TestTab", true, dvtabconfig);
                        e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
                        e.Handled = true;
                    }
                    if (model != null && model.Id == "ResultEntryQueryPanel_ListView")
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        ResultEntryQueryPanel dvrstentry = objectSpace.CreateObject<ResultEntryQueryPanel>();
                        e.ActionArguments.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, "ResultEntryQueryPanel_DetailView_Copy", true, dvrstentry);
                        e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        void ShowNavigationItemAction_Execute(object sender, DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                WebWindow.CurrentRequestWindow.RegisterStartupScript("scrollTop", "window.scrollTo(0, 0);", true);

            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.Execute -= ShowNavigationItemAction_Execute;
            Frame.GetController<ShowNavigationItemController>().CustomShowNavigationItem -= new EventHandler<CustomShowNavigationItemEventArgs>(ViewController1_CustomShowNavigationItem);
        }
    }
}
