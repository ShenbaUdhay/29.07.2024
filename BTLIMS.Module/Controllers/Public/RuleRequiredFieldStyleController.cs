using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Layout;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Drawing;
using System.Web.UI.WebControls;

namespace LDM.Module.Controllers.Public
{
    public partial class RuleRequiredFieldStyleController : ViewController<DetailView>
    {
        MessageTimer timer = new MessageTimer();
        public RuleRequiredFieldStyleController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View != null && View.LayoutManager != null && View.LayoutManager is WebLayoutManager)
                {
                    if (View.ObjectTypeInfo.Type != typeof(Samplecheckin))
                    {
                        ((WebLayoutManager)View.LayoutManager).ItemCreated += ViewController1_ItemCreated;
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

        private void ViewController1_ItemCreated(object sender, ItemCreatedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject != null && e.TemplateContainer != null && e.TemplateContainer is LayoutItemTemplateContainer)
                {
                    string strCaption = ((LayoutItemTemplateContainer)e.TemplateContainer).Caption;
                    if (!string.IsNullOrEmpty(strCaption) && strCaption.Length > 0)
                    {
                        if (View.Id == "SampleBottleAllocation_DetailView_SampleTransfer" && strCaption != "Brought By:")
                        {
                            ((LayoutItemTemplateContainer)e.TemplateContainer).Caption = strCaption.Replace("*", "");
                        }
                        else if ((View.Id != "SampleBottleAllocation_DetailView_SampleTransfer" && strCaption.Substring(strCaption.Length - 1) == "*") || (View.Id == "SampleBottleAllocation_DetailView_SampleTransfer" && strCaption == "Brought By:"))
                        {
                            if (View.Id == "SampleBottleAllocation_DetailView_SampleTransfer" && strCaption == "Brought By:")
                            {
                                ((LayoutItemTemplateContainer)e.TemplateContainer).Caption = ((LayoutItemTemplateContainer)e.TemplateContainer).Caption + "*";
                            }
                            if (e.TemplateContainer.CaptionControl != null)
                            {
                                CustomizeCaptionControl(e.TemplateContainer.CaptionControl);
                            }
                            else
                            {
                                e.TemplateContainer.Load += TemplateContainer_Load;
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

        private void TemplateContainer_Load(object sender, EventArgs e)
        {
            try
            {
                if (sender != null && sender is LayoutItemTemplateContainerBase)
                {
                    LayoutItemTemplateContainerBase templateControler = (LayoutItemTemplateContainerBase)sender;
                    if (templateControler != null)
                    {
                        templateControler.Load -= TemplateContainer_Load;
                        if (templateControler != null && templateControler.CaptionControl != null)
                        {
                            CustomizeCaptionControl(templateControler.CaptionControl);
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

        private void CustomizeCaptionControl(WebControl captionControl)
        {
            try
            {
                if (captionControl != null)
                {
                    captionControl.ForeColor = Color.Red;
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
            try
            {
                if (View != null && View.LayoutManager != null && View.LayoutManager is WebLayoutManager)
                {
                    ((WebLayoutManager)View.LayoutManager).ItemCreated -= ViewController1_ItemCreated;
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
