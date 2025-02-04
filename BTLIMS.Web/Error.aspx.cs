﻿using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.TestScripts;
using System;
using System.Web.UI;

public partial class ErrorPage : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        if (WebApplication.Instance != null)
            WebApplication.Instance.InitializeCulture();
    }
    private void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            TestScriptsManager testScriptsManager = new TestScriptsManager(Page);
            testScriptsManager.RegisterControl(JSLabelTestControl.ClassName, "FormCaption", TestControlType.Field, "FormCaption");
            testScriptsManager.RegisterControl(JSLabelTestControl.ClassName, "DescriptionTextBox", TestControlType.Field, "Description");
            testScriptsManager.RegisterControl(JSDefaultTestControl.ClassName, "ReportButton", TestControlType.Action, "Report");
            testScriptsManager.AllControlRegistered();
            ClientScript.RegisterStartupScript(GetType(), "EasyTest", testScriptsManager.GetScript(), true);

            if (WebApplication.Instance != null)
            {
                ApplicationTitle.Text = WebApplication.Instance.Title;
            }
            else
            {
                ApplicationTitle.Text = "No application";
            }
            Header.Title = "Application Error - " + ApplicationTitle.Text;

            ErrorInfo errorInfo = ErrorHandling.GetApplicationError();
            if (errorInfo != null)
            {
                if (ErrorHandling.CanShowDetailedInformation)
                {
                    DetailsText.Text = errorInfo.GetTextualPresentation(true);
                }
                else
                {
                    Details.Visible = false;
                }
                ReportResult.Visible = false;
                ReportForm.Visible = ErrorHandling.CanSendAlertToAdmin;
            }
            else
            {
                ErrorPanel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        InitializeComponent();
        base.OnInit(e);
    }

    private void InitializeComponent()
    {
        this.Load += new System.EventHandler(this.Page_Load);
        this.PreRender += new EventHandler(ErrorPage_PreRender);
    }

    private void ErrorPage_PreRender(object sender, EventArgs e)
    {
        try
        {
            RegisterThemeAssemblyController.RegisterThemeResources((Page)sender);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    #endregion
    protected void ReportButton_Click(object sender, EventArgs e)
    {
        try
        {
            ErrorInfo errorInfo = ErrorHandling.GetApplicationError();
            if (errorInfo != null)
            {
                ErrorHandling.SendAlertToAdmin(errorInfo.Id, DescriptionTextBox.Text, errorInfo.Exception.Message);
                ErrorHandling.ClearApplicationError();
                ApologizeMessage.Visible = false;
                ReportForm.Visible = false;
                Details.Visible = false;
                ReportResult.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void NavigateToStart_Click(object sender, EventArgs e)
    {
        WebApplication.Instance.LogOff();
    }
}
