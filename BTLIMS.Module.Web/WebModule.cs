using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Updating;
using LDM.Module.Web.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BTLIMS.Module.Web
{
    [ToolboxItemFilter("Xaf.Platform.Web")]
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class BTLIMSAspNetModule : ModuleBase
    {
        public BTLIMSAspNetModule()
        {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            return ModuleUpdater.EmptyModuleUpdaters;
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.

        }
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelLayoutGroup, IModelWebLayoutGroup>();
            extenders.Add<IModelLayoutViewItem, IModelWebLayoutItem>();
        }
    }
}
