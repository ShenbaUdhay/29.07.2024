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
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.TaskManagement;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SamplingManagement;

namespace Modules.BusinessObjects.SamplingManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TaskJobIDAutomation : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TaskJobIDAutomation(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            LastUpdatedDate = Library.GetServerTime(Session);
            LastUpdatedBy = (Employee)Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }

        //#region SRID
        //private SamplingProposal _SRID;
        //[ImmediatePostData(true)]
        //public SamplingProposal SRID
        //{
        //    get
        //    {
        //        return _SRID;
        //    }
        //    set
        //    {
        //        SetPropertyValue<SamplingProposal>("SRID", ref _SRID, value);
        //    }
        //}
        //#endregion


        #region COCID
        private COCSettings _COCID;
        public COCSettings COCID
        {
            get
            {
                return _COCID;
            }
            set { SetPropertyValue<COCSettings>("COCID", ref _COCID, value); }

        }
        #endregion


        #region ProjectID
        private string _ProjectID;
        [NonPersistent]
        public string ProjectID
        {
            get
            {
                if (COCID != null)
                {
                    if (COCID.ProjectID != null)
                    {
                        _ProjectID = COCID.ProjectID.ProjectId.ToString();
                    }
                    else
                    {
                        _ProjectID = string.Empty;
                    }

                }
                else
                {
                    _ProjectID = string.Empty;
                }
                return _ProjectID;
            }
            set { SetPropertyValue<string>("ProjectID", ref _ProjectID, value); }

        }
        #endregion


        #region ProjectName
        private string _ProjectName;
        [NonPersistent]
        public string ProjectName
        {
            get
            {
                if (COCID != null)
                {
                    if (COCID.ProjectName != null)
                    {
                        _ProjectName = COCID.ProjectName.ToString();
                    }
                    else
                    {
                        _ProjectName = string.Empty;
                    }

                }
                else
                {
                    _ProjectName = string.Empty;
                }
                return _ProjectName;
            }
            set { SetPropertyValue<string>("ProjectName", ref _ProjectName, value); }

        }
        #endregion


        #region DaysinAdvance
        private int _DaysinAdvance;
        [ImmediatePostData]
        public int DaysinAdvance
        {
            get
            {
                return _DaysinAdvance;
            }
            set
            {
                SetPropertyValue("DaysinAdvance", ref _DaysinAdvance, value);
            }
        }
        #endregion


        #region LastUpdatedBy
        private Employee _LastUpdatedBy;
        public Employee LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { SetPropertyValue<Employee>("LastUpdatedBy", ref _LastUpdatedBy, value); }
        }
        #endregion

        #region LastUpdatedDate
        private DateTime _LastUpdatedDate;
        [ImmediatePostData]
        public DateTime LastUpdatedDate
        {
            get
            {
                return _LastUpdatedDate;
            }
            set
            {
                SetPropertyValue("LastUpdatedBy", ref _LastUpdatedDate, value);
            }
        }
        #endregion

       

    }
}