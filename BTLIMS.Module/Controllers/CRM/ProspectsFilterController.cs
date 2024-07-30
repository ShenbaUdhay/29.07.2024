using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.Accounts
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ProspectsFilterController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public String strCurrentUserId = string.Empty;
        Boolean bolISAdmin = false;
        Boolean bolIsManager = false;
        ReportingToInfo objReportingInfo = new ReportingToInfo();
        public ProspectsFilterController()
        {

            InitializeComponent();
            TargetObjectType = typeof(CRMProspects);
            TargetViewType = ViewType.ListView;
            TargetViewId = "CRMProspects_ListView;" + "CRMProspects_ListView_Copy_Open;" + "CRMProspects_ListView_Copy_Closed;" + "CRMProspects_ListView_MyOpenLeads_Copy;" +
                "CRMProspects_ListView_UnfollowedProspects;";
            // Target required Views (via the TargetXXX properties) and create their Actions.

        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                strCurrentUserId = Application.Security.UserId.ToString();

                //var myOpenProspects = new ChoiceActionItem();
                //var OpenProspects = new ChoiceActionItem();
                //var ClosedProspects = new ChoiceActionItem();

                //ProspectsFilterAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
                //ProspectsFilterAction.TargetViewType = ViewType.ListView;
                //ProspectsFilterAction.Items.Add(new ChoiceActionItem("MyOpenProspects","My Open Prospects", myOpenProspects));
                //ProspectsFilterAction.Items.Add(new ChoiceActionItem("OpenProspects","Open Prospects", OpenProspects));
                //ProspectsFilterAction.Items.Add(new ChoiceActionItem("ClosedProspects","Closed Prospects", ClosedProspects));
                //ProspectsFilterAction.SelectedIndex = 2;
                //ProspectsFilterAction.Category = "View";
                //ProspectsFilterAction.TargetObjectType = typeof(CRMProspects);
                // Perform various tasks depending on the target View.
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                Employee objUser = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                //CriteriaOperator criteria = CriteriaOperator.Parse("[ReportingTo.Oid] = ?", SecuritySystem.CurrentUserId);
                IList<string> lstReportingTo = new List<string>();
                CriteriaOperator criteria = CriteriaOperator.Parse("[ReportingTo] is not null ");
                IList<Employee> lstUserReportingTo = ObjectSpace.GetObjects<Employee>(criteria);

                string strUser = SecuritySystem.CurrentUserId.ToString();
                Hashtable ht = new Hashtable();
                Hashtable inner = new Hashtable();
                ht.Add(strUser, false);
                lstReportingTo.Add(strUser);
            Loop1:
                foreach (DictionaryEntry di in ht)
                {
                    if (di.Value.Equals(false))
                    {
                        strUser = di.Key.ToString();
                        ht[di.Key] = true;
                        foreach (Employee obj in lstUserReportingTo.Where<Employee>(i => i.Oid != new Guid(strUser)))
                        {
                            if (obj.ReportingTo != null)
                            {
                                string[] ids = obj.ReportingTo.Replace(" ", "").Split(';');
                                if (ids.Contains(strUser))
                                {
                                    if (!lstReportingTo.Contains(obj.Oid.ToString()))
                                    {
                                        lstReportingTo.Add(obj.Oid.ToString());
                                        ht.Add(obj.Oid.ToString(), false);

                                    }
                                }
                            }
                        }
                        goto Loop1;
                    }
                }

                objReportingInfo.ReportingTo = lstReportingTo;

                if (objUser.IsManager == true)
                {
                    bolIsManager = true;
                }

                XPCollection<PermissionPolicyRole> currentUserRoles = ((PermissionPolicyUser)SecuritySystem.CurrentUser).Roles;
                foreach (PermissionPolicyRole role in currentUserRoles)
                {
                    if (role.IsAdministrative == true)
                    {
                        bolISAdmin = true;
                        break;
                    }
                }
                if (View.Id == "CRMProspects_ListView")
                {
                    ((ListView)View).CollectionSource.Criteria.Clear();
                    ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter"] = CriteriaOperator.Parse("[Status]='None'");
                    //if (bolISAdmin == false)
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter"] = CriteriaOperator.Parse("[Owner.Oid]='" + strCurrentUserId + "'");
                    //}
                    if (bolIsManager == false && bolISAdmin == false)
                    {
                        ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter1"] = CriteriaOperator.Parse("[Status]='None' And [Owner.Oid] In(" + string.Format("'{0}'", string.Join("','", lstReportingTo.Select(i => i.ToString().Replace("'", "''")))) + ") or [CreatedBy.Oid] In(" + string.Format("'{0}'", string.Join("','", lstReportingTo.Select(i => i.ToString().Replace("'", "''")))) + ") ");
                    }
                }
                if (View.Id == "CRMProspects_ListView_Copy_Open")
                {
                    ((ListView)View).CollectionSource.Criteria.Clear();
                    ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter"] = CriteriaOperator.Parse("[Status]='None'");
                    //if(bolISAdmin == false)
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter"] = CriteriaOperator.Parse("[Owner.Oid]='" + strCurrentUserId + "'");
                    //}

                    if (bolIsManager == false && bolISAdmin == false)
                    {
                        ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter1"] = CriteriaOperator.Parse("[Status]='None' And [Owner.Oid] In(" + string.Format("'{0}'", string.Join("','", lstReportingTo.Select(i => i.ToString().Replace("'", "''")))) + ") or [CreatedBy.Oid] In(" + string.Format("'{0}'", string.Join("','", lstReportingTo.Select(i => i.ToString().Replace("'", "''")))) + ") ");
                    }
                }
                else if (View.Id == "CRMProspects_ListView_Copy_Closed")
                {
                    ((ListView)View).CollectionSource.Criteria.Clear();
                    ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter"] = CriteriaOperator.Parse("[Status] <> 'None'"); //[Status]='Won'or [Status]='Cancelled' And
                    //if (bolISAdmin == false)
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter"] = CriteriaOperator.Parse("[Owner.Oid]='" + strCurrentUserId + "'");
                    //}

                    if (bolIsManager == false && bolISAdmin == false)
                    {
                        string stroid = string.Format("'{0}'", string.Join("','", lstReportingTo.Select(i => i.ToString().Replace("'", "''"))));
                        //((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter1"] = CriteriaOperator.Parse("[Status]='Won'or [Status]='Cancelled' or [Status]='None' And ([Owner.Oid] In(" + string.Format("'{0}'", string.Join("','", lstReportingTo.Select(i => i.ToString().Replace("'", "''")))) + ") or [CreatedBy.Oid] In(" + string.Format("'{0}'", string.Join("','", lstReportingTo.Select(i => i.ToString().Replace("'", "''")))) + ")) ");
                        ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter1"] = CriteriaOperator.Parse("[CreatedBy.Oid] In(" + stroid + ")");
                    }
                }
                else if (View.Id == "CRMProspects_ListView_MyOpenLeads_Copy")
                {
                    ((ListView)View).CollectionSource.Criteria.Clear();
                    ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter"] = CriteriaOperator.Parse("[Status]='None' And [Owner.Oid]='" + strCurrentUserId + "'");
                }
                else if (View.Id == "CRMProspects_ListView_UnfollowedProspects")
                {
                    ((ListView)View).CollectionSource.Criteria.Clear();
                    ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter"] = CriteriaOperator.Parse("[IsUnFollowed] = True And [Status]='None'");
                    //if(bolISAdmin == false)
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter"] = CriteriaOperator.Parse("[Owner.Oid]='" + strCurrentUserId + "'");
                    //}

                    if (bolIsManager == false && bolISAdmin == false)
                    {
                        string strCriteria = string.Format("'{0}'", string.Join("','", lstReportingTo.Select(i => i.ToString().Replace("'", "''"))));
                        ((ListView)View).CollectionSource.Criteria["ProspectsNavigationFilter1"] = CriteriaOperator.Parse("[IsUnFollowed] = True And [Status]='None' And [Owner.Oid] In(" + strCriteria + ") or [CreatedBy.Oid] In(" + strCriteria + ") ");
                    }
                }

                //if (ProspectsFilterAction.SelectedItem== ProspectsFilterAction.Items[2])//(ProspectsFilterAction.SelectedItem == ProspectsFilterAction.Items[0])
                //{
                //    ((ListView)View).CollectionSource.Criteria.Clear();
                //    ((ListView)View).CollectionSource.Criteria["ProspectsStatusFilter"] = CriteriaOperator.Parse("[Status]='None' AND [Owner.Oid]='" + strCurrentUserId + "'");
                //}
                //else if (ProspectsFilterAction.SelectedItem== ProspectsFilterAction.Items[0])//(ProspectsFilterAction.SelectedItem == ProspectsFilterAction.Items[1])
                //{
                //    ((ListView)View).CollectionSource.Criteria.Clear();
                //    ((ListView)View).CollectionSource.Criteria["ProspectsStatusFilter"] = CriteriaOperator.Parse("[Status]='None'");
                //}
                //else if (ProspectsFilterAction.SelectedItem == ProspectsFilterAction.Items[1])//(ProspectsFilterAction.SelectedItem == ProspectsFilterAction.Items[2])
                //{
                //    ((ListView)View).CollectionSource.Criteria.Clear();
                //    ((ListView)View).CollectionSource.Criteria["ProspectsStatusFilter"] = CriteriaOperator.Parse("[Status]='Won'");
                //}
                // Access and customize the target View control.
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
        }
    }
}
