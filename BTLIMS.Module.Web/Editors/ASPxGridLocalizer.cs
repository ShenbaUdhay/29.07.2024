using DevExpress.ExpressApp.Web.Localization;
using DevExpress.Web.Localization;

namespace BTLIMS.Module.Web.Editors
{
    public class ASPxGridLocalizer : ASPxGridViewResourceLocalizer
    {
        public override string GetLocalizedString(ASPxGridViewStringId id)
        {
            switch (id)
            {
                case ASPxGridViewStringId.EmptyDataRow:
                    return "没有数据显示";
                case ASPxGridViewStringId.Summary_Average:
                    return "平均数：{0}";
                case ASPxGridViewStringId.Summary_Count:
                    return "计数：{0}";
                case ASPxGridViewStringId.Summary_Max:
                    return "最大值：{0}";
                case ASPxGridViewStringId.Summary_Min:
                    return "最小值：{0}";
                case ASPxGridViewStringId.Summary_Sum:
                    return "合计：{0}";
            }
            return base.GetLocalizedString(id);
        }
    }
}
