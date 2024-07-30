using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.QC;
using System;

namespace Modules.BusinessObjects.SampleManagement.SamplePreparation
{
    [DefaultClassOptions]
    public class Reagent : BaseObject
    {
        public Reagent(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (DevExpress.Xpo.Metadata.Helpers.IntermediateObject obj in Session.CollectReferencingObjects(this))
                {
                    Exception ex = new Exception("Already used can't allow to delete");
                    throw ex;
                    break;
                }
            }
            //System.Collections.ICollection lstReferenceObjects = Session.CollectReferencingObjects(this);
            //if (lstReferenceObjects.Count > 0)
            //{
            //    foreach (var obj in Session.CollectReferencingObjects(this))
            //    {
            //        if (obj.GetType() != typeof(DevExpress.Xpo.Metadata.Helpers.))
            //        {
            //            Exception ex = new Exception("Already used can't allow to delete");
            //            throw ex;
            //            break;
            //        }
            //    }
            //}
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(ReagentID))
            {
                CriteriaOperator idct = CriteriaOperator.Parse("Max(SUBSTRING(ReagentID, 2))");
                string tempid = (Convert.ToInt32(Session.Evaluate(typeof(Reagent), idct, null)) + 1).ToString();
                var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempid != "1")
                {
                    var predate = tempid.Substring(0, 6);
                    if (predate == curdate)
                    {
                        tempid = "LT" + tempid;
                    }
                    else
                    {
                        tempid = "LT" + curdate + "01";
                    }
                }
                else
                {
                    tempid = "LT" + curdate + "01";
                }
                ReagentID = tempid;
            }
        }

        #region ReagentID
        private string _ReagentID;
        public string ReagentID
        {
            get
            {
                return _ReagentID;
            }
            set
            {
                SetPropertyValue<string>(nameof(ReagentID), ref _ReagentID, value);
            }
        }
        #endregion

        #region ReagentName
        private string _ReagentName;
        [RuleRequiredField("Enter the ReagentName", DefaultContexts.Save)]
        public string ReagentName
        {
            get
            {
                return _ReagentName;
            }
            set
            {
                SetPropertyValue<string>(nameof(ReagentName), ref _ReagentName, value);
            }
        }
        #endregion

        #region ExpiredDate
        private Nullable<DateTime> _ExpiredDate;
        [ImmediatePostData]
        public Nullable<DateTime> ExpiredDate
        {
            get
            {
                return _ExpiredDate;
            }
            set
            {
                SetPropertyValue(nameof(ExpiredDate), ref _ExpiredDate, value);
            }
        }
        #endregion

        #region Description
        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetPropertyValue<string>(nameof(Description), ref _Description, value);
            }
        }
        #endregion

        #region CreatedBy
        private CustomSystemUser _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value);
            }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value);
            }
        }
        #endregion

        #region PrepBatchs
        [Association("SamplePrepBatchReagents", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<SamplePrepBatch> PrepBatchs
        {
            get
            {
                return GetCollection<SamplePrepBatch>(nameof(PrepBatchs));
            }
        }
        #endregion
        #region QcBatch
        [Association("QcBatchReagents", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<QCBatch> QcBatchs
        {
            get
            {
                return GetCollection<QCBatch>(nameof(QcBatchs));
            }
        }
        #endregion
        #region AnalyticalBatch
        [Association("AnalyticalBatchReagents", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<SpreadSheetEntry_AnalyticalBatch> SpreadSheetEntry_AnalyticalBatchs
        {
            get
            {
                return GetCollection<SpreadSheetEntry_AnalyticalBatch>(nameof(SpreadSheetEntry_AnalyticalBatchs));
            }
        }
        #endregion
    }
}