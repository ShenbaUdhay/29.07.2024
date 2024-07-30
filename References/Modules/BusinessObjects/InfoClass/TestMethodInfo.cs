using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using ICM.Module.BusinessObjects;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.EDD;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.Metrc;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.ReagentPreparation;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Invoicing;
using Modules.BusinessObjects.Setting.Labware;
using Modules.BusinessObjects.Setting.Quotes;
using Modules.BusinessObjects.Setting.SDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace Modules.BusinessObjects.InfoClass
{

    public class DBConnectioninfo
    {
        public string strdbconstring
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strdbconstring");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strdbconstring");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class objectspaceinfo
    {
        public IObjectSpace tempobjspace
        {
            get
            {
                IValueManager<IObjectSpace> valueManager = ValueManager.GetValueManager<IObjectSpace>("tempobjspace");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IObjectSpace> valueManager = ValueManager.GetValueManager<IObjectSpace>("tempobjspace");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Frame tempFrame
        {
            get
            {
                IValueManager<Frame> valueManager = ValueManager.GetValueManager<Frame>("tempFrame");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Frame> valueManager = ValueManager.GetValueManager<Frame>("tempFrame");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public XafApplication tempapplication
        {
            get
            {
                IValueManager<XafApplication> valueManager = ValueManager.GetValueManager<XafApplication>("tempapplication");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<XafApplication> valueManager = ValueManager.GetValueManager<XafApplication>("tempapplication");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class DailyQCinfo
    {
        public DailyQCSettings Dailyqc
        {
            get
            {
                IValueManager<DailyQCSettings> valueManager = ValueManager.GetValueManager<DailyQCSettings>("Dailyqc");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<DailyQCSettings> valueManager = ValueManager.GetValueManager<DailyQCSettings>("Dailyqc");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Modules.BusinessObjects.Setting.DailyQC> sellist
        {
            get
            {
                IValueManager<List<Modules.BusinessObjects.Setting.DailyQC>> valueManager = ValueManager.GetValueManager<List<Modules.BusinessObjects.Setting.DailyQC>>("sellist");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Modules.BusinessObjects.Setting.DailyQC>();
            }
            set
            {
                IValueManager<List<Modules.BusinessObjects.Setting.DailyQC>> valueManager = ValueManager.GetValueManager<List<Modules.BusinessObjects.Setting.DailyQC>>("sellist");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class testparameter
    {
        public List<string> lstaccridation
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstaccridation");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstaccridation");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class Contractlabinfo
    {
        public List<Matrix> listMatrix
        {
            get
            {
                IValueManager<List<Matrix>> valueManager = ValueManager.GetValueManager<List<Matrix>>("listMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Matrix>();
            }
            set
            {
                IValueManager<List<Matrix>> valueManager = ValueManager.GetValueManager<List<Matrix>>("listMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<TestMethod> listTestMethod
        {
            get
            {
                IValueManager<List<TestMethod>> valueManager = ValueManager.GetValueManager<List<TestMethod>>("listTestMethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<TestMethod>();
            }
            set
            {
                IValueManager<List<TestMethod>> valueManager = ValueManager.GetValueManager<List<TestMethod>>("listTestMethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Modules.BusinessObjects.Setting.TestMethod> listMethod
        {
            get
            {
                IValueManager<List<Modules.BusinessObjects.Setting.TestMethod>> valueManager = ValueManager.GetValueManager<List<Modules.BusinessObjects.Setting.TestMethod>>("listMethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Modules.BusinessObjects.Setting.TestMethod>();
            }
            set
            {
                IValueManager<List<Modules.BusinessObjects.Setting.TestMethod>> valueManager = ValueManager.GetValueManager<List<Modules.BusinessObjects.Setting.TestMethod>>("listMethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SetupRemoveInfo
    {
        public List<SpreadSheetBuilder_InitialQCTestRun> RemoveInitialQCTestRun
        {
            get
            {
                IValueManager<List<SpreadSheetBuilder_InitialQCTestRun>> valueManager = ValueManager.GetValueManager<List<SpreadSheetBuilder_InitialQCTestRun>>("RemoveInitialQCTestRun");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SpreadSheetBuilder_InitialQCTestRun>();
            }
            set
            {
                IValueManager<List<SpreadSheetBuilder_InitialQCTestRun>> valueManager = ValueManager.GetValueManager<List<SpreadSheetBuilder_InitialQCTestRun>>("RemoveInitialQCTestRun");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<SpreadSheetBuilder_SampleQCTestRun> RemoveSampleQCTestRun
        {
            get
            {
                IValueManager<List<SpreadSheetBuilder_SampleQCTestRun>> valueManager = ValueManager.GetValueManager<List<SpreadSheetBuilder_SampleQCTestRun>>("RemoveSampleQCTestRun");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SpreadSheetBuilder_SampleQCTestRun>();
            }
            set
            {
                IValueManager<List<SpreadSheetBuilder_SampleQCTestRun>> valueManager = ValueManager.GetValueManager<List<SpreadSheetBuilder_SampleQCTestRun>>("RemoveSampleQCTestRun");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<SpreadSheetBuilder_ClosingQCTestRun> RemoveClosingQCTestRun
        {
            get
            {
                IValueManager<List<SpreadSheetBuilder_ClosingQCTestRun>> valueManager = ValueManager.GetValueManager<List<SpreadSheetBuilder_ClosingQCTestRun>>("RemoveClosingQCTestRun");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SpreadSheetBuilder_ClosingQCTestRun>();
            }
            set
            {
                IValueManager<List<SpreadSheetBuilder_ClosingQCTestRun>> valueManager = ValueManager.GetValueManager<List<SpreadSheetBuilder_ClosingQCTestRun>>("RemoveClosingQCTestRun");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class NavigationItemsInfo
    {
        public List<string> InVisibleNavigationItems
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>(nameof(InVisibleNavigationItems));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>(nameof(InVisibleNavigationItems));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> VisibleNavigationItems
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>(nameof(VisibleNavigationItems));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>(nameof(VisibleNavigationItems));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool UserIsAdministrator
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(UserIsAdministrator));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(UserIsAdministrator));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ClickedNavigationItem
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClickedNavigationItem");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClickedNavigationItem");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class LeadInfo
    {
        public string LeadName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LeadName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LeadName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string CompanyName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("CompanyName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("CompanyName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Nullable<Guid> NewContactOid
        {
            get
            {
                IValueManager<Nullable<Guid>> valueManager = ValueManager.GetValueManager<Nullable<Guid>>("NewContactOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Nullable<Guid>> valueManager = ValueManager.GetValueManager<Nullable<Guid>>("NewContactOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> ListNewContact
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(ListNewContact));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(ListNewContact));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }



        public string LeadStatus
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LeadStatus");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LeadStatus");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool boolOpportunity
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(boolOpportunity));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(boolOpportunity));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }


        public class ProspectsInfo
        {
            public string PotentialCustomer
            {
                get
                {
                    IValueManager<string> valueManager = ValueManager.GetValueManager<string>("PotentialCustomer");
                    if (valueManager.CanManageValue)
                        return valueManager.Value;
                    else return string.Empty;
                }
                set
                {
                    IValueManager<string> valueManager = ValueManager.GetValueManager<string>("PotentialCustomer");
                    if (valueManager.CanManageValue)
                        valueManager.Value = value;
                }
            }

            public string CompanyName
            {
                get
                {
                    IValueManager<string> valueManager = ValueManager.GetValueManager<string>("CompanyName");
                    if (valueManager.CanManageValue)
                        return valueManager.Value;
                    else return string.Empty;
                }
                set
                {
                    IValueManager<string> valueManager = ValueManager.GetValueManager<string>("CompanyName");
                    if (valueManager.CanManageValue)
                        valueManager.Value = value;
                }
            }
        }


        public class VersionControlInfo
        {
            public string VersionNumber
            {
                get
                {
                    IValueManager<string> valueManager = ValueManager.GetValueManager<string>("VersionNumber");
                    if (valueManager.CanManageValue)
                        return valueManager.Value;
                    else return string.Empty;
                }
                set
                {
                    IValueManager<string> valueManager = ValueManager.GetValueManager<string>("VersionNumber");
                    if (valueManager.CanManageValue)
                        valueManager.Value = value;
                }
            }
        }




        public Guid LeadOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(LeadOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(LeadOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }




    public class ClientInfo
    {
        public string ClientName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClientName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClientName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Customer Client
        {
            get
            {
                IValueManager<Customer> valueManager = ValueManager.GetValueManager<Customer>("Client");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Customer> valueManager = ValueManager.GetValueManager<Customer>("Client");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }



    public class VendorInfo
    {
        public Vendors Vendors
        {
            get
            {
                IValueManager<Vendors> valueManager = ValueManager.GetValueManager<Vendors>("Vendors");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Vendors> valueManager = ValueManager.GetValueManager<Vendors>("Vendors");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }

    public class ItemInfo
    {
        public string Total
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Total");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Total");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public double TotalPriceWithTax
        {
            get
            {
                IValueManager<double> valueManager = ValueManager.GetValueManager<double>("TotalPriceWithTax");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0.0;
            }
            set
            {
                IValueManager<double> valueManager = ValueManager.GetValueManager<double>("TotalPriceWithTax");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }






    public class TimeZoneinfo
    {
        public TimeZoneInfo TimeZone
        {
            get
            {
                IValueManager<TimeZoneInfo> valueManager = ValueManager.GetValueManager<TimeZoneInfo>("TimeZone");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<TimeZoneInfo> valueManager = ValueManager.GetValueManager<TimeZoneInfo>("TimeZone");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string Date
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Date");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Date");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class SaleBaseInfo
    {
        public string ViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsCRMQuotes
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsCRMQuotes");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsCRMQuotes");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class ReportingToInfo
    {
        public IList<string> ReportingTo
        {
            get
            {
                IValueManager<IList<string>> valueManager = ValueManager.GetValueManager<IList<string>>("ReportingTo");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<string>> valueManager = ValueManager.GetValueManager<IList<string>>("ReportingTo");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    class PaymentInfo
    {
        public DateTime AccountFilterDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("AccountFilterDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.MinValue;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("AccountFilterDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class Tabinfo
    {
        public string strtabname
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtabname");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtabname");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class QuotesInfo
    {
        public bool Analysispopupwindow
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Analysispopupwindow");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Analysispopupwindow");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool Itempopupwindow
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Itempopupwindow");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Itempopupwindow");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid QuotePopupCrtAnalysispriceOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("QuotePopupCrtAnalysispriceOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.Empty;
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("QuotePopupCrtAnalysispriceOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid QuotePopupTATOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("QuotePopupTATOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.Empty;
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("QuotePopupTATOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid QuotePopupVMOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("QuotePopupVMOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.Empty;
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("QuotePopupVMOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strtempparamsstatus
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtempparamsstatus");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtempparamsstatus");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lsttempparamsoid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lsttempparamsoid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lsttempparamsoid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<AnalysisPricing> lsttempAnalysisPricing
        {
            get
            {
                IValueManager<List<AnalysisPricing>> valueManager = ValueManager.GetValueManager<List<AnalysisPricing>>("lsttempAnalysisPricing");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<AnalysisPricing>();
            }
            set
            {
                IValueManager<List<AnalysisPricing>> valueManager = ValueManager.GetValueManager<List<AnalysisPricing>>("lsttempAnalysisPricing");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<QuotesItemChargePrice> lsttempItemPricing
        {
            get
            {
                IValueManager<List<QuotesItemChargePrice>> valueManager = ValueManager.GetValueManager<List<QuotesItemChargePrice>>("lsttempItemPricing");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<QuotesItemChargePrice>();
            }
            set
            {
                IValueManager<List<QuotesItemChargePrice>> valueManager = ValueManager.GetValueManager<List<QuotesItemChargePrice>>("lsttempItemPricing");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<AnalysisPricing> lsttempAnalysisPricingpopup
        {
            get
            {
                IValueManager<List<AnalysisPricing>> valueManager = ValueManager.GetValueManager<List<AnalysisPricing>>("lsttempAnalysisPricingpopup");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<AnalysisPricing>();
            }
            set
            {
                IValueManager<List<AnalysisPricing>> valueManager = ValueManager.GetValueManager<List<AnalysisPricing>>("lsttempAnalysisPricingpopup");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<AnalysisPricing> lstInitialtempAnalysisPricingpopup
        {
            get
            {
                IValueManager<List<AnalysisPricing>> valueManager = ValueManager.GetValueManager<List<AnalysisPricing>>("lstInitialtempAnalysisPricingpopup");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<AnalysisPricing>();
            }
            set
            {
                IValueManager<List<AnalysisPricing>> valueManager = ValueManager.GetValueManager<List<AnalysisPricing>>("lstInitialtempAnalysisPricingpopup");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<ItemChargePricing> lsttempItemPricingpopup
        {
            get
            {
                IValueManager<List<ItemChargePricing>> valueManager = ValueManager.GetValueManager<List<ItemChargePricing>>("lsttempItemPricingpopup");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<ItemChargePricing>();
            }
            set
            {
                IValueManager<List<ItemChargePricing>> valueManager = ValueManager.GetValueManager<List<ItemChargePricing>>("lsttempItemPricingpopup");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<ItemChargePricing> lstinitialtempItemPricingpopup
        {
            get
            {
                IValueManager<List<ItemChargePricing>> valueManager = ValueManager.GetValueManager<List<ItemChargePricing>>("lstinitialtempItemPricingpopup");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<ItemChargePricing>();
            }
            set
            {
                IValueManager<List<ItemChargePricing>> valueManager = ValueManager.GetValueManager<List<ItemChargePricing>>("lstinitialtempItemPricingpopup");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsAnalycialPricingpopupselectall
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsAnalycialPricingpopupselectall");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsAnalycialPricingpopupselectall");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsItemchargePricingpopupselectall
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsItemchargePricingpopupselectall");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsItemchargePricingpopupselectall");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstpricecodeempty
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstpricecodeempty");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstpricecodeempty");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Component> lsttempComponent
        {
            get
            {
                IValueManager<List<Component>> valueManager = ValueManager.GetValueManager<List<Component>>("lsttempComponent");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Component>();
            }
            set
            {
                IValueManager<List<Component>> valueManager = ValueManager.GetValueManager<List<Component>>("lsttempComponent");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsobjChangedproperty
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsobjChangedproperty");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsobjChangedproperty");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsobjChangedpropertyinQuotes
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsobjChangedpropertyinQuotes");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsobjChangedpropertyinQuotes");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public decimal lvDetailedPrice
        {
            get
            {
                IValueManager<decimal> valueManager = ValueManager.GetValueManager<decimal>("lvDetailedPrice");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<decimal> valueManager = ValueManager.GetValueManager<decimal>("lvDetailedPrice");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsQuotesSave
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsQuotesSave");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsQuotesSave");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public CRMQuotes popupcurtquote
        {
            get
            {
                IValueManager<CRMQuotes> valueManager = ValueManager.GetValueManager<CRMQuotes>("popupcurtquote");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<CRMQuotes> valueManager = ValueManager.GetValueManager<CRMQuotes>("popupcurtquote");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsDiscountAmtChanged
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsDiscountAmtChanged");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsDiscountAmtChanged");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsTabDiscountChanged
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTabDiscountChanged");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTabDiscountChanged");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class DocumentAttachmentInfo
    {
        public Guid DocumentIDOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("DocumentIDOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("DocumentIDOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid RevNumberOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("RevNumberOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("RevNumberOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int rowCount
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("rowCount");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("rowCount");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class CorrectiveActionVerificationInfo
    {
        public Guid CorrectiveActionVerificationOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("CorrectiveActionVerificationOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("CorrectiveActionVerificationOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class viewInfo
    {
        public string strtempviewid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtempviewid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtempviewid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strtempresultentryviewid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtempresultentryviewid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtempresultentryviewid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strsampleviewid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strsampleviewid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strsampleviewid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strsampleviewtype
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strsampleviewtype");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strsampleviewtype");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class GroupTestParameterInfo
    {
        public List<GroupTestMethod> lsttempgrouptest
        {
            get
            {
                IValueManager<List<GroupTestMethod>> valueManager = ValueManager.GetValueManager<List<GroupTestMethod>>("lsttempgrouptest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<GroupTestMethod>();
            }
            set
            {
                IValueManager<List<GroupTestMethod>> valueManager = ValueManager.GetValueManager<List<GroupTestMethod>>("lsttempgrouptest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstgrouptestparameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstgrouptestparameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstgrouptestparameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class UADECInfo
    {
        public Guid CurDataEntry
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("CurDataEntry");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("CurDataEntry");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> TestName
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("TestName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("TestName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SDMSInfo
    {
        public List<Guid> lstqcbatcseqoid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstqcbatcseqoid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstqcbatcseqoid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstnonexpqcbatcseqoid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstnonexpqcbatcseqoid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstnonexpqcbatcseqoid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class Testpricesurchargeinfo
    {
        public bool IsNotRegularSelectAll
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNotRegularSelectAll");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNotRegularSelectAll");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<TestPriceSurcharge> lsttpsnewlist
        {
            get
            {
                IValueManager<List<TestPriceSurcharge>> valueManager = ValueManager.GetValueManager<List<TestPriceSurcharge>>("lsttpsnewlist");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<TestPriceSurcharge>();
            }
            set
            {
                IValueManager<List<TestPriceSurcharge>> valueManager = ValueManager.GetValueManager<List<TestPriceSurcharge>>("lsttpsnewlist");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<TestPriceSurcharge> lsttpssurchargedefTAT
        {
            get
            {
                IValueManager<List<TestPriceSurcharge>> valueManager = ValueManager.GetValueManager<List<TestPriceSurcharge>>("lsttpssurchargedefTAT");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<TestPriceSurcharge>();
            }
            set
            {
                IValueManager<List<TestPriceSurcharge>> valueManager = ValueManager.GetValueManager<List<TestPriceSurcharge>>("lsttpssurchargedefTAT");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lsttpsTAT
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lsttpsTAT");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lsttpsTAT");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string lsttpsTATedit
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("lsttpsTATedit");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("lsttpsTATedit");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class ContainerSettingInfo
    {
        public List<string> lsthtvalues
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lsthtvalues");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lsthtvalues");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strcontainer
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcontainer");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcontainer");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strpreservative
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strpreservative");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strpreservative");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstcontainer
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcontainer");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcontainer");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstpreservative
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstpreservative");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstpreservative");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class PricingInfo
    {
        public List<ConstituentPricingTier> lstselectremove
        {
            get
            {
                IValueManager<List<ConstituentPricingTier>> valueManager = ValueManager.GetValueManager<List<ConstituentPricingTier>>("lstselectremove");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<ConstituentPricingTier>();
            }
            set
            {
                IValueManager<List<ConstituentPricingTier>> valueManager = ValueManager.GetValueManager<List<ConstituentPricingTier>>("lstselectremove");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strTPSpricecode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTPSpricecode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTPSpricecode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strCPpricecode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strCPpricecode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strCPpricecode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strPPpricecode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strPPpricecode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strPPpricecode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> tpsguidlst
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("tpsguidlst");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("tpsguidlst");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ISPPunique
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ISPPunique");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ISPPunique");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ISPPNew
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ISPPNew");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ISPPNew");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class TATinfo
    {
        public List<TurnAroundTime> lsttat
        {
            get
            {
                IValueManager<List<TurnAroundTime>> valueManager = ValueManager.GetValueManager<List<TurnAroundTime>>("lsttat");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<List<TurnAroundTime>> valueManager = ValueManager.GetValueManager<List<TurnAroundTime>>("lsttat");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class Testpriceinfo
    {
        public List<Guid> lsttestpriceoid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lsttestprice");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lsttestprice");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public TestPrice CurrentTestPrice
        //{
        //    get
        //    {
        //        IValueManager<TestPrice> valueManager = ValueManager.GetValueManager<TestPrice>("CurrentTestPrice");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return null;
        //    }
        //    set
        //    {
        //        IValueManager<TestPrice> valueManager = ValueManager.GetValueManager<TestPrice>("CurrentTestPrice");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public string testpricetypeinfo
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("testpricetypeinfo");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("testpricetypeinfo");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public IList<TestPrice> lsttestprice
        //{
        //    get
        //    {
        //        IValueManager<IList<TestPrice>> valueManager = ValueManager.GetValueManager<IList<TestPrice>>("lsttestprice");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return null;
        //    }
        //    set
        //    {
        //        IValueManager<IList<TestPrice>> valueManager = ValueManager.GetValueManager<IList<TestPrice>>("lsttestprice");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        //public IList<TestPrice> lstRemovedTestPrice
        //{
        //    get
        //    {
        //        IValueManager<IList<TestPrice>> valueManager = ValueManager.GetValueManager<IList<TestPrice>>("lstRemovedTestPrice");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return null;
        //    }
        //    set
        //    {
        //        IValueManager<IList<TestPrice>> valueManager = ValueManager.GetValueManager<IList<TestPrice>>("lstRemovedTestPrice");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        //public IList<TestPrice> lstpertest
        //{
        //    get
        //    {
        //        IValueManager<IList<TestPrice>> valueManager = ValueManager.GetValueManager<IList<TestPrice>>("lstpretest");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return null;
        //    }
        //    set
        //    {
        //        IValueManager<IList<TestPrice>> valueManager = ValueManager.GetValueManager<IList<TestPrice>>("lstpretest");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public List<Guid> lstremovepertest
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstremovepertest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstremovepertest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public IList<TestPrice> lstperparameter
        //{
        //    get
        //    {
        //        IValueManager<IList<TestPrice>> valueManager = ValueManager.GetValueManager<IList<TestPrice>>("lstperparameter");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return null;
        //    }
        //    set
        //    {
        //        IValueManager<IList<TestPrice>> valueManager = ValueManager.GetValueManager<IList<TestPrice>>("lstperparameter");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public List<Guid> lstRemovedperparameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemovedperparameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemovedperparameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid crttestprice
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("crttestprice");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("crttestprice");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public TestPrice TestPriceCurrentObject
        //{
        //    get
        //    {
        //        IValueManager<TestPrice> valueManager = ValueManager.GetValueManager<TestPrice>("TestPriceCurrentObject");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return null;
        //    }
        //    set
        //    {
        //        IValueManager<TestPrice> valueManager = ValueManager.GetValueManager<TestPrice>("TestPriceCurrentObject");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public TestMethod testmethod
        {
            get
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>("testmethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>("testmethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }

        }

    }
    public class QCtypeinfo
    {
        public List<QCType> lstintialqctype
        {
            get
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstintialqctype");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<QCType>();
            }
            set
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstintialqctype");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<QCType> lstsampleqctype
        {
            get
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstsampleqctype");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<QCType>();
            }
            set
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstsampleqctype");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<QCType> lstclosingqctype
        {
            get
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstclosingqctype");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<QCType>();
            }
            set
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstclosingqctype");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<QCType> lstremoveintialqctype
        {
            get
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstremoveintialqctype");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<QCType>();
            }
            set
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstremoveintialqctype");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<QCType> lstremovesampleqctype
        {
            get
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstremovesampleqctype");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<QCType>();
            }
            set
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstremovesampleqctype");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<QCType> lstremoveclosingqctype
        {
            get
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstremoveclosingqctype");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<QCType>();
            }
            set
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("lstremoveclosingqctype");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class ProjectDetailsInfo
    {
        public string ProjectName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ProjectName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ProjectName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class TestMethodInfo
    {
        public TestMethodInfo()
        { }
        public Guid MatrixName;
        public Guid TestName;
        //public Guid MethodName;
        //public Guid ParameterName;

        //public static string ClientName = string.Empty;
        public string ClientName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClientName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClientName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ProjectName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ProjectName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ProjectName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static Guid uqMethodName;
        public string TestMethodOid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TestMethodOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TestMethodOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class spreadsheetitemsltno
    {
        public List<string> Items
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Items");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Items");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class Testmethodmethodname
    {
        public List<Modules.BusinessObjects.Setting.Method> lstmethod
        {
            get
            {
                IValueManager<List<Modules.BusinessObjects.Setting.Method>> valueManager = ValueManager.GetValueManager<List<Modules.BusinessObjects.Setting.Method>>("lstmethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Modules.BusinessObjects.Setting.Method>();
            }
            set
            {
                IValueManager<List<Modules.BusinessObjects.Setting.Method>> valueManager = ValueManager.GetValueManager<List<Modules.BusinessObjects.Setting.Method>>("lstmethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class sequencelinkviewinfo
    {
        public List<Guid> lstQCType
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstQCType");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstQCType");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class TestmethodQctypeinfo
    {
        public string chkselectall
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("chkselectall");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("chkselectall");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstQCType
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstQCType");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstQCType");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<QCType> objtmQCType
        {
            get
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("objtmQCType");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<QCType>();
            }
            set
            {
                IValueManager<List<QCType>> valueManager = ValueManager.GetValueManager<List<QCType>>("objtmQCType");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstcopytest
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcopytest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcopytest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string applicationviewid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("applicationviewid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("applicationviewid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strinstruments
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strinstruments");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strinstruments");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class CompReportInfo
    {
        public Nullable<Guid> CompReportOid
        {
            get
            {
                IValueManager<Nullable<Guid>> valueManager = ValueManager.GetValueManager<Nullable<Guid>>(nameof(CompReportOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Nullable<Guid>> valueManager = ValueManager.GetValueManager<Nullable<Guid>>(nameof(CompReportOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstNavigationItem
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(lstNavigationItem));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(lstNavigationItem));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstEmployee
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(lstEmployee));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(lstEmployee));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool CanSelect
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(CanSelect));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(CanSelect));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SampleMatrixSetupFieldsinfo
    {
        public bool IsSampleMatrixSetupFieldsCallBackSelectAll
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSampleMatrixSetupFieldsCallBackSelectAll");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSampleMatrixSetupFieldsCallBackSelectAll");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string chkselectall
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("chkselectall");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("chkselectall");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstsamplematrixguid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstsamplematrixguid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstsamplematrixguid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<SampleMatrixSetupFields> lstSampleMatrixField
        {
            get
            {
                IValueManager<List<SampleMatrixSetupFields>> valueManager = ValueManager.GetValueManager<List<SampleMatrixSetupFields>>("lstSampleMatrixField");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SampleMatrixSetupFields>();
            }
            set
            {
                IValueManager<List<SampleMatrixSetupFields>> valueManager = ValueManager.GetValueManager<List<SampleMatrixSetupFields>>("lstSampleMatrixField");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SamplingMatrixSetupFieldsinfo
    {
        public bool IsSampleMatrixSetupFieldsCallBackSelectAll
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSampleMatrixSetupFieldsCallBackSelectAll");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSampleMatrixSetupFieldsCallBackSelectAll");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string chkselectall
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("chkselectall");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("chkselectall");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstsamplematrixguid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstsamplematrixguid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstsamplematrixguid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<SampleMatrixSetupFields> lstSampleMatrixField
        {
            get
            {
                IValueManager<List<SampleMatrixSetupFields>> valueManager = ValueManager.GetValueManager<List<SampleMatrixSetupFields>>("lstSampleMatrixField");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SampleMatrixSetupFields>();
            }
            set
            {
                IValueManager<List<SampleMatrixSetupFields>> valueManager = ValueManager.GetValueManager<List<SampleMatrixSetupFields>>("lstSampleMatrixField");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class itemsvaluemanager
    {
        public List<Packageunits> packageunit
        {
            get
            {
                IValueManager<List<Packageunits>> valueManager = ValueManager.GetValueManager<List<Packageunits>>("packageunitsortno");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Packageunits>();
            }
            set
            {
                IValueManager<List<Packageunits>> valueManager = ValueManager.GetValueManager<List<Packageunits>>("packageunitsortno");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Vendors> vendors
        {
            get
            {
                IValueManager<List<Vendors>> valueManager = ValueManager.GetValueManager<List<Vendors>>("vendors");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Vendors>();
            }
            set
            {
                IValueManager<List<Vendors>> valueManager = ValueManager.GetValueManager<List<Vendors>>("vendors");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class ReportCommentVM
    {
        public string Comment
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Comment");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Comment");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class ReportManagement
    {
        public string Mode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Mode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Mode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DateTime ReportFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(ReportFilterByMonthDate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(ReportFilterByMonthDate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsCanRefresh
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsCanRefresh));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsCanRefresh));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class requisitionquerypanelinfo
    {
        //public static string strFilter = string.Empty;
        public string strQuoteID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strQuoteID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strQuoteID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string RequisitionFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RequisitionFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RequisitionFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> Items
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Items");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Items");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool fromStockAlert
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("fromStockAlert");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("fromStockAlert");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Items SelectedItem
        {
            get
            {
                IValueManager<Items> valueManager = ValueManager.GetValueManager<Items>("SelectedItem");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Items> valueManager = ValueManager.GetValueManager<Items>("SelectedItem");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public static string itemremaining = string.Empty;
        //public static string itemreceived = string.Empty;
    }

    public class consumptionquerypanelinfo
    {
        //public static string strFilter = string.Empty;
        public string ConsumptionFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ConsumptionFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ConsumptionFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string rgMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("consumptionrgMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("consumptionrgMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> Items
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Items");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Items");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class disposalquerypanelinfo
    {
        //public static string strFilter = string.Empty;
        public string DisposalFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("DisposalFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("DisposalFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string rgMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("disposalrgMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("disposalrgMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class Poquerypanelinfo
    {
        public string poquery
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("poquery");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("poquery");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string rgMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class Receivequerypanelinfo
    {
        public string receivequery
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("receivequery");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("receivequery");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string vendorid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("vendorid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("vendorid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ReceiveQueryFilter
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ReceiveQueryFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ReceiveQueryFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string rgMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }

    public class Existingstockquerypanelinfo
    {
        //public static string strFilter = string.Empty;
        public string ExistingstockFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ExistingstockFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ExistingstockFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> Esidlist
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Esidlist");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Esidlist");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ISnew
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ISnew");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ISnew");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string rgMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }



    public class Distributionquerypanelinfo
    {
        //public static string strFilter = string.Empty;
        public string DistributionFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("DistributionFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("DistributionFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool QueryFilter
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("QueryFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("QueryFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string sesitem
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("sesitem");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("sesitem");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string rgMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class ResultEntryQueryPanelInfo
    {
        // public static string strFilter = string.Empty;
        //public static CriteriaOperator CriteriaInfo = null;
        // public static string CurrentViewID = string.Empty;
        public ResultEntryQueryPanel ResultEntryCurrentobject
        {
            get
            {
                IValueManager<ResultEntryQueryPanel> valueManager = ValueManager.GetValueManager<ResultEntryQueryPanel>("ResultEntryCurrentobject");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<ResultEntryQueryPanel> valueManager = ValueManager.GetValueManager<ResultEntryQueryPanel>("ResultEntryCurrentobject");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ResultEntryCurrentselectionid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryCurrentselectionid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryCurrentselectionid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string CurrentViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryCurrentViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryCurrentViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static Guid slOid;
        //public static string strJobID = string.Empty;
        //public static string strMatrix = string.Empty;
        //public static string strTest = string.Empty;
        //public static string strMethod = string.Empty;
        //public static string strProjectID = string.Empty;
        //public static string strProjectName = string.Empty;
        //public static string strClientName = string.Empty;
        public string ResultEntryQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryQueryPanel");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryQueryPanel");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ResultEntryADCFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryADCFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryADCFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string QCResultEntryQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCResultEntryQueryFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCResultEntryQueryFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DateTime rgFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("ResultEntryrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("ResultEntryrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string rgMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstJobID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("RE_lstJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("RE_lstJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstQCJobID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("RE_lstQCJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("RE_lstQCJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstQCBatchID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("RE_lstQCBatchID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("RE_lstQCBatchID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string objJobID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("objJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }

            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("objJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string objQCBatchID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("objQCBatchID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("objQCBatchID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstABID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstABID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstABID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool FromDashboard
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ResultEntryFromDashboard");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ResultEntryFromDashboard");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool Fromloaddefault
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Fromloaddefault");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Fromloaddefault");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool FilterOpened
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("FilterOpened");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("FilterOpened");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool REViewMode
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("REViewMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("REViewMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string tempResultEntryQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("tempResultEntryQueryFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("tempResultEntryQueryFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Modules.BusinessObjects.SampleManagement.QueryMode SelectMode
        {
            get
            {
                IValueManager<Modules.BusinessObjects.SampleManagement.QueryMode> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.SampleManagement.QueryMode>(nameof(SelectMode));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return Modules.BusinessObjects.SampleManagement.QueryMode.Job;
            }
            set
            {
                IValueManager<Modules.BusinessObjects.SampleManagement.QueryMode> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.SampleManagement.QueryMode>(nameof(SelectMode));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsQueryPanelOpened
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsQueryPanelOpened");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsQueryPanelOpened");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SubOutTrackingQueryPanelInfo
    {
        // public static string strFilter = string.Empty;
        //public static CriteriaOperator CriteriaInfo = null;
        // public static string CurrentViewID = string.Empty;
        public string CurrentViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SubOutTrackingCurrentViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SubOutTrackingCurrentViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static Guid slOid;
        //public static string strJobID = string.Empty;
        //public static string strMatrix = string.Empty;
        //public static string strTest = string.Empty;
        //public static string strMethod = string.Empty;
        //public static string strProjectID = string.Empty;
        //public static string strProjectName = string.Empty;
        //public static string strClientName = string.Empty;
        public string SubOutTrackingQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SubOutTrackingQueryPanel");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SubOutTrackingQueryPanel");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DateTime rgFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("SubOutTrackingrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("SubOutTrackingrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string rgMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("rgMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class ReportingQueryPanelInfo
    {
        // public static string strFilter = string.Empty;
        //public static CriteriaOperator CriteriaInfo = null;
        //public static string CurrentViewID = string.Empty;
        public string CurrentViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ReportingCurrentViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ReportingCurrentViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ReportingQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ReportingQueryPanel");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ReportingQueryPanel");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }

        }

        public IList<Guid> lstreporting
        {
            get
            {
                IValueManager<IList<Guid>> valueManager = ValueManager.GetValueManager<IList<Guid>>("lstreporting");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Guid>> valueManager = ValueManager.GetValueManager<IList<Guid>>("lstreporting");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DateTime rgFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("ReportingrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("ReportingrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string rgFilterByJobID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ReportingrgFilterByJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ReportingrgFilterByJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstJobID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Rep_lstJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("Rep_lstJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Reporting curReport
        {
            get
            {
                IValueManager<Reporting> valueManager = ValueManager.GetValueManager<Reporting>("curReport");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Reporting> valueManager = ValueManager.GetValueManager<Reporting>("curReport");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int Revision
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("Revision");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("Revision");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string RevisionReason
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RevisionReason");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RevisionReason");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string PreviousReportID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("PreviousReportID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("PreviousReportID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class ProjectTrackingStatusFilter
    {
        public string ProjectTrackingStatus
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ProjectTrackingStatus");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ProjectTrackingStatus");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DateTime ptFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("QCResultValidationrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("QCResultValidationrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class QCResultValidationQueryPanelInfo
    {
        public string CurrentViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCResultValidationCurrentViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCResultValidationCurrentViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SampleResultValidationQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SampleResultValidationQueryFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SampleResultValidationQueryFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string QCResultValidationQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCResultValidationQueryFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCResultValidationQueryFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DateTime rgFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("QCResultValidationrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("QCResultValidationrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string CurrentTabViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCResultValidationCurrentTabViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCResultValidationCurrentTabViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstJobID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("RE_QC_lstJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("RE_QC_lstJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstQCBatchID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstQCBatchID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstQCBatchID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstSampleID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstSampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstSampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstSampleOid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSampleOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSampleOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public QueryMode SelectMode
        {
            get
            {
                IValueManager<QueryMode> valueManager = ValueManager.GetValueManager<QueryMode>(nameof(SelectMode));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return QueryMode.Job;
            }
            set
            {
                IValueManager<QueryMode> valueManager = ValueManager.GetValueManager<QueryMode>(nameof(SelectMode));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsQueryPanelOpened
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsQueryPanelOpened");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsQueryPanelOpened");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool boolSDMS
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolSDMSopen");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolSDMSopen");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class NavigationRefresh
    {
        public string Refresh
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Refresh");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Refresh");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ClickedNavigationItem
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClickedNavigationItem");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClickedNavigationItem");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SelectedNavigationItem
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SelectedNavigationItem");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SelectedNavigationItem");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class SampleLogInInfo
    {
        public List<SampleLogIn> lstSelectedObject
        {
            get
            {
                IValueManager<List<SampleLogIn>> valueManager = ValueManager.GetValueManager<List<SampleLogIn>>("lstSelectedObject");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<List<SampleLogIn>> valueManager = ValueManager.GetValueManager<List<SampleLogIn>>("lstSelectedObject");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string TestJobID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TestJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TestJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static string JobID = string.Empty;
        public string JobID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        // public static string SampleID = string.Empty;
        public string SampleID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLSampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLSampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static string focusedJobID = string.Empty;
        public string focusedJobID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLfocusedJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLfocusedJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        // public static bool boolCopySamples = false;
        public bool boolCopySamples
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolCopySamples");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolCopySamples");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public static string SLVisualMatrixName;
        public string SLVisualMatrixName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLVisualMatrixName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLVisualMatrixName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsColumnsCustomized
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsColumnsCustomized));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsColumnsCustomized));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static string SLOid = string.Empty;
        public string SLOid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static string Format = string.Empty;
        //public static string strQueryFilter = string.Empty;
        public string SLQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLQueryPanel");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLQueryPanel");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DateTime rgFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("SLQueryPanelrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("SLQueryPanelrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public SampleIDDigit SampleIDDigit
        {
            get
            {
                IValueManager<SampleIDDigit> valueManager = ValueManager.GetValueManager<SampleIDDigit>("SampleIDDigit");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return SampleIDDigit.Three;
            }
            set
            {
                IValueManager<SampleIDDigit> valueManager = ValueManager.GetValueManager<SampleIDDigit>("SampleIDDigit");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public IList<Testparameter> lstEditTest_TP
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstEditTest_TP");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstEditTest_TP");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SchedularInfo
    {
        public IList<TaskCheckList> lstTaskCheckList
        {
            get
            {
                IValueManager<IList<TaskCheckList>> valueManager = ValueManager.GetValueManager<IList<TaskCheckList>>("lstTaskCheckList");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<TaskCheckList>> valueManager = ValueManager.GetValueManager<IList<TaskCheckList>>("lstTaskCheckList");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> ChecklistOid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ChecklistOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ChecklistOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DateTime NextDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(NextDate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(NextDate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ViewClose
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ViewClose));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ViewClose));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class MessageTimer
    {
        public int Seconds
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("Seconds");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("Seconds");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class TrendInfo
    {
        public string actionname
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("actionname");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("actionname");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool Refresh
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Refresh");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Refresh");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<SampleParameter> lstSelectedObject
        {
            get
            {
                IValueManager<List<SampleParameter>> valueManager = ValueManager.GetValueManager<List<SampleParameter>>("lstSelectedObject");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<List<SampleParameter>> valueManager = ValueManager.GetValueManager<List<SampleParameter>>("lstSelectedObject");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class CopyNoOfSamplesPopUp
    {
        //public static int NoOfSamples = 0;
        public int NoOfSamples
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("NoOfSamples");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("NoOfSamples");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int NoOfCOCSamples
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("NoOfCOCSamples");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("NoOfCOCSamples");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        // public static bool Msgflag = false;
        public bool Msgflag
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Msgflag");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Msgflag");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isCopySamples
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isCopySamples");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isCopySamples");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class SampleCheckInInfo
    {
        //public static string JobID = string.Empty;
        public byte[] CocByteValue
        {
            get
            {
                IValueManager<byte[]> valueManager = ValueManager.GetValueManager<byte[]>("CocByteValue");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<byte[]> valueManager = ValueManager.GetValueManager<byte[]>("CocByteValue");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string JobID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static Guid Oid;
        //public static string SCVisualMatrixName;
        public string SCVisualMatrixName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCVisualMatrixName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCVisualMatrixName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        // public static string strQueryFilter = string.Empty;
        //public static bool bolGoToSampleLogin = false;
        public bool bolGoToSampleLogin
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("bolGoToSampleLogin");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("bolGoToSampleLogin");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SCQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCQueryPanel");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCQueryPanel");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DateTime rgFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("SCQueryPanelrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("SCQueryPanelrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstTestOid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class DynamicReportDesignerConnection
    {
        //public static string LDMSQLServerName = string.Empty;
        public string LDMSQLServerName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LDMSQLServerName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LDMSQLServerName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        // public static string LDMSQLDatabaseName = string.Empty;
        public string LDMSQLDatabaseName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LDMSQLDatabaseName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LDMSQLDatabaseName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static string LDMSQLUserID = string.Empty;
        public string LDMSQLUserID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LDMSQLUserID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LDMSQLUserID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        // public static string LDMSQLPassword = string.Empty;
        public string LDMSQLPassword
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LDMSQLPassword");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("LDMSQLPassword");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public static string WebConfigConn = string.Empty;
        public string WebConfigConn
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("WebConfigConn");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("WebConfigConn");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        // public static string WebConfigFTPConn = string.Empty;
        public string WebConfigFTPConn
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("WebConfigFTPConn");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("WebConfigFTPConn");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SC_SL_QueryPanelInfo
    {
        // public static  bool SCSLQueryPanelExecuted = false;

        public bool SCSLQueryPanelExecuted
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("SCSLQueryPanelExecuted");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("SCSLQueryPanelExecuted");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstJobID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class CopyPermissioninfo
    {
        // public static  bool SCSLQueryPanelExecuted = false;

        public CustomSystemUser SelectedUser
        {
            get
            {
                IValueManager<CustomSystemUser> valueManager = ValueManager.GetValueManager<CustomSystemUser>("CopyPermissioninfo");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<CustomSystemUser> valueManager = ValueManager.GetValueManager<CustomSystemUser>("CopyPermissioninfo");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstSelectedUser
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("CopyPermissioninfo_lstSelectedUser");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("CopyPermissioninfo_lstSelectedUser");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool BUpdated
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("BUpdated");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("BUpdated");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool DUpdated
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("DUpdated");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("DUpdated");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class ICMinfo
    {
        // public static  bool SCSLQueryPanelExecuted = false;

        public string RollBackReason
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RollBackReason");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RollBackReason");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string Vendor
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Vendor");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Vendor");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string ApproveFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ApproveFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ApproveFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> ObjectsToShow
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ObjectsToShow");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ObjectsToShow");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class LDMReportingVariables
    {
        public string strJobID = string.Empty;
        public string struqSampleParameterID = string.Empty;
        public string strLT = string.Empty;
        public string strlabwarebarcode = string.Empty;
        public string strLimsReportedDate = string.Empty;
        public string strReportID = string.Empty;
        public string strPOID = string.Empty;
        public string strQuoteID = string.Empty;
        public string stringQuoteID = string.Empty;
        public string strItem = string.Empty;
        public string strSampleID = string.Empty;
        public string strLTOid = string.Empty;
        public string strSuboutOrderID = string.Empty;
        public string SCCOid = string.Empty;
        public string BottlesOrderOid = string.Empty;
        public string strSampleParameterOid = string.Empty;
        public string strQcBatchID = string.Empty;
        public string strABID = string.Empty;
        public string strTestMethodID = string.Empty;
        public string strParameterID = string.Empty;
        public string strInvoiceID = string.Empty;
        public string strviewid = string.Empty;
        public string strDLQCID = string.Empty;
    }
    public class SampleRegistrationInfo
    {

        public List<Guid> lstSampleparameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(lstSampleparameter));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(lstSampleparameter));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string curPackageName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("curPackageName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("curPackageName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string curReportName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("curReportName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("curReportName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string Reportstr
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Reportstr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Reportstr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public byte[] bytevalues
        {
            get
            {
                IValueManager<byte[]> valueManager = ValueManager.GetValueManager<byte[]>("bytevalues");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<byte[]> valueManager = ValueManager.GetValueManager<byte[]>("bytevalues");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }


        public bool BoolReset
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("BoolReset");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("BoolReset");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int count
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("count");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("count");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstTestgridviewrow
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTestgridviewrow");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTestgridviewrow");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strtempNPTEST
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtempNPTEST");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strtempNPTEST");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsSamplePopupClose
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSamplePopup");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSamplePopup");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool Isbottleselectall
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Isbottleselectall");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Isbottleselectall");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DataTable dtSample
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtSample");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtSample");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strNPTest
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strNPTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strNPTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtTest
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<VisualMatrix> lstSRvisualmat
        {
            get
            {
                IValueManager<List<VisualMatrix>> valueManager = ValueManager.GetValueManager<List<VisualMatrix>>("lstSRvisualmat");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<VisualMatrix>();
            }
            set
            {
                IValueManager<List<VisualMatrix>> valueManager = ValueManager.GetValueManager<List<VisualMatrix>>("lstSRvisualmat");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isSampleloop
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isSampleloop");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isSampleloop");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsAttchedSubout
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsAttchedSubout");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsAttchedSubout");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isNoOfSampleDisable
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isNoOfSampleDisable");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isNoOfSampleDisable");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isVmatloop
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isVmatloop");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isVmatloop");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> selectionhideGuid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("selectionhideGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("selectionhideGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<DummyClass> lstviewselected
        {
            get
            {
                IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstviewselected");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<DummyClass>();
            }
            set
            {
                IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstviewselected");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<DummyClass> lstbottleid
        {
            get
            {
                IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<DummyClass>();
            }
            set
            {
                IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool Ispopup
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(Ispopup));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(Ispopup));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstcrtbottleid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrtbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrtbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstotherbottleid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstotherbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstotherbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstObjectsToShow
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ObjectsToShowSampleRegistration");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ObjectsToShowSampleRegistration");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strselSample
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strselSample");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strselSample");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public int counter
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("counter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("counter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool? CanProcess
        {
            get
            {
                IValueManager<bool?> valueManager = ValueManager.GetValueManager<bool?>("CanProcess");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool?> valueManager = ValueManager.GetValueManager<bool?>("CanProcess");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strJobID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strSuboutOrderID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSuboutOrderID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSuboutOrderID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid SamplingGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid SampleIDGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleIDGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleIDGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int SampleIDchangeindex
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("SampleIDchangeindex");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("SampleIDchangeindex");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstcopytosampleID
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstcopytosampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstcopytosampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstavailtest
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstavailtest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstavailtest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid lstsmplbtlalloGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("lstsmplbtlalloGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("lstsmplbtlalloGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public string strCOCID
        //{
        //    get
        //    {
        //        IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strCOCID");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return string.Empty;
        //    }
        //    set
        //    {
        //        IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strCOCID");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}

        public string strSampleID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool CanRefresh
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CanRefresh");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CanRefresh");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsTestAssignmentClosed
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestAssignmentClosed");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestAssignmentClosed");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool bolNewJobID
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("bolNewJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("bolNewJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strMatrix
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstTest
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid SampleOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstgroupTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstgroupTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstgroupTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstSubOutTest
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSubOutTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSubOutTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isConfirmedToUncheckSubout
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isConfirmedToUncheckSubout");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isConfirmedToUncheckSubout");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstHoldTest
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstHoldTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstHoldTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int Totparam
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("Totparam");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("Totparam");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstSelParameter
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstSelParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstSelParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstRemoveTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemoveTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemoveTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public object indexOfCurrentObject
        {
            get
            {
                IValueManager<object> valueManager = ValueManager.GetValueManager<object>("indexOfCurrentObject");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<object> valueManager = ValueManager.GetValueManager<object>("indexOfCurrentObject");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstOfUncheckedSubout
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstOfUncheckedSubout");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstOfUncheckedSubout");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstSavedTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSavedTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSavedTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstMethod
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstMethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstMethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstMatrix
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsBottleidvalid
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsBottleidvalid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsBottleidvalid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool UseSelchanged
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("UseSelchanged");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("UseSelchanged");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsTestcanFilter
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestcanFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestcanFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strSelectionMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSelectionMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSelectionMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstdupfilterstr
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterstr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterstr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstdupfilterSuboutstr
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterSuboutstr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterSuboutstr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstdupfilterHoldstr
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterHoldstr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterHoldstr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstdupfilterguid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdupfilterguid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdupfilterguid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public ViewEditMode ViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("ViewEditMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("ViewEditMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ImportToNewJob
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ImportToNewJob");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ImportToNewJob");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Samplecheckin CurrentJob
        {
            get
            {
                IValueManager<Samplecheckin> valueManager = ValueManager.GetValueManager<Samplecheckin>(nameof(CurrentJob));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Samplecheckin> valueManager = ValueManager.GetValueManager<Samplecheckin>(nameof(CurrentJob));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public TestMethod CurrentTest
        {
            get
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>(nameof(CurrentTest));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>(nameof(CurrentTest));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public SamplePrepBatch currentPrepbatchID
        {
            get
            {
                IValueManager<SamplePrepBatch> valueManager = ValueManager.GetValueManager<SamplePrepBatch>(nameof(currentPrepbatchID));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<SamplePrepBatch> valueManager = ValueManager.GetValueManager<SamplePrepBatch>(nameof(currentPrepbatchID));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Customer NewClient
        {
            get
            {
                IValueManager<Customer> valueManager = ValueManager.GetValueManager<Customer>(nameof(NewClient));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Customer> valueManager = ValueManager.GetValueManager<Customer>(nameof(NewClient));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Project NewProject
        {
            get
            {
                IValueManager<Project> valueManager = ValueManager.GetValueManager<Project>(nameof(NewProject));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Project> valueManager = ValueManager.GetValueManager<Project>(nameof(NewProject));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public InspectCategory NewInspectCategory
        {
            get
            {
                IValueManager<InspectCategory> valueManager = ValueManager.GetValueManager<InspectCategory>(nameof(NewInspectCategory));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<InspectCategory> valueManager = ValueManager.GetValueManager<InspectCategory>(nameof(NewInspectCategory));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public List<ClauseInspectionSettings> lstAddedClause
        //{
        //    get
        //    {
        //        IValueManager<List<ClauseInspectionSettings>> valueManager = ValueManager.GetValueManager<List<ClauseInspectionSettings>>(nameof(lstAddedClause));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<ClauseInspectionSettings>();
        //    }
        //    set
        //    {
        //        IValueManager<List<ClauseInspectionSettings>> valueManager = ValueManager.GetValueManager<List<ClauseInspectionSettings>>(nameof(lstAddedClause));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        //public List<ClauseInspectionSettings> lstUnlinkedClause
        //{
        //    get
        //    {
        //        IValueManager<List<ClauseInspectionSettings>> valueManager = ValueManager.GetValueManager<List<ClauseInspectionSettings>>(nameof(lstUnlinkedClause));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<ClauseInspectionSettings>();
        //    }
        //    set
        //    {
        //        IValueManager<List<ClauseInspectionSettings>> valueManager = ValueManager.GetValueManager<List<ClauseInspectionSettings>>(nameof(lstUnlinkedClause));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}

        public Frame ResultEntryFrame
        {
            get
            {
                IValueManager<Frame> valueManager = ValueManager.GetValueManager<Frame>(nameof(ResultEntryFrame));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Frame> valueManager = ValueManager.GetValueManager<Frame>(nameof(ResultEntryFrame));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid TaskOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(TaskOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(TaskOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public ViewEditMode TRViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("TRViewEditMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("TRViewEditMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstdummytests
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummytests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummytests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<SampleBottleAllocation> lstsmplbtlallo
        {
            get
            {
                IValueManager<List<SampleBottleAllocation>> valueManager = ValueManager.GetValueManager<List<SampleBottleAllocation>>("lstsmplbtlallo");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SampleBottleAllocation>();
            }
            set
            {
                IValueManager<List<SampleBottleAllocation>> valueManager = ValueManager.GetValueManager<List<SampleBottleAllocation>>("lstsmplbtlallo");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid visualmatrixGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("visualmatrixGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("visualmatrixGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid SelectedvisualmatrixGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SelectedvisualmatrixGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SelectedvisualmatrixGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstdummycreationtests
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummycreationtests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummycreationtests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstcrttests
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrttests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrttests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> EditColumnName
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool canGridRefresh
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("canGridRefresh");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("canGridRefresh");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsSampling
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSampling");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSampling");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public List<ClauseInspectionSettings> lstSavedClause
        //{
        //    get
        //    {
        //        IValueManager<List<ClauseInspectionSettings>> valueManager = ValueManager.GetValueManager<List<ClauseInspectionSettings>>(nameof(lstSavedClause));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<ClauseInspectionSettings>();
        //    }
        //    set
        //    {
        //        IValueManager<List<ClauseInspectionSettings>> valueManager = ValueManager.GetValueManager<List<ClauseInspectionSettings>>(nameof(lstSavedClause));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}

        //public List<ClauseInspectionSettings> lstRemovedClause
        //{
        //    get
        //    {
        //        IValueManager<List<ClauseInspectionSettings>> valueManager = ValueManager.GetValueManager<List<ClauseInspectionSettings>>(nameof(lstRemovedClause));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<ClauseInspectionSettings>();
        //    }
        //    set
        //    {
        //        IValueManager<List<ClauseInspectionSettings>> valueManager = ValueManager.GetValueManager<List<ClauseInspectionSettings>>(nameof(lstRemovedClause));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}

    }


    public class COCSettingsInfo
    {
        public bool isSampleloop
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isSampleloop");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isSampleloop");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isVmatloop
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isVmatloop");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isVmatloop");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstcopytosampleID
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstcopytosampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstcopytosampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstavailtest
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstavailtest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstavailtest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid lstsmplbtlalloGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("lstsmplbtlalloGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("lstsmplbtlalloGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstdummycreationtests
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummycreationtests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummycreationtests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstcrttests
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrttests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrttests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid COCOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(COCOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(COCOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strSampleID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool COCAssigned
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCAssigned));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCAssigned));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool SampleAssigned
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleAssigned));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleAssigned));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool TestAssigned
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TestAssigned));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TestAssigned));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }


        public bool IsTestcanFilter
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestcanFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestcanFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public List<GroupTest> lstBottleID
        //{
        //    get
        //    {
        //        IValueManager<List<GroupTest>> valueManager = ValueManager.GetValueManager<List<GroupTest>>("lstBottleID");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<GroupTest>();
        //    }
        //    set
        //    {
        //        IValueManager<List<GroupTest>> valueManager = ValueManager.GetValueManager<List<GroupTest>>("lstBottleID");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}

        public List<Guid> lstTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstdupfilterstr
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterstr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterstr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstdupfilterguid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdupfilterguid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdupfilterguid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstSavedTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSavedTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSavedTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid SampleOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strSelectionMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSelectionMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSelectionMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool UseSelchanged
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("UseSelchanged");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("UseSelchanged");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstRemovedTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemovedTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemovedTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstdummytests
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummytests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummytests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<SampleBottleAllocation> lstsmplbtlallo
        {
            get
            {
                IValueManager<List<SampleBottleAllocation>> valueManager = ValueManager.GetValueManager<List<SampleBottleAllocation>>("lstsmplbtlallo");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SampleBottleAllocation>();
            }
            set
            {
                IValueManager<List<SampleBottleAllocation>> valueManager = ValueManager.GetValueManager<List<SampleBottleAllocation>>("lstsmplbtlallo");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid visualmatrixGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("visualmatrixGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("visualmatrixGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstMatrix
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strcocID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcocID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcocID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> selectionhideGuid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("selectionhideGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("selectionhideGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public List<DummyClass> lstviewselected
        //{
        //    get
        //    {
        //        IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstviewselected");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<DummyClass>();
        //    }
        //    set
        //    {
        //        IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstviewselected");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public Guid COCGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public List<DummyClass> lstbottleid
        //{
        //    get
        //    {
        //        IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstbottleid");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<DummyClass>();
        //    }
        //    set
        //    {
        //        IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstbottleid");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public bool Ispopup
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(Ispopup));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(Ispopup));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstTest
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strMatrix
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstMethod
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstMethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstMethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid SamplingGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstcrtbottleid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrtbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrtbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstotherbottleid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstotherbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstotherbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsBottleidvalid
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsBottleidvalid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsBottleidvalid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strvisualmatrix
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strvisualmatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strvisualmatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class TestInfo
    {
        public bool IsCancel
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsCancel");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsCancel");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isgroup
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isgroup");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isgroup");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isTestsave
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isTestsave");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isTestsave");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<GroupTestMethod> delGtestmethod
        {
            get
            {
                IValueManager<List<GroupTestMethod>> valueManager = ValueManager.GetValueManager<List<GroupTestMethod>>("delGtestmethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<GroupTestMethod>();
            }
            set
            {
                IValueManager<List<GroupTestMethod>> valueManager = ValueManager.GetValueManager<List<GroupTestMethod>>("delGtestmethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> testmethodguid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("testmethodguid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("testmethodguid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public TestMethod objCurrentTest
        {
            get
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>(nameof(objCurrentTest));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>(nameof(objCurrentTest));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid methodguid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("methodguid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("methodguid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsTestMethodDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestMethodDelete");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestMethodDelete");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsCopyTestAction
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsCopyTestAction");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsCopyTestAction");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public IList<string> lstAvailableComponentParam
        {
            get
            {
                IValueManager<IList<string>> valueManager = ValueManager.GetValueManager<IList<string>>("lstAvailableComponentParam");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<string>> valueManager = ValueManager.GetValueManager<IList<string>>("lstAvailableComponentParam");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public TestMethod CurrentTest
        {
            get
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>("CurrentTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>("CurrentTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public QCType CurrentQcType
        {
            get
            {
                IValueManager<QCType> valueManager = ValueManager.GetValueManager<QCType>("CurrentQcType");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<QCType> valueManager = ValueManager.GetValueManager<QCType>("CurrentQcType");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public IList<QCType> ModifiedQCTypes
        {
            get
            {
                IValueManager<IList<QCType>> valueManager = ValueManager.GetValueManager<IList<QCType>>("ModifiedQCTypes");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<QCType>> valueManager = ValueManager.GetValueManager<IList<QCType>>("ModifiedQCTypes");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public IList<Testparameter> NewTestParameters
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("NewTestParameters");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("NewTestParameters");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public IList<Testparameter> RemovedTestParameters
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("RemovedTestParameters");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("RemovedTestParameters");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public IList<Testparameter> ExistingTestParameters
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("ExistingTestParameters");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("ExistingTestParameters");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public IList<Testparameter> lstInternalStandard
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstInternalStandard");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstInternalStandard");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public IList<Testparameter> lstRemovedInternalStandard
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstRemovedInternalStandard");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstRemovedInternalStandard");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public IList<Testparameter> lstSampleParameters
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstSampleParameters");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstSampleParameters");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public IList<Testparameter> lstRemovedSampleParameters
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstRemovedSampleParameters");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstRemovedSampleParameters");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public IList<Testparameter> lstQcParameters
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstQcParameters");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstQcParameters");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public IList<Testparameter> lstRemovedQcParameters
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstRemovedQcParameters");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstRemovedQcParameters");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public IList<Testparameter> lstSurrogates
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstSurrogates");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstSurrogates");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public IList<Testparameter> lstRemovedSurrogates
        {
            get
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstRemovedSurrogates");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Testparameter>> valueManager = ValueManager.GetValueManager<IList<Testparameter>>("lstRemovedSurrogates");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public IList<object> AllSelAvailableTestParam
        {
            get
            {
                IValueManager<IList<object>> valueManager = ValueManager.GetValueManager<IList<object>>("AllSelAvailableTestParam");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<object>> valueManager = ValueManager.GetValueManager<IList<object>>("AllSelAvailableTestParam");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsSaved
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSaved");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSaved");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool OpenSettings
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("OpenSettings");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("OpenSettings");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public bool ClearSearch
        //{
        //    get
        //    {
        //        IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ClearSearch");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return false;
        //    }
        //    set
        //    {
        //        IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ClearSearch");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}

        public bool CloseOnSave
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CloseOnSave");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CloseOnSave");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsNewTest
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNewTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNewTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class AssignItemsToTestInfo
    {
        public Guid CurrentTest
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(CurrentTest));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(CurrentTest));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> ItemsOid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(ItemsOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(ItemsOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }


    public class SequenceSetupInfo
    {
        public TestMethod CurrentTest
        {
            get
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>("CurrentTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<TestMethod> valueManager = ValueManager.GetValueManager<TestMethod>("CurrentTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }

    public class ScientificDataInfo
    {
        public static readonly string RawData = "RawDataTable";
        public static readonly string Calibration = "CalibrationTable";

    }

    public class Templateinfo
    {
        public int TempID
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("TempID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("TempID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string TempInfo
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TempInfo");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TempInfo");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<int> lstfieldname
        {
            get
            {
                IValueManager<List<int>> valueManager = ValueManager.GetValueManager<List<int>>("lstfieldname");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<int>();
            }
            set
            {
                IValueManager<List<int>> valueManager = ValueManager.GetValueManager<List<int>>("lstfieldname");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class Metrcinfo
    {
        public System.ComponentModel.BindingList<MetrcFacility> dtFacilitydatasource
        {
            get
            {
                IValueManager<System.ComponentModel.BindingList<MetrcFacility>> valueManager = ValueManager.GetValueManager<System.ComponentModel.BindingList<MetrcFacility>>("dtFacilitydatasource");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new System.ComponentModel.BindingList<MetrcFacility>();
            }
            set
            {
                IValueManager<System.ComponentModel.BindingList<MetrcFacility>> valueManager = ValueManager.GetValueManager<System.ComponentModel.BindingList<MetrcFacility>>("dtFacilitydatasource");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtincdatasource
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtincdatasource");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtincdatasource");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtincdetdatasource
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtincdetdatasource");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtincdetdatasource");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class Qcbatchinfo
    {
        public bool IsResetActionEnable
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsResetActionEnable");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsResetActionEnable");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsSortActionEnable
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSortActionEnable");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSortActionEnable");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lststrseqlayercount
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqlayercount");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqlayercount");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lststrseqdilutioncount
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqdilutioncount");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqdilutioncount");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lststrseqstringdilution
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqstringdilution");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqstringdilution");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lststrseqstringSampleAmount
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqstringSampleAmount");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqstringSampleAmount");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lststrseqstringFinalVolume
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqstringFinalVolume");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqstringFinalVolume");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool boolComment
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolComment");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolComment");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsPLMTest
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsPLMTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsPLMTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsMoldTest
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsMoldTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsMoldTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isview
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isview");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isview");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strcurlanguage
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcurlanguage");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcurlanguage");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }


        public int templateid
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("templateid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("templateid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public int qcstatus
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("qcstatus");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("qcstatus");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strqcid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strqcid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strqcid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strqcbatchid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strqcbatchid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strqcbatchid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid? QCBatchOid
        {
            get
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("QCBatchOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("QCBatchOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strDataTransfer
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strDataTransfer");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strDataTransfer");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strTest
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strAB
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strAB");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strAB");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strCB
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strCB");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strCB");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool canfilter
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("canfilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("canfilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool Isedit
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Isedit");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Isedit");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsSheetloaded
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSheetloaded");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSheetloaded");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid? OidTestMethod
        {
            get
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("OidTestMethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("OidTestMethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strTestMethodMatrixName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTestMethodMatrixName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTestMethodMatrixName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strTestMethodTestName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTestMethodTestName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTestMethodTestName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strTestMethodMethodNumber
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTestMethodMethodNumber");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTestMethodMethodNumber");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid strTestMethodMethodNumberOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("strTestMethodMethodNumberOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.Empty;
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("strTestMethodMethodNumberOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtTestdatasource
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtTestdatasource");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtTestdatasource");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtTemplateInfo
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtTemplateInfo");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtTemplateInfo");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DataTable dtMold
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtMold");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtMold");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtDataTransfer
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtDataTransfer");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtDataTransfer");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtselectedsamplefields
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtselectedsamplefields");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtselectedsamplefields");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtsample
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtsample");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtsample");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtHeader
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtHeader");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtHeader");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtDetail
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtDetail");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtDetail");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtCalibration
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtCalibration");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtCalibration");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtDataParsing
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtDataParsing");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtDataParsing");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DateTime qcFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(qcFilterByMonthDate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(qcFilterByMonthDate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public Modules.BusinessObjects.QC.QCBatch TempQCBatch
        //{
        //    get
        //    {
        //        IValueManager<Modules.BusinessObjects.QC.QCBatch> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.QC.QCBatch>(nameof(TempQCBatch));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return null;
        //    }
        //    set
        //    {
        //        IValueManager<Modules.BusinessObjects.QC.QCBatch> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.QC.QCBatch>(nameof(TempQCBatch));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}

        public bool IsAnalyticalBatch
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsAnalyticalBatch));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsAnalyticalBatch));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid? AnalyticalQCBatchOid
        {
            get
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("AnalyticalQCBatchOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("AnalyticalQCBatchOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }

    //public class IndoorInspectionInfo
    //{
    //    public IndoorInspection CurrentIndoorInspection
    //    {
    //        get
    //        {
    //            IValueManager<IndoorInspection> valueManager = ValueManager.GetValueManager<IndoorInspection>(nameof(CurrentIndoorInspection));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return null;
    //        }
    //        set
    //        {
    //            IValueManager<IndoorInspection> valueManager = ValueManager.GetValueManager<IndoorInspection>(nameof(CurrentIndoorInspection));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public DateTime indoorFilterByMonthDate
    //    {
    //        get
    //        {
    //            IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(indoorFilterByMonthDate));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return DateTime.Now;
    //        }
    //        set
    //        {
    //            IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(indoorFilterByMonthDate));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public OutdoorInspection CurrentOutdoorInspection
    //    {
    //        get
    //        {
    //            IValueManager<OutdoorInspection> valueManager = ValueManager.GetValueManager<OutdoorInspection>(nameof(CurrentOutdoorInspection));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return null;
    //        }
    //        set
    //        {
    //            IValueManager<OutdoorInspection> valueManager = ValueManager.GetValueManager<OutdoorInspection>(nameof(CurrentOutdoorInspection));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public DateTime outdoorFilterByMonthDate
    //    {
    //        get
    //        {
    //            IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(outdoorFilterByMonthDate));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return DateTime.Now;
    //        }
    //        set
    //        {
    //            IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(outdoorFilterByMonthDate));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public SampleUpload NewImageEdit
    //    {
    //        get
    //        {
    //            IValueManager<SampleUpload> valueManager = ValueManager.GetValueManager<SampleUpload>(nameof(NewImageEdit));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return null;
    //        }
    //        set
    //        {
    //            IValueManager<SampleUpload> valueManager = ValueManager.GetValueManager<SampleUpload>(nameof(NewImageEdit));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }

    //    public List<Guid> SavedClauseOptions
    //    {
    //        get
    //        {
    //            IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(SavedClauseOptions));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return new List<Guid>();
    //        }
    //        set
    //        {
    //            IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(SavedClauseOptions));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public List<ClauseOptions> SelectedClauseOptions
    //    {
    //        get
    //        {
    //            IValueManager<List<ClauseOptions>> valueManager = ValueManager.GetValueManager<List<ClauseOptions>>(nameof(SelectedClauseOptions));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return new List<ClauseOptions>();
    //        }
    //        set
    //        {
    //            IValueManager<List<ClauseOptions>> valueManager = ValueManager.GetValueManager<List<ClauseOptions>>(nameof(SelectedClauseOptions));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }

    //    public int PreviewImageWidth
    //    {
    //        get
    //        {
    //            IValueManager<int> valueManager = ValueManager.GetValueManager<int>(nameof(PreviewImageWidth));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return 0;
    //        }
    //        set
    //        {
    //            IValueManager<int> valueManager = ValueManager.GetValueManager<int>(nameof(PreviewImageWidth));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public int PreviewImageHeight
    //    {
    //        get
    //        {
    //            IValueManager<int> valueManager = ValueManager.GetValueManager<int>(nameof(PreviewImageHeight));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return 0;
    //        }
    //        set
    //        {
    //            IValueManager<int> valueManager = ValueManager.GetValueManager<int>(nameof(PreviewImageHeight));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public List<Guid> UnSavedComponents
    //    {
    //        get
    //        {
    //            IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(UnSavedComponents));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return new List<Guid>();
    //        }
    //        set
    //        {
    //            IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(UnSavedComponents));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //}

    public class ReportPackageInfo
    {
        public string PackageOldName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(PackageOldName));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(PackageOldName));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }


        public string CurrentPackageName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(CurrentPackageName));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(CurrentPackageName));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string NewPackageName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(NewPackageName));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(NewPackageName));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsNewPackage
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsNewPackage));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsNewPackage));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public ReportPackage ReportPackageOid
        {
            get
            {
                IValueManager<ReportPackage> valueManager = ValueManager.GetValueManager<ReportPackage>(nameof(ReportPackageOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<ReportPackage> valueManager = ValueManager.GetValueManager<ReportPackage>(nameof(ReportPackageOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }






    public class PermissionInfo
    {
        public bool IsSamplePrepInstruments
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsSamplePrepInstruments));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsSamplePrepInstruments));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool TaskBottleIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TaskBottleIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TaskBottleIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool TaskBottleIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TaskBottleIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TaskBottleIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool TaskBottleIsRead
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TaskBottleIsRead));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TaskBottleIsRead));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SampleBottleIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleBottleIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleBottleIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SampleBottleIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleBottleIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleBottleIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool InvoiceReviewIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(InvoiceReviewIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(InvoiceReviewIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }


        //public List<bool> InvoiceReviewIsWrite
        //{
        //    get
        //    {
        //        IValueManager<List<bool>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(InvoiceReviewIsWrite));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else
        //            return new List<bool>();
        //    }
        //    set
        //    {
        //        IValueManager<List<bool>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(InvoiceReviewIsWrite));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}


        public bool SampleBottleIsRead
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleBottleIsRead));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleBottleIsRead));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool COCBottleIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCBottleIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCBottleIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool COCBottleIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCBottleIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCBottleIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool COCBottleIsRead
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCBottleIsRead));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCBottleIsRead));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public List<RoleNavigationPermissionDetails> TypePermissionsDetails
        //{
        //    get
        //    {
        //        IValueManager<List<RoleNavigationPermissionDetails>> valueManager = ValueManager.GetValueManager<List<RoleNavigationPermissionDetails>>(nameof(TypePermissionsDetails));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<RoleNavigationPermissionDetails>();
        //    }
        //    set
        //    {
        //        IValueManager<List<RoleNavigationPermissionDetails>> valueManager = ValueManager.GetValueManager<List<RoleNavigationPermissionDetails>>(nameof(TypePermissionsDetails));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public List<Guid> LinkedTypePermissionsDetails
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(LinkedTypePermissionsDetails));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(LinkedTypePermissionsDetails));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> UnLinkedTypePermissionsDetails
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(UnLinkedTypePermissionsDetails));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(UnLinkedTypePermissionsDetails));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> LinkedNavigationItems
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(LinkedNavigationItems));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(LinkedNavigationItems));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> UnLinkedNavigationItems
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(UnLinkedNavigationItems));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(UnLinkedNavigationItems));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> DeletedPermissions
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(DeletedPermissions));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(DeletedPermissions));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<PermissionPolicyTypePermissionObject> TypeObjectPermissions
        {
            get
            {
                IValueManager<List<PermissionPolicyTypePermissionObject>> valueManager = ValueManager.GetValueManager<List<PermissionPolicyTypePermissionObject>>(nameof(TypeObjectPermissions));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<PermissionPolicyTypePermissionObject>();
            }
            set
            {
                IValueManager<List<PermissionPolicyTypePermissionObject>> valueManager = ValueManager.GetValueManager<List<PermissionPolicyTypePermissionObject>>(nameof(TypeObjectPermissions));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public ViewEditMode SampleRegistrationViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(SampleRegistrationViewEditMode));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(SampleRegistrationViewEditMode));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SampleRegIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleRegIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleRegIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public bool SamplingProposalIsWrite
        //{
        //    get
        //    {
        //        IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SamplingProposalIsWrite));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return false;
        //    }
        //    set
        //    {
        //        IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SamplingProposalIsWrite));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        //public ViewEditMode SamplingProposalViewEditMode
        //{
        //    get
        //    {
        //        IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(SamplingProposalViewEditMode));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return ViewEditMode.View;
        //    }
        //    set
        //    {
        //        IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(SamplingProposalViewEditMode));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public bool SampleRegIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleRegIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleRegIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public bool SamplingProposalIsCreate
        //{
        //    get
        //    {
        //        IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SamplingProposalIsCreate));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return false;
        //    }
        //    set
        //    {
        //        IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SamplingProposalIsCreate));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public bool RegistrationSignOffIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RegistrationSignOffIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RegistrationSignOffIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool SampleRegIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleRegIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleRegIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public bool SamplingProposalIsDelete
        //{
        //    get
        //    {
        //        IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SamplingProposalIsDelete));
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return false;
        //    }
        //    set
        //    {
        //        IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SamplingProposalIsDelete));
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public bool ResultEntryIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultEntryIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultEntryIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ResultEntryIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultEntryIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultEntryIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ResultViewIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultViewIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultViewIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ResultViewIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultViewIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultViewIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ResultValidationIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultValidationIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultValidationIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ResultValidationIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultValidationIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultValidationIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ResultApprovalIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultApprovalIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultApprovalIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ResultApprovalIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultApprovalIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultApprovalIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }


        public bool CustomReportingIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(CustomReportingIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(CustomReportingIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool CustomReportingIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(CustomReportingIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(CustomReportingIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ReportViewIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportViewIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportViewIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportViewIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportViewIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportViewIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportDeliveryIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportDeliveryIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportDeliveryIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportDeliveryIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportDeliveryIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportDeliveryIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportValidationIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportValidationIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportValidationIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportValidationIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportValidationIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportValidationIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportApprovalIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportApprovalIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportApprovalIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportApprovalIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportApprovalIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportApprovalIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportPrintIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportPrintIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportPrintIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ReportArchiveIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportArchiveIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportArchiveIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportPackageIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportPackageIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportPackageIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportPackageIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportPackageIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportPackageIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool TestInstrumentsIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TestInstrumentsIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(TestInstrumentsIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool RequisitionIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ICMStockqtyedit
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ICMStockqtyedit));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ICMStockqtyedit));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ICMImportFile
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ICMImportFile));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ICMImportFile));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool RequisitionIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool RequisitionIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool NavigationIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(NavigationIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(NavigationIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool NavigationIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(NavigationIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(NavigationIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool NavigationIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(NavigationIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(NavigationIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ConsumptionIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ConsumptionIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ConsumptionIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool DisposalIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(DisposalIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(DisposalIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool DistributionIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(DistributionIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(DistributionIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool RequisitionReviewIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionReviewIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionReviewIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool RequisitionApprovalIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionApprovalIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RequisitionApprovalIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReceivingItemsIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReceivingItemsIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReceivingItemsIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReceiveOrderDirectIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReceiveOrderDirectIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReceiveOrderDirectIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReceiveOrderDirectIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReceiveOrderDirectIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReceiveOrderDirectIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReceiveOrderDirectIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReceiveOrderDirectIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReceiveOrderDirectIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SDMSIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SDMSIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SDMSIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SDMSIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SDMSIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SDMSIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SDMSIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SDMSIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SDMSIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ExistingStockIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ExistingStockIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ExistingStockIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ExistingStockIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ExistingStockIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ExistingStockIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ExistingStockIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ExistingStockIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ExistingStockIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReportPackageIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportPackageIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReportPackageIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool AnalysisDepartmentChainIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(AnalysisDepartmentChainIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(AnalysisDepartmentChainIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SamplePreparationChainIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SamplePreparationChainIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SamplePreparationChainIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool RolesIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RolesIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(RolesIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SampleDispositionIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleDispositionIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleDispositionIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SampleDispositionIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleDispositionIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SampleDispositionIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IndoorInspectionIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IndoorInspectionIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IndoorInspectionIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IndoorInspectionIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IndoorInspectionIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IndoorInspectionIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public ViewEditMode IndoorInspectionViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(IndoorInspectionViewEditMode));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(IndoorInspectionViewEditMode));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool OutdoorInspectionIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(OutdoorInspectionIsWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(OutdoorInspectionIsWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool OutdoorInspectionIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(OutdoorInspectionIsDelete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(OutdoorInspectionIsDelete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public ViewEditMode OutdoorInspectionViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(OutdoorInspectionViewEditMode));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(OutdoorInspectionViewEditMode));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public ViewEditMode COCSettingsViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(OutdoorInspectionViewEditMode));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(OutdoorInspectionViewEditMode));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool COCSettingsIsCreate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCSettingsIsCreate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(COCSettingsIsCreate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool COCSettingsIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("COCSettingsIsDelete");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("COCSettingsIsDelete");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool COCSettingsIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("COCSettingsIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("COCSettingsIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ProductAndSamplingIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ProductAndSamplingIsDelete");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ProductAndSamplingIsDelete");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ProductAndSamplingIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ProductAndSamplingIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ProductAndSamplingIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public ViewEditMode TestsViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(TestsViewEditMode));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>(nameof(TestsViewEditMode));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool TestsIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("TestsIsDelete");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("TestsIsDelete");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool TestsIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("TestsIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("TestsIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ParameterDefaultsIsDelete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ParameterDefaultsIsDelete");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ParameterDefaultsIsDelete");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsAllowAllByDefault
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsAllowAllByDefault");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsAllowAllByDefault");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ParameterDefaultsIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ParameterDefaultsIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ParameterDefaultsIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool RawDataBatchLevel2ReviewIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("RawDataBatchLevel2ReviewIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("RawDataBatchLevel2ReviewIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool RawDataBatchLevel3ReviewIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("RawDataBatchLevel3ReviewIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("RawDataBatchLevel3ReviewIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ClientCallLogIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ClientCallLogIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ClientCallLogIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ProspectsCallLogIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ProspectsCallLogIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ProspectsCallLogIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool OpenQuotesIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("OpenQuotesIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("OpenQuotesIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool QuotesReviewIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("QuotesReviewIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("QuotesReviewIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool CancelledQuotesIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CancelledQuotesIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CancelledQuotesIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ExpiredQuotesIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ExpiredQuotesIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ExpiredQuotesIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool CanceledItemChargeIsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CanceledItemChargeIsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CanceledItemChargeIsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }


    public class AnalysisDepartmentChainInfo
    {
        public Guid CurrentTest
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(CurrentTest));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(CurrentTest));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> EmployeesOid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(EmployeesOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(EmployeesOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ResultValidation
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultValidation));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultValidation));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ResultApproval
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultApproval));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultApproval));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ResultEntry
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultEntry));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ResultEntry));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }

    public class ProductSampleMappingInfo
    {
        public Guid CurrentMappingOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(CurrentMappingOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(CurrentMappingOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ShowComplete
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ShowComplete));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ShowComplete));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DateTime productSampleFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(productSampleFilterByMonthDate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(productSampleFilterByMonthDate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class SampleWeighingBatchInfo
    {
        public string strWeighingBatchID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(strWeighingBatchID));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(strWeighingBatchID));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsSorted
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsSorted));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsSorted));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool canfilter
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(canfilter));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(canfilter));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid? OidTestMethod
        {
            get
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>(nameof(OidTestMethod));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>(nameof(OidTestMethod));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }

    public class NoteInfo
    {
        public string FormName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("FormName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("FormName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string Customer
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Customer");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Customer");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }


    public class NotificationInfo
    {
        public List<Guid> DismissedNotifications
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(DismissedNotifications));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(DismissedNotifications));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> DeletedNotifications
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(DeletedNotifications));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(DeletedNotifications));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }




    public class NavigationInfo
    {
        public string SelectedNavigationCaption
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SelectedNavigationCaption");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SelectedNavigationCaption");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class AuditInfo
    {

        public List<AuditData> Auditedlist
        {
            get
            {
                IValueManager<List<AuditData>> valueManager = ValueManager.GetValueManager<List<AuditData>>(nameof(Auditedlist));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<AuditData>();
            }
            set
            {
                IValueManager<List<AuditData>> valueManager = ValueManager.GetValueManager<List<AuditData>>(nameof(Auditedlist));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool? SaveData
        {
            get
            {
                IValueManager<bool?> valueManager = ValueManager.GetValueManager<bool?>(nameof(SaveData));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<bool?> valueManager = ValueManager.GetValueManager<bool?>(nameof(SaveData));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public SimpleAction action
        {
            get
            {
                IValueManager<SimpleAction> valueManager = ValueManager.GetValueManager<SimpleAction>(nameof(action));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<SimpleAction> valueManager = ValueManager.GetValueManager<SimpleAction>(nameof(action));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public SingleChoiceAction choiceaction
        {
            get
            {
                IValueManager<SingleChoiceAction> valueManager = ValueManager.GetValueManager<SingleChoiceAction>(nameof(choiceaction));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<SingleChoiceAction> valueManager = ValueManager.GetValueManager<SingleChoiceAction>(nameof(choiceaction));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public ChoiceActionItem choiceactionitem
        {
            get
            {
                IValueManager<ChoiceActionItem> valueManager = ValueManager.GetValueManager<ChoiceActionItem>(nameof(choiceactionitem));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<ChoiceActionItem> valueManager = ValueManager.GetValueManager<ChoiceActionItem>(nameof(choiceactionitem));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string comment
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(comment));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>(nameof(comment));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid? currentViewOid
        {
            get
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("currentViewOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("currentViewOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class SamplePrepBatchInfo
    {
        public string strcurlanguage
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcurlanguage");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcurlanguage");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strSamplePrepID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSamplePrepID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSamplePrepID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strTest
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool canfilter
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("canfilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("canfilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsEnbStat
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsEnbStat");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsEnbStat");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsNewPrepBatch
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNewPrepBatch");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNewPrepBatch");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string qctypeCriteria
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("qctypeCriteria");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty; ;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("qctypeCriteria");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid? OidTestMethod
        {
            get
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("OidTestMethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Guid?> valueManager = ValueManager.GetValueManager<Guid?>("OidTestMethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public SampleManagement.SamplePreparation.SamplePrepBatch CopyFromPrepBatchSource
        {
            get
            {
                IValueManager<SampleManagement.SamplePreparation.SamplePrepBatch> valueManager = ValueManager.GetValueManager<SampleManagement.SamplePreparation.SamplePrepBatch>("CopyFromPrepBatchSource");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<SampleManagement.SamplePreparation.SamplePrepBatch> valueManager = ValueManager.GetValueManager<SampleManagement.SamplePreparation.SamplePrepBatch>("CopyFromPrepBatchSource");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool CanAutopopulateSampleGrid
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CanAutopopulateSampleGrid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CanAutopopulateSampleGrid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool SamplepreparationWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("SamplepreparationWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("SamplepreparationWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lststrseqdilutioncount
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqdilutioncount");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lststrseqdilutioncount");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<SamplePrepBatchSequence> lstRemoveSampleSequence
        {
            get
            {
                IValueManager<List<SamplePrepBatchSequence>> valueManager = ValueManager.GetValueManager<List<SamplePrepBatchSequence>>("lstRemoveSampleSequence");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SamplePrepBatchSequence>();
            }
            set
            {
                IValueManager<List<SamplePrepBatchSequence>> valueManager = ValueManager.GetValueManager<List<SamplePrepBatchSequence>>("lstRemoveSampleSequence");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<SamplePrepBatchSequence> lstAddSampleSequence
        {
            get
            {
                IValueManager<List<SamplePrepBatchSequence>> valueManager = ValueManager.GetValueManager<List<SamplePrepBatchSequence>>("lstAddSampleSequence");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SamplePrepBatchSequence>();
            }
            set
            {
                IValueManager<List<SamplePrepBatchSequence>> valueManager = ValueManager.GetValueManager<List<SamplePrepBatchSequence>>("lstAddSampleSequence");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsDeletePreID
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsDeletePreID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsDeletePreID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class TestInstrumentClass
    {
        public List<Guid> lstExistingInstruments
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(lstExistingInstruments));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>(nameof(lstExistingInstruments));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class RollbackCC
    {
        // public static  bool SCSLQueryPanelExecuted = false;

        public string Rollback
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RollbackCC");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RollbackCC");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class PurchaseOrderInfo
    {
        public List<Requisition> SelectedPOItem
        {
            get
            {
                IValueManager<List<Requisition>> valueManager = ValueManager.GetValueManager<List<Requisition>>(nameof(SelectedPOItem));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Requisition>();
            }
            set
            {
                IValueManager<List<Requisition>> valueManager = ValueManager.GetValueManager<List<Requisition>>(nameof(SelectedPOItem));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsItemSelected
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsItemSelected");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsItemSelected");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Requisition> LinkedPOItems
        {
            get
            {
                IValueManager<List<Requisition>> valueManager = ValueManager.GetValueManager<List<Requisition>>(nameof(LinkedPOItems));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Requisition>();
            }
            set
            {
                IValueManager<List<Requisition>> valueManager = ValueManager.GetValueManager<List<Requisition>>(nameof(LinkedPOItems));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Requisition> UnLinkedPOItems
        {
            get
            {
                IValueManager<List<Requisition>> valueManager = ValueManager.GetValueManager<List<Requisition>>(nameof(UnLinkedPOItems));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Requisition>();
            }
            set
            {
                IValueManager<List<Requisition>> valueManager = ValueManager.GetValueManager<List<Requisition>>(nameof(UnLinkedPOItems));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class VendorReagentInfo
    {
        public Guid FileOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("FileOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.NewGuid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("FileOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class ICMRequisition
    {
        public IList<Guid> lstItemsOid
        {
            get
            {
                IValueManager<IList<Guid>> valueManager = ValueManager.GetValueManager<IList<Guid>>("lstItemsOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<Guid>> valueManager = ValueManager.GetValueManager<IList<Guid>>("lstItemsOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class DefaultSettingInfo
    {
        public bool boolReportValidation
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportValidation");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportValidation");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool boolReportApprove
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportApprove");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportApprove");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool boolReportPrintDownload
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportPrintDownload");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportPrintDownload");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool boolReportdelivery
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportdelivery");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportdelivery");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool boolReportArchive
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportArchive");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolReportArchive");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Modules.BusinessObjects.Setting.NavigationItem DefaultReportValidation
        {
            get
            {
                IValueManager<Modules.BusinessObjects.Setting.NavigationItem> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.Setting.NavigationItem>("DefaultReportValidation");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Modules.BusinessObjects.Setting.NavigationItem> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.Setting.NavigationItem>("DefaultReportValidation");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Modules.BusinessObjects.Setting.NavigationItem DefaultReportApprove
        {
            get
            {
                IValueManager<Modules.BusinessObjects.Setting.NavigationItem> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.Setting.NavigationItem>("DefaultReportApprove");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Modules.BusinessObjects.Setting.NavigationItem> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.Setting.NavigationItem>("DefaultReportApprove");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Modules.BusinessObjects.Setting.NavigationItem DefaultReportPrintDownload
        {
            get
            {
                IValueManager<Modules.BusinessObjects.Setting.NavigationItem> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.Setting.NavigationItem>("DefaultReportPrintDownload");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Modules.BusinessObjects.Setting.NavigationItem> valueManager = ValueManager.GetValueManager<Modules.BusinessObjects.Setting.NavigationItem>("DefaultReportPrintDownload");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string NavigationName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("NavigationName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("NavigationName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class AnalyticalBatchInfo
    {
        public string strabbatchid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strabbatchid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strabbatchid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DateTime analyticalFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(analyticalFilterByMonthDate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(analyticalFilterByMonthDate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strabid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strabid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strabid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool QCTypeIsLoaded
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("QCTypeIsLoaded");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("QCTypeIsLoaded");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool SeqIsLoaded
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("SeqIsLoaded");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("SeqIsLoaded");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class curlanguage
    {
        public string strcurlanguage
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcurlanguage");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcurlanguage");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class CustomReportBuilderInfo
    {
        public ReportDataV2 ReportDataOid
        {
            get
            {
                IValueManager<ReportDataV2> valueManager = ValueManager.GetValueManager<ReportDataV2>("ReportDataOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<ReportDataV2> valueManager = ValueManager.GetValueManager<ReportDataV2>("ReportDataOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strabid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strabid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strabid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool QCTypeIsLoaded
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("QCTypeIsLoaded");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("QCTypeIsLoaded");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool SeqIsLoaded
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("SeqIsLoaded");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("SeqIsLoaded");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class SuboutSampleInfo
    {
        public bool IsNewObject
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNewObject");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNewObject");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> EditColumnNameSample
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnNameSample");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnNameSample");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> EditColumnNameQCSample
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnNameQCSample");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnNameQCSample");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class BatchTestAssignmentInfo
    {
        public Matrix CurrentMatrix
        {
            get
            {
                IValueManager<Matrix> valueManager = ValueManager.GetValueManager<Matrix>("CurrentMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Matrix> valueManager = ValueManager.GetValueManager<Matrix>("CurrentMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class TaskManagementInfo
    {
        public DateTime taskFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(taskFilterByMonthDate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(taskFilterByMonthDate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string JobID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("JobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("JobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ClientName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClientName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ClientName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string TaskID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TaskID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TaskID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SamplingStatus
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SamplingStatus");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SamplingStatus");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string MSVisualMatrixName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("MSVisualMatrixName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("MSVisualMatrixName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstRemoveTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemoveTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemoveTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstSubOutTest
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSubOutTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSubOutTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstdupfilterstr
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterstr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterstr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstSavedTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSavedTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSavedTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstdupfilterguid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdupfilterguid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdupfilterguid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsTestcanFilter
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestcanFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestcanFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strSampleID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid SampleOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strSelectionMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSelectionMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSelectionMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool UseSelchanged
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("UseSelchanged");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("UseSelchanged");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool RefreshSampleProposal
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("RefreshSampleProposal");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("RefreshSampleProposal");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid TaskOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("TaskOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("TaskOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strSamplingSampleID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSamplingSampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSamplingSampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstdupfilterSuboutstr
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterSuboutstr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterSuboutstr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DateTime SamplingAssignmentDateFilter
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(SamplingAssignmentDateFilter));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(SamplingAssignmentDateFilter));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsColumnsCustomized
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsColumnsCustomized));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsColumnsCustomized));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsTestsColumnsCustomized
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsTestsColumnsCustomized));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsTestsColumnsCustomized));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsStationColumnsCustomized
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsStationColumnsCustomized));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsStationColumnsCustomized));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsFieldDataEntryStationColumnsCustomized
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsFieldDataEntryStationColumnsCustomized));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsFieldDataEntryStationColumnsCustomized));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsTRFiltered
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsTRFiltered));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsTRFiltered));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsSamplesFiltered
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsSamplesFiltered));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsSamplesFiltered));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsTestsFiltered
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsTestsFiltered));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsTestsFiltered));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid SamplingSampleOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingSampleOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingSampleOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string TaskRegistrationID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TaskRegistrationID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TaskRegistrationID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsBottleidvalid
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsBottleidvalid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsBottleidvalid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstTest
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstMatrix
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstMethod
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstMethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstMethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strMatrix
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> EditColumnName
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid MROid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(MROid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(MROid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid MRTaskOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(MRTaskOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(MRTaskOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsModifcationRequestNew
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsModifcationRequestNew");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsModifcationRequestNew");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsModifcationApprove
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsModifcationApprove");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsModifcationApprove");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstTestOid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string CurrentViewID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryCurrentViewID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ResultEntryCurrentViewID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

    public class ContractManagementInfo
    {
        public bool IsReviewed
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsReviewed");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsReviewed");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DateTime contractFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(contractFilterByMonthDate));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(contractFilterByMonthDate));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class FielddataEntryReviewInfo
    {
        public Nullable<Guid> TaskRegistrationOid
        {
            get
            {
                IValueManager<Nullable<Guid>> valueManager = ValueManager.GetValueManager<Nullable<Guid>>("TaskRegistrationOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Nullable<Guid>> valueManager = ValueManager.GetValueManager<Nullable<Guid>>("TaskRegistrationOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsFieldDataReview1StationColumnsCustomized
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsFieldDataReview1StationColumnsCustomized));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsFieldDataReview1StationColumnsCustomized));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsFieldDataReview2StationColumnsCustomized
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsFieldDataReview2StationColumnsCustomized));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsFieldDataReview2StationColumnsCustomized));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public ViewEditMode SampleTransferViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("SampleTransferViewEditMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("SampleTransferViewEditMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ReviewWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReviewWrite));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ReviewWrite));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SamplingCopyNoOfSamplesPopUp
    {
        //public static int NoOfSamples = 0;
        public int NoOfSamples
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("NoOfSamples");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("NoOfSamples");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        // public static bool Msgflag = false;
        public bool Msgflag
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Msgflag");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Msgflag");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SamplingInfo
    {
        public string SLVisualMatrixName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLVisualMatrixName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLVisualMatrixName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SLOid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string focusedJobID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLfocusedJobID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLfocusedJobID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SampleID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLSampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLSampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string RegistrationID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RegistrationID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RegistrationID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isSampleloop
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isSampleloop");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isSampleloop");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool boolCopySamples
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolCopySamples");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolCopySamples");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isVmatloop
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isVmatloop");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isVmatloop");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool Ispopup
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(Ispopup));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(Ispopup));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstcrtbottleid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrtbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrtbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstotherbottleid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstotherbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstotherbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> selectionhideGuid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("selectionhideGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("selectionhideGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public List<DummyClass> lstviewselected
        //{
        //    get
        //    {
        //        IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstviewselected");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<DummyClass>();
        //    }
        //    set
        //    {
        //        IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstviewselected");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        //public List<DummyClass> lstbottleid
        //{
        //    get
        //    {
        //        IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstbottleid");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return new List<DummyClass>();
        //    }
        //    set
        //    {
        //        IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstbottleid");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public string strbottleid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SamplingOid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SamplingOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SamplingOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid SamplingGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string FocusedSamplingOid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("FocusedSamplingOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("FocusedSamplingOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool TaskStatus
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("TaskStatus");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("TaskStatus");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstdummytests
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummytests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummytests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<SampleBottleAllocation> lstsmplbtlallo
        {
            get
            {
                IValueManager<List<SampleBottleAllocation>> valueManager = ValueManager.GetValueManager<List<SampleBottleAllocation>>("lstsmplbtlallo");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SampleBottleAllocation>();
            }
            set
            {
                IValueManager<List<SampleBottleAllocation>> valueManager = ValueManager.GetValueManager<List<SampleBottleAllocation>>("lstsmplbtlallo");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid visualmatrixGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("visualmatrixGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("visualmatrixGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstdummycreationtests
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummycreationtests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummycreationtests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstcrttests
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrttests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrttests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstcopytosampleID
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstcopytosampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstcopytosampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstavailtest
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstavailtest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstavailtest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid lstsmplbtlalloGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("lstsmplbtlalloGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("lstsmplbtlalloGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class SamplingFieldConfigurationInfo
    {
        public IList<VisualMatrix> lstMandatoryColumn
        {
            get
            {
                IValueManager<IList<VisualMatrix>> valueManager = ValueManager.GetValueManager<IList<VisualMatrix>>("lstMandatoryColumn");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<VisualMatrix>> valueManager = ValueManager.GetValueManager<IList<VisualMatrix>>("lstMandatoryColumn");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<SamplingFieldConfiguration> lstSamplingColumn
        {
            get
            {
                IValueManager<List<SamplingFieldConfiguration>> valueManager = ValueManager.GetValueManager<List<SamplingFieldConfiguration>>("lstSamplingColumn");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<List<SamplingFieldConfiguration>> valueManager = ValueManager.GetValueManager<List<SamplingFieldConfiguration>>("lstSamplingColumn");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<SamplingFieldConfiguration> lstStationColumn
        {
            get
            {
                IValueManager<List<SamplingFieldConfiguration>> valueManager = ValueManager.GetValueManager<List<SamplingFieldConfiguration>>("lstStationColumn");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<List<SamplingFieldConfiguration>> valueManager = ValueManager.GetValueManager<List<SamplingFieldConfiguration>>("lstStationColumn");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<SamplingFieldConfiguration> lstTestColumn
        {
            get
            {
                IValueManager<List<SamplingFieldConfiguration>> valueManager = ValueManager.GetValueManager<List<SamplingFieldConfiguration>>("lstTestColumn");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<List<SamplingFieldConfiguration>> valueManager = ValueManager.GetValueManager<List<SamplingFieldConfiguration>>("lstTestColumn");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class VersionControlInfo
    {
        public string VersionNumber
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("VersionNumber");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("VersionNumber");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class BottlesOrderInfo
    {
        public string Matrix
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Matrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Matrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string TestName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TestName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TestName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public class GroupTestPricingInfo
        {
            public Guid CurrentOid
            {
                get
                {
                    IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(CurrentOid));
                    if (valueManager.CanManageValue)
                        return valueManager.Value;
                    else return new Guid();
                }
                set
                {
                    IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(CurrentOid));
                    if (valueManager.CanManageValue)
                        valueManager.Value = value;
                }
            }
        }
        public class ConstituentInfo
        {
            public uint From
            {
                get
                {
                    IValueManager<uint> valueManager = ValueManager.GetValueManager<uint>("From");
                    if (valueManager.CanManageValue)
                        return valueManager.Value;
                    else return 0;
                }
                set
                {
                    IValueManager<uint> valueManager = ValueManager.GetValueManager<uint>("From");
                    if (valueManager.CanManageValue)
                        valueManager.Value = value;
                }
            }
        }

    }
    public class PLMInfo
    {
        public Dictionary<object, string> Materials
        {
            get
            {
                IValueManager<Dictionary<object, string>> valueManager = ValueManager.GetValueManager<Dictionary<object, string>>("Materials");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Dictionary<object, string>> valueManager = ValueManager.GetValueManager<Dictionary<object, string>>("Materials");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Dictionary<object, string> lstPLMSte
        {
            get
            {
                IValueManager<Dictionary<object, string>> valueManager = ValueManager.GetValueManager<Dictionary<object, string>>("lstPLMSte");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Dictionary<object, string>> valueManager = ValueManager.GetValueManager<Dictionary<object, string>>("lstPLMSte");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public PLMStereoscopicObservation LastPLMSte
        {
            get
            {
                IValueManager<PLMStereoscopicObservation> valueManager = ValueManager.GetValueManager<PLMStereoscopicObservation>("LastPLMSte");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<PLMStereoscopicObservation> valueManager = ValueManager.GetValueManager<PLMStereoscopicObservation>("LastPLMSte");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public PLMExam LastPLMEx
        {
            get
            {
                IValueManager<PLMExam> valueManager = ValueManager.GetValueManager<PLMExam>("LastPLMEx");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<PLMExam> valueManager = ValueManager.GetValueManager<PLMExam>("LastPLMEx");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public IList<FiberTypesValues> lstFiberTypesValues
        {
            get
            {
                IValueManager<IList<FiberTypesValues>> valueManager = ValueManager.GetValueManager<IList<FiberTypesValues>>("lstFiberTypesValues");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<FiberTypesValues>> valueManager = ValueManager.GetValueManager<IList<FiberTypesValues>>("lstFiberTypesValues");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class AnaliytialBatchinfo
    {
        public IList<QCBatchSequence> lstQCBatchSequence
        {
            get
            {
                IValueManager<IList<QCBatchSequence>> valueManager = ValueManager.GetValueManager<IList<QCBatchSequence>>("lstQCBatchSequence");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<IList<QCBatchSequence>> valueManager = ValueManager.GetValueManager<IList<QCBatchSequence>>("lstQCBatchSequence");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public SpreadSheetEntry_AnalyticalBatch lstSpreadSheetEntry_AnalyticalBatch
        {
            get
            {
                IValueManager<SpreadSheetEntry_AnalyticalBatch> valueManager = ValueManager.GetValueManager<SpreadSheetEntry_AnalyticalBatch>("lstQCBatch");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<SpreadSheetEntry_AnalyticalBatch> valueManager = ValueManager.GetValueManager<SpreadSheetEntry_AnalyticalBatch>("lstQCBatch");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DataTable dtQCdatatable
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtQCBatchdatasource");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtQCBatchdatasource");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Matrix CurrentMatrix
        {
            get
            {
                IValueManager<Matrix> valueManager = ValueManager.GetValueManager<Matrix>("CurrentMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Matrix> valueManager = ValueManager.GetValueManager<Matrix>("CurrentMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string Comments
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Comments");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Comments");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class PTStudyLogInfo
    {
        public Guid PTStudyLogGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("PTStudyLogGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("PTStudyLogGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstexistTestparamOid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstexistTestparamOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstexistTestparamOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class HelpCenterInfo
    {
        public string strcrtTheme
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcrtTheme");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strcrtTheme");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool manualFAQ
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("manualFAQ");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("manualFAQ");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SelectedCategory
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SelectedCategory");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SelectedCategory");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int SelectedIndex
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("SelectedIndex");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("SelectedIndex");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string StrTopic
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("StrTopic");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("StrTopic");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string StrDownloadTopic
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("StrDownloadTopic");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("StrDownloadTopic");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class DataPackageInfo
    {
        public bool EntryValidate
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("EntryValidate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("EntryValidate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SelectedStatus
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SelectedStatus");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SelectedStatus");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int SelectedIndex
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("SelectedIndex");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("SelectedIndex");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    //public class SchedularInfo
    //{
    //    public IList<TaskCheckList> lstTaskCheckList
    //    {
    //        get
    //        {
    //            IValueManager<IList<TaskCheckList>> valueManager = ValueManager.GetValueManager<IList<TaskCheckList>>("lstTaskCheckList");
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return null;
    //        }
    //        set
    //        {
    //            IValueManager<IList<TaskCheckList>> valueManager = ValueManager.GetValueManager<IList<TaskCheckList>>("lstTaskCheckList");
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public List<Guid> ChecklistOid
    //    {
    //        get
    //        {
    //            IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ChecklistOid");
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return new List<Guid>();
    //        }
    //        set
    //        {
    //            IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ChecklistOid");
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public DateTime NextDate
    //    {
    //        get
    //        {
    //            IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(NextDate));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return DateTime.Now;
    //        }
    //        set
    //        {
    //            IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>(nameof(NextDate));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //    public bool ViewClose
    //    {
    //        get
    //        {
    //            IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ViewClose));
    //            if (valueManager.CanManageValue)
    //                return valueManager.Value;
    //            else return false;
    //        }
    //        set
    //        {
    //            IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(ViewClose));
    //            if (valueManager.CanManageValue)
    //                valueManager.Value = value;
    //        }
    //    }
    //}

    public class EDDInfo
    {
        public bool IsViewClose
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsViewClose));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsViewClose));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable QueryData
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("QueryData");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("QueryData");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DataTable dtsample
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtsample");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtsample");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid EddBuildOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("EddBuildOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.Empty;
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("EddBuildOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public uint EDDBuilderQuerycount
        {
            get
            {
                IValueManager<uint> valueManager = ValueManager.GetValueManager<uint>("EDDBuilderQuerycount");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<uint> valueManager = ValueManager.GetValueManager<uint>("EDDBuilderQuerycount");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable EDDDataSource
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("EDDDataSource");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("EDDDataSource");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataSet EDDDataSet
        {
            get
            {
                IValueManager<DataSet> valueManager = ValueManager.GetValueManager<DataSet>("EDDDataSet");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new DataSet();
            }
            set
            {
                IValueManager<DataSet> valueManager = ValueManager.GetValueManager<DataSet>("EDDDataSet");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public EDDReportGenerator EDDRptGtr
        {
            get
            {
                IValueManager<EDDReportGenerator> valueManager = ValueManager.GetValueManager<EDDReportGenerator>("EDDRptGtr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<EDDReportGenerator> valueManager = ValueManager.GetValueManager<EDDReportGenerator>("EDDRptGtr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public EDDReportGenerator EDDID
        {
            get
            {
                IValueManager<EDDReportGenerator> valueManager = ValueManager.GetValueManager<EDDReportGenerator>("EDDID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<EDDReportGenerator> valueManager = ValueManager.GetValueManager<EDDReportGenerator>("EDDID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class CalibrationLogbookInfo
    {
        public bool SaveAs
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SaveAs));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(SaveAs));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string BuilderSettings
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("BuilderSettings");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("BuilderSettings");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string InstrumentID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("InstrumentID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("InstrumentID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string Analyst
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Analyst");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Analyst");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public NotebookBuilder NotebookBuilderOid
        //{
        //    get
        //    {
        //        IValueManager<NotebookBuilder> valueManager = ValueManager.GetValueManager<NotebookBuilder>("NotebookBuilderOid");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else return null;
        //    }
        //    set
        //    {
        //        IValueManager<NotebookBuilder> valueManager = ValueManager.GetValueManager<NotebookBuilder>("NotebookBuilderOid");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
    }
    public class CustomLoginInfo
    {
        public string strMessage
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMessage");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMessage");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class InvoiceInfo
    {
        public Guid InvoicePopupPriorityOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("InvoicePopupPriorityOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.Empty;
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("InvoicePopupPriorityOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid InvoicePopupTATOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("InvoicePopupTATOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.Empty;
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("InvoicePopupTATOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid InvoicePopupCrtAnalysispriceOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("InvoicePopupCrtAnalysispriceOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.Empty;
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("InvoicePopupCrtAnalysispriceOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<InvoicingItemCharge> lsttempItemPricing
        {
            get
            {
                IValueManager<List<InvoicingItemCharge>> valueManager = ValueManager.GetValueManager<List<InvoicingItemCharge>>("lsttempItemPricing");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<InvoicingItemCharge>();
            }
            set
            {
                IValueManager<List<InvoicingItemCharge>> valueManager = ValueManager.GetValueManager<List<InvoicingItemCharge>>("lsttempItemPricing");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<ItemChargePricing> lsttempItemPricingpopup
        {
            get
            {
                IValueManager<List<ItemChargePricing>> valueManager = ValueManager.GetValueManager<List<ItemChargePricing>>("lsttempItemPricingpopup");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<ItemChargePricing>();
            }
            set
            {
                IValueManager<List<ItemChargePricing>> valueManager = ValueManager.GetValueManager<List<ItemChargePricing>>("lsttempItemPricingpopup");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public decimal lvDetailedPrice
        {
            get
            {
                IValueManager<decimal> valueManager = ValueManager.GetValueManager<decimal>("lvDetailedPrice");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<decimal> valueManager = ValueManager.GetValueManager<decimal>("lvDetailedPrice");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Invoicing popupcurtinvoice
        {
            get
            {
                IValueManager<Invoicing> valueManager = ValueManager.GetValueManager<Invoicing>("popupcurtinvoice");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Invoicing> valueManager = ValueManager.GetValueManager<Invoicing>("popupcurtinvoice");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsDataLoaded
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsDataLoaded));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsDataLoaded));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lsttempparamsoid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lsttempparamsoid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lsttempparamsoid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsItemchargePricingpopupselectall
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsItemchargePricingpopupselectall");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsItemchargePricingpopupselectall");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
   
    public class StockQtyEditInfo
    {
        public bool ItemIsPopulated
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ItemIsPopulated");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ItemIsPopulated");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }

    public class QCBatchViewMode
    {
        public bool IsViewMode
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsViewMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsViewMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class ProductInfo
    {
        //public List<Guid> AvailElements
        //{
        //    get
        //    {
        //        IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("AvailElements");
        //        if (valueManager.CanManageValue)
        //            return valueManager.Value;
        //        else
        //            return new List<Guid>();
        //    }
        //    set
        //    {
        //        IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("AvailElements");
        //        if (valueManager.CanManageValue)
        //            valueManager.Value = value;
        //    }
        //}
        public List<Parameter> Elements
        {
            get
            {
                IValueManager<List<Parameter>> valueManager = ValueManager.GetValueManager<List<Parameter>>("Elements");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<Parameter>();
            }
            set
            {
                IValueManager<List<Parameter>> valueManager = ValueManager.GetValueManager<List<Parameter>>("Elements");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Parameter> lstRemovedElements
        {
            get
            {
                IValueManager<List<Parameter>> valueManager = ValueManager.GetValueManager<List<Parameter>>("lstRemovedElements");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<Parameter>();
            }
            set
            {
                IValueManager<List<Parameter>> valueManager = ValueManager.GetValueManager<List<Parameter>>("lstRemovedElements");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Parameter> lstTempRemovedElements
        {
            get
            {
                IValueManager<List<Parameter>> valueManager = ValueManager.GetValueManager<List<Parameter>>("lstTempRemovedElements");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<Parameter>();
            }
            set
            {
                IValueManager<List<Parameter>> valueManager = ValueManager.GetValueManager<List<Parameter>>("lstTempRemovedElements");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstAvailableComponents
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstAvailableComponents");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstAvailableComponents");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        ////public List<NPDynamicClass> lstConcentrations
        ////{
        ////    get
        ////    {
        ////        IValueManager<List<NPDynamicClass>> valueManager = ValueManager.GetValueManager<List<NPDynamicClass>>("lstConcentrations");
        ////        if (valueManager.CanManageValue)
        ////            return valueManager.Value;
        ////        else
        ////            return new List<NPDynamicClass>();
        ////    }
        ////    set
        ////    {
        ////        IValueManager<List<NPDynamicClass>> valueManager = ValueManager.GetValueManager<List<NPDynamicClass>>("lstConcentrations");
        ////        if (valueManager.CanManageValue)
        ////            valueManager.Value = value;
        ////    }
        ////}
        public bool IsNew
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNew");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsNew");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsSelected
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSelected");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSelected");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsDisabled
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsDisabled");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsDisabled");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ConcIsFiltered
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ConcIsFiltered");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ConcIsFiltered");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool DetailViewConcIsFiltered
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("DetailViewConcIsFiltered");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("DetailViewConcIsFiltered");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool ElementIsFiltered
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ElementIsFiltered");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ElementIsFiltered");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int NoOfConc
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("NoOfConc");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("NoOfConc");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string Unit
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Unit");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("Unit");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool HideColumns
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("HideColumns");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("HideColumns");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class ResultEntrySelectionInfo
    {
        public bool IsResultEntrySelectionChanged
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsResultEntrySelectionChanged");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsResultEntrySelectionChanged");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<SampleParameter> lstresultentry
        {
            get
            {
                IValueManager<List<SampleParameter>> valueManager = ValueManager.GetValueManager<List<SampleParameter>>("lstresultentry");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SampleParameter>();
            }
            set
            {
                IValueManager<List<SampleParameter>> valueManager = ValueManager.GetValueManager<List<SampleParameter>>("lstresultentry");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<SampleParameter> lsttempresultentry
        {
            get
            {
                IValueManager<List<SampleParameter>> valueManager = ValueManager.GetValueManager<List<SampleParameter>>("lsttempresultentry");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<SampleParameter>();
            }
            set
            {
                IValueManager<List<SampleParameter>> valueManager = ValueManager.GetValueManager<List<SampleParameter>>("lsttempresultentry");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class AnalysisDeptUser
    {
        public List<Employee> lstAnalysisEmp
        {
            get
            {
                IValueManager<List<Employee>> valueManager = ValueManager.GetValueManager<List<Employee>>("lstAnalysisEmp");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Employee>();
            }
            set
            {
                IValueManager<List<Employee>> valueManager = ValueManager.GetValueManager<List<Employee>>("lstAnalysisEmp");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class ResultRollback
    {
        public string StrResultRollback
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("StrResultRollback");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("StrResultRollback");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class COCSettingsSampleInfo
    {
        public string TestCOCID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TestCOCID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("TestCOCID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static string JobID = string.Empty;
        public string COCID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLCOCID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SLCOCID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        // public static string SampleID = string.Empty;
        public string SampleID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static string focusedJobID = string.Empty;
        public string focusedCOCID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCfocusedCOCID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCfocusedCOCID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        // public static bool boolCopySamples = false;
        public bool boolCopySamples
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolCopySamples");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("boolCopySamples");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        //public static string SLVisualMatrixName;
        public string COCVisualMatrixName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCVisualMatrixName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCVisualMatrixName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsColumnsCustomized
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsColumnsCustomized));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(IsColumnsCustomized));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static string SLOid = string.Empty;
        public string COCOid
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        //public static string Format = string.Empty;
        //public static string strQueryFilter = string.Empty;
        public string COCQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCQueryPanel");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCQueryPanel");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DateTime rgFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("COCQueryPanelrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("COCQueryPanelrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public SampleIDDigit SampleIDDigit
        {
            get
            {
                IValueManager<SampleIDDigit> valueManager = ValueManager.GetValueManager<SampleIDDigit>("SampleIDDigit");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return SampleIDDigit.Three;
            }
            set
            {
                IValueManager<SampleIDDigit> valueManager = ValueManager.GetValueManager<SampleIDDigit>("SampleIDDigit");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class COCSettingsSampleCheckInInfo
    {
        //public static string JobID = string.Empty;
        public string COCID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCCOCID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCCOCID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string COCVisualMatrixName
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCVisualMatrixName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCVisualMatrixName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool bolGoToCOCSettingsSample
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("bolGoToCOCSettingsSample");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("bolGoToCOCSettingsSample");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string COCQueryFilter
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCQueryPanel");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("COCQueryPanel");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DateTime rgFilterByMonthDate
        {
            get
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("COCQueryPanelrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return DateTime.Now;
            }
            set
            {
                IValueManager<DateTime> valueManager = ValueManager.GetValueManager<DateTime>("COCQueryPanelrgFilterByMonthDate");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstTestOid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class COCSettingsRegistrationInfo
    {
        public bool IsSamplePopupClose
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSamplePopup");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsSamplePopup");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool Isbottleselectall
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Isbottleselectall");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("Isbottleselectall");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DataTable dtSample
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtSample");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtSample");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string strNPTest
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strNPTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strNPTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public DataTable dtTest
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<VisualMatrix> lstCOCvisualmat
        {
            get
            {
                IValueManager<List<VisualMatrix>> valueManager = ValueManager.GetValueManager<List<VisualMatrix>>("lstCOCvisualmat");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<VisualMatrix>();
            }
            set
            {
                IValueManager<List<VisualMatrix>> valueManager = ValueManager.GetValueManager<List<VisualMatrix>>("lstCOCvisualmat");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isSampleloop
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isSampleloop");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isSampleloop");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isNoOfSampleDisable
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isNoOfSampleDisable");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isNoOfSampleDisable");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool isVmatloop
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isVmatloop");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("isVmatloop");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> selectionhideGuid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("selectionhideGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("selectionhideGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<DummyClass> lstviewselected
        {
            get
            {
                IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstviewselected");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<DummyClass>();
            }
            set
            {
                IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstviewselected");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<DummyClass> lstbottleid
        {
            get
            {
                IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<DummyClass>();
            }
            set
            {
                IValueManager<List<DummyClass>> valueManager = ValueManager.GetValueManager<List<DummyClass>>("lstbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool Ispopup
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(Ispopup));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>(nameof(Ispopup));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstcrtbottleid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrtbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrtbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstotherbottleid
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstotherbottleid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstotherbottleid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstObjectsToShow
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ObjectsToShowSampleRegistration");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("ObjectsToShowSampleRegistration");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strselSample
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strselSample");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strselSample");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public int counter
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("counter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("counter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool? CanProcess
        {
            get
            {
                IValueManager<bool?> valueManager = ValueManager.GetValueManager<bool?>("CanProcess");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool?> valueManager = ValueManager.GetValueManager<bool?>("CanProcess");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strCOCID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strCOCID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strCOCID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strSuboutOrderID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSuboutOrderID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSuboutOrderID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid SamplingGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SamplingGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid SampleIDGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleIDGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleIDGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public int SampleIDchangeindex
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("SampleIDchangeindex");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("SampleIDchangeindex");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstcopytosampleID
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstcopytosampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstcopytosampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstavailtest
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstavailtest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstavailtest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid lstsmplbtlalloGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("lstsmplbtlalloGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("lstsmplbtlalloGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strSampleID
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSampleID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSampleID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool CanRefresh
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CanRefresh");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CanRefresh");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsTestAssignmentClosed
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestAssignmentClosed");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestAssignmentClosed");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool bolNewCOCID
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("bolNewCOCID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("bolNewCOCID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strMatrix
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstTest
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Guid SampleOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleOid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SampleOid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstgroupTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstgroupTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstgroupTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstSubOutTest
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSubOutTest");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSubOutTest");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public int Totparam
        {
            get
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("Totparam");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return 0;
            }
            set
            {
                IValueManager<int> valueManager = ValueManager.GetValueManager<int>("Totparam");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstSelParameter
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstSelParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstSelParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstRemoveTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemoveTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstRemoveTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstSavedTestParameter
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSavedTestParameter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstSavedTestParameter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstMethod
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstMethod");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstMethod");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstMatrix
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstMatrix");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstMatrix");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsBottleidvalid
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsBottleidvalid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsBottleidvalid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool UseSelchanged
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("UseSelchanged");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("UseSelchanged");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool IsTestcanFilter
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestcanFilter");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return false;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsTestcanFilter");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public string strSelectionMode
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSelectionMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("strSelectionMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<string> lstdupfilterstr
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterstr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterstr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstdupfilterSuboutstr
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterSuboutstr");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstdupfilterSuboutstr");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public List<Guid> lstdupfilterguid
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdupfilterguid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdupfilterguid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public ViewEditMode ViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("ViewEditMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("ViewEditMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public bool ImportToNewJCOC
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ImportToNewJCOC");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ImportToNewJCOC");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public COCSettings CurrentCOC
        {
            get
            {
                IValueManager<COCSettings> valueManager = ValueManager.GetValueManager<COCSettings>(nameof(CurrentCOC));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<COCSettings> valueManager = ValueManager.GetValueManager<COCSettings>(nameof(CurrentCOC));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Customer NewClient
        {
            get
            {
                IValueManager<Customer> valueManager = ValueManager.GetValueManager<Customer>(nameof(NewClient));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Customer> valueManager = ValueManager.GetValueManager<Customer>(nameof(NewClient));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public Project NewProject
        {
            get
            {
                IValueManager<Project> valueManager = ValueManager.GetValueManager<Project>(nameof(NewProject));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Project> valueManager = ValueManager.GetValueManager<Project>(nameof(NewProject));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public InspectCategory NewInspectCategory
        {
            get
            {
                IValueManager<InspectCategory> valueManager = ValueManager.GetValueManager<InspectCategory>(nameof(NewInspectCategory));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<InspectCategory> valueManager = ValueManager.GetValueManager<InspectCategory>(nameof(NewInspectCategory));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }


        public Frame ResultEntryFrame
        {
            get
            {
                IValueManager<Frame> valueManager = ValueManager.GetValueManager<Frame>(nameof(ResultEntryFrame));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<Frame> valueManager = ValueManager.GetValueManager<Frame>(nameof(ResultEntryFrame));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid TaskOid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(TaskOid));
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>(nameof(TaskOid));
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

        public ViewEditMode TRViewEditMode
        {
            get
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("TRViewEditMode");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return ViewEditMode.View;
            }
            set
            {
                IValueManager<ViewEditMode> valueManager = ValueManager.GetValueManager<ViewEditMode>("TRViewEditMode");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstdummytests
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummytests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummytests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<COCSettingsBottleAllocation> lstsmplbtlallo
        {
            get
            {
                IValueManager<List<COCSettingsBottleAllocation>> valueManager = ValueManager.GetValueManager<List<COCSettingsBottleAllocation>>("lstsmplbtlallo");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<COCSettingsBottleAllocation>();
            }
            set
            {
                IValueManager<List<COCSettingsBottleAllocation>> valueManager = ValueManager.GetValueManager<List<COCSettingsBottleAllocation>>("lstsmplbtlallo");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid visualmatrixGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("visualmatrixGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("visualmatrixGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid SelectedvisualmatrixGuid
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SelectedvisualmatrixGuid");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new Guid();
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SelectedvisualmatrixGuid");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<Guid> lstdummycreationtests
        {
            get
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummycreationtests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<Guid>();
            }
            set
            {
                IValueManager<List<Guid>> valueManager = ValueManager.GetValueManager<List<Guid>>("lstdummycreationtests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> lstcrttests
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrttests");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstcrttests");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public List<string> EditColumnName
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnName");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else
                    return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("EditColumnName");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool canGridRefresh
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("canGridRefresh");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("canGridRefresh");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class DOCInfo
    {
        public bool ViewClose
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ViewClose");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ViewClose");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public bool IsWrite
        {
            get
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsWrite");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set
            {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("IsWrite");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
    public class CaseNarativeInfo
    {
        public string SCJobId
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCJobId");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCJobId");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SCSampleMatries
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCSampleMatries");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SCSampleMatries");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SPJobId
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SPJobId");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SPJobId");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SPSampleMatries
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SPSampleMatries");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SPSampleMatries");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string REQCBatchId
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("REQCBatchId");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("REQCBatchId");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string RESampleMatries
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RESampleMatries");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RESampleMatries");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SDMSJobId
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SDMSJobId");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SDMSJobId");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string SDMSSampleMatries
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SDMSSampleMatries");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("SDMSSampleMatries");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public Guid SCoidValue
        {
            get
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SCoidValue");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return Guid.Empty; // Return empty Guid if value cannot be managed
            }
            set
            {
                IValueManager<Guid> valueManager = ValueManager.GetValueManager<Guid>("SCoidValue");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string QCJobId
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCJobId");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCJobId");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string QCSampleMatries
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCSampleMatries");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("QCSampleMatries");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string RpJobId
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RpJobId");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RpJobId");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string RpSampleMatries
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RpSampleMatries");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("RpSampleMatries");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }

    }
    public class ReagentPreparationInfo
    {
        public List<string> lstEditorID
        {
            get
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstEditorID");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new List<string>();
            }
            set
            {
                IValueManager<List<string>> valueManager = ValueManager.GetValueManager<List<string>>("lstEditorID");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public string ActiveTabText
        {
            get
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ActiveTabText");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return string.Empty;
            }
            set
            {
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ActiveTabText");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DataTable dtReagentPrepLog
        {
            get
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtReagentPrepLog");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return new DataTable();
            }
            set
            {
                IValueManager<DataTable> valueManager = ValueManager.GetValueManager<DataTable>("dtReagentPrepLog");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public DataRow drReagentPrepLog
        {
            get
            {
                IValueManager<DataRow> valueManager = ValueManager.GetValueManager<DataRow>("drReagentPrepLog");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return null;
            }
            set
            {
                IValueManager<DataRow> valueManager = ValueManager.GetValueManager<DataRow>("drReagentPrepLog");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }

}
