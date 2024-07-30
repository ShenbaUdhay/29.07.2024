using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Web;
using System;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors
{
    public interface IModelProgressViewItem : IModelViewItem
    {
    }

    [ViewItemAttribute(typeof(IModelProgressViewItem))]
    public class ProgressBarViewItem : ViewItem
    {
        public ProgressBarViewItem(IModelProgressViewItem info, Type classType)
            : base(classType, info.Id)
        {
        }

        protected override object CreateControlCore()
        {
            ASPxProgressBar bar = new ASPxProgressBar();
            bar.ClientInstanceName = "myprogresscontrol";
            bar.Width = Unit.Percentage(100);
            bar.ClientSideEvents.Init =
@"function(s,e) { 
        if(window.timer) { 
            window.clearInterval(window.timer);
        }
        var controlToUpdate = s;
        window.timer = window.setInterval(function(){
        LDM.Web.WebService1.GetLongOperationStatus(function(result) { 
            if (controlToUpdate){
                controlToUpdate.SetPosition(parseInt(result));
            }
        });
    },1000
    ); 
}";
            return bar;
        }

    }
}
