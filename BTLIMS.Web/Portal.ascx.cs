// ================================================================================
// Table Name: [Portal]
// Author: Sunny
// Date: 2017年04月06日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：自定义Portal页
// ================================================================================
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
//using Modules.BusinessObjects.Assets;
//using Modules.BusinessObjects.Biz;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Web;
using System.Web.Security;

namespace BTLIMS.Web
{
    public partial class Portal : System.Web.UI.UserControl, IComplexControl
    {
        private IObjectSpace _objectSpace = null;
        private XafApplication _application = null;
        MessageTimer timer = new MessageTimer();
        curlanguage objLanguage = new curlanguage();

        protected void Page_Load(object sender, EventArgs e)
        {
            SetModuleLiTitle();
            SetModuleLiVisible();
            Session["IsFirstLogin"] = false;
            //lblCount.Attributes["data-count"] = "10";
        }
        /// <summary>
        /// 调用全局资源文件
        /// </summary>
        /// <param name="resources">文件名，不带后缀</param>
        /// <param name="resourceKey">资源文件的key</param>
        /// <returns>返回资源文件的值</returns>
        private string GetGlobalResources(string resources, string resourceKey)
        {
            var objName = HttpContext.GetGlobalResourceObject(resources, resourceKey);
            return objName != null ? objName.ToString() : resourceKey;
        }
        /// <summary>
        /// 根据用户设置的系统语言翻译Portal页面
        /// </summary>
        private void SetModuleLiTitle()
        {
            string resources = "LocalizeResourcesChinese";
            var currentUser = SecuritySystem.CurrentUser as Employee;
            //var os = _application.CreateObjectSpace();
            //Session currentSession = ((XPObjectSpace)(os)).Session;
            //SelectedData sproc = currentSession.ExecuteSproc("getCurrentLanguage", "");
            //var CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
            if (objLanguage.strcurlanguage == "En")
            {
                resources = "LocalizeResourcesEnglish";
            }

            //if (currentUser != null)
            //{
            //    //取得员工所在公司的统一语言，如果个人设置了语言，则以个人为准
            //    var language = currentUser.IsEnabledLanguage ? currentUser.Language.ToString() : currentUser.Company.Language.ToString();
            //    if (language != "Chinese")
            //    {
            //        resources = "LocalizeResourcesEnglish";
            //    }
            //}
            //我的账号
            ltlMyAccount.Text = GetGlobalResources(resources, "MyAccount");
            ltlLogOff.Text = GetGlobalResources(resources, "LogOff");
            //管理看板
            ltlDashboard.Text = GetGlobalResources(resources, "Dashboard");
            ltlTaskAssignment.Text = GetGlobalResources(resources, "TaskAssignment");
            ltlResultEntry.Text = GetGlobalResources(resources, "ResultEntry");
            ltlReportReview.Text = GetGlobalResources(resources, "ReportReview");
            ltlReportApproval.Text = GetGlobalResources(resources, "ReportApproval");
            ltlReportPrint.Text = GetGlobalResources(resources, "ReportPrint");
            ltlCertificateReception.Text = GetGlobalResources(resources, "CertificateReception");
            ltlCertificateRelease.Text = GetGlobalResources(resources, "CertificateRelease");
            ltlBusinessArchiving.Text = GetGlobalResources(resources, "BusinessArchiving");
            ltlInstrumentStorage.Text = GetGlobalResources(resources, "InstrumentStorage");
            ltlTaskReceipt.Text = GetGlobalResources(resources, "TaskReceipt");
            ltlInstrumentReturn.Text = GetGlobalResources(resources, "InstrumentReturn");
            ltlReturnConfirmation.Text = GetGlobalResources(resources, "ReturnConfirmation");
            ltlInstrumentRelease.Text = GetGlobalResources(resources, "InstrumentRelease");
            //应用中心
            ltlApplicationCenter.Text = GetGlobalResources(resources, "ApplicationCenter");
            ////业务系统
            ltlBusinessSystem.Text = GetGlobalResources(resources, "BusinessSystem");
            ltlMeasurementBusiness.Text = GetGlobalResources(resources, "MeasurementBusiness");
            ltlQualityTestingBusiness.Text = GetGlobalResources(resources, "QualityTestingBusiness");
            ////通用数据
            ltlGeneralData.Text = GetGlobalResources(resources, "GeneralData");
            ltlHumanResources.Text = GetGlobalResources(resources, "HumanResources");
            ltlAssetManagement.Text = GetGlobalResources(resources, "AssetManagement");
            ltlCustomerManagement.Text = GetGlobalResources(resources, "CustomerManagement");
            ltlConfiguration.Text = GetGlobalResources(resources, "Configuration");
            ltlReports.Text = GetGlobalResources(resources, "Reports");
            ltlSystemManagement.Text = GetGlobalResources(resources, "SystemManagement");
            ltlDataEntry.Text = GetGlobalResources(resources, "DataEntry");
            ltlSample.Text = GetGlobalResources(resources, "Sample Management");
            ltlSetting.Text = GetGlobalResources(resources, "Settings");
        }

