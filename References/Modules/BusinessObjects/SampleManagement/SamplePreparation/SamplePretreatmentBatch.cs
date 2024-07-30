using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Method = Modules.BusinessObjects.Setting.Method;

namespace Modules.BusinessObjects.SampleManagement.SamplePreparation
{
    [DefaultClassOptions]
    public class SamplePretreatmentBatch : BaseObject, ICheckedListBoxItemsProvider
    {
        public SamplePretreatmentBatch(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            Datecreated = Library.GetServerTime(Session);
        }

        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> properties = new Dictionary<object, string>();
            if (targetMemberName == "Jobid")
            {
                if (JobidDataSource != null && JobidDataSource.Count > 0)
                {
                    foreach (SampleLogIn objsample in JobidDataSource.Where(a => a.JobID != null && a.SamplePretreatmentBatchID == null).Distinct().OrderByDescending(a => a.JobID.JobID).ToList())
                    {
                        if (!properties.ContainsKey(objsample.JobID.Oid))
                        {
                            properties.Add(objsample.JobID.Oid, objsample.JobID.JobID);
                        }
                    }
                }
                else if (JobidDataSource == null && Jobid != null)
                {
                    string[] ids = Jobid.Split(';');
                    foreach (string id in ids)
                    {
                        Samplecheckin sample = Session.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", new Guid(id.Replace(" ", ""))));
                        if (sample != null)
                        {
                            properties.Add(sample.Oid, sample.JobID);
                        }
                    }
                }
            }
            else if (targetMemberName == "Equipment" && EquipmentDataSource != null && EquipmentDataSource.Count > 0)
            {
                foreach (Labware objlab in EquipmentDataSource.OrderBy(a => a.AssignedName).ToList())
                {
                    if (!properties.ContainsKey(objlab.Oid))
                    {
                        properties.Add(objlab.Oid, objlab.AssignedName);
                    }
                }
            }
            return properties;
        }

        #region PreTreatBatchID
        private string _PreTreatBatchID;
        public string PreTreatBatchID
        {
            get { return _PreTreatBatchID; }
            set { SetPropertyValue(nameof(PreTreatBatchID), ref _PreTreatBatchID, value); }
        }
        #endregion

        #region Datecreated
        private DateTime _Datecreated;
        public DateTime Datecreated
        {
            get { return _Datecreated; }
            set { SetPropertyValue(nameof(Datecreated), ref _Datecreated, value); }
        }
        #endregion

        #region CreatedBy
        private CustomSystemUser _CreatedBy;
        public CustomSystemUser CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion

        #region Matrix
        private Matrix _Matrix;
        [ImmediatePostData]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public Matrix Matrix
        {
            get
            {
                return _Matrix;
            }
            set
            {
                SetPropertyValue(nameof(Matrix), ref _Matrix, value);
            }
        }
        #endregion

        #region PrepMethod
        private Method _PrepMethod;
        [ImmediatePostData]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public Method PrepMethod
        {
            get
            {
                return _PrepMethod;
            }
            set
            {
                SetPropertyValue(nameof(PrepMethod), ref _PrepMethod, value);
            }
        }
        #endregion

        #region Equipment
        private string _Equipment;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [RuleRequiredField]
        public string Equipment
        {
            get { return _Equipment; }
            set { SetPropertyValue("Equipment", ref _Equipment, value); }
        }
        #endregion

        #region Temperature
        private string _Temperature;
        public string Temperature
        {
            get { return _Temperature; }
            set { SetPropertyValue(nameof(Temperature), ref _Temperature, value); }
        }
        #endregion

        #region Humidity
        private string _Humidity;

        public event EventHandler ItemsChanged;

        public string Humidity
        {
            get { return _Humidity; }
            set { SetPropertyValue(nameof(Humidity), ref _Humidity, value); }
        }
        #endregion

        #region JobID
        private string _Jobid;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [RuleRequiredField]
        public string Jobid
        {
            get { return _Jobid; }
            set { SetPropertyValue("Jobid", ref _Jobid, value); }
        }
        #endregion

        #region TemplateID
        private SamplePrepTemplates _Templateid;
        //[DataSourceProperty(nameof(TemplatesDataSource))]
        [RuleRequiredField]
        public SamplePrepTemplates TemplateID
        {
            get { return _Templateid; }
            set { SetPropertyValue("TemplateID", ref _Templateid, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                SetPropertyValue(nameof(Comment), ref _Comment, value);
            }
        }
        #endregion

        #region CustomDataSource
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<SampleLogIn> JobidDataSource
        {
            get
            {
                if (Matrix != null && string.IsNullOrEmpty(PreTreatBatchID))
                {
                    return new XPCollection<SampleLogIn>(Session, CriteriaOperator.Parse("[SamplePretreatmentBatchID] Is Null And [VisualMatrix.MatrixName.Oid] = ?", Matrix.Oid));
                }
                else
                {
                    return null;
                }
            }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Labware> EquipmentDataSource
        {
            get
            {
                return new XPCollection<Labware>(Session, CriteriaOperator.Parse(""));
            }
        }

        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<SamplePrepTemplates> TemplatesDataSource
        //{
        //    get
        //    {
        //        if (PrepMethod1 != null && Matrix != null)
        //        {
        //            return new XPCollection<SamplePrepTemplates>(Session, CriteriaOperator.Parse("[PrepMethod.Oid] = ? AND [VisualMatrix.MatrixName.Oid] = ?", PrepMethod1.Oid,Matrix.Oid));
        //            //return new XPCollection<SamplePrepTemplates>(Session, CriteriaOperator.Parse("[PrepMethod.Oid] = ?", PrepMethod1.Oid));
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        #endregion

        #region SamplePretreatmentBatchlink
        [Association("SamplePretreatmentBatchlink")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<SamplePretreatmentBatchSequence> SamplePretreatmentBatchSeqDetail
        {
            get { return GetCollection<SamplePretreatmentBatchSequence>(nameof(SamplePretreatmentBatchSeqDetail)); }
        }
        #endregion
    }
}