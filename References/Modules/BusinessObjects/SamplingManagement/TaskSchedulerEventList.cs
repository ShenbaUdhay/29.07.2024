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
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;

namespace Modules.BusinessObjects.SamplingManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TaskSchedulerEventList : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TaskSchedulerEventList(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        public enum RegistrationStatus
        {
            [XafDisplayName("Pending Attach Job ID")]
            PendingAttachJobID,
            [XafDisplayName("Pending Sampling")]
            PendingSampling,
            [XafDisplayName("Sampled")]
            Sampled,
        };

        #region TaskSchedulerID
        private TaskRecurranceSetup _TaskSchedulerID;
        public TaskRecurranceSetup TaskSchedulerID
        {
            get
            {
                return _TaskSchedulerID;
            }
            set
            {
                SetPropertyValue<TaskRecurranceSetup>("TaskSchedulerID", ref _TaskSchedulerID, value);
            }
        }
        #endregion

        #region StartDate
        private DateTime _StartDate;
        public DateTime StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                SetPropertyValue("StartDate", ref _StartDate, value);
            }
        }
        #endregion

        #region EndDate
        private DateTime _EndDate;
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                SetPropertyValue("EndDate", ref _EndDate, value);
            }
        }
        #endregion

        #region JobID
        private Samplecheckin _JobID;
        public Samplecheckin JobID
        {
            get
            {
                return _JobID;
            }
            set
            {
                SetPropertyValue<Samplecheckin>("JobID", ref _JobID, value);
            }
        }
        #endregion
        #region ClientName
        private string _ClientName;
        [ImmediatePostData(true)]
        [NonPersistent]
        public string ClientName
        {
            get
            {

                if (TaskSchedulerID!=null&&TaskSchedulerID.COCSettings != null)
                {
                    _ClientName = TaskSchedulerID.COCSettings.ClientName.CustomerName.ToString();
                }
                else
                {
                    _ClientName = string.Empty;
                }
                return _ClientName;
            }
            set
            {
                SetPropertyValue("ClientName", ref _ClientName, value);
            }
        }
        #endregion


        #region ProjectID
        private string _ProjectID;
        [ImmediatePostData(true)]
        [NonPersistent]
        public string ProjectID
        {
            get
            {
                if (TaskSchedulerID != null && TaskSchedulerID.COCSettings != null)
                {
                    if (TaskSchedulerID.COCSettings.ProjectID != null)
                    {
                        _ProjectID = TaskSchedulerID.COCSettings.ProjectID.ProjectId.ToString();
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
        [ImmediatePostData(true)]
        [NonPersistent]
        public string ProjectName
        {
            get
            {
                if (TaskSchedulerID != null && TaskSchedulerID.COCSettings != null)
                {
                    if (TaskSchedulerID.COCSettings.ProjectName != null)
                    {
                        _ProjectName = TaskSchedulerID.COCSettings.ProjectName.ToString();
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


      


        #region SampleMatrix
        private string _SampleMatrix;
        [ImmediatePostData(true)]
        [NonPersistent]
        public string SampleMatrix
        {
            get
            {
                if (TaskSchedulerID != null && TaskSchedulerID.COCSettings != null)
                {
                    if (TaskSchedulerID.COCSettings.SampleMatries != null)
                    {
                            List<string> lstSM = new List<string>();
                            List<string> lstSMOid = TaskSchedulerID.COCSettings.SampleMatries.Split(';').ToList();
                            if (lstSMOid != null)
                            {
                                foreach (string objOid in lstSMOid)
                                {
                                    if (!string.IsNullOrEmpty(objOid))
                                    {
                                        VisualMatrix objVM = Session.GetObjectByKey<VisualMatrix>(new Guid(objOid.Trim()));
                                        if (objVM != null && !lstSM.Contains(objVM.MatrixName.MatrixName))
                                        {
                                            lstSM.Add(objVM.MatrixName.MatrixName);
                                        }
                                    }

                                }
                            }
                  
                        _SampleMatrix = string.Join(",", lstSM);
                    }
                    else
                    {
                        _SampleMatrix = string.Empty;
                    }
                }
                else
                {
                    _SampleMatrix = string.Empty;
                }
                return _SampleMatrix;
            }
            set { SetPropertyValue<string>("SampleMatrix", ref _SampleMatrix, value); }
        }
        #endregion


        //#region TestSummary
        //private string _TestSummary;
        //[ImmediatePostData(true)]
        //[NonPersistent]
        //public string TestSummary
        //{
        //    get
        //    {
        //        if (TaskSchedulerID != null && TaskSchedulerID.COCSettings != null)
        //        {
        //            if (TaskSchedulerID.COCSettings.TestSummary != null)
        //            {
        //                _TestSummary = TaskSchedulerID.COCSettings.TestSummary.ToString();
        //            }
        //            else
        //            {
        //                _TestSummary = string.Empty;
        //            }

        //        }
        //        else
        //        {
        //            _TestSummary = string.Empty;
        //        }
        //        return _TestSummary;
        //    }
        //    set { SetPropertyValue<string>("TestSummary", ref _TestSummary, value); }

        //}
        //#endregion

        #region RecurranceType
        private string _RecurranceType;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public string RecurranceType
        {
            get
            {
                return _RecurranceType;
            }
            set
            {
                SetPropertyValue<string>("RecurranceType", ref _RecurranceType, value);
            }
        }
        #endregion






        #region Status
        private RegistrationStatus _Status;
        [NonPersistent]
        public RegistrationStatus Status
        {
            get
            {

                if (JobID == null)
                {
                    _Status = RegistrationStatus.PendingAttachJobID;
                }
                else if (JobID != null)
                {
                    _Status = RegistrationStatus.PendingSampling;
                }
                return _Status;
            }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }
        #endregion
    }
}