        //private IObjectSpace CreateObjectSpace()
        //{
        //    throw new NotImplementedException();
        //}

        #region 根据权限设置Portal中各模块是否显示-待办数量计量

        /// <summary>
        /// 1.根据当前登陆人显示对应模块
        /// 2.获取待办数据
        /// </summary>
        private void SetModuleLiVisible()
        {
            var cUser = SecuritySystem.CurrentUser as CustomSystemUser;
            var cEmp = SecuritySystem.CurrentUser as Employee;
            if (cUser == null) return;
            if (cEmp == null) return;

            var isExistRoleYp = cUser.IsUserInRole("样品管理员");
            var isExistRoleJd = cUser.IsUserInRole("检定员");
            var isExistRoleSh = cUser.IsUserInRole("审核员");
            var isExistRolePz = cUser.IsUserInRole("批准人");
            var isExistRoleDy = cUser.IsUserInRole("打印专员");
            var isExistRoleAdministrator = cUser.IsUserInRole("Administrator");
            var isExistRoleAdmin = cUser.IsUserInRole("系统管理员");

            if (isExistRoleAdministrator)
            {
                //管理看板
                divKB.Visible = true;

                //待办数据
                liAllocation.Visible = true;    //任务分配
                liReception.Visible = true;     //证书接收
                liRelease.Visible = true;       //证书发放
                liArchiving.Visible = true;     //业务归档
                liInStorage.Visible = true;     //器具入库
                liConfirmation.Visible = true;  //归还确认
                liSampleRelease.Visible = true; //器具发放
                liRegistration.Visible = true;  //结果登记
                liUnclaimed.Visible = true;     //任务领取
                liReturned.Visible = true;      //归还
                liAudit.Visible = true;         //审核
                liPrinting.Visible = true;      //打印
                liReport.Visible = true;
                liApproval.Visible = true;      //批准
                //应用中心
                divYY.Visible = true;
                //业务系统外层Div
                divBizTitle.Visible = true;
                divBizContent.Visible = true;

                //业务模块
                liBizJL.Visible = true;           //计量业务  

                //通用数据外层Div
                divDataTitle.Visible = true;
                divDataContent.Visible = true;
                //通用数据模块 
                liAssets.Visible = true;
                liHr.Visible = true;
                liCrm.Visible = true;
                liConfig.Visible = true;
                liReport.Visible = true;
                liSystem.Visible = true;
                liDataEntry.Visible = true;
                liSettings.Visible = true;
                liSample.Visible = true;

            }
            else if (!isExistRoleAdmin)
            {
                //管理看板
                divKB.Visible = true;
                //应用中心
                divYY.Visible = true;

                #region 样品管理员
                if (isExistRoleYp)  //样品管理员
                {
                    //业务系统外层Div
                    divBizTitle.Visible = true;
                    divBizContent.Visible = true;
                    //待办数据
                    liAllocation.Visible = true;    //任务分配
                    liReception.Visible = true;     //证书接收
                    liRelease.Visible = true;       //证书发放
                    liArchiving.Visible = true;     //业务归档
                    liInStorage.Visible = true;     //器具入库
                    liConfirmation.Visible = true;  //归还确认
                    liSampleRelease.Visible = true; //器具发放
                    //业务模块
                    liBizJL.Visible = true;           //计量业务

                    //var allocationCriteria = CriteriaOperator.Parse("Company = ? And CreatedBy = ? And Status = ?", 
                    //    _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), BizStatus.Allocation);                      //待任务分配
                    //var receptionCriteria = CriteriaOperator.Parse("Company = ? And CreatedBy = ? And Status = ?", 
                    //    _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), BizStatus.Reception);                       //待证书接收
                    //var releaseCriteria = CriteriaOperator.Parse("Company = ? And CreatedBy = ? And Status = ?", 
                    //    _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), BizStatus.Release);                         //待证书发放
                    //var archivingCriteria = CriteriaOperator.Parse("Company = ? And CreatedBy = ? And Status = ?", 
                    //    _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), BizStatus.Archiving);                       //待业务归档

                    //var inStorageCriteria = CriteriaOperator.Parse("Company = ? And CreatedBy = ? And Status = ? And SampleStatus = ?",
                    //    _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), BizStatus.InStorage, SampleStatus.InStorage);      //待器具入库
                    //var confirmationCriteria = CriteriaOperator.Parse("Company = ? And CreatedBy = ? And SampleStatus = ?",
                    //     _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), SampleStatus.Confirmation); //待归还确认
                    //var sampleReleaseCriteria = CriteriaOperator.Parse("Company = ? And CreatedBy = ? And SampleStatus = ?",
                    //    _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), SampleStatus.Release);  //待器具发放


                    //var allocationCount = _objectSpace.GetObjects<BizMeasure>(allocationCriteria).Count;
                    //var receptionCount = _objectSpace.GetObjects<BizMeasure>(receptionCriteria).Count;
                    //var releaseCount = _objectSpace.GetObjects<BizMeasure>(releaseCriteria).Count;
                    //var archivingCount = _objectSpace.GetObjects<BizMeasure>(archivingCriteria).Count;

                    //var inStorageCount = _objectSpace.GetObjects<BizMeasure>(inStorageCriteria).Count;
                    //var confirmationCount = _objectSpace.GetObjects<BizMeasure>(confirmationCriteria).Count;
                    //var sampleReleaseCount = _objectSpace.GetObjects<BizMeasure>(sampleReleaseCriteria).Count;

                    //ltlAllocation.Text = allocationCount.ToString();
                    //ltlReception.Text = receptionCount.ToString();
                    //ltlRelease.Text = releaseCount.ToString();
                    //ltlArchiving.Text = archivingCount.ToString();

                    //ltlInStorage.Text = inStorageCount.ToString();
                    //ltlConfirmation.Text = confirmationCount.ToString();
                    //ltlSampleRelease.Text = sampleReleaseCount.ToString();

                }
                #endregion

                #region 检定员
                if (isExistRoleJd)  //检定员
                {
                    //业务系统外层Div
                    divBizTitle.Visible = true;
                    divBizContent.Visible = true;
                    //业务模块
                    liBizJL.Visible = true;           //计量业务
                    //待办数据
                    liRegistration.Visible = true;  //结果登记
                    liUnclaimed.Visible = true;     //任务领取
                    //liPrinting.Visible = true;      //打印--转移到打印专员-20170504
                    liReturned.Visible = true;      //归还

                    //获取待办数据                                          
                    //var registrationCriteria = CriteriaOperator.Parse("Company = ? And MainInspector = ? And Status = ?", 
                    //    _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), BizStatus.Registration);                        //待结果登记
                    //var unclaimedCriteria = CriteriaOperator.Parse("Company = ? And MainInspector = ? And Status = ?", 
                    //    _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), BizStatus.Unclaimed);                           //待任务领取
                    //var returnedCriteria = CriteriaOperator.Parse("Company = ? And MainInspector = ? And SampleStatus = ?", 
                    //    _objectSpace.GetObject(cEmp.Company), _objectSpace.GetObject(cUser), SampleStatus.Returned);                         //待归还

                    //var registrationCount = _objectSpace.GetObjects<BizMeasure>(registrationCriteria).Count;
                    //var unclaimedCount = _objectSpace.GetObjects<BizMeasure>(unclaimedCriteria).Count;
                    //var returnedCount = _objectSpace.GetObjects<BizMeasure>(returnedCriteria).Count;

                    //ltlRegistration.Text = registrationCount.ToString();
                    //ltlUnclaimed.Text = unclaimedCount.ToString();
                    //ltlReturned.Text = returnedCount.ToString();
                }
                #endregion

                #region 审核员
                if (isExistRoleSh) //审核员
                {
                    //业务系统外层Div
                    divBizTitle.Visible = true;
                    divBizContent.Visible = true;
                    //业务模块
                    liBizJL.Visible = true;           //计量业务
                    //待办数据
                    liAudit.Visible = true;         //审核

                    //var auditCriteria = CriteriaOperator.Parse("Company = ? And Status = ? And MainInspector <> ?", 
                    //    _objectSpace.GetObject(cEmp.Company), BizStatus.Audit, _objectSpace.GetObject(cUser));                        //待审核
                    //var auditCount = _objectSpace.GetObjects<BizMeasure>(auditCriteria).Count;

                    //ltlAudit.Text = auditCount.ToString();
                }
                #endregion

                #region 批准人
                if (isExistRolePz) //批准人
                {
                    //业务系统外层Div
                    divBizTitle.Visible = true;
                    divBizContent.Visible = true;
                    //业务模块
                    liBizJL.Visible = true;           //计量业务
                    //待办数据
                    liApproval.Visible = true;      //批准

                    //var approvalCriteria = CriteriaOperator.Parse("Company = ? And Status = ? And MainInspector <> ?", 
                    //    _objectSpace.GetObject(cEmp.Company), BizStatus.Approval, _objectSpace.GetObject(cUser));                        //待批准
                    //var approvalCount = _objectSpace.GetObjects<BizMeasure>(approvalCriteria).Count;
                    //ltlApproval.Text = approvalCount.ToString();
                }
                #endregion

                #region 打印专员
                if (isExistRoleDy)
                {
                    //业务系统外层Div
                    divBizTitle.Visible = true;
                    divBizContent.Visible = true;
                    //业务模块
                    liBizJL.Visible = true;           //计量业务
                    //待办数据
                    liPrinting.Visible = true;      //打印
                    //var printingCriteria = CriteriaOperator.Parse("Company = ? And Status = ?", 
                    //    _objectSpace.GetObject(cEmp.Company), BizStatus.Printing);                                 //待报告打印，打印功能只给朱春凤-20170504
                    //var printingCount = _objectSpace.GetObjects<BizMeasure>(printingCriteria).Count;
                    //ltlPrinting.Text = printingCount.ToString();
                }
                #endregion

                #region 报表-所有用户都有报表模块权限
                divDataTitle.Visible = true;
                divDataContent.Visible = true;
                liReport.Visible = true;
                #endregion

            }
            else
            {
                //应用中心
                divYY.Visible = true;
                //通用数据外层Div
                divDataTitle.Visible = true;
                divDataContent.Visible = true;
                //通用数据模块 
                liAssets.Visible = true;
                liHr.Visible = true;
                liCrm.Visible = true;
                liSettings.Visible = true;
                liReport.Visible = true;
                liSystem.Visible = true;
            }
        }
        #endregion

