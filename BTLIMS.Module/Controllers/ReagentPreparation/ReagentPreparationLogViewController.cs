using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.ExpressApp.Win.Templates.ActionContainers;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraEditors;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.ReagentPreparation;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace LDM.Module.Controllers.ReagentPreparation
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ReagentPreparationLogViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        DialogController dcOK;
        ReagentPreparationInfo RPInfo = new ReagentPreparationInfo();
        List<char> lstOperators;
        bool IsEnable = true;
        bool IsNew = false;
        Dictionary<string, string> dicCaption;
        public ReagentPreparationLogViewController()
        {
            InitializeComponent();
            TargetViewId = "ReagentPreparation_ListView;"+ "NonPersistentReagent_DetailView_SelectType;"+ "ReagentPrepChemical;"+ "ReagentPrepLog_DetailView_Chemistry;"
                + "ReagentPrepLog_ListView_Chemistry;"+ "Items_LookupListView_ReagentPrep;"+ "ReagentPreparation_DetailView_Chemistry;"
                + "ReagentPreparation_ReagentPrepLogs_ListView;"+ "ReagentPreparation_DetailView_Calibration;"+ "ReagentPreparation_ReagentPrepLogs_ListView_Calibration;"
                + "NPSampleFields_ListView_Reagent;"+ "ReagentPrepMicroMedia;"+ "ReagentPrepLog_DetailView_MicroMedia;"+ "ReagentPrepLog_ListView_MicroMedia;"
                + "ReagentPreparation_DetailView_MicroMedia;"+ "ReagentPreparation_ReagentPrepLogs_ListView_MicroMedia;"+ "ReagentPrepLog_DetailView_Chemistry_PrepNotepad;"
                + "ReagentPrepChemical_PrepNotepad;"+ "ReagentPrepMicroMedia_PrepNotepad;"+ "ReagentPrepLog_DetailView_MicroMedia_PrepNotePad;"+ "ReagentPrepLog_ListView_Chemistry_PrepNotepad;"
                + "ReagentPrepLog_ListView_MicroMedia_PrepNotepad;"+ "ReagentPreparation_ListView_CopyPrevious;"+ "ReagentPrepClassify;"+ "NPReagentPrepLog_DetailView_Chemistry;"+ "NPReagentPrep_ListView;"
                + "NPReagentPrepLog_DetailView_MicroMedia;";
            ResetReagentPrepLog.TargetViewId = NextReagentPrepLog.TargetViewId = PreviousReagentPrepLog.TargetViewId = "ReagentPrepChemical;"+ "ReagentPrepMicroMedia;"+ "ReagentPrepChemical_PrepNotepad;"+ "ReagentPrepMicroMedia_PrepNotepad;";
            LevelOfOk.TargetViewId = "ReagentPreparation_DetailView_Calibration";
            OkReagentPrepLog.TargetViewId = "ReagentPrepChemical;"+ "ReagentPrepChemical_PrepNotepad;"+ "ReagentPrepMicroMedia;"+ "ReagentPrepMicroMedia_PrepNotepad;";
            PrepNotepad.TargetViewId = "ReagentPreparation_DetailView_Chemistry;"+ "ReagentPreparation_DetailView_MicroMedia;";


            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if(RPInfo.lstEditorID==null)
                {
                    RPInfo.lstEditorID = new List<string>();
                    RPInfo.lstEditorID.Add("StockConc_C1");
                    RPInfo.lstEditorID.Add("FinalVol_V2");
                    RPInfo.lstEditorID.Add("FinalConc_C2");
                    RPInfo.lstEditorID.Add("MW");
                    RPInfo.lstEditorID.Add("EqWt");
                    RPInfo.lstEditorID.Add("Cal_Weight_g_w1");
                    RPInfo.lstEditorID.Add("Cal_VolumeTaken_V1");
                    RPInfo.lstEditorID.Add("Cal_FinalVol_V2");
                    RPInfo.lstEditorID.Add("Cal_FinalConc_C2");
                    RPInfo.lstEditorID.Add("Purity");
                    RPInfo.lstEditorID.Add("Density");
                    RPInfo.lstEditorID.Add("Constant");
                    RPInfo.lstEditorID.Add("Weight_g_w1");
                    RPInfo.lstEditorID.Add("VolumeTaken_V1");
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View.Id == "ReagentPreparation_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                    Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem += ReagentPreparationLogViewController_CustomProcessSelectedItem;
                    List<ReagentPrepLog> lstPrep = View.ObjectSpace.GetObjects<ReagentPrepLog>(CriteriaOperator.Parse("[ReagentPreparation] Is Null")).ToList();
                    if(lstPrep.Count>0)
                    {
                        foreach(ReagentPrepLog obj in lstPrep.ToList())
                        {
                            View.ObjectSpace.Delete(obj);
                        }
                        View.ObjectSpace.CommitChanges();
                    }
                }
                else if (View.Id == "NonPersistentReagent_DetailView_SelectType")
                {
                    View.CurrentObject = View.ObjectTypeInfo.CreateInstance();
                    ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
                }
                else if (View.Id == "ReagentPrepChemical" ||View.Id== "ReagentPrepMicroMedia")
                {
                    ArithematicOperators();
                    if (RPInfo.dtReagentPrepLog != null && RPInfo.dtReagentPrepLog.Rows.Count > 0)
                    {
                        RPInfo.dtReagentPrepLog.Rows.Clear();
                    }
                    Application.DetailViewCreating += Application_DetailViewCreating;
                    PreviousReagentPrepLog.Enabled["boolPrevios"]= false;
                    dcOK = Frame.GetController<DialogController>();
                    if (dcOK != null)
                    {
                        dcOK.Accepting += DcOK_Accepting;
                        dcOK.AcceptAction.Active.SetItemValue("Ok", false);
                        dcOK.CancelAction.Active.SetItemValue("Cancel", false);
                        dcOK.CloseAction.Executing += CloseAction_Executing;
                        dcOK.ViewClosing += DcOK_ViewClosing;
                    }
                    dicColumnValues();
                    if (View.Id=="ReagentPrepChemical")
                    {
                        CreateDataTableInChemistry(); 
                    }
                    else
                    {
                        CreateDataTableInMicroMedia();
                    }
                    if (Application.MainWindow.View is DetailView)
                    {
                        Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objReagentPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                        if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.View)
                        {
                            ResetReagentPrepLog.Enabled["boolReset"] = false;
                            PreviousReagentPrepLog.Enabled["boolPrevios"] = false;
                            OkReagentPrepLog.Enabled["boolOKPrep"] = false;
                        }
                        IList<ReagentPrepLog> lstPrep = View.ObjectSpace.GetObjects<ReagentPrepLog>(CriteriaOperator.Parse("[ReagentPreparation]=?", objReagentPrep.Oid));
                        if (lstPrep.Count == 1)
                        {
                            NextReagentPrepLog.Enabled["boolNext"] = false;
                        }
                    }
                }
                else if(View.Id== "Items_LookupListView_ReagentPrep")
                {
                    List<Guid> lstofItem = View.ObjectSpace.GetObjects<Distribution>(CriteriaOperator.Parse("")).Where(i => i.LT != null && !string.IsNullOrEmpty(i.LT) && i.Item != null).Select(i => i.Item.Oid).Distinct().ToList() ;
                    if(lstofItem.Count>0)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstofItem);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is null");
                    }
                }
                //else if(View.Id== "ReagentPrepLog_DetailView_Chemistry" ||View.Id== "ReagentPrepLog_DetailView_MicroMedia" ||View.Id== "ReagentPrepLog_DetailView_Chemistry_PrepNotepad")
                //{
                //    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                //    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated += ChangeLayoutGroupCaptionViewController_ItemCreated;
                //}
                //else if(View.Id== "ReagentPrepLog_ListView_Chemistry" ||View.Id== "ReagentPrepLog_ListView_MicroMedia")
                //{
                //    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is  null");
                //}
                else if(View.Id== "ReagentPreparation_DetailView_Chemistry" ||View.Id== "ReagentPreparation_DetailView_MicroMedia")
                {
                    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    IsEnable = false;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                }
                else if (View.Id == "ReagentPreparation_DetailView_Calibration")
                {
                    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                }
                else if(View.Id== "NPSampleFields_ListView_Reagent")
                {
                    Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objRP = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                    if (objRP.SelectStockSolution==StockSolution.VendorStock)
                    {
                        if (objRP != null && objRP.VendorStock != null)
                        {
                            NPSampleFields objNew = View.ObjectSpace.CreateObject<NPSampleFields>();
                            objNew.FieldCaption = objRP.VendorStock.items;
                            ((ListView)View).CollectionSource.Add(objNew);
                        } 
                    }
                    else
                    {
                        if (objRP != null && objRP.LabStock != null)
                        {
                            NPSampleFields objNew = View.ObjectSpace.CreateObject<NPSampleFields>();
                            objNew.FieldCaption = objRP.LabStock.STandardName;
                            ((ListView)View).CollectionSource.Add(objNew);
                        }
                    }
                    ListPropertyEditor lvPrep = ((DetailView)Application.MainWindow.View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                    if(lvPrep!=null && lvPrep.ListView!=null && ((ListView)lvPrep.ListView).CollectionSource.GetCount()>1)
                    {
                        foreach(ReagentPrepLog obj in ((ListView)lvPrep.ListView).CollectionSource.List.Cast<ReagentPrepLog>().Where(i=>i.WorkingStdName!=null && i.Oid!=new Guid(HttpContext.Current.Session["rowid"].ToString()) && i.ComponentID <= Convert.ToUInt16(HttpContext.Current.Session["ComponentID"].ToString())))
                        {
                            if (((ListView)View).CollectionSource.List.Cast<NPSampleFields>().Any(i=>i.FieldCaption== obj.WorkingStdName)==false)
                            {
                                NPSampleFields objNew = View.ObjectSpace.CreateObject<NPSampleFields>();
                                objNew.FieldCaption = obj.WorkingStdName;
                                ((ListView)View).CollectionSource.Add(objNew); 
                            }
                        }
                    }
                }
                else if(View.Id== "ReagentPreparation_ReagentPrepLogs_ListView" ||View.Id== "ReagentPreparation_ReagentPrepLogs_ListView_MicroMedia")
                {
                    IsEnable = false;
                }
                else if(View.Id== "ReagentPrepChemical_PrepNotepad" ||View.Id== "ReagentPrepMicroMedia_PrepNotepad")
                {
                    Application.DetailViewCreating += Application_DetailViewCreating;
                    if (RPInfo.dtReagentPrepLog != null && RPInfo.dtReagentPrepLog.Rows.Count > 0)
                    {
                        RPInfo.dtReagentPrepLog.Rows.Clear();
                    }
                    if(RPInfo.drReagentPrepLog!=null)
                    {
                        RPInfo.drReagentPrepLog = null;
                    }
                    Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objReagentPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                    if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.View)
                    {
                        ResetReagentPrepLog.Enabled["boolReset"] = false;
                        PreviousReagentPrepLog.Enabled["boolPrevios"] = false;
                        OkReagentPrepLog.Enabled["boolOKPrep"] = false;
                        if (objReagentPrep.ReagentPrepLogs.Count()==1)
                        {
                            NextReagentPrepLog.Enabled["boolNext"] = false;
                        }
                    }
                   PreviousReagentPrepLog.Enabled["boolPrevios"] = false;
                    if (View.Id=="ReagentPrepChemical_PrepNotepad")
                    {
                        CreateDataTableInChemistry();
                        foreach (ReagentPrepLog obj in objReagentPrep.ReagentPrepLogs.ToList())
                        {
                            string strVendorstock = string.Empty;
                            Guid strVendorstockOid = Guid.Empty;
                            string strLabstock = string.Empty;
                            Guid strLabstockOid = Guid.Empty;
                            string strLT = string.Empty;
                            Guid strLTOid = Guid.Empty;
                            string strLLT = string.Empty;
                            Guid strLLTOid = Guid.Empty;
                            string CalculationApproach = string.Empty;
                            Guid CalculationApproachOid = Guid.Empty;
                            string FinalVol_V2_Units = string.Empty;
                            Guid FinalVol_V2_UnitsOid = Guid.Empty;
                            string StockConc_C1_Units = string.Empty;
                            Guid StockConc_C1_UnitsOid = Guid.Empty;
                            string InitialVolTaken_V1_Units = string.Empty;
                            Guid InitialVolTaken_V1_UnitsOid = Guid.Empty;
                            string FinalConc_C2_Units = string.Empty;
                            Guid FinalConc_C2_UnitsOid = Guid.Empty;
                            bool solvent = false;
                            DateTime? LTExpDate = null;
                            DateTime? LLTExpDate = null;
                            if (obj.VendorStock != null)
                            {
                                strVendorstock = obj.VendorStock.items;
                                strVendorstockOid = obj.VendorStock.Oid;
                            }
                            if (obj.LabStock != null)
                            {
                                strLabstock = obj.LabStock.STandardName;
                                strLabstockOid = obj.LabStock.Oid;
                            }
                            if (obj.LLT != null)
                            {
                                strLLT = obj.LLT.LLT;
                                strLTOid = obj.LLT.Oid;
                            }
                            if (obj.LT != null)
                            {
                                strLT = obj.LT.LT;
                                strLTOid = obj.LT.Oid;
                            }
                            if (obj.LT != null)
                            {
                                strLT = obj.LT.LT;
                                strLTOid = obj.LT.Oid;
                            }
                            if (obj.CalculationApproach != null)
                            {
                                CalculationApproach = obj.CalculationApproach.Approach;
                                CalculationApproachOid = obj.CalculationApproach.Oid;
                            }
                            if (obj.Cal_FinalVol_V2_Units != null)
                            {
                                FinalVol_V2_Units = obj.Cal_FinalVol_V2_Units.Units;
                                FinalVol_V2_UnitsOid = obj.Cal_FinalVol_V2_Units.Oid;
                            }
                            if (obj.StockConc_C1_Units != null)
                            {
                                StockConc_C1_Units = obj.StockConc_C1_Units.Units;
                                StockConc_C1_UnitsOid = obj.StockConc_C1_Units.Oid;
                            }
                            if (obj.InitialVolTaken_V1_Units != null)
                            {
                                InitialVolTaken_V1_Units = obj.InitialVolTaken_V1_Units.Units;
                                InitialVolTaken_V1_UnitsOid = obj.InitialVolTaken_V1_Units.Oid;
                            }
                            if (obj.FinalConc_C2_Units != null)
                            {
                                FinalConc_C2_Units = obj.FinalConc_C2_Units.Units;
                                FinalConc_C2_UnitsOid = obj.FinalConc_C2_Units.Oid;
                            }
                            solvent = obj.Solvent;
                            if (obj.LTExpDate != null && obj.LTExpDate != DateTime.MinValue)
                            {
                                LTExpDate = obj.LTExpDate;
                            }
                            if (obj.LLTExpDate != null && obj.LLTExpDate != DateTime.MinValue)
                            {
                                LLTExpDate = obj.LLTExpDate;
                            }
                            RPInfo.dtReagentPrepLog.Rows.Add(obj.ComponentID, strVendorstock, strVendorstockOid, strLT, strLTOid
                                , strLabstock, strLabstockOid, strLLT, strLLTOid, CalculationApproach, CalculationApproachOid, FinalVol_V2_Units, FinalVol_V2_UnitsOid
                                , obj.Purity, obj.Density, obj.Constant, obj.Weight_g_w1, obj.VolumeTaken_V1, obj.StockConc_C1, StockConc_C1_Units, StockConc_C1_UnitsOid,
                                obj.FinalVol_V2, obj.FinalConc_C2, InitialVolTaken_V1_Units, InitialVolTaken_V1_UnitsOid, FinalConc_C2_Units, FinalConc_C2_UnitsOid
                                , obj.MW, obj.EqWt, solvent, LTExpDate, LLTExpDate);
                        } 
                    }
                    else
                    {
                        CreateDataTableInMicroMedia();
                        foreach (ReagentPrepLog obj in objReagentPrep.ReagentPrepLogs.ToList())
                        {
                            string strVendorstock = string.Empty;
                            Guid strVendorstockOid = Guid.Empty;
                            string strLabstock = string.Empty;
                            Guid strLabstockOid = Guid.Empty;
                            string strLT = string.Empty;
                            Guid strLTOid = Guid.Empty;
                            string strLLT = string.Empty;
                            Guid strLLTOid = Guid.Empty;
                            bool solvent = false;
                            DateTime? LTExpDate = null;
                            DateTime? LLTExpDate = null;
                            if (obj.VendorStock != null)
                            {
                                strVendorstock = obj.VendorStock.items;
                                strVendorstockOid = obj.VendorStock.Oid;
                            }
                            if (obj.LabStock != null)
                            {
                                strLabstock = obj.LabStock.STandardName;
                                strLabstockOid = obj.LabStock.Oid;
                            }
                            if (obj.LLT != null)
                            {
                                strLLT = obj.LLT.LLT;
                                strLTOid = obj.LLT.Oid;
                            }
                            if (obj.LT != null)
                            {
                                strLT = obj.LT.LT;
                                strLTOid = obj.LT.Oid;
                            }
                            if (obj.LT != null)
                            {
                                strLT = obj.LT.LT;
                                strLTOid = obj.LT.Oid;
                            }
                            solvent = obj.Solvent;
                            if (obj.LTExpDate != null && obj.LTExpDate != DateTime.MinValue)
                            {
                                LTExpDate = obj.LTExpDate;
                            }
                            if (obj.LLTExpDate != null && obj.LLTExpDate != DateTime.MinValue)
                    {
                                LLTExpDate = obj.LLTExpDate;
                            }
                            RPInfo.dtReagentPrepLog.Rows.Add(obj.ComponentID, strVendorstock, strVendorstockOid, strLT, strLTOid
                                , strLabstock, strLabstockOid, strLLT, strLLTOid,obj.Weight_g_w1,obj.VolumeTaken_V1,obj.FinalVol_V2, obj.FilterSterilization
                                , obj.SporeGrowth,obj.PHCriteria,obj.PH, obj.PositiveControl, obj.NegativeControl, obj.Autoclave, solvent, LTExpDate, LLTExpDate);
                        }
                    }
                }
                else if(View.Id== "ReagentPrepLog_DetailView_Chemistry_PrepNotepad" ||View.Id== "ReagentPrepLog_DetailView_MicroMedia_PrepNotePad")
                {
                    if(((DetailView)View).ViewEditMode==ViewEditMode.View)
                    {
                        ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
                    }
                }
                else if(View.Id== "ReagentPrepClassify")
                {
                    IsNew = true;
                    ((WebLayoutManager)((DashboardView)View).LayoutManager).PageControlCreated += TabViewController_PageControlCreated;
                }
                else if(View.Id== "NPReagentPrepLog_DetailView_Chemistry")
                {
                    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated += ChangeLayoutGroupCaptionViewController_ItemCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CreateDataTableInChemistry()
        {
            try
            {
            RPInfo.dtReagentPrepLog = new DataTable { TableName = "Chemistry" };
            RPInfo.dtReagentPrepLog.Columns.Add("ID", typeof(uint));
            RPInfo.dtReagentPrepLog.Columns.Add("VendorStock");
            RPInfo.dtReagentPrepLog.Columns.Add("VendorStockOid", typeof(Guid));
            RPInfo.dtReagentPrepLog.Columns.Add("LT#");
            RPInfo.dtReagentPrepLog.Columns.Add("LT#Oid", typeof(Guid));
            RPInfo.dtReagentPrepLog.Columns.Add("LabStock");
            RPInfo.dtReagentPrepLog.Columns.Add("LabStockOid", typeof(Guid));
            RPInfo.dtReagentPrepLog.Columns.Add("LLT#");
            RPInfo.dtReagentPrepLog.Columns.Add("LLT#Oid", typeof(Guid));
            RPInfo.dtReagentPrepLog.Columns.Add("CalculationApproach");
            RPInfo.dtReagentPrepLog.Columns.Add("CalculationApproachOid", typeof(Guid));
            RPInfo.dtReagentPrepLog.Columns.Add("FinalVol(V2)Units");
            RPInfo.dtReagentPrepLog.Columns.Add("FinalVol(V2)UnitsOid", typeof(Guid));
            RPInfo.dtReagentPrepLog.Columns.Add("Purity(%)");
            RPInfo.dtReagentPrepLog.Columns.Add("Density");
            RPInfo.dtReagentPrepLog.Columns.Add("Constant");
            RPInfo.dtReagentPrepLog.Columns.Add("Weight(g)(W1)");
            RPInfo.dtReagentPrepLog.Columns.Add("VolumeTaken(V1)");
            RPInfo.dtReagentPrepLog.Columns.Add("StockConc(C1)");
            RPInfo.dtReagentPrepLog.Columns.Add("StockConc(C1)Units");
            RPInfo.dtReagentPrepLog.Columns.Add("StockConc(C1)UnitsOid", typeof(Guid));
            RPInfo.dtReagentPrepLog.Columns.Add("FinalVol(V2)");
            RPInfo.dtReagentPrepLog.Columns.Add("FinalConc(C2)");
            RPInfo.dtReagentPrepLog.Columns.Add("InitialVolTaken(V1)Units");
            RPInfo.dtReagentPrepLog.Columns.Add("InitialVolTaken(V1)UnitsOid", typeof(Guid));
            RPInfo.dtReagentPrepLog.Columns.Add("FinalConc(C2)Units");
            RPInfo.dtReagentPrepLog.Columns.Add("FinalConc(C2)UnitsOid", typeof(Guid));
            RPInfo.dtReagentPrepLog.Columns.Add("MW");
            RPInfo.dtReagentPrepLog.Columns.Add("EqWt");
            RPInfo.dtReagentPrepLog.Columns.Add("Solvent",typeof(bool));
            RPInfo.dtReagentPrepLog.Columns.Add("LT#ExpDate", typeof(DateTime));
            RPInfo.dtReagentPrepLog.Columns.Add("LLT#ExpDate", typeof(DateTime));
                RPInfo.dtReagentPrepLog.Columns.Add("Formula");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CreateDataTableInMicroMedia()
        {
            try
            {
                RPInfo.dtReagentPrepLog = new DataTable { TableName = "MicroMedia" };
                RPInfo.dtReagentPrepLog.Columns.Add("ID", typeof(uint));
                RPInfo.dtReagentPrepLog.Columns.Add("VendorStock");
                RPInfo.dtReagentPrepLog.Columns.Add("VendorStockOid", typeof(Guid));
                RPInfo.dtReagentPrepLog.Columns.Add("LT#");
                RPInfo.dtReagentPrepLog.Columns.Add("LT#Oid", typeof(Guid));
                RPInfo.dtReagentPrepLog.Columns.Add("LabStock");
                RPInfo.dtReagentPrepLog.Columns.Add("LabStockOid", typeof(Guid));
                RPInfo.dtReagentPrepLog.Columns.Add("LLT#");
                RPInfo.dtReagentPrepLog.Columns.Add("LLT#Oid", typeof(Guid));
                RPInfo.dtReagentPrepLog.Columns.Add("WtTaken(g)");
                RPInfo.dtReagentPrepLog.Columns.Add("VolumeTaken(ml)");
                RPInfo.dtReagentPrepLog.Columns.Add("FinalVolume(ml)");
                RPInfo.dtReagentPrepLog.Columns.Add("FilterSterilization(Y/N)");
                RPInfo.dtReagentPrepLog.Columns.Add("SporeGrowth(Y/N)");
                RPInfo.dtReagentPrepLog.Columns.Add("PHCriteria");
                RPInfo.dtReagentPrepLog.Columns.Add("PH");
                RPInfo.dtReagentPrepLog.Columns.Add("+Control");
                RPInfo.dtReagentPrepLog.Columns.Add("-Control");
                RPInfo.dtReagentPrepLog.Columns.Add("Autoclave(Y/N)");
                RPInfo.dtReagentPrepLog.Columns.Add("Solvent", typeof(bool));
                RPInfo.dtReagentPrepLog.Columns.Add("LT#ExpDate", typeof(DateTime));
                RPInfo.dtReagentPrepLog.Columns.Add("LLT#ExpDate", typeof(DateTime));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TabViewController_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
          try
            {
                if (e.Model.Id == "Reagent")
                {
                    if (IsNew)
                    {
                        e.PageControl.ClientSideEvents.Init = "function(s,e){s.SetActiveTabIndex(0);}"; 
                        IsNew = false;
                    }
                    e.PageControl.Callback += PageControl_Callback;
                    e.PageControl.Init += PageControl_Init;
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void PageControl_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxPageControl pageControl = (ASPxPageControl)sender;
                ClientSideEventsHelper.AssignClientHandlerSafe(pageControl, "ActiveTabChanged", "function(s, e) { s.PerformCallback('TabChanged'); } ", "ActiveTab");
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PageControl_Callback(object sender, CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter == "TabChanged")
                {
                    TabPage activePage = ((ASPxPageControl)sender).ActiveTabPage;
                    if(activePage!=null)
                    {
                        RPInfo.ActiveTabText = activePage.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DcOK_ViewClosing(object sender, EventArgs e)
        {
          
        }

        private void CloseAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void lvPrep_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objReagentPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                if(lvPrep != null && lvPrep.InnerView==null)
                {
                    lvPrep.CreateControl();
                }
                if(lvPrep!=null && lvPrep.InnerView!=null && objReagentPrep!=null)
                {
                    ((ListView)lvPrep.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReagentPreparation]=?", objReagentPrep.Oid);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ChangeLayoutGroupCaptionViewController_ItemCreated(object sender, ItemCreatedEventArgs e)
        {
            try
            {
                if (e.ViewItem is PropertyEditor && (((PropertyEditor)e.ViewItem).PropertyName == "MW" || ((PropertyEditor)e.ViewItem).PropertyName == "EqWt"))
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ReagentPreparationLogViewController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
           try
            {
                e.Handled = true;
                Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objCurrent = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)View.CurrentObject;
                if (objCurrent != null && objCurrent.PrepSelectType!=null)
                {
                    IObjectSpace newObjectSpace = Application.CreateObjectSpace();
                    if (objCurrent.PrepSelectType == PrepSelectTypes.ChemicalReagentPrep)
                    {
                        e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDetailView(newObjectSpace, "ReagentPreparation_DetailView_Chemistry",true, newObjectSpace.GetObject(objCurrent));
                       
                    }
                    else if (objCurrent.PrepSelectType == PrepSelectTypes.CalibrationSetPrep)
                    {
                        e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDetailView(newObjectSpace, "ReagentPreparation_DetailView_Calibration", true, newObjectSpace.GetObject(objCurrent));
                    }
                    else if (objCurrent.PrepSelectType == PrepSelectTypes.MicroMediaAndReagentPrep)
                    {
                        e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDetailView(newObjectSpace, "ReagentPreparation_DetailView_MicroMedia", true, newObjectSpace.GetObject(objCurrent));
                        
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objCurrent = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)View.CurrentObject;
                if(objCurrent!=null && objCurrent.PrepSelectType!=null)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Modules.BusinessObjects.ReagentPreparation.ReagentPreparation obj = os.GetObject<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>(objCurrent);
                    if (objCurrent.PrepSelectType==PrepSelectTypes.ChemicalReagentPrep)
                    {
                        if (obj != null)
                        {
                            DetailView dv = Application.CreateDetailView(os, "ReagentPreparation_DetailView_Chemistry", true, obj);
                            dv.ViewEditMode = ViewEditMode.Edit;
                            Frame.SetView(dv);
                        }
                    }
                    else if (objCurrent.PrepSelectType == PrepSelectTypes.CalibrationSetPrep)
                    {
                        if (obj != null)
                        {
                            DetailView dv = Application.CreateDetailView(os, "ReagentPreparation_DetailView_Calibration", true, obj);
                            dv.ViewEditMode = ViewEditMode.Edit;
                            Frame.SetView(dv);
                        }
                    }
                    else if (objCurrent.PrepSelectType == PrepSelectTypes.MicroMediaAndReagentPrep)
                    {
                        if (obj != null)
                        {
                            DetailView dv = Application.CreateDetailView(os, "ReagentPreparation_DetailView_MicroMedia", true, obj);
                            dv.ViewEditMode = ViewEditMode.Edit;
                            Frame.SetView(dv);
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndCloseAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ListPropertyEditor lvPrep = ((DetailView)View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                if (lvPrep != null && lvPrep.ListView != null)
                {
                    if (((ListView)lvPrep.ListView).CollectionSource.GetCount() == 0)
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "enterednooflevel"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          try
            {
                ListPropertyEditor lvPrep = ((DetailView)View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                if(lvPrep!=null && lvPrep.ListView!=null)
                {
                    if(((ListView)lvPrep.ListView).CollectionSource.GetCount()==0)
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "enterednooflevel"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DcOK_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if(View.Id== "ReagentPrepChemical")
                {
                    DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                    DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                    //if (dvPrep!=null && dvPrep.InnerView!=null)
                    //{
                    //    ReagentPrepLog objCurReagent = (ReagentPrepLog)dvPrep.InnerView.CurrentObject;
                    //    dvPrep.InnerView.ObjectSpace.CommitChanges();
                    //    if(objCurReagent.LabStock==null && objCurReagent.VendorStock==null && ((ListView)lvPrep.InnerView).CollectionSource.GetCount()==0)
                    //    {
                    //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Haventaddanycomponetyet"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //        e.Cancel = true;
                    //    }
                    //    else
                    //    {
                    //        IObjectSpace os = Application.CreateObjectSpace();
                    //        Modules.BusinessObjects.ReagentPreparation.ReagentPreparation newReagentPreparation = os.CreateObject<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>();
                    //        List<ReagentPrepLog> lstReagentPreLog = os.GetObjects<ReagentPrepLog>(new InOperator("Oid", RPInfo.lstReagentPrepLog.Select(i => i.Oid))).Where(i=>i.LabStock!=null || i.VendorStock!=null).ToList();
                    //        foreach(ReagentPrepLog objNew in lstReagentPreLog.ToList())
                    //        {
                    //            newReagentPreparation.ReagentPrepLogs.Add(objNew);
                    //        }
                    //        newReagentPreparation.PrepSelectType= PrepSelectTypes.ChemicalReagentPrep;
                    //        DetailView detailView = Application.CreateDetailView(os, "ReagentPreparation_DetailView_Chemistry",true, newReagentPreparation);
                    //        detailView.ViewEditMode = ViewEditMode.Edit;
                    //        Application.MainWindow.SetView(detailView);
                    //    }
                    //}
                }
                else if(View.Id== "ReagentPrepMicroMedia")
                {
                    DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                    DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                    if (dvPrep != null && dvPrep.InnerView != null)
                    {
                        ReagentPrepLog objCurReagent = (ReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        dvPrep.InnerView.ObjectSpace.CommitChanges();
                        if (objCurReagent.LabStock == null && objCurReagent.VendorStock == null && ((ListView)lvPrep.InnerView).CollectionSource.GetCount() == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Haventaddanycomponetyet"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                        else
                        {
                            //IObjectSpace os = Application.CreateObjectSpace();
                            //Modules.BusinessObjects.ReagentPreparation.ReagentPreparation newReagentPreparation = os.CreateObject<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>();
                            //List<ReagentPrepLog> lstReagentPreLog = os.GetObjects<ReagentPrepLog>(new InOperator("Oid", RPInfo.lstReagentPrepLog.Select(i => i.Oid))).Where(i => i.LabStock != null || i.VendorStock != null).ToList();
                            //foreach (ReagentPrepLog objNew in lstReagentPreLog.ToList())
                            //{
                            //    newReagentPreparation.ReagentPrepLogs.Add(objNew);
                            //}
                            //newReagentPreparation.PrepSelectType = PrepSelectTypes.MicroMediaAndReagentPrep;
                            //DetailView detailView = Application.CreateDetailView(os, "ReagentPreparation_DetailView_MicroMedia", true, newReagentPreparation);
                            //detailView.ViewEditMode = ViewEditMode.Edit;
                            //Application.MainWindow.SetView(detailView);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if ((View.Id == "ReagentPrepLog_DetailView_Chemistry" || View.Id == "ReagentPrepLog_DetailView_Chemistry_PrepNotepad") && e.OldValue != e.NewValue)
                {
                    ReagentPrepLog objPrepLog = (ReagentPrepLog)e.Object;
                    if (objPrepLog != null)
                    {
                        if (e.PropertyName == "Solvent")
                        {
                            if (objPrepLog.Solvent)
                            {
                                DisableControles(false);
                            }
                            else
                            {
                                DisableControles(true);
                            }
                        }
                        else if (e.PropertyName == "VendorStock")
                        {
                            if (objPrepLog.VendorStock != null)
                            {
                                objPrepLog.LabStock = null;
                                objPrepLog.LLT = null;
                                objPrepLog.LLTExpDate = null;
                            }
                        }
                        else if (e.PropertyName == "LabStock")
                        {
                            if (objPrepLog.LabStock != null)
                            {
                                objPrepLog.VendorStock = null;
                                objPrepLog.LT = null;
                                objPrepLog.LTExpDate = null;
                                ASPxBooleanPropertyEditor aSPxPropertySolvent = ((DetailView)View).FindItem("Solvent") as ASPxBooleanPropertyEditor;
                                if (aSPxPropertySolvent != null)
                                {
                                    aSPxPropertySolvent.AllowEdit.SetItemValue("AlowEdit", false);
                                }
                            }
                            else
                            {
                                ASPxBooleanPropertyEditor aSPxPropertySolvent = ((DetailView)View).FindItem("Solvent") as ASPxBooleanPropertyEditor;
                                if (aSPxPropertySolvent != null)
                                {
                                    aSPxPropertySolvent.AllowEdit.SetItemValue("AlowEdit", true);
                                }
                            }
                        }
                        else if (e.PropertyName == "InitialVolTaken_V1_Units" || e.PropertyName == "StockConc_C1_Units" || e.PropertyName == "FinalVol_V2_Units"
                            || e.PropertyName == "FinalConc_C2_Units" || e.PropertyName == "CalculationApproach" || e.PropertyName == "FinalConc_C2" || e.PropertyName == "FinalVol_V2"
                            || e.PropertyName == "StockConc_C1" || e.PropertyName == "VolumeTaken_V1")
                        {
                            if (objPrepLog.CalculationApproach != null && !objPrepLog.CalculationApproach.Approach.Contains("None"))
                            {
                                NCalc.Expression exp;
                                if (objPrepLog.CalculationApproach.Approach.Contains("V1"))
                                {
                                    DisableV1andC1Units(true, false, View);
                                    if (objPrepLog.InitialVolTaken_V1_Units != null && objPrepLog.StockConc_C1_Units != null && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                                    {
                                        List<RegentPrepCalculationEditor> lstCalculation = View.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                                        if (lstCalculation.Count > 0)
                                        {
                                            string str = objPrepLog.CalculationApproach.Approach.Split(' ').ToList().LastOrDefault().ToString();
                                            RegentPrepCalculationEditor objCalculationEditor = lstCalculation.FirstOrDefault(i => i.V1Units != null && i.V2Units != null && i.C1Units != null && i.C2Units != null && i.V1Units == objPrepLog.InitialVolTaken_V1_Units && i.V2Units == objPrepLog.FinalVol_V2_Units && i.C1Units == objPrepLog.StockConc_C1_Units && i.C2Units == objPrepLog.FinalConc_C2_Units);
                                            if (objCalculationEditor != null)
                                            {
                                                string Type = string.Empty;
                                                if (objPrepLog.Formula != objCalculationEditor.Formula)
                                                {
                                                    objPrepLog.Formula = objCalculationEditor.Formula;
                                                    if (!string.IsNullOrEmpty(objPrepLog.Formula))
                                                    {

                                                        DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str, View);
                                                        if (str == "C1")
                                                        {
                                                            objPrepLog.StockConc_C1 = null;
                                                            ASPxStringPropertyEditor StockConc_C1 = ((DetailView)View).FindItem("StockConc_C1") as ASPxStringPropertyEditor;
                                                            if (StockConc_C1 != null)
                                                            {
                                                                StockConc_C1.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (StockConc_C1 != null && StockConc_C1.Editor != null)
                                                                {
                                                                    StockConc_C1.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                        else if (str == "C2")
                                                        {
                                                            objPrepLog.FinalConc_C2 = null;
                                                            ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)View).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                                            if (FinalConc_C2 != null)
                                                            {
                                                                FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                                                {
                                                                    FinalConc_C2.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                        else if (str == "V1")
                                                        {
                                                            objPrepLog.VolumeTaken_V1 = null;
                                                            ASPxStringPropertyEditor VolumeTaken_V1 = ((DetailView)View).FindItem("VolumeTaken_V1") as ASPxStringPropertyEditor;
                                                            if (VolumeTaken_V1 != null)
                                                            {
                                                                VolumeTaken_V1.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (VolumeTaken_V1 != null && VolumeTaken_V1.Editor != null)
                                                                {
                                                                    VolumeTaken_V1.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }

                                                        }
                                                        else if (str == "V2")
                                                        {
                                                            objPrepLog.FinalVol_V2 = null;
                                                            ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)View).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                                            if (FinalVol_V2 != null)
                                                            {
                                                                FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                                                {
                                                                    FinalVol_V2.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                objPrepLog.Formula = string.Empty;
                                            }
                                            if (!string.IsNullOrEmpty(objPrepLog.Formula) && (e.PropertyName == "FinalConc_C2" || e.PropertyName == "FinalVol_V2" || e.PropertyName == "StockConc_C1" || e.PropertyName == "VolumeTaken_V1"))
                                            {

                                                List<string> lstFormula = objCalculationEditor.Formula.Split('=').ToList();
                                                if (lstFormula.Count == 2)
                                                {
                                                    bool IsCalculate = true;
                                                    if (lstOperators == null)
                                                    {
                                                        ArithematicOperators();
                                                    }
                                                    //string[] arryOperator = lstFormula[1].Split(lstOperators.ToArray());
                                                    string strFormula = string.Empty;
                                                    string strValue = string.Empty;
                                                    foreach (string objSymbol in lstFormula[1].Split('('))
                                                    {
                                                        int Len = objSymbol.IndexOf(")");
                                                        if (Len > 0)
                                                        {
                                                            strValue = objSymbol.Substring(Len + 1);
                                                            if (string.IsNullOrEmpty(strFormula))
                                                            {
                                                                strFormula = strValue;
                                                            }
                                                            else
                                                            {
                                                                strFormula = strFormula + strValue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strFormula = objSymbol;
                                                        }

                                                    }
                                                    string[] arryOperator = strFormula.Split(lstOperators.ToArray());
                                                    foreach (string obj in arryOperator.Distinct().ToList())
                                                    {
                                                        if (obj.Contains("C1"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.StockConc_C1) || objPrepLog.StockConc_C1 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("V1"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.VolumeTaken_V1) || objPrepLog.VolumeTaken_V1 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("C2"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.FinalConc_C2) || objPrepLog.FinalConc_C2 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("V2"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.FinalVol_V2) || objPrepLog.FinalVol_V2 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("Purity"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.Purity) || objPrepLog.Purity == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (string.IsNullOrEmpty(objPrepLog.Density) || obj.Contains("Density"))
                                                        {
                                                            if (objPrepLog.Density == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (string.IsNullOrEmpty(objPrepLog.MW) || obj.Contains("MW"))
                                                        {
                                                            if (objPrepLog.MW == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                    }
                                                    if (objCalculationEditor != null && IsCalculate == true)
                                                    {

                                                        exp = new NCalc.Expression(strFormula.ToLower());
                                                        foreach (string obj in strFormula.Split(lstOperators.ToArray()))
                                                        {
                                                            if (obj == "C1")
                                                            {
                                                                if (double.TryParse(objPrepLog.StockConc_C1.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["c1"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "C2")
                                                            {
                                                                if (double.TryParse(objPrepLog.FinalConc_C2.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["c2"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "V1")
                                                            {
                                                                if (double.TryParse(objPrepLog.VolumeTaken_V1.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["v1"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "V2")
                                                            {
                                                                if (double.TryParse(objPrepLog.FinalVol_V2.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["v2"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "Purity")
                                                            {
                                                                if (double.TryParse(objPrepLog.Purity.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["purity"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "Density")
                                                            {
                                                                if (double.TryParse(objPrepLog.Density.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["density"] = newval;
                                                                }
                                                            }
                                                        }
                                                        if (!str.Contains(e.PropertyName))
                                                        {
                                                            if (lstFormula[0].Contains("V1"))
                                                            {
                                                                objPrepLog.VolumeTaken_V1 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("V2"))
                                                            {
                                                                objPrepLog.FinalVol_V2 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("C1"))
                                                            {
                                                                objPrepLog.StockConc_C1 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("C2"))
                                                            {
                                                                objPrepLog.FinalConc_C2 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (objPrepLog.CalculationApproach.Approach.Contains("W1"))
                                {
                                    DisableV1andC1Units(false, true, View);
                                    if (objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                                    {
                                        List<RegentPrepCalculationEditor> lstCalculation = View.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                                        if (lstCalculation.Count > 0)
                                        {
                                            string str = objPrepLog.CalculationApproach.Approach.Split(' ').ToList().LastOrDefault().ToString();
                                            RegentPrepCalculationEditor objCalculationEditor = lstCalculation.FirstOrDefault(i => i.C2Units != null && i.V2Units != null && i.C2Units == objPrepLog.FinalConc_C2_Units && i.V2Units == objPrepLog.FinalVol_V2_Units);
                                            if (objCalculationEditor != null)
                                            {
                                                string Type = string.Empty;
                                                if (objPrepLog.Formula != objCalculationEditor.Formula)
                                                {
                                                    objPrepLog.Formula = objCalculationEditor.Formula;
                                                    if (!string.IsNullOrEmpty(objPrepLog.Formula))
                                                    {
                                                        DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str, View);
                                                        if (str == "C2")
                                                        {
                                                            objPrepLog.FinalConc_C2 = null;
                                                            ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)View).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                                            if (FinalConc_C2 != null)
                                                            {
                                                                FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                                                {
                                                                    FinalConc_C2.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                        else if (str == "W1")
                                                        {
                                                            objPrepLog.VolumeTaken_V1 = null;
                                                            ASPxStringPropertyEditor Weight_g_w1 = ((DetailView)View).FindItem("Weight_g_w1") as ASPxStringPropertyEditor;
                                                            if (Weight_g_w1 != null)
                                                            {
                                                                Weight_g_w1.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (Weight_g_w1 != null && Weight_g_w1.Editor != null)
                                                                {
                                                                    Weight_g_w1.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                        else if (str == "V2")
                                                        {
                                                            objPrepLog.FinalVol_V2 = null;
                                                            ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)View).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                                            if (FinalVol_V2 != null)
                                                            {
                                                                FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                                                {
                                                                    FinalVol_V2.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                objPrepLog.Formula = string.Empty;
                                            }
                                            if (!string.IsNullOrEmpty(objPrepLog.Formula) && (e.PropertyName == "FinalConc_C2" || e.PropertyName == "FinalVol_V2" || e.PropertyName == "StockConc_C1" || e.PropertyName == "VolumeTaken_V1"))
                                            {

                                                List<string> lstFormula = objCalculationEditor.Formula.Split('=').ToList();
                                                if (lstFormula.Count == 2)
                                                {
                                                    bool IsCalculate = true;
                                                    if (lstOperators == null)
                                                    {
                                                        ArithematicOperators();
                                                    }
                                                    //string[] arryOperator = lstFormula[1].Split(lstOperators.ToArray());
                                                    string strFormula = string.Empty;
                                                    string strValue = string.Empty;
                                                    foreach (string objSymbol in lstFormula[1].Split('('))
                                                    {
                                                        int Len = objSymbol.IndexOf(")");
                                                        if (Len > 0)
                                                        {
                                                            strValue = objSymbol.Substring(Len + 1);
                                                            if (string.IsNullOrEmpty(strFormula))
                                                            {
                                                                strFormula = strValue;
                                                            }
                                                            else
                                                            {
                                                                strFormula = strFormula + strValue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strFormula = objSymbol;
                                                        }

                                                    }
                                                    string[] arryOperator = strFormula.Split(lstOperators.ToArray());
                                                    foreach (string obj in arryOperator.Distinct().ToList())
                                                    {
                                                        if (obj.Contains("C1"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.StockConc_C1) || objPrepLog.StockConc_C1 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("V1"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.VolumeTaken_V1) || objPrepLog.VolumeTaken_V1 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("C2"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.FinalConc_C2) || objPrepLog.FinalConc_C2 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("V2"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.FinalVol_V2) || objPrepLog.FinalVol_V2 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("Purity"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.Purity) || objPrepLog.Purity == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("Density"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.Density) || objPrepLog.Density == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("MW"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.MW) || objPrepLog.MW == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                    }
                                                    if (objCalculationEditor != null && IsCalculate == true)
                                                    {
                                                        exp = new NCalc.Expression(strFormula.ToLower());
                                                        foreach (string obj in strFormula.Split(lstOperators.ToArray()))
                                                        {
                                                            if (obj == "C1")
                                                            {
                                                                if (double.TryParse(objPrepLog.StockConc_C1.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["c1"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "C2")
                                                            {
                                                                if (double.TryParse(objPrepLog.FinalConc_C2.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["c2"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "V1")
                                                            {
                                                                if (double.TryParse(objPrepLog.VolumeTaken_V1.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["v1"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "V2")
                                                            {
                                                                if (double.TryParse(objPrepLog.FinalVol_V2.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["v2"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "Purity")
                                                            {
                                                                if (double.TryParse(objPrepLog.Purity.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["purity"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "Density")
                                                            {
                                                                if (double.TryParse(objPrepLog.Density.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["density"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "MW")
                                                            {
                                                                if (double.TryParse(objPrepLog.Density.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["mw"] = newval;
                                                                }
                                                            }
                                                        }
                                                        if (!str.Contains(e.PropertyName))
                                                        {
                                                            if (lstFormula[0].Contains("V1"))
                                                            {
                                                                objPrepLog.VolumeTaken_V1 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("V2"))
                                                            {
                                                                objPrepLog.FinalVol_V2 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("C1"))
                                                            {
                                                                objPrepLog.StockConc_C1 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("C2"))
                                                            {
                                                                objPrepLog.FinalConc_C2 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objPrepLog.Formula = string.Empty;
                                foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible))
                                {
                                    if (item.GetType() == typeof(ASPxStringPropertyEditor))
                                    {
                                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.ForeColor = Color.Black;

                                        }

                                    }
                                    else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                                    {
                                        ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }

                                    }
                                    else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                                    {
                                        ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            if (propertyEditor.Id == "Component")
                                            {
                                                propertyEditor.Editor.BackColor = Color.LightGray;
                                            }
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                                    {
                                        ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }

                                    }
                                    else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                                    {
                                        ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            if (propertyEditor.Id == "LLTExpDate" || propertyEditor.Id == "LTExpDate")
                                            {
                                                propertyEditor.Editor.BackColor = Color.LightGray;
                                            }
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }

                                    }

                                }
                            }
                        }
                    }
                }
                else if ((View.Id == "ReagentPreparation_DetailView_Chemistry" || View.Id == "ReagentPreparation_DetailView_MicroMedia") && e.OldValue != e.NewValue)
                {
                    if (e.Object.GetType() == typeof(Modules.BusinessObjects.ReagentPreparation.ReagentPreparation))
                    {
                        Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objReaPre = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)e.Object;
                        if (e.PropertyName == "Solvent")
                        {
                            if (objReaPre.Solvent == Solvent.NA)
                            {
                                ASPxStringPropertyEditor propertyEditor = ((DetailView)View).FindItem("SolventID") as ASPxStringPropertyEditor;
                                if (propertyEditor != null)
                                {
                                    propertyEditor.AllowEdit.SetItemValue("AlowEdit", false);
                                }
                                objReaPre.BoolSolvent = false;
                                objReaPre.SolventID = string.Empty;
                            }
                            else
                            {
                                ASPxStringPropertyEditor propertyEditor = ((DetailView)View).FindItem("SolventID") as ASPxStringPropertyEditor;
                                if (propertyEditor != null)
                                {
                                    propertyEditor.AllowEdit.SetItemValue("AlowEdit", true);
                                }
                                objReaPre.BoolSolvent = true;
                            }
                        }
                        else if (e.PropertyName == "ExpirationDate")
                        {
                            if (objReaPre.ExpirationDate != DateTime.MinValue && objReaPre.ExpirationDate <= DateTime.Today)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "validexpirationdate"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                    }

                }
                else if (View.Id == "ReagentPreparation_DetailView_Calibration" && e.OldValue != e.NewValue)
                {
                    if (e.PropertyName == "ExpirationDate")
                    {
                        Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objReaPre = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)e.Object;
                        if (objReaPre.ExpirationDate != DateTime.MinValue && objReaPre.ExpirationDate <= DateTime.Today)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "validexpirationdate"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                    else if (e.PropertyName == "StockConc")
                    {
                        Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objReaPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)e.Object;
                        ListPropertyEditor lvPrep = ((DetailView)View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                        if (objReaPrep != null && lvPrep != null && lvPrep.ListView != null)
                        {
                            ((ListView)lvPrep.ListView).CollectionSource.List.Cast<ReagentPrepLog>().ToList().ForEach(i => { i.Cal_Weight_g_w1 = objReaPrep.StockConc; });
                            ((ListView)lvPrep.ListView).Refresh();
                        }
                    }
                }
                else if (View.Id == "ReagentPrepLog_DetailView_MicroMedia" && e.OldValue != e.NewValue)
                {
                    ReagentPrepLog objPrepLog = (ReagentPrepLog)e.Object;
                    if (e.PropertyName == "VendorStock")
                    {
                        if (objPrepLog.VendorStock != null)
                        {
                            objPrepLog.LabStock = null;
                            objPrepLog.LLT = null;
                            objPrepLog.LLTExpDate = null;
                        }
                    }
                    else if (e.PropertyName == "LabStock")
                    {
                        if (objPrepLog.LabStock != null)
                        {
                            objPrepLog.VendorStock = null;
                            objPrepLog.LT = null;
                            objPrepLog.LTExpDate = null;
                        }
                    }
                    else if (e.PropertyName == "Solvent")
                    {
                        if (objPrepLog.Solvent)
                        {
                            DisableMicromediaSolvent(false);
                        }
                        else
                        {
                            DisableMicromediaSolvent(true);
                        }
                    }
                }
                else if (View.Id == "NPReagentPrepLog_DetailView_Chemistry" && e.OldValue != e.NewValue && e.Object.GetType() == typeof(NPReagentPrepLog))
                {
                    NPReagentPrepLog objPrepLog = (NPReagentPrepLog)e.Object;
                    if (objPrepLog != null)
                    {
                        if (e.PropertyName == "Solvent")
                        {
                            if (objPrepLog.Solvent)
                            {
                                DisableControles(false);
                            }
                            else
                            {
                                DisableControles(true);
                            }
                        }
                        else if (e.PropertyName == "VendorStock")
                        {
                            if (objPrepLog.VendorStock != null)
                            {
                                objPrepLog.LabStock = null;
                                objPrepLog.LLT = null;
                                objPrepLog.LLTExpDate = null;
                            }
                        }
                        else if (e.PropertyName == "LabStock")
                        {
                            if (objPrepLog.LabStock != null)
                            {
                                objPrepLog.VendorStock = null;
                                objPrepLog.LT = null;
                                objPrepLog.LTExpDate = null;
                                ASPxBooleanPropertyEditor aSPxPropertySolvent = ((DetailView)View).FindItem("Solvent") as ASPxBooleanPropertyEditor;
                                if (aSPxPropertySolvent != null)
                                {
                                    aSPxPropertySolvent.AllowEdit.SetItemValue("AlowEdit", false);
                                }
                            }
                            else
                            {
                                ASPxBooleanPropertyEditor aSPxPropertySolvent = ((DetailView)View).FindItem("Solvent") as ASPxBooleanPropertyEditor;
                                if (aSPxPropertySolvent != null)
                                {
                                    aSPxPropertySolvent.AllowEdit.SetItemValue("AlowEdit", true);
                                }
                            }

                        }
                        else if (e.PropertyName == "InitialVolTaken_V1_Units" || e.PropertyName == "StockConc_C1_Units" || e.PropertyName == "FinalVol_V2_Units"
                            || e.PropertyName == "FinalConc_C2_Units" || e.PropertyName == "CalculationApproach" || e.PropertyName == "FinalConc_C2" || e.PropertyName == "FinalVol_V2"
                            || e.PropertyName == "StockConc_C1" || e.PropertyName == "VolumeTaken_V1")
                        {
                            if (objPrepLog.CalculationApproach != null && !objPrepLog.CalculationApproach.Approach.Contains("None"))
                            {
                                NCalc.Expression exp;
                                if (objPrepLog.CalculationApproach.Approach.Contains("V1"))
                                {
                                    DisableV1andC1Units(true, false, View);
                                    if (objPrepLog.InitialVolTaken_V1_Units != null && objPrepLog.StockConc_C1_Units != null && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                                    {
                                        List<RegentPrepCalculationEditor> lstCalculation = View.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                                        if (lstCalculation.Count > 0)
                                        {
                                            string str = objPrepLog.CalculationApproach.Approach.Split(' ').ToList().LastOrDefault().ToString();
                                            RegentPrepCalculationEditor objCalculationEditor = lstCalculation.FirstOrDefault(i => i.V1Units != null && i.V2Units != null && i.C1Units != null && i.C2Units != null && i.V1Units == objPrepLog.InitialVolTaken_V1_Units && i.V2Units == objPrepLog.FinalVol_V2_Units && i.C1Units == objPrepLog.StockConc_C1_Units && i.C2Units == objPrepLog.FinalConc_C2_Units);
                                            if (objCalculationEditor != null)
                                            {
                                                string Type = string.Empty;
                                                if (objPrepLog.Formula != objCalculationEditor.Formula)
                                                {
                                                    objPrepLog.Formula = objCalculationEditor.Formula;
                                                    if (!string.IsNullOrEmpty(objPrepLog.Formula))
                                                    {

                                                        DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str, View);
                                                        if (str == "C1")
                                                        {
                                                            objPrepLog.StockConc_C1 = null;
                                                            ASPxStringPropertyEditor StockConc_C1 = ((DetailView)View).FindItem("StockConc_C1") as ASPxStringPropertyEditor;
                                                            if (StockConc_C1 != null)
                                                            {
                                                                StockConc_C1.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (StockConc_C1 != null && StockConc_C1.Editor != null)
                                                                {
                                                                    StockConc_C1.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                        else if (str == "C2")
                                                        {
                                                            objPrepLog.FinalConc_C2 = null;
                                                            ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)View).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                                            if (FinalConc_C2 != null)
                                                            {
                                                                FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                                                {
                                                                    FinalConc_C2.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                        else if (str == "V1")
                                                        {
                                                            objPrepLog.VolumeTaken_V1 = null;
                                                            ASPxStringPropertyEditor VolumeTaken_V1 = ((DetailView)View).FindItem("VolumeTaken_V1") as ASPxStringPropertyEditor;
                                                            if (VolumeTaken_V1 != null)
                                                            {
                                                                VolumeTaken_V1.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (VolumeTaken_V1 != null && VolumeTaken_V1.Editor != null)
                                                                {
                                                                    VolumeTaken_V1.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                        else if (str == "V2")
                                                        {
                                                            objPrepLog.FinalVol_V2 = null;
                                                            ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)View).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                                            if (FinalVol_V2 != null)
                                                            {
                                                                FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                                                {
                                                                    FinalVol_V2.Editor.BackColor = Color.Yellow;
                                                                }

                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                objPrepLog.Formula = string.Empty;
                                            }
                                            if (!string.IsNullOrEmpty(objPrepLog.Formula) && (e.PropertyName == "FinalConc_C2" || e.PropertyName == "FinalVol_V2" || e.PropertyName == "StockConc_C1" || e.PropertyName == "VolumeTaken_V1"))
                                            {

                                                List<string> lstFormula = objCalculationEditor.Formula.Split('=').ToList();
                                                if (lstFormula.Count == 2)
                                                {
                                                    bool IsCalculate = true;
                                                    if (lstOperators == null)
                                                    {
                                                        ArithematicOperators();
                                                    }
                                                    //string[] arryOperator = lstFormula[1].Split(lstOperators.ToArray());
                                                    string strFormula = string.Empty;
                                                    string strValue = string.Empty;
                                                    foreach (string objSymbol in lstFormula[1].Split('('))
                                                    {
                                                        int Len = objSymbol.IndexOf(")");
                                                        if (Len > 0)
                                                        {
                                                            strValue = objSymbol.Substring(Len + 1);
                                                            if (string.IsNullOrEmpty(strFormula))
                                                            {
                                                                strFormula = strValue;
                                                            }
                                                            else
                                                            {
                                                                strFormula = strFormula + strValue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strFormula = objSymbol;
                                                        }

                                                    }
                                                    string[] arryOperator = strFormula.Split(lstOperators.ToArray());
                                                    foreach (string obj in arryOperator.Distinct().ToList())
                                                    {
                                                        if (obj.Contains("C1"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.StockConc_C1) || objPrepLog.StockConc_C1 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("V1"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.VolumeTaken_V1) || objPrepLog.VolumeTaken_V1 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("C2"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.FinalConc_C2) || objPrepLog.FinalConc_C2 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("V2"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.FinalVol_V2) || objPrepLog.FinalVol_V2 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("Purity"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.Purity) || objPrepLog.Purity == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (string.IsNullOrEmpty(objPrepLog.Density) || obj.Contains("Density"))
                                                        {
                                                            if (objPrepLog.Density == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (string.IsNullOrEmpty(objPrepLog.MW) || obj.Contains("MW"))
                                                        {
                                                            if (objPrepLog.MW == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                    }
                                                    if (objCalculationEditor != null && IsCalculate == true)
                                                    {

                                                        exp = new NCalc.Expression(strFormula.ToLower());
                                                        foreach (string obj in strFormula.Split(lstOperators.ToArray()))
                                                        {
                                                            if (obj == "C1")
                                                            {
                                                                if (double.TryParse(objPrepLog.StockConc_C1.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["c1"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "C2")
                                                            {
                                                                if (double.TryParse(objPrepLog.FinalConc_C2.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["c2"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "V1")
                                                            {
                                                                if (double.TryParse(objPrepLog.VolumeTaken_V1.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["v1"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "V2")
                                                            {
                                                                if (double.TryParse(objPrepLog.FinalVol_V2.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["v2"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "Purity")
                                                            {
                                                                if (double.TryParse(objPrepLog.Purity.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["purity"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "Density")
                                                            {
                                                                if (double.TryParse(objPrepLog.Density.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["density"] = newval;
                                                                }
                                                            }
                                                        }
                                                        if (!str.Contains(e.PropertyName))
                                                        {
                                                            if (lstFormula[0].Contains("V1"))
                                                            {
                                                                objPrepLog.VolumeTaken_V1 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("V2"))
                                                            {
                                                                objPrepLog.FinalVol_V2 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("C1"))
                                                            {
                                                                objPrepLog.StockConc_C1 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("C2"))
                                                            {
                                                                objPrepLog.FinalConc_C2 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }

                                }
                                else if (objPrepLog.CalculationApproach.Approach.Contains("W1"))
                                {
                                    DisableV1andC1Units(false, true, View);
                                    if (objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                                    {
                                        List<RegentPrepCalculationEditor> lstCalculation = View.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                                        if (lstCalculation.Count > 0)
                                        {
                                            string str = objPrepLog.CalculationApproach.Approach.Split(' ').ToList().LastOrDefault().ToString();
                                            RegentPrepCalculationEditor objCalculationEditor = lstCalculation.FirstOrDefault(i => i.C2Units != null && i.V2Units != null && i.C2Units == objPrepLog.FinalConc_C2_Units && i.V2Units == objPrepLog.FinalVol_V2_Units);
                                            if (objCalculationEditor != null)
                                            {
                                                string Type = string.Empty;
                                                if (objPrepLog.Formula != objCalculationEditor.Formula)
                                                {
                                                    objPrepLog.Formula = objCalculationEditor.Formula;
                                                    if (!string.IsNullOrEmpty(objPrepLog.Formula))
                                                    {
                                                        DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str, View);
                                                        if (str == "C2")
                                                        {
                                                            objPrepLog.FinalConc_C2 = null;
                                                            ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)View).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                                            if (FinalConc_C2 != null)
                                                            {
                                                                FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                                                {
                                                                    FinalConc_C2.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                        else if (str == "W1")
                                                        {
                                                            objPrepLog.VolumeTaken_V1 = null;
                                                            ASPxStringPropertyEditor Weight_g_w1 = ((DetailView)View).FindItem("Weight_g_w1") as ASPxStringPropertyEditor;
                                                            if (Weight_g_w1 != null)
                                                            {
                                                                Weight_g_w1.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (Weight_g_w1 != null && Weight_g_w1.Editor != null)
                                                                {
                                                                    Weight_g_w1.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                        else if (str == "V2")
                                                        {
                                                            objPrepLog.FinalVol_V2 = null;
                                                            ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)View).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                                            if (FinalVol_V2 != null)
                                                            {
                                                                FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                                                if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                                                {
                                                                    FinalVol_V2.Editor.BackColor = Color.Yellow;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                objPrepLog.Formula = string.Empty;
                                            }
                                            if (!string.IsNullOrEmpty(objPrepLog.Formula) && (e.PropertyName == "FinalConc_C2" || e.PropertyName == "FinalVol_V2" || e.PropertyName == "StockConc_C1" || e.PropertyName == "VolumeTaken_V1"))
                                            {

                                                List<string> lstFormula = objCalculationEditor.Formula.Split('=').ToList();
                                                if (lstFormula.Count == 2)
                                                {
                                                    bool IsCalculate = true;
                                                    if (lstOperators == null)
                                                    {
                                                        ArithematicOperators();
                                                    }
                                                    //string[] arryOperator = lstFormula[1].Split(lstOperators.ToArray());
                                                    string strFormula = string.Empty;
                                                    string strValue = string.Empty;
                                                    foreach (string objSymbol in lstFormula[1].Split('('))
                                                    {
                                                        int Len = objSymbol.IndexOf(")");
                                                        if (Len > 0)
                                                        {
                                                            strValue = objSymbol.Substring(Len + 1);
                                                            if (string.IsNullOrEmpty(strFormula))
                                                            {
                                                                strFormula = strValue;
                                                            }
                                                            else
                                                            {
                                                                strFormula = strFormula + strValue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strFormula = objSymbol;
                                                        }
                                                    }
                                                    string[] arryOperator = strFormula.Split(lstOperators.ToArray());
                                                    foreach (string obj in arryOperator.Distinct().ToList())
                                                    {
                                                        if (obj.Contains("C1"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.StockConc_C1) || objPrepLog.StockConc_C1 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("V1"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.VolumeTaken_V1) || objPrepLog.VolumeTaken_V1 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("C2"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.FinalConc_C2) || objPrepLog.FinalConc_C2 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("V2"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.FinalVol_V2) || objPrepLog.FinalVol_V2 == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("Purity"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.Purity) || objPrepLog.Purity == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("Density"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.Density) || objPrepLog.Density == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                        else if (obj.Contains("MW"))
                                                        {
                                                            if (string.IsNullOrEmpty(objPrepLog.MW) || objPrepLog.MW == null)
                                                            {
                                                                IsCalculate = false;
                                                                return;
                                                            }
                                                        }
                                                    }
                                                    if (objCalculationEditor != null && IsCalculate == true)
                                                    {
                                                        exp = new NCalc.Expression(strFormula.ToLower());
                                                        foreach (string obj in strFormula.Split(lstOperators.ToArray()))
                                                        {
                                                            if (obj == "C1")
                                                            {
                                                                if (double.TryParse(objPrepLog.StockConc_C1.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["c1"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "C2")
                                                            {
                                                                if (double.TryParse(objPrepLog.FinalConc_C2.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["c2"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "V1")
                                                            {
                                                                if (double.TryParse(objPrepLog.VolumeTaken_V1.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["v1"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "V2")
                                                            {
                                                                if (double.TryParse(objPrepLog.FinalVol_V2.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["v2"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "Purity")
                                                            {
                                                                if (double.TryParse(objPrepLog.Purity.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["purity"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "Density")
                                                            {
                                                                if (double.TryParse(objPrepLog.Density.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["density"] = newval;
                                                                }
                                                            }
                                                            else if (obj == "MW")
                                                            {
                                                                if (double.TryParse(objPrepLog.Density.ToString(), out double newval))
                                                                {
                                                                    exp.Parameters["mw"] = newval;
                                                                }
                                                            }
                                                        }
                                                        if (!str.Contains(e.PropertyName))
                                                        {
                                                            if (lstFormula[0].Contains("V1"))
                                                            {
                                                                objPrepLog.VolumeTaken_V1 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("V2"))
                                                            {
                                                                objPrepLog.FinalVol_V2 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("C1"))
                                                            {
                                                                objPrepLog.StockConc_C1 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }
                                                            else if (lstFormula[0].Contains("C2"))
                                                            {
                                                                objPrepLog.FinalConc_C2 = Convert.ToString(Math.Round(Convert.ToDecimal(exp.Evaluate()), 3));
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objPrepLog.Formula = string.Empty;
                                foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible))
                                {
                                    if (item.GetType() == typeof(ASPxStringPropertyEditor))
                                    {
                                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.ForeColor = Color.Black;

                                        }

                                    }
                                    else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                                    {
                                        ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }

                                    }
                                    else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                                    {
                                        ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            if (propertyEditor.Id == "Component")
                                            {
                                                propertyEditor.Editor.BackColor = Color.LightGray;
                                            }
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                                    {
                                        ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }

                                    }
                                    else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                                    {
                                        ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            if (propertyEditor.Id == "LLTExpDate" || propertyEditor.Id == "LTExpDate")
                                            {
                                                propertyEditor.Editor.BackColor = Color.LightGray;
                                            }
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }

                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DisableV1andC1Units(bool enable,bool edit,DevExpress.ExpressApp.View view)
        {
            try
            {
                ASPxLookupPropertyEditor aSPxPropertyInitialVolTaken_V1_Units = ((DetailView)view).FindItem("InitialVolTaken_V1_Units") as ASPxLookupPropertyEditor;
                if (aSPxPropertyInitialVolTaken_V1_Units != null)
                {
                    aSPxPropertyInitialVolTaken_V1_Units.AllowEdit.SetItemValue("AlowEdit", enable);
                    if (aSPxPropertyInitialVolTaken_V1_Units != null && aSPxPropertyInitialVolTaken_V1_Units.Editor != null && enable==false)
                    {
                        aSPxPropertyInitialVolTaken_V1_Units.DropDownEdit.DropDown.BackColor = Color.LightGray;
                        }
                    else
                        {
                        aSPxPropertyInitialVolTaken_V1_Units.DropDownEdit.DropDown.BackColor = Color.White;
                    }
                }
                ASPxLookupPropertyEditor aSPxPropertyStockConc_C1_Units = ((DetailView)view).FindItem("StockConc_C1_Units") as ASPxLookupPropertyEditor;
                if (aSPxPropertyStockConc_C1_Units != null)
                {
                    aSPxPropertyStockConc_C1_Units.AllowEdit.SetItemValue("AlowEdit", enable);
                    if (aSPxPropertyStockConc_C1_Units != null && aSPxPropertyStockConc_C1_Units.Editor != null && enable==false)
                    {
                        aSPxPropertyStockConc_C1_Units.DropDownEdit.DropDown.BackColor = Color.LightGray;
                    }
                    else
                        {
                        aSPxPropertyStockConc_C1_Units.DropDownEdit.DropDown.BackColor = Color.White;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyWeight_g_w1 = ((DetailView)view).FindItem("Weight_g_w1") as ASPxStringPropertyEditor;
                if (aSPxPropertyWeight_g_w1 != null)
                            {
                    aSPxPropertyWeight_g_w1.AllowEdit.SetItemValue("AlowEdit", edit);
                                }
                            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                        } 
                    }
        private void DisableMicromediaSolvent(bool enable)
                    {
            try
                        {
                foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible))
                            {
                    if (item.Id== "Weight_g_w1" || item.Id== "VolumeTaken_V1" || item.Id== "FinalVol_V2" ||item.Id== "PHCriteria"
                        ||item.Id== "PHCriteria" ||item.Id== "PH" ||item.Id== "Autoclave" ||item.Id== "FilterSterilization"
                        ||item.Id== "SporeGrowth" ||item.Id== "PositiveControl" ||item.Id== "NegativeControl")
                                {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                                {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            propertyEditor.AllowEdit.SetItemValue("AllowEdit", enable);
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null&& enable)
                                {
                                propertyEditor.Editor.BackColor = Color.White;
                                }
                            else
                                {
                                propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                            }
                        else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                        {
                            ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                            propertyEditor.AllowEdit.SetItemValue("AllowEdit", enable);
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null && enable)
                            {
                                propertyEditor.Editor.BackColor = Color.White;
                        }
                            else
                            {
                                propertyEditor.Editor.BackColor = Color.LightGray;
                    }
                }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            propertyEditor.AllowEdit.SetItemValue("AllowEdit", enable);
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null && enable)
                    {
                                propertyEditor.Editor.BackColor = Color.White;
                    }
                            else
                    {
                                propertyEditor.Editor.BackColor = Color.LightGray;
                            }
                        }
                    }

                }
                    }
            catch(Exception ex)
                    {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
                    }

        private void Application_DetailViewCreating(object sender, DetailViewCreatingEventArgs e)
        {
            try
            {
                //if (e.ViewID == "ReagentPrepLog_DetailView_Chemistry")
                //        {
                //    Application.DetailViewCreating -= Application_DetailViewCreating;
                //    IObjectSpace os = Application.CreateObjectSpace();
                //    ReagentPrepLog newObjPrepLog = os.CreateObject<ReagentPrepLog>();
                //    newObjPrepLog.ComponentID = 1;
                //    e.View = Application.CreateDetailView(os, "ReagentPrepLog_DetailView_Chemistry", false, newObjPrepLog);
                //    e.View.ViewEditMode = ViewEditMode.Edit;
                //    if (RPInfo.lstReagentPrepLog == null)
                //                {
                //        RPInfo.lstReagentPrepLog = new List<NPReagentPrepLog>();
                //        //RPInfo.lstReagentPrepLog.Add(newObjPrepLog);
                //                }
                //                else
                //                {
                //       // RPInfo.lstReagentPrepLog.Add(newObjPrepLog);
                //    }
                //                }                                 
                //else if (e.ViewID != null && e.ViewID == "ReagentPrepLog_DetailView_MicroMedia")
                //            {
                //    Application.DetailViewCreating -= Application_DetailViewCreating;
                //    IObjectSpace os = Application.CreateObjectSpace();
                //    ReagentPrepLog newObjPrepLog = os.CreateObject<ReagentPrepLog>();
                //    newObjPrepLog.ComponentID = 1;
                //    e.View = Application.CreateDetailView(os, "ReagentPrepLog_DetailView_MicroMedia", false, newObjPrepLog);
                //    e.View.ViewEditMode = ViewEditMode.Edit;
                //    if (RPInfo.lstReagentPrepLog == null)
                //               {
                //        //RPInfo.lstReagentPrepLog = new List<ReagentPrepLog>();
                //        //RPInfo.lstReagentPrepLog.Add(newObjPrepLog);
                //                 }
                //                 else
                //                 {
                //        //RPInfo.lstReagentPrepLog.Add(newObjPrepLog);
                //                 }
	               //            }
                //else if (e.ViewID != null && e.ViewID == "ReagentPrepLog_DetailView_Chemistry_PrepNotepad")
                //{
                //    Application.DetailViewCreating -= Application_DetailViewCreating;
                //    IObjectSpace os = Application.CreateObjectSpace();
                //    Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objReagentPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                //    ReagentPrepLog objPrepLog = os.FindObject<ReagentPrepLog>(CriteriaOperator.Parse("[ReagentPreparation]=? And [ComponentID]=1", objReagentPrep.Oid));
                //    if (objPrepLog!=null)
                //    {
                //        e.View = Application.CreateDetailView(os, "ReagentPrepLog_DetailView_Chemistry_PrepNotepad", false, objPrepLog); 
                //    }
                //    else
                //    {
                //        ListPropertyEditor lvReagetPrepLog = ((DetailView)Application.MainWindow.View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                //        if(lvReagetPrepLog!=null)
                //        {
                //            objPrepLog = ((ListView)lvReagetPrepLog.ListView).CollectionSource.List.Cast<ReagentPrepLog>().Where(i => i.ComponentID == 1).FirstOrDefault();
                //        }
                //        e.View = Application.CreateDetailView(Application.MainWindow.View.ObjectSpace, "ReagentPrepLog_DetailView_Chemistry_PrepNotepad", false, objPrepLog);
                //    }
                //    e.View.ViewEditMode = ViewEditMode.Edit;
                //}
                //else if (e.ViewID != null && e.ViewID == "ReagentPrepLog_DetailView_MicroMedia_PrepNotePad")
                //{
                //    Application.DetailViewCreating -= Application_DetailViewCreating;
                //    IObjectSpace os = Application.CreateObjectSpace();
                //    Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objReagentPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                //    ReagentPrepLog objPrepLog = os.FindObject<ReagentPrepLog>(CriteriaOperator.Parse("[ReagentPreparation]=? And [ComponentID]=1", objReagentPrep.Oid));
                //    e.View = Application.CreateDetailView(os, "ReagentPrepLog_DetailView_MicroMedia_PrepNotePad", false, objPrepLog);
                //    e.View.ViewEditMode = ViewEditMode.Edit;
                //}
                if(e.ViewID!=null && e.ViewID== "NPReagentPrepLog_DetailView_Chemistry")
                {
                    Application.DetailViewCreating -= Application_DetailViewCreating;
                    IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                    NPReagentPrepLog newObj=os.CreateObject<NPReagentPrepLog>();
                    if (Application.MainWindow.View is DetailView)
                    {
                        ListPropertyEditor lvReagetPrepLog = ((DetailView)Application.MainWindow.View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                        if (lvReagetPrepLog != null)
                        {
                            ReagentPrepLog objPrepLog = ((ListView)lvReagetPrepLog.ListView).CollectionSource.List.Cast<ReagentPrepLog>().Where(i => i.ComponentID == 1).FirstOrDefault();
                            ChangePrepLog(newObj, objPrepLog,"Chemistry");
                        } 
                    }
                    else
                    {
                        newObj.ComponentID = 1;
                    }
                    e.View = Application.CreateDetailView(os, "NPReagentPrepLog_DetailView_Chemistry", false, newObj);
                    e.View.ViewEditMode = ViewEditMode.Edit;
                }
                else if (e.ViewID != null && e.ViewID == "NPReagentPrepLog_DetailView_MicroMedia")
                {
                    Application.DetailViewCreating -= Application_DetailViewCreating;
                    IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                    NPReagentPrepLog newObj = os.CreateObject<NPReagentPrepLog>();
                    if (Application.MainWindow.View is DetailView)
                    {
                        ListPropertyEditor lvReagetPrepLog = ((DetailView)Application.MainWindow.View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                        if (lvReagetPrepLog != null)
                        {
                            ReagentPrepLog objPrepLog = ((ListView)lvReagetPrepLog.ListView).CollectionSource.List.Cast<ReagentPrepLog>().Where(i => i.ComponentID == 1).FirstOrDefault();
                            ChangePrepLog(newObj, objPrepLog,"Micro");
                        }
                    }
                    else
                    {
                        newObj.ComponentID = 1;
                    }
                    e.View = Application.CreateDetailView(os, "NPReagentPrepLog_DetailView_MicroMedia", false, newObj);
                    e.View.ViewEditMode = ViewEditMode.Edit;
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ChangePrepLog(NPReagentPrepLog NPval, ReagentPrepLog val,string Type)
        {
            try
            {
                NPval.ComponentID = val.ComponentID;
                NPval.VendorStock = val.VendorStock;
                NPval.LT = val.LT;
                NPval.LabStock = val.LabStock;
                NPval.LLT = val.LLT;
                NPval.Formula = val.Formula;
                NPval.Comment = val.Comment;
                NPval.FinalVol_V2 = val.FinalVol_V2;
                NPval.VolumeTaken_V1 = val.VolumeTaken_V1;
                NPval.Weight_g_w1 = val.Weight_g_w1;
                NPval.Solvent = val.Solvent;
                if (Type=="Chemistry")
                {
                    NPval.CalculationApproach = val.CalculationApproach;
                    NPval.InitialVolTaken_V1_Units = val.InitialVolTaken_V1_Units;
                    NPval.StockConc_C1_Units = val.StockConc_C1_Units;
                    NPval.FinalVol_V2_Units = val.FinalVol_V2_Units;
                    NPval.FinalConc_C2_Units = val.FinalConc_C2_Units;
                    NPval.Density = val.Density;
                    NPval.Purity = val.Purity;
                    NPval.Constant = val.Constant;
                    NPval.StockConc_C1 = val.StockConc_C1;
                    NPval.FinalConc_C2 = val.FinalConc_C2;
                    NPval.LTExpDate = val.LTExpDate;
                    NPval.LLTExpDate = val.LLTExpDate;
                   
                    NPval.EqWt = val.EqWt;
                    NPval.MW = val.MW; 
                }
                else
                {
                    NPval.Autoclave = val.Autoclave;
                    NPval.FilterSterilization = val.FilterSterilization;
                    NPval.PHCriteria = val.PHCriteria;
                    NPval.PH = val.PH;
                    NPval.PositiveControl = val.PositiveControl;
                    NPval.NegativeControl = val.NegativeControl;
                    NPval.SporeGrowth = val.SporeGrowth;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ChangePrepLog(NPReagentPrepLog NPval, NPReagentPrepLog val,string Type)
        {
            try
            {
                NPval.ComponentID = val.ComponentID;
                NPval.VendorStock = val.VendorStock;
                NPval.LT = val.LT;
                NPval.LabStock = val.LabStock;
                NPval.LLT = val.LLT;
                NPval.Formula = val.Formula;
                NPval.Comment = val.Comment;
                NPval.FinalVol_V2 = val.FinalVol_V2;
                NPval.VolumeTaken_V1 = val.VolumeTaken_V1;
                NPval.Weight_g_w1 = val.Weight_g_w1;
                NPval.Solvent = val.Solvent;
                if (Type == "Chemistry")
                {
                    NPval.CalculationApproach = val.CalculationApproach;
                    NPval.InitialVolTaken_V1_Units = val.InitialVolTaken_V1_Units;
                    NPval.StockConc_C1_Units = val.StockConc_C1_Units;
                    NPval.FinalVol_V2_Units = val.FinalVol_V2_Units;
                    NPval.FinalConc_C2_Units = val.FinalConc_C2_Units;
                    NPval.Density = val.Density;
                    NPval.Purity = val.Purity;
                    NPval.Constant = val.Constant;
                    NPval.StockConc_C1 = val.StockConc_C1;
                    NPval.FinalConc_C2 = val.FinalConc_C2;
                    NPval.LTExpDate = val.LTExpDate;
                    NPval.LLTExpDate = val.LLTExpDate;

                    NPval.EqWt = val.EqWt;
                    NPval.MW = val.MW;
                }
                else
                {
                    NPval.Autoclave = val.Autoclave;
                    NPval.FilterSterilization = val.FilterSterilization;
                    NPval.PHCriteria = val.PHCriteria;
                    NPval.PH = val.PH;
                    NPval.PositiveControl = val.PositiveControl;
                    NPval.NegativeControl = val.NegativeControl;
                    NPval.SporeGrowth = val.SporeGrowth;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ChangePrepLog(DataRow dr, NPReagentPrepLog NPval, string Type)
        {
            try
            {
                dr["ID"] = NPval.ComponentID;
                if (NPval.VendorStock != null)
                {
                    dr["VendorStock"] = NPval.VendorStock.items;
                    dr["VendorStockOid"] = NPval.VendorStock.Oid;
                }
                else
                {
                    dr["VendorStock"] = null;
                    dr["VendorStockOid"] = Guid.Empty;
                }
                if (NPval.LT != null)
                {
                    dr["LT#"] = NPval.LT.LT;
                    dr["LT#Oid"] = NPval.LT.Oid;
                }
                else
                {
                    dr["LT#"] = null;
                    dr["LT#Oid"] = Guid.Empty;
                }
                if (NPval.LabStock != null)
                {
                    dr["LabStock"] = NPval.LabStock.STandardName;
                    dr["LabStockOid"] = NPval.LabStock.Oid;
                }
                else
                {
                    dr["LabStock"] = null;
                    dr["LabStockOid"] = Guid.Empty;
                }
                if (NPval.LLT != null)
                {
                    dr["LLT#"] = NPval.LLT.LLT;
                    dr["LLT#Oid"] = NPval.LLT.Oid;
                }
                else
                {
                    dr["LLT#"] = null;
                    dr["LLT#Oid"] = Guid.Empty;
                }
                dr["Solvent"] = NPval.Solvent;
                if (NPval.LTExpDate != null && NPval.LTExpDate != DateTime.MinValue)
                {
                    dr["LT#ExpDate"] = NPval.LTExpDate;
                }
                else
                {
                    dr["LT#ExpDate"] = DBNull.Value;
                }
                if (NPval.LLTExpDate != null && NPval.LLTExpDate != DateTime.MinValue)
                {
                    dr["LLT#ExpDate"] = NPval.LLTExpDate;
                }
                else
                {
                    dr["LLT#ExpDate"] = DBNull.Value;
                }
                if (Type=="Chemistry")
                {
                    if (NPval.CalculationApproach != null)
                    {
                        dr["CalculationApproach"] = NPval.CalculationApproach.Approach;
                        dr["CalculationApproachOid"] = NPval.CalculationApproach.Oid;
                    }
                    else
                    {
                        dr["CalculationApproach"] = null;
                        dr["CalculationApproachOid"] = Guid.Empty;
                    }
                    if (NPval.FinalVol_V2_Units != null)
                    {
                        dr["FinalVol(V2)Units"] = NPval.FinalVol_V2_Units.Units;
                        dr["FinalVol(V2)UnitsOid"] = NPval.FinalVol_V2_Units.Oid;
                    }
                    else
                    {
                        dr["FinalVol(V2)Units"] = null;
                        dr["FinalVol(V2)UnitsOid"] = Guid.Empty;
                    }
                    dr["Formula"] = NPval.Formula;
                    dr["Purity(%)"] = NPval.Purity;
                    dr["Density"] = NPval.Density;
                    dr["Constant"] = NPval.Constant;
                    dr["StockConc(C1)"] = NPval.StockConc_C1;
                    dr["Weight(g)(W1)"] = NPval.Weight_g_w1;
                    dr["VolumeTaken(V1)"] = NPval.VolumeTaken_V1;
                    dr["FinalVol(V2)"] = NPval.FinalVol_V2;
                    if (NPval.StockConc_C1_Units != null)
                    {
                        dr["StockConc(C1)Units"] = NPval.StockConc_C1_Units.Units;
                        dr["StockConc(C1)UnitsOid"] = NPval.StockConc_C1_Units.Oid;
                    }
                    else
                    {
                        dr["StockConc(C1)Units"] = null;
                        dr["StockConc(C1)UnitsOid"] = Guid.Empty;
                    }
                   
                    dr["FinalConc(C2)"] = NPval.FinalConc_C2;
                    if (NPval.InitialVolTaken_V1_Units != null)
                    {
                        dr["InitialVolTaken(V1)Units"] = NPval.InitialVolTaken_V1_Units.Units;
                        dr["InitialVolTaken(V1)UnitsOid"] = NPval.InitialVolTaken_V1_Units.Oid;
                    }
                    else
                    {
                        dr["InitialVolTaken(V1)Units"] = null;
                        dr["InitialVolTaken(V1)UnitsOid"] = Guid.Empty;
                    }
                    if (NPval.FinalConc_C2_Units != null)
                    {
                        dr["FinalConc(C2)Units"] = NPval.FinalConc_C2_Units.Units;
                        dr["FinalConc(C2)UnitsOid"] = NPval.FinalConc_C2_Units.Oid;
                    }
                    else
                    {
                        dr["FinalConc(C2)Units"] = null;
                        dr["FinalConc(C2)UnitsOid"] = Guid.Empty;
                    }
                    dr["MW"] = NPval.MW;
                    dr["EqWt"] = NPval.EqWt; 
                }
                else
                {
                    dr["Autoclave(Y/N)"] = NPval.Autoclave;
                    dr["FilterSterilization(Y/N)"] = NPval.FilterSterilization;
                    dr["-Control"] = NPval.NegativeControl;
                    dr["PH"] = NPval.PH;
                    dr["PHCriteria"] = NPval.PHCriteria;
                    dr["+Control"] = NPval.PositiveControl;
                    dr["SporeGrowth(Y/N)"] = NPval.SporeGrowth;
                    dr["WtTaken(g)"] = NPval.Weight_g_w1;
                    dr["VolumeTaken(ml)"] = NPval.VolumeTaken_V1;
                    dr["FinalVolume(ml)"] = NPval.FinalVol_V2;
                }
               
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ClearReagentPrepChecmical()
        {
            DashboardViewItem dvReagentPrepLog = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
            NPReagentPrepLog newObjPrepLog = (NPReagentPrepLog)dvReagentPrepLog.InnerView.CurrentObject;
            if (newObjPrepLog != null)
            {
                newObjPrepLog.VendorStock = null;
                newObjPrepLog.LT = null;
                newObjPrepLog.LabStock = null;
                newObjPrepLog.LLT = null;
                newObjPrepLog.CalculationApproach = null;
                newObjPrepLog.InitialVolTaken_V1_Units = null;
                newObjPrepLog.StockConc_C1_Units = null;
                newObjPrepLog.FinalVol_V2_Units = null;
                newObjPrepLog.FinalConc_C2_Units = null;
                newObjPrepLog.Density = null;
                newObjPrepLog.Purity = null;
                newObjPrepLog.Constant = null;
                newObjPrepLog.Weight_g_w1 = null;
                newObjPrepLog.VolumeTaken_V1 = null;
                newObjPrepLog.StockConc_C1 = null;
                newObjPrepLog.FinalVol_V2 = null;
                newObjPrepLog.FinalConc_C2 = null;
                newObjPrepLog.LTExpDate = null;
                newObjPrepLog.LLTExpDate = null;
                newObjPrepLog.Formula = null;
                newObjPrepLog.Comment = null;
                newObjPrepLog.EqWt = null;
                newObjPrepLog.MW = null;
                dvReagentPrepLog.InnerView.Refresh();
            }
        }
        private void ClearReagentPrepMicroMedia()
        {
            DashboardViewItem dvReagentPrepLog = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
            NPReagentPrepLog newObjPrepLog = (NPReagentPrepLog)dvReagentPrepLog.InnerView.CurrentObject;
            if (newObjPrepLog != null)
            {
                newObjPrepLog.VendorStock = null;
                newObjPrepLog.LT = null;
                newObjPrepLog.LabStock = null;
                newObjPrepLog.LLT = null;
                newObjPrepLog.Weight_g_w1 = null;
                newObjPrepLog.VolumeTaken_V1 = null;
                newObjPrepLog.FinalVol_V2 = null;
                newObjPrepLog.PHCriteria = null;
                newObjPrepLog.PH = null;
                newObjPrepLog.Autoclave = null;
                newObjPrepLog.FilterSterilization = null;
                newObjPrepLog.SporeGrowth = null;
                newObjPrepLog.PositiveControl = null;
                newObjPrepLog.NegativeControl = null;
                newObjPrepLog.LTExpDate = null;
                newObjPrepLog.LLTExpDate = null;
                newObjPrepLog.Comment = null;
                dvReagentPrepLog.InnerView.Refresh();
            }
        }
        private void ChangePrepLog(NPReagentPrepLog NPval, DataRow dr,IObjectSpace os,string Type)
        {
            try
            {
                NPval.ComponentID = Convert.ToUInt16(dr["ID"]);
                if (dr["VendorStockOid"] != null && new Guid(dr["VendorStockOid"].ToString())!=Guid.Empty)
                {
                    NPval.VendorStock=os.GetObjectByKey<Items>(dr["VendorStockOid"]);
                }
                else
                {
                    NPval.VendorStock = null;
                }
                if (dr["LT#Oid"] != null && new Guid(dr["LT#Oid"].ToString()) != Guid.Empty)
                {
                    NPval.LT =os.GetObjectByKey<Distribution> (dr["LT#Oid"]);
                }
                else
                {
                    NPval.LT = null;
                }
                if (dr["LabStockOid"] != null && new Guid(dr["LabStockOid"].ToString()) != Guid.Empty)
                {
                    NPval.LabStock = os.GetObjectByKey<StandardName>(dr["LabStockOid"]);
                }
                else
                {
                    NPval.LabStock = null;
                }
                if (dr["LLT#Oid"] != null && new Guid(dr["LLT#Oid"].ToString()) != Guid.Empty)
                {
                    NPval.LLT = os.GetObjectByKey<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>(dr["LLT#Oid"]);
                }
                else
                {
                    NPval.LLT = null;
                }
                NPval.Solvent = Convert.ToBoolean(dr["Solvent"]);
                if (dr["LT#ExpDate"] != null && dr["LT#ExpDate"] != DBNull.Value && Convert.ToDateTime(dr["LT#ExpDate"]) != DateTime.MinValue)
                {
                    NPval.LTExpDate = Convert.ToDateTime(dr["LT#ExpDate"]);
                }
                else
                {
                    NPval.LTExpDate = null;
                }
                if (dr["LLT#ExpDate"] != null && dr["LLT#ExpDate"] != DBNull.Value && Convert.ToDateTime(dr["LLT#ExpDate"]) != DateTime.MinValue)
                {
                    NPval.LLTExpDate = Convert.ToDateTime(dr["LLT#ExpDate"]);
                }
                else
                {
                    NPval.LLTExpDate = null;
                }
                if (Type=="Chemistry")
                {
                    if (dr["CalculationApproachOid"] != null && new Guid(dr["CalculationApproachOid"].ToString()) != Guid.Empty)
                    {
                        NPval.CalculationApproach = os.GetObjectByKey<CalculationApproach>(dr["CalculationApproachOid"]);
                    }
                    else
                    {
                        NPval.CalculationApproach = null;
                    }
                    if (dr["FinalVol(V2)UnitsOid"] != null && new Guid(dr["FinalVol(V2)UnitsOid"].ToString()) != Guid.Empty)
                    {
                        NPval.Cal_FinalVol_V2_Units = os.GetObjectByKey<ReagentUnits>(dr["FinalVol(V2)UnitsOid"]);
                    }
                    else
                    {
                        NPval.Cal_FinalVol_V2_Units = null;
                    }
                   
                    NPval.StockConc_C1 = Convert.ToString(dr["StockConc(C1)"]);
                    NPval.VolumeTaken_V1 = Convert.ToString(dr["VolumeTaken(V1)"]);
                    NPval.FinalVol_V2 = Convert.ToString(dr["FinalVol(V2)"]);
                    NPval.Purity = Convert.ToString(dr["Purity(%)"]);
                    NPval.Density = Convert.ToString(dr["Density"]);
                    NPval.Constant = Convert.ToString(dr["Constant"]);
                    NPval.Formula = Convert.ToString(dr["Formula"]);
                    if (dr["StockConc(C1)UnitsOid"] != null && new Guid(dr["StockConc(C1)UnitsOid"].ToString()) != Guid.Empty)
                    {
                        NPval.StockConc_C1_Units = os.GetObjectByKey<ReagentUnits>(dr["StockConc(C1)UnitsOid"]);
                    }
                    else
                    {
                        NPval.StockConc_C1_Units = null;
                    }
                    NPval.Weight_g_w1 = Convert.ToString(dr["Weight(g)(W1)"]);
                    NPval.FinalConc_C2 = Convert.ToString(dr["FinalConc(C2)"]);
                    if (dr["InitialVolTaken(V1)UnitsOid"] != null && new Guid(dr["InitialVolTaken(V1)UnitsOid"].ToString()) != Guid.Empty)
                    {
                        NPval.InitialVolTaken_V1_Units = os.GetObjectByKey<ReagentUnits>(dr["InitialVolTaken(V1)UnitsOid"]);
                    }
                    else
                    {
                        NPval.InitialVolTaken_V1_Units = null;
                    }
                    if (dr["FinalConc(C2)UnitsOid"] != null && new Guid(dr["FinalConc(C2)UnitsOid"].ToString()) != Guid.Empty)
                    {
                        NPval.FinalConc_C2_Units = os.GetObjectByKey<ReagentUnits>(dr["FinalConc(C2)UnitsOid"]);
                    }
                    else
                    {
                        NPval.FinalConc_C2_Units = null;
                    }
                    NPval.MW = Convert.ToString(dr["MW"]);
                    NPval.EqWt = Convert.ToString(dr["EqWt"]); 
                }
                else
                {

                    NPval.Autoclave=Convert.ToString(dr["Autoclave(Y/N)"]);
                    NPval.FilterSterilization=Convert.ToString(dr["FilterSterilization(Y/N)"]);
                    NPval.NegativeControl=Convert.ToString(dr["-Control"]);
                    NPval.PH=Convert.ToString(dr["PH"]);
                    NPval.PHCriteria=Convert.ToString(dr["PHCriteria"]);
                    NPval.PositiveControl=Convert.ToString(dr["+Control"]);
                    NPval.SporeGrowth = Convert.ToString(dr["SporeGrowth(Y/N)"]);
                    NPval.Weight_g_w1 = Convert.ToString(dr["WtTaken(g)"]);
                    NPval.VolumeTaken_V1 = Convert.ToString(dr["VolumeTaken(ml)"]);
                    NPval.FinalVol_V2 = Convert.ToString(dr["FinalVolume(ml)"]);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ChangePrepLog(ReagentPrepLog NPval, DataRow dr, IObjectSpace os,string Type)
        {
            try
            {
                NPval.ComponentID = Convert.ToUInt16(dr["ID"]);
                if (dr["VendorStockOid"] != null && new Guid(dr["VendorStockOid"].ToString()) != Guid.Empty)
                {
                    NPval.VendorStock = os.GetObjectByKey<Items>(dr["VendorStockOid"]);
                }
                else
                {
                    NPval.VendorStock = null;
                }
                if (dr["LT#Oid"] != null && new Guid(dr["LT#Oid"].ToString()) != Guid.Empty)
                {
                    NPval.LT = os.GetObjectByKey<Distribution>(dr["LT#Oid"]);
                }
                else
                {
                    NPval.LT = null;
                }
                if (dr["LabStockOid"] != null && new Guid(dr["LabStockOid"].ToString()) != Guid.Empty)
                {
                    NPval.LabStock = os.GetObjectByKey<StandardName>(dr["LabStockOid"]);
                }
                else
                {
                    NPval.LabStock = null;
                }
                if (dr["LLT#Oid"] != null && new Guid(dr["LLT#Oid"].ToString()) != Guid.Empty)
                {
                    NPval.LLT = os.GetObjectByKey<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>(dr["LLT#Oid"]);
                }
                else
                {
                    NPval.LLT = null;
                }
                NPval.Solvent = Convert.ToBoolean(dr["Solvent"]);
                if (dr["LT#ExpDate"] != null && dr["LT#ExpDate"] != DBNull.Value && Convert.ToDateTime(dr["LT#ExpDate"]) != DateTime.MinValue)
                {
                    NPval.LTExpDate = Convert.ToDateTime(dr["LT#ExpDate"]);
                }
                else
                {
                    NPval.LTExpDate = null;
                }
                if (dr["LLT#ExpDate"] != null && dr["LLT#ExpDate"] != DBNull.Value && Convert.ToDateTime(dr["LLT#ExpDate"]) != DateTime.MinValue)
                {
                    NPval.LLTExpDate = Convert.ToDateTime(dr["LLT#ExpDate"]);
                }
                else
                {
                    NPval.LLTExpDate = null;
                }
                if (Type=="Chemistry")
                {
                    if (dr["CalculationApproachOid"] != null && new Guid(dr["CalculationApproachOid"].ToString()) != Guid.Empty)
                    {
                        NPval.CalculationApproach = os.GetObjectByKey<CalculationApproach>(dr["CalculationApproachOid"]);
                    }
                    else
                    {
                        NPval.CalculationApproach = null;
                    }
                    if (dr["FinalVol(V2)UnitsOid"] != null && new Guid(dr["FinalVol(V2)UnitsOid"].ToString()) != Guid.Empty)
                    {
                        NPval.FinalVol_V2_Units = os.GetObjectByKey<ReagentUnits>(dr["FinalVol(V2)UnitsOid"]);
                    }
                    else
                    {
                        NPval.FinalVol_V2_Units = null;
                    }
                    NPval.StockConc_C1 = Convert.ToString(dr["StockConc(C1)"]);
                    NPval.Purity = Convert.ToString(dr["Purity(%)"]);
                    NPval.Density = Convert.ToString(dr["Density"]);
                    NPval.Constant = Convert.ToString(dr["Constant"]);
                    NPval.Weight_g_w1 = Convert.ToString(dr["Weight(g)(W1)"]);
                    NPval.VolumeTaken_V1 = Convert.ToString(dr["VolumeTaken(V1)"]);
                    NPval.FinalVol_V2 = Convert.ToString(dr["FinalVol(V2)"]);
                    NPval.Formula = Convert.ToString(dr["Formula"]);
                    if (dr["StockConc(C1)UnitsOid"] != null && new Guid(dr["StockConc(C1)UnitsOid"].ToString()) != Guid.Empty)
                    {
                        NPval.StockConc_C1_Units = os.GetObjectByKey<ReagentUnits>(dr["StockConc(C1)UnitsOid"]);
                    }
                    else
                    {
                        NPval.StockConc_C1_Units = null;
                    }
                    NPval.FinalConc_C2 = Convert.ToString(dr["FinalConc(C2)"]);
                    if (dr["InitialVolTaken(V1)UnitsOid"] != null && new Guid(dr["InitialVolTaken(V1)UnitsOid"].ToString()) != Guid.Empty)
                    {
                        NPval.InitialVolTaken_V1_Units = os.GetObjectByKey<ReagentUnits>(dr["InitialVolTaken(V1)UnitsOid"]);
                    }
                    else
                    {
                        NPval.InitialVolTaken_V1_Units = null;
                    }
                    if (dr["FinalConc(C2)UnitsOid"] != null && new Guid(dr["FinalConc(C2)UnitsOid"].ToString()) != Guid.Empty)
                    {
                        NPval.FinalConc_C2_Units = os.GetObjectByKey<ReagentUnits>(dr["FinalConc(C2)UnitsOid"]);
                    }
                    else
                    {
                        NPval.FinalConc_C2_Units = null;
                    }
                    NPval.MW = Convert.ToString(dr["MW"]);
                    NPval.EqWt = Convert.ToString(dr["EqWt"]); 
                }
                else
                {
                    NPval.Autoclave = Convert.ToString(dr["Autoclave(Y/N)"]);
                    NPval.FilterSterilization = Convert.ToString(dr["FilterSterilization(Y/N)"]);
                    NPval.NegativeControl = Convert.ToString(dr["-Control"]);
                    NPval.PH = Convert.ToString(dr["PH"]);
                    NPval.PHCriteria = Convert.ToString(dr["PHCriteria"]);
                    NPval.PositiveControl = Convert.ToString(dr["+Control"]);
                    NPval.SporeGrowth = Convert.ToString(dr["SporeGrowth(Y/N)"]);
                    NPval.Weight_g_w1 = Convert.ToString(dr["WtTaken(g)"]);
                    NPval.VolumeTaken_V1 = Convert.ToString(dr["VolumeTaken(ml)"]);
                    NPval.FinalVol_V2 = Convert.ToString(dr["FinalVolume(ml)"]);
                }
               
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DisableControles(bool enbstat)
        {
            try
            {
                ASPxStringPropertyEditor aSPxPropertyFormula = ((DetailView)View).FindItem("Formula") as ASPxStringPropertyEditor;
                if(aSPxPropertyFormula != null)
                {
                    aSPxPropertyFormula.AllowEdit.SetItemValue("AlowEdit", enbstat);
                    if (aSPxPropertyFormula != null&& enbstat==false && aSPxPropertyFormula.Editor != null)
                    {
                        aSPxPropertyFormula.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxLookupPropertyEditor aSPxPropertyCA= ((DetailView)View).FindItem("CalculationApproach") as ASPxLookupPropertyEditor;
                if (aSPxPropertyCA != null)
                {
                    aSPxPropertyCA.AllowEdit.SetItemValue("AlowEdit", enbstat);
                    if (aSPxPropertyCA != null && enbstat == false && aSPxPropertyCA.Editor != null)
                    {
                        aSPxPropertyCA.DropDownEdit.DropDown.BackColor = Color.LightGray;
                    }
                }
                ASPxLookupPropertyEditor aSPxPropertyStockConc_C1_Units = ((DetailView)View).FindItem("StockConc_C1_Units") as ASPxLookupPropertyEditor;
                if (aSPxPropertyStockConc_C1_Units != null)
                {
                    aSPxPropertyStockConc_C1_Units.AllowEdit.SetItemValue("AlowEdit", enbstat);
                    if (aSPxPropertyStockConc_C1_Units != null && enbstat == false && aSPxPropertyStockConc_C1_Units.Editor != null)
                    {
                        aSPxPropertyStockConc_C1_Units.DropDownEdit.DropDown.BackColor = Color.LightGray;
                    }
                }
                ASPxLookupPropertyEditor aSPxPropertyInitialVolTaken_V1_Units = ((DetailView)View).FindItem("InitialVolTaken_V1_Units") as ASPxLookupPropertyEditor;
                if (aSPxPropertyInitialVolTaken_V1_Units != null)
                {
                    aSPxPropertyInitialVolTaken_V1_Units.AllowEdit.SetItemValue("AlowEdit", enbstat);
                    if (aSPxPropertyInitialVolTaken_V1_Units != null && enbstat == false && aSPxPropertyInitialVolTaken_V1_Units.Editor != null)
                    {
                        aSPxPropertyInitialVolTaken_V1_Units.DropDownEdit.DropDown.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyFinalVol_C2_Units = ((DetailView)View).FindItem("FinalVol_C2_Units") as ASPxStringPropertyEditor;
                if (aSPxPropertyFinalVol_C2_Units != null)
                {
                    aSPxPropertyFinalVol_C2_Units.AllowEdit.SetItemValue("AlowEdit", enbstat);
                    if (aSPxPropertyFinalVol_C2_Units != null && enbstat == false && aSPxPropertyFinalVol_C2_Units.Editor != null)
                    {
                        aSPxPropertyFinalVol_C2_Units.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxIntPropertyEditor aSPxPropertyVolumeTaken_V1 = ((DetailView)View).FindItem("VolumeTaken_V1") as ASPxIntPropertyEditor;
                if (aSPxPropertyVolumeTaken_V1 != null)
                {
                    aSPxPropertyVolumeTaken_V1.AllowEdit.SetItemValue("AlowEdit", enbstat);
                    if (aSPxPropertyVolumeTaken_V1 != null && enbstat == false && aSPxPropertyVolumeTaken_V1.Editor != null)
                    {
                        aSPxPropertyVolumeTaken_V1.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxIntPropertyEditor aSPxPropertyWeight_g_w1 = ((DetailView)View).FindItem("Weight_g_w1") as ASPxIntPropertyEditor;
                if (aSPxPropertyWeight_g_w1 != null)
                {
                    aSPxPropertyWeight_g_w1.AllowEdit.SetItemValue("AlowEdit", enbstat);
                    if (aSPxPropertyWeight_g_w1 != null && enbstat == false && aSPxPropertyWeight_g_w1.Editor != null)
                    {
                        aSPxPropertyWeight_g_w1.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxIntPropertyEditor aSPxPropertyFinalConc_C2 = ((DetailView)View).FindItem("FinalConc_C2") as ASPxIntPropertyEditor;
                if (aSPxPropertyFinalConc_C2 != null)
                {
                    aSPxPropertyFinalConc_C2.AllowEdit.SetItemValue("AlowEdit", enbstat);
                    if (aSPxPropertyFinalConc_C2 != null && enbstat == false && aSPxPropertyFinalConc_C2.Editor != null)
                    {
                        aSPxPropertyFinalConc_C2.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxLookupPropertyEditor aSPxPropertyFinalVol_V2_Units = ((DetailView)View).FindItem("FinalVol_V2_Units") as ASPxLookupPropertyEditor;
                if (aSPxPropertyFinalVol_V2_Units != null)
                {
                    aSPxPropertyFinalVol_V2_Units.AllowEdit.SetItemValue("AlowEdit", enbstat);
                    if (aSPxPropertyFinalVol_V2_Units != null && enbstat == false && aSPxPropertyFinalVol_V2_Units.Editor != null)
                    {
                        aSPxPropertyFinalVol_V2_Units.DropDownEdit.DropDown.BackColor = Color.LightGray;
                    }
                }
                if(View is DetailView)
                {
                    foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible))
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.White;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.White;
                            }
                        }

                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                {
                                    propertyEditor.FindEdit.Editor.BackColor = Color.White;
                                }
                                else if (propertyEditor.DropDownEdit != null)
                                {
                                    propertyEditor.DropDownEdit.DropDown.BackColor = Color.White;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.White;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.White;
                            }
                        }

                    }
                }

            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DisableControlsInFormulaBased(string formula,string value,DevExpress.ExpressApp.View view)
        {
            try
            {
                ASPxStringPropertyEditor aSPxPropertyVolumeTaken_V1 = ((DetailView)view).FindItem("VolumeTaken_V1") as ASPxStringPropertyEditor;
                if (aSPxPropertyVolumeTaken_V1 != null && formula.Contains("V1"))
                {
                    aSPxPropertyVolumeTaken_V1.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyVolumeTaken_V1 != null && aSPxPropertyVolumeTaken_V1.Editor != null)
                    {
                        aSPxPropertyVolumeTaken_V1.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if(aSPxPropertyVolumeTaken_V1 != null && !formula.Contains("V1") && !value.Contains("V1"))
                {
                    aSPxPropertyVolumeTaken_V1.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyVolumeTaken_V1 != null && aSPxPropertyVolumeTaken_V1.Editor != null)
                    {
                        aSPxPropertyVolumeTaken_V1.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyWeight_g_w1 = ((DetailView)view).FindItem("Weight_g_w1") as ASPxStringPropertyEditor;
                if (aSPxPropertyWeight_g_w1 != null && formula.Contains("W1"))
                {
                    aSPxPropertyWeight_g_w1.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyWeight_g_w1 != null && aSPxPropertyWeight_g_w1.Editor != null)
                    {
                        aSPxPropertyWeight_g_w1.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if (aSPxPropertyWeight_g_w1 != null && !formula.Contains("W1") && !value.Contains("W1"))
                {
                    aSPxPropertyWeight_g_w1.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyWeight_g_w1 != null && aSPxPropertyWeight_g_w1.Editor != null)
                    {
                        aSPxPropertyWeight_g_w1.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyStockConc_C1 = ((DetailView)view).FindItem("StockConc_C1") as ASPxStringPropertyEditor;
                if (aSPxPropertyStockConc_C1 != null && formula.Contains("C1"))
                {
                    aSPxPropertyStockConc_C1.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyStockConc_C1 != null && aSPxPropertyStockConc_C1.Editor != null)
                    {
                        aSPxPropertyStockConc_C1.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if (aSPxPropertyStockConc_C1 != null && !formula.Contains("C1") && !value.Contains("C1"))
                {
                    aSPxPropertyStockConc_C1.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyStockConc_C1 != null && aSPxPropertyStockConc_C1.Editor != null)
                    {
                        aSPxPropertyStockConc_C1.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyFinalConc_C2 = ((DetailView)view).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                if (aSPxPropertyFinalConc_C2 != null && formula.Contains("C2"))
                {
                    aSPxPropertyFinalConc_C2.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyFinalConc_C2 != null&& aSPxPropertyFinalConc_C2.Editor != null)
                    {
                        aSPxPropertyFinalConc_C2.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if (aSPxPropertyFinalConc_C2 != null && !formula.Contains("C2") && !value.Contains("C2"))
                {
                    aSPxPropertyFinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyFinalConc_C2 != null && aSPxPropertyFinalConc_C2.Editor != null)
                    {
                        aSPxPropertyFinalConc_C2.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyFinalVol_V2_Units = ((DetailView)view).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                if (aSPxPropertyFinalVol_V2_Units != null && formula.Contains("V2"))
                {
                    aSPxPropertyFinalVol_V2_Units.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyFinalVol_V2_Units != null &&  aSPxPropertyFinalVol_V2_Units.Editor != null)
                    {
                        aSPxPropertyFinalVol_V2_Units.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if (aSPxPropertyFinalVol_V2_Units != null && !formula.Contains("V2") && !value.Contains("V2"))
                {
                    aSPxPropertyFinalVol_V2_Units.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyFinalVol_V2_Units != null && aSPxPropertyFinalVol_V2_Units.Editor != null)
                    {
                        aSPxPropertyFinalVol_V2_Units.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyPurity = ((DetailView)view).FindItem("Purity") as ASPxStringPropertyEditor;
                if (aSPxPropertyPurity != null && formula.Contains("Purity"))
                {
                    aSPxPropertyPurity.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyPurity != null && aSPxPropertyPurity.Editor != null)
                    {
                        aSPxPropertyPurity.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if (aSPxPropertyPurity != null && !formula.Contains("Purity") && !formula.Contains(value))
                {
                    aSPxPropertyPurity.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyPurity != null && aSPxPropertyPurity.Editor != null)
                    {
                        aSPxPropertyPurity.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyDensity = ((DetailView)view).FindItem("Density") as ASPxStringPropertyEditor;
                if (aSPxPropertyDensity != null && formula.Contains("Density"))
                {
                    aSPxPropertyDensity.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyDensity != null && aSPxPropertyDensity.Editor != null)
                    {
                        aSPxPropertyDensity.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if (aSPxPropertyDensity != null && !formula.Contains("Density") && !formula.Contains(value))
                {
                    aSPxPropertyDensity.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyDensity != null && aSPxPropertyDensity.Editor != null)
                    {
                        aSPxPropertyDensity.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyMW= ((DetailView)view).FindItem("MW") as ASPxStringPropertyEditor;
                if (aSPxPropertyMW != null && formula.Contains("MW"))
                {
                    aSPxPropertyMW.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyMW != null && aSPxPropertyMW.Editor != null)
                    {
                        aSPxPropertyMW.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if (aSPxPropertyMW != null && !formula.Contains("MW") && !formula.Contains(value))
                {
                    aSPxPropertyMW.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyMW != null && aSPxPropertyMW.Editor != null)
                    {
                        aSPxPropertyMW.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyConstant = ((DetailView)view).FindItem("Constant") as ASPxStringPropertyEditor;
                if (aSPxPropertyConstant != null && formula.Contains("Constant"))
                {
                    aSPxPropertyConstant.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyConstant != null && aSPxPropertyMW.Editor != null)
                    {
                        aSPxPropertyConstant.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if (aSPxPropertyConstant != null && !formula.Contains("Constant") && !formula.Contains(value))
                {
                    aSPxPropertyConstant.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyConstant != null && aSPxPropertyMW.Editor != null)
                    {
                        aSPxPropertyConstant.Editor.BackColor = Color.LightGray;
                    }
                }
                ASPxStringPropertyEditor aSPxPropertyEqWt = ((DetailView)view).FindItem("EqWt") as ASPxStringPropertyEditor;
                if (aSPxPropertyEqWt != null && formula.Contains("EqWt"))
                {
                    aSPxPropertyEqWt.AllowEdit.SetItemValue("AlowEdit", true);
                    if (aSPxPropertyEqWt != null && aSPxPropertyMW.Editor != null)
                    {
                        aSPxPropertyEqWt.Editor.BackColor = Color.LightYellow;
                    }
                }
                else if (aSPxPropertyEqWt != null && !formula.Contains("EqWt") && !formula.Contains(value))
                {
                    aSPxPropertyEqWt.AllowEdit.SetItemValue("AlowEdit", false);
                    if (aSPxPropertyEqWt != null && aSPxPropertyMW.Editor != null)
                    {
                        aSPxPropertyEqWt.Editor.BackColor = Color.LightGray;
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += XafPopupWindowControl_CustomizePopupWindowSize;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void XafPopupWindowControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "ReagentPrepChemical" ||e.PopupFrame.View.Id== "ReagentPrepMicroMedia" || e.PopupFrame.View.Id == "ReagentPrepChemical_PrepNotepad" ||e.PopupFrame.View.Id== "ReagentPrepMicroMedia_PrepNotepad")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1500);
                    e.Height = new System.Web.UI.WebControls.Unit(648);
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void NewObjectAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if(View.Id== "ReagentPreparation_ListView" || View.Id == "ReagentPreparation_DetailView_Chemistry" || View.Id == "ReagentPreparation_DetailView_MicroMedia")
                {
                    e.Cancel = true;
                    IObjectSpace os = Application.CreateObjectSpace();
                    DashboardView dashboard = Application.CreateDashboardView(os, "ReagentPrepClassify", false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //showViewParameters.CreatedView.Caption = "Result Correction Formula";
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += Dc_Accepting;
                    //dc.AcceptAction.Executed += AcceptAction_Executed;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        //{
        //   try
        //    {

        //    }
        //    catch(Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.Id== "ReagentPreparation_ReagentPrepLogs_ListView_Calibration")
                {
                    if (e.AcceptActionArgs.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                    else if (e.AcceptActionArgs.SelectedObjects.Count > 1)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    DialogController dc = sender as DialogController;
                    if (RPInfo.ActiveTabText== "Copy Previous")
                    {
                        DashboardViewItem lvReagent = ((DashboardView)dc.Frame.View).FindItem("CopyPrevious") as DashboardViewItem;
                        if(lvReagent!=null && lvReagent.InnerView!=null && lvReagent.InnerView.SelectedObjects.Count==1)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objOldReagent =os.GetObject((Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)lvReagent.InnerView.CurrentObject);
                            if(objOldReagent!=null)
                            {
                                Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objNewReagent = os.CreateObject<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>();
                                if (objOldReagent.PrepSelectType==PrepSelectTypes.ChemicalReagentPrep)
                                {
                                    objNewReagent.PrepSelectType = PrepSelectTypes.ChemicalReagentPrep;
                                    objNewReagent.PrepType =objOldReagent.PrepType;
                                    objNewReagent.PrepName =objOldReagent.PrepName;
                                    objNewReagent.Test =objOldReagent.Test;
                                    objNewReagent.Storage =objOldReagent.Storage;
                                    objNewReagent.Solvent = objOldReagent.Solvent;
                                    objNewReagent.SolventID = objOldReagent.SolventID;
                                    objNewReagent.ExpirationDate = DateTime.Now;
                                    objNewReagent.PreparedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    foreach (ReagentPrepLog objOldReagentPreplog in objOldReagent.ReagentPrepLogs.ToList())
                                    {
                                        ReagentPrepLog ObjNew = os.CreateObject<ReagentPrepLog>();
                                        ObjNew.ComponentID = objOldReagentPreplog.ComponentID;
                                        ObjNew.VendorStock = objOldReagentPreplog.VendorStock;
                                        ObjNew.LT = objOldReagentPreplog.LT;
                                        ObjNew.LabStock = objOldReagentPreplog.LabStock;
                                        ObjNew.LLT = objOldReagentPreplog.LLT;
                                        ObjNew.CalculationApproach = objOldReagentPreplog.CalculationApproach;
                                        ObjNew.InitialVolTaken_V1_Units = objOldReagentPreplog.InitialVolTaken_V1_Units;
                                        ObjNew.StockConc_C1_Units = objOldReagentPreplog.StockConc_C1_Units;
                                        ObjNew.FinalConc_C2_Units = objOldReagentPreplog.FinalConc_C2_Units;
                                        ObjNew.FinalVol_V2_Units = objOldReagentPreplog.FinalVol_V2_Units;
                                        ObjNew.Purity = objOldReagentPreplog.Purity;
                                        ObjNew.Density = objOldReagentPreplog.Density;
                                        ObjNew.MW = objOldReagentPreplog.MW;
                                        ObjNew.EqWt = objOldReagentPreplog.EqWt;
                                        ObjNew.Constant = objOldReagentPreplog.Constant;
                                        ObjNew.Solvent = objOldReagentPreplog.Solvent;
                                        ObjNew.LTExpDate = objOldReagentPreplog.LTExpDate;
                                        ObjNew.LLTExpDate = objOldReagentPreplog.LLTExpDate;
                                        ObjNew.Formula = objOldReagentPreplog.Formula;
                                        ObjNew.Weight_g_w1 = objOldReagentPreplog.Weight_g_w1;
                                        ObjNew.VolumeTaken_V1 = objOldReagentPreplog.VolumeTaken_V1;
                                        ObjNew.StockConc_C1 = objOldReagentPreplog.StockConc_C1;
                                        ObjNew.FinalVol_V2 = objOldReagentPreplog.FinalVol_V2;
                                        ObjNew.FinalConc_C2 = objOldReagentPreplog.FinalConc_C2;
                                        ObjNew.Comment = objOldReagentPreplog.Comment;
                                        objNewReagent.ReagentPrepLogs.Add(ObjNew);
                                    }
                                }
                                else if(objOldReagent.PrepSelectType == PrepSelectTypes.CalibrationSetPrep)
                                {
                                    objNewReagent.PrepSelectType = PrepSelectTypes.CalibrationSetPrep;
                                    objNewReagent.SelectStockSolution = objOldReagent.SelectStockSolution;
                                    objNewReagent.StockConc = objOldReagent.StockConc;
                                    objNewReagent.Comment = objOldReagent.Comment;
                                    objNewReagent.PrepName = objOldReagent.PrepName;
                                    objNewReagent.PrepType = objOldReagent.PrepType;
                                    objNewReagent.Solvent = objOldReagent.Solvent;
                                    objNewReagent.SolventID = objOldReagent.SolventID;
                                    objNewReagent.PreparedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objNewReagent.LabStock = objOldReagent.LabStock;
                                    objNewReagent.LLT = objOldReagent.LLT;
                                    objNewReagent.StockConcUnit = objOldReagent.StockConcUnit;
                                    objNewReagent.NoOfLevels = objOldReagent.NoOfLevels;
                                    objNewReagent.Department = objOldReagent.Department;
                                    objNewReagent.Storage = objOldReagent.Storage;
                                    foreach (ReagentPrepLog objOldReagentPreplog in objOldReagent.ReagentPrepLogs.ToList())
                                    {
                                        ReagentPrepLog ObjNew = os.CreateObject<ReagentPrepLog>();
                                        ObjNew.ComponentID = objOldReagentPreplog.ComponentID;
                                        ObjNew.WorkingStdName = objOldReagentPreplog.WorkingStdName;
                                        ObjNew.StockStdName = objOldReagentPreplog.StockStdName;
                                        ObjNew.Cal_FinalConc_C2 = objOldReagentPreplog.Cal_FinalConc_C2;
                                        ObjNew.WSCons_Units = objOldReagentPreplog.WSCons_Units;
                                        ObjNew.Cal_VolumeTaken_V1 = objOldReagentPreplog.Cal_VolumeTaken_V1;
                                        ObjNew.Cal_VolTaken_V1_Units = objOldReagentPreplog.Cal_VolTaken_V1_Units;
                                        ObjNew.Cal_FinalVol_V2 = objOldReagentPreplog.Cal_FinalVol_V2;
                                        ObjNew.Cal_FinalVol_V2_Units = objOldReagentPreplog.Cal_FinalVol_V2_Units;
                                        ObjNew.Cal_FinalConc_C2 = objOldReagentPreplog.Cal_FinalConc_C2;
                                        ObjNew.Cal_FinalConc_C2_Units = objOldReagentPreplog.Cal_FinalConc_C2_Units;
                                        ObjNew.Formula = objOldReagentPreplog.Formula;
                                        ObjNew.Comment = objOldReagentPreplog.Comment;
                                        objNewReagent.ReagentPrepLogs.Add(ObjNew);
                                    }
                                }
                                else if(objOldReagent.PrepSelectType == PrepSelectTypes.MicroMediaAndReagentPrep)
                                {
                                    objNewReagent.PrepSelectType = PrepSelectTypes.MicroMediaAndReagentPrep;
                                    objNewReagent.PrepType = objOldReagent.PrepType;
                                    objNewReagent.PrepName = objOldReagent.PrepName;
                                    objNewReagent.Test = objOldReagent.Test;
                                    objNewReagent.Storage = objOldReagent.Storage;
                                    objNewReagent.Solvent = objOldReagent.Solvent;
                                    objNewReagent.SolventID = objOldReagent.SolventID;
                                    objNewReagent.PreparedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    foreach (ReagentPrepLog objOldReagentPreplog in objOldReagent.ReagentPrepLogs.ToList())
                                    {
                                        ReagentPrepLog ObjNew = os.CreateObject<ReagentPrepLog>();
                                        ObjNew.ComponentID = objOldReagentPreplog.ComponentID;
                                        ObjNew.VendorStock = objOldReagentPreplog.VendorStock;
                                        ObjNew.LT = objOldReagentPreplog.LT;
                                        ObjNew.LabStock = objOldReagentPreplog.LabStock;
                                        ObjNew.LLT = objOldReagentPreplog.LLT;
                                        ObjNew.Solvent = objOldReagentPreplog.Solvent;
                                        ObjNew.LTExpDate = objOldReagentPreplog.LTExpDate;
                                        ObjNew.LLTExpDate = objOldReagentPreplog.LLTExpDate;
                                        ObjNew.Weight_g_w1 = objOldReagentPreplog.Weight_g_w1;
                                        ObjNew.VolumeTaken_V1 = objOldReagentPreplog.VolumeTaken_V1;
                                        ObjNew.FinalVol_V2 = objOldReagentPreplog.FinalVol_V2;
                                        ObjNew.Comment = objOldReagentPreplog.Comment;
                                        ObjNew.NegativeControl = objOldReagentPreplog.NegativeControl;
                                        ObjNew.PositiveControl = objOldReagentPreplog.PositiveControl;
                                        ObjNew.PH = objOldReagentPreplog.PH;
                                        ObjNew.PHCriteria = objOldReagentPreplog.PHCriteria;
                                        ObjNew.Autoclave = objOldReagentPreplog.Autoclave;
                                        ObjNew.SporeGrowth = objOldReagentPreplog.SporeGrowth;
                                        ObjNew.FilterSterilization = objOldReagentPreplog.FilterSterilization;
                                        objNewReagent.ReagentPrepLogs.Add(ObjNew);
                                    }
                                }
                                DetailView detailView = null;
                                if (objNewReagent.PrepSelectType == PrepSelectTypes.ChemicalReagentPrep)
                                {
                                    detailView = Application.CreateDetailView(os, "ReagentPreparation_DetailView_Chemistry", true, objNewReagent); 
                                }
                                else if (objNewReagent.PrepSelectType == PrepSelectTypes.CalibrationSetPrep)
                                {
                                    detailView = Application.CreateDetailView(os, "ReagentPreparation_DetailView_Calibration", true, objNewReagent);
                                }
                                else if (objNewReagent.PrepSelectType == PrepSelectTypes.MicroMediaAndReagentPrep)
                                {
                                    detailView = Application.CreateDetailView(os, "ReagentPreparation_DetailView_MicroMedia", true, objNewReagent);
                                }
                                detailView.ViewEditMode = ViewEditMode.Edit;
                                Application.MainWindow.SetView(detailView);
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                      
                        if (dc != null && dc.Frame != null && dc.Frame.View != null)
                        {
                            DashboardViewItem dvSelType = ((DashboardView)dc.Frame.View).FindItem("SelecteType") as DashboardViewItem;
                            if (dvSelType != null && dvSelType.InnerView != null && dvSelType.InnerView.CurrentObject != null)
                            {
                                NonPersistentReagent obj = (NonPersistentReagent)dvSelType.InnerView.CurrentObject;
                                if (obj.PrepSelectType == PrepSelectTypes.ChemicalReagentPrep)
                                {
                                    DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "ReagentPrepChemical", false);
                                    ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                                    showViewParameters.Context = TemplateContext.PopupWindow;
                                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                    showViewParameters.CreatedView.Caption = "Prep Notepad";
                                    e.ShowViewParameters.CreatedView = dashboard;
                                }
                                else if (obj.PrepSelectType == PrepSelectTypes.CalibrationSetPrep)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    Modules.BusinessObjects.ReagentPreparation.ReagentPreparation newReagentPreparation = os.CreateObject<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>();
                                    newReagentPreparation.PrepSelectType = PrepSelectTypes.CalibrationSetPrep;
                                    newReagentPreparation.SelectStockSolution = StockSolution.LabStock;
                                    DetailView detailView = Application.CreateDetailView(os, "ReagentPreparation_DetailView_Calibration", true, newReagentPreparation);
                                    detailView.ViewEditMode = ViewEditMode.Edit;
                                    Application.MainWindow.SetView(detailView);
                                }
                                else if (obj.PrepSelectType == PrepSelectTypes.MicroMediaAndReagentPrep)
                                {
                                    DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "ReagentPrepMicroMedia", false);
                                    ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                                    showViewParameters.Context = TemplateContext.PopupWindow;
                                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                    showViewParameters.CreatedView.Caption = "Prep Notepad";
                                    e.ShowViewParameters.CreatedView = dashboard;
                                }

                            }
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if(View.Id== "ReagentPrepLog_ListView_Chemistry" ||View.Id== "ReagentPreparation_ReagentPrepLogs_ListView" ||View.Id== "ReagentPrepLog_ListView_MicroMedia"
                    ||View.Id== "ReagentPreparation_ReagentPrepLogs_ListView_MicroMedia" ||View.Id== "ReagentPrepLog_ListView_Chemistry_PrepNotepad"
                    ||View.Id== "ReagentPrepLog_ListView_MicroMedia_PrepNotepad")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if(gridListEditor!=null)
                    {
                        gridListEditor.Grid.Settings.HorizontalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;
                        if (gridListEditor.Grid.Columns["VendorStock"] != null)
                        {
                            gridListEditor.Grid.Columns["VendorStock"].Width = 250;
                        }
                        if (gridListEditor.Grid.Columns["LabStock"] != null)
                        {
                            gridListEditor.Grid.Columns["LabStock"].Width = 150;
                        }
                        if (gridListEditor.Grid.Columns["InitialVolTaken(V1)Units"] != null)
                        {
                            gridListEditor.Grid.Columns["InitialVolTaken(V1)Units"].Width = 150;
                        }
                        if (gridListEditor.Grid.Columns["StockConc(C1)Units"] != null)
                        {
                            gridListEditor.Grid.Columns["StockConc(C1)Units"].Width = 130;
                        }
                        if (gridListEditor.Grid.Columns["FinalConc(C2)Units"] != null)
                        {
                            gridListEditor.Grid.Columns["FinalConc(C2)Units"].Width = 130;
                        }
                        if (gridListEditor.Grid.Columns["CalculationApproach"] != null)
                        {
                            gridListEditor.Grid.Columns["CalculationApproach"].Width = 150;
                        }
                        if (gridListEditor.Grid.Columns["Weight(g)(W1)"] != null)
                        {
                            gridListEditor.Grid.Columns["Weight(g)(W1)"].Width = 120;
                        }
                        //gridListEditor.Grid.Settings.VerticalScrollableHeight = 385;
                        //gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        //gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                    }
                }
                else if(View.Id== "ReagentPrepLog_DetailView_Chemistry" ||View.Id== "ReagentPrepLog_DetailView_MicroMedia")
                {
                    if (RPInfo.lstEditorID.Count>0)
                    {
                        foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible && i.GetType() == typeof(ASPxStringPropertyEditor)))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null && RPInfo.lstEditorID.Contains(propertyEditor.Id))
                            {
                                ASPxTextBox editor = (ASPxTextBox)propertyEditor.Editor;
                                if (editor != null)
                                {
                                    editor.ClientSideEvents.KeyPress = @"function(s, e){
                                var regex = /[0-9]|\./;   
                                if (!regex.test(e.htmlEvent.key)) {
                                    e.htmlEvent.returnValue = false;
                                }}";
                                }
                            }

                        } 
                    }
                    ASPxGridLookupPropertyEditor lvVendor = ((DetailView)View).FindItem("VendorStock") as ASPxGridLookupPropertyEditor;
                    if(lvVendor!=null)
                    {
                        if (lvVendor != null && lvVendor.AllowEdit && lvVendor.Editor != null)
                        {
                            ASPxGridLookup editor = (ASPxGridLookup)lvVendor.Editor;
                            if (editor != null)
                            {
                                editor.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                editor.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                editor.GridView.Settings.VerticalScrollableHeight = 300;
                                if (editor.GridView.Columns["ItemName"] != null)
                                {
                                    editor.GridView.Columns["ItemName"].Width = 130;
                                }
                                if (editor.GridView.Columns["Specification"] != null)
                                {
                                    editor.GridView.Columns["Specification"].Width = 150;
                                }
                                if (editor.GridView.Columns["Vendor"] != null)
                                {
                                    editor.GridView.Columns["Vendor"].Width = 130;
                                }
                                if (editor.GridView.Columns["ItemCode"] != null)
                                {
                                    editor.GridView.Columns["ItemCode"].Width = 100;
                                }
                            }
                        }
                    }
                    ReagentPrepLog objPrepLog = (ReagentPrepLog)View.CurrentObject;
                    AppearenceappliedInEditorFormulaBased(objPrepLog,View);
                }
                else if(View.Id== "ReagentPreparation_ReagentPrepLogs_ListView_Calibration")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesController");
                    gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    if (gridListEditor != null && Convert.ToInt32(strscreenwidth) < 1600)
                    {
                        gridListEditor.Grid.Settings.HorizontalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;
                    }
                   if(gridListEditor.Grid.Columns["StockStdName"] !=null)
                    {
                        gridListEditor.Grid.Columns["StockStdName"].Width = 200;
                    }
                    if (gridListEditor.Grid.Columns["WorkingStdName"] != null)
                    {
                        gridListEditor.Grid.Columns["WorkingStdName"].Width = 200;
                    }
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    XafCallbackManager parameter = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    parameter.RegisterHandler("StockStdPopup", this);
                    gridListEditor.Grid.ClientInstanceName = "StockStd";

                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        {
                                if(sessionStorage.getItem('CurrFocusedColumn') == null)
                                {
                                    sessionStorage.setItem('PrevFocusedColumn', e.cellInfo.column.fieldName);
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }
                                else
                                {
                                    var precolumn = sessionStorage.getItem('CurrFocusedColumn');
                                    sessionStorage.setItem('PrevFocusedColumn', precolumn);                           
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }                                 
                        }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() { 
                            var fieldName = sessionStorage.getItem('PrevFocusedColumn');
                            if( s.batchEditApi.HasChanges(e.visibleIndex) && (fieldName == 'Cal_Weight_g_w1' || fieldName== 'Cal_VolumeTaken_V1' || fieldName == 'Cal_FinalVol_V2' ))
                            {
                               var W = s.batchEditApi.GetCellValue(e.visibleIndex, 'Cal_Weight_g_w1');
                               var V = s.batchEditApi.GetCellValue(e.visibleIndex, 'Cal_VolumeTaken_V1');  
                               var F = s.batchEditApi.GetCellValue(e.visibleIndex, 'Cal_FinalVol_V2'); 
                               let  formula =W+'*'+V+'/'+F;
                               formula=formula.replaceAll('null','');
                              s.batchEditApi.SetCellValue(e.visibleIndex, 'Formula',formula); 
                              if (W!=null && V!=null && F!=null)
                               {
                                 var FinalConc=W * V /F;
                                 if (FinalConc === Math.floor(FinalConc))
                                 {
                                     s.batchEditApi.SetCellValue(e.visibleIndex, 'Cal_FinalConc_C2',FinalConc);
                                 }
                                 else
                                 {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Cal_FinalConc_C2',FinalConc.toFixed(3));  
                                 }
		                       
	                           }
                            }
                        
                           
                            }, 20); }";
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');                                
                                    var oid;
                                    var text;
                                   if (FocusedColumn!='ComponentID' && FocusedColumn!='Formula' && FocusedColumn!='SubLLT')
                                 	{
		                                if(FocusedColumn.includes('.'))
                                            {                                       
                                        oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                        text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                        if (e.item.name =='CopyToAllCell')
                                        {
                                            for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                            { 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                {                                               
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                                }
                                            }
                                        }        
                                    }
                                    else
                                    {                                                             
                                        var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                        if (e.item.name =='CopyToAllCell')
                                        {
                                            for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                            { 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                {
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                                }
                                            }
                                        }                            
                                    } 
	}
                                }
                                e.processOnServer = false;
                            }";
                }
                else if(View.Id== "NPSampleFields_ListView_Reagent")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if(gridListEditor!=null && gridListEditor.Grid!=null)
                    {
                        var selectionBoxColumn = gridListEditor.Grid.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                        if (selectionBoxColumn != null)
                        {
                            selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                        }
                    }
                }
                else if(View.Id== "ReagentPreparation_DetailView_Chemistry" ||View.Id== "ReagentPreparation_DetailView_MicroMedia")
                {
                    if(IsEnable==false)
                    {
                        ASPxStringPropertyEditor propertyEditor = ((DetailView)View).FindItem("SolventID") as ASPxStringPropertyEditor;
                        if (propertyEditor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("AlowEdit", false);
                            IsEnable = true;
                        }
                    }
                    ASPxDateTimePropertyEditor propertyEditorDatePrepared = ((DetailView)View).FindItem("DatePrepared") as ASPxDateTimePropertyEditor;
                    if (propertyEditorDatePrepared != null && propertyEditorDatePrepared.Editor != null)
                    {
                        propertyEditorDatePrepared.Editor.CalendarCustomDisabledDate += Editor_CalendarCustomDisabledDate;
                    }
                }
                else if(View.Id== "ReagentPrepChemical" ||View.Id== "ReagentPreparation_ListView" || View.Id== "ReagentPrepMicroMedia")
                {
                    ((IXafPopupWindowControlContainer)WebWindow.CurrentRequestPage).XafPopupWindowControl.CustomizePopupControl += XafPopupWindowControl_CustomizePopupControl;
                }
                else if(View.Id== "ReagentPrepLog_DetailView_Chemistry_PrepNotepad" || View.Id== "ReagentPrepLog_DetailView_MicroMedia_PrepNotePad")
                {
                    if (Application.MainWindow.View is DetailView && ((DetailView)Application.MainWindow.View).ViewEditMode==ViewEditMode.View)
                    {
                        disenbcontrolsinviewmode(false, View);
                    }
                    else
                    {
                        ASPxGridLookupPropertyEditor lvVendor = ((DetailView)View).FindItem("VendorStock") as ASPxGridLookupPropertyEditor;
                        if (lvVendor != null)
                        {
                            if (lvVendor != null && lvVendor.AllowEdit && lvVendor.Editor != null)
                            {
                                ASPxGridLookup editor = (ASPxGridLookup)lvVendor.Editor;
                                if (editor != null)
                                {
                                    editor.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                    editor.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                    editor.GridView.Settings.VerticalScrollableHeight = 300;
                                    if (editor.GridView.Columns["ItemName"] != null)
                                    {
                                        editor.GridView.Columns["ItemName"].Width = 130;
                                    }
                                    if (editor.GridView.Columns["Specification"] != null)
                                    {
                                        editor.GridView.Columns["Specification"].Width = 150;
                                    }
                                    if (editor.GridView.Columns["Vendor"] != null)
                                    {
                                        editor.GridView.Columns["Vendor"].Width = 130;
                                    }
                                    if (editor.GridView.Columns["ItemCode"] != null)
                                    {
                                        editor.GridView.Columns["ItemCode"].Width = 100;
                                    }
                                }
                            }
                        }
                        ReagentPrepLog objPrepLog = (ReagentPrepLog)View.CurrentObject;
                        if (objPrepLog!=null && string.IsNullOrEmpty(objPrepLog.Formula))
                        {
                            foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible))
                            {
                                if (item.GetType() == typeof(ASPxStringPropertyEditor))
                                {
                                    ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        if (propertyEditor.AllowEdit)
                                        {
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }
                                        else
                                        {
                                            propertyEditor.Editor.BackColor = Color.LightGray;
                                        }
                                    }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                    }

                                }
                                else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                                {
                                    ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        if (propertyEditor.AllowEdit)
                                        {
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }
                                        else
                                        {
                                            propertyEditor.Editor.BackColor = Color.LightGray;
                                        }
                                    }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                    }

                                }
                                else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                                {
                                    ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        if (propertyEditor.AllowEdit)
                                        {
                                            if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                            {
                                                propertyEditor.FindEdit.Editor.BackColor = Color.White;
                                            }
                                            else if (propertyEditor.DropDownEdit != null)
                                            {
                                                propertyEditor.DropDownEdit.DropDown.BackColor = Color.White;
                                            }
                                            else
                                            {
                                                propertyEditor.Editor.BackColor = Color.White;
                                            }
                                        }
                                        else
                                        {
                                            if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                            {
                                                propertyEditor.FindEdit.Editor.BackColor = Color.LightGray;
                                            }
                                            else if (propertyEditor.DropDownEdit != null)
                                            {
                                                propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightGray;
                                            }
                                            else
                                            {
                                                propertyEditor.Editor.BackColor = Color.LightGray;
                                            }
                                        }
                                    }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                    }
                                }
                                else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                                {
                                    ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        if (propertyEditor.AllowEdit)
                                        {
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }
                                        else
                                        {
                                            propertyEditor.Editor.BackColor = Color.LightGray;
                                        }
                                    }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                    }
                                }
                                else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                                {
                                    ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        if (propertyEditor.AllowEdit)
                                        {
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }
                                        else
                                        {
                                            propertyEditor.Editor.BackColor = Color.LightGray;
                                        }
                                    }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                    }

                                }
                                else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                                {
                                    ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        if (propertyEditor.AllowEdit)
                                        {
                                            propertyEditor.Editor.BackColor = Color.White;
                                        }
                                        else
                                        {
                                            propertyEditor.Editor.BackColor = Color.LightGray;
                                        }
                                    }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                    }

                                }

                            }
                        }
                        else
                        {
                            if (objPrepLog.CalculationApproach != null)
                            {
                                string str = objPrepLog.CalculationApproach.Approach.Split(' ').ToList().LastOrDefault().ToString();
                                if (objPrepLog.CalculationApproach.Approach.Contains("V1") && objPrepLog.InitialVolTaken_V1_Units != null && objPrepLog.StockConc_C1_Units != null && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                                {
                                    DisableV1andC1Units(true,false,View);
                                    List<RegentPrepCalculationEditor> lstCalculation = View.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                                    if (lstCalculation.Count > 0)
                                    {
                                        DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str,View);
                                        if (str == "C1")
                                        {
                                            objPrepLog.StockConc_C1 = null;
                                            ASPxStringPropertyEditor StockConc_C1 = ((DetailView)View).FindItem("StockConc_C1") as ASPxStringPropertyEditor;
                                            if (StockConc_C1 != null)
                                            {
                                                StockConc_C1.AllowEdit.SetItemValue("AlowEdit", false);
                                                if (StockConc_C1 != null && StockConc_C1.Editor != null)
                                                {
                                                    StockConc_C1.Editor.BackColor = Color.Yellow;
                                                }
                                            }
                                        }
                                        else if (str == "C2")
                                        {
                                            objPrepLog.FinalConc_C2 = null;
                                            ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)View).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                            if (FinalConc_C2 != null)
                                            {
                                                FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                                if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                                {
                                                    FinalConc_C2.Editor.BackColor = Color.Yellow;
                                                }
                                            }
                                        }
                                        else if (str == "V1")
                                        {
                                            objPrepLog.VolumeTaken_V1 = null;
                                            ASPxStringPropertyEditor VolumeTaken_V1 = ((DetailView)View).FindItem("VolumeTaken_V1") as ASPxStringPropertyEditor;
                                            if (VolumeTaken_V1 != null)
                                            {
                                                VolumeTaken_V1.AllowEdit.SetItemValue("AlowEdit", false);
                                                if (VolumeTaken_V1 != null && VolumeTaken_V1.Editor != null)
                                                {
                                                    VolumeTaken_V1.Editor.BackColor = Color.Yellow;
                                                }
                                            }

                                        }
                                        else if (str == "V2")
                                        {
                                            objPrepLog.FinalVol_V2 = null;
                                            ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)View).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                            if (FinalVol_V2 != null)
                                            {
                                                FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                                if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                                {
                                                    FinalVol_V2.Editor.BackColor = Color.Yellow;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (objPrepLog.CalculationApproach.Approach.Contains("W1") && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                                {
                                    DisableV1andC1Units(false,true,View);
                                    List<RegentPrepCalculationEditor> lstCalculation = View.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                                    if (lstCalculation.Count > 0)
                                    {
                                        RegentPrepCalculationEditor objCalculationEditor = lstCalculation.FirstOrDefault(i => i.C2Units != null && i.V2Units != null && i.C2Units == objPrepLog.FinalConc_C2_Units && i.V2Units == objPrepLog.FinalVol_V2_Units);
                                        if (objCalculationEditor != null)
                                        {
                                            DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str,View);
                                            if (str == "C2")
                                            {
                                                objPrepLog.FinalConc_C2 =null;
                                                ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)View).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                                if (FinalConc_C2 != null)
                                                {
                                                    FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                                    if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                                    {
                                                        FinalConc_C2.Editor.BackColor = Color.Yellow;
                                                    }
                                                }
                                            }
                                            else if (str == "W1")
                                            {
                                                objPrepLog.VolumeTaken_V1 =null;
                                                ASPxStringPropertyEditor Weight_g_w1 = ((DetailView)View).FindItem("Weight_g_w1") as ASPxStringPropertyEditor;
                                                if (Weight_g_w1 != null)
                                                {
                                                    Weight_g_w1.AllowEdit.SetItemValue("AlowEdit", false);
                                                    if (Weight_g_w1 != null && Weight_g_w1.Editor != null)
                                                    {
                                                        Weight_g_w1.Editor.BackColor = Color.Yellow;
                                                    }
                                                }
                                            }
                                            else if (str == "V2")
                                            {
                                                objPrepLog.FinalVol_V2 =null;
                                                ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)View).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                                if (FinalVol_V2 != null)
                                                {
                                                    FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                                    if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                                    {
                                                        FinalVol_V2.Editor.BackColor = Color.Yellow;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible))
                                {
                                    if (item.GetType() == typeof(ASPxStringPropertyEditor))
                                    {
                                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        
                                    }
                                    else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                                    {
                                        ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                        }
                           
                                    }
                                    else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                                    {
                                        ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            if (propertyEditor.Id == "Component")
                                            {
                                                propertyEditor.Editor.BackColor = Color.LightGray;
                                            }
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                            { 
                                        ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                { 
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                        }

                                    }
                                    else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                                 	{
                                        ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                            {                                       
                                            if (propertyEditor.Id == "LLTExpDate" || propertyEditor.Id == "LTExpDate")
                                        {
                                                propertyEditor.Editor.BackColor = Color.LightGray;
                                            }
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                        }

                                    }

                                }
                            }
                        }
                    }
                }
                else if(View.Id== "ReagentPreparation_DetailView_Calibration")
                                            { 
                    ASPxDateTimePropertyEditor propertyEditorDatePrepared = ((DetailView)View).FindItem("DatePrepared") as ASPxDateTimePropertyEditor;
                    if (propertyEditorDatePrepared != null && propertyEditorDatePrepared.Editor != null)
                                                {                                               
                        propertyEditorDatePrepared.Editor.CalendarCustomDisabledDate += Editor_CalendarCustomDisabledDate;
                    }
                                                }
                else if(View.Id== "ReagentPreparation_ListView_CopyPrevious")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if(gridListEditor!=null)
                    {
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 270;
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                        gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                                            }
                                        }        
                else if (View.Id == "NPReagentPrep_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                                    }
                else if(View.Id== "NPReagentPrepLog_DetailView_Chemistry" ||View.Id== "NPReagentPrepLog_DetailView_MicroMedia")
                {
                    ASPxGridLookupPropertyEditor lvVendor = ((DetailView)View).FindItem("VendorStock") as ASPxGridLookupPropertyEditor;
                    if (lvVendor != null)
                    {
                        if (lvVendor != null && lvVendor.AllowEdit && lvVendor.Editor != null)
                        {
                            ASPxGridLookup editor = (ASPxGridLookup)lvVendor.Editor;
                            if (editor != null)
                            {
                                editor.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                editor.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                editor.GridView.Settings.VerticalScrollableHeight = 300;
                                if (editor.GridView.Columns["Item"] != null)
                                {
                                    editor.GridView.Columns["Item"].Width = 90;
                                }
                                if (editor.GridView.Columns["Specification"] != null)
                                {
                                    editor.GridView.Columns["Specification"].Width = 150;
                                }
                                if (editor.GridView.Columns["Vendor"] != null)
                                {
                                    editor.GridView.Columns["Vendor"].Width = 90;
                                }
                                if (editor.GridView.Columns["ItemCode"] != null)
                                {
                                    editor.GridView.Columns["ItemCode"].Width = 80;
                                }
                            }
                        }
                    }
                    if (Application.MainWindow.View is DetailView && ((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.View)
                    {
                        disenbcontrolsinviewmode(false, View);
                        if(View.Id== "NPReagentPrepLog_DetailView_Chemistry")
                        {
                            NPReagentPrepLog objPrepLog = (NPReagentPrepLog)View.CurrentObject;
                            if (objPrepLog != null && !string.IsNullOrEmpty(objPrepLog.Formula))
                            {
                                if (objPrepLog.CalculationApproach != null)
                                {
                                    string str = objPrepLog.CalculationApproach.Approach.Split(' ').ToList().LastOrDefault().ToString();
                                    if (objPrepLog.CalculationApproach.Approach.Contains("V1") && objPrepLog.InitialVolTaken_V1_Units != null && objPrepLog.StockConc_C1_Units != null && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                                    {
                                        List<RegentPrepCalculationEditor> lstCalculation = View.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                                        if (lstCalculation.Count > 0)
                                    {                                                             
                                            if (str == "C1")
                                        {
                                                objPrepLog.StockConc_C1 = null;
                                                ASPxStringPropertyEditor StockConc_C1 = ((DetailView)View).FindItem("StockConc_C1") as ASPxStringPropertyEditor;
                                                if (StockConc_C1 != null)
                                            { 
                                                    StockConc_C1.AllowEdit.SetItemValue("AlowEdit", false);
                                                    if (StockConc_C1 != null && StockConc_C1.Editor != null)
                                                {
                                                        StockConc_C1.Editor.BackColor = Color.Yellow;
                                                    }
                                                }
                                                }
                                            else if (str == "C2")
                                            {
                                                objPrepLog.FinalConc_C2 = null;
                                                ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)View).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                                if (FinalConc_C2 != null)
                                                {
                                                    FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                                    if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                                    {
                                                        FinalConc_C2.Editor.BackColor = Color.Yellow;
                                            }
                                        }                            
                                    } 
                                            else if (str == "V1")
                                            {
                                                objPrepLog.VolumeTaken_V1 = null;
                                                ASPxStringPropertyEditor VolumeTaken_V1 = ((DetailView)View).FindItem("VolumeTaken_V1") as ASPxStringPropertyEditor;
                                                if (VolumeTaken_V1 != null)
                                                {
                                                    VolumeTaken_V1.AllowEdit.SetItemValue("AlowEdit", false);
                                                    if (VolumeTaken_V1 != null && VolumeTaken_V1.Editor != null)
                                                    {
                                                        VolumeTaken_V1.Editor.BackColor = Color.Yellow;
	}
                                }

                }
                                            else if (str == "V2")
                {
                                                objPrepLog.FinalVol_V2 = null;
                                                ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)View).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                                if (FinalVol_V2 != null)
                    {
                                                    FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                                    if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                        {
                                                        FinalVol_V2.Editor.BackColor = Color.Yellow;
                                                    }
                                                }
                        }
                    }
                }
                                    else if (objPrepLog.CalculationApproach.Approach.Contains("W1") && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                                    {
                                        List<RegentPrepCalculationEditor> lstCalculation = View.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                                        if (lstCalculation.Count > 0)
                                        {
                                            RegentPrepCalculationEditor objCalculationEditor = lstCalculation.FirstOrDefault(i => i.C2Units != null && i.V2Units != null && i.C2Units == objPrepLog.FinalConc_C2_Units && i.V2Units == objPrepLog.FinalVol_V2_Units);
                                            if (objCalculationEditor != null)
                                            {
                                                if (str == "C2")
                {
                                                    objPrepLog.FinalConc_C2 = null;
                                                    ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)View).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                                    if (FinalConc_C2 != null)
                    {
                                                        FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                                        if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                        {
                                                            FinalConc_C2.Editor.BackColor = Color.Yellow;
                                                        }
                        }
                    }
                                                else if (str == "W1")
                                                {
                                                    objPrepLog.VolumeTaken_V1 = null;
                                                    ASPxStringPropertyEditor Weight_g_w1 = ((DetailView)View).FindItem("Weight_g_w1") as ASPxStringPropertyEditor;
                                                    if (Weight_g_w1 != null)
                                                    {
                                                        Weight_g_w1.AllowEdit.SetItemValue("AlowEdit", false);
                                                        if (Weight_g_w1 != null && Weight_g_w1.Editor != null)
                    {
                                                            Weight_g_w1.Editor.BackColor = Color.Yellow;
                                                        }
                    }
                }
                                                else if (str == "V2")
                                                {
                                                    objPrepLog.FinalVol_V2 = null;
                                                    ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)View).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                                    if (FinalVol_V2 != null)
                                                    {
                                                        FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                                        if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                {
                                                            FinalVol_V2.Editor.BackColor = Color.Yellow;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                }
                    else
                    {
                        if (RPInfo.lstEditorID.Count > 0)
                        {
                            foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible && i.GetType() == typeof(ASPxStringPropertyEditor)))
                            {
                                ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null && RPInfo.lstEditorID.Contains(propertyEditor.Id))
                {
                                    ASPxTextBox editor = (ASPxTextBox)propertyEditor.Editor;
                                    if (editor != null)
                    {
                                        editor.ClientSideEvents.KeyPress = @"function(s, e){
                                var regex = /[0-9]|\./;   
                                if (!regex.test(e.htmlEvent.key)) {
                                    e.htmlEvent.returnValue = false;
                                }}";
                                    }
                                }

                            }
                        }
                        NPReagentPrepLog objPrepLog = (NPReagentPrepLog)View.CurrentObject;
                        AppearenceappliedInEditorFormulaBased(objPrepLog, View);
                    }
                   
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                foreach (DataColumn row in RPInfo.dtReagentPrepLog.Columns)
                {
                    GridViewDataColumn data_column = new GridViewDataTextColumn();
                    data_column.FieldName = row.Caption;
                    data_column.Caption = row.Caption;
                    data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    data_column.ShowInCustomizationForm = false;
                    data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                    grid.Columns.Add(data_column);
                }
                RPInfo.dtReagentPrepLog.DefaultView.Sort = "ID asc";
                grid.DataSource = RPInfo.dtReagentPrepLog;
                grid.DataBind();
                if (grid != null && grid.Columns.Count > 0)
                {
                    foreach (WebColumnBase column in grid.VisibleColumns)
                    {
                        if (column!=null && column.Caption.Contains("Oid"))
                        {
                            column.Visible = false; 
                        }
                    }
                }
                if (grid.Columns["VendorStock"] !=null)
                {
                    grid.Columns["VendorStock"].Width = 150;
                }
                if (grid.Columns["LabStock"] != null)
                {
                    grid.Columns["LabStock"].Width = 180;
                }
                if (grid.Columns["CalculationApproach"] != null)
                {
                    grid.Columns["CalculationApproach"].Width = 150;
                }
                if (grid.Columns["Weight(g)(W1)"] != null)
                {
                    grid.Columns["Weight(g)(W1)"].Width = 120;
                }
                if (grid.Columns["StockConc(C1)Units"] != null)
                {
                    grid.Columns["StockConc(C1)Units"].Width = 130;
                }
                if (grid.Columns["InitialVolTaken(V1)Units"] != null)
                {
                    grid.Columns["InitialVolTaken(V1)Units"].Width = 150;
                }
                if (grid.Columns["Formula"] != null)
                {
                    grid.Columns["Formula"].Width = 200;
                }
                if (grid.Columns["FinalConc(C2)Units"] != null)
                {
                    grid.Columns["FinalConc(C2)Units"].Width = 130;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Editor_CalendarCustomDisabledDate(object sender, CalendarCustomDisabledDateEventArgs e)
        {
            try
            {
                if (DateTime.Today<e.Date)
                {
                    e.IsDisabled = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void disenbcontrolsinviewmode(bool enbstat, DevExpress.ExpressApp.View view)
        {
            try
                        {
                foreach (ViewItem item in ((DetailView)view).Items)
                            {
                                if (item.GetType() == typeof(ASPxStringPropertyEditor))
                                {
                                    ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                    {
                        ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                                        }
                    else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                    {
                        ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                    }

                                }
                    else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                    {
                        ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                        if (propertyEditor != null && propertyEditor.DropDownEdit != null)
                        {
                            propertyEditor.DropDownEdit.DropDown.ForeColor = Color.Black;
                        }
                    }
                    else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                    {
                        ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                                        }
                    else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                    {
                        ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                    }

                                }
                    else if (item.GetType() == typeof(ASPxBooleanPropertyEditor))
                    {
                        ASPxBooleanPropertyEditor propertyEditor = item as ASPxBooleanPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void XafPopupWindowControl_CustomizePopupControl(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupControlEventArgs e)
        {
            try
            {
                string existingScript = e.PopupControl.ClientSideEvents.CloseUp;
                ICallbackManagerHolder holder = WebWindow.CurrentRequestPage as ICallbackManagerHolder;
                if (holder != null)
                {
                    string redirectScript = holder.CallbackManager.GetScript();
                    string closingScript = e.PopupControl.ClientSideEvents.CloseUp;
                    e.PopupControl.ClientSideEvents.CloseUp = string.Format("function(s, e) {{ ({0}); {1}; }}", closingScript, redirectScript);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id== "ReagentPreparation_ReagentPrepLogs_ListView_Calibration")
                {
                    if (e.DataColumn.FieldName != "StockStdName") return;
                    e.Cell.Attributes.Add("ondblclick", string.Format(@"Grid.UpdateEdit();RaiseXafCallback(globalCallbackControl, 'StockStdPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex)); 
                }
                else if(View.Id== "NPReagentPrep_ListView")
                                        {
                    e.Cell.Attributes.Add("onclick", "event.stopPropagation();");
                }
            }
            catch (Exception ex)
                                            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
                                            }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View.Id == "ReagentPreparation_ListView")
                                            {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
                    Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem -= ReagentPreparationLogViewController_CustomProcessSelectedItem;
                                            }
                else if(View.Id== "ReagentPrepChemical" ||View.Id== "ReagentPrepMicroMedia")
                {
                    dcOK = Frame.GetController<DialogController>();
                    if (dcOK != null)
                                            {
                        dcOK.Accepting -= DcOK_Accepting;
                    }
                    Application.DetailViewCreating -= Application_DetailViewCreating;
                                            }
                else if (View.Id == "ReagentPrepLog_DetailView_Chemistry" ||View.Id== "ReagentPreparation_DetailView_Chemistry" ||View.Id== "ReagentPrepLog_DetailView_MicroMedia"
                    ||View.Id== "ReagentPreparation_DetailView_MicroMedia" ||View.Id== "ReagentPrepLog_DetailView_Chemistry_PrepNotepad")
                {
                    View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    if(View.Id== "ReagentPrepLog_DetailView_Chemistry" ||View.Id== "ReagentPrepLog_DetailView_Chemistry_PrepNotepad")
                    {
                        ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated -= ChangeLayoutGroupCaptionViewController_ItemCreated;
                                        }
                    if(View.Id== "ReagentPreparation_DetailView_MicroMedia"||View.Id== "ReagentPreparation_DetailView_Chemistry")
                                        {
                        Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    }
                }
                else if (View.Id == "ReagentPreparation_DetailView_Calibration")
                                            {
                    View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                                            }
                else if(View.Id== "ReagentPrepChemical_PrepNotepad" ||View.Id== "ReagentPrepMicroMedia_PrepNotepad")
                {
                    Application.DetailViewCreating -= Application_DetailViewCreating;
                    DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                    if (lvPrep != null)
                                            {
                        lvPrep.ControlCreated -= lvPrep_ControlCreated;
                    }
                                            }
                else if (View.Id == "ReagentPrepClassify")
                                            {
                    IsNew = false;
                    ((WebLayoutManager)((DashboardView)View).LayoutManager).PageControlCreated -= TabViewController_PageControlCreated;
                                            }
                else if (View.Id == "NPReagentPrepLog_DetailView_Chemistry")
                {
                    View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated -= ChangeLayoutGroupCaptionViewController_ItemCreated;
                                        }
                                    }
            catch(Exception ex)
                                    {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }
        private void Reset_Execute(object sender, SimpleActionExecuteEventArgs e)
                                {
           try
                                    {
                if(View.Id == "ReagentPrepChemical" ||View.Id== "ReagentPrepChemical_PrepNotepad")
                                        {
                    ClearReagentPrepChecmical();
                                        }
                else if(View.Id == "ReagentPrepMicroMedia" ||View.Id== "ReagentPrepMicroMedia_PrepNotepad")
                                        {
                    ClearReagentPrepMicroMedia();
                                        }
                 
                                    }
            catch (Exception ex)
                                    {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }
        private void Previous_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

                DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                if(dvPrep!=null && dvPrep.InnerView!=null && lvPrep!=null && lvPrep.InnerView!=null)
                {
                    NPReagentPrepLog dvCurrentObj = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                    uint intNumber = dvCurrentObj.ComponentID;
                    uint intNextNumber = dvCurrentObj.ComponentID - 1;
                    DataRow[] rows = RPInfo.dtReagentPrepLog.Select("ID ='" + intNumber + "'");
                    DataRow[] rowsNext = RPInfo.dtReagentPrepLog.Select("ID ='" + intNextNumber + "'");
                    if (rows != null && rows.Count() > 0)
                    {
                        ChangePrepLog(rows[0], dvCurrentObj,"Chemistry");
                        if (rowsNext != null && rowsNext.Count() > 0)
                                {
                            ChangePrepLog(dvCurrentObj, rowsNext[0], dvPrep.InnerView.ObjectSpace, "Chemistry");
                        }
                    }
                    else
                                    {
                        DataRow dr = null;
                        if (RPInfo.drReagentPrepLog==null)
                                        {
                            dr =  RPInfo.dtReagentPrepLog.NewRow();
                            ChangePrepLog(dr, dvCurrentObj,"Chemistry");
                            RPInfo.drReagentPrepLog = dr;
                                        }
                                        else
                                        {
                            ChangePrepLog(RPInfo.drReagentPrepLog, dvCurrentObj, "Chemistry");
                                        }
                        if (rowsNext != null && rowsNext.Count() > 0)
                                    {
                            ChangePrepLog(dvCurrentObj, rowsNext[0], dvPrep.InnerView.ObjectSpace, "Chemistry");
                                    }

                                }
                    if (intNextNumber == 1)
                                {
                        PreviousReagentPrepLog.Enabled["boolPrevios"] = false;
                                        }
                    uint intNext = dvCurrentObj.ComponentID + 1;
                    DataRow[] IsNext = RPInfo.dtReagentPrepLog.Select("ID ='" + intNext + "'");
                    if(IsNext.Count()>0)
                                        {
                        NextReagentPrepLog.Enabled["boolNext"] = true;
                                        }
                    RPInfo.dtReagentPrepLog.AcceptChanges();
                    AppearenceappliedInEditorFormulaBased(dvCurrentObj, dvPrep.InnerView);
                    ASPxGridListEditor gridListEditor = ((ListView)lvPrep.InnerView).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                                    {
                        gridListEditor.Grid.DataSource = RPInfo.dtReagentPrepLog;
                        gridListEditor.Grid.DataBind();
                                    }



                    //dvPrep.InnerView.ObjectSpace.CommitChanges();
                    //ReagentPrepLog dvCurrentObj = (ReagentPrepLog)dvPrep.InnerView.CurrentObject;
                    //if(dvCurrentObj!=null)
                    //{
                    //    ReagentPrepLog lvOldobj=((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i=>i.ComponentID!=0 && i.ComponentID== dvCurrentObj.ComponentID-1);
                    //    if(lvOldobj!=null)
                    //    {
                    //        dvPrep.InnerView.CurrentObject = dvPrep.InnerView.ObjectSpace.GetObject(lvOldobj);
                    //        if(lvOldobj.ComponentID==1)
                    //        {
                    //            PreviousReagentPrepLog.Enabled["boolPrevios"] =false;
                    //        }
                    //        ReagentPrepLog NextObj = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i => i.ComponentID != 0 && i.ComponentID == lvOldobj.ComponentID +1);
                    //        if (NextObj!=null)
                    //        {
                    //            NextReagentPrepLog.Enabled["boolNext"] = true; 
                    //        }
                    //        ((DetailView)dvPrep.InnerView).Refresh();
                    //        lvPrep.InnerView.Refresh();
                    //        AppearenceappliedInEditorFormulaBased(lvOldobj, dvPrep.InnerView);
                    //        ((DetailView)dvPrep.InnerView).RefreshDataSource();
                    //        View.Refresh();
                    //    }
                    //}    
                                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                        }
        private void Next_Execute(object sender, SimpleActionExecuteEventArgs e)
                        {
            try
            {
                DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                if (View.Id == "ReagentPrepChemical" || View.Id == "ReagentPrepChemical_PrepNotepad")
                {
                    if (dvPrep != null && dvPrep.InnerView != null && dvPrep.InnerView.CurrentObject != null && lvPrep != null && lvPrep.InnerView != null)
                    {
                        NPReagentPrepLog objReaPrep = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        uint intNumber = objReaPrep.ComponentID;
                        uint intNextNumber = objReaPrep.ComponentID + 1;
                        uint intNext = objReaPrep.ComponentID + 2;
                        DataRow[] rows = RPInfo.dtReagentPrepLog.Select("ID ='" + intNumber + "'");
                        DataRow[] rowsNext = RPInfo.dtReagentPrepLog.Select("ID ='" + intNextNumber + "'");
                        DataRow[] IsNext = RPInfo.dtReagentPrepLog.Select("ID ='" + intNext + "'");
                        if (rows != null && rows.Count() > 0)
                        {
                            if (Application.MainWindow.View is DetailView)
                            {
                                if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                                {
                                    ChangePrepLog(rows[0], objReaPrep, "Chemistry");
                                }
                                if (rowsNext != null && rowsNext.Count() > 0)
                                {
                                    ChangePrepLog(objReaPrep, rowsNext[0], dvPrep.InnerView.ObjectSpace, "Chemistry");

                                    if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.View && IsNext.Count() == 0)
                                    {
                                        NextReagentPrepLog.Enabled["boolNext"] = false;
                                    }
                                }
                                else
                                {
                                    if (RPInfo.drReagentPrepLog != null && Convert.ToUInt16(RPInfo.drReagentPrepLog["ID"]) == intNextNumber)
                                    {
                                        ChangePrepLog(objReaPrep, RPInfo.drReagentPrepLog, dvPrep.InnerView.ObjectSpace, "Chemistry");
                                    }
                                    else
                                    {
                                        objReaPrep.ComponentID = intNextNumber;
                                        ClearReagentPrepChecmical();
                                    }
                                }
                            }
                            else
                            {
                                ChangePrepLog(rows[0], objReaPrep, "Chemistry");
                                if (rowsNext != null && rowsNext.Count() > 0)
                                {
                                    ChangePrepLog(objReaPrep, rowsNext[0], dvPrep.InnerView.ObjectSpace, "Chemistry");
                                }
                                else
                                {
                                    if (RPInfo.drReagentPrepLog != null && Convert.ToUInt16(RPInfo.drReagentPrepLog["ID"]) == intNextNumber)
                                    {
                                        ChangePrepLog(objReaPrep, RPInfo.drReagentPrepLog, dvPrep.InnerView.ObjectSpace, "Chemistry");
                                    }
                                    else
                                    {
                                        objReaPrep.ComponentID = intNextNumber;
                                        ClearReagentPrepChecmical();
                                    }
                                }
                            }

                        }
                        else
                        {
                            DataRow dr = RPInfo.dtReagentPrepLog.NewRow();
                            ChangePrepLog(dr, objReaPrep, "Chemistry");
                            RPInfo.dtReagentPrepLog.Rows.Add(dr);
                            objReaPrep.ComponentID = intNextNumber;
                            ClearReagentPrepChecmical();
                        }
                        if (intNextNumber != 1)
                        {
                            PreviousReagentPrepLog.Enabled["boolPrevios"] = true;
                        }
                        RPInfo.dtReagentPrepLog.AcceptChanges();
                        AppearenceappliedInEditorFormulaBased(objReaPrep, dvPrep.InnerView);
                        ASPxGridListEditor gridListEditor = ((ListView)lvPrep.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.DataSource = RPInfo.dtReagentPrepLog;
                            gridListEditor.Grid.DataBind();
                        }
                    }
                }
                else if (View.Id == "ReagentPrepMicroMedia" || View.Id == "ReagentPrepMicroMedia_PrepNotepad")
                {
                    if (dvPrep != null && dvPrep.InnerView != null && dvPrep.InnerView.CurrentObject != null && lvPrep != null && lvPrep.InnerView != null)
                    {
                        NPReagentPrepLog objReaPrep = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        uint intNumber = objReaPrep.ComponentID;
                        uint intNextNumber = objReaPrep.ComponentID + 1;
                        uint intNext = objReaPrep.ComponentID + 2;
                        DataRow[] rows = RPInfo.dtReagentPrepLog.Select("ID ='" + intNumber + "'");
                        DataRow[] rowsNext = RPInfo.dtReagentPrepLog.Select("ID ='" + intNextNumber + "'");
                        DataRow[] IsNext = RPInfo.dtReagentPrepLog.Select("ID ='" + intNext + "'");
                        if (rows != null && rows.Count() > 0)
                        {
                            if (Application.MainWindow.View is DetailView)
                            {
                                if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                                {
                                    ChangePrepLog(rows[0], objReaPrep, "Micro");
                                }
                                if (rowsNext != null && rowsNext.Count() > 0)
                                {
                                    ChangePrepLog(objReaPrep, rowsNext[0], dvPrep.InnerView.ObjectSpace, "Micro");
                                    if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.View && IsNext.Count() == 0)
                                    {
                                        NextReagentPrepLog.Enabled["boolNext"] = false;
                                    }
                                }
                                else
                                {
                                    if (RPInfo.drReagentPrepLog != null && Convert.ToUInt16(RPInfo.drReagentPrepLog["ID"]) == intNextNumber)
                                    {
                                        ChangePrepLog(objReaPrep, RPInfo.drReagentPrepLog, dvPrep.InnerView.ObjectSpace, "Micro");
                                    }
                                    else
                                    {
                                        objReaPrep.ComponentID = intNextNumber;
                                        ClearReagentPrepMicroMedia();
                                    }
                                }
                            }
                            else
                            {
                                ChangePrepLog(rows[0], objReaPrep, "Micro");
                                if (rowsNext != null && rowsNext.Count() > 0)
                                {
                                    ChangePrepLog(objReaPrep, rowsNext[0], dvPrep.InnerView.ObjectSpace, "Micro");
                                }
                                else
                                {
                                    if (RPInfo.drReagentPrepLog != null && Convert.ToUInt16(RPInfo.drReagentPrepLog["ID"]) == intNextNumber)
                                    {
                                        ChangePrepLog(objReaPrep, RPInfo.drReagentPrepLog, dvPrep.InnerView.ObjectSpace, "Micro");
                                    }
                                    else
                                    {
                                        objReaPrep.ComponentID = intNextNumber;
                                        ClearReagentPrepMicroMedia();
                                    }
                                }
                            }

                        }
                        else
                        {
                            DataRow dr = RPInfo.dtReagentPrepLog.NewRow();
                            ChangePrepLog(dr, objReaPrep, "Micro");
                            RPInfo.dtReagentPrepLog.Rows.Add(dr);
                            objReaPrep.ComponentID = intNextNumber;
                            ClearReagentPrepMicroMedia();
                        }
                        if (intNextNumber != 1)
                        {
                            PreviousReagentPrepLog.Enabled["boolPrevios"] = true;
                        }
                        RPInfo.dtReagentPrepLog.AcceptChanges();
                        ASPxGridListEditor gridListEditor = ((ListView)lvPrep.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.DataSource = RPInfo.dtReagentPrepLog;
                            gridListEditor.Grid.DataBind();
                        }
                    }
                }
                //if (View.Id== "ReagentPrepChemical_PrepNotepad" || View.Id== "ReagentPrepMicroMedia_PrepNotepad")
                //{
                //    if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.View)
                //    {
                //        if (dvPrep != null && dvPrep.InnerView != null && dvPrep.InnerView.CurrentObject != null && lvPrep != null && lvPrep.InnerView != null)
                //        {
                //            ReagentPrepLog objOldRep = null;
                //            ReagentPrepLog objReaPrep = (ReagentPrepLog)dvPrep.InnerView.CurrentObject;
                //            objOldRep = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().ToList().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID + 1);
                //            ReagentPrepLog objPrepNext = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().ToList().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID + 2);
                //            dvPrep.InnerView.CurrentObject = dvPrep.InnerView.ObjectSpace.GetObject(objOldRep);
                //            if (((ReagentPrepLog)dvPrep.InnerView.CurrentObject).ComponentID != 1)
                //            {
                //                PreviousReagentPrepLog.Enabled["boolPrevios"] = true;
                //            }
                //            if (objPrepNext == null)
                //            {
                //                NextReagentPrepLog.Enabled["boolNext"] = false;
                //            }
                //            AppearenceappliedInEditorFormulaBased(objOldRep, dvPrep.InnerView);
                //        }  
                //    }
                //    else
                //    {
                //        if (dvPrep != null && dvPrep.InnerView != null && dvPrep.InnerView.CurrentObject != null && lvPrep != null && lvPrep.InnerView != null)
                //        {
                //            ReagentPrepLog objOldRep = null;
                //            dvPrep.InnerView.ObjectSpace.CommitChanges();
                //            lvPrep.InnerView.ObjectSpace.CommitChanges();
                //            ReagentPrepLog objReaPrep = (ReagentPrepLog)dvPrep.InnerView.CurrentObject;
                //            if (objReaPrep.VendorStock == null && objReaPrep.LabStock == null)
                //            {
                //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectvendorstockorlabstock"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                //                return;
                //            }
                //            else
                //            {
                //                if (((ListView)lvPrep.InnerView).CollectionSource.GetCount() == 0 || ((ListView)lvPrep.InnerView).CollectionSource.GetCount() > 0 && RPInfo.lstReagentPrepLog.FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) == null)
                //                {
                //                    PreviousReagentPrepLog.Enabled["boolPrevios"] = true;
                //                    ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep));
                //                    ReagentPrepLog newObj = dvPrep.InnerView.ObjectSpace.CreateObject<ReagentPrepLog>();
                //                    newObj.ComponentID = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().Max(i => i.ComponentID + 1);
                //                    //newObj.FinalVol_V2 = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().Where(i => i.FinalVol_V2 != 0).Select(i => i.FinalVol_V2).FirstOrDefault();
                //                    dvPrep.InnerView.CurrentObject = newObj;
                //                    RPInfo.lstReagentPrepLog.Add(newObj);
                //                    AppearenceappliedInEditorFormulaBased(newObj, dvPrep.InnerView);
                //                }
                //                else
                //                {
                //                    objOldRep = RPInfo.lstReagentPrepLog.FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID + 1);
                //                    if (objOldRep != null)
                //                    {
                //                        ReagentPrepLog objOldRepPrep = dvPrep.InnerView.ObjectSpace.GetObject(objReaPrep);
                //                        if (objOldRepPrep != null)
                //                        {
                //                            dvPrep.InnerView.CurrentObject = dvPrep.InnerView.ObjectSpace.GetObject(objOldRep);
                //                            if (((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) == null)
                //                            {
                //                                ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objOldRepPrep));

                //                            }
                //                            else if (((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) != null)
                //                            {
                //                                lvPrep.InnerView.ObjectSpace.ReloadObject(lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep));
                //                                ReagentPrepLog objlvPrep = lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep);
                //                                ((ListView)lvPrep.InnerView).CollectionSource.Add(objlvPrep);

                //                            }
                //                        }
                //                        else
                //                        {
                //                            dvPrep.InnerView.ObjectSpace.CommitChanges();
                //                            dvPrep.InnerView.CurrentObject = dvPrep.InnerView.ObjectSpace.GetObject(objOldRep);
                //                            if (((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) == null)
                //                            {
                //                                ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objOldRep));

                //                            }
                //                            else if (((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) != null)
                //                            {
                //                                lvPrep.InnerView.ObjectSpace.RemoveFromModifiedObjects(objReaPrep);
                //                                ((ListView)lvPrep.InnerView).CollectionSource.Remove(objReaPrep);

                //                                ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep));
                //                            }
                //                        }

                //                    }
                //                    else
                //                    {
                //                        PreviousReagentPrepLog.Enabled["boolPrevios"] = true;
                //    //    dvPrep.InnerView.ObjectSpace.CommitChanges();
                //                        ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep));
                //                        ReagentPrepLog newObj = dvPrep.InnerView.ObjectSpace.CreateObject<ReagentPrepLog>();
                //                        newObj.ComponentID = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().Max(i => i.ComponentID + 1);
                //                        //newObj.FinalVol_V2 = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().Where(i => i.FinalVol_V2 != 0).Select(i => i.FinalVol_V2).FirstOrDefault();
                //                        ((DetailView)dvPrep.InnerView).CurrentObject = newObj;
                //                        RPInfo.lstReagentPrepLog.Add(newObj);
                //                        AppearenceappliedInEditorFormulaBased(newObj, dvPrep.InnerView);
                //                    }
                //                }
                //                 ((ListView)lvPrep.InnerView).Refresh();
                //                ((DetailView)dvPrep.InnerView).Refresh();
                //                ((DetailView)dvPrep.InnerView).RefreshDataSource();
                //                View.Refresh();
                //                if (((ReagentPrepLog)dvPrep.InnerView.CurrentObject).ComponentID != 1)
                //                {
                //                    PreviousReagentPrepLog.Enabled["boolPrevios"] = true;
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    if (dvPrep != null && dvPrep.InnerView != null && dvPrep.InnerView.CurrentObject != null && lvPrep != null && lvPrep.InnerView != null)
                //    {
                //        ReagentPrepLog objOldRep = null;
                //        dvPrep.InnerView.ObjectSpace.CommitChanges();
                //        lvPrep.InnerView.ObjectSpace.CommitChanges();
                //        ReagentPrepLog objReaPrep = (ReagentPrepLog)dvPrep.InnerView.CurrentObject;
                //        if (objReaPrep.VendorStock == null && objReaPrep.LabStock == null)
                //        {
                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectvendorstockorlabstock"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                //            return;
                //        }
                //        else
                //        {
                //            if (((ListView)lvPrep.InnerView).CollectionSource.GetCount() == 0 || ((ListView)lvPrep.InnerView).CollectionSource.GetCount() > 0 && RPInfo.lstReagentPrepLog.FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) == null)
                //            {
                //                PreviousReagentPrepLog.Enabled["boolPrevios"] = true;
                //                ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep));
                //                ReagentPrepLog newObj = dvPrep.InnerView.ObjectSpace.CreateObject<ReagentPrepLog>();
                //                newObj.ComponentID = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().Max(i => i.ComponentID + 1);
                //                //newObj.FinalVol_V2 = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().Where(i => i.FinalVol_V2 != 0).Select(i => i.FinalVol_V2).FirstOrDefault();
                //                dvPrep.InnerView.CurrentObject = newObj;
                //                RPInfo.lstReagentPrepLog.Add(newObj);
                //                AppearenceappliedInEditorFormulaBased(newObj, dvPrep.InnerView);
                //            }
                //            else
                //            {
                //                objOldRep = RPInfo.lstReagentPrepLog.FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID + 1);
                //                if (objOldRep != null)
                //                {
                //                    ReagentPrepLog objOldRepPrep = dvPrep.InnerView.ObjectSpace.GetObject(objReaPrep);
                //                    if (objOldRepPrep != null)
                //                    {
                //                        dvPrep.InnerView.CurrentObject = dvPrep.InnerView.ObjectSpace.GetObject(objOldRep);
                //                        if (((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) == null)
                //                        {
                //                            ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objOldRepPrep));

                //                        }
                //                        else if (((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) != null)
                //                        {
                //                            lvPrep.InnerView.ObjectSpace.ReloadObject(lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep));
                //                            ReagentPrepLog objlvPrep = lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep);
                //                            ((ListView)lvPrep.InnerView).CollectionSource.Add(objlvPrep);

                //                        }
                //                    }
                //                    else
                //                    {
                //                        dvPrep.InnerView.ObjectSpace.CommitChanges();
                //                        dvPrep.InnerView.CurrentObject = dvPrep.InnerView.ObjectSpace.GetObject(objOldRep);
                //                        if (((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) == null)
                //                        {
                //                            ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objOldRep));

                //                        }
                //                        else if (((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().FirstOrDefault(i => i.ComponentID == objReaPrep.ComponentID) != null)
                //                        {
                //                            lvPrep.InnerView.ObjectSpace.RemoveFromModifiedObjects(objReaPrep);
                //                            ((ListView)lvPrep.InnerView).CollectionSource.Remove(objReaPrep);

                //                            ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep));
                //                        }
                //                    }

                //                }
                //                else
                //                {
                //                    PreviousReagentPrepLog.Enabled["boolPrevios"] = true;
                //                    //dvPrep.InnerView.ObjectSpace.CommitChanges();
                //                    ((ListView)lvPrep.InnerView).CollectionSource.Add(lvPrep.InnerView.ObjectSpace.GetObject(objReaPrep));
                //                    ReagentPrepLog newObj = dvPrep.InnerView.ObjectSpace.CreateObject<ReagentPrepLog>();
                //                    newObj.ComponentID = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().Max(i => i.ComponentID + 1);
                //                    //newObj.FinalVol_V2 = ((ListView)lvPrep.InnerView).CollectionSource.List.Cast<ReagentPrepLog>().Where(i => i.FinalVol_V2 != 0).Select(i => i.FinalVol_V2).FirstOrDefault();
                //                    dvPrep.InnerView.CurrentObject = newObj;
                //                    RPInfo.lstReagentPrepLog.Add(newObj);
                //                    AppearenceappliedInEditorFormulaBased(newObj, dvPrep.InnerView);
                //                }
                //            }
                //             ((ListView)lvPrep.InnerView).Refresh();
                //            ((DetailView)dvPrep.InnerView).Refresh();
                //            ((DetailView)dvPrep.InnerView).RefreshDataSource();
                //            View.Refresh();
                //            if (((ReagentPrepLog)dvPrep.InnerView.CurrentObject).ComponentID != 1)
                //            {
                //                PreviousReagentPrepLog.Enabled["boolPrevios"] = true;
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
                }
        private void Save_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ObjectSpace.CommitChanges();
                View.ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "SuccessfulMessage"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private List<char> ArithematicOperators()
        {
            try
            {
                lstOperators = new List<char>();
                lstOperators.Add('+');
                lstOperators.Add('-');
                lstOperators.Add('/');
                lstOperators.Add('*');
                lstOperators.Add('^');
                lstOperators.Add('(');
                lstOperators.Add(')');
                lstOperators.Add('<');
                lstOperators.Add('>');
                lstOperators.Add('=');
                lstOperators.Add(',');
                lstOperators.Add('#');
                return lstOperators;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private void Ok_Level_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objRP = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)View.CurrentObject;
                if (objRP != null && objRP.NoOfLevels > 0)
                {
                    ListPropertyEditor lvPrep = ((DetailView)View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                    if (lvPrep != null && lvPrep.ListView != null && ((ListView)lvPrep.ListView).CollectionSource.GetCount() < objRP.NoOfLevels)
                    {
                        int insertRowCount = Convert.ToUInt16(objRP.NoOfLevels - ((ListView)lvPrep.ListView).CollectionSource.GetCount());
                        for (int i = 0; i < insertRowCount; i++)
                        {
                            ReagentPrepLog objNewPrep = lvPrep.ListView.ObjectSpace.CreateObject<ReagentPrepLog>();
                            if (objRP.StockConc != null)
                            {
                                objNewPrep.Cal_Weight_g_w1 = objRP.StockConc;
                            }
                            if (objRP.StockConcUnit != null)
                            {
                                objNewPrep.WSCons_Units = objRP.StockConcUnit;
                            }
                            objNewPrep.ComponentID = Convert.ToUInt16(((ListView)lvPrep.ListView).CollectionSource.GetCount() + 1);
                            ((ListView)lvPrep.ListView).CollectionSource.Add(objNewPrep);
                        }
                        ((ListView)lvPrep.ListView).Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void ProcessAction(string parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    string[] param = parameter.Split('|');
                    if (param[0] == "StockStdName")
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null)
                        {
                            HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["ComponentID"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "ComponentID");
                            IObjectSpace os = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(ObjectSpace, typeof(NPSampleFields));
                            ListView lv = Application.CreateListView("NPSampleFields_ListView_Reagent", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = "StockStdName";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.AcceptAction.Execute += AcceptAction_Execute;
                            dc.Accepting += Dc_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedObjects.Count>0)
                {
                    NPSampleFields obj = (NPSampleFields)e.CurrentObject;
                    if (obj != null)
                    {
                        if (HttpContext.Current.Session["rowid"] != null)
                        {
                            ReagentPrepLog objsampling = ((ListView)View).CollectionSource.List.Cast<ReagentPrepLog>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                            if (objsampling != null)
                {
                                objsampling.StockStdName = obj.FieldCaption;
                            }
                            ((ListView)View).Refresh();
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private Dictionary<string, string> dicColumnValues()
        {
            try
            {
                dicCaption = new Dictionary<string, string>();
                dicCaption.Add("C1", "StockConc_C1");
                dicCaption.Add("V1", "InitialVolTaken_V1_Units");
                dicCaption.Add("V2", "FinalVol_V2");
                dicCaption.Add("C2", "FinalConc_C2");
                dicCaption.Add("Purity", "Purity");
                dicCaption.Add("Density", "Density");
                dicCaption.Add("Constant", "Constant");
                return dicCaption;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private void Ok_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id == "ReagentPrepChemical" || View.Id == "ReagentPrepChemical_PrepNotepad")
                {
                    DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                    DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                    if (dvPrep != null && dvPrep.InnerView != null)
                    {
                        NPReagentPrepLog objCurReagent = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        if (objCurReagent.LabStock == null && objCurReagent.VendorStock == null && ((ListView)lvPrep.InnerView).CollectionSource.GetCount() == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Haventaddanycomponetyet"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                        if (objCurReagent.LabStock != null || objCurReagent.VendorStock != null)
                        {
                            uint intNumber = objCurReagent.ComponentID;
                            DataRow[] rows = RPInfo.dtReagentPrepLog.Select("ID ='" + intNumber + "'");
                            if (rows != null && rows.Count() > 0)
                            {
                                ChangePrepLog(rows[0], objCurReagent, "Chemistry");
                            }
                            else
                            {
                                DataRow dr = RPInfo.dtReagentPrepLog.NewRow();
                                ChangePrepLog(dr, objCurReagent, "Chemistry");
                                RPInfo.dtReagentPrepLog.Rows.Add(dr);

                            }
                        }
                        if (RPInfo.drReagentPrepLog != null && (RPInfo.drReagentPrepLog["LabStock"] != null || RPInfo.drReagentPrepLog["VendorStock"] != null))
                        {
                            uint intNumber = Convert.ToUInt16(RPInfo.drReagentPrepLog["ID"]);
                            DataRow[] rows = RPInfo.dtReagentPrepLog.Select("ID ='" + intNumber + "'");
                            if (rows.Count() == 0)
                            {
                                DataRow dr = RPInfo.dtReagentPrepLog.NewRow();
                                ChangePrepLog(dr, objCurReagent, "Chemistry");
                                RPInfo.dtReagentPrepLog.Rows.Add(dr);
                            }
                        }
                        RPInfo.dtReagentPrepLog.AcceptChanges();

                    }
                }
                else if (View.Id == "ReagentPrepMicroMedia" || View.Id == "ReagentPrepMicroMedia_PrepNotepad")
                {
                    DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                    DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                    if (dvPrep != null && dvPrep.InnerView != null)
                    {

                        NPReagentPrepLog objCurReagent = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        if (objCurReagent.LabStock == null && objCurReagent.VendorStock == null && ((ListView)lvPrep.InnerView).CollectionSource.GetCount() == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Haventaddanycomponetyet"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                        if (objCurReagent.LabStock != null || objCurReagent.VendorStock != null)
                        {
                            uint intNumber = objCurReagent.ComponentID;
                            DataRow[] rows = RPInfo.dtReagentPrepLog.Select("ID ='" + intNumber + "'");
                            if (rows != null && rows.Count() > 0)
                            {
                                ChangePrepLog(rows[0], objCurReagent, "Micro");
                            }
                            else
                            {
                                DataRow dr = RPInfo.dtReagentPrepLog.NewRow();
                                ChangePrepLog(dr, objCurReagent, "Micro");
                                RPInfo.dtReagentPrepLog.Rows.Add(dr);

                            }
                        }
                        if (RPInfo.drReagentPrepLog != null && (RPInfo.drReagentPrepLog["LabStock"] != null || RPInfo.drReagentPrepLog["VendorStock"] != null))
                        {
                            uint intNumber = Convert.ToUInt16(RPInfo.drReagentPrepLog["ID"]);
                            DataRow[] rows = RPInfo.dtReagentPrepLog.Select("ID ='" + intNumber + "'");
                            if (rows.Count() == 0)
                            {
                                DataRow dr = RPInfo.dtReagentPrepLog.NewRow();
                                ChangePrepLog(dr, objCurReagent, "Micro");
                                RPInfo.dtReagentPrepLog.Rows.Add(dr);
                            }
                        }
                        RPInfo.dtReagentPrepLog.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Ok_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "ReagentPrepChemical")
                {
                    DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                    DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                    if (dvPrep != null && dvPrep.InnerView != null)
                    {
                        NPReagentPrepLog objCurReagent = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        if (Application.MainWindow.View is DetailView)
                        {
                            Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                            ListPropertyEditor lvPreps = ((DetailView)Application.MainWindow.View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                            foreach (DataRow row in RPInfo.dtReagentPrepLog.Rows)
                            {
                                ReagentPrepLog lvObj = ((ListView)lvPreps.ListView).CollectionSource.List.Cast<ReagentPrepLog>().ToList().Where(i => i.ComponentID == Convert.ToUInt16(row["ID"])).FirstOrDefault();
                                if (lvObj != null)
                                {
                                    ChangePrepLog(lvObj, row, Application.MainWindow.View.ObjectSpace, "Chemistry");
                                }
                                else
                                {
                                    ReagentPrepLog newObj = Application.MainWindow.View.ObjectSpace.CreateObject<ReagentPrepLog>();
                                    ChangePrepLog(newObj, row, Application.MainWindow.View.ObjectSpace, "Chemistry");
                                    objPrep.ReagentPrepLogs.Add(newObj);
                                }
                            }
                        }
                        else
                        {
                            IObjectSpace os = ObjectSpace;
                            Modules.BusinessObjects.ReagentPreparation.ReagentPreparation newReagentPreparation = os.CreateObject<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>();
                            foreach (DataRow row in RPInfo.dtReagentPrepLog.Rows)
                            {
                                ReagentPrepLog newObj = os.CreateObject<ReagentPrepLog>();
                                ChangePrepLog(newObj, row, os, "Chemistry");
                                newReagentPreparation.ReagentPrepLogs.Add(newObj);
                            }
                            newReagentPreparation.PrepSelectType = PrepSelectTypes.ChemicalReagentPrep;
                            DetailView detailView = Application.CreateDetailView(os, "ReagentPreparation_DetailView_Chemistry", true, newReagentPreparation);
                            detailView.ViewEditMode = ViewEditMode.Edit;
                            Application.MainWindow.SetView(detailView);
                        }
                        Window window = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                        if (window != null)
                        {
                            ((DevExpress.ExpressApp.Web.PopupWindow)Frame).Close(true);
                        }
                        Application.MainWindow.View.Refresh();
                        Application.MainWindow.View.RefreshDataSource();

                    }
                }
                else if (View.Id == "ReagentPrepMicroMedia")
                {

                    DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                    DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                    if (dvPrep != null && dvPrep.InnerView != null)
                    {
                        NPReagentPrepLog objCurReagent = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        if (Application.MainWindow.View is DetailView)
                        {
                            Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                            ListPropertyEditor lvPreps = ((DetailView)Application.MainWindow.View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                            foreach (DataRow row in RPInfo.dtReagentPrepLog.Rows)
                            {
                                ReagentPrepLog lvObj = ((ListView)lvPreps.ListView).CollectionSource.List.Cast<ReagentPrepLog>().ToList().Where(i => i.ComponentID == Convert.ToUInt16(row["ID"])).FirstOrDefault();
                                if (lvObj != null)
                                {
                                    ChangePrepLog(lvObj, row, Application.MainWindow.View.ObjectSpace, "Micro");
                                }
                                else
                                {
                                    ReagentPrepLog newObj = Application.MainWindow.View.ObjectSpace.CreateObject<ReagentPrepLog>();
                                    ChangePrepLog(newObj, row, Application.MainWindow.View.ObjectSpace, "Micro");
                                    objPrep.ReagentPrepLogs.Add(newObj);
                                }
                            }
                        }
                        else
                        {
                            IObjectSpace os = ObjectSpace;
                            Modules.BusinessObjects.ReagentPreparation.ReagentPreparation newReagentPreparation = os.CreateObject<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>();
                            foreach (DataRow row in RPInfo.dtReagentPrepLog.Rows)
                            {
                                ReagentPrepLog newObj = os.CreateObject<ReagentPrepLog>();
                                ChangePrepLog(newObj, row, os, "Micro");
                                newReagentPreparation.ReagentPrepLogs.Add(newObj);
                            }
                            newReagentPreparation.PrepSelectType = PrepSelectTypes.MicroMediaAndReagentPrep;
                            DetailView detailView = Application.CreateDetailView(os, "ReagentPreparation_DetailView_MicroMedia", true, newReagentPreparation);
                            detailView.ViewEditMode = ViewEditMode.Edit;
                            Application.MainWindow.SetView(detailView);
                        }
                        Window window = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                        if (window != null)
                        {
                            ((DevExpress.ExpressApp.Web.PopupWindow)Frame).Close(true);
                        }
                        Application.MainWindow.View.Refresh();
                        Application.MainWindow.View.RefreshDataSource();

                        //ReagentPrepLog objCurReagent = (ReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        //dvPrep.InnerView.ObjectSpace.CommitChanges();
                        //IObjectSpace os = ObjectSpace;
                        //Modules.BusinessObjects.ReagentPreparation.ReagentPreparation newReagentPreparation = os.CreateObject<Modules.BusinessObjects.ReagentPreparation.ReagentPreparation>();
                        //List<ReagentPrepLog> lstReagentPreLog = os.GetObjects<ReagentPrepLog>(new InOperator("Oid", RPInfo.lstReagentPrepLog.Select(i => i.Oid))).Where(i => i.LabStock != null || i.VendorStock != null).ToList();
                        //foreach (ReagentPrepLog objNew in lstReagentPreLog.ToList())
                        //{
                        //    if (!newReagentPreparation.ReagentPrepLogs.Contains(objNew))
                        //    {
                        //    newReagentPreparation.ReagentPrepLogs.Add(objNew);
                        //}
                        //}
                        //newReagentPreparation.PrepSelectType = PrepSelectTypes.MicroMediaAndReagentPrep;
                        //Window window = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                        //if (window != null)
                        //{
                        //    ((DevExpress.ExpressApp.Web.PopupWindow)Frame).Close(true);
                        //}
                        //DetailView detailView = Application.CreateDetailView(os, "ReagentPreparation_DetailView_MicroMedia", true, newReagentPreparation);
                        //detailView.ViewEditMode = ViewEditMode.Edit;
                        //Application.MainWindow.SetView(detailView);
                        //Application.MainWindow.View.Refresh();
                        //Application.MainWindow.View.RefreshDataSource();
                    }
                }
                else if (View.Id == "ReagentPrepChemical_PrepNotepad")
                {
                    DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                    DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                    Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objOldReagentPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                    if (dvPrep != null && dvPrep.InnerView != null)
                    {
                        Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                        NPReagentPrepLog objCurReagent = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        if (Application.MainWindow.View is DetailView)
                        {
                            ListPropertyEditor lvPreps = ((DetailView)Application.MainWindow.View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                            foreach (DataRow row in RPInfo.dtReagentPrepLog.Rows)
                            {
                                ReagentPrepLog lvObj = ((ListView)lvPreps.ListView).CollectionSource.List.Cast<ReagentPrepLog>().ToList().Where(i => i.ComponentID == Convert.ToUInt16(row["ID"])).FirstOrDefault();
                                if (lvObj != null)
                                {
                                    ChangePrepLog(lvObj, row, Application.MainWindow.View.ObjectSpace, "Chemistry");
                                }
                                else
                                {
                                    ReagentPrepLog newObj = Application.MainWindow.View.ObjectSpace.CreateObject<ReagentPrepLog>();
                                    ChangePrepLog(newObj, row, Application.MainWindow.View.ObjectSpace, "Chemistry");
                                    objPrep.ReagentPrepLogs.Add(newObj);
                                }
                            }
                        }
                        Window window = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                        if (window != null)
                        {
                            ((DevExpress.ExpressApp.Web.PopupWindow)Frame).Close(true);
                        }
                        Application.MainWindow.View.Refresh();
                        Application.MainWindow.View.RefreshDataSource();

                        //ReagentPrepLog objCurReagent = (ReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        //dvPrep.InnerView.ObjectSpace.CommitChanges();
                        //List<ReagentPrepLog> lstReagentPreLog =Application.MainWindow.View.ObjectSpace.GetObjects<ReagentPrepLog>(new InOperator("Oid", RPInfo.lstReagentPrepLog.Select(i => i.Oid))).Where(i => i.LabStock != null || i.VendorStock != null).ToList();
                        //foreach (ReagentPrepLog objNew in lstReagentPreLog.ToList())
                        //{
                        //    if (!objOldReagentPrep.ReagentPrepLogs.Contains(objNew))
                        //    {
                        //        objOldReagentPrep.ReagentPrepLogs.Add(objNew);
                        //    }
                        //}
                        //Window window = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                        //if (window != null)
                        //{
                        //    ((DevExpress.ExpressApp.Web.PopupWindow)Frame).Close(true);
                        //}
                        //Application.MainWindow.View.Refresh();
                        //Application.MainWindow.View.RefreshDataSource();
                    }
                }
                else if (View.Id == "ReagentPrepMicroMedia_PrepNotepad")
                {
                    DashboardViewItem dvPrep = ((DashboardView)View).FindItem("dvReagentPrepLog") as DashboardViewItem;
                    DashboardViewItem lvPrep = ((DashboardView)View).FindItem("lvReagentPrepLog") as DashboardViewItem;
                    Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objOldReagentPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                    if (dvPrep != null && dvPrep.InnerView != null)
                    {
                        Modules.BusinessObjects.ReagentPreparation.ReagentPreparation objPrep = (Modules.BusinessObjects.ReagentPreparation.ReagentPreparation)Application.MainWindow.View.CurrentObject;
                        NPReagentPrepLog objCurReagent = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        if (Application.MainWindow.View is DetailView)
                        {
                            ListPropertyEditor lvPreps = ((DetailView)Application.MainWindow.View).FindItem("ReagentPrepLogs") as ListPropertyEditor;
                            foreach (DataRow row in RPInfo.dtReagentPrepLog.Rows)
                            {
                                ReagentPrepLog lvObj = ((ListView)lvPreps.ListView).CollectionSource.List.Cast<ReagentPrepLog>().ToList().Where(i => i.ComponentID == Convert.ToUInt16(row["ID"])).FirstOrDefault();
                                if (lvObj != null)
                                {
                                    ChangePrepLog(lvObj, row, Application.MainWindow.View.ObjectSpace, "Micro");
                                }
                                else
                                {
                                    ReagentPrepLog newObj = Application.MainWindow.View.ObjectSpace.CreateObject<ReagentPrepLog>();
                                    ChangePrepLog(newObj, row, Application.MainWindow.View.ObjectSpace, "Micro");
                                    objPrep.ReagentPrepLogs.Add(newObj);
                                }
                            }
                        }
                        Window window = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                        if (window != null)
                        {
                            ((DevExpress.ExpressApp.Web.PopupWindow)Frame).Close(true);
                        }
                        Application.MainWindow.View.Refresh();
                        Application.MainWindow.View.RefreshDataSource();


                        //ReagentPrepLog objCurReagent = (ReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        //dvPrep.InnerView.ObjectSpace.CommitChanges();
                        //List<ReagentPrepLog> lstReagentPreLog =Application.MainWindow.View.ObjectSpace.GetObjects<ReagentPrepLog>(new InOperator("Oid", RPInfo.lstReagentPrepLog.Select(i => i.Oid))).Where(i => i.LabStock != null || i.VendorStock != null).ToList();
                        //foreach (ReagentPrepLog objNew in lstReagentPreLog.ToList())
                        //{
                        //    if (!objOldReagentPrep.ReagentPrepLogs.Contains(objNew))
                        //    {
                        //        objOldReagentPrep.ReagentPrepLogs.Add(objNew);
                        //    }
                        //}
                        //Window window = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                        //if (window != null)
                        //{
                        //    ((DevExpress.ExpressApp.Web.PopupWindow)Frame).Close(true);
                        //}
                        //Application.MainWindow.View.Refresh();
                        //Application.MainWindow.View.RefreshDataSource();


                        //NPReagentPrepLog objCurReagent = (NPReagentPrepLog)dvPrep.InnerView.CurrentObject;
                        //dvPrep.InnerView.ObjectSpace.CommitChanges();
                        //List<ReagentPrepLog> lstReagentPreLog = Application.MainWindow.View.ObjectSpace.GetObjects<ReagentPrepLog>(new InOperator("Oid", RPInfo.lstReagentPrepLog.Select(i => i.Oid))).Where(i => i.LabStock != null || i.VendorStock != null).ToList();
                        //foreach (ReagentPrepLog objNew in lstReagentPreLog.ToList())
                        //{
                        //    if (!objOldReagentPrep.ReagentPrepLogs.Contains(objNew))
                        //    {
                        //        objOldReagentPrep.ReagentPrepLogs.Add(objNew);
                        //    }
                        //}
                        //Window window = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                        //if (window != null)
                        //{
                        //    ((DevExpress.ExpressApp.Web.PopupWindow)Frame).Close(true);
                        //}
                        //Application.MainWindow.View.Refresh();
                        //Application.MainWindow.View.RefreshDataSource();
                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void PrepNotePad_Excecute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                DashboardView dashboard = null;
                if (View.Id == "ReagentPreparation_DetailView_Chemistry")
                {
                    dashboard = Application.CreateDashboardView(os, "ReagentPrepChemical_PrepNotepad", false);
                }
                else
                {
                    dashboard = Application.CreateDashboardView(os, "ReagentPrepMicroMedia_PrepNotepad", false);
                }
                ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                showViewParameters.CreatedView.Caption = "Prep Notepad";
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.AcceptAction.Active.SetItemValue("Ok", false);
                dc.CancelAction.Active.SetItemValue("Cancel", false);
                dc.Accepting += Dc_Accepting;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void AppearenceappliedInEditorFormulaBased(ReagentPrepLog objPrepLog, DevExpress.ExpressApp.View view)
        {
            try
            {
                if (string.IsNullOrEmpty(objPrepLog.Formula))
                {
                    foreach (ViewItem item in ((DetailView)view).Items.Where(i => i.IsCaptionVisible))
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    propertyEditor.Editor.BackColor = Color.White;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    propertyEditor.Editor.BackColor = Color.White;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }

                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                    {
                                        propertyEditor.FindEdit.Editor.BackColor = Color.White;
                                    }
                                    else if (propertyEditor.DropDownEdit != null)
                                    {
                                        propertyEditor.DropDownEdit.DropDown.BackColor = Color.White;
                                    }
                                    else
                                    {
                                        propertyEditor.Editor.BackColor = Color.White;
                                    }
                                }
                                else
                                {
                                    if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                    {
                                        propertyEditor.FindEdit.Editor.BackColor = Color.LightGray;
                                    }
                                    else if (propertyEditor.DropDownEdit != null)
                                    {
                                        propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightGray;
                                    }
                                    else
                                    {
                                        propertyEditor.Editor.BackColor = Color.LightGray;
                                    }
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    propertyEditor.Editor.BackColor = Color.White;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    propertyEditor.Editor.BackColor = Color.White;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    propertyEditor.Editor.BackColor = Color.White;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }

                    }
                }
                else
                {
                    if (objPrepLog.CalculationApproach != null && !objPrepLog.CalculationApproach.Approach.Contains("None"))
                    {
                        string str = objPrepLog.CalculationApproach.Approach.Split(' ').ToList().LastOrDefault().ToString();
                        if (objPrepLog.CalculationApproach.Approach.Contains("V1") && objPrepLog.InitialVolTaken_V1_Units != null && objPrepLog.StockConc_C1_Units != null && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                        {
                            DisableV1andC1Units(true, false, view);
                            List<RegentPrepCalculationEditor> lstCalculation = view.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                            if (lstCalculation.Count > 0)
                            {
                                DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str, view);
                                if (str == "C1")
                                {
                                    objPrepLog.StockConc_C1 = null;
                                    ASPxStringPropertyEditor StockConc_C1 = ((DetailView)view).FindItem("StockConc_C1") as ASPxStringPropertyEditor;
                                    if (StockConc_C1 != null)
                                    {
                                        StockConc_C1.AllowEdit.SetItemValue("AlowEdit", false);
                                        if (StockConc_C1 != null && StockConc_C1.Editor != null)
                                        {
                                            StockConc_C1.Editor.BackColor = Color.Yellow;
                                        }
                                    }
                                }
                                else if (str == "C2")
                                {
                                    objPrepLog.FinalConc_C2 = null;
                                    ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)view).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                    if (FinalConc_C2 != null)
                                    {
                                        FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                        if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                        {
                                            FinalConc_C2.Editor.BackColor = Color.Yellow;
                                        }
                                    }
                                }
                                else if (str == "V1")
                                {
                                    objPrepLog.VolumeTaken_V1 = null;
                                    ASPxStringPropertyEditor VolumeTaken_V1 = ((DetailView)view).FindItem("VolumeTaken_V1") as ASPxStringPropertyEditor;
                                    if (VolumeTaken_V1 != null)
                                    {
                                        VolumeTaken_V1.AllowEdit.SetItemValue("AlowEdit", false);
                                        if (VolumeTaken_V1 != null && VolumeTaken_V1.Editor != null)
                                        {
                                            VolumeTaken_V1.Editor.BackColor = Color.Yellow;
                                        }
                                    }
                                }
                                else if (str == "V2")
                                {
                                    objPrepLog.FinalVol_V2 = null;
                                    ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)view).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                    if (FinalVol_V2 != null)
                                    {
                                        FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                        if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                        {
                                            FinalVol_V2.Editor.BackColor = Color.Yellow;
                                        }
                                    }
                                }
                            }

                        }
                        else if (objPrepLog.CalculationApproach.Approach.Contains("W1") && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                        {
                            DisableV1andC1Units(false, true, view);
                            List<RegentPrepCalculationEditor> lstCalculation = view.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                            if (lstCalculation.Count > 0)
                            {
                                RegentPrepCalculationEditor objCalculationEditor = lstCalculation.FirstOrDefault(i => i.C2Units != null && i.V2Units != null && i.C2Units == objPrepLog.FinalConc_C2_Units && i.V2Units == objPrepLog.FinalVol_V2_Units);
                                if (objCalculationEditor != null)
                                {
                                    DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str, view);
                                    if (str == "C2")
                                    {
                                        objPrepLog.FinalConc_C2 = null;
                                        ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)view).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                        if (FinalConc_C2 != null)
                                        {
                                            FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                            if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                            {
                                                FinalConc_C2.Editor.BackColor = Color.Yellow;
                                            }
                                        }
                                    }
                                    else if (str == "W1")
                                    {
                                        objPrepLog.VolumeTaken_V1 = null;
                                        ASPxStringPropertyEditor Weight_g_w1 = ((DetailView)view).FindItem("Weight_g_w1") as ASPxStringPropertyEditor;
                                        if (Weight_g_w1 != null)
                                        {
                                            Weight_g_w1.AllowEdit.SetItemValue("AlowEdit", false);
                                            if (Weight_g_w1 != null && Weight_g_w1.Editor != null)
                                            {
                                                Weight_g_w1.Editor.BackColor = Color.Yellow;
                                            }
                                        }
                                    }
                                    else if (str == "V2")
                                    {
                                        objPrepLog.FinalVol_V2 = null;
                                        ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)view).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                        if (FinalVol_V2 != null)
                                        {
                                            FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                            if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                            {
                                                FinalVol_V2.Editor.BackColor = Color.Yellow;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        foreach (ViewItem item in ((DetailView)view).Items.Where(i => i.IsCaptionVisible))
                        {
                            if (item.GetType() == typeof(ASPxStringPropertyEditor))
                            {
                                ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }
                            }
                            else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                            {
                                ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }

                            }
                            else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                            {
                                ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    if (propertyEditor.Id == "Component")
                                    {
                                        propertyEditor.Editor.BackColor = Color.LightGray;
                                    }
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }
                            }
                            else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                            {
                                ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }

                            }
                            else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                            {
                                ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    if (propertyEditor.Id == "LLTExpDate" || propertyEditor.Id == "LTExpDate")
                                    {
                                        propertyEditor.Editor.BackColor = Color.LightGray;
                                    }
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }

                            }

                        }
                    }
                    else
                    {
                        foreach (ViewItem item in ((DetailView)view).Items.Where(i => i.IsCaptionVisible))
                        {
                            if (item.GetType() == typeof(ASPxStringPropertyEditor))
                            {
                                ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;

                                }
                            }
                            else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                            {
                                ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                    propertyEditor.Editor.BackColor = Color.White;
                                }

                            }
                            else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                            {
                                ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    if (propertyEditor.Id == "Component")
                                    {
                                        propertyEditor.Editor.BackColor = Color.LightGray;
                                    }
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                    propertyEditor.Editor.BackColor = Color.White;
                                }
                            }
                            else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                            {
                                ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                    propertyEditor.Editor.BackColor = Color.White;
                                    propertyEditor.AllowEdit.SetItemValue("AlowEdit", true);
                                }
                            }
                            else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                            {
                                ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    if (propertyEditor.Id == "LLTExpDate" || propertyEditor.Id == "LTExpDate")
                                    {
                                        propertyEditor.Editor.BackColor = Color.LightGray;
                                    }
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                    propertyEditor.Editor.BackColor = Color.White;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void AppearenceappliedInEditorFormulaBased(NPReagentPrepLog objPrepLog, DevExpress.ExpressApp.View view)
        {
            try
        {
            if (string.IsNullOrEmpty(objPrepLog.Formula))
            {
                foreach (ViewItem item in ((DetailView)view).Items.Where(i => i.IsCaptionVisible))
                {
                    if (item.GetType() == typeof(ASPxStringPropertyEditor))
                    {
                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            if (propertyEditor.AllowEdit)
                            {
                                propertyEditor.Editor.BackColor = Color.White;
                            }
                            else
                            {
                                propertyEditor.Editor.BackColor = Color.LightGray;
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                    }
                    else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                    {
                        ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            if (propertyEditor.AllowEdit)
                            {
                                propertyEditor.Editor.BackColor = Color.White;
                            }
                            else
                            {
                                propertyEditor.Editor.BackColor = Color.LightGray;
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }

                    }
                    else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                    {
                        ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            if (propertyEditor.AllowEdit)
                            {
                                if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                {
                                    propertyEditor.FindEdit.Editor.BackColor = Color.White;
                                }
                                else if (propertyEditor.DropDownEdit != null)
                                {
                                    propertyEditor.DropDownEdit.DropDown.BackColor = Color.White;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.White;
                                }
                            }
                            else
                            {
                                if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                {
                                    propertyEditor.FindEdit.Editor.BackColor = Color.LightGray;
                                }
                                else if (propertyEditor.DropDownEdit != null)
                                {
                                    propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightGray;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                    }
                    else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                    {
                        ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            if (propertyEditor.AllowEdit)
                            {
                                propertyEditor.Editor.BackColor = Color.White;
                            }
                            else
                            {
                                propertyEditor.Editor.BackColor = Color.LightGray;
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                    }
                    else if (item.GetType() == typeof(ASPxStringPropertyEditor))
                    {
                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            if (propertyEditor.AllowEdit)
                            {
                                propertyEditor.Editor.BackColor = Color.White;
                            }
                            else
                            {
                                propertyEditor.Editor.BackColor = Color.LightGray;
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }

                    }
                    else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                    {
                        ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            if (propertyEditor.AllowEdit)
                            {
                                propertyEditor.Editor.BackColor = Color.White;
                            }
                            else
                            {
                                propertyEditor.Editor.BackColor = Color.LightGray;
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }

                    }

                }
            }
            else
            {
                if (objPrepLog.CalculationApproach != null && !objPrepLog.CalculationApproach.Approach.Contains("None"))
                {
                    string str = objPrepLog.CalculationApproach.Approach.Split(' ').ToList().LastOrDefault().ToString();
                    if (objPrepLog.CalculationApproach.Approach.Contains("V1") && objPrepLog.InitialVolTaken_V1_Units != null && objPrepLog.StockConc_C1_Units != null && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                    {
                        DisableV1andC1Units(true, false,view);
                        List<RegentPrepCalculationEditor> lstCalculation = view.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                        if (lstCalculation.Count > 0)
                        {
                            DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str, view);
                            if (str == "C1")
                            {
                                objPrepLog.StockConc_C1 = null;
                                ASPxStringPropertyEditor StockConc_C1 = ((DetailView)view).FindItem("StockConc_C1") as ASPxStringPropertyEditor;
                                if (StockConc_C1 != null)
                                {
                                    StockConc_C1.AllowEdit.SetItemValue("AlowEdit", false);
                                    if (StockConc_C1 != null && StockConc_C1.Editor != null)
                                    {
                                        StockConc_C1.Editor.BackColor = Color.Yellow;
                                    }
                                }
                            }
                            else if (str == "C2")
                            {
                                objPrepLog.FinalConc_C2 = null;
                                ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)view).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                if (FinalConc_C2 != null)
                                {
                                    FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                    if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                    {
                                        FinalConc_C2.Editor.BackColor = Color.Yellow;
                                    }
                                }
                            }
                            else if (str == "V1")
                            {
                                objPrepLog.VolumeTaken_V1 = null;
                                ASPxStringPropertyEditor VolumeTaken_V1 = ((DetailView)view).FindItem("VolumeTaken_V1") as ASPxStringPropertyEditor;
                                if (VolumeTaken_V1 != null)
                                {
                                    VolumeTaken_V1.AllowEdit.SetItemValue("AlowEdit", false);
                                    if (VolumeTaken_V1 != null && VolumeTaken_V1.Editor != null)
                                    {
                                        VolumeTaken_V1.Editor.BackColor = Color.Yellow;
                                    }
                                }

                            }
                            else if (str == "V2")
                            {
                                objPrepLog.FinalVol_V2 = null;
                                ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)view).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                if (FinalVol_V2 != null)
                                {
                                    FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                    if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                    {
                                        FinalVol_V2.Editor.BackColor = Color.Yellow;
                                    }
                                }
                            }
                        }
                    }
                    else if (objPrepLog.CalculationApproach.Approach.Contains("W1") && objPrepLog.FinalVol_V2_Units != null && objPrepLog.FinalConc_C2_Units != null)
                    {
                        DisableV1andC1Units(false, true,view);
                        List<RegentPrepCalculationEditor> lstCalculation = view.ObjectSpace.GetObjects<RegentPrepCalculationEditor>(CriteriaOperator.Parse("[CalculationApproch.Oid] = ?", objPrepLog.CalculationApproach.Oid)).ToList();
                        if (lstCalculation.Count > 0)
                        {
                            RegentPrepCalculationEditor objCalculationEditor = lstCalculation.FirstOrDefault(i => i.C2Units != null && i.V2Units != null && i.C2Units == objPrepLog.FinalConc_C2_Units && i.V2Units == objPrepLog.FinalVol_V2_Units);
                            if (objCalculationEditor != null)
                            {
                                DisableControlsInFormulaBased(objPrepLog.Formula.Split('=').LastOrDefault(), str,view);
                                if (str == "C2")
                                {
                                    objPrepLog.FinalConc_C2 = null;
                                    ASPxStringPropertyEditor FinalConc_C2 = ((DetailView)view).FindItem("FinalConc_C2") as ASPxStringPropertyEditor;
                                    if (FinalConc_C2 != null)
                                    {
                                        FinalConc_C2.AllowEdit.SetItemValue("AlowEdit", false);
                                        if (FinalConc_C2 != null && FinalConc_C2.Editor != null)
                                        {
                                            FinalConc_C2.Editor.BackColor = Color.Yellow;
                                        }
                                    }
                                }
                                else if (str == "W1")
                                {
                                    objPrepLog.VolumeTaken_V1 = null;
                                    ASPxStringPropertyEditor Weight_g_w1 = ((DetailView)view).FindItem("Weight_g_w1") as ASPxStringPropertyEditor;
                                    if (Weight_g_w1 != null)
                                    {
                                        Weight_g_w1.AllowEdit.SetItemValue("AlowEdit", false);
                                        if (Weight_g_w1 != null && Weight_g_w1.Editor != null)
                                        {
                                            Weight_g_w1.Editor.BackColor = Color.Yellow;
                                        }
                                    }
                                }
                                else if (str == "V2")
                                {
                                    objPrepLog.FinalVol_V2 = null;
                                    ASPxStringPropertyEditor FinalVol_V2 = ((DetailView)view).FindItem("FinalVol_V2") as ASPxStringPropertyEditor;
                                    if (FinalVol_V2 != null)
                                    {
                                        FinalVol_V2.AllowEdit.SetItemValue("AlowEdit", false);
                                        if (FinalVol_V2 != null && FinalVol_V2.Editor != null)
                                        {
                                            FinalVol_V2.Editor.BackColor = Color.Yellow;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (ViewItem item in ((DetailView)view).Items.Where(i => i.IsCaptionVisible))
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }

                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }

                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.Id == "Component")
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                        {
                            ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }

                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.Id == "LLTExpDate" || propertyEditor.Id == "LTExpDate")
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }

                        }

                    }
                }
                else
                {
                    foreach (ViewItem item in ((DetailView)view).Items.Where(i => i.IsCaptionVisible))
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;

                            }

                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                                propertyEditor.Editor.BackColor = Color.White;
                            }

                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.Id == "Component")
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                                propertyEditor.Editor.ForeColor = Color.Black;
                                propertyEditor.Editor.BackColor = Color.White;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                        {
                            ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                                propertyEditor.Editor.BackColor = Color.White;
                                propertyEditor.AllowEdit.SetItemValue("AlowEdit", true);
                            }

                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.Id == "LLTExpDate" || propertyEditor.Id == "LTExpDate")
                                {
                                    propertyEditor.Editor.BackColor = Color.LightGray;
                                }
                                propertyEditor.Editor.ForeColor = Color.Black;
                                propertyEditor.Editor.BackColor = Color.White;
                            }

                        }

                    }
                }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void GridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    GridViewContextMenuItem item = null;
                    item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                    {
                        e.Items.Remove(item);
                    }
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";
                    e.Items.Remove(e.Items.FindByText("Edit"));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
