/* ================================================================================
// Table Name: [MessageBoxUtil]
// Author: Sunny
// Date: 2017年01月17日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：信息提示类
// ================================================================================*/
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace Modules.BusinessObjects.Libraries
{
    public class MessageBoxUtil
    {
        //SimpleAction点击后弹出对话框。
        public static void PopupMessageBox(XafApplication application, ShowViewParameters svp, string msg)
        {
            var objSpaceInMem = ObjectSpaceInMemory.CreateNew();

            svp.CreatedView = application.CreateDetailView(objSpaceInMem, new TextMessage(msg));
            svp.TargetWindow = TargetWindow.NewModalWindow; svp.NewWindowTarget = NewWindowTarget.MdiChild;
            svp.CreatedView.IsRoot = false;

            var dc = application.CreateController<DialogController>();
            dc.CancelAction.Active.SetItemValue("对话框无需显示CancelAction", false);
            dc.SaveOnAccept = false;
            svp.Controllers.Clear();
            svp.Controllers.Add(dc);
        }
        /// <summary>
        /// 适用在PopupAction的CustomizePopupWindowParams事件弹出
        /// </summary>
        /// <param name="application">ObjectSpace</param>
        /// <param name="dialogController">e.DialogController</param>
        /// <param name="msg">要提示的信息</param>
        /// <returns>e.View=返回值</returns>
        public static DetailView PopupMessageBox(XafApplication application, DialogController dialogController, string msg)
        {
            dialogController.AcceptAction.Active.SetItemValue("提示", false);
            var objSpaceInMem = ObjectSpaceInMemory.CreateNew();
            return application.CreateDetailView(objSpaceInMem, new TextMessage(msg));
        }

    }
}
