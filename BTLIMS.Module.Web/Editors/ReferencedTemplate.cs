using ALPACpre.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.Web;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.ReagentPreparation;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Priority = Modules.BusinessObjects.Setting.Priority;

namespace LDM.Module.Web.Editors
{
    class ReferencedTemplate : ITemplate
    {
        AnalysisDeptUser analysisDeptUser = new AnalysisDeptUser();
        viewInfo tempviewinfo = new viewInfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        COCSettingsRegistrationInfo COCsr = new COCSettingsRegistrationInfo();
        QuotesInfo quoteinfo = new QuotesInfo();
        public bool isAdministrator = false;
        Employee currentUser = SecuritySystem.CurrentUser as Employee;
        string PropertyName = string.Empty;
        string ViewName = string.Empty;
        const string BatchEditKeyDown =
        @"function(s, e) {
            var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
            if (keyCode !== ASPx.Key.Tab && keyCode !== ASPx.Key.Enter) 
                return;
            var moveActionName = e.htmlEvent.shiftKey ? 'MoveFocusBackward' : 'MoveFocusForward';
            var clientGridView = s.grid;
            if (clientGridView.batchEditApi[moveActionName]()) {
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
                window.batchPreventEndEditOnLostFocus = true;
            }
        }";
        //Handle the editor's client-side event to emulate the behavior of standard ASPxClientEdit.LostFocus grid editor. 
        const string BatchEditLostFocus =
            @"function (s, e) {
            var clientGridView = s.grid;
            if (!window.batchPreventEndEditOnLostFocus)
                clientGridView.batchEditApi.EndEdit();
            window.batchPreventEndEditOnLostFocus = false;
        }";

        public IEnumerable<FiberTypes> FiberTypeObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.Unit> Objects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.Unit> UnitObjects { get; private set; }
        public IEnumerable<ICM.Module.BusinessObjects.Packageunits> PackageUnitObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.ICM.ICMStorage> ICMObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Hr.Employee> EMPObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.ICM.Items> ICMItems { get; private set; }
        public IEnumerable<ICM.Module.BusinessObjects.Vendors> VENObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.ICM.Manufacturer> BRANDObjects { get; private set; }
        public IEnumerable<ICM.Module.BusinessObjects.Shippingoptions> SHIPObjects { get; private set; }
        public IEnumerable<Priority> Priorities { get; private set; }
        public IEnumerable<Department> department { get; private set; }
        public IEnumerable<DeliveryPriority> DeliveryPriorities { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.VisualMatrix> VMObjects { get; private set; }

        public IEnumerable<ICM.Module.BusinessObjects.Category> CategoryObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.SampleManagement.Category> AttachmentCategoryObjects { get; private set; }
        public IEnumerable<ICM.Module.BusinessObjects.Grades> GradeObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.QCType> QCTypeObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.QcRole> QcRoleObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.QCRootRole> QCRootRoleObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.QCSource> QCSourceObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.SampleType> SampleTypeObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.SampleManagement.SampleStatus> Samplestatuses { get; private set; }

        public IEnumerable<Employee> AssignedByObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.TurnAroundTime> TestPriceObjects { get; private set; }
        public IEnumerable<Modules.BusinessObjects.Setting.TestMethod> TestNameObjects { get; private set; }
        //public IEnumerable<TestMethod> MethodObjects { get; private set; }
        public IEnumerable<Component> ComponentObjects { get; private set; }
        public IEnumerable<HoldingTimes> HoldingTimesObjects { get; private set; }
         public IEnumerable<QCCategory> QCCategoryObjects { get; private set; }
        public IEnumerable<Project> ProjectObjects { get; private set; }
        public IEnumerable<Collector> CollectorObjects { get; private set; }
        public IEnumerable<ReagentUnits> ReagentUnitsObjects { get; private set; }
        //public IEnumerable<Modules.BusinessObjects.Setting.ClauseOptions> ClauseOptionsObjects { get; private set; }

        public ReferencedTemplate(IEnumerable<FiberTypes> objects, string ColumnName)
        {
            FiberTypeObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.Unit> objects)
        {
            Objects = objects;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.Unit> objects, string ColumnName)
        {
            Objects = objects;
            UnitObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<ICM.Module.BusinessObjects.Packageunits> objects, string ColumnName)
        {
            PackageUnitObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<ICM.Module.BusinessObjects.Category> objects, string ColumnName)
        {
            CategoryObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.SampleManagement.Category> objects, string ColumnName)
        {
            AttachmentCategoryObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<ICM.Module.BusinessObjects.Grades> objects, string ColumnName)
        {
            GradeObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.ICM.ICMStorage> objects, string ColumnName)
        {
            ICMObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Hr.Employee> objects, string ColumnName)
        {
            EMPObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<ICM.Module.BusinessObjects.Vendors> objects, string ColumnName)
        {
            VENObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.ICM.Manufacturer> objects, string ColumnName)
        {
            BRANDObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<ICM.Module.BusinessObjects.Shippingoptions> objects, string ColumnName)
        {
            SHIPObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Priority> objects, string ColumnName)
        {
            Priorities = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<DeliveryPriority> objects, string ColumnName)
        {
            DeliveryPriorities = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Department> objects, string ColumnName)
        {
            department = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.ICM.Items> objects, string ColumnName)
        {
            ICMItems = objects;
            PropertyName = ColumnName;
        }

        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.VisualMatrix> objects, string ColumnName)
        {
            VMObjects = objects;
            PropertyName = ColumnName;
        }

        public ReferencedTemplate(IEnumerable<QCCategory> objects, string ColumnName)
        {
            QCCategoryObjects = objects;
            PropertyName = ColumnName;
        }

        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.QCType> objects, string ColumnName)
        {
            QCTypeObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.QcRole> objects, string ColumnName)
        {
            QcRoleObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.QCRootRole> objects, string ColumnName)
        {
            QCRootRoleObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.QCSource> objects, string ColumnName)
        {
            QCSourceObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.SampleType> objects, string ColumnName)
        {
            SampleTypeObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.SampleManagement.SampleStatus> objects, string ColumnName)
        {
            Samplestatuses = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.TurnAroundTime> objects, string Columnname)
        {
            TestPriceObjects = objects;
            PropertyName = Columnname;
        }
        public ReferencedTemplate(IEnumerable<Modules.BusinessObjects.Setting.TestMethod> objects, string Columnname)
        {
            TestNameObjects = objects;
            PropertyName = Columnname;
        }
        //public ReferencedTemplate(IEnumerable<TestMethod> objects, string Columnname)
        //{
        //    MethodObjects = objects;
        //    PropertyName = Columnname;
        //}
        public ReferencedTemplate(IEnumerable<Component> objects, string Columnname)
        {
            ComponentObjects = objects;
            PropertyName = Columnname;
        }

        public ReferencedTemplate(IEnumerable<HoldingTimes> objects, string ColumnName)
        {
            HoldingTimesObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Project> objects, string ColumnName)
        {
            ProjectObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<Collector> objects, string ColumnName)
        {
            CollectorObjects = objects;
            PropertyName = ColumnName;
        }
        public ReferencedTemplate(IEnumerable<ReagentUnits> objects, string ColumnName)
        {
            ReagentUnitsObjects = objects;
            PropertyName = ColumnName;
        }
        public void InstantiateIn(Control container)
        {
            GridViewEditItemTemplateContainer gridContainer = (GridViewEditItemTemplateContainer)container;

            ASPxComboBox comboBox = new ASPxComboBox();
            comboBox.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            if (PropertyName == "FiberType")
            {
                comboBox.ClientInstanceName = "ClientFiberType";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in FiberTypeObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.FiberType, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }

            if (PropertyName == "FinalDefaultUnits")
            {
                comboBox.ClientInstanceName = "ClientFinalDefaultUnits";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in UnitObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }

            else if (PropertyName == "DefaultUnits")
            {
                comboBox.ClientInstanceName = "ClientDefaultUnits";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in UnitObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "SurrogateUnits")
            {
                comboBox.ClientInstanceName = "ClientSurrogateUnits";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in UnitObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "FinalResultUnits")
            {
                comboBox.ClientInstanceName = "ClientFinalResultUnits";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in UnitObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "STDConcUnit")
            {
                comboBox.ClientInstanceName = "ClientSTDConcUnit";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in UnitObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "STDVolUnit")
            {
                comboBox.ClientInstanceName = "ClientSTDVolUnit";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in UnitObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            //else if (PropertyName == "SpikeAmountUnit")
            //{
            //    comboBox.ClientInstanceName = "ClientSpikeAmountUnit";
            //    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
            //    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
            //    ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
            //    comboBox.Items.Add(notAssignedItem1);
            //    foreach (var currentObject in UnitObjects)
            //    {
            //        ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
            //        comboBox.Items.Add(item);
            //    }
            //    container.Controls.Add(comboBox);
            //    return;
            //}
            else if (PropertyName == "ShippingOption")
            {
                comboBox.ClientInstanceName = "clientship";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in SHIPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.option, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else
            if (PropertyName == "DeliveryPriority")
            {
                comboBox.ClientInstanceName = "clientdeliverypriority";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in DeliveryPriorities)
                {
                    ListEditItem item = new ListEditItem(currentObject.Name, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else
            if (PropertyName == "department")
            {
                comboBox.ClientInstanceName = "clientdepartment";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in department)
                {
                    ListEditItem item = new ListEditItem(currentObject.Name, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Manufacturer")
            {
                comboBox.ClientInstanceName = "clientManufacturer";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in BRANDObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.ManufacturerName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Itemname")
            {
                comboBox.ClientInstanceName = "clientItemname";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                //ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                //comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in ICMItems)
                {
                    if (currentObject.Vendor != null)
                    {
                        ListEditItem item = new ListEditItem(currentObject.items, currentObject.Oid + "#" + currentObject.StockQty + "#" + currentObject.Vendor.Vendor + "#" + currentObject.Vendor.Oid);
                        comboBox.Items.Add(item);
                    }
                    else
                    {
                        ListEditItem item = new ListEditItem(currentObject.items, currentObject.Oid + "#" + currentObject.StockQty);
                        comboBox.Items.Add(item);
                    }
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Storage")
            {
                comboBox.ClientInstanceName = "clientstorage";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in ICMObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.storage, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "givento")
            {
                comboBox.ClientInstanceName = "clientgivento";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "GivenBy")
            {
                comboBox.ClientInstanceName = "clientGivenBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "DistributedBy")
            {
                comboBox.ClientInstanceName = "clientDistributedBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            if (PropertyName == "EnteredBy")
            {
                comboBox.ClientInstanceName = "clientEnteredBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "ReceivedBy")
            {
                comboBox.ClientInstanceName = "clientReceiveby";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "AnalyzedBy")
            {
                comboBox.ClientInstanceName = "clientAnalyzedBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);
                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                if (tempviewinfo.strtempviewid == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" && currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) == null)
                {
                    if (analysisDeptUser.lstAnalysisEmp != null && analysisDeptUser.lstAnalysisEmp.Count > 0)
                    {
                        foreach (var currentObject in analysisDeptUser.lstAnalysisEmp.ToList())
                        {
                            ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                            comboBox.Items.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var currentObject in EMPObjects)
                    {
                        ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "ApprovedBy")
            {
                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                if (tempviewinfo.strtempviewid == "SampleParameter_ListView_Copy_QCResultApproval" || tempviewinfo.strtempviewid == "SampleParameter_ListView_Copy_ResultApproval")
                {
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) == null)
                    {
                        if (analysisDeptUser.lstAnalysisEmp != null && analysisDeptUser.lstAnalysisEmp.Count > 0)
                        {
                            comboBox.ClientInstanceName = "clientResultApprovedBy";
                            comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                            comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                            ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                            comboBox.Items.Add(notAssignedItem2);
                            foreach (var currentObject in analysisDeptUser.lstAnalysisEmp.ToList())
                            {
                                ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                                comboBox.Items.Add(item);
                            }
                        }
                    }
                    else
                    {
                        comboBox.ClientInstanceName = "clientResultApprovedBy";
                        comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                        comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                        ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                        comboBox.Items.Add(notAssignedItem2);
                        foreach (var currentObject in EMPObjects)
                        {
                            ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                            comboBox.Items.Add(item);
                        }
                    }
                }
                else
                {
                    comboBox.ClientInstanceName = "clientApprovedBy";
                    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                    ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                    comboBox.Items.Add(notAssignedItem2);
                    foreach (var currentObject in EMPObjects)
                    {
                        ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "ValidatedBy")
            {
                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                comboBox.ClientInstanceName = "clientResultValidatedBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);
                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) == null && (tempviewinfo.strtempviewid == "SampleParameter_ListView_Copy_QCResultValidation" || tempviewinfo.strtempviewid == "SampleParameter_ListView_Copy_ResultValidation"))
                {
                    if (analysisDeptUser.lstAnalysisEmp != null && analysisDeptUser.lstAnalysisEmp.Count > 0)
                    {
                        foreach (var currentObject in analysisDeptUser.lstAnalysisEmp.ToList())
                        {
                            ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                            comboBox.Items.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var currentObject in EMPObjects)
                    {
                        ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Vendor")
            {
                comboBox.ClientInstanceName = "clientVendor";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem3 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem3);

                foreach (var currentObject in VENObjects)
                {
                    if (currentObject.RetiredDate >= DateTime.Now)
                    {
                        ListEditItem item = new ListEditItem(currentObject.Vendor, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "ConsumptionBy")
            {
                comboBox.ClientInstanceName = "clientConsumptionBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "DisposedBy")
            {
                comboBox.ClientInstanceName = "clientHandledBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "SubOutBy")
            {
                comboBox.ClientInstanceName = "clientSubOutBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "VisualMatrix")
            {
                if (tempviewinfo.strtempviewid == "SampleLogIn_ListView_Copy_SampleRegistration")
                {
                    comboBox.ClientInstanceName = "clientVisualMatrix";
                    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                    ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                    comboBox.Items.Add(notAssignedItem1);
                    foreach (var currentObject in SRInfo.lstSRvisualmat.ToList())
                    {
                        ListEditItem item = new ListEditItem(currentObject.VisualMatrixName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                    container.Controls.Add(comboBox);
                    return;
                }
                else if (tempviewinfo.strtempviewid == "COCSettingsSamples_ListView_Copy_SampleRegistration")
                {
                    comboBox.ClientInstanceName = "clientVisualMatrix";
                    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                    ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                    comboBox.Items.Add(notAssignedItem1);
                    foreach (var currentObject in COCsr.lstCOCvisualmat.ToList())
                    {
                        ListEditItem item = new ListEditItem(currentObject.VisualMatrixName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                    container.Controls.Add(comboBox);
                    return;
                }
                else
                {
                    comboBox.ClientInstanceName = "clientVisualMatrix";
                    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                    ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                    comboBox.Items.Add(notAssignedItem1);
                    foreach (var currentObject in VMObjects)
                    {
                        ListEditItem item = new ListEditItem(currentObject.VisualMatrixName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                    container.Controls.Add(comboBox);
                    return;
                }
            }
            else if (PropertyName == "QCCategory")
            {
                comboBox.ClientInstanceName = "clientQCCategory";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in QCCategoryObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.QCCategoryName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Collector")
            {
                comboBox.ClientInstanceName = "clientCollector";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in CollectorObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.FullName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Category")
            {
                comboBox.ClientInstanceName = "clientItemCategory";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in CategoryObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.category, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Grade")
            {
                comboBox.ClientInstanceName = "clientItemGrade";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in GradeObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.Grade, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Unit" || PropertyName == "Units")
            {
                if (tempviewinfo.strsampleviewtype != "SampleParameter")
                {
                    comboBox.ClientInstanceName = "clientItemUnit";
                    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                    ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                    comboBox.Items.Add(notAssignedItem2);
                    if (PackageUnitObjects != null)
                    {
                        foreach (var currentObject in PackageUnitObjects)
                        {
                            ListEditItem item = new ListEditItem(currentObject.Option, currentObject.Oid);
                            comboBox.Items.Add(item);
                        }
                        container.Controls.Add(comboBox);
                        return;
                    }
                }
                else if (tempviewinfo.strsampleviewtype == "SampleParameter")
                {
                    comboBox.ClientInstanceName = "clientUnitName";
                    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                    ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                    comboBox.Items.Add(notAssignedItem2);
                    if (UnitObjects != null)
                    {
                        foreach (var currentObject in UnitObjects)
                        {
                            ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                            comboBox.Items.Add(item);
                        }
                        container.Controls.Add(comboBox);
                        return;
                    }
                }

            }

            if (PropertyName == "AmountUnit")
            {
                comboBox.ClientInstanceName = "clientItemAmountUnit";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);
                if (Objects != null)
                {
                    foreach (var currentObject in Objects)
                    {
                        ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                    container.Controls.Add(comboBox);
                    return;
                }

            }
            if (PropertyName == "Units")
            {
                comboBox.ClientInstanceName = "clientUnit";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);
                if (Objects != null)
                {
                    foreach (var currentObject in Objects)
                    {
                        ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                    container.Controls.Add(comboBox);
                    return;
                }
            }
            if (PropertyName == "SampleparameterUnit")
            {
                comboBox.ClientInstanceName = "clientUnit";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);
                if (Objects != null)
                {
                    foreach (var currentObject in Objects)
                    {
                        ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                    container.Controls.Add(comboBox);
                    return;
                }
            }
            if (PropertyName == "SpikeAmountUnit")
            {
                comboBox.ClientInstanceName = "clientSpikeAmountUnit";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);
                if (Objects != null)
                {
                    foreach (var currentObject in Objects)
                    {
                        ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                    container.Controls.Add(comboBox);
                    return;
                }
            }

            else if (PropertyName == "QCType")
            {
                comboBox.ClientInstanceName = "clientColQCType";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in QCTypeObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.QCTypeName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "QcRole")
            {
                comboBox.ClientInstanceName = "clientColQcRole";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in QcRoleObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.QC_Role, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "QCRootRole")
            {
                comboBox.ClientInstanceName = "clientColQCRootRole";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in QCRootRoleObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.QCRoot_Role, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "QCSource")
            {
                comboBox.ClientInstanceName = "clientColQCSource";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in QCSourceObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.QC_Source, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "SampleType")
            {
                comboBox.ClientInstanceName = "clientSampleType";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in SampleTypeObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.SampleTypeName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "SampleStatus")
            {
                comboBox.ClientInstanceName = "clientSampleStatus";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in Samplestatuses)
                {
                    ListEditItem item = new ListEditItem(currentObject.Samplestatus, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "HTBeforeAnalysis")
            {
                comboBox.ClientInstanceName = "clientHTBeforeAnalysis";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in HoldingTimesObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.HoldingTime, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "HTBeforePrep")
            {
                comboBox.ClientInstanceName = "clientHTBeforePrep";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in HoldingTimesObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.HoldingTime, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "TAT")
            {
                //if(tatinfo.lsttat != null && tatinfo.lsttat.Count > 0)
                //{
                //    comboBox.ClientInstanceName = "ClientTestPriceTAT";
                //    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                //    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                //    //ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                //    //comboBox.Items.Add(notAssignedItem2);
                //    foreach (var currentObject in tatinfo.lsttat.Cast<TurnAroundTime>().OrderBy(i => i.Count))
                //    {
                //        ListEditItem item = new ListEditItem(currentObject.TAT, currentObject.Oid);
                //        comboBox.Items.Add(item);
                //    }
                //    container.Controls.Add(comboBox);
                //    return;
                //}
                //else
                //{
                //comboBox.ClientInstanceName = "ClientTestPriceTAT";
                comboBox.ClientInstanceName = "InvoiceTAT";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                //ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                //comboBox.Items.Add(notAssignedItem2);
                foreach (var currentObject in TestPriceObjects.Cast<TurnAroundTime>().OrderBy(i => i.Count))
                {
                    ListEditItem item = new ListEditItem(currentObject.TAT, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
                //}
            }
            else if (PropertyName == "AssignedBy")
            {
                comboBox.ClientInstanceName = "clientAssignedBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "TransferredBy")
            {
                comboBox.ClientInstanceName = "clientTransferredBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "DeliveredBy")
            {
                comboBox.ClientInstanceName = "clientDeliveredBy";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);

                foreach (var currentObject in EMPObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.DisplayName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Component")
            {
                comboBox.ClientInstanceName = "clientComponent";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem2);
                if (quoteinfo.lsttempComponent != null && quoteinfo.lsttempComponent.Count > 0)
                {
                    foreach (var currentObject in quoteinfo.lsttempComponent.ToList())
                    {
                        ListEditItem item = new ListEditItem(currentObject.Components, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                    container.Controls.Add(comboBox);
                    return;
                }
                else
                {
                    foreach (var currentObject in ComponentObjects)
                    {
                        ListEditItem item = new ListEditItem(currentObject.Components, currentObject.Oid);
                        comboBox.Items.Add(item);
                    }
                    container.Controls.Add(comboBox);
                    return;
                }

            }
            //else if (PropertyName == "Matrix")
            //{
            //    comboBox.ClientInstanceName = "clientMatrix";
            //    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
            //    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
            //    ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
            //    comboBox.Items.Add(notAssignedItem1);
            //    foreach (var currentObject in VMObjects)
            //    {
            //        ListEditItem item = new ListEditItem(currentObject.VisualMatrixName, currentObject.Oid);
            //        comboBox.Items.Add(item);
            //    }
            //    container.Controls.Add(comboBox);
            //    return;
            //}
            else if (PropertyName == "Test")
            {
                comboBox.ClientInstanceName = "clientTestName";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in TestNameObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.TestName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Collector")
            {
                comboBox.ClientInstanceName = "clientCollector";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in CollectorObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.FullName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Method")
            {
                comboBox.ClientInstanceName = "clientTestMethodNumber";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in TestNameObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.MethodName.MethodNumber, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "ProjectID")
            {
                comboBox.ClientInstanceName = "clientProjectID";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in ProjectObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.ProjectId, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "WSCons_Units")
            {
                comboBox.ClientInstanceName = "clientWSCons_Units";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in ReagentUnitsObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.Units, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Cal_VolTaken_V1_Units")
            {
                comboBox.ClientInstanceName = "clientCal_VolTaken_V1_Units";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in ReagentUnitsObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.Units, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Cal_FinalVol_V2_Units")
            {
                comboBox.ClientInstanceName = "clientCal_FinalVol_V2_Units";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in ReagentUnitsObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.Units, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }
            else if (PropertyName == "Cal_FinalConc_C2_Units")
            {
                comboBox.ClientInstanceName = "clientCal_FinalConc_C2_Units";
                comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;
                comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;
                ListEditItem notAssignedItem1 = new ListEditItem("N/A", null);
                comboBox.Items.Add(notAssignedItem1);
                foreach (var currentObject in ReagentUnitsObjects)
                {
                    ListEditItem item = new ListEditItem(currentObject.Units, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
                return;
            }


            //else if (PropertyName == "Conclusion")
            //{
            //    comboBox.ClientInstanceName = "clientColOptions";
            //    comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

            //    comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

            //    ListEditItem notAssignedItem2 = new ListEditItem("N/A", null);
            //    comboBox.Items.Add(notAssignedItem2);

            //    foreach (var currentObject in ClauseOptionsObjects)
            //    {
            //        ListEditItem item = new ListEditItem(currentObject.Option, currentObject.Oid);
            //        comboBox.Items.Add(item);
            //    }
            //    container.Controls.Add(comboBox);
            //    return;
            //}
            else
            {
                comboBox.ClientInstanceName = "ReferencedEdit";
            }

            comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

            comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

            ListEditItem notAssignedItem = new ListEditItem("N/A", null);
            comboBox.Items.Add(notAssignedItem);

            if (Objects != null)
            {
                foreach (var currentObject in Objects)
                {
                    ListEditItem item = new ListEditItem(currentObject.UnitName, currentObject.Oid);
                    comboBox.Items.Add(item);
                }
                container.Controls.Add(comboBox);
            }
        }
    }
}
