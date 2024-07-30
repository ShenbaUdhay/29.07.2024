using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Web
{
    public partial class CustomError : System.Web.UI.Page
    {
        CustomLoginInfo obj = new CustomLoginInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Application["Error"] != null)
            {
                Errortext.InnerText = Application["Error"].ToString();
                Application["Error"] = string.Empty;
            }
            else if (!string.IsNullOrEmpty(obj.strMessage))
            {
                Errortext.InnerText = obj.strMessage;
                obj.strMessage = string.Empty;
            }
        }
    }
}