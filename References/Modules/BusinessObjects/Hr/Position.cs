// ================================================================================
// Table Name: [Position]
// Author: Sunny
// Date: 2016��12��13��
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption��ְ��
// ================================================================================
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Hr
{
    /// <summary>
    /// ��[Position]��ʵ����
    /// </summary>
    [DefaultClassOptions]
    //[RuleCombinationOfPropertiesIsUnique("Position", DefaultContexts.Save, "PositionName", SkipNullOrEmptyValues = false)]
    public class Position : BaseObject
    {

        /// <summary>
        /// ��ʼ���� Position ����ʵ����
        /// </summary>
        public Position(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;

            //����
            var maxSort = Session.Evaluate<Position>(CriteriaOperator.Parse("MAX(Sort)"), null);
            int sort = 1;
            if (maxSort != null)
            {
                sort = int.Parse(maxSort.ToString()) + 1;
            }
            Sort = sort;
        }
        #region OnDeleting
        protected override void OnDeleting()
        {
            base.OnDeleting();
            System.Collections.ICollection lstReferenceObjects = Session.CollectReferencingObjects(this);
            if (lstReferenceObjects.Count > 0)
            {
                foreach (var obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.GetType() != typeof(DevExpress.Xpo.Metadata.Helpers.IntermediateObject))
                    {
                        Exception ex = new Exception("Already used can't allow to delete");
                        throw ex;
                        break;
                    }
                }
            }
        }
        #endregion

        //#region OnDelete
        //protected override void OnDeleting()
        //{
        //    base.OnDeleting();
        //    if (Session.CollectReferencingObjects(this).Count > 0)
        //    {
        //        foreach (BaseObject obj in Session.CollectReferencingObjects(this))
        //        {
        //            if (obj.Oid != null)
        //            {
        //                Exception ex = new Exception("Already Used Can't allow to Delete");
        //                throw ex;
        //                break;

        //            }
        //        }
        //    }
        //}
        //#endregion

        #region ְ�����
        private string _positionCode;
        /// <summary>
        /// ְ�����
        /// </summary>
        [Size(32)]
        //[RuleRequiredField("Position.PositionCode", DefaultContexts.Save)]
        [Browsable(false)]
        public string PositionCode
        {
            get
            {
                return _positionCode;
            }
            set
            {
                SetPropertyValue("PositionCode", ref _positionCode, value);
            }
        }
        #endregion

        #region ְ������
        private string _positionName;
        /// <summary>
        /// ְ������
        /// </summary>
        [Size(64)]
        [RuleRequiredField("Position.PositionName", DefaultContexts.Save,"'Position' must not be empty")]
        public string PositionName
        {
            get
            {
                return _positionName;
            }
            set
            {
                SetPropertyValue("PositionName", ref _positionName, value);
            }
        }
        #endregion

        #region ����
        private int _sort;
        /// <summary>
        /// ����
        /// </summary>
        public int Sort
        {
            get
            {
                return _sort;
            }
            set
            {
                SetPropertyValue("Sort", ref _sort, value);
            }
        }
        #endregion

        #region ��˾
        private Company _company;
        /// <summary>
        /// ��˾
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Company-Positions")]
        public Company Company
        {
            get
            {
                return _company;
            }
            set
            {
                SetPropertyValue("Company", ref _company, value);
            }
        }
        #endregion

    }
}
