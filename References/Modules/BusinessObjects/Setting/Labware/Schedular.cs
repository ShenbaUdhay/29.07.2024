using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace Modules.BusinessObjects.Setting.Labware
{
    [DefaultClassOptions]
    [NonPersistent]
    [DomainComponent]
    public class Schedular : Event
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Schedular(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            //if (MaintenanceSetup!=null&& MaintenanceSetup.InstrumentID!=null)
            //{
            //    Subject = MaintenanceSetup.InstrumentID.LabwareName; 
            //}
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            //if (MaintenanceSetup != null)
            //{
            //    Subject = MaintenanceSetup.InstrumentID.LabwareName;
            //}
        }
    }
}