        #region 管理看板-待办按钮事件
        //任务分配
        protected void btnAllocation_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Allocation", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Allocation";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入结果登记";
            //    log.Remark = "从Portal进入结果登记异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //结果登记
        protected void btnRegistration_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Registration", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Registration";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入结果登记";
            //    log.Remark = "从Portal进入结果登记异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //任务领取，就是器具领取
        //BizRequest_Measures_ListView_Sample_Verified
        protected void btnUnclaimed_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Sample_Unclaimed", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Sample_Unclaimed";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入任务领取";
            //    log.Remark = "从Portal进入任务领取异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //审核
        protected void btnAudit_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Audit", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Audit";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入审核";
            //    log.Remark = "从Portal进入审核异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //批准
        protected void btnApproval_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Approval", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Approval";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入批准";
            //    log.Remark = "从Portal进入批准异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //打印
        protected void btnPrinting_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Printing", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Printing";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入打印";
            //    log.Remark = "从Portal进入打印异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //证书接收
        protected void btnReception_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Reception", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Reception";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入证书接收";
            //    log.Remark = "从Portal进入证书接收异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //证书发放
        protected void btnRelease_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Release", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Release";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入证书发放";
            //    log.Remark = "从Portal进入证书发放异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //业务归档
        protected void btnArchiving_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Archiving", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Archiving";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入业务归档";
            //    log.Remark = "从Portal进入业务归档异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //} 
        }
        //器具入库
        protected void btnInStorage_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Sample_InStorage", typeof(BizMeasure));
        }

        //器具归还
        protected void btnReturned_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Sample_Returned", typeof(BizMeasure));
            //var listViewId = "BizRequest_Measures_ListView_Sample_Returned";
            //if (_application == null) return;
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
            //    if (cs == null) return;
            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };
            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入器具归还";
            //    log.Remark = "从Portal进入器具归还异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //归还确认
        protected void btnConfirmation_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Sample_Confirmation", typeof(BizMeasure));
        }
        //器具发放
        protected void btnSampleRelease_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Sample_Release", typeof(BizMeasure));
        }
        #endregion

        #region 业务模块-应用中心

        //计量业务
        protected void btnBiz_OnClick(object sender, EventArgs e)
        {
            var os = _application.CreateObjectSpace();
            if (os == null) return;
            try
            {
                var cUser = SecuritySystem.CurrentUser as CustomSystemUser;
                var cEmp = SecuritySystem.CurrentUser as Employee;
                if (cUser == null) return;
                if (cEmp == null) return;

                var listViewId = "BizRequest_ListView";
                var isExistRoleYp = cUser.IsUserInRole("样品管理员");
                var isExistRoleJd = cUser.IsUserInRole("检定员");
                var isExistRoleSh = cUser.IsUserInRole("审核员");
                var isExistRolePz = cUser.IsUserInRole("批准人");
                var isExistRoleAdmin = cUser.IsUserInRole("Administrator");

                //var cs = _application.CreateCollectionSource(os, typeof(BizMeasure), listViewId);
                //if (cs == null) return;
                //if (isExistRoleYp)
                //{
                //    listViewId = "BizRequest_ListView";
                //    cs = _application.CreateCollectionSource(os, typeof(BizRequest), listViewId);
                //}
                if (isExistRoleJd)
                {
                    listViewId = "BizRequest_Measures_ListView_Registration";
                }
                if (isExistRoleSh)
                {
                    listViewId = "BizRequest_Measures_ListView_Audit";
                }
                if (isExistRolePz)
                {
                    listViewId = "BizRequest_Measures_ListView_Approval";
                }

                //var lv = _application.CreateListView(listViewId, cs, true);
                //if (lv == null) return;
                //var showViewParameters = new ShowViewParameters { CreatedView = lv };

                //_application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                #region 操作日志
                var log = os.CreateObject<OperationLog>();
                log.Category = OperationCategory.Error;
                log.OperationEvent = "进入计量业务";
                log.Remark = "从Portal进入计量业务异常：" + ex;
                #endregion
                log.Save();
                os.CommitChanges();
            }
        }
        //人力资源
        protected void btnHr_OnClick(object sender, EventArgs e)
        {
            RedirectDashboardToListView("Company_ListView", typeof(Company));
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var listViewId = "Company_ListView";
            //    var cs = _application.CreateCollectionSource(os, typeof(Company), listViewId);
            //    if (cs == null) return;

            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };

            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入人力资源";
            //    log.Remark = "从Portal进入人力资源异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //资产管理
        protected void btnAssets_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("Labware_ListView", typeof(Labware));
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var listViewId = "Labware_ListView";
            //    var cs = _application.CreateCollectionSource(os, typeof(Labware), listViewId);
            //    if (cs == null) return;

            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };

            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入资产管理";
            //    log.Remark = "从Portal进入资产管理异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        //客户管理
        protected void btnCrm_OnClick(object sender, EventArgs e)
        {
            RedirectDashboardToListView("Customer_ListView", typeof(Customer));
            var os = _application.CreateObjectSpace();
            if (os == null) return;
            try
            {
                var listViewId = "Customer_ListView";
                var cs = _application.CreateCollectionSource(os, typeof(Customer), listViewId);
                if (cs == null) return;

                var lv = _application.CreateListView(listViewId, cs, true);
                if (lv == null) return;
                var showViewParameters = new ShowViewParameters { CreatedView = lv };

                _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                #region 操作日志
                var log = os.CreateObject<OperationLog>();
                log.Category = OperationCategory.Error;
                log.OperationEvent = "进入客户管理";
                log.Remark = "从Portal进入客户管理异常：" + ex;
                #endregion
                log.Save();
                os.CommitChanges();
            }
        }
        //基础配置
        protected void btnSeting_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("MeasureMethodLibrary_ListView", typeof(MeasureMethodLibrary));
            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var listViewId = "MeasureMethodLibrary_ListView";
            //    var cs = _application.CreateCollectionSource(os, typeof(MeasureMethodLibrary), listViewId);
            //    if (cs == null) return;

            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };

            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入基础配置";
            //    log.Remark = "从Portal进入基础配置异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }

        protected void btnReport_OnClick(object sender, EventArgs e)
        {
            //RedirectDashboardToListView("BizRequest_Measures_ListView_Report_Order", typeof(BizMeasure));
        }

        //系统管理
        protected void btnSystem_OnClick(object sender, EventArgs e)
        {
            RedirectDashboardToListView("CustomSystemRole_ListView", typeof(CustomSystemRole));

            //var os = _application.CreateObjectSpace();
            //if (os == null) return;
            //try
            //{
            //    var listViewId = "CustomSystemRole_ListView";
            //    var cs = _application.CreateCollectionSource(os, typeof(CustomSystemRole), listViewId);
            //    if (cs == null) return;

            //    var lv = _application.CreateListView(listViewId, cs, true);
            //    if (lv == null) return;
            //    var showViewParameters = new ShowViewParameters { CreatedView = lv };

            //    _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //}
            //catch (Exception ex)
            //{
            //    #region 操作日志
            //    var log = os.CreateObject<OperationLog>();
            //    log.Category = OperationCategory.Error;
            //    log.OperationEvent = "进入系统管理";
            //    log.Remark = "从Portal进入系统管理异常：" + ex;
            //    #endregion
            //    log.Save();
            //    os.CommitChanges();
            //}
        }
        #endregion

        #region 退出
        //退出
        protected void btnLogout_OnClick(object sender, EventArgs e)
        {
            Session.Abandon();
            Request.Cookies.Clear();
            FormsAuthentication.SignOut();
            var p = HttpContext.Current.Profile;
            Response.Redirect("~/Default.aspx");
        }
        #endregion

        #region 我的信息
        protected void btnMyDetails_OnClick(object sender, EventArgs e)
        {
            if (_application == null) return;
            var os = _application.CreateObjectSpace();
            try
            {
                var emp = os.FindObject<Employee>(CriteriaOperator.Parse("Oid = ?", SecuritySystem.CurrentUserId));
                if (emp == null) return;
                var dv = _application.CreateDetailView(os, "Employee_DetailView", true, emp);
                if (dv == null) return;
                var showViewParameters = new ShowViewParameters { CreatedView = dv };

                _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                #region 操作日志
                var log = os.CreateObject<OperationLog>();
                log.Category = OperationCategory.Error;
                log.OperationEvent = "查看我的信息";
                log.Remark = "从Portal查看我的信息异常：" + ex;
                #endregion
                log.Save();
                os.CommitChanges();
            }
        }
        #endregion

        #region 跳转，从Dashboard跳转到指定模块的ListView
        /// <summary>
        /// 跳转，从Dashboard跳转到指定模块的ListView
        /// </summary>
        /// <param name="listViewId">要跳转的ListViewId</param>
        /// <param name="objectType">要跳转的类型</param>
        private void RedirectDashboardToListView(string listViewId, Type objectType)
        {
            if (_application == null) return;
            var os = _application.CreateObjectSpace();
            if (os == null) return;
            try
            {
                var cs = _application.CreateCollectionSource(os, objectType, listViewId);
                if (cs == null) return;
                //if(objectType.Name == "DashboardView")
                //{

                //}
                var lv = _application.CreateListView(listViewId, cs, true);
                if (lv == null) return;
                var showViewParameters = new ShowViewParameters { CreatedView = lv };
                _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                #region 操作日志
                var log = os.CreateObject<OperationLog>();
                log.Category = OperationCategory.Error;
                log.OperationEvent = "进入业务模块";
                log.Remark = "从Portal进入业务模块异常：" + ex;
                #endregion
                log.Save();
                os.CommitChanges();
            }
        }
        #endregion


        #region 实现IComplexControl
        public void Refresh()
        {

        }

        public void Setup(DevExpress.ExpressApp.IObjectSpace objectSpace, DevExpress.ExpressApp.XafApplication application)
        {
            _objectSpace = objectSpace;
            _application = application;
        }
        #endregion

        protected void btnSample_Click(object sender, EventArgs e)
        {
            RedirectDashboardToListView("Samplecheckin_ListView", typeof(Samplecheckin));
        }

        protected void btnDataEntry_Click(object sender, EventArgs e)
        {
            // RedirectDashboardToListView("ResultEntryDV", typeof(DashboardView) );
            var os = _application.CreateObjectSpace();
            if (os == null) return;
            try
            {

                var lv = _application.CreateDashboardView(os, "ResultEntryDV", false);
                if (lv == null) return;
                var showViewParameters = new ShowViewParameters { CreatedView = lv };
                _application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

            }
            catch (Exception ex)
            {
                _application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected void btnSystem1_Click(object sender, EventArgs e)
        {
            RedirectDashboardToListView("CurrentLanguage_ListView", typeof(CurrentLanguage));
        }

    }
}