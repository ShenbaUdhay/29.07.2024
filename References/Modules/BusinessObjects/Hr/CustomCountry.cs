// ================================================================================
// Table Name: [Country]
// Author: Sunny
// Date: 2016年12月15日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：国家
// ================================================================================
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Drawing;

namespace Modules.BusinessObjects.Hr
{
    /// <summary>
    /// 国家
    /// </summary>
    //[RuleCombinationOfPropertiesIsUnique("CustomCountry", DefaultContexts.Save, "EnglishLongName", "Country Name must unique", SkipNullOrEmptyValues = false)]
    [DefaultClassOptions]
    public class CustomCountry : BaseObject
    {
        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Constructor
        public CustomCountry(Session session)
            : base(session)
        {
        }
        #endregion

        #region Events
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region OnDelete
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (BaseObject obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.Oid != null)
                    {
                        Exception ex = new Exception("Already Used Can't allow to Delete");
                        throw ex;
                        break;

                    }
                }
            }
        }
        #endregion

        #region 中文名称
        private string _chineseLongName;
        /// <summary>
        /// 中文名称
        /// </summary>
        [Size(64)]
        //[RuleRequiredField("CustomCountry.ChineseLongName", DefaultContexts.Save)]
        public string ChineseLongName
        {
            get
            {
                return _chineseLongName;
            }
            set
            {
                SetPropertyValue("ChineseLongName", ref _chineseLongName, value);
            }
        }
        #endregion

        #region 英文名称
        private string _englishLongName;
        /// <summary>
        /// 英文名称
        /// </summary>
        [Size(128)]
        [RuleUniqueValue]
        [RuleRequiredField("CustomCountry.EnglishName", DefaultContexts.Save)]
        public string EnglishLongName
        {
            get
            {
                return _englishLongName;
            }
            set
            {
                SetPropertyValue("EnglishLongName", ref _englishLongName, value);
            }
        }
        #endregion

        #region 中文缩写
        private string _chineseShortName;
        /// <summary>
        /// 中文缩写
        /// </summary>
        [Size(64)]
        public string ChineseShortName
        {
            get
            {
                return _chineseShortName;
            }
            set
            {
                SetPropertyValue("ChineseShortName", ref _chineseShortName, value);
            }
        }
        #endregion

        #region 英文缩写
        private string _englishShortName;
        /// <summary>
        /// 英文缩写
        /// </summary>
        [Size(128)]
        [RuleUniqueValue]
        //[RuleRequiredField("EnglishShortName", DefaultContexts.Save)]
        public string EnglishShortName
        {
            get
            {
                return _englishShortName;
            }
            set
            {
                SetPropertyValue("EnglishShortName", ref _englishShortName, value);
            }
        }
        #endregion

        #region 国旗
        private byte[] _nationalFlag;
        /// <summary>
        /// 国旗
        /// </summary>
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 100)]
        public byte[] NationalFlag
        {
            get
            {
                return _nationalFlag;
            }
            set
            {
                SetPropertyValue("NationalFlag", ref _nationalFlag, value);
            }
        }
        #endregion

        #region CountryState
        /// <summary>
        /// 省、州
        /// </summary>
        [Association("Country-States")]
        public XPCollection<CustomState> States
        {
            get
            {
                return GetCollection<CustomState>("States");
            }
        }
        #endregion
    }
}