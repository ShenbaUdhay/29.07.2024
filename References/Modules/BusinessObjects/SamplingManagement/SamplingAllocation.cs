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
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;

namespace Modules.BusinessObjects.SamplingManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SamplingAllocation : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SamplingAllocation(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        SampleLogInInfo sLInfo = new SampleLogInInfo();

        #region Registration
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

        #region Sampling;
        private SampleLogIn _SampleLogIn;
        public SampleLogIn SampleLogIn
        {
            get
            {
                if(JobID!= null)
                {
                    _SampleLogIn = Session.FindObject<SampleLogIn>(CriteriaOperator.Parse("[JobID.Oid] = ?", JobID.Oid));
                }
                return _SampleLogIn;
            }
            set
            {
                SetPropertyValue<SampleLogIn>("SampleLogIn", ref _SampleLogIn, value);
            }
        }
        #endregion

        #region SampleMatrix;
        private string _SampleMatrix;
        [NonPersistent]
        public string SampleMatrix
        {
            get
            {
                if (SampleLogIn != null)
                {
                    _SampleMatrix = SampleLogIn.VisualMatrix.ToString();
                }
                return _SampleMatrix;
            }
            set
            {
                SetPropertyValue<string>("SampleMatrix", ref _SampleMatrix, value);
            }
        }
        #endregion

        #region SiteName;
        private string _SiteName;
        [NonPersistent]
        public string SiteName
        {
            get
            {
                if (SampleLogIn != null)
                {
                    _SiteName = SampleLogIn.StationLocation.ToString();
                }
                return _SiteName;
            }
            set
            {
                SetPropertyValue<string>("SiteName", ref _SiteName, value);
            }
        }
        #endregion


        #region SampleID;
        private string _SampleID;
        [NonPersistent]
        public string SampleID
        {
            get
            {
                if (SampleLogIn.SampleNo > 0)
                {
                    if (sLInfo.SampleIDDigit == SampleIDDigit.Three)
                    {
                        if (SampleLogIn.SampleNo.ToString().Length == 1)
                        {
                            return string.Format("{0}{1}{2}", JobID.JobID, "-00", SampleLogIn.SampleNo.ToString());
                        }
                        else if (SampleLogIn.SampleNo.ToString().Length == 2)
                        {
                            return string.Format("{0}{1}{2}", JobID.JobID, "-0", SampleLogIn.SampleNo.ToString());
                        }
                        else
                        {
                            return string.Format("{0}{1}{2}", JobID.JobID, "-", SampleLogIn.SampleNo.ToString());
                        }
                    }
                    else
                    {
                        if (SampleLogIn.SampleNo.ToString().Length == 1)
                        {
                            return string.Format("{0}{1}{2}", JobID.JobID, "-0", SampleLogIn.SampleNo.ToString());
                        }
                        else
                        {
                            return string.Format("{0}{1}{2}", JobID.JobID, "-", SampleLogIn.SampleNo.ToString());
                        }
                    }

                }
                else
                {
                    return string.Empty;
                }

            }
            set
            {
                SetPropertyValue<string>("SiteName", ref _SiteName, value);
            }
        }
        #endregion

        #region SampleName;
        private string _SampleName;
        [NonPersistent]
        public string SampleName
        {
            get
            {
                if (SampleLogIn != null)
                {
                    _SampleName = SampleLogIn.ClientSampleID.ToString();
                }
                return _SampleName;
            }
            set
            {
                SetPropertyValue<string>("SampleName", ref _SampleName, value);
            }
        }
        #endregion


        #region SamplingEquipment;
        private string _SamplingEquipment;
        [NonPersistent]
        public string SamplingEquipment
        {
            get
            {
                if (SampleLogIn != null)
                {
                    _SamplingEquipment = SampleLogIn.SamplingEquipment.ToString();
                }
                return _SamplingEquipment;
            }
            set
            {
                SetPropertyValue<string>("SamplingEquipment", ref _SamplingEquipment, value);
            }
        }
        #endregion


        #region SamplingDate;
        private DateTime _SamplingDate;
        [NonPersistent]
        public DateTime SamplingDate
        {
            get
            {
                return _SamplingDate;
            }
            set
            {
                SetPropertyValue<DateTime>("SamplingDate", ref _SamplingDate, value);
            }
        }
        #endregion


        #region AssignTo
        private Employee _AssignTo;
        public Employee AssignTo
        {
            get { return _AssignTo; }
            set { SetPropertyValue(nameof(AssignTo), ref _AssignTo, value); }
        }
        #endregion

        #region DateAssigned
        private DateTime _DateAssigned;

        public DateTime DateAssigned
        {
            get { return _DateAssigned; }
            set { SetPropertyValue(nameof(DateAssigned), ref _DateAssigned, value); }
        }
        #endregion

        #region AssignedBy
        private Employee _AssignedBy;
        public Employee AssignedBy
        {
            get { return _AssignedBy; }
            set { SetPropertyValue(nameof(AssignedBy), ref _AssignedBy, value); }
        }
        #endregion
        #region LastUpdatedBy
        private Employee _LastUpdatedBy;
        public Employee LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { SetPropertyValue(nameof(LastUpdatedBy), ref _LastUpdatedBy, value); }
        }
        #endregion


        #region LastUpdatedDate
        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { SetPropertyValue(nameof(LastUpdatedDate), ref _LastUpdatedDate, value); }
        }
        #endregion

    }
}