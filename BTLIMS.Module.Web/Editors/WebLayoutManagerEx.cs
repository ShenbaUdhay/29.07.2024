﻿using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace LDM.Module.Web.Editors
{
    public enum TextAlignModeGroup
    {
        UseParentOptions = 0,
        AlignLocal = 1,
        AutoSize = 2,
        //CustomSize = 3,
        AlignWithChildren = 4
    }
    public enum TextAlignModeItem
    {
        UseParentOptions = 0,
        AlignLocal = 1,
        AutoSize = 2,
        //CustomSize = 3,
        AlignWithChildren = 4
    }
    public interface IModelWebLayoutGroup
    {
        [Category("Behavior"), DefaultValue(TextAlignModeGroup.UseParentOptions)]
        TextAlignModeGroup TextAlignMode { get; set; }
    }
    public interface IModelWebLayoutItem
    {
        [Category("Behavior"), DefaultValue(TextAlignModeItem.UseParentOptions)]
        TextAlignModeItem TextAlignMode { get; set; }
    }
    public class WebLayoutManagerEx : WebLayoutManager
    {
        public WebLayoutManagerEx(bool simple, bool delayedItemsInitialization) : base(simple, delayedItemsInitialization) { }
        protected override LayoutItemTemplateContainer LayoutItem(ViewItemsCollection viewItems, IModelLayoutViewItem layoutItemModel)
        {
            LayoutItemTemplateContainer templateContainer = new LayoutItemTemplateContainer(this, viewItems, layoutItemModel);
            templateContainer.Template = LayoutItemTemplate;
            templateContainer.ID = WebIdHelper.GetCorrectedLayoutItemId(layoutItemModel);
            ViewItem viewItem = FindViewItem(viewItems, layoutItemModel);
            templateContainer.ViewItem = viewItem;
            if (viewItem != null)
            {
                MarkRequiredFieldCaptionEventArgs args = new MarkRequiredFieldCaptionEventArgs(viewItem, false);
                OnMarkRequiredFieldCaption(args);
                templateContainer.Caption = BuildItemCaption(viewItem, args.NeedMarkRequiredField, args.RequiredFieldMark);
            }
            templateContainer.ShowCaption = GetIsLayoutItemCaptionVisible(layoutItemModel, viewItem);
            templateContainer.CaptionWidth = CalculateCaptionWidth(viewItem, viewItems, layoutItemModel);
            templateContainer.CaptionLocation = GetCaptionLocation(layoutItemModel);
            templateContainer.CaptionHorizontalAlignment = GetCaptionHorizontalAlignment(layoutItemModel);
            templateContainer.CaptionVerticalAlignment = GetCaptionVerticalAlignment(layoutItemModel);
            OnLayoutItemCreatedNew(templateContainer, layoutItemModel, viewItem);
            return templateContainer;
        }
        private void OnLayoutItemCreatedNew(LayoutItemTemplateContainerBase templateContainer, IModelViewLayoutElement layoutItemModel, ViewItem viewItem)
        {
            if (!DelayedItemsInitialization)
            {
                templateContainer.Instantiate();
            }
            OnLayoutItemCreated(new ItemCreatedEventArgs(layoutItemModel, viewItem, templateContainer));
            OnCustomizeAppearance(new CustomizeAppearanceEventArgs(layoutItemModel.Id, new WebLayoutItemAppearanceAdapter(templateContainer), null));
        }
        private DevExpress.Utils.Locations GetCaptionLocation(IModelLayoutViewItem layoutItemModel)
        {
            Locations captionLocation = layoutItemModel.CaptionLocation;
            return Equals(captionLocation, Locations.Default) ? DefaultLayoutItemCaptionLocation : (DevExpress.Utils.Locations)captionLocation;
        }
        private DevExpress.Utils.HorzAlignment GetCaptionHorizontalAlignment(IModelLayoutViewItem layoutItemModel)
        {
            DevExpress.Utils.HorzAlignment captionHorizontalAlignment = layoutItemModel.CaptionHorizontalAlignment;
            return Equals(captionHorizontalAlignment, DevExpress.Utils.HorzAlignment.Default) ? DefaultLayoutItemCaptionHorizontalAlignment : captionHorizontalAlignment;
        }
        private DevExpress.Utils.VertAlignment GetCaptionVerticalAlignment(IModelLayoutViewItem layoutItemModel)
        {
            DevExpress.Utils.VertAlignment captionVerticalAlignment = layoutItemModel.CaptionVerticalAlignment;
            return Equals(captionVerticalAlignment, DevExpress.Utils.VertAlignment.Default) ? DefaultLayoutItemCaptionVerticalAlignment : captionVerticalAlignment;
        }
        private static ViewItem FindViewItem(ViewItemsCollection viewItems, IModelLayoutViewItem layoutItemModel)
        {
            IModelViewItem viewItem = layoutItemModel.ViewItem;
            string viewItemId = viewItem != null ? viewItem.Id : layoutItemModel.Id;
            return viewItems[viewItemId];
        }
        private System.Web.UI.WebControls.Unit CalculateCaptionWidth(ViewItem viewItem, ViewItemsCollection viewItems, IModelLayoutViewItem layoutItemModel)
        {
            var item = layoutItemModel as IModelWebLayoutItem;
            if (item != null)
            {
                if (item.TextAlignMode == TextAlignModeItem.AutoSize)
                {
                    if (viewItem.View.Id == "SamplePrepBatch_DetailView_Copy")
                    {
                        ViewItem viPrepBatch = viewItems["PrepBatchID"];
                        System.Web.UI.WebControls.Unit prepBatchIDUnit = this.GetMaxStringWidth(new string[] { this.EnsureCaptionColon(viewItem.Caption) });
                        return new System.Web.UI.WebControls.Unit(prepBatchIDUnit.Value + 250, prepBatchIDUnit.Type);
                    }
                    else
                    {
                        return this.GetMaxStringWidth(new string[] { this.EnsureCaptionColon(viewItem.Caption) });
                    }
                }
                else
                {
                    IModelViewLayoutElement current = layoutItemModel;
                    while (current != null)
                    {
                        var group = current.Parent as IModelWebLayoutGroup;
                        if (group != null)
                        {
                            if (group.TextAlignMode == TextAlignModeGroup.AutoSize)
                            {
                                System.Web.UI.WebControls.Unit unit = this.GetMaxStringWidth(new string[] { this.EnsureCaptionColon(viewItem.Caption) });
                                if (viewItem.View.Id == "SamplePrepBatch_DetailView_Copy")
                                {
                                    ViewItem viPrepBatch = viewItems["PrepBatchID"];
                                    System.Web.UI.WebControls.Unit prepBatchIDUnit = this.GetMaxStringWidth(new string[] { this.EnsureCaptionColon(viewItem.Caption) });
                                    return new System.Web.UI.WebControls.Unit(prepBatchIDUnit.Value + 250, prepBatchIDUnit.Type);
                                }
                                else
                                {
                                    return unit;
                                }
                            }
                            if (group.TextAlignMode == TextAlignModeGroup.AlignLocal)
                            {
                                if (viewItem.View.Id == "SamplePrepBatch_DetailView_Copy")
                                {
                                    ViewItem viPrepBatch = viewItems["PrepBatchID"];
                                    System.Web.UI.WebControls.Unit prepBatchIDUnit = this.GetMaxStringWidth(new string[] { this.EnsureCaptionColon(viewItem.Caption) });
                                    return new System.Web.UI.WebControls.Unit(prepBatchIDUnit.Value + 250, prepBatchIDUnit.Type);
                                }
                                else
                                {
                                    return CalculateLayoutItemCaptionWidthNew((IModelLayoutGroup)group, viewItems, false);
                                }
                            }
                            if (group.TextAlignMode == TextAlignModeGroup.AlignWithChildren)
                            {
                                if (viewItem.View.Id == "SamplePrepBatch_DetailView_Copy")
                                {
                                    ViewItem viPrepBatch = viewItems["PrepBatchID"];
                                    System.Web.UI.WebControls.Unit prepBatchIDUnit = this.GetMaxStringWidth(new string[] { this.EnsureCaptionColon(viewItem.Caption) });
                                    return new System.Web.UI.WebControls.Unit(prepBatchIDUnit.Value + 250, prepBatchIDUnit.Type);
                                }
                                else
                                {
                                    return CalculateLayoutItemCaptionWidthNew((IModelLayoutGroup)group, viewItems, true);
                                }
                                return CalculateLayoutItemCaptionWidthNew((IModelLayoutGroup)group, viewItems, true);
                            }
                        }
                        current = current.Parent as IModelViewLayoutElement;
                    }
                }
            }
            return this.LayoutItemCaptionWidth;
        }
        private System.Web.UI.WebControls.Unit CalculateLayoutItemCaptionWidthNew(IEnumerable<IModelViewLayoutElement> layoutInfo, ViewItemsCollection viewItems, bool recursively)
        {
            List<string> list = new List<string>();
            CollectLayoutItemVisibleCaptions<IModelViewLayoutElement>(list, layoutInfo, viewItems, recursively);
            return this.GetMaxStringWidth(list);
        }
        private void CollectLayoutItemVisibleCaptions<T>(IList<string> captions, IEnumerable<T> layoutInfo, ViewItemsCollection viewItems, bool recursively)
        {
            foreach (T itemInfo in layoutInfo)
            {
                if (itemInfo is IModelLayoutViewItem)
                {
                    IModelLayoutViewItem layoutItemModel = (IModelLayoutViewItem)itemInfo;
                    ViewItem viewItem = FindViewItem(viewItems, layoutItemModel);
                    if (viewItem != null && GetIsLayoutItemCaptionVisible(layoutItemModel, viewItem) && GetIsItemForCaptionCalculation(layoutItemModel, viewItem))
                    {
                        MarkRequiredFieldCaptionEventArgs args = new MarkRequiredFieldCaptionEventArgs(viewItem, false);
                        OnMarkRequiredFieldCaption(args);
                        captions.Add(BuildItemCaption(viewItem, args.NeedMarkRequiredField, args.RequiredFieldMark));
                    }
                }
                else if (recursively)
                {
                    if (itemInfo is IEnumerable<IModelViewLayoutElement>)
                    {
                        CollectLayoutItemVisibleCaptions<IModelViewLayoutElement>(captions, (IEnumerable<IModelViewLayoutElement>)itemInfo, viewItems, recursively);
                    }
                    else if (itemInfo is IEnumerable<IModelLayoutGroup>)
                    {
                        CollectLayoutItemVisibleCaptions<IModelLayoutGroup>(captions, (IEnumerable<IModelLayoutGroup>)itemInfo, viewItems, recursively);
                    }
                }
            }
        }

    }
}
