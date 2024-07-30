using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using Modules.Controllers.Hr;
using System;
using System.Collections.Generic;

namespace Modules
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class ModulesModule : ModuleBase
    {
        public ModulesModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            application.CreateCustomLogonWindowControllers += application_CreateCustomLogonWindowControllers;

        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }

        private void application_CreateCustomLogonWindowControllers(object sender, CreateCustomLogonWindowControllersEventArgs e)
        {
            e.Controllers.Add(((XafApplication)sender).CreateController<CustomLogOnViewController>());
            // e.Controllers.Add(((XafApplication)sender).CreateController<UserLinkRoleController>());

        }
    }
}
